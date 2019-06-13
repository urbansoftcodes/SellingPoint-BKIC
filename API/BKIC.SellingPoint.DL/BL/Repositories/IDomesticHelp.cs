using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IDomesticHelp
    {
        DomesticHelpQuoteResponse GetDomesticHelpQuote(DomesticHelpQuote pQuoteInputs);
        DomesticHelpPolicyResponse PostDomesticPolicy(DomesticPolicyDetails domestic);
        DomesticHelpSavedQuotationResponse GetSavedDomesticHelp(int domesticID, string insuredCode);
        AgencyDomesticPolicyResponse GetDomesticAgencyPolicy(AgencyDomesticRequest req);       
        DomesticHelpSavedQuotationResponse GetSavedDomesticPolicy(string documentNo, string agentCode,
                                                    bool isEndorsement = false, long endorsementID = 0);

    }
}
