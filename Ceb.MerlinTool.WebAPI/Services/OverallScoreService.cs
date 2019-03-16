using Ceb.Logger;
using Ceb.MerlinTool.DataAccess;
using Ceb.MerlinTool.WebAPI.Constants;
using Ceb.MerlinTool.WebAPI.ExportHelper;
using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.Services.Relay;
using Ceb.MerlinTool.WebAPI.Services.Relay.Enum;
using Ceb.MerlinTool.WebAPI.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Services
{
    public class OverallScoreService : IOverAllScore
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private DataAccessHelper _dsHelper = new DataAccessHelper();
        private JSONConverter _jsonUtility = new JSONConverter();

        public Response<string> GetOverAllScore(int companyId)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsOverall = _dsHelper.GetOverAllScore(companyId);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetOverAllScore(dsOverall);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Data = response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> ExportHighchart(string chartJson, string title, string subTitle, string exportType, string company, string timePeriod, string page)
        {
            var response = new Response<string>();
            string ExportFilePath = string.Empty;
            try
            {
                HighChartsExportHelper objExport = new HighChartsExportHelper();
                chartJson = chartJson.Replace(",\r\n    \"backgroundColor\": \"#f2f2f2\"\r\n", "");
                string imageFile = HighChartExportPhantomJS.Export((chartJson));
                if (exportType.Equals("pptx") || exportType.Equals("pdf"))
                {
                    ExportFilePath = Path.Combine(ConfigurationManager.AppSettings["ExportFolderPath"], Guid.NewGuid().ToString() + ".pptx");
                    if (objExport.ExportAllPPTX(ExportFilePath, imageFile, title, subTitle, company, exportType, timePeriod,page))
                    {
                        if (exportType.Equals("pdf"))
                            ExportFilePath = ExportFilePath.Replace(".pptx", ".pdf");
                        response.Data = System.IO.Path.GetFileName(ExportFilePath);
                        response.Status = ServiceStatusCode.Success;
                    }
                    else
                    {
                        response.Status = ServiceStatusCode.Failure;
                        response.Message = "Problem in exporting.Please try again later";
                    }
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

        public Response<string> ExportToExcel(string tableJSON, string title, string subTitle, string company, string timePeriod, string page
            , string columnTitles = "")
        {
            var response = new Response<string>();
            string ExportFilePath = string.Empty;
            try
            {
                ExportFilePath = Path.Combine(ConfigurationManager.AppSettings["ExportFolderPath"], Guid.NewGuid().ToString() + ".xlsx");
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    Formatting = Formatting.None,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    Converters = new List<JsonConverter> { new DecimalConverter() }
                };

                DataTable dtData = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(tableJSON, settings);

                if (ExcelProcessHelper.ExportDashboardExcel(dtData, ExportFilePath, title, subTitle, company, timePeriod,
                    (columnTitles.Length == 0) ? MerlinConstants.ExcelColumnTitles.ContainsKey(page) ? MerlinConstants.ExcelColumnTitles[page] : null : Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(columnTitles), page))
                {
                    response.Data = System.IO.Path.GetFileName(ExportFilePath);
                    response.Status = ServiceStatusCode.Success;
                }
                else
                {
                    response.Status = ServiceStatusCode.Failure;
                    response.Message = "Problem in exporting.Please try again later";
                }
                return response;


            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Data = response.Message = ex.Message;
            }
            return response;
        }

        //private string WriteJson(string json)
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory;
        //    string fileName = Guid.NewGuid().ToString() + ".json";
        //    using (StreamWriter r = new StreamWriter(ConfigurationManager.AppSettings["ExportFolderPath"] + "\\" + fileName))
        //    {
        //        r.Write(json);
        //    }
        //    return ConfigurationManager.AppSettings["ExportFolderPath"] + "\\" + fileName;
        //}
    }

    class DecimalConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(decimal?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Float || token.Type == JTokenType.Integer)
            {
                return token.ToObject<decimal>();
            }
            if (token.Type == JTokenType.String)
            {
                // customize this to suit your needs
                return Decimal.Parse(token.ToString(),
                       System.Globalization.CultureInfo.GetCultureInfo("es-ES"));
            }
            if (token.Type == JTokenType.Null && objectType == typeof(decimal?))
            {
                return null;
            }
            throw new JsonSerializationException("Unexpected token type: " +
                                                  token.Type.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}