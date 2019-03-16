using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Relay
{
    public class AjaxResponse
    {
        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Status 
        /// </summary>
        public AjaxResponseStatus Status { get; set; }
        /// <summary>
        /// Web api response data
        /// </summary>
        public object Data { get; set; }
    }

    public enum AjaxResponseStatus
    {
        Unknown,
        Success,
        Failure,
        ErrorWithMessage,
        Duplicate,
        Exception,
        TokenExpired,
        Authenticated,
        UnAuthenticated
    }
}