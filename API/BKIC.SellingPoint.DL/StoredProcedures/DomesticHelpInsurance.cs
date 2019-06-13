using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class DomesticHelpInsuranceSP
    {
        public const string GetQuote = "SP_DomesticHelpQuote";
        public const string PostDomesticPolicy = "SP_InsertDomesticPolicy";
        public const string SavedQuotation = "SP_GetMyDomesticSavedQuotationsByDomesticId";
        public const string UpdateDomesticDetails = "SP_UpdateDomesticHelpDetails";
        public const string GetDomesticAgencyPolicy = "SP_GetDomesticAgencyPolicy";
        public const string GetSavedQuotationByDocumentNo = "SP_GetMyDomesticSavedQuotationsByDocumentNumber";
    }
}
