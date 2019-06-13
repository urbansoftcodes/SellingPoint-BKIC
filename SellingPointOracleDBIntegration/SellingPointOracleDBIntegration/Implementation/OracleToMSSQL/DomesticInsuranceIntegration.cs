using SellingPoint.OracleDBIntegration.DBObjects;
using SellingPoint.OracleDBIntegration.StoredProcedure.OracleToMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation.OracleToMSSQL
{
    public class DomesticInsuranceIntegration
    {
        public TransactionWrapper IntegrateDomesticInsuranceMaster()
        {
            try
            {
                SellingPointSQL.weds(DomesticTableIntegration.DomesticDetailsIntegration);
                return new TransactionWrapper() { IsTransactionDone = true };
            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
