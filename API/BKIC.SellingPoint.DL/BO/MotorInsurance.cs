using System;
using System.Collections.Generic;

namespace BKIC.SellingPoint.DL.BO
{
    public class AgencyMotorPolicy
    {
        public string DocumentNo { get; set; }
        public string InsuredCode { get; set; }
        public long MotorID { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public int RenewalCount { get; set; }
        public string DocumentRenewalNo { get; set;}
    }

    public class AgencyMotorPolicyResponse : TransactionWrapper
    {
        public AgencyMotorPolicyResponse()
        {
            AgencyMotorPolicies = new List<AgencyMotorPolicy>();
        }
        public List<AgencyMotorPolicy> AgencyMotorPolicies { get; set; }
    }

    public class AgencyMotorRequest
    {
        public string Agency { get; set; }
        public string AgentBranch { get; set; }
        public string AgentCode { get; set; }
        public string CPR { get; set; }
        public string Type { get; set; }
        public bool isEndorsement { get; set; }
        public bool IncludeHIR { get; set; }
        public bool IsRenewal { get; set; }
        public string DocumentNo { get; set; }
    }

    public class ExcessAmountRequest
    {
        public string ExcessType { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleType { get; set; }       
        public string Agency { get; set; }        
        public string AgentCode { get; set; }       
        public string MainClass { get; set; }       
        public string SubClass { get; set; }       
        public bool IsUnderAge { get; set; }       
        public string InsuredCode { get; set; }
    }

    public class ExcessAmountResponse : TransactionWrapper
    {
        public decimal ExcessAmount { get; set; }
        public string BodyType { get; set; }
    }


    public class MotorBodyRequest
    {       
        public string VehicleMake { get; set; }
        
        public string VehicleModel { get; set; }
    }

    public class InsuranceCertificateResponse : TransactionWrapper
    {
        public string AdditionalBenefits { get; set; }
        public string AreaCode { get; set; }
        public string Branch { get; set; }
        public string BuilidingNo { get; set; }
        public string ChassisNo { get; set; }
        public DateTime CommencementDate { get; set; }
        public string CoverType { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal ExcessAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string FilePath { get; set; }
        public DateTime InsurancePeroid { get; set; }
        public string InsuranceType { get; set; }
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public bool IsNCB { get; set; }
        public DateTime? NCBEndDate { get; set; }
        public DateTime? NCBStartDate { get; set; }
        public string PaymentAuthCode { get; set; }
        public string PolicyCode { get; set; }
        public string PolicyNo { get; set; }
        public decimal Premium { get; set; }
        public string RegistrationNo { get; set; }
        public string RoadNo { get; set; }
        public string Source { get; set; }
        public string Subclass { get; set; }
        public string TypeOfCover { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public decimal VehicleValue { get; set; }
        public string YearOfMake { get; set; }
        public string Agency { get; set; }
        public string FinanceCompany { get; set; }
        public List<MotorCovers> Covers { get; set; }
        public List<MotorCovers> OptionalCovers { get; set; }

    }

    public class MigRenewDetailsResponse : TransactionWrapper
    {
        public bool IsUserExist { get; set; }
    }

    public abstract class MotorInsurance
    {
        public DateTime DOB { get; set; }
        public bool IsNCB { get; set; }
        public string VehicleType { get; set; }
        public string YearOfMake { get; set; }
    }

    public class MotorInsurancePolicy
    {
        public string AccountNumber { get; set; }
        public string Agency { get; set; }
        public string AgencyCode { get; set; }
        public string AgentBranch { get; set; }
        public int AuthorizedBy { get; set; }
        public string Branch { get; set; }
        public string ChassisNo { get; set; }
        public decimal CommisionBeforeDiscount { get; set; }
        public decimal CommissionAfterDiscount { get; set; }
        public string CPR { get; set; }
        public int CreatedBy { get; set; }
        public string DeliveryBranch { get; set; }
        public string DeliveryOption { get; set; }
        public string DelvArea { get; set; }
        public string DelvBldNo { get; set; }
        public string DelvBlockNo { get; set; }
        public string DelvFlatNo { get; set; }
        public string DelvRoadNo { get; set; }
        public decimal? DiscountAmount { get; set; }
        public DateTime DOB { get; set; }
        public string DocumentNo { get; set; }
        public int EngineCC { get; set; }
        public decimal ExcessAmount { get; set; }
        public string ExcessType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string FinancierCompanyCode { get; set; }
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
        public bool IsActivePolicy { get; set; }
        public bool IsHIR { get; set; }
        public bool IsNCB { get; set; }
        public bool IsSaved { get; set; }
        public decimal LoadAmount { get; set; }
        public string Mainclass { get; set; }
        public string MobileNumber { get; set; }
        public long MotorID { get; set; }
        public DateTime? NCBEndDate { get; set; }
        public DateTime? NCBStartDate { get; set; }
        public string PaymentAuthorizationCode { get; set; }
        public string PaymentType { get; set; }
        public string PolicyCode { get; set; }
        public DateTime PolicyCommencementDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
        public string RegistrationNumber { get; set; }
        public string Remarks { get; set; }
        public string Subclass { get; set; }
        public string TransactionNo { get; set; }
        public int UpdatedBy { get; set; }
        public bool UserChangedPremium { get; set; }
        public string vehicleBodyType { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public string vehicleTypeCode { get; set; }
        public decimal VehicleValue { get; set; }
        public int YearOfMake { get; set; }
        public List<MotorCovers> Covers { get; set; }
        public List<MotorCovers> ProductCovers { get; set; }
        public List<MotorCovers> OptionalCovers { get; set; }
        public int HIRStatus { get; set; }
        public int EndorsementCount { get; set; }
        public bool IsCancelled { get; set; }
        public decimal TaxOnPremium { get; set; }
        public decimal TaxOnCommission { get; set; }
        public decimal OptionalCoverAmount { get; set; }
        public bool IsUnderBCFC { get; set; }
        public int SeatingCapacity { get; set; }      
        public bool IsRenewal { get; set; }
        public decimal ClaimAmount { get; set; }
        public int RenewalCount { get; set; }      
        public string OldDocumentNumber { get; set; }
        public bool IsSavedRenewal { get; set; }
        public string EndorsementType { get; set; }
        public int RenewalDelayedDays { get; set; }
        public DateTime? ActualRenewalStartDate { get; set; }

    }

    public class MotorCovers
    {
        public int ID { get; set; }
        public string CoverCode { get; set; }
        public string CoverDescription { get; set; }
        public decimal CoverAmount { get; set; }       
        public bool AddedByEndorsement { get; set; }
        public bool IsOptional { get; set; }
    }

    public class MotorInsurancePolicyResponse : TransactionWrapper
    {
        public string DocumentNo { get; set; }
        public bool IsHIR { get; set; }
        public long MotorID { get; set; }
        public int RenewalCount { get; set; }
    }

    public class MotorInsurancePremium
    {
        public int CarReplacementDays { get; set; }
        public DateTime DOB { get; set; }
        public string ExcessType { get; set; }
        public bool IsNCB { get; set; }
        public bool IsPersonalAccidentCovered { get; set; }
        public bool IsSRCC { get; set; }
        public DateTime? NCBFromDate { get; set; }
        public DateTime? NCBToDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public string ProductCode { get; set; }
        public string VehicleType { get; set; }
        public decimal VehileValue { get; set; }
        public int YearOfMake { get; set; }
    }

    public class MotorInsurancePremiumResult : TransactionWrapper
    {
        public decimal PremiumAfterDiscount { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
    }

    public class MotorInsuranceQuote : MotorInsurance
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string ExcessType { get; set; }
        public string MainClass { get; set; }
        public DateTime? NCBFromDate { get; set; }
        public DateTime? NCBToDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public string RegistrationMonth { get; set; }
        public string TypeOfInsurance { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public decimal VehicleSumInsured { get; set; }
        public decimal OptionalCoverAmount { get; set; }
        public int RenewalDelayedDays { get; set; }
    }

    public class MotorInsuranceQuoteResponse : TransactionWrapper
    {
        public decimal DiscountPremium { get; set; }
        public decimal TotalPremium { get; set; }
    }

    public class MotorRenewalDetailsResponse : TransactionWrapper
    {
        public MotorRenewalDetailsResponse()
        {
            MototorRenewDetails = new MotorInsurancePolicy();
        }

        public bool IsEarlyRenewal { get; set; }
        public bool IsPolicyExpired { get; set; }
        public bool IsRenewDetailsExist { get; set; }
        public MotorInsurancePolicy MototorRenewDetails { get; set; }
    }

    public class MotorSavedQuotationResponse : TransactionWrapper
    {
        public MotorSavedQuotationResponse()
        {
            MotorPolicyDetails = new MotorInsurancePolicy();
        }

        public MotorInsurancePolicy MotorPolicyDetails { get; set; }
    }

    public class RenewMotorPolicyRequest
    {
        public string CPR { get; set; }
        public string DeliveryBranch { get; set; }
        public string DeliveryOption { get; set; }
        public string DelvArea { get; set; }
        public string DelvBldNo { get; set; }
        public string DelvBlockNo { get; set; }
        public string DelvFlatNo { get; set; }
        public string DelvRoadNo { get; set; }
        public string EmailAddress { get; set; }
        public string FFPNumber { get; set; }
        public string MobileNo { get; set; }
        public long RenewMotorID { get; set; }
    }

    public class RenewMotorPolicyResponse : TransactionWrapper
    {
        public string PaymentTrackID { get; set; }
    }

    public class RenewPrecheckResponse : TransactionWrapper
    {
        public bool IsEarlyRenewal { get; set; }
        public bool IsPolicyExpired { get; set; }
        public bool IsRenewDetailsExist { get; set; }
    }

    public class UpdateMotorRequest
    {
        public UpdateMotorRequest()
        {
            MotorInsurance = new MotorInsurancePolicy();
        }

        public MotorInsurancePolicy MotorInsurance { get; set; }
    }

    public class UpdateMotorResponse : TransactionWrapper
    {
        public bool IsHIR { get; set; }
        public string PaymentTrackId { get; set; }
    }

    public class OptionalCoverRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
    }

    public class OptionalCoverResponse : TransactionWrapper
    {        
        public List<MotorCovers> OptionalCovers { get; set; }

        public OptionalCoverResponse()
        {
            OptionalCovers = new List<MotorCovers>();
        }
    }

    public class CalculateCoverAmountRequest
    {        
        public string Agency { get; set; }       
        public string AgentCode { get; set; }        
        public string MainClass { get; set; }       
        public string SubClass { get; set; }
        public string CoverCode { get; set; }
        public decimal BaseCoverAmount { get; set; }       
        public int NoOfSeats { get; set; }
        public decimal SumInsured { get; set; }
    }

    public class CalculateCoverAmountResponse : TransactionWrapper
    {       
        public decimal CoverAmount { get; set; }
    }

    //public class MotorVehicle
    //{
       
    //    public string ManufacturerID { get; set; }      
    //    public string ManufacturerDescription { get; set; }        
    //    public string ModelID { get; set; }        
    //    public string ModelDescription { get; set; }      
    //    public string Type { get; set; }      
    //    public string BodyType { get; set; }     
    //    public int Capacity { get; set; }      
    //    public int VehicleValue { get; set; }      
    //    public decimal Excess { get; set; }
    //}

    //public class MotorVehicleRequest
    //{
    //    public string Type { get; set; }
    //    public int ID { get; set; }
    //    public string Agency { get; set; }
    //    public string AgentCode{ get; set; }
    //    public string ManufacturerID { get; set; }       
    //    public string ModelID { get; set; }
    //    public MotorVehicle MotorVehicle { get; set; }
    //}
    //public class MotorVehicleResponse : TransactionWrapper
    //{       
    //    public List<MotorVehicle> MotorVehicles { get; set; }
    //    public MotorVehicleResponse()
    //    {
    //        MotorVehicles = new List<MotorVehicle>();
    //    }
    //}
}