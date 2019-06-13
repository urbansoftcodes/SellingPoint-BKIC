using BKIC.SellingPoint.DL.BO;

namespace KBIC.DL.BL.Repositories
{
    public interface IBKICDropDowns
    {
        DropDownResult GetPageDropDowns(string pageName);
        VehicleModelResponse GetVehicleModel(string model);
    }
}