using Ceb.MerlinTool.WebAPI.Services.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Interfaces
{
    public interface IOverAllScore
    {
        Response<string> GetOverAllScore(int companyId);
        Response<string> ExportHighchart(string chartJson, string title, string subTitle, string exportType, string company, string timePeriod,string page);
        Response<string> ExportToExcel(string tableJSON, string title, string subTitle,string company, string timePeriod,string page, string columnTitles="");
        //Response<string> ExportPDF(string title, string subTitle, string exportType, string company, string timePeriod, string page
        //   , byte[] buffer);
    }
}