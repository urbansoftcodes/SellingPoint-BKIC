using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using BKIC.SellingPoint.Presentation;
using KBIC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SellingPoint.Presentation
{
    public partial class MotorVehicleMaster : System.Web.UI.Page
    {
        General master;
        public MotorVehicleMaster()
        {
            master = Master as General;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!Page.IsPostBack)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                btnSubmit.Text = "Save";
                LoadVehicle(userInfo, service);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (Page.IsValid)
                {
                    string opertaion = string.Empty;

                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMaster
                    {
                        Make = txtMake.Text.Trim(),
                        Model = txtModel.Text.Trim(),
                        Body = txtBodyType.Text.Trim(),
                        Tonnage = string.IsNullOrEmpty(txtCapacity.Text) ? 0 : Convert.ToInt32(txtCapacity.Text),
                        NewExcessAmount = string.IsNullOrEmpty(txtExcess.Text) ? 0 : Convert.ToDecimal(txtExcess.Text),
                        VehicleValue = string.IsNullOrEmpty(txtCapacity.Text) ? 0 : Convert.ToInt32(txtCapacity.Text),
                        SeatingCapacity = string.IsNullOrEmpty(txtSeatingCapacity.Text) ? 0 : Convert.ToInt32(txtSeatingCapacity.Text),
                        Year = string.IsNullOrEmpty(txtYear.Text) ? 0 : Convert.ToInt32(txtYear.Text),
                        VehicleType = "",
                        // details.Category = "";
                    };
                    opertaion = (sender as Button).Text;
                    if (opertaion == "Update")
                    {
                        details.ID = Convert.ToInt32(ViewState["Id"].ToString());
                        details.Type = "update";
                    }
                    else
                    {
                        details.Type = "insert";

                    }

                    var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMasterResponse>,
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMaster>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorVehicleOperation, details);

                    if (results.StatusCode == 200 && results.Result.IsTransactionDone)
                    {                     
                        btnSubmit.Text = "Save";
                        if (details.Type == "insert")
                        {
                            master.ShowErrorPopup("Motor vehicle added successfully", "Motor Vehicle");
                        }
                        if (details.Type == "Update")
                        {
                            master.ShowErrorPopup("Motor vehicle updated successfully", "Motor Vehicle");
                        }
                        LoadVehicle(userInfo, service);
                        ClearControls();
                    }
                    else
                    {
                        master.ShowErrorPopup(results.Result.TransactionErrorMessage, "Request Failed!");
                    }
                }
             }
            catch(Exception ex)
            {
                //throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "Save";            
            Response.Redirect("Homepage.aspx");
        }
        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                
                int id = Convert.ToInt32(row.Cells[1].Text.Trim());
                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMaster
                {
                    ID = id,
                    Type = "delete"
                };

                var vehicleResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                    <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMasterResponse>,
                                    BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMaster>
                                    (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorVehicleOperation, details);

                if (vehicleResult.StatusCode == 200 && vehicleResult.Result.IsTransactionDone)
                {
                    LoadVehicle(userInfo, service);
                    master.ShowErrorPopup("Motor vehicle deleted successfully", "Motor Vehicle");
                }
            }
        }

        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                   
                    if (ViewState["Id"] != null)
                    {
                        ViewState["Id"] = string.Empty;
                    }
                    string id = HttpUtility.HtmlDecode(row.Cells[1].Text);
                    ViewState["Id"] = id;

                    txtMake.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
                    txtModel.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
                    txtCapacity.Text = HttpUtility.HtmlDecode(row.Cells[4].Text);
                    txtVehicleValue.Text = HttpUtility.HtmlDecode(row.Cells[5].Text);
                    txtExcess.Text = HttpUtility.HtmlDecode(row.Cells[6].Text);
                    txtBodyType.Text = HttpUtility.HtmlDecode(row.Cells[7].Text);
                    txtYear.Text = HttpUtility.HtmlDecode(row.Cells[8].Text);
                    txtSeatingCapacity.Text = HttpUtility.HtmlDecode(row.Cells[9].Text);
                    btnSubmit.Text = "Update";
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }

        }

        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
            //dlist.DefaultView.Sort = e.SortExpression + " " + SortDir(e.SortExpression);
            //gvMotorInsurance.DataSource = dlist;
            //gvMotorInsurance.DataBind();
        }
        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //bindgridview will get the data source and bind it again
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            gvMotorVehicle.PageIndex = e.NewPageIndex;
            LoadVehicle(userInfo, service); 
        }

        private void LoadVehicle(OAuthTokenResponse userInfo, DataServiceManager service)
        {

            var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMaster
            {
                Type = "fetch",
                Make = txtMakeSearch.Text.Trim()
            };
            var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMasterResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorVehicleMaster>
                                 (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorVehicleOperation, request);

            if (results.StatusCode == 200 && results.Result.IsTransactionDone)
            {
                gvMotorVehicle.DataSource = results.Result.MotorVehicleMaster;
                gvMotorVehicle.DataBind();
            }
        }
        protected void gvMotorVehicle_DataBound(object sender, EventArgs e)
        {
        }

        protected void btnMakeSearch_Click(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();
                LoadVehicle(userInfo, service);
            }
            catch(Exception ex)
            {
                //throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        public void ClearControls()
        {
            txtMake.Text = string.Empty;
            txtModel.Text = string.Empty;
            txtBodyType.Text = string.Empty;
            txtCapacity.Text = string.Empty;
            txtExcess.Text = string.Empty;
            txtSeatingCapacity.Text = string.Empty;
            txtVehicleValue.Text = string.Empty;
            txtYear.Text = string.Empty;
        }
    }
}