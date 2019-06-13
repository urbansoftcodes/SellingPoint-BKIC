using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading.Tasks;
using OracleDBIntegration = SellingPoint.OracleDBIntegration;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    public class TravelEndorsement : ITravelEndorsement
    {
        public readonly OracleDBIntegration.Implementation.TravelEndorsement _oracleTravelEndorsement;
        public readonly IMail _mail;

        public TravelEndorsement()
        {
            _oracleTravelEndorsement = new OracleDBIntegration.Implementation.TravelEndorsement();
            _mail = new Mail();
        }


        /// <summary>
        /// Travel endorsement operation - authorize or delete.
        /// </summary>
        /// <param name="request">Endorsement operation request.</param>
        /// <returns></returns>
        public TravelEndorsementOperationResponse EndorsementOperation(TravelEndorsementOperation request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                     new  SqlParameter("@Type", request.Type),
                     new  SqlParameter("@TravelEndorsementID", request.TravelEndorsementID),
                     new  SqlParameter("@TravelID", request.TravelID),
                     new  SqlParameter("@Agency", request.Agency),
                     new  SqlParameter("@AgentCode", request.AgentCode),
                     new  SqlParameter("@UpdatedBy", request.UpdatedBy),

                };

                DataTable dt = BKICSQL.edt(TravelEndorsementSP.TravelEndorsementOperation, paras);
                if (request.Type == Constants.EndorsementOpeationType.Authorize)
                {
                    
                    try
                    {
                        new Task(() =>
                        {
                            OracleDBIntegration.DBObjects.TransactionWrapper oracleResult
                               = _oracleTravelEndorsement.MoveIntegrationToOracle(request.TravelEndorsementID);
                        }).Start();
                    }
                    catch (AggregateException ex)
                    {
                        foreach (Exception inner in ex.InnerExceptions)
                        {
                            _mail.SendMailLogError(ex.Message, "", "Travelendorsement", "", true);
                        }
                    }                  

                }
                return new TravelEndorsementOperationResponse
                {
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new TravelEndorsementOperationResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Check the if the policy already have saved endorsement,if it is there don't allow to pass the new endorsement.
        /// </summary>
        /// <param name="request">Endorsement precheck request.</param>
        /// <returns>Returns there an endorsemnt with saved staus or not.</returns>
        public TravelEndorsementPreCheckResponse EndorsementPrecheck(TravelEndorsementPreCheckRequest request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                     new  SqlParameter("@docNo", request.DocNo),
                     new SqlParameter ("@type", Constants.Insurance.Travel)
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsAlreadyHaveEndorsement" , Precision = 38, Scale =3},
                };
                var alreadyHave = false;
                object[] dataSet = BKICSQL.GetValues(AdminSP.EndorsementPreCheck, paras, outParams);
                if (dataSet != null && dataSet[0] != null)
                {
                    alreadyHave = string.IsNullOrEmpty(dataSet[0].ToString()) ? false : Convert.ToBoolean(dataSet[0].ToString());
                }
                return new TravelEndorsementPreCheckResponse()
                {
                    IsTransactionDone = true,
                    IsAlreadyHave = alreadyHave,
                    EndorsementNo = ""
                };
            }
            catch (Exception ex)
            {
                return new TravelEndorsementPreCheckResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all the travel endorsement details for the specific policy.
        /// To show the top of the page on any travel endorsement page.
        /// </summary>
        /// <param name="request">Endorsement request.</param>
        /// <returns>list of endorsemnt details.</returns>
        public TravelEndoResponse GetAllEndorsements(TravelEndoRequest request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
               {
                     new  SqlParameter("@Insurancetype", request.InsuranceType),
                     new  SqlParameter("@Agency", request.Agency),
                     new  SqlParameter("@AgentCode", request.AgentCode),
                     new  SqlParameter("@DocumentNo", request.DocumentNo),
               };
                DataSet travelEndo = BKICSQL.eds(StoredProcedures.PortalSP.GetEndorsementByDocNo, paras);
                List<BO.TravelEndorsement> listEndo = new List<BO.TravelEndorsement>();
                if (travelEndo != null && travelEndo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in travelEndo.Tables[0].Rows)
                    {
                        BO.TravelEndorsement result = new BO.TravelEndorsement();

                        result.TravelEndorsementID = dr.IsNull("TravelEndorsementID") ? 0 : Convert.ToInt64(dr["TravelEndorsementID"]);
                        result.TravelID = dr.IsNull("TravelID") ? 0 : Convert.ToInt64(dr["TravelID"]);
                        result.DocumentNo = dr.IsNull("DocumentNo") ? string.Empty : Convert.ToString(dr["DocumentNo"]);
                        result.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                        result.EndorsementType = dr.IsNull("EndorsementType") ? string.Empty : Convert.ToString(dr["EndorsementType"]);
                        result.PremiumBeforeDiscount = dr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(dr["PremiumBeforeDiscount"]);
                        result.PremiumAfterDiscount = dr.IsNull("PremiumAfterDiscount") ? 0 : Convert.ToDecimal(dr["PremiumAfterDiscount"]);
                        result.CommisionBeforeDiscount = dr.IsNull("CommissionBeforeDiscount") ? 0 : Convert.ToDecimal(dr["CommissionBeforeDiscount"]);
                        result.CommissionAfterDiscount = dr.IsNull("CommissionAfterDiscount") ? 0 : Convert.ToDecimal(dr["CommissionAfterDiscount"]);
                        result.RefundAmount = dr.IsNull("RefundAmount") ? 0 : Convert.ToDecimal(dr["RefundAmount"]);
                        result.RefundAfterDiscount = dr.IsNull("RefundAfterDiscount") ? 0 : Convert.ToDecimal(dr["RefundAfterDiscount"]);
                        result.PolicyCommencementDate = dr.IsNull("COMMENCEDATE") ? DateTime.Now : Convert.ToDateTime(dr["COMMENCEDATE"]);
                        result.ExpiryDate = dr.IsNull("EXPIRYDATE") ? DateTime.Now : Convert.ToDateTime(dr["EXPIRYDATE"]);
                        result.IsSaved = dr.IsNull("IsSaved") ? false : Convert.ToBoolean(dr["IsSaved"]);
                        result.IsActivePolicy = dr.IsNull("IsActive") ? false : Convert.ToBoolean(dr["IsActive"]);

                        listEndo.Add(result);
                    }
                }
                return new TravelEndoResponse
                {
                    TravelEndorsements = listEndo,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new TravelEndoResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Calculate endorsement premium by endorsement type.
        /// </summary>
        /// <param name="travelEndorsement">Endorement quote request.</param>
        /// <returns>Returns endorsement premium and commision.</returns>
        public TravelEndorsementQuoteResponse GetTravelEndorsementQuote(TravelEndorsementQuote travelEndorsement)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@Agency",travelEndorsement.Agency),
                    new  SqlParameter("@AgentCode",travelEndorsement.AgentCode),
                    new SqlParameter("@DocumentNo", travelEndorsement.DocumentNo ?? ""),
                    new  SqlParameter("@MainClass",travelEndorsement.MainClass),
                    new  SqlParameter("@SubClass",travelEndorsement.SubClass),
                    new  SqlParameter("@EffectiveFromDate",travelEndorsement.EffectiveFromDate),
                    new  SqlParameter("@EffectiveToDate",travelEndorsement.EffectiveToDate),
                    new  SqlParameter("@CancelationDate",travelEndorsement.CancelationDate),
                    new  SqlParameter("@PaidPremium",travelEndorsement.PaidPremium),
                    new SqlParameter("@NewSumInsured", travelEndorsement.NewSumInsured),
                    new SqlParameter("@RefundType", travelEndorsement.RefundType),
                    new SqlParameter("@PolicyPeriodName", travelEndorsement.PolicyPeriodName),
                    new  SqlParameter("@EndorsementType",travelEndorsement.EndorsementType),
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@EndorsementPremium" , Precision = 38, Scale =3},
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@Commission" , Precision = 38, Scale =3}
                };

                object[] dataSet = BKICSQL.GetValues(TravelEndorsementSP.GetQuote, paras, outParams);
                var endorsementPremium = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : decimal.Parse(dataSet[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                var commission = string.IsNullOrEmpty(dataSet[1].ToString()) ? 0 : decimal.Parse(dataSet[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                return new TravelEndorsementQuoteResponse()
                {
                    IsTransactionDone = true,
                    EndorsementPremium = endorsementPremium,
                    Commision = commission
                };
            }
            catch (Exception ex)
            {
                return new TravelEndorsementQuoteResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Insert the travel endorsement.
        /// </summary>
        /// <param name="travelEndorsement">Travel endorsement details.</param>
        /// <returns>Travelendorsementid, Travelendorementnumber.</returns>
        public TravelEndorsementResponse PostTravelEndorsement(BO.TravelEndorsement travelEndorsement)
        {
            try
            {
                DataTable member = new DataTable();
                member.Columns.Add("TRAVELID", typeof(Int64));
                member.Columns.Add("DOCUMENTNO", typeof(string));
                member.Columns.Add("ITEMSERIALNO", typeof(Int32));
                member.Columns.Add("ITEMNAME", typeof(string));
                member.Columns.Add("SUMINSURED", typeof(decimal));
                member.Columns.Add("FOREIGNSUMINSURED", typeof(decimal));
                member.Columns.Add("CATEGORY", typeof(string));
                member.Columns.Add("TITLE", typeof(string));
                member.Columns.Add("SEX", typeof(string));
                member.Columns.Add("DATEOFBIRTH", typeof(DateTime));
                member.Columns.Add("AGE", typeof(string));
                member.Columns.Add("PREMIUMAMOUNT", typeof(decimal));
                member.Columns.Add("MAKE", typeof(string));
                member.Columns.Add("OCCUPATIONCODE", typeof(string));
                member.Columns.Add("CPR", typeof(string));
                member.Columns.Add("PASSPORT", typeof(string));
                member.Columns.Add("FIRSTNAME", typeof(string));
                member.Columns.Add("MIDDLENAME", typeof(string));
                member.Columns.Add("LASTNAME", typeof(string));
                member.Columns.Add("CREATEDBY", typeof(int));
                member.Columns.Add("CREATEDDATE", typeof(DateTime));
                member.Columns.Add("UPDATEDBY", typeof(int));
                member.Columns.Add("UPDATEDDATE", typeof(DateTime));
                member.Columns.Add("LINKID", typeof(string));
                if (travelEndorsement.TravelMembers.Count > 0)
                {

                    foreach (var members in travelEndorsement.TravelMembers)
                    {
                        members.UpdatedDate = DateTime.Now;
                        //members.CreatedDate = DateTime.Now;
                        //members.DateOfBirth = DateTime.Now;

                        member.Rows.Add(members.TravelID, members.DocumentNo, members.ItemSerialNo, members.ItemName, members.SumInsured,
                            members.ForeignSumInsured, members.Category, members.Title, members.Sex, members.DateOfBirth, members.Age,
                            members.PremiumAmount, members.Make, members.OccupationCode, members.CPR, members.Passport, members.FirstName,
                            members.MiddleName, members.LastName, members.CreatedBy, members.CreatedDate, members.UpdatedBy, members.UpdatedDate, "");
                    }
                }

                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@TravelID", travelEndorsement.TravelID),
                    new  SqlParameter("@TravelendorsementID",travelEndorsement.TravelEndorsementID),
                    new  SqlParameter("@EndorsementType",travelEndorsement.EndorsementType ?? ""),
                    new  SqlParameter("@Agency",travelEndorsement.Agency),
                    new  SqlParameter("@AgentCode",travelEndorsement.AgencyCode),
                    new  SqlParameter("@BranchCode",travelEndorsement.AgentBranch ?? ""),
                    new SqlParameter("@CreatedBy" , travelEndorsement.CreatedBy),
                    new  SqlParameter("@DocumentNo",travelEndorsement.DocumentNo ?? ""),
                    new  SqlParameter("@InsuredCode",travelEndorsement.InsuredCode ?? ""),
                    new  SqlParameter("@InsuredName",travelEndorsement.InsuredName ?? ""),
                    new  SqlParameter("@Premium",travelEndorsement.PremiumAmount),
                    new  SqlParameter("@FinanceCompany",travelEndorsement.FinancierCompanyCode ?? ""),
                    new  SqlParameter("@MainClass",travelEndorsement.Mainclass ?? ""),
                    new  SqlParameter("@SubClass",travelEndorsement.Subclass ?? ""),
                    new  SqlParameter("@CommencementDate",travelEndorsement.PolicyCommencementDate),
                    new  SqlParameter("@ExpireDate",travelEndorsement.ExpiryDate),
                    new  SqlParameter("@ExtendedExpireDate",travelEndorsement.ExtendedExpireDate.HasValue ? travelEndorsement.ExtendedExpireDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@CancelDate",travelEndorsement.CancelDate.HasValue ? travelEndorsement.CancelDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@PaymentDate",travelEndorsement.PaymentDate.HasValue ? travelEndorsement.PaymentDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@PaymentType",travelEndorsement.PaymentType ?? ""),
                    new  SqlParameter("@AccountNumber",travelEndorsement.AccountNumber ?? ""),
                    new  SqlParameter("@Remarks",travelEndorsement.Remarks ?? ""),
                    new  SqlParameter("@Source",travelEndorsement.Source ?? ""),
                    new  SqlParameter("@IsSaved",travelEndorsement.IsSaved),
                    new  SqlParameter("@IsActive",travelEndorsement.IsActivePolicy),
                    new SqlParameter("@dt", member),
                    new SqlParameter("@RefundType", travelEndorsement.RefundType ?? ""),
                    new SqlParameter("@PolicyPeriodName", travelEndorsement.PolicyPeriodName ?? ""),
                    new SqlParameter("@NewPremium", travelEndorsement.NewPremium),
                    new  SqlParameter("@RefoundAmount",travelEndorsement.RefundAmount),
                    new  SqlParameter("@RefoundAfterDiscount",travelEndorsement.RefundAfterDiscount),
                    new SqlParameter("@PremiumBeforeDiscount",travelEndorsement.PremiumBeforeDiscount),
                    new SqlParameter("@PremiumAfterDiscount",travelEndorsement.PremiumAfterDiscount),
                    new SqlParameter("@CommissionBeforeDiscount",travelEndorsement.CommisionBeforeDiscount),
                    new SqlParameter("@CommissionAfterDiscount",travelEndorsement.CommissionAfterDiscount),
                    new SqlParameter("@UserChangedPremium",travelEndorsement.UserChangedPremium),
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                new SPOut() {
                                   OutPutType = SqlDbType.BigInt, ParameterName= "@NewTravelEndorsementID"},
                                   new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsHIR"},
                                   new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@EndorsementNumber", Size=50},
                     };

                object[] dataSet = BKICSQL.GetValues(TravelEndorsementSP.PostTravelEndorsement, paras, outParams);

                var endorsementID = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : Convert.ToInt64(dataSet[0].ToString());
                var isHIR = string.IsNullOrEmpty(dataSet[1].ToString()) ? false : Convert.ToBoolean(dataSet[1].ToString());
                var EndorsementNumber = string.IsNullOrEmpty(dataSet[2].ToString()) ? string.Empty : Convert.ToString(dataSet[2].ToString());
                return new TravelEndorsementResponse()
                {
                    IsTransactionDone = true,
                    EndorsementNo = EndorsementNumber,
                    TravelEndorsementID = endorsementID,
                    IsHIR = isHIR
                };
            }
            catch (Exception ex)
            {
                _mail.SendMailLogError(ex.Message, travelEndorsement.InsuredCode, "MotorEndorsement", travelEndorsement.Agency, false);
                return new TravelEndorsementResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
    }
}