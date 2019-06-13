using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using KBIC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class CategoryMaster : System.Web.UI.Page
    {
        General master;
        public CategoryMaster()
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
                LoadCategoryData(userInfo, service);
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

                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster();

                    details.Agency = txtAgency.Text.Trim();
                    details.AgentCode = txtAgentCode.Text.Trim();
                    details.Category = txtCategory.Text.Trim();
                    details.Code = txtCode.Text.Trim();
                    details.MainClass = txtMainclass.Text.Trim();
                    details.SubClass = txtSubClass.Text.Trim();
                    details.ValueType = txtValueType.Text.Trim();
                    details.Value = Convert.ToDecimal(txtValue.Text);
                    details.Status = true;

                    opertaion = (sender as Button).Text;

                    if (opertaion == "Update")
                    {
                        details.id = Convert.ToInt32(ViewState["CategoryId"].ToString());
                        details.Type = "edit";
                    }
                    else
                    {
                        details.Type = "insert";
                    }

                    var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMasterResponse>,
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.CategoryMasterOperation, details);

                    if (results.StatusCode == 200 && results.Result.IsTransactionDone)
                    {
                        LoadCategoryData(userInfo, service);
                        ClearControl();
                        if (details.Type == "insert")
                            master.ShowErrorPopup("Commission details saved sucessfully", "Commission");
                        else
                            master.ShowErrorPopup("Commission details updated sucessfully", "Commission");

                        btnSubmit.Text = "Save";
                    }
                }
            }
            catch(Exception ex)
            {
                ////throw ex;
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
        }

        private void ClearControl()
        {
            txtAgency.Text = string.Empty;
            txtAgentCode.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtMainclass.Text = string.Empty;
            txtSubClass.Text = string.Empty;
            txtValueType.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtCategory.Text = string.Empty;           
        }
        public void LoadCategoryData(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster categoryReq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster();
            categoryReq.Type = "fetch";
            categoryReq.Agency = userInfo.Agency;
            categoryReq.AgentCode = userInfo.AgentCode;

            var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMasterResponse>,
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.CategoryMasterOperation, categoryReq);

            if (response.StatusCode == 200 && response.Result.IsTransactionDone)
            {
                gvCategoryMaster.DataSource = response.Result.Categories;
                gvCategoryMaster.DataBind();
            }
            else
            {
                master.ShowErrorPopup("Commission data loaded failed", "Commission");
            }

        }

    protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {        
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            gvCategoryMaster.PageIndex = e.NewPageIndex;
            LoadCategoryData(userInfo, service); //bindgridview will get the data source and bind it again
    }
    protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
    {
        //foreach (GridViewRow row in gvCategoryMaster.Rows)
        //{
        //    string HIRStatusCode = (row.FindControl("lblHIRStatusCode") as Label).Text.Trim();
        //    string IsMessage = (row.FindControl("lblIsMessage") as Label).Text.Trim();
        //    string IsDocuments = (row.FindControl("IsDocument") as Label).Text.Trim();

        //    if (IsDocuments == "True")
        //    {
        //        var btnHIRFiles = row.FindControl("btnDocument") as LinkButton;
        //        btnHIRFiles.Visible = true;
        //    }
        //    else
        //    {
        //        var btnHIRFiles = row.FindControl("btnDocument") as LinkButton;
        //        btnHIRFiles.Visible = false;
        //    }

        //    if (IsMessage == "True")
        //    {
        //        var btnDocFiles = row.FindControl("btnViewMail") as LinkButton;
        //        btnDocFiles.Visible = true;
        //    }
        //    else
        //    {
        //        var btnDocFiles = row.FindControl("btnViewMail") as LinkButton;
        //        btnDocFiles.Visible = false;
        //    }
        //}
         }
protected void lnkbtnEdit_Click(object sender, EventArgs e)
{
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    if (ViewState["CategoryId"] != null)
                    {
                        ViewState["CategoryId"] = string.Empty;
                    }
                    string id = row.Cells[1].Text.Trim();
                    ViewState["CategoryId"] = id;

                    txtAgency.Text = row.Cells[2].Text.Trim();
                    txtAgentCode.Text = row.Cells[3].Text.Trim();
                    txtMainclass.Text = row.Cells[4].Text.Trim();
                    txtSubClass.Text = row.Cells[5].Text.Trim();
                    txtCategory.Text = row.Cells[6].Text.Trim();

                    txtCode.Text = row.Cells[7].Text.Trim();
                    txtValueType.Text = row.Cells[8].Text.Trim();
                    txtValue.Text = row.Cells[9].Text.Trim();

                    btnSubmit.Text = "Update";
                }
            }
            catch(Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
       
    }
        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster categoryReq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster();

                    int id = Convert.ToInt32(row.Cells[1].Text.Trim());
                    categoryReq.id = id;
                    categoryReq.Type = "delete";

                    var categoryResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMasterResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster>
                                       (BKIC.SellingPoint.DTO.Constants.AdminURI.CategoryMasterOperation, categoryReq);
                    if (categoryResponse.StatusCode == 200 && categoryResponse.Result.IsTransactionDone)
                    {
                        LoadCategoryData(userInfo, service);
                        ClearControl();
                        master.ShowErrorPopup("Commission data deleted successfully", "Commission");
                        btnSubmit.Text = "Save";
                    }
                }
            }
            catch(Exception ex)
            {
                ////throw ex;
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
    }
}