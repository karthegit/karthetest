using Ceb.Logger;
using Ceb.MerlinTool.DataAccess;
using Ceb.MerlinTool.WebAPI.Constants;
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
    public class SupplierAnalysisService : ISupplierAnalysis
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private LookupHelper _dsLookupHelper = new LookupHelper();
        private SupplierAnalysis _dsHelper = new SupplierAnalysis();
        private JSONConverter _jsonUtility = new JSONConverter();

        public Response<string> GetScore(int companyId, int categoryId, int supplier, int demographic, string demographicOption)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScore = GetScoreBySelectedFilters(companyId, categoryId, supplier, demographic, demographicOption);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetSupplierOverallScore(dsScore);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        private DataSet GetScoreBySelectedFilters(int companyId, int categoryId, int supplier, int demographic, string demographicOption)
        {
            if (demographic == 0)
                return _dsHelper.GetSupplierScore(companyId, categoryId, supplier);
            else
            {
                Dictionary<string, string> dicDemographic = MerlinConstants.DemographicDBColumns[demographicOption];
                return _dsHelper.GetSupplierScoreByCategory(companyId, categoryId, supplier, demographic, dicDemographic.Keys.FirstOrDefault(), dicDemographic.Values.FirstOrDefault());
            }
        }


        public Response<string> GetMetricScore(int companyId, int categoryId, int supplier, int demographic, string demographicOption)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScore = GetMetricScoreBySelectedFilters(companyId, categoryId, supplier, demographic, demographicOption);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetSupplierMetricScore(dsScore);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> GetKPIScore(int companyId, int categoryId, int supplier, int demographic, string demographicOption)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScore = GetKPIScoreBySelectedFilters(companyId, categoryId, supplier, demographic, demographicOption);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetSupplierKPIScore(dsScore);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }


        private DataSet GetKPIScoreBySelectedFilters(int companyId, int categoryId, int supplier, int demographic, string demographicOption)
        {
            if (demographic == 0)
                return _dsHelper.GetSupplierKPIScore(companyId, categoryId, supplier);
            else
            {
                Dictionary<string, string> dicDemographic = MerlinConstants.DemographicDBColumns[demographicOption];
                return _dsHelper.GetSupplierKPIScoreByCategory(companyId, categoryId, supplier, demographic, dicDemographic.Values.FirstOrDefault(), MerlinConstants.DemographicMetricDBColumns[demographicOption].Keys.FirstOrDefault());
            }
        }

        private DataSet GetMetricScoreBySelectedFilters(int companyId, int categoryId, int supplier, int demographic, string demographicOption)
        {
            if (demographic == 0)
                return _dsHelper.GetSupplierMetricScore(companyId, categoryId, supplier);
            else
            {
                Dictionary<string, string> dicDemographic = MerlinConstants.DemographicMetricDBColumns[demographicOption];
                return _dsHelper.GetSupplierMetricScoreByCategory(companyId, categoryId, supplier, demographic, dicDemographic.Keys.FirstOrDefault(), dicDemographic.Values.FirstOrDefault());
            }
        }

        public Response<string> GetSupplierComments(int companyId, int categoryId, int supplier, int demographic, string demographicOption)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScore = GetCommentsBySelectedFilters(companyId, categoryId, supplier, demographic, demographicOption);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetSupplierComments(dsScore);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        private DataSet GetCommentsBySelectedFilters(int companyId, int categoryId, int supplier, int demographic, string demographicOption)
        {
            if (demographic == 0)
                return _dsHelper.GetSupplierComments(companyId, categoryId, supplier);
            else
            {
                Dictionary<string, string> dicDemographic = MerlinConstants.DemographicDBColumns[demographicOption];
                return _dsHelper.GetSupplierComments(companyId, categoryId, supplier, demographic, dicDemographic.Values.FirstOrDefault(), MerlinConstants.DemographicMetricDBColumns[demographicOption].Keys.FirstOrDefault());
            }
        }
    }
}