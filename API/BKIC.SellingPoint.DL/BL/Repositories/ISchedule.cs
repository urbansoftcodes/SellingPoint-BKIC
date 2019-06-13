using BKIC.SellingPoint.DL.BO;
using System;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface ISchedule
    {
        DownloadScheduleResponse GetScheduleFilePath(DownloadScheuleRequest request);
        DownloadScheduleResponse GetProposalFilePath(DownloadScheuleRequest request);
    }
}