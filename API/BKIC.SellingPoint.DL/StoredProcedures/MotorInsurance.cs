using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class MotorInsuranceSP
    {       
        public const string GetPerimium = "CalculateMotorPremium";
        public const string PostPolicy = "SP_InsertMotorPolicy";
        public const string QuotationBoard = "GetMyMotorSavedPolicy";
        public const string UpdatePolicy = "SP_updateMotorPolicy";
        public const string GetQuote = "CalculateMotorQuotation";        
        public const string ExcessAmount = "ExcessCalculation";       
        public const string FetchRenewDetails = "FetchMotorRenewalDetails";
        public const string RenewalPrecheck = "Web_RenewalPolicyPrecheck";
        public const string Updatedeliverydetails = "UpdateMotorDeliveryDetails";
        public const string MigRenewDetails = "MIG_RenewDetailsOracleToSQLByPolicy";
        public const string DeleteDueRenewal = "DeleteDueRenewal";
        public const string FetchInsuranceCertificate = "FetchInsuranceCertificate";
        public const string GetMotorPolicyByDocNo = "SP_GetMyMotorQuotationsByDocumentNo";
        public const string GetOracleMotorRenewalPolicyByDocNo = "SP_GetOracleMotorRenewalByDocumentNo";
        public const string GetMotorRenewalPolicyByDocNo = "SP_GetRenewalMotorByDocumentNo";
        public const string GetMotorAgencyPolicy = "SP_GetMotorAgencyPolicy";
        public const string GETPoliciesByTypeByCPR = "SP_GETPoliciesByTypeByCPR";       
        public const string GetEndorsementSchedule = "sp_CreateMotorEndorsementSchedule";
        public const string GetOptionalCovers = "SP_GetOptionalCovers";
        public const string CalculateOptionalCoverAmount = "SP_CalculateOptionalCoverAmount";
        public const string MotorInsert = "SP_InsertMotor";
        public const string MotorRenewalInsert = "SP_InsertMotorRenewal";
        public const string PolicyCategoryInsert = "SP_PolicyCategory";
        public const string GETMotorPoliciesByDocumentNo = "SP_GETMotorPoliciesByDocumentNo";
        public const string GetMotorPoliciesEndorsement = "SP_GETMotorPoliciesEndorsement";
        public const string GetOracleMotorRenewalPolicies = "SP_GetOracleMotorRenewalPolicies";

    }
}
