using SellingPoint.OracleDBIntegration.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation
{
    public class HomeEndorsement
    {
        public TransactionWrapper MoveIntegrationToOracle(long homeEndorsementID)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@EndorsementID",homeEndorsementID)
                };

                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.HomeEndorsement.PushOracleHomeEndorsement, para);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
