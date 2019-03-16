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
    public class PerceptionGapsService : IPerceptionGaps
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private PerceptionGapsHelper _dsHelper = new PerceptionGapsHelper();
        private JSONConverter _jsonUtility = new JSONConverter();

        //need to be removed in next release
        public Response<string> CheckIfPerceptionGapDataExists(int companyId)
        {
            var response = new Response<string>();

            try
            {
                int dsScore = _dsHelper.CheckIfPerceptionGapDataExists(companyId);
                if(dsScore==1)
                    response.Status = ServiceStatusCode.Success;
                else
                    response.Status = ServiceStatusCode.Failure;
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> GetScore(int companyId, int categoryId, int supplier)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScore = _dsHelper.GetSupplierScore(companyId, categoryId, supplier);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetSupplierOverallScorePG(dsScore);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }
        
        public Response<string> GetMetricScore(int companyId, int categoryId, int supplier)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScore = _dsHelper.GetSupplierMetricScore(companyId, categoryId, supplier);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetSupplierMetricScorePG(dsScore);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> GetKPIScore(int companyId, int categoryId, int supplier)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScore = _dsHelper.GetSupplierKPIScore(companyId, categoryId, supplier);
                response.Status = ServiceStatusCode.Success;
                response.Data = _jsonUtility.GetSupplierKPIScorePG(dsScore);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> GetSupplierComments(int companyId, int categoryId, int supplier)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsScore = _dsHelper.GetSupplierComments(companyId, categoryId, supplier);
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
    }
}