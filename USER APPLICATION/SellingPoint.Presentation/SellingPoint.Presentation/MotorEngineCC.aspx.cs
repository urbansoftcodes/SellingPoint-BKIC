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
    public partial class MotorEngineCC : System.Web.UI.Page
    {
        General master;
        public MotorEngineCC()
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

                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCMaster
                    {
                        Tonnage = Convert.ToInt32(txtEngineCC.Text.Trim()),
                        Capacity = txtEngineCC.Text.Trim() + "CC"
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
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCResponse>,
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCMaster>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorEngineCCOperation, details);

                    if (results.StatusCode == 200 && results.Result.IsTransactionDone)
                    {
                        btnSubmit.Text = "Save";
                        if (details.Type == "insert")
                        {
                            master.ShowErrorPopup("Motor Engine CC added successfully", "Motor EngineCC");
                        }
                        if (details.Type == "Update")
                        {
                            master.ShowErrorPopup("Motor Engine CC updated successfully", "Motor EngineCC");
                        }
                        LoadVehicle(userInfo, service);
                    }
                    else
                    {
                        master.ShowErrorPopup(results.ErrorMessage, "Request Failed!");
                    }
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
                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCMaster
                {
                    ID = id,
                    Type = "delete"
                };

                var vehicleResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                    <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCResponse>,
                                    BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCMaster>
                                    (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorEngineCCOperation, details);

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
                    string id = row.Cells[1].Text.Trim();
                    ViewState["Id"] = id;

                    txtEngineCC.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
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
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            gvMotorEngineCC.PageIndex = e.NewPageIndex;
            LoadVehicle(userInfo, service); //bindgridview will get the data source and bind it again
        }

        private void LoadVehicle(OAuthTokenResponse userInfo, DataServiceManager service)
        {

            var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCMaster
            {
                Type = "fetch"
            };
            var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEngineCCMaster>
                                 (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorEngineCCOperation, request);

            if (results.StatusCode == 200 && results.Result.IsTransactionDone)
            {
                gvMotorEngineCC.DataSource = results.Result.MotorEngineCC;
                gvMotorEngineCC.DataBind();
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
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }
    }
}