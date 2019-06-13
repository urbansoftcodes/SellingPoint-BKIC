using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.WebAPI.Framework;
using KBIC.DL.BL.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLO = BKIC.SellingPoint.DL.BO;
using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;
using URI = BKIC.SellingPoint.DTO.Constants;

namespace KBIC.WebAPI.Controllers
{
    /// <summary>
    /// Home policy methods.
    /// </summary>
    public class HomeInsuranceController : ApiController
    {
        private readonly IHomeInsurance _homeInsuranceRepository;
        private readonly AutoMapper.IMapper _mapper;

        public HomeInsuranceController(IHomeInsurance homeInsurance)
        {
            _homeInsuranceRepository = homeInsurance;
            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetHomeAutoMapper();
        }

        /// <summary>
        /// Get quote for the home policy.
        /// </summary>
        /// <param name="homeQuote">home quote request.</param>
        /// <returns>Premium and commission.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.HomeURI.GetQuote)]
        public RR.HomeInsuranceQuoteResponse GetQuote(RR.HomeInsuranceQuote homeQuote)
        {
            try
            {
                BLO.HomeInsuranceQuote homeInsuranceQuote = _mapper.Map<RR.HomeInsuranceQuote, BLO.HomeInsuranceQuote>(homeQuote);
                BLO.HomeInsuranceQuoteResponse res = _homeInsuranceRepository.GetHomeInsuranceQuote(homeInsuranceQuote);
                return _mapper.Map<BLO.HomeInsuranceQuoteResponse, RR.HomeInsuranceQuoteResponse>(res);
            }
            catch (Exception ex)
            {
                return new RR.HomeInsuranceQuoteResponse
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
        //[ApiAuthorize]
        [HttpPost]
        [Route(URI.HomeURI.GetHomeAgencyPolicy)]
        public RR.AgencyHomePolicyResponse GetHomeAgencyPolicy(RR.AgencyHomeRequest request)
        {
            try
            {
                BLO.AgencyHomeRequest home = _mapper.Map<RR.AgencyHomeRequest, BLO.AgencyHomeRequest>(request);
                BLO.AgencyHomePolicyResponse result = _homeInsuranceRepository.GetHomeAgencyPolicy(home);
                return _mapper.Map<BLO.AgencyHomePolicyResponse, RR.AgencyHomePolicyResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyHomePolicyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

        }
        /// <summary>
        /// Get home policies by certain CPR.
        /// </summary>
        /// <param name="request">Agency home request.</param>
        /// <returns>List of home policies by CPR.</returns>
        [HttpPost]
        [Route(URI.HomeURI.GetHomePoliciesByCPR)]
        public RR.AgencyHomePolicyResponse GetHomePoliciesByTypeByCPR(RR.AgencyHomeRequest request)
        {
            try
            {
                BLO.AgencyHomeRequest home = _mapper.Map<RR.AgencyHomeRequest, BLO.AgencyHomeRequest>(request);
                BLO.AgencyHomePolicyResponse result = _homeInsuranceRepository.GetHomeAgencyPolicyByCPR(home);
                return _mapper.Map<BLO.AgencyHomePolicyResponse, RR.AgencyHomePolicyResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyHomePolicyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

        }
        /// <summary>
        /// Get home policy by document number
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isendorsement"></param>
        /// <param name="endorsementid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(URI.HomeURI.GetHomeSavedQuoteDocumentNo)]
        public RR.HomeSavedQuotationResponse GetHomeSavedQuotationPolicy(string documentNo, string type, string agentCode,
                                             bool isendorsement, long endorsementid, int renewalCount)
        {
            try
            {
                BLO.HomeSavedQuotationResponse result = _homeInsuranceRepository.GetSavedQuotationPolicy(documentNo, "", agentCode,
                                                        isendorsement, endorsementid, renewalCount);
                return _mapper.Map<BLO.HomeSavedQuotationResponse, RR.HomeSavedQuotationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get home policy by document number
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isendorsement"></param>
        /// <param name="endorsementid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(URI.HomeURI.GetHomeRenewalPolicyByDocNo)]
        public RR.HomeSavedQuotationResponse GetHomeRenewalPolicy(string documentNo, string type, string agentCode,
                                             int renewalCount)
        {
            try
            {
                BLO.HomeSavedQuotationResponse result = _homeInsuranceRepository.GetRenewalHomePolicy(documentNo, type, agentCode, renewalCount);
                return _mapper.Map<BLO.HomeSavedQuotationResponse, RR.HomeSavedQuotationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get home policy by document number
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isendorsement"></param>
        /// <param name="endorsementid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(URI.HomeURI.GetOracleHomeRenewalPolicyByDocNo)]
        public RR.HomeSavedQuotationResponse GetHomeOracleRenewalPolicy(string documentNo, string agency, string agentCode)
        {
            try
            {
                BLO.HomeSavedQuotationResponse result = _homeInsuranceRepository.GetOracleRenewHomePolicy(documentNo, agency, agentCode);
                return _mapper.Map<BLO.HomeSavedQuotationResponse, RR.HomeSavedQuotationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Post the home policy.
        /// </summary>
        /// <param name="homePolicyDetails">Home policy details.</param>
        /// <returns>Posted home id, document number and hir status.</returns>
        // [ApiAuthorize]
        [HttpPost]
        [Route(URI.HomeURI.PostPolicy)]
        public RR.HomeInsurancePolicyResponse PostPolicy(RR.HomeInsurancePolicyDetails homePolicyDetails)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    BLO.HomeInsurancePolicyDetails homeInsurance = new BLO.HomeInsurancePolicyDetails();

                    homeInsurance.HomeInsurancePolicy.HomeID = homePolicyDetails.HomeInsurancePolicy.HomeID;
                    homeInsurance.HomeInsurancePolicy.InsuredCode = homePolicyDetails.HomeInsurancePolicy.InsuredCode;
                    homeInsurance.HomeInsurancePolicy.InsuredName = homePolicyDetails.HomeInsurancePolicy.InsuredName;
                    homeInsurance.HomeInsurancePolicy.CPR = homePolicyDetails.HomeInsurancePolicy.CPR;
                    homeInsurance.HomeInsurancePolicy.Agency = homePolicyDetails.HomeInsurancePolicy.Agency;
                    homeInsurance.HomeInsurancePolicy.AgentCode = homePolicyDetails.HomeInsurancePolicy.AgentCode;
                    homeInsurance.HomeInsurancePolicy.AgentBranch = homePolicyDetails.HomeInsurancePolicy.AgentBranch;
                    homeInsurance.HomeInsurancePolicy.MainClass = homePolicyDetails.HomeInsurancePolicy.MainClass;
                    homeInsurance.HomeInsurancePolicy.SubClass = homePolicyDetails.HomeInsurancePolicy.SubClass;
                    homeInsurance.HomeInsurancePolicy.PolicyStartDate = homePolicyDetails.HomeInsurancePolicy.PolicyStartDate;
                    homeInsurance.HomeInsurancePolicy.BuildingValue = homePolicyDetails.HomeInsurancePolicy.BuildingValue;
                    homeInsurance.HomeInsurancePolicy.ContentValue = homePolicyDetails.HomeInsurancePolicy.ContentValue;
                    homeInsurance.HomeInsurancePolicy.JewelleryValue = homePolicyDetails.HomeInsurancePolicy.JewelleryValue;
                    homeInsurance.HomeInsurancePolicy.PremiumAfterDiscount = homePolicyDetails.HomeInsurancePolicy.PremiumAfterDiscount;
                    homeInsurance.HomeInsurancePolicy.PremiumBeforeDiscount = homePolicyDetails.HomeInsurancePolicy.PremiumBeforeDiscount;
                    homeInsurance.HomeInsurancePolicy.BuildingAge = homePolicyDetails.HomeInsurancePolicy.BuildingAge;
                    homeInsurance.HomeInsurancePolicy.IsPropertyMortgaged = homePolicyDetails.HomeInsurancePolicy.IsPropertyMortgaged;
                    homeInsurance.HomeInsurancePolicy.FinancierCode = homePolicyDetails.HomeInsurancePolicy.FinancierCode;
                    homeInsurance.HomeInsurancePolicy.IsSafePropertyInsured = homePolicyDetails.HomeInsurancePolicy.IsSafePropertyInsured;
                    homeInsurance.HomeInsurancePolicy.JewelleryCover = homePolicyDetails.HomeInsurancePolicy.JewelleryCover;
                    homeInsurance.HomeInsurancePolicy.IsRiotStrikeDamage = homePolicyDetails.HomeInsurancePolicy.IsRiotStrikeDamage;
                    homeInsurance.HomeInsurancePolicy.IsJointOwnership = homePolicyDetails.HomeInsurancePolicy.IsJointOwnership;
                    homeInsurance.HomeInsurancePolicy.JointOwnerName = homePolicyDetails.HomeInsurancePolicy.JointOwnerName;
                    homeInsurance.HomeInsurancePolicy.IsPropertyInConnectionTrade = homePolicyDetails.HomeInsurancePolicy.IsPropertyInConnectionTrade;
                    homeInsurance.HomeInsurancePolicy.IsPropertyCoveredOtherInsurance = homePolicyDetails.HomeInsurancePolicy.IsPropertyCoveredOtherInsurance;
                    homeInsurance.HomeInsurancePolicy.NamePolicyReasonSeekingReasons = homePolicyDetails.HomeInsurancePolicy.NamePolicyReasonSeekingReasons;
                    homeInsurance.HomeInsurancePolicy.IsPropertyInsuredSustainedAnyLoss = homePolicyDetails.HomeInsurancePolicy.IsPropertyInsuredSustainedAnyLoss;
                    homeInsurance.HomeInsurancePolicy.IsPropertyUndergoingConstruction = homePolicyDetails.HomeInsurancePolicy.IsPropertyUndergoingConstruction;
                    homeInsurance.HomeInsurancePolicy.IsSingleItemAboveContents = homePolicyDetails.HomeInsurancePolicy.IsSingleItemAboveContents;
                    homeInsurance.HomeInsurancePolicy.BuildingNo = homePolicyDetails.HomeInsurancePolicy.BuildingNo;
                    homeInsurance.HomeInsurancePolicy.FlatNo = homePolicyDetails.HomeInsurancePolicy.FlatNo;
                    homeInsurance.HomeInsurancePolicy.RoadNo = homePolicyDetails.HomeInsurancePolicy.RoadNo;
                    homeInsurance.HomeInsurancePolicy.Area = homePolicyDetails.HomeInsurancePolicy.Area;
                    homeInsurance.HomeInsurancePolicy.BlockNo = homePolicyDetails.HomeInsurancePolicy.BlockNo;
                    homeInsurance.HomeInsurancePolicy.HouseNo = homePolicyDetails.HomeInsurancePolicy.HouseNo;
                    homeInsurance.HomeInsurancePolicy.BuildingType = homePolicyDetails.HomeInsurancePolicy.BuildingType;
                    homeInsurance.HomeInsurancePolicy.NoOfFloors = homePolicyDetails.HomeInsurancePolicy.NoOfFloors;
                    homeInsurance.HomeInsurancePolicy.FFPNumber = homePolicyDetails.HomeInsurancePolicy.FFPNumber;
                    homeInsurance.HomeInsurancePolicy.IsRequireDomestic = homePolicyDetails.HomeInsurancePolicy.IsRequireDomestic;
                    homeInsurance.HomeInsurancePolicy.NoOfDomesticWorker = homePolicyDetails.HomeInsurancePolicy.NoOfDomesticWorker;
                    homeInsurance.HomeInsurancePolicy.CreatedBy = homePolicyDetails.HomeInsurancePolicy.CreatedBy;
                    homeInsurance.HomeInsurancePolicy.AuthorizedBy = homePolicyDetails.HomeInsurancePolicy.IsActivePolicy ? homePolicyDetails.HomeInsurancePolicy.CreatedBy : 0;
                    homeInsurance.HomeInsurancePolicy.Mobile = homePolicyDetails.HomeInsurancePolicy.Mobile;
                    homeInsurance.HomeInsurancePolicy.AccountNumber = homePolicyDetails.HomeInsurancePolicy.AccountNumber;
                    homeInsurance.HomeInsurancePolicy.PaymentType = homePolicyDetails.HomeInsurancePolicy.PaymentType;
                    homeInsurance.HomeInsurancePolicy.Remarks = homePolicyDetails.HomeInsurancePolicy.Remarks;
                    homeInsurance.HomeInsurancePolicy.IsSaved = homePolicyDetails.HomeInsurancePolicy.IsSaved;
                    homeInsurance.HomeInsurancePolicy.IsActivePolicy = homePolicyDetails.HomeInsurancePolicy.IsActivePolicy;
                    homeInsurance.HomeInsurancePolicy.UserChangedPremium = homePolicyDetails.HomeInsurancePolicy.UserChangedPremium;
                    homeInsurance.HomeInsurancePolicy.PremiumAfterDiscount = homePolicyDetails.HomeInsurancePolicy.PremiumAfterDiscount;
                    homeInsurance.HomeInsurancePolicy.CommissionAfterDiscount = homePolicyDetails.HomeInsurancePolicy.CommissionAfterDiscount;
                    homeInsurance.HomeInsurancePolicy.RenewalCount = homePolicyDetails.HomeInsurancePolicy.RenewalCount;
                    homeInsurance.HomeInsurancePolicy.DocumentNo = homePolicyDetails.HomeInsurancePolicy.DocumentNo;
                    homeInsurance.HomeInsurancePolicy.IsRenewal = homePolicyDetails.HomeInsurancePolicy.IsRenewal;
                    homeInsurance.HomeInsurancePolicy.OldDocumentNumber = homePolicyDetails.HomeInsurancePolicy.OldDocumentNumber;
                    homeInsurance.HomeInsurancePolicy.RenewalDelayedDays = homePolicyDetails.HomeInsurancePolicy.RenewalDelayedDays;
                    homeInsurance.HomeInsurancePolicy.ActualRenewalStartDate = homePolicyDetails.HomeInsurancePolicy.ActualRenewalStartDate;

                    homeInsurance.HomeSubItemsdt = GetHomeSubItems(homePolicyDetails, homeInsurance);
                    homeInsurance.HomeDomesticHelpdt = GetHomeDomesticHelps(homePolicyDetails, homeInsurance);

                    BLO.HomeInsurancePolicyResponse res = _homeInsuranceRepository.PostHomeInsurancePolicy(homeInsurance);

                    return _mapper.Map<BLO.HomeInsurancePolicyResponse, RR.HomeInsurancePolicyResponse>(res);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.HomeInsurancePolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch (Exception ex)
            {
                return new RR.HomeInsurancePolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }


        /// <summary>
        /// Get all home policies by agency for endorsement page, all policies should be ACTIVE.
        /// Used in every  home endorsement page search by policy number.
        /// </summary>
        /// <param name="req">Agency home request.</param>
        /// <returns>List of active home policies for an endorsement.</returns>
        [HttpPost]
        [Route(URI.HomeURI.GetHomePoliciesEndorsement)]
        public RR.AgencyHomePolicyResponse GetHomeEndorsemntPolicies(RR.AgencyHomeRequest request)
        {
            try
            {
                BLO.AgencyHomeRequest home = _mapper.Map<RR.AgencyHomeRequest, BLO.AgencyHomeRequest>(request);
                BLO.AgencyHomePolicyResponse result = _homeInsuranceRepository.GetHomePoliciesEndorsement(home);
                return _mapper.Map<BLO.AgencyHomePolicyResponse, RR.AgencyHomePolicyResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyHomePolicyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

        }

        /// <summary>
        /// Get all eligible oracle home policies for renewal by agency.
        /// </summary>
        /// <param name="req">Agency home request.</param>
        /// <returns>List of renewalble oracle home policies by agency.</returns>
        //[ApiAuthorize]
        [HttpPost]
        [Route(URI.HomeURI.GetOracleHomeRenewalPolicies)]
        public RR.AgencyHomePolicyResponse GetOracleHomeRenewalPolicies(RR.AgencyHomeRequest request)
        {
            try
            {
                BLO.AgencyHomeRequest home = _mapper.Map<RR.AgencyHomeRequest, BLO.AgencyHomeRequest>(request);
                BLO.AgencyHomePolicyResponse result = _homeInsuranceRepository.GetOracleHomeRenewalPolicies(home);
                return _mapper.Map<BLO.AgencyHomePolicyResponse, RR.AgencyHomePolicyResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyHomePolicyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

        }

        #region Private Methods
        private DataTable GetHomeSubItems(RR.HomeInsurancePolicyDetails homePolicyDetails, BLO.HomeInsurancePolicyDetails homeInsurance)
        {
            DataTable subitems = new DataTable();

            subitems.Columns.Add("HOMEID", typeof(Int32));
            subitems.Columns.Add("LINKID", typeof(string));
            subitems.Columns.Add("DOCUMENTNO", typeof(string));
            subitems.Columns.Add("ITEMSERIALNO", typeof(Int32));
            subitems.Columns.Add("ITEMCODE", typeof(string));
            subitems.Columns.Add("ITEMNAME", typeof(string));
            subitems.Columns.Add("SUBITEMSERIALNO", typeof(Int32));
            subitems.Columns.Add("SUBITEMCODE", typeof(string));
            subitems.Columns.Add("SUBITEMNAME", typeof(string));
            subitems.Columns.Add("DESCRIPTION", typeof(string));
            subitems.Columns.Add("SUMINSURED", typeof(decimal));
            subitems.Columns.Add("REMARKS", typeof(string));
            subitems.Columns.Add("CREATEDBY", typeof(Int32));
            subitems.Columns.Add("CREATEDDATE", typeof(DateTime));
            subitems.Columns.Add("UPDATEDBY", typeof(Int32));
            subitems.Columns.Add("UPDATEDDATE", typeof(DateTime));

            foreach (var items in homePolicyDetails.HomeSubItemsList)
            {
                subitems.Rows.Add(0, "", "", 0, "", "", items.SubItemSerialNo,
                    items.SubItemCode, items.SubItemName, items.Description,
                    items.SumInsured, "", homeInsurance.HomeInsurancePolicy.CreatedBy,
                    null, homeInsurance.HomeInsurancePolicy.CreatedBy, null);
            }
            return subitems;
        }
        private DataTable GetHomeDomesticHelps(RR.HomeInsurancePolicyDetails homePolicyDetails, BLO.HomeInsurancePolicyDetails homeInsurance)
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

            foreach (var members in homePolicyDetails.HomeDomesticHelpList)
            {
                domestichelp.Rows.Add(0, "", "", 0, 0, 0, "", "",
                              members.MemberSerialNo, members.Name, members.CPR,
                              members.Title, members.Sex, members.Age, members.DOB, members.SumInsured,
                              members.PremiumAmount, homeInsurance.HomeInsurancePolicy.CreatedBy, null,
                              homeInsurance.HomeInsurancePolicy.CreatedBy, null, members.Occupation, members.Nationality);
            }
            return domestichelp;
        }
        #endregion
    }
}