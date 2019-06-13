using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class HomeInsuranceSP
    {
        public const string GetQuote = "CalculateHomeInsurancePremium";
        public const string PostInsurance = "SP_InsertHomePolicy";
        public const string InsertHome = "SP_InsertHome";
        public const string InsertHomeRenewal = "SP_InsertHomeRenewal";
        public const string GetSavedQuotation = "GetMyHomeSavedQuotationsByHomeID";
        public const string GetSavedQuotationByDocumentNo = "SP_GetMyHomeSavedQuotationByDocumentNumber";
        public const string UpdateHomeDetails = "SP_UpdateHomeDetails";
        public const string FetchRenewDetails = "FetchHomeRenewPolicy";
        public const string UpdateHomeRenewDetails = "UpdateHomeRenewDetails";
        public const string GetHomeAgencyPolicy = "SP_GetHomeAgencyPolicy";
        public const string GetHomeAgencyPolicyByCPR = "SP_GetHomeAgencyPolicyByCPR";
        public const string GetHomeEndorsementCommission = "SP_HomeCalculateEndorsementCommission";
        public const string GetHomeCommission = "SP_Admin_HomeCalculateCommission";
        public const string GetRenewalHomeByDocumentNo = "SP_GetRenewalHomeByDocumentNo";
        public const string GetHomePoliciesEndorsement = "SP_GETHomePoliciesEndorsement";
        public const string GetOracleHomeRenewalPolicies = "SP_GetOracleHomeRenewalPolicies";
    }
}
