using System;
using System.Collections.Generic;
using System.Data;

namespace BKIC.SellingPoint.DL.BO
{
    public class HomeInsuranceQuote
    {
        public decimal BuildingValue { get; set; }
        public decimal ContentValue { get; set; }
        public decimal JewelleryValue { get; set; }
        public bool IsPropertyToBeInsured { get; set; }
        public string JewelleryCover { get; set; }
        public bool IsRiotStrikeAdded { get; set; }
        public int NumberOfDomesticWorker { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public int RenewalDelayedDays { get; set; }
    }

    public class HomeInsuranceQuoteResponse : TransactionWrapper
    {
        public decimal TotalPremium { get; set; }
        public decimal TotalCommission { get; set; }
    }

    public class HomeInsurancePolicy : UserDetailsEgov
    {
        public long HomeID { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public decimal BuildingValue { get; set; }
        public decimal ContentValue { get; set; }
        public decimal JewelleryValue { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
        public decimal CommisionBeforeDiscount { get; set; }
        public decimal CommissionAfterDiscount { get; set; }
        public int BuildingAge { get; set; }
        public char IsPropertyMortgaged { get; set; }
        public string FinancierCode { get; set; }
        public char IsSafePropertyInsured { get; set; }
        public string JewelleryCover { get; set; }
        public string JewelleryCoverType { get; set; }
        public char IsRiotStrikeDamage { get; set; }
        public char IsJointOwnership { get; set; }
        public string JointOwnerName { get; set; }
        public char IsPropertyInConnectionTrade { get; set; }
        public char IsPropertyUndergoingConstruction { get; set; }
        public string NamePolicyReasonSeekingReasons { get; set; }
        public char IsPropertyInsuredSustainedAnyLoss { get; set; }
        public char IsPropertyCoveredOtherInsurance { get; set; }
        public char IsSingleItemAboveContents { get; set; }
        public string BuildingNo { get; set; }
        public string FlatNo { get; set; }
        public string RoadNo { get; set; }
        public string BlockNo { get; set; }
        public string ResidanceTypeCode { get; set; }
        public string FFPNumber { get; set; }
        public char IsRequireDomestic { get; set; }
        public int NoOfDomesticWorker { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int AuthorizedBy { get; set; }
        public bool IsSaved { get; set; }
        public string DocumentNo { get; set; }
        public DateTime PolicyExpiryDate { get; set; }
        public decimal? LoadAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string Code { get; set; }
        public string Remarks { get; set; }
        public decimal SumInsured { get; set; }
        public bool IsHIR { get; set; }
        public string MainClass { get; set; }
        public bool UserChangedPremium { get; set; }
        public bool IsActivePolicy { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string SubClass { get; set; }
        public string Area { get; set; }
        public int NoOfFloors { get; set; }
        public int BuildingType { get; set; }
        public string HouseNo { get; set; }
        public string PaymentType { get; set; }
        public string AccountNumber { get; set; }
        public int HIRStatus { get; set; }
        public int EndorsementCount { get; set; }      
        public bool IsCancelled { get; set; }
        public decimal TaxOnPremium { get; set; }
        public decimal TaxOnCommission { get; set; }
        public bool IsRenewal { get; set; }
        public int RenewalCount { get; set; }
        public bool IsSavedRenewal { get; set; }
        public string OldDocumentNumber { get; set; }
        public string EndorsementType { get; set; }        
        public DateTime? ActualRenewalStartDate { get; set; }
        public int RenewalDelayedDays { get; set; }
    }

    public class HomeInsurancePolicyDetails
    {
        public HomeInsurancePolicy HomeInsurancePolicy { get; set; }
        public DataTable HomeSubItemsdt { get; set; }
        public DataTable HomeDomesticHelpdt { get; set; }

        public HomeInsurancePolicyDetails()
        {
            HomeInsurancePolicy = new HomeInsurancePolicy();
            HomeSubItemsdt = new DataTable();
            HomeDomesticHelpdt = new DataTable();
        }
    }

    public class HomeInsurancePolicyResponse: TransactionWrapper
    {
        public long HomeId { get; set; }
        public bool IsHIR { get; set; }
        public string TrackID { get; set; }
        public string DocumentNo { get; set; }
        public int RenewalCount { get; set; }
    }

    public class Questionaire
    {
        public string Code { get; set; }
        public string QuestionType { get; set; }
        public string QuestionDescription { get; set; }
        public string Answers { get; set; }
    }

    public class HomeDomesticHelp
    {
        public int HomeSID { get; set; }
        public int MemberSerialNo { get; set; }
        public string Name { get; set; }
        public string CPR { get; set; }
        public string Title { get; set; }
        public char Sex { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public decimal SumInsured { get; set; }
        public decimal PremiumAmount { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
    }

    public class HomeSavedQuotationResponse : TransactionWrapper
    {
        public HomeInsurancePolicy HomeInsurancePolicy { get; set; }

        public List<HomeDomesticHelp> DomesticHelp { get; set; }

        public List<HomeSubItems> HomeSubItems { get; set; }

        public HomeSavedQuotationResponse()
        {
            HomeInsurancePolicy = new HomeInsurancePolicy();
            DomesticHelp = new List<HomeDomesticHelp>();
        }
    }

    public class HomeSubItems
    {
        public int HomeSID { get; set; }
        public long HomeID { get; set; }
        public string LinkID { get; set; }
        public string DocumentNo { get; set; }
        public int SubItemSerialNo { get; set; }
        public string SubItemCode { get; set; }
        public string SubItemName { get; set; }
        public string Description { get; set; }
        public decimal SumInsured { get; set; }
        public string Remarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime Updatedtime { get; set; }
    }

    public class UpdateHomePolicyRequest
    {
        public HomeInsurancePolicy HomeInsurancePolicy { get; set; }
        public DataTable HomeSubItemsdt { get; set; }
        public DataTable HomeDomesticHelpdt { get; set; }

        public UpdateHomePolicyRequest()
        {
            HomeInsurancePolicy = new HomeInsurancePolicy();
            HomeSubItemsdt = new DataTable();
            HomeDomesticHelpdt = new DataTable();
        }
    }

    public class UpdateHomePolicyResponse : TransactionWrapper
    {
        public bool IsHIR { get; set; }
        public string PaymentTrackId { get; set; }
    }

    public class RenewHomePolicyResponse : TransactionWrapper
    {
        public string PaymentTrackID { get; set; }
    }

    public class RenewHomePolicyRequest
    {
        public int RenewID { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        public string FFPNumber { get; set; }
    }

    public class RenewalHomeInsuranceResponse : TransactionWrapper
    {
        public HomeInsurancePolicy HomeInsurancePolicy { get; set; }

        public List<HomeDomesticHelp> DomesticHelp { get; set; }

        public List<HomeSubItems> HomeSubItems { get; set; }

        public bool IsRenewExist { get; set; }
        public bool IsPolicyExpired { get; set; }
        public bool IsEarlyRenewal { get; set; }

        public RenewalHomeInsuranceResponse()
        {
            HomeInsurancePolicy = new HomeInsurancePolicy();
            DomesticHelp = new List<HomeDomesticHelp>();
            HomeSubItems = new List<HomeSubItems>();
        }
    }

    public class UserDetailsEgov
    {
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public string UserName { get; set; }
        public string Passport_ICNumber { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public int YearOfBirth { get; set; }
        public string Email { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public DateTime DOB { get; set; }
        public string MartialStatus { get; set; }
        public string GroupCode { get; set; }
        public string GroupCodeDetails { get; set; }
        public string CPR { get; set; }
        public string Mobile { get; set; }
    }

    public class AgencyHomeRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string Type { get; set; }
        public string CPR { get; set; }
        public bool IncludeHIR { get; set; }
        public bool IsRenewal { get; set; }
        public string DocumentNo { get; set; }
    }

    public class AgencyHomePolicy
    {
        public long HomeID { get; set; }
        public string InsuredCode { get; set; }
        public string DocumentNo { get; set; }       
        public DateTime PolicyStartDate { get; set; }       
        public DateTime PolicyEndDate { get; set; }        
        public int RenewalCount { get; set; }
        public string DocumentRenewalNo { get; set; }
    }

    public class AgencyHomePolicyResponse : TransactionWrapper
    {
        public List<AgencyHomePolicy> AgencyHomePolicies { get; set; }

        public AgencyHomePolicyResponse()
        {
            AgencyHomePolicies = new List<AgencyHomePolicy>();
        }
    }
}