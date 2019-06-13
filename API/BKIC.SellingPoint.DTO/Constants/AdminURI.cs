namespace BKIC.SellingPoint.DTO.Constants
{
    public class AdminURI
    {
        public const string CDUInsuranceProductMaster = "api/admin/CDUInsuranceProductMaster";
        public const string BranchDetailsOperation = "api/admin/BranchDetailsOperation";
        public const string MotorCoverMasterOperation = "api/admin/MotorCoverMasterOperation";
        public const string MotorProductCoverOperation = "api/admin/MotorProductCoverOperation";
        public const string AgentOperation = "api/admin/AgentOperation";
        public const string UserOperation = "api/admin/UserOperation";
        public const string CategoryMasterOperation = "api/admin/CategoryMasterOperation";
        public const string InsuredMasterOperation = "api/admin/InsuredMasterOperation";
        public const string GetAgencyInsured = "api/admin/GetAgencyInsured";
        public const string GetAgencyUsers = "api/admin/GetAgencyUsers";
        public const string GetMasterTableByTableName = "api/admin/GetMasterTableByTableName/{tableName}";
        //public const string FetchUserDetailsByCPR = "api/user/fetchdetailscpr/{cpr}";
        //public const string FetchUserDetailsByCPRInsuredCode = "api/user/fetchdetailscprinsuredCode/{cpr}/{insuredCode}";
        public const string FetchUserDetailsByCPRInsuredCode = "api/user/fetchdetailscprinsuredCode";
        public const string FetchMotorPolicyDetails = "api/admin/fetchmotorpolicies";
        public const string FetchDomesticPolicyDetails = "api/admin/fetchdomesticpolicies";
        public const string FetchHomePolicyDetails = "api/admin/fetchhomepolicies";
        public const string FetchTravelPolicyDetails = "api/admin/fetchtravelpolicies";
        public const string CalculateCommission = "api/admin/calculateCommision";
        public const string FetchDocumentDetailsByCPR = "api/admin/getdocumentsbycpr/{cpr}/{agentcode}";
        public const string FetchAgencyProductByType = "api/admin/getagencyproductbytpe";
        public const string MotorCoverOperation = "api/admin/motorcoveroperation";
        public const string MotorVehicleOperation = "api/admin/motorvehicleoperation";
        public const string MotorProductOperation = "api/admin/motorproductoperation";
        public const string MotorYearOperation = "api/admin/motoryearoperation";
        public const string MotorEngineCCOperation = "api/admin/motorengineccoperation";
        public const string GetAgencyCPR = "api/admin/getagencycpr/{CPR}/{Agency}";
        public const string RenewalPrecheckByInsuranceType = "api/admin/renewalprecheck";
    }
}