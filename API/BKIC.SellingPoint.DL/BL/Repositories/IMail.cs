using BKIC.SellingPoint.DL.BO;
using SendGridUtility.Wrappers;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IMail
    {
        MailMessageResponse GetMessageByKey(string key);
        bool sendWithOutAttachementMail(MailSendRequestWithoutCCAndBcc sendRequest);
        bool sendWithAttachment(MailSendRequestWithAttachment sendRequest, string FilePath, string FileName);
        void InsertEmailMessageAudit(EmailMessageAudit emailMessage);
        void SendMailLogError(string exceptonMessage, string insuredCode, string insuranceType, string agency, bool isOracle);
    }
}