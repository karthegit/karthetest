using Ceb.Logger;
using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.Relay;
using Ceb.MerlinTool.WebAPI.Services.Relay.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ceb.MerlinTool.WebAPI.Controllers
{
    [RoutePrefix("api/v1/APIOverAll")]
    public class APIOverAllController : BaseController
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        private IOverAllScore _overallService;

        public APIOverAllController(IOverAllScore overallService)
        {
            this._overallService = overallService;
        }

        [HttpGet]
        [Route("GetOverAllScore")]
        public IHttpActionResult GetOverAllScore(int companyId)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._overallService.GetOverAllScore(companyId);
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

                LogHelper.Error(_objType, ex, "An error occured while retrieving over all score");
                this.ApiResponse.Message = "An error occured while getting over all score";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        [Route("ExportChart")]
        [HttpPost]
        public IHttpActionResult ExportChart(JObject requestParams)
        {

            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._overallService.ExportHighchart(Convert.ToString(requestParams["chartJson"]), Convert.ToString(requestParams["title"])
                    , Convert.ToString(requestParams["subTitle"]), Convert.ToString(requestParams["exportType"]), Convert.ToString(requestParams["companyDetails"][0]["CompanyName"])
                    , Convert.ToString(requestParams["companyDetails"][0]["TimePeriod"]), Convert.ToString(requestParams["page"]));
                if (serviceResponse.Status == ServiceStatusCode.Success)
                {
                    if (serviceResponse.Data != null)
                    {
                        this.ApiResponse.Data = System.IO.Path.GetFileName(serviceResponse.Data);
                        this.ApiResponse.Status = AjaxResponseStatus.Success;
                        this.ApiResponse.Message = "Image fetched successfully";
                    }
                    else
                    {
                        this.ApiResponse.Message = serviceResponse.Message;
                        this.ApiResponse.Status = AjaxResponseStatus.Failure;
                    }
                }
                else
                {
                    this.ApiResponse.Message = serviceResponse.Message;
                    this.ApiResponse.Status = AjaxResponseStatus.Failure;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, "An error occured while generating image for User");
                this.ApiResponse.Message = "An error occured while exporting";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        [Route("ExportTable")]
        [HttpPost]
        public IHttpActionResult ExportToExcel(JObject requestParams)
        {

            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._overallService.ExportToExcel(Convert.ToString(requestParams["chartJson"]), Convert.ToString(requestParams["title"])
                    , Convert.ToString(requestParams["subTitle"]), Convert.ToString(requestParams["companyDetails"][0]["CompanyName"])
                    , Convert.ToString(requestParams["companyDetails"][0]["TimePeriod"]), Convert.ToString(requestParams["page"]), Convert.ToString(requestParams["columnTitles"]));
                if (serviceResponse.Status == ServiceStatusCode.Success)
                {
                    this.ApiResponse.Data = System.IO.Path.GetFileName(serviceResponse.Data);
                    this.ApiResponse.Status = AjaxResponseStatus.Success;
                    this.ApiResponse.Message = "Exported successfully";
                }
                else
                {
                    this.ApiResponse.Message = serviceResponse.Message;
                    this.ApiResponse.Status = AjaxResponseStatus.Failure;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, "An error occured while generating image for User");
                this.ApiResponse.Message = "An error occured while generating data";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        //[Route("ExportPDF")]
        //[HttpPost]
        //public IHttpActionResult ExportTable(string title, string subTitle, string exportType, string company, string timePeriod, string page
        //    , byte[] buffer)
        //{

        //    if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
        //    try
        //    {
        //        var serviceResponse = this._overallService.ExportPDF(title, subTitle, exportType, company, timePeriod, page, buffer);
        //        if (serviceResponse.Status == ServiceStatusCode.Success)
        //        {
        //            this.ApiResponse.Data = System.IO.Path.GetFileName(serviceResponse.Data);
        //            this.ApiResponse.Status = AjaxResponseStatus.Success;
        //            this.ApiResponse.Message = "Exported successfully";
        //        }
        //        else
        //        {
        //            this.ApiResponse.Message = serviceResponse.Message;
        //            this.ApiResponse.Status = AjaxResponseStatus.Failure;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(_objType, ex, "An error occured while generating image for User");
        //        this.ApiResponse.Message = "An error occured while generating data";
        //        this.ApiResponse.Status = AjaxResponseStatus.Failure;
        //    }
        //    return Ok<AjaxResponse>(this.ApiResponse);
        //}

        //[HttpPost]
        //public IHttpActionResult Save(string contentType, string base64, string fileName)
        //{
        //    byte[] fileContents = Convert.FromBase64String(base64);

        //    var serviceResponse = this._overallService.ExportPDF("", "", "", "", "", "", fileContents);

        //    if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();

        //    return Ok<AjaxResponse>(this.ApiResponse);

        //    //return File(fileContents, contentType, fileName);
        //}

    }
}
