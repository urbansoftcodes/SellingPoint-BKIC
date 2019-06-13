using SellingPoint.OracleDBIntegration.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation
{
    public class MotorInsurance
    {


        public TransactionWrapper IntegrateMotorToOracle(long motorID)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@MotorID", motorID)
                };

                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.MotorInsurance.IntegrateMotorInsurance, para);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }


        public TransactionWrapper MoveIntegrationToOracle(int mototorID)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@MotorID",mototorID)
                };

                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.MotorInsurance.IntegrateMotorInsurance, para);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
 
}
