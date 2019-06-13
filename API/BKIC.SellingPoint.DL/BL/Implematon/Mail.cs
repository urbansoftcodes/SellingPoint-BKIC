using BKIC.SellingPoint.DL.BO;
using SendGridUtility.Wrappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    public class Mail : Repositories.IMail
    {
        public readonly SendGridUtility.SendGridClient _sgClient;
        public readonly AWSMail.Utility.AES _awsClient;

        public Mail()
        {
            _sgClient = new SendGridUtility.SendGridClient(Utility.sgApiKey);
            _awsClient = new AWSMail.Utility.AES(BKIC.DL.Utility.AwsAccessKey, BKIC.DL.Utility.AwsSecretKey);
        }

        /// <summary>
        /// Sender key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public MailMessageResponse GetMessageByKey(string key)
        {
            try
            {
                MailMessageResponse response = new MailMessageResponse();
                response.IsTransactionDone = true;

                List<SPOut> outParams = new List<SPOut>() {
                    new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@Message", Size = int.MaxValue },
                    new SPOut() { OutPutType= SqlDbType.NVarChar, Size = 250, ParameterName="@SendMailTo" },
                    new SPOut() { OutPutType= SqlDbType.NVarChar, Size=int.MaxValue, ParameterName="@Subject" }
                };

                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Key", key)
                };

                object[] dataSet = BKICSQL.GetValues(StoredProcedures.Mail.GetMessageBykey, para, outParams);
                response.Message = dataSet[0].ToString();
                response.Email = Convert.ToString(dataSet[1]);
                response.Subject = Convert.ToString(dataSet[2]);

                return response;
            }
            catch (Exception exc)
            {
                return new MailMessageResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = exc.Message
                };
            }
        }

        /// <summary>
        /// Send mail.
        /// </summary>
        /// <param name="sendRequest"></param>
        /// <returns></returns>
        public bool sendWithOutAttachementMail(MailSendRequestWithoutCCAndBcc sendRequest)
        {
            try
            {
                if (Utility.sgTest == "1")
                {
                    string testMailSendTo = Utility.sgTestMail;

                    string originalSendToEmail = sendRequest.Personalizations
                                                 .Select(x => x.ToRecipient
                                                 .Select(to => to.Email)
                                                 .FirstOrDefault())
                                                 .FirstOrDefault();

                    sendRequest.Personalizations.ForEach(x =>
                    {
                        x.ToRecipient.ForEach(to =>
                        {
                            to.Email = testMailSendTo;
                        });
                    });

                    sendRequest.Contents.ForEach(x =>
                    {
                        x.Value = x.Value + "</br></br><b>Original Mail send to : " + originalSendToEmail;
                    });
                }
                sendRequest.FromRecipient = new MailRecipientDetails()
                {
                    Email = Utility.sgFromRecipientEmail,
                    Name = Utility.sgFromRecipientEmailName
                };

                //_sgClient.SendMail(sendRequest);

                string receiverAddress = sendRequest.Personalizations
                                         .Select(x => x.ToRecipient.Select(to => to.Email)
                                         .FirstOrDefault())
                                         .FirstOrDefault();

                string htmlBody = sendRequest.Contents
                                 .Select(x => x.Value)
                                 .FirstOrDefault();

                _awsClient.SendMailWithoutAttachmentsAndCC(BKIC.DL.Utility.AwsFromRecipientEmail, receiverAddress,
                    sendRequest.Subject, htmlBody, "");

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        /// <summary>
        /// Send mail with attachment.
        /// </summary>
        /// <param name="sendRequest"></param>
        /// <param name="FilePath"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool sendWithAttachment(MailSendRequestWithAttachment sendRequest, string FilePath, string FileName)
        {
            try
            {
                if (Utility.sgTest == "1")
                {
                    string testMailSendTo = Utility.sgTestMail;
                    string originalSendToEmail = sendRequest.Personalizations.Select(x => x.ToRecipient.Select(to => to.Email).FirstOrDefault()).FirstOrDefault();
                    sendRequest.Personalizations.ForEach(x =>
                    {
                        x.ToRecipient.ForEach(to =>
                        {
                            to.Email = testMailSendTo;
                        });
                    });

                    sendRequest.Contents.ForEach(x =>
                    {
                        x.Value = x.Value + "</br></br><b>Original Mail send to : " + originalSendToEmail;
                    });
                }
                sendRequest.FromRecipient = new MailRecipientDetails()
                {
                    Email = Utility.sgFromRecipientEmail,
                    Name = Utility.sgFromRecipientEmailName
                };
                string base64String = _sgClient.GetFileBase64String(FilePath);
                sendRequest.Attachments = new List<AttachmentDetail>()
                {
                    new AttachmentDetail()
                    {
                        Content = base64String, FileName = FileName
                    }
                }; ;
                //SendGridResponse response = _sgClient.SendMail(sendRequest);

                string receiverAddress = sendRequest.Personalizations.Select(x => x.ToRecipient.Select(to => to.Email).FirstOrDefault()).FirstOrDefault();
                string htmlBody = sendRequest.Contents.Select(x => x.Value).FirstOrDefault();
                string ccAddress = sendRequest.Personalizations.Select(x => x.CCRecipient.Select(c => c.Email).FirstOrDefault()).FirstOrDefault();

                _awsClient.SendMailWithAttachmentsWithCC(BKIC.DL.Utility.AwsFromRecipientEmail, receiverAddress
                    , sendRequest.Subject, htmlBody, FilePath, FileName, ccAddress);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (File.Exists(FilePath))
                {
                    string dirpath = Path.GetDirectoryName(FilePath);
                    Array.ForEach(Directory.GetFiles(dirpath), File.Delete);
                    //System.IO.File.Delete(FilePath);
                }
            }
        }

        /// <summary>
        /// Email audit.
        /// </summary>
        /// <param name="emailMessage"></param>
        public void InsertEmailMessageAudit(EmailMessageAudit emailMessage)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@MessageKey", emailMessage.MessageKey),
                    new SqlParameter("@Message", emailMessage.Message),
                    new SqlParameter("@InsuredCode", emailMessage.InsuredCode ?? ""),
                    new SqlParameter("@PolicyNo", emailMessage.PolicyNo ?? ""),
                    new SqlParameter("@LinkId", emailMessage.LinkNo ?? ""),
                    new SqlParameter("@InsuredType", emailMessage.InsuranceType),
                    new SqlParameter("@Subject", emailMessage.Subject ?? ""),
                    new SqlParameter("@Agency", emailMessage.Agency ?? "")
                };

                DataSet result = BKICSQL.eds(StoredProcedures.PortalSP.InsertEmailMessage, para);
            }
            catch (Exception exc)
            {
                var message = exc.Message;
            }
        }

        /// <summary>
        /// If error send a mail to the configured mail id's.
        /// </summary>
        /// <param name="exceptionMessage"></param>
        /// <param name="insuredCode"></param>
        /// <param name="insuranceType"></param>
        /// <param name="agency"></param>
        /// <param name="isOracle"></param>
        public void SendMailLogError(string exceptionMessage, string insuredCode, string insuranceType, string agency, bool isOracle)
        {
            MailMessageResponse mailResponseMessage = new MailMessageResponse();
            MailSendRequestWithoutCCAndBcc newRequest = new MailSendRequestWithoutCCAndBcc();
            EmailMessageAudit emailMessageAudit = new EmailMessageAudit();
            if (!isOracle)
            {
                mailResponseMessage = GetMessageByKey(BKIC.SellingPoint.DL.Constants.MailMessageKey.PolicyInsertFailed);
                emailMessageAudit.MessageKey = BKIC.SellingPoint.DL.Constants.MailMessageKey.PolicyInsertFailed;
            }
            else
            {
                mailResponseMessage = GetMessageByKey(BKIC.SellingPoint.DL.Constants.MailMessageKey.OraclePolicyInsertFailed);
                emailMessageAudit.MessageKey = BKIC.SellingPoint.DL.Constants.MailMessageKey.OraclePolicyInsertFailed;
            }

            newRequest.Subject = mailResponseMessage.Subject.Replace("{{Site_name}}", /*Constants.MailMessageKey.SiteName*/ "");

            newRequest.Personalizations = new List<Personalization>()
            {
                new Personalization()
                {
                    ToRecipient = new List<MailRecipientDetails>()
                    {
                      new MailRecipientDetails()
                      {
                          Email = mailResponseMessage.Email
                      }
                    }
                }
            };

            newRequest.Contents = new List<ContentDetail>()
            {
                new ContentDetail()
                {
                    Value = mailResponseMessage.Message
                }
            };
            new Task(() => 
            {
                sendWithOutAttachementMail(newRequest);

            }).Start();

            emailMessageAudit.InsuranceType = insuranceType;
            emailMessageAudit.InsuredCode = insuredCode;
            emailMessageAudit.Message = exceptionMessage;
            emailMessageAudit.Agency = agency;

            new Task(() => 
            {
                InsertEmailMessageAudit(emailMessageAudit);
            }).Start();
        }
    }
}