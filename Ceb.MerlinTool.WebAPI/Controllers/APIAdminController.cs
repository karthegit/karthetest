using Ceb.Logger;
using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.Models;
using Ceb.MerlinTool.WebAPI.Relay;
using Ceb.MerlinTool.WebAPI.Services.Relay.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Ceb.MerlinTool.WebAPI.Controllers
{
    [RoutePrefix("api/v1/APIAdmin")]
    public class APIAdminController : BaseController
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        private IAdminConfigurations _adminService;

        public APIAdminController(IAdminConfigurations adminService)
        {
            this._adminService = adminService;
        }

        /// <summary>
        /// Upload survey data for Client
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="memberName"></param>
        /// <param name="updatedBy"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadSurveyData")]
        public IHttpActionResult PostResponse(int companyId, string memberName,int updatedBy,string oldFileName, bool isEdit = false)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._adminService.UploadSurveyData(companyId, memberName, isEdit,updatedBy, oldFileName);
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

                LogHelper.Error(_objType, ex, "An error occured while uploading survey data for the client '" + memberName + "'");
                this.ApiResponse.Message = "An error occured while uploading survey data";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        /// <summary>
        /// Get Institution list from Salesforce
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInstitutions")]
        public IHttpActionResult GetInstitutions()
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._adminService.GetActiveInstitutions();
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

                LogHelper.Error(_objType, ex, ex.Message);
                this.ApiResponse.Message = "An error occured while retrieving institutions from salesforce";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        /// <summary>
        /// Get Institutions/Members added within the tool
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAvailableInstitutions")]
        public IHttpActionResult GetAvailableInstitutions(string page = "")
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._adminService.GetAvailableInstitutions(page);
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

                LogHelper.Error(_objType, ex, ex.Message);
                this.ApiResponse.Message = "An error occured while retrieving institutions from salesforce";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        /// <summary>
        /// Check whether survey data exists for the specific client or not
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckifCompanySurvey")]
        public IHttpActionResult CheckifCompanySurvey(string companyId)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._adminService.CheckifCompanySurvey(companyId);
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

                //LogHelper.Error(_objType, ex, "An error occured while uploading benchmark data");
                this.ApiResponse.Message = "An error occured";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteSurveyData")]
        public IHttpActionResult Delete(List<MemberModel> members)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._adminService.DeleteSurveyData(members);
                if (serviceResponse.Status == ServiceStatusCode.Success)
                {
                    this.ApiResponse.Message = serviceResponse.Message;
                    this.ApiResponse.Status = AjaxResponseStatus.Success;
                }
                else
                {
                    this.ApiResponse.Message = serviceResponse.Message;
                    this.ApiResponse.Status = AjaxResponseStatus.Failure;
                }
            }
            catch (Exception ex)
            {

                LogHelper.Error(_objType, ex, "An error occured while deleting clients");
                this.ApiResponse.Message = "An error occured while uploading survey data";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Download(string fileName)
        {
            string path = Ceb.MerlinTool.WebAPI.Constants.MerlinConstants.SurveyDataPath;
            if (!path.EndsWith("\\"))
                path += "\\";
            if (path != null)
                return FileAsAttachment(path, fileName);
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static HttpResponseMessage FileAsAttachment(string path, string filename)
        {
            if (File.Exists(path))
            {

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(path, FileMode.Open);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = filename;
                return result;
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("UploadSummaryReport")]
        public IHttpActionResult UploadSummaryReport(int companyId, int updatedBy, string oldSummaryReport)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._adminService.UploadReportSummary(companyId, updatedBy, oldSummaryReport);
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

                LogHelper.Error(_objType, ex, "An error occured while uploading report summary for the client");
                this.ApiResponse.Message = "An error occured while uploading report summary";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }
    }
}
