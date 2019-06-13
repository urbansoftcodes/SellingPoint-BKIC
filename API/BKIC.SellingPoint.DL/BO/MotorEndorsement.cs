using System;
using System.Collections.Generic;

namespace BKIC.SellingPoint.DL.BO
{
    public class MotorEndorsementQuoteResult : TransactionWrapper
    {
        public decimal EndorsementPremium { get; set; }
        public decimal RefundPremium { get; set; }
        public decimal Commision { get; set; }
        public decimal RefundVat { get; set; }
    }

    public class MotorEndorsementQuote : TransactionWrapper
    {
        public string EndorsementType { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public DateTime CancelationDate { get; set; }
        public int ExtendedDays { get; set; }
        public decimal PaidPremium { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string NewInsuredCode { get; set; }
        public decimal NewPremium { get; set; }         
        public decimal NewSumInsured { get; set; }
        public string RefundType { get; set; }
        public string InsuredCode { get; set; }
        public string CPR { get; set; }
        public string DocumentNo { get; set; }
        public decimal OldSumInsured { get; set; }

    }

    public class MotorEndorsement : TransactionWrapper
    {
        public long MotorID { get; set; }
        public long MotorEndorsementID { get; set; }
        public string Agency { get; set; }
        public string AgencyCode { get; set; }
        public string AgentBranch { get; set; }
        public String DocumentNo { get; set; }
        public String EndorsementNo { get; set; }
        public string InsuredCode { get; set; }
        public string OldInsuredCode { get; set; }
        public string InsuredName { get; set; }
        public string OldInsuredName { get; set; }
        public string RegistrationNo { get; set; }
        public string OldRegistrationNo { get; set; }
        public string ChassisNo { get; set; }
        public string OldChassisNo { get; set; }
        public string EndorsementType { get; set; }
        public DateTime PolicyCommencementDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal VehicleValue { get; set; }
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
        public bool IsAddBank { get; set; }        
        public bool IsRemoveBank { get; set; }       
        public decimal OldPremium { get; set; }       
        public decimal NewPremium { get; set; } 
        public decimal OldSumInsured { get; set; }        
        public decimal NewSumInsured { get; set; }
        public decimal OldExcess { get; set; }
        public decimal NewExcess { get; set; }      
        public List<MotorCovers> Covers { get; set; }
        public string RefundType { get; set; }
        public string CPR { get; set; }
        public int RenewalCount { get; set; }
        public decimal TaxOnPremium { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleBodyType { get; set; }
        public int VehicleYear { get; set; }
        public int EngineCC { get; set; }
    }    

    public class MotorEndorsementResult : TransactionWrapper
    {
        public long MotorEndorsementID { get; set; }        
        public string DocumentNo { get; set; }
        public string EndorsementNo { get; set; }
        public string LinkID { get; set; }
        public int EndorsementCount { get; set; }
    }

    public class MotorEndorsementPreCheckRequest
    {
        public string DocNo { get; set; }
    }

    public class MotorEndorsementPreCheckResponse : TransactionWrapper
    {
        public string EndorsementNo { get; set; }
        public bool IsAlreadyHave { get; set; }
    }

    public class MotorEndoRequest
    {
        public string DocumentNo { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string InsuranceType { get; set; }
    }

    public class MotorEndoResult : TransactionWrapper
    {
        public List<MotorEndorsement> MotorEndorsements { get; set; }

        public MotorEndoResult()
        {
            MotorEndorsements = new List<MotorEndorsement>();
        }
    }

    public class MotorEndorsementOperation
    {
        public string Type { get; set; }
        public long MotorID { get; set; }
        public long MotorEndorsementID { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class MotorEndorsementOperationResponse : TransactionWrapper
    {
    }
}