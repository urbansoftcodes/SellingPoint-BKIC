using BKIC.SellingPoint.DL.BO;

namespace BKIC.SellingPoint.DL.BL.Repositories
{
    public interface IDropDowns
    {
        FetchDropDownsResponse FetchDropDown(string type);
        VehicleModelResponse GetVehicleModel(string make);
        VehicleBodyResponse GetVehicleBody(string make, string model);
        FetchDropDownsResponse FetchAgencyProducts(string agency, string agencyCode, string mainclass, string type);
        string FetchInsuranceProductCode(string agency, string agencyCode, int insuranceType);
    }
}