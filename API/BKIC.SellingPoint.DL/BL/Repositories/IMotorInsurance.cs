using BKIC.SellingPoint.DL.BO;
using System;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IMotorInsurance
    {
        MotorInsuranceQuoteResponse GetMotorInsuranceQuote(MotorInsuranceQuote motorInsurance);
        MotorInsurancePolicyResponse PostMotorInsurance(MotorInsurancePolicy policy);
        ExcessAmountResponse GetExcessCalcualtion(ExcessAmountRequest request); 
        AdminFetchMotorDetailsResponse FetchMotorPolicyDetails(AdminFetchMotorDetailsRequest request);
        AgencyMotorPolicyResponse GetMotorAgencyPolicy(AgencyMotorRequest req);
        AgencyMotorPolicyResponse GetMotorPoliciesByTypeByCPR(AgencyMotorRequest req);
        OptionalCoverResponse GetOptionalCover(OptionalCoverRequest req);
        CalculateCoverAmountResponse CalculateOptionalCoverAmount(CalculateCoverAmountRequest req);
        MotorSavedQuotationResponse GetSavedMotorPolicy(string documentNo, string type, string agentCode,
                                                        bool isEndorsement = false, long endorsementID = 0, int renewalCount = 0);
        InsuranceCertificateResponse GetInsuranceCertificate(string documentNo, string type, string agentCode,
                                                        bool isEndorsement = false, long endorsementID = 0, int renewalCount = 0);

        MotorSavedQuotationResponse GetOracleRenewMotorPolicy(string documentNo, string agency, string agentCode);

        AgencyMotorPolicyResponse GetMotorPoliciesByDocumentNo(AgencyPolicyDetailsRequest req);

        MotorSavedQuotationResponse GetRenewMotorPolicy(string documentNo, string agency, string agentCode, int renewalCount);
        AgencyMotorPolicyResponse GetMotorPoliciesEndorsement(AgencyMotorRequest req);
        AgencyMotorPolicyResponse GetOracleMotorRenewalPolicies(AgencyMotorRequest req);

    }
}