using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using BDL = BKIC.SellingPoint.DL.BL;
using BLO = BKIC.SellingPoint.DL.BO;
using BLR = BKIC.SellingPoint.DL.BL.Repositories;

namespace BKIC.SellingPoint.WebAPI.Framework
{
    public class ApiTokenAuth : OAuthAuthorizationServerProvider
    {
        private readonly BLR.IUser _user;

        public ApiTokenAuth()
        {
            _user = new BDL.User();
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            var validUser = _user.IsUserValid(context.UserName, context.Password);

            BLO.LoginAudit loginAudit = new BLO.LoginAudit();
            //loginAudit.IPAddress =GetClientIP();
            loginAudit.UserName = context.UserName;
            loginAudit.LoginDate = DateTime.Now;
            loginAudit.LoginStatus = "Failed";

            if (validUser.IsTransactionDone)
            {
                if (validUser.IsValidUser)
                {
                    string[] roles = _user.GetUserRoles(context.UserName);

                    foreach (string role in roles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }                    

                    identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

                    //if (roles[0] != "SuperAdmin")
                    //{
                    //    var userDetails = _user.FetchUserInformation(context.UserName);
                    //    identity.AddClaim(new Claim(ClaimTypes.Name, userDetails.UserID));
                    //    identity.AddClaim(new Claim("userName", userDetails.UserName));
                    //    identity.AddClaim(new Claim("userID", userDetails.UserID));
                    //    identity.AddClaim(new Claim("agentCode", userDetails.AgentCode));
                    //    identity.AddClaim(new Claim("agentBranch", userDetails.AgentBranch));
                    //    identity.AddClaim(new Claim("agency", userDetails.Agency));
                    //    identity.AddClaim(new Claim("products", userDetails.Products));
                    //}
                    //else
                    //{
                    //    identity.AddClaim(new Claim(ClaimTypes.Name, "SuperAdmin"));
                    //}
                    var userDetails = _user.FetchUserInformation(context.UserName);
                    identity.AddClaim(new Claim(ClaimTypes.Name, userDetails.UserID));
                    identity.AddClaim(new Claim("userName", userDetails.UserName));
                    identity.AddClaim(new Claim("userID", userDetails.UserID));
                    identity.AddClaim(new Claim("agentCode", userDetails.AgentCode));
                    identity.AddClaim(new Claim("agentBranch", userDetails.AgentBranch));
                    identity.AddClaim(new Claim("agency", userDetails.Agency));
                    identity.AddClaim(new Claim("products", userDetails.Products));
                    identity.AddClaim(new Claim("id", userDetails.ID.ToString()));
                    identity.AddClaim(new Claim("isShowPayments", userDetails.IsShowPayments.ToString()));
                    //identity.AddClaim(new Claim("agentLogo", Convert.ToBase64String(userDetails.AgentLogo)));

                    context.Validated(identity);
                    loginAudit.LoginStatus = "Success";
                    //new Task(() => { TrackLoginAudit(loginAudit); }).Start();
                }
                else
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    //new Task(() => { TrackLoginAudit(loginAudit); }).Start();
                    return base.GrantResourceOwnerCredentials(context);
                }
            }
            else
            {
                context.SetError("Transaction_error", "Transaction failed. Please try again");
                //new Task(() => { TrackLoginAudit(loginAudit); }).Start();
                return base.GrantResourceOwnerCredentials(context);
            }

            return base.GrantResourceOwnerCredentials(context);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            var identity = context.Identity as ClaimsIdentity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);

            // Append authenticated user roles in token end point response
            if (roles != null)
            {
                var roleKeyValue = new KeyValuePair<string, string>("roles", string.Join(",", roles.ToList()));
                context.AdditionalResponseParameters.Add(roleKeyValue.Key, roleKeyValue.Value);
            }

            if (!string.IsNullOrEmpty(identity.Name))
            {
                var userKeyValue = new KeyValuePair<string, string>("name", identity.Name);
                context.AdditionalResponseParameters.Add(userKeyValue.Key, userKeyValue.Value);
            }

            var userId = identity.Claims
                        .Where(c => c.Type == "userId")
                        .Select(c => c.Value);

            if (userId != null)
            {
                var userIdKeyValue = new KeyValuePair<string, string>("userID", string.Join(",", userId.ToList()));
                context.AdditionalResponseParameters.Add(userIdKeyValue.Key, userIdKeyValue.Value);
            }

            var Id = identity.Claims
                      .Where(c => c.Type == "id")
                      .Select(c => c.Value);

            if (Id != null)
            {
                var IdKeyValue = new KeyValuePair<string, string>("ID", string.Join(",", Id.ToList()));
                context.AdditionalResponseParameters.Add(IdKeyValue.Key, IdKeyValue.Value);
            }

            var userName = identity.Claims
                        .Where(c => c.Type == "userName")
                        .Select(c => c.Value).DefaultIfEmpty();

            if (userName != null)
            {
                context.AdditionalResponseParameters.Add("userName", string.Join(",", userName.ToList()));
            }

            var agency = identity.Claims
                         .Where(c => c.Type == "agency")
                         .Select(c => c.Value).DefaultIfEmpty();

            if (agency != null)
            {
                context.AdditionalResponseParameters.Add("agency", string.Join(",", agency.ToList()));
            }

            var agentCode = identity.Claims
                        .Where(c => c.Type == "agentCode")
                        .Select(c => c.Value).DefaultIfEmpty();

            if (agentCode != null)
            {
                context.AdditionalResponseParameters.Add("agentCode", string.Join(",", agentCode.ToList()));
            }

            var agentBranch = identity.Claims
                        .Where(c => c.Type == "agentBranch")
                        .Select(c => c.Value).DefaultIfEmpty();

            if (agentBranch != null)
            {
                context.AdditionalResponseParameters.Add("agentBranch", string.Join(",", agentBranch.ToList()));
            }

            var products = identity.Claims
                    .Where(c => c.Type == "products")
                    .Select(c => c.Value).DefaultIfEmpty();

            var bb = products.ToList();

            if (products != null)
            {
                context.AdditionalResponseParameters.Add("products", string.Join(",", products.ToList()));
            }

            var agentLogo = identity.Claims
                           .Where(c => c.Type == "agentLogo")
                           .Select(c => c.Value).DefaultIfEmpty();

            if (agentLogo != null)
            {
                context.AdditionalResponseParameters.Add("agentLogo", string.Join(",", agentLogo.ToList()));
            }

            var isShowPayments = identity.Claims
                          .Where(c => c.Type == "isShowPayments")
                          .Select(c => c.Value).DefaultIfEmpty();

            if (isShowPayments != null)
            {
                context.AdditionalResponseParameters.Add("isShowPayments", string.Join(",", isShowPayments.ToList()));
            }

            context.AdditionalResponseParameters.Add("statusCode", 200);

            return Task.FromResult<object>(null);
        }

        private void TrackLoginAudit(BLO.LoginAudit audit)
        {
            _user.TrackLogin(audit);
        }

        public static String GetClientIP()
        {
            String ip =
                HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
    }
}