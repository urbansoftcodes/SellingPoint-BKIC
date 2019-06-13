using SellingPoint.OracleDBIntegration.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation
{
   public class TravelEndorsement
    {
        public TransactionWrapper MoveIntegrationToOracle(long travelEndorsementID)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@EndorsementID",travelEndorsementID)
                };

                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.TravelEndorsement.PushOracleTravelEndorsement, para);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
