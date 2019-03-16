using Ceb.MerlinTool.WebAPI.Services.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Interfaces
{
    public interface IMerlinLookup
    {
        Response<string> GetFilter(int companyId, string filterType, int categoryId, int metricId);
        Response<string> GetDemographic(int companyId, string filterType, int categoryId, int supplierId);
        Response<string> GetSupplier(int companyId,  int categoryId, int supplierId);
    }
}