using Ceb.Logger;
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
    [RoutePrefix("api/v1/APIPerceptionGaps")]
    public class APIPerceptionGapsController : BaseController
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        private IPerceptionGaps _perceptionGapsService;

        public APIPerceptionGapsController(IPerceptionGaps perceptionGapsService)
        {
            this._perceptionGapsService = perceptionGapsService;
        }

        [HttpGet]
        [Route("CheckifDataUploaded")]
        public IHttpActionResult CheckifDataUploaded(int companyId)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._perceptionGapsService.CheckIfPerceptionGapDataExists(companyId);
                if (serviceResponse.Status == ServiceStatusCode.Success)
                {                 
                    this.ApiResponse.Status = AjaxResponseStatus.Success;
                }
                else
                {
                    this.ApiResponse.Status = AjaxResponseStatus.Failure;
                }
            }
            catch (Exception ex)
            {

                LogHelper.Error(_objType, ex, "An error occured while retrieving supplier score");
                this.ApiResponse.Message = "An error occured while retrieving supplier score";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        [HttpGet]
        [Route("GetSupplierScore")]
        public IHttpActionResult GetScore(int companyId, int category, int supplier)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._perceptionGapsService.GetScore(companyId, category, supplier);
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

                LogHelper.Error(_objType, ex, "An error occured while retrieving supplier score");
                this.ApiResponse.Message = "An error occured while retrieving supplier score";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        [HttpGet]
        [Route("GetSupplierMetricScore")]
        public IHttpActionResult GetSupplierMetricScore(int companyId, int category, int supplier)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._perceptionGapsService.GetMetricScore(companyId, category, supplier);
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

                LogHelper.Error(_objType, ex, "An error occured while retrieving supplier score");
                this.ApiResponse.Message = "An error occured while retrieving supplier score";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        [HttpGet]
        [Route("GetSupplierKPIScore")]
        public IHttpActionResult GetSupplierKPIScore(int companyId, int category, int supplier)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._perceptionGapsService.GetKPIScore(companyId, category, supplier);
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

                LogHelper.Error(_objType, ex, "An error occured while retrieving supplier KPI score");
                this.ApiResponse.Message = "An error occured while retrieving supplier KPI score";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        [HttpGet]
        [Route("GetSupplierComments")]
        public IHttpActionResult GetSupplierComments(int companyId, int category, int supplier)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._perceptionGapsService.GetSupplierComments(companyId, category, supplier);
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

                LogHelper.Error(_objType, ex, "An error occured while retrieving supplier KPI score");
                this.ApiResponse.Message = "An error occured while retrieving supplier KPI score";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }
    }
}
