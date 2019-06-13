using SellingPoint.OracleDBIntegration.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation
{
    public class DomesticInsurance
    {
        public TransactionWrapper IntegrateDomesticToOracle(long domesticID)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@DomesticID", domesticID)
                };
                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.DomesticInsurance.IntegrateDomesticDetails, para);
                return new TransactionWrapper() { IsTransactionDone = true };
            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
