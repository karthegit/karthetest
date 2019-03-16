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
    [RoutePrefix("api/v1/APISupplierAnalysis")]
    public class APISupplierAnalysisController : BaseController
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        private ISupplierAnalysis _supplierAnalysisService;

        public APISupplierAnalysisController(ISupplierAnalysis supplierAnalysisService)
        {
            this._supplierAnalysisService = supplierAnalysisService;
        }

        [HttpGet]
        [Route("GetSupplierScore")]
        public IHttpActionResult GetScore(int companyId, int category, int supplier,string demographicOption, int demographic = 0)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._supplierAnalysisService.GetScore(companyId, category, supplier, demographic,demographicOption);
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
        public IHttpActionResult GetSupplierMetricScore(int companyId, int category, int supplier, string demographicOption, int demographic = 0)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._supplierAnalysisService.GetMetricScore(companyId, category, supplier, demographic, demographicOption);
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
        public IHttpActionResult GetSupplierKPIScore(int companyId, int category, int supplier, string demographicOption, int demographic = 0)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._supplierAnalysisService.GetKPIScore(companyId, category, supplier, demographic, demographicOption);
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
        public IHttpActionResult GetSupplierComments(int companyId, int category, int supplier, string demographicOption, int demographic = 0)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._supplierAnalysisService.GetSupplierComments(companyId, category, supplier, demographic, demographicOption);
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
