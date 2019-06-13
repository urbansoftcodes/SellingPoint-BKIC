using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class MotorEndorsementQuote : MotorInsurance
    {
        [JsonProperty(PropertyName = "endorsementType")]
        public string EndorsementType { get; set; }

        [JsonProperty(PropertyName = "effectiveFromDate")]
        public DateTime EffectiveFromDate { get; set; }

        [JsonProperty(PropertyName = "effectiveToDate")]
        public DateTime EffectiveToDate { get; set; }

        [JsonProperty(PropertyName = "cancelationDate")]
        public DateTime CancelationDate { get; set; }

        [JsonProperty(PropertyName = "extendedDays")]
        public int ExtendedDays { get; set; }

        [JsonProperty(PropertyName = "paidPremium")]
        public decimal PaidPremium { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "newInsuredCode")]
        public string NewInsuredCode { get; set; }

        [JsonProperty(PropertyName = "newPremium")]
        public decimal NewPremium { get; set; }

        [JsonProperty(PropertyName = "newSumInsured")]
        public decimal NewSumInsured { get; set; }

        [JsonProperty(PropertyName = "refundType")]
        public string RefundType { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "oldSumInsured")]
        public decimal OldSumInsured { get; set; }

    }

    public class MotorEndorsementQuoteResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "endorsementPremium")]
        public decimal EndorsementPremium { get; set; }

        [JsonProperty(PropertyName = "refundPremium")]
        public decimal RefundPremium { get; set; }

        [JsonProperty(PropertyName = "commision")]
        public decimal Commision { get; set; }

        [JsonProperty(PropertyName = "refundVat")]
        public decimal RefundVat { get; set; }
    }

    public class MotorEndorsement : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorID")]
        public long MotorID { get; set; }

        [JsonProperty(PropertyName = "motorEndorsementID")]
        public long MotorEndorsementID { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agencyCode")]
        public string AgencyCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public String DocumentNo { get; set; }

        [JsonProperty(PropertyName = "endorsementNo")]
        public String EndorsementNo { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "oldInsuredCode")]
        public string OldInsuredCode { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "oldInsuredName")]
        public string OldInsuredName { get; set; }

        [JsonProperty(PropertyName = "registrationNo")]
        public string RegistrationNo { get; set; }

        [JsonProperty(PropertyName = "oldRegistrationNo")]
        public string OldRegistrationNo { get; set; }

        [JsonProperty(PropertyName = "chassisNo")]
        public string ChassisNo { get; set; }

        [JsonProperty(PropertyName = "oldChassisNo")]
        public string OldChassisNo { get; set; }

        [JsonProperty(PropertyName = "endorsementType")]
        public String EndorsementType { get; set; }

        [JsonProperty(PropertyName = "policyCommencementDate")]
        public DateTime PolicyCommencementDate { get; set; }

        [JsonProperty(PropertyName = "expiryDate")]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty(PropertyName = "vehicleValue")]
        public decimal VehicleValue { get; set; }

        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [JsonProperty(PropertyName = "mainclass")]
        public string Mainclass { get; set; }

        [JsonProperty(PropertyName = "subclass")]
        public string Subclass { get; set; }

        [JsonProperty(PropertyName = "extendedExpireDate")]
        public DateTime? ExtendedExpireDate { get; set; }

        [JsonProperty(PropertyName = "cancelDate")]
        public DateTime? CancelDate { get; set; }

        [JsonProperty(PropertyName = "financierCompanyCode")]
        public string FinancierCompanyCode { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "isSaved")]
        public bool IsSaved { get; set; }

        [JsonProperty(PropertyName = "paymentDate")]
        public DateTime? PaymentDate { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "userChangedPremium")]
        public bool UserChangedPremium { get; set; }

        [JsonProperty(PropertyName = "premiumAfterDiscount")]
        public decimal PremiumAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "premiumBeforeDiscount")]
        public decimal PremiumBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "refundAmount")]
        public decimal RefundAmount { get; set; }

        [JsonProperty(PropertyName = "refundAfterDiscount")]
        public decimal RefundAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "commisonBeforeDiscount")]
        public decimal CommisionBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "commisonAfterDiscount")]
        public decimal CommissionAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "isActivePolicy")]
        public bool IsActivePolicy { get; set; }

        [JsonProperty(PropertyName = "isAddBank")]
        public bool IsAddBank { get; set; }

        [JsonProperty(PropertyName = "isRemoveBank")]
        public bool IsRemoveBank { get; set; }


        [JsonProperty(PropertyName = "oldPremium")]
        public decimal OldPremium{ get; set; }

        [JsonProperty(PropertyName = "newPremium")]
        public decimal NewPremium { get; set; }


        [JsonProperty(PropertyName = "oldSumInsured")]
        public decimal OldSumInsured { get; set; }

        [JsonProperty(PropertyName = "newSumInsured")]
        public decimal NewSumInsured { get; set; }

        [JsonProperty(PropertyName = "oldExcess")]
        public decimal OldExcess { get; set; }

        [JsonProperty(PropertyName = "newExcess")]
        public decimal NewExcess { get; set; }

        [JsonProperty(PropertyName = "covers")]
        public List<MotorCovers> Covers { get; set; }

        [JsonProperty(PropertyName = "refundType")]
        public string RefundType { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }

        [JsonProperty(PropertyName = "taxOnPremium")]
        public decimal TaxOnPremium { get; set; }

        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }

        [JsonProperty(PropertyName = "vehicleModel")]
        public string VehicleModel { get; set; }

        [JsonProperty(PropertyName = "vehicleBodyType")]
        public string VehicleBodyType { get; set; }

        [JsonProperty(PropertyName = "vehicleYear")]
        public int VehicleYear { get; set; }

        [JsonProperty(PropertyName = "engineCC")]
        public int EngineCC { get; set; }


    }

    public class MotorEndorsementResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorEndorsementID")]
        public long MotorEndorsementID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "endorsementNo")]
        public string EndorsementNo { get; set; }

        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "endorsementCount")]
        public int EndorsementCount { get; set; }
    }

    public class MotorEndorsementPreCheckRequest
    {
        [JsonProperty(PropertyName = "docNo")]
        public string DocNo { get; set; }
    }

    public class MotorEndorsementPreCheckResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "docNo")]
        public string EndorsementNo { get; set; }

        [JsonProperty(PropertyName = "isAlreadyHave")]
        public bool IsAlreadyHave { get; set; }
    }

    public class MotorEndoRequest
    {
        [JsonProperty(PropertyName = "DocumentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "Agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "AgentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
    }

    public class MotorEndoResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "MotorEndorsements")]
        public List<MotorEndorsement> MotorEndorsements { get; set; }

        public MotorEndoResult()
        {
            MotorEndorsements = new List<MotorEndorsement>();
        }
    }

    public class MotorEndorsementOperation
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "motorID")]
        public long MotorID { get; set; }

        [JsonProperty(PropertyName = "motorEndorsementID")]
        public long MotorEndorsementID { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }
    }

    public class MotorEndorsementOperationResponse : TransactionWrapper
    {
    }
}