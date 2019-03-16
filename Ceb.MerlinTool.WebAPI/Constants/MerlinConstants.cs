using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Constants
{
    public static class MerlinConstants
    {
        public const string CATEGORYANALYSISTITLE = "Supplier Scores, by Category";
        public const string OVERALLTITLE = "Overall Supplier Ratings";
        public const string SUPPLIERBYMETRIC = "Supplier Metric Scores";
        public static string SUPPLIERBYKPI = "Supplier KPI Scores";
        public const string METRICTRENDS = "Metric Trends";
        public static string KPITRENDS = "KPI Trends";
        public static string TRENDKPISCORE = "Trend Supplier KPI Scores";
        public const string TRENDMETRICSCORE = "Trend Supplier Metric Scores";
        public const string TRENDOVERALLSCORE = "Overall Supplier Trends";
        public static string OVERALLGRIDTITLE = "Overall and Metric Ratings, by Category";
        public static string PERCEPTIONKPISCORE = "Perception Supplier KPI Scores";
        public const string PERCEPTIONMETRICSCORE = "Perception Supplier Metric Scores";

        public static string SurveyDataPath = ConfigurationManager.AppSettings["SurveyFilePath"];
        public static string MEMBERREPORTSUMMARY = ConfigurationManager.AppSettings["MemberReportPath"];
        public static List<Ceb.MerlinTool.WebAPI.SalesforceServiceReference.Institution> InstitutionList = new List<Ceb.MerlinTool.WebAPI.SalesforceServiceReference.Institution>() { };
        public static Dictionary<string, string> OverAllRatingColumnTitle = new Dictionary<string, string>() { { "OverAllScore", "Overall Score" }, { "MetricText", "Metric" }, { "KPIText", "KPI" } };
        public static List<string> ExcelRequiredColumns = new List<string>() { 
            "Supplier Overall Score", "Time Period","Cat Met Avg Score","Respondent Type","Metric","Supplier","Category","KPI","Func Overall Score",
            "Supplier Metric Score", "Email", "Respondent's Name", "Supplier's Name","Functional Area", "Functional Area (Others)","Location",
            "Location (Others)","Level","Level (Others)","KPI Raw","Score", "Comments","Supplier Id","Category Id","Metric Id","Metric Weight", "KPI Id",
            "KPI Average","Functional Area Id","Location Id", "Level Id","Cat Avg Score","Func Metric Score", "Location Metric Score","Location Overall Score","Level Metric Score", "Level Overall Score"
        };

        public static List<string> ExcelRequiredColumnsPerceptionGap = new List<string>() { 
            	"Supplier Id" ,	"Category Id","Metric Id" ,"KPI Id","Internal KPI Score","Supplier KPI Score","No of Internal Respondents","No of Supplier Respondents"
                ,"KPI Differential","Supplier Metric Score(Internal)","Supplier Metric Score(Supplier)","Supplier Metric Differential","Supplier Overall Score(Internal)"
                ,"Supplier Overall Score(Supplier)","Supplier Overall Differential","Internal Respondents","Supplier Respondents"
        };

        public static Dictionary<string, string> ExportMappings = new Dictionary<string, string>() { 
        { "Overall Supplier Ratings", "OverallSupplierRating" },
        { "Supplier Scores, by Category", "SupplierScores,byCategory" }, 
        { "Overall and Metric Ratings, by Category", "OverallRatingsByCategory" } ,
        { "Supplier Metric Scores", "SupplierAnalysis" } ,
        { "Supplier KPI Scores", "SupplierAnalysis" } ,
        { "Supplier Comments", "SupplierAnalysis" } ,
        {"Overall Supplier Trends","OverallSupplierScore"},
        {"Metric Trends","MetricTrends"},
        {"KPI Trends","KPITrends"},
        {"Trend Supplier KPI Scores","SupplierKPIScores"},
        {"Trend Supplier Metric Scores","SupplierMetricScores"},
        {"Perception Supplier KPI Scores","SupplierKPIScores"},
        {"Perception Supplier Metric Scores","SupplierMetricScores"}
        };
        public static Dictionary<string, Dictionary<string, string>> ExcelColumnTitles = new Dictionary<string, Dictionary<string, string>>() { 
        {"Overall Supplier Ratings",new Dictionary<string,string>(){{"Score","Supplier Rating"},{"Supplier","Supplier Name"},{"NoOfRespondants","No of Respondents"}}},
        {"Supplier Scores, by Category",new Dictionary<string,string>(){{"Score","Supplier Rating"},{"Supplier","Supplier Name"},{"CatAvgScore","Category Average Score"},{"NoOfRespondants","No of Respondents"}}},
        {"Supplier Metric Scores",new Dictionary<string,string>(){{"Score","Metric Rating"},{"MetricText","Metric"},{"NoOfRespondants","No of Respondents"},{"CatMetAvgScore","Category Average Score"}}},
        {"Supplier KPI Scores",new Dictionary<string,string>(){{"Score","Supplier Score"},{"KPIText","KPI"},{"NoOfRespondants","No of Respondents"},{"KPIAverage","Category Average"},{"MetricText","Metric"}}},
        {"Supplier Comments",new Dictionary<string,string>(){{"Metric","Metric"},{"KPI","KPI"},{"Comments","Comments"}}},
        {"Overall Supplier Trends",new Dictionary<string,string>(){{"TimePeriod","Year"},{"OverallScore","Supplier Score"}}},
        {"Metric Trends",new Dictionary<string,string>(){{"TimePeriod","Year"},{"MetricText","Metric"},{"Score","Metrics Score"}}},
        {"KPI Trends",new Dictionary<string,string>(){{"TimePeriod","Year"},{"KPIText","KPI"},{"Score","KPI Score"}}},
        {"Trend Supplier KPI Scores",new Dictionary<string,string>(){{"TimePeriod","Year"},{"KPIText","KPI"},{"Score","KPI Score"}}},
        {"Trend Supplier Metric Scores",new Dictionary<string,string>(){{"TimePeriod","Year"},{"MetricText","Metric"},{"Score","Metrics Score"}}},
        {"Perception Supplier KPI Scores",new Dictionary<string,string>(){{"MetricText","Metric"},{"KPIText","KPI"},{"Score","KPI Score(Internal)"},{"KPIAverage","KPI Score(Supplier)"}}},
        {"Perception Supplier Metric Scores",new Dictionary<string,string>(){{"MetricText","Metric"},{"InternalScore","Metric Score(Internal)"},{"Score","Metric Score(Supplier)"}}},
       };

        public static Dictionary<string, Dictionary<string, string>> Demographic = new Dictionary<string, Dictionary<string, string>>() 
        {
        { "Functional Area" ,new Dictionary<string,string>(){{"Functional Area","Functional Area Id"} }  },
        { "Level" ,new Dictionary<string,string>(){{"Level","Level Id"} }  },
        { "Location" ,new Dictionary<string,string>(){{"Location","Location Id"} }  },
        };

        public static Dictionary<string, Dictionary<string, string>> DemographicDBColumns = new Dictionary<string, Dictionary<string, string>>() 
        {
        { "Functional Area" ,new Dictionary<string,string>(){{"[Func Overall Score]","[Functional Area Id]"} }  },
        { "Level" ,new Dictionary<string,string>(){{"[Level Overall Score]","[Level Id]"} }  },
        { "Location" ,new Dictionary<string,string>(){{"[Location Overall Score]","[Location Id]"} }  },
        };

        public static Dictionary<string, Dictionary<string, string>> DemographicMetricDBColumns = new Dictionary<string, Dictionary<string, string>>() 
        {
        { "Functional Area" ,new Dictionary<string,string>(){{"[Func Metric Score]","[Functional Area Id]"} }  },
        { "Level" ,new Dictionary<string,string>(){{"[Level Metric Score]","[Level Id]"} }  },
        { "Location" ,new Dictionary<string,string>(){{"[Location Metric Score]","[Location Id]"} }  },
        };
    }
}