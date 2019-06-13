using BKIC.SellingPoint.DL.BL.Implematon;
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
    public class TravelEndorsementController : ApiController
    {
        private readonly ITravelEndorsement _travelEndorsementRepository;

        private readonly AutoMapper.IMapper _mapper;

        public TravelEndorsementController(ITravelEndorsement travelEndorsement)
        {
            _travelEndorsementRepository = travelEndorsement;

            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetTravelEndorsementAutoMapper();
        }


        /// <summary>
        /// Calculate travel endorsement premium.
        /// </summary>
        /// <param name="travelEndorsementQuote">Travel endorsement quote request.</param>
        /// <returns>Endorsement premium,Endorsement commission.</returns>
        [HttpPost]
        [Route(URI.TravelEndorsementURI.GetTravelEndorsementQuote)]
        public RR.TravelEndorsementQuoteResponse GetQuote(RR.TravelEndorsementQuote travelEndorsementQuote)
        {
            try
            {
                BLO.TravelEndorsementQuote details = _mapper.Map<RR.TravelEndorsementQuote, BLO.TravelEndorsementQuote>(travelEndorsementQuote);
                BLO.TravelEndorsementQuoteResponse result = _travelEndorsementRepository.GetTravelEndorsementQuote(details);
                return _mapper.Map<BLO.TravelEndorsementQuoteResponse, RR.TravelEndorsementQuoteResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.TravelEndorsementQuoteResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Insert the travel endorsement.
        /// </summary>
        /// <param name="travelEndorsement">Travel endorsement details.</param>
        /// <returns>Travelendorsementid, Travelendorsementnumber.</returns>
        [HttpPost]
        [Route(URI.TravelEndorsementURI.PostTravelEndorsement)]
        public RR.TravelEndorsementResponse PostTravelendorsement(RR.TravelEndorsement policy)
        {
            try
            {
                BLO.TravelEndorsement details = _mapper.Map<RR.TravelEndorsement, BLO.TravelEndorsement>(policy);
                BLO.TravelEndorsementResponse result = _travelEndorsementRepository.PostTravelEndorsement(details);
                return _mapper.Map<BLO.TravelEndorsementResponse, RR.TravelEndorsementResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.TravelEndorsementResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Check the if the policy already have saved endorsement,if it is there don't allow to pass the new endorsement.
        /// </summary>
        /// <param name="req">Endorsement precheck request.</param>
        /// <returns>Returns there an endorsemnt with saved staus or not.</returns>
        [HttpPost]
        [Route(URI.TravelEndorsementURI.EndorsementPreCheck)]
        public RR.TravelEndorsementPreCheckResponse EndorsementPrecheck(RR.TravelEndorsementPreCheckRequest req)
        {
            try
            {
                BLO.TravelEndorsementPreCheckRequest details = _mapper.Map<RR.TravelEndorsementPreCheckRequest, BLO.TravelEndorsementPreCheckRequest>(req);
                var result = _travelEndorsementRepository.EndorsementPrecheck(details);
                return _mapper.Map<BLO.TravelEndorsementPreCheckResponse, RR.TravelEndorsementPreCheckResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.TravelEndorsementPreCheckResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }


        /// <summary>
        /// Get all the travel endorsement details for the specific policy.
        /// To show the top of the page on any travel endorsement page.
        /// </summary>
        /// <param name="request">Endorsement request.</param>
        /// <returns>list of endorsemnt details.</returns>
        [HttpPost]
        [Route(URI.TravelEndorsementURI.GetAllEndorsements)]
        public RR.TravelEndoResponse GetAllEndorsements(RR.TravelEndoRequest req)
        {
            try
            {
                BLO.TravelEndoRequest details = _mapper.Map<RR.TravelEndoRequest,BLO.TravelEndoRequest>(req);
                var result = _travelEndorsementRepository.GetAllEndorsements(details);
                return _mapper.Map<BLO.TravelEndoResponse,RR.TravelEndoResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.TravelEndoResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        ///Travel endorsement operation - authorize or delete.
        /// </summary>
        /// <param name="request">Endorsement operation request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(URI.TravelEndorsementURI.EndorsementOperation)]
        public RR.TravelEndorsementOperationResponse EndorsementOperation(RR.TravelEndorsementOperation req)
        {
            try
            {
                BLO.TravelEndorsementOperation details = _mapper.Map<RR.TravelEndorsementOperation,BLO.TravelEndorsementOperation>(req);
                var result = _travelEndorsementRepository.EndorsementOperation(details);
                return _mapper.Map<BLO.TravelEndorsementOperationResponse,RR.TravelEndorsementOperationResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.TravelEndorsementOperationResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
    }
}