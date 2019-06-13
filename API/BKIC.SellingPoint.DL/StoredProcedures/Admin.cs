namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class AdminSP
    {
        public const string FetchInformation = "SP_FetchInformationForAdmin";
        public const string BranchDetailsOperation = "SP_BranchMaster";
        public const string InsuranceMasterOperation = "SP_FetchInsuranceMaster";
        public const string MotorCoverMasterOperation = "SP_MotorCoverMaster";
        public const string MotorProductCoverOperation = "SP_MotorProductCover";
        public const string MotorVehicleMasterOperation = "SP_MotorVehicleMaster";
        public const string AgentMasterOperation = "SP_AgentMaster";
        public const string CategoryMasterOperation = "SP_CategoryMaster";
        public const string InsuredMasterOperation = "SP_InsuredMasterDetail";
        public const string UserMasterOperation = "SP_InsertUserMaster";
        public const string GetAgencyInsured = "SP_GetAgencyInsured";
        public const string GetAgencyUsers = "SP_GetAgencyUsers";
        public const string GetAgencyCPR = "SP_GetAgencyCPR";

        //public const string FetchUserDetailsByCPR = "FetchUserDetailsByCPR";
        public const string FetchUserDetailsByCPRInsuredCode = "SP_FetchUserDetailsByCPRInsuredCode";

        public const string FetchMotorDetails = "SP_Admin_FetchMotorPolicy";
        public const string FetchTravelDetails = "SP_Admin_FetchTravelPolicy";
        public const string FetchHomeDetails = "SP_Admin_FetchHomePolicy";
        public const string FetchDomesticDetails = "SP_Admin_FetchDomesticPolicy";
        public const string CalculateCommision = "SP_Admin_CalculateCommission";
        public const string GetDocumentsByCPR = "SP_GetPolicyByCPR";
        public const string EndorsementPreCheck = "SP_EndorsementPreCheck";
        public const string GETPoliciesByTypeByCPR = "SP_GETPoliciesByTypeByCPR";
        public const string GETAgencyProductByType = "SP_GetAgencyProductsByType";
        public const string GETVAT = "SP_GetVat";
        public const string GetMotorProductCover = "SP_GetMotorProductCover";
        public const string MotorProductMaster = "SP_MotorProductMasetrOperation";
        public const string MotorYearMaster = "SP_MotorYearMasterOperation";
        public const string MotorEngineCCMaster = "SP_MotorEngineCCMasterOperation";
        public const string GetMotorProduct = "SP_GetMotorProduct";
        public const string GetHomeProduct = "SP_GetHomeProduct";
        public const string RenewalPrecheck = "SP_RenewalPrechek";
    }
}