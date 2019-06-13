using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BO
{
    public class MailMessageResponse : TransactionWrapper
    {
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Subject { get; set; }
    }

    public class EmailMessageAudit
    {
        public string MessageKey { get; set; }
        public string Message { get; set; }
        public string InsuredCode { get; set; }
        public string PolicyNo { get; set; }
        public string LinkNo { get; set; }
        public string InsuranceType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Subject { get; set; }
        public string TrackId { get; set; }
        public string Agency { get; set; }
    }

    public class EmailMessageAuditResult : TransactionWrapper
    {
        public List<EmailMessageAudit> EmailMessage { get; set; }

        public EmailMessageAuditResult()
        {
            EmailMessage = new List<EmailMessageAudit>();
        }

    }


}
