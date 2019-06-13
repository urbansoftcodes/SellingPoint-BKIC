using SellingPoint.OracleDBIntegration.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation
{
    public class MotorEndorsement
    {
        public TransactionWrapper MoveIntegrationToOracle(long mototorEndorsementID)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@EndorsementID",mototorEndorsementID)
                };

                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.MotorEndorsement.PushOracleMotorEndorsement, para);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
