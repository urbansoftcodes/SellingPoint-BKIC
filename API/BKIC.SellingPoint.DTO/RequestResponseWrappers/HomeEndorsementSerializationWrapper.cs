using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class HomeEndorsementQuote
    {
        [JsonProperty(PropertyName = "endorsementType")]
        public string EndorsementType { get; set; }

        [JsonProperty(PropertyName = "effectiveFromDate")]
        public DateTime EffectiveFromDate { get; set; }

        [JsonProperty(PropertyName = "effectiveToDate")]
        public DateTime EffectiveToDate { get; set; }

        [JsonProperty(PropertyName = "cancelationDate")]
        public DateTime CancelationDate { get; set; }

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

        [JsonProperty(PropertyName = "newSumInsured")]
        public decimal NewSumInsured { get; set; }

        [JsonProperty(PropertyName = "refundType")]
        public string RefundType { get; set; }

        [JsonProperty(PropertyName = "documentNumber")]
        public string DocumentNumber { get; set; }

        [JsonProperty(PropertyName = "buildingSumInsured")]
        public decimal BuildingSumInsured { get; set; }

        [JsonProperty(PropertyName = "contentSumInsured")]
        public decimal ContentSumInsured { get; set; }

        [JsonProperty(PropertyName = "noOfDomesticHelp")]
        public int NoOfDomesticHelp { get; set; }

        [JsonProperty(PropertyName = "jewelleryCoverType")]
        public string jewelleryCoverType { get; set; }
    }

    public class HomeEndorsementQuoteResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "endorsementPremium")]
        public decimal EndorsementPremium { get; set; }

        [JsonProperty(PropertyName = "refundPremium")]
        public decimal RefundPremium { get; set; }

        [JsonProperty(PropertyName = "commision")]
        public decimal Commission { get; set; }

        [JsonProperty(PropertyName = "refundVat")]
        public decimal RefundVat{ get; set; }
    }

    public class HomeEndorsementDomesticHelpQuote : TransactionWrapper
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "documentNumber")]
        public string DocumentNumber { get; set; }

        [JsonProperty(PropertyName = "doemsticHelp")]
        public List<HomeDomesticHelp> Domestichelp { get; set; }

        [JsonProperty(PropertyName = "isRiotAdded")]
        public bool IsRiotAdded { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }
    }

    public class HomeEndorsement : TransactionWrapper
    {
        [JsonProperty(PropertyName = "homeID")]
        public long HomeID { get; set; }

        [JsonProperty(PropertyName = "homeEndorsementID")]
        public long HomeEndorsementID { get; set; }

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

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "endorsementType")]
        public String EndorsementType { get; set; }

        [JsonProperty(PropertyName = "policyCommencementDate")]
        public DateTime PolicyCommencementDate { get; set; }

        [JsonProperty(PropertyName = "expiryDate")]
        public DateTime ExpiryDate { get; set; }

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

        [JsonProperty(PropertyName = "newSumInsured")]
        public decimal NewSumInsured { get; set; }

        [JsonProperty(PropertyName = "refundType")]
        public string RefundType { get; set; }

        [JsonProperty(PropertyName = "homeDomesticHelp")]
        public List<HomeDomesticHelp> HomeDomesticHelp { get; set; }

        [JsonProperty(PropertyName = "area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "noOfFloors")]
        public int NoOfFloors { get; set; }

        [JsonProperty(PropertyName = "buildingType")]
        public int BuildingType { get; set; }

        [JsonProperty(PropertyName = "houseNo")]
        public string HouseNo { get; set; }

        [JsonProperty(PropertyName = "buildingNo")]
        public string BuildingNo { get; set; }

        [JsonProperty(PropertyName = "flatNo")]
        public string FlatNo { get; set; }

        [JsonProperty(PropertyName = "roadNo")]
        public string RoadNo { get; set; }

        [JsonProperty(PropertyName = "blockNo")]
        public string BlockNo { get; set; }

        [JsonProperty(PropertyName = "residanceTypeCode")]
        public string ResidanceTypeCode { get; set; }

        [JsonProperty(PropertyName = "buildingAge")]
        public int BuildingAge { get; set; }

        [JsonProperty(PropertyName = "sumInsuredType")]
        public string SumInsuredType { get; set; }

        [JsonProperty(PropertyName = "buildingSumInsured")]
        public decimal BuildingSumInsured { get; set; }

        [JsonProperty(PropertyName = "contentSumInsured")]
        public decimal ContentSumInsured { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }

        [JsonProperty(PropertyName = "taxOnPremium")]
        public decimal TaxOnPremium { get; set; }

        [JsonProperty(PropertyName = "isRiotStrikeDamage")]
        public string IsRiotStrikeDamage { get; set; }

        [JsonProperty(PropertyName = "JewellerySumInsured")]
        public decimal JewellerySumInsured { get; set; }
       
    }

    public class HomeEndorsementResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "homeEndorsementID")]
        public long HomeEndorsementID { get; set; }       

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "endorsementNo")]
        public string EndorsementNo { get; set; }

        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "endorsementCount")]
        public int EndorsementCount { get; set; }
    }
    public class HomeEndorsementPreCheckRequest
    {
        [JsonProperty(PropertyName = "docNo")]
        public string DocNo { get; set; }
    }

    public class HomeEndorsementPreCheckResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "docNo")]
        public string EndorsementNo { get; set; }

        [JsonProperty(PropertyName = "isAlreadyHave")]
        public bool IsAlreadyHave { get; set; }
    }

    public class HomeEndoRequest
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

    public class HomeEndoResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "homeEndorsements")]
        public List<HomeEndorsement> HomeEndorsements { get; set; }

        public HomeEndoResponse()
        {
            HomeEndorsements = new List<HomeEndorsement>();
        }
    }

    public class HomeEndorsementOperation
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "homeID")]
        public long HomeID { get; set; }

        [JsonProperty(PropertyName = "homeEndorsementID")]
        public long HomeEndorsementID { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }
    }

    public class HomeEndorsementOperationResponse : TransactionWrapper
    {
    }
}