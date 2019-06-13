using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class PolicyDetails
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "transactionNo")]
        public string TransactionNo { get; set; }

        [JsonProperty(PropertyName = "authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonProperty(PropertyName = "transactionDate")]
        public DateTime? TransactionDate { get; set; }

        [JsonProperty(PropertyName = "netPremium")]
        public decimal NetPremium { get; set; }

        [JsonProperty(PropertyName = "hirStatusCode")]
        public string HIRStatusCode { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "isEmailMessageAvailable")]
        public bool IsEmailMessageAvailable { get; set; }

        [JsonProperty(PropertyName = "hirRejectReason")]
        public string HIRRejectReason { get; set; }

        [JsonProperty(PropertyName = "isDocumentAvailable")]
        public bool IsDocumentAvailable { get; set; }

        [JsonProperty(PropertyName = "commenceDate")]
        public DateTime CommenceDate { get; set; }

        [JsonProperty(PropertyName = "expiryDate")]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subclass")]
        public string Subclass { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public string IsRenewal { get; set; }

        [JsonProperty(PropertyName = "actions")]
        public string Actions { get; set; }
    }

    public class FetchPolicyDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "policyDetailList")]
        public List<PolicyDetails> PolicyDetailList { get; set; }

        public FetchPolicyDetailsResponse()
        {
            PolicyDetailList = new List<PolicyDetails>();
        }
    }

    public class FetchPolicyDetailsRequest
    {
        [JsonProperty(PropertyName = "hirStatusCode")]
        public int HIRStatusCode { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "filterType")]
        public string FilterType { get; set; }

        [JsonProperty(PropertyName = "policyType")]
        public string PolicyType { get; set; }
    }

    public class UpdateHIRStatusRequest
    {
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "hirStatusCode")]
        public int HIRStatusCode { get; set; }

        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "linkId")]
        public string LinkId { get; set; }
    }

    public class UpdateHIRStatusResponse : TransactionWrapper
    {
    }


    public class UpdateHIRRemarksRequest
    {
        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }
        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }
    }

    public class UpdateHIRRemarksResponse : TransactionWrapper
    {

    }

    public class MotorDetailsPortalResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorInsurancePolicy")]
        public MotorInsurancePolicy MotorInsurancePolicy { get; set; }

        [JsonProperty(PropertyName = "loadAmount")]
        public decimal LoadAmount { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "discountAmount")]
        public decimal DiscountAmount { get; set; }

        public MotorDetailsPortalResponse()
        {
            MotorInsurancePolicy = new MotorInsurancePolicy();
        }
    }

    public class HIRRequestDocuments
    {
        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "fileURL")]
        public string FileURL { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }
    }

    public class FetchDocumentsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "filesDocuments")]
        public List<HIRRequestDocuments> FilesDocuments { get; set; }

        public FetchDocumentsResponse()
        {
            FilesDocuments = new List<HIRRequestDocuments>();
        }
    }

    public class FetchDocumentsRequest
    {
        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }
    }

    public class HIRDocumentsUploadPrecheckResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isValidUser")]
        public bool IsValidUser { get; set; }

        [JsonProperty(PropertyName = "isDocumentsCanUpload")]
        public bool IsDocumentsCanUpload { get; set; }
    }

    public class EmailMessageAudit
    {
        [JsonProperty(PropertyName = "messageKey")]
        public string MessageKey { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "policyNo")]
        public string PolicyNo { get; set; }

        [JsonProperty(PropertyName = "linkNo")]
        public string LinkNo { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "trackId")]
        public string TrackId { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
    }

    public class EmailMessageAuditResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "emailMessage")]
        public List<EmailMessageAudit> EmailMessage { get; set; }

        public EmailMessageAuditResult()
        {
            EmailMessage = new List<EmailMessageAudit>();
        }
    }

    public class HIR
    {
        [JsonProperty(PropertyName = "hirCount")]
        public int HIRCount { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
    }

    public class Active
    {
        [JsonProperty(PropertyName = "activeCount")]
        public int ActiveCount { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "totalAmount")]
        public double TotalAmount { get; set; }
    }

    public class Renew
    {
        [JsonProperty(PropertyName = "renewCount")]
        public int ActiveCount { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "totalAmount")]
        public double TotalAmount { get; set; }
    }

    public class DashboardResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "hirList")]
        public List<HIR> HIRList { get; set; }

        [JsonProperty(PropertyName = "activeList")]
        public List<Active> ActiveList { get; set; }

        

        public DashboardResponse()
        {
            HIRList = new List<HIR>();
            ActiveList = new List<Active>();           
        }
    }

    public class DashboardRequest
    {
        [JsonProperty(PropertyName = "fromDate")]
        public DateTime FromDate { get; set; }

        [JsonProperty(PropertyName = "toDate")]
        public DateTime ToDate { get; set; }

        [JsonProperty(PropertyName = "agencyCode")]
        public string AgencyCode { get; set; }

        [JsonProperty(PropertyName = "agent")]
        public string Agent { get; set; }

        [JsonProperty(PropertyName = "branchCode")]
        public string BranchCode { get; set; }
    }

    public class PortalUserDetails
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "userAccountExist")]
        public string UserAccountExist { get; set; }
    }

    public class FetchUserDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "userDetails")]
        public List<PortalUserDetails> UserDetails { get; set; }

        public FetchUserDetailsResponse()
        {
            UserDetails = new List<PortalUserDetails>();
        }
    }

    public class FetchUserDetailsRequest
    {
        [JsonProperty(PropertyName = "search")]
        public string Search { get; set; }

        [JsonProperty(PropertyName = "filterType")]
        public string FilterType { get; set; }
    }

    public class ChangeUserStatusResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "insuredCode")]
        public bool Status { get; set; }
    }

    public class ChangeUserStatusRequest
    {
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
    }

    public class FeedbackReplyMessageResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "replyMessage")]
        public List<string> ReplyMessage { get; set; }

        public FeedbackReplyMessageResponse()
        {
            ReplyMessage = new List<string>();
        }
    }

    public class InsertFeedbackReply
    {
        [JsonProperty(PropertyName = "parentFeedbackId")]
        public long ParentFeedbackId { get; set; }

        [JsonProperty(PropertyName = "replyMessage")]
        public string ReplyMessage { get; set; }
    }

    public class DownloadScheuleRequest
    {
        [JsonProperty(PropertyName = "refID")]
        public int RefID { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "isEndorsement")]
        public bool IsEndorsement { get; set; }

        [JsonProperty(PropertyName = "endorsementID")]
        public long EndorsementID { get; set; }
    }

    public class MotorEndorsementScheduleRequest
    {
        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
        [JsonProperty(PropertyName = "docNo")]
        public string DocNo { get; set; }
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "isEndorsement")]
        public bool IsEndorsement { get; set; }
        [JsonProperty(PropertyName = "endorsementType")]
        public string EndorsementType { get; set; }
        [JsonProperty(PropertyName = "oldInsuredCode")]
        public string OldInsuredCode { get; set; }
        [JsonProperty(PropertyName = "oldInsuredName")]
        public string OldInsuredName { get; set; }
        [JsonProperty(PropertyName = "newInsuredCode")]
        public string NewInsuredCode { get; set; }
        [JsonProperty(PropertyName = "newInsuredName")]
        public string NewInsuredName { get; set; }
        [JsonProperty(PropertyName = "oldChassisNo")]
        public string OldChassisNo { get; set; }
        [JsonProperty(PropertyName = "oldRegistrationNo")]
        public string OldRegistrationNo { get; set; }
        [JsonProperty(PropertyName = "newChassisNo")]
        public string NewChassisNo { get; set; }
        [JsonProperty(PropertyName = "newRegistrationNo")]
        public string NewRegistrationNo { get; set; }
        [JsonProperty(PropertyName = "newExpireDate")]
        public DateTime NewExpireDate { get; set; }
        [JsonProperty(PropertyName = "cancelDate")]
        public DateTime CancelDate { get; set; }
        [JsonProperty(PropertyName = "oldPremium")]
        public decimal OldPremium { get; set; }
        [JsonProperty(PropertyName = "newPremium ")]
        public decimal NewPremium { get; set; }
        [JsonProperty(PropertyName = "oldExcess")]
        public decimal OldExcess { get; set; }
        [JsonProperty(PropertyName = "newExcess")]
        public decimal NewExcess { get; set; }
        [JsonProperty(PropertyName = "oldSumInsured ")]
        public decimal OldSumInsured { get; set; }
        [JsonProperty(PropertyName = "newSumInsured")]
        public decimal NewSumInsured { get; set; }
        [JsonProperty(PropertyName = "oldFinancierName")]
        public string OldFinancierName { get; set; }
        [JsonProperty(PropertyName = "newFinancierName")]
        public string NewFinancierName { get; set; }
    }

    public class CustomerCallBackDetails
    {
        [JsonProperty(PropertyName = "custID")]
        public Int64 CustID { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "PhoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty(PropertyName = "isAttend")]
        public bool IsAttend { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime? UpdatedDate { get; set; }
    }

    public class CustomerCallBackRequest
    {
        [JsonProperty(PropertyName = "custID")]
        public Int64 CustID { get; set; }

        [JsonProperty(PropertyName = "phoneNo")]
        public string PhoneNo { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class CustomerCallBackResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "customerPhoneList")]
        public List<CustomerCallBackDetails> CustomerPhoneList { get; set; }
    }

    public class PaidPolicy
    {
        [JsonProperty(PropertyName = "refID")]
        public string RefID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "paymentDate")]
        public string PaymentDate { get; set; }

        [JsonProperty(PropertyName = "transactionNo")]
        public string TransactionNo { get; set; }

        [JsonProperty(PropertyName = "authorizationNo")]
        public string AuthorizationNo { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
    }

    public class PaidPolicyList : TransactionWrapper
    {
        [JsonProperty(PropertyName = "paidPolicy")]
        public List<PaidPolicy> PaidPolicy { get; set; }

        public PaidPolicyList()
        {
            PaidPolicy = new List<PaidPolicy>();
        }
    }

    public class PaidPolicyRequest
    {
        [JsonProperty(PropertyName = "fromdate")]
        public DateTime Fromdate { get; set; }

        [JsonProperty(PropertyName = "toDate")]
        public DateTime ToDate { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
    }

    public class UnTrackPaidPolicyRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class UntrackPaidPolicy
    {
        [JsonProperty(PropertyName = "paymentTrack")]
        public string PaymentTrack { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "paidAmount")]
        public decimal PaidAmount { get; set; }

        [JsonProperty(PropertyName = "errorMsg")]
        public string ErrorMsg { get; set; }
    }

    public class UntrackPaidPolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "untrackPolicy")]
        public List<UntrackPaidPolicy> UntrackPolicy { get; set; }

        public UntrackPaidPolicyResponse()
        {
            UntrackPolicy = new List<UntrackPaidPolicy>();
        }
    }
}