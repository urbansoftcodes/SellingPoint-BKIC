using System.Data;

namespace BKIC.SellingPoint.DL.BO
{
    public class PageType
    {
        public const string Profile = "UserProfile";
        public const string TravelQuote = "TravelQuote";
        public const string TravelInsurance = "TravelInsurance";
        public const string MotorQuote = "MotorQuote";
        public const string MotorInsurance = "MotorInsurance";
        public const string HomeQuote = "HomeQuote";
        public const string HomeInsurance = "HomeInsurance";
        public const string DomesticHelp = "DomesticHelpInsurance";
    }

    public class DropDownResult : TransactionWrapper
    {
        public DataSet DataSets { get; set; }
    }

    public class VehicleModelResponse : TransactionWrapper
    {
        public DataTable VehicleModeldt { get; set; }        
    }

    public class VehicleBodyResponse : TransactionWrapper
    {
        public DataTable VehicleBodydt { get; set; }
        public DataTable VehicleEngineCCdt { get; set; }
    }
}