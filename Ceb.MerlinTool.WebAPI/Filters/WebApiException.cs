using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Ceb.MerlinTool.WebAPI.Filters
{
    public class WebApiException : ExceptionFilterAttribute
    {
        System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        /// <summary>
        /// On Exception
        /// </summary>
        /// <param name="context">Http Action Executed Context</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            //LogHelper.Error(_objType, context.Exception, "Unhandled exception occurred while executing Action");
            context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
    }
}