using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace BKIC.SellingPoint.DL.BL
{
    public class User : IUser
    {
        public User()
        {
            //_insuredMaster = new DBIntegration.Implementations.InsuranceMaster();
            //_mail = new Implementation.Mail();
        }

        public string[] GetUserRoles(string userName)
        {
            return Roles.GetRolesForUser(userName);
        }

        /// <summary>
        /// Check if the user is valid or not.
        /// </summary>
        /// <param name="userName">LoggedIn username.</param>
        /// <param name="password">LoggedIn password.</param>
        /// <returns>User is valid or not.</returns>
        public AuthenticationResult IsUserValid(string userName, string password)
        {
            var userValidation = new AuthenticationResult();
            userValidation.IsValidUser = false;
            userValidation.IsTransactionDone = true;

            try
            {
                if (Membership.ValidateUser(userName, password))
                {
                    userValidation.IsValidUser = true;
                    //   var userDetails = Membership.GetUser(authentication.UserName);
                    //userValidation.UserEmail = userDetails.Email;
                    //userValidation.MembershipUserName = userDetails.UserName;
                }
            }
            catch (Exception exc)
            {
                userValidation.IsTransactionDone = false;
                userValidation.TransactionErrorMessage =
                    exc.InnerException != null ?
                   (exc.InnerException.InnerException != null ? exc.InnerException.InnerException.Message :
                   exc.InnerException.Message) : exc.Message;
            }

            return userValidation;
        }

        public void TrackLogin(LoginAudit audit)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@UserName", audit.UserName),
                    new SqlParameter("@IPAddress", audit.IPAddress),
                    new SqlParameter("@LoginDate", audit.LoginDate),
                    new SqlParameter("@LoginStatus", audit.LoginStatus)
                };

                DataSet result = BKICSQL.eds(UsersSP.LoginAudit, para);
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        /// <summary>
        /// Insert the new user to the database.
        /// </summary>
        /// <param name="details">User details.</param>
        /// <returns>User inserted or not.</returns>
        public PostUserDetailsResult InsertUserMasterDetails(UserMaster details)
        {
            try
            {
                bool isExist = false;

                Match password = Regex.Match(details.Password, Constants.RegularExpressions.PasswordStrength);
                if (!password.Success)
                {
                    return new PostUserDetailsResult { PasswordStrength = true };
                }
                if (details.Type != "edit")
                {
                    SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@UserName",details.UserName)
                };
                    List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsUserNameExists" },
                };
                    object[] dataSet = BKICSQL.GetValues(UsersSP.UserNamePrecheck, param, outParams);
                    isExist = dataSet[0].ToString() == "True" ? true : false;
                }

                if (!isExist)
                {
                    SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Id",details.Id),
                    new SqlParameter("@Agency",string.IsNullOrEmpty(details.Agency)?"":details.Agency),
                    new SqlParameter("@AgentCode", details.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch", details.AgentBranch??string.Empty),
                     new SqlParameter("@UserID", details.UserID??string.Empty),
                    new SqlParameter("@UserName", details.UserName??string.Empty),
                    new SqlParameter("@CreatedDate", details.CreatedDate),
                    // new SqlParameter("@Password", details.Password),
                    //new SqlParameter("@PasswordExpiryDate", details.PasswordExpiryDate),
                    new SqlParameter("@Mobile", details.Mobile??string.Empty),
                    new SqlParameter("@Email", details.Email??string.Empty),
                    new SqlParameter("@IsActive", true),
                    new SqlParameter("@StaffNo", details.StaffNo),
                    new SqlParameter("@Role", details.Role??string.Empty),
                    new SqlParameter("@CreatedBy", details.CreatedBy),
                    new SqlParameter("@Type", details.Type)
                };

                    if (details.Type == "insert")
                    {
                        Membership.CreateUser(details.UserName, details.Password, details.Email);

                        //Roles.AddUserToRole(details.UserName, Constants.Roles.User);
                        Roles.AddUserToRole(details.UserName, details.Role);
                    }
                    else if (details.Type == "edit")
                    {
                        var memUser = Membership.GetUser(details.UserName);
                        memUser.Email = details.Email;
                        memUser.ChangePassword(memUser.ResetPassword(), details.Password);
                        //memUser.ResetPassword(details.Password);
                        Membership.UpdateUser(memUser);
                    }
                    BKICSQL.enq(UsersSP.PostUserMaster, para);
                    return new PostUserDetailsResult
                    {
                        IsTransactionDone = true
                    };
                }
                else
                {
                    return new PostUserDetailsResult
                    {
                        IsUserAlreadyExists = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new PostUserDetailsResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Fetch user details by the user name.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>User details.</returns>
        public UserDetailsResult FetchUserInformation(string userName)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@UserName",userName)
                };

                DataSet ds = BKICSQL.eds(UsersSP.FetchUserDetails, param);
                UserDetailsResult userdetails = new UserDetailsResult();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userdetails.UserName = Convert.ToString(dr["USERNAME"]);
                    userdetails.AgentBranch = Convert.ToString(dr["AGENTBRANCH"]);
                    userdetails.Agency = Convert.ToString(dr["AGENCY"]);
                    userdetails.AgentCode = Convert.ToString(dr["AGENTCODE"]);
                    userdetails.UserID = Convert.ToString(dr["USERID"]);
                    userdetails.ID = Convert.ToInt32(dr["ID"]);
                    userdetails.IsShowPayments = Convert.ToBoolean(dr["IsShowPayment"]);
                    //userdetails.AgentLogo = (byte[])dr["Logo"];
                }
                string products = string.Empty;
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    products += Convert.ToString(dr["InsuranceTypeID"]);
                    products += ";";
                }
                userdetails.IsTransactionDone = true;
                userdetails.Products = products;
                return userdetails;
            }
            catch (Exception ex)
            {
                return new UserDetailsResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Register the new user based the role.
        /// </summary>
        /// <param name="details">User details.</param>
        /// <returns></returns>
        public PostAdminUserResult PostAdminUser(AdminRegister details)
        {
            try
            {
                Membership.CreateUser(details.UserName, details.Password, details.EmailAddress);
                Roles.AddUserToRole(details.UserName, Constants.Roles.SuperAdmin);
                return new PostAdminUserResult
                {
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new PostAdminUserResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
    }
}