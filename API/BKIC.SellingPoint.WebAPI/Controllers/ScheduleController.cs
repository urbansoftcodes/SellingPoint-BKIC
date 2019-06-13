using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DTO.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BLO = BKIC.SellingPoint.DL.BO;
using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;

namespace BKIC.SellingPoint.WebAPI.Controllers
{
    public class ScheduleController : ApiController
    {
        public readonly ISchedule _scheduleRepo;

        public ScheduleController(ISchedule repository)
        {
            _scheduleRepo = repository;
        }

        /// <summary>
        /// Get the schedule for specific insurance type (Motor or Home or Travel or Domestic)
        /// </summary>
        /// <param name="insuranceType">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="documentNo">Document number.</param>
        /// <param name="isEndorsement">Schedule for endorsement page or policy buy page.</param>
        /// <param name="EndorsementID">Endorsement id.</param>
        /// <returns></returns>
        [HttpGet]
        [Route(ScheduleURI.downloadschedule)]
        public HttpResponseMessage DownloadScheduleFile(string insuranceType, string agentCode, string documentNo,
                                   bool isEndorsement, long EndorsementID, int RenewalCount)
        {
            DownloadScheuleRequest request = new DownloadScheuleRequest();
            request.DocNo = documentNo;
            request.InsuranceType = insuranceType;
            request.AgentCode = agentCode;
            request.EndorsementID = EndorsementID;
            request.IsEndorsement = isEndorsement;
            request.RenewalCount = RenewalCount;

            DownloadScheduleResponse result = _scheduleRepo.GetScheduleFilePath(request);
            String FileName = Path.GetFileName(result.FilePath);
            HttpResponseMessage response = new HttpResponseMessage();

            if (System.IO.File.Exists(result.FilePath))
            {
                FileStream stream = File.OpenRead(result.FilePath);

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = FileName;
                //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Utility.GetMimeType(FileName));//new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return response;
            }
            return response = new HttpResponseMessage(HttpStatusCode.Gone);
        }  

        /// <summary>
        /// Download proposal file.
        /// </summary>
        /// <param name="insuranceType">Insurance type.</param>
        /// <param name="agentCode">Agent code.</param>
        /// <param name="documentNo">Document number.</param>
        /// <returns></returns>
        [HttpGet]
        [Route(ScheduleURI.downloadproposal)]
        public HttpResponseMessage DownloadProposalFile(string insuranceType, string agentCode, string documentNo, int renewalCount = 0)
        {
            DownloadScheuleRequest request = new DownloadScheuleRequest();
            request.DocNo = documentNo;
            request.InsuranceType = insuranceType;
            request.AgentCode = agentCode;
            request.RenewalCount = renewalCount;

            DownloadScheduleResponse result = _scheduleRepo.GetProposalFilePath(request);
            String FileName = Path.GetFileName(result.FilePath);
            HttpResponseMessage response = new HttpResponseMessage();

            if (System.IO.File.Exists(result.FilePath))
            {
                FileStream stream = File.OpenRead(result.FilePath);

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = FileName;
                //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Utility.GetMimeType(FileName));//new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return response;
            }
            return response = new HttpResponseMessage(HttpStatusCode.Gone);
        }        

    }
}