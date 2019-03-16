using Ceb.Logger;
using Ceb.MerlinTool.DataAccess;
using Ceb.MerlinTool.WebAPI.Interfaces;
using Ceb.MerlinTool.WebAPI.SalesforceServiceReference;
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
    public class UserService : IUser
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private DataAccessHelper _dsHelper = new DataAccessHelper();
        private JSONConverter _jsonUtility = new JSONConverter();

        public Response<string> CheckIfUserExists(string email)
        {
            var response = new Response<string>();

            try
            {
                DataSet dsUser = _dsHelper.CheckifUserExists(email);
                if (dsUser != null && dsUser.Tables != null && dsUser.Tables[0].Rows.Count > 0)
                {
                    response.Data = _jsonUtility.GetUsers(dsUser);
                    response.Message = "Authentication success with DB";
                    response.Status = ServiceStatusCode.Success;
                }
                else
                {
                    response.Message = "Failed authentication with DB";
                    response.Status = ServiceStatusCode.Failure;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.ErrorMessage = ex.Message;
                response.Status = ServiceStatusCode.Failure;
            }
            return response;
        }

        public Response<string> GetAllUsers()
        {
            var response = new Response<string>();

            try
            {
                DataSet users = _dsHelper.GetAllUsers();
                if (users == null || users.Tables == null || users.Tables[0].Rows.Count == 0)
                {
                    response.Message = "No Users found";
                    response.Status = ServiceStatusCode.Failure;
                }
                else
                {
                    response.Data = _jsonUtility.GetUsers(users);
                    response.Status = ServiceStatusCode.Success;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.ErrorMessage = ex.Message;
                response.Status = ServiceStatusCode.Failure;
            }
            return response;
        }

        public Response<string> AddUser(string email, int companyId, int userId, string cebId,bool isAdmin)
        {
            var response = new Response<string>();
            try
            {
                User user = null;
                if (IsValidUser(email.Trim(), out user))
                {
                    if (user.Institution.CEBId.Equals(cebId))
                    {
                        int id = _dsHelper.AddUserDetails(companyId, userId, user.Email, user.Name,isAdmin);
                        if (id == -1)
                        {
                            response.Message = string.Format("User '{0}' already exists", email);
                            response.Status = ServiceStatusCode.Failure;
                            return response;
                        }
                        else
                        {
                            response.Message = string.Format("User '{0}' added successfully", email);
                            response.Status = ServiceStatusCode.Success;
                            return response;
                        }
                    }
                    else
                    {
                        response.Message = string.Format("User '{0}' doesn't belong to the selected institution", email);
                        response.Status = ServiceStatusCode.Failure;
                        return response;
                    }
                }
                else
                {
                    response.Data = response.Message = string.Format("User '{0}' is not an authorized user", email);
                    response.Status = ServiceStatusCode.Failure;
                    return response;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> DeleteUser(string users)
        {
            var response = new Response<string>();
            try
            {
                _dsHelper.DeleteUser(users);
                response.Status = ServiceStatusCode.Success;
                response.Message = "Survey for selected clients have been deleted successfully";
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        public Response<string> UpdateUser(string email, bool isAdmin,int updatedby)
        {
            var response = new Response<string>();
            try
            {
                _dsHelper.UpdateIsAdmin(email, isAdmin,updatedby);
                response.Status = ServiceStatusCode.Success;
                response.Message = "Survey for selected clients have been updated successfully";
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                response.Status = ServiceStatusCode.Failure;
                response.Message = ex.Message;
            }
            return response;
        }

        private bool IsValidUser(string userName, out SalesforceServiceReference.User userObj)
        {
            userObj = null;
            bool isAuthorized = true;

            SalesforceWebServiceClient client = new SalesforceWebServiceClient();
            userObj = client.GetUserByEmail(userName);

            if (userObj == null) return false;

            if (userObj.Institution == null) return false;

            return isAuthorized;
        }
    }
}