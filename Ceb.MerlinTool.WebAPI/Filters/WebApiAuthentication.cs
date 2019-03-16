using Ceb.MerlinTool.WebAPI.Relay;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Ceb.MerlinTool.WebAPI.Filters
{
    public class WebApiAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            AjaxResponse response = new AjaxResponse();
            var request = actionContext.Request;
            var headers = request.Headers;

            if (!headers.Contains("X-Requested-With") || headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest")
            {
                response.Data = null;
                response.Status = AjaxResponseStatus.Failure;
                response.Message = "Access has been denied";
                actionContext.Response = request.CreateResponse<AjaxResponse>(HttpStatusCode.Unauthorized, response);
            }
        }
    }
}