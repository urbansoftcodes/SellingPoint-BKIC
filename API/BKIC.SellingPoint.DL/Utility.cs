using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace BKIC.SellingPoint.DL
{
    public static class Utility
    {
        //public static string mailfrom = ConfigurationManager.AppSettings["mailid"].ToString();
        //public static string username = ConfigurationManager.AppSettings["UserName"].ToString();
        //public static string password = ConfigurationManager.AppSettings["Password"].ToString();
        //public static string connectionString = ConfigurationManager.AppSettings["DBConnection"].ToString();
        public static string sgApiKey = ConfigurationManager.AppSettings["SGAPIKey"].ToString();

        public static string sgFromRecipientEmail = ConfigurationManager.AppSettings["SGFromRecipientEmail"].ToString();
        public static string sgFromRecipientEmailName = ConfigurationManager.AppSettings["SGFromRecipientEmailName"].ToString();
        public static string sgTest = ConfigurationManager.AppSettings["SGTest"].ToString();
        public static string sgTestMail = ConfigurationManager.AppSettings["SGTestMail"].ToString();
        //public static string frontWebUI = ConfigurationManager.AppSettings["FrontWebApplicationURI"].ToString();
        //public static string APIURI = ConfigurationManager.AppSettings["WebApiUri"].ToString();
        //public static string CommercialEmail = ConfigurationManager.AppSettings["GroupMail"].ToString();
        //public static string MarkettingGroup = ConfigurationManager.AppSettings["MarkettingGroup"].ToString();
        //public static string MedicalGroup = ConfigurationManager.AppSettings["MedicalGroup"].ToString();
        //public static string MarineGroup = ConfigurationManager.AppSettings["MarineGroup"].ToString();
        //public static string InsurancePurchaseCCRecipient = ConfigurationManager.AppSettings["InsurancePurchaseCCRecipient"].ToString();
        //public static string ClaimIntimation = ConfigurationManager.AppSettings["ClaimIntimation"].ToString();
        //public static string ClaimTestUser = ConfigurationManager.AppSettings["SGClaimsUser"].ToString();
        //public static string SupportGroupMails = ConfigurationManager.AppSettings["supportgroupmail"].ToString();

        public static List<T> BindList<T>(this DataTable dt)
        {
            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                try
                {
                    foreach (DataColumn column in dr.Table.Columns)
                    {
                        foreach (PropertyInfo pro in temp.GetProperties())
                        {
                            if (pro.Name.ToLower() == column.ColumnName.ToLower())
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dr[column.ColumnName])))
                                    pro.SetValue(obj, dr[column.ColumnName], null);
                            }
                            else
                                continue;
                        }
                    }
                }
                catch (Exception exc)
                {
                }

                lst.Add(obj);
            }

            return lst;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static void LogEvent(string message)
        {
            try
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    message = "BKIC Selleing Point message \n\n " + message;
                    eventLog.Source = "BKICSellingPoint";
                    eventLog.WriteEntry(message, EventLogEntryType.Information, 101, 1);
                }
            }
            catch (Exception exc)
            {
            }
        }
        public static int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = 0;
            if (birthDate != null)
            {
                age = now.Year - birthDate.Year;
                if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                    age--;

                return age;
            }
            return age;
        }

        public static string GetEndorsementType(string endorsementName, MotorInsurancePolicy motorInsurancePolicy)
        {
            if(endorsementName == Constants.MotorEndorsementTypesNames.AddRemoveBank)
            {
                if(string.IsNullOrEmpty(motorInsurancePolicy.FinancierCompanyCode))
                {
                    return "Remove-Bank";
                }
                else
                {
                    return "Add-Bank";
                }
            }
            else if(endorsementName == Constants.MotorEndorsementTypesNames.AddCover)
            {
                return "Add-Cover";
            }
            else if(endorsementName == Constants.MotorEndorsementTypesNames.CancelPolicy)
            {
                return "Cancel-Policy";
            }
            else if(endorsementName == Constants.MotorEndorsementTypesNames.ChangeExess)
            {
                return "Change-Excess";
            }
            else if(endorsementName == Constants.MotorEndorsementTypesNames.ChangePremium)
            {
                return "Change-Premium";
            }
            else if(endorsementName == Constants.MotorEndorsementTypesNames.ChangeRegistration)
            {
                return "Change-Registration";
            }
            else if(endorsementName == Constants.MotorEndorsementTypesNames.ChangeSumInsured)
            {
                return "Change-Suminsured";
            }
            else if (endorsementName == Constants.MotorEndorsementTypesNames.Extended)
            {
                return "Extension";
            }
            else if (endorsementName == Constants.MotorEndorsementTypesNames.InternalEndorsement)
            {
                return "Internal";
            }
            return String.Empty;
        }

        public static string GetHomeEndorsementType(string endorsementName, HomeInsurancePolicy homeInsurancePolicy)
        {
            if (endorsementName == Constants.HomeEndorsementTypesNames.AddRemoveBank)
            {
                if (string.IsNullOrEmpty(homeInsurancePolicy.FinancierCode))
                {
                    return "Remove-Bank";
                }
                else
                {
                    return "Add-Bank";
                }
            }
            else if (endorsementName == Constants.HomeEndorsementTypesNames.AddRemoveDomesticHelp)
            {
                return "Add-DomesticHelp";
            }
            else if (endorsementName == Constants.HomeEndorsementTypesNames.CancelPolicy)
            {
                return "Cancel-Policy";
            }
            else if (endorsementName == Constants.HomeEndorsementTypesNames.ChangeAddress)
            {
                return "Change-Address";
            }           
            else if (endorsementName == Constants.MotorEndorsementTypesNames.ChangeSumInsured)
            {
                return "Change-Suminsured";
            }            
            return String.Empty;
        }

        public static string GetTravelEndorsementType(string endorsementName, TravelInsurancePolicy travelInsurancePolicy)
        {
            if (endorsementName == Constants.TravelEndorsementTypesNames.CancelPolicy)
            {
                return "Cancel-Policy";
            }
            else if (endorsementName == Constants.TravelEndorsementTypesNames.ChangePremium)
            {
                return "Change-Premium";
            }
            else if (endorsementName == Constants.TravelEndorsementTypesNames.ChangeMemeber)
            {
                return "Change-Member";
            }           
            return String.Empty;
        }
    }
}