using Ceb.Logger;
using Ceb.MerlinTool.SalesforceServiceReference;
using CEB.CustomControls;
using CEB.CustomControls.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ceb.MerlinTool.Controllers
{
    public class AccountController : Controller
    {
        private System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        public async Task<ActionResult> Login(string d)
        {
            ViewBag.Page = "Login";

            LoginDetails loginDetails = new LoginDetails();
            if (!string.IsNullOrWhiteSpace(d))
            {
                try
                {
                    loginDetails = AuthHelper.DecryptLoginDetails(d);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(_objType, ex, ex.Message);
                    Session.Abandon();
                    Session.Clear();
                    AuthHelper.Logout(false);
                    return View("UserInfo");
                }
            }
            else
            {
                if (AuthHelper.AuthenticationResponse == null)
                {
                    AuthSSOHelper.AuthenticateForSSO();
                }

                if (AuthHelper.AuthenticationResponse != null && AuthHelper.AuthenticationResponse.IsAuthenticated)
                {
                    loginDetails = new LoginDetails
                    {
                        Username = AuthHelper.AuthenticationResponse.Username
                    };
                }
            }
            if (!string.IsNullOrWhiteSpace(loginDetails.Username))
            {
                LogHelper.Info(_objType, "SBWS Authorized User : " + loginDetails.Username);
                Dictionary<string, object> userDetails = await isAuthorizedUser(loginDetails.Username);
                if (userDetails != null && Convert.ToInt16(userDetails["Status"]) == 1)
                {
                    LogHelper.Info(_objType, "Authenticated with DB and Salesforce");
                    Dictionary<string, string> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(Convert.ToString(userDetails["Data"]))[0];

                    Session["Email"] = loginDetails.Username;
                    Session["UserName"] = data["Name"];
                    Session["Company"] = data["MemberName"];
                    Session["CEBId"] = data["CEBId"];
                    Session["isAdmin"] = data["IsAdmin"];
                    Session["UserId"] = data["Id"];
                    Session["CompanyId"] = data["CompanyId"];
                    Session["SummaryReportPath"] = data["SummaryReportPath"];
                    Session["SummaryReportName"] = data["SummaryReportName"];
                    return RedirectToAction("About");
                }
                else
                {
                    LogHelper.Info(_objType, "Failed authentication with DB and Salesforce");
                    Session.Abandon();
                    Session.Clear();
                    AuthHelper.Logout(false);
                    return View("UserInfo");
                }
            }

            return View();
        }

        public ActionResult UserInfo()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Page = "About";
            if (Session["UserName"] == null)
                return RedirectToAction("Login");
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            AuthHelper.Logout(false);
            return RedirectToAction("Login");
        }

        public ActionResult Log()
        {
            if (Session["Email"] != null)
                if (ConfigurationManager.AppSettings["ViewLog"].Contains(Convert.ToString(Session["Email"])))
                    return View();
            return RedirectToAction("UserInfo");
        }

        private async Task<Dictionary<string, object>> isAuthorizedUser(string email)
        {
            try
            {
                string URL = ConfigurationManager.AppSettings["APIURL"] + "api/v1/APIUser/IsAuthorizedUser?email=" + email;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                // List data response.
                HttpResponseMessage response = client.GetAsync(URL).Result;  // Blocking call!
                if (response.IsSuccessStatusCode)
                {
                    Dictionary<string, object> result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());
                    return result;
                }
            }
            catch (TaskCanceledException ex)
            {
            }
            return null;
        }

        [HttpGet]
        public FilePathResult Download(string path, string type, string downloadName, bool isDataFile = false)
        {
            try
            {
                Response.BufferOutput = true;
                string format = string.Empty, filePath = Path.Combine(ConfigurationManager.AppSettings["ExportFolderPath"], path); ;
                switch (type)
                {
                    case "pptx":
                        format = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        downloadName = downloadName + "." + path.Split('.')[1];
                        break;
                    case "excel":
                        format = "application/vnd.ms-excel";
                        downloadName = downloadName + ".xlsx";
                        break;
                    case "pdf":
                        format = "application/pdf";
                        downloadName = downloadName + ".pdf";
                        break;
                    case "png":
                        format = "image/png";
                        downloadName = downloadName + ".png";
                        break;
                    case "word":
                        format = "application/octet-stream";
                        downloadName = downloadName + ".docx";
                        break;
                    default:
                        return null;
                }
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.BufferOutput = true;

                string contentDisposition;

                if (Request.Browser.Browser == "IE" && (Request.Browser.Version == "7.0" || Request.Browser.Version == "8.0"))
                    contentDisposition = "attachment; filename=" + Uri.EscapeDataString(downloadName);
                else if (Request.Browser.Browser == "Safari")
                    contentDisposition = "attachment; filename=" + downloadName;
                else
                    contentDisposition = "attachment; filename*=UTF-8''" + Uri.EscapeDataString(downloadName);
                Response.AddHeader("Content-Disposition", contentDisposition);
                //Response.AddHeader("Content-Disposition", "attachment; filename*=UTF-8''" + Uri.EscapeDataString(downloadName));

                //Response.AddHeader("content-disposition", "attachment; filename=" + temp);
                this.Response.ContentType = format;

                //Response.AddHeader("content-disposition", "attachment; filename=" + downloadName);
                //this.Response.ContentType = format;
                return File(filePath, format);
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                return null;
            }
        }
    }
}