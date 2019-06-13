using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKIC.SellingPoint.DL.BO;


namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IUser
    {
        BO.AuthenticationResult IsUserValid(string userName, string password);
        string[] GetUserRoles(string userName);
        void TrackLogin(LoginAudit audit);
        PostUserDetailsResult InsertUserMasterDetails(UserMaster details);
        UserDetailsResult FetchUserInformation(string userID);
        PostAdminUserResult PostAdminUser(AdminRegister details);
    }
}
