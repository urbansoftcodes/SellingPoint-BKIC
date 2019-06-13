using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class AuthenticationHeader
    {
        [JsonProperty(PropertyName = "userName")]
        [Required]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "password")]
        [Required]
        public string Password { get; set; }
    }


    public class LoginAudit
    {
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "ipAddress")]
        public string IPAddress { get; set; }
        [JsonProperty(PropertyName = "loginDate")]
        public DateTime LoginDate { get; set; }
        [JsonProperty(PropertyName = "loginStatus")]
        public string LoginStatus { get; set; }
    }

    public class UserMaster
    {

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }
        [JsonProperty(PropertyName = "userID")]
        public string UserID { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        //[JsonProperty(PropertyName = "passwordExpiryDate")]
        //public DateTime PasswordExpiryDate { get; set; }
        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
        [JsonProperty(PropertyName = "staffNo")]
        public int StaffNo { get; set; }

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class PostUserDetailsResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isUserAlreadyExists")]
        public bool IsUserAlreadyExists { get; set; }
        [JsonProperty(PropertyName = "passwordStrength")]
        public bool PasswordStrength { get; set; }
    }

    public class UserDetailsResult : TransactionWrapper
    {
         [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

         [JsonProperty(PropertyName = "userID")]
        public string UserID { get; set; }

        [JsonProperty(PropertyName = "products")]
        public string Products { get; set; }

        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "agentLogo")]
        public byte[] AgentLogo { get; set; }

    }

    public class AdminRegister
    {
         [JsonProperty(PropertyName = "userName")]
         public string UserName { get; set; }
         [JsonProperty(PropertyName = "password")]
         public string Password { get; set; }
         [JsonProperty(PropertyName = "emailAddress")]
         public string EmailAddress { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
    public class PostAdminUserResult:TransactionWrapper
    {
        
    }


    public class InsuredMaster
    {
        [JsonProperty(PropertyName = "userInfo")]
        public UserDetails UserInfo { get; set; }
        [JsonProperty(PropertyName = "userAddressInfo")]
        public UserAdderDetails UserAddressInfo { get; set; }
        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }
        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime UpdatedDate { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public InsuredMaster()
        {
            UserInfo = new UserDetails();
            UserAddressInfo = new UserAdderDetails();
        }

    }

    public class InsuredMasterUpdate
    {
        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }
        [JsonProperty(PropertyName = "userInfo")]
        public UserDetails UserInfo { get; set; }
        [JsonProperty(PropertyName = "userAddressInfo")]
        public UserAdderDetails UserAddressInfo { get; set; }
        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }

        public InsuredMasterUpdate()
        {
            UserInfo = new UserDetails();
            UserAddressInfo = new UserAdderDetails();
        }

    }

    public class InsuredMasterUpdateResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isUserUpdate")]
        public bool IsUserUpdate { get; set; }

    }

    public class UserDetails
    {
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }
        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "passport_ICNumber")]
        public string Passport_ICNumber { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "sex")]
        public string Sex { get; set; }
        [JsonProperty(PropertyName = "yearOfBirth")]
        public int YearOfBirth { get; set; }
        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "nationality")]
        public string Nationality { get; set; }
        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }
        [JsonProperty(PropertyName = "dOB")]
        public DateTime DOB { get; set; }
        [JsonProperty(PropertyName = "martialStatus")]
        public string MartialStatus { get; set; }
        [JsonProperty(PropertyName = "groupCode")]
        public string GroupCode { get; set; }
        [JsonProperty(PropertyName = "groupCodeDetails")]
        public string GroupCodeDetails { get; set; }
        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
    }

    public class UserAdderDetails
    {
        [JsonProperty(PropertyName = "roadNumber")]
        public string RoadNumber { get; set; }
        [JsonProperty(PropertyName = "blockNumber")]
        public string BlockNumber { get; set; }
        [JsonProperty(PropertyName = "flatNumber")]
        public string FlatNumber { get; set; }
        [JsonProperty(PropertyName = "buildingNumber")]
        public string BuildingNumber { get; set; }
        [JsonProperty(PropertyName = "address1")]
        public string Address1 { get; set; }
        [JsonProperty(PropertyName = "telephone")]
        public string Telephone { get; set; }
        [JsonProperty(PropertyName = "fax")]
        public string Fax { get; set; }
        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty(PropertyName = "webSiteAddress")]
        public string WebSiteAddress { get; set; }
        [JsonProperty(PropertyName = "telephoneResidence")]
        public string TelephoneResidence { get; set; }
        [JsonProperty(PropertyName = "telephoneMobile")]
        public string TelephoneMobile { get; set; }
        [JsonProperty(PropertyName = "poBox")]
        public string POBox { get; set; }
        [JsonProperty(PropertyName = "lineNo")]
        public string LineNo { get; set; }
        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }
        [JsonProperty(PropertyName = "addressType")]
        public string AddressType { get; set; }
        [JsonProperty(PropertyName = "town")]
        public string Town { get; set; }
    }

    public class InsuredMasterResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isUserCreated")]
        public bool IsUserCreated { get; set; }
        [JsonProperty(PropertyName = "isUserCPRAlreadyExists")]
        public bool IsUserCPRAlreadyExists { get; set; }
        [JsonProperty(PropertyName = "isUserEmailAlreadyExist")]
        public bool IsUserEmailAlreadyExist { get; set; }
        [JsonProperty(PropertyName = "isMailSend")]
        public bool IsMailSend { get; set; }
    }

    public class InsuredMasterDetailsResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "insuredUserDetails")]
        public UserDetails InsuredUserDetails { get; set; }

        [JsonProperty(PropertyName = "insuredUserAddressDetails")]
        public UserAdderDetails InsuredUserAddressDetails { get; set; }

        public InsuredMasterDetailsResult()
        {
            InsuredUserDetails = new UserDetails();
            InsuredUserAddressDetails = new UserAdderDetails();
        }


    }
    public class ChangePasword
    {
        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }
        [JsonProperty(PropertyName = "currentPassword")]
        public string CurrentPassword { get; set; }
        [JsonProperty(PropertyName = "newPassword")]
        public string NewPassword { get; set; }
    }

    public class ChangePasswordResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isPasswordChanged")]
        public bool IsPasswordChanged { get; set; }
        [JsonProperty(PropertyName = "isCurrentPasswordCorrect")]
        public bool IsCurrentPasswordCorrect { get; set; }
        [JsonProperty(PropertyName = "isMailSend")]
        public bool IsMailSend { get; set; }
    }

    public class ForgotPassword
    {
        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }
    }

    public class ForgotPasswordResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isCPRExistsInSystem")]
        public bool IsCPRExistsInSystem { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "isMailSend")]
        public bool IsMailSend { get; set; }
    }

    public class UserSavedQuotationsResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "myQuotations")]
        public List<UserQuotationDashboardDetails> MyQuotations { get; set; }


        public UserSavedQuotationsResult()
        {
            MyQuotations = new List<UserQuotationDashboardDetails>();
        }

    }

    public class UserQuotationDashboardDetails
    {
        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
        [JsonProperty(PropertyName = "insuranceId")]
        public int InsuranceId { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public string HIRStatus { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }
    }

    public class IsUserCPRExistResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isCPRExists")]
        public bool IsCPRExists { get; set; }

        [JsonProperty(PropertyName = "isUserloginExist")]
        public bool IsUserloginExist { get; set; }
    }

    public class IsEmailExistResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isEmailExists")]
        public bool IsEmailExists { get; set; }
    }
    public class PolicyDashboardDetails
    {
        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
        [JsonProperty(PropertyName = "insuranceId")]
        public int InsuranceId { get; set; }
        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }
        [JsonProperty(PropertyName = "hirStatus")]
        public string HIRStatus { get; set; }
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate { get; set; }
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }
        [JsonProperty(PropertyName = "renewalExpiryDays")]
        public int RenewalExpiryDays { get; set; }
        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
        [JsonProperty(PropertyName = "hirStatusID")]
        public int HIRStatusID { get; set; }
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
        [JsonProperty(PropertyName = "isSaved")]
        public bool IsSaved { get; set; }
        [JsonProperty(PropertyName = "dueForRenew")]
        public bool DueForRenew { get; set; }
        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }
        [JsonProperty(PropertyName = "trackID")]
        public string TrackID { get; set; }
    }
    public class UserPolicyDashboardResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "myPolicy")]
        public List<PolicyDashboardDetails> MyPolicy { get; set; }


        public UserPolicyDashboardResult()
        {
            MyPolicy = new List<PolicyDashboardDetails>();
        }
    }

    public class PreCheckForgotPasswordLink : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isValidLink")]
        public bool IsValidLink { get; set; }
    }

    public class ResetPassword
    {
        [JsonProperty(PropertyName = "trackId")]
        public string TrackId { get; set; }
        [JsonProperty(PropertyName = "newPassword")]
        public string NewPassword { get; set; }
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }
    }

    public class ResetPasswordResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isAccountPresent")]
        public bool IsAccountPresent { get; set; }
        [JsonProperty(PropertyName = "isPasswordChanged")]
        public bool IsPasswordChanged { get; set; }
    }

    public class CommercialProductquoteRequest
    {
        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "phoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "productType")]
        public string ProductType { get; set; }
    }

    public class CommercialProductquoteResponse : TransactionWrapper
    {

    }
}
