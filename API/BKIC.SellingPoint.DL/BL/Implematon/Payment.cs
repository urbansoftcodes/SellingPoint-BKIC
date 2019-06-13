using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    public class Payment : IPayment
    {
        ////public readonly OracleDBIntegration.Implementations.TravelInsurance _oracleIntegration;
        //public readonly IMail _mail;
        //public Payment()
        //{
        //    _mail = new Mail();
        //}

        public PaymentTrackInsertResult InsertPaymentTrackDetails(Int64 RefID, string insuranceType, bool isrenew, string emailaddress = "")
        {
            try
            {
                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@TrackId", Size=50 }
                };

                SqlParameter[] para = new SqlParameter[] {
                   // new SqlParameter("@InsuredCode", insuredCode),
                  // new SqlParameter("@PolicyNo", policyNo),
                  //  new SqlParameter("@LinkId", linkId),
                    new SqlParameter("@RefID", RefID),
                    new SqlParameter("@InsuranceType",insuranceType),
                     new SqlParameter("@IsRenew",isrenew),
                     new SqlParameter("@EmailAddress",emailaddress)
                };

                object[] dataSet = BKICSQL.GetValues(StoredProcedures.Payment.InsertPaymentTrack, para, outParams);

                return new PaymentTrackInsertResult() { IsTransactionDone = true, TrackId = dataSet[0].ToString() };

            }
            catch (Exception exc)
            {
                return new PaymentTrackInsertResult() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }

        public PaymentTrackDetailResult GetPaymentTrackDetails(string trackId)
        {
            try
            {
                PaymentTrackDetailResult result = new PaymentTrackDetailResult();

                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@TrackId", trackId)
                };

                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@InsuredCode", Size = 50},
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@PolicyNo", Size = 50 },
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@LinkId", Size=50 },
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName = "@InsuranceType", Size=50 },
                    new SPOut() { OutPutType = SqlDbType.Decimal, ParameterName="@Amount", Precision=38, Scale =3 },
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName ="@IsTrackIdAvailable" },
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName = "@IsNotUsedAlready" },
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@UserName", Size = 100 },
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@Email", Size=50 }
                };

                object[] dataSet = BKICSQL.GetValues(StoredProcedures.Payment.PaymentBasicDetailsByTrackId, para, outParams);
                result.InsuredCode = string.IsNullOrEmpty(dataSet[0].ToString()) ? "" : dataSet[0].ToString();
                result.PolicyNo = string.IsNullOrEmpty(dataSet[1].ToString()) ? "" : dataSet[1].ToString();
                result.LinkId = string.IsNullOrEmpty(dataSet[2].ToString()) ? "" : dataSet[2].ToString();
                result.InsuredType = string.IsNullOrEmpty(dataSet[3].ToString()) ? "" : dataSet[3].ToString();
                result.Amount = string.IsNullOrEmpty(dataSet[4].ToString()) ? 0 : decimal.Parse(dataSet[4].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                result.IsTrackIdAvailable = dataSet[5].ToString() == "True" ? true : false;
                result.IsNotUsedAlready = dataSet[6].ToString() == "True" ? true : false;
                result.UserName = dataSet[7].ToString();
                result.Email = dataSet[8].ToString();
                result.IsTransactionDone = true;
                result.TrackId = trackId;

                return result;

            }
            catch (Exception exc)
            {
                return new PaymentTrackDetailResult() { IsTransactionDone = true, TransactionErrorMessage = exc.Message };
            }
        }

        public TransactionWrapper UpdatePaymentDetailsByTrackId(string trackId, string transactionNo, DateTime paymentDate,
            string paymentType, string paymentAuthorizationCode)
        {
            try
            {
                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@InsuredCode", Size = 50},
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@PolicyNo", Size = 50 },
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@LinkId", Size=50 },
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName = "@InsuranceType", Size=50 },
                     new SPOut() { OutPutType = SqlDbType.Int, ParameterName = "@RefID"},
                     new SPOut() { OutPutType = SqlDbType.Bit, ParameterName = "@IsRenewal"},
                     new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName = "@EmailAddress",Size=50}
                };

                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@TrackId", trackId),
                    new SqlParameter("@TransactionNo", transactionNo),
                    new SqlParameter("@PaymentDate", paymentDate),
                    new SqlParameter("@PaymentType", paymentType),
                    new SqlParameter("@PaymentAuthorizationCode",paymentAuthorizationCode)
                };

                object[] dataSet = BKICSQL.GetValues(StoredProcedures.Payment.UpdatePaymentDetailsByTrackId, para, outParams);

                string insuredCode = dataSet[0].ToString();
                string policyNo = dataSet[1].ToString();
                string linkId = dataSet[2].ToString();
                string insuranceType = dataSet[3].ToString();
                int RefID = Convert.ToInt32(dataSet[4]);
                bool IsRenewal = Convert.ToBoolean(dataSet[5]);
                string EmailAddress = Convert.ToString(dataSet[6]);

                if (IsRenewal)
                {
                    SqlParameter[] para1 = new SqlParameter[] {
                    new SqlParameter("@InsuredCode", insuredCode),
                    new SqlParameter("@DocumentNo", policyNo),
                    new SqlParameter("@LinkID", linkId),
                    new SqlParameter("@InsuranceType", insuranceType),
                    new SqlParameter("@RenewID",RefID),
                };
                    List<SPOut> outParams1 = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Int, ParameterName= "@RefID"}

                };


                    object[] dataSet1 = BKICSQL.GetValues(StoredProcedures.Payment.RenewPolicy, para1, outParams1);
                    if (dataSet1 != null)
                    {
                        RefID = Convert.ToInt32(dataSet1[0]);
                    }
                    new Task(() =>
                    {
                        BackendOracleIntegration(insuredCode, policyNo, insuranceType, RefID, true, EmailAddress);
                    }).Start();
                }
                else
                {

                    new Task(() =>
                    {
                        BackendOracleIntegration(insuredCode, policyNo, insuranceType, RefID);
                    }).Start();
                }
                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }

        public TransactionWrapper UpdatePGTrackIdByTrackId(string trackId, string pgTrackId)
        {
            try
            {
                List<SPOut> outParams = new List<SPOut>()
                {
                };

                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@TrackId", trackId),
                    new SqlParameter("@PGTrackId", pgTrackId)
                };

                object[] dataSet = BKICSQL.GetValues(StoredProcedures.Payment.UpdatePGTrackIdByTrackId, para, outParams);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = true, TransactionErrorMessage = exc.Message };
            }
        }

        public PaymentUpdatePreCheckResult PreCheckPaymentUpdate(string trackId, string pgTrackId)
        {
            try
            {
                PaymentUpdatePreCheckResult result = new PaymentUpdatePreCheckResult();

                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@TrackId", trackId),
                    new SqlParameter("@PGTrackId",pgTrackId)
                };

                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName ="@IsAvailable" },
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName = "@IsAlreadyUsed" }
                };

                object[] dataSet = BKICSQL.GetValues(StoredProcedures.Payment.PreCheckPaymentUpdate, para, outParams);
                result.IsTrackIdAvailable = dataSet[0].ToString() == "True" ? true : false;
                result.IsNotUsedAlready = dataSet[1].ToString() == "True" ? true : false;
                result.IsTransactionDone = true;

                return result;
            }
            catch (Exception exc)
            {
                return new PaymentUpdatePreCheckResult() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }




        public void BackendOracleIntegration(string insuredCode, string policyNo, string insuranceType, int refID, bool isRenew = false, string emailAddress = "")
        {
            try
            {
                if (insuranceType == Constants.Insurance.Travel)
                {
                    new Task(() =>
                    {
                        TravelInsurance travel = new TravelInsurance();
                        TravelSavedQuotationResponse travelresult = travel.GetSavedQuotationByTravelId(refID, insuredCode, Constants.Insurance.Travel);
                        //CreateTravelSchudles(travelresult);
                    }).Start();

                    //new Task(() =>
                    //{
                    //    OracleDBIntegration.Implementations.TravelInsurance _oracleTravelIntegration = new OracleDBIntegration.Implementations.TravelInsurance();
                    //    OracleDBIntegration.DBObjects.TransactionWrapper oracleResult = _oracleTravelIntegration.IntegrateTravelToOracle((int)refID);
                    //}).Start();
                }
                else if (insuranceType == Constants.Insurance.Motor)
                {
                //    new Task(() =>
                //    {
                //        MotorInsurance motor = new MotorInsurance();
                //        Bo.MotorSavedQuotationResult motorresult = motor.GetSavedMotorQuotation(refID, insuredCode);
                //        if (isRenew)
                //        {
                //            CreateMotorRenewalSchedule(motorresult.MotorPolicyDetails, emailAddress);
                //        }
                //        else
                //        {
                //            CreateMotorSchudles(motorresult.MotorPolicyDetails);
                //        }
                //    }).Start();
                //    //new Task(() =>
                //    //{
                //    //    OracleDBIntegration.Implementations.MotorInsurance _oracleMotorInsurance = new OracleDBIntegration.Implementations.MotorInsurance();
                //    //    OracleDBIntegration.DBObjects.TransactionWrapper oracleResult = _oracleMotorInsurance.IntegrateMotorToOracle((int)refID);
                //    //}).Start();

                }
                else if (insuranceType == Constants.Insurance.DomesticHelp)
                {
                    //new Task(() =>
                    //{
                    //    DomesticHelp domestic = new DomesticHelp();
                    //    Bo.DomesticHelpSavedQuotationResult domesticresult = domestic.GetSavedDomesticHelp(refID, insuredCode);
                    //    CreateDomesticSchudles(domesticresult);
                    //}).Start();

                    ////new Task(() =>
                    ////{
                    ////    OracleDBIntegration.Implementations.DomesticInsurance _oracleDomesticIntegration = new OracleDBIntegration.Implementations.DomesticInsurance();
                    ////    OracleDBIntegration.DBObjects.TransactionWrapper oracleResult = _oracleDomesticIntegration.IntegrateDomesticToOracle((int)refID);
                    ////}).Start();


                }
                else if (insuranceType == Constants.Insurance.Home)
                {
                    //new Task(() =>
                    //{
                    //    HomeInsurance home = new HomeInsurance();
                    //    HomeSavedQuotationResult homeresult = home.GetSavedQuotation(refID, insuredCode);
                    //    if (isRenew)
                    //    {
                    //        CreateHomeRenewalSchedule(homeresult, emailAddress);
                    //    }
                    //    else
                    //    {
                    //        CreateHomeSchudles(homeresult);
                    //    }

                    //}).Start();

                    ////new Task(() =>
                    ////{
                    ////    OracleDBIntegration.Implementations.HomeInsurance _oracleHomeIntegration = new OracleDBIntegration.Implementations.HomeInsurance();
                    ////    OracleDBIntegration.DBObjects.TransactionWrapper oracleResult = _oracleHomeIntegration.IntegrateHomeToOracle((int)refID);
                    ////}).Start();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


//        public string CreateHomeRenewalSchedule(HomeSavedQuotationResult home, String emailaddress)
//        {

//            try
//            {
//                string FileName = home.HomeInsurancePolicy.DocumentNo + ".pdf";
//                string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + home.HomeInsurancePolicy.InsuredCode + "/" + home.HomeInsurancePolicy.DocumentNo + "/";

//                if (!System.IO.Directory.Exists(FileSavePath))
//                {
//                    System.IO.Directory.CreateDirectory(FileSavePath);
//                }
//                else
//                {
//                    //ExtensionMethods.DeleteFilesInsideDirectory(FileSavePath);
//                }

//                string FullPath = FileSavePath + FileName;

//                ExtensionMethods.CopyCompanyLogos(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/", FileSavePath);

//                string htmlCode = File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/Home.html",
//                    FileSavePath + "Home.html"));
//                string DomesticWorkersDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/HomeDomestic.html",
//                    FileSavePath + "HomeDomestic.html"));
//                string HomesubItemsDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/HomeSingleItems.html",
//                    FileSavePath + "HomeSingleItems.html"));

//                PdfDocument pdf = new PdfDocument();
//                PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
//                htmlLayoutFormat.IsWaiting = false;
//                PdfPageSettings setting = new PdfPageSettings();
//                string memberslist = string.Empty;
//                string members = string.Empty;
//                string homeitems = string.Empty;
//                string homesubitems = string.Empty;
//                int WorkerCount = 1;
//                foreach (var list in home.DomesticHelp)
//                {
//                    members = DomesticWorkersDiv.Replace("{{Name}}", list.Name).Replace("{{DOB}}", list.DOB.CovertToLocalFormat())
//                           .Replace("{{CPR}}", list.CPR)
//                           .Replace("{{SumInsured}}", Convert.ToString(list.SumInsured))
//                           .Replace("{{WorkerCount}}", WorkerCount.ToString());


//                    memberslist += members;
//                    WorkerCount++;
//                }

//                foreach (var list in home.HomeSubItems)
//                {

//                    homeitems = HomesubItemsDiv.Replace("{{Item_description}}", list.Description).Replace("{{Item_Value}}",
//                        Convert.ToString(list.SumInsured));
//                    homesubitems += homeitems;
//                }
//                htmlCode = htmlCode.Replace("{{DomesticWorkersDiv}}", memberslist).Replace("{{PolicyNo}}", home.HomeInsurancePolicy.DocumentNo)
//                    .Replace("{{SingleItemsDiv}}", homesubitems)
//                    .Replace("{{InsuredName}}", home.HomeInsurancePolicy.InsuredName).Replace("{{FlatNo}}", home.HomeInsurancePolicy.FlatNo)
//                    .Replace("{{BlockNo}}", home.HomeInsurancePolicy.BlockNo).Replace("{{RoadNo}}", home.HomeInsurancePolicy.RoadNo).Replace("{{Town}}", home.HomeInsurancePolicy.Town)
//                    .Replace("{{BuildingAge}}", Convert.ToString(home.HomeInsurancePolicy.BuildingAge))
//                    .Replace("{{BuildingValue}}", Convert.ToString(home.HomeInsurancePolicy.BuildingValue))
//                    .Replace("{{ContentValue}}", Convert.ToString(home.HomeInsurancePolicy.ContentValue))
//                    .Replace("{{JewelleryCover}}", home.HomeInsurancePolicy.JewelleryCover)
//                    .Replace("{{TotalSumInsured}}", Convert.ToString((home.HomeInsurancePolicy.BuildingValue + home.HomeInsurancePolicy.ContentValue)))
//                    .Replace("{{PolicyStartDate}}", home.HomeInsurancePolicy.PolicyStartDate.CovertToLocalFormat())
//                    .Replace("{{ExpiryDate}}", home.HomeInsurancePolicy.PolicyExpiryDate.CovertToLocalFormat())
//                    .Replace("{{SumInsured}}", Convert.ToString(home.HomeInsurancePolicy.SumInsured))
//                    .Replace("{{Premium}}", Convert.ToString(home.HomeInsurancePolicy.PremiumAfterDiscount))
//                    .Replace("{{1}}", home.HomeInsurancePolicy.IsSafePropertyInsured == 'Y' ? "Yes" : "No")
//                    .Replace("{{2}}", home.HomeInsurancePolicy.IsRiotStrikeDamage == 'Y' ? "Yes" : "No")
//                    .Replace("{{3}}", home.HomeInsurancePolicy.IsRequireDomestic == 'Y' ? "Yes" : "No")
//                    .Replace("{{4}}", home.HomeInsurancePolicy.IsSingleItemAboveContents == 'Y' ? "Yes" : "No")
//                    .Replace("{{5}}", home.HomeInsurancePolicy.IsJointOwnership == 'Y' ? "Yes" : "No")
//                    .Replace("{{6}}", home.HomeInsurancePolicy.IsPropertyInConnectionTrade == 'Y' ? "Yes" : "No")
//                    .Replace("{{7}}", home.HomeInsurancePolicy.IsPropertyCoveredOtherInsurance == 'Y' ? "Yes" : "No")
//                    .Replace("{{8}}", home.HomeInsurancePolicy.IsPropertyInsuredSustainedAnyLoss == 'Y' ? "Yes" : "No")
//                    .Replace("{{9}}", home.HomeInsurancePolicy.IsPropertyUndergoingConstruction == 'Y' ? "Yes" : "No")
//                    ;

//                if (home.HomeSubItems.Count > 0)
//                {
//                    string singleitemdiv = @"<TABLE WIDTH=638 CELLPADDING=7 CELLSPACING=0><COL WIDTH=463><COL WIDTH=145><TR><TD COLSPAN=2 WIDTH=622 VALIGN=TOP STYLE='border: 1px solid #00000a; padding-top: 0in; padding-bottom: 0in; padding-left: 0.08in; padding-right: 0.08in'>
//< P ALIGN = CENTER >< FONT FACE = 'serif' >< FONT SIZE = 3 STYLE = 'font-size: 11pt' >< B > Single items </ B ></ FONT ></ FONT ></ P ></ TD ></ TR >
//{{ SingleItemsDiv}}</ TABLE >";

//                    htmlCode = htmlCode.Replace(singleitemdiv, "");
//                }
//                /*Single thread*/
//                Thread thread = new Thread(() =>
//                { pdf.LoadFromHTML(htmlCode, true, setting, htmlLayoutFormat); });
//                thread.SetApartmentState(ApartmentState.STA);
//                thread.Start();
//                thread.Join();

//                if (System.IO.File.Exists(FullPath))
//                {
//                    System.IO.File.Delete(FullPath);
//                }

//                pdf.SaveToFile(FileSavePath + FileName);


//                MailMessageResponse mailResponseMessage = _mail.GetMessageByKey(MailMessageKey.RenewPolicy, home.HomeInsurancePolicy.InsuredCode);

//                if (mailResponseMessage.IsTransactionDone)
//                {
//                    mailResponseMessage.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);
//                    mailResponseMessage.Message = mailResponseMessage.Message.Replace("({{policy_no}))", home.HomeInsurancePolicy.DocumentNo).Replace("{{site_name}}", MailMessageKey.SiteName)
//                        .Replace("api/cms", Utility.APIURI + "api/cms").Replace("{{name}}", home.HomeInsurancePolicy.InsuredName);
//                    mailResponseMessage.Message = mailResponseMessage.Message.Replace("{{policy_verification_url}}",
//                        Utility.APIURI + PolicyReviewLinks.HomeReview.Replace("{refId}", home.HomeInsurancePolicy.HomeID.ToString()));
//                    MailSendRequestWithAttachment newRequest = new MailSendRequestWithAttachment();
//                    newRequest.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);

//                    //newRequest.Personalizations = new List<PersonalizationWithCC>() { new PersonalizationWithCC() { ToRecipient = new List<MailRecipientDetails>() {
//                    //    new MailRecipientDetails() { Email = emailaddress} } } };

//                    //newRequest.Personalizations = IncludeInsurancePurchaseCC(newRequest.Personalizations);

//                    if (!string.IsNullOrEmpty(Utility.InsurancePurchaseCCRecipient))
//                    {
//                        newRequest.Personalizations = new List<PersonalizationWithCC>() { new PersonalizationWithCC()
//                        { ToRecipient = new List<MailRecipientDetails>() {
//                            new MailRecipientDetails() { Email = emailaddress} },
//                        CCRecipient = new List<MailRecipientDetails>()
//                        {
//                            new MailRecipientDetails() { Email = Utility.InsurancePurchaseCCRecipient }
//                        }
//                        } };
//                    }
//                    else
//                    {
//                        newRequest.Personalizations = new List<PersonalizationWithCC>() { new PersonalizationWithCC() { ToRecipient = new List<MailRecipientDetails>() {
//                        new MailRecipientDetails() { Email = emailaddress} } } };
//                    }

//                    newRequest.Contents = new List<ContentDetail>() { new ContentDetail() { Value = mailResponseMessage.Message } };
//                    new Task(() => { _mail.sendWithAttachment(newRequest, FullPath, FileName); }).Start();

//                    EmailMessageAudit emailMessageAudit = new EmailMessageAudit();
//                    emailMessageAudit.LinkNo = "";
//                    emailMessageAudit.PolicyNo = home.HomeInsurancePolicy.DocumentNo;
//                    emailMessageAudit.InsuredCode = home.HomeInsurancePolicy.InsuredCode;
//                    emailMessageAudit.InsuranceType = Constants.Insurance.Home;
//                    emailMessageAudit.MessageKey = MailMessageKey.NEWPolicy;
//                    emailMessageAudit.Message = mailResponseMessage.Message;
//                    emailMessageAudit.Subject = newRequest.Subject;

//                    new Task(() => { _mail.InsertEmailMessageAudit(emailMessageAudit); }).Start();

//                }


//                return FullPath;
//            }
//            catch (Exception ex)
//            {
//                return "";
//            }

//        }

//        public string CreateMotorRenewalSchedule(MotorInsurancePolicy motordetails, string emailaddress)
//        {
//            try
//            {
//                string FileName = motordetails.DocumentNo + ".pdf";
//                string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + motordetails.InsuredCode + "/" + motordetails.DocumentNo + "/";

//                if (!System.IO.Directory.Exists(FileSavePath))
//                {
//                    System.IO.Directory.CreateDirectory(FileSavePath);
//                }
//                else
//                {
//                    //ExtensionMethods.DeleteFilesInsideDirectory(FileSavePath);
//                }

//                string FullPath = FileSavePath + FileName;
//                Document document = new Document();
//                var applicationPath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/";

//                ExtensionMethods.CopyCompanyLogos(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/", FileSavePath);

//                if (motordetails.Mainclass == "TP")
//                {
//                    document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "MotorTP.docx", FileSavePath + "MotorTP.docx"));
//                }
//                else
//                {
//                    document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "Motor.docx", FileSavePath + "Motor.docx"));
//                }


//                document.Replace("{{PolicyNo}}", motordetails.DocumentNo, false, true);
//                document.Replace("{{InsuredName}}", motordetails.InsuredName, false, true);
//                document.Replace("{{RegistrationNo}}", Convert.ToString(motordetails.RegistrationNumber), false, true);
//                document.Replace("{{ChasisNo}}", motordetails.ChassisNo, false, true);
//                document.Replace("{{VehicleMake}}", motordetails.VehicleMake, false, true);
//                document.Replace("{{VehicleModel}}", motordetails.VehicleModel, false, true);
//                document.Replace("{{EngineCapacity}}", Convert.ToString(motordetails.EngineCC), false, true);
//                document.Replace("{{YearOfMake}}", Convert.ToString(motordetails.YearOfMake), false, true);
//                document.Replace("{{StartDate}}", motordetails.PolicyCommencementDate.CovertToLocalFormat(), false, true);
//                document.Replace("{{ExpiryDate}}", motordetails.ExpiryDate.CovertToLocalFormat(), false, true);
//                document.Replace("{{Product}}", motordetails.PolicyCode, false, true);
//                document.Replace("{{VehicleValue}}", Convert.ToString(motordetails.VehicleValue), false, true);
//                document.Replace("{{Premium}}", Convert.ToString(motordetails.PremiumAfterDiscount), false, true);
//                document.Replace("{{ExcessValue}}", Convert.ToString(motordetails.ExcessAmount), false, true);
//                document.Replace("{{ExcessType}}", motordetails.ExcessType, false, true);
//                document.Replace("{{RoadAssistance}}", motordetails.RoadAssistance == true ? "Yes" : "No", false, true);

//                if (string.IsNullOrEmpty(motordetails.FinancierCompanyCode))
//                {
//                    document.Replace("{{FinancierType}}", "Privately owned", false, true);
//                    //document.Replace("{{FinancierCompany}}", "", false, true);
//                }
//                else
//                {
//                    //document.Replace("{{FinancierType}}", "FinancierType:", false, true);
//                    document.Replace("{{FinancierType}}", motordetails.FinancierCompanyCode, false, true);
//                }

//                document.Replace("{{CarReplacementDays}}", Convert.ToString(motordetails.CarReplacementDays), false, true);
//                document.Replace("{{IsPersonalAccident}}", motordetails.IsPersonalAccidentCovered == true ? "Yes" : "No", false, true);
//                document.Replace("{{IsSRCC}}", motordetails.IsCommonCoverage == true ? "Yes" : "No", false, true);
//                document.Replace("{{CurrentDateTime}}", DateTime.Now.CovertToLocalFormatTime(), false, true);

//                if (System.IO.File.Exists(FullPath))
//                {
//                    System.IO.File.Delete(FullPath);
//                }

//                document.SaveToFile(FileSavePath + FileName, Spire.Doc.FileFormat.PDF);

//                MailMessageResponse mailResponseMessage = _mail.GetMessageByKey(MailMessageKey.RenewPolicy, motordetails.InsuredCode);
//                if (mailResponseMessage.IsTransactionDone)
//                {
//                    mailResponseMessage.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);
//                    mailResponseMessage.Message = mailResponseMessage.Message.Replace("({{policy_no}))", motordetails.DocumentNo).Replace("{{name}}", motordetails.InsuredName).Replace("api/cms", Utility.APIURI + "api/cms");
//                    mailResponseMessage.Message = mailResponseMessage.Message.Replace("{{policy_verification_url}}",
//                         Utility.frontWebUI + PolicyReviewLinks.MotorReview.Replace("{refId}", motordetails.MotorID.ToString()));
//                    MailSendRequestWithAttachment newRequest = new MailSendRequestWithAttachment();
//                    newRequest.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);

//                    newRequest.Personalizations = new List<PersonalizationWithCC>() { new PersonalizationWithCC() { ToRecipient = new List<MailRecipientDetails>() {
//                    new MailRecipientDetails() { Email = emailaddress} } } };

//                    newRequest.Personalizations = IncludeInsurancePurchaseCC(newRequest.Personalizations);

//                    newRequest.Contents = new List<ContentDetail>() { new ContentDetail() { Value = mailResponseMessage.Message } };
//                    new Task(() => { _mail.sendWithAttachment(newRequest, FullPath, FileName); }).Start();


//                    EmailMessageAudit emailMessageAudit = new EmailMessageAudit();
//                    emailMessageAudit.LinkNo = "";
//                    emailMessageAudit.PolicyNo = motordetails.DocumentNo;
//                    emailMessageAudit.InsuredCode = motordetails.InsuredCode;
//                    emailMessageAudit.InsuranceType = Constants.Insurance.Motor;
//                    emailMessageAudit.MessageKey = MailMessageKey.NEWPolicy;
//                    emailMessageAudit.Message = mailResponseMessage.Message;
//                    emailMessageAudit.Subject = mailResponseMessage.Subject;

//                    new Task(() => { _mail.InsertEmailMessageAudit(emailMessageAudit); }).Start();
//                }


//                return FullPath;
//            }
//            catch (Exception ex)
//            {
//                return "";
//            }
//        }

//        public string CreateHomeSchudles(HomeSavedQuotationResult home, bool isMailSend = true)
//        {
//            try
//            {
//                string FileName = home.HomeInsurancePolicy.DocumentNo + ".pdf";
//                string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + home.HomeInsurancePolicy.InsuredCode + "/" + home.HomeInsurancePolicy.DocumentNo + "/";

//                if (!System.IO.Directory.Exists(FileSavePath))
//                {
//                    System.IO.Directory.CreateDirectory(FileSavePath);
//                }
//                else
//                {
//                    //ExtensionMethods.DeleteFilesInsideDirectory(FileSavePath);
//                }

//                string FullPath = FileSavePath + FileName;

//                ExtensionMethods.CopyCompanyLogos(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/", FileSavePath);

//                string htmlCode = File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/Home.html",
//                    FileSavePath + "Home.html"));
//                string DomesticWorkersDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/HomeDomestic.html",
//                    FileSavePath + "HomeDomestic.html"));
//                string HomesubItemsDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/HomeSingleItems.html",
//                    FileSavePath + "HomeSingleItems.html"));

//                PdfDocument pdf = new PdfDocument();
//                PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
//                htmlLayoutFormat.IsWaiting = false;
//                PdfPageSettings setting = new PdfPageSettings();
//                string memberslist = string.Empty;
//                string members = string.Empty;
//                string homeitems = string.Empty;
//                string homesubitems = string.Empty;
//                int WorkerCount = 1;
//                foreach (var list in home.DomesticHelp)
//                {
//                    members = DomesticWorkersDiv.Replace("{{Name}}", list.Name).Replace("{{DOB}}", list.DOB.CovertToLocalFormat())
//                           .Replace("{{CPR}}", list.CPR)
//                           .Replace("{{SumInsured}}", Convert.ToString(list.SumInsured))
//                           .Replace("{{WorkerCount}}", WorkerCount.ToString());
//                    memberslist += members;
//                }

//                foreach (var list in home.HomeSubItems)
//                {

//                    homeitems = HomesubItemsDiv.Replace("{{Item_description}}", list.Description).Replace("{{Item_Value}}",
//                        Convert.ToString(list.SumInsured));
//                    homesubitems += homeitems;
//                }
//                htmlCode = htmlCode.Replace("{{DomesticWorkersDiv}}", memberslist).Replace("{{PolicyNo}}", home.HomeInsurancePolicy.DocumentNo)
//                    .Replace("{{SingleItemsDiv}}", homesubitems)
//                    .Replace("{{InsuredName}}", home.HomeInsurancePolicy.InsuredName).Replace("{{FlatNo}}", home.HomeInsurancePolicy.FlatNo)
//                    .Replace("{{BlockNo}}", home.HomeInsurancePolicy.BlockNo).Replace("{{RoadNo}}", home.HomeInsurancePolicy.RoadNo).Replace("{{Town}}", home.HomeInsurancePolicy.Town)
//                    .Replace("{{BuildingAge}}", Convert.ToString(home.HomeInsurancePolicy.BuildingAge))
//                    .Replace("{{BuildingValue}}", Convert.ToString(home.HomeInsurancePolicy.BuildingValue))
//                    .Replace("{{ContentValue}}", Convert.ToString(home.HomeInsurancePolicy.ContentValue))
//                    .Replace("{{JewelleryCover}}", home.HomeInsurancePolicy.JewelleryCover)
//                    .Replace("{{TotalSumInsured}}", Convert.ToString((home.HomeInsurancePolicy.BuildingValue + home.HomeInsurancePolicy.ContentValue)))
//                    .Replace("{{PolicyStartDate}}", home.HomeInsurancePolicy.PolicyStartDate.CovertToLocalFormat())
//                    .Replace("{{ExpiryDate}}", home.HomeInsurancePolicy.PolicyExpiryDate.CovertToLocalFormat())
//                    .Replace("{{SumInsured}}", Convert.ToString(home.HomeInsurancePolicy.SumInsured))
//                    .Replace("{{Premium}}", Convert.ToString(home.HomeInsurancePolicy.PremiumAfterDiscount))
//                    .Replace("{{1}}", home.HomeInsurancePolicy.IsSafePropertyInsured == 'Y' ? "Yes" : "No")
//                    .Replace("{{2}}", home.HomeInsurancePolicy.IsRiotStrikeDamage == 'Y' ? "Yes" : "No")
//                    .Replace("{{3}}", home.HomeInsurancePolicy.IsRequireDomestic == 'Y' ? "Yes" : "No")
//                    .Replace("{{4}}", home.HomeInsurancePolicy.IsSingleItemAboveContents == 'Y' ? "Yes" : "No")
//                    .Replace("{{5}}", home.HomeInsurancePolicy.IsJointOwnership == 'Y' ? "Yes" : "No")
//                    .Replace("{{6}}", home.HomeInsurancePolicy.IsPropertyInConnectionTrade == 'Y' ? "Yes" : "No")
//                    .Replace("{{7}}", home.HomeInsurancePolicy.IsPropertyCoveredOtherInsurance == 'Y' ? "Yes" : "No")
//                    .Replace("{{8}}", home.HomeInsurancePolicy.IsPropertyInsuredSustainedAnyLoss == 'Y' ? "Yes" : "No")
//                    .Replace("{{9}}", home.HomeInsurancePolicy.IsPropertyUndergoingConstruction == 'Y' ? "Yes" : "No")
//                    ;

//                if (home.HomeSubItems.Count == 0)
//                {
//                    string singleitemdiv = @"<TABLE WIDTH=638 CELLPADDING=7 CELLSPACING=0><COL WIDTH=463>
//<COL WIDTH=145><TR><TD COLSPAN=2 WIDTH=622 VALIGN=TOP STYLE='border: 1px solid #00000a; padding-top: 0in; padding-bottom: 0in; padding-left: 0.08in; padding-right: 0.08in'>
//<P ALIGN = CENTER><FONT FACE = 'serif'><FONT SIZE =3 STYLE ='font-size: 11pt'><B> Single items </B></FONT></FONT></P></TD></TR></TABLE>";

//                    htmlCode = htmlCode.Replace(singleitemdiv, "");
//                }
//                /*Single thread*/
//                Thread thread = new Thread(() =>
//                { pdf.LoadFromHTML(htmlCode, true, setting, htmlLayoutFormat); });
//                thread.SetApartmentState(ApartmentState.STA);
//                thread.Start();
//                thread.Join();

//                if (System.IO.File.Exists(FullPath))
//                {
//                    System.IO.File.Delete(FullPath);
//                }

//                pdf.SaveToFile(FileSavePath + FileName);

//                if (isMailSend)
//                {
//                    MailMessageResponse mailResponseMessage = _mail.GetMessageByKey(MailMessageKey.NEWPolicy, home.HomeInsurancePolicy.InsuredCode);
//                    if (mailResponseMessage.IsTransactionDone)
//                    {
//                        mailResponseMessage.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);
//                        mailResponseMessage.Message = mailResponseMessage.Message.Replace("({{policy_no}))", home.HomeInsurancePolicy.DocumentNo).Replace("{{site_name}}", MailMessageKey.SiteName).Replace("api/cms", Utility.APIURI + "api/cms")
//                            .Replace("{{name}}", home.HomeInsurancePolicy.InsuredName);

//                        mailResponseMessage.Message = mailResponseMessage.Message.Replace("{{policy_verification_url}}",
//                            Utility.frontWebUI + PolicyReviewLinks.HomeReview.Replace("{refId}", home.HomeInsurancePolicy.HomeID.ToString()));
//                        MailSendRequestWithAttachment newRequest = new MailSendRequestWithAttachment();
//                        newRequest.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);

//                        newRequest.Personalizations = new List<PersonalizationWithCC>() { new PersonalizationWithCC() { ToRecipient = new List<MailRecipientDetails>() {
//                    new MailRecipientDetails() { Email = mailResponseMessage.Email} } } };

//                        newRequest.Personalizations = IncludeInsurancePurchaseCC(newRequest.Personalizations);

//                        newRequest.Contents = new List<ContentDetail>() { new ContentDetail() { Value = mailResponseMessage.Message } };
//                        new Task(() => { _mail.sendWithAttachment(newRequest, FullPath, FileName); }).Start();

//                        EmailMessageAudit emailMessageAudit = new EmailMessageAudit();
//                        emailMessageAudit.LinkNo = "";
//                        emailMessageAudit.PolicyNo = home.HomeInsurancePolicy.DocumentNo;
//                        emailMessageAudit.InsuredCode = home.HomeInsurancePolicy.InsuredCode;
//                        emailMessageAudit.InsuranceType = Constants.Insurance.Home;
//                        emailMessageAudit.MessageKey = MailMessageKey.NEWPolicy;
//                        emailMessageAudit.Message = mailResponseMessage.Message;
//                        emailMessageAudit.Subject = newRequest.Subject;

//                        new Task(() => { _mail.InsertEmailMessageAudit(emailMessageAudit); }).Start();

//                    }
//                }

//                return FullPath;
//            }
//            catch (Exception ex)
//            {
//                return "";
//            }

//        }

        //public string CreateDomesticSchudles(DomesticHelpSavedQuotationResult domestic, bool isMailSend = true)
        //{
        //    try
        //    {
        //        string FileName = domestic.DomesticHelp.DocumentNo + ".pdf";
        //        string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + domestic.DomesticHelp.InsuredCode + "/" + domestic.DomesticHelp.DocumentNo + "/";

        //        if (!System.IO.Directory.Exists(FileSavePath))
        //        {
        //            System.IO.Directory.CreateDirectory(FileSavePath);
        //        }
        //        else
        //        {
        //            //ExtensionMethods.DeleteFilesInsideDirectory(FileSavePath);
        //        }

        //        string FullPath = FileSavePath + FileName;

        //        ExtensionMethods.CopyCompanyLogos(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/", FileSavePath);

        //        string htmlCode = File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/DomesticHelp.html",
        //            FileSavePath + "DomesticHelp.html"));
        //        string DomesticWorkersDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/DomesticWorkersDiv.txt",
        //            FileSavePath + "DomesticWorkersDiv.txt"));

        //        PdfDocument pdf = new PdfDocument();
        //        PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
        //        htmlLayoutFormat.IsWaiting = false;
        //        PdfPageSettings setting = new PdfPageSettings();
        //        string memberslist = string.Empty;
        //        string memebrs = string.Empty;
        //        int MembersCount = 1;
        //        foreach (var list in domestic.DomesticHelpMemberList)
        //        {
        //            string sex = list.Sex == 'M' ? "Male" : "Female";
        //            memebrs = DomesticWorkersDiv.Replace("{{Name}}", list.Name).Replace("{{SEX}}", sex).Replace("{{DOB}}", list.DOB.CovertToLocalFormat())
        //                .Replace("{{Nationality}}", list.Nationality).Replace("{{CPR}}", list.CPRNumber).Replace("{{Passport}}", list.Passport)
        //                .Replace("{{Occupation}}", list.Occupation).Replace("{{Address}}", list.AddressType).Replace("{{WorkerCount}}", Convert.ToString(MembersCount));
        //            MembersCount++;
        //            memberslist += memebrs;
        //        }
        //        htmlCode = htmlCode.Replace("{{DomesticWorkersDiv}}", memberslist).Replace("{{PolicyStartDate}}", domestic.DomesticHelp.PolicyStartDate.CovertToLocalFormat())
        //            .Replace("{{PolicyExpiryDate}}", domestic.DomesticHelp.PolicyExpiryDate.CovertToLocalFormat()).Replace("{{SumInsured}}", Convert.ToString(domestic.DomesticHelp.SumInsured))
        //            .Replace("{{Premium}}", Convert.ToString(domestic.DomesticHelp.PremiumAfterDiscount)).
        //            Replace("{{CurrentDate}}", DateTime.Now.CovertToLocalFormat())
        //            .Replace("{{InsuredName}}", domestic.DomesticHelp.FullName)
        //            .Replace("{{EmployedUnder}}", domestic.DomesticHelp.DomesticWorkType)
        //            .Replace("{{PhysicalDefect}}", domestic.DomesticHelp.IsPhysicalDefect)
        //            .Replace("{{PolicyNo}}", domestic.DomesticHelp.DocumentNo);
        //        /*Single thread*/
        //        Thread thread = new Thread(() =>
        //        { pdf.LoadFromHTML(htmlCode, true, setting, htmlLayoutFormat); });
        //        thread.SetApartmentState(ApartmentState.STA);
        //        thread.Start();
        //        thread.Join();

        //        //System.Diagnostics.Process.Start("output.pdf");

        //        if (System.IO.File.Exists(FullPath))
        //        {
        //            System.IO.File.Delete(FullPath);
        //        }

        //        pdf.SaveToFile(FileSavePath + FileName);

        //        if (isMailSend)
        //        {
        //            MailMessageResponse mailResponseMessage = _mail.GetMessageByKey(MailMessageKey.NEWPolicy, domestic.DomesticHelp.InsuredCode);
        //            if (mailResponseMessage.IsTransactionDone)
        //            {
        //                mailResponseMessage.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);
        //                mailResponseMessage.Message = mailResponseMessage.Message.Replace("({{policy_no}))", domestic.DomesticHelp.DocumentNo).Replace("{{site_name}}", MailMessageKey.SiteName).Replace("api/cms", Utility.APIURI + "api/cms")
        //                    .Replace("{{name}}", domestic.DomesticHelp.InsuredName);
        //                mailResponseMessage.Message = mailResponseMessage.Message.Replace("{{policy_verification_url}}",
        //                    Utility.frontWebUI + PolicyReviewLinks.DomesticHelpReview.Replace("{refId}", domestic.DomesticHelp.DomesticID.ToString()));
        //                MailSendRequestWithAttachment newRequest = new MailSendRequestWithAttachment();
        //                newRequest.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);

        //                newRequest.Personalizations = new List<PersonalizationWithCC>() { new PersonalizationWithCC() { ToRecipient = new List<MailRecipientDetails>() {
        //                new MailRecipientDetails() { Email = mailResponseMessage.Email} } } };

        //                newRequest.Personalizations = IncludeInsurancePurchaseCC(newRequest.Personalizations);

        //                newRequest.Contents = new List<ContentDetail>() { new ContentDetail() { Value = mailResponseMessage.Message } };
        //                new Task(() => { _mail.sendWithAttachment(newRequest, FullPath, FileName); }).Start();
        //                //(FileSavePath + FileName).DeletFile();


        //                EmailMessageAudit emailMessageAudit = new EmailMessageAudit();
        //                emailMessageAudit.LinkNo = "";
        //                emailMessageAudit.PolicyNo = domestic.DomesticHelp.DocumentNo;
        //                emailMessageAudit.InsuredCode = domestic.DomesticHelp.InsuredCode;
        //                emailMessageAudit.InsuranceType = Constants.Insurance.DomesticHelp;
        //                emailMessageAudit.Message = mailResponseMessage.Message;
        //                emailMessageAudit.MessageKey = MailMessageKey.NEWPolicy;
        //                emailMessageAudit.Subject = newRequest.Subject;

        //                new Task(() => { _mail.InsertEmailMessageAudit(emailMessageAudit); }).Start();

        //            }
        //        }

        //        return FullPath;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }

        //}

        //public string CreateMotorSchudles(MotorInsurancePolicy motordetails, bool isMailSend = true)
        //{
        //    try
        //    {
        //        string FileName = motordetails.DocumentNo + ".pdf";
        //        string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + motordetails.InsuredCode + "/" + motordetails.DocumentNo + "/";

        //        if (!System.IO.Directory.Exists(FileSavePath))
        //        {
        //            System.IO.Directory.CreateDirectory(FileSavePath);
        //        }
        //        else
        //        {
        //            //ExtensionMethods.DeleteFilesInsideDirectory(FileSavePath);
        //        }

        //        string FullPath = FileSavePath + FileName;
        //        Document document = new Document();
        //        var applicationPath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/";
        //        if (motordetails.Mainclass == "TP")
        //        {
        //            document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "MotorTP.docx", FileSavePath + "MotorTP.docx"));
        //        }
        //        else
        //        {
        //            document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "Motor.docx", FileSavePath + "Motor.docx"));
        //        }
        //        ExtensionMethods.CopyCompanyLogos(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/", FileSavePath);

        //        // document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "Motor.docx", FileSavePath + "Motor.docx"));

        //        document.Replace("{{PolicyNo}}", motordetails.DocumentNo, false, true);
        //        document.Replace("{{InsuredName}}", motordetails.InsuredName, false, true);
        //        document.Replace("{{RegistrationNo}}", Convert.ToString(motordetails.RegistrationNumber), false, true);
        //        document.Replace("{{ChasisNo}}", motordetails.ChassisNo, false, true);
        //        document.Replace("{{VehicleMake}}", motordetails.VehicleMake, false, true);
        //        document.Replace("{{VehicleModel}}", motordetails.VehicleModel, false, true);
        //        document.Replace("{{EngineCapacity}}", Convert.ToString(motordetails.EngineCC), false, true);
        //        document.Replace("{{YearOfMake}}", Convert.ToString(motordetails.YearOfMake), false, true);
        //        document.Replace("{{StartDate}}", motordetails.PolicyCommencementDate.CovertToLocalFormat(), false, true);
        //        document.Replace("{{ExpiryDate}", motordetails.ExpiryDate.CovertToLocalFormat(), false, true);
        //        document.Replace("{{Product}}", motordetails.PolicyCode, false, true);
        //        document.Replace("{{VehicleValue}}", Convert.ToString(motordetails.VehicleValue), false, true);
        //        document.Replace("{{Premium}}", Convert.ToString(motordetails.PremiumAfterDiscount), false, true);
        //        document.Replace("{{ExcessValue}}", Convert.ToString(motordetails.ExcessAmount), false, true);
        //        document.Replace("{{ExcessType}}", motordetails.ExcessType, false, true);

        //        if (string.IsNullOrEmpty(motordetails.FinancierCompanyCode))
        //        {
        //            document.Replace("{{FinancierType}}", "Privately owned", false, true);
        //            // document.Replace("{{FinancierCompany}}", "", false, true);
        //        }
        //        else
        //        {
        //            //document.Replace("{{FinancierType}}", "FinancierType:", false, true);
        //            document.Replace("{{FinancierType}}", motordetails.FinancierCompanyCode, false, true);
        //        }

        //        document.Replace("{{CarReplacementDays}}", Convert.ToString(motordetails.CarReplacementDays), false, true);
        //        document.Replace("{{IsPersonalAccident}}", motordetails.IsPersonalAccidentCovered == true ? "Yes" : "No", false, true);
        //        document.Replace("{{IsSRCC}}", motordetails.IsCommonCoverage == true ? "Yes" : "No", false, true);
        //        document.Replace("{{CurrentDateTime}}", Convert.ToString(DateTime.Now), false, true);

        //        if (System.IO.File.Exists(FullPath))
        //        {
        //            System.IO.File.Delete(FullPath);
        //        }

        //        document.SaveToFile(FileSavePath + FileName, Spire.Doc.FileFormat.PDF);

        //        if (isMailSend)
        //        {
        //            MailMessageResponse mailResponseMessage = _mail.GetMessageByKey(MailMessageKey.NEWPolicy, motordetails.InsuredCode);
        //            if (mailResponseMessage.IsTransactionDone)
        //            {
        //                mailResponseMessage.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);
        //                mailResponseMessage.Message = mailResponseMessage.Message.Replace("({{policy_no}))", motordetails.DocumentNo).Replace("{{site_name}}", MailMessageKey.SiteName)
        //                    .Replace("api/cms", Utility.APIURI + "api/cms").Replace("{{name}}", motordetails.InsuredName);
        //                mailResponseMessage.Message = mailResponseMessage.Message.Replace("{{policy_verification_url}}",
        //                     Utility.frontWebUI + PolicyReviewLinks.MotorReview.Replace("{refId}", motordetails.MotorID.ToString()));
        //                MailSendRequestWithAttachment newRequest = new MailSendRequestWithAttachment();
        //                newRequest.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);

        //                newRequest.Personalizations = new List<PersonalizationWithCC>() { new PersonalizationWithCC() { ToRecipient = new List<MailRecipientDetails>() {
        //            new MailRecipientDetails() { Email = mailResponseMessage.Email} } } };

        //                newRequest.Personalizations = IncludeInsurancePurchaseCC(newRequest.Personalizations);

        //                newRequest.Contents = new List<ContentDetail>() { new ContentDetail() { Value = mailResponseMessage.Message } };
        //                new Task(() => { _mail.sendWithAttachment(newRequest, FullPath, FileName); }).Start();


        //                EmailMessageAudit emailMessageAudit = new EmailMessageAudit();
        //                emailMessageAudit.LinkNo = "";
        //                emailMessageAudit.PolicyNo = motordetails.DocumentNo;
        //                emailMessageAudit.InsuredCode = motordetails.InsuredCode;
        //                emailMessageAudit.InsuranceType = Constants.Insurance.Motor;
        //                emailMessageAudit.MessageKey = MailMessageKey.NEWPolicy;
        //                emailMessageAudit.Message = mailResponseMessage.Message;
        //                emailMessageAudit.Subject = mailResponseMessage.Subject;

        //                new Task(() => { _mail.InsertEmailMessageAudit(emailMessageAudit); }).Start();
        //            }
        //        }

        //        return FullPath;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }

        //}

        //public string CreateTravelSchudles(TravelSavedQuotationResult travel, bool isMailSend = true)
        //{
        //    try
        //    {
        //        string FileName = travel.TravelInsurancePolicyDetails.DocumentNumber + ".pdf";
        //        string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + travel.TravelInsurancePolicyDetails.InsuredCode + "/" + travel.TravelInsurancePolicyDetails.DocumentNumber + "/";

        //        if (!System.IO.Directory.Exists(FileSavePath))
        //        {
        //            System.IO.Directory.CreateDirectory(FileSavePath);
        //        }
        //        else
        //        {
        //            //ExtensionMethods.DeleteFilesInsideDirectory(FileSavePath);
        //        }

        //        ExtensionMethods.CopyCompanyLogos(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/", FileSavePath);

        //        string FullPath = FileSavePath + FileName;
        //        string htmlCode = File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/travel.html",
        //            FileSavePath + "travel.html"));
        //        string DomesticWorkersDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/TravelMembers.html",
        //            FileSavePath + "TravelMembers.html"));

        //        PdfDocument pdf = new PdfDocument();
        //        PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
        //        htmlLayoutFormat.IsWaiting = false;
        //        PdfPageSettings setting = new PdfPageSettings();
        //        setting.Size = PdfPageSize.A4;
        //        string memberslist = string.Empty;
        //        string members = string.Empty;
        //        int MembersCount = 1;
        //        foreach (var list in travel.TravelMembers)
        //        {
        //            string sex = list.Sex.Trim() == "M" ? "Male" : "Female";
        //            members = DomesticWorkersDiv.Replace("{{MemberName}}", list.FirstName + list.LastName).Replace("{{Sex}}", sex).Replace("{{DOB}}", list.DateOfBirth.CovertToLocalFormat())
        //                .Replace("{{Nationality}}", list.Make).Replace("{{CPR}}", list.CPR).Replace("{{Passport}}", list.Passport)
        //                .Replace("{{Occupation}}", list.OccupationCode).Replace("{{Count}}", Convert.ToString(MembersCount)).Replace("{{Relationship}}", list.Category);
        //            MembersCount++;
        //            memberslist += members;
        //        }
        //        htmlCode = htmlCode.Replace("{{TravelMembersDiv}}", memberslist).Replace("{{StartDate}}", travel.TravelInsurancePolicyDetails.InsuranceStartDate.CovertToLocalFormat())
        //            .Replace("{{ExpiryDate}}", travel.TravelInsurancePolicyDetails.ExpiryDate.CovertToLocalFormat()).Replace("{{SumInsured}}", Convert.ToString(travel.TravelInsurancePolicyDetails.SumInsured)).Replace("{{SumInsuredFormat}}", Convert.ToString(travel.TravelInsurancePolicyDetails.CoverageType) == "Schengen" ? "Euro" : "UST")
        //            .Replace("{{Premium}}", Convert.ToString(travel.TravelInsurancePolicyDetails.DiscountAmount)).Replace("{{PolicyNumber}}", travel.TravelInsurancePolicyDetails.DocumentNumber)
        //            .Replace("{{PolicyHolderName}}", travel.TravelInsurancePolicyDetails.InsuredName).Replace("{{IsPhysical}}", travel.TravelInsurancePolicyDetails.IsPhysicalDefect)
        //            .Replace("{{PolicyType}}", travel.TravelInsurancePolicyDetails.SubClass == "FAMIL" ? "Family" : "Individual")
        //            .Replace("{{CurrentDate}}", DateTime.Now.CovertToLocalFormatTime())
        //            .Replace("{{Geographical}}", travel.TravelInsurancePolicyDetails.CoverageType);


        //        /*Single thread*/
        //        Thread thread = new Thread(() =>
        //        {
        //            pdf.LoadFromHTML(htmlCode, true, setting, htmlLayoutFormat);
        //        });
        //        thread.SetApartmentState(ApartmentState.STA);
        //        thread.Start();
        //        thread.Join();

        //        if (System.IO.File.Exists(FullPath))
        //        {
        //            System.IO.File.Delete(FullPath);
        //        }

        //        pdf.SaveToFile(FileSavePath + FileName);

        //        if (isMailSend)
        //        {
        //            MailMessageResponse mailResponseMessage = _mail.GetMessageByKey(MailMessageKey.NEWPolicy, travel.TravelInsurancePolicyDetails.InsuredCode);
        //            if (mailResponseMessage.IsTransactionDone)
        //            {
        //                mailResponseMessage.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);
        //                mailResponseMessage.Message = mailResponseMessage.Message.Replace("({{policy_no}))", travel.TravelInsurancePolicyDetails.DocumentNumber).Replace("{{site_name}}", MailMessageKey.SiteName)
        //                    .Replace("api/cms", Utility.APIURI + "api/cms").Replace("{{name}}", travel.TravelInsurancePolicyDetails.InsuredName);
        //                mailResponseMessage.Message = mailResponseMessage.Message.Replace("{{policy_verification_url}}",
        //                     Utility.frontWebUI + PolicyReviewLinks.TravelReview.Replace("{refId}", travel.TravelInsurancePolicyDetails.TravelID.ToString()));

        //                MailSendRequestWithAttachment newRequest = new MailSendRequestWithAttachment();
        //                newRequest.Subject = mailResponseMessage.Subject.Replace("{{site_name}}", MailMessageKey.SiteName);
        //                newRequest.Personalizations = new List<PersonalizationWithCC>() { new PersonalizationWithCC() { ToRecipient = new List<MailRecipientDetails>() {
        //            new MailRecipientDetails() { Email = mailResponseMessage.Email} } } };

        //                newRequest.Personalizations = IncludeInsurancePurchaseCC(newRequest.Personalizations);

        //                newRequest.Contents = new List<ContentDetail>() { new ContentDetail() { Value = mailResponseMessage.Message } };
        //                new Task(() => { _mail.sendWithAttachment(newRequest, FullPath, FileName); }).Start();
        //                //(FileSavePath + FileName).DeletFile();

        //                EmailMessageAudit emailMessageAudit = new EmailMessageAudit();
        //                emailMessageAudit.LinkNo = "";
        //                emailMessageAudit.PolicyNo = travel.TravelInsurancePolicyDetails.DocumentNumber;
        //                emailMessageAudit.InsuredCode = travel.TravelInsurancePolicyDetails.InsuredCode;
        //                emailMessageAudit.InsuranceType = Constants.Insurance.Travel;
        //                emailMessageAudit.MessageKey = MailMessageKey.NEWPolicy;
        //                emailMessageAudit.Message = mailResponseMessage.Message;
        //                emailMessageAudit.Subject = mailResponseMessage.Subject;

        //                new Task(() => { _mail.InsertEmailMessageAudit(emailMessageAudit); }).Start();

        //            }
        //        }

        //        return FullPath;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }

        //}

        public PaymentErrorUpdateResponse UpdatePaymentError(PaymentErrorUpdateRequest updateRequest)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@TrackId", updateRequest.TrackId),
                    new SqlParameter("@ErrorCode",updateRequest.ErrorCode),
                    new SqlParameter("@ErrorExplaination",updateRequest.ErrorExplaination)
                };

                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName ="@RefreshedTrackId", Size=50 }
                };

                object[] dataSet = BKICSQL.GetValues(StoredProcedures.Payment.PaymentErrorUpdateByTrackId, para, outParams);

                return new PaymentErrorUpdateResponse() { IsTransactionDone = true, RefreshedTrackId = dataSet[0].ToString() };

            }
            catch (Exception exc)
            {
                return new PaymentErrorUpdateResponse() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }

        public void Logger(String lines)
        {

            // Write the string to a file.append mode is enabled so that the log
            // lines get appended to  test.txt than wiping content and writing the log

            System.IO.StreamWriter file = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Error.txt", true);
            file.WriteLine(lines);

            file.Close();

        }

        //public List<PersonalizationWithCC> IncludeInsurancePurchaseCC(List<PersonalizationWithCC> personalization)
        //{
        //    if (!string.IsNullOrEmpty(DL.Utility.InsurancePurchaseCCRecipient))
        //    {
        //        personalization.ForEach(p =>
        //        {
        //            p.CCRecipient = new List<MailRecipientDetails>() { new MailRecipientDetails() { Email = DL.Utility.InsurancePurchaseCCRecipient } };
        //        });
        //    }

        //    return personalization;
        //}

    }
}
