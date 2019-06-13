using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class ReportSP
    {

        public const string GetMotorAgeReport = "SP_ReportMotorPolicyPerAge";
        public const string GetMotorBranchReport = "SP_ReportMotorPolicyByBranch";
        public const string GetMotorUserReport = "SP_ReportMotorPolicyByAuthorizedUser";
        public const string GetMotorVehicleReport = "SP_ReportMotorPolicyByVehicleMake";


        public const string GetTravelUserReport = "SP_ReportTravelPolicyByAuthorizedUser";
        public const string GetTravelBranchReport = "SP_ReportTravelPolicyByBranch";


        public const string GetHomeUserReport = "SP_ReportHomePolicyByAuthorizedUser";
        public const string GetHomeBranchReport = "SP_ReportHomePolicyByBranch";

        public const string GetHomeMainReport = "SP_ReportHomeMain";
        public const string GetTravelMainReport = "SP_ReportTravelMain";
        public const string GetMotorMainReport = "SP_ReportMotorMain";
    }
}
