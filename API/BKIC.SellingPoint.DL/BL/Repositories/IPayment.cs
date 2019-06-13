using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IPayment
    {
        PaymentTrackInsertResult InsertPaymentTrackDetails(Int64 refID, string insuranceType, bool isrenew, string emailaddress);
        PaymentTrackDetailResult GetPaymentTrackDetails(string trackId);
        TransactionWrapper UpdatePaymentDetailsByTrackId(string trackId, string transactionNo, DateTime paymentDate,
        string paymentType, string paymentAuthorizationCode);
        TransactionWrapper UpdatePGTrackIdByTrackId(string trackId, string pgTrackId);
        PaymentUpdatePreCheckResult PreCheckPaymentUpdate(string trackId, string pgTrackId);
        PaymentErrorUpdateResponse UpdatePaymentError(PaymentErrorUpdateRequest updateRequest);
    }
}
