using BKIC.SellingPoint.DL.BO;
using System;
using System.Data;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IInsurancePortal
    {
        #region MotorInsurance
        AdminFetchMotorDetailsResponse FetchMotorReportDetails(AdminFetchMotorDetailsRequest request);
        #endregion MotorInsurance
        #region HomeInsurance
        AdminFetchHomeDetailsResponse FetchHomePolicyDetails(AdminFetchHomeDetailsRequest request);
        #endregion HomeInsurance
        #region TravelInsurance
        AdminFetchTravelDetailsResponse FetchTravelPolicyDetails(AdminFetchTravelDetailsRequest request);
        #endregion TravelInsurance
        #region TravelInsurance
        AdminFetchDomesticDetailsResponse FetchDomesticPolicyDetails(AdminFetchDomesticDetailsRequest request);
        #endregion TravelInsurance
        MotorDetailsPortalResponse GetMotorDetails(long MotorID, string InsuredCode);
        UpdateHIRStatusResponse UpdateHIRStatusCode(UpdateHIRStatusRequest request);
        TransactionWrapper UploadHIRDocuments(DataTable uploadDocuments, string insuredCode, string documentNo, string linkId,
         string insuredType, string refID);
        FetchDocumentsResponse FetchHIRDocuments(FetchDocumentsRequest req);
        DocumentsUploadPrecheckResponse PrecheckHIRDocumentUpload(string insuredCode, string documentNo, string linkID,
        string insuredType);
        EmailMessageAuditResult GetEmailMessageForRecord(string insuredCode, string policyNo, string linkId);
        DashboardResponse GetPortalDashboard(DateTime fromdate, DateTime todate, string agentCode, string agent, string branchCode);
        FetchUserDetailsResponse FetchUserDetails(FetchUserDetailsRequest request);
        ResetPasswordResult ResetPassword(string newPassword, string insuredCode);
        ChangeUserStatusResponse ChangeUserStatus(ChangeUserStatusRequest prequest);       
        CommissionResponse GetCommission(CommissionRequest request);
        HomeCommissionResponse GetHomeEndorsementCommission(HomeCommissionRequest request);
        HomeCommissionResponse GetHomePolicyCommission(HomeCommissionRequest request);
        VatResponse GetVat(VatRequest request);
        UpdateHIRRemarksResponse UpdateHIRRemarks(UpdateHIRRemarksRequest request);
    }
}