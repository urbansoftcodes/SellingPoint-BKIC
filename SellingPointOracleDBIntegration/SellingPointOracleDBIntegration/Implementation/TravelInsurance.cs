using SellingPoint.OracleDBIntegration.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation
{
    public class TravelInsurance
    {
        public TransactionWrapper IntegrateTravelToOracle(long travelId)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@TRAVELID", travelId)
                };

                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.TravelInsurance.IntegrateTravelInsuraceByTravelId, para);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
