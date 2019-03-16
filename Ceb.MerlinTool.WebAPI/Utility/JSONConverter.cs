using Ceb.Logger;
using Ceb.MerlinTool.WebAPI.Constants;
using Ceb.MerlinTool.WebAPI.Models;
using Ceb.MerlinTool.WebAPI.SalesforceServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Utility
{
    public class JSONConverter
    {
        public string GetInstitutions(List<Institution> institutionList)
        {
            var instList = institutionList.Select(I => new { displayId = I.CEBId, id = I.CEBId + "," + I.Name, name = I.Name });
            return JsonConvert.SerializeObject(instList);
        }

        public string GetInstitutions(DataSet institutionList, string page)
        {
            if (page.Equals("accessDetails"))
            {
                var instList = institutionList.Tables[0].Rows.OfType<DataRow>().Select(S => new
                {
                    displayId = S["CEBId"],
                    id = S["pk_CompanyId"],
                    name = S["CompanyName"]
                }).ToList();

                return JsonConvert.SerializeObject(instList);
            }
            else
            {
                var instList = institutionList.Tables[0].Rows.OfType<DataRow>().Where(C => (C["FileName"] == DBNull.Value) || string.IsNullOrEmpty(Convert.ToString(C["FileName"]))).Select(S => new
                {
                    displayId = S["CEBId"],
                    id = S["pk_CompanyId"],
                    name = S["CompanyName"]
                }).ToList();

                return JsonConvert.SerializeObject(instList);
            }
        }

        public string GetMemberDetails(DataSet dsMembers, bool isImport)
        {
            List<MemberModel> lstMemberDetails = dsMembers.Tables[0].Rows.OfType<DataRow>().Where(C => ((C["FileName"] != DBNull.Value && !(string.IsNullOrEmpty(Convert.ToString(C["FileName"])))) && isImport) || (!isImport)).Select(S => new MemberModel
            {
                FileName = Convert.ToString(S["FileName"]),
                CEBMemberId = Convert.ToString(S["CEBId"]),
                Id = Convert.ToInt16(S["pk_CompanyId"]),
                MemberName = Convert.ToString(S["CompanyName"]),
                TimePeriod = Convert.ToString(S["TimePeriod"]),
                Industry = Convert.ToString(S["Industry"]),
                Region = Convert.ToString(S["Region"]),
                Revenue = Convert.ToString(S["Revenue"]),
                SummaryReportName = Convert.ToString(S["summaryReportPath"]),
                FilePath = System.IO.Path.Combine(Ceb.MerlinTool.WebAPI.Constants.MerlinConstants.SurveyDataPath, Convert.ToString(S["FileName"])),
                SummaryFilePath = System.IO.Path.Combine(Ceb.MerlinTool.WebAPI.Constants.MerlinConstants.MEMBERREPORTSUMMARY, Convert.ToString(S["summaryReportPath"])),
                AvailableTimePeriods = Convert.ToString(S["AvailableTimePeriods"]).Replace('$', '\n'),
                NoOfAvailPeriods = (S["AvailableTimePeriods"] != DBNull.Value || Convert.ToString(S["AvailableTimePeriods"]).Length > 0) ? Convert.ToString(S["AvailableTimePeriods"]).Split('$').Length : 0
            }).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(lstMemberDetails);
        }

        public string GetOverAllScore(DataSet dsOverall)
        {
            Dictionary<string, string> columnTitles = new Dictionary<string, string>();
            var supplierScore = dsOverall.Tables[0].Rows.OfType<DataRow>().OrderByDescending(o => o["Score"]).Select(S => new
            {
                score = new Dictionary<string, object>() { { "y", S["Score"] }, { "n", S["NoOfRespondants"] } },
                axistext = S["Supplier"]
            });

            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);

            //DataColumn[] dcColumns = dsOverall.Tables[1].Columns.Cast<DataColumn>().ToArray();
            //int i = 0;
            var regexItem = new Regex("^[a-zA-Z0-9_]*$");
            int i = 0;
            foreach (DataColumn c in dsOverall.Tables[1].Columns)
            {
                string temp = MerlinConstants.OverAllRatingColumnTitle.ContainsKey(c.ColumnName) ? MerlinConstants.OverAllRatingColumnTitle[c.ColumnName] : c.ColumnName;
                if (!regexItem.IsMatch(c.ColumnName))
                {
                    c.ColumnName = "metric" + ++i;
                }
                else
                {
                    c.ColumnName = c.ColumnName.Replace(',', ' ');
                    c.ColumnName = String.Join("", c.ColumnName.Split());
                }
                columnTitles.Add(c.ColumnName, temp);
            }
            var tblMetricScore = dsOverall.Tables[1];

            //Overall table color ranges
            var colorRange = new
            {
                range1From = ConfigurationManager.AppSettings["range1From"],
                range1To = ConfigurationManager.AppSettings["range1To"],
                range1Color = ConfigurationManager.AppSettings["range1Color"],

                range2From = ConfigurationManager.AppSettings["range2From"],
                range2To = ConfigurationManager.AppSettings["range2To"],
                range2Color = ConfigurationManager.AppSettings["range2Color"],

                range3From = ConfigurationManager.AppSettings["range3From"],
                range3To = ConfigurationManager.AppSettings["range3To"],
                range3Color = ConfigurationManager.AppSettings["range3Color"],

                range4From = ConfigurationManager.AppSettings["range4From"],
                range4To = ConfigurationManager.AppSettings["range4To"],
                range4Color = ConfigurationManager.AppSettings["range4Color"],

                colorIfzero = ConfigurationManager.AppSettings["greyColor"],
                kpiIndicator = ConfigurationManager.AppSettings["KPIRiskIndicator"]
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "tableRangeColor", colorRange }, { "companyDetails", dsOverall.Tables[2] }, { "excelExport", excelExportData }, { "supplierScore", supplierScore }, { "catMetAvgScore", Newtonsoft.Json.JsonConvert.SerializeObject(tblMetricScore) }, { "tableColumnTitles", columnTitles } });
        }

        public string GetUsers(DataSet dsUsers)
        {
            //var dataRows = isImport ? dsMembers.Tables[0].Rows.OfType<DataRow>().Where(C => (C["FileName"] != DBNull.Value || !string.IsNullOrEmpty(Convert.ToString(C["FileName"])))).Select(S => S)
            //                       : dsMembers.Tables[0].Rows.OfType<DataRow>().Select(S => S);
            var lstUsers = dsUsers.Tables[0].Rows.OfType<DataRow>().Select(S => new
            {
                Id = Convert.ToInt16(S["pk_UserId"]),
                MemberName = Convert.ToString(S["CompanyName"]),
                Email = S["Email"],
                Name = S["Name"],
                IsAdmin = S["IsAdmin"] == DBNull.Value ? false : S["IsAdmin"],
                CEBId = S["CEBId"],
                CompanyId = S["fk_Company"],
                SummaryReportPath = System.IO.Path.Combine(Ceb.MerlinTool.WebAPI.Constants.MerlinConstants.MEMBERREPORTSUMMARY, Convert.ToString(S["summaryReportPath"])),
                SummaryReportName = Convert.ToString(S["summaryReportPath"])
            }).ToList();
            return Newtonsoft.Json.JsonConvert.SerializeObject(lstUsers);
        }

        public string GetJSONForFilter(DataSet dsFilter)
        {
            var filterList = dsFilter.Tables[0].Rows.OfType<DataRow>().Select(S => new
            {
                Id = S["Id"],
                Text = S["Text"]
            }).ToList().OrderBy(O => O.Text);
            return Newtonsoft.Json.JsonConvert.SerializeObject(filterList);
        }

        public string GetJSONForFilterSupplier(DataSet dsFilter)
        {
            var filterList = dsFilter.Tables[0].Rows.OfType<DataRow>().Select(S => new
            {
                Id = S["Id"],
                Text = S["Text"],
                CategoryId = S["fk_CategoryId"]
            }).ToList().OrderBy(O => O.Text);
            return Newtonsoft.Json.JsonConvert.SerializeObject(filterList);
        }

        public string GetScoreByCategory(DataSet dsOverall)
        {
            Dictionary<string, string> columnTitles = new Dictionary<string, string>();
            var supplierScore = dsOverall.Tables[0].Rows.OfType<DataRow>().OrderBy(o => o["Supplier"]).Select(S => new
            {
                score = new Dictionary<string, object>() { { "y", S["Score"] }, { "n", S["NoOfRespondants"] }, { "id", S["pk_SupplierId"] }, { "benchmarkScore", S["CatAvgScore"] } },
                axistext = S["Supplier"]
            });

            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "companyDetails", dsOverall.Tables[1] }, { "excelExport", excelExportData }, { "supplierScore", supplierScore } });
        }

        public string GetSupplierOverallScore(DataSet dsOverall)
        {
            //Dictionary<string, string> columnTitles = new Dictionary<string, string>();
            var supplierScore = dsOverall.Tables[0].Rows.OfType<DataRow>().Select(S => new
            {
                Score1 = S["OverallScore"],
                Score2 = S["CatAvgScore"]
            });

            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "companyDetails", dsOverall.Tables[1] }, { "excelExport", excelExportData }, { "supplierScore", supplierScore } });
        }

        public string GetSupplierMetricScore(DataSet dsOverall)
        {
            Dictionary<string, string> columnTitles = new Dictionary<string, string>();
            var supplierScore = dsOverall.Tables[0].Rows.OfType<DataRow>().OrderBy(o => o["MetricText"]).Select(S => new
            {
                score = new Dictionary<string, object>() { { "y", S["Score"] }, { "n", S["NoOfRespondants"] }, { "benchmarkScore", S["CatMetAvgScore"] } },
                axistext = S["MetricText"]
            });

            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "companyDetails", dsOverall.Tables[1] }, { "excelExport", excelExportData }, { "supplierScore", supplierScore } });
        }

        public string GetSupplierKPIScore(DataSet dsOverall)
        {

            var colorRange = new
            {
                range1From = ConfigurationManager.AppSettings["range1From"],
                range1To = ConfigurationManager.AppSettings["range1To"],
                range1Color = ConfigurationManager.AppSettings["range1Color"],

                range2From = ConfigurationManager.AppSettings["range2From"],
                range2To = ConfigurationManager.AppSettings["range2To"],
                range2Color = ConfigurationManager.AppSettings["range2Color"],

                range3From = ConfigurationManager.AppSettings["range3From"],
                range3To = ConfigurationManager.AppSettings["range3To"],
                range3Color = ConfigurationManager.AppSettings["range3Color"],

                range4From = ConfigurationManager.AppSettings["range4From"],
                range4To = ConfigurationManager.AppSettings["range4To"],
                range4Color = ConfigurationManager.AppSettings["range4Color"],
                colorIfzero = ConfigurationManager.AppSettings["greyColor"],
                kpiIndicator = ConfigurationManager.AppSettings["KPIRiskIndicator"]
            };
            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "excelExportData", excelExportData }, { "tableRangeColor", colorRange }, { "companyDetails", dsOverall.Tables[1] }, { "supplierScore", dsOverall.Tables[0] } });
        }

        public string GetSupplierComments(DataSet dsComments)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "companyDetails", dsComments.Tables[1] }, { "supplierComment", dsComments.Tables[0] } });
        }

        #region TrendAnalysis
        public string GetJSONForTimePeriod(DataSet dsFilter)
        {
            var filterList = dsFilter.Tables[0].Rows.OfType<DataRow>().Select(S => new
            {
                Id = S["TimePeriod"],
                Text = S["TimePeriod"]
            });
            return Newtonsoft.Json.JsonConvert.SerializeObject(filterList);
        }

        public string GetOverallScoreByTimePeriod(DataSet dsOverall)
        {
            string excelExportData;
            var supplierScore = dsOverall.Tables[0].Rows.OfType<DataRow>().Select(S => new
            {
                score = S["OverallScore"],
                axistext = S["TimePeriod"]
            });

            var series = new Dictionary<string, object>()
            {
              { "name", "Overall Supplier Trends" }, { "data", dsOverall.Tables[0].Rows.OfType<DataRow>().Select(S => S["OverallScore"]).ToList()}
            };

            var categories = dsOverall.Tables[0].Rows.OfType<DataRow>().Select(S => S["TimePeriod"]).Distinct().ToList();
            if (dsOverall.Tables.Count <= 2)
                excelExportData = "";
            else
                excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[2]);

            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "supplierScore", supplierScore }, { "companyDetails", dsOverall.Tables[1] }, { "excelExport", excelExportData }, { "series", new List<object> { series } }, { "categories", categories } });
        }

        public string GetSupplierMetricScoreByTimePeriod(DataSet dsOverall)
        {
            if (dsOverall != null && dsOverall.Tables[0] != null && dsOverall.Tables[0].Rows.Count > 0)
            {
                List<Dictionary<string, object>> series = new List<Dictionary<string, object>>();
                List<string> categories = dsOverall.Tables[0].Rows.OfType<DataRow>().Select(S => Convert.ToString(S["TimePeriod"])).Distinct().ToList();
                List<string> metricList = dsOverall.Tables[3].AsEnumerable().OrderBy(o => Convert.ToString(o["MetricText"])).Select(S => Convert.ToString(S["MetricText"])).Distinct().ToList();
                for (int metric = 0; metric < metricList.Count; metric++)
                {
                    string name = metricList[metric];
                    List<object> data = new List<object>();
                    for (int t = 0; t < categories.Count; t++)
                    {
                        object obj = dsOverall.Tables[0].Rows.OfType<DataRow>().Where(C => C.Field<string>("TimePeriod").Equals(categories[t]) && C.Field<string>("MetricText").Equals(name)).Select(S => S["Score"]).FirstOrDefault();
                        obj = obj == DBNull.Value ? "null" : obj;
                        data.Add(obj);
                    }
                    if (data.Where(d => d != null).Count() > 0)
                        series.Add(new Dictionary<string, object>() { { "name", name }, { "data", data } });
                }

                var supplierScore = dsOverall.Tables[0].Rows.OfType<DataRow>().OrderBy(o => o["MetricText"]).Select(S => new
                {
                    score = new Dictionary<string, object>() { { "y", S["Score"] } },
                    axistext = S["MetricText"]
                });

                var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[2]);
                return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "supplierScore", supplierScore }, { "companyDetails", dsOverall.Tables[1] }, { "excelExport", excelExportData }, { "series", series }, { "categories", categories } });
            }
            else
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "supplierScore", new List<string>() }, { "companyDetails", dsOverall.Tables[1] }, { "excelExport", "[]" }, { "series", "[]" }, { "categories", "[]" } });
            }
        }

        public string GetSupplierKPIScoreByTimePeriod(DataSet dsOverall)
        {
            System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

            Dictionary<string, string> columnTitles = new Dictionary<string, string>();

            var regexItem = new Regex("^[a-zA-Z0-9_]*$");
            int i = 0;
            foreach (DataColumn c in dsOverall.Tables[0].Columns)
            {
                string temp = MerlinConstants.OverAllRatingColumnTitle.ContainsKey(c.ColumnName) ? MerlinConstants.OverAllRatingColumnTitle[c.ColumnName] : c.ColumnName;
                if (!regexItem.IsMatch(c.ColumnName))
                {
                    c.ColumnName = "timeperiod" + ++i;
                }
                else
                {
                    c.ColumnName = c.ColumnName.Replace(',', ' ');
                    c.ColumnName = String.Join("", c.ColumnName.Split());
                }
                columnTitles.Add(c.ColumnName, temp);
            }

            var colorRange = new
            {
                range1From = ConfigurationManager.AppSettings["range1From"],
                range1To = ConfigurationManager.AppSettings["range1To"],
                range1Color = ConfigurationManager.AppSettings["range1Color"],

                range2From = ConfigurationManager.AppSettings["range2From"],
                range2To = ConfigurationManager.AppSettings["range2To"],
                range2Color = ConfigurationManager.AppSettings["range2Color"],

                range3From = ConfigurationManager.AppSettings["range3From"],
                range3To = ConfigurationManager.AppSettings["range3To"],
                range3Color = ConfigurationManager.AppSettings["range3Color"],

                range4From = ConfigurationManager.AppSettings["range4From"],
                range4To = ConfigurationManager.AppSettings["range4To"],
                range4Color = ConfigurationManager.AppSettings["range4Color"],
                colorIfzero = ConfigurationManager.AppSettings["greyColor"],
                kpiIndicator = ConfigurationManager.AppSettings["KPIRiskIndicator"]
            };

            //var excelExportData = dsOverall.Tables[0];
            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);

            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "tableColumnTitles", columnTitles }, { "tableRangeColor", colorRange }, { "supplierScore", excelExportData }, { "companyDetails", dsOverall.Tables[1] }, { "excelExport", excelExportData } });
        }
        #endregion

        #region Perception Gaps
        public string GetSupplierOverallScorePG(DataSet dsOverall)
        {
            var supplierScore = dsOverall.Tables[0].Rows.OfType<DataRow>().Select(S => new
            {
                Score1 = S["OverallScore"],
                Score2 = S["Internal"]
            });

            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "companyDetails", dsOverall.Tables[1] }, { "excelExport", excelExportData }, { "supplierScore", supplierScore } });
        }

        public string GetSupplierMetricScorePG(DataSet dsOverall)
        {
            Dictionary<string, string> columnTitles = new Dictionary<string, string>();
            var supplierScore = dsOverall.Tables[0].Rows.OfType<DataRow>().OrderBy(o => o["MetricText"]).Select(S => new
            {
                score = new Dictionary<string, object>() { { "y", S["InternalScore"] }, { "benchmarkScore", S["Score"] } },
                axistext = S["MetricText"]
            });

            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "companyDetails", dsOverall.Tables[1] }, { "excelExport", excelExportData }, { "supplierScore", supplierScore } });
        }

        public string GetSupplierKPIScorePG(DataSet dsOverall)
        {

            var colorRange = new
            {
                range1From = ConfigurationManager.AppSettings["range1From"],
                range1To = ConfigurationManager.AppSettings["range1To"],
                range1Color = ConfigurationManager.AppSettings["range1Color"],

                range2From = ConfigurationManager.AppSettings["range2From"],
                range2To = ConfigurationManager.AppSettings["range2To"],
                range2Color = ConfigurationManager.AppSettings["range2Color"],

                range3From = ConfigurationManager.AppSettings["range3From"],
                range3To = ConfigurationManager.AppSettings["range3To"],
                range3Color = ConfigurationManager.AppSettings["range3Color"],

                range4From = ConfigurationManager.AppSettings["range4From"],
                range4To = ConfigurationManager.AppSettings["range4To"],
                range4Color = ConfigurationManager.AppSettings["range4Color"],
                colorIfzero = ConfigurationManager.AppSettings["greyColor"],
                kpiIndicator = ConfigurationManager.AppSettings["KPIRiskIndicator"]
            };
            var excelExportData = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new Dictionary<string, object>() { { "excelExportData", excelExportData }, { "tableRangeColor", colorRange }, { "companyDetails", dsOverall.Tables[1] }, { "supplierScore", dsOverall.Tables[0] } });
        }
        #endregion
    }

}
