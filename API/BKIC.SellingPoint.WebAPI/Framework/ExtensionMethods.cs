using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BKIC.SellingPoint.WebAPI.Framework
{
    public static class ExtensionMethods
    {
        public static string DataTableToJSON(this DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }

        public static string DataSetToJSON(this DataSet ds)
        {
            int tableNamePosition = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (string tablename in ds.Tables[0].Rows[0][0].ToString().Split(','))
                        {
                            ds.Tables[++tableNamePosition].TableName = tablename;
                        }

                        ds.Tables.Remove(ds.Tables[0].TableName);
                    }
                }
            }
            

            return JsonConvert.SerializeObject(ds, Formatting.Indented);

        }

        public static string ConvertTodatestring(this DateTime dt)
        {
            if (dt != null)
            {
                string DateTime = Convert.ToString(dt);
                string[] date = DateTime.Split();
                return date[0];
            }

            return "";
        }

        public static void DeletFile(this string PathWithFileName)
        {
            if(System.IO.Directory.Exists(PathWithFileName))
            {
                System.IO.Directory.Delete(PathWithFileName);
            }
        }
    }


}