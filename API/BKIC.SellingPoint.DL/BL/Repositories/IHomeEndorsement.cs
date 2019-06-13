using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IHomeEndorsement
    {
        HomeEndorsementQuoteResponse GetHomeEndorsementQuote(HomeEndorsementQuote motorEndorsement);
        HomeEndorsementResponse PostHomeEndorsement(HomeEndorsement motorEndorsement);
        HomeEndorsementPreCheckResponse EndorsementPrecheck(HomeEndorsementPreCheckRequest request);
        HomeEndoResponse GetAllEndorsements(HomeEndoRequest request);
        HomeEndorsementOperationResponse EndorsementOperation(HomeEndorsementOperation request);
        HomeEndorsementQuoteResponse GetHomeDomesticHelpQuote(HomeEndorsementDomesticHelpQuote homeEndorsement);
    }
}
