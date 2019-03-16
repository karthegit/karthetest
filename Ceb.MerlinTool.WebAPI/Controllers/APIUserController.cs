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
    [RoutePrefix("api/v1/APIUser")]
    public class APIUserController : BaseController
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        private IUser _userService;

        public APIUserController(IUser userService)
        {
            this._userService = userService;
        }

        [HttpGet]
        [Route("IsAuthorizedUser")]
        public IHttpActionResult IsAuthorizedUser(string email)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._userService.CheckIfUserExists(email);
                if (serviceResponse.Status == ServiceStatusCode.Success)
                {
                    this.ApiResponse.Data = serviceResponse.Data;
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

                LogHelper.Error(_objType, ex, ex.Message);
                this.ApiResponse.Message = ex.Message;
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers()
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._userService.GetAllUsers();
                if (serviceResponse.Status == ServiceStatusCode.Success)
                {
                    this.ApiResponse.Message = serviceResponse.Message;
                    this.ApiResponse.Data = serviceResponse.Data;
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

                LogHelper.Error(_objType, ex, ex.Message);
                this.ApiResponse.Message = "An error occured while retrieving all users";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }


        [HttpPost]
        [Route("AddUser")]
        public IHttpActionResult AddUser(JObject userDetails)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._userService.AddUser(Convert.ToString(userDetails["email"]), Convert.ToInt32(userDetails["companyId"]), Convert.ToInt32(userDetails["userCreatedBy"]), Convert.ToString(userDetails["CEBId"]), Convert.ToBoolean(userDetails["isAdmin"]));
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
                LogHelper.Error(_objType, ex, ex.Message);
                this.ApiResponse.Message = ex.Message;
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }


        [HttpPost]
        [Route("Update")]
        public IHttpActionResult PostUser(JObject userDetails)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._userService.UpdateUser(Convert.ToString(userDetails["email"]), Convert.ToBoolean(userDetails["isAdmin"]), Convert.ToInt16(userDetails["updatedBy"]));
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
                LogHelper.Error(_objType, ex, ex.Message);
                this.ApiResponse.Message = ex.Message;
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult Delete(string users)
        {
            AjaxResponse response = new AjaxResponse();
            if (this.ApiResponse == null) this.ApiResponse = new AjaxResponse();
            try
            {
                var serviceResponse = this._userService.DeleteUser(users);
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
                this.ApiResponse.Message = "An error occured while deleting clients";
                this.ApiResponse.Status = AjaxResponseStatus.Failure;
            }
            return Ok<AjaxResponse>(this.ApiResponse);
        }

    }
}
