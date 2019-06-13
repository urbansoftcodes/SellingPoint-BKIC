namespace BKIC.SellingPoint.DL.BO
{
    public class Schedule
    {
        public class DownloadScheuleRequest
        {
            public string InsuranceType { get; set; }
            public int RefID { get; set; }
            public string InsuredCode { get; set; }
        }

        public class DownloadScheduleResponse : TransactionWrapper
        {
            public string FilePath { get; set; }
        }
    }
}