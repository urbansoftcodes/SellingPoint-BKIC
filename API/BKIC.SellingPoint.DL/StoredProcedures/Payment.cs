using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.StoredProcedures
{
    public class Payment
    {
        public const string InsertPaymentTrack = "Payment_InsertTrack";
        public const string PaymentBasicDetailsByTrackId = "Payment_GetTrackDetails";
        public const string UpdatePaymentDetailsByTrackId = "Payment_UpdatePaymentDetailsByTrackId";
        public const string UpdatePGTrackIdByTrackId = "Payment_UpdatePGSessionTrackId";
        public const string PreCheckPaymentUpdate = "Payment_PreCheckPaymetUpdate";
        public const string RenewPolicy = "RenewPolicyDetails";
        public const string PaymentErrorUpdateByTrackId = "Payment_ErrorUpdateByTrackId";
    }
}
