using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ceb.MerlinTool.Controllers
{
    [SessionExpireFilterAttribute]
    public class SupplierAnalysisController : Controller
    {
        // GET: SupplierAnalysis
        public ActionResult SupplierScorecard(int id = 0)
        {
            ViewBag.Page = "Supplier Scorecard";
            ViewBag.SupplierId = id;
            return View();
        }
        public ActionResult PerceptionGaps()
        {
            ViewBag.Page = "Perception Gaps";
            return View();
        }
        public ActionResult TrendAnalysis()
        {
            ViewBag.Page = "Trend Analysis";
            return View();
        }
    }
}