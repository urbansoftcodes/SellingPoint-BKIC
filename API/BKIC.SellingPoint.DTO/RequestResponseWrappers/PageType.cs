namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class PageType
    {
        //public PageType();
        public const string UserMaster = "UserMaster";

        public const string Travelnsurance = "TravelInsurance";
        public const string DomesticHelp = "DomesticHelp";
        public const string HomeInsurance = "HomeInsurance";
        public const string MotorInsurance = "MotorInsurance";
        public const string Profile = "UserProfile";
        public const string TravelQuote = "TravelQuote";
        public const string TravelInsurance = "TravelInsurance";
        public const string MotorQuote = "MotorQuote";
        public const string HomeQuote = "HomeQuote";
        public const string InsurancePortal = "InsurancePortal";
        public const string Reports = "Reports";
    }

    public class DropDownResult : TransactionWrapper
    {
        public string DataSets { get; set; }
    }

    public class VehicleModelResponse : TransactionWrapper
    {
        public string VehicleModeldt { get; set; }
    }

    public class VehicleBodyResponse : TransactionWrapper
    {
        public string VehicleBodydt { get; set; }
        public string VehicleEngineCCdt { get; set; }
    }
}