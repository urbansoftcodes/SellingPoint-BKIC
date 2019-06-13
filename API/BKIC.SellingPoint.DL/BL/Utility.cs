using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Net.Mail;
using System.Reflection;
using System.Web;

namespace BKIC.DL
{
    public static class Utility
    {
        public static string mailfrom = ConfigurationManager.AppSettings["mailid"].ToString();
        public static string username = ConfigurationManager.AppSettings["UserName"].ToString();
        public static string password = ConfigurationManager.AppSettings["Password"].ToString();
        public static string connectionString = ConfigurationManager.AppSettings["DBConnection"].ToString();
        public static string sgApiKey = ConfigurationManager.AppSettings["SGAPIKey"].ToString();
        public static string sgFromRecipientEmail = ConfigurationManager.AppSettings["SGFromRecipientEmail"].ToString();
        public static string sgFromRecipientEmailName = ConfigurationManager.AppSettings["SGFromRecipientEmailName"].ToString();
        public static string sgTest = ConfigurationManager.AppSettings["SGTest"].ToString();
        public static string sgTestMail = ConfigurationManager.AppSettings["SGTestMail"].ToString();
        public static string frontWebUI = ConfigurationManager.AppSettings["FrontWebApplicationURI"].ToString();
        public static string APIURI = ConfigurationManager.AppSettings["WebApiUri"].ToString();
        public static string CommercialEmail = ConfigurationManager.AppSettings["GroupMail"].ToString();
        public static string MarkettingGroup = ConfigurationManager.AppSettings["MarkettingGroup"].ToString();
        public static string MedicalGroup = ConfigurationManager.AppSettings["MedicalGroup"].ToString();
        public static string MarineGroup = ConfigurationManager.AppSettings["MarineGroup"].ToString();
        public static string InsurancePurchaseCCRecipient = ConfigurationManager.AppSettings["InsurancePurchaseCCRecipient"].ToString();

        public static string AwsSecretKey = ConfigurationManager.AppSettings["AwsSecretKey"].ToString();
        public static string AwsAccessKey = ConfigurationManager.AppSettings["AwsAccessKey"].ToString();
        public static string AwsFromRecipientEmail = ConfigurationManager.AppSettings["AwsFromRecipientEmail"].ToString();


        #region MD5Encryption
        public static string MD5Encryption(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }
        #endregion

        #region Send Email
        public static void Sendmail(string To, string subject, string Body)
        {
            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.EnableSsl = true;

            sc.Credentials = new System.Net.NetworkCredential(username, password);
            try
            {
                MailMessage msg = new MailMessage();
                string frommail = mailfrom;

                string mailbodyval = Body;
                System.Net.Mail.MailAddress frommsg = new System.Net.Mail.MailAddress(frommail, "HRMS");
                msg.From = frommsg;
                msg.To.Add(new MailAddress(To));
                msg.Subject = subject;
                msg.Body = mailbodyval.ToString();

                msg.IsBodyHtml = true;
                sc.Send(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public static string getUserFullName(string firstName,string lastName)
        {
            return firstName + " " + lastName;
        }

        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

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
                            if (pro.Name == column.ColumnName)
                            {
                                if(!string.IsNullOrEmpty( Convert.ToString(dr[column.ColumnName])))
                                pro.SetValue(obj, dr[column.ColumnName], null);
                            }
                            else
                                continue;
                        }
                    }
                }
                catch(Exception exc)
                {

                }

                lst.Add(obj);
            }


            return lst;
        }

        public static void LogEvent(string message)
        {
            try
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    message = "BKIC Error message \n\n " + message;
                    eventLog.Source = "BKIC-SellingPoint";
                    eventLog.WriteEntry(message, EventLogEntryType.Information, 101, 1);
                }
            }
            catch (Exception exc)
            {

            }
        }

        public static int CalculateAgeCorrect(DateTime dob)
        {
            int age = DateTime.Now.Year - dob.Year;

            if (DateTime.Now.Month < dob.Month || (DateTime.Now.Month == dob.Month && DateTime.Now.Day < dob.Day))
                age--;

            return age;
        }


    }
}