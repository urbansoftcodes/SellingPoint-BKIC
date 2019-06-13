using BKIC.SellingPoint.DL.BO;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface ITravelInsurance
    {
        TravelInsuranceQuoteResponse GetTravelInsuranceQuote(TravelInsuranceQuote quote);
        TravelPolicyResponse PostTravelPolicyDetails(TravelPolicy travel);
        TravelSavedQuotationResponse GetSavedQuotationByTravelId(int travelQuotationId, string userInsuredCode, string type);        
        UpdateTravelDetailsResponse UpdateTravelDetails(UpdateTravelDetailsRequest travel);
        //FetchSumInsuredResponse FetchSumInsuredAmount(string insuranceType);
        AgencyTravelPolicyResponse GetTravelAgencyPolicy(AgencyTravelRequest req);
        RenewPrecheckResponse RenewalPrecheck(string documentNo, string cpr, string type);
        AgencyTravelPolicyResponse GetTravelAgencyPolicyByCPR(AgencyTravelRequest req);
        TravelSavedQuotationResponse GetSavedQuotationByPolicy(string documentNo, string type, string agentCode,
                                                               bool isEndorsement = false, long endorsementID = 0);
        AgencyTravelPolicyResponse GetTravelPoliciesEndorsement(AgencyTravelRequest req);
    }
}