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
    public class TrendAnalysisService : ITrendAnalysis
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private JSONConverter _jsonUtility = new JSONConverter();
        private TrendAnalysisHelper _dsHelper = new TrendAnalysisHelper();

        public Relay.Response<string> GetTimePeriodAvailableForMember(int companyId)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScoreByCategory = _dsHelper.GetAllTimePeriod(companyId);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetJSONForTimePeriod(dsScoreByCategory);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Data = response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> GetOverallScore(int companyId, int categoryId, int supplierId, string timePeriod)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScoreByCategory = _dsHelper.GetOverallScore(companyId, categoryId, supplierId, timePeriod);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetOverallScoreByTimePeriod(dsScoreByCategory);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Data = response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> GetMetricScore(int companyId, int categoryId, int supplierId, string timePeriod)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScoreByCategory = _dsHelper.GetMetricsScore(companyId, categoryId, supplierId, timePeriod);
                response.Status = ServiceStatusCode.Success;
                response.Data =  _jsonUtility.GetSupplierMetricScoreByTimePeriod(dsScoreByCategory);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Data = response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> GetKPIScore(int companyId, int categoryId, int supplierId, string timePeriod)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScoreByCategory = _dsHelper.GetKPIScore(companyId, categoryId, supplierId, timePeriod);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetSupplierKPIScoreByTimePeriod(dsScoreByCategory);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Data = response.Message = ex.Message;
            }
            return response;
        }
    }
}