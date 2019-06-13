using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SellingPoint.OracleDBIntegration.DBObjects
{
    public class TransactionWrapper
    {
        public bool IsTransactionDone { get; set; }
        public string TransactionErrorMessage { get; set; }
    }
}
