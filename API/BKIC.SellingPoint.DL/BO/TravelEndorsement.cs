using System;
using System.Collections.Generic;

namespace BKIC.SellingPoint.DL.BO
{
    public class TravelEndorsementQuote : TransactionWrapper
    {
        public string EndorsementType { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public DateTime CancelationDate { get; set; }
        public decimal PaidPremium { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public decimal NewSumInsured { get; set; }
        public string RefundType { get; set; }
        public string PolicyPeriodName { get; set; }
        public string DocumentNo { get; set; }
    }

    public class TravelEndorsementQuoteResponse : TransactionWrapper
    {
        public decimal EndorsementPremium { get; set; }
        public decimal RefundPremium { get; set; }
        public decimal Commision { get; set; }
    }

    public class TravelEndorsement : TransactionWrapper
    {
        public long TravelID { get; set; }
        public long TravelEndorsementID { get; set; }
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
        public decimal NewPremium { get; set; }
        public List<TravelMembers> TravelMembers { get; set; }
        public string RefundType { get; set; }
        public string PolicyPeriodName { get; set; }
    }

    public class TravelEndorsementResponse : TransactionWrapper
    {
        public long TravelEndorsementID { get; set; }
        public bool IsHIR { get; set; }
        public string DocumentNo { get; set; }
        public string EndorsementNo { get; set; }
    }

    public class TravelEndorsementPreCheckRequest
    {
        public string DocNo { get; set; }
    }

    public class TravelEndorsementPreCheckResponse : TransactionWrapper
    {
        public string EndorsementNo { get; set; }
        public bool IsAlreadyHave { get; set; }
    }

    public class TravelEndoRequest
    {
        public string DocumentNo { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string InsuranceType { get; set; }
    }

    public class TravelEndoResponse : TransactionWrapper
    {
        public List<TravelEndorsement> TravelEndorsements { get; set; }

        public TravelEndoResponse()
        {
            TravelEndorsements = new List<TravelEndorsement>();
        }
    }

    public class TravelEndorsementOperation
    {
        public string Type { get; set; }
        public long TravelID { get; set; }
        public long TravelEndorsementID { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class TravelEndorsementOperationResponse : TransactionWrapper
    {
    }
}