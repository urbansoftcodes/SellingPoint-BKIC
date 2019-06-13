using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BKIC.SellingPoint.DL.BL.Implementation
{
    /// <summary>
    /// Fetch dropdown's for the specific pages like branch dropdowns, user dropdowns...
    /// </summary>
    public class DropDowns : IDropDowns
    {
        /// <summary>
        /// Fetch dropdowns by page type.
        /// </summary>
        /// <param name="type">Page type(e.g domestic help page)</param>
        /// <returns>Dropdown response.</returns>
        public FetchDropDownsResponse FetchDropDown(string type)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@PageType",type)
                };
                DataSet ds = BKICSQL.eds(DropDownSP.FetchDropdowns, param);
                return new FetchDropDownsResponse
                {
                    IsTransactionDone = true,
                    dropdownds = ds
                };
            }
            catch (Exception ex)
            {
                return new FetchDropDownsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get the vehicle model's based on the make.
        /// e.g for make (BMW) fetch all the models that make 318i,320,535I..
        /// </summary>
        /// <param name="vehicleMake">vehicle make</param>
        /// <returns>vehicle model's</returns>
        public VehicleModelResponse GetVehicleModel(string vehicleMake)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                     new SqlParameter("@Make", vehicleMake)
                };

                DataTable dt = BKICSQL.edt(StoredProcedures.BKICDropDowns.GetVehicleModel, param);
                return new VehicleModelResponse
                {
                    IsTransactionDone = true,
                    VehicleModeldt = dt                    
                };
            }
            catch (Exception ex)
            {
                return new VehicleModelResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get vehicle body type for the particular make and model.
        /// e.g for make(BMW) and model(318i) fetch the body type's  SALOON,HATCHBACK..
        /// </summary>
        /// <param name="vehicleMake">Vehicle make.</param>
        /// <param name="vehicleModel">Vehicle model.</param>
        /// <returns>Vehicle body response.</returns>
        public VehicleBodyResponse GetVehicleBody(string vehicleMake, string vehicleModel)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                     new SqlParameter("@Make", vehicleMake),
                     new SqlParameter("@Model", vehicleModel)
                };

                DataSet ds = BKICSQL.eds(StoredProcedures.BKICDropDowns.GetVehicleBody, param);
                return new VehicleBodyResponse
                {
                    IsTransactionDone = true,
                    VehicleBodydt = ds.Tables[0],
                    VehicleEngineCCdt = ds.Tables[1]
                };
            }
            catch (Exception ex)
            {
                return new VehicleBodyResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get agency product's based on the insurance type.
        /// e.g for the agency SECURA and insurance type MOTOR fetch the product's (Product 1 and Prodcut 2)
        /// for the agency TISCO and insurance type MOTOR fetch the product's (GLD,NMC,PLT etc.)
        /// </summary>
        /// <param name="agency">Agency.</param>
        /// <param name="agencyCode">Agent code.</param>
        /// <param name="mainclass">MainClass(e.g MOTOR product's for SECURA -SECUR for TISCO - TISCO)</param>
        /// <param name="type">Insurance type.</param>
        /// <returns></returns>
        public FetchDropDownsResponse FetchAgencyProducts(string agency, string agencyCode, string mainclass, string type)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[] {
                    new SqlParameter("@Agency", agency),
                    new SqlParameter("@AgencyCode",agencyCode),
                    new SqlParameter("@MainClass", mainclass),
                    new SqlParameter("@PageType",type)
                };
                DataSet ds = BKICSQL.eds(DropDownSP.FetchAgencyProducts, param);
                return new FetchDropDownsResponse
                {
                    IsTransactionDone = true,
                    dropdownds = ds
                };
            }
            catch (Exception ex)
            {
                return new FetchDropDownsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Fetch insurance product codes.
        /// e.g for the insurance type domestichelp MAINCLASS is PADN.
        /// </summary>
        /// <param name="agency">Agency.</param>
        /// <param name="agencyCode">Agent Code.</param>
        /// <param name="insuranceTypeID">Insurance TypeID(1-domesticHelp, 2-Travel)</param>
        /// <returns></returns>
        public string FetchInsuranceProductCode(string agency, string agencyCode, int insuranceTypeID)
        {
            SqlParameter[] param = new SqlParameter[]
             {
                 new SqlParameter("@Agency", agency),
                 new SqlParameter("@AgentCode",agencyCode),
                 new SqlParameter("@InsuranceTypeID", insuranceTypeID)
             };
            List<SPOut> outParams = new List<SPOut>()
            {
               new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@MainClass", Size= 50}
            };
            object[] dataSet = BKICSQL.GetValues(DropDownSP.FetchInsuranceProductCode, param, outParams);
            if (dataSet != null && dataSet[0] != null)
            {
                return dataSet[0].ToString();
            }
            return string.Empty;
        }
    }
}