using SellingPoint.OracleDBIntegration.DBObjects;
using SellingPoint.OracleDBIntegration.StoredProcedure.OracleToMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation.OracleToMSSQL
{
    public class TravelInsuranceIntegration
    {
        public TransactionWrapper IntegrateTravelInsurance()
        {
            try
            {
                SellingPointSQL.weds(TravelTableIntegration.TravelInsuranceIntegration);
                return new TransactionWrapper() { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }
    }
}
