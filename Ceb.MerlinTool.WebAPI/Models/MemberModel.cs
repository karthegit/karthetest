using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Models
{
    public class MemberModel
    {
        public string CEBMemberId { get; set; }
        public int Id { get; set; }
        public string FileName { get; set; }
        public string MemberName { get; set; }
        public string TimePeriod { get; set; }
        public string Industry { get; set; }
        public string Revenue { get; set; }
        public string Region { get; set; }
        public string FilePath { get; set; }
        public string AvailableTimePeriods { get; set; }
        public int NoOfAvailPeriods { get; set; }
        public string SummaryReportName { get; set; }
        public string SummaryFilePath { get; set; }
    }
}