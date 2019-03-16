using Ceb.MerlinTool.DataAccess;
using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.Services.Relay;
using Ceb.MerlinTool.WebAPI.Services.Relay.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Services
{
    public class LogService : ILogService
    {
        private DataAccessHelper _dsHelper = new DataAccessHelper();

        public Relay.Response<string> GetAllLog(DateTime fromDate,DateTime toDate)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsOverall = _dsHelper.GetLog(fromDate, toDate);
                response.Status = ServiceStatusCode.Success;
                response.Data = Newtonsoft.Json.JsonConvert.SerializeObject(dsOverall.Tables[0]);
            }
            catch (Exception ex)
            {
                response.Status = ServiceStatusCode.Failure;
                response.Data = response.Message = ex.Message;
            }
            return response;
        }
    }
}