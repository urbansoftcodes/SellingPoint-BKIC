namespace BKIC.SellingPoint.DL.Constants
{
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string BranchAdmin = "BranchAdmin";
        public const string User = "User";
    }

    public static class Insurance
    {
        public const string Motor = "MotorInsurance";
        public const string Home = "HomeInsurance";
        public const string Travel = "TravelInsurance";
        public const string DomesticHelp = "DomesticInsurance";
    }

    public static class SearchType
    {
        public static bool ByAgencyCode = false;
        public static bool ByHIRStatus = false;
        public static bool ByDocumentNo = false;
        public static bool ByStatusAndAgency = false;
        public static bool All = false;
    }

    public static class MotorEndorsementTypes
    {
        public const string Transfer = "Transfer";
        public const string AddRemoveBank = "AddRemoveBank";
        public const string ChangeRegistration = "ChangeRegistration";
        public const string CancelPolicy = "CancelPolicy";
        public const string Extended = "Extended";
        public const string ChangeSumInsured = "ChangeSumInsured";
    }

    public static class MotorEndorsementTypesNames
    {
        public const string Transfer = "POLICY_TRANSFER";
        public const string AddRemoveBank = "CHANGE_FINANCE_COMP";
        public const string ChangeRegistration = "CHANGE_VEH_REG";
        public const string CancelPolicy = "CANCELLATION_POLICY";
        public const string Extended = "EXTENSION_POLICY";
        public const string ChangeSumInsured = "SUMINSURED-CHANGE";
        public const string ChangeExess = "CHANGE_EXCESS_AMT";
        public const string AddCover = "ADD_COVER";
        public const string ChangePremium = "CHANGE_PREMIUM";
        public const string InternalEndorsement = "INTERNAL_ENDORSEMENT";
    }

    public static class HomeEndorsementTypesNames
    {       
        public const string AddRemoveBank = "CHANGE_BENEFICIARY";       
        public const string CancelPolicy = "CANCELLATION_POLICY";      
        public const string ChangeSumInsured = "SUMINSURED_CHANGE";
        public const string ChangeAddress = "CHANGE_RISK_ADDRESS";
        public const string AddRemoveDomesticHelp = "CHANGE_DOMESTIC_HELP";        
    }

    public static class TravelEndorsementTypesNames
    {       
        public const string CancelPolicy = "CANCELLATION";
        public const string ChangeMemeber = "MEMBER CHANGES";       
        public const string ChangePremium = "PREMIUM_CHANGE";
    }

    public static class MailMessageKey
    {
        public const string RegistrationSuccess = "register-to-user";
        public const string ChangePassword = "new-password-to-user";
        public const string ForgotPassword = "new-password-to-user";
        public const string PolicyRejected = "PolicyRejected";
        public const string PolicyApproved = "policy-approval-to-user";
        public const string AwaitingDocuments = "policy-document-request-to-user";
        public const string PolicyRenewalCompletedToAdmin = "policy-renewal-hir-complete-to-admin";
        public const string PolicyRenewalComplete = "policy-renewal-hir-complete-to-user";

        //  public const string PolicyRejected = "policy-rejected-notification-to-user";
        public const string PolicyRenewalDue = "policy-renew-notification-to-user";
        public const string PolicyRenewed = "policy-renew-to-user";
        public const string PolicyRegisterHIRComplete = "policy-register-hir-complete-to-admin";
        public const string RequestDocumentsReceived = "policy-register-hir-document-to-admin";

        //public static string SiteName = ConfigurationManager.AppSettings["SiteName"].ToString();
        public const string HIRAttachUIUri = "UploadDocuments.aspx?InsuredCode={insuredCode}&PolicyNumber={policyNo}&LinkedId={linkId}&InsuranceType={insuranceType}&RefID={refID}";

        public const string ForgetResetUIUri = "ResetPassword.aspx?TrackId={trackId}";
        public const string NEWPolicy = "policy-register-hin-to-admin";
        public const string ApprovedURlTravel = "TravelInsurance.aspx?Ref={{RefID}}&TrackId={{trackid}}";
        public const string ApprovedURlMotor = "MotorInsurance.aspx?Ref={{RefID}}&TrackId={{trackid}}";
        public const string ApprovedURlDomesticHelp = "DomesticHelp.aspx?Ref={{RefID}}&TrackId={{trackid}}";
        public const string ApprovedURlHome = "HomeInsurance.aspx?Ref={{RefID}}&TrackId={{trackid}}";
        public const string CommercialEmail = "Commercial-quote-request";
        public const string RenewPolicy = "policy-renewal-hin-to-user";
        public const string Claims = "claims-attachment-to-admin";
        public const string SendSucessMail = "payment-success";
        public const string PolicyActivationFail = "policy-activation-fail";
        public const string PolicyInsertFailed = "PolicyInsertFailed-{insurancetype}-{documentNo}";
        public const string OraclePolicyInsertFailed = "OraclePolicyInsertFailed";
    }
    public static class HomeEndorsementTypes
    {
        public const string ChangeSumInsured = "ChangeSumInsured";
        public const string Delete = "delete";
    }   
    public static class EndorsementOpeationType
    {
        public const string Authorize = "authorize";
        public const string Delete = "delete";
    }
    public static class ReportType
    {
        public const string MotorAgeReport = "MotorAgeReport";
        public const string MotorBranchReport = "MotorBranchReport";
        public const string MotorUserReport = "MotorUserReport";
        public const string MotorVehicleReport = "MotorVehicleReport";
        public const string TravelUserReport = "TravelUserReport";
        public const string TravelBranchReport = "TravelBranchReport";
        public const string HomeUserReport = "HomeUserReport";
        public const string HomeBranchReport = "HomeBranchReport";
        public const string MotorMainReport = "MotorMainReport";
        public const string HomeMainReport = "HomeMainReport";
        public const string TravelMainReport = "TravelMainReport";
    }

    public static class VehicleBodyTypes
    {
        public const string MiniBus = "MINI BUS";
        public const string PickUp = "PICK UP";
        public const string Bus = "BUS";
        public const string Truck = "TRUCK";
        public const string SixWheel = "6-WHEEL";
        public const string Coupe = "COUPE";
        public const string Sport = "SPORT";
        public const string Saloon = "SALOON";
        public const string Jeep = "JEEP";
        public const string Van = "VAN";
        public const string HatchBack = "HATCH BACK";
        public const string HatchBack1 = "H-BACK";
        public const string Suv = "SUV";
    }
}