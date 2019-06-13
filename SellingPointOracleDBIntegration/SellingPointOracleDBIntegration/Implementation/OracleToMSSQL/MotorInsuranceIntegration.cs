using SellingPoint.OracleDBIntegration.DBObjects;
using SellingPoint.OracleDBIntegration.StoredProcedure.OracleToMSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.Implementation.OracleToMSSQL
{
    public class MotorInsuranceIntegration
    {
        public TransactionWrapper IntegrateMotorInsurance()
        {
            try
            {
                SellingPointSQL.weds(MotorTableIntegration.MotorInsuranceIntegration);
                return new TransactionWrapper() { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new TransactionWrapper() { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        public TransactionWrapper IntegrateRenewalMotorInsurance()
        {
            try
            {
                SellingPointSQL.weds(MotorTableIntegration.RenewalInsuranceIntegration);
                return new TransactionWrapper { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new TransactionWrapper { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }
    }
}
