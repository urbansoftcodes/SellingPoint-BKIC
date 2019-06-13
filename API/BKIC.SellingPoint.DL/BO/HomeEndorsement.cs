using System;
using System.Collections.Generic;
using System.Data;

namespace BKIC.SellingPoint.DL.BO
{
    public class HomeEndorsementQuote : TransactionWrapper
    {
        public string EndorsementType { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public DateTime CancelationDate { get; set; }
        public decimal PaidPremium { get; set; }
        public decimal NewSumInsured { get; set; }
        public string RefundType { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string DocumentNumber { get; set; }
        public decimal BuildingSumInsured { get; set; }
        public decimal ContentSumInsured { get; set; }
        public int NoOfDomesticHelp { get; set; }
        public string jewelleryCoverType { get; set; }
    }

    public class HomeEndorsementQuoteResponse : TransactionWrapper
    {
        public decimal EndorsementPremium { get; set; }
        public decimal RefundPremium { get; set; }
        public decimal Commission { get; set; }
        public decimal RefundVat { get; set; }
    }

    public class HomeEndorsementDomesticHelpQuote : TransactionWrapper
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string DocumentNumber { get; set; }
        public List<HomeDomesticHelp> Domestichelp { get; set; }
        public bool IsRiotAdded { get; set; }
        public int RenewalCount { get; set; }
    }

    public class HomeEndorsement : TransactionWrapper
    {
        public long HomeID { get; set; }
        public long HomeEndorsementID { get; set; }
        public string Agency { get; set; }
        public string AgencyCode { get; set; }
        public string AgentBranch { get; set; }
        public String DocumentNo { get; set; }
        public String EndorsementNo { get; set; }
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public String EndorsementType { get; set; }
        public DateTime PolicyCommencementDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public string Mainclass { get; set; }
        public string Subclass { get; set; }
        public DateTime? ExtendedExpireDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public string FinancierCompanyCode { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsSaved { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string Source { get; set; }
        public string AccountNumber { get; set; }
        public string Remarks { get; set; }
        public bool UserChangedPremium { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
        public decimal RefundAmount { get; set; }
        public decimal RefundAfterDiscount { get; set; }
        public decimal CommisionBeforeDiscount { get; set; }
        public decimal CommissionAfterDiscount { get; set; }
        public bool IsActivePolicy { get; set; }
        public decimal NewSumInsured { get; set; }
        public string RefundType { get; set; }
        public List<HomeDomesticHelp> HomeDomesticHelp { get; set; }
        public string Area { get; set; }
        public int NoOfFloors { get; set; }
        public int BuildingType { get; set; }
        public string HouseNo { get; set; }
        public string BuildingNo { get; set; }
        public string FlatNo { get; set; }
        public string RoadNo { get; set; }
        public string BlockNo { get; set; }
        public string ResidanceTypeCode { get; set; }
        public int BuildingAge { get; set; }        
        public string SumInsuredType { get; set; }       
        public decimal BuildingSumInsured { get; set; }       
        public decimal ContentSumInsured { get; set; }       
        public int RenewalCount { get; set; }       
        public decimal TaxOnPremium { get; set; }
        public string IsRiotStrikeDamage { get; set; }
        public decimal JewellerySumInsured { get; set; }
    }

    public class HomeEndorsementResponse : TransactionWrapper
    {
        public long HomeEndorsementID { get; set; }        

        public string DocumentNo { get; set; }

        public string EndorsementNo { get; set; }

        public string LinkID { get; set; }

        public int EndorsementCount { get; set; }
    }

    public class HomeEndorsementPreCheckRequest
    {
        public string DocNo { get; set; }
    }

    public class HomeEndorsementPreCheckResponse : TransactionWrapper
    {
        public string EndorsementNo { get; set; }

        public bool IsAlreadyHave { get; set; }
    }

    public class HomeEndoRequest
    {
        public string DocumentNo { get; set; }

        public string Agency { get; set; }

        public string AgentCode { get; set; }

        public string InsuranceType { get; set; }
    }

    public class HomeEndoResponse : TransactionWrapper
    {
        public List<HomeEndorsement> HomeEndorsements { get; set; }

        public HomeEndoResponse()
        {
            HomeEndorsements = new List<HomeEndorsement>();
        }
    }

    public class HomeEndorsementOperation
    {
        public string Type { get; set; }

        public long HomeID { get; set; }

        public long HomeEndorsementID { get; set; }

        public string Agency { get; set; }

        public string AgentCode { get; set; }
        
        public int UpdatedBy { get; set; }
    }

    public class HomeEndorsementOperationResponse : TransactionWrapper
    {
    }
}