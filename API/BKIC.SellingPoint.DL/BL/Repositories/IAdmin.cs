using BKIC.SellingPoint.DL.BO;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IAdmin
    {
        MasterTableResponse<T> GetMasterTableByTableName<T>(string tableName) where T : class;
        InsuranceProductMasterResponse InsuranceProductOperation(InsuranceProductMaster request);
        BranchMasterResponse BranchOperation(BranchMaster details);
        MotorCoverMasterResponse MotorCoverOperation(MotorCoverMaster details);
        MotorProductCoverResponse MotorProductCoverOperation(MotorProductCover details);
        AgentMasterResponse AgentOperation(AgentMaster details);
        UserMasterDetailsResponse UserOperation(UserMasterDetails details);
        CategoryMasterResponse CategoryOperation(CategoryMaster request);
        InsuredMasterDetailsResponse InsuredOperation(InsuredMasterDetails request);
        //InsuredResult GetUserDetails(UserRequest request);
        InsuredResponse GetInsuredDetailsByCPRInsuredCode(InsuredRequest request);
        AgencyInsuredResponse GetAgencyInsured(AgencyInsuredRequest request);
        AgencyUserResponse GetAgencyUser(AgencyUserRequest request);
        DocumentDetailsResponse GetDocumentsByCPR(DocumentDetailsRequest request);
        AgencyProductResponse GetAgencyProducts(AgecyProductRequest request);
        MotorCoverResponse GetProductCover(MotorCoverRequest request);
        MotorVehicleMasterResponse MotorVehicleMasterOperation(MotorVehicleMaster details);
        MotorProductMasterResponse MotorProductMasterOperation(MotorProductMaster details);
        MotorYearMasterResponse MotorYearMasterOperation(MotorYearMaster details);
        MotorEngineCCResponse MotorEngineCCOperation(MotorEngineCCMaster details);
        MotorProductMasterResponse GetMotorProduct(MotorProductRequest req);
        HomeProductResponse GetHomeProduct(HomeProductRequest req);
        AgencyInsuredResponse GetAgencyCPR(string CPR, string Agency);
        RenewalPrecheckResponse RenewalPrecheck(RenewalPrecheckRequest request);
    }
}