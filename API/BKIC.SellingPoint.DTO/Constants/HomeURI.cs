namespace BKIC.SellingPoint.DTO.Constants
{
    public class HomeURI
    {
        public const string GetQuote = "api/homeinsurance/getquote";
        public const string PostPolicy = "api/homeinsurance/postpolicy";
        public const string GeSavedQuotation = "api/homeinsurance/GetSavedQuotation/{homeID}/{insuredCode}";
        public const string UpdatePolicyDetails = "api/homeinsurance/updatepolicydetails";
        public const string FetchHomeRenewalDetails = "api/homeinsurance/fetchhomerenewal/{cpr}/{documentNo}";
        public const string PolicyRenew = "api/homeinsurance/renewhomepolicy";
        public const string GetHomeSavedQuoteDocumentNo = "api/home/GetHomeSavedQuotation/{documentNo}/{type}/{agentCode}/{isendorsement}/{endorsementid}/{renewalCount}";
        public const string GetHomeAgencyPolicy = "api/home/GetHomeAgencyPolicy";
        public const string GetHomePoliciesByCPR = "api/home/gethomepoliciesbytypebyCPR";
        public const string GetHomeRenewalPolicyByDocNo = "api/home/GetHomeRenewal/{documentNo}/{type}/{agentCode}/{renewalCount}";
        public const string GetOracleHomeRenewalPolicyByDocNo = "api/home/getoraclehomerenewalbydocumentno/{documentNo}/{agency}/{agentCode}";
        public const string GetHomePoliciesEndorsement = "api/home/gethomepoliciesendorsement";
        public const string GetOracleHomeRenewalPolicies = "api/home/GetOracleHomeRenewalPolicies";
        
    }
}