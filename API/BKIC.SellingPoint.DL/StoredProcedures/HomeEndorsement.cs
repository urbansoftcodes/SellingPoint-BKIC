using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class HomeEndorsementSP
    {
        public const string GetQuote = "CalculateHomeEndorsement";
        public const string GetHomeCancelQuote = "CalculateHomeCancelEndorsement";
        public const string PostHomeEndorsement = "SP_InsertHomeEndorsement";       
        public const string GetHomeEndorsementByDocNo = "SP_GetHomeEndorsementByDocumentNo";
        public const string HomeEndorsementOperation = "SP_HomeEndorsementOperation";
        public const string GetHomeDomesticHelpQuote = "SP_CalculateHomeDomesticMembers";       
    }
}
