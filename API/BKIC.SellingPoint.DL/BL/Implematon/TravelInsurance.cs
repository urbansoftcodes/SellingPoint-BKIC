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
    /// Travel Insurance methods.
    /// </summary>
    public class TravelInsurance : ITravelInsurance
    {
        public readonly OracleDBIntegration.Implementation.TravelInsurance _oracleTravelInsurance;

        public readonly IMail _mail;

        public TravelInsurance()
        {
            _oracleTravelInsurance = new OracleDBIntegration.Implementation.TravelInsurance();
            _mail = new Mail();
        }

        /// <summary>
        /// Get travel policies by agency.
        /// </summary>
        /// <param name="request">Agency travel request.</param>
        /// <returns>List of travel policies belonging to the particular agency.</returns>
        public AgencyTravelPolicyResponse GetTravelAgencyPolicy(AgencyTravelRequest req)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency",req.Agency??string.Empty),
                    new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch",req.AgentBranch??string.Empty),
                    new SqlParameter("@IncludeHIR", req.includeHIR),
                    new SqlParameter("@DocumentNo", req.DocumentNo ?? string.Empty)
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.TravelInsuranceSP.GetAgencyPolicy, para);
                List<AgencyTravelPolicy> agencyTravelPolicies = new List<AgencyTravelPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyTravelPolicy();
                        res.TravelId = Convert.ToInt64(dr["TRAVELID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        agencyTravelPolicies.Add(res);
                    }
                }
                return new AgencyTravelPolicyResponse
                {
                    AgencyTravelPolicies = agencyTravelPolicies,
                    IsTransactionDone = true
                };

            }
            catch (Exception ex)
            {
                return new AgencyTravelPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Get travel policies by CPR.
        /// </summary>
        /// <param name="request">Agency travel request.</param>
        /// <returns>List of travel policies belonging to the particular CPR.</returns>
        public AgencyTravelPolicyResponse GetTravelAgencyPolicyByCPR(AgencyTravelRequest req)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Type",req.Type??string.Empty),
                    new SqlParameter("@Agency",req.Agency??string.Empty),
                    new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                   // new SqlParameter("@AgentBranch",req.AgentBranch??string.Empty),
                    new SqlParameter("@CPR",req.CPR??string.Empty),
                    new SqlParameter("@isEndorsement",req.isEndorsement)
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.MotorInsuranceSP.GETPoliciesByTypeByCPR, para);
                List<AgencyTravelPolicy> agencyTravelPolicies = new List<AgencyTravelPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyTravelPolicy();
                        res.TravelId = Convert.ToInt64(dr["TRAVELID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        agencyTravelPolicies.Add(res);
                    }
                }
                return new AgencyTravelPolicyResponse
                {
                    IsTransactionDone = true,
                    AgencyTravelPolicies = agencyTravelPolicies
                };
            }
            catch (Exception ex)
            {
                return new AgencyTravelPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Get Travel Insurance Quote
        /// </summary>
        /// <param name="quote">Travel quote request.</param>
        /// <returns>Travel premium.</returns>  
        public TravelInsuranceQuoteResponse GetTravelInsuranceQuote(TravelInsuranceQuote quote)
        {
            try
            {
                
                SqlParameter[] para = new SqlParameter[] {
                new SqlParameter("@PackageCode", quote.PackageCode),
                new SqlParameter("@PolicyPeroidCode",quote.PolicyPeriodCode),
                new SqlParameter("@Agency",quote.Agency),
                new SqlParameter("@AgentCode",quote.AgentCode),
                new SqlParameter("@MainClass",quote.MainClass),
                new SqlParameter("@SubClass",quote.SubClass),
                new SqlParameter("@DOB",quote.DateOfBirth),
                new SqlParameter("@CoverageType",quote.CoverageType)
            };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@Premium", Precision= 38, Scale =3 },
                    new SPOut() {OutPutType = SqlDbType.Decimal, ParameterName= "@DiscountPremium", Precision= 38, Scale =3 }
               };


                object[] dataSet = BKICSQL.GetValues(TravelInsuranceSP.GetQuote, para, outParams);
                var premium = decimal.Parse(dataSet[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                // decimal discount = decimal.Parse(dataSet[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                return new TravelInsuranceQuoteResponse()
                {
                    IsTransactionDone = true,
                    Premium = premium
                };
            }
            catch (Exception ex)
            {
                return new TravelInsuranceQuoteResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Insert the travel policy.
        /// </summary>
        /// <param name="travel">Travel policy properties.</param>
        /// <returns>TravelID, Document number.</returns>
        public TravelPolicyResponse PostTravelPolicyDetails(TravelPolicy travel)
        {
            try
            {

                DateTime? expiryDate = GetExpirtyDate(travel.TravelInsurancePolicyDetails.PeroidOfCoverCode,
                    travel.TravelInsurancePolicyDetails.InsuranceStartDate.Value);

                SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@TravelID", travel.TravelInsurancePolicyDetails.TravelID),
                new SqlParameter("@Agency", travel.TravelInsurancePolicyDetails.Agency??string.Empty),
                new SqlParameter("@AgentCode", travel.TravelInsurancePolicyDetails.AgentCode??string.Empty),
                new SqlParameter("@AgentBranch", travel.TravelInsurancePolicyDetails.AgentBranch??string.Empty),
                new SqlParameter("@DOB", travel.TravelInsurancePolicyDetails.DOB),
                new SqlParameter("@PackageCode", travel.TravelInsurancePolicyDetails.PackageCode),
                new SqlParameter("@PolicyPeroidYears",  1 ),
                new SqlParameter("@InsuredCode",travel.TravelInsurancePolicyDetails.InsuredCode??""),
                new SqlParameter("@InsuredName", travel.TravelInsurancePolicyDetails.InsuredName??""),
                new SqlParameter("@SumInsured", travel.TravelInsurancePolicyDetails.SumInsured),
                new SqlParameter("@PremiumAmount", travel.TravelInsurancePolicyDetails.PremiumAmount),
                new SqlParameter("@InsuranceStartDate", travel.TravelInsurancePolicyDetails.InsuranceStartDate),
                new SqlParameter("@MainClass",travel.TravelInsurancePolicyDetails.MainClass??""),
                new SqlParameter("@SubClass",travel.TravelInsurancePolicyDetails.SubClass??""),
                new SqlParameter("@PassportNumber",travel.TravelInsurancePolicyDetails.Passport??""),
                new SqlParameter("@Renewal",travel.TravelInsurancePolicyDetails.Renewal),
                new SqlParameter("@Occupation",travel.TravelInsurancePolicyDetails.Occupation??""),
                new SqlParameter("@PeroidOfCoverCode",travel.TravelInsurancePolicyDetails.PeroidOfCoverCode),
                new SqlParameter("@DiscountAmount", travel.TravelInsurancePolicyDetails.DiscountAmount),
                new SqlParameter("@CPR", travel.TravelInsurancePolicyDetails.CPR),
                new SqlParameter("@MobileNumber", travel.TravelInsurancePolicyDetails.Mobile ?? ""),
                new SqlParameter("@FFPNumber", travel.TravelInsurancePolicyDetails.FFPNumber??string.Empty),
                new SqlParameter("@QuestionaireCode", travel.TravelInsurancePolicyDetails.QuestionaireCode ??""),
                new SqlParameter("@IsPhysicalDefect", travel.TravelInsurancePolicyDetails.IsPhysicalDefect??""),
                new SqlParameter("@PhysicalDescription", travel.TravelInsurancePolicyDetails.PhysicalStateDescription??""),
                new SqlParameter("@CreatedBy", travel.TravelInsurancePolicyDetails.CreatedBy),
                new SqlParameter("@AuthorizedBy", travel.TravelInsurancePolicyDetails.AuthorizedBy),
                new SqlParameter("@dt", travel.TravelMembers),
                new SqlParameter("@IsSaved", travel.TravelInsurancePolicyDetails.IsSaved),
                new SqlParameter("@IsActive", travel.TravelInsurancePolicyDetails.IsActivePolicy),
                new SqlParameter("@CoverageType", travel.TravelInsurancePolicyDetails.CoverageType),
                new SqlParameter("@Source", travel.TravelInsurancePolicyDetails.Source),
                new SqlParameter("@PeriodCode", travel.TravelInsurancePolicyDetails.PeroidOfCoverCode),
                new SqlParameter("@PremiumAfterDiscountAmount", travel.TravelInsurancePolicyDetails.PremiumAfterDiscount),
                new SqlParameter("@CommisionAfterDiscountAmount", travel.TravelInsurancePolicyDetails.CommissionAfterDiscount),
                new SqlParameter("@UserChangedPremium", travel.TravelInsurancePolicyDetails.UserChangedPremium),
                new SqlParameter("@PaymentType", string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.PaymentType) ? "" : travel.TravelInsurancePolicyDetails.PaymentType),
                new SqlParameter("@AccountNumber", string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.AccountNumber) ? "" : travel.TravelInsurancePolicyDetails.AccountNumber),
                new SqlParameter("@Remarks", string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.Remarks) ? "" : travel.TravelInsurancePolicyDetails.Remarks),
                new SqlParameter("@ExpiryDate", expiryDate),
            };
                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType =SqlDbType.Int, ParameterName= "@NewTravelId"},
                    new SPOut() { OutPutType= SqlDbType.NVarChar, ParameterName="@HIRStatusMessage",Size =50 },
                    new SPOut() { OutPutType= SqlDbType.Bit, ParameterName="@IsHIRStatus" },
                    new SPOut() { OutPutType= SqlDbType.NVarChar, ParameterName="@DocumentNumber", Size = 50}
                };
                object[] dataSet = BKICSQL.GetValues(TravelInsuranceSP.PostPolicyDetails, para, outParams);
                var TravelId = Convert.ToInt64(dataSet[0]);
                var HIRStatus = Convert.ToString(dataSet[1]);
                bool IsHIR = Convert.ToBoolean(dataSet[2]);
                var DocumentNo = Convert.ToString(dataSet[3]);
                if (!IsHIR && travel.TravelInsurancePolicyDetails.IsActivePolicy)
                {                   
                    try
                    {
                        new Task(() =>
                        {
                            OracleDBIntegration.DBObjects.TransactionWrapper oracleResult
                             = _oracleTravelInsurance.IntegrateTravelToOracle((int)TravelId);
                        }).Start();
                    }
                    catch (AggregateException ex)
                    {
                        foreach (Exception inner in ex.InnerExceptions)
                        {
                            _mail.SendMailLogError(inner.Message, travel.TravelInsurancePolicyDetails.InsuredCode, "TravelInsurance", travel.TravelInsurancePolicyDetails.Agency, true);
                        }
                    }
                }
                return new TravelPolicyResponse()
                {
                    IsTransactionDone = true,
                    PaymentTrackID = "",
                    IsHIR = IsHIR,
                    DocumentNo = DocumentNo,
                    TravelId = TravelId
                };
            }
            catch (Exception ex)
            {
                _mail.SendMailLogError(ex.Message, 
                    travel.TravelInsurancePolicyDetails.InsuredCode, "TravelInsurance", travel.TravelInsurancePolicyDetails.Agency,
                    false);
                return new TravelPolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get travel policy details by document number.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">details fetched for endorsement page or policy page.</param>
        /// <param name="endorsementid">Endorsement id.</param>
        /// <returns></returns>
        public TravelSavedQuotationResponse GetSavedQuotationByPolicy(string documentNo, string type, string agentCode, 
                                            bool isEndorsement = false, long endorsementID = 0)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@DocumentNo", documentNo),
                    new SqlParameter("@Type", isEndorsement ? "Endorsement" : ""),
                    new SqlParameter("@AgentCode", agentCode),
                    new SqlParameter("@EndorsementID", endorsementID),
                    new SqlParameter("@IsEndorsement", isEndorsement)

                };

                DataSet dataSet = BKICSQL.eds(StoredProcedures.TravelInsuranceSP.GetSavedQuotationByDocumentNo, para);
                TravelInsurancePolicy policyDetails = new TravelInsurancePolicy();               
                List<TravelMembers> members = new List<TravelMembers>();
                InsuredMaster insureddetail = new InsuredMaster();              

                if(dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        var travelPolicy = dataSet.Tables[0].Rows[0];                        

                        policyDetails.TravelID = Convert.ToInt64(travelPolicy["TRAVELID"]);
                        policyDetails.InsuredCode = Convert.ToString(travelPolicy["INSUREDCODE"]);
                        policyDetails.InsuredName = Convert.ToString(travelPolicy["INSUREDNAME"]);
                        policyDetails.DocumentNumber = Convert.ToString(travelPolicy["DOCUMENTNO"]);
                        policyDetails.SumInsured = decimal.Parse(travelPolicy["SUMINSURED"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        policyDetails.PremiumAmount = decimal.Parse(travelPolicy["PREMIUMBEFOREDISCOUNT"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        policyDetails.AgentBranch = Convert.ToString(travelPolicy["BRANCHCODE"]);

                        //policyDetails.SumInsured = travelPolicy["SUMINSURED"].ToString() == "" ? 0 : decimal.Parse(travelPolicy["SUMINSURED"].ToString());
                        //policyDetails.PremiumAmount = travelPolicy["PREMIUMBEFOREDISCOUNT"].ToString() == "" ? 0 : decimal.Parse(travelPolicy["PREMIUMBEFOREDISCOUNT"].ToString());

                        policyDetails.InsuranceStartDate = Convert.ToDateTime(travelPolicy["COMMENCEDATE"]).Date;
                        policyDetails.ExpiryDate = Convert.ToDateTime(travelPolicy["EXPIRYDATE"]).Date;
                        policyDetails.MainClass = Convert.ToString(travelPolicy["MAINCLASS"]);
                        policyDetails.SubClass = Convert.ToString(travelPolicy["SUBCLASS"]);
                        policyDetails.Passport = Convert.ToString(travelPolicy["PASSPORT"]);
                        policyDetails.Occupation = Convert.ToString(travelPolicy["OCCUPATION"]);
                        policyDetails.PeroidOfCoverCode = Convert.ToString(travelPolicy["PERIODOFCOVER"]);
                        policyDetails.LoadAmount = travelPolicy.IsNull("LOADAMOUNT") ? 0 : Convert.ToDecimal(travelPolicy["LOADAMOUNT"]);
                        policyDetails.DiscountAmount = travelPolicy["PREMIUMAFTERDISCOUNT"].ToString() == "" ? 0 : decimal.Parse(travelPolicy["PREMIUMAFTERDISCOUNT"].ToString());
                        //policyDetails.DiscountAmount = 0; //decimal.Parse(travelPolicy["PREMIUMAFTERDISCOUNT"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        //policyDetails.Code = Convert.ToString(travelPolicy["CODE"]);
                        policyDetails.CPR = Convert.ToString(travelPolicy["CPR"]);
                        policyDetails.AccountNumber = travelPolicy.IsNull("AccountNumber") ? "" : Convert.ToString(travelPolicy["AccountNumber"]);
                        policyDetails.Remarks = travelPolicy.IsNull("Remarks") ? "" : Convert.ToString(travelPolicy["Remarks"]);
                        policyDetails.PaymentType = travelPolicy.IsNull("PaymentType") ? "" : Convert.ToString(travelPolicy["PaymentType"]);
                        policyDetails.Mobile = travelPolicy.IsNull("MOBILENUMBER") ? string.Empty : travelPolicy["MOBILENUMBER"].ToString();
                        policyDetails.CreadtedDate = Convert.ToDateTime(travelPolicy["CREATEDDATE"].ToString()).Date;
                        policyDetails.UpdatedDate = Convert.ToDateTime(travelPolicy["UPDATEDDATE"].ToString()).Date;
                        policyDetails.IsPhysicalDefect = Convert.ToString(travelPolicy["ANSWER"]);
                        policyDetails.PhysicalStateDescription = Convert.ToString(travelPolicy["QuestionRemarks"]);
                        policyDetails.IsHIR = travelPolicy.IsNull("IsHIR") ? false : Convert.ToBoolean(travelPolicy["IsHIR"]);
                        policyDetails.FFPNumber = travelPolicy.IsNull("FFPNUMBER") ? string.Empty : Convert.ToString(travelPolicy["FFPNUMBER"]);
                        policyDetails.CoverageType = Convert.ToString(travelPolicy["CoverageType"]);
                        policyDetails.PackageName = Convert.ToString(travelPolicy["PackageName"]);
                        policyDetails.PolicyPeroidName = Convert.ToString(travelPolicy["PeriodName"]);
                        policyDetails.PremiumBeforeDiscount = travelPolicy.IsNull("PREMIUMBEFOREDISCOUNT") ? 0 : Convert.ToDecimal(travelPolicy["PREMIUMBEFOREDISCOUNT"]);
                        policyDetails.PremiumAfterDiscount = travelPolicy.IsNull("PremiumAfterDiscount") ? 0 : Convert.ToDecimal(travelPolicy["PremiumAfterDiscount"]);
                        policyDetails.CommisionBeforeDiscount = travelPolicy.IsNull("CommissionBeforeDiscount") ? 0 : Convert.ToDecimal(travelPolicy["CommissionBeforeDiscount"]);
                        policyDetails.CommissionAfterDiscount = travelPolicy.IsNull("CommissionAfterDiscount") ? 0 : Convert.ToDecimal(travelPolicy["CommissionAfterDiscount"]);
                        policyDetails.IsSaved = travelPolicy.IsNull("IsSaved") ? false : Convert.ToBoolean(travelPolicy["IsSaved"]);
                        policyDetails.IsActivePolicy = travelPolicy.IsNull("IsActive") ? false : Convert.ToBoolean(travelPolicy["IsActive"]);
                        policyDetails.HIRStatus = travelPolicy.IsNull("HIRStatus") ? 0 : Convert.ToInt32(travelPolicy["HIRStatus"]);
                        policyDetails.EndorsementCount = travelPolicy.IsNull("EndorsementCount") ? 0 : Convert.ToInt32(travelPolicy["EndorsementCount"]);
                        policyDetails.IsCancelled = travelPolicy.IsNull("IsCancelled") ? false : Convert.ToBoolean(travelPolicy["IsCancelled"]);   
                        policyDetails.EndorsementType = travelPolicy.IsNull("EndorsementType") ? string.Empty : Convert.ToString(travelPolicy["EndorsementType"]);

                        if (dataSet.Tables[1].Rows.Count > 0)
                        {                            

                            foreach (DataRow row in dataSet.Tables[1].Rows)
                            {
                                TravelMembers member = new TravelMembers();
                                member.Sex = Convert.ToString(row["SEX"]);
                                member.DocumentNo = Convert.ToString(row["DOCUMENTNO"]);
                                member.ItemSerialNo = Convert.ToInt32(row["ITEMSERIALNO"].ToString());
                                member.ItemName = row["ITEMNAME"].ToString();
                                member.SumInsured = decimal.Parse(row["SUMINSURED"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                member.ForeignSumInsured = decimal.Parse(row["FOREIGNSUMINSURED"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                //policyDetails.SumInsured = row["SUMINSURED"].ToString() == "" ? 0 : decimal.Parse(row["SUMINSURED"].ToString());
                                //policyDetails.PremiumAmount = row["FOREIGNSUMINSURED"].ToString() == "" ? 0 : decimal.Parse(row["FOREIGNSUMINSURED"].ToString());
                                member.Category = string.IsNullOrEmpty(row["CATEGORY"].ToString()) ? "SELF" : row["CATEGORY"].ToString();
                                //member.Title = Convert.ToString(row["TITLE"]);
                                member.DateOfBirth = Convert.ToDateTime(Convert.ToDateTime(row["DATEOFBIRTH"]));
                                member.Age = Convert.ToInt32(row["AGE"].ToString());
                                //policyDetails.PremiumAmount = row["PREMIUMAMOUNT"].ToString() == "" ? 0 : decimal.Parse(row["PREMIUMAMOUNT"].ToString());
                                //member.PremiumAmount = decimal.Parse(row["PREMIUMAMOUNT"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                member.Make = Convert.ToString(row["MAKE"]);
                                member.MakeDescription = Convert.ToString(row["MAKEDESCRIPTION"]);
                                member.OccupationCode = Convert.ToString(row["OCCUPATIONCODE"]);
                                member.CPR = Convert.ToString(row["CPR"]);
                                member.Passport = Convert.ToString(row["PASSPORT"]);
                                member.FirstName = row.IsNull("FIRSTNAME") ? row["ITEMNAME"].ToString() : Convert.ToString(row["FIRSTNAME"]);
                                member.MiddleName = Convert.ToString(row["MIDDLENAME"]);
                                member.LastName = Convert.ToString(row["LASTNAME"]);
                                members.Add(member);
                            }                            
                        }
                        
                        if (dataSet.Tables[2].Rows.Count > 0)
                        {
                            var userdetails = dataSet.Tables[2].Rows[0];
                            
                            insureddetail.UserInfo.DOB = Convert.ToDateTime(Convert.ToString(userdetails["DATEOFBIRTH"]));
                            insureddetail.UserInfo.CPR = Convert.ToString(userdetails["CPR"]);
                            insureddetail.UserInfo.Nationality = Convert.ToString(userdetails["NATIONALITY"]);
                            insureddetail.UserInfo.Sex = Convert.ToString(userdetails["Gender"]);
                            //insureddetail.UserInfo.Title = Convert.ToString(userdetails["TITLE"]);
                            insureddetail.UserInfo.FirstName = Convert.ToString(userdetails["FIRSTNAME"]);
                            insureddetail.UserInfo.MiddleName = Convert.ToString(userdetails["MIDDLENAME"]);
                            insureddetail.UserInfo.LastName = Convert.ToString(userdetails["LASTNAME"]);                            
                        }

                        return new TravelSavedQuotationResponse
                        {
                            IsTransactionDone = true,
                            TravelInsurancePolicyDetails = policyDetails,
                            InsuredDetails = insureddetail,
                            TravelMembers = members
                        };
                    }
                    return new TravelSavedQuotationResponse
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = "Policy not found !"
                    };
                }
                return new TravelSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = "Policy not found !"
                };
            }
            catch (Exception exc)
            {
                return new TravelSavedQuotationResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = exc.Message
                };
            }
        }
        /// <summary>
        /// Get travel policies by agency.
        /// </summary>
        /// <param name="request">Agency travel request.</param>
        /// <returns>List of travel policies belonging to the particular agency.</returns>
        public AgencyTravelPolicyResponse GetTravelPoliciesEndorsement(AgencyTravelRequest req)
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
                DataTable dt = BKICSQL.edt(StoredProcedures.TravelInsuranceSP.GetTravelPoliciesEndorsement, para);
                List<AgencyTravelPolicy> agencyTravelPolicies = new List<AgencyTravelPolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyTravelPolicy();
                        res.TravelId = Convert.ToInt64(dr["TRAVELID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        agencyTravelPolicies.Add(res);
                    }
                }
                return new AgencyTravelPolicyResponse
                {
                    AgencyTravelPolicies = agencyTravelPolicies,
                    IsTransactionDone = true
                };

            }
            catch (Exception ex)
            {
                return new AgencyTravelPolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Calculate the travel policy expire date.
        /// </summary>
        /// <param name="commenceDetails">Policy start date.</param>
        /// <returns>Policy expire date.</returns>
        private DateTime GetExpirtyDate(string period, DateTime startDate)
        {
            try
            {
                DateTime ExpiryDate;
                DateTime startdate = startDate;
                if (period == "AN001")
                {
                    DateTime date = startdate.AddYears(1);
                    ExpiryDate = date.AddDays(-1);
                }
                else if (period == "TW001")
                {
                    DateTime date = startdate.AddYears(2);
                    ExpiryDate = date.AddDays(-1);
                }
                else if (period == "7d")
                {
                    ExpiryDate = startdate.AddDays(6);
                }
                else if (period == "10d")
                {
                    ExpiryDate = startdate.AddDays(9);
                }
                else if (period == "15d")
                {
                    ExpiryDate = startdate.AddDays(14);
                }
                else if (period == "21d")
                {
                    ExpiryDate = startdate.AddDays(20);
                }
                else if (period == "30d")
                {
                    ExpiryDate = startdate.AddDays(29);
                }
                else if (period == "60d")
                {
                    ExpiryDate = startdate.AddDays(59);
                }
                else if (period == "90d")
                {
                    ExpiryDate = startdate.AddDays(89);
                }
                else
                {
                    ExpiryDate = startdate.AddDays(179);
                }
                return ExpiryDate;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #region Unused methods
        public UpdateTravelDetailsResponse UpdateTravelDetails(UpdateTravelDetailsRequest travel)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@TravelId", travel.TravelInsurancePolicyDetails.TravelID),
                new SqlParameter("@PackageCode", travel.TravelInsurancePolicyDetails.PackageCode),
                new SqlParameter("@PolicyPeroidYears", travel.TravelInsurancePolicyDetails.PolicyPeroidYears),
                new SqlParameter("@InsuredCode", (!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.InsuredCode) ? travel.TravelInsurancePolicyDetails.InsuredCode : "" )),
                new SqlParameter("@InsuredName", (!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.InsuredName) ? travel.TravelInsurancePolicyDetails.InsuredName : "")),
                new SqlParameter("@SumInsured", travel.TravelInsurancePolicyDetails.SumInsured),
                new SqlParameter("@PremiumAmount", travel.TravelInsurancePolicyDetails.PremiumAmount),
                new SqlParameter("@InsuranceStartDate", travel.TravelInsurancePolicyDetails.InsuranceStartDate),
                new SqlParameter("@MainClass",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.MainClass)?travel.TravelInsurancePolicyDetails.MainClass:(object) DBNull.Value),
                new SqlParameter("@SubClass",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.SubClass)?travel.TravelInsurancePolicyDetails.SubClass:(object) DBNull.Value),
                new SqlParameter("@PassportNumber",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.Passport)?travel.TravelInsurancePolicyDetails.Passport:(object) DBNull.Value),
                new SqlParameter("@Renewal",travel.TravelInsurancePolicyDetails.Renewal),
                new SqlParameter("@Occupation",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.Occupation)?travel.TravelInsurancePolicyDetails.Occupation:(object) DBNull.Value),
                new SqlParameter("@PeroidOfCoverCode",travel.TravelInsurancePolicyDetails.PeroidOfCoverCode),
                new SqlParameter("@DiscountPremiumAmount", travel.TravelInsurancePolicyDetails.DiscountAmount),
                new SqlParameter("@CPR",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.CPR)?travel.TravelInsurancePolicyDetails.CPR:""),
                new SqlParameter("@MobileNumber",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.Mobile)?travel.TravelInsurancePolicyDetails.Mobile:""),
                new SqlParameter("@FFPNumber", travel.TravelInsurancePolicyDetails.FFPNumber != null ? travel.TravelInsurancePolicyDetails.FFPNumber : ""),
                new SqlParameter("@QuestionaireCode", travel.TravelInsurancePolicyDetails.QuestionaireCode != null ? travel.TravelInsurancePolicyDetails.QuestionaireCode : ""),
                new SqlParameter("@IsPhysicalDefect", travel.TravelInsurancePolicyDetails.IsPhysicalDefect != null ? travel.TravelInsurancePolicyDetails.IsPhysicalDefect : ""),
                new SqlParameter("@PhysicalDescription", travel.TravelInsurancePolicyDetails.PhysicalStateDescription != null ? travel.TravelInsurancePolicyDetails.PhysicalStateDescription : ""),
                new SqlParameter("@UpdatedBy",travel.TravelInsurancePolicyDetails.UpdatedBy),
                new SqlParameter("@dt", travel.TravelMembers),
                new SqlParameter("@IsSaved", travel.TravelInsurancePolicyDetails.IsSaved),
                new SqlParameter("@LoadAmount",travel.TravelInsurancePolicyDetails.LoadAmount.HasValue?travel.TravelInsurancePolicyDetails.LoadAmount:0),
                new SqlParameter("@DiscountAmount", travel.TravelInsurancePolicyDetails.Discounted.HasValue?travel.TravelInsurancePolicyDetails.Discounted:0),
                new SqlParameter("@PaymentType", string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.PaymentType) ? "" : travel.TravelInsurancePolicyDetails.PaymentType),
                new SqlParameter("@AccountNumber", string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.AccountNumber) ? "" : travel.TravelInsurancePolicyDetails.AccountNumber),
                 new SqlParameter("@Remarks",string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.Remarks)?"":travel.TravelInsurancePolicyDetails.Remarks),
                new SqlParameter("@CoverageType",string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.CoverageType)?"":travel.TravelInsurancePolicyDetails.CoverageType),
                new SqlParameter("@IsActive", travel.TravelInsurancePolicyDetails.IsActivePolicy),
                new SqlParameter("@Agency", travel.TravelInsurancePolicyDetails.Agency),
                new SqlParameter("@AgentCode", travel.TravelInsurancePolicyDetails.AgentCode),
                new SqlParameter("@BranchCode", travel.TravelInsurancePolicyDetails.AgentBranch),
                new SqlParameter("@PremiumAfterDiscountAmount", travel.TravelInsurancePolicyDetails.PremiumAfterDiscount),
                new SqlParameter("@CommissionAfterDiscountAmount", travel.TravelInsurancePolicyDetails.CommissionAfterDiscount),
                new SqlParameter("@UserChangedPremium", travel.TravelInsurancePolicyDetails.UserChangedPremium),
                new SqlParameter("@Source", travel.TravelInsurancePolicyDetails.Source ?? "")
            };
                List<SPOut> outParams = new List<SPOut>() {
                        new SPOut() { OutPutType =SqlDbType.Bit, ParameterName= "@IsHIR"},
                        new SPOut() { OutPutType =SqlDbType.NVarChar, ParameterName= "@HIRStatusMessage" , Size=200}
                 };
                object[] dataSet = BKICSQL.GetValues(TravelInsuranceSP.UpdatePolicyDetails, para, outParams);
                var IsHIR = Convert.ToBoolean(dataSet[0]);

                return new UpdateTravelDetailsResponse { IsHIR = IsHIR, IsTransactionDone = true, PaymentTrackId = "" };
            }
            catch (Exception ex)
            {
                return new UpdateTravelDetailsResponse { TransactionErrorMessage = ex.Message, IsTransactionDone = false };
            }
        }

        public FetchSumInsuredResponse FetchSumInsuredAmount(string insuranceType)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@InsuranceType",insuranceType )
            };
                DataTable dt = BKICSQL.edt(StoredProcedures.TravelInsuranceSP.FetchSumInsured, para);
                FetchSumInsuredResponse response = new FetchSumInsuredResponse();
                response.IsTransactionDone = true;
                if (dt.Rows.Count > 0)
                {
                    response.SumInsuredBHD = Convert.ToInt32(dt.Rows[0]["SumInsuredBHD"]);
                    response.SumInsuredUSD = Convert.ToInt32(dt.Rows[0]["SumInsuredUSD"]);
                }
                return response;
            }
            catch (Exception ex)
            {
                return new FetchSumInsuredResponse { TransactionErrorMessage = ex.Message };
            }
        }



        //RenewPrecheck
        public RenewPrecheckResponse RenewalPrecheck(string documentNo, string cpr, string type)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]{
                      new SqlParameter("@PolicyNo",documentNo),
                     new  SqlParameter("@CPR",cpr),
                     new  SqlParameter("@Type",type)
                  };

                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsRenewExist"},
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsPolicyExpired"},
                     new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsEarlyRenewal"}
                };
                object[] dataSet = BKICSQL.GetValues(TravelInsuranceSP.RenewalPrecheck, param, outParams);
                var IsRenewExist = Convert.ToBoolean(dataSet[0]);
                var IsPolicyExpired = Convert.ToBoolean(dataSet[1]);
                var IsEarlyRenewal = Convert.ToBoolean(dataSet[2]);

                return new RenewPrecheckResponse { IsTransactionDone = true, IsRenewDetailsExist = IsRenewExist, IsPolicyExpired = IsPolicyExpired, IsEarlyRenewal = IsEarlyRenewal };
            }
            catch (Exception ex)
            {
                return new RenewPrecheckResponse { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }
        public TravelPolicyResponse InsertTravelPolicy(TravelPolicy travel)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@DOB", travel.TravelInsurancePolicyDetails.DOB),
                new SqlParameter("@PackageCode", travel.TravelInsurancePolicyDetails.PackageCode),
                new SqlParameter("@PolicyPeroidYears", travel.TravelInsurancePolicyDetails.PolicyPeroidYears),
                new SqlParameter("@InsuredCode", (!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.InsuredCode) ? travel.TravelInsurancePolicyDetails.InsuredCode : "" )),
                new SqlParameter("@InsuredName", (!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.InsuredName) ? travel.TravelInsurancePolicyDetails.InsuredName : "")),
                new SqlParameter("@SumInsured", travel.TravelInsurancePolicyDetails.SumInsured),
                new SqlParameter("@PremiumAmount", travel.TravelInsurancePolicyDetails.PremiumAmount),
                new SqlParameter("@InsuranceStartDate", travel.TravelInsurancePolicyDetails.InsuranceStartDate),
                new SqlParameter("@MainClass",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.MainClass)?travel.TravelInsurancePolicyDetails.MainClass:(object) DBNull.Value),
                new SqlParameter("@PassportNumber",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.Passport)?travel.TravelInsurancePolicyDetails.Passport:(object) DBNull.Value),
                new SqlParameter("@Renewal",travel.TravelInsurancePolicyDetails.Renewal),
                new SqlParameter("@Occupation",!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.Occupation)?travel.TravelInsurancePolicyDetails.Occupation:(object) DBNull.Value),
                new SqlParameter("@PeroidOfCoverCode",travel.TravelInsurancePolicyDetails.PeroidOfCoverCode),
                new SqlParameter("@DiscountAmount", travel.TravelInsurancePolicyDetails.DiscountAmount),
                new SqlParameter("@CPR", travel.TravelInsurancePolicyDetails.CPR),
                new SqlParameter("@MobileNumber", travel.TravelInsurancePolicyDetails.Mobile),
                new SqlParameter("@FFPNumber", travel.TravelInsurancePolicyDetails.FFPNumber != null ? travel.TravelInsurancePolicyDetails.FFPNumber : ""),
                new SqlParameter("@QuestionaireCode", travel.TravelInsurancePolicyDetails.QuestionaireCode != null ? travel.TravelInsurancePolicyDetails.QuestionaireCode : ""),
                new SqlParameter("@IsPhysicalDefect", travel.TravelInsurancePolicyDetails.IsPhysicalDefect != null ? travel.TravelInsurancePolicyDetails.IsPhysicalDefect : ""),
                new SqlParameter("@PhysicalDescription", travel.TravelInsurancePolicyDetails.PhysicalStateDescription != null ? travel.TravelInsurancePolicyDetails.PhysicalStateDescription : ""),
                new SqlParameter("@CreatedBy", travel.TravelInsurancePolicyDetails.CreatedBy),
                new SqlParameter("@dt", travel.TravelMembers),
                new SqlParameter("@IsSaved", travel.TravelInsurancePolicyDetails.IsSaved),
                new SqlParameter("@Source", travel.TravelInsurancePolicyDetails.Source),
                new SqlParameter("@CoverageType", travel.TravelInsurancePolicyDetails.CoverageType),
                new SqlParameter("@PremiumAfterDiscountAmount", travel.TravelInsurancePolicyDetails.PremiumAfterDiscount),
                new SqlParameter("@CommisionAfterDiscountAmount", travel.TravelInsurancePolicyDetails.CommissionAfterDiscount),
                new SqlParameter("@PaymentType", string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.PaymentType) ? "" : travel.TravelInsurancePolicyDetails.PaymentType),
                new SqlParameter("@AccountNumber", string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.AccountNumber) ? "" : travel.TravelInsurancePolicyDetails.AccountNumber),
                new SqlParameter("@Remarks", string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.Remarks) ? "" : travel.TravelInsurancePolicyDetails.Remarks),
            };
                List<SPOut> outParams = new List<SPOut>() {
                new SPOut() { OutPutType =SqlDbType.Int, ParameterName= "@TravelId"},
                new SPOut() { OutPutType= SqlDbType.NVarChar, ParameterName="@HIRStatusMessage",Size =50 },
                 new SPOut() { OutPutType= SqlDbType.Bit, ParameterName="@IsHIRStatus" }
                    };
                object[] dataSet = BKICSQL.GetValues(TravelInsuranceSP.PostPolicyDetails, para, outParams);
                var TravelId = Convert.ToInt64(dataSet[0]);
                var HIRStatus = Convert.ToString(dataSet[1]);
                bool IsHIR = Convert.ToBoolean(dataSet[2]);

                Payment payment = new Payment();
                PaymentTrackInsertResult paymenttrack = new PaymentTrackInsertResult();
                if (!IsHIR && travel.TravelInsurancePolicyDetails.IsActivePolicy)
                {
                    new Task(() => { OracleDBIntegration.DBObjects.TransactionWrapper oracleResult 
                        = _oracleTravelInsurance.IntegrateTravelToOracle((int)TravelId); }).Start();
                }

                paymenttrack = payment.InsertPaymentTrackDetails(TravelId, Constants.Insurance.Travel, false);

                return new TravelPolicyResponse() { IsTransactionDone = true, PaymentTrackID = paymenttrack.TrackId, IsHIR = IsHIR };
            }
            catch (Exception ex)
            {
                return new TravelPolicyResponse() { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        public TravelSavedQuotationResponse GetSavedQuotationByTravelId(int travelQuotationId, string userInsuredCode, string type)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@InsuredCode", userInsuredCode),
                    new SqlParameter("@TravelId", travelQuotationId),
                     new SqlParameter("@Type", type),
                };

                DataSet dataSet = BKICSQL.eds(StoredProcedures.TravelInsuranceSP.GetSavedTravelQuotation, para);
                TravelSavedQuotationResponse quotation = new TravelSavedQuotationResponse();
                quotation.IsTransactionDone = true;

                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        var travelPolicy = dataSet.Tables[0].Rows[0];
                        TravelInsurancePolicy policyDetails = new TravelInsurancePolicy();
                        policyDetails.TravelID = travelQuotationId;
                        policyDetails.InsuredCode = userInsuredCode;
                        policyDetails.InsuredName = Convert.ToString(travelPolicy["INSUREDNAME"]);
                        policyDetails.DocumentNumber = Convert.ToString(travelPolicy["DOCUMENTNO"]);
                        //policyDetails.SumInsured = decimal.Parse(travelPolicy["SUMINSURED"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        //policyDetails.PremiumAmount = decimal.Parse(travelPolicy["PREMIUMBEFOREDISCOUNT"].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                        policyDetails.SumInsured = travelPolicy["SUMINSURED"].ToString() == "" ? 0 : decimal.Parse(travelPolicy["SUMINSURED"].ToString());
                        policyDetails.PremiumAmount = travelPolicy["PREMIUMBEFOREDISCOUNT"].ToString() == "" ? 0 : decimal.Parse(travelPolicy["PREMIUMBEFOREDISCOUNT"].ToString());

                        policyDetails.InsuranceStartDate = Convert.ToDateTime(travelPolicy["COMMENCEDATE"]);
                        policyDetails.ExpiryDate = Convert.ToDateTime(travelPolicy["EXPIRYDATE"]);
                        policyDetails.MainClass = Convert.ToString(travelPolicy["MAINCLASS"]);
                        policyDetails.SubClass = Convert.ToString(travelPolicy["SUBCLASS"]);
                        policyDetails.Passport = Convert.ToString(travelPolicy["PASSPORT"]);
                        policyDetails.Occupation = Convert.ToString(travelPolicy["OCCUPATION"]);
                        policyDetails.PeroidOfCoverCode = Convert.ToString(travelPolicy["PERIODOFCOVER"]);
                        policyDetails.LoadAmount = travelPolicy.IsNull("LOADAMOUNT") ? 0 : Convert.ToDecimal(travelPolicy["LOADAMOUNT"]);
                        policyDetails.DiscountAmount = travelPolicy["PREMIUMAFTERDISCOUNT"].ToString() == "" ? 0 : decimal.Parse(travelPolicy["PREMIUMAFTERDISCOUNT"].ToString());
                        //policyDetails.DiscountAmount = 0; //decimal.Parse(travelPolicy["PREMIUMAFTERDISCOUNT"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        //policyDetails.Code = Convert.ToString(travelPolicy["CODE"]);
                        policyDetails.CPR = Convert.ToString(travelPolicy["CPR"]);
                        policyDetails.Mobile = travelPolicy["MOBILENUMBER"].ToString();
                        policyDetails.CreadtedDate = Convert.ToDateTime(travelPolicy["CREATEDDATE"].ToString());
                        policyDetails.UpdatedDate = Convert.ToDateTime(travelPolicy["UPDATEDDATE"].ToString());
                        policyDetails.IsPhysicalDefect = Convert.ToString(travelPolicy["ANSWER"]);
                        policyDetails.PhysicalStateDescription = Convert.ToString(travelPolicy["REMARKS"]);
                        policyDetails.IsHIR = Convert.ToBoolean(travelPolicy["IsHIR"]);
                        policyDetails.FFPNumber = Convert.ToString(travelPolicy["FFPNUMBER"]);
                        policyDetails.CoverageType = Convert.ToString(travelPolicy["CoverageType"]);
                        policyDetails.PackageName = Convert.ToString(travelPolicy["PackageName"]);
                        policyDetails.PolicyPeroidName = Convert.ToString(travelPolicy["PeriodName"]);

                        quotation.TravelInsurancePolicyDetails = policyDetails;
                        if (dataSet.Tables[1].Rows.Count > 0)
                        {
                            List<TravelMembers> members = new List<TravelMembers>();

                            foreach (DataRow row in dataSet.Tables[1].Rows)
                            {
                                TravelMembers member = new TravelMembers();
                                member.Sex = Convert.ToString(row["SEX"]);
                                member.DocumentNo = Convert.ToString(row["DOCUMENTNO"]);
                                member.ItemSerialNo = Convert.ToInt32(row["ITEMSERIALNO"].ToString());
                                member.ItemName = row["ITEMNAME"].ToString();
                                //member.SumInsured = decimal.Parse(row["SUMINSURED"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                //member.ForeignSumInsured = decimal.Parse(row["FOREIGNSUMINSURED"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                policyDetails.SumInsured = row["SUMINSURED"].ToString() == "" ? 0 : decimal.Parse(row["SUMINSURED"].ToString());
                                policyDetails.PremiumAmount = row["FOREIGNSUMINSURED"].ToString() == "" ? 0 : decimal.Parse(row["FOREIGNSUMINSURED"].ToString());
                                member.Category = row["CATEGORY"].ToString();
                                member.Title = Convert.ToString(row["TITLE"]);
                                member.DateOfBirth = Convert.ToDateTime(Convert.ToDateTime(row["DATEOFBIRTH"]));
                                member.Age = Convert.ToInt32(row["AGE"].ToString());
                                policyDetails.PremiumAmount = row["PREMIUMAMOUNT"].ToString() == "" ? 0 : decimal.Parse(row["PREMIUMAMOUNT"].ToString());
                                //member.PremiumAmount = decimal.Parse(row["PREMIUMAMOUNT"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                member.Make = Convert.ToString(row["MAKE"]);
                                member.OccupationCode = Convert.ToString(row["OCCUPATIONCODE"]);
                                member.CPR = Convert.ToString(row["CPR"]);
                                member.Passport = Convert.ToString(row["PASSPORT"]);
                                member.FirstName = Convert.ToString(row["FIRSTNAME"]);
                                member.MiddleName = Convert.ToString(row["MIDDLENAME"]);
                                member.LastName = Convert.ToString(row["LASTNAME"]);
                                members.Add(member);
                            }
                            quotation.TravelMembers = members;
                        }

                        if (dataSet.Tables[2].Rows.Count > 0)
                        {
                            var userdetails = dataSet.Tables[2].Rows[0];
                            InsuredMaster insureddetail = new InsuredMaster();
                            insureddetail.UserInfo.DOB = Convert.ToDateTime(Convert.ToString(userdetails["DATEOFBIRTH"]));
                            insureddetail.UserInfo.CPR = Convert.ToString(userdetails["CPR"]);
                            insureddetail.UserInfo.Nationality = Convert.ToString(userdetails["NATIONALITY"]);
                            insureddetail.UserInfo.Sex = Convert.ToString(userdetails["Gender"]);
                            //insureddetail.UserInfo.Title = Convert.ToString(userdetails["TITLE"]);
                            insureddetail.UserInfo.FirstName = Convert.ToString(userdetails["FIRSTNAME"]);
                            insureddetail.UserInfo.MiddleName = Convert.ToString(userdetails["MIDDLENAME"]);
                            insureddetail.UserInfo.LastName = Convert.ToString(userdetails["LASTNAME"]);
                            quotation.InsuredDetails = insureddetail;
                        }
                    }
                }
                return quotation;
            }
            catch (Exception exc)
            {
                return new TravelSavedQuotationResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = exc.Message
                };
            }
        }
        #endregion

    }
}