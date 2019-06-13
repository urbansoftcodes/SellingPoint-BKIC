using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.WebAPI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLO = BKIC.SellingPoint.DL.BO;
using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;
using URI = BKIC.SellingPoint.DTO.Constants;

namespace BKIC.SellingPoint.WebAPI.Controllers
{
    /// <summary>
    /// Motor endorsement functionalities.
    /// </summary>
    public class MotorEndorsementController : ApiController
    {
        private readonly IMotorEndorsement _motorEndorsementRepository;
        private readonly AutoMapper.IMapper _mapper;

        public MotorEndorsementController(IMotorEndorsement motorEndorsement)
        {
            _motorEndorsementRepository = motorEndorsement;
            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetMotorEndorsementAutoMapper();
        }

        /// <summary>
        /// Calculate motor endorsement premium.
        /// </summary>
        /// <param name="motorEndorsementQuote">Motor endorsement quote request.</param>
        /// <returns>Endorsement premium,Endorsement commission.</returns>
        [HttpPost]
        [Route(URI.MotorEndorsementURI.GetMotorEndorsementQuote)]
        public RR.MotorEndorsementQuoteResult GetQuote(RR.MotorEndorsementQuote motorEndorsementQuote)
        {
            try
            {
                BLO.MotorEndorsementQuote details = _mapper.Map<RR.MotorEndorsementQuote, BLO.MotorEndorsementQuote>(motorEndorsementQuote);
                BLO.MotorEndorsementQuoteResult result = _motorEndorsementRepository.GetMotorEndorsementQuote(details);
                return _mapper.Map<BLO.MotorEndorsementQuoteResult, RR.MotorEndorsementQuoteResult>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorEndorsementQuoteResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Insert the motor endorsement.
        /// </summary>
        /// <param name="motorEndorsement">Motor endorsement details.</param>
        /// <returns>Motorendorsementid, Motorendorsementnumber.</returns>
        [HttpPost]
        [Route(URI.MotorEndorsementURI.PostMotorEndorsement)]
        public RR.MotorEndorsementResult PostMotorEndorsement(RR.MotorEndorsement policy)
        {
            try
            {
                BLO.MotorEndorsement details = _mapper.Map<RR.MotorEndorsement, BLO.MotorEndorsement>(policy);
                BLO.MotorEndorsementResult result = _motorEndorsementRepository.PostMotorEndorsement(details);
                return _mapper.Map<BLO.MotorEndorsementResult, RR.MotorEndorsementResult>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorEndorsementResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Insert the motor endorsement type of cancel.
        /// </summary>
        /// <param name="motorEndorsement">Motor endorsement request</param>
        /// <returns>Motorendorsementid, Motorendorsementnumber.</returns>
        [HttpPost]
        [Route(URI.MotorEndorsementURI.PostAdminMotorEndorsement)]
        public RR.MotorEndorsementResult PostAdminMotorPolicy(RR.MotorEndorsement policy)
        {
            try
            {
                BLO.MotorEndorsement details = _mapper.Map<RR.MotorEndorsement, BLO.MotorEndorsement>(policy);
                BLO.MotorEndorsementResult result = _motorEndorsementRepository.PostAdminMotorEndorsement(details);
                return _mapper.Map<BLO.MotorEndorsementResult, RR.MotorEndorsementResult>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorEndorsementResult
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
        [HttpPost]
        [Route(URI.MotorEndorsementURI.EndorsementPreCheck)]
        public RR.MotorEndorsementPreCheckResponse EndorsementPrecheck(RR.MotorEndorsementPreCheckRequest req)
        {
            try
            {
                BLO.MotorEndorsementPreCheckRequest details = _mapper.Map<RR.MotorEndorsementPreCheckRequest, BLO.MotorEndorsementPreCheckRequest>(req);
                var result = _motorEndorsementRepository.EndorsementPrecheck(details);
                return _mapper.Map<BLO.MotorEndorsementPreCheckResponse, RR.MotorEndorsementPreCheckResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorEndorsementPreCheckResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all the motor endorsement details for the specific policy.
        /// To show the top of the page on any motor endorsement page.
        /// </summary>
        /// <param name="request">Endorsement request.</param>
        /// <returns>list of endorsemnt details.</returns>
        [HttpPost]
        [Route(URI.MotorEndorsementURI.GetAllEndorsements)]
        public RR.MotorEndoResult GetAllEndorsements(RR.MotorEndoRequest req)
        {
            try
            {
                BLO.MotorEndoRequest details = _mapper.Map<RR.MotorEndoRequest,BLO.MotorEndoRequest>(req);
                var result = _motorEndorsementRepository.GetAllEndorsements(details);
                return _mapper.Map<BLO.MotorEndoResult, RR.MotorEndoResult>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorEndoResult()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        ///Motor endorsement operation - authorize or delete.
        /// </summary>
        /// <param name="request">Endorsement operation request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(URI.MotorEndorsementURI.EndorsementOperation)]
        public RR.MotorEndorsementOperationResponse EndorsementOperation(RR.MotorEndorsementOperation req)
        {
            try
            {
                BLO.MotorEndorsementOperation details = _mapper.Map<RR.MotorEndorsementOperation,BLO.MotorEndorsementOperation>(req);
                var result = _motorEndorsementRepository.EndorsementOperation(details);
                return _mapper.Map<BLO.MotorEndorsementOperationResponse,RR.MotorEndorsementOperationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorEndorsementOperationResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
    }
}