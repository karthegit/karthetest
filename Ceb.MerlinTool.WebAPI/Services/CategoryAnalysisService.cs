using Ceb.Logger;
using Ceb.MerlinTool.DataAccess;
using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.Services.Relay;
using Ceb.MerlinTool.WebAPI.Services.Relay.Enum;
using Ceb.MerlinTool.WebAPI.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Services
{
    public class CategoryAnalysisService : ICategoryAnalysis
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private LookupHelper _dsLookupHelper = new LookupHelper();
        private CategoryAnalysisHelper _dsHelper = new CategoryAnalysisHelper();
        private JSONConverter _jsonUtility = new JSONConverter();

        public Relay.Response<string> GetFilter(int companyId, string filterType, int categoryId, int metricId)
        {
            var response = new Response<string>();
            DataSet dsFilter = new DataSet();
            try
            {
                switch (filterType)
                {
                    case "category":
                        dsFilter = _dsLookupHelper.GetCategory(companyId);
                        break;
                    case "metric":
                        dsFilter = _dsLookupHelper.GetMetric(companyId, categoryId);
                        break;
                    case "kpi":
                        dsFilter = _dsLookupHelper.GetKPI(companyId, categoryId, metricId);
                        break;
                    default: break;
                }

                response.Data = _jsonUtility.GetJSONForFilter(dsFilter);
                response.Status = ServiceStatusCode.Success;
                response.Message = "Retrieved category successfully";
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> GetScore(int companyId, int categoryId, int metric, int kpi)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScoreByCategory = GetScoreBySelectedFilters(companyId, categoryId, metric, kpi);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetScoreByCategory(dsScoreByCategory);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Data = response.Message = ex.Message;
            }
            return response;
        }

        private DataSet GetScoreBySelectedFilters(int companyId, int categoryId, int metric, int kpi)
        {
            if (metric != 0 && kpi != 0)
                return _dsHelper.GetSupplierScoreByCategoryKPIMetric(companyId, categoryId, metric, kpi);
            else if (metric != 0 && kpi == 0)
                return _dsHelper.GetSupplierScoreByCategoryMetric(companyId, categoryId, metric);

            return _dsHelper.GetSupplierScoreByCategory(companyId, categoryId);//default only category
        }
    }
}