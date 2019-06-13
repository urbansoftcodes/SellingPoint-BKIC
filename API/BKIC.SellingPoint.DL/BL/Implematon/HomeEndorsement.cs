using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OracleDBIntegration = SellingPoint.OracleDBIntegration;
using BKIC.SellingPoint.DL.StoredProcedures;
using System.Globalization;
using BKIC.SellingPoint.DL.Constants;
using BKIC.SellingPoint.DL.BL.Implementation;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    /// <summary>
    /// Home endorsement methods. 
    /// </summary>
    public class HomeEndorsement : IHomeEndorsement
    {

        public readonly OracleDBIntegration.Implementation.HomeEndorsement _oracleHomeEndorsement;
        public readonly IMail _mail;
        public readonly IAdmin _adminRepository;

        public HomeEndorsement()
        {
            _oracleHomeEndorsement = new OracleDBIntegration.Implementation.HomeEndorsement();
            _mail = new Mail();
            _adminRepository = new Admin();
        }

        /// <summary>
        /// Home endorsement operation - authorize or delete.
        /// </summary>
        /// <param name="request">Endorsement operation request.</param>
        /// <returns></returns>
        public HomeEndorsementOperationResponse EndorsementOperation(HomeEndorsementOperation request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                     new  SqlParameter("@Type", request.Type),
                     new  SqlParameter("@HomeEndorsementID", request.HomeEndorsementID),
                     new  SqlParameter("@HomeID", request.HomeID),
                     new  SqlParameter("@Agency", request.Agency),
                     new  SqlParameter("@AgentCode", request.AgentCode),
                     new  SqlParameter("@UpdatedBy", request.UpdatedBy),
                };
                DataTable dt = BKICSQL.edt(HomeEndorsementSP.HomeEndorsementOperation, paras);
                if (request.Type == Constants.EndorsementOpeationType.Authorize)
                {                    
                    try
                    {
                        new Task(() =>
                        {
                            OracleDBIntegration.DBObjects.TransactionWrapper oracleResult
                                    = _oracleHomeEndorsement.MoveIntegrationToOracle(request.HomeEndorsementID);
                        }).Start();
                    }
                    catch (AggregateException ex)
                    {
                        foreach (Exception inner in ex.InnerExceptions)
                        {
                            _mail.SendMailLogError(inner.Message, "", "Homeendorsement", "", true);
                        }
                    }

                }
                return new HomeEndorsementOperationResponse
                {
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new HomeEndorsementOperationResponse()
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
        public HomeEndorsementPreCheckResponse EndorsementPrecheck(HomeEndorsementPreCheckRequest request)
        {

            try
            {
                var alreadyHave = false;
                SqlParameter[] paras = new SqlParameter[]
                {
                     new  SqlParameter("@docNo", request.DocNo),
                     new SqlParameter("@type", Constants.Insurance.Home)
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsAlreadyHaveEndorsement" , Precision = 38, Scale =3},
                };
                object[] dataSet = BKICSQL.GetValues(AdminSP.EndorsementPreCheck, paras, outParams);
                if (dataSet != null && dataSet[0] != null)
                {
                    alreadyHave = string.IsNullOrEmpty(dataSet[0].ToString()) ? false : Convert.ToBoolean(dataSet[0].ToString());
                }
                return new HomeEndorsementPreCheckResponse()
                {
                    IsTransactionDone = true,
                    IsAlreadyHave = alreadyHave,
                    EndorsementNo = ""
                };
            }
            catch (Exception ex)
            {
                return new HomeEndorsementPreCheckResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all the home endorsement details for the specific policy.
        /// To show the top of the page on any home endorsement page.
        /// </summary>
        /// <param name="request">Endorsement request.</param>
        /// <returns>list of endorsemnt details.</returns>
        public HomeEndoResponse GetAllEndorsements(HomeEndoRequest request)
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
                DataSet homeEndo = BKICSQL.eds(StoredProcedures.PortalSP.GetEndorsementByDocNo, paras);
                List<BO.HomeEndorsement> homeEndorsements = new List<BO.HomeEndorsement>();

                if (homeEndo != null && homeEndo.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in homeEndo.Tables[0].Rows)
                    {
                        BO.HomeEndorsement result = new BO.HomeEndorsement();

                        result.HomeEndorsementID = dr.IsNull("HomeEndorsementID") ? 0 : Convert.ToInt64(dr["HomeEndorsementID"]);
                        result.HomeID = dr.IsNull("HomeID") ? 0 : Convert.ToInt64(dr["HomeID"]);
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
                        result.TaxOnPremium = dr.IsNull("TaxOnPremium") ? 0 : Convert.ToDecimal(dr["TaxOnPremium"]);
                        result.RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"]);

                        homeEndorsements.Add(result);
                    }
                }
                return new HomeEndoResponse
                {
                    HomeEndorsements = homeEndorsements,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new HomeEndoResponse
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
        public HomeEndorsementQuoteResponse GetHomeEndorsementQuote(HomeEndorsementQuote homeEndorsement)
        {
            try
            {
                if (homeEndorsement.EndorsementType == MotorEndorsementTypes.ChangeSumInsured)
                {
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new  SqlParameter("@DocumentNumber",homeEndorsement.DocumentNumber),
                        new  SqlParameter("@Agency",homeEndorsement.Agency),
                        new  SqlParameter("@AgentCode",homeEndorsement.AgentCode),
                        new  SqlParameter("@MainClass",homeEndorsement.MainClass ?? string.Empty),
                        new  SqlParameter("@SubClass",homeEndorsement.SubClass ?? string.Empty),
                        new  SqlParameter("@EffectiveFromDate",homeEndorsement.EffectiveFromDate),
                        new  SqlParameter("@EffectiveToDate",homeEndorsement.EffectiveToDate),
                        new  SqlParameter("@CancelationDate",homeEndorsement.CancelationDate),
                        new  SqlParameter("@PaidPremium",homeEndorsement.PaidPremium),
                        new SqlParameter("@NewSumInsured", homeEndorsement.NewSumInsured),
                        new SqlParameter("@RefundType", homeEndorsement.RefundType ?? ""),
                        new  SqlParameter("@EndorsementType",homeEndorsement.EndorsementType ?? ""),
                    };
                    List<SPOut> outParams = new List<SPOut>()
                    {
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@EndorsementPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@SRCCPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@BasicPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@CommissionAmount" , Precision = 38, Scale =3}
                    };
                    object[] dataSet = BKICSQL.GetValues(HomeEndorsementSP.GetQuote, paras, outParams);
                    var endorsementPremium = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : decimal.Parse(dataSet[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    var TotalCommission = string.IsNullOrEmpty(dataSet[3].ToString()) ? 0 : decimal.Parse(dataSet[3].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                    return new HomeEndorsementQuoteResponse()
                    {
                        IsTransactionDone = true,
                        EndorsementPremium = endorsementPremium,
                        Commission = TotalCommission
                    };
                }
                else
                {
                    SqlParameter[] paras = new SqlParameter[]
                    {
                        new  SqlParameter("@DocumentNumber",homeEndorsement.DocumentNumber),
                        new  SqlParameter("@Agency",homeEndorsement.Agency),
                        new  SqlParameter("@AgentCode",homeEndorsement.AgentCode),
                        new  SqlParameter("@MainClass",homeEndorsement.MainClass ?? string.Empty),
                        new  SqlParameter("@SubClass",homeEndorsement.SubClass ?? string.Empty),
                        new  SqlParameter("@EffectiveFromDate",homeEndorsement.EffectiveFromDate),
                        new  SqlParameter("@EffectiveToDate",homeEndorsement.EffectiveToDate),
                        new  SqlParameter("@CancelationDate",homeEndorsement.CancelationDate),
                        new  SqlParameter("@PaidPremium",homeEndorsement.PaidPremium),
                        new SqlParameter("@BuildingSumInsured", homeEndorsement.BuildingSumInsured),
                        new SqlParameter("@ContentSumInsured", homeEndorsement.ContentSumInsured),
                        new SqlParameter("@NoOfDomesticHelp", homeEndorsement.NoOfDomesticHelp),
                        new SqlParameter("@JewelleryCoverType", homeEndorsement.jewelleryCoverType),
                        new SqlParameter("@NewSumInsured", homeEndorsement.NewSumInsured),
                        new SqlParameter("@RefundType", homeEndorsement.RefundType ?? ""),
                        new  SqlParameter("@EndorsementType",homeEndorsement.EndorsementType ?? ""),
                     };
                    List<SPOut> outParams = new List<SPOut>()
                    {
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@EndorsementPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@TotalPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@SRCCPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@BasicPremium" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@Commission" , Precision = 38, Scale =3},
                        new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@RefundVat" , Precision = 38, Scale =3}
                    };

                    object[] dataSet = BKICSQL.GetValues(HomeEndorsementSP.GetHomeCancelQuote, paras, outParams);
                    var endorsementPremium = string.IsNullOrEmpty(dataSet[1].ToString()) ? 0 : decimal.Parse(dataSet[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    var TotalCommission = string.IsNullOrEmpty(dataSet[4].ToString()) ? 0 : decimal.Parse(dataSet[4].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    var RefundVat = string.IsNullOrEmpty(dataSet[5].ToString()) ? 0 : decimal.Parse(dataSet[5].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                    return new HomeEndorsementQuoteResponse()
                    {
                        IsTransactionDone = true,
                        EndorsementPremium = endorsementPremium,
                        Commission = TotalCommission,
                        RefundVat = RefundVat
                    };
                }
            }
            catch (Exception ex)
            {
                return new HomeEndorsementQuoteResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
        /// <summary>
        /// Home endorsement to add the domestic members.
        /// if we add new domestic member calculate premium for that memmber.
        /// </summary>
        /// <param name="homeEndorsement">Endorsement request</param>
        /// <returns>Returns endorsement premium and commision.</returns>
        public HomeEndorsementQuoteResponse GetHomeDomesticHelpQuote(HomeEndorsementDomesticHelpQuote homeEndorsement)
        {
            try
            {
                DataTable domestichelp = new DataTable();
                domestichelp.Columns.Add("HOMEID", typeof(Int32));
                domestichelp.Columns.Add("LINKID", typeof(string));
                domestichelp.Columns.Add("DOCUMENTNO", typeof(string));
                domestichelp.Columns.Add("LINENO", typeof(Int32));
                domestichelp.Columns.Add("SERIALNO", typeof(Int32));
                domestichelp.Columns.Add("ITEMSERIALNO", typeof(Int32));
                domestichelp.Columns.Add("ITEMCODE", typeof(string));
                domestichelp.Columns.Add("ITEMNAME", typeof(string));
                domestichelp.Columns.Add("MEMBERSERIALNO", typeof(Int32));
                domestichelp.Columns.Add("NAME", typeof(string));
                domestichelp.Columns.Add("CPRNUMBER", typeof(string));
                domestichelp.Columns.Add("TITLE", typeof(string));
                domestichelp.Columns.Add("SEX", typeof(char));
                domestichelp.Columns.Add("AGE", typeof(Int32));
                domestichelp.Columns.Add("DATEOFBIRTH", typeof(DateTime));
                domestichelp.Columns.Add("SUMINSURED", typeof(decimal));
                domestichelp.Columns.Add("PREMIUMAMOUNT", typeof(decimal));
                domestichelp.Columns.Add("CREATEDBY", typeof(Int32));
                domestichelp.Columns.Add("CREATEDDATE", typeof(DateTime));
                domestichelp.Columns.Add("UPDATEDBY", typeof(Int32));
                domestichelp.Columns.Add("UPDATEDDATE", typeof(DateTime));
                domestichelp.Columns.Add("OCCUPATION", typeof(string));
                domestichelp.Columns.Add("NATIONALITY", typeof(string));

                foreach (var members in homeEndorsement.Domestichelp)
                {
                    domestichelp.Rows.Add(0, "", "", 0, 0, 0, "", "",
                                         members.MemberSerialNo, members.Name,
                                         members.CPR, members.Title, members.Sex,
                                         members.Age, members.DOB, members.SumInsured,
                                         members.PremiumAmount, 0, null,
                                         0, null, members.Occupation, members.Nationality);
                }
                SqlParameter[] paras = new SqlParameter[]
                {
                    new  SqlParameter("@DocumentNo",homeEndorsement.DocumentNumber),
                    new SqlParameter("@RenewalCount", homeEndorsement.RenewalCount),
                    new  SqlParameter("@dt",domestichelp)
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@premium" , Precision = 38, Scale =3}
                };
                object[] dataSet = BKICSQL.GetValues(HomeEndorsementSP.GetHomeDomesticHelpQuote, paras, outParams);
                var endorsementPremium = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : decimal.Parse(dataSet[0].ToString(),
                                         CultureInfo.InvariantCulture.NumberFormat);
                return new HomeEndorsementQuoteResponse()
                {
                    IsTransactionDone = true,
                    EndorsementPremium = endorsementPremium
                };
            }
            catch (Exception ex)
            {
                return new HomeEndorsementQuoteResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Insert the home endorsement.
        /// </summary>
        /// <param name="homeEndorsement">Home endorsement details.</param>
        /// <returns>Homeendorsementid, Homeendorementnumber.</returns>
        public HomeEndorsementResponse PostHomeEndorsement(BO.HomeEndorsement homeEndorsement)
        {
            try
            {
                var req = new BO.HomeProductRequest
                {
                    Type = "fetch",
                    Agency = homeEndorsement.Agency,
                    AgentCode = homeEndorsement.AgencyCode,
                    MainClass = homeEndorsement.Mainclass,
                    SubClass = homeEndorsement.Subclass
                };
                BO.HomeProductResponse productRes = _adminRepository.GetHomeProduct(req);
                HomeEndorsementResponse result = InsertHomeEndorsement(homeEndorsement);
                var homeProduct = productRes.HomeProducts[0];

                if (result.IsTransactionDone)
                {

                    if (productRes != null && productRes.IsTransactionDone && productRes.HomeProducts.Count > 0)
                    {
                        decimal BasicPremium = 0;
                        decimal SRCCPremium = 0;
                        decimal DateDiffernce = 0;
                        decimal TotalPremium = 0;
                        DateTime CurrentDate = DateTime.Now;
                        if(homeEndorsement.EndorsementType == "ChangeSumInsured")
                        {
                            DateDiffernce = (decimal)(homeEndorsement.ExpiryDate.Date - CurrentDate.Date).TotalDays;
                            BasicPremium = ((homeEndorsement.NewSumInsured * homeProduct.Rate) / 100) * (DateDiffernce / 365);
                            if (!string.IsNullOrEmpty(homeEndorsement.IsRiotStrikeDamage) && homeEndorsement.IsRiotStrikeDamage.ToString().ToUpper() == "Y")
                            {
                                SRCCPremium = ((homeEndorsement.NewSumInsured * homeProduct.RiotCoverRate) / 100) * (DateDiffernce / 365);
                            }
                           TotalPremium  = BasicPremium + SRCCPremium;
                        }
                        else if(homeEndorsement.EndorsementType == "AddRemoveDomesticHelp")
                        {
                            BasicPremium = homeEndorsement.PremiumBeforeDiscount;
                            TotalPremium = BasicPremium + SRCCPremium;
                        } 
                        var hasCommission = homeProduct.HomeEndorsementMaster.Find(c => c.EndorsementType == homeEndorsement.EndorsementType);
                        if (hasCommission != null && hasCommission.HasCommission)
                        {
                            CalculateCommission(homeProduct, homeEndorsement, homeEndorsement.HomeID, result.DocumentNo,
                                            result.LinkID, homeEndorsement.RenewalCount, result.EndorsementCount,
                                            result.EndorsementNo, result.HomeEndorsementID, BasicPremium, SRCCPremium, TotalPremium);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _mail.SendMailLogError(ex.Message, homeEndorsement.InsuredCode, "HomeEndorsement", homeEndorsement.Agency, false);
                return new HomeEndorsementResponse() { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        public void CalculateCommission(BO.HomeProduct homeProduct, BO.HomeEndorsement endorsement,
            long homeID, string documentNo, string LinkID, int renewalCount, int endorsementCount, string endorsementNumber,
            long endorsementID, decimal basicPremium, decimal srccPremium, decimal totalPremium)
        {
            try
            {
                int lineNo = 0;
                if (homeProduct.Category != null && homeProduct.Category.Count > 0)
                {
                    List<PolicyCategory> policyCategories = new List<PolicyCategory>();
                    foreach (var dr in homeProduct.Category)
                    {
                        lineNo++;
                        if (!endorsement.UserChangedPremium)
                        {
                            if (dr.ValueType == "Percent" && dr.Code == "BASICCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = basicPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = basicPremium * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = basicPremium;
                                policyCategory.PremiumBeforeDiscount = basicPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(basicPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(basicPremium, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;
                                policyCategory.HomeEndorsementID = endorsementID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && dr.Code == "SRCCCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = srccPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = srccPremium * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = srccPremium;
                                policyCategory.PremiumBeforeDiscount = srccPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(srccPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(srccPremium, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;
                                policyCategory.HomeEndorsementID = endorsementID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && dr.Code == "AGTCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = totalPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = totalPremium * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = totalPremium;
                                policyCategory.PremiumBeforeDiscount = totalPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(totalPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(totalPremium, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;
                                policyCategory.HomeEndorsementID = endorsementID;

                                policyCategories.Add(policyCategory);

                            }
                        }
                        else
                        {
                            if (dr.ValueType == "Percent" && dr.Code == "BASICCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = basicPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = endorsement.CommissionAfterDiscount;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = endorsement.PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = basicPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(basicPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(endorsement.PremiumAfterDiscount, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;
                                policyCategory.HomeEndorsementID = endorsementID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && dr.Code == "SRCCCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = srccPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = endorsement.IsRiotStrikeDamage.ToString().ToLower() == "y" ? endorsement.CommissionAfterDiscount : 0;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = endorsement.IsRiotStrikeDamage.ToString().ToLower() == "y" ? endorsement.PremiumAfterDiscount : 0;
                                policyCategory.PremiumBeforeDiscount = srccPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(srccPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(srccPremium, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;
                                policyCategory.HomeEndorsementID = endorsementID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && dr.Code == "AGTCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = endorsement.PremiumBeforeDiscount * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = endorsement.PremiumAfterDiscount * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = endorsementCount;
                                policyCategory.EndorsementNo = endorsementNumber;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = endorsement.PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = endorsement.PremiumBeforeDiscount;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(endorsement.PremiumBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(endorsement.PremiumAfterDiscount, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;
                                policyCategory.HomeEndorsementID = endorsementID;

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
                                    pc.TaxOnCommissionAfterDiscount = GetTax(pc.CommissionBeforeDiscount, homeProduct.TaxRate);
                                    commissionDiscount = commissionDiscount - pc.CommissionBeforeDiscount;
                                }
                                else
                                {
                                    pc.CommissionAfterDiscount = commissionDiscount;
                                    pc.TaxOnCommissionAfterDiscount = GetTax(commissionDiscount, homeProduct.TaxRate);
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

        private void InsertCategory(BO.HomeEndorsement policy, List<PolicyCategory> policyCategories)
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
                                new SqlParameter("@HomeID", dr.HomeID),
                                new SqlParameter("@MotorID", DBNull.Value),
                                new SqlParameter("@MotorEndorsementID", DBNull.Value),
                                new SqlParameter("@TravelEndorsementID", DBNull.Value),
                                new SqlParameter("@HomeEndorsementID", dr.HomeEndorsementID),

                     };
                    BKICSQL.edt(MotorInsuranceSP.PolicyCategoryInsert, paras);
                }
            }
        }

        private HomeEndorsementResponse InsertHomeEndorsement(BO.HomeEndorsement homeEndorsement)
        {


            DataTable domestichelp = new DataTable();
            domestichelp.Columns.Add("HOMEID", typeof(Int32));
            domestichelp.Columns.Add("LINKID", typeof(string));
            domestichelp.Columns.Add("DOCUMENTNO", typeof(string));
            domestichelp.Columns.Add("LINENO", typeof(Int32));
            domestichelp.Columns.Add("SERIALNO", typeof(Int32));
            domestichelp.Columns.Add("ITEMSERIALNO", typeof(Int32));
            domestichelp.Columns.Add("ITEMCODE", typeof(string));
            domestichelp.Columns.Add("ITEMNAME", typeof(string));
            domestichelp.Columns.Add("MEMBERSERIALNO", typeof(Int32));
            domestichelp.Columns.Add("NAME", typeof(string));
            domestichelp.Columns.Add("CPRNUMBER", typeof(string));
            domestichelp.Columns.Add("TITLE", typeof(string));
            domestichelp.Columns.Add("SEX", typeof(char));
            domestichelp.Columns.Add("AGE", typeof(Int32));
            domestichelp.Columns.Add("DATEOFBIRTH", typeof(DateTime));
            domestichelp.Columns.Add("SUMINSURED", typeof(decimal));
            domestichelp.Columns.Add("PREMIUMAMOUNT", typeof(decimal));
            domestichelp.Columns.Add("CREATEDBY", typeof(Int32));
            domestichelp.Columns.Add("CREATEDDATE", typeof(DateTime));
            domestichelp.Columns.Add("UPDATEDBY", typeof(Int32));
            domestichelp.Columns.Add("UPDATEDDATE", typeof(DateTime));
            domestichelp.Columns.Add("OCCUPATION", typeof(string));
            domestichelp.Columns.Add("NATIONALITY", typeof(string));

            foreach (var members in homeEndorsement.HomeDomesticHelp)
            {
                domestichelp.Rows.Add(0, "", "", 0, 0, 0, "", "", members.MemberSerialNo, members.Name, members.CPR, members.Title, members.Sex,
                    members.Age, members.DOB, members.SumInsured, members.PremiumAmount, homeEndorsement.CreatedBy, null,
                    homeEndorsement.CreatedBy, null, members.Occupation, members.Nationality);
            }
            SqlParameter[] paras = new SqlParameter[]
            {
                    new  SqlParameter("@HomeID", homeEndorsement.HomeID),
                    new  SqlParameter("@HomeendorsementID",homeEndorsement.HomeEndorsementID),
                    new  SqlParameter("@EndorsementType",homeEndorsement.EndorsementType ?? ""),
                    new  SqlParameter("@Agency",homeEndorsement.Agency),
                    new  SqlParameter("@AgentCode",homeEndorsement.AgencyCode),
                    new  SqlParameter("@BranchCode",homeEndorsement.AgentBranch ?? ""),
                    new SqlParameter("@CreatedBy" , homeEndorsement.CreatedBy),
                    new  SqlParameter("@DocumentNo",homeEndorsement.DocumentNo ?? ""),
                    new  SqlParameter("@InsuredCode",homeEndorsement.InsuredCode ?? ""),
                    new  SqlParameter("@InsuredName",homeEndorsement.InsuredName ?? ""),
                    new  SqlParameter("@Premium",homeEndorsement.PremiumAmount),
                    new  SqlParameter("@FinanceCompany",homeEndorsement.FinancierCompanyCode ?? ""),
                    new  SqlParameter("@MainClass",homeEndorsement.Mainclass ?? "PPTY"),
                    new  SqlParameter("@SubClass",homeEndorsement.Subclass ?? "SH"),
                    new  SqlParameter("@CommencementDate",homeEndorsement.PolicyCommencementDate),
                    new  SqlParameter("@ExpireDate",homeEndorsement.ExpiryDate),
                    new  SqlParameter("@CancelDate",homeEndorsement.CancelDate.HasValue ? homeEndorsement.CancelDate.Value : (object)DBNull.Value),
                    //new  SqlParameter("@CancelDate", (object)DBNull.Value),
                    new  SqlParameter("@PaymentDate",homeEndorsement.PaymentDate.HasValue ? homeEndorsement.PaymentDate.Value : (object)DBNull.Value),
                    new  SqlParameter("@PaymentType",homeEndorsement.PaymentType ?? ""),
                    new  SqlParameter("@AccountNumber",homeEndorsement.AccountNumber ?? ""),
                    new  SqlParameter("@Remarks",homeEndorsement.Remarks ?? ""),
                    new  SqlParameter("@Source",homeEndorsement.Source ?? ""),
                    new  SqlParameter("@IsSaved",homeEndorsement.IsSaved),
                    new  SqlParameter("@IsActive",homeEndorsement.IsActivePolicy),
                    new  SqlParameter("@Type",""),
                    new  SqlParameter("@BuildingNo", homeEndorsement.BuildingNo ?? ""),
                    new  SqlParameter("@FlatNo", homeEndorsement.FlatNo ?? ""),
                    new  SqlParameter("@HouseNo", homeEndorsement.HouseNo ?? ""),
                    new  SqlParameter("@NoOfFloors", homeEndorsement.NoOfFloors),
                    new  SqlParameter("@Area", homeEndorsement.Area ?? ""),
                    new  SqlParameter("@BuildingType", homeEndorsement.BuildingType),
                    new  SqlParameter("@RoadNo",homeEndorsement.RoadNo ?? ""),
                    new  SqlParameter("@BlockNo", homeEndorsement.BlockNo ?? ""),
                    new  SqlParameter("@ResidanceTypeCode", homeEndorsement.BuildingType == 1 ? "H" : "F"),
                    new  SqlParameter("@BuildingAge",homeEndorsement.BuildingAge),
                    new  SqlParameter("@SumInsured",homeEndorsement.NewSumInsured),
                    new  SqlParameter("@BuildingSumInsured",homeEndorsement.BuildingSumInsured),
                    new  SqlParameter("@ContentSumInsured",homeEndorsement.ContentSumInsured),
                    new SqlParameter("@SumInsuredType", homeEndorsement.SumInsuredType ?? ""),
                    new SqlParameter("@dt", domestichelp),
                    new SqlParameter("@RefundType", homeEndorsement.RefundType ?? ""),
                    new  SqlParameter("@RefoundAmount",homeEndorsement.RefundAmount),
                    new  SqlParameter("@RefoundAfterDiscount",homeEndorsement.RefundAfterDiscount),
                    new SqlParameter("@PremiumBeforeDiscount",homeEndorsement.PremiumBeforeDiscount),
                    new SqlParameter("@PremiumAfterDiscount",homeEndorsement.PremiumAfterDiscount),
                    new SqlParameter("@CommissionBeforeDiscount",homeEndorsement.CommisionBeforeDiscount),
                    new SqlParameter("@CommissionAfterDiscount",homeEndorsement.CommissionAfterDiscount),
                    new SqlParameter("@UserChangedPremium",homeEndorsement.UserChangedPremium),
            };
            List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut(){OutPutType = SqlDbType.BigInt, ParameterName= "@NewHomeEndorsementID"},
                    new SPOut() { OutPutType = SqlDbType.NVarChar,ParameterName = "@EndorsementNumber", Size =50 },
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@DocumentNumber", Size=50},
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@EndorsementLinkID", Size=50},
                    new SPOut() { OutPutType = SqlDbType.Int, ParameterName= "@EndorsementCount"},
                };
            object[] dataSet = BKICSQL.GetValues(HomeEndorsementSP.PostHomeEndorsement, paras, outParams);

            var endorsementID = string.IsNullOrEmpty(dataSet[0].ToString()) ? 0 : Convert.ToInt64(dataSet[0].ToString());
            var endorsementNumber = string.IsNullOrEmpty(dataSet[1].ToString()) ? string.Empty : Convert.ToString(dataSet[1].ToString());
            var documentNumber = string.IsNullOrEmpty(dataSet[2].ToString()) ? string.Empty : Convert.ToString(dataSet[2].ToString());
            var endorsementLinkID = string.IsNullOrEmpty(dataSet[3].ToString()) ? string.Empty : Convert.ToString(dataSet[3].ToString());
            var endorsementCount = string.IsNullOrEmpty(dataSet[4].ToString()) ? 0 : Convert.ToInt32(dataSet[4].ToString());

            return new HomeEndorsementResponse()
            {
                IsTransactionDone = true,
                EndorsementNo = endorsementNumber,
                HomeEndorsementID = endorsementID,
                LinkID = endorsementLinkID,
                EndorsementCount = endorsementCount,
                DocumentNo = documentNumber                
            };
        }

        public decimal GetTax(decimal premium, decimal taxRate)
        {
            return premium * taxRate / 100;
        }

    }
}

