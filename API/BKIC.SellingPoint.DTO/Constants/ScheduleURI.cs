namespace BKIC.SellingPoint.DTO.Constants
{
    public class ScheduleURI
    {
        public const string downloadschedule = "api/schedule/downloadschedule/{insuranceType}/{agentCode}/{documentNo}/{isEndorsement}/{endorsementID}/{renewalCount}"; 
        public const string downloadproposal = "api/schedule/downloadproposal/{insuranceType}/{agentCode}/{documentNo}/{renewalCount}";
        public const string downloadMotorEndorsementSchedule = "api/schedule/downloadmotorendorsementschedule/{documentNo}/{endorsementID}/{agentCode}";
    }
}