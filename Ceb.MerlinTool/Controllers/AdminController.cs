using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ceb.MerlinTool.Controllers
{
    [SessionExpireFilterAttribute]
    public class AdminController : Controller
    {
        // GET: Account
        public ActionResult ImportFile()
        {
            ViewBag.Page = "Import File";
            return View();
        }

        public ActionResult MemberProfile()
        {
            ViewBag.Page = "Client Profile";
            return View();
        }

        [HttpGet]
        public ActionResult Download(string folderPath, string fileName)
        {
            try
            {
                Response.BufferOutput = true;
                string format = "application/vnd.ms-excel";
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.BufferOutput = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                this.Response.ContentType = format;
                if(System.IO.File.Exists(folderPath))
                    return File(folderPath, format);

                return RedirectToAction("Logout", "Account");
            }
            catch (Exception ex)
            {
                //LogHelper.Error(_objType, ex, ex.Message);
                return RedirectToAction("Logout", "Account");
            }
        }
        public ActionResult AccessDetails()
        {
            ViewBag.Page = "Access Details";
            return View();
        }
        
    }
}