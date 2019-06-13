using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DTO.Constants
{
   public class InsurancePortalURI
    {
        public const string FetchDetails = "api/insuranceportal/fetchdetails";
        public const string MotorDetails = "api/insuranceportal/getmotordetails/{motorid}/{insuredCode}";
        public const string UpdateHIRStatus = "api/insuranceportal/StatusUpdate";
        public const string UploadTempFile = "api/insuranceportal/uploadtempfile/{insuranceType}/{insuredCode}/{policyNo}/{linkId}";
        public const string DownloadTempFile = "api/insuranceportal/downloadtempfile/{insuranceType}/{insuredCode}/{policyNo}/{linkId}/{fileName}";
        public const string DeleteTempFile = "api/insuranceportal/deletetempfile/{insuranceType}/{insuredCode}/{policyNo}/{linkId}/{fileName}";
        public const string DownloadInsranceFile = "api/insuranceportal/downloadinsuranceportalfile/{insuranceType}/{insuredCode}/{policyNo}/{linkId}/{fileName}";
        public const string UploadHIRDocuments = "api/insuranceportal/uploadHIRDocuments/{insuranceType}/{insuredCode}/{policyNo}/{linkId}/{refID}";
        public const string DeleteAllTempFiles = "api/insuranceportal/deletealltempfiles/{insuranceType}/{insuredCode}/{policyNo}/{linkId}/{fileName}";
        public const string FetchDocuments = "api/insuranceportal/fetchDocuments";
        public const string PrecheckHIRDocumentUpload = "api/insuranceportal/precheckHIRDocumentUpload/{insuredCode}/{policyNo}/{linkId}/{insuranceType}";
        public const string GetEmailMessageForRecord = "api/insuranceportal/emailMessageForRecord/{insuredCode}/{policyNo}/{linkId}";
        public const string FetchDashboard = "api/insuranceportal/getdashboarddetails";
        public const string FetchUserDetails = "api/insuranceportal/getUserdetails";
        public const string UpdatePassword = "api/insuranceportal/updatePassword";
        public const string UpdateUserStatus = "api/insuranceportal/changeuserstatus";
        public const string InsertFeedbackReply = "api/insuranceportal/insertFeedbackReply";
        public const string GetFeedbackReplyById = "api/insuranceportal/getReplyByFeedbackId/{feedbackId}";
        public const string migraterenewpolicy = "api/insuranceportal/migraterenewpolicy/{insuranceType}/{documentNo}";
        public const string deletedueRenewal = "api/insuranceportal/deleteduerenewal";
        public const string downloadschedule = "api/insuranceportal/downloadschedule/{insuranceType}/{insuredCode}/{RefID}";
        public const string CallBackDetails = "api/insuranceportal/customercallback";
        public const string GetPaidPolicy = "api/insuranceportal/getpaidpolicy";
        public const string PushPolicyOracle = "api/insuranceportal/pushpolicyoracle/{insuranceType}/{refid}";
        public const string UntrackPolicy = "api/insuranceportal/untrackedpolicy";
        public const string UpdateHIRRemarks = "api/insuranceportal/updateHIRRemarks";

    }
}
