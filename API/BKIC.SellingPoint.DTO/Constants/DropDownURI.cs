namespace BKIC.SellingPoint.DTO.Constants
{
    public class DropDownURI
    {
        public const string GetPageDropDowns = "api/pagedropdonw/getdropdowns/{pageName}";
        public const string GetVehicleModel = "api/pagedropdown/getVehicleModel/{vehicleMake}";
        public const string GetVehicleBody = "api/pagedropdown/getVehicleBody";
        public const string GetAgencyProducts = "api/pagedropdown/getproducts/{agency}/{agencyCode}/{mainclass}/{page}";
        public const string GetInsuranceProductCode = "api/pagedropdown/getinsuranceproductcode/{agency}/{agencyCode}/{insurancetypeid}";
    }
}