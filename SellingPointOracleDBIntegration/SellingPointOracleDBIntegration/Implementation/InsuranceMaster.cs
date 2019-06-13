using SellingPoint.OracleDBIntegration.DBObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation
{
    public class InsuranceMaster
    {
        public TransactionWrapper IntegrateInsuredMasterToOracle(string insuranceCode)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@InsuredCode", insuranceCode)
                };

                SellingPointSQL.eds(StoredProcedure.MSSQLToOracle.InsuredMaster.IntegrateInsuredMaster, para);

                return new TransactionWrapper() { IsTransactionDone = true };

            }
            catch (Exception exc)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }
    }
}
