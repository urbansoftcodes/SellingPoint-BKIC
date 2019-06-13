using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DTO.Constants
{
    public class DomesticURI
    {
        public const string GetQuote = "api/domestichelp/getquote";
        public const string PostQuote = "api/domestichelp/postDomesticPolicy";
        public const string GetSavedQuote = "api/domestichelp/GetSavedQuotation/{domesticID}/{insuredCode}";
        public const string UpdateDomesticDetails = "api/domestichelp/updateDomesticDetails";
        public const string GetDomesticAgencyPolicy = "api/domestichelp/GetDomesticAgencyPolicy";
        public const string RenewPrecheck = "api/domestichelp/renewprecheck";
        public const string GetSavedQuoteDocumentNo = "api/domestichelp/GetSavedQuotationByDocumentNo/{documentNo}/{agentCode}";
    }
}
