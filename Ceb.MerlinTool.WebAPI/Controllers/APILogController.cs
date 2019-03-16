using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.Relay;
using Ceb.MerlinTool.WebAPI.Services.Relay.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ceb.MerlinTool.WebAPI.Controllers
{
     [RoutePrefix("api/v1/APILog")]
    public class APILogController : BaseController
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        private ILogService _logService;

        public APILogController(ILogService logService)
        {
            this._logService = logService;
        }

        [HttpGet]
        [Route("GetLog")]
        public IHttpActionResult GetLog(string fromDate, string toDate)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._logService.GetAllLog(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
                if (serviceResponse.Status == ServiceStatusCode.Success)
                {
                    this.ApiResponse.Message = serviceResponse.Message;
                    this.ApiResponse.Status = AjaxResponseStatus.Success;
                    this.ApiResponse.Data = serviceResponse.Data;
                }
                else
                {
                    this.ApiResponse.Message = serviceResponse.Message;
                    this.ApiResponse.Status = AjaxResponseStatus.Failure;
                }
            }
            catch (Exception ex)
            {
                this.ApiResponse.Message = "An error occured while uploading survey data";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }
    }
}
