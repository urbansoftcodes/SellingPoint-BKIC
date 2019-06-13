using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class FetchDropDownsResponse : TransactionWrapper
    {

         [JsonProperty(PropertyName = "dropdownresult")]
        public string dropdownresult { get; set; }

       
    }
    public class FetchProductCodeResponse : TransactionWrapper
    {

        [JsonProperty(PropertyName = "productcode")]
        public string productCode { get; set; }


    }
}
