using Ceb.MerlinTool.WebAPI.Services.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Interfaces
{
    public interface ITrendAnalysis
    {
        Response<string> GetTimePeriodAvailableForMember(int companyId);
        Response<string> GetOverallScore(int companyId, int categoryId, int supplierId, string timePeriod);
        Response<string> GetMetricScore(int companyId, int categoryId, int supplierId, string timePeriod);
        Response<string> GetKPIScore(int companyId, int categoryId, int supplierId, string timePeriod);
    }
}