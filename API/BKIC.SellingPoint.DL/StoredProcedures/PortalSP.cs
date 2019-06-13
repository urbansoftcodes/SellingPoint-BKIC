namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class PortalSP
    {
        public const string GetMotorDetails = "Portal_GetMotorDetailsByMotorId";
        public const string UpdateHIRStatus = "SP_Admin_UpdateHIRStatus";
        public const string UploadInsuranceDocuments = "SP_Admin_InsertHIRRequestDocuments";
        public const string FetchHIRDocuments = "SP_Admin_FetchHIRDocuments";
        public const string HIRUploadDocumentPreCheck = "SP_Admin_HIRDocumentsUploadPrecheck";
        public const string GetEmailMessageForRecord = "SP_Admin_GetEmailMessageForRecord";
        public const string InsertEmailMessage = "SP_Admin_InsertEmailMessageAudit";
        public const string Fetchdashboard = "PortalDashboard";
        public const string FetchUserDetails = "Portal_LoadUserDetails";
        public const string UpdatePassword = "ChangePasswordByPortal";
        public const string UpdateUserStatus = "UpdateUserStatus";
        public const string GetScheduleInput = "GetScheduleInput";       
        public const string GetEndorsementByDocNo = "SP_GetEndorsementByDocumentNo";
        public const string UpdateHIRRemarks = "SP_Admin_HIRRemarks";
    }
}