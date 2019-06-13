using System;
using System.Collections.Generic;
using System.Data;

namespace BKIC.SellingPoint.DL.BO
{
    public class TravelInsuranceQuote
    {
        public string PackageCode { get; set; }
        public string PolicyPeriodCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CoverageType { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
    }

    public class TravelInsuranceQuoteResponse : TransactionWrapper
    {
        public decimal Premium { get; set; }
        public decimal DiscountPremium { get; set; }
    }

    public class TravelInsurancePolicy
    {
        public long TravelID { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public DateTime? DOB { get; set; }
        public string PackageCode { get; set; }
        public string PackageName { get; set; }
        public string PolicyPeroid { get; set; }
        public string PolicyPeroidName { get; set; }
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public decimal SumInsured { get; set; }
        public decimal PremiumAmount { get; set; }
        public DateTime? InsuranceStartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string Passport { get; set; }
        public char Renewal { get; set; }
        public string Occupation { get; set; }
        public string PeroidOfCoverCode { get; set; }
        public decimal? LoadAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string FFPNumber { get; set; }
        public string QuestionaireCode { get; set; }
        public string IsPhysicalDefect { get; set; }
        public string PhysicalStateDescription { get; set; }
        public string Code { get; set; }
        public string CPR { get; set; }
        public string Mobile { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int AuthorizedBy { get; set; }
        public DateTime? CreadtedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string DocumentNumber { get; set; }
        public string Remarks { get; set; }
        public bool IsSaved { get; set; }
        public decimal? Discounted { get; set; }
        public string Source { get; set; }
        public string PaymentType { get; set; }
        public bool IsHIR { get; set; }
        public string CoverageType { get; set; }
        public int PolicyPeroidYears { get; set; }
        public bool IsActivePolicy { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
        public decimal CommisionBeforeDiscount { get; set; }
        public decimal CommissionAfterDiscount { get; set; }
        public bool UserChangedPremium { get; set; }
        public string AccountNumber { get; set; }
        public int HIRStatus { get; set; }
        public int EndorsementCount { get; set; }
        public bool IsCancelled { get; set; }
        public string EndorsementType { get; set; }
    }

    public class TravelMembers
    {
        public int TravelID { get; set; }
        public string DocumentNo { get; set; }
        public int ItemSerialNo { get; set; }
        public string ItemName { get; set; }
        public decimal SumInsured { get; set; }

        public decimal ForeignSumInsured { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Sex { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Age { get; set; }
        public decimal PremiumAmount { get; set; }
        public string Make { get; set; }
        public string OccupationCode { get; set; }
        public string CPR { get; set; }
        public string Passport { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MakeDescription { get; set; }
    }

    public class TravelPolicy
    {
        public TravelInsurancePolicy TravelInsurancePolicyDetails { get; set; }
        public DataTable TravelMembers { get; set; }

        public TravelPolicy()
        {
            TravelInsurancePolicyDetails = new TravelInsurancePolicy();
            TravelMembers = new DataTable();
        }
    }

    public class TravelPolicyResponse : TransactionWrapper
    {
        public long TravelId { get; set; }
        public string HIRStatusMessage { get; set; }
        public bool IsHIR { get; set; }
        public string PaymentTrackID { get; set; }
        public string DocumentNo { get; set; }
    }

    public class TravelSavedQuotationResponse : TransactionWrapper
    {
        public InsuredMaster InsuredDetails { get; set; }
        public TravelInsurancePolicy TravelInsurancePolicyDetails { get; set; }
        public List<TravelMembers> TravelMembers { get; set; }

        public TravelSavedQuotationResponse()
        {
            TravelInsurancePolicyDetails = new TravelInsurancePolicy();
            TravelMembers = new List<TravelMembers>();
        }
    }

    public class UpdateTravelDetailsRequest
    {
        public TravelInsurancePolicy TravelInsurancePolicyDetails { get; set; }
        public DataTable TravelMembers { get; set; }

        public UpdateTravelDetailsRequest()
        {
            TravelInsurancePolicyDetails = new TravelInsurancePolicy();
            TravelMembers = new DataTable();
        }
    }

    public class UpdateTravelDetailsResponse : TransactionWrapper
    {
        public bool IsHIR { get; set; }
        public string PaymentTrackId { get; set; }
        public string DocumentNo { get; set; }
    }

    public class FetchSumInsuredResponse : TransactionWrapper
    {
        public int SumInsuredUSD { get; set; }
        public int SumInsuredBHD { get; set; }
    }

    public class AgencyTravelRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string Type { get; set; }
        public string CPR { get; set; }
        public bool isEndorsement { get; set; }
        public bool includeHIR { get; set; }
        public bool IsRenewal { get; set; }
        public string DocumentNo { get; set; }
    }

    public class AgencyTravelPolicy
    {
        public long TravelId { get; set; }
        public string DocumentNo { get; set; }
        public string InsuredCode { get; set; }       
        public DateTime PolicyStartDate { get; set; }       
        public DateTime PolicyEndDate { get; set; }
        public int RenewalCount { get; set; }
        public string DocumentRenewalNo { get; set; }
    }

    public class AgencyTravelPolicyResponse : TransactionWrapper
    {
        public List<AgencyTravelPolicy> AgencyTravelPolicies { get; set; }

        public AgencyTravelPolicyResponse()
        {
            AgencyTravelPolicies = new List<AgencyTravelPolicy>();
        }
    }
}