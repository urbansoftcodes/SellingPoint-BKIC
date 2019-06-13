using System;
using System.Collections.Generic;

namespace BKIC.SellingPoint.DL.BO
{
    public class MasterTable
    {
        public const string BranchMaster = "MT_BranchMaster";
        public const string InsuranceProduct = "MT_InsuranceProduct";
        public const string InsuranceProductMaster = "MT_InsuranceProductMaster";
        public const string AgencyMaster = "MT_AgentMaster";
        public const string UserMaster = "MT_UserMaster";
        public const string CategoryMaster = "MT_CategoryMaster";
        public const string MotorProductCover = "MT_MotorProductCover";
        public const string MotorCoverMaster = "MT_MotorCoverMaster";
        public const string InsuredMaster = "MT_InsuredMaster";
        public const string IntroducedbyMaster = "MT_IntroducedbyMaster";
    }

    public class MasterTableResponse<T> : TransactionWrapper
    {
        public List<T> TableRows { get; set; }

        public MasterTableResponse()
        {
            TableRows = new List<T>();
        }
    }

    public class MTIntroducedbyMaster
    {
        public long Id { get; set; }
        public string CompanayCode { get; set; }
        public string UnitCode { get; set; }
        public string LookupType { get; set; }
        public string LookupClass { get; set; }
        public string LookupCode { get; set; }
        public string Description { get; set; }
        public string IsEnabled { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
        public string CharValue { get; set; }
    }

    public class MTInsuredMaster
    {
        public long InsuredId { get; set; }
        public string InsuredCode { get; set; }
        public string PassportNo { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string CPR { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Flat { get; set; }
        public string Building { get; set; }
        public string Road { get; set; }
        public string Block { get; set; }
        public string Area { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
    }

    public class MTMotorCoverMaster
    {
        public int COVERID { get; set; }
        public string CoverCode { get; set; }
        public string COVERSDescription { get; set; }
    }

    public class MTMotorProductCover
    {
        public int COVERID { get; set; }
        public string AGENCY { get; set; }
        public string AGENTCODE { get; set; }
        public string MAINCLASS { get; set; }
        public string SUBCLASS { get; set; }
        public string IsCovered { get; set; }
        public string COVERTYPE { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public decimal COVERAMOUNT { get; set; }
        public decimal RATE { get; set; }
        public string COVERDescription { get; set; }
        public bool IsActive { get; set; }
        public string CoverCode { get; set; }
    }

    public class MTUserMaster
    {
        public int Id { get; set; }
        public string AGENCY { get; set; }
        public string AGENTCODE { get; set; }
        public string AGENTBRANCH { get; set; }
        public string USERID { get; set; }
        public string USERNAME { get; set; }
        public DateTime? CREATEDDATE { get; set; }
        public string MOBILE { get; set; }
        public string EMAIL { get; set; }
        public bool IsActive { get; set; }
        public int STAFFNO { get; set; }
        public string Role { get; set; }
        public string CreatedBy { get; set; }
    }

    public class MTAgentMaster
    {
        public int Id { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public bool IsActive { get; set; }
        public string CustomerCode { get; set; }
    }

    public class MTBranchMaster
    {
        public int Id { get; set; }
        public string AGENCY { get; set; }
        public string AGENTCODE { get; set; }
        public string AGENTBRANCH { get; set; }
        public string BRANCHNAME { get; set; }
        public string BRANCHADDRESS { get; set; }
        public string TELEPHONENO { get; set; }
        public string INCHARGE { get; set; }
        public string EMAIL { get; set; }
        public bool ISACTIVE { get; set; }
        public string CreatedBy { get; set; }
    }

    public class MTInsuranceProductMaster
    {
        public int ID { get; set; }
        public string AGENCY { get; set; }
        public string AGENTCODE { get; set; }
        public string MAINCLASS { get; set; }
        public string SUBCLASS { get; set; }
        public DateTime? EFFECTIVEDATEFROM { get; set; }
        public DateTime? EFFECTIVEDATETO { get; set; }
        public bool ISACTIVE { get; set; }
        public DateTime? CREATEDDATE { get; set; }
        public DateTime? UPDATEDDATE { get; set; }
        public string CreatedBy { get; set; }
    }

    public class BranchMaster
    {
        public int Id { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string Phone { get; set; }
        public string Incharge { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
    }

    public class BranchMasterResponse : TransactionWrapper
    {
    }

    public class InsuranceProduct
    {
        public string Agency { get; set; }
        public string Mainclass { get; set; }
        public string SubClass { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffectiveDateTo { get; set; }
    }

    public class InsuranceProductMaster
    {
        public int Id { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string Mainclass { get; set; }
        public string SubClass { get; set; }
        public DateTime? EffectiveDateFrom { get; set; }
        public DateTime? EffectiveDateTo { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
    }

    public class InsuranceProductMasterResponse : TransactionWrapper
    {
    }

    public class MotorCoverMaster
    {
        public int CoverId { get; set; }
        public string CoversDescription { get; set; }
        public string CoversCode { get; set; }
        public string Type { get; set; }
    }

    public class MotorCoverMasterResponse : TransactionWrapper
    {
    }

    public class MotorProductCover
    {
        public int CoverId { get; set; }
        public string Agency { get; set; }
        public string AgencyCode { get; set; }
        public string Mainclass { get; set; }
        public string SubClass { get; set; }
        public string IsCovered { get; set; }
        public decimal Rate { get; set; }
        public string CoverType { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Type { get; set; }
        public decimal CoverAmount { get; set; }
        public decimal Percent { get; set; }
        public string CoverDescription { get; set; }
        public bool IsActive { get; set; }
        public string CoverCode { get; set; }
        public bool IsOptionalCover { get; set; }
    }

    public class MotorProductCoverResponse : TransactionWrapper
    {
        public string CoverDescription { get; set; }
    }

    public class MotorVehicleMaster
    {
        public int ID { get; set; }
        public string Make { get; set; }
        public string MakeDescription { get; set; }
        public string Model { get; set; }
        public string ModelDescription { get; set; }
        public string VehicleType { get; set; }
        public int Year { get; set; }
        public decimal VehicleValue { get; set; }
        public int Tonnage { get; set; }
        public string Body { get; set; }
        public decimal ExcessAmount { get; set; }
        public decimal NewExcessAmount { get; set; }
        public int SeatingCapacity { get; set; }       
        public string Type { get; set; }
    }

    public class MotorVehicleMasterResponse : TransactionWrapper
    {
       public List<MotorVehicleMaster> MotorVehicleMaster { get; set; }

        public MotorVehicleMasterResponse()
        {
            MotorVehicleMaster = new List<MotorVehicleMaster>();
        }

    }


    public class MotorYearMaster
    {        
        public int ID { get; set; }        
        public int Year { get; set; }        
        public string Type { get; set; }
    }
    public class MotorYearMasterResponse : TransactionWrapper
    {       
        public List<MotorYearMaster> MotorYears { get; set; }
        public MotorYearMasterResponse()
        {
            MotorYears = new List<MotorYearMaster>();
        }
    }

    public class MotorEngineCCMaster
    {        
        public int ID { get; set; }        
        public int Tonnage { get; set; }       
        public string Capacity { get; set; }        
        public string Type { get; set; }
    }
    public class MotorEngineCCResponse : TransactionWrapper
    {       
        public List<MotorEngineCCMaster> MotorEngineCC { get; set; }
        public MotorEngineCCResponse()
        {
            MotorEngineCC = new List<MotorEngineCCMaster>();
        }
    }

    public class AgentMaster
    {
        public int Id { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string CustomerCode { get; set; }
        public string AgentBranch { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }
    }

    public class AgentMasterResponse : TransactionWrapper
    {
    }

    public class UserMasterDetails
    {
        public int Id { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int StaffNo { get; set; }
        public string Role { get; set; }
        public string CreatedBy { get; set; }
        public string Type { get; set; }
    }

    public class UserMasterDetailsResponse : TransactionWrapper
    {
        public List<UserMasterDetails> UserMaster { get; set; }

        public UserMasterDetailsResponse()
        {
            UserMaster = new List<UserMasterDetails>();
        }
    }

    public class CategoryMaster
    {
        public int id { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string ValueType { get; set; }
        public decimal Value { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
        public bool IsDeductable { get; set; }
    }

    public class CategoryMasterResponse : TransactionWrapper
    {
        public List<CategoryMaster> Categories { get; set; }

        public CategoryMasterResponse()
        {
            Categories = new List<CategoryMaster>();
        }
    }

    public class InsuredMasterDetails
    {
        public long InsuredId { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string InsuredCode { get; set; }
        public string PassportNo { get; set; }
        public string CPR { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Flat { get; set; }
        public string Building { get; set; }
        public string Road { get; set; }
        public string Block { get; set; }
        public string Area { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public bool IsActive { get; set; }
        public string InsuredName { get; set; }
        public string Type { get; set; }
    }

    public class InsuredMasterDetailsResponse : TransactionWrapper
    {
        public string InsuredCode { get; set; }
        public string InsuredName { get; set; }
    }

    public class InsuredRequest
    {
        public string CPR { get; set; }
        public string InsuredCode { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
    }

    public class InsuredResponse : TransactionWrapper
    {
        public InsuredMasterDetails InsuredDetails { get; set; }

        public InsuredResponse()
        {
            InsuredDetails = new InsuredMasterDetails();
        }
    }

    public class Introducedby
    {
        public long Id { get; set; }
        public string CompanayCode { get; set; }
        public string UnitCode { get; set; }
        public string LookupType { get; set; }
        public string LookupClass { get; set; }
        public string LookupCode { get; set; }
        public string Description { get; set; }
        public string IsEnabled { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
        public string CharValue { get; set; }
        public string Type { get; set; }
    }

    public class IntroducedbyResponse : TransactionWrapper
    {
    }

    public class AgencyInsuredRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string CPR { get; set; }
    }

    public class AgencyInsuredResponse : TransactionWrapper
    {
        public List<InsuredMasterDetails> AgencyInsured { get; set; }

        public AgencyInsuredResponse()
        {
            AgencyInsured = new List<InsuredMasterDetails>();
        }
    }

    public class AgencyUserRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string AgentBranch { get; set; }
        public string CPR { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }

    public class AgencyUserResponse : TransactionWrapper
    {
        public List<UserMasterDetails> AgencyUsers { get; set; }

        public AgencyUserResponse()
        {
            AgencyUsers = new List<UserMasterDetails>();
        }
    }

    public class MotorPolicyDetails
    {
        public long MotorID { get; set; }
        public string DocumentNo { get; set; }
        public string InsuredName { get; set; }
        public string InsuredCode { get; set; }
        public string CPR { get; set; }
        public string LinkID { get; set; }
        public string HIRReason { get; set; }
        public string HIRStatus { get; set; }
        public decimal GrossPremium { get; set; }
        public string AgentCode { get; set; }
        public string Agency { get; set; }
        public string Responsibility { get; set; }
        public string HIRStatusDesc { get; set; }
        public bool IsMessageAvailable { get; set; }
        public bool IsDocumentsAvailable { get; set; }
        public string MainClass { get; set; }
        public string Subclass { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string AccountNo { get; set; }
        public bool UserChangedPremium { get; set; }       
        public string HIRRemarks { get; set; }       
        public int RenewalCount { get; set; }
    }

    public class AdminFetchMotorDetailsResponse : TransactionWrapper
    {
        public List<MotorPolicyDetails> MotorDetails { get; set; }

        public AdminFetchMotorDetailsResponse()
        {
            MotorDetails = new List<MotorPolicyDetails>();
        }
    }

    public class AdminFetchMotorDetailsRequest
    {
        public string Type { get; set; }
        public string AgencyCode { get; set; }
        public bool ByAgencyCode { get; set; }
        public int HIRStatus { get; set; }
        public bool ByHIRStatus { get; set; }
        public string DocumentNo { get; set; }
        public bool ByDocumentNo { get; set; }
        public bool ByStatusAndAgency { get; set; }
        public bool All { get; set; }       
        public int UserID { get; set; }
        public bool ByUserID { get; set; }
    }

    public class HomePolicyDetails
    {
        public long HomeID { get; set; }
        public string DocumentNo { get; set; }
        public string InsuredName { get; set; }
        public decimal GrossPremium { get; set; }
        public string AgentCode { get; set; }
        public string HIRStatusDesc { get; set; }
        public bool IsMessageAvailable { get; set; }
        public bool IsDocumentsAvailable { get; set; }
        public string MainClass { get; set; }
        public string Subclass { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string AccountNo { get; set; }
        public string InsuredCode { get; set; }
        public string CPR { get; set; }
        public string LinkID { get; set; }
        public string AuthorizationCode { get; set; }
        public string Agency { get; set; }
        public string HIRStatus { get; set; }
        public string HIRReason { get; set; }
        public string Responsibility { get; set; }        
        public string HIRRemarks { get; set; }     
        public int RenewalCount { get; set; }
    }

    public class AdminFetchHomeDetailsResponse : TransactionWrapper
    {
        public List<HomePolicyDetails> HomeDetails { get; set; }

        public AdminFetchHomeDetailsResponse()
        {
            HomeDetails = new List<HomePolicyDetails>();
        }
    }

    public class AdminFetchHomeDetailsRequest
    {
        public string Type { get; set; }
        public string AgencyCode { get; set; }
        public bool ByAgencyCode { get; set; }
        public int HIRStatus { get; set; }
        public bool ByHIRStatus { get; set; }
        public string DocumentNo { get; set; }
        public bool ByDocumentNo { get; set; }
        public bool ByStatusAndAgency { get; set; }
        public bool All { get; set; }
        public int UserID { get; set; }
        public bool ByUserID { get; set; }
    }

    public class TravelPolicyDetails
    {
        public Int64 ID { get; set; }
        public string DocumentNo { get; set; }
        public string InsuredName { get; set; }
        public string InsuredCode { get; set; }
        public string CPR { get; set; }
        public string LinkID { get; set; }
        public string AuthorizationCode { get; set; }
        public string Source { get; set; }
        public decimal NetPremium { get; set; }
        public string AgentCode { get; set; }
        public string Agency { get; set; }
        public string HIRStatusDesc { get; set; }
        public bool IsMessageAvailable { get; set; }
        public bool IsDocumentsAvailable { get; set; }
        public string MainClass { get; set; }
        public string Subclass { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string AccountNo { get; set; }
        public string Status { get; set; }
        public string HIRStatus { get; set; }       
        public string HIRRemarks { get; set; }
    }

    public class AdminFetchTravelDetailsResponse : TransactionWrapper
    {
        public List<TravelPolicyDetails> TravelDetails { get; set; }

        public AdminFetchTravelDetailsResponse()
        {
            TravelDetails = new List<TravelPolicyDetails>();
        }
    }

    public class AdminFetchTravelDetailsRequest
    {
        public string Type { get; set; }
        public string AgencyCode { get; set; }
        public bool ByAgencyCode { get; set; }
        public int HIRStatus { get; set; }
        public bool ByHIRStatus { get; set; }
        public string DocumentNo { get; set; }
        public bool ByDocumentNo { get; set; }
        public bool ByStatusAndAgency { get; set; }
        public bool All { get; set; }
        public int UserID { get; set; }
        public bool ByUserID { get; set; }
    }

    public class DomesticInsurancePolicyDetails
    {
        public Int64 ID { get; set; }
        public string DocumentNo { get; set; }
        public string InsuredName { get; set; }
        public string InsuredCode { get; set; }
        public string CPR { get; set; }
        public string LinkID { get; set; }
        public string AuthorizationCode { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public decimal NetPremium { get; set; }
        public string AgentCode { get; set; }
        public string Agency { get; set; }
        public decimal GrossPremium { get; set; }
        public string HIRStatusDesc { get; set; }
        public string MainClass { get; set; }
        public string Subclass { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
        public string AccountNo { get; set; }
        public bool IsMessageAvailable { get; set; }
        public bool IsDocumentsAvailable { get; set; }
        public string HIRStatus { get; set; }       
        public string HIRRemarks { get; set; }
    }

    public class AdminFetchDomesticDetailsResponse : TransactionWrapper
    {
        public List<DomesticInsurancePolicyDetails> DomesticDetails { get; set; }

        public AdminFetchDomesticDetailsResponse()
        {
            DomesticDetails = new List<DomesticInsurancePolicyDetails>();
        }
    }

    public class AdminFetchDomesticDetailsRequest
    {
        public string Type { get; set; }
        public string AgencyCode { get; set; }
        public bool ByAgencyCode { get; set; }
        public int HIRStatus { get; set; }
        public bool ByHIRStatus { get; set; }
        public string DocumentNo { get; set; }
        public bool ByDocumentNo { get; set; }
        public bool ByStatusAndAgency { get; set; }
        public bool All { get; set; }
        public int UserID { get; set; }
        public bool ByUserID { get; set; }
    }

    public class CommissionRequest
    {
        public string AgentCode { get; set; }
        public decimal PremiumAmount { get; set; }
        public string Agency { get; set; }
        public string SubClass { get; set; }
        public string InsuranceType { get; set; }
        public bool IsDeductable { get; set; }
        public string CommissionCode { get; set; }
    }

    public class CommissionResponse : TransactionWrapper
    {
        public decimal CommissionAmount { get; set; }
    }

    public class HomeCommissionRequest
    {
        public string AgentCode { get; set; }
        public decimal PremiumAmount { get; set; }
        public string Agency { get; set; }
        public string SubClass { get; set; }
        public string EndorsementType { get; set; }
        public decimal ActualBuildingSumInsured { get; set; }
        public decimal ActualContentSumInsured { get; set; }
        public decimal TotalBasicPremium { get; set; }
        public decimal TotalSRCCPremium { get; set; }
        public decimal NewSumInsured { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsRoitAdded { get; set; }
    }

    public class HomeCommissionResponse : TransactionWrapper
    {
        public decimal BasicCommission { get; set; }
        public decimal SRCCCommission { get; set; }
    }

    public class VatRequest
    {
        public decimal PremiumAmount { get; set; }
        public decimal CommissionAmount { get; set; }
    }

    public class VatResponse : TransactionWrapper
    {
        public decimal VatAmount { get; set; }
        public decimal VatCommissionAmount { get; set; }
    }

    public class DocumentDetailsRequest
    {
        public string CPR { get; set; }
        public string AgentCode { get; set; }
    }

    public class DocumentDetails
    {
        public string DocumentNo { get; set; }
        public string ExpireDate { get; set; }
        public string PolicyType { get; set; }
        public int RenewalCount { get; set; }
    }

    public class DocumentDetailsResponse : TransactionWrapper
    {
        public List<DocumentDetails> DocumentDetails { get; set; }

        public DocumentDetailsResponse()
        {
            DocumentDetails = new List<DocumentDetails>();
        }
    }

    public class AdminFetchReportRequest
    {
        public string InsuranceType { get; set; }
        public string ReportType { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string BranchCode { get; set; }
        public int AuthorizedUserID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public string VehicleMake { get; set; }
    }

    public class BaseReportDetails
    {
        public string PolicyNo { get; set; }
        public string EndorsementNo { get; set; }
        public string SubClass { get; set; }
        public decimal SumInsured { get; set; }
        public decimal NewPremium { get; set; }
        public decimal RenewalPremium { get; set; }
        public decimal AdditionalPremium { get; set; }
        public decimal RefundPremium { get; set; }
        public decimal Vat { get; set; }
    }

    public class MotorReportDetails : BaseReportDetails
    {
        public string BranchCode { get; set; }
        public string AuthorizedCode { get; set; }
        public string HandledBy { get; set; }
        public string CPR { get; set; }
        public int Age { get; set; }
        public string VehicleType { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public int Year { get; set; }
    }

    public class TravelHomeReportDetails : BaseReportDetails
    {
        public string BranchCode { get; set; }
        public string AuthorizedCode { get; set; }
        public string HandledBy { get; set; }
    }

    public class MotorReportResponse : TransactionWrapper
    {
        public List<MotorReportDetails> MotorReportDetails { get; set; }

        public MotorReportResponse()
        {
            MotorReportDetails = new List<MotorReportDetails>();
        }
    }

    public class TravelHomeReportResponse : TransactionWrapper
    {
        public List<TravelHomeReportDetails> TravelHomeReportDetails { get; set; }

        public TravelHomeReportResponse()
        {
            TravelHomeReportDetails = new List<TravelHomeReportDetails>();
        }
    }

    public class MainReportDetails : BaseReportDetails
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string InsuredName { get; set; }
        public string AuthorizedUser { get; set; }
        public string AuthorizedDate { get; set; }
        public string HandledBy { get; set; }
        public string CommenceDate { get; set; }
        public string ExpiryDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal RefundCommision { get; set; }
        public decimal Commission { get; set; }
        public decimal Discount { get; set; }
        public decimal PremiumLessCredit { get; set; }
        public string PremiumReference { get; set; }
        public string CommisionReference { get; set; }
        public string BatchDate { get; set; }
        public string CPR { get; set; }        
    }

    public class MainReportResponse : TransactionWrapper
    {
        public List<MainReportDetails> MainReportDetails { get; set; }

        public MainReportResponse()
        {
            MainReportDetails = new List<MainReportDetails>();
        }
    }

    public class AgecyProductRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string Type { get; set; }
    }

    public class AgencyProduct
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string Description { get; set; }
        public decimal MinimumPremium { get; set; }
        public decimal Rate { get; set; }
    }

    public class MotorProduct : AgencyProduct
    {
        public bool AllowUnderAge { get; set; }
        public int UnderAge { get; set; }
        public bool HasAgeLoading { get; set; }
        public bool HasAdditionalDays { get; set; }
        public decimal UnderAgeminPremium { get; set; }
        public bool AllowMaxVehicleAge { get; set; }
        public int MaximumVehicleAge { get; set; }
        public decimal MaximumVehicleValue { get; set; }
        public int GCCCoverRangeInYears { get; set; }
        public bool HasGCC { get; set; }
        public bool IsProductSport { get; set; }
        public string PolicyCode { get; set; }
        public decimal ExcessAmount { get; set; }
        public decimal UnderAgeExcessAmount { get; set; }
        public decimal AgeLoadingPercent { get; set; }
        public bool UnderAgeToHIR { get; set; }
        public long LastSeries { get; set; }
        public int SeriesFormatLength { get; set; }
        public List<CategoryMaster> Category { get; set; }
        public List<MotorOptionalBenefit> MotorOptionalBenefits { get; set; }
        public List<MotorClaim> MotorClaim { get; set; }
        public List<MotorEndorsementMaster> MotorEndorsementMaster { get; set; }
        public decimal TaxRate { get; set; }
        public decimal UnderAgeRate { get; set; }
        public bool AllowUsedVehicle { get; set; }
        public decimal GulfAssitAmount { get; set; }

        public MotorProduct()
        {
            Category = new List<CategoryMaster>();
            MotorOptionalBenefits = new List<MotorOptionalBenefit>();
            MotorEndorsementMaster = new List<MotorEndorsementMaster>();
        }

    }

    public class HomeProduct : AgencyProduct
    {
        public decimal RiotCoverRate { get; set; }
        public decimal RiotCoverMinAmount { get; set; }
        public decimal DomesticHelperAmount { get; set; }
        public int MaximumHomeAge { get; set; }
        public decimal MaximumBuildingValue { get; set; }
        public decimal MaximumContentValue { get; set; }
        public decimal MaximumJewelleryValue { get; set; }
        public decimal MaximumTotalValue { get; set; }
        public List<JewelleryCover> JewelleryCover { get; set; }
        public List<CategoryMaster> Category { get; set; }
        public List<HomeEndorsementMaster> HomeEndorsementMaster { get; set; }
        public decimal TaxRate { get; set; }
    }

    public class HomeProductResponse :TransactionWrapper
    {
        public List<HomeProduct> HomeProducts { get; set; }

        public HomeProductResponse()
        {
            HomeProducts = new List<HomeProduct>();
        }
    }

    public class AgencyProductResponse : TransactionWrapper
    {
        public List<MotorProduct> MotorProducts { get; set; }
        public List<HomeProduct> HomeProducts { get; set; }

        public AgencyProductResponse()
        {
            MotorProducts = new List<MotorProduct>();
            HomeProducts = new List<HomeProduct>();
        }
    }


    public class MotorCoverRequest
    {       
        public string Agency { get; set; }       
        public string AgentCode { get; set; }       
        public string MainClass { get; set; }       
        public string SubClass { get; set; }
        public string Type { get; set; }
    }

    public class MotorCoverResponse : TransactionWrapper
    {       
        public List<MotorCovers> Covers { get; set; }
        public MotorCoverResponse()
        {
            Covers = new List<MotorCovers>();
        }
    }
    public class MotorProductMaster : MotorProduct
    {
        public string Type { get; set; }
        public int ID { get; set; }
    }
    public class MotorProductMasterResponse :TransactionWrapper
    {
        public List<MotorProductMaster> motorProductMaster { get; set; }       
        public MotorProductMasterResponse()
        {
            motorProductMaster = new List<MotorProductMaster>();
        } 
    }
    
    public class MotorProductRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string Type { get; set; }
    }

    public class PolicyCategory
    {
        public long DocumentID { get; set; }
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string LinkID{ get; set; }
        public string DocumentNo { get; set; }
        public string LineNo { get; set; }
        public string EndorsementNo { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public string ValueType { get; set; }
        public int EndorsementCount { get; set; }
        public decimal Value { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
        public decimal CommissionBeforeDiscount { get; set; }
        public decimal CommissionAfterDiscount { get; set; }
        public decimal TaxOnPremiumBeforeDiscount { get; set; }
        public decimal TaxOnPremiumAfterDiscount { get; set; }
        public decimal TaxOnCommissionBeforeDiscount { get; set; }
        public decimal TaxOnCommissionAfterDiscount { get; set; }
        public bool IsDeductable { get; set; }
        public int RenewalCount { get; set; }
        public long MotorID { get; set; }
        public long HomeID { get; set; }
        public long DomesticID { get; set; }
        public long TravelID { get; set; }
        public long MotorEndorsementID { get; set; }
        public long HomeEndorsementID { get; set; }       
        public long TravelEndorsementID { get; set; }
    }

    public class MotorOptionalBenefit
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public decimal Percentage { get; set; }
    }
    public class MotorClaim
    {   
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public decimal Percentage { get; set; }
        public decimal MaximumClaimAmount { get; set; }
    }
    public class MotorEndorsementMaster
    {
        public string EndorsementType { get; set; }
        public string EndorsementCode { get; set; }
        public decimal ChargeAmount { get; set; }
        public bool HasCommission{ get; set; }
    }
    public class JewelleryCover
    {
        public string KeyType { get; set; }
        public string ValueType { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
    }
    public class HomeEndorsementMaster
    {
        public string EndorsementType { get; set; }
        public string EndorsementCode { get; set; }
        public decimal ChargeAmount { get; set; }
        public bool HasCommission { get; set; }
    }

    public class HomeProductRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string MainClass { get; set; }
        public string SubClass { get; set; }
        public string Type { get; set; }
    }

    public class PolicyRecord
    {
        public string DocumentNumber { get; set; }
        public long NewHomeID { get; set; }
        public long NewMotorID { get; set; }
        public string LinkID { get; set; }
        public bool IsInserted { get; set; }
        public int RenewalCount { get; set; }
    }

    public class AgencyPolicyDetailsRequest
    {
        public string Agency { get; set; }
        public string AgentCode { get; set; }
        public string DocumentNo { get; set; }
        public string InsuranceType { get; set; }
    }

    public class RenewalPrecheckRequest
    {        
        public string Agency { get; set; }      
        public string AgentCode { get; set; }      
        public string DocumentNo { get; set; }      
        public string InsuranceType { get; set; }       
        public int CurrentRenewalCount { get; set; }       
        public DateTime? PolicyStartDate { get; set; }        
        public DateTime? PolicyExpireDate { get; set; }

    }
    public class RenewalPrecheckResponse : TransactionWrapper
    {        
        public bool IsAlreadyRenewed { get; set; }
    }
}