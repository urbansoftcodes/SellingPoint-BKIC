using BKIC.SellingPoint.DL.BL.Implementation;
using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.Constants;
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
    /// <summary>
    /// Motor endorsement functionalities.
    /// </summary>
    public class MotorEndorsement : IMotorEndorsement
    {
        public readonly OracleDBIntegration.Implementation.MotorEndorsement _oracleMotorEndorsement;
        public readonly IAdmin _adminRepository;
        public readonly IMail _mail;

        public MotorEndorsement()
        {
            _adminRepository = new Admin();
            _oracleMotorEndorsement = new OracleDBIntegration.Implementation.MotorEndorsement();
            _mail = new Mail();
        }

        /// <summary>
        ///Motor endorsement operation - authorize or delete.
        /// </summary>
        /// <param name="request">Endorsement operation request.</param>
        /// <returns></returns>
        public MotorEndorsementOperationResponse EndorsementOperation(MotorEndorsementOperation request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                     new  SqlParameter("@Type", request.Type),
                     new  SqlParameter("@MotorEndorsementID", request.MotorEndorsementID),
                     new  SqlParameter("@MotorID", request.MotorID),
                     new  SqlParameter("@Agency", request.Agency),
                     new  SqlParameter("@AgentCode", request.AgentCode),
                     new  SqlParameter("@UpdatedBy", request.UpdatedBy)
                };
                DataTable dt = BKICSQL.edt(MotorEndorsementSP.EndorsementOperation, paras);
                if (request.Type == Constants.EndorsementOpeationType.Authorize)
                {                  
                    try
                    {
                        new Task(() =>
                        {
                            OracleDBIntegration.DBObjects.TransactionWrapper oracleResult
                                 = _oracleMotorEndorsement.MoveIntegrationToOracle(request.MotorEndorsementID);
                        }).Start();
                    }                   
                    catch (AggregateException ex)
                    {
                        foreach (Exception inner in ex.InnerExceptions)
                        {
                            _mail.SendMailLogError(ex.Message, "", "Motorendorsement", "", true);
                        }
                    }                   
                }
                return new MotorEndorsementOperationResponse
                {
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new MotorEndorsementOperationResponse()
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
        public MotorEndorsementPreCheckResponse EndorsementPrecheck(MotorEndorsementPreCheckRequest request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                     new  SqlParameter("@docNo", request.DocNo),
                      new SqlParameter ("@type", Constants.Insurance.Motor)
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() {
                        OutPutType = SqlDbType.Bit,
                        ParameterName = "@IsAlreadyHaveEndorsement" , Precision = 38, Scale =3},
                };
                var alreadyHave = false;
                object[] dataSet = BKICSQL.GetValues(AdminSP.EndorsementPreCheck, paras, outParams);
                if (dataSet != null && dataSet[0] != null)
                {
                    alreadyHave = string.IsNullOrEmpty(dataSet[0].ToString()) ? false : Convert.ToBoolean(dataSet[0].ToString());
                }
                return new MotorEndorsementPreCheckResponse()
                {
                    IsTransactionDone = true,
                    IsAlreadyHave = alreadyHave,
                    EndorsementNo = ""
                };
            }
            catch (Exception ex)
            {
                return new MotorEndorsementPreCheckResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all the motor endorsement details for the specific policy.
        /// To show the top of the page on any home endorsement page.
        /// </summary>
        /// <param name="request">Endorsement request.</param>
        /// <returns>list of endorsemnt details.</returns>
        public MotorEndoResult GetAllEndorsements(MotorEndoRequest request)
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
                DataSet motorEndo = BKICSQL.eds(StoredProcedures.PortalSP.GetEndorsementByDocNo, paras);
                List<BO.MotorEndorsement> listEndo = new List<BO.MotorEndorsement>();

                if (motorEndo != null && motorEndo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in motorEndo.Tables[0].Rows)
                    {
                        BO.MotorEndorsement result = new BO.MotorEndorsement();

                        result.MotorEndorsementID = dr.IsNull("MotorEndorsementID") ? 0 : Convert.ToInt64(dr["MotorEndorsementID"]);
                        result.MotorID = dr.IsNull("MotorID") ? 0 : Convert.ToInt64(dr["MotorID"]);
                        result.DocumentNo = dr.IsNull("DocumentNo") ? string.Empty : Convert.ToString(dr["DocumentNo"]);
                        result.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                        result.EndorsementType = dr.IsNull("EndorsementType") ? string.Empty : Convert.ToString(dr["EndorsementType"]);
                        result.VehicleValue = dr.IsNull("VehicleValue") ? 0 : Convert.ToDecimal(dr["VehicleValue"]);
                        result.PremiumBeforeDiscount = dr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(dr["PremiumBeforeDiscount"]);
                        result.PremiumAfterDiscount = dr.IsNull("PremiumAfterDiscount") ? 0 : Convert.ToDecimal(dr["PremiumAfterDiscount"]);
                        result.CommisionBeforeDiscount = dr.IsNull("CommissionBeforeDiscount") ? 0 : Convert.ToDecimal(dr["CommissionBeforeDiscount"]);
                        result.CommissionAfterDiscount = dr.IsNull("CommissionAfterDiscount") ? 0 : Convert.ToDecimal(dr["CommissionAfterDiscount"]);
                        //result.RefundAmount = dr.IsNull("RefundAmount") ? 0 : Convert.ToDecimal(dr["RefundAmount"]);
                        //result.RefundAfterDiscount = dr.IsNull("RefundAfterDiscount") ? 0 : Convert.ToDecimal(dr["RefundAfterDiscount"]);
                        result.PolicyCommencementDate = dr.IsNull("COMMENCEDATE") ? DateTime.Now : Convert.ToDateTime(dr["COMMENCEDATE"]);
                        result.ExpiryDate = dr.IsNull("EXPIRYDATE") ? DateTime.Now : Convert.ToDateTime(dr["EXPIRYDATE"]);
                        result.IsSaved = dr.IsNull("IsSaved") ? false : Convert.ToBoolean(dr["IsSaved"]);
                        result.IsActivePolicy = dr.IsNull("IsActive") ? false : Convert.ToBoolean(dr["IsActive"]);
                        result.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);
                        result.OldInsuredCode = dr.IsNull("OldInsuredCode") ? string.Empty : Convert.ToString(dr["OldInsuredCode"]);
                        result.OldInsuredName = dr.IsNull("OldInsuredName") ? string.Empty : Convert.ToString(dr["OldInsuredName"]);
                        result.InsuredCode = dr.IsNull("InsuredCode") ? string.Empty : Convert.ToString(dr["InsuredCode"]);
                        result.InsuredName = dr.IsNull("InsuredName") ? string.Empty : Convert.ToString(dr["InsuredName"]);
                        result.RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"]);
                        result.TaxOnPremium = dr.IsNull("TaxOnPremium") ? 0 : Convert.ToDecimal(dr["TaxOnPremium"]);

                        listEndo.Add(result);
                    }
                }
                return new MotorEndoResult
                {
                    MotorEndorsements = listEndo,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new MotorEndoResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Calculate endorsement premium by endorsement type.
        /// </summary>
        /// <param name="homeEndorsement">Endorement quote request.</param>
        /// <returns>Returns endorsement premium and commision.</returns>
        public MotorEndorsementQuoteResult GetMotorEndorsementQuote(MotorEndorsementQuote motorEndorsement)
        {
            if (motorEndorsement.EndorsementType == MotorEndorsementTypes.ChangeSumInsured)
            {
                try
                {
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new  SqlParameter("@Agency",motorEndorsement.Agency),
                        new  SqlParameter("@AgentCode",motorEndorsement.AgentCode),
                        new  SqlParameter("@MainClass",motorEndorsement.MainClass),
                        new  SqlParameter("@SubClass",motorEndorsement.SubClass),
                        new SqlParameter("@InsuredCode", motorEndorsement.InsuredCode),
                        new  SqlParameter("@EffectiveFromDate",motorEndorsement.EffectiveFromDate),
                        new  SqlParameter("@EffectiveToDate",motorEndorsement.EffectiveToDate),
                        //new  SqlParameter("@PaidPremium",motorEndorsement.PaidPremium),
                        new  SqlParameter("@NewSumInsured",motorEndorsement.NewSumInsured),
                        new  SqlParameter("@EndorsementType",motorEndorsement.EndorsementType),
                        new SqlParameter("@DocumentNo", motorEndorsement.DocumentNo)
                    };
                    List<SPOut> outParams = new List<SPOut>()
                    {
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@EndorsementPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@RefundPremium", Precision = 38, Scale =3 },
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@RefundVat", Precision = 38, Scale =3},
                    };

                    object[] dataSet = BKICSQL.GetValues(MotorEndorsementSP.GetAdminQuote, paras, outParams);

                    var endorsementPremium = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : decimal.Parse(dataSet[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    var refundPremium = string.IsNullOrEmpty(dataSet[1].ToString()) ? 0 : decimal.Parse(dataSet[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    var refundVat= string.IsNullOrEmpty(dataSet[2].ToString()) ? 0 : decimal.Parse(dataSet[2].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                    return new MotorEndorsementQuoteResult()
                    {
                        IsTransactionDone = true,
                        EndorsementPremium = endorsementPremium,
                        RefundPremium = refundPremium,
                        RefundVat = refundVat
                    };
                }
                catch (Exception ex)
                {
                    return new MotorEndorsementQuoteResult()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = ex.Message
                    };
                }
            }
            else
            {
                try
                {
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new  SqlParameter("@Agency",motorEndorsement.Agency),
                        new  SqlParameter("@AgentCode",motorEndorsement.AgentCode),
                        new SqlParameter("DocumentNo", motorEndorsement.DocumentNo),
                        new  SqlParameter("@MainClass",motorEndorsement.MainClass),
                        new  SqlParameter("@SubClass",motorEndorsement.SubClass),
                        new SqlParameter("@OldInsuredCode", motorEndorsement.InsuredCode ?? string.Empty),
                        new  SqlParameter("@NewInsuredCode",motorEndorsement.NewInsuredCode),
                        new  SqlParameter("@EffectiveFromDate",motorEndorsement.EffectiveFromDate),
                        new  SqlParameter("@EffectiveToDate",motorEndorsement.EffectiveToDate),
                        new  SqlParameter("@CancelationDate",motorEndorsement.CancelationDate),
                        new  SqlParameter("@ExtendedDays",motorEndorsement.ExtendedDays),
                        new  SqlParameter("@PaidPremium",motorEndorsement.PaidPremium),
                        new  SqlParameter("@SumInsured", motorEndorsement.OldSumInsured),
                        new  SqlParameter("@EndorsementType",motorEndorsement.EndorsementType ?? ""),
                        new  SqlParameter("@RefundType",motorEndorsement.RefundType ?? ""),
                    };
                    List<SPOut> outParams = new List<SPOut>()
                    {
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@EndorsementPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@Commission", Precision = 38, Scale =3 },
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@RefundVat", Precision = 38, Scale =3},
                    };

                    object[] dataSet = BKICSQL.GetValues(MotorEndorsementSP.GetQuote, paras, outParams);
                    var endorsementPremium = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : decimal.Parse(dataSet[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    var commission = string.IsNullOrEmpty(dataSet[1].ToString()) ? 0 : decimal.Parse(dataSet[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    var refundVat = string.IsNullOrEmpty(dataSet[2].ToString()) ? 0 : decimal.Parse(dataSet[2].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                    return new MotorEndorsementQuoteResult()
                    {
                        IsTransactionDone = true,
                        EndorsementPremium = endorsementPremium,
                        Commision = commission,
                        RefundVat = refundVat
                    };
                }
                catch (Exception ex)
                {
                    return new MotorEndorsementQuoteResult()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = ex.Message
                    };
                }
            }
        }

        /// <summary>
        /// Insert the motor endorsement.
        /// </summary>
        /// <param name="motorEndorsement">Motor endorsement details.</param>
        /// <returns>Motorendorsementid, Motorendorsementnumber.</returns>
        public MotorEndorsementResult PostMotorEndorsement(BO.MotorEndorsement motorEndorsement)
        {
            try
            {
                var req = new BO.MotorProductRequest
                {
                    Type = "fetch",
                    Agency = motorEndorsement.Agency,
                    AgentCode = motorEndorsement.AgencyCode,
                    MainClass = motorEndorsement.Mainclass,
                    SubClass = motorEndorsement.Subclass,
                };
                BO.MotorProductMasterResponse productRes = _adminRepository.GetMotorProduct(req);
                BO.MotorProductMaster product = productRes.motorProductMaster[0];
                MotorEndorsementResult result = InsertMotorEndorsement(motorEndorsement);
                if(result.IsTransactionDone)
                {
                    
                    if(productRes != null && productRes.IsTransactionDone && productRes.motorProductMaster.Count > 0)
                    {
                        var endorsementType = product.MotorEndorsementMaster
                                              .Find(c => c.EndorsementType == motorEndorsement.EndorsementType);

                        if(endorsementType != null && endorsementType.HasCommission && motorEndorsement.PremiumBeforeDiscount > endorsementType.ChargeAmount)
                        {
                            CalculateCommission(product, motorEndorsement, motorEndorsement.MotorID, result.DocumentNo,
                                            result.LinkID, motorEndorsement.RenewalCount, result.EndorsementCount,
                                            result.EndorsementNo, result.MotorEndorsementID);
                        }                        
                    }                   
                }
                return result;
            }
            catch (Exception ex)
            {
                _mail.SendMailLogError(ex.Message, motorEndorsement.InsuredCode, "MotorEndorsement", motorEndorsement.Agency, false);
                return new MotorEndorsementResult() { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        private  MotorEndorsementResult InsertMotorEndorsement(BO.MotorEndorsement motorEndorsement)
        {
            DataTable motorCovers = new DataTable();
            motorCovers.Columns.Add("CoverCode", typeof(string));
            motorCovers.Columns.Add("CoverDescription", typeof(string));
            motorCovers.Columns.Add("CoverAmount", typeof(decimal));
            motorCovers.Columns.Add("AddedByEndorsement", typeof(bool));

            if (motorEndorsement.Covers != null && motorEndorsement.Covers.Count > 0)
            {
                foreach (var cover in motorEndorsement.Covers)
                {
                    motorCovers.Rows.Add(cover.CoverCode, cover.CoverDescription, cover.CoverAmount, cover.AddedByEndorsement);
                }
            }
            SqlParameter[] paras = new SqlParameter[]
            {
                    new  SqlParameter("@MotorID", motorEndorsement.MotorID),
                    new  SqlParameter("@MotorendorsementID",motorEndorsement.MotorEndorsementID),
                    new  SqlParameter("@EndorsementType",motorEndorsement.EndorsementType ?? ""),
                    new  SqlParameter("@Agency",motorEndorsement.Agency),
                    new  SqlParameter("@AgentCode",motorEndorsement.AgencyCode),
                    new  SqlParameter("@BranchCode",motorEndorsement.AgentBranch ?? ""),
                    new SqlParameter("@CreatedBy" , motorEndorsement.CreatedBy),
                    new  SqlParameter("@DocumentNo",motorEndorsement.DocumentNo ?? ""),
                    new  SqlParameter("@InsuredCode",motorEndorsement.InsuredCode ?? ""),
                    new  SqlParameter("@OldInsuredCode",motorEndorsement.OldInsuredCode ?? " "),
                    new  SqlParameter("@InsuredName",motorEndorsement.InsuredName ?? ""),
                    new  SqlParameter("@OldInsuredName",motorEndorsement.OldInsuredName ?? ""),
                    new  SqlParameter("@RegistrationNo",motorEndorsement.RegistrationNo ?? ""),
                    new  SqlParameter("@OldRegistrationNo",motorEndorsement.OldRegistrationNo ?? ""),
                    new  SqlParameter("@ChassisNo",motorEndorsement.ChassisNo ?? ""),
                    new  SqlParameter("@OldChassisNo",motorEndorsement.OldChassisNo ?? ""),
                    new  SqlParameter("@VechicleValue",motorEndorsement.VehicleValue),
                    new  SqlParameter("@Premium",motorEndorsement.PremiumAmount),
                    new  SqlParameter("@FinanceCompany",motorEndorsement.FinancierCompanyCode ?? ""),
                    new  SqlParameter("@MainClass",motorEndorsement.Mainclass ?? ""),
                    new  SqlParameter("@SubClass",motorEndorsement.Subclass ?? ""),
                    new  SqlParameter("@CommencementDate",motorEndorsement.PolicyCommencementDate),
                    new  SqlParameter("@ExpireDate",motorEndorsement.ExpiryDate),
                    new  SqlParameter("@ExtendedExpireDate",motorEndorsement.ExtendedExpireDate.HasValue ? motorEndorsement.ExtendedExpireDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@CancelDate",motorEndorsement.CancelDate.HasValue ? motorEndorsement.CancelDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@PaymentDate",motorEndorsement.PaymentDate.HasValue ? motorEndorsement.PaymentDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@PaymentType",motorEndorsement.PaymentType ?? ""),
                    new  SqlParameter("@AccountNumber",motorEndorsement.AccountNumber ?? ""),
                    new  SqlParameter("@Remarks",motorEndorsement.Remarks ?? ""),
                    new  SqlParameter("@Source",motorEndorsement.Source ?? ""),
                    new  SqlParameter("@RefundType",motorEndorsement.RefundType ?? ""),
                    new  SqlParameter("@IsSaved",motorEndorsement.IsSaved),
                    new  SqlParameter("@IsActive",motorEndorsement.IsActivePolicy),
                    new SqlParameter("@dt", motorCovers),
                    new  SqlParameter("@RefoundAmount",motorEndorsement.RefundAmount),
                    new  SqlParameter("@RefoundAfterDiscount",motorEndorsement.RefundAfterDiscount),
                    new SqlParameter("@NewExcess",motorEndorsement.NewExcess),
                    new SqlParameter("@CPR",motorEndorsement.CPR),
                    new SqlParameter("@PremiumBeforeDiscount", motorEndorsement.PremiumBeforeDiscount),
                    new SqlParameter("@PremiumAfterDiscount",motorEndorsement.PremiumAfterDiscount),
                    new SqlParameter("@CommissionBeforeDiscount", motorEndorsement.CommisionBeforeDiscount),
                    new SqlParameter("@CommissionAfterDiscount",motorEndorsement.CommissionAfterDiscount),
                    new SqlParameter("@UserChangedPremium",motorEndorsement.UserChangedPremium),
                    new SqlParameter("@VehicleMake", motorEndorsement.VehicleMake),
                    new SqlParameter("@VehicleModel", motorEndorsement.VehicleModel),
                    new SqlParameter("@VehicleYear", motorEndorsement.VehicleYear),
                    new SqlParameter("@VehicleBodyType", motorEndorsement.VehicleBodyType),
                    new SqlParameter("@Tonnage", motorEndorsement.EngineCC)
            };
            List<SPOut> outParams = new List<SPOut>()
            {
                                    new SPOut(){OutPutType = SqlDbType.BigInt, ParameterName= "@NewMotorEndorsementID"},
                                    new SPOut() { OutPutType = SqlDbType.NVarChar,ParameterName = "@EndorsementNumber", Size =50 },
                                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@DocumentNumber", Size=50},
                                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@EndorsementLinkID", Size=50},
                                    new SPOut() { OutPutType = SqlDbType.Int, ParameterName= "@EndorsementCount"},
             };
            object[] dataSet = BKICSQL.GetValues(MotorEndorsementSP.PostMotorEndorsement, paras, outParams);

            var endorsementID = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : Convert.ToInt64(dataSet[0].ToString());
            var endorsementNumber = string.IsNullOrEmpty(dataSet[1].ToString()) ? string.Empty : Convert.ToString(dataSet[1].ToString());
            var documentNumber = string.IsNullOrEmpty(dataSet[2].ToString()) ? string.Empty : Convert.ToString(dataSet[2].ToString());
            var endorsementLinkID = string.IsNullOrEmpty(dataSet[3].ToString()) ? string.Empty : Convert.ToString(dataSet[3].ToString());
            var endorsementCount = string.IsNullOrEmpty(dataSet[4].ToString()) ? 0: Convert.ToInt32(dataSet[4].ToString());

            return new MotorEndorsementResult()
            {
                IsTransactionDone = true,
                EndorsementNo = endorsementNumber,
                MotorEndorsementID = endorsementID,
                LinkID = endorsementLinkID,
                DocumentNo = documentNumber,
                EndorsementCount = endorsementCount
            };
        }       

        /// <summary>
        /// Insert the motor endorsement type of cancel.
        /// </summary>
        /// <param name="motorEndorsement">Motor endorsement request</param>
        /// <returns>Motorendorsementid, Motorendorsementnumber.</returns>
        public MotorEndorsementResult PostAdminMotorEndorsement(BO.MotorEndorsement motorEndorsement)
        {
            MotorEndorsementResult result = InsertAdminMotorEndorsement(motorEndorsement);
            if (result.IsTransactionDone)
            {

                var req = new BO.MotorProductRequest
                {
                    Type = "fetch",
                    Agency = motorEndorsement.Agency,
                    AgentCode = motorEndorsement.AgencyCode,
                    MainClass = motorEndorsement.Mainclass,
                    SubClass = motorEndorsement.Subclass,
                };
                BO.MotorProductMasterResponse productRes = _adminRepository.GetMotorProduct(req);
                BO.MotorProductMaster product = productRes.motorProductMaster[0];
                if (productRes != null && productRes.IsTransactionDone && productRes.motorProductMaster.Count > 0)
                {
                    var endorsementType = product.MotorEndorsementMaster
                                         .Find(c => c.EndorsementType == motorEndorsement.EndorsementType);
                    if (endorsementType != null && endorsementType.HasCommission && motorEndorsement.PremiumBeforeDiscount > endorsementType.ChargeAmount)
                    {
                        CalculateCommission(product, motorEndorsement, motorEndorsement.MotorID, result.DocumentNo,
                                        result.LinkID, motorEndorsement.RenewalCount, result.EndorsementCount,
                                        result.EndorsementNo, result.MotorEndorsementID);
                    }
                }
            }
            return result;
        }

        private MotorEndorsementResult InsertAdminMotorEndorsement(BO.MotorEndorsement motorEndorsement)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@MotorID", motorEndorsement.MotorID),
                    new  SqlParameter("@MotorendorsementID",motorEndorsement.MotorEndorsementID),
                    new  SqlParameter("@EndorsementType",motorEndorsement.EndorsementType ?? ""),
                    new  SqlParameter("@Agency",motorEndorsement.Agency),
                    new  SqlParameter("@AgentCode",motorEndorsement.AgencyCode),
                    new  SqlParameter("@BranchCode",motorEndorsement.AgentBranch ?? ""),
                    new SqlParameter("@CreatedBy" , motorEndorsement.CreatedBy),
                    new  SqlParameter("@DocumentNo",motorEndorsement.DocumentNo ?? ""),
                    new  SqlParameter("@InsuredCode",motorEndorsement.InsuredCode ?? ""),
                    new  SqlParameter("@OldInsuredCode",motorEndorsement.OldInsuredCode ?? " "),
                    new  SqlParameter("@InsuredName",motorEndorsement.InsuredName ?? ""),
                    new  SqlParameter("@OldInsuredName",motorEndorsement.OldInsuredName ?? ""),
                    new  SqlParameter("@RegistrationNo",motorEndorsement.RegistrationNo ?? ""),
                    new  SqlParameter("@OldRegistrationNo",motorEndorsement.OldRegistrationNo ?? ""),
                    new  SqlParameter("@ChassisNo",motorEndorsement.ChassisNo ?? ""),
                    new  SqlParameter("@OldChassisNo",motorEndorsement.OldChassisNo ?? ""),
                    new  SqlParameter("@VechicleValue",motorEndorsement.VehicleValue),
                    new  SqlParameter("@Premium",motorEndorsement.PremiumAmount),
                    new  SqlParameter("@FinanceCompany",motorEndorsement.FinancierCompanyCode ?? ""),
                    new  SqlParameter("@MainClass",motorEndorsement.Mainclass ?? ""),
                    new  SqlParameter("@SubClass",motorEndorsement.Subclass ?? ""),
                    new  SqlParameter("@CommencementDate",motorEndorsement.PolicyCommencementDate),
                    new  SqlParameter("@ExpireDate",motorEndorsement.ExpiryDate),
                    new  SqlParameter("@ExtendedExpireDate",motorEndorsement.ExtendedExpireDate.HasValue ? motorEndorsement.ExtendedExpireDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@CancelDate",motorEndorsement.CancelDate.HasValue ? motorEndorsement.CancelDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@PaymentDate",motorEndorsement.PaymentDate.HasValue ? motorEndorsement.PaymentDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@PaymentType",motorEndorsement.PaymentType ?? ""),
                    new  SqlParameter("@AccountNumber",motorEndorsement.AccountNumber ?? ""),
                    new  SqlParameter("@Remarks",motorEndorsement.Remarks ?? ""),
                    new  SqlParameter("@Source",motorEndorsement.Source ?? ""),
                    new  SqlParameter("@IsSaved",motorEndorsement.IsSaved),
                    new  SqlParameter("@IsActive",motorEndorsement.IsActivePolicy),
                    new  SqlParameter("@RefoundAmount",motorEndorsement.RefundAmount),
                    new  SqlParameter("@RefoundAfterDiscount",motorEndorsement.RefundAfterDiscount),
                    new  SqlParameter("@NewPremium",motorEndorsement.NewPremium),
                    new  SqlParameter("@NewSumInsured",motorEndorsement.NewSumInsured),
                    new SqlParameter("@NewExcess",motorEndorsement.NewExcess),
                    new SqlParameter("@CPR",motorEndorsement.CPR),
                    new SqlParameter("PremiumBeforeDiscount", motorEndorsement.PremiumBeforeDiscount),
                    new SqlParameter("@PremiumAfterDiscount",motorEndorsement.PremiumAfterDiscount),
                    new SqlParameter("@CommissionBeforeDiscount", motorEndorsement.CommisionBeforeDiscount),
                    new SqlParameter("@CommissionAfterDiscount",motorEndorsement.CommissionAfterDiscount),
                    new SqlParameter("@UserChangedPremium",motorEndorsement.UserChangedPremium),
                    new SqlParameter("@VehicleMake", motorEndorsement.VehicleMake),
                    new SqlParameter("@VehicleModel", motorEndorsement.VehicleModel),
                    new SqlParameter("@VehicleYear", motorEndorsement.VehicleYear),
                    new SqlParameter("@VehicleBodyType", motorEndorsement.VehicleBodyType),
                    new SqlParameter("@Tonnage", motorEndorsement.EngineCC)
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                     new SPOut(){OutPutType = SqlDbType.BigInt, ParameterName= "@NewMotorEndorsementID"},
                     new SPOut() { OutPutType = SqlDbType.NVarChar,ParameterName = "@EndorsementNumber", Size =50 },
                     new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@DocumentNumber", Size=50},
                     new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@EndorsementLinkID", Size=50},
                     new SPOut() { OutPutType = SqlDbType.Int, ParameterName= "@EndorsementCount"},
                };

                object[] dataSet = BKICSQL.GetValues(MotorEndorsementSP.PostAdminMotorEndorsement, paras, outParams);

                var endorsementID = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : Convert.ToInt64(dataSet[0].ToString());
                var endorsementNumber = string.IsNullOrEmpty(dataSet[1].ToString()) ? string.Empty : Convert.ToString(dataSet[1].ToString());
                var documentNumber = string.IsNullOrEmpty(dataSet[2].ToString()) ? string.Empty : Convert.ToString(dataSet[2].ToString());
                var endorsementLinkID = string.IsNullOrEmpty(dataSet[3].ToString()) ? string.Empty : Convert.ToString(dataSet[3].ToString());
                var endorsementCount = string.IsNullOrEmpty(dataSet[4].ToString()) ? 0 : Convert.ToInt32(dataSet[4].ToString());

                return new MotorEndorsementResult()
                {
                    IsTransactionDone = true,
                    EndorsementNo = endorsementNumber,
                    MotorEndorsementID = endorsementID,
                    LinkID = endorsementLinkID,
                    DocumentNo = documentNumber,
                    EndorsementCount = endorsementCount
                };
            }
            catch (Exception ex)
            {
                _mail.SendMailLogError(ex.Message, motorEndorsement.InsuredCode, "MotorEndorsement", motorEndorsement.Agency, false);
                return new MotorEndorsementResult()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        public void CalculateCommission(BO.MotorProduct motorProduct, BO.MotorEndorsement endorsement,long motorID, string documentNo, string LinkID, 
                                        int renewalCount, int endorsementCount, string endorsementNumber, long endorsementID)
        {
            try
            {
                int lineNo = 0;
                if (motorProduct.Category != null && motorProduct.Category.Count > 0)
                {
                    List<PolicyCategory> policyCategories = new List<PolicyCategory>();
                    foreach (var dr in motorProduct.Category)
                    {
                        lineNo++;
                        if (!endorsement.UserChangedPremium)
                        {
                            if (dr.ValueType == "Percent")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = dr.IsDeductable ?
                                                                          endorsement.PremiumBeforeDiscount * dr.Value / 100
                                                                          : endorsement.PremiumAfterDiscount * dr.Value / 100;

                                policyCategory.CommissionAfterDiscount = endorsement.PremiumAfterDiscount * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = motorID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = endorsement.PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = endorsement.PremiumBeforeDiscount;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(endorsement.PremiumBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(endorsement.PremiumAfterDiscount, motorProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.MotorID = motorID;
                                policyCategory.MotorEndorsementID = endorsementID;

                                policyCategories.Add(policyCategory);
                            }
                        }
                        else
                        {
                            if (dr.ValueType == "Percent" && dr.IsDeductable)
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = endorsement.PremiumBeforeDiscount * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = endorsement.PremiumAfterDiscount * dr.Value / 100; //CommissionAfterDiscount - NonDeductableCommission;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = motorID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = endorsement.PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = endorsement.PremiumBeforeDiscount;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(endorsement.PremiumBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(endorsement.PremiumAfterDiscount, motorProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.MotorID = motorID;
                                policyCategory.MotorEndorsementID = endorsementID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && !dr.IsDeductable)
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = endorsement.PremiumAfterDiscount * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = endorsement.PremiumAfterDiscount * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = motorID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = endorsement.PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = endorsement.PremiumAfterDiscount;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(endorsement.PremiumAfterDiscount, 5);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(endorsement.PremiumAfterDiscount, 5);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.MotorID = motorID;
                                policyCategory.MotorEndorsementID = endorsementID;

                                policyCategories.Add(policyCategory);
                            }
                        }
                    }
                    if (endorsement.UserChangedPremium)
                    {
                        var commissionDiscount = endorsement.CommissionAfterDiscount;
                        foreach (var pc in policyCategories)
                        {
                            if (pc.IsDeductable)
                            {
                                if (pc.CommissionBeforeDiscount < commissionDiscount)
                                {
                                    pc.CommissionAfterDiscount = pc.CommissionBeforeDiscount;
                                    pc.TaxOnCommissionAfterDiscount = GetTax(pc.CommissionBeforeDiscount, motorProduct.TaxRate);
                                    commissionDiscount = commissionDiscount - pc.CommissionBeforeDiscount;
                                }
                                else
                                {
                                    pc.CommissionAfterDiscount = commissionDiscount;
                                    pc.TaxOnCommissionAfterDiscount = GetTax(commissionDiscount, motorProduct.TaxRate);
                                    commissionDiscount = 0;
                                }
                            }
                        }
                    }
                    InsertCategory(endorsement, policyCategories);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertCategory(BO.MotorEndorsement policy, List<PolicyCategory> policyCategories)
        {
            if (policyCategories != null && policyCategories.Count > 0)
            {
                foreach (var dr in policyCategories)
                {
                    SqlParameter[] paras = new SqlParameter[]
                    {
                                new SqlParameter("@DocumentID", dr.DocumentID),
                                new SqlParameter("@InsuredCode", policy.InsuredCode),
                                new SqlParameter("@LinkID", dr.LinkID),
                                new SqlParameter("@DocumentNo",dr.DocumentNo),
                                new SqlParameter("@EndorsementNo", dr.EndorsementNo ?? string.Empty),
                                new SqlParameter("@EndorsementCount", dr.EndorsementCount),
                                new SqlParameter("@AgentCode", dr.AgentCode),
                                new SqlParameter("@LineNo", dr.LineNo),
                                new SqlParameter("@Category", dr.Category),
                                new SqlParameter("@Code", dr.Code),
                                new SqlParameter("@ValueType", dr.ValueType),
                                new SqlParameter("@Value", dr.Value),
                                new SqlParameter("@PremiumBeforeDiscount", dr.PremiumBeforeDiscount),
                                new SqlParameter("@PremiumAfterDiscount", dr.PremiumAfterDiscount),
                                new SqlParameter("@CommissionBeforeDiscount", dr.CommissionBeforeDiscount),
                                new SqlParameter("@CommissionAfterDiscount", dr.CommissionAfterDiscount),
                                new SqlParameter("@TaxOnPremiumBeforeDiscount", dr.TaxOnPremiumBeforeDiscount),
                                new SqlParameter("@TaxOnPremiumAfterDiscount", dr.TaxOnPremiumAfterDiscount),
                                new SqlParameter("@TaxOnCommissionBeforeDiscount", dr.TaxOnCommissionBeforeDiscount),
                                new SqlParameter("@TaxOnCommissionAfterDiscount", dr.TaxOnCommissionAfterDiscount),
                                new SqlParameter("@IsDeductable", dr.IsDeductable),
                                new SqlParameter("@RenewalCount", dr.RenewalCount),
                                new SqlParameter("@DomesticID", DBNull.Value),
                                new SqlParameter("@TravelID", DBNull.Value),
                                new SqlParameter("@HomeID", DBNull.Value),
                                new SqlParameter("@MotorID", dr.MotorID),
                                new SqlParameter("@MotorEndorsementID", dr.MotorEndorsementID),
                                new SqlParameter("@TravelEndorsementID", DBNull.Value),
                                new SqlParameter("@HomeEndorsementID", DBNull.Value),

                     };
                    BKICSQL.edt(MotorInsuranceSP.PolicyCategoryInsert, paras);
                }
            }
        }

        public decimal GetTax(decimal premium, decimal taxRate)
        {
            return premium * taxRate / 100;
        }
    }
    }
