using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSMail.Utility
{
    public class AES
    {
        public readonly string accesskey;
        public readonly string secretkey;

        public AES(string accesskey, string secretkey)
        {
            this.accesskey = accesskey;
            this.secretkey = secretkey;
        }
        public bool SendMailWithoutAttachmentsAndCC(string senderAddress, string receiverAddress, string subject, string htmlBody,
            string textBody)
        {
            using (var client = new AmazonSimpleEmailServiceClient(accesskey, secretkey, RegionEndpoint.EUWest1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { receiverAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBody//ITS OPTIONAL FOR NON HTML CLIENT EMAILS
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    //ConfigurationSetName = configSet
                };
                try
                {
                    var response = client.SendEmail(sendRequest);
                    return true;
                }
                catch (Exception EX)
                {
                    return false;
                }
            }
        }


        public bool SendMailWithAttachments(MemoryStream stream)
        {
            using (var client = new AmazonSimpleEmailServiceClient(accesskey, secretkey,
              RegionEndpoint.EUWest1))
            {
                var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(stream) };
                try
                {
                    var response = client.SendRawEmail(sendRequest);
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }


        public bool SendMailWithCCWithoutAttachment(string senderAddress, string receiverAddress, string subject, string htmlBody,
            string textBody, List<string> ccaddress)
        {
            using (var client = new AmazonSimpleEmailServiceClient(accesskey, secretkey, RegionEndpoint.EUWest1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { receiverAddress },
                        CcAddresses=ccaddress
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBody//ITS OPTIONAL FOR NON HTML CLIENT EMAILS
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    //ConfigurationSetName = configSet
                };
                try
                {
                    var response = client.SendEmail(sendRequest);
                    return true;
                }
                catch (Exception EX)
                {
                    return false;
                }
            }
        }



        //private static BodyBuilder GetMessageBody()
        //{
        //    var body = new BodyBuilder()
        //    {
        //        HtmlBody = @"<p>Amazon SES Test body</p>",
        //        TextBody = "Amazon SES Test body",
        //    };
        //    body.Attachments.Add(@"D:\MotorInvoice.rtf");
        //    return body;
        //}

        //private static MimeMessage GetMessage()
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Foo Bar", "manojamarnath@gmail.com"));
        //    message.To.Add(new MailboxAddress(string.Empty, "manojamarnath@gmail.com"));
        //    message.Subject = "Amazon SES Test";
        //    message.Body = GetMessageBody().ToMessageBody();
        //    return message;
        //}

        //private static MemoryStream GetMessageStream()
        //{
        //    var stream = new MemoryStream();
        //    GetMessage().WriteTo(stream);
        //    return stream;
        //}

    }
}
