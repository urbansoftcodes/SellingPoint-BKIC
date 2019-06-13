using BKIC.SellingPoint.DL.BO;
using KBIC.DL.BL.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    public class BKICDropDowns : IBKICDropDowns
    {
        public DropDownResult GetPageDropDowns(string pageName)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@PageType", pageName)
                };

                DataSet result = BKICSQL.eds(StoredProcedures.BKICDropDowns.GetPageDropDowns, para);
                
                if(pageName == PageType.Profile || pageName == PageType.TravelInsurance)
                {
                    DataTable userTitle = new DataTable();
                    userTitle.Columns.Add("Value");
                    userTitle.Columns.Add("Text");
                    userTitle.Rows.Add("MR", "MR");
                    userTitle.Rows.Add("MRS", "MRS");
                    userTitle.Rows.Add("MISS", "MISS");

                    DataTable userGender = new DataTable();
                    userGender.Columns.Add("Value");
                    userGender.Columns.Add("Text");
                    userGender.Rows.Add("M", "Male");
                    userGender.Rows.Add("F", "Female");

                    result.Tables.Add(userTitle);
                    result.Tables.Add(userGender);
                }

                if(pageName == PageType.MotorQuote || pageName == PageType.MotorInsurance)
                {
                    DataTable motorType = new DataTable();
                    motorType.Columns.Add("Value");
                    motorType.Columns.Add("Text");
                    motorType.Rows.Add("NEW", "New Vehicle");
                    motorType.Rows.Add("USED", "Used Vehicle");

                    result.Tables.Add(motorType);


                    DataTable months = new DataTable();
                    months.Columns.Add("Months");
                    months.Rows.Add("January");
                    months.Rows.Add("February");
                    months.Rows.Add("March");
                    months.Rows.Add("April");
                    months.Rows.Add("May");
                    months.Rows.Add("June");
                    months.Rows.Add("July");
                    months.Rows.Add("August");
                    months.Rows.Add("September");
                    months.Rows.Add("October");
                    months.Rows.Add("November");
                    months.Rows.Add("December");
                    result.Tables.Add(months);

                    if (pageName == PageType.MotorInsurance)
                    {
                        DataTable deliveryOptions = new DataTable();
                        deliveryOptions.Columns.Add("Value");
                        deliveryOptions.Columns.Add("Text");
                        deliveryOptions.Rows.Add("BRANCH", "Branch");
                        deliveryOptions.Rows.Add("ADDRESS", "Deliver to address");

                        result.Tables.Add(deliveryOptions);

                        DataTable deliveryAddressOptions = new DataTable();
                        deliveryAddressOptions.Columns.Add("Value");
                        deliveryAddressOptions.Columns.Add("Text");
                        deliveryAddressOptions.Rows.Add("Same Address", "Same Address");
                        deliveryAddressOptions.Rows.Add("Different Address", "Different Address");

                        result.Tables.Add(deliveryAddressOptions);
                        DataTable CarReplacementDays = new DataTable();
                        CarReplacementDays.Columns.Add("Value");
                        CarReplacementDays.Columns.Add("Text");
                        CarReplacementDays.Rows.Add("7", "7 days");
                        CarReplacementDays.Rows.Add("14", "14 days");

                        result.Tables.Add(CarReplacementDays);
                    }

                }

                if(pageName == PageType.HomeQuote || pageName == PageType.HomeInsurance)
                {
                    DataTable propertyInsured = new DataTable();
                    propertyInsured.Columns.Add("Value");
                    propertyInsured.Columns.Add("Text");
                    propertyInsured.Rows.Add("1", "Yes");
                    propertyInsured.Rows.Add("0", "No");
                    result.Tables.Add(propertyInsured);

                    DataTable maliciousCover = new DataTable();
                    maliciousCover.Columns.Add("Value");
                    maliciousCover.Columns.Add("Text");
                    maliciousCover.Rows.Add("1", "Yes");
                    maliciousCover.Rows.Add("0", "No");
                    result.Tables.Add(maliciousCover);

                    DataTable helpCover = new DataTable();
                    helpCover.Columns.Add("Value");
                    helpCover.Columns.Add("Text");
                    helpCover.Rows.Add("1", "Yes");
                    helpCover.Rows.Add("0", "No");
                    result.Tables.Add(helpCover);

                    DataTable workersCover = new DataTable();
                    workersCover.Columns.Add("Value");
                    workersCover.Columns.Add("Text");
                    workersCover.Rows.Add("1", "1");
                    workersCover.Rows.Add("2", "2");
                    workersCover.Rows.Add("3", "3");
                    workersCover.Rows.Add("4", "4");
                    workersCover.Rows.Add("5", "5");
                    workersCover.Rows.Add("6", "6");
                    workersCover.Rows.Add("7", "7");
                    workersCover.Rows.Add("8", "8");
                    workersCover.Rows.Add("9", "9");
                    workersCover.Rows.Add("10", "10");
                    result.Tables.Add(workersCover);

                    if(pageName == PageType.HomeInsurance)
                    {
                        DataTable yesOrNo = new DataTable();
                        yesOrNo.Columns.Add("Value");
                        yesOrNo.Columns.Add("Text");
                        yesOrNo.Rows.Add("yes", "Yes");
                        yesOrNo.Rows.Add("no", "No");
                        result.Tables.Add(yesOrNo);

                        DataTable buildingAge = new DataTable();
                        buildingAge.Columns.Add("Value");
                        buildingAge.Columns.Add("Text");
                        buildingAge.Rows.Add("1", "1");
                        buildingAge.Rows.Add("2", "2");
                        buildingAge.Rows.Add("3", "3");
                        buildingAge.Rows.Add("4", "4");
                        buildingAge.Rows.Add("5", "5");
                        buildingAge.Rows.Add("6", "6");
                        buildingAge.Rows.Add("7", "7");
                        buildingAge.Rows.Add("8", "8");
                        buildingAge.Rows.Add("9", "9");
                        buildingAge.Rows.Add("10", "10");
                        buildingAge.Rows.Add("11", "11");
                        buildingAge.Rows.Add("12", "12");
                        buildingAge.Rows.Add("13", "13");
                        buildingAge.Rows.Add("14", "14");
                        buildingAge.Rows.Add("15", "15");
                        buildingAge.Rows.Add("16", "16");
                        buildingAge.Rows.Add("17", "17");
                        buildingAge.Rows.Add("18", "18");
                        buildingAge.Rows.Add("19", "19");
                        buildingAge.Rows.Add("20", "20");
                        buildingAge.Rows.Add("21", "20+");
                        result.Tables.Add(buildingAge);

                        DataTable residentialType = new DataTable();
                        residentialType.Columns.Add("Value");
                        residentialType.Columns.Add("Text");
                        residentialType.Rows.Add("H", "Residential Villa");
                        residentialType.Rows.Add("F", "Residential Flat/Apartment");
                        result.Tables.Add(residentialType);

                    }

                    
                }
                if(pageName==PageType.DomesticHelp)
                {
                    DataTable domesticInsuranceYears = new DataTable();
                    domesticInsuranceYears.Columns.Add("Value");
                    domesticInsuranceYears.Columns.Add("Text");
                    domesticInsuranceYears.Rows.Add("1", "1");
                    domesticInsuranceYears.Rows.Add("2", "2");
                    result.Tables.Add(domesticInsuranceYears);

                    DataTable domesticHelpWorkers = new DataTable();
                    domesticHelpWorkers.Columns.Add("Value");
                    domesticHelpWorkers.Columns.Add("Text");
                    domesticHelpWorkers.Rows.Add("1", "1");
                    domesticHelpWorkers.Rows.Add("2", "2");
                    domesticHelpWorkers.Rows.Add("3", "3");
                    domesticHelpWorkers.Rows.Add("4", "4");
                    domesticHelpWorkers.Rows.Add("5", "5");
                    domesticHelpWorkers.Rows.Add("6", "6");
                    domesticHelpWorkers.Rows.Add("7", "7");
                    domesticHelpWorkers.Rows.Add("8", "8");
                    domesticHelpWorkers.Rows.Add("9", "9");
                    domesticHelpWorkers.Rows.Add("10", "10");
                    result.Tables.Add(domesticHelpWorkers);

                    DataTable domesticHelpWorkersType = new DataTable();
                    domesticHelpWorkersType.Columns.Add("Value");
                    domesticHelpWorkersType.Columns.Add("Text");
                    domesticHelpWorkersType.Rows.Add("Domestic", "Domestic");
                    domesticHelpWorkersType.Rows.Add("Business", "Business");
                    result.Tables.Add(domesticHelpWorkersType);

                    DataTable userGender = new DataTable();
                    userGender.Columns.Add("Value");
                    userGender.Columns.Add("Text");
                    userGender.Rows.Add("M", "Male");
                    userGender.Rows.Add("F", "Female");
                    result.Tables.Add(userGender);
                }

                return new DropDownResult()
                {
                    IsTransactionDone = true,
                    DataSets = result
                };

            }
            catch(Exception exc)
            {
                return new DropDownResult()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = exc.Message
                };
            }
        }

        public VehicleModelResponse GetVehicleModel(string vehicleMake)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                     new SqlParameter("@Make", vehicleMake)
                };

                DataTable modeldt = BKICSQL.edt(StoredProcedures.BKICDropDowns.GetVehicleModel,param);
                return new VehicleModelResponse { IsTransactionDone = true, VehicleModeldt = modeldt };
            }
            catch(Exception ex)
            {
                return new VehicleModelResponse { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }
    }
}
