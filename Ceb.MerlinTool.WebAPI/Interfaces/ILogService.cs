using Ceb.MerlinTool.WebAPI.Services.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Interfaces
{
    public interface ILogService
    {
       Response<string> GetAllLog(DateTime fromDate,DateTime toDate);
    }
}