namespace BKIC.SellingPoint.DTO.Constants
{
    public class MotorURI
    {
        public const string GetQuote = "api/motorinsurance/getquote";
        public const string GetPremium = "api/motorinsurance/getpremium";
        public const string PostMotorPolicy = "api/motorinsurance/postMotorPolicy";
        public const string GetExcessAmount = "api/motorinsurance/getexcessamount";
        public const string GetSavedQuotation = "api/motorinsurance/getsavedquotationdetails/{motorID}/{insuredCode}";
        public const string UpdateMotorDetails = "api/motorinsurance/updatemotordetails";
        public const string QuickRenewal = "api/motorinsurance/quickrenewal/{policyNo}/{cpr}";
        public const string RenewPolicy = "api/motorinsurance/renewPolicy";
        public const string RenewPrecheck = "api/motorinsurance/renewprecheck";
        public const string migraterenewpolicy = "api/motorinsurance/migraterenewpolicy/{insuranceType}/{documentNo}";
        public const string FetchInsuranceCertificate = "api/motorinsurance/fetchmotorcertificate/{documentNo}/{type}/{agentCode}/{isEndorsement}/{endorsementID}/{renewalCount}";
        public const string GetSavedQuoteDocumentNo = "api/motor/GetSavedQuotation/{documentNo}/{type}/{agentCode}/{isendorsement}/{endorsementid}/{renewalCount}";
        public const string GetMotorAgencyPolicy = "api/motor/GetMotorAgencyPolicy";
        public const string GetMotorPoliciesByTypeByCPR = "api/motor/getmotorpoliciesbytypebyCPR";
        public const string GetOptionalCover = "api/motorinsurance/getoptionalcover";
        public const string CalculateOptionalCoverAmount = "api/motorinsurance/calculateoptionalcoveramount";
        public const string GetOracleMotorRenewalPolicyByDocNo = "api/motor/getoraclemotorrenewalbydocumentno/{documentNo}/{agency}/{agentCode}";
        public const string GetMotorPoliciesByDocumentNo = "api/motor/getmotorpoliciesbytypebydocumentno";
        public const string GetMotorRenewalPolicyByDocNo = "api/motor/getmotorrenewalbydocumentno/{documentNo}/{agency}/{agentCode}/{renewalCount}";
        public const string GetMotorPoliciesEndorsement = "api/motor/getmotorpoliciesendorsement";
        public const string GetOracleMotorRenewalPolicies = "api/motor/GetOracleMotorRenewalPolicies";
    }
}