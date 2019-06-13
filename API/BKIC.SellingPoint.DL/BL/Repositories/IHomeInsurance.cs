using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IHomeInsurance
    {
        HomeInsuranceQuoteResponse GetHomeInsuranceQuote(HomeInsuranceQuote HomeInsurance);
        HomeInsurancePolicyResponse PostHomeInsurancePolicy(HomeInsurancePolicyDetails policydetails);
        HomeSavedQuotationResponse GetSavedQuotation(int homeId, string insuredCode); 
        AgencyHomePolicyResponse GetHomeAgencyPolicy(AgencyHomeRequest req);
        AgencyHomePolicyResponse GetHomeAgencyPolicyByCPR(AgencyHomeRequest req);
        HomeSavedQuotationResponse GetSavedQuotationPolicy(string documentNo, string type, string agentCode, 
                                   bool isEndorsement = false, long endorsementID = 0, int renewalCount = 0);
        HomeSavedQuotationResponse GetRenewalHomePolicy(string documentNo, string type, string agentCode, int renewalCount);
        HomeSavedQuotationResponse GetOracleRenewHomePolicy(string documentNo, string agency, string agentCode);
        AgencyHomePolicyResponse GetHomePoliciesEndorsement(AgencyHomeRequest req);
        AgencyHomePolicyResponse GetOracleHomeRenewalPolicies(AgencyHomeRequest req);
    }
}
