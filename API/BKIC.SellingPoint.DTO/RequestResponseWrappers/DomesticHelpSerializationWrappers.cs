using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{

    public class AgencyDomesticRequest
    {
        [Required(ErrorMessage = "agency is required")]
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [Required(ErrorMessage = "agent code is required")]
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }      
        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }
        [JsonProperty(PropertyName = "includeHIR")]
        public bool IncludeHIR { get; set; }
        [JsonProperty(PropertyName = "isRenewal")]
        public bool IsRenewal { get; set; }      
        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

    }

    public class AgencyDomesticPolicy
    {
        [JsonProperty(PropertyName = "domesticId")]
        public long DomesticId { get; set; }

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

    public class AgencyDomesticPolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "domesticAgencyPolicies")]
        public List<AgencyDomesticPolicy> DomesticAgencyPolicies { get; set; }
        public AgencyDomesticPolicyResponse()
        {
            DomesticAgencyPolicies = new List<AgencyDomesticPolicy>();
        }
    }

    public class DomesticHelpQuote
    {
        [JsonProperty(PropertyName = "insurancePeroid")]
        public int InsurancePeroid { get; set; }

        [JsonProperty(PropertyName = "numberOfDomesticWorkers")]
        public int NumberOfDomesticWorkers { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }
    }

    public class DomesticHelpQuoteResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "premiumBeforeDiscount")]
        public decimal PremiumBeforeDiscount { get; set; }
        [JsonProperty(PropertyName = "premiumAfterDiscount")]
        public decimal PremiumAfterDiscount { get; set; }
    }


    public class DomesticHelpPolicy 
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

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "domesticID")]
        public long DomesticID { get; set; }

        [Required(ErrorMessage = "insurance period is required")]
        [JsonProperty(PropertyName = "insurancePeroid")]
        public int InsurancePeroid { get; set; }

        [Required(ErrorMessage = "noOfDomesticWorkers is required")]
        [JsonProperty(PropertyName = "noOfDomesticWorkers")]
        public int NoOfDomesticWorkers { get; set; }


        [JsonProperty(PropertyName = "isPhysicalDefect")]
        public string IsPhysicalDefect { get; set; }

        [JsonProperty(PropertyName = "physicalDefectDescription")]
        public string PhysicalDefectDescription { get; set; }

        [Required(ErrorMessage = "policy start date is required")]
        [JsonProperty(PropertyName = "policyStartDate")]
        public DateTime PolicyStartDate { get; set; }

        [JsonProperty(PropertyName = "domesticWorkType")]
        public string DomesticWorkType { get; set; }

        [JsonProperty(PropertyName = "ffpNumber")]
        public string FFPNumber { get; set; }

        [Required(ErrorMessage = "policy expire date is required")]
        [JsonProperty(PropertyName = "policyExpiryDate")]
        public DateTime PolicyExpiryDate { get; set; }


        [JsonProperty(PropertyName = "premiumBeforeDiscount")]
        public decimal PremiumBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "premiumAfterDiscount")]
        public decimal PremiumAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "commisonBeforeDiscount")]
        public decimal CommisionBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "commisonAfterDiscount")]
        public decimal CommissionAfterDiscount { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "authorizedBy")]
        public int AuthorizedBy { get; set; }

        [Required(ErrorMessage = "isSaved is required")]
        [JsonProperty(PropertyName = "isSaved")]
        public bool IsSaved { get; set; }

        [Required(ErrorMessage = "isActivePolicy is required")]
        [JsonProperty(PropertyName = "isActive")]
        public bool IsActivePolicy { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "loadAmount")]
        public decimal LoadAmount { get; set; }

        [JsonProperty(PropertyName = "discountAmount")]
        public decimal DiscountAmount { get; set; }

        [JsonProperty(PropertyName = "remarks")]
        public string Remarks { get; set; }

        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "commissionAmount")]
        public decimal CommissionAmount { get; set; }

        [JsonProperty(PropertyName = "issueDate")]
        public DateTime PolicyIssueDate { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }      

        [JsonProperty(PropertyName = "userChangedPremium")]
        public bool UserChangedPremium { get; set; }
        
        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public int HIRStatus { get; set; }

        [JsonProperty(PropertyName = "dOB")]
        public DateTime DOB { get; set; }

        [JsonProperty(PropertyName = "taxOnPremium")]
        public decimal TaxOnPremium { get; set; }

        [JsonProperty(PropertyName = "taxOnCommission")]
        public decimal TaxOnCommission { get; set; }

    }

    public class DomesticHelpMember
    {
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }

        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [JsonProperty(PropertyName = "otherOccupation")]
        public string OtherOccupation { get; set; }

        [JsonProperty(PropertyName = "dateOfSubmission")]
        public DateTime DateOfSubmission { get; set; }

        [JsonProperty(PropertyName = "commencementDate")]
        public DateTime CommencementDate { get; set; }

        [JsonProperty(PropertyName = "expiryDate")]
        public DateTime ExpiryDate { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string Name { get; set; }  

        [JsonProperty(PropertyName = "sex")]
        public char Sex { get; set; }

        [JsonProperty(PropertyName = "dob")]
        public DateTime DOB { get; set; }

        [JsonProperty(PropertyName = "nationality")]
        public string Nationality { get; set; }

        [JsonProperty(PropertyName = "cprNumber")]
        public string CPRNumber { get; set; }

        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }       

        [JsonProperty(PropertyName = "itemserialNo")]
        public int ItemserialNo { get; set; }

        [JsonProperty(PropertyName = "addressType")]
        public string AddressType { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "passport")]
        public string Passport { get; set; }       

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "nationalityDescription")]
        public string NationalityDescription { get; set; }
    }



    public class DomesticPolicyDetails
    {
        [JsonProperty(PropertyName = "domesticHelp")]
        public DomesticHelpPolicy DomesticHelp { get; set; }

        [JsonProperty(PropertyName = "domesticHelpMemberList")]
        public List<DomesticHelpMember> DomesticHelpMemberList { get; set; }

        public DomesticPolicyDetails()
        {
            DomesticHelp = new DomesticHelpPolicy();
            DomesticHelpMemberList = new List<DomesticHelpMember>();
        }
    }

    public class DomesticHelpPolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "domesticID")]
        public long DomesticID { get; set; }
        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }
        [JsonProperty(PropertyName = "paymentTrackID")]
        public string PaymentTrackID { get; set; }
        [JsonProperty(PropertyName = "documentNumber")]
        public string DocumentNo { get; set; }
    }

    public class DomesticHelpSavedQuotationResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "domesticHelp")]
        public DomesticHelpPolicy DomesticHelp { get; set; }

        [JsonProperty(PropertyName = "domesticHelpMemberList")]
        public List<DomesticHelpMember> DomesticHelpMemberList { get; set; }


        public DomesticHelpSavedQuotationResponse()
        {
            DomesticHelp = new DomesticHelpPolicy();
            DomesticHelpMemberList = new List<DomesticHelpMember>();
        }
    }

}
