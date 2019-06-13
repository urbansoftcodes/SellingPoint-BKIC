using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public abstract class MotorInsurance
    {
        [JsonProperty(PropertyName = "yearOfMake")]
        public string YearOfMake { get; set; }

        [JsonProperty(PropertyName = "dob")]
        public DateTime DOB { get; set; }

        [JsonProperty(PropertyName = "isNCB")]
        public bool IsNCB { get; set; }

        [JsonProperty(PropertyName = "vehicleType")]
        public string VehicleType { get; set; }
    }

    public class AgencyMotorRequest
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

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "isEndorsement")]
        public bool isEndorsement { get; set; }

        [JsonProperty(PropertyName = "includeHIR")]
        public bool IncludeHIR { get; set; }

        [JsonProperty(PropertyName = "isRenewal")]
        public bool IsRenewal { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
    }

    public class AgencyMotorPolicy
    {
        [JsonProperty(PropertyName = "motorID")]
        public long MotorID { get; set; }

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

    public class AgencyMotorPolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "agencyMotorPolicies")]
        public List<AgencyMotorPolicy> AgencyMotorPolicies { get; set; }

        public AgencyMotorPolicyResponse()
        {
            AgencyMotorPolicies = new List<AgencyMotorPolicy>();
        }
    }

    public class MotorInsuranceQuote : MotorInsurance
    {
        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }

        [JsonProperty(PropertyName = "vehicleModel")]
        public string VehicleModel { get; set; }

        [JsonProperty(PropertyName = "vehicleSumInsured")]
        public decimal VehicleSumInsured { get; set; }

        [JsonProperty(PropertyName = "typeOfInsurance")]
        public string TypeOfInsurance { get; set; }

        [JsonProperty(PropertyName = "ncbFromDate")]
        public DateTime? NCBFromDate { get; set; }

        [JsonProperty(PropertyName = "ncbToDate")]
        public DateTime? NCBToDate { get; set; }

        [JsonProperty(PropertyName = "policyStartDate")]
        public DateTime PolicyStartDate { get; set; }

        [JsonProperty(PropertyName = "policyEndDate")]
        public DateTime PolicyEndDate { get; set; }

        [JsonProperty(PropertyName = "registrationMonth")]
        public string RegistrationMonth { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "excessType")]
        public string ExcessType { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "optionalCoverAmount")]
        public decimal OptionalCoverAmount { get; set; }

        [JsonProperty(PropertyName = "renewalDelayedDays")]
        public int RenewalDelayedDays { get; set; }
    }

    public class MotorInsuranceQuoteResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "totalPremium")]
        public decimal TotalPremium { get; set; }

        [JsonProperty(PropertyName = "discountPremium")]
        public decimal DiscountPremium { get; set; }
    }

    public class MotorInsurancePolicy
    {
        [Required(ErrorMessage = "agency is required")]
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [Required(ErrorMessage = "agencycode is required")]
        [JsonProperty(PropertyName = "agencyCode")]
        public string AgencyCode { get; set; }

        [Required(ErrorMessage = "agentbranch is required")]
        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [Required(ErrorMessage = "insured code is required")]
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [Required(ErrorMessage = "insured name is required")]
        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [Required(ErrorMessage = "expire date is required")]
        [JsonProperty(PropertyName = "expiryDate")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "dob is required")]
        [JsonProperty(PropertyName = "dob")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "year of make is required")]
        [JsonProperty(PropertyName = "yearOfMake")]
        public int YearOfMake { get; set; }

        [Required(ErrorMessage = "vehicle make is required")]
        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }

        [Required(ErrorMessage = "vehicle model is required")]
        [JsonProperty(PropertyName = "vehicleModel")]
        public string VehicleModel { get; set; }

        [Required(ErrorMessage = "vehicle value is required")]
        [JsonProperty(PropertyName = "vehicleValue")]
        public decimal VehicleValue { get; set; }
       
        [JsonProperty(PropertyName = "registrationNumber")]
        public string RegistrationNumber { get; set; }

        [Required(ErrorMessage = "chassisNo is required")]
        [JsonProperty(PropertyName = "chassisNo")]
        public string ChassisNo { get; set; }

        [Required(ErrorMessage = "policy start date is required")]
        public DateTime PolicyCommencementDate { get; set; }

        [Required(ErrorMessage = "policy end date is required")]
        [JsonProperty(PropertyName = "policyEndDate")]
        public DateTime PolicyEndDate { get; set; }

        [Required(ErrorMessage = "mainclass is required")]
        [JsonProperty(PropertyName = "mainclass")]
        public string Mainclass { get; set; }

        [Required(ErrorMessage = "subclass is required")]
        [JsonProperty(PropertyName = "subclass")]
        public string Subclass { get; set; }

        [Required(ErrorMessage = "isSaved is required")]
        [JsonProperty(PropertyName = "isSaved")]
        public bool IsSaved { get; set; }

        [Required(ErrorMessage = "isActive is required")]
        [JsonProperty(PropertyName = "isActivePolicy")]
        public bool IsActivePolicy { get; set; }

        [JsonProperty(PropertyName = "motorID")]
        public long MotorID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public String DocumentNo { get; set; }

        [JsonProperty(PropertyName = "vehicleTypeCode")]
        public string VehicleTypeCode { get; set; }

        [JsonProperty(PropertyName = "vehicleBodyType")]
        public string vehicleBodyType { get; set; }

        [JsonProperty(PropertyName = "policyCode")]
        public string PolicyCode { get; set; }

        [JsonProperty(PropertyName = "isNCB")]
        public bool IsNCB { get; set; }

        [JsonProperty(PropertyName = "ncbStartDate")]
        public DateTime? NCBStartDate { get; set; }

        [JsonProperty(PropertyName = "ncbEndDate")]
        public DateTime? NCBEndDate { get; set; }

        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [JsonProperty(PropertyName = "deliveryOption")]
        public string DeliveryOption { get; set; }

        [JsonProperty(PropertyName = "deliveryBranch")]
        public string DeliveryBranch { get; set; }

        [JsonProperty(PropertyName = "excessAmount")]
        public decimal ExcessAmount { get; set; }

        [JsonProperty(PropertyName = "engineCC")]
        public int EngineCC { get; set; }

        [JsonProperty(PropertyName = "financierCompanyCode")]
        public string FinancierCompanyCode { get; set; }

        [JsonProperty(PropertyName = "ExcessType")]
        public string ExcessType { get; set; }

        [JsonProperty(PropertyName = "carReplacementDays")]
        public int CarReplacementDays { get; set; }

        [JsonProperty(PropertyName = "createdby")]
        public int Createdby { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public int UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "authorizedBy")]
        public int AuthorizedBy { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "mobileNumber")]
        public string MobileNumber { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "paymentAuthorizationCode")]
        public string PaymentAuthorizationCode { get; set; }

        [JsonProperty(PropertyName = "transactionNo")]
        public string TransactionNo { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

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

        [JsonProperty(PropertyName = "commisonBeforeDiscount")]
        public decimal CommisionBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "commisonAfterDiscount")]
        public decimal CommissionAfterDiscount { get; set; }      

        [JsonProperty(PropertyName = "covers")]
        public List<MotorCovers> Covers { get; set; }

        [JsonProperty(PropertyName = "productCovers")]
        public List<MotorCovers> ProductCovers { get; set; }

        [JsonProperty(PropertyName = "optionalCovers")]
        public List<MotorCovers> OptionalCovers { get; set; }

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

        [JsonProperty(PropertyName = "optionalCoverAmount")]
        public decimal OptionalCoverAmount { get; set; }

        [JsonProperty(PropertyName = "isUnderBCFC")]
        public bool IsUnderBCFC { get; set; }

        [JsonProperty(PropertyName = "seatingCapacity")]
        public int SeatingCapacity { get; set; }

        [JsonProperty(PropertyName = "loadAmount")]
        public decimal LoadAmount { get; set; }

        [JsonProperty(PropertyName = "isRenewal")]
        public bool IsRenewal { get; set; }

        [JsonProperty(PropertyName = "claimAmount")]
        public decimal ClaimAmount { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int  RenewalCount { get; set; }

        [JsonProperty(PropertyName = "OldDocumentNumber")]
        public string OldDocumentNumber { get; set; }

        [JsonProperty(PropertyName = "isSavedRenewal")]
        public bool IsSavedRenewal { get; set; }

        [JsonProperty(PropertyName = "endorsementType")]
        public string EndorsementType { get; set; }

        [JsonProperty(PropertyName = "renewalDelayedDays")]
        public int RenewalDelayedDays { get; set; }

        [JsonProperty(PropertyName = "actualRenewalStartDate")]
        public DateTime? ActualRenewalStartDate { get; set; }
    }

    public class MotorCovers
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "coverCode")]
        public string CoverCode { get; set; }

        [JsonProperty(PropertyName = "coverDescription")]
        public string CoverDescription { get; set; }

        [JsonProperty(PropertyName = "coverAmount")]
        public decimal CoverAmount { get; set; }

        [JsonProperty(PropertyName = "addedByEndorsement")]
        public bool AddedByEndorsement { get; set; }

        [JsonProperty(PropertyName = "isOptional")]
        public bool IsOptional { get; set; }
    }

    public class MotorInsurancePolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorID")]
        public long MotorID { get; set; }

        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "paymentTrackID")]
        public string PaymentTrackID { get; set; }

        [JsonProperty(PropertyName = "documentNumber")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }
    }

    public class MotorInsurancePremium
    {
        [JsonProperty(PropertyName = "vehicleValue")]
        public decimal VehicleValue { get; set; }

        [JsonProperty(PropertyName = "yearOfMake")]
        public int YearOfMake { get; set; }

        [JsonProperty(PropertyName = "isSRCC")]
        public bool IsSRCC { get; set; }

        [JsonProperty(PropertyName = "isNCB")]
        public bool IsNCB { get; set; }

        [JsonProperty(PropertyName = "ncbFromDate")]
        public DateTime? NCBFromDate { get; set; }

        [JsonProperty(PropertyName = "ncbToDate")]
        public DateTime? NCBToDate { get; set; }

        [JsonProperty(PropertyName = "vehicleType")]
        public string VehicleType { get; set; }

        [JsonProperty(PropertyName = "productCode")]
        public string ProductCode { get; set; }

        [JsonProperty(PropertyName = "carReplacementDays")]
        public int CarReplacementDays { get; set; }

        [JsonProperty(PropertyName = "excessType")]
        public string ExcessType { get; set; }

        [JsonProperty(PropertyName = "isPersonalAccidentCovered")]
        public bool IsPersonalAccidentCovered { get; set; }

        [JsonProperty(PropertyName = "dob")]
        public DateTime DOB { get; set; }

        [JsonProperty(PropertyName = "policyStartDate")]
        public DateTime PolicyStartDate { get; set; }
    }

    public class MotorInsurancePremiumResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "premiumBeforeDiscount")]
        public decimal PremiumBeforeDiscount { get; set; }

        [JsonProperty(PropertyName = "premiumAfterDiscount")]
        public decimal PremiumAfterDiscount { get; set; }
    }

    public class ExcessAmountRequest
    {
        [Required(ErrorMessage = "vehicle make is required")]
        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }

        [Required(ErrorMessage = "vehicle model is required")]
        [JsonProperty(PropertyName = "vehicleModel")]
        public string VehicleModel { get; set; }

        [JsonProperty(PropertyName = "vehicleType")]
        public string VehicleType { get; set; }

        [Required(ErrorMessage = "excess type is required")]
        [JsonProperty(PropertyName = "excessType")]
        public string ExcessType { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "isUnderAge")]
        public bool IsUnderAge { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }
    }

    public class MotorBodyRequest
    {
        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }
        
        [JsonProperty(PropertyName = "vehicleModel")]
        public string VehicleModel { get; set; }
    }

    public class ExcessAmountResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "excessAmount")]
        public decimal ExcessAmount { get; set; }

        [JsonProperty(PropertyName = "bodyType")]
        public string BodyType { get; set; }
    }

    public class MotorSavedQuotationResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorPolicyDetails")]
        public MotorInsurancePolicy MotorPolicyDetails { get; set; }

        [JsonProperty(PropertyName = "InsuredDetails")]
        public InsuredMaster InsuredDetails { get; set; }

        public MotorSavedQuotationResponse()
        {
            MotorPolicyDetails = new MotorInsurancePolicy();
            InsuredDetails = new InsuredMaster();
        }
    }

    public class UpdateMotorRequest
    {
        [JsonProperty(PropertyName = "motorInsurance")]
        public MotorInsurancePolicy MotorInsurance { get; set; }

        public UpdateMotorRequest()
        {
            MotorInsurance = new MotorInsurancePolicy();
        }
    }

    public class UpdateMotorResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isHIR")]
        public bool IsHIR { get; set; }

        [JsonProperty(PropertyName = "paymentTrackId")]
        public string PaymentTrackId { get; set; }
    }

    public class MotorRenewalDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "mototorRenewDetails")]
        public MotorInsurancePolicy MototorRenewDetails { get; set; }

        public MotorRenewalDetailsResponse()
        {
            MototorRenewDetails = new MotorInsurancePolicy();
        }
    }

    public class RenewMotorPolicyRequest
    {
        [JsonProperty(PropertyName = "renewMotorID")]
        public long RenewMotorID { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "deliveryOption")]
        public string DeliveryOption { get; set; }

        [JsonProperty(PropertyName = "deliveryBranch")]
        public string DeliveryBranch { get; set; }

        [JsonProperty(PropertyName = "delvFlatNo")]
        public string DelvFlatNo { get; set; }

        [JsonProperty(PropertyName = "delvBldNo")]
        public string DelvBldNo { get; set; }

        [JsonProperty(PropertyName = "delvRoadNo")]
        public string DelvRoadNo { get; set; }

        [JsonProperty(PropertyName = "delvBlockNo")]
        public string DelvBlockNo { get; set; }

        [JsonProperty(PropertyName = "delvArea")]
        public string DelvArea { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "ffpNumber")]
        public string FFPNumber { get; set; }

        [JsonProperty(PropertyName = "mobileNo")]
        public string MobileNo { get; set; }
    }

    public class RenewMotorPolicyResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "paymentTrackID")]
        public string PaymentTrackID { get; set; }
    }

    public class MigRenewDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "isUserExist")]
        public bool IsUserExist { get; set; }
    }

    public class DeleteDueRenewalRequest
    {
        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
    }

    public class InsuranceCertificateResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "policyNo")]
        public string PolicyNo { get; set; }

        [JsonProperty(PropertyName = "typeOfCover")]
        public string TypeOfCover { get; set; }

        [JsonProperty(PropertyName = "excessAmount")]
        public decimal ExcessAmount { get; set; }

        [JsonProperty(PropertyName = "additionalBenefits")]
        public string AdditionalBenefits { get; set; }

        [JsonProperty(PropertyName = "insurancePeroid")]
        public DateTime InsurancePeroid { get; set; }

        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }

        [JsonProperty(PropertyName = "vehicleModel")]
        public string VehicleModel { get; set; }

        [JsonProperty(PropertyName = "policyCode")]
        public string PolicyCode { get; set; }

        [JsonProperty(PropertyName = "isNCB")]
        public bool IsNCB { get; set; }

        [JsonProperty(PropertyName = "ncbStartDate")]
        public DateTime? NCBStartDate { get; set; }

        [JsonProperty(PropertyName = "ncbEndDate")]
        public DateTime? NCBEndDate { get; set; }

        [JsonProperty(PropertyName = "insurancePeroid")]
        public string ChassisNo { get; set; }

        [JsonProperty(PropertyName = "commencementDate")]
        public DateTime CommencementDate { get; set; }

        [JsonProperty(PropertyName = "vehicleValue")]
        public decimal VehicleValue { get; set; }

        [JsonProperty(PropertyName = "premium")]
        public decimal Premium { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "builidingNo")]
        public string BuilidingNo { get; set; }

        [JsonProperty(PropertyName = "roadNo")]
        public string RoadNo { get; set; }

        [JsonProperty(PropertyName = "areaCode")]
        public string AreaCode { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "registrationNo")]
        public string RegistrationNo { get; set; }

        [JsonProperty(PropertyName = "subclass")]
        public string Subclass { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "financeCompany")]
        public string FinanceCompany { get; set; }

        [JsonProperty(PropertyName = "covers")]
        public List<MotorCovers> Covers { get; set; }

        [JsonProperty(PropertyName = "optionalCovers")]
        public List<MotorCovers> OptionalCovers { get; set; }
    }

    public class OptionalCoverRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }
        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }
    }

    public class OptionalCoverResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "optionalCovers")]
        public List<MotorCovers> OptionalCovers { get; set; }

        public OptionalCoverResponse()
        {
            OptionalCovers = new List<MotorCovers>();
        }
    }

    public class CalculateCoverAmountRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }
        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }
        [JsonProperty(PropertyName = "coverCode")]
        public string CoverCode { get; set; }
        [JsonProperty(PropertyName = "baseCoverAmount")]
        public decimal BaseCoverAmount { get; set; }
        [JsonProperty(PropertyName = "noOfSeats")]
        public int NoOfSeats { get; set; }
        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }
    }

    public class CalculateCoverAmountResponse: TransactionWrapper
    {
        [JsonProperty(PropertyName = "coverAmount")]
        public decimal CoverAmount { get; set; }
    }

    //public class MotorVehicle
    //{
    //    [JsonProperty(PropertyName = "manufacturerID")]
    //    public string ManufacturerID { get; set; }
    //    [JsonProperty(PropertyName = "manufacturerDescription")]
    //    public string ManufacturerDescription { get; set; }
    //    [JsonProperty(PropertyName = "modelID")]
    //    public string ModelID { get; set; }
    //    [JsonProperty(PropertyName = "modelDescription")]
    //    public string ModelDescription { get; set; }
    //    [JsonProperty(PropertyName = "type")]
    //    public string Type { get; set; }
    //    [JsonProperty(PropertyName = "bodyType")]
    //    public string BodyType { get; set; }
    //    [JsonProperty(PropertyName = "capacity")]
    //    public int Capacity { get; set; }
    //    [JsonProperty(PropertyName = "vehicleValue")]
    //    public int VehicleValue{ get; set; }
    //    [JsonProperty(PropertyName = "excess")]
    //    public decimal Excess { get; set; }
    //}

    //public class MotorVehicleRequest
    //{
    //    [JsonProperty(PropertyName = "Type")]
    //    public string Type { get; set; }
    //    [JsonProperty(PropertyName = "ID")]
    //    public int ID { get; set; }
    //    [JsonProperty(PropertyName = "agency")]
    //    public string Agency { get; set; }
    //    [JsonProperty(PropertyName = "agentCode")]
    //    public string AgentCode { get; set; }
    //    [JsonProperty(PropertyName = "manufacturerID")]
    //    public string ManufacturerID { get; set; }
    //    [JsonProperty(PropertyName = "modelID")]
    //    public string ModelID { get; set; }
    //    [JsonProperty(PropertyName = "motorVehicle")]
    //    public MotorVehicle MotorVehicle { get; set; }
    //}
    //public class MotorVehicleResponse : TransactionWrapper
    //{
    //    [JsonProperty(PropertyName = "motorvehicles")]
    //    public List<MotorVehicle> MotorVehicles { get; set; }

    //    public MotorVehicleResponse()
    //    {
    //        MotorVehicles = new List<MotorVehicle>();
    //    }
    //}
}