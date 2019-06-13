using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class BranchMaster : System.Web.UI.Page
    {
        General master;
        public BranchMaster()
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

                LoadBranchData(userInfo, service);
                ClearControl();
                BindDropdown(userInfo, service);
            }
        }

        private void BindDropdown(OAuthTokenResponse userInfo, DataServiceManager service)
        {         


            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns
                                .Replace("{type}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.UserMaster));


            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable AgencyDt = dropdownds.Tables["AgentCodeDD"];
                DataTable AgencyCodeDt = dropdownds.Tables["AgentCodeDD"];
                DataTable agentBranchDt = dropdownds.Tables["AgentBranchDD"];

                ddlAgency.DataValueField = "AgentCode";
                ddlAgency.DataTextField = "Agency";
                ddlAgency.DataSource = AgencyDt;
                ddlAgency.DataBind();
                ddlAgency.Items.Insert(0, new ListItem("--Please Select--", ""));              
                ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));
                ddlAgency.Enabled = false;

                //ddlAgentCode.DataValueField = "Agency";
                //ddlAgentCode.DataTextField = "AgentCode";
                //ddlAgentCode.DataSource = AgencyCodeDt;
                //ddlAgentCode.DataBind();
                //ddlAgentCode.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlAgentBranch.DataValueField = "Agency";
                //ddlAgentBranch.DataTextField = "AgentBranch";
                //ddlAgentBranch.DataSource = agentBranchDt;
                //ddlAgentBranch.DataBind();
                //ddlAgentBranch.Items.Insert(0, new ListItem("--Please Select--", ""));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    string opertaion = string.Empty;
                    
                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    var branchdetails = new BKIC.SellingPoint.DTO.RequestResponseWrappers.BranchMaster();

                    branchdetails.Agency = ddlAgency.SelectedItem.Text.Trim();
                    branchdetails.AgentCode = ddlAgency.SelectedItem.Value.ToString();
                    branchdetails.AgentBranch = txtAgentBranch.Text.Trim();
                    branchdetails.BranchName = txtBranchName.Text.Trim();
                    //branchdetails.BranchAddress = txtBranchAddress.Text.ToString();
                    branchdetails.Phone = txtPhone.Text.Trim();
                    branchdetails.Email = txtEmail.Text.Trim();
                    branchdetails.Incharge = txtIncharge.Text.Trim();
                    branchdetails.CreatedBy = "";

                    opertaion = (sender as Button).Text;

                    if (opertaion == "Update")
                    {
                        branchdetails.Id = Convert.ToInt32(ViewState["BrnachId"].ToString());
                        branchdetails.Type = "edit";
                    }
                    else
                    {
                        branchdetails.Type = "insert";
                    }

                    var branchResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.BranchMasterResponse>, 
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.BranchMaster>
                                      (BKIC.SellingPoint.DTO.Constants.AdminURI.BranchDetailsOperation, branchdetails);

                    if (branchResult.StatusCode == 200 && branchResult.Result.IsTransactionDone)
                    {
                        LoadBranchData(userInfo, service);
                        ClearControl();
                        ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));

                        if (branchdetails.Type == "insert")
                            master.ShowErrorPopup("Branch details saved sucessfully", "Branch"); 
                        else
                            master.ShowErrorPopup("Branch details updated sucessfully", "Branch");
                        //btnSubmit.Text = (branchdetails.Type == "edit") ? "Save" : "Update";
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

        private void ClearControl()
        {
           
            ddlAgency.SelectedIndex = 0;
           // txtAgentCode.Text = string.Empty;
            txtAgentBranch.Text = string.Empty;
            txtBranchName.Text = string.Empty;
            //txtBranchAddress.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtIncharge.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }
        public void LoadBranchData(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            

            var response = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.MasterTableResult
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.MTBranchMaster>>>
                           (BKIC.SellingPoint.DTO.Constants.AdminURI.GetMasterTableByTableName
                           .Replace("{tableName}", BKIC.SellingPoint.DTO.RequestResponseWrappers.MasterTable.BranchMaster));

            if (response.StatusCode == 200 && response.Result.IsTransactionDone) 
            {
                List<MTBranchMaster> listBranch = new List<MTBranchMaster>();
                listBranch = (List<MTBranchMaster>)response.Result.TableRows;
                gvBranch.DataSource = listBranch.FindAll(c => c.AGENCY == userInfo.Agency);
                gvBranch.DataBind();
            }
            else
            {
                master.ShowErrorPopup("Branch details loaded failed", "Branch");
            }
        }

        #region Test_1
    protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {          
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            gvBranch.PageIndex = e.NewPageIndex;
            LoadBranchData(userInfo, service); //bindgridview will get the data source and bind it again
    }
    protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
    {
       
    }

        protected void gvBranch_RowDataBound(object sender, EventArgs e)
        {
            //Change the Index number as per your Grid Column
            foreach (GridViewRow row in gvBranch.Rows)
            {
                if (row.Cells[5].Text.Equals("&nbsp;") || row.Cells[5].Text.Equals("&amp;nbsp;"))
                {
                    row.Cells[5].Text = string.Empty;
                }
                if (row.Cells[6].Text.Equals("&nbsp;") || row.Cells[6].Text.Equals("&amp;nbsp;"))
                {
                    row.Cells[6].Text = string.Empty;
                }
                if (row.Cells[7].Text.Equals("&nbsp;") || row.Cells[7].Text.Equals("&amp;nbsp;"))
                {
                    row.Cells[7].Text = string.Empty;
                }
                //row.Cells[5].Text = HttpUtility.HtmlDecode()
            }
            
        }
        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ClearControl();
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    if (ViewState["BrnachId"] != null)
                    {
                        ViewState["BrnachId"] = string.Empty;
                    }
                    string id = HttpUtility.HtmlDecode(row.Cells[1].Text.Trim());
                    ViewState["BrnachId"] = id;
                    ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(HttpUtility.HtmlDecode(row.Cells[2].Text.Trim())));
                    // txtAgentCode.Text = row.Cells[3].Text.Trim();
                    txtAgentBranch.Text = HttpUtility.HtmlDecode(row.Cells[3].Text.Trim());

                    txtBranchName.Text = HttpUtility.HtmlDecode(row.Cells[4].Text.Trim());
                    //txtBranchAddress.Text = row.Cells[6].Text.Trim();
                    txtPhone.Text = HttpUtility.HtmlDecode(row.Cells[5].Text.Trim());
                    txtIncharge.Text = HttpUtility.HtmlDecode(row.Cells[6].Text.Trim()); 
                    txtEmail.Text = HttpUtility.HtmlDecode(row.Cells[7].Text.Trim());
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

                    int id = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text.Trim()));
                   
                    var branchdetails = new BKIC.SellingPoint.DTO.RequestResponseWrappers.BranchMaster();

                    branchdetails.Id = id;
                    branchdetails.Type = "delete";

                    var branchResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.BranchMasterResponse>, 
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.BranchMaster>
                                       (BKIC.SellingPoint.DTO.Constants.AdminURI.BranchDetailsOperation, branchdetails);
                    if (branchResult.StatusCode == 200 && branchResult.Result.IsTransactionDone)
                    {
                        LoadBranchData(userInfo, service);
                        ClearControl();
                        ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));
                        master.ShowErrorPopup("Branch details deleted successfuly", "Branch");
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

        protected void ddlAgency_Changed(object sender, EventArgs e)
        {
            //txtAgentCode.Text = ddlAgency.SelectedValue;
        }

        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "Save";
            ClearControl();
            Response.Redirect("Homepage.aspx");
        }
        
    }
}