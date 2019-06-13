using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using OracleDBIntegration = SellingPoint.OracleDBIntegration;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    /// <summary>
    /// Home policy methods.
    /// </summary>
    public class HomeInsurance : IHomeInsurance
    {
        public readonly OracleDBIntegration.Implementation.HomeInsurance _oracleHomeInsurance;
        public readonly IMail _mail;

        public HomeInsurance()
        {
            _oracleHomeInsurance = new OracleDBIntegration.Implementation.HomeInsurance();
            _mail = new Mail();
        }

        /// <summary>
        /// Get quote for the home policy.
        /// </summary>
        /// <param name="homeQuoteRequest">home quote request.</param>
        /// <returns></returns>
        public HomeInsuranceQuoteResponse GetHomeInsuranceQuote(HomeInsuranceQuote homeQuoteReuest)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]{
                   new SqlParameter("@BuildingValue",homeQuoteReuest.BuildingValue),
                   new SqlParameter("@ContentValue",homeQuoteReuest.ContentValue),
                   new SqlParameter("@JewelleryValue", homeQuoteReuest.JewelleryValue),
                   new SqlParameter("@IsPropertToBeInsured",homeQuoteReuest.IsPropertyToBeInsured),
                   new SqlParameter("@JewelleryCover",homeQuoteReuest.JewelleryCover),
                   new SqlParameter("@IsRiotStrikeAdded",homeQuoteReuest.IsRiotStrikeAdded),
                   new SqlParameter("@NumberOfDomesticWorker",homeQuoteReuest.NumberOfDomesticWorker),
                   new SqlParameter("@Agency",homeQuoteReuest.Agency),
                   new SqlParameter("@AgentCode",homeQuoteReuest.AgentCode),                 
                   new SqlParameter("@MainClass",homeQuoteReuest.MainClass),
                   new SqlParameter("@SubClass",homeQuoteReuest.SubClass),
                   new SqlParameter("@RenewalDelayedDays", homeQuoteReuest.RenewalDelayedDays)
               };
                List<SPOut> outParams = new List<SPOut>() {
                new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName= "@TotalPremium", Precision = 30, Scale =3 },
                new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName = "@Commision", Precision = 30, Scale = 3 }
               };

                object[] dataSet = BKICSQL.GetValues(HomeInsuranceSP.GetQuote, paras, outParams);
                var premium = decimal.Parse(dataSet[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                var commission = decimal.Parse(dataSet[1].ToString(), CultureInfo.InvariantCulture.NumberFormat);

                return new HomeInsuranceQuoteResponse()
                {
                    IsTransactionDone = true,
                    TotalPremium = premium,
                    TotalCommission = commission
                };
            }
            catch (Exception ex)
            {
                return new HomeInsuranceQuoteResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Post the home policy.
        /// </summary>
        /// <param name="policydetails">Home policy details.</param>
        /// <returns>Posted home id, document number and hir status.</returns>
        public HomeInsurancePolicyResponse PostHomeInsurancePolicy(HomeInsurancePolicyDetails policydetails)
        {
            try
            {
                HomeCalculator hc = new HomeCalculator();
                return hc.InsertHome(policydetails);
            }
            catch (Exception ex)
            {
                return new HomeInsurancePolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
            //try
            //{
            //    SqlParameter[] paras = new SqlParameter[]{
            //        new SqlParameter("@HomeID",policydetails.HomeInsurancePolicy.HomeID),
            //        new SqlParameter("@InsuredCode",policydetails.HomeInsurancePolicy.InsuredCode),
            //        new SqlParameter("@InsuredName",policydetails.HomeInsurancePolicy.InsuredName),
            //        new SqlParameter("@CPR",policydetails.HomeInsurancePolicy.CPR),
            //        new SqlParameter("@Agency", policydetails.HomeInsurancePolicy.Agency ),
            //        new SqlParameter("@AgentCode",policydetails.HomeInsurancePolicy.AgentCode),
            //        new SqlParameter("@BranchCode",policydetails.HomeInsurancePolicy.AgentBranch),
            //        new SqlParameter("@MainClass", policydetails.HomeInsurancePolicy.MainClass),
            //        new SqlParameter("@SubClass", policydetails.HomeInsurancePolicy.SubClass),
            //        new SqlParameter("@MobileNumber",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.Mobile)?policydetails.HomeInsurancePolicy.Mobile:""),
            //        new SqlParameter("@PolicyStartDate",policydetails.HomeInsurancePolicy.PolicyStartDate!=null?policydetails.HomeInsurancePolicy.PolicyStartDate:(object) DBNull.Value),
            //        new SqlParameter("@BuildingValue",policydetails.HomeInsurancePolicy.BuildingValue),
            //        new SqlParameter("@ContentValue",policydetails.HomeInsurancePolicy.ContentValue),
            //        //new SqlParameter("@PremiumAfterDiscount",policydetails.HomeInsurancePolicy.PremiumAfterDiscount),
            //        //new SqlParameter("@PremiumBeforeDiscount",policydetails.HomeInsurancePolicy.PremiumBeforeDiscount),
            //        new SqlParameter("@BuildingAge",policydetails.HomeInsurancePolicy.BuildingAge),
            //        new SqlParameter("@IsPropertyMortgaged",policydetails.HomeInsurancePolicy.IsPropertyMortgaged),
            //        new SqlParameter("@FinancierCode",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.FinancierCode) ? policydetails.HomeInsurancePolicy.FinancierCode : ""),
            //        new SqlParameter("@IsSafePropertyInsured",policydetails.HomeInsurancePolicy.IsSafePropertyInsured),
            //        new SqlParameter("@JewelleryCover",policydetails.HomeInsurancePolicy.JewelleryCover),
            //        new SqlParameter("@IsRiotStrikeDamage",policydetails.HomeInsurancePolicy.IsRiotStrikeDamage),
            //        new SqlParameter("@IsJointOwnership",policydetails.HomeInsurancePolicy.IsJointOwnership),
            //        new SqlParameter("@JointOwnerName",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.JointOwnerName) ? policydetails.HomeInsurancePolicy.JointOwnerName :""),
            //        new SqlParameter("@IsPropertyInConnectionTrade",policydetails.HomeInsurancePolicy.IsPropertyInConnectionTrade),
            //        new SqlParameter("@IsPropertyCoveredOtherInsurance",policydetails.HomeInsurancePolicy.IsPropertyCoveredOtherInsurance),
            //         new SqlParameter("@NamePolicyReasonSeekingReasons",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.NamePolicyReasonSeekingReasons) ? policydetails.HomeInsurancePolicy.NamePolicyReasonSeekingReasons : ""),
            //       new SqlParameter("@IsPropertyInsuredSustainedAnyLoss",policydetails.HomeInsurancePolicy.IsPropertyInsuredSustainedAnyLoss),
            //        new SqlParameter("@IsPropertyUndergoingConstruction",policydetails.HomeInsurancePolicy.IsPropertyUndergoingConstruction),
            //        new SqlParameter("@IsSingleItemAboveContents",policydetails.HomeInsurancePolicy.IsSingleItemAboveContents),
            //        new SqlParameter("@BuildingNo",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.BuildingNo)?policydetails.HomeInsurancePolicy.BuildingNo:""),
            //        new SqlParameter("@FlatNo",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.FlatNo)?policydetails.HomeInsurancePolicy.FlatNo:""),
            //        new SqlParameter("@RoadNo",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.RoadNo)?policydetails.HomeInsurancePolicy.RoadNo:""),
            //        new SqlParameter("@Area",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.Area)?policydetails.HomeInsurancePolicy.Area:""),
            //        new SqlParameter("@BlockNo",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.BlockNo)?policydetails.HomeInsurancePolicy.BlockNo:""),
            //        new SqlParameter("@BuildingType", policydetails.HomeInsurancePolicy.BuildingType),
            //        new SqlParameter("@NoOfFloors", policydetails.HomeInsurancePolicy.NoOfFloors),
            //        new SqlParameter("@HouseNo",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.HouseNo)?policydetails.HomeInsurancePolicy.HouseNo:""),
            //        new SqlParameter("@ResidanceTypeCode",policydetails.HomeInsurancePolicy.BuildingType == 1 ? "H" : "F"),
            //        new SqlParameter("@FFPNumber",!string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.FFPNumber)?policydetails.HomeInsurancePolicy.FFPNumber:""),
            //        new SqlParameter("@IsRequireDomestic",policydetails.HomeInsurancePolicy.IsRequireDomestic),
            //        new SqlParameter("@NumberOfDomesticWorker",policydetails.HomeInsurancePolicy.NoOfDomesticWorker),
            //        new SqlParameter("@CreatedBy",policydetails.HomeInsurancePolicy.CreatedBy),
            //        new SqlParameter("@AuthorizedBy", policydetails.HomeInsurancePolicy.AuthorizedBy),
            //        new SqlParameter("@IsSaved",policydetails.HomeInsurancePolicy.IsSaved),
            //        new SqlParameter("@IsActive",policydetails.HomeInsurancePolicy.IsActivePolicy),
            //        new SqlParameter("@HomeSubItemsdt",policydetails.HomeSubItemsdt),
            //        new SqlParameter("@HomeDomesticdt",policydetails.HomeDomesticHelpdt),
            //        new SqlParameter("@UserChangedPremium", policydetails.HomeInsurancePolicy.UserChangedPremium),
            //        new SqlParameter("@PremiumAfterDiscountAmount", policydetails.HomeInsurancePolicy.PremiumAfterDiscount),
            //        new SqlParameter("@CommisionAfterDiscountAmount", policydetails.HomeInsurancePolicy.CommissionAfterDiscount),
            //        new SqlParameter("@PaymentType",string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.PaymentType) ? string.Empty : policydetails.HomeInsurancePolicy.PaymentType),
            //        new SqlParameter("@AccountNumber", string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.AccountNumber) ? string.Empty : policydetails.HomeInsurancePolicy.AccountNumber),
            //        new SqlParameter("@Remarks", string.IsNullOrEmpty(policydetails.HomeInsurancePolicy.Remarks) ? string.Empty : policydetails.HomeInsurancePolicy.Remarks),
            //   };
            //    List<SPOut> outParams = new List<SPOut>() {
            //    new SPOut() { OutPutType = SqlDbType.Int, ParameterName= "@NewHomeID"},
            //    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsHir"},
            //    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@DocumentNumber", Size=50}
            //   };
            //    PaymentTrackInsertResult paymenttrack = new PaymentTrackInsertResult();
            //    object[] dataSet = BKICSQL.GetValues(HomeInsuranceSP.PostInsurance, paras, outParams);
            //    var HomeID = Convert.ToInt32(dataSet[0]);
            //    bool IsHIR = Convert.ToBoolean(dataSet[1]);
            //    var DocNo = Convert.ToString(dataSet[2]);

            //    if (HomeID != 0 && !IsHIR && policydetails.HomeInsurancePolicy.IsActivePolicy)
            //    {
            //        Task moveToOracleTask = Task.Factory.StartNew(() => { OracleDBIntegration.DBObjects.TransactionWrapper oracleResult = _oracleHomeInsurance.IntegrateHomeToOracle((int)HomeID); });
            //        try
            //        {
            //            moveToOracleTask.Wait();
            //        }
            //        catch (AggregateException ex)
            //        {
            //            foreach (Exception inner in ex.InnerExceptions)
            //            {
            //                _mail.SendMailLogError(ex.Message, policydetails.HomeInsurancePolicy.InsuredCode, "HomeInsurance", policydetails.HomeInsurancePolicy.Agency, true);
            //            }
            //        }
            //    }
            //    return new HomeInsurancePolicyResponse()
            //    {
            //        IsTransactionDone = true,
            //        HomeId = HomeID,
            //        IsHIR = IsHIR,
            //        TrackID = paymenttrack.TrackId,
            //        DocumentNo = DocNo
            //    };
            //}
            //catch (Exception ex)
            //{
            //    _mail.SendMailLogError(ex.Message, policydetails.HomeInsurancePolicy.InsuredCode, "HomeInsurance", policydetails.HomeInsurancePolicy.Agency, false);
            //    return new HomeInsurancePolicyResponse()
            //    {
            //        IsTransactionDone = false,
            //        TransactionErrorMessage = ex.Message
            //    };
            //}
        }

        /// <summary>
        /// Get the home policy details by home id.
        /// </summary>
        /// <param name="homeId">home id</param>
        /// <param name="insuredCode">insured code</param>
        /// <returns></returns>
        public HomeSavedQuotationResponse GetSavedQuotation(int homeId, string insuredCode)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]{
                   new SqlParameter("@HomeID",homeId),
                   new SqlParameter("@InsuredCode",insuredCode),
               };

                DataSet homeds = BKICSQL.eds(HomeInsuranceSP.GetSavedQuotation, para);

                HomeInsurancePolicy homepolicy = new HomeInsurancePolicy();
                if (homeds != null && homeds.Tables[0].Rows.Count > 0)
                {
                    DataRow homedr = homeds.Tables[0].Rows[0];
                    homepolicy.HomeID = homeId;
                    homepolicy.PolicyStartDate = Convert.ToDateTime(homedr["COMMENCEDATE"]);
                    homepolicy.BuildingValue = !homedr.IsNull("BuildingValue") ? Convert.ToDecimal(homedr["BuildingValue"]) : 0;
                    homepolicy.ContentValue = !homedr.IsNull("ContentValue") ? Convert.ToDecimal(homedr["ContentValue"]) : 0;
                    homepolicy.PremiumBeforeDiscount = homedr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(homedr["PremiumBeforeDiscount"]);
                    homepolicy.PremiumAfterDiscount = !homedr.IsNull("PremiumAfterDiscount") ? Convert.ToDecimal(homedr["PremiumAfterDiscount"]) : 0;
                    homepolicy.BuildingAge = !homedr.IsNull("AGEOFBUILDING") ? Convert.ToInt32(homedr["AGEOFBUILDING"]) : 0;
                    homepolicy.FinancierCode = Convert.ToString(homedr["FINANCECOMPANYID"]);
                    homepolicy.JewelleryCover = Convert.ToString(homedr["JEWELLERYCOVER"]);
                    homepolicy.IsRiotStrikeDamage = !homedr.IsNull("IsSRCC") ? Convert.ToChar(homedr["IsSRCC"]) : 'N';
                    homepolicy.BuildingNo = Convert.ToString(homedr["BUILDINGNO"]);
                    homepolicy.FlatNo = Convert.ToString(homedr["FLATNO"]);
                    homepolicy.RoadNo = Convert.ToString(homedr["STREETNO"]);
                    homepolicy.BlockNo = Convert.ToString(homedr["BLOCKNUMBER"]);
                    homepolicy.Area = Convert.ToString(homedr["AREA"]);
                    homepolicy.ResidanceTypeCode = Convert.ToString(homedr["ResidenceType"]);
                    homepolicy.FFPNumber = Convert.ToString(homedr["FFPNUMBER"]);
                    homepolicy.InsuredName = Convert.ToString(homedr["INSUREDNAME"]);
                    homepolicy.CPR = Convert.ToString(homedr["CPR"]);
                    homepolicy.PolicyExpiryDate = Convert.ToDateTime(homedr["EXPIRYDATE"]);
                    homepolicy.DocumentNo = Convert.ToString(homedr["DOCUMENTNO"]);
                    homepolicy.Mobile = Convert.ToString(homedr["MOBILENUMBER"]);
                    homepolicy.SumInsured = !homedr.IsNull("SumInsured") ? Convert.ToDecimal(homedr["SumInsured"]) : 0;
                    homepolicy.InsuredCode = insuredCode;
                    homepolicy.IsHIR = Convert.ToBoolean(homedr["IsHIR"]);
                }
                if (homeds != null && homeds.Tables[1].Rows.Count > 0)
                {
                    DataRow questiondr = homeds.Tables[1].Rows[0];

                    // homepolicy.BuildingAge =!questiondr.IsNull("BuildingAge")? Convert.ToInt32(questiondr["BuildingAge"]):0;
                    homepolicy.IsPropertyInConnectionTrade = !questiondr.IsNull("PropertyInConnectionTrade") ? (Convert.ToString(questiondr["PropertyInConnectionTrade"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsSafePropertyInsured = !questiondr.IsNull("SafePropertyInsured") ? (Convert.ToString(questiondr["SafePropertyInsured"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsRiotStrikeDamage = !questiondr.IsNull("RiotStrikeDamage") ? (Convert.ToString(questiondr["RiotStrikeDamage"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsPropertyCoveredOtherInsurance = !questiondr.IsNull("PropertyCoveredOtherInsurance") ? (Convert.ToString(questiondr["PropertyCoveredOtherInsurance"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsPropertyInsuredSustainedAnyLoss = !questiondr.IsNull("PropertyInsuredSustainedAnyLoss") ? (Convert.ToString(questiondr["PropertyInsuredSustainedAnyLoss"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsPropertyUndergoingConstruction = !questiondr.IsNull("PropertyUndergoingConstruction") ? (Convert.ToString(questiondr["PropertyUndergoingConstruction"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsSingleItemAboveContents = !questiondr.IsNull("SingleItemAboveContents") ? (Convert.ToString(questiondr["SingleItemAboveContents"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsRequireDomestic = !questiondr.IsNull("RequireDomestic") ? (Convert.ToString(questiondr["RequireDomestic"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsJointOwnership = !questiondr.IsNull("IsJointOwnership") ? (Convert.ToString(questiondr["IsJointOwnership"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.JointOwnerName = !questiondr.IsNull("JointOwnerName") ? Convert.ToString(questiondr["JointOwnerName"]) : "";
                }

                List<HomeDomesticHelp> domestichelpmembers = new List<HomeDomesticHelp>();
                if (homeds != null && homeds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in homeds.Tables[2].Rows)
                    {
                        HomeDomesticHelp homedomestic = new HomeDomesticHelp()
                        {
                            HomeSID = Convert.ToInt32(dr["HOMEDID"]),
                            MemberSerialNo = dr.IsNull("MEMBERSERIALNO") ? 0 : Convert.ToInt32(dr["MEMBERSERIALNO"]),
                            Name = Convert.ToString(dr["NAME"]),
                            Sex = dr.IsNull("SEX") ? '\0' : Convert.ToChar(dr["SEX"]),
                            CPR = Convert.ToString(dr["CPRNUMBER"]),
                            Title = Convert.ToString(dr["TITLE"]),
                            DOB = Convert.ToDateTime(dr["DATEOFBIRTH"]),
                            Age = !dr.IsNull("AGE") ? Convert.ToInt32(dr["AGE"]) : 0
                        };
                        domestichelpmembers.Add(homedomestic);
                    }
                }

                List<HomeSubItems> homeitems = new List<HomeSubItems>();
                if (homeds != null && homeds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in homeds.Tables[3].Rows)
                    {
                        HomeSubItems homedomestic = new HomeSubItems()
                        {
                            HomeSID = Convert.ToInt32(dr["HOMESID"]),
                            HomeID = Convert.ToInt32(dr["HOMEID"]),
                            SubItemName = Convert.ToString(dr["SUBITEMNAME"]),
                            Description = Convert.ToString(dr["DESCRIPTION"]),
                            SumInsured = Convert.ToDecimal(dr["SUMINSURED"])
                        };
                        homeitems.Add(homedomestic);
                    }
                }
                return new HomeSavedQuotationResponse
                {
                    HomeInsurancePolicy = homepolicy,
                    DomesticHelp = domestichelpmembers,
                    HomeSubItems = homeitems,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get home policy details by document number.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">details fetched for endorsement page or policy page.</param>
        /// <param name="endorsementID">Endorsement id</param>
        /// <returns></returns>
        public HomeSavedQuotationResponse GetSavedQuotationPolicy(string documentNo, string type, string agentCode,
                                          bool isEndorsement = false, long endorsementID = 0, int renewalCount = 0)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@DocumentNo", documentNo),
                    new SqlParameter("@Type", type),
                    new SqlParameter("@AgentCode", agentCode),
                    new SqlParameter("@IsEndorsement", isEndorsement),
                    new SqlParameter("@EndorsementID", endorsementID),
                    new SqlParameter("@RenewalCount", renewalCount)
                };

                DataSet homeds = BKICSQL.eds(HomeInsuranceSP.GetSavedQuotationByDocumentNo, para);
                HomeInsurancePolicy homepolicy = new HomeInsurancePolicy();
                if (homeds != null && homeds.Tables[0] != null && homeds.Tables[0].Rows.Count > 0)
                {
                    DataRow homedr = homeds.Tables[0].Rows[0];

                    homepolicy.HomeID = Convert.ToInt64(homedr["HomeID"]);
                    homepolicy.PolicyStartDate = Convert.ToDateTime(homedr["COMMENCEDATE"]);
                    homepolicy.BuildingValue = !homedr.IsNull("BuildingValue") ? Convert.ToDecimal(homedr["BuildingValue"]) : 0;
                    homepolicy.ContentValue = !homedr.IsNull("ContentValue") ? Convert.ToDecimal(homedr["ContentValue"]) : 0;
                    homepolicy.JewelleryValue = !homedr.IsNull("JewelleryValue") ? Convert.ToDecimal(homedr["JewelleryValue"]) : 0;
                    homepolicy.PremiumBeforeDiscount = homedr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(homedr["PremiumBeforeDiscount"]);
                    homepolicy.PremiumAfterDiscount = !homedr.IsNull("PremiumAfterDiscount") ? Convert.ToDecimal(homedr["PremiumAfterDiscount"]) : 0;
                    homepolicy.BuildingAge = !homedr.IsNull("AGEOFBUILDING") ? Convert.ToInt32(homedr["AGEOFBUILDING"]) : 0;
                    homepolicy.FinancierCode = !homedr.IsNull("FINANCECOMPANYID") ? Convert.ToString(homedr["FINANCECOMPANYID"]) : string.Empty;
                    homepolicy.JewelleryCover = !homedr.IsNull("JEWELLERYCOVER") ? Convert.ToString(homedr["JEWELLERYCOVER"]) : string.Empty;
                    homepolicy.IsRiotStrikeDamage = !homedr.IsNull("IsSRCC") ? Convert.ToChar(homedr["IsSRCC"]) : 'N';
                    homepolicy.BuildingNo = !homedr.IsNull("BUILDINGNO") ? Convert.ToString(homedr["BUILDINGNO"]) : string.Empty;
                    homepolicy.FlatNo = !homedr.IsNull("FLATNO") ? Convert.ToString(homedr["FLATNO"]) : string.Empty;
                    homepolicy.RoadNo = !homedr.IsNull("STREETNO") ? Convert.ToString(homedr["STREETNO"]) : string.Empty;
                    homepolicy.BlockNo = !homedr.IsNull("BLOCKNUMBER") ? Convert.ToString(homedr["BLOCKNUMBER"]) : string.Empty;
                    homepolicy.Area = !homedr.IsNull("AREA") ? Convert.ToString(homedr["AREA"]) : string.Empty;
                    homepolicy.ResidanceTypeCode = !homedr.IsNull("ResidenceType") ? Convert.ToString(homedr["ResidenceType"]) : string.Empty;
                    homepolicy.BuildingType = !homedr.IsNull("BuildingType") ? Convert.ToInt32(homedr["BuildingType"]) : 0;
                    homepolicy.NoOfFloors = !homedr.IsNull("NoOfFloors") ? Convert.ToInt32(homedr["NoOfFloors"]) : 0;
                    homepolicy.HouseNo = !homedr.IsNull("HouseNo") ? Convert.ToString(homedr["HouseNo"]) : string.Empty;
                    homepolicy.InsuredName = !homedr.IsNull("INSUREDNAME") ? Convert.ToString(homedr["INSUREDNAME"]) : string.Empty;
                    homepolicy.CPR = !homedr.IsNull("CPR") ? Convert.ToString(homedr["CPR"]) : string.Empty;
                    homepolicy.PolicyExpiryDate = Convert.ToDateTime(homedr["EXPIRYDATE"]);
                    homepolicy.DocumentNo = !homedr.IsNull("DOCUMENTNO") ? Convert.ToString(homedr["DOCUMENTNO"]) : string.Empty;
                    homepolicy.Mobile = !homedr.IsNull("MOBILENUMBER") ? Convert.ToString(homedr["MOBILENUMBER"]) : string.Empty;
                    homepolicy.SumInsured = !homedr.IsNull("SumInsured") ? Convert.ToDecimal(homedr["SumInsured"]) : 0;
                    homepolicy.InsuredCode = !homedr.IsNull("InsuredCode") ? Convert.ToString(homedr["InsuredCode"]) : string.Empty;
                    homepolicy.IsHIR = !homedr.IsNull("IsHIR") ? Convert.ToBoolean(homedr["IsHIR"]) : false;
                    homepolicy.CommisionBeforeDiscount = !homedr.IsNull("CommissionBeforeDiscount") ? Convert.ToDecimal(homedr["CommissionBeforeDiscount"]) : 0;
                    homepolicy.CommissionAfterDiscount = !homedr.IsNull("CommissionAfterDiscount") ? Convert.ToDecimal(homedr["CommissionAfterDiscount"]) : 0;
                    homepolicy.IsSaved = !homedr.IsNull("IsSaved") ? Convert.ToBoolean(homedr["IsSaved"]) : false;
                    homepolicy.IsActivePolicy = !homedr.IsNull("IsActive") ? Convert.ToBoolean(homedr["IsActive"]) : false;
                    homepolicy.PaymentType = !homedr.IsNull("PaymentType") ? Convert.ToString(homedr["PaymentType"]) : string.Empty;
                    homepolicy.AccountNumber = !homedr.IsNull("Accountnumber") ? Convert.ToString(homedr["Accountnumber"]) : string.Empty;
                    homepolicy.Remarks = !homedr.IsNull("Remarks") ? Convert.ToString(homedr["Remarks"]) : string.Empty;
                    homepolicy.HIRStatus = !homedr.IsNull("HIRStatus") ? Convert.ToInt32(homedr["HIRStatus"]) : 0;
                    homepolicy.EndorsementCount = !homedr.IsNull("EndorsementCount") ? Convert.ToInt32(homedr["EndorsementCount"]) : 0;
                    homepolicy.MainClass = !homedr.IsNull("MainClass") ? Convert.ToString(homedr["MainClass"]) : "PPTY";
                    homepolicy.SubClass = !homedr.IsNull("SubClass") ? Convert.ToString(homedr["SubClass"]) : "SH";
                    homepolicy.IsCancelled = !homedr.IsNull("IsCancelled") ? Convert.ToBoolean(homedr["IsCancelled"]) : false;
                    homepolicy.TaxOnPremium = !homedr.IsNull("TAXONPREMIUM") ? Convert.ToDecimal(homedr["TAXONPREMIUM"]) : decimal.Zero;
                    homepolicy.TaxOnCommission = !homedr.IsNull("TAXONCOMMISSION") ? Convert.ToDecimal(homedr["TAXONCOMMISSION"]) : decimal.Zero;
                    homepolicy.EndorsementType = !homedr.IsNull("EndorsementType") ? Convert.ToString(homedr["EndorsementType"]) : string.Empty;
                    homepolicy.RenewalCount = !homedr.IsNull("RenewalCount") ? Convert.ToInt32(homedr["RenewalCount"]) : 0;
                    homepolicy.RenewalDelayedDays = !homedr.IsNull("RenewalDelayedDays") ? Convert.ToInt32(homedr["RenewalDelayedDays"]) : 0;
                    homepolicy.ActualRenewalStartDate = !homedr.IsNull("ActualRenewalStartDate") ? Convert.ToDateTime(homedr["ActualRenewalStartDate"]) : (DateTime?)null;

                    if (homeds != null && homeds.Tables[1] != null && homeds.Tables[1].Rows.Count > 0)
                    {
                        DataRow questiondr = homeds.Tables[1].Rows[0];

                        // homepolicy.BuildingAge =!questiondr.IsNull("BuildingAge")? Convert.ToInt32(questiondr["BuildingAge"]):0;
                        homepolicy.IsPropertyMortgaged = !questiondr.IsNull("PropertyMortgaged") ? (Convert.ToString(questiondr["PropertyMortgaged"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsPropertyInConnectionTrade = !questiondr.IsNull("PropertyInConnectionTrade") ? (Convert.ToString(questiondr["PropertyInConnectionTrade"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsSafePropertyInsured = !questiondr.IsNull("SafePropertyInsured") ? (Convert.ToString(questiondr["SafePropertyInsured"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsRiotStrikeDamage = !questiondr.IsNull("RiotStrikeDamage") ? (Convert.ToString(questiondr["RiotStrikeDamage"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsPropertyCoveredOtherInsurance = !questiondr.IsNull("PropertyCoveredOtherInsurance") ? (Convert.ToString(questiondr["PropertyCoveredOtherInsurance"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsPropertyInsuredSustainedAnyLoss = !questiondr.IsNull("PropertyInsuredSustainedAnyLoss") ? (Convert.ToString(questiondr["PropertyInsuredSustainedAnyLoss"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsPropertyUndergoingConstruction = !questiondr.IsNull("PropertyUndergoingConstruction") ? (Convert.ToString(questiondr["PropertyUndergoingConstruction"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsSingleItemAboveContents = !questiondr.IsNull("SingleItemAboveContents") ? (Convert.ToString(questiondr["SingleItemAboveContents"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsRequireDomestic = !questiondr.IsNull("RequireDomestic") ? (Convert.ToString(questiondr["RequireDomestic"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.IsJointOwnership = !questiondr.IsNull("IsJointOwnership") ? (Convert.ToString(questiondr["IsJointOwnership"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                        homepolicy.JointOwnerName = !questiondr.IsNull("JointOwnerName") ? Convert.ToString(questiondr["JointOwnerName"]) : "";
                        homepolicy.NamePolicyReasonSeekingReasons = !questiondr.IsNull("PolicyReasonSeekingReasons") ? Convert.ToString(questiondr["PolicyReasonSeekingReasons"]) : "";
                        homepolicy.FinancierCode = !questiondr.IsNull("FinancierCode") ? Convert.ToString(questiondr["FinancierCode"]) : "";
                        homepolicy.JewelleryCoverType = !questiondr.IsNull("JewelleryCoverType") ? Convert.ToString(questiondr["JewelleryCoverType"]) : "";
                    }

                    List<HomeDomesticHelp> domestichelpmembers = new List<HomeDomesticHelp>();
                    if (homeds != null && homeds.Tables[2] != null && homeds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in homeds.Tables[2].Rows)
                        {
                            HomeDomesticHelp homedomestic = new HomeDomesticHelp()
                            {
                                HomeSID = Convert.ToInt32(dr["HOMEDID"]),
                                MemberSerialNo = dr.IsNull("MEMBERSERIALNO") ? 0 : Convert.ToInt32(dr["MEMBERSERIALNO"]),
                                Name = Convert.ToString(dr["NAME"]),
                                Sex = dr.IsNull("SEX") ? '\0' : Convert.ToChar(dr["SEX"]),
                                CPR = Convert.ToString(dr["CPRNUMBER"]),
                                Title = Convert.ToString(dr["TITLE"]),
                                DOB = Convert.ToDateTime(dr["DATEOFBIRTH"]),
                                Occupation = dr.IsNull("OCCUPATION") ? "" : Convert.ToString(dr["OCCUPATION"]),
                                Nationality = dr.IsNull("NATIONALITY") ? "" : Convert.ToString(dr["NATIONALITY"]),
                                Age = !dr.IsNull("AGE") ? Convert.ToInt32(dr["AGE"]) : 0
                            };
                            domestichelpmembers.Add(homedomestic);
                        }
                    }
                    homepolicy.NoOfDomesticWorker = domestichelpmembers.Count;

                    List<HomeSubItems> homeitems = new List<HomeSubItems>();
                    if (homeds != null && homeds.Tables[3] != null && homeds.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr in homeds.Tables[3].Rows)
                        {
                            HomeSubItems homeitem = new HomeSubItems()
                            {
                                HomeSID = Convert.ToInt32(dr["HOMESID"]),
                                HomeID = Convert.ToInt64(dr["HOMEID"]),
                                SubItemName = dr.IsNull("SUBITEMNAME") ? string.Empty : Convert.ToString(dr["SUBITEMNAME"]),
                                Description = dr.IsNull("DESCRIPTION") ? string.Empty : Convert.ToString(dr["DESCRIPTION"]),
                                SumInsured = Convert.ToDecimal(dr["SUMINSURED"])
                            };
                            homeitems.Add(homeitem);
                        }
                    }
                    return new HomeSavedQuotationResponse
                    {
                        HomeInsurancePolicy = homepolicy,
                        DomesticHelp = domestichelpmembers,
                        HomeSubItems = homeitems,
                        IsTransactionDone = true
                    };
                }
                return new HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = "Policy not found!"
                };
            }
            catch (Exception ex)
            {
                return new HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get home policy details by document number.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">details fetched for endorsement page or policy page.</param>
        /// <param name="endorsementID">Endorsement id</param>
        /// <returns></returns>
        public HomeSavedQuotationResponse GetRenewalHomePolicy(string documentNo, string type, string agentCode, int renewalCount)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@DocumentNo", documentNo),
                    new SqlParameter("@Type", type),
                    new SqlParameter("@AgentCode", agentCode),
                    new SqlParameter("@RenewalCount", renewalCount)
                };
                DataSet homeds = BKICSQL.eds(HomeInsuranceSP.GetRenewalHomeByDocumentNo, para);
                HomeInsurancePolicy homepolicy = new HomeInsurancePolicy();
                if (homeds != null && homeds.Tables[0] != null && homeds.Tables[0].Rows.Count > 0)
                {
                    DataRow homedr = homeds.Tables[0].Rows[0];

                    homepolicy.HomeID = Convert.ToInt64(homedr["HomeID"]);
                    homepolicy.PolicyStartDate = Convert.ToDateTime(homedr["COMMENCEDATE"]);
                    homepolicy.BuildingValue = !homedr.IsNull("BuildingValue") ? Convert.ToDecimal(homedr["BuildingValue"]) : 0;
                    homepolicy.ContentValue = !homedr.IsNull("ContentValue") ? Convert.ToDecimal(homedr["ContentValue"]) : 0;
                    homepolicy.JewelleryValue = !homedr.IsNull("JewelleryValue") ? Convert.ToDecimal(homedr["JewelleryValue"]) : 0;
                    homepolicy.PremiumBeforeDiscount = homedr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(homedr["PremiumBeforeDiscount"]);
                    homepolicy.PremiumAfterDiscount = !homedr.IsNull("PremiumAfterDiscount") ? Convert.ToDecimal(homedr["PremiumAfterDiscount"]) : 0;
                    homepolicy.BuildingAge = !homedr.IsNull("AGEOFBUILDING") ? Convert.ToInt32(homedr["AGEOFBUILDING"]) : 0;
                    homepolicy.FinancierCode = !homedr.IsNull("FINANCECOMPANYID") ? Convert.ToString(homedr["FINANCECOMPANYID"]) : string.Empty;
                    homepolicy.JewelleryCover = !homedr.IsNull("JEWELLERYCOVER") ? Convert.ToString(homedr["JEWELLERYCOVER"]) : string.Empty;
                    homepolicy.IsRiotStrikeDamage = !homedr.IsNull("IsSRCC") ? Convert.ToChar(homedr["IsSRCC"]) : 'N';
                    homepolicy.BuildingNo = !homedr.IsNull("BUILDINGNO") ? Convert.ToString(homedr["BUILDINGNO"]) : string.Empty;
                    homepolicy.FlatNo = !homedr.IsNull("FLATNO") ? Convert.ToString(homedr["FLATNO"]) : string.Empty;
                    homepolicy.RoadNo = !homedr.IsNull("STREETNO") ? Convert.ToString(homedr["STREETNO"]) : string.Empty;
                    homepolicy.BlockNo = !homedr.IsNull("BLOCKNUMBER") ? Convert.ToString(homedr["BLOCKNUMBER"]) : string.Empty;
                    homepolicy.Area = !homedr.IsNull("AREA") ? Convert.ToString(homedr["AREA"]) : string.Empty;
                    homepolicy.ResidanceTypeCode = !homedr.IsNull("ResidenceType") ? Convert.ToString(homedr["ResidenceType"]) : string.Empty;
                    homepolicy.BuildingType = !homedr.IsNull("BuildingType") ? Convert.ToInt32(homedr["BuildingType"]) : 0;
                    homepolicy.NoOfFloors = !homedr.IsNull("NoOfFloors") ? Convert.ToInt32(homedr["NoOfFloors"]) : 0;
                    homepolicy.HouseNo = !homedr.IsNull("HouseNo") ? Convert.ToString(homedr["HouseNo"]) : string.Empty;
                    homepolicy.InsuredName = !homedr.IsNull("INSUREDNAME") ? Convert.ToString(homedr["INSUREDNAME"]) : string.Empty;
                    homepolicy.CPR = !homedr.IsNull("CPR") ? Convert.ToString(homedr["CPR"]) : string.Empty;
                    homepolicy.PolicyExpiryDate = Convert.ToDateTime(homedr["EXPIRYDATE"]);
                    homepolicy.DocumentNo = !homedr.IsNull("DOCUMENTNO") ? Convert.ToString(homedr["DOCUMENTNO"]) : string.Empty;
                    homepolicy.Mobile = !homedr.IsNull("MOBILENUMBER") ? Convert.ToString(homedr["MOBILENUMBER"]) : string.Empty;
                    homepolicy.SumInsured = !homedr.IsNull("SumInsured") ? Convert.ToDecimal(homedr["SumInsured"]) : 0;
                    homepolicy.InsuredCode = !homedr.IsNull("InsuredCode") ? Convert.ToString(homedr["InsuredCode"]) : string.Empty;
                    homepolicy.IsHIR = !homedr.IsNull("IsHIR") ? Convert.ToBoolean(homedr["IsHIR"]) : false;
                    homepolicy.CommisionBeforeDiscount = !homedr.IsNull("CommissionBeforeDiscount") ? Convert.ToDecimal(homedr["CommissionBeforeDiscount"]) : 0;
                    homepolicy.CommissionAfterDiscount = !homedr.IsNull("CommissionAfterDiscount") ? Convert.ToDecimal(homedr["CommissionAfterDiscount"]) : 0;
                    homepolicy.IsSaved = !homedr.IsNull("IsSaved") ? Convert.ToBoolean(homedr["IsSaved"]) : false;
                    homepolicy.IsActivePolicy = !homedr.IsNull("IsActive") ? Convert.ToBoolean(homedr["IsActive"]) : false;
                    homepolicy.PaymentType = !homedr.IsNull("PaymentType") ? Convert.ToString(homedr["PaymentType"]) : string.Empty;
                    homepolicy.AccountNumber = !homedr.IsNull("Accountnumber") ? Convert.ToString(homedr["Accountnumber"]) : string.Empty;
                    homepolicy.Remarks = !homedr.IsNull("Remarks") ? Convert.ToString(homedr["Remarks"]) : string.Empty;
                    homepolicy.HIRStatus = !homedr.IsNull("HIRStatus") ? Convert.ToInt32(homedr["HIRStatus"]) : 0;
                    homepolicy.EndorsementCount = !homedr.IsNull("EndorsementCount") ? Convert.ToInt32(homedr["EndorsementCount"]) : 0;
                    homepolicy.MainClass = !homedr.IsNull("MainClass") ? Convert.ToString(homedr["MainClass"]) : "PPTY";
                    homepolicy.SubClass = !homedr.IsNull("SubClass") ? Convert.ToString(homedr["SubClass"]) : "SH";
                    homepolicy.IsCancelled = !homedr.IsNull("IsCancelled") ? Convert.ToBoolean(homedr["IsCancelled"]) : false;
                    homepolicy.TaxOnPremium = !homedr.IsNull("TAXONPREMIUM") ? Convert.ToDecimal(homedr["TAXONPREMIUM"]) : decimal.Zero;
                    homepolicy.TaxOnCommission = !homedr.IsNull("TAXONCOMMISSION") ? Convert.ToDecimal(homedr["TAXONCOMMISSION"]) : decimal.Zero;
                    homepolicy.IsSavedRenewal = !homedr.IsNull("IsSavedRenewal") ? Convert.ToBoolean(homedr["IsSavedRenewal"]) : false;
                    homepolicy.RenewalCount = !homedr.IsNull("RenewalCount") ? Convert.ToInt32(homedr["RenewalCount"]) : 0;
                    homepolicy.RenewalDelayedDays = !homedr.IsNull("RenewalDelayedDays") ? Convert.ToInt32(homedr["RenewalDelayedDays"]) : 0;
                    homepolicy.ActualRenewalStartDate = !homedr.IsNull("ActualRenewalStartDate") ? Convert.ToDateTime(homedr["ActualRenewalStartDate"]) : (DateTime?)null;
                }
                if (homeds != null && homeds.Tables[1] != null && homeds.Tables[1].Rows.Count > 0)
                {
                    DataRow questiondr = homeds.Tables[1].Rows[0];

                    // homepolicy.BuildingAge =!questiondr.IsNull("BuildingAge")? Convert.ToInt32(questiondr["BuildingAge"]):0;
                    homepolicy.IsPropertyMortgaged = !questiondr.IsNull("PropertyMortgaged") ? (Convert.ToString(questiondr["PropertyMortgaged"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsPropertyInConnectionTrade = !questiondr.IsNull("PropertyInConnectionTrade") ? (Convert.ToString(questiondr["PropertyInConnectionTrade"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsSafePropertyInsured = !questiondr.IsNull("SafePropertyInsured") ? (Convert.ToString(questiondr["SafePropertyInsured"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsRiotStrikeDamage = !questiondr.IsNull("RiotStrikeDamage") ? (Convert.ToString(questiondr["RiotStrikeDamage"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsPropertyCoveredOtherInsurance = !questiondr.IsNull("PropertyCoveredOtherInsurance") ? (Convert.ToString(questiondr["PropertyCoveredOtherInsurance"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsPropertyInsuredSustainedAnyLoss = !questiondr.IsNull("PropertyInsuredSustainedAnyLoss") ? (Convert.ToString(questiondr["PropertyInsuredSustainedAnyLoss"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsPropertyUndergoingConstruction = !questiondr.IsNull("PropertyUndergoingConstruction") ? (Convert.ToString(questiondr["PropertyUndergoingConstruction"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsSingleItemAboveContents = !questiondr.IsNull("SingleItemAboveContents") ? (Convert.ToString(questiondr["SingleItemAboveContents"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsRequireDomestic = !questiondr.IsNull("RequireDomestic") ? (Convert.ToString(questiondr["RequireDomestic"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.IsJointOwnership = !questiondr.IsNull("IsJointOwnership") ? (Convert.ToString(questiondr["IsJointOwnership"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                    homepolicy.JointOwnerName = !questiondr.IsNull("JointOwnerName") ? Convert.ToString(questiondr["JointOwnerName"]) : "";
                    homepolicy.NamePolicyReasonSeekingReasons = !questiondr.IsNull("PolicyReasonSeekingReasons") ? Convert.ToString(questiondr["PolicyReasonSeekingReasons"]) : "";
                    homepolicy.FinancierCode = !questiondr.IsNull("FinancierCode") ? Convert.ToString(questiondr["FinancierCode"]) : "";
                    homepolicy.JewelleryCoverType = !questiondr.IsNull("JewelleryCoverType") ? Convert.ToString(questiondr["JewelleryCoverType"]) : "";
                }

                List<HomeDomesticHelp> domestichelpmembers = new List<HomeDomesticHelp>();
                if (homeds != null && homeds.Tables[2] != null && homeds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow dr in homeds.Tables[2].Rows)
                    {
                        HomeDomesticHelp homedomestic = new HomeDomesticHelp()
                        {
                            HomeSID = Convert.ToInt32(dr["HOMEDID"]),
                            MemberSerialNo = dr.IsNull("MEMBERSERIALNO") ? 0 : Convert.ToInt32(dr["MEMBERSERIALNO"]),
                            Name = Convert.ToString(dr["NAME"]),
                            Sex = dr.IsNull("SEX") ? '\0' : Convert.ToChar(dr["SEX"]),
                            CPR = Convert.ToString(dr["CPRNUMBER"]),
                            Title = Convert.ToString(dr["TITLE"]),
                            DOB = Convert.ToDateTime(dr["DATEOFBIRTH"]),
                            Occupation = dr.IsNull("OCCUPATION") ? "" : Convert.ToString(dr["OCCUPATION"]),
                            Nationality = dr.IsNull("NATIONALITY") ? "" : Convert.ToString(dr["NATIONALITY"]),
                            Age = !dr.IsNull("AGE") ? Convert.ToInt32(dr["AGE"]) : 0
                        };
                        domestichelpmembers.Add(homedomestic);
                    }
                }
                homepolicy.NoOfDomesticWorker = domestichelpmembers.Count;

                List<HomeSubItems> homeitems = new List<HomeSubItems>();
                if (homeds != null && homeds.Tables[3] != null && homeds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in homeds.Tables[3].Rows)
                    {
                        HomeSubItems homeitem = new HomeSubItems()
                        {
                            HomeSID = Convert.ToInt32(dr["HOMESID"]),
                            HomeID = Convert.ToInt64(dr["HOMEID"]),
                            SubItemName = dr.IsNull("SUBITEMNAME") ?  string.Empty : Convert.ToString(dr["SUBITEMNAME"]),
                            Description = dr.IsNull("DESCRIPTION") ?  string.Empty : Convert.ToString(dr["DESCRIPTION"]),
                            SumInsured = Convert.ToDecimal(dr["SUMINSURED"])
                        };
                        homeitems.Add(homeitem);
                    }
                }
                return new HomeSavedQuotationResponse
                {
                    HomeInsurancePolicy = homepolicy,
                    DomesticHelp = domestichelpmembers,
                    HomeSubItems = homeitems,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get home policy details by document number.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">details fetched for endorsement page or policy page.</param>
        /// <param name="endorsementID">Endorsement id</param>
        /// <returns></returns>
        public HomeSavedQuotationResponse GetOracleRenewHomePolicy(string documentNo, string type, string agentCode)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@OracleDocumentNo",documentNo),
                    new SqlParameter("@Agency", string.Empty),
                    new SqlParameter("@AgentCode", string.Empty),
                };

                DataSet homeds = BKICSQL.eds("SP_GetOracleHomeRenewalByDocumentNo", para);
                HomeInsurancePolicy homepolicy = new HomeInsurancePolicy();
                if (homeds != null && homeds.Tables[0] != null && homeds.Tables[0].Rows.Count > 0)
                {
                    if (homeds.Tables[0].Rows[0].Table.Columns.Contains("IsSystem"))
                    {
                        DataRow homedr = homeds.Tables[0].Rows[0];
                        homepolicy.HomeID = Convert.ToInt64(homedr["HomeID"]);
                        homepolicy.PolicyStartDate = Convert.ToDateTime(homedr["COMMENCEDATE"]);
                        homepolicy.BuildingValue = !homedr.IsNull("BuildingValue") ? Convert.ToDecimal(homedr["BuildingValue"]) : 0;
                        homepolicy.ContentValue = !homedr.IsNull("ContentValue") ? Convert.ToDecimal(homedr["ContentValue"]) : 0;
                        homepolicy.JewelleryValue = !homedr.IsNull("JewelleryValue") ? Convert.ToDecimal(homedr["JewelleryValue"]) : 0;
                        homepolicy.PremiumBeforeDiscount = homedr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(homedr["PremiumBeforeDiscount"]);
                        homepolicy.PremiumAfterDiscount = !homedr.IsNull("PremiumAfterDiscount") ? Convert.ToDecimal(homedr["PremiumAfterDiscount"]) : 0;
                        homepolicy.BuildingAge = !homedr.IsNull("AGEOFBUILDING") ? Convert.ToInt32(homedr["AGEOFBUILDING"]) : 0;
                        homepolicy.FinancierCode = !homedr.IsNull("FINANCECOMPANYID") ? Convert.ToString(homedr["FINANCECOMPANYID"]) : string.Empty;
                        homepolicy.JewelleryCover = !homedr.IsNull("JEWELLERYCOVER") ? Convert.ToString(homedr["JEWELLERYCOVER"]) : string.Empty;
                        homepolicy.IsRiotStrikeDamage = !homedr.IsNull("IsSRCC") ? Convert.ToChar(homedr["IsSRCC"]) : 'N';
                        homepolicy.BuildingNo = !homedr.IsNull("BUILDINGNO") ? Convert.ToString(homedr["BUILDINGNO"]) : string.Empty;
                        homepolicy.FlatNo = !homedr.IsNull("FLATNO") ? Convert.ToString(homedr["FLATNO"]) : string.Empty;
                        homepolicy.RoadNo = !homedr.IsNull("STREETNO") ? Convert.ToString(homedr["STREETNO"]) : string.Empty;
                        homepolicy.BlockNo = !homedr.IsNull("BLOCKNUMBER") ? Convert.ToString(homedr["BLOCKNUMBER"]) : string.Empty;
                        homepolicy.Area = !homedr.IsNull("AREA") ? Convert.ToString(homedr["AREA"]) : string.Empty;
                        homepolicy.ResidanceTypeCode = !homedr.IsNull("ResidenceType") ? Convert.ToString(homedr["ResidenceType"]) : string.Empty;
                        homepolicy.BuildingType = !homedr.IsNull("BuildingType") ? Convert.ToInt32(homedr["BuildingType"]) : 0;
                        homepolicy.NoOfFloors = !homedr.IsNull("NoOfFloors") ? Convert.ToInt32(homedr["NoOfFloors"]) : 0;
                        homepolicy.HouseNo = !homedr.IsNull("HouseNo") ? Convert.ToString(homedr["HouseNo"]) : string.Empty;
                        homepolicy.InsuredName = !homedr.IsNull("INSUREDNAME") ? Convert.ToString(homedr["INSUREDNAME"]) : string.Empty;
                        homepolicy.CPR = !homedr.IsNull("CPR") ? Convert.ToString(homedr["CPR"]) : string.Empty;
                        homepolicy.PolicyExpiryDate = Convert.ToDateTime(homedr["EXPIRYDATE"]);
                        homepolicy.DocumentNo = !homedr.IsNull("DOCUMENTNO") ? Convert.ToString(homedr["DOCUMENTNO"]) : string.Empty;
                        homepolicy.Mobile = !homedr.IsNull("MOBILENUMBER") ? Convert.ToString(homedr["MOBILENUMBER"]) : string.Empty;
                        homepolicy.SumInsured = !homedr.IsNull("SumInsured") ? Convert.ToDecimal(homedr["SumInsured"]) : 0;
                        homepolicy.InsuredCode = !homedr.IsNull("InsuredCode") ? Convert.ToString(homedr["InsuredCode"]) : string.Empty;
                        homepolicy.IsHIR = !homedr.IsNull("IsHIR") ? Convert.ToBoolean(homedr["IsHIR"]) : false;
                        homepolicy.CommisionBeforeDiscount = !homedr.IsNull("CommissionBeforeDiscount") ? Convert.ToDecimal(homedr["CommissionBeforeDiscount"]) : 0;
                        homepolicy.CommissionAfterDiscount = !homedr.IsNull("CommissionAfterDiscount") ? Convert.ToDecimal(homedr["CommissionAfterDiscount"]) : 0;
                        homepolicy.IsSaved = !homedr.IsNull("IsSaved") ? Convert.ToBoolean(homedr["IsSaved"]) : false;
                        homepolicy.IsActivePolicy = !homedr.IsNull("IsActive") ? Convert.ToBoolean(homedr["IsActive"]) : false;
                        homepolicy.PaymentType = !homedr.IsNull("PaymentType") ? Convert.ToString(homedr["PaymentType"]) : string.Empty;
                        homepolicy.AccountNumber = !homedr.IsNull("Accountnumber") ? Convert.ToString(homedr["Accountnumber"]) : string.Empty;
                        homepolicy.Remarks = !homedr.IsNull("Remarks") ? Convert.ToString(homedr["Remarks"]) : string.Empty;
                        homepolicy.HIRStatus = !homedr.IsNull("HIRStatus") ? Convert.ToInt32(homedr["HIRStatus"]) : 0;
                        homepolicy.EndorsementCount = !homedr.IsNull("EndorsementCount") ? Convert.ToInt32(homedr["EndorsementCount"]) : 0;
                        homepolicy.MainClass = !homedr.IsNull("MainClass") ? Convert.ToString(homedr["MainClass"]) : "PPTY";
                        homepolicy.SubClass = !homedr.IsNull("SubClass") ? Convert.ToString(homedr["SubClass"]) : "SH";
                        homepolicy.IsCancelled = !homedr.IsNull("IsCancelled") ? Convert.ToBoolean(homedr["IsCancelled"]) : false;
                        homepolicy.TaxOnPremium = !homedr.IsNull("TAXONPREMIUM") ? Convert.ToDecimal(homedr["TAXONPREMIUM"]) : decimal.Zero;
                        homepolicy.TaxOnCommission = !homedr.IsNull("TAXONCOMMISSION") ? Convert.ToDecimal(homedr["TAXONCOMMISSION"]) : decimal.Zero;
                        homepolicy.OldDocumentNumber = !homedr.IsNull("OldDocumentNumber") ? Convert.ToString(homedr["OldDocumentNumber"]) : string.Empty;
                        homepolicy.IsSavedRenewal = !homedr.IsNull("IsSavedRenewal") ? Convert.ToBoolean(homedr["IsSavedRenewal"]) : false;
                        homepolicy.RenewalCount = !homedr.IsNull("RenewalCount") ? Convert.ToInt32(homedr["RenewalCount"]) : 0;
                        homepolicy.RenewalDelayedDays = !homedr.IsNull("RenewalDelayedDays") ? Convert.ToInt32(homedr["RenewalDelayedDays"]) : 0;
                        homepolicy.ActualRenewalStartDate = !homedr.IsNull("ActualRenewalStartDate") ? Convert.ToDateTime(homedr["ActualRenewalStartDate"]) : (DateTime?)null;

                        if (homeds != null && homeds.Tables[1] != null && homeds.Tables[1].Rows.Count > 0)
                        {
                            DataRow questiondr = homeds.Tables[1].Rows[0];

                            // homepolicy.BuildingAge =!questiondr.IsNull("BuildingAge")? Convert.ToInt32(questiondr["BuildingAge"]):0;
                            homepolicy.IsPropertyMortgaged = !questiondr.IsNull("PropertyMortgaged") ? (Convert.ToString(questiondr["PropertyMortgaged"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsPropertyInConnectionTrade = !questiondr.IsNull("PropertyInConnectionTrade") ? (Convert.ToString(questiondr["PropertyInConnectionTrade"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsSafePropertyInsured = !questiondr.IsNull("SafePropertyInsured") ? (Convert.ToString(questiondr["SafePropertyInsured"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsRiotStrikeDamage = !questiondr.IsNull("RiotStrikeDamage") ? (Convert.ToString(questiondr["RiotStrikeDamage"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsPropertyCoveredOtherInsurance = !questiondr.IsNull("PropertyCoveredOtherInsurance") ? (Convert.ToString(questiondr["PropertyCoveredOtherInsurance"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsPropertyInsuredSustainedAnyLoss = !questiondr.IsNull("PropertyInsuredSustainedAnyLoss") ? (Convert.ToString(questiondr["PropertyInsuredSustainedAnyLoss"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsPropertyUndergoingConstruction = !questiondr.IsNull("PropertyUndergoingConstruction") ? (Convert.ToString(questiondr["PropertyUndergoingConstruction"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsSingleItemAboveContents = !questiondr.IsNull("SingleItemAboveContents") ? (Convert.ToString(questiondr["SingleItemAboveContents"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsRequireDomestic = !questiondr.IsNull("RequireDomestic") ? (Convert.ToString(questiondr["RequireDomestic"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.IsJointOwnership = !questiondr.IsNull("IsJointOwnership") ? (Convert.ToString(questiondr["IsJointOwnership"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                            homepolicy.JointOwnerName = !questiondr.IsNull("JointOwnerName") ? Convert.ToString(questiondr["JointOwnerName"]) : "";
                            homepolicy.NamePolicyReasonSeekingReasons = !questiondr.IsNull("PolicyReasonSeekingReasons") ? Convert.ToString(questiondr["PolicyReasonSeekingReasons"]) : "";
                            homepolicy.FinancierCode = !questiondr.IsNull("FinancierCode") ? Convert.ToString(questiondr["FinancierCode"]) : "";
                            homepolicy.JewelleryCoverType = !questiondr.IsNull("JewelleryCoverType") ? Convert.ToString(questiondr["JewelleryCoverType"]) : "";
                        }

                        List<HomeDomesticHelp> domestichelpmembers = new List<HomeDomesticHelp>();
                        if (homeds != null && homeds.Tables[2] != null && homeds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr in homeds.Tables[2].Rows)
                            {
                                HomeDomesticHelp homedomestic = new HomeDomesticHelp()
                                {
                                    HomeSID = Convert.ToInt32(dr["HOMEDID"]),
                                    MemberSerialNo = dr.IsNull("MEMBERSERIALNO") ? 0 : Convert.ToInt32(dr["MEMBERSERIALNO"]),
                                    Name = Convert.ToString(dr["NAME"]),
                                    Sex = dr.IsNull("SEX") ? '\0' : Convert.ToChar(dr["SEX"]),
                                    CPR = Convert.ToString(dr["CPRNUMBER"]),
                                    Title = Convert.ToString(dr["TITLE"]),
                                    DOB = Convert.ToDateTime(dr["DATEOFBIRTH"]),
                                    Occupation = dr.IsNull("OCCUPATION") ? "" : Convert.ToString(dr["OCCUPATION"]),
                                    Nationality = dr.IsNull("NATIONALITY") ? "" : Convert.ToString(dr["NATIONALITY"]),
                                    Age = !dr.IsNull("AGE") ? Convert.ToInt32(dr["AGE"]) : 0
                                };
                                domestichelpmembers.Add(homedomestic);
                            }
                        }
                        homepolicy.NoOfDomesticWorker = domestichelpmembers.Count;

                        List<HomeSubItems> homeitems = new List<HomeSubItems>();
                        if (homeds != null && homeds.Tables[3] != null && homeds.Tables[3].Rows.Count > 0)
                        {
                            foreach (DataRow dr in homeds.Tables[3].Rows)
                            {
                                HomeSubItems homeitem = new HomeSubItems()
                                {
                                    HomeSID = Convert.ToInt32(dr["HOMESID"]),
                                    HomeID = Convert.ToInt64(dr["HOMEID"]),
                                    SubItemName = dr.IsNull("SUBITEMNAME") ?  string.Empty : Convert.ToString(dr["SUBITEMNAME"]),
                                    Description = dr.IsNull("DESCRIPTION") ? string.Empty : Convert.ToString(dr["DESCRIPTION"]),
                                    SumInsured = Convert.ToDecimal(dr["SUMINSURED"])
                                };
                                homeitems.Add(homeitem);
                            }
                        }

                        return new HomeSavedQuotationResponse
                        {
                            HomeInsurancePolicy = homepolicy,
                            DomesticHelp = domestichelpmembers,
                            HomeSubItems = homeitems,
                            IsTransactionDone = true
                        };
                    }
                    else
                    {
                        if (homeds != null && homeds.Tables[0] != null && homeds.Tables[0].Rows.Count > 0)
                        {
                            DataRow homedr = homeds.Tables[0].Rows[0];

                            homepolicy.ResidanceTypeCode = !homedr.IsNull("ADDRESS1") ? Convert.ToString(homedr["ADDRESS1"]) : string.Empty;
                            homepolicy.PolicyStartDate = Convert.ToDateTime(homedr["COMMENCEDATE"]);
                            //homepolicy.BuildingValue = !homedr.IsNull("BuildingValue") ? Convert.ToDecimal(homedr["BuildingValue"]) : 0;
                            //homepolicy.ContentValue = !homedr.IsNull("ContentValue") ? Convert.ToDecimal(homedr["ContentValue"]) : 0;
                            //homepolicy.PremiumBeforeDiscount = homedr.IsNull("PremiumBeforeDiscount") ? 0 : Convert.ToDecimal(homedr["PremiumBeforeDiscount"]);
                            //homepolicy.PremiumAfterDiscount = !homedr.IsNull("PremiumAfterDiscount") ? Convert.ToDecimal(homedr["PremiumAfterDiscount"]) : 0;
                            homepolicy.BuildingAge = !homedr.IsNull("AGEOFBUILDING") ? Convert.ToInt32(homedr["AGEOFBUILDING"]) : 0;
                            homepolicy.FinancierCode = !homedr.IsNull("FINANCECOMPANYID") ? Convert.ToString(homedr["FINANCECOMPANYID"]) : string.Empty;
                            homepolicy.JewelleryCover = !homedr.IsNull("JEWELLERYCOVER") ? Convert.ToString(homedr["JEWELLERYCOVER"]) : string.Empty;
                            homepolicy.IsRiotStrikeDamage = !homedr.IsNull("RSMDCOVER") ? Convert.ToChar(homedr["RSMDCOVER"]) : 'N';
                            homepolicy.BuildingNo = !homedr.IsNull("BUILDINGNO") ? Convert.ToString(homedr["BUILDINGNO"]) : string.Empty;
                            homepolicy.FlatNo = !homedr.IsNull("FLATNO") ? Convert.ToString(homedr["FLATNO"]) : string.Empty;
                            homepolicy.RoadNo = !homedr.IsNull("STREETNO") ? Convert.ToString(homedr["STREETNO"]) : string.Empty;
                            homepolicy.BlockNo = !homedr.IsNull("BLOCKNUMBER") ? Convert.ToString(homedr["BLOCKNUMBER"]) : string.Empty;
                            homepolicy.Area = !homedr.IsNull("CITYCODE") ? Convert.ToString(homedr["CITYCODE"]) : string.Empty;                           
                            homepolicy.BuildingType = homepolicy.ResidanceTypeCode == "H" ? 1 : 2;
                            homepolicy.NoOfFloors = 0; //!homedr.IsNull("NoOfFloors") ? Convert.ToInt32(homedr["NoOfFloors"]) : 0;
                            homepolicy.HouseNo = !homedr.IsNull("FLATNO") ? Convert.ToString(homedr["FLATNO"]) : string.Empty;
                            homepolicy.InsuredName = !homedr.IsNull("INSUREDNAME") ? Convert.ToString(homedr["INSUREDNAME"]) : string.Empty;
                            homepolicy.InsuredCode = !homedr.IsNull("InsuredCode") ? Convert.ToString(homedr["InsuredCode"]) : string.Empty;
                            homepolicy.CPR = !homedr.IsNull("CPR") ? Convert.ToString(homedr["CPR"]) : string.Empty;
                            homepolicy.PolicyExpiryDate = Convert.ToDateTime(homedr["EXPIRYDATE"]);
                            homepolicy.DocumentNo = !homedr.IsNull("DOCUMENTNO") ? Convert.ToString(homedr["DOCUMENTNO"]) : string.Empty;
                            homepolicy.Mobile = !homedr.IsNull("MOBILENUMBER") ? Convert.ToString(homedr["MOBILENUMBER"]) : string.Empty;
                            homepolicy.SumInsured = !homedr.IsNull("SumInsured") ? Convert.ToDecimal(homedr["SumInsured"]) : 0;
                            homepolicy.MainClass = !homedr.IsNull("MainClass") ? Convert.ToString(homedr["MainClass"]) : "PPTY";
                            homepolicy.SubClass = !homedr.IsNull("SubClass") ? Convert.ToString(homedr["SubClass"]) : "SH";
                            homepolicy.OldDocumentNumber = string.Empty;
                            homepolicy.RenewalCount = !homedr.IsNull("RenewalCount") ? Convert.ToInt32(homedr["RenewalCount"]) : 0;
                        }
                        if (homeds != null && homeds.Tables[1] != null && homeds.Tables[1].Rows.Count > 0)
                        {
                            var HomeItems = homeds.Tables[1];
                            foreach (DataRow dr in HomeItems.AsEnumerable())
                            {
                                string itemCode = dr.IsNull("ITEMCODE") ? string.Empty : Convert.ToString(dr["ITEMCODE"]);
                                if (itemCode == "PL-01")
                                {
                                    homepolicy.BuildingValue = dr.IsNull("SUMINSURED") ? decimal.Zero : Convert.ToDecimal(dr["SUMINSURED"]);
                                }
                                else if (itemCode == "PL-02")
                                {
                                    homepolicy.ContentValue = dr.IsNull("SUMINSURED") ? decimal.Zero : Convert.ToDecimal(dr["SUMINSURED"]);
                                }
                                else if (itemCode == "PL-04")
                                {
                                    homepolicy.JewelleryValue = dr.IsNull("SUMINSURED") ? decimal.Zero : Convert.ToDecimal(dr["SUMINSURED"]);
                                }
                                else if (itemCode == "DOMESTIC COVER")
                                {
                                    //homepolicy.DOME = dr.IsNull("SUMINSURED") ? decimal.Zero : Convert.ToDecimal(dr["SUMINSURED"]);
                                }
                            }
                        }
                        List<HomeDomesticHelp> domestichelpmembers = new List<HomeDomesticHelp>();
                        if (homeds != null && homeds.Tables[2] != null && homeds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr in homeds.Tables[2].Rows)
                            {
                                HomeDomesticHelp homedomestic = new HomeDomesticHelp()
                                {
                                    MemberSerialNo = dr.IsNull("MEMBERSERIALNO") ? 0 : Convert.ToInt32(dr["MEMBERSERIALNO"]),
                                    Name = Convert.ToString(dr["NAME"]),
                                    Sex = dr.IsNull("SEX") ? '\0' : Convert.ToChar(dr["SEX"]),
                                    CPR = Convert.ToString(dr["CPRNUMBER"]),
                                    Title = Convert.ToString(dr["TITLE"]),
                                    DOB = Convert.ToDateTime(dr["DATEOFBIRTH"]),
                                    Occupation = "", //dr.IsNull("OCCUPATION") ? "" : Convert.ToString(dr["OCCUPATION"]),
                                    Nationality = "", // dr.IsNull("NATIONALITY") ? "" : Convert.ToString(dr["NATIONALITY"]),
                                    Age = !dr.IsNull("AGE") ? Convert.ToInt32(dr["AGE"]) : 0
                                };
                                domestichelpmembers.Add(homedomestic);
                            }
                        }
                        homepolicy.NoOfDomesticWorker = domestichelpmembers.Count;

                        List<HomeSubItems> homeitems = new List<HomeSubItems>();
                        if (homeds != null && homeds.Tables[3] != null && homeds.Tables[3].Rows.Count > 0)
                        {
                            foreach (DataRow dr in homeds.Tables[3].Rows)
                            {
                                HomeSubItems homeitem = new HomeSubItems()
                                {
                                    SubItemName = dr.IsNull("SUBITEMNAME") ? string.Empty : Convert.ToString(dr["SUBITEMNAME"]),
                                    Description = dr.IsNull("DESCRIPTION") ? string.Empty : Convert.ToString(dr["DESCRIPTION"]) ,
                                    SumInsured = Convert.ToDecimal(dr["SUMINSURED"])
                                };
                                homeitems.Add(homeitem);
                            }
                        }
                        if (homeds != null && homeds.Tables[4] != null && homeds.Tables[4].Rows.Count > 0)
                        {
                            DataTable questiondr = homeds.Tables[4];

                            foreach (DataRow dr in questiondr.Rows)
                            {
                                string questinarieCode = dr.IsNull("CODE") ? string.Empty : Convert.ToString(dr["CODE"]);
                                if (questinarieCode == "QST_SH_013")
                                {
                                    homepolicy.JewelleryCoverType = !dr.IsNull("REMARKS") ? Convert.ToString(dr["REMARKS"]) : "";
                                }
                                else if (questinarieCode == "QST_SH_011")
                                {
                                    homepolicy.IsPropertyUndergoingConstruction = !dr.IsNull("ANSWER") ? (Convert.ToString(dr["ANSWER"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                                }
                                else if (questinarieCode == "QST_SH_010")
                                {
                                    homepolicy.IsPropertyInsuredSustainedAnyLoss = !dr.IsNull("ANSWER") ? (Convert.ToString(dr["ANSWER"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                                }
                                else if (questinarieCode == "QST_SH_009")
                                {
                                    homepolicy.IsPropertyCoveredOtherInsurance = !dr.IsNull("ANSWER") ? (Convert.ToString(dr["ANSWER"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                                }
                                else if (questinarieCode == "QST_SH_012")
                                {
                                    homepolicy.IsSingleItemAboveContents = !dr.IsNull("ANSWER") ? (Convert.ToString(dr["ANSWER"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                                }
                                else if (questinarieCode == "QST_SH_007")
                                {
                                    homepolicy.IsRiotStrikeDamage = !dr.IsNull("ANSWER") ? (Convert.ToString(dr["ANSWER"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                                }
                                else if (questinarieCode == "QST_SH_006")
                                {
                                    homepolicy.IsSafePropertyInsured = !dr.IsNull("ANSWER") ? (Convert.ToString(dr["ANSWER"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                                }
                                else if (questinarieCode == "QST_SH_004")
                                {
                                    homepolicy.IsPropertyInConnectionTrade = !dr.IsNull("ANSWER") ? (Convert.ToString(dr["ANSWER"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                                }
                                else if (questinarieCode == "QST_SH_014")
                                {
                                    homepolicy.IsRequireDomestic = !dr.IsNull("ANSWER") ? (Convert.ToString(dr["ANSWER"]).ToUpper() == "YES" ? 'Y' : 'N') : 'N';
                                }
                            }
                        }
                        return new HomeSavedQuotationResponse
                        {
                            HomeInsurancePolicy = homepolicy,
                            DomesticHelp = domestichelpmembers,
                            HomeSubItems = homeitems,
                            IsTransactionDone = true
                        };
                    }
                }
                return new HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = "This policy is not eligible for renewal !"
                };
            }
            catch (Exception ex)
            {
                return new HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get home policies by agency.
        /// </summary>
        /// <param name="req">Home policy request.</param>
        /// <returns>List of home policies by agency.</returns>
        public AgencyHomePolicyResponse GetHomeAgencyPolicy(AgencyHomeRequest req)
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
                DataTable dt = BKICSQL.edt(StoredProcedures.HomeInsuranceSP.GetHomeAgencyPolicy, para);
                List<AgencyHomePolicy> agencyHomePolicy = new List<AgencyHomePolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyHomePolicy();
                        res.HomeID = Convert.ToInt64(dr["HOMEID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        res.RenewalCount = dr.IsNull("RENEWALCOUNT") ? 0 : Convert.ToInt32(dr["RENEWALCOUNT"]);
                        res.DocumentRenewalNo = res.RenewalCount + "-" + res.DocumentNo;
                        agencyHomePolicy.Add(res);
                    }
                }
                return new AgencyHomePolicyResponse
                {
                    AgencyHomePolicies = agencyHomePolicy,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AgencyHomePolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// Get home policies by certain CPR.
        /// </summary>
        /// <param name="req">Agency home request.</param>
        /// <returns>List of home policies by CPR.</returns>
        public AgencyHomePolicyResponse GetHomeAgencyPolicyByCPR(AgencyHomeRequest req)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                new SqlParameter("@Agency",req.Agency??string.Empty),
                new SqlParameter("@AgentCode",req.AgentCode??string.Empty),
                new SqlParameter("@Type",req.Type??string.Empty),
                new SqlParameter("@CPR",req.CPR??string.Empty),
                new SqlParameter("@isEndorsement", false)
                };
                DataTable dt = BKICSQL.edt(StoredProcedures.MotorInsuranceSP.GETPoliciesByTypeByCPR, para);
                List<AgencyHomePolicy> agencyHomePolicy = new List<AgencyHomePolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyHomePolicy();
                        res.HomeID = Convert.ToInt64(dr["homeID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        res.RenewalCount = dr.IsNull("RENEWALCOUNT") ? 0 : Convert.ToInt32(dr["RENEWALCOUNT"]);
                        res.DocumentRenewalNo = res.RenewalCount + "-" + res.DocumentNo;
                        agencyHomePolicy.Add(res);
                    }
                }
                return new AgencyHomePolicyResponse
                {
                    IsTransactionDone = true,
                    AgencyHomePolicies = agencyHomePolicy
                };
            }
            catch (Exception ex)
            {
                return new AgencyHomePolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }


        /// <summary>
        /// Get all home policies by agency for endorsement page, all policies should be ACTIVE.
        /// Used in every  home endorsement page search by policy number.
        /// </summary>
        /// <param name="req">Agency home request.</param>
        /// <returns>List of active home policies for an endorsement.</returns>        
        public AgencyHomePolicyResponse GetHomePoliciesEndorsement(AgencyHomeRequest req)
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
                DataTable dt = BKICSQL.edt(StoredProcedures.HomeInsuranceSP.GetHomePoliciesEndorsement, para);
                List<AgencyHomePolicy> agencyHomePolicy = new List<AgencyHomePolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyHomePolicy();
                        res.HomeID = Convert.ToInt64(dr["HOMEID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        res.RenewalCount = dr.IsNull("RENEWALCOUNT") ? 0 : Convert.ToInt32(dr["RENEWALCOUNT"]);
                        res.DocumentRenewalNo = res.RenewalCount + "-" + res.DocumentNo;
                        agencyHomePolicy.Add(res);
                    }
                }
                return new AgencyHomePolicyResponse
                {
                    AgencyHomePolicies = agencyHomePolicy,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AgencyHomePolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }



        /// <summary>
        /// Get home policies by agency.
        /// </summary>
        /// <param name="req">Home policy request.</param>
        /// <returns>List of home policies by agency.</returns>
        public AgencyHomePolicyResponse GetOracleHomeRenewalPolicies(AgencyHomeRequest req)
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
                DataTable dt = BKICSQL.edt(StoredProcedures.HomeInsuranceSP.GetOracleHomeRenewalPolicies, para);
                List<AgencyHomePolicy> agencyHomePolicy = new List<AgencyHomePolicy>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var res = new AgencyHomePolicy();
                        //res.HomeID = Convert.ToInt64(dr["HOMEID"]);
                        res.DocumentNo = Convert.ToString(dr["DOCUMENTNO"]);
                        res.InsuredCode = Convert.ToString(dr["INSUREDCODE"]);
                        res.RenewalCount = dr.IsNull("RENEWALCOUNT") ? 0 : Convert.ToInt32(dr["RENEWALCOUNT"]);
                        res.DocumentRenewalNo = res.RenewalCount + "-" + res.DocumentNo;
                        agencyHomePolicy.Add(res);
                    }
                }
                return new AgencyHomePolicyResponse
                {
                    AgencyHomePolicies = agencyHomePolicy,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AgencyHomePolicyResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        #region Unused method        
        #endregion Unused method
    }
}