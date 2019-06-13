using BKIC.SellingPoint.DL.BL.Implematon;
using BKIC.SellingPoint.DL.BO;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BKIC.SellingPoint.DL.BL.Implementation
{
    /// <summary>
    /// HelperClass for appliction useage.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Convert the datetime to string format.
        /// </summary>
        /// <param name="dt">DateTime.</param>
        /// <returns>String datetime.</returns>
        public static string ConvertTodatestring(this DateTime dt)
        {
            if (dt != null)
            {
                string DateTime = Convert.ToString(dt);
                string[] date = DateTime.Split();
                return date[0];
            }
            return "";
        }

        /// <summary>
        /// Delete the file if it is already exists.
        /// </summary>
        /// <param name="PathWithFileName">Actual file path</param>
        public static void DeletFile(this string PathWithFileName)
        {
            if (System.IO.Directory.Exists(PathWithFileName))
            {
                System.IO.Directory.Delete(PathWithFileName);
            }
        }

        /// <summary>
        /// Convert the given datetime into local format('dd/MM/yyyy').
        /// </summary>
        /// <param name="value">Actual date.</param>
        /// <returns>String date format.</returns>
        public static string CovertToLocalFormat(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Convert the given datetime into local format('dd/MM/yyyy hh:mm:ss tt').
        /// </summary>
        /// <param name="value">Actual date.</param>
        /// <returns>String date format.</returns>
        public static string CovertToLocalFormatTime(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        /// <summary>
        /// Get copied file path with file name.
        /// </summary>
        /// <param name="sourceFilePath">Source file path.</param>
        /// <param name="destinationFilePath">Destination file path.</param>
        /// <returns>Returns copied file path with file name.</returns>        
        public static string CopyFile(string sourceFilePath, string destinationFilePath)
        {
            if (System.IO.File.Exists(sourceFilePath))
            {
                if (System.IO.File.Exists(destinationFilePath))
                {
                    System.IO.File.Delete(destinationFilePath);
                }
                System.IO.File.Copy(sourceFilePath, destinationFilePath);
                return destinationFilePath;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Copy the company logos's
        /// </summary>
        /// <param name="sourceFilePath">Source file path.</param>
        /// <param name="destinationFilePath">Destination file path.</param>
        public static void CopyCompanyLogos(string sourceFilePath, string destinationFileDirectory)
        {
            try
            {
                if (System.IO.File.Exists(destinationFileDirectory + "BKIC-logo.png"))
                {
                    System.IO.File.Delete(destinationFileDirectory + "BKIC-logo.png");
                }

                if (System.IO.File.Exists(destinationFileDirectory + "BKIC-stamp.png"))
                {
                    System.IO.File.Delete(destinationFileDirectory + "BKIC-stamp.png");
                }

                System.IO.File.Copy(sourceFilePath + "BKIC-logo.png", destinationFileDirectory + "BKIC-logo.png");
                System.IO.File.Copy(sourceFilePath + "BKIC-stamp.png", destinationFileDirectory + "BKIC-stamp.png");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create the motor certificate.
        /// </summary>
        /// <param name="motordetails">Motor policy details.</param>
        /// <returns>Returns saved file path.</returns>
        public static string CreateMotorInvoice(InsuranceCertificateResponse motordetails)
        {
            string FileName = motordetails.PolicyNo + ".pdf";
            string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + motordetails.InsuredCode + "/Certificate/" + motordetails.PolicyNo + "/";

            if (!System.IO.Directory.Exists(FileSavePath))
            {
                System.IO.Directory.CreateDirectory(FileSavePath);
            }

            string FullPath = FileSavePath + FileName;
            var applicationPath = AppDomain.CurrentDomain.BaseDirectory + "/Templates/";
            string insuranceperoid = motordetails.CommencementDate.CovertToLocalFormat() + " To " + motordetails.ExpiryDate.CovertToLocalFormat();
            string vehicleMakeModel = motordetails.VehicleMake + "/" + motordetails.VehicleModel;
            string htmlCode = File.ReadAllText(ExtensionMethods.CopyFile(applicationPath + "Invoice-ByGIRISH.html", FileSavePath + "Invoice-ByGIRISH.html"));

            string coverCodes = string.Empty;
            if (motordetails.Covers != null)
            {
                for (int i = 0; i < motordetails.Covers.Count; i++)
                {                   
                    coverCodes += motordetails.Covers[i].CoverCode.ToLower() + ", ";
                }
                //Optional covers.
                if (motordetails.OptionalCovers != null && motordetails.OptionalCovers.Count > 0)
                {
                    for (int i = 0; i < motordetails.OptionalCovers.Count; i++)
                    {
                        coverCodes += motordetails.OptionalCovers[i].CoverCode.ToLower() + ", ";
                    }
                }
                coverCodes = coverCodes.TrimStart(' ').TrimEnd(' ').TrimStart(',').Trim(',');
            }           

            htmlCode = htmlCode.Replace("{{PolicyNo}}", motordetails.PolicyNo)
                        .Replace("{{InsuredName}}", motordetails.InsuredName)
                        .Replace("{{PolicyCover}}", motordetails.PolicyCode)
                        //.Replace("{{OptionalCover}}", string.IsNullOrEmpty(motordetails.AdditionalBenefits) ? "" : motordetails.AdditionalBenefits)
                        .Replace("{{InsurancePeriod}}", insuranceperoid)
                        .Replace("{{Make/Model}}", vehicleMakeModel)
                        .Replace("{{RegistrationNo}}", motordetails.RegistrationNo)
                        .Replace("{{CreatedDate}}", motordetails.CreatedDate.CovertToLocalFormat())
                        .Replace("{{VehicleValue}}", Convert.ToString(motordetails.VehicleValue))
                        .Replace("{{InsuredCode}}", motordetails.InsuredCode)
                        .Replace("{{PaymentAuth}}", string.IsNullOrEmpty(motordetails.PaymentAuthCode) ? "" : motordetails.PaymentAuthCode)
                        .Replace("{{BuildingNo}}", motordetails.BuilidingNo)
                        .Replace("{{Premium}}", Convert.ToString(motordetails.Premium))
                        .Replace("{{Excess}}", Convert.ToString(motordetails.ExcessAmount))
                        .Replace("{{VechicleMake}}", Convert.ToString(motordetails.VehicleMake))
                        .Replace("{{RegNo}}", motordetails.RegistrationNo)
                        .Replace("{{ChassisNo}}", motordetails.ChassisNo)
                        .Replace("{{IssuedDate}}", motordetails.CommencementDate.CovertToLocalFormat())
                        .Replace("{{Year}}", motordetails.YearOfMake)
                        .Replace("{{Finance}}", motordetails.FinanceCompany)
                        .Replace("{{OptionalCover}}", coverCodes);

            if (System.IO.File.Exists(FullPath))
            {
                System.IO.File.Delete(FullPath);
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(htmlCode);
            MemoryStream stream = new MemoryStream(byteArray);
            HtmlToPdf.GenerateInvoice(stream, FullPath, "", "");

            return FullPath;
        }

        /// <summary>
        /// Replace the specified text in the pdf document.
        /// </summary>
        /// <param name="documents">Pdf document.</param>
        /// <param name="dictionary">Text with key and values for replace.</param>
        /// <returns>Pdf document.</returns>
        public static PdfDocument FintTextInPDFAndReplaceIt(PdfDocument documents, Dictionary<string, string> dictionary)
        {
            PdfTextFind[] result = null;
            foreach (var word in dictionary)
            {
                foreach (PdfPageBase page in documents.Pages)
                {
                    result = page.FindText(word.Key).Finds;
                    foreach (PdfTextFind find in result)
                    {
                        find.ApplyRecoverString(word.Value);
                        //replace word in pdf
                        // find.ApplyRecoverString(word.Value, System.Drawing.Color.White, true);
                    }
                }
            }

            return documents;
        }
    }
}