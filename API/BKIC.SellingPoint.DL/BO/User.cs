using System;
using System.Collections.Generic;

namespace BKIC.SellingPoint.DL.BO
{
    public class AuthenticationHeader
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientIPAddress { get; set; }
    }

    public class AuthenticationResult : TransactionWrapper
    {
        public bool IsValidUser { get; set; }
    }

    public class UserMaster
    {
        public int Id { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Password { get; set; }
        //public DateTime PasswordExpiryDate { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int StaffNo { get; set; }
        public string Role { get; set; }
        public string CreatedBy { get; set; }
        public string Type { get; set; }
    }

    public class PostUserDetailsResult : TransactionWrapper
    {
        public bool IsUserAlreadyExists { get; set; }
        public bool PasswordStrength { get; set; }
    }

    public class UserDetailsResult : TransactionWrapper
    {
        public string AgentCode { get; set; }
        public byte[] AgentLogo { get; set; }
        public string Agency { get; set; }
        public string AgentBranch { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string Products { get; set; }
        public int ID { get; set; }
        public bool IsShowPayments { get; set; }
    }

    public class AdminRegister
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class PostAdminUserResult : TransactionWrapper
    {
    }

    public class InsuredMaster
    {
        public UserDetails UserInfo { get; set; }
        public UserAdderDetails UserAddressInfo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }

        public InsuredMaster()
        {
            UserInfo = new UserDetails();
            UserAddressInfo = new UserAdderDetails();
        }
    }

    public class InsuredMasterResult : TransactionWrapper
    {
        public UserDetails InsuredUserDetails { get; set; }
        public UserAdderDetails InsuredAddressDetails { get; set; }

        public InsuredMasterResult()
        {
            InsuredUserDetails = new UserDetails();
            InsuredAddressDetails = new UserAdderDetails();
        }
    }

    public class InsuredMasterUpdate
    {
        public int UserId { get; set; }
        public UserDetailsEgov UserInfo { get; set; }
        public UserAdderDetails UserAddressInfo { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public InsuredMasterUpdate()
        {
            UserInfo = new UserDetailsEgov();
            UserAddressInfo = new UserAdderDetails();
        }
    }

    public class InsuredMasterUpdateResult : TransactionWrapper
    {
        public bool IsUserUpdate { get; set; }
    }

    public class UserDetails
    {
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public string UserName { get; set; }
        public string Passport_ICNumber { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public int YearOfBirth { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public DateTime DOB { get; set; }
        public string MartialStatus { get; set; }
        public string GroupCode { get; set; }
        public string GroupCodeDetails { get; set; }
        public string CPR { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserAdderDetails
    {
        public string RoadNumber { get; set; }
        public string BlockNumber { get; set; }
        public string FlatNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string Address1 { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string EmailAddress { get; set; }
        public string WebSiteAddress { get; set; }
        public string TelephoneResidence { get; set; }
        public string TelephoneMobile { get; set; }
        public string POBox { get; set; }
        public string LineNo { get; set; }
        public string CountryCode { get; set; }
        public string AddressType { get; set; }
        public string Town { get; set; }
    }

    public class InuredMasterResult : TransactionWrapper
    {
        public bool IsUserCreated { get; set; }
        public bool IsUserCPRAlreadyExists { get; set; }
        public bool IsUserEmailAlreadyExist { get; set; }
        public bool IsMailSend { get; set; }
    }

    public class ChangePasword
    {
        public int UserId { get; set; }

        public string InsuredCode { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordResult : TransactionWrapper
    {
        public bool IsPasswordChanged { get; set; }
        public bool IsCurrentPasswordCorrect { get; set; }
        public bool IsMailSend { get; set; }
    }

    public class ForgotPassword
    {
        public string CPR { get; set; }
    }

    public class ForgotPasswordResult : TransactionWrapper
    {
        public bool IsCPRExistsInSystem { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsMailSend { get; set; }
    }

    public class IsUserCPRExist : TransactionWrapper
    {
        public bool IsCPRExists { get; set; }
        public bool IsUserLoginExist { get; set; }
    }

    public class LoginAudit
    {
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public DateTime LoginDate { get; set; }
        public string LoginStatus { get; set; }
    }

    public class UserPolicyDashboardResult : TransactionWrapper
    {
        public List<PolicyDashboardDetails> MyPolicy { get; set; }

        public UserPolicyDashboardResult()
        {
            MyPolicy = new List<PolicyDashboardDetails>();
        }
    }

    public class UserSavedQuotationsResult : TransactionWrapper
    {
        public List<UserQuotationDashboardDetails> MyQuotations { get; set; }

        public UserSavedQuotationsResult()
        {
            MyQuotations = new List<UserQuotationDashboardDetails>();
        }
    }

    public class UserQuotationDashboardDetails
    {
        public string InsuranceType { get; set; }
        public int InsuranceId { get; set; }
        public bool IsHIR { get; set; }

        public string HIRStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string InsuredCode { get; set; }
    }

    public class PolicyDashboardDetails
    {
        public string InsuranceType { get; set; }
        public int InsuranceId { get; set; }
        public bool IsHIR { get; set; }

        public string HIRStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InsuredCode { get; set; }

        public int RenewalExpiryDays { get; set; }
        public string DocumentNo { get; set; }
        public int HIRStatusID { get; set; }
        public bool IsActive { get; set; }
        public bool IsSaved { get; set; }
        public bool DueForRenew { get; set; }
        public string CPR { get; set; }

        public string TrackID { get; set; }
    }

    public class PreCheckForgotPasswordLink : TransactionWrapper
    {
        public bool IsValidLink { get; set; }
    }

    public class ResetPasswordResult : TransactionWrapper
    {
        public bool IsAccountPresent { get; set; }
        public bool IsPasswordChanged { get; set; }
    }
}