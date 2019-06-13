using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.WebAPI.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

using BLO = BKIC.SellingPoint.DL.BO;

using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;
using URI = BKIC.SellingPoint.DTO.Constants;

namespace BKIC.SellingPoint.WebAPI.Controllers
{
    public class TravelInsuranceController : ApiController
    {
        //public readonly IAdmin _adminRepository;
        private readonly ITravelInsurance _travelInsuranceRep;

        private readonly AutoMapper.IMapper _mapper;

        public TravelInsuranceController(ITravelInsurance repository)
        {
            _travelInsuranceRep = repository;
            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetTravelAutoMapper();
        }


        /// <summary>
        /// Get travel policies by agency.
        /// </summary>
        /// <param name="request">Agency travel request.</param>
        /// <returns>List of travel policies belonging to the particular agency.</returns>
        [HttpPost]
        [Route(URI.TravelInsuranceURI.GetAgencyPolicy)]
        public RR.AgencyTravelPolicyResponse GetAgencyPolicy(RR.AgencyTravelRequest request)
        {
            try
            {
                BLO.AgencyTravelRequest travel = _mapper.Map<RR.AgencyTravelRequest, BLO.AgencyTravelRequest>(request);
                BLO.AgencyTravelPolicyResponse result = _travelInsuranceRep.GetTravelAgencyPolicy(travel);
                return _mapper.Map<BLO.AgencyTravelPolicyResponse, RR.AgencyTravelPolicyResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.AgencyTravelPolicyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
           
        }

        /// <summary>
        /// Get travel policies by CPR.
        /// </summary>
        /// <param name="request">Agency travel request.</param>
        /// <returns>List of travel policies belonging to the particular CPR.</returns>
        [HttpPost]
        [Route(URI.TravelInsuranceURI.GetTravelPoliciesByCPR)]
        public RR.AgencyTravelPolicyResponse GetPoliciesByTypeByCPR(RR.AgencyTravelRequest request)
        {
            try
            {
                BLO.AgencyTravelRequest travel = _mapper.Map<RR.AgencyTravelRequest, BLO.AgencyTravelRequest>(request);
                BLO.AgencyTravelPolicyResponse result = _travelInsuranceRep.GetTravelAgencyPolicyByCPR(travel);
                return _mapper.Map<BLO.AgencyTravelPolicyResponse, RR.AgencyTravelPolicyResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.AgencyTravelPolicyResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }
            
        }

        /// <summary>
        /// Get Travel Insurance Quote
        /// </summary>
        /// <param name="quote">Travel quote request.</param>
        /// <returns>Travel premium.</returns>      
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.TravelInsuranceURI.GetQuote)]
        public RR.TravelInsuranceQuoteResponse GetQuote(RR.TravelInsuranceQuote quote)
        {
            try
            {
                BLO.TravelInsuranceQuote req = _mapper.Map<RR.TravelInsuranceQuote, BLO.TravelInsuranceQuote>(quote);
                BLO.TravelInsuranceQuoteResponse result = _travelInsuranceRep.GetTravelInsuranceQuote(req);
                return _mapper.Map<BLO.TravelInsuranceQuoteResponse, RR.TravelInsuranceQuoteResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.TravelInsuranceQuoteResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Insert the travel policy.
        /// </summary>
        /// <param name="travel">Travel policy properties.</param>
        /// <returns></returns>
        //[ApiAuthorize]
        [HttpPost]
        [Route(URI.TravelInsuranceURI.PostTravel)]
        public RR.TravelPolicyResponse PostTravelPolicy(RR.TravelPolicy travel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BLO.TravelPolicy bo = new BLO.TravelPolicy();
                    bo.TravelInsurancePolicyDetails.TravelID = travel.TravelInsurancePolicyDetails.TravelID;
                    bo.TravelInsurancePolicyDetails.Agency = travel.TravelInsurancePolicyDetails.Agency;
                    bo.TravelInsurancePolicyDetails.AgentCode = travel.TravelInsurancePolicyDetails.AgentCode;
                    bo.TravelInsurancePolicyDetails.AgentBranch = travel.TravelInsurancePolicyDetails.AgentBranch;

                    bo.TravelInsurancePolicyDetails.MainClass = travel.TravelInsurancePolicyDetails.MainClass;
                    bo.TravelInsurancePolicyDetails.SubClass = travel.TravelInsurancePolicyDetails.SubClass;

                    bo.TravelInsurancePolicyDetails.DOB = travel.TravelInsurancePolicyDetails.DOB;
                    bo.TravelInsurancePolicyDetails.PackageCode = travel.TravelInsurancePolicyDetails.PackageCode;
                    bo.TravelInsurancePolicyDetails.PolicyPeroidYears = travel.TravelInsurancePolicyDetails.PolicyPeroidYears;
                    bo.TravelInsurancePolicyDetails.InsuredCode = travel.TravelInsurancePolicyDetails.InsuredCode;
                    bo.TravelInsurancePolicyDetails.InsuredName = travel.TravelInsurancePolicyDetails.InsuredName;
                    bo.TravelInsurancePolicyDetails.SumInsured = travel.TravelInsurancePolicyDetails.SumInsured;
                    bo.TravelInsurancePolicyDetails.PremiumAmount = travel.TravelInsurancePolicyDetails.PremiumAmount;
                    bo.TravelInsurancePolicyDetails.InsuranceStartDate = travel.TravelInsurancePolicyDetails.InsuranceStartDate;

                    bo.TravelInsurancePolicyDetails.ExpiryDate = travel.TravelInsurancePolicyDetails.ExpiryDate;
                    bo.TravelInsurancePolicyDetails.MainClass = travel.TravelInsurancePolicyDetails.MainClass;
                    bo.TravelInsurancePolicyDetails.Passport = travel.TravelInsurancePolicyDetails.Passport;

                    bo.TravelInsurancePolicyDetails.FFPNumber = travel.TravelInsurancePolicyDetails.FFPNumber;
                    bo.TravelInsurancePolicyDetails.QuestionaireCode = travel.TravelInsurancePolicyDetails.QuestionaireCode;
                    bo.TravelInsurancePolicyDetails.IsPhysicalDefect = travel.TravelInsurancePolicyDetails.IsPhysicalDefect;
                    bo.TravelInsurancePolicyDetails.PhysicalStateDescription = travel.TravelInsurancePolicyDetails.PhysicalStateDescription;

                    bo.TravelInsurancePolicyDetails.PaymentType = travel.TravelInsurancePolicyDetails.PaymentType;
                    bo.TravelInsurancePolicyDetails.AccountNumber = travel.TravelInsurancePolicyDetails.AccountNumber;
                    bo.TravelInsurancePolicyDetails.Remarks = travel.TravelInsurancePolicyDetails.Remarks;

                    bo.TravelInsurancePolicyDetails.Renewal = travel.TravelInsurancePolicyDetails.Renewal;
                    bo.TravelInsurancePolicyDetails.Occupation = travel.TravelInsurancePolicyDetails.Occupation;
                    bo.TravelInsurancePolicyDetails.PeroidOfCoverCode = travel.TravelInsurancePolicyDetails.PeroidOfCoverCode;
                    bo.TravelInsurancePolicyDetails.LoadAmount = travel.TravelInsurancePolicyDetails.LoadAmount;
                    bo.TravelInsurancePolicyDetails.DiscountAmount = travel.TravelInsurancePolicyDetails.DiscountAmount;
                    bo.TravelInsurancePolicyDetails.Code = travel.TravelInsurancePolicyDetails.Code;
                    bo.TravelInsurancePolicyDetails.CPR = travel.TravelInsurancePolicyDetails.CPR;
                    bo.TravelInsurancePolicyDetails.Mobile = travel.TravelInsurancePolicyDetails.Mobile;
                    bo.TravelInsurancePolicyDetails.CreatedBy = travel.TravelInsurancePolicyDetails.CreatedBy;
                    bo.TravelInsurancePolicyDetails.AuthorizedBy = travel.TravelInsurancePolicyDetails.IsActivePolicy == true ? travel.TravelInsurancePolicyDetails.CreatedBy : 0;
                    bo.TravelInsurancePolicyDetails.IsSaved = travel.TravelInsurancePolicyDetails.IsSaved;
                    bo.TravelInsurancePolicyDetails.IsActivePolicy = travel.TravelInsurancePolicyDetails.IsActivePolicy;
                    bo.TravelInsurancePolicyDetails.Source = travel.TravelInsurancePolicyDetails.Source;
                    bo.TravelInsurancePolicyDetails.CoverageType = travel.TravelInsurancePolicyDetails.CoverageType;
                    bo.TravelInsurancePolicyDetails.PaymentType = travel.TravelInsurancePolicyDetails.PaymentType;
                    bo.TravelInsurancePolicyDetails.CommissionAfterDiscount = travel.TravelInsurancePolicyDetails.CommissionAfterDiscount;
                    bo.TravelInsurancePolicyDetails.PremiumAfterDiscount = travel.TravelInsurancePolicyDetails.PremiumAfterDiscount;
                    bo.TravelInsurancePolicyDetails.UserChangedPremium = travel.TravelInsurancePolicyDetails.UserChangedPremium;

                    if (travel.TravelMembers.Count > 0)
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

                        foreach (var members in travel.TravelMembers)
                        {
                            members.UpdatedDate = DateTime.Now;
                            //members.CreatedDate = DateTime.Now;
                            //members.DateOfBirth = DateTime.Now;

                            member.Rows.Add(members.TravelID, members.DocumentNo, members.ItemSerialNo, members.ItemName, members.SumInsured,
                                members.ForeignSumInsured, members.Category, members.Title, members.Sex, members.DateOfBirth, members.Age,
                                members.PremiumAmount, members.Make, members.OccupationCode, members.CPR, members.Passport, members.FirstName,
                                members.MiddleName, members.LastName, members.CreatedBy, members.CreatedDate, members.UpdatedBy, members.UpdatedDate, "");
                        }

                        bo.TravelMembers = member;
                    }

                    BLO.TravelPolicyResponse result = _travelInsuranceRep.PostTravelPolicyDetails(bo);

                    return new RR.TravelPolicyResponse
                    {
                        TravelId = result.TravelId,
                        IsHIR = result.IsHIR,
                        HIRStatusMessage = result.HIRStatusMessage,
                        IsTransactionDone = result.IsTransactionDone,
                        TransactionErrorMessage = result.TransactionErrorMessage,
                        PaymentTrackID = result.PaymentTrackID,
                        DocumentNo = result.DocumentNo
                    };
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.TravelPolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch (Exception ex)
            {
                return new RR.TravelPolicyResponse()
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
        [HttpGet]
        [Route(URI.TravelInsuranceURI.GetSavedQuoteDocumentNo)]
        public RR.TravelSavedQuotationResponse GetTravelSavedQuotationPolicy(string documentNo, string type, string agentCode, 
                                               bool isendorsement, long endorsementid)
        {
            BLO.TravelSavedQuotationResponse result = _travelInsuranceRep.GetSavedQuotationByPolicy(documentNo, "", agentCode, 
                                                     isendorsement, endorsementid);

            return _mapper.Map<BLO.TravelSavedQuotationResponse, RR.TravelSavedQuotationResponse>(result);           
        }     

        /// <summary>
        /// Calculate the travel policy expire date.
        /// </summary>
        /// <param name="commenceDetails">Policy start date.</param>
        /// <returns>Policy expire date.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.TravelInsuranceURI.GetPolicyExpirtyDate)]
        public RR.TravelInsuranceExpiryDateResponse GetExpirtyDate(RR.TravelInsuranceExpiryDate commenceDetails)
        {
            try
            {
                DateTime ExpiryDate;
                string period = commenceDetails.PackageCode;
                DateTime startdate = commenceDetails.CommenceDate.Value;
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

                return new DTO.RequestResponseWrappers.TravelInsuranceExpiryDateResponse()
                {
                    IsTransactionDone = true,
                    ExpiryDate = ExpiryDate
                };
            }
            catch (Exception exc)
            {
                return new RR.TravelInsuranceExpiryDateResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = exc.Message
                };
            }
        }
        /// <summary>
        /// Get all travel policies by agency for endorsement page, all policies should be ACTIVE.
        /// Used in every  travel endorsement page search by policy number.
        /// </summary>
        /// <param name="req">Agency travel request.</param>
        /// <returns>List of active travel policies for an endorsement.</returns>
        [HttpPost]
        [Route(URI.TravelInsuranceURI.GetTravelPoliciesEndorsement)]
        public RR.AgencyTravelPolicyResponse GetTravelPoliciesEndorsement(RR.AgencyTravelRequest request)
        {
            try
            {
                BLO.AgencyTravelRequest travel = _mapper.Map<RR.AgencyTravelRequest, BLO.AgencyTravelRequest>(request);
                BLO.AgencyTravelPolicyResponse result = _travelInsuranceRep.GetTravelPoliciesEndorsement(travel);
                return _mapper.Map<BLO.AgencyTravelPolicyResponse, RR.AgencyTravelPolicyResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyTravelPolicyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

        }

        #region Unused actions

        //[ApiAuthorize]
        [HttpGet]
        [Route(URI.TravelInsuranceURI.GetSavedQuotation)]
        public RR.TravelSavedQuotationResponse GetTravelSavedQuotation(int travelQuotationId, string userInsuredCode, string type)
        {
            BLO.TravelSavedQuotationResponse result = _travelInsuranceRep.GetSavedQuotationByTravelId(travelQuotationId, userInsuredCode, type);

            RR.TravelSavedQuotationResponse rrResult = new RR.TravelSavedQuotationResponse();
            RR.TravelInsurancePolicy policyDetails = new RR.TravelInsurancePolicy();

            policyDetails.TravelID = result.TravelInsurancePolicyDetails.TravelID;
            policyDetails.InsuredCode = result.TravelInsurancePolicyDetails.InsuredCode;
            policyDetails.InsuredName = result.TravelInsurancePolicyDetails.InsuredName;
            policyDetails.SumInsured = result.TravelInsurancePolicyDetails.SumInsured;
            policyDetails.PremiumAmount = result.TravelInsurancePolicyDetails.PremiumAmount;
            policyDetails.InsuranceStartDate = result.TravelInsurancePolicyDetails.InsuranceStartDate;
            policyDetails.ExpiryDate = result.TravelInsurancePolicyDetails.ExpiryDate;
            policyDetails.MainClass = result.TravelInsurancePolicyDetails.MainClass;
            policyDetails.SubClass = result.TravelInsurancePolicyDetails.SubClass;
            policyDetails.Passport = result.TravelInsurancePolicyDetails.Passport;
            policyDetails.Occupation = result.TravelInsurancePolicyDetails.Occupation;
            policyDetails.PeroidOfCoverCode = result.TravelInsurancePolicyDetails.PeroidOfCoverCode;
            policyDetails.DiscountAmount = result.TravelInsurancePolicyDetails.DiscountAmount;
            //policyDetails.Code = result.TravelInsurancePolicyDetails.Code;
            policyDetails.CPR = result.TravelInsurancePolicyDetails.CPR;
            policyDetails.Mobile = result.TravelInsurancePolicyDetails.Mobile;
            policyDetails.CreadtedDate = result.TravelInsurancePolicyDetails.CreadtedDate;
            policyDetails.UpdatedDate = result.TravelInsurancePolicyDetails.UpdatedDate;
            policyDetails.DocumentNumber = result.TravelInsurancePolicyDetails.DocumentNumber;
            //policyDetailsls.ISHIR = result.TravelInsurancePolicyDetails.IsHIR,
            policyDetails.IsPhysicalDefect = result.TravelInsurancePolicyDetails.IsPhysicalDefect;
            policyDetails.PhysicalStateDescription = result.TravelInsurancePolicyDetails.PhysicalStateDescription;
            policyDetails.FFPNumber = result.TravelInsurancePolicyDetails.FFPNumber;
            policyDetails.CoverageType = result.TravelInsurancePolicyDetails.CoverageType;
            policyDetails.PackageName = result.TravelInsurancePolicyDetails.PackageName;
            policyDetails.PolicyPeroidName = result.TravelInsurancePolicyDetails.PolicyPeroidName;

            rrResult.TravelInsurancePolicyDetails = policyDetails;
            List<RR.TravelMembers> rrMembers = new List<RR.TravelMembers>();

            if (result.TravelMembers.Count > 0)
            {
                foreach (var member in result.TravelMembers)
                {
                    RR.TravelMembers rrMember = new RR.TravelMembers();
                    rrMember.Age = member.Age;
                    rrMember.DocumentNo = member.DocumentNo;
                    rrMember.ItemSerialNo = member.ItemSerialNo;
                    rrMember.ItemName = member.ItemName;
                    rrMember.SumInsured = member.SumInsured;
                    rrMember.ForeignSumInsured = member.ForeignSumInsured;
                    rrMember.Category = member.Category;
                    rrMember.Title = member.Title;
                    rrMember.Sex = member.Sex;
                    rrMember.DateOfBirth = member.DateOfBirth;
                    rrMember.Age = member.Age;
                    rrMember.PremiumAmount = member.PremiumAmount;
                    rrMember.Make = member.Make;
                    rrMember.OccupationCode = member.OccupationCode;
                    rrMember.CPR = member.CPR;
                    rrMember.Passport = member.Passport;
                    rrMember.FirstName = member.FirstName;
                    rrMember.LastName = member.LastName;
                    rrMember.MiddleName = member.MiddleName;
                    rrMember.CreatedDate = member.CreatedDate;
                    rrMember.UpdatedDate = member.UpdatedDate;
                    rrMembers.Add(rrMember);
                }
            }
            rrResult.TravelMembers = rrMembers;

            RR.InsuredMaster details = new RR.InsuredMaster();
            if (result.InsuredDetails != null)
            {
                details.UserInfo.CPR = result.InsuredDetails.UserInfo.CPR;
                details.UserInfo.DOB = result.InsuredDetails.UserInfo.DOB;
                details.UserInfo.Nationality = result.InsuredDetails.UserInfo.Nationality;
                details.UserInfo.Sex = result.InsuredDetails.UserInfo.Sex;
                details.UserInfo.Title = result.InsuredDetails.UserInfo.Title;
                details.UserInfo.FirstName = result.InsuredDetails.UserInfo.FirstName;
                details.UserInfo.MiddleName = result.InsuredDetails.UserInfo.MiddleName;
                details.UserInfo.LastName = result.InsuredDetails.UserInfo.LastName;
            }
            rrResult.InsuredDetails = details;
            rrResult.IsTransactionDone = result.IsTransactionDone;
            rrResult.TransactionErrorMessage = result.TransactionErrorMessage;

            return rrResult;
        }

        [ApiAuthorize]
        [HttpPost]
        [Route(URI.TravelInsuranceURI.UpdatePolicyDetails)]
        public RR.UpdateTravelDetailsResponse UpdateTravelDetails(RR.UpdateTravelDetailsRequest ptraveldetails)
        {
            try
            {
                BLO.UpdateTravelDetailsRequest bo = new BLO.UpdateTravelDetailsRequest();
                bo.TravelInsurancePolicyDetails.Agency = ptraveldetails.TravelInsurancePolicyDetails.Agency;
                bo.TravelInsurancePolicyDetails.AgentCode = ptraveldetails.TravelInsurancePolicyDetails.AgentCode;
                bo.TravelInsurancePolicyDetails.AgentBranch = ptraveldetails.TravelInsurancePolicyDetails.AgentBranch;
                bo.TravelInsurancePolicyDetails.TravelID = ptraveldetails.TravelInsurancePolicyDetails.TravelID;
                bo.TravelInsurancePolicyDetails.DOB = ptraveldetails.TravelInsurancePolicyDetails.DOB;
                bo.TravelInsurancePolicyDetails.PackageCode = ptraveldetails.TravelInsurancePolicyDetails.PackageCode;
                bo.TravelInsurancePolicyDetails.PolicyPeroidYears = ptraveldetails.TravelInsurancePolicyDetails.PolicyPeroidYears;
                bo.TravelInsurancePolicyDetails.InsuredCode = ptraveldetails.TravelInsurancePolicyDetails.InsuredCode;
                bo.TravelInsurancePolicyDetails.InsuredName = ptraveldetails.TravelInsurancePolicyDetails.InsuredName;
                bo.TravelInsurancePolicyDetails.SumInsured = ptraveldetails.TravelInsurancePolicyDetails.SumInsured;
                bo.TravelInsurancePolicyDetails.PremiumAmount = ptraveldetails.TravelInsurancePolicyDetails.PremiumAmount;
                bo.TravelInsurancePolicyDetails.InsuranceStartDate = ptraveldetails.TravelInsurancePolicyDetails.InsuranceStartDate;
                bo.TravelInsurancePolicyDetails.ExpiryDate = ptraveldetails.TravelInsurancePolicyDetails.ExpiryDate;
                bo.TravelInsurancePolicyDetails.MainClass = ptraveldetails.TravelInsurancePolicyDetails.MainClass;
                bo.TravelInsurancePolicyDetails.SubClass = ptraveldetails.TravelInsurancePolicyDetails.SubClass;
                bo.TravelInsurancePolicyDetails.Passport = ptraveldetails.TravelInsurancePolicyDetails.Passport;
                bo.TravelInsurancePolicyDetails.FFPNumber = ptraveldetails.TravelInsurancePolicyDetails.FFPNumber;
                bo.TravelInsurancePolicyDetails.QuestionaireCode = ptraveldetails.TravelInsurancePolicyDetails.QuestionaireCode;
                bo.TravelInsurancePolicyDetails.IsPhysicalDefect = ptraveldetails.TravelInsurancePolicyDetails.IsPhysicalDefect;
                bo.TravelInsurancePolicyDetails.PhysicalStateDescription = ptraveldetails.TravelInsurancePolicyDetails.PhysicalStateDescription;
                bo.TravelInsurancePolicyDetails.Renewal = ptraveldetails.TravelInsurancePolicyDetails.Renewal;
                bo.TravelInsurancePolicyDetails.Occupation = ptraveldetails.TravelInsurancePolicyDetails.Occupation;
                bo.TravelInsurancePolicyDetails.PeroidOfCoverCode = ptraveldetails.TravelInsurancePolicyDetails.PeroidOfCoverCode;
                bo.TravelInsurancePolicyDetails.LoadAmount = ptraveldetails.TravelInsurancePolicyDetails.LoadAmount;
                bo.TravelInsurancePolicyDetails.DiscountAmount = ptraveldetails.TravelInsurancePolicyDetails.DiscountAmount;
                bo.TravelInsurancePolicyDetails.Code = ptraveldetails.TravelInsurancePolicyDetails.Code;
                bo.TravelInsurancePolicyDetails.CPR = ptraveldetails.TravelInsurancePolicyDetails.CPR;
                bo.TravelInsurancePolicyDetails.Mobile = ptraveldetails.TravelInsurancePolicyDetails.Mobile;
                bo.TravelInsurancePolicyDetails.UpdatedBy = ptraveldetails.TravelInsurancePolicyDetails.UpdatedBy;
                bo.TravelInsurancePolicyDetails.IsSaved = ptraveldetails.TravelInsurancePolicyDetails.IsSaved;
                bo.TravelInsurancePolicyDetails.Remarks = ptraveldetails.TravelInsurancePolicyDetails.Remarks;
                bo.TravelInsurancePolicyDetails.AccountNumber = ptraveldetails.TravelInsurancePolicyDetails.AccountNumber;
                bo.TravelInsurancePolicyDetails.PaymentType = ptraveldetails.TravelInsurancePolicyDetails.PaymentType;
                bo.TravelInsurancePolicyDetails.Discounted = ptraveldetails.TravelInsurancePolicyDetails.Discounted;
                bo.TravelInsurancePolicyDetails.CoverageType = ptraveldetails.TravelInsurancePolicyDetails.CoverageType;
                bo.TravelInsurancePolicyDetails.IsActivePolicy = ptraveldetails.TravelInsurancePolicyDetails.IsActivePolicy;
                bo.TravelInsurancePolicyDetails.UserChangedPremium = ptraveldetails.TravelInsurancePolicyDetails.UserChangedPremium;
                bo.TravelInsurancePolicyDetails.CommissionAfterDiscount = ptraveldetails.TravelInsurancePolicyDetails.CommissionAfterDiscount;
                bo.TravelInsurancePolicyDetails.PremiumAfterDiscount = ptraveldetails.TravelInsurancePolicyDetails.PremiumAfterDiscount;

                if (ptraveldetails.TravelMembers.Count > 0)
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

                    foreach (var members in ptraveldetails.TravelMembers)
                    {
                        member.Rows.Add(members.TravelID, members.DocumentNo, members.ItemSerialNo, members.ItemName, members.SumInsured,
                            members.ForeignSumInsured, members.Category, members.Title, members.Sex, members.DateOfBirth, members.Age,
                            members.PremiumAmount, members.Make, members.OccupationCode, members.CPR, members.Passport, members.FirstName,
                            members.MiddleName, members.LastName, members.CreatedBy, members.CreatedDate, members.UpdatedBy, members.UpdatedDate, "");
                    }

                    bo.TravelMembers = member;
                }

                BLO.UpdateTravelDetailsResponse result = _travelInsuranceRep.UpdateTravelDetails(bo);

                return new RR.UpdateTravelDetailsResponse
                {
                    IsHIR = result.IsHIR,
                    IsTransactionDone = result.IsTransactionDone,
                    TransactionErrorMessage = result.TransactionErrorMessage,
                    PaymentTrackId = result.PaymentTrackId,
                };
            }
            catch (Exception ex)
            {
                return new RR.UpdateTravelDetailsResponse { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        //[ApiAuthorize]
        //[HttpGet]
        //[Route(BKICURI.FetchSumInsured)]
        //public RR.FetchSumInsuredResponse FetchSumInsured(string insuranceType)
        //{
        //    try
        //    {
        //        BLO.FetchSumInsuredResponse response = _travelInsuranceRep.FetchSumInsuredAmount(insuranceType);
        //        return new RR.FetchSumInsuredResponse { SumInsuredBHD = response.SumInsuredBHD, SumInsuredUSD = response.SumInsuredUSD, IsTransactionDone = response.IsTransactionDone };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new RR.FetchSumInsuredResponse { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
        //    }
        //}
        #endregion
    }
}