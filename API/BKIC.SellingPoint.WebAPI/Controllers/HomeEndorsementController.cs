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
    public class HomeEndorsementController : ApiController
    {
        private readonly IHomeEndorsement _homeEndorsementRepository;

        private readonly AutoMapper.IMapper _mapper;

        public HomeEndorsementController(IHomeEndorsement homeEndorsement)
        {
            _homeEndorsementRepository = homeEndorsement;

            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetHomeEndorsementAutoMapper();
        }

        /// <summary>
        /// Calculate home endorsement premium.
        /// </summary>
        /// <param name="homeEndorsementQuote">Home endorsement quote request.</param>
        /// <returns>Endorsement premium,Endorsement commission.</returns>
        [HttpPost]
        [Route(URI.HomeEndorsementURI.GetHomeEndorsementQuote)]
        public RR.HomeEndorsementQuoteResponse GetQuote(RR.HomeEndorsementQuote homeEndorsementQuote)
        {
            try
            {
                BLO.HomeEndorsementQuote details = _mapper.Map<RR.HomeEndorsementQuote, BLO.HomeEndorsementQuote>(homeEndorsementQuote);
                BLO.HomeEndorsementQuoteResponse result = _homeEndorsementRepository.GetHomeEndorsementQuote(details);
                return _mapper.Map<BLO.HomeEndorsementQuoteResponse, RR.HomeEndorsementQuoteResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeEndorsementQuoteResponse
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
        /// <returns>Homeendorsementid, Homeendorsementnumber.</returns>
        [HttpPost]
        [Route(URI.HomeEndorsementURI.PostHomeEndorsement)]
        public RR.HomeEndorsementResponse PostHomePolicy(RR.HomeEndorsement homeEndorsement)
        {
            try
            {
                BLO.HomeEndorsement details = _mapper.Map<RR.HomeEndorsement, BLO.HomeEndorsement>(homeEndorsement);
                BLO.HomeEndorsementResponse result = _homeEndorsementRepository.PostHomeEndorsement(details);
                return _mapper.Map<BLO.HomeEndorsementResponse, RR.HomeEndorsementResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeEndorsementResponse
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
        [Route(URI.HomeEndorsementURI.EndorsementPreCheck)]
        public RR.HomeEndorsementPreCheckResponse EndorsementPrecheck(RR.HomeEndorsementPreCheckRequest req)
        {
            try
            {
                BLO.HomeEndorsementPreCheckRequest details = _mapper.Map<RR.HomeEndorsementPreCheckRequest, BLO.HomeEndorsementPreCheckRequest>(req);
                var result = _homeEndorsementRepository.EndorsementPrecheck(details);
                return _mapper.Map<BLO.HomeEndorsementPreCheckResponse, RR.HomeEndorsementPreCheckResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeEndorsementPreCheckResponse
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
        [HttpPost]
        [Route(URI.HomeEndorsementURI.GetAllEndorsements)]
        public RR.HomeEndoResponse GetAllEndorsements(RR.HomeEndoRequest req)
        {
            try
            {
                BLO.HomeEndoRequest details = _mapper.Map<RR.HomeEndoRequest, BLO.HomeEndoRequest>(req);
                var result = _homeEndorsementRepository.GetAllEndorsements(details);
                return _mapper.Map<BLO.HomeEndoResponse,RR.HomeEndoResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeEndoResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }


        /// <summary>
        ///Home endorsement operation - authorize or delete.
        /// </summary>
        /// <param name="request">Endorsement operation request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(URI.HomeEndorsementURI.EndorsementOperation)]
        public RR.HomeEndorsementOperationResponse EndorsementOperation(RR.HomeEndorsementOperation req)
        {
            try
            {
                BLO.HomeEndorsementOperation details = _mapper.Map<RR.HomeEndorsementOperation, BLO.HomeEndorsementOperation>(req);
                var result = _homeEndorsementRepository.EndorsementOperation(details);
                return _mapper.Map<BLO.HomeEndorsementOperationResponse, RR.HomeEndorsementOperationResponse>(result);                
            }
            catch (Exception ex)
            {
                return new RR.HomeEndorsementOperationResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Calculate endorsement premium for domestic help add.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(URI.HomeEndorsementURI.GetHomeDomesticHelpEndorsementQuote)]
        public RR.HomeEndorsementQuoteResponse GetHomeDomesticHelpEndorsementQuote(RR.HomeEndorsementDomesticHelpQuote req)
        {
            try
            {
                BLO.HomeEndorsementDomesticHelpQuote details = _mapper.Map<RR.HomeEndorsementDomesticHelpQuote,BLO.HomeEndorsementDomesticHelpQuote>(req);
                var result = _homeEndorsementRepository.GetHomeDomesticHelpQuote(details);
                return _mapper.Map<BLO.HomeEndorsementQuoteResponse, RR.HomeEndorsementQuoteResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeEndorsementQuoteResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
    }
}
