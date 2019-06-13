using System;
using System.Collections.Generic;
using System.Data;

namespace BKIC.SellingPoint.DL.BO
{
    public class FetchPolicyDetailsRequest
    {
        public int HIRStatusCode { get; set; }
        public string InsuredCode { get; set; }
        public string InsuranceType { get; set; }
        public string DocumentNo { get; set; }
        public string CPR { get; set; }
        public string Source { get; set; }
        public bool IsHIR { get; set; }
        public string FilterType { get; set; }
        public string PolicyType { get; set; }
    }

    public class FetchPolicyDetailsRespose : TransactionWrapper
    {
        public DataTable PolicyDetaildt { get; set; }
        public FetchPolicyDetailsRespose()
        {
            PolicyDetaildt = new DataTable();
        }
    }

    public class UpdateHIRStatusRequest
    {
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public string DocumentNo { get; set; }
        public string URL { get; set; }
        public string LinkId { get; set; }
        public string Message { get; set; }
        public int HIRStatusCode { get; set; }
        public int ID { get; set; }
        public string InsuranceType { get; set; }
    }

    public class UpdateHIRStatusResponse : TransactionWrapper
    {
        public bool IsMailSend { get; set; }
    }


    public class UpdateHIRRemarksRequest
    {
        public string DocumentNo { get; set; }
        public string Remarks { get; set; }
        public string InsuranceType { get; set; }
        public int RenewalCount { get; set; }
    }

    public class UpdateHIRRemarksResponse : TransactionWrapper
    {
       
    }

    public class MotorDetailsPortalResponse : TransactionWrapper
    {
        public MotorInsurancePolicy MotorInsurancePolicy { get; set; }
        public decimal LoadAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Code { get; set; }
        public string Remarks { get; set; }

        public MotorDetailsPortalResponse()
        {
            MotorInsurancePolicy = new MotorInsurancePolicy();
        }
    }

    public class FetchDocumentsResponse : TransactionWrapper
    {
        public DataTable HIRDocdt { get; set; }
        public FetchDocumentsResponse()
        {
            HIRDocdt = new DataTable();
        }
    }

    public class FetchDocumentsRequest
    {
        public string LinkID { get; set; }
        public string DocumentNo { get; set; }
        public string InsuredCode { get; set; }
    }

    public class DocumentsUploadPrecheckResponse : TransactionWrapper
    {
        public bool IsValidUser { get; set; }
        public bool IsDocumentsCanUpload { get; set; }
    }

    public class HIR
    {
        public int HIRCount { get; set; }
        public string InsuranceType { get; set; }
    }

    public class Active
    {
        public int ActiveCount { get; set; }
        public string InsuranceType { get; set; }
        public double TotalAmount { get; set; }
    }

    public class Renew
    {
        public int ActiveCount { get; set; }
        public string InsuranceType { get; set; }
        public double TotalAmount { get; set; }
    }

    public class DashboardResponse : TransactionWrapper
    {
        public List<HIR> HIRList { get; set; }
        public List<Active> ActiveList { get; set; }
        public DashboardResponse()
        {
            HIRList = new List<HIR>();
            ActiveList = new List<Active>();

        }
    }

    public class DashboardRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string AgencyCode { get; set; }
        public string Agent { get; set; }
        public string BranchCode { get; set; }
    }

    public class PortalUserDetails
    {
        public int ID { get; set; }
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public string CPR { get; set; }
        public string Mobile { get; set; }
        public string EmailAddress { get; set; }
        public string UserAccountExist { get; set; }
    }

    public class FetchUserDetailsResponse : TransactionWrapper
    {
        public List<PortalUserDetails> UserDetails { get; set; }
        public FetchUserDetailsResponse()
        {
            UserDetails = new List<PortalUserDetails>();
        }
    }

    public class FetchUserDetailsRequest
    {
        public string Search { get; set; }
        public string FilterType { get; set; }
    }

    public class ChangeUserStatusResponse : TransactionWrapper
    {
        public bool Status { get; set; }
    }

    public class ChangeUserStatusRequest
    {
        public string InsuredCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class FeedbackReplyMessageResponse : TransactionWrapper
    {
        public List<string> ReplyMessage { get; set; }

        public FeedbackReplyMessageResponse()
        {
            ReplyMessage = new List<string>();
        }
    }

    public class DownloadScheuleRequest
    {
        public string InsuranceType { get; set; }
        public string DocNo { get; set; }
        public string InsuredCode { get; set; }
        public string AgentCode { get; set; }
        public bool IsEndorsement { get; set; }
        public long EndorsementID { get; set; }
        public int RenewalCount { get; set; }

    }

    public class DownloadScheduleResponse : TransactionWrapper
    {
        public string FilePath { get; set; }
    }

    public class CustomerCallBack
    {
        public Int64 CustID { get; set; }
        public string Remarks { get; set; }
        public string PhoneNo { get; set; }
        public bool IsAttend { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CustomerCallBackRequest
    {
        public Int64 CustID { get; set; }
        public string PhoneNo { get; set; }
        public string Remarks { get; set; }
        public string Type { get; set; }
    }

    public class CustomerCallBackResponse : TransactionWrapper
    {
        public List<CustomerCallBack> CustomerPhoneList { get; set; }
    }

    public class PaidPolicy
    {
        public string RefID { get; set; }
        public string DocumentNo { get; set; }
        public string InsuredCode { get; set; }
        public string CPR { get; set; }
        public string PaymentDate { get; set; }
        public string InsuranceType { get; set; }
        public string TransactionNo { get; set; }
        public string Authorization { get; set; }
    }

    public class PaidPolicyList : TransactionWrapper
    {
        public List<PaidPolicy> PaidPolicy { get; set; }

        public PaidPolicyList()
        {
            PaidPolicy = new List<PaidPolicy>();
        }
    }

    public class UntrackPaidPolicy
    {
        public string PaymentTrack { get; set; }
        public string DocumentNo { get; set; }
        public string InsuranceType { get; set; }
        public decimal PaidAmount { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class UntrackPaidPolicyResponse : TransactionWrapper
    {
        public List<UntrackPaidPolicy> UntrackPolicy { get; set; }

        public UntrackPaidPolicyResponse()
        {
            UntrackPolicy = new List<UntrackPaidPolicy>();
        }
    }
}