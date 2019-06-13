using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class MotorProductCover : System.Web.UI.Page
    {
        General master;
        public static string MainClass { get; set; }
        public MotorProductCover()
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
                LoadMotorProductData(userInfo, service);
                ClearControl();
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

                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductCover
                    {
                        CoverCode = txtCoverCode.Text.Trim(),
                        CoverDescription = txtCoverDescription.Text.Trim(),
                        CoverAmount = string.IsNullOrEmpty(txtCoverAmount.Text) ? decimal.Zero :
                                          Convert.ToDecimal(txtCoverAmount.Text),
                        Agency = userInfo.Agency,
                        AgencyCode = userInfo.AgentCode,
                        Mainclass = MainClass,
                        SubClass = ddlCover.SelectedItem.Value.Trim(),
                        IsOptionalCover = chkIsOptionalCover.Checked,
                        CoverType = "Cover"
                    };

                    opertaion = (sender as Button).Text;
                    if (opertaion == "Update")
                    {
                        details.CoverId = Convert.ToInt32(ViewState["CoverId"].ToString());
                        details.Type = "edit";
                        details.UpdatedBy = "Admin";
                    }
                    else
                    {
                        details.Type = "insert";
                        details.UpdatedBy = "";
                    }

                    var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductCoverResponse>,
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductCover>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorProductCoverOperation, details);

                    if (results.StatusCode == 200 && results.Result.IsTransactionDone)
                    {
                        LoadProductCover();
                        ClearControl();
                        btnSubmit.Text = "Save";
                        if (details.Type == "insert")
                        {
                            master.ShowErrorPopup("Motor cover added successfully", "Motor Cover");
                        }
                        if (details.Type == "edit")
                        {
                            master.ShowErrorPopup("Motor cover updated successfully", "Motor Cover");
                        }
                    }
                    else
                    {
                        master.ShowErrorPopup(results.ErrorMessage, "Request Failed!");
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
            ClearControl();
            Response.Redirect("HomePage.aspx");
        }

        private void ClearControl()
        {           

            txtCoverCode.Text = string.Empty;
            txtCoverDescription.Text = string.Empty;
            txtCoverAmount.Text = string.Empty;
        }
        public void LoadMotorProductData(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var productCode = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchProductCodeResponse>>(
                             BKIC.SellingPoint.DTO.Constants.DropDownURI.GetInsuranceProductCode
                             .Replace("{agency}", userInfo.Agency)
                             .Replace("{agencyCode}", userInfo.AgentCode)
                             .Replace("{insurancetypeid}", "4"));

            MainClass = productCode.Result.productCode;

            if (productCode != null && productCode.StatusCode == 200 && productCode.Result.IsTransactionDone)
            {
                var products = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>(
                              BKIC.SellingPoint.DTO.Constants.DropDownURI.GetAgencyProducts
                              .Replace("{agency}", userInfo.Agency)
                              .Replace("{agencyCode}", userInfo.AgentCode)
                              .Replace("{mainclass}", productCode.Result.productCode)
                              .Replace("{page}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.MotorInsurance));


                if (products != null && products.StatusCode == 200 && products.Result.IsTransactionDone)
                {
                    DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(products.Result.dropdownresult);
                    DataTable prods = dropdownds.Tables["Products"];
                    ddlCover.DataValueField = "SUBCLASS";
                    ddlCover.DataTextField = "DESCRIPTION";
                    ddlCover.DataSource = prods;
                    ddlCover.DataBind();
                    ddlCover.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
            }

        }

        #region Test_1
        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            gvMotorProductCover.PageIndex = e.NewPageIndex;
            LoadMotorProductData(userInfo, service); //bindgridview will get the data source and bind it again
        }
        protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
        {
        }
        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    ClearControl();
                    if (ViewState["CoverId"] != null)
                    {
                        ViewState["CoverId"] = string.Empty;
                    }
                    string id = row.Cells[1].Text.Trim();
                    ViewState["CoverId"] = id;

                    txtCoverCode.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
                    txtCoverDescription.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);
                    txtCoverAmount.Text = HttpUtility.HtmlDecode(row.Cells[4].Text);
                    chkIsOptionalCover.Checked = Convert.ToBoolean(HttpUtility.HtmlDecode(row.Cells[5].Text));
                    btnSubmit.Text = "Update";
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
        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                ClearControl();
                int id = Convert.ToInt32(row.Cells[1].Text.Trim());
                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductCover
                {
                    CoverId = id,
                    Type = "delete"
                };

                var branchResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductCoverResponse>,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductCover>
                                   (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorProductCoverOperation, details);

                if (branchResult.StatusCode == 200 && branchResult.Result.IsTransactionDone)
                {
                    LoadProductCover();
                    master.ShowErrorPopup("Motor cover deleted successfully", "Motor Cover");
                }
            }
        }
        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
            //dlist.DefaultView.Sort = e.SortExpression + " " + SortDir(e.SortExpression);
            //gvMotorInsurance.DataSource = dlist;
            //gvMotorInsurance.DataBind();
        }

        public void GetCoversByProduct()
        {

        }

        protected void MotorProduct_changed(object sender, EventArgs e)
        {
            try
            {
                LoadProductCover();
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

        private void LoadProductCover()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var request = new MotorCoverRequest
            {
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode,
                MainClass = userInfo.Agency,
                SubClass = ddlCover.SelectedItem.Value
            };


            var coverResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                          <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCoverResponse>,
                          BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCoverRequest>
                          (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorCoverOperation, request);

            if (coverResult.StatusCode == 200 && coverResult.Result.IsTransactionDone)
            {
                gvMotorProductCover.DataSource = coverResult.Result.Covers;
                gvMotorProductCover.DataBind();
                gvMotorProductCover.Visible = true;
            }
        }

        #endregion
    }
}