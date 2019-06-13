using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL
{
    public static class  OracleDateFormat
    {
        public static DateTime ConvertToOracleSPDateInput(this DateTime date)
        {
            string dateTimeFormat = date.ToString("yyyy-MM-dd");
            return DateTime.ParseExact(dateTimeFormat, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static DateTime ConvertToOracleSPDateTimeInput(this DateTime date)
        {
            string dateTimeFormat = date.ToString("yyyy-MM-dd HH:mm:ss");
            return DateTime.ParseExact(dateTimeFormat, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static DateTime ConvertLocalToServerFormat(this DateTime date)
        {
            return DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static DateTime ConvertToInvitaDateTimeFormat(this DateTime date)
        {
            string dateTimeFormat = date.ToString("dd/MM/yyyy HH:mm:ss");
            DateTime convertedValue = DateTime.ParseExact(dateTimeFormat, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            return convertedValue;
        }

        public static string ConvertToInvDateTimeFormat(this DateTime? date)
        {
            if(date == null)
            {
                return "";
            }
            return date.Value.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static DateTime AddEndOfDayTime(this DateTime datetime)
        {
            return datetime.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public static string ConvertToYYYYMMDDHH(this DateTime datetime)
        {
            return datetime.ToString("YYYY-MM-dd HH:mm:ss");
        }

    }
}
