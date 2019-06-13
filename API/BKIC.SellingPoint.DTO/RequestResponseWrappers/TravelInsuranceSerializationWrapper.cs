using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class AgencyTravelPolicy
    {
        [JsonProperty(PropertyName = "travelId")]
        public long TravelId { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "policyStartDate")]
        public DateTime PolicyStartDate { get; set; }

        [JsonProperty(PropertyName = "policyEndDate")]
        public DateTime PolicyEndDate { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }

        [JsonProperty(PropertyName = "documentRenewalNo")]
        public string DocumentRenewalNo { get; set; }
    }

    public class AgencyTravelPolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "agencyTravelPolicies")]
        public List<AgencyTravelPolicy> AgencyTravelPolicies { get; set; }

        public AgencyTravelPolicyResponse()
        {
            AgencyTravelPolicies = new List<AgencyTravelPolicy>();
        }
    }

    public class AgencyTravelRequest
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

        [JsonProperty(PropertyName = "isEndorsement")]
        public bool isEndorsement { get; set; }

        [JsonProperty(PropertyName = "includeHIR")]
        public bool includeHIR { get; set; }

        [JsonProperty(PropertyName = "isRenewal")]
        public bool IsRenewal { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
    }

    public class TravelInsuranceQuote
    {
        [JsonProperty(PropertyName = "packageCode")]
        public string PackageCode { get; set; }

        [JsonProperty(PropertyName = "policyPeriodCode")]
        public string PolicyPeriodCode { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "coverageType")]
        public string CoverageType { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }
    }

    public class TravelInsuranceQuoteResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "premium")]
        public decimal Premium { get; set; }

        [JsonProperty(PropertyName = "discountPremium")]
        public decimal DiscountPremium { get; set; }
    }

    public class TravelInsurancePolicy
    {
        [JsonProperty(PropertyName = "travelID")]
        public long TravelID { get; set; }

        [Required(ErrorMessage = "agency is required")]
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [Required(ErrorMessage = "agent code is required")]
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [Required(ErrorMessage = "agent branch is required")]
        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "DOB")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "package code is required")]
        [JsonProperty(PropertyName = "packageCode")]
        public string PackageCode { get; set; }

        [JsonProperty(PropertyName = "packageName")]
        public string PackageName { get; set; }

        [JsonProperty(PropertyName = "policyPeriod")]
        public string PolicyPeroid { get; set; }

        [JsonProperty(PropertyName = "policyPeroidName")]
        public string PolicyPeroidName { get; set; }

        [Required(ErrorMessage = "insured code is required")]
        [JsonProperty(PropertyName = "insuranceCode")]
        public string InsuredCode { get; set; }

        [Required(ErrorMessage = "insured name is required")]
        [JsonProperty(PropertyName = "insuranceName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }

        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [Required(ErrorMessage = "insurance start date is required")]
        [JsonProperty(PropertyName = "insuranceStartDate")]
        public DateTime? InsuranceStartDate { get; set; }

        [JsonProperty(PropertyName = "expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "passport")]
        public string Passport { get; set; }

        [JsonProperty(PropertyName = "renewal")]
        public char Renewal { get; set; }

        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }

        [Required(ErrorMessage = "period of cover code is required")]
        [JsonProperty(PropertyName = "peroidOfCoverCode")]
        public string PeroidOfCoverCode { get; set; }

        [JsonProperty(PropertyName = "loadAmount")]
        public decimal? LoadAmount { get; set; }

        [JsonProperty(PropertyName = "discountAmount")]
        public decimal DiscountAmount { get; set; }

        [JsonProperty(PropertyName = "ffpNumber")]
        public string FFPNumber { get; set; }

        [JsonProperty(PropertyName = "questionaireCode")]
        public string QuestionaireCode { get; set; }

        [JsonProperty(PropertyName = "isPhysicalDefect")]
        public string IsPhysicalDefect { get; set; }

        [JsonProperty(PropertyName = "physicalStateDescription")]
        public string PhysicalStateDescription { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "cpr is required")]
        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "authorizedBy")]
        public int AuthorizedBy { get; set; }

        [JsonProperty(PropertyName = "creadtedDate")]
        public DateTime? CreadtedDate { get; set; }

        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty(PropertyName = "documentNumber")]
        public string DocumentNumber { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "isSaved")]
        public bool IsSaved { get; set; }

        [JsonProperty(PropertyName = "discounted")]
        public decimal? Discounted { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool ISHIR { get; set; }

        [JsonProperty(PropertyName = "coverageType")]
        public string CoverageType { get; set; }

        [Required(ErrorMessage = "policy period years is required")]
        [JsonProperty(PropertyName = "policyPeroidYears")]
        public int PolicyPeroidYears { get; set; }

        [JsonProperty(PropertyName = "isActivePolicy")]
        public bool IsActivePolicy { get; set; }

        [JsonProperty(PropertyName = "premiumBeforeDiscount")]
        public decimal PremiumBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "premiumAfterDiscount")]
        public decimal PremiumAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "commisonBeforeDiscount")]
        public decimal CommisionBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "commisonAfterDiscount")]
        public decimal CommissionAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "userChangedPremium")]
        public bool UserChangedPremium { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public int HIRStatus { get; set; }

        [JsonProperty(PropertyName = "endorsementCount")]
        public int EndorsementCount { get; set; }

        [JsonProperty(PropertyName = "isCancelled")]
        public bool IsCancelled { get; set; }

        [JsonProperty(PropertyName = "endorsementType")]
        public string EndorsementType { get; set; }
    }

    public class TravelMembers
    {
        [JsonProperty(PropertyName = "travelID")]
        public long TravelID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "itemSerialNo")]
        public int ItemSerialNo { get; set; }

        [JsonProperty(PropertyName = "itemName")]
        public string ItemName { get; set; }

        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }

        [JsonProperty(PropertyName = "foreignSumInsured")]
        public decimal ForeignSumInsured { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "sex")]
        public string Sex { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [JsonProperty(PropertyName = "make")]
        public string Make { get; set; }

        [JsonProperty(PropertyName = "occupationCode")]
        public string OccupationCode { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "passport")]
        public string Passport { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty(PropertyName = "makeDescription")]
        public string MakeDescription { get; set; }
    }

    public class TravelPolicy
    {
        [JsonProperty(PropertyName = "travelInsurancePolicyDetails")]
        public TravelInsurancePolicy TravelInsurancePolicyDetails { get; set; }

        [JsonProperty(PropertyName = "travelMembers")]
        public List<TravelMembers> TravelMembers { get; set; }

        public TravelPolicy()
        {
            TravelInsurancePolicyDetails = new TravelInsurancePolicy();
            TravelMembers = new List<TravelMembers>();
        }
    }

    public class TravelPolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "travelId")]
        public long TravelId { get; set; }

        [JsonProperty(PropertyName = "hirStatusMessage")]
        public string HIRStatusMessage { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "paymentTrackID")]
        public string PaymentTrackID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
    }

    public class TravelSavedQuotationResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "travelInsurancePolicyDetails")]
        public TravelInsurancePolicy TravelInsurancePolicyDetails { get; set; }

        [JsonProperty(PropertyName = "travelMembers")]
        public List<TravelMembers> TravelMembers { get; set; }

        [JsonProperty(PropertyName = "insuredDetails")]
        public InsuredMaster InsuredDetails { get; set; }

        public TravelSavedQuotationResponse()
        {
            TravelInsurancePolicyDetails = new TravelInsurancePolicy();
            TravelMembers = new List<TravelMembers>();
        }
    }

    public class UpdateTravelDetailsRequest
    {
        [JsonProperty(PropertyName = "travelInsurancePolicyDetails")]
        public TravelInsurancePolicy TravelInsurancePolicyDetails { get; set; }

        [JsonProperty(PropertyName = "travelMembers")]
        public List<TravelMembers> TravelMembers { get; set; }

        public UpdateTravelDetailsRequest()
        {
            TravelInsurancePolicyDetails = new TravelInsurancePolicy();
            TravelMembers = new List<TravelMembers>();
        }
    }

    public class UpdateTravelDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "paymentTrackId")]
        public string PaymentTrackId { get; set; }
    }

    public class FetchSumInsuredResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "sumInsuredUSD")]
        public int SumInsuredUSD { get; set; }

        [JsonProperty(PropertyName = "sumInsuredBHD")]
        public int SumInsuredBHD { get; set; }
    }

    public class TravelInsuranceExpiryDateResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "expiryDate")]
        public DateTime? ExpiryDate { get; set; }
    }

    public class TravelInsuranceExpiryDate
    {
        [JsonProperty(PropertyName = "packageCode")]
        public string PackageCode { get; set; }

        [JsonProperty(PropertyName = "commenceDate")]
        public DateTime? CommenceDate { get; set; }
    }
}