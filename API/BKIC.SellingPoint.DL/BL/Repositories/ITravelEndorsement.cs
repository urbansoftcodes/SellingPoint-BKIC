using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface ITravelEndorsement
    {
        TravelEndorsementQuoteResponse GetTravelEndorsementQuote(TravelEndorsementQuote motorEndorsement);
        TravelEndorsementResponse PostTravelEndorsement(TravelEndorsement motorEndorsement);
        TravelEndorsementPreCheckResponse EndorsementPrecheck(TravelEndorsementPreCheckRequest request);
        TravelEndoResponse GetAllEndorsements(TravelEndoRequest request);
        TravelEndorsementOperationResponse EndorsementOperation(TravelEndorsementOperation request);
    }
}
