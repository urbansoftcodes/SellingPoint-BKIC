using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class MasterTable
    {
        public const string BranchMaster = "MT_BranchMaster";
        public const string InsuranceProduct = "MT_InsuranceProduct";
        public const string InsuranceProductMaster = "MT_InsuranceProductMaster";
        public const string AgentMaster = "MT_AgentMaster";
        public const string UserMaster = "MT_UserMaster";
        public const string CategoryMaster = "MT_CategoryMaster";
        public const string MotorProductCover = "MT_MotorProductCover";
        public const string MotorCoverMaster = "MT_MotorCoverMaster";
        public const string InsuredMaster = "MT_InsuredMaster";
        public const string IntroducedbyMaster = "MT_IntroducedbyMaster";
    }

    public class MasterTableResult<T> : TransactionWrapper
    {
        public List<T> TableRows { get; set; }

        public MasterTableResult()
        {
            TableRows = new List<T>();
        }
    }

    public class MTIntroducedbyMaster
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "companayCode")]
        public string CompanayCode { get; set; }

        [JsonProperty(PropertyName = "unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty(PropertyName = "lookupType")]
        public string LookupType { get; set; }

        [JsonProperty(PropertyName = "lookupClass")]
        public string LookupClass { get; set; }

        [JsonProperty(PropertyName = "lookupCode")]
        public string LookupCode { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public string IsEnabled { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get; set; }

        [JsonProperty(PropertyName = "charValue")]
        public string CharValue { get; set; }
    }

    public class MTInsuredMasterDetails
    {
        [JsonProperty(PropertyName = "insuredId")]
        public long InsuredId { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "passportNo")]
        public string PassportNo { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "cPR")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "flat")]
        public string Flat { get; set; }

        [JsonProperty(PropertyName = "building")]
        public string Building { get; set; }

        [JsonProperty(PropertyName = "road")]
        public string Road { get; set; }

        [JsonProperty(PropertyName = "block")]
        public string Block { get; set; }

        [JsonProperty(PropertyName = "area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "nationality")]
        public string Nationality { get; set; }

        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
    }

    public class MTMotorCoverMaster
    {
        [JsonProperty(PropertyName = "coverId")]
        public int COVERID { get; set; }

        [JsonProperty(PropertyName = "coversDescription")]
        public string COVERSDescription { get; set; }

        [JsonProperty(PropertyName = "coverCode")]
        public string CoverCode { get; set; }
    }

    public class MTMotorProductCover
    {
        [JsonProperty(PropertyName = "coverID")]
        public int COVERID { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string AGENCY { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AGENTCODE { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MAINCLASS { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SUBCLASS { get; set; }

        [JsonProperty(PropertyName = "isCovered")]
        public string IsCovered { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public decimal RATE { get; set; }

        [JsonProperty(PropertyName = "coversType")]
        public string COVERTYPE { get; set; }

        [JsonProperty(PropertyName = "coversCode")]
        public string COVERCODE { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public string UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "coverAmount")]
        public decimal CoverAmount { get; set; }

        [JsonProperty(PropertyName = "coverDescription")]
        public string CoverDescription { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }
    } 

    public class MTUserMaster
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string AGENCY { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AGENTCODE { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AGENTBRANCH { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string USERID { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string USERNAME { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CREATEDDATE { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string MOBILE { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string EMAIL { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "staffNo")]
        public int STAFFNO { get; set; }

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }
    }

    public class MTAgentMaster
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "customerCode")]
        public string CustomerCode { get; set; }
    }

    public class MTBranchMaster
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string AGENCY { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AGENTCODE { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AGENTBRANCH { get; set; }

        [JsonProperty(PropertyName = "branchName")]
        public string BRANCHNAME { get; set; }

        [JsonProperty(PropertyName = "branchAddress")]
        public string BRANCHADDRESS { get; set; }

        [JsonProperty(PropertyName = "telephoneNo")]
        public string TELEPHONENO { get; set; }

        [JsonProperty(PropertyName = "incharge")]
        public string INCHARGE { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string EMAIL { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool ISACTIVE { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }
    }

    public class MTInsuranceProductMaster
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string AGENCY { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AGENTCODE { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MAINCLASS { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SUBCLASS { get; set; }

        [JsonProperty(PropertyName = "effectiveDateFrom")]
        public DateTime? EFFECTIVEDATEFROM { get; set; }

        [JsonProperty(PropertyName = "effectiveDateTo")]
        public DateTime? EFFECTIVEDATETO { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool ISACTIVE { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CREATEDDATE { get; set; }

        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime? UPDATEDDATE { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }
    }

    public class BranchMaster
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "branchName")]
        public string BranchName { get; set; }

        [JsonProperty(PropertyName = "branchAddress")]
        public string BranchAddress { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "incharge")]
        public string Incharge { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }
    }

    public class BranchMasterResponse : TransactionWrapper
    {
    }

    public class InsuranceProduct
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "mainclass")]
        public string Mainclass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "effectiveDateFrom")]
        public DateTime EffectiveDateFrom { get; set; }

        [JsonProperty(PropertyName = "effectiveDateTo")]
        public DateTime EffectiveDateTo { get; set; }
    }

    public class InsuranceProductMaster
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainclass")]
        public string Mainclass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "effectiveDateFrom")]
        public DateTime? EffectiveDateFrom { get; set; }

        [JsonProperty(PropertyName = "effectiveDateTo")]
        public DateTime? EffectiveDateTo { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty(PropertyName = "updatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }
    }

    public class InsuranceProductMasterResponse : TransactionWrapper
    {
    }

    public class MotorCoverMaster
    {
        [JsonProperty(PropertyName = "coverId")]
        public int CoverId { get; set; }

        [JsonProperty(PropertyName = "coversDescription")]
        public string CoversDescription { get; set; }

        [JsonProperty(PropertyName = "coversCode")]
        public string CoversCode { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class MotorCoverMasterResponse : TransactionWrapper
    {
    }

    public class MotorProductCover
    {
        [JsonProperty(PropertyName = "productCoverId")]
        public int CoverId { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agencyCode")]
        public string AgencyCode { get; set; }

        [JsonProperty(PropertyName = "mainclass")]
        public string Mainclass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "isCovered")]
        public string IsCovered { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public decimal Rate { get; set; }

        [JsonProperty(PropertyName = "coverType")]
        public string CoverType { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedBy")]
        public string UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "coverAmount")]
        public decimal CoverAmount { get; set; }

        [JsonProperty(PropertyName = "percent")]
        public decimal Percent { get; set; }

        [JsonProperty(PropertyName = "coverDescription")]
        public string CoverDescription { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "coversCode")]
        public string CoverCode { get; set; }

        [JsonProperty(PropertyName = "isOptionalCover")]
        public bool IsOptionalCover { get; set; }
    }

    public class MotorProductCoverResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "coverDescription")]
        public string CoverDescription { get; set; }
    }

    public class MotorVehicleMaster
    {
        [JsonProperty(PropertyName = "ID")]
        public int  ID { get; set; }

        [JsonProperty(PropertyName = "make")]
        public string Make { get; set; }

        [JsonProperty(PropertyName = "makeDescription")]
        public string MakeDescription { get; set; }

        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }

        [JsonProperty(PropertyName = "modelDescription")]
        public string ModelDescription { get; set; }

        [JsonProperty(PropertyName = "vehicleType")]
        public string VehicleType { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "vehicleValue")]
        public decimal VehicleValue { get; set; }

        [JsonProperty(PropertyName = "tonnage")]
        public int Tonnage { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "excessAmount")]
        public decimal ExcessAmount { get; set; }

        [JsonProperty(PropertyName = "newExcessAmount")]
        public decimal NewExcessAmount { get; set; }

        [JsonProperty(PropertyName = "seatingCapacity")]
        public int SeatingCapacity { get; set; }       

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class MotorVehicleMasterResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorVehicleMaster")]
       public List<MotorVehicleMaster> MotorVehicleMaster { get; set; }

        public MotorVehicleMasterResponse()
        {
            MotorVehicleMaster = new List<MotorVehicleMaster>();
        }
        
    }

    public class MotorYearMaster
    {
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
    public class MotorYearMasterResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorYears")]
        public List<MotorYearMaster> MotorYears { get; set; }

        public MotorYearMasterResponse()
        {
            MotorYears = new List<MotorYearMaster>();
        }
    }

    public class MotorEngineCCMaster
    {
        [JsonProperty(PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "tonnage")]
        public int Tonnage { get; set; }

        [JsonProperty(PropertyName = "capacity")]
        public string Capacity { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
    public class MotorEngineCCResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorEngineCC")]
        public List<MotorEngineCCMaster> MotorEngineCC { get; set; }

        public MotorEngineCCResponse()
        {
            MotorEngineCC = new List<MotorEngineCCMaster>();
        }
    }

    public class AgentMaster
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "customerCode")]
        public string CustomerCode { get; set; }
    }

    public class AgentMasterResponse : TransactionWrapper
    {
    }

    public class UserMasterDetails
    {
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "staffNo")]
        public int StaffNo { get; set; }

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class UserMasterDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "userMaster")]
        public List<UserMasterDetails> UserMaster { get; set; }

        public UserMasterDetailsResponse()
        {
            UserMaster = new List<UserMasterDetails>();
        }
    }

    public class CategoryMaster
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agenctCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "valueType")]
        public string ValueType { get; set; }

        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }

        [JsonProperty(PropertyName = "effectiveFrom")]
        public DateTime? EffectiveFrom { get; set; }

        [JsonProperty(PropertyName = "effectiveTo")]
        public DateTime? EffectiveTo { get; set; }

        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "isDeductable")]
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
        [JsonProperty(PropertyName = "insuredId")]
        public long InsuredId { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "passportNo")]
        public string PassportNo { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "cPR")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "middleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "flat")]
        public string Flat { get; set; }

        [JsonProperty(PropertyName = "building")]
        public string Building { get; set; }

        [JsonProperty(PropertyName = "road")]
        public string Road { get; set; }

        [JsonProperty(PropertyName = "block")]
        public string Block { get; set; }

        [JsonProperty(PropertyName = "area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "nationality")]
        public string Nationality { get; set; }

        [JsonProperty(PropertyName = "occupation")]
        public string Occupation { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }
    }

    public class InsuredMasterDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }
    }

    public class InsuredResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "insuredDetails")]
        public InsuredMasterDetails InsuredDetails { get; set; }

        public InsuredResponse()
        {
            InsuredDetails = new InsuredMasterDetails();
        }
    }

    public class Introducedby
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "companayCode")]
        public string CompanayCode { get; set; }

        [JsonProperty(PropertyName = "unitCode")]
        public string UnitCode { get; set; }

        [JsonProperty(PropertyName = "lookupType")]
        public string LookupType { get; set; }

        [JsonProperty(PropertyName = "lookupClass")]
        public string LookupClass { get; set; }

        [JsonProperty(PropertyName = "lookupCode")]
        public string LookupCode { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public string IsEnabled { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get; set; }

        [JsonProperty(PropertyName = "charValue")]
        public string CharValue { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class IntroducedbyResponse : TransactionWrapper
    {
    }

    public class InsuredRequest
    {
        [JsonProperty(PropertyName = "cPR")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
    }

    public class AgencyInsuredRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }
    }

    public class AgencyInsuredResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "agencyInsured")]
        public List<InsuredMasterDetails> AgencyInsured { get; set; }

        public AgencyInsuredResponse()
        {
            AgencyInsured = new List<InsuredMasterDetails>();
        }
    }

    public class AgencyUserRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
    }

    public class AgencyUserResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "agencyUser")]
        public List<UserMasterDetails> AgencyUsers { get; set; }

        public AgencyUserResponse()
        {
            AgencyUsers = new List<UserMasterDetails>();
        }
    }

    public class MotorPolicyDetails
    {
        [JsonProperty(PropertyName = "motorID")]
        public long MotorID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "cPR")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "grossPremium")]
        public decimal GrossPremium { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "resposibility")]
        public string Responsibility { get; set; }

        [JsonProperty(PropertyName = "hirStatusDesc")]
        public string HIRStatusDesc { get; set; }

        [JsonProperty(PropertyName = "hirReason")]
        public string HIRReason { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public string HIRStatus { get; set; }

        [JsonProperty(PropertyName = "isMessageAvailable")]
        public bool IsMessageAvailable { get; set; }

        [JsonProperty(PropertyName = "isDocumentsAvailable")]
        public bool IsDocumentsAvailable { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subclass")]
        public string Subclass { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "paymentDate")]
        public DateTime PaymentDate { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "accountNo")]
        public string AccountNo { get; set; }

        [JsonProperty(PropertyName = "userChangedPremium")]
        public bool UserChangedPremium { get; set; }

        [JsonProperty(PropertyName = "hirRemarks")]
        public string HIRRemarks { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }
    }

    public class AdminFetchMotorDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorDetails")]
        public List<MotorPolicyDetails> MotorDetails { get; set; }

        public AdminFetchMotorDetailsResponse()
        {
            MotorDetails = new List<MotorPolicyDetails>();
        }
    }

    public class AdminFetchMotorDetailsRequest
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "agencyCode")]
        public string AgencyCode { get; set; }

        [JsonProperty(PropertyName = "byAgencyCode")]
        public bool ByAgencyCode { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public int HIRStatus { get; set; }

        [JsonProperty(PropertyName = "byHirStatus")]
        public bool ByHIRStatus { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "byDocumentNo")]
        public bool ByDocumentNo { get; set; }

        [JsonProperty(PropertyName = "byStatusAndAgency")]
        public bool ByStatusAndAgency { get; set; }

        [JsonProperty(PropertyName = "all")]
        public bool All { get; set; }

        [JsonProperty(PropertyName = "userID")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "byUserID")]
        public  bool ByUserID { get; set; }




    }

    public class TravelPolicyDetails
    {
        [JsonProperty(PropertyName = "travelID")]
        public Int64 ID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "CPR")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "netPremium")]
        public decimal NetPremium { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "hirStatusDesc")]
        public string HIRStatusDesc { get; set; }

        [JsonProperty(PropertyName = "isMessageAvailable")]
        public bool IsMessageAvailable { get; set; }

        [JsonProperty(PropertyName = "isDocumentsAvailable")]
        public bool IsDocumentsAvailable { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subclass")]
        public string Subclass { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "TransactionDate")]
        public DateTime TransactionDate { get; set; }

        [JsonProperty(PropertyName = "paymentDate")]
        public DateTime PaymentDate { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "accountNo")]
        public string AccountNo { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public string HIRStatus { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "hirRemarks")]
        public string HIRRemarks { get; set; }
    }

    public class AdminFetchTravelDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorDetails")]
        public List<TravelPolicyDetails> TravelDetails { get; set; }

        public AdminFetchTravelDetailsResponse()
        {
            TravelDetails = new List<TravelPolicyDetails>();
        }
    }

    public class AdminFetchTravelDetailsRequest
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "agencyCode")]
        public string AgencyCode { get; set; }

        [JsonProperty(PropertyName = "byAgencyCode")]
        public bool ByAgencyCode { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public int HIRStatus { get; set; }

        [JsonProperty(PropertyName = "byHirStatus")]
        public bool ByHIRStatus { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "byDocumentNo")]
        public bool ByDocumentNo { get; set; }

        [JsonProperty(PropertyName = "byStatusAndAgency")]
        public bool ByStatusAndAgency { get; set; }

        [JsonProperty(PropertyName = "all")]
        public bool All { get; set; }

        [JsonProperty(PropertyName = "userID")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "byUserID")]
        public bool ByUserID { get; set; }
    }

    public class HomePolicyDetails
    {
        [JsonProperty(PropertyName = "homeID")]
        public long HomeID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "grossPremium")]
        public decimal GrossPremium { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "hirStatusDesc")]
        public string HIRStatusDesc { get; set; }

        [JsonProperty(PropertyName = "isMessageAvailable")]
        public bool IsMessageAvailable { get; set; }

        [JsonProperty(PropertyName = "isDocumentsAvailable")]
        public bool IsDocumentsAvailable { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subclass")]
        public string Subclass { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "paymentDate")]
        public DateTime PaymentDate { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "accountNo")]
        public string AccountNo { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "CPR")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public string HIRStatus { get; set; }

        [JsonProperty(PropertyName = "hirReason")]
        public string HIRReason { get; set; }

        [JsonProperty(PropertyName = "resposibility")]
        public string Responsibility { get; set; }

        [JsonProperty(PropertyName = "hirRemarks")]
        public string HIRRemarks { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }
    }

    public class AdminFetchHomeDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorDetails")]
        public List<HomePolicyDetails> HomeDetails { get; set; }

        public AdminFetchHomeDetailsResponse()
        {
            HomeDetails = new List<HomePolicyDetails>();
        }
    }

    public class AdminFetchHomeDetailsRequest
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "agencyCode")]
        public string AgencyCode { get; set; }

        [JsonProperty(PropertyName = "byAgencyCode")]
        public bool ByAgencyCode { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public int HIRStatus { get; set; }

        [JsonProperty(PropertyName = "byHirStatus")]
        public bool ByHIRStatus { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "byDocumentNo")]
        public bool ByDocumentNo { get; set; }

        [JsonProperty(PropertyName = "byStatusAndAgency")]
        public bool ByStatusAndAgency { get; set; }

        [JsonProperty(PropertyName = "all")]
        public bool All { get; set; }

        [JsonProperty(PropertyName = "userID")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "byUserID")]
        public bool ByUserID { get; set; }
    }

    public class DomesticInsurancePolicyDetails
    {
        [JsonProperty(PropertyName = "domesticID")]
        public Int64 ID { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "grossPremium")]
        public decimal GrossPremium { get; set; }

        [JsonProperty(PropertyName = "hirStatusDesc")]
        public string HIRStatusDesc { get; set; }

        [JsonProperty(PropertyName = "isMessageAvailable")]
        public bool IsMessageAvailable { get; set; }

        [JsonProperty(PropertyName = "isDocumentsAvailable")]
        public bool IsDocumentsAvailable { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subclass")]
        public string Subclass { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "paymentDate")]
        public DateTime PaymentDate { get; set; }

        [JsonProperty(PropertyName = "paymentType")]
        public string PaymentType { get; set; }

        [JsonProperty(PropertyName = "accountNo")]
        public string AccountNo { get; set; }

        [JsonProperty(PropertyName = "insuredCode")]
        public string InsuredCode { get; set; }

        [JsonProperty(PropertyName = "CPR")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }

        [JsonProperty(PropertyName = "authorizationCode")]
        public string AuthorizationCode { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "netPremium")]
        public decimal NetPremium { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "transactionDate")]
        public DateTime TransactionDate { get; set; }

        [JsonProperty(PropertyName = "HIRStatus")]
        public string HIRStatus { get; set; }

        [JsonProperty(PropertyName = "hirRemarks")]
        public string HIRRemarks { get; set; }
    }

    public class AdminFetchDomesticDetailsResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorDetails")]
        public List<DomesticInsurancePolicyDetails> DomesticDetails { get; set; }

        public AdminFetchDomesticDetailsResponse()
        {
            DomesticDetails = new List<DomesticInsurancePolicyDetails>();
        }
    }

    public class AdminFetchDomesticDetailsRequest
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "agencyCode")]
        public string AgencyCode { get; set; }

        [JsonProperty(PropertyName = "byAgencyCode")]
        public bool ByAgencyCode { get; set; }

        [JsonProperty(PropertyName = "hirStatus")]
        public int HIRStatus { get; set; }

        [JsonProperty(PropertyName = "byHirStatus")]
        public bool ByHIRStatus { get; set; }

        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "byDocumentNo")]
        public bool ByDocumentNo { get; set; }

        [JsonProperty(PropertyName = "byStatusAndAgency")]
        public bool ByStatusAndAgency { get; set; }

        [JsonProperty(PropertyName = "all")]
        public bool All { get; set; }

        [JsonProperty(PropertyName = "userID")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "byUserID")]
        public bool ByUserID { get; set; }
    }

    public class CommissionRequest
    {
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "isDeductable")]
        public bool IsDeductable { get; set; }

        [JsonProperty(PropertyName = "commissionCode")]
        public string CommissionCode { get; set; }
    }

    public class CommissionResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "commissionAmount")]
        public decimal CommissionAmount { get; set; }
    }

    public class HomeCommissionRequest
    {
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "endorsementType")]
        public string EndorsementType { get; set; }

        [JsonProperty(PropertyName = "actualBuildingSumInsured")]
        public decimal ActualBuildingSumInsured { get; set; }

        [JsonProperty(PropertyName = "actualContentSumInsured")]
        public decimal ActualContentSumInsured { get; set; }

        [JsonProperty(PropertyName = "totalBasicPremium")]
        public decimal TotalBasicPremium { get; set; }

        [JsonProperty(PropertyName = "totalSRCCPremium")]
        public decimal TotalSRCCPremium { get; set; }

        [JsonProperty(PropertyName = "newSummInsured")]
        public decimal NewSumInsured { get; set; }

        [JsonProperty(PropertyName = "documentNumber")]
        public string DocumentNumber { get; set; }

        [JsonProperty(PropertyName = "isRiotAdded")]
        public bool IsRoitAdded { get; set; }
    }

    public class HomeCommissionResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "basicCommission")]
        public decimal BasicCommission { get; set; }

        [JsonProperty(PropertyName = "SRCCCommission")]
        public decimal SRCCCommission { get; set; }
    }

    public class VatRequest
    {
        [JsonProperty(PropertyName = "premiumAmount")]
        public decimal PremiumAmount { get; set; }

        [JsonProperty(PropertyName = "commissionAmount")]
        public decimal CommissionAmount { get; set; }
    }

    public class VatResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "vatAmount")]
        public decimal VatAmount { get; set; }

        [JsonProperty(PropertyName = "vatCommissionAmount")]
        public decimal VatCommissionAmount { get; set; }
    }

    public class DocumentDetailsRequest
    {
        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "agentcode")]
        public string AgentCode { get; set; }
    }

    public class DocumentDetails
    {
        [JsonProperty(PropertyName = "domesticNo")]
        public string DocumentNo { get; set; }

        [JsonProperty(PropertyName = "expireDate")]
        public string ExpireDate { get; set; }

        [JsonProperty(PropertyName = "policyType")]
        public string PolicyType { get; set; }

        [JsonProperty(PropertyName = "renewalCount")]
        public int RenewalCount { get; set; }
    }

    public class DocumentDetailsResult : TransactionWrapper
    {
        [JsonProperty(PropertyName = "documentDetails")]
        public List<DocumentDetails> DocumentDetails { get; set; }

        public DocumentDetailsResult()
        {
            DocumentDetails = new List<DocumentDetails>();
        }
    }

    public class AdminFetchReportRequest
    {
        [JsonProperty(PropertyName = "type")]
        public string InsuranceType { get; set; }

        [JsonProperty(PropertyName = "reportType")]
        public string ReportType { get; set; }

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentcode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "branchCode")]
        public string BranchCode { get; set; }

        [JsonProperty(PropertyName = "authorizedUserID")]
        public int AuthorizedUserID { get; set; }

        [JsonProperty(PropertyName = "dateFrom")]
        public DateTime? DateFrom { get; set; }

        [JsonProperty(PropertyName = "dateTo")]
        public DateTime? DateTo { get; set; }

        [JsonProperty(PropertyName = "ageFrom")]
        public int AgeFrom { get; set; }

        [JsonProperty(PropertyName = "ageTo")]
        public int AgeTo { get; set; }

        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }
    }

    public class BaseReportDetails
    {
        [JsonProperty(PropertyName = "policyNo")]
        public string PolicyNo { get; set; }

        [JsonProperty(PropertyName = "endorsementNo")]
        public string EndorsementNo { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "sumInsured")]
        public decimal SumInsured { get; set; }

        [JsonProperty(PropertyName = "newPremium")]
        public decimal NewPremium { get; set; }

        [JsonProperty(PropertyName = "renewalPremium")]
        public decimal RenewalPremium { get; set; }

        [JsonProperty(PropertyName = "additionalPremium")]
        public decimal AdditionalPremium { get; set; }

        [JsonProperty(PropertyName = "refundPremium")]
        public decimal RefundPremium { get; set; }

        [JsonProperty(PropertyName = "vat")]
        public decimal Vat { get; set; }
    }

    public class MotorReportDetails : BaseReportDetails
    {
        [JsonProperty(PropertyName = "branchCode")]
        public string BranchCode { get; set; }

        [JsonProperty(PropertyName = "authorizedCode")]
        public string AuthorizedCode { get; set; }

        [JsonProperty(PropertyName = "handledBy")]
        public string HandledBy { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "vehicleMake")]
        public string VehicleMake { get; set; }

        [JsonProperty(PropertyName = "vehicleType")]
        public string VehicleType { get; set; }

        [JsonProperty(PropertyName = "vehicleModel")]
        public string VehicleModel { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }
    }

    public class TravelHomeReportDetails : BaseReportDetails
    {
        [JsonProperty(PropertyName = "branchCode")]
        public string BranchCode { get; set; }

        [JsonProperty(PropertyName = "authorizedCode")]
        public string AuthorizedCode { get; set; }

        [JsonProperty(PropertyName = "handledBy")]
        public string HandledBy { get; set; }
    }

    public class MotorReportResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorReportDetails")]
        public List<MotorReportDetails> MotorReportDetails { get; set; }

        public MotorReportResponse()
        {
            MotorReportDetails = new List<MotorReportDetails>();
        }
    }

    public class TravelHomeReportResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "travelHomeReportDetails")]
        public List<TravelHomeReportDetails> TravelHomeReportDetails { get; set; }

        public TravelHomeReportResponse()
        {
            TravelHomeReportDetails = new List<TravelHomeReportDetails>();
        }
    }

    public class MainReportDetails : BaseReportDetails
    {
        [JsonProperty(PropertyName = "branchCode")]
        public string BranchCode { get; set; }

        [JsonProperty(PropertyName = "branchName")]
        public string BranchName { get; set; }

        [JsonProperty(PropertyName = "insuredName")]
        public string InsuredName { get; set; }

        [JsonProperty(PropertyName = "authorizedUser")]
        public string AuthorizedUser { get; set; }

        [JsonProperty(PropertyName = "authorizedDate")]
        public string AuthorizedDate { get; set; }

        [JsonProperty(PropertyName = "handledBy")]
        public string HandledBy { get; set; }

        [JsonProperty(PropertyName = "commenceDate")]
        public string CommenceDate { get; set; }

        [JsonProperty(PropertyName = "expiryDate ")]
        public string ExpiryDate { get; set; }

        [JsonProperty(PropertyName = "paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonProperty(PropertyName = "refundCommission")]
        public decimal RefundCommision { get; set; }

        [JsonProperty(PropertyName = "commission")]
        public decimal Commission { get; set; }

        [JsonProperty(PropertyName = "discount")]
        public decimal Discount { get; set; }

        [JsonProperty(PropertyName = "premiumLessCredit")]
        public decimal PremiumLessCredit { get; set; }

        [JsonProperty(PropertyName = "premiumRefernce")]
        public string PremiumReference { get; set; }

        [JsonProperty(PropertyName = "commissionRefernce")]
        public string CommisionReference { get; set; }

        [JsonProperty(PropertyName = "batchDate")]
        public string BatchDate { get; set; }

        [JsonProperty(PropertyName = "cpr")]
        public string CPR { get; set; }
    }

    public class MainReportResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "mainReportDetails")]
        public List<MainReportDetails> MainReportDetails { get; set; }

        public MainReportResponse()
        {
            MainReportDetails = new List<MainReportDetails>();
        }
    }

    public class AgecyProductRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class AgencyProduct
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }

        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }

        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }

        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "minimumPremium")]
        public decimal MinimumPremium { get; set; }

        [JsonProperty(PropertyName = "rate")]
        public decimal Rate { get; set; }
    }

    public class MotorProduct : AgencyProduct
    {
        [JsonProperty(PropertyName = "allowUnderAge")]
        public bool AllowUnderAge { get; set; }

        [JsonProperty(PropertyName = "underAge")]
        public int UnderAge { get; set; }

        [JsonProperty(PropertyName = "hasAgeLoading")]
        public bool HasAgeLoading { get; set; }

        [JsonProperty(PropertyName = "hasAdditionalDays")]
        public bool HasAdditionalDays { get; set; }

        [JsonProperty(PropertyName = "underAgeminPremium")]
        public decimal UnderAgeminPremium { get; set; }

        [JsonProperty(PropertyName = "allowMaxVehicleAge")]
        public bool AllowMaxVehicleAge { get; set; }

        [JsonProperty(PropertyName = "maximumVehicleAge")]
        public int MaximumVehicleAge { get; set; }

        [JsonProperty(PropertyName = "maximumVehicleValue")]
        public decimal MaximumVehicleValue { get; set; }

        [JsonProperty(PropertyName = "gccCoverRangeInYears")]
        public int GCCCoverRangeInYears { get; set; }

        [JsonProperty(PropertyName = "hasGCC")]
        public bool HasGCC { get; set; }

        [JsonProperty(PropertyName = "isProductSport")]
        public bool IsProductSport { get; set; }

        [JsonProperty(PropertyName = "policyCode")]
        public string PolicyCode { get; set; }

        [JsonProperty(PropertyName = "excessAmount")]
        public decimal ExcessAmount { get; set; }

        [JsonProperty(PropertyName = "underAgeExcessAmount")]
        public decimal UnderAgeExcessAmount { get; set; }

        [JsonProperty(PropertyName = "ageLoadingPercent")]
        public decimal AgeLoadingPercent { get; set; }

        [JsonProperty(PropertyName = "underAgeToHIR")]
        public bool UnderAgeToHIR { get; set; }

        [JsonProperty(PropertyName = "lastSeries")]
        public long LastSeries { get; set; }

        [JsonProperty(PropertyName = "seriesFormatLength")]
        public int SeriesFormatLength { get; set; }

        [JsonProperty(PropertyName = "category")]
        public List<CategoryMaster> Category { get; set; }

        [JsonProperty(PropertyName = "motorOptionalBefefits")]
        public List<MotorOptionalBenefit> MotorOptionalBenefits { get; set; }

        [JsonProperty(PropertyName = "motorClaim")]
        public List<MotorClaim> MotorClaim { get; set; }

        [JsonProperty(PropertyName = "motorEndorsementMaster")]
        public List<MotorEndorsementMaster> MotorEndorsementMaster { get; set; }

        [JsonProperty(PropertyName = "taxRate")]
        public decimal TaxRate { get; set; }

        [JsonProperty(PropertyName = "underAgeRate")]
        public decimal UnderAgeRate { get; set; }

        [JsonProperty(PropertyName = "allowUsedVehicle")]
        public bool AllowUsedVehicle { get; set; }

        [JsonProperty(PropertyName = "gulfAssitAmount")]
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
        [JsonProperty(PropertyName = "riotCoverRate")]
        public decimal RiotCoverRate { get; set; }

        [JsonProperty(PropertyName = "riotCoverMinAmount")]
        public decimal RiotCoverMinAmount { get; set; }

        [JsonProperty(PropertyName = "domesticHelperAmount")]
        public decimal DomesticHelperAmount { get; set; }

        [JsonProperty(PropertyName = "maximumHomeAge")]
        public int MaximumHomeAge { get; set; }

        [JsonProperty(PropertyName = "maximumBuildingValue")]
        public decimal MaximumBuildingValue { get; set; }

        [JsonProperty(PropertyName = "maximumContentValue")]
        public decimal MaximumContentValue { get; set; }

        [JsonProperty(PropertyName = "maximumJewelleryValue")]
        public decimal MaximumJewelleryValue { get; set; }

        [JsonProperty(PropertyName = "maximumTotalValue")]
        public decimal MaximumTotalValue { get; set; }

        [JsonProperty(PropertyName = "jewelleryCover")]
        public List<JewelleryCover> JewelleryCover { get; set; }

        [JsonProperty(PropertyName = "category")]
        public List<CategoryMaster> Category { get; set; }

        [JsonProperty(PropertyName = "homeEndorsementMaster")]
        public List<HomeEndorsementMaster> HomeEndorsementMaster { get; set; }

        [JsonProperty(PropertyName = "taxRate")]
        public decimal TaxRate { get; set; }
    }

    public class AgencyProductResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "motorProducts")]
        public List<MotorProduct> MotorProducts { get; set; }

        [JsonProperty(PropertyName = "homeProducts")]
        public List<HomeProduct> HomeProducts { get; set; }

        public AgencyProductResponse()
        {
            MotorProducts = new List<MotorProduct>();
            HomeProducts = new List<HomeProduct>();
        }
    }


    public class MotorCoverRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }
        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class MotorCoverResponse : TransactionWrapper
    {
        [JsonProperty(PropertyName = "covers")]
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
    public class MotorProductMasterResponse : TransactionWrapper
    {
        public List<MotorProductMaster> motorProductMaster { get; set; }       

        public MotorProductMasterResponse()
        {
            motorProductMaster = new List<MotorProductMaster>();
        }
    }
    public class MotorProductRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [JsonProperty(PropertyName = "agenctCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }
        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public class MotorOptionalBenefit
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "percentage")]
        public decimal Percentage { get; set; }
    }

    public class MotorClaim
    {
        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }
        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }
        [JsonProperty(PropertyName = "amountFrom")]
        public decimal AmountFrom { get; set; }
        [JsonProperty(PropertyName = "amountTo")]
        public decimal AmountTo { get; set; }
        [JsonProperty(PropertyName = "percentage")]
        public decimal Percentage { get; set; }
        [JsonProperty(PropertyName = "maximumClaimAmount")]
        public decimal MaximumClaimAmount { get; set; }
    }

    public class MotorEndorsementMaster
    {
        [JsonProperty(PropertyName = "endorsementType")]
        public string EndorsementType { get; set; }
        [JsonProperty(PropertyName = "endorsementCode")]
        public string EndorsementCode { get; set; }
        [JsonProperty(PropertyName = "chargeAmount")]
        public decimal ChargeAmount { get; set; }
        [JsonProperty(PropertyName = "HasCommission")]
        public bool HasCommission { get; set; }
    }

    public class JewelleryCover
    {
        [JsonProperty(PropertyName = "keyType")]
        public string KeyType { get; set; }
        [JsonProperty(PropertyName = "valueType")]
        public string ValueType { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }
        [JsonProperty(PropertyName = "rate")]
        public decimal Rate { get; set; }
    }
    public class HomeEndorsementMaster
    {
        [JsonProperty(PropertyName = "endorsementType")]
        public string EndorsementType { get; set; }
        [JsonProperty(PropertyName = "endorsementCode")]
        public string EndorsementCode { get; set; }
        [JsonProperty(PropertyName = "chargeAmount")]
        public decimal ChargeAmount { get; set; }
        [JsonProperty(PropertyName = "HasCommission")]
        public bool HasCommission { get; set; }
    }
    public class HomeProductRequest
    {

        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [JsonProperty(PropertyName = "agenctCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "mainClass")]
        public string MainClass { get; set; }
        [JsonProperty(PropertyName = "subClass")]
        public string SubClass { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
    public class PolicyRecord
    {
        [JsonProperty(PropertyName = "documentNumber")]
        public string DocumentNumber { get; set; }
        [JsonProperty(PropertyName = "newHomeID")]
        public long NewHomeID { get; set; }
        [JsonProperty(PropertyName = "newMotorID")]
        public long NewMotorID { get; set; }
        [JsonProperty(PropertyName = "linkID")]
        public string LinkID { get; set; }
        [JsonProperty(PropertyName = "isInserted")]
        public bool IsInserted { get; set; }
    }

    public class AgencyPolicyDetailsRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
    }

    public class RenewalPrecheckRequest
    {
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "documentNo")]
        public string DocumentNo { get; set; }
        [JsonProperty(PropertyName = "insuranceType")]
        public string InsuranceType { get; set; }
        [JsonProperty(PropertyName = "renewalCount")]
        public int CurrentRenewalCount { get; set; }
        [JsonProperty(PropertyName = "policyStartDate")]
        public DateTime? PolicyStartDate{ get; set; }
        [JsonProperty(PropertyName = "policyExpireDate")]
        public DateTime? PolicyExpireDate { get; set; }

    }

    public class RenewalPrecheckResponse : TransactionWrapper
    {        
        [JsonProperty(PropertyName = "isAlreadyRenewed")]
        public bool IsAlreadyRenewed { get; set; }        
    }

}