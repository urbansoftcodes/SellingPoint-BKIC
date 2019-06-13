using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BKIC.SellingPoint.Presentation
{
    public static class ExtensionMethod
    {
        public static List<T> BindList<T>(this DataTable dt)
        {

            var fields = typeof(T).GetFields();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = dr[dc.ColumnName];

                            // Set the value into the object
                            fieldInfo.SetValue(ob, value);
                            break;
                        }
                    }
                }

                lst.Add(ob);
            }

            return lst;
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


        public static string ConvertTodatestringNullable(this DateTime? dt)
        {
            if (dt != null)
            {
                string DateTime = Convert.ToString(dt);
                string[] date = DateTime.Split();
                return date[0];
            }

            return "";
        }


        public static DateTime CovertToCustomDateTime(this string value)
        {
            return DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.CurrentCulture);
        }

        public static DateTime CovertToCustomDateTime2(this string value)
        {

            string[] formats = {
                "MM/dd/yyyy hh:mm:ss tt",
                "M/dd/yyyy hh:mm:ss tt", "M/d/yyyy hh:mm:ss tt", "M/d/yyyy hh:mm:ss tt"
            ,  "M/d/yyyy hh:mm:ss tt"};


            return DateTime.ParseExact(value, formats, CultureInfo.CurrentCulture, DateTimeStyles.None);
        }

        public static DateTime? ConvertToDateTimeNull(this string value)
        {
            return !(string.IsNullOrEmpty(value)) ? DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.CurrentCulture)
                : (DateTime?)null;
        }

        public static string CovertToLocalFormat(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy");
        }

        public static string ConvertToLocalFormat(this DateTime? value)
        {
            if (value.HasValue)
            {
                return value.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                return "";
            }
        }

        public static void MsgBox(String ex, Page pg, Object obj)
        {
            string s = "<SCRIPT language='javascript'>alert('" + ex.Replace("\r\n", "\\n").Replace("'", "") + "'); </SCRIPT>";
            Type cstype = obj.GetType();
            ClientScriptManager cs = pg.ClientScript;
            cs.RegisterClientScriptBlock(cstype, s, s.ToString());
        }
        
        public static DataTable GetDistictModel(string subClass, DataTable dt)
        {
            List<object> results = new List<object>();
            if (dt != null)
            {
                if (subClass == "TMCTR")
                {

                    results = (from myRow in dt.AsEnumerable()
                               where myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.MiniBus ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.PickUp ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Bus ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Truck ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.SixWheel
                               select myRow["Model"]).Distinct().ToList();
                }
                else if (subClass == "TSPRT")
                {
                     results = (from myRow in dt.AsEnumerable()
                                  where myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Coupe ||
                                        myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Sport
                                  select myRow["Model"]).Distinct().ToList();
                }
                else if (subClass == "NMC" || subClass == "ELT" ||subClass == "GLD" || subClass == "PLT")
                {
                    results = (from myRow in dt.AsEnumerable()
                               where myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Saloon ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Jeep ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Van ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.HatchBack ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.HatchBack1 ||
                                     myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Suv
                               select myRow["Model"]).Distinct().ToList();                   
                }
                else
                {
                     results = (from myRow in dt.AsEnumerable()
                                  select myRow["Model"]).Distinct().ToList();
                }                
            }
            return ConvertListToDataTable(results, "Model");
        }


        public static DataTable GetDistinctMake(string subClass, DataTable dt)
        {
            List<object> results = new List<object>();
            if (dt != null)
            {
                if (subClass == "TMCTR")
                {

                    results = (from myRow in dt.AsEnumerable()
                              where myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.MiniBus ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.PickUp ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Bus ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Truck ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.SixWheel
                              select myRow["Make"]).Distinct().ToList();
                }
                else if (subClass == "TSPRT")
                {
                    results = (from myRow in dt.AsEnumerable()
                              where myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Coupe ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Sport
                              select myRow["Make"]).Distinct().ToList();
                }
                else if (subClass == "NMC" || subClass == "ELT" || subClass == "GLD" || subClass == "PLT")
                {
                    results = (from myRow in dt.AsEnumerable()
                              where myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Saloon ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Jeep ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Van ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.HatchBack ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.HatchBack1 ||
                                    myRow.Field<string>("Body") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Suv
                              select myRow["Make"]).Distinct().ToList();
                }
                else
                {
                    results = (from myRow in dt.AsEnumerable()
                              select myRow["Make"]).Distinct().ToList();
                }
            }
            return ConvertListToDataTable(results, "Make");
        }

        public static DataTable GetDistinctBody(string subClass, DataTable dt)
        {
            List<object> results = new List<object>();
            if (dt != null)
            {
                if (subClass == "TMCTR")
                {

                    results = (from myRow in dt.AsEnumerable()
                               where myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.MiniBus ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.PickUp ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Bus ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Truck ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.SixWheel
                               select myRow["BodyType"]).ToList();
                }
                else if (subClass == "TSPRT")
                {
                    results = (from myRow in dt.AsEnumerable()
                               where myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Coupe ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Sport
                               select myRow["BodyType"]).Distinct().ToList();
                }
                else if (subClass == "NMC" || subClass == "ELT" || subClass == "GLD" || subClass == "PLT")
                {
                    results = (from myRow in dt.AsEnumerable()
                               where myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Saloon ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Jeep ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Van ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.HatchBack ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.HatchBack1 ||
                                     myRow.Field<string>("BodyType") == BKIC.SellingPoint.DL.Constants.VehicleBodyTypes.Suv
                               select myRow["BodyType"]).Distinct().ToList();
                }
                else
                {
                    results = (from myRow in dt.AsEnumerable()
                               select myRow["BodyType"]).Distinct().ToList();
                }
            }
            return ConvertListToDataTable(results, "BodyType");
        }

        public static DataTable GetDistinctArea(DataTable dt)
        {

            List<object> results = new List<object>();

            results = (from myRow in dt.AsEnumerable()                      
                       select myRow["Description"]).Distinct().ToList();

            return ConvertListToDataTable(results, "Area");
        }


        static DataTable ConvertListToDataTable(List<object> list, string columnName)
        {
            // New table.
            DataTable table = new DataTable();                     

            // Add columns.
            for (int i = 0; i < 1; i++)
            {
                table.Columns.Add(columnName);
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }

    }
}