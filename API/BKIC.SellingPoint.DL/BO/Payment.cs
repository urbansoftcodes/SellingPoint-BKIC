using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BO
{
    public class PaymentTrackInsertResult : TransactionWrapper
    {
        public string TrackId { get; set; }
    }

    public class PaymentTrackDetailResult : TransactionWrapper
    {
        public string TrackId { get; set; }
        public string InsuredCode { get; set; }
        public string PolicyNo { get; set; }
        public string LinkId { get; set; }
        public string InsuredType { get; set; }
        public decimal Amount { get; set; }
        public bool IsTrackIdAvailable { get; set; }
        public bool IsNotUsedAlready { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class PaymentUpdatePreCheckResult : TransactionWrapper
    {
        public bool IsTrackIdAvailable { get; set; }
        public bool IsNotUsedAlready { get; set; }
    }

    public class PaymentErrorUpdateRequest
    {
        public string TrackId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorExplaination { get; set; }
    }

    public class PaymentErrorUpdateResponse : TransactionWrapper
    {
        public string RefreshedTrackId { get; set; }
    }
}
