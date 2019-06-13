using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class TravelEndorsementSP
    {
        public const string GetQuote = "CalculateTravelEndorsement";
        public const string PostTravelEndorsement = "SP_InsertTravelEndorsement";       
        public const string GetTravelEndorsementByDocNo = "SP_GetTravelEndorsementByDocumentNo";
        public const string TravelEndorsementOperation = "SP_TravelEndorsementOperation";
    }
}
