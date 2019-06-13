using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BKIC.SellingPoint.DL.BO;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IReport
    {
        MotorReportResponse GetMotorReport(AdminFetchReportRequest reportReq);
        TravelHomeReportResponse GetTravelReport(AdminFetchReportRequest reportReq);
        TravelHomeReportResponse GetHomeReport(AdminFetchReportRequest reportReq);
        MainReportResponse GetMainReport(AdminFetchReportRequest reportReq);
    }
}
