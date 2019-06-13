using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class TransactionWrapper
    {
        [JsonProperty(PropertyName = "isTransactionDone")]
        public bool IsTransactionDone { get; set; }
        [JsonProperty(PropertyName = "transactionErrorMessage")]
        public string TransactionErrorMessage { get; set; }
    }
}
