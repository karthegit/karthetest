using Ceb.MerlinTool.WebAPI.Models;
using Ceb.MerlinTool.WebAPI.Services.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Interfaces
{
    public interface IAdminConfigurations
    {
        Response<string> UploadSurveyData(int companyId, string memberName, bool isEdit,int updatedBy,string oldFileName);
        Response<string> GetActiveInstitutions();
        Response<string> CheckifCompanySurvey(string companyId);
        Response<string> GetAvailableInstitutions(string page);
        Response<string> DeleteSurveyData(List<MemberModel> members);
        Response<string> UploadReportSummary(int companyId, int updatedBy,string oldSummaryReport);

        #region Member API
        Response<string> GetMembers(bool isImport);
        Response<string> AddMember(MemberModel memberDetails, int createdBy);
        Response<string> DeleteMember(string members);
        Response<string> UpdateMember(MemberModel memberDetails,int createdBy);
        #endregion
    }
}