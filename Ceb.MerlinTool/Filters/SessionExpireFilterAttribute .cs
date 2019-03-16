 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ceb.MerlinTool
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            if (filterContext.RouteData.GetRequiredString("controller").Equals("Account", StringComparison.CurrentCultureIgnoreCase)
                && filterContext.RouteData.GetRequiredString("action").Equals("Login", StringComparison.CurrentCultureIgnoreCase))
            {
                if (HttpContext.Current.Session["UserName"] != null)
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Abandon();
                }
                return;
            }

            if (filterContext.RouteData.GetRequiredString("controller").Equals("Account", StringComparison.CurrentCultureIgnoreCase) && filterContext.RouteData.GetRequiredString("action").Equals("Login", StringComparison.CurrentCultureIgnoreCase)
            || filterContext.RouteData.GetRequiredString("action").Equals("UserInfo", StringComparison.OrdinalIgnoreCase)
            || filterContext.RouteData.GetRequiredString("controller").Equals("SBWSAuth", StringComparison.CurrentCultureIgnoreCase) && filterContext.RouteData.GetRequiredString("action").Equals("Login", StringComparison.CurrentCultureIgnoreCase))
            {
                HttpContext.Current.Response.BufferOutput = true;
                return;
            }

            HttpContext ctx = HttpContext.Current;

            // check if session is supported
            if (ctx.Session != null)
            {
                // check if a new session id was generated
                if (ctx.Session.IsNewSession || (!ctx.Session.IsNewSession && ctx.Session["UserName"] == null))
                {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if (ctx.Session["UserName"] == null || (null == sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") < 0 && sessionCookie == "reset"))
                    {
                        ctx.Session.Clear();
                        ctx.Session.Abandon();
                        HttpContext.Current.Response.BufferOutput = true;
                        //ctx.Response.Redirect("~/Home/AccountSelection", true);
                        filterContext.Result = new RedirectResult("~/Account/Login");
                    }
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}