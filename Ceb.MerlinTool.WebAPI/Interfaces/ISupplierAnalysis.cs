using Ceb.MerlinTool.WebAPI.Services.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Interfaces
{
    public interface ISupplierAnalysis
    {
        Response<string> GetScore(int companyId, int categoryId, int supplier, int demographic, string demographicOption);
        Response<string> GetMetricScore(int companyId, int categoryId, int supplier, int demographic, string demographicOption);
        Response<string> GetKPIScore(int companyId, int categoryId, int supplier, int demographic, string demographicOption);
        Response<string> GetSupplierComments(int companyId, int categoryId, int supplier, int demographic, string demographicOption);
    }
}