using Ceb.MerlinTool.WebAPI.Services.Relay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Interfaces
{
    public interface IUser
    {
        Response<string> CheckIfUserExists(string email);
        Response<string> GetAllUsers();
        Response<string> AddUser(string email, int companyId, int userId, string cebId,bool isAdmin);
        Response<string> DeleteUser(string users);
        Response<string> UpdateUser(string email, bool isAdmin,int updatedby);
    }
}