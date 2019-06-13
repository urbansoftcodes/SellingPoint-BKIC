using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    /// <summary>
    /// Portal methods implementation - HIR Policies, Active policies...
    /// </summary>
    public class InsurancePortal : IInsurancePortal
    {
        //public readonly IMail _mail;

        public InsurancePortal()
        {
            //_mail = new Implementation.Mail();
        }

        /// <summary>
        /// Fetch the motor policies by type(Active or HIR).
        /// </summary>
        /// <param name="request">Search the policies by branch or agency or user.</param>
        /// <returns>List of motor policy details.</returns>
        public AdminFetchMotorDetailsResponse FetchMotorReportDetails(AdminFetchMotorDetailsRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Type",string.IsNullOrEmpty(request.Type)?"":request.Type),
                     new SqlParameter("@ByAgencyCode", request.ByAgencyCode),
                    new SqlParameter("@ByDocumentNo", request.ByDocumentNo),
                    new SqlParameter("@ByHIRStatus", request.ByHIRStatus),
                    new SqlParameter("@ByStatusAndAgency", request.ByStatusAndAgency),
                    new SqlParameter("@All", request.All),
                    new SqlParameter("@AgencyCode",string.IsNullOrEmpty(request.AgencyCode)?"":request.AgencyCode),
                    new SqlParameter("@DocumentNo",string.IsNullOrEmpty(request.DocumentNo)?"":request.DocumentNo),
                };
                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.FetchMotorDetails, para);
                List<MotorPolicyDetails> policylist = new List<MotorPolicyDetails>();
                if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "HIR")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        MotorPolicyDetails detail = new MotorPolicyDetails()
                        {
                            MotorID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            InsuredCode = dr.IsNull("InsuredCode") ? string.Empty : Convert.ToString("InsuredCode"),
                            HIRReason = dr.IsNull("HIRReason") ? string.Empty : Convert.ToString("HIRReason"),
                            HIRStatus = dr.IsNull("HIRStatus") ? string.Empty : Convert.ToString("HIRStatus"),
                            HIRStatusDesc = dr.IsNull("HIRStatusID") ? string.Empty : Convert.ToString("HIRStatusID"),
                            GrossPremium = dr.IsNull("GROSSPREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["GROSSPREMIUM"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            IsMessageAvailable = dr.IsNull("IsMessageAvailable") ? false : Convert.ToBoolean(dr["IsMessageAvailable"]),
                            IsDocumentsAvailable = dr.IsNull("IsDocumentAvailable") ? false : Convert.ToBoolean(dr["IsDocumentAvailable"]),
                            MainClass = dr.IsNull("MAINCLASS") ? string.Empty : Convert.ToString(dr["MAINCLASS"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"])
                        };
                        policylist.Add(detail);
                    }
                }
                else if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "Active")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        MotorPolicyDetails detail = new MotorPolicyDetails()
                        {
                            MotorID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            GrossPremium = dr.IsNull("GROSSPREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["GROSSPREMIUM"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            PaymentDate = dr.IsNull("PAYMENTDATE") ? DateTime.Now : Convert.ToDateTime(dr["PAYMENTDATE"]),
                            PaymentType = dr.IsNull("PAYMENT_TYPE") ? string.Empty : Convert.ToString(dr["PAYMENT_TYPE"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"])
                        };
                        policylist.Add(detail);
                    }
                }
                return new AdminFetchMotorDetailsResponse
                {
                    MotorDetails = policylist,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AdminFetchMotorDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Fetch the home policies by type(Active or HIR).
        /// </summary>
        /// <param name="request">Search the policies by branch or agency or user.</param>
        /// <returns>List of home policy details.</returns>
        public AdminFetchHomeDetailsResponse FetchHomePolicyDetails(AdminFetchHomeDetailsRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Type",string.IsNullOrEmpty(request.Type)?"":request.Type),
                     new SqlParameter("@ByAgencyCode", request.ByAgencyCode),
                    new SqlParameter("@ByDocumentNo", request.ByDocumentNo),
                    new SqlParameter("@ByHIRStatus", request.ByHIRStatus),
                    new SqlParameter("@ByStatusAndAgency", request.ByStatusAndAgency),
                    new SqlParameter("@All", request.All),
                    new SqlParameter("@AgencyCode",string.IsNullOrEmpty(request.AgencyCode)?"":request.AgencyCode),
                    new SqlParameter("@DocumentNo",string.IsNullOrEmpty(request.DocumentNo)?"":request.DocumentNo),
                    new SqlParameter("@HIRStatus",request.HIRStatus),
                };
                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.FetchHomeDetails, para);
                List<HomePolicyDetails> policylist = new List<HomePolicyDetails>();
                if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "HIR")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        HomePolicyDetails detail = new HomePolicyDetails()
                        {
                            HomeID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            LinkID = dr.IsNull("LINKID") ? string.Empty : Convert.ToString(dr["LINKID"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            InsuredCode = dr.IsNull("InsuredCode") ? string.Empty : Convert.ToString(dr["InsuredCode"]),
                            HIRReason = dr.IsNull("HIRReason") ? string.Empty : Convert.ToString(dr["HIRReason"]),
                            HIRStatus = dr.IsNull("HIRStatus") ? string.Empty : Convert.ToString(dr["HIRStatus"]),
                            HIRStatusDesc = dr.IsNull("StatusID") ? string.Empty : Convert.ToString(dr["StatusID"]),
                            GrossPremium = dr.IsNull("NETPREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["NETPREMIUM"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            Agency = dr.IsNull("Agency") ? string.Empty : Convert.ToString(dr["Agency"]),
                            IsMessageAvailable = dr.IsNull("IsMessageAvailable") ? false : Convert.ToBoolean(dr["IsMessageAvailable"]),
                            IsDocumentsAvailable = dr.IsNull("IsDocumentAvailable") ? false : Convert.ToBoolean(dr["IsDocumentAvailable"]),
                            MainClass = dr.IsNull("MAINCLASS") ? string.Empty : Convert.ToString(dr["MAINCLASS"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"]),
                            RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"])
                        };
                        policylist.Add(detail);
                    }
                }
                if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "Active")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        HomePolicyDetails detail = new HomePolicyDetails()
                        {
                            HomeID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            LinkID = dr.IsNull("LINKID") ? string.Empty : Convert.ToString(dr["LINKID"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]),
                            GrossPremium = dr.IsNull("PREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["PREMIUM"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            Agency = dr.IsNull("AGENCY") ? string.Empty : Convert.ToString(dr["AGENCY"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            IsMessageAvailable = dr.IsNull("IsMessageAvailable") ? false : Convert.ToBoolean(dr["IsMessageAvailable"]),
                            IsDocumentsAvailable = dr.IsNull("IsDocumentAvailable") ? false : Convert.ToBoolean(dr["IsDocumentAvailable"]),
                            MainClass = dr.IsNull("MAINCLASS") ? string.Empty : Convert.ToString(dr["MAINCLASS"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"]),
                            RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"])
                        };
                        policylist.Add(detail);
                    }
                }
                return new AdminFetchHomeDetailsResponse
                {
                    HomeDetails = policylist,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AdminFetchHomeDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Fetch the travel policies by type(Active or HIR).
        /// </summary>
        /// <param name="request">Search the policies by branch or agency or user.</param>
        /// <returns>List of travel policy details.</returns>
        public AdminFetchTravelDetailsResponse FetchTravelPolicyDetails(AdminFetchTravelDetailsRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Type",string.IsNullOrEmpty(request.Type)?"":request.Type),
                     new SqlParameter("@ByAgencyCode", request.ByAgencyCode),
                    new SqlParameter("@ByDocumentNo", request.ByDocumentNo),
                    new SqlParameter("@ByHIRStatus", request.ByHIRStatus),
                    new SqlParameter("@ByStatusAndAgency", request.ByStatusAndAgency),
                    new SqlParameter("@All", request.All),
                    new SqlParameter("@AgencyCode",string.IsNullOrEmpty(request.AgencyCode)?"":request.AgencyCode),
                    new SqlParameter("@DocumentNo",string.IsNullOrEmpty(request.DocumentNo)?"":request.DocumentNo),
                    new SqlParameter("@HIRStatus", request.HIRStatus),
                };
                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.FetchTravelDetails, para);
                List<TravelPolicyDetails> policylist = new List<TravelPolicyDetails>();
                if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "HIR")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        TravelPolicyDetails detail = new TravelPolicyDetails()
                        {
                            ID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]),
                            LinkID = dr.IsNull("LinkID") ? string.Empty : Convert.ToString(dr["LinkID"]),
                            AuthorizationCode = dr.IsNull("AuthorizationCode") ? string.Empty : Convert.ToString(dr["AuthorizationCode"]),
                            TransactionDate = dr.IsNull("TransactionDate") ? DateTime.Now : Convert.ToDateTime(dr["TransactionDate"]),
                            NetPremium = dr.IsNull("NETPREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["NETPREMIUM"]),
                            Source = dr.IsNull("Source") ? string.Empty : Convert.ToString(dr["Source"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            Agency = dr.IsNull("AGENCY") ? string.Empty : Convert.ToString(dr["AGENCY"]),
                            IsMessageAvailable = dr.IsNull("IsMessageAvailable") ? false : Convert.ToBoolean(dr["IsMessageAvailable"]),
                            IsDocumentsAvailable = dr.IsNull("IsDocumentAvailable") ? false : Convert.ToBoolean(dr["IsDocumentAvailable"]),
                            MainClass = dr.IsNull("MAINCLASS") ? string.Empty : Convert.ToString(dr["MAINCLASS"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"]),
                            HIRStatusDesc = dr.IsNull("StatusDescription") ? string.Empty : Convert.ToString(dr["StatusDescription"]),
                            Status = dr.IsNull("Status") ? string.Empty : Convert.ToString(dr["Status"]),
                            HIRStatus = dr.IsNull("HIRStatus") ? string.Empty : Convert.ToString(dr["HIRStatus"]),
                        };
                        policylist.Add(detail);
                    }
                }
                if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "Active")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        TravelPolicyDetails detail = new TravelPolicyDetails()
                        {
                            ID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]),
                            LinkID = dr.IsNull("LinkID") ? string.Empty : Convert.ToString(dr["LinkID"]),
                            AuthorizationCode = dr.IsNull("AuthorizationCode") ? string.Empty : Convert.ToString(dr["AuthorizationCode"]),
                            TransactionDate = dr.IsNull("TransactionDate") ? DateTime.Now : Convert.ToDateTime(dr["TransactionDate"]),
                            NetPremium = dr.IsNull("NETPREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["NETPREMIUM"]),
                            Source = dr.IsNull("Source") ? string.Empty : Convert.ToString(dr["Source"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            Agency = dr.IsNull("AGENCY") ? string.Empty : Convert.ToString(dr["AGENCY"]),
                            IsMessageAvailable = dr.IsNull("IsMessageAvailable") ? false : Convert.ToBoolean(dr["IsMessageAvailable"]),
                            IsDocumentsAvailable = dr.IsNull("IsDocumentAvailable") ? false : Convert.ToBoolean(dr["IsDocumentAvailable"]),
                            MainClass = dr.IsNull("MAINCLASS") ? string.Empty : Convert.ToString(dr["MAINCLASS"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"])
                        };
                        policylist.Add(detail);
                    }
                }
                return new AdminFetchTravelDetailsResponse
                {
                    TravelDetails = policylist,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AdminFetchTravelDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        // <summary>
        /// Fetch the deomestichelp policies by type(Active or HIR).
        /// </summary>
        /// <param name="request">Search the policies by branch or agency or user.</param>
        /// <returns>List of domestichelp policy details.</returns>
        public AdminFetchDomesticDetailsResponse FetchDomesticPolicyDetails(AdminFetchDomesticDetailsRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Type",string.IsNullOrEmpty(request.Type)?"":request.Type),
                    new SqlParameter("@ByAgencyCode", request.ByAgencyCode),
                    new SqlParameter("@ByDocumentNo", request.ByDocumentNo),
                    new SqlParameter("@ByHIRStatus", request.ByHIRStatus),
                    new SqlParameter("@ByStatusAndAgency", request.ByStatusAndAgency),
                    new SqlParameter("@All", request.All),
                    new SqlParameter("@AgencyCode",string.IsNullOrEmpty(request.AgencyCode)?"":request.AgencyCode),
                    new SqlParameter("@DocumentNo",string.IsNullOrEmpty(request.DocumentNo)?"":request.DocumentNo),
                    new SqlParameter("@HIRStatus", request.HIRStatus),
                };
                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.FetchDomesticDetails, para);
                List<DomesticInsurancePolicyDetails> policylist = new List<DomesticInsurancePolicyDetails>();
                if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "HIR")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        DomesticInsurancePolicyDetails detail = new DomesticInsurancePolicyDetails()
                        {
                            ID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]),
                            LinkID = dr.IsNull("LinkID") ? string.Empty : Convert.ToString(dr["LinkID"]),
                            AuthorizationCode = dr.IsNull("AuthorizationCode") ? string.Empty : Convert.ToString(dr["AuthorizationCode"]),
                            TransactionDate = dr.IsNull("CommenceDate") ? DateTime.Now : Convert.ToDateTime(dr["CommenceDate"]),
                            NetPremium = dr.IsNull("NETPREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["NETPREMIUM"]),
                            Source = dr.IsNull("Source") ? string.Empty : Convert.ToString(dr["Source"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            Agency = dr.IsNull("AGENCY") ? string.Empty : Convert.ToString(dr["AGENCY"]),
                            IsMessageAvailable = dr.IsNull("IsMessageAvailable") ? false : Convert.ToBoolean(dr["IsMessageAvailable"]),
                            IsDocumentsAvailable = dr.IsNull("IsDocumentAvailable") ? false : Convert.ToBoolean(dr["IsDocumentAvailable"]),
                            MainClass = dr.IsNull("MAINCLASS") ? string.Empty : Convert.ToString(dr["MAINCLASS"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"]),
                            HIRStatusDesc = dr.IsNull("StatusDescription") ? string.Empty : Convert.ToString(dr["StatusDescription"]),
                            HIRStatus = dr.IsNull("HIRStatus") ? string.Empty : Convert.ToString(dr["HIRStatus"]),
                            Status = dr.IsNull("Status") ? string.Empty : Convert.ToString(dr["Status"])
                        };
                        policylist.Add(detail);
                    }
                }
                if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "Active")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        DomesticInsurancePolicyDetails detail = new DomesticInsurancePolicyDetails()
                        {
                            ID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]),
                            LinkID = dr.IsNull("LinkID") ? string.Empty : Convert.ToString(dr["LinkID"]),
                            AuthorizationCode = dr.IsNull("AuthorizationCode") ? string.Empty : Convert.ToString(dr["AuthorizationCode"]),
                            TransactionDate = dr.IsNull("TransactionDate") ? DateTime.Now : Convert.ToDateTime(dr["TransactionDate"]),
                            NetPremium = dr.IsNull("NETPREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["NETPREMIUM"]),
                            Source = dr.IsNull("Source") ? string.Empty : Convert.ToString(dr["Source"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            Agency = dr.IsNull("AGENCY") ? string.Empty : Convert.ToString(dr["AGENCY"]),
                            IsMessageAvailable = dr.IsNull("IsMessageAvailable") ? false : Convert.ToBoolean(dr["IsMessageAvailable"]),
                            IsDocumentsAvailable = dr.IsNull("IsDocumentAvailable") ? false : Convert.ToBoolean(dr["IsDocumentAvailable"]),
                            MainClass = dr.IsNull("MAINCLASS") ? string.Empty : Convert.ToString(dr["MAINCLASS"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"]),
                            HIRStatusDesc = dr.IsNull("StatusDescription") ? string.Empty : Convert.ToString(dr["StatusDescription"]),
                            HIRStatus = dr.IsNull("HIRStatus") ? string.Empty : Convert.ToString(dr["HIRStatus"]),
                            Status = dr.IsNull("Status") ? string.Empty : Convert.ToString(dr["Status"])
                        };
                        policylist.Add(detail);
                    }
                }
                return new AdminFetchDomesticDetailsResponse
                {
                    DomesticDetails = policylist,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AdminFetchDomesticDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Update the HIR status to pending to  approve.
        /// Only BKIC admin to this.
        /// </summary>
        /// <param name="request">HIR status update request.</param>
        /// <returns>Status updated or not.</returns>
        public UpdateHIRStatusResponse UpdateHIRStatusCode(UpdateHIRStatusRequest request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@ID",request.ID),
                    new  SqlParameter("@PolicyNo",request.DocumentNo),
                    new  SqlParameter("@HIRStatusCode",request.HIRStatusCode),
                    new  SqlParameter("@InsuranceType",request.InsuranceType)
                };
                BKICSQL.enq(PortalSP.UpdateHIRStatus, paras);
                return new UpdateHIRStatusResponse
                {
                    IsTransactionDone = true,
                    IsMailSend = false
                };
            }
            catch (Exception ex)
            {
                return new UpdateHIRStatusResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get the motor policies by motorid and insuredcode.
        /// </summary>
        /// <param name="MotorID">Motor id.</param>
        /// <param name="InsuredCode">Insured code.</param>
        /// <returns>Motor policy details.</returns>
        public MotorDetailsPortalResponse GetMotorDetails(long MotorID, string InsuredCode)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@MotorId",MotorID),
                    new SqlParameter("@InsuredCode",InsuredCode)
                };
                DataSet motords = BKICSQL.eds(PortalSP.GetMotorDetails, paras);
                MotorDetailsPortalResponse result = new MotorDetailsPortalResponse();

                if (motords != null && motords.Tables[0] != null && motords.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = motords.Tables[0].Rows[0];
                    result.MotorInsurancePolicy.DocumentNo = dr.IsNull("DocumentNo") ? string.Empty : Convert.ToString(dr["DocumentNo"]);
                    result.MotorInsurancePolicy.InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]);
                    result.MotorInsurancePolicy.InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]);
                    result.MotorInsurancePolicy.ExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                    result.MotorInsurancePolicy.YearOfMake = dr.IsNull("YEAR") ? 0 : Convert.ToInt32(dr["YEAR"]);
                    result.MotorInsurancePolicy.VehicleMake = dr.IsNull("MAKE") ? string.Empty : Convert.ToString(dr["MAKE"]);
                    result.MotorInsurancePolicy.VehicleModel = dr.IsNull("MODEL") ? string.Empty : Convert.ToString(dr["MODEL"]);
                    result.MotorInsurancePolicy.vehicleTypeCode = dr.IsNull("VEHICLETYPE") ? string.Empty : Convert.ToString(dr["VEHICLETYPE"]);
                    result.MotorInsurancePolicy.PolicyCode = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]);
                    result.MotorInsurancePolicy.IsNCB = dr.IsNull("NCBYEARS") ? true : false;
                    result.MotorInsurancePolicy.NCBStartDate = dr.IsNull("NCBFROMDATE") ? (DateTime?)null : Convert.ToDateTime(dr["NCBFROMDATE"]);
                    result.MotorInsurancePolicy.NCBEndDate = dr.IsNull("NCBTODATE") ? (DateTime?)null : Convert.ToDateTime(dr["NCBTODATE"]);
                    result.MotorInsurancePolicy.VehicleValue = dr.IsNull("VEHICLEVALUE") ? 0 : Convert.ToDecimal(dr["VEHICLEVALUE"]);
                    result.MotorInsurancePolicy.PremiumAmount = dr.IsNull("PREMIUMAFTERDISCOUNT") ? 0 : Convert.ToDecimal(dr["PREMIUMAFTERDISCOUNT"]);
                    result.MotorInsurancePolicy.PolicyCommencementDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                    result.MotorInsurancePolicy.RegistrationNumber = dr.IsNull("REGISTRATIONNO") ? string.Empty : Convert.ToString(dr["REGISTRATIONNO"]);
                    result.MotorInsurancePolicy.ChassisNo = dr.IsNull("CHASSISNO") ? string.Empty : Convert.ToString(dr["CHASSISNO"]);
                    result.MotorInsurancePolicy.EngineCC = dr.IsNull("TONNAGE") ? 0 : Convert.ToInt32(dr["TONNAGE"]);
                    result.MotorInsurancePolicy.FinancierCompanyCode = dr.IsNull("FINANCECOMPANY") ? string.Empty : Convert.ToString(dr["FINANCECOMPANY"]);
                    result.MotorInsurancePolicy.ExcessType = dr.IsNull("EXCESSTYPE") ? string.Empty : Convert.ToString(dr["EXCESSTYPE"]);
                    result.MotorInsurancePolicy.ExcessAmount = dr.IsNull("EXCESSAMOUNT") ? 0 : Convert.ToDecimal(dr["EXCESSAMOUNT"]);
                    result.MotorInsurancePolicy.Branch = dr.IsNull("Source") ? string.Empty : Convert.ToString(dr["Source"]);
                    result.LoadAmount = dr.IsNull("LOADAMOUNT") ? 0 : Convert.ToDecimal(dr["LOADAMOUNT"]);
                    result.Code = dr.IsNull("CODE") ? string.Empty : Convert.ToString(dr["CODE"]);
                    result.LoadAmount = dr.IsNull("DISCOUNTAMOUNT") ? 0 : Convert.ToDecimal(dr["DISCOUNTAMOUNT"]);
                    result.Code = dr.IsNull("REMARK") ? string.Empty : Convert.ToString(dr["REMARK"]);
                    result.MotorInsurancePolicy.MobileNumber = dr.IsNull("MOBILENUMBER") ? string.Empty : Convert.ToString(dr["MOBILENUMBER"]);
                    result.MotorInsurancePolicy.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);
                    result.MotorInsurancePolicy.IsHIR = dr.IsNull("IsHIR") ? false : (Convert.ToBoolean(dr["IsHIR"]));
                }
                result.IsTransactionDone = true;
                return result;
            }
            catch (Exception ex)
            {
                return new MotorDetailsPortalResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Get dashboard deatils for the user.
        /// </summary>
        /// <param name="fromDate">Date from.</param>
        /// <param name="todate">Date to.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="agency">Agency.</param>
        /// <param name="branchCode">Branch code.</param>
        /// <returns>Dashboard response.</returns>
        public DashboardResponse GetPortalDashboard(DateTime fromDate, DateTime todate, string agentCode, string agency, string branchCode)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@FromDate",fromDate),
                    new  SqlParameter("@ToDate" ,todate),
                    new  SqlParameter("@AgentCode" ,string.IsNullOrEmpty(agentCode)?"":agentCode),
                    new  SqlParameter("@Agency" , string.IsNullOrEmpty(agency)?"":agency),
                    new  SqlParameter("@BranchCode" , string.IsNullOrEmpty(branchCode)?"":branchCode)
                };
                DataSet dt = BKICSQL.eds(PortalSP.Fetchdashboard, paras);
                List<Active> activelist = new List<Active>();
                List<HIR> hirlist = new List<HIR>();
                List<Active> NewPolicy = new List<Active>();
                List<Renew> RenewPolicy = new List<Renew>();
                if (dt != null && dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Tables[0].Rows)
                    {
                        HIR hir = new HIR();
                        Active active = new Active();
                        hir.InsuranceType = Convert.ToString(row["InsuranceType"]);
                        if (hir.InsuranceType.Contains("HIR"))
                        {
                            hir.InsuranceType = hir.InsuranceType;
                            hir.HIRCount = Convert.ToInt32(row["PolicyCount"]);
                            hirlist.Add(hir);
                        }
                        else
                        {
                            active.InsuranceType = hir.InsuranceType;
                            active.ActiveCount = Convert.ToInt32(row["PolicyCount"]);
                            activelist.Add(active);
                        }
                    }
                }
                return new DashboardResponse
                {
                    ActiveList = activelist,
                    HIRList = hirlist,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new DashboardResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get commission for the specified insurance type.
        /// </summary>
        /// <param name="request">Commission request</param>
        /// <returns>Commission amount.</returns>
        public CommissionResponse GetCommission(CommissionRequest request)
        {
            try
            {
                decimal commissionAmount = decimal.Zero;

                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@AgentCode",request.AgentCode ?? string.Empty),
                    new SqlParameter("@Agency",request.Agency ?? string.Empty),
                    new SqlParameter("@Premium",request.PremiumAmount),
                    new SqlParameter("@SubClass",request.SubClass ?? string.Empty),
                    new SqlParameter("@IsDeductable", request.IsDeductable),
                    new SqlParameter("@InsuranceType", request.InsuranceType ?? string.Empty),
                    new SqlParameter("@CommissionCode", request.CommissionCode ?? string.Empty)
                };

                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@Commision", Precision = 30, Scale = 3 }
                };
                object[] dataSet = BKICSQL.GetValues(StoredProcedures.AdminSP.CalculateCommision, para, outParams);
                if (dataSet != null)
                {
                    commissionAmount = Convert.ToDecimal(dataSet[0].ToString());
                }
                return new CommissionResponse
                {
                    CommissionAmount = commissionAmount,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new CommissionResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get commission for the home policies based on BASIC and SRCC commission rate.
        /// </summary>
        /// <param name="request">Home commission request.</param>
        /// <returns>Basic commission and SRCC commission.</returns>
        public HomeCommissionResponse GetHomeEndorsementCommission(HomeCommissionRequest request)
        {
            try
            {
                decimal basicCommission = decimal.Zero;
                decimal srccCommission = decimal.Zero;

                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@AgentCode",request.AgentCode ?? string.Empty),
                    new SqlParameter("@Agency",request.Agency ?? string.Empty),
                    new SqlParameter("@Premium",request.PremiumAmount),
                    new SqlParameter("@NewSumInsured", request.NewSumInsured),
                    new SqlParameter("@IsRiotAdded", request.IsRoitAdded),
                    new SqlParameter("@SubClass",request.SubClass ?? string.Empty),
                    new SqlParameter("@EndorsementType",request.EndorsementType ?? string.Empty),
                };
                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@BasicCommission", Precision = 30, Scale = 3 },
                     new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@SRCCCommission", Precision = 30, Scale = 3 }
                };
                object[] dataSet = BKICSQL.GetValues(StoredProcedures.HomeInsuranceSP.GetHomeEndorsementCommission, para, outParams);
                if (dataSet != null)
                {
                    basicCommission = Convert.ToDecimal(dataSet[0].ToString());
                    srccCommission = Convert.ToDecimal(dataSet[1].ToString());
                }
                return new HomeCommissionResponse
                {
                    IsTransactionDone = true,
                    BasicCommission = basicCommission,
                    SRCCCommission = srccCommission
                };
            }
            catch (Exception ex)
            {
                return new HomeCommissionResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }


        /// <summary>
        /// Get commission for the home policies based on BASIC and SRCC commission rate.
        /// </summary>
        /// <param name="request">Home commission request.</param>
        /// <returns>Total commission.</returns>
        public HomeCommissionResponse GetHomePolicyCommission(HomeCommissionRequest request)
        {
            try
            {
                decimal Commission = decimal.Zero;

                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Agency",request.Agency ?? string.Empty),
                    new SqlParameter("@AgentCode",request.AgentCode ?? string.Empty),
                    new SqlParameter("@SubClass", request.SubClass ?? string.Empty),
                    new SqlParameter("@TotalBasicPremium",request.TotalBasicPremium),
                    new SqlParameter("@TotalSRCCPremium", request.TotalSRCCPremium)
                };
                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@Commision", Precision = 30, Scale = 3 }

                };
                object[] dataSet = BKICSQL.GetValues(StoredProcedures.HomeInsuranceSP.GetHomeCommission, para, outParams);
                if (dataSet != null)
                {
                    Commission = Convert.ToDecimal(dataSet[0].ToString());
                }
                return new HomeCommissionResponse
                {
                    IsTransactionDone = true,
                    BasicCommission = Commission
                };
            }
            catch (Exception ex)
            {
                return new HomeCommissionResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get vat amount for the premium.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Vat amount for policy premium and vat amount for commission.</returns>
        public VatResponse GetVat(VatRequest request)
        {
            try
            {
                decimal Vat = 0;
                decimal VatCommission = 0;
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Premium",request.PremiumAmount),
                    new SqlParameter("@Commission", request.CommissionAmount)
                };
                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@VatAmount", Precision = 30, Scale = 3 },
                     new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@VatCommissionAmount", Precision = 30, Scale = 3 }
                };
                var resultdt = BKICSQL.GetValues(StoredProcedures.AdminSP.GETVAT, para, outParams);
                if (resultdt != null)
                {
                    if (resultdt[0] != null)
                    {
                        Vat = Convert.ToDecimal(resultdt[0].ToString());
                    }
                    if (resultdt[1] != null)
                    {
                        VatCommission = Convert.ToDecimal(resultdt[1].ToString());
                    }
                }
                return new VatResponse
                {
                    IsTransactionDone = true,
                    VatAmount = Vat,
                    VatCommissionAmount = VatCommission
                };
            }
            catch (Exception ex)
            {
                return new VatResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        #region Unused method

        public TransactionWrapper UploadHIRDocuments(DataTable uploadDocuments,
            string insuredCode, string documentNo, string linkId, string insuredType, string refID)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@Documents",uploadDocuments),
                };

                BKICSQL.enq(PortalSP.UploadInsuranceDocuments, paras);

                UpdateHIRStatusRequest hirStatus = new UpdateHIRStatusRequest();
                hirStatus.LinkId = linkId;
                hirStatus.InsuredCode = insuredCode;
                hirStatus.InsuranceType = insuredType;
                hirStatus.DocumentNo = documentNo;
                hirStatus.HIRStatusCode = 6;
                hirStatus.ID = Convert.ToInt32(refID);
                UpdateHIRStatusCode(hirStatus);

                return new TransactionWrapper() { IsTransactionDone = true };
            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }

        public ResetPasswordResult ResetPassword(string newPassword, string insuredCode)
        {
            try
            {
                ResetPasswordResult result = new ResetPasswordResult();

                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsPasswordChanged" },
                    //new SPOut() { OutPutType= SqlDbType.Bit, ParameterName = "@IsUserAccountPresent" }
                };

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@InsuredCode", insuredCode),
                    new SqlParameter("@NewPassword", newPassword)
                };

                object[] dataSet = BKICSQL.GetValues(PortalSP.UpdatePassword, para, outParams);
                result.IsPasswordChanged = dataSet[0].ToString() == "True" ? true : false;
                result.IsTransactionDone = true;

                return result;
            }
            catch (Exception exc)
            {
                return new ResetPasswordResult()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = exc.Message,
                    IsPasswordChanged = false
                };
            }
        }

        public ChangeUserStatusResponse ChangeUserStatus(ChangeUserStatusRequest prequest)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@InsuredCode", prequest.InsuredCode),
                    new SqlParameter("@IsActive", prequest.IsActive)
                };
                BKICSQL.enq(PortalSP.UpdateUserStatus, para);
                return new ChangeUserStatusResponse { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new ChangeUserStatusResponse { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        public FetchDocumentsResponse FetchHIRDocuments(FetchDocumentsRequest req)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@InsuredCode",req.InsuredCode),
                     new  SqlParameter("@PolicyDocNo",req.DocumentNo),
                      new  SqlParameter("@LinkID",req.LinkID),
                };

                DataTable dt = BKICSQL.edt(PortalSP.FetchHIRDocuments, paras);
                return new FetchDocumentsResponse { HIRDocdt = dt, IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new FetchDocumentsResponse { TransactionErrorMessage = ex.Message };
            }
        }

        public DocumentsUploadPrecheckResponse PrecheckHIRDocumentUpload(string insuredCode, string documentNo, string linkID,
            string insuredType)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@InsuredCode",insuredCode),
                    new  SqlParameter("@PolicyDocNo",documentNo),
                    new  SqlParameter("@LinkID",linkID),
                     new  SqlParameter("@InsuranceType",insuredType),
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() {   OutPutType = SqlDbType.Bit, ParameterName= "@IsValidUser"},
                    new SPOut() {   OutPutType = SqlDbType.Bit, ParameterName= "@IsDocumentsCanUploaded" }
                };

                object[] dataSet = BKICSQL.GetValues(PortalSP.HIRUploadDocumentPreCheck, paras, outParams);
                bool IsValidUser = false;
                bool IsDocumentsCanUpload = false;
                if (dataSet != null)
                {
                    IsValidUser = dataSet[0] != null ? Convert.ToBoolean(dataSet[0]) : false;
                    IsDocumentsCanUpload = dataSet[1] != null ? Convert.ToBoolean(dataSet[1]) : false;
                }
                return new DocumentsUploadPrecheckResponse { IsDocumentsCanUpload = IsDocumentsCanUpload, IsValidUser = IsValidUser, IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new DocumentsUploadPrecheckResponse { TransactionErrorMessage = ex.Message, IsTransactionDone = false };
            }
        }

        public EmailMessageAuditResult GetEmailMessageForRecord(string insuredCode, string policyNo, string linkId)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@InsuredCode",insuredCode),
                    new  SqlParameter("@PolicyNo",policyNo),
                    new  SqlParameter("@LinkId",linkId)
                };

                DataTable dt = BKICSQL.edt(PortalSP.GetEmailMessageForRecord, paras);

                if (dt != null && dt.Rows.Count > 0)
                {
                    List<EmailMessageAudit> messageAudit = new List<EmailMessageAudit>();

                    foreach (DataRow row in dt.Rows)
                    {
                        messageAudit.Add(new EmailMessageAudit()
                        {
                            //  Message = Convert.ToString(row["Message"]).Replace("api/cms", Utility.frontWebUI + "api/cms"),
                            MessageKey = row.IsNull("MessageKey") ? "" : row["MessageKey"].ToString(),
                            InsuredCode = row.IsNull("InsuredCode") ? "" : row["InsuredCode"].ToString(),
                            PolicyNo = row.IsNull("PolicyNo") ? "" : row["PolicyNo"].ToString(),
                            LinkNo = row.IsNull("LinkId") ? "" : row["LinkId"].ToString(),
                            InsuranceType = row.IsNull("InsuredType") ? "" : row["InsuredType"].ToString(),
                            CreatedDate = row.IsNull("CreatedDate") ? (DateTime?)null : Convert.ToDateTime(row["CreatedDate"]),
                            Subject = row.IsNull("Subject") ? "" : row["Subject"].ToString(),
                            TrackId = row.IsNull("TrackId") ? "" : row["TrackId"].ToString()
                        });
                    }

                    return new EmailMessageAuditResult() { EmailMessage = messageAudit, IsTransactionDone = true };
                }
                else
                {
                    return new EmailMessageAuditResult() { IsTransactionDone = true };
                }
            }
            catch (Exception exc)
            {
                return new EmailMessageAuditResult() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }

        /// <summary>
        /// Get user information.
        /// </summary>
        /// <param name="request">User fetch request.</param>
        /// <returns></returns>
        public FetchUserDetailsResponse FetchUserDetails(FetchUserDetailsRequest request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@Search",string.IsNullOrEmpty(request.Search)?"":request.Search),
                    new  SqlParameter("@Filtertype",string.IsNullOrEmpty(request.FilterType)?"":request.FilterType )
                };

                DataTable dt = BKICSQL.edt(PortalSP.FetchUserDetails, paras);
                List<PortalUserDetails> details = new List<PortalUserDetails>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        PortalUserDetails info = new PortalUserDetails();
                        info.ID = dr.IsNull("INSUREDID") ? 0 : Convert.ToInt32(dr["INSUREDID"]);
                        info.InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]);
                        info.InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]);
                        info.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);
                        info.EmailAddress = dr.IsNull("EMAIL") ? string.Empty : Convert.ToString(dr["EMAIL"]);
                        info.Mobile = dr.IsNull("TELEPHONEMOBILE") ? string.Empty : Convert.ToString(dr["TELEPHONEMOBILE"]);
                        info.UserAccountExist = string.IsNullOrEmpty(Convert.ToString(dr["UserName"])) ? "No" : "Yes";
                        details.Add(info);
                    }
                }
                return new FetchUserDetailsResponse
                {
                    IsTransactionDone = true,
                    UserDetails = details
                };
            }
            catch (Exception ex)
            {
                return new FetchUserDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Update the HIR Remarks.
        /// </summary>
        /// <param name="request">HIR Remarks.</param>
        /// <returns></returns>
        public UpdateHIRRemarksResponse UpdateHIRRemarks(UpdateHIRRemarksRequest request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@DocumentNo",string.IsNullOrEmpty(request.DocumentNo)? "" : request.DocumentNo),
                    new  SqlParameter("@Remarks",string.IsNullOrEmpty(request.Remarks)? "" : request.Remarks),
                    new  SqlParameter("@InsuranceType",string.IsNullOrEmpty(request.InsuranceType)? "" : request.InsuranceType),
                    new SqlParameter("@RenewalCount", request.RenewalCount)
                };
                DataTable dt = BKICSQL.edt(PortalSP.UpdateHIRRemarks, paras);
                if (dt != null && dt.Rows.Count > 0)
                {

                }
                return new UpdateHIRRemarksResponse
                {
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new UpdateHIRRemarksResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
        #endregion Unused method

    }
}