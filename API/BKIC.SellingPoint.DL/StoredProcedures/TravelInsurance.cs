using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class TravelInsuranceSP
    {
        public const string GetQuote = "SP_CalculatePremiumTravel";
        public const string PostPolicyDetails = "SP_InsertTravelPolicyDetails";
        public const string GetSavedTravelQuotation = "SP_GetMyTravelSavedQuotationsByTravelId";
        public const string GetSavedQuotationByDocumentNo = "SP_GetMyTravelSavedQuotationByDocumentNo";
        public const string UpdatePolicyDetails = "SP_UpdateTravelPolicy";
        public const string FetchSumInsured = "SP_FetchSumInsuredType";
        public const string GetAgencyPolicy = "SP_GetAgencyPolicy";     
        public const string RenewalPrecheck = "Web_RenewalPolicyPrecheck";
        public const string GetTravelPoliciesEndorsement = "SP_GETTravelPoliciesEndorsement";

    }
}
