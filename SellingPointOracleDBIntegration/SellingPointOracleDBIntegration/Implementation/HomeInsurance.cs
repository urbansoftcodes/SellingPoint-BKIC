using SellingPoint.OracleDBIntegration.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation
{
    public class HomeInsurance
    {
        public TransactionWrapper IntegrateHomeToOracle(long homeID)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@HomeID", homeID)
                };

                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.HomeInsurance.IntegrateMotorInsurance, para);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
