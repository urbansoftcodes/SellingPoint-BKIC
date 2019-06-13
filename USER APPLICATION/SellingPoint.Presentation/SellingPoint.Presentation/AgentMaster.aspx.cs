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
    public partial class AgentMaster : System.Web.UI.Page
    {
        private General master;

        public AgentMaster()
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
                LoadAgentData(userInfo, service);
                LoadCategoryData(userInfo, service);
                ClearControl();
                ClearCategoryControl();
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

                    var details = new DTO.RequestResponseWrappers.AgentMaster();
                    details.Agency = txtAgency.Text.Trim();
                    details.AgentCode = txtAgentCode.Text.Trim();
                    details.CustomerCode = txtCustomerCode.Text.Trim();

                    // details.AgentBranch = txtAgentBranch.Text.ToString();

                    opertaion = (sender as Button).Text;

                    if (opertaion == "Update")
                    {
                        details.Id = Convert.ToInt32(ViewState["AgentId"].ToString());
                        details.Type = "edit";
                    }
                    else
                    {
                        details.Type = "insert";
                    }

                    var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgentMasterResponse>, 
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.AgentMaster>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.AgentOperation, details);
                    if (results.StatusCode == 200)
                    {
                        LoadAgentData(userInfo, service);
                        ClearControl();

                        if (details.Type == "insert")
                            master.ShowErrorPopup("Agent details saved sucessfully", "Agent");
                        else
                            master.ShowErrorPopup("Agent details updated sucessfully", "Agent");
                        
                        btnSubmit.Text = "Save";
                    }
                    else
                    {
                        Response.Write("<script>alert('Unauthorized')</script>");
                        //ExtensionMethod.MsgBox("Unauthorized!", , this);
                    }
                }
            }
            catch (Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        private void ClearControl()
        {
            txtAgency.Text = string.Empty;
            txtAgentCode.Text = string.Empty;
            txtCustomerCode.Text = string.Empty;
        }

        public void LoadAgentData(OAuthTokenResponse userInfo, DataServiceManager service)
        {           

            var response = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.MasterTableResult
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.MTAgentMaster>>>
                           (BKIC.SellingPoint.DTO.Constants.AdminURI.GetMasterTableByTableName
                           .Replace("{tableName}", BKIC.SellingPoint.DTO.RequestResponseWrappers.MasterTable.AgentMaster));

            if (response.StatusCode == 200 && response.Result.IsTransactionDone)
            {
                if (this.gvAgentMaster.DataSource != null)
                {
                    this.gvAgentMaster.DataSource = null;
                }
                gvAgentMaster.DataSource = response.Result.TableRows;
                gvAgentMaster.DataBind();

                //ddlAgency.DataValueField = "AgentCode";
                //ddlAgency.DataTextField = "Agency";
                //ddlAgency.DataSource = response.Result.TableRows;
                //ddlAgency.DataBind();
                //ddlAgency.Items.Insert(0, new ListItem("--Please Select--", ""));
            }
            else
            {
                master.ShowErrorPopup("Agent data loaded failed", "AGENT");
            }
           
        }

        public void LoadCategoryData(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster categoryReq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster();
            categoryReq.Type = "fetch";

            var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMasterResponse>,
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.CategoryMasterOperation, categoryReq);           

            if (response.StatusCode == 200 && response.Result.IsTransactionDone)
            {
                //gvCategoryMaster.DataSource = response.Result.Categories;
                //gvCategoryMaster.DataBind();
            }
            else
            {
                master.ShowErrorPopup("Commission data loaded failed", "COMMISSION");
            }
        }

        protected void btnCategorySubmit_Click(object sender, EventArgs e)
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
                    //details.Agency = ddlAgency.SelectedItem.Text;
                    //details.AgentCode = ddlAgency.SelectedItem.Value;
                    //details.Category = txtCategory.Text.ToString();
                    //details.Code = txtCode.Text.ToString();
                    //details.MainClass = txtMainclass.Text.ToString();
                    //details.SubClass = txtSubClass.Text.ToString();
                    //details.ValueType = txtValueType.Text.ToString();
                    //details.Value = Convert.ToDecimal(txtValue.Text);
                    

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

                    var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMasterResponse>,
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.CategoryMasterOperation, details);

                    if (response.StatusCode == 200 && response.Result.IsTransactionDone)
                    {
                        LoadCategoryData(userInfo, service);
                        ClearCategoryControl();                       

                        if (details.Type == "insert")
                            master.ShowErrorPopup("Commission details saved sucessfully", "Commission");
                        else
                            master.ShowErrorPopup("Commission details updated sucessfully", "Commission");
                        
                       // btnCategorySubmit.Text = "Save";
                    }
                }
            }
            catch (Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void btnCategoryCancel_Click(object sender, EventArgs e)
        {
            //btnCategorySubmit.Text = "Save";
            ClearCategoryControl();
        }

        private void ClearCategoryControl()
        {
            txtAgentCode.Text = string.Empty;
            //txtCode.Text = string.Empty;
            //txtMainclass.Text = string.Empty;
            //txtSubClass.Text = string.Empty;
            //txtValueType.Text = string.Empty;
            //txtValue.Text = string.Empty;
            //txtCategory.Text = string.Empty;            
        }

        protected void lnkbtnCategoryEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    if (ViewState["CategoryId"] != null)
                    {
                        ViewState["CategoryId"] = string.Empty;
                    }
                    string id = HttpUtility.HtmlDecode(row.Cells[1].Text.Trim());
                    ViewState["CategoryId"] = id;
                    //ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(HttpUtility.HtmlDecode(row.Cells[2].Text.Trim())));                  
                    //txtAgentCode.Text = HttpUtility.HtmlDecode(row.Cells[3].Text.Trim());
                    //txtMainclass.Text = HttpUtility.HtmlDecode(row.Cells[4].Text.Trim());
                    //txtSubClass.Text = HttpUtility.HtmlDecode(row.Cells[5].Text.Trim());
                    //txtCategory.Text = HttpUtility.HtmlDecode(row.Cells[6].Text.Trim());

                    //txtCode.Text = HttpUtility.HtmlDecode(row.Cells[7].Text.Trim());
                    //txtValueType.Text = HttpUtility.HtmlDecode(row.Cells[8].Text.Trim());
                    //txtValue.Text = HttpUtility.HtmlDecode(row.Cells[9].Text.Trim());

                    //btnCategorySubmit.Text = "Update";
                }
            }
            catch (Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void lnkbtnCategoryDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {


                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    int id = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text.Trim()));

                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster();
                    details.id = id;
                    details.Type = "delete";                   

                    var categoryResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers
                                       .ApiResponseWrapper<BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMasterResponse>, 
                                        BKIC.SellingPoint.DTO.RequestResponseWrappers.CategoryMaster>
                                       (BKIC.SellingPoint.DTO.Constants.AdminURI.CategoryMasterOperation, details);

                    if (categoryResult.StatusCode == 200 && categoryResult.Result.IsTransactionDone)
                    {
                        LoadCategoryData(userInfo, service);
                    }
                }
            }
            catch (Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        #region Test_1

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            gvAgentMaster.PageIndex = e.NewPageIndex;
            LoadAgentData(userInfo, service); //bindgridview will get the data source and bind it again
        }

        protected void gv_CategoryPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            //  gvCategoryMaster.PageIndex = e.NewPageIndex;
            LoadCategoryData(userInfo, service); //bindgridview will get the data source and bind it again
        }

        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
            //dlist.DefaultView.Sort = e.SortExpression + " " + SortDir(e.SortExpression);
            //gvMotorInsurance.DataSource = dlist;
            //gvMotorInsurance.DataBind();
        }

        protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
        {
            //foreach (GridViewRow row in gvAgentMaster.Rows)
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
                    ClearControl();

                    if (ViewState["AgentId"] != null)
                    {
                        ViewState["AgentId"] = string.Empty;
                    }

                    //upnlAgentMaster.Update();
                    string id = HttpUtility.HtmlDecode(row.Cells[1].Text.Trim());
                    ViewState["AgentId"] = id;
                    txtAgency.Text = HttpUtility.HtmlDecode(row.Cells[2].Text.Trim());
                    txtAgentCode.Text = HttpUtility.HtmlDecode(row.Cells[3].Text.Trim());
                    txtCustomerCode.Text = HttpUtility.HtmlDecode(row.Cells[4].Text.Trim());
                    //txtAgentBranch.Text = row.Cells[4].Text.Trim();

                    btnSubmit.Text = "Update";
                }
            }
            catch (Exception ex)
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

                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgentMaster();
                    int id = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text.Trim()));

                    details.Id = id;
                    details.Type = "delete";

                    var agentResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                      <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgentMasterResponse>, 
                                      BKIC.SellingPoint.DTO.RequestResponseWrappers.AgentMaster>
                                      (BKIC.SellingPoint.DTO.Constants.AdminURI.AgentOperation, details);
                    if (agentResult.StatusCode == 200 && agentResult.Result.IsTransactionDone)
                    {
                        LoadAgentData(userInfo, service);
                        ClearControl();
                        master.ShowErrorPopup("Agent data deleted successfully", "Agent");
                        btnSubmit.Text = "Save";
                    }
                }
            }
            catch (Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        #endregion Test_1

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "Save";
            ClearControl();
            Response.Redirect("Homepage.aspx");
        }
    }
}