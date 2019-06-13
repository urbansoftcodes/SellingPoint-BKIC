namespace BKIC.SellingPoint.DTO.Constants
{
    public class TravelInsuranceURI
    {
        public const string GetQuote = "api/travelinsurance/getquote";
        public const string PostTravel = "api/travelinsurance/posttravelpolicy";
        public const string GetSavedQuotation = "api/travelinsurance/gettravelsavedquotation/{travelQuotationId}/{userInsuredCode}/{type}";
        public const string UpdatePolicyDetails = "api/travelinsurance/updatetraveldetails";
        public const string FetchSumInsured = "api/travelinsurance/fetchsuminsured/{insuranceType}";
        public const string GetAgencyPolicy = "api/travelinsurance/getAgencyPolicy";
        public const string RenewPrecheck = "api/travelinsurance/renewprecheck";
        public const string GetPolicyExpirtyDate = "api/travelinsurance/GetPolicyExpirtyDate";
        public const string GetSavedQuoteDocumentNo = "api/travel/GetSavedQuotation/{documentNo}/{type}/{agentCode}/{isendorsement}/{endorsementid}";
        public const string GetTravelPoliciesByCPR = "api/travel/gettravelpoliciesbytypebyCPR";
        public const string GetTravelPoliciesEndorsement = "api/travel/gettravelpoliciesendorsement";        
    }
}