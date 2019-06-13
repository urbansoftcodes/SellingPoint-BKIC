using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class MotorEndorsementSP
    {
        public const string GetQuote = "CalculateMotorEndorsement";
        public const string GetAdminQuote = "CalculateAdminMotorEndorsement";
        public const string PostMotorEndorsement = "SP_InsertMotorEndorsement";
        public const string PostAdminMotorEndorsement = "SP_InsertAdminMotorEndorsement";             
        public const string EndorsementOperation = "SP_MotorEndorsementOperation";
    }
}
