using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.WebAPI.Framework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BLO = BKIC.SellingPoint.DL.BO;
using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;
using URI = BKIC.SellingPoint.DTO.Constants;

namespace KBIC.WebAPI.Controllers
{
    public class MotorInsuranceController : ApiController
    {
        private readonly IMotorInsurance _motorInsuranceRepository;
        private readonly AutoMapper.IMapper _mapper;

        public MotorInsuranceController(IMotorInsurance motorInsurance)
        {
            _motorInsuranceRepository = motorInsurance;

            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetMotorAutoMapper();
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
        [HttpGet]
        [Route(URI.MotorURI.FetchInsuranceCertificate)]
        public HttpResponseMessage DownloadMotorInvoice(string documentNo, string type, string agentCode, bool isEndorsement, long endorsementID, int renewalCount)
        {
            BLO.InsuranceCertificateResponse result = _motorInsuranceRepository.GetInsuranceCertificate(documentNo, type, agentCode, isEndorsement, endorsementID, renewalCount);
            String FileName = Path.GetFileName(result.FilePath);
            HttpResponseMessage response = new HttpResponseMessage();
            if (System.IO.File.Exists(result.FilePath))
            {
                FileStream stream = File.OpenRead(result.FilePath);

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = FileName;
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(BKIC.SellingPoint.WebAPI.Framework.Utility.GetMimeType(FileName));
                return response;
            }
            return response = new HttpResponseMessage(HttpStatusCode.Gone);
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
        [HttpGet]
        //[ApiAuthorize]
        [Route(URI.MotorURI.GetSavedQuoteDocumentNo)]
        public RR.MotorSavedQuotationResponse GetSavedQuotationDetails(string documentNo, string type, string agentCode,
                                              bool isendorsement, long endorsementid, int renewalcount = 0)
        {
            try
            {
                BLO.MotorSavedQuotationResponse result = _motorInsuranceRepository.GetSavedMotorPolicy(documentNo, type,
                                                            agentCode, isendorsement, endorsementid, renewalcount);
                return _mapper.Map<BLO.MotorSavedQuotationResponse, RR.MotorSavedQuotationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all motor policies by agency.
        /// </summary>
        /// <param name="req">Agency motor request.</param>
        /// <returns>List of motor policies by agency.</returns>
        [HttpPost]
        [Route(URI.MotorURI.GetMotorAgencyPolicy)]
        public RR.AgencyMotorPolicyResponse GetMotorAgencyPolicy(RR.AgencyMotorRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BLO.AgencyMotorRequest motor = _mapper.Map<RR.AgencyMotorRequest, BLO.AgencyMotorRequest>(request);
                    BLO.AgencyMotorPolicyResponse result = _motorInsuranceRepository.GetMotorAgencyPolicy(motor);
                    return _mapper.Map<BLO.AgencyMotorPolicyResponse, RR.AgencyMotorPolicyResponse>(result);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.AgencyMotorPolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch(Exception ex)
            {
                return new RR.AgencyMotorPolicyResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }
            
        }

        /// <summary>
        /// Get all motor policies by CPR.
        /// </summary>
        /// <param name="req">Agency motor request.</param>
        /// <returns>List of motor policies by CPR.</returns>

        [HttpPost]
        [Route(URI.MotorURI.GetMotorPoliciesByTypeByCPR)]
        public RR.AgencyMotorPolicyResponse GetMotorPoliciesByTypeByCPR(RR.AgencyMotorRequest request)
        {
            try
            {
                BLO.AgencyMotorRequest motor = _mapper.Map<RR.AgencyMotorRequest, BLO.AgencyMotorRequest>(request);
                BLO.AgencyMotorPolicyResponse result = _motorInsuranceRepository.GetMotorPoliciesByTypeByCPR(motor);
                return _mapper.Map<BLO.AgencyMotorPolicyResponse, RR.AgencyMotorPolicyResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyMotorPolicyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all motor policies by Document Number(If it has any renewed list out all renewal details).
        /// </summary>
        /// <param name="req">Agency policy details request.</param>
        /// <returns>List of motor policies by document number(Renewal and new policies).</returns>
        [HttpPost]
        [Route(URI.MotorURI.GetMotorPoliciesByDocumentNo)]
        public RR.AgencyMotorPolicyResponse GetMotorPoliciesByDocumentNo(RR.AgencyPolicyDetailsRequest request)
        {
            try
            {
                BLO.AgencyPolicyDetailsRequest req = _mapper.Map<RR.AgencyPolicyDetailsRequest, BLO.AgencyPolicyDetailsRequest>(request);
                BLO.AgencyMotorPolicyResponse result = _motorInsuranceRepository.GetMotorPoliciesByDocumentNo(req);
                return _mapper.Map<BLO.AgencyMotorPolicyResponse, RR.AgencyMotorPolicyResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyMotorPolicyResponse
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
        // [AllowAnonymous]
        [HttpPost]
        [Route(URI.MotorURI.GetQuote)]
        public RR.MotorInsuranceQuoteResponse GetQuote(RR.MotorInsuranceQuote motorQuote)
        {
            try
            {
                BLO.MotorInsuranceQuote details = _mapper.Map<RR.MotorInsuranceQuote, BLO.MotorInsuranceQuote>(motorQuote);
                BLO.MotorInsuranceQuoteResponse result = _motorInsuranceRepository.GetMotorInsuranceQuote(details);
                return _mapper.Map<BLO.MotorInsuranceQuoteResponse, RR.MotorInsuranceQuoteResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorInsuranceQuoteResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

        }

        /// <summary>
        /// Insert the motor insurance policy.
        /// </summary>
        /// <param name="policy">Motor policy details.</param>
        /// <returns>MotorID and Document Number.</returns>
        //[ApiAuthorize]
        [HttpPost]
        [Route(URI.MotorURI.PostMotorPolicy)]
        public RR.MotorInsurancePolicyResponse PostMotorPolicy(RR.MotorInsurancePolicy policy)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BLO.MotorInsurancePolicy details = _mapper.Map<RR.MotorInsurancePolicy, BLO.MotorInsurancePolicy>(policy);
                    details.AuthorizedBy = policy.IsActivePolicy ? policy.Createdby : 0;
                    BLO.MotorInsurancePolicyResponse result = _motorInsuranceRepository.PostMotorInsurance(details);
                    return _mapper.Map<BLO.MotorInsurancePolicyResponse, RR.MotorInsurancePolicyResponse>(result);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.MotorInsurancePolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch (Exception ex)
            {
                return new RR.MotorInsurancePolicyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

        }  

        /// <summary>
        /// Calculate the excess amount based on vehicle make and model.
        /// e.g for make BMW and model 350i the excess amount is BD 100.
        /// </summary>
        /// <param name="request">Excess amount request.</param>
        /// <returns>Excess amount and body type.</returns>
        [ApiAuthorize]
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.MotorURI.GetExcessAmount)]
        public RR.ExcessAmountResponse GetExcessAmount(RR.ExcessAmountRequest request)
        {
           try
            {
                if (ModelState.IsValid)
                {

                    BLO.ExcessAmountRequest details = _mapper.Map<RR.ExcessAmountRequest, BLO.ExcessAmountRequest>(request);
                    BLO.ExcessAmountResponse result = _motorInsuranceRepository.GetExcessCalcualtion(details);
                    return _mapper.Map<BLO.ExcessAmountResponse, RR.ExcessAmountResponse>(result);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.ExcessAmountResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch(Exception ex)
            {
                return new RR.ExcessAmountResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }
            
        }

        /// <summary>
        /// Get Optional cover for the motor product,TISCO have optional covers for the product (GLD,ELT,NMC)
        /// </summary>
        /// <param name="req">Optional cover request.</param>
        /// <returns>List of optional covers.</returns>
        [ApiAuthorize]
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.MotorURI.GetOptionalCover)]
        public RR.OptionalCoverResponse GetOptionalCovers(RR.OptionalCoverRequest request)
        {
            try
            {
                BLO.OptionalCoverRequest details = _mapper.Map<RR.OptionalCoverRequest, BLO.OptionalCoverRequest>(request);
                BLO.OptionalCoverResponse result = _motorInsuranceRepository.GetOptionalCover(details);
                return _mapper.Map<BLO.OptionalCoverResponse, RR.OptionalCoverResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.OptionalCoverResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }

        }


        /// <summary>
        /// Calculate optional cover amount.
        /// </summary>
        /// <param name="req">calculate cover request.</param>
        /// <returns>Optional cover amount.</returns>
        [ApiAuthorize]
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.MotorURI.CalculateOptionalCoverAmount)]
        public RR.CalculateCoverAmountResponse GetOptionalCoverAmount(RR.CalculateCoverAmountRequest request)
        {
            try
            {
                BLO.CalculateCoverAmountRequest details = _mapper.Map<RR.CalculateCoverAmountRequest, BLO.CalculateCoverAmountRequest>(request);
                BLO.CalculateCoverAmountResponse result = _motorInsuranceRepository.CalculateOptionalCoverAmount(details);
                return _mapper.Map<BLO.CalculateCoverAmountResponse, RR.CalculateCoverAmountResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.CalculateCoverAmountResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }

        }

        /// <summary>
        /// Get renewal motor policy details by the document number.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">Get the policy details for the endorsemnt page or policy buy page.</param>
        /// <param name="endorsementID">Endorsement id.</param>
        /// <returns></returns>
        [HttpGet]
        [Route(URI.MotorURI.GetOracleMotorRenewalPolicyByDocNo)]
        public RR.MotorSavedQuotationResponse GetOracleMotorRenewalDetails(string documentNo, string agency, string agentCode)

        {
            try
            {
                BLO.MotorSavedQuotationResponse result = _motorInsuranceRepository.GetOracleRenewMotorPolicy(documentNo, agency, agentCode);
                return _mapper.Map<BLO.MotorSavedQuotationResponse, RR.MotorSavedQuotationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }


        /// <summary>
        /// Get renewal motor policy details by the document number.
        /// </summary>
        /// <param name="documentNo">Document number.</param>
        /// <param name="type">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="isEndorsement">Get the policy details for the endorsemnt page or policy buy page.</param>
        /// <param name="endorsementID">Endorsement id.</param>
        /// <returns></returns>
        [HttpGet]
        [Route(URI.MotorURI.GetMotorRenewalPolicyByDocNo)]
        public RR.MotorSavedQuotationResponse GetMotorRenewalDetails(string documentNo, string agency, string agentCode, int renewalCount)

        {
            try
            {
                BLO.MotorSavedQuotationResponse result = _motorInsuranceRepository.GetRenewMotorPolicy(documentNo, agency, agentCode, renewalCount);
                return _mapper.Map<BLO.MotorSavedQuotationResponse, RR.MotorSavedQuotationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorSavedQuotationResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }


        /// <summary>
        /// Get all motor policies by agency for endorsement page, all policies should be ACTIVE.
        /// Used in every motor endorsement page search by policy number.
        /// </summary>
        /// <param name="req">Agency motor request.</param>
        /// <returns>List of active motor policies for an endorsement.</returns>
        [HttpPost]
        [Route(URI.MotorURI.GetMotorPoliciesEndorsement)]
        public RR.AgencyMotorPolicyResponse GetMotorEndorsementPolicies(RR.AgencyMotorRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BLO.AgencyMotorRequest motor = _mapper.Map<RR.AgencyMotorRequest, BLO.AgencyMotorRequest>(request);
                    BLO.AgencyMotorPolicyResponse result = _motorInsuranceRepository.GetMotorPoliciesEndorsement(motor);
                    return _mapper.Map<BLO.AgencyMotorPolicyResponse, RR.AgencyMotorPolicyResponse>(result);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.AgencyMotorPolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch (Exception ex)
            {
                return new RR.AgencyMotorPolicyResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }

        }


        /// <summary>
        /// Get all eligible oracle motor policies for renewal by agency.
        /// </summary>
        /// <param name="req">Agency motor request.</param>
        /// <returns>List of renewalble oracle motor policies by agency.</returns>
        [HttpPost]
        [Route(URI.MotorURI.GetOracleMotorRenewalPolicies)]
        public RR.AgencyMotorPolicyResponse GetOracleMotorRenewalPolicies(RR.AgencyMotorRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BLO.AgencyMotorRequest motor = _mapper.Map<RR.AgencyMotorRequest, BLO.AgencyMotorRequest>(request);
                    BLO.AgencyMotorPolicyResponse result = _motorInsuranceRepository.GetOracleMotorRenewalPolicies(motor);
                    return _mapper.Map<BLO.AgencyMotorPolicyResponse, RR.AgencyMotorPolicyResponse>(result);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return new RR.AgencyMotorPolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = message
                    };
                }
            }
            catch (Exception ex)
            {
                return new RR.AgencyMotorPolicyResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }

        }

    }
}