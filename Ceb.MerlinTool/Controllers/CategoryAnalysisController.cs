using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ceb.MerlinTool.Controllers
{
    [SessionExpireFilterAttribute]
    public class CategoryAnalysisController : Controller
    {
        // GET: CategoryAnalysis
        public ActionResult Index(int id = 0)
        {
            ViewBag.Page = "Category Analysis";
            ViewBag.CategoryId = id;
            return View();
        }
    }
}