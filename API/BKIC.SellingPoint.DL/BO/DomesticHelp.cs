using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BKIC.SellingPoint.DL.BO
{
    public class DomesticHelpQuote
    {
        public int InsurancePeroid { get; set; }
        public int NumberOfDomesticWorkers { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
    }

    public class DomesticHelpQuoteResponse : TransactionWrapper
    {
        public decimal PremiumBeforeDiscount { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
    }

    public class DomesticHelpPolicy
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public string CPR { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }      
        public long DomesticID { get; set; }
        public int InsurancePeroid { get; set; }
        public int NoOfDomesticWorkers { get; set; }
        public string IsPhysicalDefect { get; set; }
        public string PhysicalDefectDescription { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public string DomesticWorkType { get; set; }
        public string FFPNumber { get; set; }
        public DateTime PolicyExpiryDate { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
        public decimal CommisionBeforeDiscount { get; set; }
        public decimal CommissionAfterDiscount { get; set; }
        public int CreatedBy { get; set; }    
        public int UpdatedBy { get; set; }
        public int AuthorizedBy { get; set; }
        public bool IsSaved { get; set; }
        public bool IsActivePolicy { get; set; }
        public string DocumentNo { get; set; }
        public decimal LoadAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Remarks { get; set; }
        public decimal SumInsured { get; set; }
        public bool IsHIR { get; set; }
        public decimal CommissionAmount { get; set; }       
        public DateTime PolicyIssueDate { get; set; }
        public string PaymentType { get; set; }
        public string AccountNumber { get; set; }
        public bool UserChangedPremium { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }       
        public int HIRStatus { get; set; }
        public DateTime DOB { get; set; }        
        public decimal TaxOnPremium { get; set; }        
        public decimal TaxOnCommission { get; set; }
    }

    public class DomesticHelpMember
    {
        public string InsuredCode { get; set; }
        public decimal SumInsured { get; set; }
        public decimal PremiumAmount { get; set; }
        public string OtherOccupation { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public DateTime CommencementDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Name { get; set; }
        public char Sex { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; }
        public string CPRNumber { get; set; }
        public string Occupation { get; set; }
        public int ItemserialNo { get; set; }
        public string AddressType { get; set; }
        public int Age { get; set; }
        public string  Passport { get; set; }
        public int CreatedBy { get; set; }
        public string NationalityDescription { get; set; }

      
    }



    public class DomesticPolicyDetails
    {
        public DomesticHelpPolicy DomesticHelp { get; set; }
        public DataTable DomesticHelpMemberdt { get; set; }

      public  DomesticPolicyDetails()
        {
            DomesticHelp = new DomesticHelpPolicy();
            DomesticHelpMemberdt = new DataTable();
        }
    }

    public class DomesticHelpPolicyResponse:TransactionWrapper
    {
        public long DomesticID { get; set; }
        public bool IsHIR { get; set; }
        public string PaymentTrackID { get; set; }
        public string DocumentNo { get; set; }
    }


    public class DomesticHelpSavedQuotationResponse : TransactionWrapper
    {
        public DomesticHelpPolicy DomesticHelp { get; set; }
        public List<DomesticHelpMember> DomesticHelpMemberList { get; set; }

        public DomesticHelpSavedQuotationResponse()
        {
            DomesticHelp = new DomesticHelpPolicy();
            DomesticHelpMemberList = new List<DomesticHelpMember>();
        }      
    } 

    public class AgencyDomesticRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public bool IncludeHIR { get; set; }
        public bool IsRenewal { get; set; }       
        public string DocumentNo { get; set; }
    }

    public class AgencyDomesticPolicy
    {
        public long DomesticId { get; set; }
        public string InsuredCode { get; set; }
        public string DocumentNo { get; set; }        
        public DateTime PolicyStartDate { get; set; }        
        public DateTime PolicyEndDate { get; set; }       
        public int RenewalCount { get; set; }
        public string DocumentRenewalNo { get; set; }
    }
    public class AgencyDomesticPolicyResponse : TransactionWrapper
    {
        public List<AgencyDomesticPolicy> DomesticAgencyPolicies { get; set; }
        public AgencyDomesticPolicyResponse()
        {
            DomesticAgencyPolicies = new List<AgencyDomesticPolicy>();
        }
    }
}
