
using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.WebAPI.Framework;
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
    /// Domestic policy methods.
    /// </summary>
    public class DomesticHelpController : ApiController
    {
        public readonly IDomesticHelp _domesticHelpRepo;
        private readonly AutoMapper.IMapper _mapper;

        public DomesticHelpController(IDomesticHelp repository)
        {
            _domesticHelpRepo = repository;
            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetDomesticAutoMapper();
        }
        /// <summary>
        /// Get all domestic policies by agency.
        /// </summary>
        /// <param name="request">Agency domestic request.</param>
        /// <returns>List of domestic policies by agency.</returns>
        //[ApiAuthorize]
        [HttpPost]
        [Route(URI.DomesticURI.GetDomesticAgencyPolicy)]
        public RR.AgencyDomesticPolicyResponse GetDomesticAgencyPolicy(RR.AgencyDomesticRequest request)
       {
            try
            {
                if (ModelState.IsValid)
                {
                    BLO.AgencyDomesticRequest domestic = _mapper.Map<RR.AgencyDomesticRequest, BLO.AgencyDomesticRequest>(request);
                    BLO.AgencyDomesticPolicyResponse result = _domesticHelpRepo.GetDomesticAgencyPolicy(domestic);
                    return _mapper.Map<BLO.AgencyDomesticPolicyResponse, RR.AgencyDomesticPolicyResponse>(result);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.AgencyDomesticPolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch (Exception ex)
            {
                return new RR.AgencyDomesticPolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

        }

        /// <summary>
        /// Get domestic policies details by document number.
        /// </summary>
        /// <param name="domesticID">document number.</param>
        /// <param name="insuredCode">agent code.</param>
        /// <returns></returns>
        //[ApiAuthorize]
        [HttpGet]
        [Route(URI.DomesticURI.GetSavedQuoteDocumentNo)]
        public RR.DomesticHelpSavedQuotationResponse GetSavedQuotationPolicy(string documentNo, string agentCode)
        {
            try
            {
                BLO.DomesticHelpSavedQuotationResponse result = _domesticHelpRepo.GetSavedDomesticPolicy(documentNo, agentCode);
                return _mapper.Map<BLO.DomesticHelpSavedQuotationResponse, RR.DomesticHelpSavedQuotationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.DomesticHelpSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
        /// <summary>
        /// Calculate premium for the domestichelp.
        /// </summary>
        /// <param name="quoteRequest">Domestic quote request.</param>
        /// <returns>Domestic quote response.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.DomesticURI.GetQuote)]
        public RR.DomesticHelpQuoteResponse GetQuote(RR.DomesticHelpQuote quoteRequest)
        {
            try
            {
                BLO.DomesticHelpQuote quote = _mapper.Map<RR.DomesticHelpQuote, BLO.DomesticHelpQuote>(quoteRequest);
                BLO.DomesticHelpQuoteResponse result = _domesticHelpRepo.GetDomesticHelpQuote(quote);
                return _mapper.Map<BLO.DomesticHelpQuoteResponse, RR.DomesticHelpQuoteResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.DomesticHelpQuoteResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Insert the domestic help policy.
        /// </summary>
        /// <param name="policydetails">Domestic policy details.</param>
        /// <returns>Domestic policy response with domestic id, documentnumber and HIR status.</returns>
        [HttpPost]
        [Route(URI.DomesticURI.PostQuote)]
        public RR.DomesticHelpPolicyResponse PostDomesticPolicy(RR.DomesticPolicyDetails policydetails)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    BLO.DomesticPolicyDetails policy = new BLO.DomesticPolicyDetails();
                    policy.DomesticHelp.Agency = policydetails.DomesticHelp.Agency;
                    policy.DomesticHelp.AgentCode = policydetails.DomesticHelp.AgentCode;
                    policy.DomesticHelp.AgentBranch = policydetails.DomesticHelp.AgentBranch;
                    policy.DomesticHelp.MainClass = policydetails.DomesticHelp.MainClass;
                    policy.DomesticHelp.SubClass = policydetails.DomesticHelp.SubClass;
                    policy.DomesticHelp.DomesticID = policydetails.DomesticHelp.DomesticID;
                    policy.DomesticHelp.InsuredCode = policydetails.DomesticHelp.InsuredCode;
                    policy.DomesticHelp.InsuredName = policydetails.DomesticHelp.InsuredName;
                    policy.DomesticHelp.CPR = policydetails.DomesticHelp.CPR;
                    policy.DomesticHelp.InsurancePeroid = policydetails.DomesticHelp.InsurancePeroid;
                    policy.DomesticHelp.NoOfDomesticWorkers = policydetails.DomesticHelp.NoOfDomesticWorkers;
                    policy.DomesticHelp.PolicyStartDate = policydetails.DomesticHelp.PolicyStartDate;
                    policy.DomesticHelp.DomesticWorkType = policydetails.DomesticHelp.DomesticWorkType;
                    policy.DomesticHelp.IsPhysicalDefect = policydetails.DomesticHelp.IsPhysicalDefect;
                    policy.DomesticHelp.PhysicalDefectDescription = policydetails.DomesticHelp.PhysicalDefectDescription;
                    policy.DomesticHelp.CreatedBy = policydetails.DomesticHelp.CreatedBy;
                    policy.DomesticHelp.AuthorizedBy = policydetails.DomesticHelp.IsActivePolicy == true ? policydetails.DomesticHelp.CreatedBy : 0;
                    policy.DomesticHelp.Mobile = policydetails.DomesticHelp.Mobile;
                    policy.DomesticHelp.IsSaved = policydetails.DomesticHelp.IsSaved;
                    policy.DomesticHelp.IsActivePolicy = policydetails.DomesticHelp.IsActivePolicy;
                    policy.DomesticHelp.CommissionAmount = policydetails.DomesticHelp.CommissionAmount;
                    policy.DomesticHelp.Remarks = policydetails.DomesticHelp.Remarks;
                    policy.DomesticHelp.AccountNumber = policydetails.DomesticHelp.AccountNumber;
                    policy.DomesticHelp.PaymentType = policydetails.DomesticHelp.PaymentType;
                    policy.DomesticHelp.CommissionAfterDiscount = policydetails.DomesticHelp.CommissionAfterDiscount;
                    policy.DomesticHelp.PremiumAfterDiscount = policydetails.DomesticHelp.PremiumAfterDiscount;
                    policy.DomesticHelp.UserChangedPremium = policydetails.DomesticHelp.UserChangedPremium;
                    policy.DomesticHelpMemberdt = GetDomesticHelpMembers(policydetails, policy);
                    BLO.DomesticHelpPolicyResponse result = _domesticHelpRepo.PostDomesticPolicy(policy);
                    return _mapper.Map<BLO.DomesticHelpPolicyResponse, RR.DomesticHelpPolicyResponse>(result);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.DomesticHelpPolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch(Exception ex)
            {
                return new RR.DomesticHelpPolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        } 
       

        #region PrivateMethod
        private DataTable GetDomesticHelpMembers(RR.DomesticPolicyDetails policyDetails, BLO.DomesticPolicyDetails policy)
        {
            DataTable member = new DataTable();
            if (policyDetails.DomesticHelpMemberList.Count > 0)
            {

                member.Columns.Add("INSUREDCODE", typeof(string));
                member.Columns.Add("INSUREDNAME", typeof(string));
                member.Columns.Add("SUMINSURED", typeof(decimal));
                member.Columns.Add("PREMIUMAMOUNT", typeof(decimal));
                member.Columns.Add("EXPIRYDATE", typeof(DateTime));
                member.Columns.Add("ADDRESS1", typeof(string));
                member.Columns.Add("OCCUPATION", typeof(string));
                member.Columns.Add("NATIONALITY", typeof(string));
                member.Columns.Add("PASSPORT", typeof(string));
                member.Columns.Add("DOB", typeof(DateTime));
                member.Columns.Add("SEX", typeof(char));
                member.Columns.Add("ITEMSERIALNO", typeof(int));
                member.Columns.Add("MAINCLASS", typeof(string));
                member.Columns.Add("SUBCLASS", typeof(string));
                member.Columns.Add("DATEOFSUBMISSION", typeof(DateTime));
                member.Columns.Add("COMMENCEDATE", typeof(DateTime));
                member.Columns.Add("IDENTITYNO", typeof(string));
                member.Columns.Add("OCCUPATIONOTHER", typeof(string));
                member.Columns.Add("CREATEDBY", typeof(int));
                member.Columns.Add("CREATEDDATE", typeof(string));

                foreach (var members in policyDetails.DomesticHelpMemberList)
                {
                    member.Rows.Add(policy.DomesticHelp.InsuredCode, members.Name, 0, 0, null,
                        members.AddressType, members.Occupation, members.Nationality, members.Passport, members.DOB,
                        members.Sex, members.ItemserialNo, "", "", null, policy.DomesticHelp.PolicyStartDate, members.CPRNumber, "",
                          policy.DomesticHelp.CreatedBy, null);
                }
            }
            return member;
        }
        #endregion
    }
}