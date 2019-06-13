using SellingPoint.OracleDBIntegration.DBObjects;
using SellingPoint.OracleDBIntegration.StoredProcedure.OracleToMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation.OracleToMSSQL
{
    public class HomeInsuranceIntegration
    {
        public TransactionWrapper IntegrateHomeInsurance()
        {
            try
            {
                SellingPointSQL.weds(HomeTablesIntegration.HomeDetailsIntegration);
                return new TransactionWrapper() { IsTransactionDone = true };
            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }

        public TransactionWrapper IntegrateRenewalHomeInsurance()
        {
            try
            {
                SellingPointSQL.weds(HomeTablesIntegration.RenewalHomeInsuranceIntegration);
                return new TransactionWrapper() { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }
    }
}
