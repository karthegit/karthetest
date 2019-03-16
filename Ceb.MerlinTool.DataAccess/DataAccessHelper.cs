using Ceb.Logger;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceb.MerlinTool.DataAccess
{

    public class DataAccessHelper
    {
        private static string _strConnString = ConfigurationManager.ConnectionStrings["ApplicationConnStr"].ConnectionString;
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private static int commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

        public struct DbTableColumns
        {
            public const string Supplier_Overall_Score = "SupplierOverallScore";
            public const string Supplier = "fk_Supplier";
            public const string Company = "fk_Company";
            public const string TimePeriod = "TimePeriod";
            public const string Cust_CategoryId = "fk_CategoryId";
            public const string Cust_MetricId = "fk_Metric";
            public const string Cust_KPIId = "fk_KPI";
            public const string CatMetAvgScore = "CatMetAvgScore";
            public const string SupplierMetricScore = "SupplierMetricScore";
            public const string Email = "Email";
            public const string RespondentName = "Respondent's Name";
            public const string SupplierName = "Supplier's Name";
            public const string FunctionalArea = "Functional Area";
            public const string FunctionalAreaOthers = "Functional Area (Others)";
            public const string Location = "Location";
            public const string LocationOthers = "Location (Others)";
            public const string Level = "Level";
            public const string LevelOthers = "Level (Others)";
            public const string KPIRaw = "KPI Raw";
            public const string Score = "Score";
            public const string Comments = "Comments";
            public const string SupplierId = "Supplier Id";
            public const string CategoryId = "Category Id";
            public const string MetricId = "Metric Id";
            public const string MetricWeight = "Metric Weight";
            public const string KPIId = "KPI Id";
            public const string KPIAverage = "KPI Average";
            public const string FunctionalAreaId = "Functional Area Id";
            public const string LocationId = "Location Id";
            public const string LevelId = "Level Id";
            public const string CatAvgScore = "Cat Avg Score";
            public const string FuncMetricScore = "Func Metric Score";
            public const string LocationMetricScore = "Location Metric Score";
            public const string LocationOverallScore = "Location Overall Score";
            public const string LevelMetricScore = "Level Metric Score";
            public const string LevelOverallScore = "Level Overall Score";
            public const string FuncOverallScore = "Func Overall Score";
            public const string RespondentType = "Respondent Type";
            public const string DteTimePeriod = "DteTimePeriod";
            public const string InternalKPIScore = "Internal KPI Score";
            public const string SupplierKPIScore = "Supplier KPI Score";
            public const string NoofInternalRespondents = "No of Internal Respondents";
            public const string NoofSupplierRespondents = "No of Supplier Respondents";
            public const string KPIDifferential = "KPI Differential";
            public const string SupplierMetricScore_Internal = "Supplier Metric Score(Internal)";
            public const string SupplierMetricScore_Supplier = "Supplier Metric Score(Supplier)";
            public const string SupplierMetricDifferential = "Supplier Metric Differential";
            public const string SupplierOverallScore_Internal = "Supplier Overall Score(Internal)";
            public const string SupplierOverallScore_Supplier = "Supplier Overall Score(Supplier)";
            public const string SupplierOverallDifferential = "Supplier Overall Differential";
            public const string InternalRespondents = "Internal Respondents";
            public const string SupplierRespondents = "Supplier Respondents";
            public const string KPI = "KPI";
        }

        public struct ExcelTableColumns
        {
            public const string Supplier_Overall_Score = "Supplier Overall Score";
            public const string Supplier = "fk_Supplier";
            public const string Company = "fk_Company";
            public const string TimePeriod = "Time Period";
            public const string Cust_MetricId = "fk_MetricId";
            public const string Cust_CategoryId = "fk_CategoryId";
            public const string Cust_KPIId = "fk_KpiId";
            public const string CatMetAvgScore = "Cat Met Avg Score";
            public const string SupplierMetricScore = "Supplier Metric Score";
            public const string Email = "Email";
            public const string RespondentName = "Respondent's Name";
            public const string SupplierName = "Supplier's Name";
            public const string FunctionalArea = "Functional Area";
            public const string FunctionalAreaOthers = "Functional Area (Others)";
            public const string Location = "Location";
            public const string LocationOthers = "Location (Others)";
            public const string Level = "Level";
            public const string LevelOthers = "Level (Others)";
            public const string KPIRaw = "KPI Raw";
            public const string Score = "Score";
            public const string Comments = "Comments";
            public const string SupplierId = "Supplier Id";
            public const string CategoryId = "Category Id";
            public const string MetricId = "Metric Id";
            public const string MetricWeight = "Metric Weight";
            public const string KPIId = "KPI Id";
            public const string KPIAverage = "KPI Average";
            public const string FunctionalAreaID = "Functional Area Id";
            public const string LocationId = "Location Id";
            public const string LevelId = "Level Id";
            public const string CatAvgScore = "Cat Avg Score";
            public const string FuncMetricScore = "Func Metric Score";
            public const string LocationMetricScore = "Location Metric Score";
            public const string LocationOverallScore = "Location Overall Score";
            public const string LevelMetricScore = "Level Metric Score";
            public const string LevelOverallScore = "Level Overall Score";
            public const string FuncOverallScore = "Func Overall Score";
            public const string RespondentType = "Respondent Type";
            public const string DteTimePeriod = "dteTimePeriod";
            public const string InternalKPIScore = "Internal KPI Score";
            public const string SupplierKPIScore = "Supplier KPI Score";
            public const string NoofInternalRespondents = "No of Internal Respondents";
            public const string NoofSupplierRespondents = "No of Supplier Respondents";
            public const string KPIDifferential = "KPI Differential";
            public const string SupplierMetricScore_Internal = "Supplier Metric Score(Internal)";
            public const string SupplierMetricScore_Supplier = "Supplier Metric Score(Supplier)";
            public const string SupplierMetricDifferential = "Supplier Metric Differential";
            public const string SupplierOverallScore_Internal = "Supplier Overall Score(Internal)";
            public const string SupplierOverallScore_Supplier = "Supplier Overall Score(Supplier)";
            public const string SupplierOverallDifferential = "Supplier Overall Differential";
            public const string InternalRespondents = "Internal Respondents";
            public const string SupplierRespondents = "Supplier Respondents";
            public const string KPI = "KPI";
        }

        public bool UploadSurveyData(DataTable dtSurveyData, int companyId)
        {
            try
            {
                using (SqlConnection sql_con = new SqlConnection(_strConnString))
                {
                    sql_con.Open();
                    string sql = string.Format(@"DELETE FROM  tbl_SupplierByCompany WHERE fk_Company={0};", companyId);
                    SqlCommand sql_cmd = new SqlCommand(sql, sql_con);
                    sql_cmd.ExecuteNonQuery();
                    sql_con.Close();
                }

                SqlBulkCopy bulkCopy = new SqlBulkCopy(_strConnString);
                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierScore.SourceColumn = ExcelTableColumns.Supplier_Overall_Score;
                SupplierScore.DestinationColumn = DbTableColumns.Supplier_Overall_Score;
                bulkCopy.ColumnMappings.Add(SupplierScore);


                System.Data.SqlClient.SqlBulkCopyColumnMapping Supplier = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Supplier.SourceColumn = ExcelTableColumns.Supplier;
                Supplier.DestinationColumn = DbTableColumns.Supplier;
                bulkCopy.ColumnMappings.Add(Supplier);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Company = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Company.SourceColumn = ExcelTableColumns.Company;
                Company.DestinationColumn = DbTableColumns.Company;
                bulkCopy.ColumnMappings.Add(Company);

                System.Data.SqlClient.SqlBulkCopyColumnMapping cust_Category = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                cust_Category.SourceColumn = ExcelTableColumns.Cust_CategoryId;
                cust_Category.DestinationColumn = DbTableColumns.Cust_CategoryId;
                bulkCopy.ColumnMappings.Add(cust_Category);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Metric = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Metric.SourceColumn = ExcelTableColumns.Cust_MetricId;
                Metric.DestinationColumn = DbTableColumns.Cust_MetricId;
                bulkCopy.ColumnMappings.Add(Metric);


                System.Data.SqlClient.SqlBulkCopyColumnMapping KPI = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                KPI.SourceColumn = ExcelTableColumns.Cust_KPIId;
                KPI.DestinationColumn = DbTableColumns.Cust_KPIId;
                bulkCopy.ColumnMappings.Add(KPI);

                System.Data.SqlClient.SqlBulkCopyColumnMapping CatMetAvgScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                CatMetAvgScore.SourceColumn = ExcelTableColumns.CatMetAvgScore;
                CatMetAvgScore.DestinationColumn = DbTableColumns.CatMetAvgScore;
                bulkCopy.ColumnMappings.Add(CatMetAvgScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierMetricScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierMetricScore.SourceColumn = ExcelTableColumns.SupplierMetricScore;
                SupplierMetricScore.DestinationColumn = DbTableColumns.SupplierMetricScore;
                bulkCopy.ColumnMappings.Add(SupplierMetricScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping TimePeriod = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                TimePeriod.SourceColumn = ExcelTableColumns.TimePeriod;
                TimePeriod.DestinationColumn = DbTableColumns.TimePeriod;
                bulkCopy.ColumnMappings.Add(TimePeriod);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Email = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Email.SourceColumn = ExcelTableColumns.Email;
                Email.DestinationColumn = DbTableColumns.Email;
                bulkCopy.ColumnMappings.Add(Email);

                System.Data.SqlClient.SqlBulkCopyColumnMapping sqlRespondentName = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                sqlRespondentName.SourceColumn = ExcelTableColumns.RespondentName;
                sqlRespondentName.DestinationColumn = DbTableColumns.RespondentName;
                bulkCopy.ColumnMappings.Add(sqlRespondentName);

                System.Data.SqlClient.SqlBulkCopyColumnMapping sqlSupplierName = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                sqlSupplierName.SourceColumn = ExcelTableColumns.SupplierName;
                sqlSupplierName.DestinationColumn = DbTableColumns.SupplierName;
                bulkCopy.ColumnMappings.Add(sqlSupplierName);

                System.Data.SqlClient.SqlBulkCopyColumnMapping sqlFunctionalArea = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                sqlFunctionalArea.SourceColumn = ExcelTableColumns.FunctionalArea;
                sqlFunctionalArea.DestinationColumn = DbTableColumns.FunctionalArea;
                bulkCopy.ColumnMappings.Add(sqlFunctionalArea);

                System.Data.SqlClient.SqlBulkCopyColumnMapping FunctionalAreaOthers = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                FunctionalAreaOthers.SourceColumn = ExcelTableColumns.FunctionalAreaOthers;
                FunctionalAreaOthers.DestinationColumn = DbTableColumns.FunctionalAreaOthers;
                bulkCopy.ColumnMappings.Add(FunctionalAreaOthers);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Location = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Location.SourceColumn = ExcelTableColumns.Location;
                Location.DestinationColumn = DbTableColumns.Location;
                bulkCopy.ColumnMappings.Add(Location);

                System.Data.SqlClient.SqlBulkCopyColumnMapping LocationOthers = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                LocationOthers.SourceColumn = ExcelTableColumns.LocationOthers;
                LocationOthers.DestinationColumn = DbTableColumns.LocationOthers;
                bulkCopy.ColumnMappings.Add(LocationOthers);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Level = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Level.SourceColumn = ExcelTableColumns.Level;
                Level.DestinationColumn = DbTableColumns.Level;
                bulkCopy.ColumnMappings.Add(Level);

                System.Data.SqlClient.SqlBulkCopyColumnMapping LevelOthers = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                LevelOthers.SourceColumn = ExcelTableColumns.LevelOthers;
                LevelOthers.DestinationColumn = DbTableColumns.LevelOthers;
                bulkCopy.ColumnMappings.Add(LevelOthers);

                System.Data.SqlClient.SqlBulkCopyColumnMapping KPIRaw = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                KPIRaw.SourceColumn = ExcelTableColumns.KPIRaw;
                KPIRaw.DestinationColumn = DbTableColumns.KPIRaw;
                bulkCopy.ColumnMappings.Add(KPIRaw);


                System.Data.SqlClient.SqlBulkCopyColumnMapping Score = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Score.SourceColumn = ExcelTableColumns.Score;
                Score.DestinationColumn = DbTableColumns.Score;
                bulkCopy.ColumnMappings.Add(Score);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Comments = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Comments.SourceColumn = ExcelTableColumns.Comments;
                Comments.DestinationColumn = DbTableColumns.Comments;
                bulkCopy.ColumnMappings.Add(Comments);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierId.SourceColumn = ExcelTableColumns.SupplierId;
                SupplierId.DestinationColumn = DbTableColumns.SupplierId;
                bulkCopy.ColumnMappings.Add(SupplierId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping CategoryId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                CategoryId.SourceColumn = ExcelTableColumns.CategoryId;
                CategoryId.DestinationColumn = DbTableColumns.CategoryId;
                bulkCopy.ColumnMappings.Add(CategoryId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping MetricId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                MetricId.SourceColumn = ExcelTableColumns.MetricId;
                MetricId.DestinationColumn = DbTableColumns.MetricId;
                bulkCopy.ColumnMappings.Add(MetricId);


                System.Data.SqlClient.SqlBulkCopyColumnMapping MetricWeight = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                MetricWeight.SourceColumn = ExcelTableColumns.MetricWeight;
                MetricWeight.DestinationColumn = DbTableColumns.MetricWeight;
                bulkCopy.ColumnMappings.Add(MetricWeight);

                System.Data.SqlClient.SqlBulkCopyColumnMapping KPIId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                KPIId.SourceColumn = ExcelTableColumns.KPIId;
                KPIId.DestinationColumn = DbTableColumns.KPIId;
                bulkCopy.ColumnMappings.Add(KPIId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping KPIAverage = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                KPIAverage.SourceColumn = ExcelTableColumns.KPIAverage;
                KPIAverage.DestinationColumn = DbTableColumns.KPIAverage;
                bulkCopy.ColumnMappings.Add(KPIAverage);

                System.Data.SqlClient.SqlBulkCopyColumnMapping FunctionalAreaID = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                FunctionalAreaID.SourceColumn = ExcelTableColumns.FunctionalAreaID;
                FunctionalAreaID.DestinationColumn = DbTableColumns.FunctionalAreaId;
                bulkCopy.ColumnMappings.Add(FunctionalAreaID);

                System.Data.SqlClient.SqlBulkCopyColumnMapping LocationId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                LocationId.SourceColumn = ExcelTableColumns.LocationId;
                LocationId.DestinationColumn = DbTableColumns.LocationId;
                bulkCopy.ColumnMappings.Add(LocationId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping LevelId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                LevelId.SourceColumn = ExcelTableColumns.LevelId;
                LevelId.DestinationColumn = DbTableColumns.LevelId;
                bulkCopy.ColumnMappings.Add(LevelId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping CatAvgScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                CatAvgScore.SourceColumn = ExcelTableColumns.CatAvgScore;
                CatAvgScore.DestinationColumn = DbTableColumns.CatAvgScore;
                bulkCopy.ColumnMappings.Add(CatAvgScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping FuncMetricScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                FuncMetricScore.SourceColumn = ExcelTableColumns.FuncMetricScore;
                FuncMetricScore.DestinationColumn = DbTableColumns.FuncMetricScore;
                bulkCopy.ColumnMappings.Add(FuncMetricScore);


                System.Data.SqlClient.SqlBulkCopyColumnMapping LocationMetricScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                LocationMetricScore.SourceColumn = ExcelTableColumns.LocationMetricScore;
                LocationMetricScore.DestinationColumn = DbTableColumns.LocationMetricScore;
                bulkCopy.ColumnMappings.Add(LocationMetricScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping LocationOverallScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                LocationOverallScore.SourceColumn = ExcelTableColumns.LocationOverallScore;
                LocationOverallScore.DestinationColumn = DbTableColumns.LocationOverallScore;
                bulkCopy.ColumnMappings.Add(LocationOverallScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping LevelMetricScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                LevelMetricScore.SourceColumn = ExcelTableColumns.LevelMetricScore;
                LevelMetricScore.DestinationColumn = DbTableColumns.LevelMetricScore;
                bulkCopy.ColumnMappings.Add(LevelMetricScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping LevelOverallScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                LevelOverallScore.SourceColumn = ExcelTableColumns.LevelOverallScore;
                LevelOverallScore.DestinationColumn = DbTableColumns.LevelOverallScore;
                bulkCopy.ColumnMappings.Add(LevelOverallScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping RespondentType = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                RespondentType.SourceColumn = ExcelTableColumns.RespondentType;
                RespondentType.DestinationColumn = DbTableColumns.RespondentType;
                bulkCopy.ColumnMappings.Add(RespondentType);

                System.Data.SqlClient.SqlBulkCopyColumnMapping FuncOverallScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                FuncOverallScore.SourceColumn = ExcelTableColumns.FuncOverallScore;
                FuncOverallScore.DestinationColumn = DbTableColumns.FuncOverallScore;
                bulkCopy.ColumnMappings.Add(FuncOverallScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping dteTimePeriod = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                dteTimePeriod.SourceColumn = ExcelTableColumns.DteTimePeriod;
                dteTimePeriod.DestinationColumn = DbTableColumns.DteTimePeriod;
                bulkCopy.ColumnMappings.Add(dteTimePeriod);

                bulkCopy.DestinationTableName = "tbl_SupplierByCompany";
                bulkCopy.WriteToServer(dtSurveyData);

                return true;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(_objType, ex, ex.Message);
                return false;
            }
        }

        public bool UploadPerceptionGapsData(DataTable dtSurveyData, int companyId)
        {
            try
            {
                using (SqlConnection sql_con = new SqlConnection(_strConnString))
                {
                    sql_con.Open();
                    string sql = string.Format(@"DELETE FROM  tbl_PerceptionGap WHERE fk_Company={0};", companyId);
                    SqlCommand sql_cmd = new SqlCommand(sql, sql_con);
                    sql_cmd.ExecuteNonQuery();
                    sql_con.Close();
                }

                SqlBulkCopy bulkCopy = new SqlBulkCopy(_strConnString);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierId.SourceColumn = ExcelTableColumns.SupplierId;
                SupplierId.DestinationColumn = DbTableColumns.SupplierId;
                bulkCopy.ColumnMappings.Add(SupplierId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping CategoryId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                CategoryId.SourceColumn = ExcelTableColumns.CategoryId;
                CategoryId.DestinationColumn = DbTableColumns.CategoryId;
                bulkCopy.ColumnMappings.Add(CategoryId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping MetricId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                MetricId.SourceColumn = ExcelTableColumns.MetricId;
                MetricId.DestinationColumn = DbTableColumns.MetricId;
                bulkCopy.ColumnMappings.Add(MetricId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping KPIId = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                KPIId.SourceColumn = ExcelTableColumns.KPIId;
                KPIId.DestinationColumn = DbTableColumns.KPIId;
                bulkCopy.ColumnMappings.Add(KPIId);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Supplier = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Supplier.SourceColumn = ExcelTableColumns.Supplier;
                Supplier.DestinationColumn = DbTableColumns.Supplier;
                bulkCopy.ColumnMappings.Add(Supplier);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Company = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Company.SourceColumn = ExcelTableColumns.Company;
                Company.DestinationColumn = DbTableColumns.Company;
                bulkCopy.ColumnMappings.Add(Company);

                System.Data.SqlClient.SqlBulkCopyColumnMapping cust_Category = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                cust_Category.SourceColumn = ExcelTableColumns.Cust_CategoryId;
                cust_Category.DestinationColumn = DbTableColumns.Cust_CategoryId;
                bulkCopy.ColumnMappings.Add(cust_Category);

                System.Data.SqlClient.SqlBulkCopyColumnMapping Metric = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                Metric.SourceColumn = ExcelTableColumns.Cust_MetricId;
                Metric.DestinationColumn = DbTableColumns.Cust_MetricId;
                bulkCopy.ColumnMappings.Add(Metric);

                System.Data.SqlClient.SqlBulkCopyColumnMapping KPI = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                KPI.SourceColumn = ExcelTableColumns.KPI;
                KPI.DestinationColumn = DbTableColumns.KPI;
                bulkCopy.ColumnMappings.Add(KPI);

                System.Data.SqlClient.SqlBulkCopyColumnMapping InternalKPIScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                InternalKPIScore.SourceColumn = ExcelTableColumns.InternalKPIScore;
                InternalKPIScore.DestinationColumn = DbTableColumns.InternalKPIScore;
                bulkCopy.ColumnMappings.Add(InternalKPIScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierKPIScore = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierKPIScore.SourceColumn = ExcelTableColumns.SupplierKPIScore;
                SupplierKPIScore.DestinationColumn = DbTableColumns.SupplierKPIScore;
                bulkCopy.ColumnMappings.Add(SupplierKPIScore);

                System.Data.SqlClient.SqlBulkCopyColumnMapping NoofInternalRespondents = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                NoofInternalRespondents.SourceColumn = ExcelTableColumns.NoofInternalRespondents;
                NoofInternalRespondents.DestinationColumn = DbTableColumns.NoofInternalRespondents;
                bulkCopy.ColumnMappings.Add(NoofInternalRespondents);

                System.Data.SqlClient.SqlBulkCopyColumnMapping NoofSupplierRespondents = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                NoofSupplierRespondents.SourceColumn = ExcelTableColumns.NoofSupplierRespondents;
                NoofSupplierRespondents.DestinationColumn = DbTableColumns.NoofSupplierRespondents;
                bulkCopy.ColumnMappings.Add(NoofSupplierRespondents);

                System.Data.SqlClient.SqlBulkCopyColumnMapping KPIDifferential = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                KPIDifferential.SourceColumn = ExcelTableColumns.KPIDifferential;
                KPIDifferential.DestinationColumn = DbTableColumns.KPIDifferential;
                bulkCopy.ColumnMappings.Add(KPIDifferential);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierMetricScore_Internal = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierMetricScore_Internal.SourceColumn = ExcelTableColumns.SupplierMetricScore_Internal;
                SupplierMetricScore_Internal.DestinationColumn = DbTableColumns.SupplierMetricScore_Internal;
                bulkCopy.ColumnMappings.Add(SupplierMetricScore_Internal);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierMetricScore_Supplier = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierMetricScore_Supplier.SourceColumn = ExcelTableColumns.SupplierMetricScore_Supplier;
                SupplierMetricScore_Supplier.DestinationColumn = DbTableColumns.SupplierMetricScore_Supplier;
                bulkCopy.ColumnMappings.Add(SupplierMetricScore_Supplier);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierOverallScore_Internal = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierOverallScore_Internal.SourceColumn = ExcelTableColumns.SupplierOverallScore_Internal;
                SupplierOverallScore_Internal.DestinationColumn = DbTableColumns.SupplierOverallScore_Internal;
                bulkCopy.ColumnMappings.Add(SupplierOverallScore_Internal);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierOverallScore_Supplier = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierOverallScore_Supplier.SourceColumn = ExcelTableColumns.SupplierOverallScore_Supplier;
                SupplierOverallScore_Supplier.DestinationColumn = DbTableColumns.SupplierOverallScore_Supplier;
                bulkCopy.ColumnMappings.Add(SupplierOverallScore_Supplier);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierOverallDifferential = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierOverallDifferential.SourceColumn = ExcelTableColumns.SupplierOverallDifferential;
                SupplierOverallDifferential.DestinationColumn = DbTableColumns.SupplierOverallDifferential;
                bulkCopy.ColumnMappings.Add(SupplierOverallDifferential);

                System.Data.SqlClient.SqlBulkCopyColumnMapping InternalRespondents = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                InternalRespondents.SourceColumn = ExcelTableColumns.InternalRespondents;
                InternalRespondents.DestinationColumn = DbTableColumns.InternalRespondents;
                bulkCopy.ColumnMappings.Add(InternalRespondents);

                System.Data.SqlClient.SqlBulkCopyColumnMapping SupplierRespondents = new System.Data.SqlClient.SqlBulkCopyColumnMapping();
                SupplierRespondents.SourceColumn = ExcelTableColumns.SupplierRespondents;
                SupplierRespondents.DestinationColumn = DbTableColumns.SupplierRespondents;
                bulkCopy.ColumnMappings.Add(SupplierRespondents);

                bulkCopy.DestinationTableName = "tbl_PerceptionGap";
                bulkCopy.WriteToServer(dtSurveyData);

                return true;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(_objType, ex, ex.Message);
                return false;
            }
        }

        public int CheckifCompanySurveyExists(string companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            return Convert.ToInt16(SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "sp_CheckifCompanySurveyExists", sqlParams));
        }

        public DataSet GetAvailableInstitutions()
        {
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetInstitutions");
        }

        public int AddMemberDetails(string companyName, string companyId, string industry, string revenue, string region, int createdBy)
        {
            SqlParameter[] sqlParams = new SqlParameter[6];
            sqlParams[0] = new SqlParameter("@CompanyName", companyName);
            sqlParams[1] = new SqlParameter("@CEBId", companyId);
            sqlParams[2] = new SqlParameter("@Industry", industry);
            sqlParams[3] = new SqlParameter("@Revenue", revenue);
            sqlParams[4] = new SqlParameter("@Region", region);
            sqlParams[5] = new SqlParameter("@CreatedBy", createdBy);
            return Convert.ToInt16(SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_AddMemberDetails", sqlParams));
        }

        public int UpdateMemberDetails(string companyName, int id, string timePeriod, string industry, string revenue, string region, int updatedBy)
        {
            SqlParameter[] sqlParams = new SqlParameter[7];
            sqlParams[0] = new SqlParameter("@CompanyName", companyName);
            sqlParams[1] = new SqlParameter("@pk_MemberId", id);
            sqlParams[2] = new SqlParameter("@Industry", industry);
            sqlParams[3] = new SqlParameter("@Revenue", revenue);
            sqlParams[4] = new SqlParameter("@Region", region);
            sqlParams[5] = new SqlParameter("@UpdatedBy", updatedBy);
            sqlParams[6] = new SqlParameter("@TimePeriod", timePeriod);
            return Convert.ToInt16(SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_UpdateMemberDetails", sqlParams));
        }

        public int UpdateMemberDetails(int id, string fileName, string timePeriod, string availTimePeriods, int updatedBy)
        {
            SqlParameter[] sqlParams = new SqlParameter[5];
            sqlParams[0] = new SqlParameter("@pk_MemberId", id);
            sqlParams[1] = new SqlParameter("@FileName", fileName);
            sqlParams[2] = new SqlParameter("@TimePeriod", timePeriod);
            sqlParams[3] = new SqlParameter("@AvailTimePeriod", availTimePeriods);
            sqlParams[4] = new SqlParameter("@UpdatedBy", updatedBy);
            return Convert.ToInt16(SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_UpdateSurveyForMember", sqlParams));
        }

        public int UpdateMemberReport(int id, string fileName, int updatedBy)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@pk_MemberId", id);
            sqlParams[1] = new SqlParameter("@FileName", fileName);
            sqlParams[2] = new SqlParameter("@UpdatedBy", updatedBy);
            return Convert.ToInt16(SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_UpdateReportSummaryForMember", sqlParams));
        }


        public DataSet AddSupplierByCompany(string xmlData, int companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@v_str_xmlSupplierData", xmlData);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_AddSupplierByCompany", sqlParams);
        }

        public DataSet AddCategoryByCompany(string xmlData, int companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@v_str_xmlSupplierData", xmlData);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_AddCategoryByCompany", sqlParams);
        }

        public DataSet AddKPIByCompany(string xmlData, int companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@v_str_xmlSupplierData", xmlData);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_AddKPIByCompany", sqlParams);
        }

        public DataSet AddMetricByCompany(string xmlData, int companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@CompanyId", companyId);
            sqlParams[1] = new SqlParameter("@v_str_xmlSupplierData", xmlData);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_AddMetricByCompany", sqlParams);
        }

        public DataSet GetMembers()
        {
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetMembers");
        }

        public DataSet GetOverAllScore(int companyId)
        {
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@companyId", companyId);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetDataForOverAll", sqlParams);
        }

        //updated to fix the issue MT-44
        public void DeleteMembers(string membersList)
        {
            //SqlParameter[] sqlParams = new SqlParameter[1];
            //sqlParams[0] = new SqlParameter("@MemberIds", membersList);
            //SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_DeleteMembers", sqlParams);
            /*int commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"])*/;
            DataSet dsResult = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(_strConnString);
            SqlCommand cmd = new SqlCommand();

            SqlDataAdapter adapter;
            cmd.CommandTimeout = commandTimeout;
            cmd.CommandText = "SP_DeleteMembers";           
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@MemberIds", SqlDbType.Text).Value = membersList;                       
            cmd.Connection = sqlConnection;
            sqlConnection.Open();

            adapter = new SqlDataAdapter();
            adapter.DeleteCommand = cmd;
            adapter.DeleteCommand.ExecuteNonQuery();
            sqlConnection.Close();            
        }

        public  void DeleteSurveyData(string membersList,Boolean isEdit)
        {
            DataSet dataObj = new DataSet();
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@MemberIds", membersList);
            sqlParams[1] = new SqlParameter("@isEdit", isEdit);
            SqlConnection sqlConn = null;
            try
            {
                using (sqlConn = new SqlConnection(_strConnString))
                {
                    sqlConn.Open();
                    SqlCommand sqlCmd = new SqlCommand("SP_DeleteSurveyForMembers", sqlConn);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandTimeout = commandTimeout;
                    sqlCmd.Parameters.Add(sqlParams[0]);
                    sqlCmd.Parameters.Add(sqlParams[1]);
                    sqlCmd.ExecuteNonQuery();
                        
                }
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open) sqlConn.Close();
            }
       

            //SqlConnection sqlConnection = new SqlConnection(_strConnString);
            //SqlCommand cmd = new SqlCommand();

            //SqlDataAdapter adapter;
            //cmd.CommandTimeout = commandTimeout;
            //cmd.CommandText = "SP_DeleteSurveyForMembers";
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@MemberIds", SqlDbType.Text).Value = membersList;
            //cmd.Parameters.Add("@isEdit", SqlDbType.Bit).Value = isEdit;
            //cmd.Connection = sqlConnection;
            //sqlConnection.Open();

            //adapter = new SqlDataAdapter();
            //adapter.DeleteCommand = cmd;
            //adapter.DeleteCommand.ExecuteNonQuery();
            //sqlConnection.Close();
        }

        public DataSet CheckifUserExists(string email)
        {
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@Email", email);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_CheckifUserExists", sqlParams);
        }

        public DataSet GetAllUsers()
        {
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetUsers");
        }

        public int AddUserDetails(int companyId, int createdBy, string email, string name, bool isAdmin)
        {
            SqlParameter[] sqlParams = new SqlParameter[5];
            sqlParams[0] = new SqlParameter("@fk_Company", companyId);
            sqlParams[1] = new SqlParameter("@Email", email);
            sqlParams[2] = new SqlParameter("@CreatedBy", createdBy);
            sqlParams[3] = new SqlParameter("@Name", name);
            sqlParams[4] = new SqlParameter("@IsAdmin", isAdmin);
            return Convert.ToInt16(SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_AddToolUser", sqlParams));
        }

        public void DeleteUser(string users)
        {
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@UserIds", users);
            SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_DeleteUsers", sqlParams);
        }

        public void UpdateIsAdmin(string email, bool isAdmin, int updatedBy)
        {
            SqlParameter[] sqlParams = new SqlParameter[3];
            sqlParams[0] = new SqlParameter("@Email", email);
            sqlParams[1] = new SqlParameter("@IsAdmin", isAdmin);
            sqlParams[2] = new SqlParameter("@UpdatedBy", updatedBy);
            SqlHelper.ExecuteScalar(_strConnString, CommandType.StoredProcedure, "SP_UpdateUser", sqlParams);
        }

        public DataSet GetLog(DateTime fromDate, DateTime toDate)
        {
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@fromDate", fromDate);
            sqlParams[1] = new SqlParameter("@toDate", toDate);
            return SqlHelper.ExecuteDataset(_strConnString, CommandType.StoredProcedure, "SP_GetLogDetails", sqlParams);
        }
    }
}


