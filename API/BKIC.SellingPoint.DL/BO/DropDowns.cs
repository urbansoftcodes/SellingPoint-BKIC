using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BO
{
     public class FetchDropDownsResponse:TransactionWrapper 
    {
        public DataSet dropdownds { get; set; }

        public FetchDropDownsResponse()
        {
            dropdownds = new DataSet();
        }
    } 
}
