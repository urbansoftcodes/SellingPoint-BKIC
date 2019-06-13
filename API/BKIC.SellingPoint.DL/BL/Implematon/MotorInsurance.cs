using BKIC.SellingPoint.DL.BL.Implementation;
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
    /// <summary>
    /// Motor insurance methods.
    /// </summary>
    public class MotorInsurance : IMotorInsurance
    {
        public readonly IMail _mail;
        public readonly OracleDBIntegration.Implementation.MotorInsurance _oracleMotorInsurance;


        public MotorInsurance()
        {
            _oracleMotorInsurance = new OracleDBIntegration.Implementation.MotorInsurance();
            _mail = new Mail();
        }

        /// <summary>
        /// Get all motor policies by agency.
        /// </summary>
        /// <param name="req">Agency motor request.</param>
        /// <returns>List of motor policies by agency.</returns>
        public AgencyMotorPolicyResponse GetMotorAgencyPolicy(AgencyMotorRequest req)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency",req.Agency??string.Empty),
                    new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch",req.AgentBranch??string.Empty),
                    new SqlParameter("@IncludeHIR", req.IncludeHIR),
                    new SqlParameter("@IsRenewal", req.IsRenewal),
                    new SqlParameter("@DocumentNo", req.DocumentNo ?? string.Empty)
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.MotorInsuranceSP.GetMotorAgencyPolicy, para);
                List<AgencyMotorPolicy> agencyMotorPolicies = new List<AgencyMotorPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyMotorPolicy();
                        res.MotorID = Convert.ToInt64(dr["MOTORID"]);
                        res.DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]);
                        res.RenewalCount = dr.IsNull("RENEWALCOUNT") ? 0 : Convert.ToInt32(dr["RENEWALCOUNT"]);
                        res.DocumentRenewalNo = res.RenewalCount + "-" + res.DocumentNo;
                        agencyMotorPolicies.Add(res);
                    }
                }
                return new AgencyMotorPolicyResponse
                {
                    IsTransactionDone = true,
                    AgencyMotorPolicies = agencyMotorPolicies
                };
            }
            catch (Exception ex)
            {
                return new AgencyMotorPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Get all motor policies by CPR.
        /// </summary>
        /// <param name="req">Agency motor request.</param>
        /// <returns>List of motor policies by CPR.</returns>
        public AgencyMotorPolicyResponse GetMotorPoliciesByTypeByCPR(AgencyMotorRequest req)
        {
            try
            {
                var response = new AgencyMotorPolicyResponse();

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency",req.Agency??string.Empty),
                    new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                    new SqlParameter("@Type",req.Type??string.Empty),
                    new SqlParameter("@CPR",req.CPR??string.Empty),
                    new SqlParameter("@isEndorsement", req.isEndorsement)
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.MotorInsuranceSP.GETPoliciesByTypeByCPR, para);
                List<AgencyMotorPolicy> agencyMotorPolicies = new List<AgencyMotorPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyMotorPolicy();
                        res.MotorID = Convert.ToInt64(dr["MOTORID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        res.RenewalCount = dr.IsNull("RENEWALCOUNT") ? 0 : Convert.ToInt32(dr["RENEWALCOUNT"]);
                        res.DocumentRenewalNo = res.RenewalCount + "-" + res.DocumentNo;
                        agencyMotorPolicies.Add(res);
                    }
                }
                return new AgencyMotorPolicyResponse
                {
                    IsTransactionDone = true,
                    AgencyMotorPolicies = agencyMotorPolicies
                };
            }
            catch (Exception ex)
            {
                return new AgencyMotorPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = true
                };
            }
        }

        /// <summary>
        /// <summary>
        /// Get all motor policies by Document Number(If it has any renewed list out all renewal details).
        /// </summary>
        /// <param name="req">Agency policy details request.</param>
        /// <returns>List of motor policies by document number(Renewal and new policies).</returns>
        public AgencyMotorPolicyResponse GetMotorPoliciesByDocumentNo(AgencyPolicyDetailsRequest req)
        {
            try
            {
                var response = new AgencyMotorPolicyResponse();

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency",req.Agency??string.Empty),
                    new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                    new SqlParameter("@InsuranceType",req.InsuranceType ??string.Empty),
                    new SqlParameter("@DocumentNo",req.DocumentNo ??string.Empty)                  
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.MotorInsuranceSP.GETMotorPoliciesByDocumentNo, para);
                List<AgencyMotorPolicy> agencyMotorPolicies = new List<AgencyMotorPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyMotorPolicy();
                        res.MotorID = Convert.ToInt64(dr["MOTORID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);                       
                        res.PolicyStartDate = Convert.ToDateTime(dr["PolicyStartDate"]);
                        res.PolicyEndDate = Convert.ToDateTime(dr["PolicyEndDate"]);
                        res.RenewalCount = Convert.ToInt32(dr["RenewalCount"]);
                        agencyMotorPolicies.Add(res);
                    }
                }
                return new AgencyMotorPolicyResponse
                {
                    IsTransactionDone = true,
                    AgencyMotorPolicies = agencyMotorPolicies
                };
            }
            catch (Exception ex)
            {
                return new AgencyMotorPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = true
                };
            }
        }

        /// <summary>
        /// Calculate the excess amount based on vehicle make and model.
        /// e.g for make BMW and model 350i the excess amount is BD 100.
        /// </summary>
        /// <param name="request">Excess amount request.</param>
        /// <returns>Excess amount and body type.</returns>
        public ExcessAmountResponse GetExcessCalcualtion(ExcessAmountRequest request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                     new  SqlParameter("@vehicleMake",request.VehicleMake),
                     new  SqlParameter("@VehicleModel",request.VehicleModel),
                     new  SqlParameter("@VehicleType",request.VehicleType ?? ""),
                     new  SqlParameter("@ExcessType",request.ExcessType),
                     new  SqlParameter("@Agency",request.Agency ?? ""),
                     new  SqlParameter("@AgentCode",request.AgentCode ?? ""),
                     new  SqlParameter("@MainClass",request.MainClass ?? ""),
                     new  SqlParameter("@SubClass",request.SubClass ?? ""),
                     new  SqlParameter("@IsUnderAge",request.IsUnderAge)
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@ExcessAmount",Precision = 38, Scale =3},
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@BodyType", Size=50}
                };
                object[] dataSet = BKICSQL.GetValues(MotorInsuranceSP.ExcessAmount, paras, outParams);
                var ExcessAmount = decimal.Parse(dataSet[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                var BodyType = string.IsNullOrEmpty(dataSet[1].ToString()) ? "" : dataSet[1].ToString();
                return new ExcessAmountResponse
                {
                    ExcessAmount = ExcessAmount,
                    BodyType = BodyType,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new ExcessAmountResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get Optional cover for the motor product,TISCO have optional covers for the product (GLD,ELT,NMC)
        /// </summary>
        /// <param name="req">Optional cover request.</param>
        /// <returns>List of optional covers.</returns>
        public OptionalCoverResponse GetOptionalCover(OptionalCoverRequest req)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]{
                    new  SqlParameter("@Agency",req.Agency),
                    new  SqlParameter("@AgentCode",req.AgentCode),
                    new  SqlParameter("@MainClass",req.MainClass),
                    new  SqlParameter("@SubClass",req.SubClass),
                };
                DataTable resultdt = BKICSQL.edt(StoredProcedures.MotorInsuranceSP.GetOptionalCovers, paras);
                List<MotorCovers> mc = new List<MotorCovers>();
                if (resultdt != null && resultdt.Rows.Count > 0)
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        var res = new MotorCovers();
                        res.CoverCode = Convert.ToString(dr["CoverCode"]);
                        res.CoverDescription = Convert.ToString(dr["CoverCodeDescription"]);
                        res.CoverAmount = Convert.ToDecimal(dr["CoverAmount"]);
                        mc.Add(res);
                    }
                }
                return new OptionalCoverResponse
                {
                    IsTransactionDone = true,
                    OptionalCovers = mc
                };
            }
            catch (Exception ex)
            {
                return new OptionalCoverResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Calculate optional cover amount.
        /// </summary>
        /// <param name="req">calculate cover request.</param>
        /// <returns>Optional cover amount.</returns>
        public CalculateCoverAmountResponse CalculateOptionalCoverAmount(CalculateCoverAmountRequest req)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]{
                    new  SqlParameter("@Agency",req.Agency),
                    new  SqlParameter("@AgentCode",req.AgentCode),
                    new  SqlParameter("@MainClass",req.MainClass),
                    new  SqlParameter("@SubClass",req.SubClass),
                    new SqlParameter("CoverCode", req.CoverCode),
                    new  SqlParameter("@BaseCoverAmount", req.BaseCoverAmount),
                    new SqlParameter("@NoOfSeats", req.NoOfSeats),
                    new SqlParameter("@SumInsured", req.SumInsured)
                };
                List<SPOut> outParams = new List<SPOut>()
                    {
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@OptionalCoverAmount",Precision = 38, Scale =3},
                    };
                var ds = BKICSQL.GetValues(StoredProcedures.MotorInsuranceSP.CalculateOptionalCoverAmount, paras, outParams);
                decimal OptionalCoverAmount = 0;
                if (ds != null && ds[0] != null)
                {
                    OptionalCoverAmount = Convert.ToDecimal(ds[0].ToString());
                }
                return new CalculateCoverAmountResponse
                {
                    IsTransactionDone = true,
                    CoverAmount = OptionalCoverAmount
                };
            }
            catch (Exception ex)
            {
                return new CalculateCoverAmountResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Calculate motor premium.
        /// </summary>
        /// <param name="insurance">Motor quote request.</param>
        /// <returns>Motor premium.</returns>
        public MotorInsuranceQuoteResponse GetMotorInsuranceQuote(MotorInsuranceQuote insurance)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]{
                new  SqlParameter("@Agency",insurance.Agency),
                new  SqlParameter("@AgencyCode",insurance.AgentCode),
                new  SqlParameter("@YearOfMake",insurance.YearOfMake ?? string.Empty),
                new  SqlParameter("@VehicleMake",insurance.VehicleMake ?? string.Empty),
                new  SqlParameter("@VehicleModel",insurance.VehicleModel ?? string.Empty),
                new  SqlParameter("@VehicleType",insurance.VehicleType ?? string.Empty),
                new  SqlParameter("@VehicleSumInsured",insurance.VehicleSumInsured),
                new  SqlParameter("@TypeOfInsurance",insurance.TypeOfInsurance),
                new  SqlParameter("@IsNCB",insurance.IsNCB),
                new  SqlParameter("@NCBFromDate",insurance.NCBFromDate.HasValue?insurance.NCBFromDate.Value:(object)DBNull.Value),
                new  SqlParameter("@NCBToDate",insurance.NCBToDate.HasValue?insurance.NCBToDate.Value:(object)DBNull.Value),
                new  SqlParameter("@PolicyStartDate",insurance.PolicyStartDate),
                new  SqlParameter("@PolicyEndDate",insurance.PolicyEndDate),
                new  SqlParameter("@RegistrationMonth",insurance.RegistrationMonth ?? string.Empty),
                new  SqlParameter("@DOB",insurance.DOB),
                new SqlParameter("@ExcessType", insurance.ExcessType ?? string.Empty),
                new SqlParameter("@MainClass", insurance.MainClass),
                new SqlParameter("@OptionalCoverAmount", insurance.OptionalCoverAmount),
                new SqlParameter("@RenewalDelayedDays", insurance.RenewalDelayedDays)
                };

                List<SPOut> outParams = new List<SPOut>() {
                new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@TotalPremium" , Precision = 38, Scale =3},
                 new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@DiscountPremium", Precision = 38, Scale =3 }};

                object[] dataSet = BKICSQL.GetValues(MotorInsuranceSP.GetQuote, paras, outParams);

                var premium = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : decimal.Parse(dataSet[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                var discount = string.IsNullOrEmpty(dataSet[1].ToString()) ? 0 : decimal.Parse(dataSet[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                return new MotorInsuranceQuoteResponse()
                {
                    IsTransactionDone = true,
                    TotalPremium = premium,
                    DiscountPremium = discount
                };
            }
            catch (Exception ex)
            {
                return new MotorInsuranceQuoteResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Fetch HIR or Active policies for admins.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AdminFetchMotorDetailsResponse FetchMotorPolicyDetails(AdminFetchMotorDetailsRequest request)
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
                    new SqlParameter("@UserID", request.UserID),
                    new SqlParameter("@ByUserID", request.ByUserID)
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
                            GrossPremium = dr.IsNull("GROSSPREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["GROSSPREMIUM"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            InsuredCode = dr.IsNull("InsuredCode") ? string.Empty : Convert.ToString(dr["InsuredCode"]),
                            HIRReason = dr.IsNull("HIRReason") ? string.Empty : Convert.ToString(dr["HIRReason"]),
                            HIRStatus = dr.IsNull("HIRStatus") ? string.Empty : Convert.ToString(dr["HIRStatus"]),
                            HIRStatusDesc = dr.IsNull("StatusID") ? string.Empty : Convert.ToString(dr["StatusID"]),
                            LinkID = dr.IsNull("LinkID") ? string.Empty : Convert.ToString(dr["LinkID"]),
                            Agency = dr.IsNull("AGENCY") ? string.Empty : Convert.ToString(dr["AGENCY"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            IsMessageAvailable = dr.IsNull("IsMessageAvailable") ? false : Convert.ToBoolean(dr["IsMessageAvailable"]),
                            IsDocumentsAvailable = dr.IsNull("IsDocumentAvailable") ? false : Convert.ToBoolean(dr["IsDocumentAvailable"]),
                            MainClass = dr.IsNull("MAINCLASS") ? string.Empty : Convert.ToString(dr["MAINCLASS"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            CreatedDate = dr.IsNull("CREATEDDATE") ? DateTime.Now : Convert.ToDateTime(dr["CREATEDDATE"]),
                            HIRRemarks = dr.IsNull("HIRRemarks") ? string.Empty : Convert.ToString(dr["HIRRemarks"]),
                            RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"])
                        };
                        policylist.Add(detail);
                    }
                }
                if (resultdt != null && resultdt.Rows.Count > 0 && request.Type == "Active")
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        MotorPolicyDetails detail = new MotorPolicyDetails()
                        {
                            MotorID = Convert.ToInt64(dr["ID"]),
                            DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]),
                            InsuredName = dr.IsNull("INSUREDNAME") ? string.Empty : Convert.ToString(dr["INSUREDNAME"]),
                            InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]),
                            GrossPremium = dr.IsNull("PREMIUM") ? decimal.Zero : Convert.ToDecimal(dr["PREMIUM"]),
                            AgentCode = dr.IsNull("AGENTCODE") ? string.Empty : Convert.ToString(dr["AGENTCODE"]),
                            Agency = dr.IsNull("AGENCY") ? string.Empty : Convert.ToString(dr["AGENCY"]),
                            PaymentDate = dr.IsNull("PAYMENTDATE") ? Convert.ToDateTime(dr["CREATEDDATE"]) : Convert.ToDateTime(dr["PAYMENTDATE"]),
                            CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]),
                            //PaymentType = Convert.ToString(dr["PAYMENTDATE"]),
                            Subclass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]),
                            RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"])
                            // CreatedDate = Convert.ToDateTime(dr["CREATEDDATE"])
                            
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
        /// Get motor insurance certificate.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agency">Agency</param>
        /// <param name="isEndorsement">Get the certificate on endorsement page if it is true otherwise false.</param>
        /// <param name="endorsementID">Endorsement id.</param>
        /// <returns></returns>
        public InsuranceCertificateResponse GetInsuranceCertificate(string documentNo, string type, string agency,
                                             bool isEndorsement = false, long endorsementID = 0, int renewalCount = 0)
        {
            try
            {
                InsuranceCertificateResponse motorcertificate = new InsuranceCertificateResponse();
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@DocumentNo",documentNo),
                    new SqlParameter("@Type", ""),
                    new SqlParameter("@AgentCode", agency),
                    new SqlParameter("@IsEndorsement", isEndorsement),
                    new SqlParameter("@EndorsementID", endorsementID),
                    new SqlParameter("@RenewalCount", renewalCount)
                };
                DataSet certificateds = BKICSQL.eds(MotorInsuranceSP.GetMotorPolicyByDocNo, param);
                if (certificateds != null && certificateds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = certificateds.Tables[0].Rows[0];
                    var IsUnderBCFC = dr.IsNull("IsUnderBCFC") ? false : Convert.ToBoolean(dr["IsUnderBCFC"]);

                    var InsuredName = IsUnderBCFC ? "BAHRAIN COMMERCIAL FACILITIE COMPANY" : Convert.ToString(dr["INSUREDNAME"]);

                    motorcertificate.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                    motorcertificate.InsuredName = InsuredName;
                    motorcertificate.ExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                    motorcertificate.YearOfMake = dr.IsNull("YEAR") ? string.Empty : Convert.ToString(dr["YEAR"]);
                    motorcertificate.VehicleMake = Convert.ToString(dr["MAKE"]);
                    motorcertificate.VehicleModel = Convert.ToString(dr["MODEL"]);
                    motorcertificate.PolicyCode = Convert.ToString(dr["SUBCLASS"]);
                    motorcertificate.VehicleValue = !dr.IsNull("VEHICLEVALUE") ? Convert.ToDecimal(dr["VEHICLEVALUE"]) : 0;
                    motorcertificate.Premium = dr.IsNull("GROSSPREMIUM") ? 0 : Convert.ToDecimal(dr["GROSSPREMIUM"]);
                    motorcertificate.CommencementDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                    motorcertificate.RegistrationNo = Convert.ToString(dr["REGISTRATIONNO"]);
                    motorcertificate.ChassisNo = Convert.ToString(dr["CHASSISNO"]);
                    motorcertificate.ExcessAmount = dr.IsNull("EXCESSAMOUNT") ? 0 : Convert.ToDecimal(dr["EXCESSAMOUNT"]);
                    motorcertificate.Source = Convert.ToString(dr["Source"]);
                    motorcertificate.PolicyNo = Convert.ToString(dr["DOCUMENTNO"]);
                    motorcertificate.Subclass = Convert.ToString(dr["SUBCLASS"]);
                    motorcertificate.CreatedDate = Convert.ToDateTime(dr["CREATEDDATE"]);
                    motorcertificate.FinanceCompany = Convert.ToString(dr["FinanceCompany"]);
                    motorcertificate.CoverType = "Comprehensive";
                    motorcertificate.IsTransactionDone = true;
                }
                if (certificateds != null && certificateds.Tables.Count > 1
                    && certificateds.Tables[1] != null && certificateds.Tables[1].Rows.Count > 0)
                {
                    motorcertificate.Covers = new List<MotorCovers>();
                    for (int i = 0; i < certificateds.Tables[1].Rows.Count; i++)
                    {
                        motorcertificate.Covers.Add(
                                new MotorCovers
                                {
                                    CoverCode = certificateds.Tables[1].Rows[i].IsNull("CoverCode") ? ""
                                                : certificateds.Tables[1].Rows[i]["CoverCode"].ToString(),
                                    CoverDescription = certificateds.Tables[1].Rows[i].IsNull("CoverCodeDescription") ? ""
                                                       : certificateds.Tables[1].Rows[i]["CoverCodeDescription"].ToString(),
                                    CoverAmount = certificateds.Tables[1].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                                  : Convert.ToDecimal(certificateds.Tables[1].Rows[i]["CoverAmount"].ToString()),
                                });
                    }
                }
                if (certificateds != null && certificateds.Tables.Count > 1
                 && certificateds.Tables[3] != null && certificateds.Tables[3].Rows.Count > 0)
                {
                    motorcertificate.OptionalCovers = new List<MotorCovers>();
                    for (int i = 0; i < certificateds.Tables[3].Rows.Count; i++)
                    {
                        motorcertificate.OptionalCovers.Add(
                                new MotorCovers
                                {
                                    CoverCode = certificateds.Tables[3].Rows[i].IsNull("CoverCode") ? ""
                                                : certificateds.Tables[3].Rows[i]["CoverCode"].ToString(),
                                    CoverDescription = certificateds.Tables[3].Rows[i].IsNull("CoverCodeDescription") ? ""
                                                       : certificateds.Tables[3].Rows[i]["CoverCodeDescription"].ToString(),
                                    CoverAmount = certificateds.Tables[3].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                                  : Convert.ToDecimal(certificateds.Tables[3].Rows[i]["CoverAmount"].ToString()),
                                });
                    }
                }
                if (motorcertificate.IsTransactionDone)
                {
                    motorcertificate.FilePath = ExtensionMethods.CreateMotorInvoice(motorcertificate);
                }
                return motorcertificate;
            }
            catch (Exception ex)
            {
                return new InsuranceCertificateResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get motor policy details by the document number.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">Get the policy details for the endorsemnt page or policy buy page.</param>
        /// <param name="endorsementID">Endorsement id.</param>
        /// <returns></returns>
        public MotorSavedQuotationResponse GetSavedMotorPolicy(string documentNo, string type, string agentCode,
                                                            bool isEndorsement = false, long endorsementID = 0, int renewalCount = 0)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@DocumentNo",documentNo),
                    new SqlParameter("@Type",string.IsNullOrEmpty(type)? "" : type),
                    new SqlParameter("@AgentCode",agentCode),
                    new SqlParameter("@IsEndorsement", isEndorsement),
                    new SqlParameter("@EndorsementID", endorsementID),
                    new SqlParameter("@RenewalCount", renewalCount)
                };
                DataSet motords = BKICSQL.eds(StoredProcedures.MotorInsuranceSP.GetMotorPolicyByDocNo, paras);
                MotorInsurancePolicy motorInsurancePolicy = new MotorInsurancePolicy();

                if (motords != null && motords.Tables[0] != null && motords.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = motords.Tables[0].Rows[0];

                    motorInsurancePolicy.MotorID = Convert.ToInt64(dr["MOTORID"]);
                    motorInsurancePolicy.Agency = dr.IsNull("Agency") ? "" : Convert.ToString(dr["Agency"]);
                    motorInsurancePolicy.AgencyCode = dr.IsNull("AgentCode") ? "" : Convert.ToString(dr["AgentCode"]);
                    motorInsurancePolicy.InsuredCode = dr.IsNull("INSUREDCODE") ? "" : Convert.ToString(dr["INSUREDCODE"]);
                    motorInsurancePolicy.InsuredName = dr.IsNull("INSUREDNAME") ? "" : Convert.ToString(dr["INSUREDNAME"]);
                    motorInsurancePolicy.CPR = dr.IsNull("CPR") ? "" : Convert.ToString(dr["CPR"]);
                    motorInsurancePolicy.ExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                    motorInsurancePolicy.YearOfMake = dr.IsNull("YEAR") ? 0 : Convert.ToInt32(dr["YEAR"]);
                    motorInsurancePolicy.VehicleMake = Convert.ToString(dr["MAKE"]);
                    motorInsurancePolicy.VehicleModel = Convert.ToString(dr["MODEL"]);
                    motorInsurancePolicy.vehicleTypeCode = Convert.ToString(dr["VEHICLETYPE"]);
                    motorInsurancePolicy.Subclass = Convert.ToString(dr["SUBCLASS"]);
                    motorInsurancePolicy.DOB = Convert.ToDateTime(dr["DateOfBirth"]);
                    motorInsurancePolicy.vehicleBodyType = dr.IsNull("BODY") ? "" : Convert.ToString(dr["BODY"]);
                    motorInsurancePolicy.VehicleValue = !dr.IsNull("VEHICLEVALUE") ? Convert.ToDecimal(dr["VEHICLEVALUE"]) : 0;
                    motorInsurancePolicy.PolicyCommencementDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                    motorInsurancePolicy.RegistrationNumber = Convert.ToString(dr["REGISTRATIONNO"]);
                    motorInsurancePolicy.ChassisNo = Convert.ToString(dr["CHASSISNO"]);
                    motorInsurancePolicy.EngineCC = Convert.ToInt32(dr["TONNAGE"]);
                    motorInsurancePolicy.FinancierCompanyCode = Convert.ToString(dr["FINANCECOMPANY"]);
                    motorInsurancePolicy.PaymentType = dr.IsNull("PaymentType") ? string.Empty : Convert.ToString(dr["PaymentType"]);
                    motorInsurancePolicy.AccountNumber = dr.IsNull("AccountNo") ? string.Empty : Convert.ToString(dr["AccountNo"]);
                    motorInsurancePolicy.Remarks = dr.IsNull("Remark") ? string.Empty : Convert.ToString(dr["Remark"]);
                    motorInsurancePolicy.PremiumBeforeDiscount = dr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(dr["PremiumBeforeDiscount"]);
                    motorInsurancePolicy.PremiumAfterDiscount = dr.IsNull("PremiumAfterDiscount") ? 0 : Convert.ToDecimal(dr["PremiumAfterdiscount"]);
                    motorInsurancePolicy.CommisionBeforeDiscount = dr.IsNull("CommissionBeforeDiscount") ? 0 : Convert.ToDecimal(dr["CommissionBeforeDiscount"]);
                    motorInsurancePolicy.CommissionAfterDiscount = dr.IsNull("CommissionAfterDiscount") ? 0 : Convert.ToDecimal(dr["CommissionAfterDiscount"]);
                    motorInsurancePolicy.ExcessType = Convert.ToString(dr["EXCESSTYPE"]);
                    motorInsurancePolicy.ExcessAmount = dr.IsNull("EXCESSAMOUNT") ? 0 : Convert.ToDecimal(dr["EXCESSAMOUNT"]);
                    motorInsurancePolicy.Branch = Convert.ToString(dr["Source"]);
                    motorInsurancePolicy.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                    motorInsurancePolicy.Mainclass = Convert.ToString(dr["MAINCLASS"]);
                    motorInsurancePolicy.IsSaved = dr.IsNull("IsSaved") ? false : Convert.ToBoolean(dr["IsSaved"]);
                    motorInsurancePolicy.IsActivePolicy = dr.IsNull("IsActive") ? false : Convert.ToBoolean(dr["IsActive"]);
                    motorInsurancePolicy.IsHIR = dr.IsNull("IsHIR") ? false : Convert.ToBoolean(dr["IsHIR"]);
                    motorInsurancePolicy.HIRStatus = dr.IsNull("HIRStatus") ? 0 : Convert.ToInt32(dr["HIRStatus"]);
                    motorInsurancePolicy.EndorsementCount = dr.IsNull("EndorsementCount") ? 0 : Convert.ToInt32(dr["EndorsementCount"]);
                    motorInsurancePolicy.IsCancelled = dr.IsNull("IsCancelled") ? false : Convert.ToBoolean(dr["IsCancelled"]);
                    motorInsurancePolicy.TaxOnPremium = dr.IsNull("TaxOnPremium") ? 0 : Convert.ToDecimal(dr["TaxOnPremium"]);
                    motorInsurancePolicy.TaxOnCommission = dr.IsNull("TaxOnCommission") ? 0 : Convert.ToDecimal(dr["TaxOnCommission"]);
                    motorInsurancePolicy.SeatingCapacity = dr.IsNull("SeatingCapacity") ? 0 : Convert.ToInt32(dr["SeatingCapacity"]);
                    motorInsurancePolicy.IsUnderBCFC = dr.IsNull("IsUnderBCFC") ? false : Convert.ToBoolean(dr["IsUnderBCFC"]);
                    motorInsurancePolicy.LoadAmount = dr.IsNull("LoadAmount") ? decimal.Zero : Convert.ToDecimal(dr["LoadAmount"]);
                    motorInsurancePolicy.RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"]);
                    motorInsurancePolicy.ClaimAmount = dr.IsNull("ClaimAmount") ? 0 : Convert.ToDecimal(dr["ClaimAmount"]);
                    motorInsurancePolicy.EndorsementType = dr.IsNull("EndorsementType") ? string.Empty : Convert.ToString(dr["EndorsementType"]);
                    motorInsurancePolicy.RenewalDelayedDays = dr.IsNull("RenewalDelayedDays") ? 0 : Convert.ToInt32(dr["RenewalDelayedDays"]);
                    motorInsurancePolicy.ActualRenewalStartDate = dr.IsNull("ActualRenewalStartDate") ? (DateTime?)null : Convert.ToDateTime(dr["ActualRenewalStartDate"]);

                    //Policy default cover.
                    if (motords != null && motords.Tables.Count > 1 && motords.Tables[1] != null && motords.Tables[1].Rows.Count > 0)
                    {
                        motorInsurancePolicy.Covers = new List<MotorCovers>();
                        for (int i = 0; i < motords.Tables[1].Rows.Count; i++)
                        {
                            motorInsurancePolicy.Covers.Add(
                                    new MotorCovers
                                    {
                                        CoverCode = motords.Tables[1].Rows[i].IsNull("CoverCode") ? ""
                                                     : motords.Tables[1].Rows[i]["CoverCode"].ToString(),
                                        CoverDescription = motords.Tables[1].Rows[i].IsNull("CoverCodeDescription") ? ""
                                                      : motords.Tables[1].Rows[i]["CoverCodeDescription"].ToString(),
                                        CoverAmount = motords.Tables[1].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                                     : Convert.ToDecimal(motords.Tables[1].Rows[i]["CoverAmount"].ToString()),
                                        AddedByEndorsement = motords.Tables[1].Rows[i].IsNull("AddedByEndorsement") ? false
                                                   : Convert.ToBoolean(motords.Tables[1].Rows[i]["AddedByEndorsement"].ToString()),
                                    });
                        }
                    }
                    //Product cover belonging to particular product.
                    if (motords != null && motords.Tables.Count > 1 && motords.Tables[2] != null && motords.Tables[2].Rows.Count > 0)
                    {
                        motorInsurancePolicy.ProductCovers = new List<MotorCovers>();
                        for (int i = 0; i < motords.Tables[2].Rows.Count; i++)
                        {
                            motorInsurancePolicy.ProductCovers.Add(
                                    new MotorCovers
                                    {
                                        CoverCode = motords.Tables[2].Rows[i].IsNull("CoverCode") ? ""
                                                   : motords.Tables[2].Rows[i]["CoverCode"].ToString(),
                                        CoverDescription = motords.Tables[2].Rows[i].IsNull("CoverCodeDescription") ? ""
                                                  : motords.Tables[2].Rows[i]["CoverCodeDescription"].ToString(),
                                        CoverAmount = motords.Tables[2].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                                  : Convert.ToDecimal(motords.Tables[2].Rows[i]["CoverAmount"].ToString()),
                                        AddedByEndorsement = false
                                    });
                        }
                    }
                    //Optional cover for the policy if it has.
                    if (motords != null && motords.Tables.Count > 1 && motords.Tables[3] != null && motords.Tables[3].Rows.Count > 0)
                    {
                        motorInsurancePolicy.OptionalCovers = new List<MotorCovers>();
                        for (int i = 0; i < motords.Tables[3].Rows.Count; i++)
                        {
                            motorInsurancePolicy.OptionalCovers.Add(
                                    new MotorCovers
                                    {
                                        CoverCode = motords.Tables[3].Rows[i].IsNull("CoverCode") ? ""
                                                   : motords.Tables[3].Rows[i]["CoverCode"].ToString(),
                                        CoverDescription = motords.Tables[3].Rows[i].IsNull("CoverCodeDescription") ? ""
                                                  : motords.Tables[3].Rows[i]["CoverCodeDescription"].ToString(),
                                        CoverAmount = motords.Tables[3].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                                  : Convert.ToDecimal(motords.Tables[3].Rows[i]["CoverAmount"].ToString()),
                                        AddedByEndorsement = false
                                    });
                        }
                    }
                    return new MotorSavedQuotationResponse
                    {
                        IsTransactionDone = true,
                        MotorPolicyDetails = motorInsurancePolicy
                    };
                } 
                return new MotorSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = "Policy not found"
                };                
            }
            catch (Exception ex)
            {
                return new MotorSavedQuotationResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Get renewal motor policy details by the document number.
        /// Actually get it from the Oracle temporary table.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">Get the policy details for the endorsemnt page or policy buy page.</param>
        /// <param name="endorsementID">Endorsement id.</param>
        /// <returns></returns>
        public MotorSavedQuotationResponse GetOracleRenewMotorPolicy(string documentNo, string agency, string agentCode)
                                                           
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@OracleDocumentNo",documentNo),
                    new SqlParameter("@Agency",agency ?? string.Empty),
                    new SqlParameter("@AgentCode",documentNo ?? string.Empty),

                };
                DataSet motords = BKICSQL.eds(StoredProcedures.MotorInsuranceSP.GetOracleMotorRenewalPolicyByDocNo, paras);

                MotorInsurancePolicy motorInsurancePolicy = new MotorInsurancePolicy();
                if (motords != null && motords.Tables[0] != null && motords.Tables[0].Rows.Count > 0)
                {

                    if (motords.Tables[0].Rows[0].Table.Columns.Contains("IsSystem"))
                    {
                        DataRow dr = motords.Tables[0].Rows[0];
                        motorInsurancePolicy.MotorID = Convert.ToInt64(dr["MOTORID"]);
                        motorInsurancePolicy.Agency = dr.IsNull("Agency") ? "" : Convert.ToString(dr["Agency"]);
                        motorInsurancePolicy.AgencyCode = dr.IsNull("AgentCode") ? "" : Convert.ToString(dr["AgentCode"]);
                        motorInsurancePolicy.InsuredCode = dr.IsNull("INSUREDCODE") ? "" : Convert.ToString(dr["INSUREDCODE"]);
                        motorInsurancePolicy.InsuredName = dr.IsNull("INSUREDNAME") ? "" : Convert.ToString(dr["INSUREDNAME"]);
                        motorInsurancePolicy.CPR = dr.IsNull("CPR") ? "" : Convert.ToString(dr["CPR"]);
                        motorInsurancePolicy.ExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                        motorInsurancePolicy.YearOfMake = dr.IsNull("YEAR") ? 0 : Convert.ToInt32(dr["YEAR"]);
                        motorInsurancePolicy.VehicleMake = Convert.ToString(dr["MAKE"]);
                        motorInsurancePolicy.VehicleModel = Convert.ToString(dr["MODEL"]);
                        motorInsurancePolicy.vehicleTypeCode = Convert.ToString(dr["VEHICLETYPE"]);
                        motorInsurancePolicy.Subclass = Convert.ToString(dr["SUBCLASS"]);
                        motorInsurancePolicy.DOB = Convert.ToDateTime(dr["DateOfBirth"]);
                        motorInsurancePolicy.vehicleBodyType = dr.IsNull("BODY") ? "" : Convert.ToString(dr["BODY"]);
                        motorInsurancePolicy.VehicleValue = !dr.IsNull("VEHICLEVALUE") ? Convert.ToDecimal(dr["VEHICLEVALUE"]) : 0;
                        motorInsurancePolicy.PolicyCommencementDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                        motorInsurancePolicy.RegistrationNumber = Convert.ToString(dr["REGISTRATIONNO"]);
                        motorInsurancePolicy.ChassisNo = Convert.ToString(dr["CHASSISNO"]);
                        motorInsurancePolicy.EngineCC = Convert.ToInt32(dr["TONNAGE"]);
                        motorInsurancePolicy.FinancierCompanyCode = Convert.ToString(dr["FINANCECOMPANY"]);
                        motorInsurancePolicy.PaymentType = dr.IsNull("PaymentType") ? string.Empty : Convert.ToString(dr["PaymentType"]);
                        motorInsurancePolicy.AccountNumber = dr.IsNull("AccountNo") ? string.Empty : Convert.ToString(dr["AccountNo"]);
                        motorInsurancePolicy.Remarks = dr.IsNull("Remark") ? string.Empty : Convert.ToString(dr["Remark"]);
                        motorInsurancePolicy.PremiumBeforeDiscount = dr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(dr["PremiumBeforeDiscount"]);
                        motorInsurancePolicy.PremiumAfterDiscount = dr.IsNull("PremiumAfterDiscount") ? 0 : Convert.ToDecimal(dr["PremiumAfterdiscount"]);
                        motorInsurancePolicy.CommisionBeforeDiscount = dr.IsNull("CommissionBeforeDiscount") ? 0 : Convert.ToDecimal(dr["CommissionBeforeDiscount"]);
                        motorInsurancePolicy.CommissionAfterDiscount = dr.IsNull("CommissionAfterDiscount") ? 0 : Convert.ToDecimal(dr["CommissionAfterDiscount"]);
                        motorInsurancePolicy.ExcessType = Convert.ToString(dr["EXCESSTYPE"]);
                        motorInsurancePolicy.ExcessAmount = dr.IsNull("EXCESSAMOUNT") ? 0 : Convert.ToDecimal(dr["EXCESSAMOUNT"]);
                        motorInsurancePolicy.Branch = Convert.ToString(dr["Source"]);
                        motorInsurancePolicy.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        motorInsurancePolicy.Mainclass = Convert.ToString(dr["MAINCLASS"]);
                        motorInsurancePolicy.IsSaved = dr.IsNull("IsSaved") ? false : Convert.ToBoolean(dr["IsSaved"]);
                        motorInsurancePolicy.IsActivePolicy = dr.IsNull("IsActive") ? false : Convert.ToBoolean(dr["IsActive"]);
                        motorInsurancePolicy.IsHIR = dr.IsNull("IsHIR") ? false : Convert.ToBoolean(dr["IsHIR"]);
                        motorInsurancePolicy.HIRStatus = dr.IsNull("HIRStatus") ? 0 : Convert.ToInt32(dr["HIRStatus"]);
                        motorInsurancePolicy.EndorsementCount = dr.IsNull("EndorsementCount") ? 0 : Convert.ToInt32(dr["EndorsementCount"]);
                        motorInsurancePolicy.IsCancelled = dr.IsNull("IsCancelled") ? false : Convert.ToBoolean(dr["IsCancelled"]);
                        motorInsurancePolicy.TaxOnPremium = dr.IsNull("TaxOnPremium") ? 0 : Convert.ToDecimal(dr["TaxOnPremium"]);
                        motorInsurancePolicy.TaxOnCommission = dr.IsNull("TaxOnCommission") ? 0 : Convert.ToDecimal(dr["TaxOnCommission"]);
                        motorInsurancePolicy.SeatingCapacity = dr.IsNull("SeatingCapacity") ? 0 : Convert.ToInt32(dr["SeatingCapacity"]);
                        motorInsurancePolicy.IsUnderBCFC = dr.IsNull("IsUnderBCFC") ? false : Convert.ToBoolean(dr["IsUnderBCFC"]);
                        motorInsurancePolicy.LoadAmount = dr.IsNull("LoadAmount") ? decimal.Zero : Convert.ToDecimal(dr["LoadAmount"]);
                        motorInsurancePolicy.RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"]);
                        motorInsurancePolicy.ClaimAmount = dr.IsNull("ClaimAmount") ? 0 : Convert.ToDecimal(dr["ClaimAmount"]);
                        motorInsurancePolicy.OldDocumentNumber = dr.IsNull("OldDocumentNumber") ? string.Empty : Convert.ToString(dr["OldDocumentNumber"]);
                        motorInsurancePolicy.RenewalDelayedDays = dr.IsNull("RenewalDelayedDays") ? 0 : Convert.ToInt32(dr["RenewalDelayedDays"]);
                        motorInsurancePolicy.ActualRenewalStartDate = dr.IsNull("ActualRenewalStartDate") ? (DateTime?)null : Convert.ToDateTime(dr["ActualRenewalStartDate"]);

                        //Get existing optional covers from the oracle table if it is there, need to show the renewal page.
                        if (motords != null && motords.Tables[3] != null && motords.Tables[3].Rows.Count > 0)
                        {
                            motorInsurancePolicy.OptionalCovers = new List<MotorCovers>();
                            for (int i = 0; i < motords.Tables[3].Rows.Count; i++)
                            {
                                motorInsurancePolicy.OptionalCovers.Add(
                                        new MotorCovers
                                        {
                                            CoverCode = motords.Tables[3].Rows[i].IsNull("CoverCode") ? ""
                                                       : motords.Tables[3].Rows[i]["CoverCode"].ToString(),
                                            CoverDescription = motords.Tables[3].Rows[i].IsNull("CoverCodeDescription") ? ""
                                                      : motords.Tables[3].Rows[i]["CoverCodeDescription"].ToString(),
                                            CoverAmount = motords.Tables[3].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                                      : Convert.ToDecimal(motords.Tables[3].Rows[i]["CoverAmount"].ToString()),
                                            AddedByEndorsement = false
                                        });
                            }
                        }
                    }
                    else
                    {
                        DataRow dr = motords.Tables[0].Rows[0];

                        motorInsurancePolicy.InsuredCode = dr.IsNull("INSUREDCODE") ? "" : Convert.ToString(dr["INSUREDCODE"]);
                        motorInsurancePolicy.InsuredName = dr.IsNull("INSUREDNAME") ? "" : Convert.ToString(dr["INSUREDNAME"]); 
                        motorInsurancePolicy.CPR = Convert.ToString(dr["CPR"]);
                        motorInsurancePolicy.ExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                        motorInsurancePolicy.YearOfMake = dr.IsNull("YEAR") ? 0 : Convert.ToInt32(dr["YEAR"]);
                        motorInsurancePolicy.VehicleMake = Convert.ToString(dr["MAKE"]);
                        motorInsurancePolicy.VehicleModel = Convert.ToString(dr["MODEL"]);
                        motorInsurancePolicy.vehicleTypeCode = Convert.ToString(dr["VEHICLETYPE"]);
                        motorInsurancePolicy.DOB = Convert.ToDateTime(dr["DateOfBirth"]);
                        motorInsurancePolicy.vehicleBodyType = dr.IsNull("BODYTYPE") ? "" : Convert.ToString(dr["BODYTYPE"]);
                        motorInsurancePolicy.VehicleValue = !dr.IsNull("VEHICLEVALUE") ? Convert.ToDecimal(dr["VEHICLEVALUE"]) : 0;
                        motorInsurancePolicy.PolicyCommencementDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                        motorInsurancePolicy.RegistrationNumber = Convert.ToString(dr["REGISTRATIONNO"]);
                        motorInsurancePolicy.ChassisNo = Convert.ToString(dr["CHASSISNO"]);
                        motorInsurancePolicy.EngineCC = Convert.ToInt32(dr["CAPACITY"]);
                        motorInsurancePolicy.FinancierCompanyCode = Convert.ToString(dr["FINANCECOMPANY"]);
                        motorInsurancePolicy.ExcessType = Convert.ToString(dr["EXCESSTYPE"]);
                        motorInsurancePolicy.ExcessAmount = dr.IsNull("EXCESSAMOUNT") ? 0 : Convert.ToDecimal(dr["EXCESSAMOUNT"]);
                        motorInsurancePolicy.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        motorInsurancePolicy.Mainclass = Convert.ToString(dr["MAINCLASS"]);
                        motorInsurancePolicy.Subclass = Convert.ToString(dr["SUBCLASS"]);
                        motorInsurancePolicy.OldDocumentNumber = string.Empty;
                        motorInsurancePolicy.RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"]);
                        motorInsurancePolicy.ClaimAmount = dr.IsNull("ClaimAmount") ? 0 : Convert.ToDecimal(dr["ClaimAmount"]);
                        ///No need below lines
                        //motorInsurancePolicy.RenewalDelayedDays = 0;
                        //motorInsurancePolicy.ActualRenewalStartDate = Convert.ToDateTime(dr["EXPIRYDATE"]);


                        //Get existing optional covers from the oracle table if it is there, need to show the renewal page.
                        if (motords != null && motords.Tables[1] != null && motords.Tables[1].Rows.Count > 0)
                        {
                            motorInsurancePolicy.OptionalCovers = new List<MotorCovers>();
                            for (int i = 0; i < motords.Tables[1].Rows.Count; i++)
                            {
                                motorInsurancePolicy.OptionalCovers.Add(
                                        new MotorCovers
                                        {
                                            CoverCode = motords.Tables[1].Rows[i].IsNull("CoverCode") ? ""
                                                       : motords.Tables[1].Rows[i]["CoverCode"].ToString(),
                                            CoverDescription = motords.Tables[1].Rows[i].IsNull("CoverCodeDescription") ? ""
                                                      : motords.Tables[1].Rows[i]["CoverCodeDescription"].ToString(),
                                            CoverAmount = motords.Tables[1].Rows[i].IsNull("CoverAmount") ? Convert.ToDecimal(motords.Tables[3].Rows[i].IsNull("Percent"))
                                                      : Convert.ToDecimal(motords.Tables[1].Rows[i]["CoverAmount"].ToString()),
                                            AddedByEndorsement = false
                                        });
                            }
                        }
                    }                  
                    return new MotorSavedQuotationResponse
                    {
                        IsTransactionDone = true,
                        MotorPolicyDetails = motorInsurancePolicy
                    };
                }               
                return new MotorSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = "This policy is not eligible for renewal !"
                };

            }
            catch (Exception ex)
            {
                return new MotorSavedQuotationResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }


        /// <summary>
        /// Get renewal motor policy details by the document number.
        /// Actually get it from the system table.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">Get the policy details for the endorsemnt page or policy buy page.</param>
        /// <param name="endorsementID">Endorsement id.</param>
        /// <returns></returns>
        public MotorSavedQuotationResponse GetRenewMotorPolicy(string documentNo, string agency, string agentCode, int renewalCount)

        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@DocumentNo",documentNo),
                    new SqlParameter("@Agency",agency ?? string.Empty),
                    new SqlParameter("@AgentCode",agentCode ?? string.Empty),
                    new SqlParameter("RenewalCount", renewalCount)

                };
                DataSet motords = BKICSQL.eds(StoredProcedures.MotorInsuranceSP.GetMotorRenewalPolicyByDocNo, paras);

                MotorInsurancePolicy motorInsurancePolicy = new MotorInsurancePolicy();
                if (motords != null && motords.Tables[0] != null && motords.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = motords.Tables[0].Rows[0];

                    motorInsurancePolicy.MotorID = Convert.ToInt64(dr["MOTORID"]);
                    motorInsurancePolicy.Agency = dr.IsNull("Agency") ? "" : Convert.ToString(dr["Agency"]);
                    motorInsurancePolicy.AgencyCode = dr.IsNull("AgentCode") ? "" : Convert.ToString(dr["AgentCode"]);
                    motorInsurancePolicy.InsuredCode = dr.IsNull("INSUREDCODE") ? "" : Convert.ToString(dr["INSUREDCODE"]);
                    motorInsurancePolicy.InsuredName = dr.IsNull("INSUREDNAME") ? "" : Convert.ToString(dr["INSUREDNAME"]);
                    motorInsurancePolicy.CPR = dr.IsNull("CPR") ? "" : Convert.ToString(dr["CPR"]);
                    motorInsurancePolicy.ExpiryDate = Convert.ToDateTime(dr["EXPIRYDATE"]);
                    motorInsurancePolicy.YearOfMake = dr.IsNull("YEAR") ? 0 : Convert.ToInt32(dr["YEAR"]);
                    motorInsurancePolicy.VehicleMake = Convert.ToString(dr["MAKE"]);
                    motorInsurancePolicy.VehicleModel = Convert.ToString(dr["MODEL"]);
                    motorInsurancePolicy.vehicleTypeCode = Convert.ToString(dr["VEHICLETYPE"]);
                    motorInsurancePolicy.Subclass = Convert.ToString(dr["SUBCLASS"]);
                    motorInsurancePolicy.DOB = Convert.ToDateTime(dr["DateOfBirth"]);
                    motorInsurancePolicy.vehicleBodyType = dr.IsNull("BODY") ? "" : Convert.ToString(dr["BODY"]);
                    motorInsurancePolicy.VehicleValue = !dr.IsNull("VEHICLEVALUE") ? Convert.ToDecimal(dr["VEHICLEVALUE"]) : 0;
                    motorInsurancePolicy.PolicyCommencementDate = Convert.ToDateTime(dr["COMMENCEDATE"]);
                    motorInsurancePolicy.RegistrationNumber = Convert.ToString(dr["REGISTRATIONNO"]);
                    motorInsurancePolicy.ChassisNo = Convert.ToString(dr["CHASSISNO"]);
                    motorInsurancePolicy.EngineCC = Convert.ToInt32(dr["TONNAGE"]);
                    motorInsurancePolicy.FinancierCompanyCode = Convert.ToString(dr["FINANCECOMPANY"]);
                    motorInsurancePolicy.PaymentType = dr.IsNull("PaymentType") ? string.Empty : Convert.ToString(dr["PaymentType"]);
                    motorInsurancePolicy.AccountNumber = dr.IsNull("AccountNo") ? string.Empty : Convert.ToString(dr["AccountNo"]);
                    motorInsurancePolicy.Remarks = dr.IsNull("Remark") ? string.Empty : Convert.ToString(dr["Remark"]);
                    motorInsurancePolicy.PremiumBeforeDiscount = dr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(dr["PremiumBeforeDiscount"]);
                    motorInsurancePolicy.PremiumAfterDiscount = dr.IsNull("PremiumAfterDiscount") ? 0 : Convert.ToDecimal(dr["PremiumAfterdiscount"]);
                    motorInsurancePolicy.CommisionBeforeDiscount = dr.IsNull("CommissionBeforeDiscount") ? 0 : Convert.ToDecimal(dr["CommissionBeforeDiscount"]);
                    motorInsurancePolicy.CommissionAfterDiscount = dr.IsNull("CommissionAfterDiscount") ? 0 : Convert.ToDecimal(dr["CommissionAfterDiscount"]);
                    motorInsurancePolicy.ExcessType = Convert.ToString(dr["EXCESSTYPE"]);
                    motorInsurancePolicy.ExcessAmount = dr.IsNull("EXCESSAMOUNT") ? 0 : Convert.ToDecimal(dr["EXCESSAMOUNT"]);
                    motorInsurancePolicy.Branch = Convert.ToString(dr["Source"]);
                    motorInsurancePolicy.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                    motorInsurancePolicy.Mainclass = Convert.ToString(dr["MAINCLASS"]);
                    motorInsurancePolicy.IsSaved = dr.IsNull("IsSaved") ? false : Convert.ToBoolean(dr["IsSaved"]);
                    motorInsurancePolicy.IsActivePolicy = dr.IsNull("IsActive") ? false : Convert.ToBoolean(dr["IsActive"]);
                    motorInsurancePolicy.IsHIR = dr.IsNull("IsHIR") ? false : Convert.ToBoolean(dr["IsHIR"]);
                    motorInsurancePolicy.HIRStatus = dr.IsNull("HIRStatus") ? 0 : Convert.ToInt32(dr["HIRStatus"]);
                    motorInsurancePolicy.EndorsementCount = dr.IsNull("EndorsementCount") ? 0 : Convert.ToInt32(dr["EndorsementCount"]);
                    motorInsurancePolicy.IsCancelled = dr.IsNull("IsCancelled") ? false : Convert.ToBoolean(dr["IsCancelled"]);
                    motorInsurancePolicy.TaxOnPremium = dr.IsNull("TaxOnPremium") ? 0 : Convert.ToDecimal(dr["TaxOnPremium"]);
                    motorInsurancePolicy.TaxOnCommission = dr.IsNull("TaxOnCommission") ? 0 : Convert.ToDecimal(dr["TaxOnCommission"]);
                    motorInsurancePolicy.SeatingCapacity = dr.IsNull("SeatingCapacity") ? 0 : Convert.ToInt32(dr["SeatingCapacity"]);
                    motorInsurancePolicy.IsUnderBCFC = dr.IsNull("IsUnderBCFC") ? false : Convert.ToBoolean(dr["IsUnderBCFC"]);
                    motorInsurancePolicy.LoadAmount = dr.IsNull("LoadAmount") ? decimal.Zero : Convert.ToDecimal(dr["LoadAmount"]);
                    motorInsurancePolicy.RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"]);
                    motorInsurancePolicy.ClaimAmount = dr.IsNull("ClaimAmount") ? 0 : Convert.ToDecimal(dr["ClaimAmount"]);
                    motorInsurancePolicy.OldDocumentNumber = dr.IsNull("OldDocumentNumber") ? string.Empty : Convert.ToString(dr["OldDocumentNumber"]);
                    motorInsurancePolicy.IsSavedRenewal = dr.IsNull("IsSavedRenewal") ? false : Convert.ToBoolean(dr["IsSavedRenewal"]);
                    motorInsurancePolicy.RenewalDelayedDays = dr.IsNull("RenewalDelayedDays") ? 0 : Convert.ToInt32(dr["RenewalDelayedDays"]);
                    motorInsurancePolicy.ActualRenewalStartDate = dr.IsNull("ActualRenewalStartDate") ? (DateTime?)null : Convert.ToDateTime(dr["ActualRenewalStartDate"]);

                }
                //Policy default cover.
                if (motords != null && motords.Tables.Count > 1 && motords.Tables[1] != null && motords.Tables[1].Rows.Count > 0)
                {
                    motorInsurancePolicy.Covers = new List<MotorCovers>();
                    for (int i = 0; i < motords.Tables[1].Rows.Count; i++)
                    {
                        motorInsurancePolicy.Covers.Add(
                                new MotorCovers
                                {
                                    CoverCode = motords.Tables[1].Rows[i].IsNull("CoverCode") ? ""
                                                 : motords.Tables[1].Rows[i]["CoverCode"].ToString(),
                                    CoverDescription = motords.Tables[1].Rows[i].IsNull("CoverCodeDescription") ? ""
                                                  : motords.Tables[1].Rows[i]["CoverCodeDescription"].ToString(),
                                    CoverAmount = motords.Tables[1].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                                 : Convert.ToDecimal(motords.Tables[1].Rows[i]["CoverAmount"].ToString()),
                                    AddedByEndorsement = motords.Tables[1].Rows[i].IsNull("AddedByEndorsement") ? false
                                               : Convert.ToBoolean(motords.Tables[1].Rows[i]["AddedByEndorsement"].ToString()),
                                });
                    }
                }
                //Product cover belonging to particular product.
                if (motords != null && motords.Tables.Count > 1 && motords.Tables[2] != null && motords.Tables[2].Rows.Count > 0)
                {
                    motorInsurancePolicy.ProductCovers = new List<MotorCovers>();
                    for (int i = 0; i < motords.Tables[2].Rows.Count; i++)
                    {
                        motorInsurancePolicy.ProductCovers.Add(
                                new MotorCovers
                                {
                                    CoverCode = motords.Tables[2].Rows[i].IsNull("CoverCode") ? ""
                                               : motords.Tables[2].Rows[i]["CoverCode"].ToString(),
                                    CoverDescription = motords.Tables[2].Rows[i].IsNull("CoverCodeDescription") ? ""
                                              : motords.Tables[2].Rows[i]["CoverCodeDescription"].ToString(),
                                    CoverAmount = motords.Tables[2].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                              : Convert.ToDecimal(motords.Tables[2].Rows[i]["CoverAmount"].ToString()),
                                    AddedByEndorsement = false
                                });
                    }
                }
                //Optional cover for the policy if it has.
                if (motords != null && motords.Tables.Count > 1 && motords.Tables[3] != null && motords.Tables[3].Rows.Count > 0)
                {
                    motorInsurancePolicy.OptionalCovers = new List<MotorCovers>();
                    for (int i = 0; i < motords.Tables[3].Rows.Count; i++)
                    {
                        motorInsurancePolicy.OptionalCovers.Add(
                                new MotorCovers
                                {
                                    CoverCode = motords.Tables[3].Rows[i].IsNull("CoverCode") ? ""
                                               : motords.Tables[3].Rows[i]["CoverCode"].ToString(),
                                    CoverDescription = motords.Tables[3].Rows[i].IsNull("CoverCodeDescription") ? ""
                                              : motords.Tables[3].Rows[i]["CoverCodeDescription"].ToString(),
                                    CoverAmount = motords.Tables[3].Rows[i].IsNull("CoverAmount") ? decimal.Zero
                                              : Convert.ToDecimal(motords.Tables[3].Rows[i]["CoverAmount"].ToString()),
                                    AddedByEndorsement = false
                                });
                    }                   

                }
                return new MotorSavedQuotationResponse
                {
                    IsTransactionDone = true,
                    MotorPolicyDetails = motorInsurancePolicy
                };                

            }
            catch (Exception ex)
            {
                return new MotorSavedQuotationResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Get all motor policies by agency for endorsement page, all policies should be ACTIVE.
        /// Used in every endorsement page search by policy number.
        /// </summary>
        /// <param name="req">Agency motor request.</param>
        /// <returns>List of motor policies for an endorsement.</returns>
        public AgencyMotorPolicyResponse GetMotorPoliciesEndorsement(AgencyMotorRequest req)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency",req.Agency??string.Empty),
                    new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch",req.AgentBranch??string.Empty),                   
                    new SqlParameter("@DocumentNo", req.DocumentNo ?? string.Empty)
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.MotorInsuranceSP.GetMotorPoliciesEndorsement, para);
                List<AgencyMotorPolicy> agencyMotorPolicies = new List<AgencyMotorPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyMotorPolicy();
                        res.MotorID = Convert.ToInt64(dr["MOTORID"]);
                        res.DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]);
                        res.RenewalCount = dr.IsNull("RENEWALCOUNT") ? 0 : Convert.ToInt32(dr["RENEWALCOUNT"]);
                        res.DocumentRenewalNo = res.RenewalCount + "-" + res.DocumentNo;
                        agencyMotorPolicies.Add(res);
                    }
                }
                return new AgencyMotorPolicyResponse
                {
                    IsTransactionDone = true,
                    AgencyMotorPolicies = agencyMotorPolicies
                };
            }
            catch (Exception ex)
            {
                return new AgencyMotorPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }
        
        /// <summary>
        /// Get all eligible oracle motor policies for renewal by agency.
        /// </summary>
        /// <param name="req">Agency motor request.</param>
        /// <returns>List of renewalble motor policies by agency.</returns>
        public AgencyMotorPolicyResponse GetOracleMotorRenewalPolicies(AgencyMotorRequest req)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency",req.Agency??string.Empty),
                    new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch",req.AgentBranch??string.Empty),
                    new SqlParameter("@IncludeHIR", req.IncludeHIR),
                    new SqlParameter("@IsRenewal", req.IsRenewal),
                    new SqlParameter("@DocumentNo", req.DocumentNo ?? string.Empty)
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.MotorInsuranceSP.GetOracleMotorRenewalPolicies, para);
                List<AgencyMotorPolicy> agencyMotorPolicies = new List<AgencyMotorPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyMotorPolicy();
                       // res.MotorID = Convert.ToInt64(dr["MOTORID"]);
                        res.DocumentNo = dr.IsNull("DOCUMENTNO") ? string.Empty : Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = dr.IsNull("INSUREDCODE") ? string.Empty : Convert.ToString(dr["INSUREDCODE"]);
                        res.RenewalCount = dr.IsNull("RENEWALCOUNT") ? 0 : Convert.ToInt32(dr["RENEWALCOUNT"]);
                        res.DocumentRenewalNo = res.RenewalCount + "-" + res.DocumentNo;
                        agencyMotorPolicies.Add(res);
                    }
                }
                return new AgencyMotorPolicyResponse
                {
                    IsTransactionDone = true,
                    AgencyMotorPolicies = agencyMotorPolicies
                };
            }
            catch (Exception ex)
            {
                return new AgencyMotorPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Insert the motor insurance policy.
        /// </summary>
        /// <param name="policy">Motor policy details.</param>
        /// <returns>MotorID and Document Number.</returns>
        public MotorInsurancePolicyResponse PostMotorInsurance(MotorInsurancePolicy policy)
        {
            try
            {
                MotorCalculator mc = new MotorCalculator();
                return mc.InsertMotor(policy);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            

            //try
            //{
            //    DataTable optionalCovers = new DataTable();
            //    optionalCovers.Columns.Add("CoverCode", typeof(string));
            //    optionalCovers.Columns.Add("CoverDescription", typeof(string));
            //    optionalCovers.Columns.Add("CoverAmount", typeof(decimal));
            //    optionalCovers.Columns.Add("IsOptionalCover", typeof(bool));

            //    if (policy.OptionalCovers != null && policy.OptionalCovers.Count > 0)
            //    {
            //        foreach (var cover in policy.OptionalCovers)
            //        {
            //            optionalCovers.Rows.Add(cover.CoverCode, cover.CoverDescription,
            //                cover.CoverAmount, 1);
            //        }
            //    }

            //    SqlParameter[] paras = new SqlParameter[]
            //    {
            //        new SqlParameter("@MotorID", policy.MotorID),
            //        new  SqlParameter("@InsuredCode",policy.InsuredCode),
            //        new  SqlParameter("@DOB",policy.DOB),
            //        new  SqlParameter("@YearOfMake",policy.YearOfMake),
            //        new  SqlParameter("@VehicleMake",policy.VehicleMake),
            //        new  SqlParameter("@VehicleModel",policy.VehicleModel),
            //        new  SqlParameter("@vehicleTypeCode",policy.vehicleTypeCode),
            //        new  SqlParameter("@vehicleBodyType",policy.vehicleBodyType),
            //        //new  SqlParameter("@PolicyCode",policy.PolicyCode),
            //        new  SqlParameter("@IsNCB",policy.IsNCB),
            //        new  SqlParameter("@NCBFromDate",policy.NCBStartDate.HasValue?policy.NCBStartDate.Value:(object)DBNull.Value),
            //        new  SqlParameter("@NCBToDate",policy.NCBEndDate.HasValue?policy.NCBEndDate.Value:(object)DBNull.Value),
            //        new  SqlParameter("@VehicleSumInsured",policy.VehicleValue),
            //        new  SqlParameter("@PremiumAmount",policy.PremiumAmount),
            //        new  SqlParameter("@PolicyCommenceDate",policy.PolicyCommencementDate),
            //        new  SqlParameter("@PolicyEndDate",policy.PolicyEndDate),
            //        new  SqlParameter("@RegistrationNumber",policy.RegistrationNumber ?? ""),
            //        new  SqlParameter("@ChassisNo",policy.ChassisNo),
            //        new  SqlParameter("@EngineCC",policy.EngineCC),
            //        new  SqlParameter("@FinancierCompanyCode",!string.IsNullOrEmpty(policy.FinancierCompanyCode)?policy.FinancierCompanyCode:""),
            //        new  SqlParameter("@ExcessType",!string.IsNullOrEmpty(policy.ExcessType)?policy.ExcessType:""),
            //        new SqlParameter("@dt", optionalCovers),
            //        new SqlParameter("OptionalCoverAmount", policy.OptionalCoverAmount),
            //        new SqlParameter("@IsUnderBCFC", policy.IsUnderBCFC),
            //        new SqlParameter("@SeatingCapacity", policy.SeatingCapacity),
            //        new  SqlParameter("@Createdby",policy.CreatedBy),
            //        new SqlParameter("@AuthorizedBy", policy.AuthorizedBy),
            //        new SqlParameter("@IsSaved",policy.IsSaved),
            //        new SqlParameter("@IsActive",policy.IsActivePolicy),
            //        new SqlParameter("@PaymentAuthorization",policy.PaymentAuthorizationCode??""),
            //        new SqlParameter("@TransactionNo",policy.TransactionNo??""),
            //        new SqlParameter("@PaymentType",policy.PaymentType??""),
            //        new SqlParameter("@AccountNumber",policy.AccountNumber ?? ""),
            //        new SqlParameter("@Remarks",policy.Remarks ??""),
            //        new SqlParameter("@MainClass",policy.Mainclass??""),
            //        new SqlParameter("@SubClass",policy.Subclass??""),
            //        new SqlParameter("@Agency",policy.Agency),
            //        new SqlParameter("@AgentCode",policy.AgencyCode),
            //        new SqlParameter("@AgentBranch", policy.AgentBranch),
            //        new SqlParameter("@PremiumAfterDiscountAmount",policy.PremiumAfterDiscount),
            //        new SqlParameter("@CommisionAfterDiscountAmount",policy.CommissionAfterDiscount),
            //        new SqlParameter("@UserChangedPremium",policy.UserChangedPremium),
            //        new SqlParameter("@LoadAmount", policy.LoadAmount),
            //        new SqlParameter("@ClaimAmount", policy.ClaimAmount)
            //    };
            //    List<SPOut> outParams = new List<SPOut>()
            //    {
            //        new SPOut() { OutPutType = SqlDbType.BigInt, ParameterName= "@NewMotorID"},
            //        new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsHIR" },
            //        new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@DocumentNumber", Size=50}
            //    };
            //    object[] dataSet = BKICSQL.GetValues(MotorInsuranceSP.PostPolicy, paras, outParams);

            //    var MotorId = Convert.ToInt32(dataSet[0] != null ? dataSet[0] : 0);
            //    var IsHIR = Convert.ToBoolean(dataSet[1]);
            //    var DocumentNo = Convert.ToString(dataSet[2]);

            //    if (!IsHIR && policy.IsActivePolicy)
            //    {
            //        Task moveToOracleTask = Task.Factory.StartNew(() =>
            //                 {
            //                     OracleDBIntegration.DBObjects.TransactionWrapper oracleResult
            //                       = _oracleMotorInsurance.IntegrateMotorToOracle((int)MotorId);
            //                 });

            //        try
            //        {
            //            moveToOracleTask.Wait();
            //        }
            //        catch (AggregateException ex)
            //        {
            //            foreach (Exception inner in ex.InnerExceptions)
            //            {
            //                _mail.SendMailLogError(ex.Message, policy.InsuredCode, "MotorInsurance", policy.Agency, true);
            //            }
            //        }
            //    }
            //    return new MotorInsurancePolicyResponse()
            //    {
            //        IsTransactionDone = true,
            //        IsHIR = IsHIR,
            //        MotorID = MotorId,
            //        DocumentNo = DocumentNo
            //    };
            //}
            //catch (Exception ex)
            //{
            //    _mail.SendMailLogError(ex.Message, policy.InsuredCode, "MotorInsurance", policy.Agency, false);
            //    return new MotorInsurancePolicyResponse()
            //    {
            //        IsTransactionDone = false,
            //        TransactionErrorMessage = ex.ToString()
            //    };
            //};
        }
        
    }
}