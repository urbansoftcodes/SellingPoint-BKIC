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
    /// 
    /// </summary>
    public class ReportController : ApiController
    {
        public readonly IReport _ReportRepo;
        private readonly AutoMapper.IMapper _mapper;

        public ReportController(IReport repository)
        {
            _ReportRepo = repository;
            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetReportAutoMapper();
        }

        /// <summary>
        /// Get motor report based on type(Report by age or branch or user)
        /// </summary>
        /// <param name="reportReq">Report request.</param>
        /// <returns>Report details.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.ReportURI.GetMotorReport)]
        public RR.MotorReportResponse GetMotorReport(RR.AdminFetchReportRequest reportRequest)
        {
            try
            {
                BLO.AdminFetchReportRequest request = _mapper.Map<RR.AdminFetchReportRequest, BLO.AdminFetchReportRequest>(reportRequest);
                BLO.MotorReportResponse result = _ReportRepo.GetMotorReport(request);
                return _mapper.Map<BLO.MotorReportResponse, RR.MotorReportResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.MotorReportResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
            
        }

        /// <summary>
        ///  Get home report based on type(Report by branch or user)
        /// </summary>
        /// <param name="reportRequest">Report request.</param>
        /// <returns>Report details.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.ReportURI.GetHomeReport)]
        public RR.TravelHomeReportResponse GetHomeReport(RR.AdminFetchReportRequest reportRequest)
        {
            try
            {
                BLO.AdminFetchReportRequest request = _mapper.Map<RR.AdminFetchReportRequest,BLO.AdminFetchReportRequest>(reportRequest);
                BLO.TravelHomeReportResponse result = _ReportRepo.GetHomeReport(request);
                return _mapper.Map<BLO.TravelHomeReportResponse, RR.TravelHomeReportResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.TravelHomeReportResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
            
        }

        /// <summary>
        ///  Get travel report based on type(Report by branch or user)
        /// </summary>
        /// <param name="reportRequest">Report request.</param>
        /// <returns>Report details.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.ReportURI.GetTravelReport)]
        public RR.TravelHomeReportResponse GetTravelReport(RR.AdminFetchReportRequest reportRequest)
        {
            try
            {
                BLO.AdminFetchReportRequest request = _mapper.Map<RR.AdminFetchReportRequest, BLO.AdminFetchReportRequest>(reportRequest);
                BLO.TravelHomeReportResponse result = _ReportRepo.GetTravelReport(request);
                return _mapper.Map<BLO.TravelHomeReportResponse, RR.TravelHomeReportResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.TravelHomeReportResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
            
        }

        /// <summary>
        ///  Get main report based on insurance type(Motor or Home or Branch)
        /// </summary>
        /// <param name="reportRequest">Report request.</param>
        /// <returns>Report details.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(URI.ReportURI.GetMainReport)]
        public RR.MainReportResponse GetMainReport(RR.AdminFetchReportRequest reportRequest)
        {
            try
            {
                BLO.AdminFetchReportRequest request = _mapper.Map<RR.AdminFetchReportRequest, BLO.AdminFetchReportRequest>(reportRequest);
                BLO.MainReportResponse result = _ReportRepo.GetMainReport(request);
                return _mapper.Map<BLO.MainReportResponse, RR.MainReportResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.MainReportResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
            
        }
    }
}