using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class AgencyHomeRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "includeHIR")]
        public bool IncludeHIR { get; set; }

        [JsonProperty(PropertyName = "isRenewal")]
        public bool IsRenewal { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
    }

    public class AgencyHomePolicy
    {
        [JsonProperty(PropertyName = "homeId")]
        public long HomeID { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "policyStartDate")]
        public DateTime PolicyStartDate { get; set; }

        [JsonProperty(PropertyName = "policyEndDate")]
        public DateTime PolicyEndDate { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }

        [JsonProperty(PropertyName = "documentRenewalNo")]
        public string DocumentRenewalNo { get; set; }
    }

    public class AgencyHomePolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "agencyHomePolicies")]
        public List<AgencyHomePolicy> AgencyHomePolicies { get; set; }

        public AgencyHomePolicyResponse()
        {
            AgencyHomePolicies = new List<AgencyHomePolicy>();
        }
    }

    public class HomeInsuranceQuote
    {
        [JsonProperty(PropertyName = "buildingValue")]
        public decimal BuildingValue { get; set; }

        [JsonProperty(PropertyName = "contentValue")]
        public decimal ContentValue { get; set; }

        [JsonProperty(PropertyName = "jewelleryValue")]
        public decimal JewelleryValue { get; set; }

        [JsonProperty(PropertyName = "isPropertyToBeInsured")]
        public bool IsPropertyToBeInsured { get; set; }

        [JsonProperty(PropertyName = "jewelleryCover")]
        public string JewelleryCover { get; set; }

        [JsonProperty(PropertyName = "isRiotStrikeAdded")]
        public bool IsRiotStrikeAdded { get; set; }

        [JsonProperty(PropertyName = "numberOfDomesticWorker")]
        public int NumberOfDomesticWorker { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "renewalDelayedDays")]
        public int RenewalDelayedDays { get; set; }
    }

    public class HomeInsuranceQuoteResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "totalPremium")]
        public decimal TotalPremium { get; set; }

        [JsonProperty(PropertyName = "totalCommission")]
        public decimal TotalCommission { get; set; }
    }

    public class HomeInsurancePolicy : UserDetails
    {
        [Required(ErrorMessage = "agency is required")]
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [Required(ErrorMessage = "agent code is required")]
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [Required(ErrorMessage = "agent branch is required")]
        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [Required(ErrorMessage = "policy start date is required")]
        [JsonProperty(PropertyName = "policyStartDate")]
        public DateTime PolicyStartDate { get; set; }

        [Required(ErrorMessage = "building value is required")]
        [JsonProperty(PropertyName = "buildingValue")]
        public decimal BuildingValue { get; set; }

        [Required(ErrorMessage = "content value is required")]
        [JsonProperty(PropertyName = "contentValue")]
        public decimal ContentValue { get; set; }

        [JsonProperty(PropertyName = "jewelleryValue")]
        public decimal JewelleryValue { get; set; }

        [Required(ErrorMessage = "isSafePropertyInsured is required")]
        [JsonProperty(PropertyName = "isSafePropertyInsured")]
        public char IsSafePropertyInsured { get; set; }

        [Required(ErrorMessage = "isPropertyMortgaged value is required")]
        [JsonProperty(PropertyName = "isPropertyMortgaged")]
        public char IsPropertyMortgaged { get; set; }

        [Required(ErrorMessage = "isJointOwnership is required")]
        [JsonProperty(PropertyName = "isJointOwnership")]
        public char IsJointOwnership { get; set; }

        [Required(ErrorMessage = "isPropertyInConnectionTrade is required")]
        [JsonProperty(PropertyName = "isPropertyInConnectionTrade")]
        public char IsPropertyInConnectionTrade { get; set; }

        [Required(ErrorMessage = "isPropertyCoveredOtherInsurance is required")]
        [JsonProperty(PropertyName = "isPropertyCoveredOtherInsurance")]
        public char IsPropertyCoveredOtherInsurance { get; set; }

        [Required(ErrorMessage = "isSaved is required")]
        [JsonProperty(PropertyName = "isSaved")]
        public bool IsSaved { get; set; }

        [Required(ErrorMessage = "isActivePolicy is required")]
        [JsonProperty(PropertyName = "isActivePolicy")]
        public bool IsActivePolicy { get; set; }

        [JsonProperty(PropertyName = "homeID")]
        public long HomeID { get; set; }

        [JsonProperty(PropertyName = "isPropertyUndergoingConstruction")]
        public char IsPropertyUndergoingConstruction { get; set; }

        [JsonProperty(PropertyName = "premiumAfterDiscount")]
        public decimal PremiumAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "premiumBeforeDiscount")]
        public decimal PremiumBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "buildingAge")]
        public int BuildingAge { get; set; }

        [JsonProperty(PropertyName = "financierCode")]
        public string FinancierCode { get; set; }

        [JsonProperty(PropertyName = "jewelleryCover")]
        public string JewelleryCover { get; set; }

        [JsonProperty(PropertyName = "jewelleryCoverType")]
        public string JewelleryCoverType { get; set; }

        [JsonProperty(PropertyName = "isRiotStrikeDamage")]
        public char IsRiotStrikeDamage { get; set; }

        [JsonProperty(PropertyName = "jointOwnerName")]
        public string JointOwnerName { get; set; }

        [JsonProperty(PropertyName = "isPropertyInsuredSustainedAnyLoss")]
        public char IsPropertyInsuredSustainedAnyLoss { get; set; }

        [JsonProperty(PropertyName = "namePolicyReasonSeekingReasons")]
        public string NamePolicyReasonSeekingReasons { get; set; }

        [JsonProperty(PropertyName = "isSingleItemAboveContents")]
        public char IsSingleItemAboveContents { get; set; }

        [JsonProperty(PropertyName = "buildingNo")]
        public string BuildingNo { get; set; }

        [JsonProperty(PropertyName = "flatNo")]
        public string FlatNo { get; set; }

        [JsonProperty(PropertyName = "roadNo")]
        public string RoadNo { get; set; }

        [JsonProperty(PropertyName = "area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "noOfFloors")]
        public int NoOfFloors { get; set; }

        [JsonProperty(PropertyName = "buildingType")]
        public int BuildingType { get; set; }

        [JsonProperty(PropertyName = "houseNo")]
        public string HouseNo { get; set; }

        [JsonProperty(PropertyName = "blockNo")]
        public string BlockNo { get; set; }

        [JsonProperty(PropertyName = "residanceTypeCode")]
        public string ResidanceTypeCode { get; set; }

        [JsonProperty(PropertyName = "ffpNumber")]
        public string FFPNumber { get; set; }

        [JsonProperty(PropertyName = "isRequireDomestic")]
        public char IsRequireDomestic { get; set; }

        [JsonProperty(PropertyName = "noOfDomesticWorker")]
        public int NoOfDomesticWorker { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "authorizedBy")]
        public int AuthorizedBy { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "policyExpiryDate")]
        public DateTime PolicyExpiryDate { get; set; }

        [JsonProperty(PropertyName = "loadAmount")]
        public decimal? LoadAmount { get; set; }

        [JsonProperty(PropertyName = "discountAmount")]
        public decimal? DiscountAmount { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "commisonBeforeDiscount")]
        public decimal CommisionBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "commisonAfterDiscount")]
        public decimal CommissionAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "userChangedPremium")]
        public bool UserChangedPremium { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public int HIRStatus { get; set; }

        [JsonProperty(PropertyName = "endorsementCount")]
        public int EndorsementCount { get; set; }

        [JsonProperty(PropertyName = "isCancelled")]
        public bool IsCancelled { get; set; }

        [JsonProperty(PropertyName = "taxOnPremium")]
        public decimal TaxOnPremium { get; set; }

        [JsonProperty(PropertyName = "taxOnCommission")]
        public decimal TaxOnCommission { get; set; }

        [JsonProperty(PropertyName = "isRenewal")]
        public bool IsRenewal { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }

        [JsonProperty(PropertyName = "isSavedRenewal")]
        public bool IsSavedRenewal { get; set; }

        [JsonProperty(PropertyName = "oldDocumentNumber")]
        public string OldDocumentNumber { get; set; }

        [JsonProperty(PropertyName = "EndorsementType")]
        public string EndorsementType { get; set; }

        [JsonProperty(PropertyName = "actualRenewalStartDate")]
        public DateTime? ActualRenewalStartDate { get; set; }

        [JsonProperty(PropertyName = "renewalDelayedDays")]
        public int RenewalDelayedDays { get; set; }
    }

    public class HomeInsurancePolicyDetails
    {
        [JsonProperty(PropertyName = "homeInsurancePolicy")]
        public HomeInsurancePolicy HomeInsurancePolicy { get; set; }

        [JsonProperty(PropertyName = "homeSubItemsList")]
        public List<HomeSubItems> HomeSubItemsList { get; set; }

        [JsonProperty(PropertyName = "homeDomesticHelpList")]
        public List<HomeDomesticHelp> HomeDomesticHelpList { get; set; }

        public HomeInsurancePolicyDetails()
        {
            HomeInsurancePolicy = new HomeInsurancePolicy();
            HomeSubItemsList = new List<HomeSubItems>();
            HomeDomesticHelpList = new List<HomeDomesticHelp>();
        }
    }

    public class HomeSubItems
    {
        [JsonProperty(PropertyName = "homeSID")]
        public int HomeSID { get; set; }

        [JsonProperty(PropertyName = "homeID")]
        public long HomeID { get; set; }

        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "subItemSerialNo")]
        public int SubItemSerialNo { get; set; }

        [JsonProperty(PropertyName = "subItemCode")]
        public string SubItemCode { get; set; }

        [JsonProperty(PropertyName = "subItemName")]
        public string SubItemName { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedtime")]
        public DateTime Updatedtime { get; set; }
    }

    public class HomeDomesticHelp
    {
        [JsonProperty(PropertyName = "homeSID")]
        public int HomeSID { get; set; }

        [JsonProperty(PropertyName = "memberSerialNo")]
        public int MemberSerialNo { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "sex")]
        public char Sex { get; set; }

        [JsonProperty(PropertyName = "dob")]
        public DateTime DOB { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }

        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [JsonProperty(PropertyName = "nationality")]
        public string Nationality { get; set; }

        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }
    }

    public class HomeInsurancePolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "homeId")]
        public long HomeId { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "trackID")]
        public string TrackID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }
    }

    public class HomeInsuranceRatesInput
    {
        [JsonProperty(PropertyName = "cType")]
        public string CType { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "buildingRate")]
        public string BuildingRate { get; set; }

        [JsonProperty(PropertyName = "contentRate")]
        public string ContentRate { get; set; }

        [JsonProperty(PropertyName = "riotCover")]
        public string RiotCover { get; set; }

        [JsonProperty(PropertyName = "riotCoverMinimum")]
        public string RiotCoverMinimum { get; set; }

        [JsonProperty(PropertyName = "domesticHelperAmount")]
        public string DomesticHelperAmount { get; set; }

        [JsonProperty(PropertyName = "minimumPremiumAmount")]
        public string MinimumPremiumAmount { get; set; }

        [JsonProperty(PropertyName = "search")]
        public string Search { get; set; }
    }

    public class Questionaire
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "questionType")]
        public string QuestionType { get; set; }

        [JsonProperty(PropertyName = "questionDescription")]
        public string QuestionDescription { get; set; }

        [JsonProperty(PropertyName = "answers")]
        public string Answers { get; set; }
    }

    public class HomeSavedQuotationResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "homeInsurancePolicy")]
        public HomeInsurancePolicy HomeInsurancePolicy { get; set; }

        [JsonProperty(PropertyName = "domesticHelp")]
        public List<HomeDomesticHelp> DomesticHelp { get; set; }

        [JsonProperty(PropertyName = "homesubitems")]
        public List<HomeSubItems> homesubitems { get; set; }

        public HomeSavedQuotationResponse()
        {
            HomeInsurancePolicy = new HomeInsurancePolicy();
            DomesticHelp = new List<HomeDomesticHelp>();
        }
    }

    public class UpdateHomePolicyRequest
    {
        [JsonProperty(PropertyName = "HomeInsurancePolicy")]
        public HomeInsurancePolicy HomeInsurancePolicy { get; set; }

        [JsonProperty(PropertyName = "homeSubItemsList")]
        public List<HomeSubItems> HomeSubItemsList { get; set; }

        [JsonProperty(PropertyName = "homeDomesticHelpList")]
        public List<HomeDomesticHelp> HomeDomesticHelpList { get; set; }

        public UpdateHomePolicyRequest()
        {
            HomeInsurancePolicy = new HomeInsurancePolicy();
            HomeSubItemsList = new List<HomeSubItems>();
            HomeDomesticHelpList = new List<HomeDomesticHelp>();
        }
    }

    public class UpdateHomePolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "paymentTrackId")]
        public string PaymentTrackId { get; set; }
    }

    public class RenewHomePolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "paymentTrackID")]
        public string PaymentTrackID { get; set; }
    }

    public class RenewHomePolicyRequest
    {
        [JsonProperty(PropertyName = "renewID")]
        public int RenewID { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "mobileNo")]
        public string MobileNo { get; set; }

        [JsonProperty(PropertyName = "ffpNumber")]
        public string FFPNumber { get; set; }
    }

    public class RenewalHomeInsuranceResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "homeInsurancePolicy")]
        public HomeInsurancePolicy HomeInsurancePolicy { get; set; }

        [JsonProperty(PropertyName = "domesticHelp")]
        public List<HomeDomesticHelp> DomesticHelp { get; set; }

        [JsonProperty(PropertyName = "homeSubItems")]
        public List<HomeSubItems> HomeSubItems { get; set; }

        public RenewalHomeInsuranceResponse()
        {
            HomeInsurancePolicy = new HomeInsurancePolicy();
            DomesticHelp = new List<HomeDomesticHelp>();
            HomeSubItems = new List<HomeSubItems>();
        }
    }
}