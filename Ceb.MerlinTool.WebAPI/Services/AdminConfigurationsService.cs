using Ceb.Logger;
using Ceb.MerlinTool.DataAccess;
using Ceb.MerlinTool.WebAPI.Constants;
using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.Models;
using Ceb.MerlinTool.WebAPI.SalesforceServiceReference;
using Ceb.MerlinTool.WebAPI.Services.Relay;
using Ceb.MerlinTool.WebAPI.Services.Relay.Enum;
using Ceb.MerlinTool.WebAPI.Utility;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Services
{
    public class AdminConfigurationsService : IAdminConfigurations
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private DataAccessHelper _dsHelper = new DataAccessHelper();
        private JSONConverter _jsonUtility = new JSONConverter();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="cebId"></param>
        /// <returns></returns>
        public Response<string> UploadSurveyData(int companyId, string memberName, bool isEdit, int updatedBy, string oldFileName)
        {
            var response = new Response<string>();
            DataTable dtSurveyData;
            DataTable dtSurveyDataPerceptionGap;
            List<string> lstTimePeriod;
            List<DateTime> lstDateList;
            string recentDate;
            string strTimePeriod;
            string path = MerlinConstants.SurveyDataPath;
            string htmlFileControl = isEdit ? "editFile" : "file";
            string responseMessage, memberFileName;
            string sheetName1 = ConfigurationManager.AppSettings["SheetName_1"];
            string sheetName2 = ConfigurationManager.AppSettings["SheetName_2"];

            try
            {
                if (!ValidateExcelFile(out responseMessage, out memberFileName, out path, htmlFileControl, oldFileName))
                {
                    response.Status = ServiceStatusCode.Failure;
                    response.Message = responseMessage;
                    return response;
                }

                if (isEdit)
                {
                    //Delete old data and upload new
                    _dsHelper.DeleteSurveyData(companyId.ToString(), isEdit);
                }

                dtSurveyData = Utility.ExcelProcessHelper.ExcelToDataTable(path, sheetName1);
                dtSurveyDataPerceptionGap = Utility.ExcelProcessHelper.ExcelToDataTable(path, sheetName2);

                lstTimePeriod = dtSurveyData.AsEnumerable().Select(C => C.Field<string>("Time Period")).Distinct().ToList();
                lstDateList = new List<DateTime>();
                for (int i = 0; i < lstTimePeriod.Count; i++)
                {
                    lstDateList.Add(Convert.ToDateTime(lstTimePeriod[i].Substring(0, lstTimePeriod[i].IndexOf("to"))));
                }

                List<DataTable> tempDT = ProcessGlobalFiltersandUpdateDataTable(companyId, lstTimePeriod, lstDateList, dtSurveyData, dtSurveyDataPerceptionGap);
                dtSurveyData = tempDT[0];
                dtSurveyDataPerceptionGap = tempDT[1];

                bool blnResult = _dsHelper.UploadSurveyData(dtSurveyData, companyId);
                bool blnPGResult = _dsHelper.UploadPerceptionGapsData(dtSurveyDataPerceptionGap, companyId);

                if (blnResult && blnPGResult)
                {

                    recentDate = lstDateList.Max(r => r).ToString("MMM yyyy");

                    strTimePeriod = lstTimePeriod.Single(S => S.StartsWith(recentDate));//.Single(S => S.Contains(recentDate +" to") );
                    //if (String.IsNullOrWhiteSpace(strTimePeriod))
                    //{
                    //    strTimePeriod = lstTimePeriod.Single(S => S.Contains("to " + recentDate));//.Single(S => S.Contains(recentDate));

                    //}
                    _dsHelper.UpdateMemberDetails(companyId, memberFileName, strTimePeriod, string.Join("$", lstTimePeriod), updatedBy);
                    response.Status = ServiceStatusCode.Success;
                    response.Message = "Survey data uploaded successfully";
                }
                else
                {
                    _dsHelper.DeleteSurveyData(companyId.ToString(),true);
                    response.Status = ServiceStatusCode.Failure;
                    response.Message = "Problem in uploading survey data.Please try again...";
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                _dsHelper.DeleteSurveyData(companyId.ToString(),true);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;

            }
            return response;
        }

        public static void DeleteFileAction(List<string> summmaryReportFileName, string filePath)
        {
            if (!filePath.EndsWith("\\"))
                filePath += "\\";


            foreach (var item in summmaryReportFileName)
            {
                if (System.IO.File.Exists(filePath + item))
                {
                    System.IO.File.Delete(filePath + item);
                }
            }
        }

        /// <summary>
        /// Institution from Database
        /// </summary>
        /// <returns></returns>
        public Response<string> GetActiveInstitutions()
        {
            SalesforceWebServiceClient client = new SalesforceWebServiceClient();
            DataSet dsAvailableInst = new DataSet();
            var response = new Response<string>();
            List<Institution> instList = null;
            try
            {
                if (MerlinConstants.InstitutionList.Count == 0)
                {
                    instList = new List<Institution>();
                    instList = client.GetActiveInstitutions().ToList(); //Institutions from Salesforce
                    MerlinConstants.InstitutionList = instList;
                }

                dsAvailableInst = _dsHelper.GetAvailableInstitutions(); //Institutions added  to database                
                var availableInstList = dsAvailableInst.Tables[0].Rows.OfType<DataRow>().Select(D => Convert.ToString(D["CEBId"])).Distinct();
                List<Institution> requiredInstList = MerlinConstants.InstitutionList.Where(I => !availableInstList.Contains(I.CEBId)).ToList<Institution>();//list to be sent to client

                response.Data = _jsonUtility.GetInstitutions(requiredInstList);
                response.Status = ServiceStatusCode.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, string.Format("Error occured while retrieving institution. ERROR: {0}", ex.Message));
                response.Message = string.Format("Error occured while retrieving institution list");
                response.Status = ServiceStatusCode.Failure;
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public Response<string> CheckifCompanySurvey(string companyId)
        {
            var response = new Response<string>();
            try
            {
                int result = CheckifCompanyDataExists(companyId);
                if (result != 1)
                {
                    response.Status = ServiceStatusCode.Success;
                    response.Message = "No survey data exists. Please contact administrator";
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Institution data from database
        /// </summary>
        /// <returns></returns>
        public Response<string> GetAvailableInstitutions(string page)
        {
            DataSet dsAvailableInst = new DataSet();
            var response = new Response<string>();

            try
            {
                dsAvailableInst = _dsHelper.GetAvailableInstitutions(); //Institutions added  to database                                
                response.Data = _jsonUtility.GetInstitutions(dsAvailableInst, page);
                response.Status = ServiceStatusCode.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, string.Format("Error occured while retrieving institution. ERROR: {1}", ex.Message));
                response.Message = string.Format("Error occured while retrieving institution list");
                response.Status = ServiceStatusCode.Failure;
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public Response<string> DeleteSurveyData(List<MemberModel> members)
        {
            var response = new Response<string>();
            string memberIds;
            List<string> fileNames = new List<string>();
            try
            {
                memberIds = string.Join(",", members.Select(z => z.Id));

                _dsHelper.DeleteSurveyData(memberIds, false);

                fileNames = members.Select(z => z.FileName).ToList<string>();
                DeleteFileAction(fileNames, MerlinConstants.SurveyDataPath);
                fileNames.Clear();
                fileNames = members.Select(z => z.SummaryReportName).ToList<string>();
                DeleteFileAction(fileNames, MerlinConstants.MEMBERREPORTSUMMARY);


                response.Status = ServiceStatusCode.Success;
                response.Message = "Survey for selected clients have been deleted successfully";
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> UploadReportSummary(int companyId, int updatedBy, string oldSummaryReport)
        {
            var response = new Response<string>();
            string memberFileName;
            List<string> oldSummaryReportList = new List<string>() { oldSummaryReport };
            try
            {
                var httpRequest = HttpContext.Current.Request;
                HttpPostedFile file = httpRequest.Files["reportFile"];
                string path = MerlinConstants.MEMBERREPORTSUMMARY;
                DeleteFileAction(oldSummaryReportList, path);
                if (httpRequest.Files["reportFile"].ContentLength > 0)
                {
                    if (!path.EndsWith("\\"))
                        path += "\\";
                    memberFileName = System.IO.Path.GetFileName(file.FileName);
                    path += memberFileName;
                    try
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        file.SaveAs(path);
                    }
                    finally
                    {
                        file.InputStream.Close();
                        file.InputStream.Dispose();
                    }
                    _dsHelper.UpdateMemberReport(companyId, memberFileName, updatedBy);
                    response.Status = ServiceStatusCode.Success;
                    response.Message = "Summary report for the selected client have been updated successfully";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }


        #region Member
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberDetails"></param>
        /// <returns></returns>
        public Response<string> AddMember(Models.MemberModel memberDetails, int createdBy)
        {
            var response = new Response<string>();

            try
            {
                _dsHelper.AddMemberDetails(memberDetails.MemberName, memberDetails.CEBMemberId, memberDetails.Industry, memberDetails.Revenue, memberDetails.Region, createdBy);
                response.Status = ServiceStatusCode.Success;
                response.Message = "Client details added successfully";
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberDetails"></param>
        /// <returns></returns>
        public Response<string> UpdateMember(MemberModel memberDetails, int createdBy)
        {
            var response = new Response<string>();

            try
            {
                _dsHelper.UpdateMemberDetails(memberDetails.MemberName, memberDetails.Id, memberDetails.TimePeriod, memberDetails.Industry, memberDetails.Revenue, memberDetails.Region, createdBy);
                response.Status = ServiceStatusCode.Success;
                response.Message = "Client details updated successfully";
            }
            catch (Exception ex)
            {
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Response<string> GetMembers(bool isImport)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsMembers = _dsHelper.GetMembers();
                if (dsMembers.Tables != null && dsMembers.Tables[0].Rows.Count > 0)
                {
                    response.Status = ServiceStatusCode.Success;
                    response.Data = _jsonUtility.GetMemberDetails(dsMembers, isImport);
                }
                else
                {
                    response.Status = ServiceStatusCode.Failure;
                    response.Message = "No clients found";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public Response<string> DeleteMember(string members)
        {
            var response = new Response<string>();

            try
            {
                _dsHelper.DeleteMembers(members);
                response.Status = ServiceStatusCode.Success;
                response.Message = "Selected clients have been deleted successfully";
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        private int CheckifCompanyDataExists(string company)
        {
            return _dsHelper.CheckifCompanySurveyExists(company);
        }

        private string GetXML(List<string> listData)
        {
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<data>");
            xmlData.Append("<filters>");
            for (int i = 0; i < listData.Count; i++)
                xmlData.AppendFormat("<{0}><{1}>{2}</{1}></{0}>", "filter", "value", listData[i]);
            xmlData.Append("</filters>");
            xmlData.Append("</data>");
            string temp = xmlData.ToString().Replace("&", "&amp;").Replace("'", "&apos;");

            return temp;
        }

        private bool ValidateExcelFile(out string message, out string memberFileName, out string path, string htmlFileControl, string oldFileName)
        {
            message = "Problem in uploading...";
            memberFileName = string.Empty;
            path = string.Empty;
            string sheetName1 = ConfigurationManager.AppSettings["SheetName_1"];
            string sheetName2 = ConfigurationManager.AppSettings["SheetName_2"];
            List<string> oldFileList = new List<string>() { oldFileName };

            //Read file from the request
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile file = httpRequest.Files[htmlFileControl];
            if (httpRequest.Files[htmlFileControl].ContentLength > 0)
            {
                //Save the uploaded file in server for reference
                string fileExtension = System.IO.Path.GetExtension(file.FileName);
                fileExtension = fileExtension.ToLower();
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    path = MerlinConstants.SurveyDataPath;
                    DeleteFileAction(oldFileList, MerlinConstants.SurveyDataPath);

                    if (!path.EndsWith("\\"))
                        path += "\\";
                    memberFileName = System.IO.Path.GetFileName(file.FileName);

                    //string fileName = (new Random()).Next(0, 10000).ToString() + "_" + memberFileName;
                    path += memberFileName;

                    try
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        file.SaveAs(path);
                    }
                    finally
                    {
                        file.InputStream.Close();
                        file.InputStream.Dispose();
                    }

                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        System.IO.FileStream openFile = new System.IO.FileInfo(path).OpenRead();
                        try
                        {
                            pck.Load(openFile);

                            //pck.Load(new System.IO.FileInfo(path).OpenRead());
                            var ws = pck.Workbook.Worksheets[sheetName1];
                            var ws2 = pck.Workbook.Worksheets[sheetName2];
                            if (ws == null)
                            {
                                message = string.Format("Invalid survey file. Required sheet '{0}' is missing", sheetName1);
                                return false;
                            }
                            if (ws2 == null)
                            {
                                message = string.Format("Invalid survey file. Required sheet '{0}' is missing", sheetName2);
                                return false;
                            }
                            if (!Utility.ExcelProcessHelper.GetColumnByName(ws))
                            {
                                message = string.Format("Invalid survey file. Required columns are missing in the sheet '{0}'.", sheetName1);
                                return false;
                            }
                            if (!Utility.ExcelProcessHelper.GetColumnByName(ws2))
                            {
                                message = string.Format("Invalid survey file. Required columns are missing in the sheet '{0}'.", sheetName2);
                                return false;
                            }

                            return true;
                        }
                        finally
                        {
                            openFile.Close();
                        }
                    }
                }
                else
                {
                    message = "File not in correct format";
                    return false;
                }
            }
            else
            {
                message = "File not found";
                return false;
            }
        }

        private List<DataTable> ProcessGlobalFiltersandUpdateDataTable(int companyId, List<string> lstTimePeriod, List<DateTime> lstDateList, DataTable dtSurveyData, DataTable dtPerceptionGaps)
        {
            DataSet dsTemp;

            dtSurveyData.Columns.Add("fk_Supplier", typeof(int));
            dtSurveyData.Columns.Add("fk_Company", typeof(int));
            dtSurveyData.Columns.Add("fk_CategoryId", typeof(int));
            dtSurveyData.Columns.Add("fk_MetricId", typeof(int));
            dtSurveyData.Columns.Add("fk_KpiId", typeof(int));
            dtSurveyData.Columns.Add("dteTimePeriod", typeof(DateTime));
            dtSurveyData.AcceptChanges();
            //update company
            dtSurveyData.AsEnumerable().ToList<DataRow>().ForEach(row => { row["fk_Company"] = companyId; });

            dtPerceptionGaps.Columns.Add("fk_Supplier", typeof(int));
            dtPerceptionGaps.Columns.Add("fk_Company", typeof(int));
            dtPerceptionGaps.Columns.Add("fk_CategoryId", typeof(int));
            dtPerceptionGaps.Columns.Add("fk_MetricId", typeof(int));
            //dtPerceptionGaps.Columns.Add("fk_KpiId", typeof(int));
            dtPerceptionGaps.Columns.Add("dteTimePeriod", typeof(DateTime));
            dtPerceptionGaps.AcceptChanges();
            //update company
            dtPerceptionGaps.AsEnumerable().ToList<DataRow>().ForEach(row => { row["fk_Company"] = companyId; });

            string xml = GetXML(dtSurveyData.AsEnumerable().Select(C => C.Field<string>("Supplier")).Distinct().ToList());
            dsTemp = new DataSet();
            dsTemp = _dsHelper.AddSupplierByCompany(xml, companyId);
            //supplier
            for (int r = 0; r < dsTemp.Tables[0].Rows.Count; r++)
            {
                var rowsToUpdate = dtSurveyData.AsEnumerable().Where(C => C.Field<string>("Supplier").Equals(Convert.ToString(dsTemp.Tables[0].Rows[r]["Supplier"])));
                var rowsToUpdatePG = dtPerceptionGaps.AsEnumerable().Where(C => C.Field<string>("Supplier").Equals(Convert.ToString(dsTemp.Tables[0].Rows[r]["Supplier"])));
                foreach (var row in rowsToUpdate)
                    row.SetField("fk_Supplier", dsTemp.Tables[0].Rows[r]["pk_SupplierId"]);

                foreach (var row in rowsToUpdatePG)
                    row.SetField("fk_Supplier", dsTemp.Tables[0].Rows[r]["pk_SupplierId"]);
            }

            //DateTime Period
            for (int t = 0; t < lstTimePeriod.Count; t++)
            {
                var rowsToUpdate = dtSurveyData.AsEnumerable().Where(C => C.Field<string>("Time Period").Equals(lstTimePeriod[t]));
                foreach (var row in rowsToUpdate)
                    row.SetField("dteTimePeriod", lstDateList[t]);
            }

            xml = GetXML(dtSurveyData.AsEnumerable().Select(C => C.Field<string>("Category")).Distinct().ToList());
            dsTemp = new DataSet();
            dsTemp = _dsHelper.AddCategoryByCompany(xml, companyId);
            //supplier
            for (int r = 0; r < dsTemp.Tables[0].Rows.Count; r++)
            {
                var rowsToUpdate = dtSurveyData.AsEnumerable().Where(C => C.Field<string>("Category").Equals(Convert.ToString(dsTemp.Tables[0].Rows[r]["CategoryText"])));
                var rowsToUpdatePG = dtPerceptionGaps.AsEnumerable().Where(C => C.Field<string>("Category").Equals(Convert.ToString(dsTemp.Tables[0].Rows[r]["CategoryText"])));

                foreach (var row in rowsToUpdate)
                    row.SetField("fk_CategoryId", dsTemp.Tables[0].Rows[r]["pk_CategoryId"]);

                foreach (var row in rowsToUpdatePG)
                    row.SetField("fk_CategoryId", dsTemp.Tables[0].Rows[r]["pk_CategoryId"]);
            }

            xml = GetXML(dtSurveyData.AsEnumerable().Select(C => C.Field<string>("Metric")).Distinct().ToList());
            dsTemp = new DataSet();
            dsTemp = _dsHelper.AddMetricByCompany(xml, companyId);
            //supplier
            for (int r = 0; r < dsTemp.Tables[0].Rows.Count; r++)
            {
                var rowsToUpdate = dtSurveyData.AsEnumerable().Where(C => C.Field<string>("Metric").Equals(Convert.ToString(dsTemp.Tables[0].Rows[r]["MetricText"])));
                var rowsToUpdatePG = dtPerceptionGaps.AsEnumerable().Where(C => C.Field<string>("Metric").Equals(Convert.ToString(dsTemp.Tables[0].Rows[r]["MetricText"])));

                foreach (var row in rowsToUpdate)
                    row.SetField("fk_MetricId", dsTemp.Tables[0].Rows[r]["pk_MetricId"]);

                foreach (var row in rowsToUpdatePG)
                    row.SetField("fk_MetricId", dsTemp.Tables[0].Rows[r]["pk_MetricId"]);
            }

            xml = GetXML(dtSurveyData.AsEnumerable().Select(C => C.Field<string>("KPI")).Distinct().ToList());
            dsTemp = new DataSet();
            dsTemp = _dsHelper.AddKPIByCompany(xml, companyId);
            //supplier
            for (int r = 0; r < dsTemp.Tables[0].Rows.Count; r++)
            {
                var rowsToUpdate = dtSurveyData.AsEnumerable().Where(C => C.Field<string>("KPI").Equals(Convert.ToString(dsTemp.Tables[0].Rows[r]["KPIText"])));
                if (rowsToUpdate != null)
                {
                    try
                    {
                        foreach (var row in rowsToUpdate)
                            row.SetField("fk_KpiId", dsTemp.Tables[0].Rows[r]["pk_KpiID"]);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            dtPerceptionGaps.AcceptChanges();
            dtSurveyData.AcceptChanges();

            //for (int r = 0; r < dsTemp.Tables[0].Rows.Count; r++)
            //{
            //    var rowsToUpdatePG = dtPerceptionGaps.AsEnumerable().Where(C => C.Field<string>("KPI").Equals(Convert.ToString(dtSurveyData..Tables[0].Rows[r]["KPI Raw"])));

            //    if (rowsToUpdatePG != null)
            //    {
            //        try
            //        {
            //            foreach (var row in rowsToUpdatePG)
            //                row.SetField("fk_KpiId", dsTemp.Tables[0].Rows[r]["pk_KpiID"]);
            //        }
            //        catch (Exception e)
            //        {

            //        }
            //    }
            //}
            return new List<DataTable>() { dtSurveyData, dtPerceptionGaps };
        }

    }
}
