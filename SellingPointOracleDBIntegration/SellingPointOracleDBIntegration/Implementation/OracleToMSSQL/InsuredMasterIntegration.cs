using SellingPoint.OracleDBIntegration.DBObjects;
using SellingPoint.OracleDBIntegration.StoredProcedure.OracleToMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation.OracleToMSSQL
{
    public class InsuredMasterIntegration
    {
        public TransactionWrapper IntegrateInsuredMaster()
        {
            try
            {
                SellingPointSQL.weds(MasterTablesIntegration.InsuredMasterIntegration);
                return new TransactionWrapper() { IsTransactionDone = true };
            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
