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
    public class LookupService : IMerlinLookup
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private LookupHelper _dsHelper = new LookupHelper();
        private JSONConverter _jsonUtility = new JSONConverter();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="filterType"></param>
        /// <param name="categoryId"></param>
        /// <param name="metricId"></param>
        /// <returns></returns>
        public Relay.Response<string> GetFilter(int companyId, string filterType, int categoryId, int metricId)
        {
            var response = new Response<string>();
            DataSet dsFilter = new DataSet();
            filterType = filterType.ToLower();
            try
            {
                switch (filterType)
                {
                    case "category":
                        dsFilter = _dsHelper.GetCategory(companyId);
                        break;
                    case "metric":
                        dsFilter = _dsHelper.GetMetric(companyId, categoryId);
                        break;
                    case "kpi":
                        dsFilter = _dsHelper.GetKPI(companyId, categoryId, metricId);
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


        public Relay.Response<string> GetSupplier(int companyId, int categoryId, int supplierId)
        {
            var response = new Response<string>();
            DataSet dsFilter = new DataSet();
            try
            {
                dsFilter = _dsHelper.GetSupplier(companyId, categoryId, supplierId);

                response.Data = _jsonUtility.GetJSONForFilterSupplier(dsFilter);
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


        public Relay.Response<string> GetDemographic(int companyId, string filterType, int categoryId, int supplierId)
        {
            var response = new Response<string>();
            DataSet dsFilter = new DataSet();
            string demographicId, demographicText;
            Dictionary<string, string> temp;

            try
            {
                temp = MerlinConstants.Demographic[filterType];
                demographicId = temp.Values.First();
                demographicText = temp.Keys.First();

                dsFilter = _dsHelper.GetDemographic(companyId, categoryId, supplierId, demographicId, demographicText);

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
    }
}