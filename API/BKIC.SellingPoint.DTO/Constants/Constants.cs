using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPont.DTO.Constants
{    
    public static class HttpContentType
    {
        public const string JsonContentType = "application/json";
        public const string FormURLEncodedContentType = "application/x-www-form-urlencoded";
    }

    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string BranchAdmin = "BranchAdmin";
        public const string User = "User";

    }

    public static class Insurance
    {
        public const string Motor = "MotorInsurance";
        public const string Home = "HomeInsurance";
        public const string Travel = "TravelInsurance";
        public const string DomesticHelp = "DomesticInsurance";
    }
}
