using Ceb.MerlinTool.WebAPI.Services.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Interfaces
{
    public interface IPerceptionGaps
    {
        Response<string> CheckIfPerceptionGapDataExists(int companyId);
        Response<string> GetScore(int companyId, int categoryId, int supplier);
        Response<string> GetMetricScore(int companyId, int categoryId, int supplier);
        Response<string> GetKPIScore(int companyId, int categoryId, int supplier);
        Response<string> GetSupplierComments(int companyId, int categoryId, int supplier);
    }
}