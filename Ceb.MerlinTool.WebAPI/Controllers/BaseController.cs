using Ceb.MerlinTool.WebAPI.Filters;
using Ceb.MerlinTool.WebAPI.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ceb.MerlinTool.WebAPI.Controllers
{
    [WebApiAuthentication]
    [WebApiException]
    public class BaseController : ApiController
    {
        public AjaxResponse ApiResponse { get; set; }
    }
}
