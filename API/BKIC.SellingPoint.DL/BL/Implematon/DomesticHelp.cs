using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OracleDBIntegration = SellingPoint.OracleDBIntegration;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    /// <summary>
    /// Domestic policy funcationalities.
    /// </summary>
    public class DomesticHelp : IDomesticHelp
    {
        public readonly OracleDBIntegration.Implementation.DomesticInsurance _oracleDomesticInsurance;
        public readonly IMail _mail;

        public DomesticHelp()
        {
            _oracleDomesticInsurance = new OracleDBIntegration.Implementation.DomesticInsurance();
            _mail = new Mail();
        }

        /// <summary>
        /// Get quote for the domestic help policy.
        /// </summary>
        /// <param name="domesticQuoteRequest">domestic quote request.</param>
        /// <returns></returns>
        public DomesticHelpQuoteResponse GetDomesticHelpQuote(DomesticHelpQuote domesticQuoteRequest)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@InsurancePeroid", domesticQuoteRequest.InsurancePeroid),
                    new SqlParameter("@NumberOfDomesticWorkers",domesticQuoteRequest.NumberOfDomesticWorkers)
                    //In future it may be added.
                   // new SqlParameter("@Agency",pQuoteInputs.Agency)
                   // new SqlParameter("@AgentCode",pQuoteInputs.AgentCode)
                   // new SqlParameter("@MainClass",pQuoteInputs.MainClass)
                   // new SqlParameter("@SubClass",pQuoteInputs.SubClass)
                };

                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() {   OutPutType = SqlDbType.Decimal, ParameterName= "@PremiumBeforeDiscount", Precision= 38, Scale =3 },
                    new SPOut() {   OutPutType = SqlDbType.Decimal, ParameterName= "@PremiumAfterDiscount", Precision= 38 }
                };

                object[] dataSet = BKICSQL.GetValues(StoredProcedures.DomesticHelpInsuranceSP.GetQuote, para, outParams);
                var premiumBeforeDiscount = decimal.Parse(Convert.ToString(dataSet[0]));
                return new DomesticHelpQuoteResponse()
                {
                    IsTransactionDone = true,
                    PremiumAfterDiscount = 0,
                    PremiumBeforeDiscount = premiumBeforeDiscount
                };
            }
            catch (Exception ex)
            {
                return new DomesticHelpQuoteResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Post the domestic policy.
        /// </summary>
        /// <param name="details">Domestic policy details.</param>
        /// <returns>Posted domestic id, document number and hir status.</returns>
        public DomesticHelpPolicyResponse PostDomesticPolicy(DomesticPolicyDetails details)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@DomesticID", details.DomesticHelp.DomesticID),
                    new SqlParameter("@Agency", details.DomesticHelp.Agency??string.Empty),
                    new SqlParameter("@AgentCode", details.DomesticHelp.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch", details.DomesticHelp.AgentBranch??string.Empty),
                    new SqlParameter("@InsuredCode", details.DomesticHelp.InsuredCode??string.Empty),
                    new SqlParameter("@InsuredName", details.DomesticHelp.InsuredName??string.Empty),
                    new SqlParameter("@CPR",details.DomesticHelp.CPR??string.Empty),
                    new SqlParameter("@InsurancePeroid", details.DomesticHelp.InsurancePeroid),
                    new SqlParameter("@NumberOfDomesticWorkers", details.DomesticHelp.NoOfDomesticWorkers),
                    new SqlParameter("@PolicyStartDate",details.DomesticHelp.PolicyStartDate),
                    new SqlParameter("@DomesticworkerType",details.DomesticHelp.DomesticWorkType ?? string.Empty),
                    new SqlParameter("@IsPhysicalDefect",details.DomesticHelp.IsPhysicalDefect == "Yes"? true : false),
                    new SqlParameter("@PhysicalDescription",string.IsNullOrEmpty(details.DomesticHelp.PhysicalDefectDescription)?"":details.DomesticHelp.PhysicalDefectDescription),
                    new SqlParameter("@MobileNumber", details.DomesticHelp.Mobile??string.Empty),
                    new SqlParameter("@Createdby",details.DomesticHelp.CreatedBy),
                    new SqlParameter("@AuthorizedBy", details.DomesticHelp.AuthorizedBy),
                    new SqlParameter("@dt",details.DomesticHelpMemberdt),
                    new SqlParameter("@IsSaved",details.DomesticHelp.IsSaved),
                    new SqlParameter("@IsActive", details.DomesticHelp.IsActivePolicy),
                    new SqlParameter("@PaymentType", details.DomesticHelp.PaymentType),
                    new SqlParameter("@Remarks", details.DomesticHelp.Remarks),
                    new SqlParameter("@AccountNumber", details.DomesticHelp.AccountNumber),
                    new SqlParameter("@PremiumAfterDiscountAmount", details.DomesticHelp.PremiumAfterDiscount),
                    new SqlParameter("@CommisionAfterDiscountAmount", details.DomesticHelp.CommissionAfterDiscount),
                    new SqlParameter("@UserChangedPremium", details.DomesticHelp.UserChangedPremium),
                    new SqlParameter("@MainClass",details.DomesticHelp.MainClass),
                    new SqlParameter("@SubClass", details.DomesticHelp.SubClass),
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() {   OutPutType = SqlDbType.Int, ParameterName= "@NewDomesticID"},
                    new SPOut() {   OutPutType = SqlDbType.Bit, ParameterName= "@IsHIR" },
                    new SPOut() {   OutPutType = SqlDbType.NVarChar, ParameterName= "@DocumentNumber", Size=50 },
                };
                object[] dataSet = BKICSQL.GetValues(StoredProcedures.DomesticHelpInsuranceSP.PostDomesticPolicy, para, outParams);
                var domesticID = Convert.ToInt64(dataSet[0]);
                var IsHIR = Convert.ToBoolean(dataSet[1]);
                var documentNo = Convert.ToString(dataSet[2]);

                if (!IsHIR && details.DomesticHelp.IsActivePolicy)
                {                   
                    try
                    {
                        new Task(() =>
                        {
                            OracleDBIntegration.DBObjects.TransactionWrapper oracleResult =
                            _oracleDomesticInsurance.IntegrateDomesticToOracle((int)domesticID);
                        }).Start();
                    }                    
                    catch (AggregateException ex)
                    {
                        foreach (Exception inner in ex.InnerExceptions)
                        {
                            _mail.SendMailLogError(inner.Message, details.DomesticHelp.InsuredCode,
                                "DomesticHelp", details.DomesticHelp.Agency, true);
                        }
                    }
                }
                return new DomesticHelpPolicyResponse()
                {
                    IsTransactionDone = true,
                    DomesticID = domesticID,
                    IsHIR = IsHIR,
                    DocumentNo = documentNo
                };
            }
            catch (Exception ex)
            {
                _mail.SendMailLogError(ex.Message, details.DomesticHelp.InsuredCode,
                    "DomesticHelp", details.DomesticHelp.Agency, false);
                return new DomesticHelpPolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get the domestic policy details by domestic id.
        /// </summary>
        /// <param name="domesticID">doemstic id</param>
        /// <param name="insuredCode">insured code</param>
        /// <returns></returns>
        public DomesticHelpSavedQuotationResponse GetSavedDomesticHelp(int domesticID, string insuredCode)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@DomesticID", domesticID),
                    new SqlParameter("@InsuredCode",insuredCode)
                };

                DataSet domesticds = BKICSQL.eds(StoredProcedures.DomesticHelpInsuranceSP.SavedQuotation, para);
                DomesticHelpPolicy domestic = new DomesticHelpPolicy();
                if (domesticds != null && domesticds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = domesticds.Tables[0].Rows[0];
                    domestic.DomesticID = domesticID;
                    domestic.InsurancePeroid = Convert.ToInt32(dr["INSURANCEPERIOD"]);
                    domestic.PolicyStartDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                    domestic.PolicyExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                    domestic.CPR = Convert.ToString(dr["CPR"]);
                    domestic.FullName = Convert.ToString(dr["INSUREDNAME"]);
                    domestic.PremiumAfterDiscount = Convert.ToDecimal(dr["PREMIUMAMOUNT"]);
                    domestic.PremiumBeforeDiscount = Convert.ToDecimal(dr["ORIGINALPREMIUMAMOUNT"]);
                    domestic.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                    domestic.SumInsured = Convert.ToDecimal(dr["SUMINSURED"]);
                    domestic.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                    domestic.DomesticWorkType = dr["DomesticWorkerType"].ToString() == null ? "" : dr["DomesticWorkerType"].ToString();
                    domestic.IsPhysicalDefect = dr["PhysicalDefect"].ToString() == null ? "" : dr["PhysicalDefect"].ToString();
                    domestic.PhysicalDefectDescription = dr["PhysicalDesc"].ToString() == null ? "" : dr["PhysicalDesc"].ToString();
                    domestic.InsuredCode = insuredCode;
                    domestic.IsHIR = Convert.ToBoolean(dr["IsHIR"]);
                }

                List<DomesticHelpMember> membersList = new List<DomesticHelpMember>();
                if (domesticds != null && domesticds.Tables[1].Rows.Count > 0)
                {
                    domestic.NoOfDomesticWorkers = domesticds.Tables[1].Rows.Count;
                    foreach (DataRow dr in domesticds.Tables[1].Rows)
                    {
                        DomesticHelpMember members = new DomesticHelpMember();
                        members.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        members.SumInsured = Convert.ToDecimal(dr["SUMINSURED"]);
                        members.PremiumAmount = Convert.ToDecimal(dr["PREMIUMAMOUNT"]);
                        members.OtherOccupation = Convert.ToString(dr["OCCUPATIONOTHER"]);
                        // members.DateOfSubmission = Convert.ToDateTime(dr["DATEOFSUBMISSION"]);
                        members.CommencementDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                        members.ExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                        members.Name = Convert.ToString(dr["INSUREDNAME"]);
                        members.Sex = Convert.ToChar(dr["SEX"]);
                        members.DOB = Convert.ToDateTime(dr["DOB"]);
                        members.Nationality = Convert.ToString(dr["NATIONALITY"]);
                        members.CPRNumber = Convert.ToString(dr["IDENTITYNO"]);
                        members.Occupation = Convert.ToString(dr["OCCUPATION"]);
                        members.ItemserialNo = Convert.ToInt32(dr["ITEMSERIALNO"]);
                        members.AddressType = Convert.ToString(dr["ADDRESS1"]);
                        members.Passport = Convert.ToString(dr["Passport"]);
                        membersList.Add(members);
                    }
                }
                return new DomesticHelpSavedQuotationResponse
                {
                    IsTransactionDone = true,
                    DomesticHelp = domestic,
                    DomesticHelpMemberList = membersList
                };
            }
            catch (Exception ex)
            {
                return new DomesticHelpSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
        /// <summary>
        /// Get domestic policy details by document number(policy number)        
        /// </summary>
        /// <param name="documentNo">document number</param>
        /// <param name="agentCode">agent code</param>
        /// <param name="isEndorsement">details fetched for endorsement page or policy page.</param>
        /// <param name="endorsementID">endoresment id</param>
        /// <returns></returns>
        public DomesticHelpSavedQuotationResponse GetSavedDomesticPolicy(string documentNo, string agentCode, bool isEndorsement = false, long endorsementID = 0)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@DocumentNo", documentNo),
                    new SqlParameter("@AgentCode", agentCode)
                };
                DataSet domesticds = BKICSQL.eds(StoredProcedures.DomesticHelpInsuranceSP.GetSavedQuotationByDocumentNo, para);
                DomesticHelpPolicy domestic = new DomesticHelpPolicy();

                if (domesticds != null && domesticds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = domesticds.Tables[0].Rows[0];
                    domestic.DomesticID = Convert.ToInt64(dr["DOMESTICID"]);
                    domestic.InsurancePeroid = Convert.ToInt32(dr["INSURANCEPERIOD"]);
                    domestic.AgentBranch = Convert.ToString(dr["BRANCHCODE"]);
                    domestic.PolicyStartDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                    domestic.PolicyExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                    domestic.PolicyIssueDate = Convert.ToDateTime(dr["DATEOFSUBMISSION"]);
                    domestic.CPR = Convert.ToString(dr["CPR"]);
                    domestic.FullName = Convert.ToString(dr["INSUREDNAME"]);
                    domestic.PremiumAfterDiscount = dr.IsNull("PREMIUMAMOUNT") ? 0 : Convert.ToDecimal(dr["PREMIUMAMOUNT"]);
                    domestic.PremiumBeforeDiscount = Convert.ToDecimal(dr["ORIGINALPREMIUMAMOUNT"]);
                    domestic.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                    domestic.SumInsured = Convert.ToDecimal(dr["SUMINSURED"]);
                    domestic.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                    domestic.DomesticWorkType = dr["DomesticWorkerType"].ToString() ?? "";
                    domestic.IsPhysicalDefect = dr["PhysicalDefect"].ToString() == null ? "" : dr["PhysicalDefect"].ToString();
                    domestic.PhysicalDefectDescription = dr["PhysicalDesc"].ToString() == null ? "" : dr["PhysicalDesc"].ToString();
                    domestic.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                    domestic.IsHIR = Convert.ToBoolean(dr["IsHIR"]);
                    domestic.Remarks = dr["Remarks"].ToString() == null ? "" : Convert.ToString(dr["Remarks"]);
                    domestic.AccountNumber = dr["AccountNumber"].ToString() == null ? "" : Convert.ToString(dr["AccountNumber"]);
                    domestic.CommisionBeforeDiscount = dr.IsNull("CommissionBeforeDiscount") ? 0 : Convert.ToDecimal(dr["CommissionBeforeDiscount"]);
                    domestic.CommissionAfterDiscount = dr.IsNull("CommissionAfterDiscount") ? 0 : Convert.ToDecimal(dr["CommissionAfterDiscount"]);
                    domestic.CommissionAmount = dr.IsNull("CommissionAmount") ? 0 : Convert.ToDecimal(dr["CommissionAmount"]);
                    domestic.IsSaved = dr.IsNull("IsSaved") ? false : Convert.ToBoolean(dr["IsSaved"]);
                    domestic.IsActivePolicy = dr.IsNull("IsActive") ? false : Convert.ToBoolean(dr["IsActive"]);
                    domestic.PolicyIssueDate = Convert.ToDateTime(dr["DATEOFSUBMISSION"]);
                    domestic.PaymentType = dr["PAYMENTTYPE"].ToString() == null ? "" : Convert.ToString(dr["PAYMENTTYPE"]);
                    domestic.HIRStatus = dr.IsNull("HIRStatus") ? 0 : Convert.ToInt32(dr["HIRStatus"]);
                    domestic.TaxOnPremium = dr.IsNull("TaxOnPremium") ? 0 : Convert.ToDecimal(dr["TaxOnPremium"]);
                    domestic.TaxOnCommission = dr.IsNull("TaxOnCommission") ? 0 : Convert.ToDecimal(dr["TaxOnCommission"]);
                }

                List<DomesticHelpMember> membersList = new List<DomesticHelpMember>();

                if (domesticds != null && domesticds.Tables[1].Rows.Count > 0)
                {
                    domestic.NoOfDomesticWorkers = domesticds.Tables[1].Rows.Count;
                    foreach (DataRow dr in domesticds.Tables[1].Rows)
                    {
                        DomesticHelpMember members = new DomesticHelpMember();
                        members.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        members.SumInsured = Convert.ToDecimal(dr["SUMINSURED"]);
                        members.PremiumAmount = Convert.ToDecimal(dr["PREMIUMAMOUNT"]);
                        members.OtherOccupation = Convert.ToString(dr["OCCUPATIONOTHER"]);
                        members.CommencementDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                        members.ExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                        members.Name = Convert.ToString(dr["INSUREDNAME"]);
                        members.Sex = Convert.ToChar(dr["SEX"]);
                        members.DOB = Convert.ToDateTime(dr["DOB"]);
                        members.Nationality = Convert.ToString(dr["NATIONALITY"]);
                        members.CPRNumber = Convert.ToString(dr["IDENTITYNO"]);
                        members.Occupation = Convert.ToString(dr["OCCUPATION"]);
                        members.ItemserialNo = Convert.ToInt32(dr["ITEMSERIALNO"]);
                        members.AddressType = Convert.ToString(dr["ADDRESS1"]);
                        members.Passport = Convert.ToString(dr["Passport"]);
                        membersList.Add(members);
                    }
                }
                return new DomesticHelpSavedQuotationResponse
                {
                    IsTransactionDone = true,
                    DomesticHelp = domestic,
                    DomesticHelpMemberList = membersList
                };
            }
            catch (Exception ex)
            {
                return new DomesticHelpSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
 
        /// <summary>
        /// Get domestic policies by agency.
        /// </summary>
        /// <param name="req">domestic policy request.</param>
        /// <returns>list of domestic policies by agency.</returns>
        public AgencyDomesticPolicyResponse GetDomesticAgencyPolicy(AgencyDomesticRequest req)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency",req.Agency??string.Empty),
                    new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch",req.AgentBranch??string.Empty),
                    new SqlParameter("@IncludeHIR", req.IncludeHIR),
                    new SqlParameter("@DocumentNo", req.DocumentNo ?? string.Empty)
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.DomesticHelpInsuranceSP.GetDomesticAgencyPolicy, para);
                List<AgencyDomesticPolicy> domesticHelpPolicies = new List<AgencyDomesticPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyDomesticPolicy();
                        res.DomesticId = Convert.ToInt32(dr["DOMESTICID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        domesticHelpPolicies.Add(res);
                    }
                }
                return new AgencyDomesticPolicyResponse
                {
                    DomesticAgencyPolicies = domesticHelpPolicies,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AgencyDomesticPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }  
    }
}