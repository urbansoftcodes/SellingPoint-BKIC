using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class UserMaster : System.Web.UI.Page
    {
        General master;
        public UserMaster()
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
                userdetails.Visible = false;
                admindetails.Visible = true;
                ClearControl();
                LoadUserData(userInfo, service);                
                BindDropdown(userInfo, service);
                BindUserRole(userInfo, service);
            }
        }

        private void BindUserRole(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            string role = userInfo.Roles;

            if (role == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin)
            {
                ddlRole.Items.Add(new ListItem() { Text = "Branch Admin", Value = "BranchAdmin" });
                ddlRole.Items.Add(new ListItem() { Text = "User", Value = "User" });
            }
            else if (role == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
            {
                ddlRole.Items.Add(new ListItem() { Text = "User", Value = "User" });
            }
            else if (role == BKIC.SellingPoint.DL.Constants.Roles.User)
            {
                //ddlRole.Items.Add(new ListItem() { Text = "User", Value = "User" });
            }
            else
            {
                ddlRole.Items.Add(new ListItem() { Text = "Super Admin", Value = "SuperAdmin" });
                ddlRole.Items.Add(new ListItem() { Text = "Branch Admin", Value = "BranchAdmin" });
                ddlRole.Items.Add(new ListItem() { Text = "User", Value = "User" });
            }
            ddlRole.Items.Insert(0, new ListItem("--Please Select--", ""));
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
                ddlAgency.Enabled = ddlAgency.SelectedIndex > 0 ? false : true;

                ddlAgentCode.DataValueField = "Agency";
                ddlAgentCode.DataTextField = "AgentCode";
                ddlAgentCode.DataSource = AgencyCodeDt;
                ddlAgentCode.DataBind();
                ddlAgentCode.Items.Insert(0, new ListItem("--Please Select--", ""));
                ddlAgentCode.SelectedIndex = ddlAgentCode.Items.IndexOf(ddlAgentCode.Items.FindByText(userInfo.AgentCode));
                ddlAgentCode.Enabled = ddlAgentCode.SelectedIndex > 0 ? false : true;


                ddlAgentBranch.DataValueField = "AgentBranch";
                ddlAgentBranch.DataTextField = "BranchName";
                ddlAgentBranch.DataSource = agentBranchDt.AsEnumerable()
                                            .Where(row => row.Field<string>("Agency") == userInfo.Agency)
                                            .CopyToDataTable();
                ddlAgentBranch.DataBind();
                ddlAgentBranch.Items.Insert(0, new ListItem("--Please Select--", ""));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    string opertaion = string.Empty;

                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster
                    {
                        Agency = ddlAgency.SelectedItem.Text.ToString(),
                        AgentCode = ddlAgentCode.SelectedItem.Text.ToString(),
                        AgentBranch = ddlAgentBranch.SelectedItem.Value.ToString(),
                        UserID = txtUserId.Text.ToString(),
                        UserName = txtUserName.Text.ToString(),
                        CreatedDate = DateTime.Now,
                        Password = txtPassword.Text.ToString(),
                        Mobile = txtMobile.Text.ToString(),
                        IsActive = true,
                        Email = txtEmail.Text.ToString(),
                        StaffNo = Convert.ToInt32(txtStaffNo.Text),
                        CreatedBy = "",
                        Role = ddlRole.SelectedItem.Value
                    };
                    opertaion = (sender as Button).Text;
                    if (opertaion == "Update")
                    {
                        details.Id = Convert.ToInt32(ViewState["UserId"].ToString());
                        details.Type = "edit";
                    }
                    else
                    {
                        details.Type = "insert";
                    }

                    var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.PostUserDetailsResult>,
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster>
                                  (BKIC.SellingPoint.DTO.Constants.UserURI.PostUserMaster, details);

                    if (results.StatusCode == 200 && results.Result.IsTransactionDone)
                    {
                        LoadUserData(userInfo, service);
                        ClearControl();                       
                        btnSubmit.Text = "Save";
                        if (details.Type == "insert")
                        {
                            master.ShowErrorPopup("User saved sucessfully", "User");
                        }
                        if (details.Type == "edit")
                        {
                            master.ShowErrorPopup("User updated sucessfully", "User");
                        }
                    }
                    else
                    {
                        if (results.Result.IsUserAlreadyExists)
                        {
                            master.ShowErrorPopup("UserName already exists !", "User");
                            return;
                        }
                        if (results.Result.PasswordStrength)
                        {
                            master.ShowErrorPopup("Password required atleast 7 character !", "User");
                            return;
                        }
                        else
                        {
                            master.ShowErrorPopup(results.Result.TransactionErrorMessage, "User");
                            return;
                        }

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

        private void ClearControl()
        {    
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPwd.Text = string.Empty;          
            txtMobile.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtStaffNo.Text = string.Empty;

        }
        public void LoadUserData(OAuthTokenResponse userInfo, DataServiceManager service)
        {

            var response = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.MasterTableResult
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.MTUserMaster>>>
                           (BKIC.SellingPoint.DTO.Constants.AdminURI.GetMasterTableByTableName
                           .Replace("{tableName}", BKIC.SellingPoint.DTO.RequestResponseWrappers.MasterTable.UserMaster));

            if (response.StatusCode == 200 && response.Result.IsTransactionDone)
            {
                gvUserMaster.DataSource = response.Result.TableRows
                                          .AsEnumerable()
                                          .Where(x => x.AGENCY == userInfo.Agency)
                                          .ToList();
                gvUserMaster.DataBind();

            }

        }
        protected void agency_changed(object sender, EventArgs e)
        {
            ddlAgentCode.SelectedIndex = ddlAgentCode.Items.IndexOf(ddlAgentCode.Items.FindByValue(ddlAgency.SelectedItem.Text));
        }

        #region Test_1
        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //bindgridview will get the data source and bind it again
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            gvUserMaster.PageIndex = e.NewPageIndex;
            LoadUserData(userInfo, service);
        }
        protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
        {

        }
        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlRole.SelectedItem.Value != "Super Admin")
                {
                    admindetails.Visible = false;
                    userdetails.Visible = true;
                }
                if (ddlRole.SelectedItem.Value == "Super Admin" && userdetails.Visible == true)
                {
                    userdetails.Visible = false;
                    admindetails.Visible = true;
                }
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    if (ViewState["UserId"] != null)
                    {
                        ViewState["UserId"] = string.Empty;
                    }
                    string id = HttpUtility.HtmlDecode(row.Cells[1].Text.Trim());
                    ViewState["UserId"] = id;

                    ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(HttpUtility.HtmlDecode(row.Cells[2].Text.Trim())));
                    ddlAgentCode.SelectedIndex = ddlAgentCode.Items.IndexOf(ddlAgentCode.Items.FindByText(HttpUtility.HtmlDecode(row.Cells[3].Text.Trim())));
                    ddlAgentBranch.SelectedIndex = ddlAgentBranch.Items.IndexOf(ddlAgentBranch.Items.FindByValue(HttpUtility.HtmlDecode(row.Cells[4].Text.Trim())));
                    ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(HttpUtility.HtmlDecode(row.Cells[5].Text.Trim())));
                    txtUserId.Text = HttpUtility.HtmlDecode(row.Cells[6].Text.Trim());
                    txtUserName.Text = HttpUtility.HtmlDecode(row.Cells[7].Text.Trim());                    
                    txtMobile.Text = HttpUtility.HtmlDecode(row.Cells[8].Text.Trim()); 
                    txtEmail.Text = HttpUtility.HtmlDecode(row.Cells[9].Text.Trim()); 
                    txtStaffNo.Text = HttpUtility.HtmlDecode(row.Cells[10].Text.Trim()); 

                    btnSubmit.Text = "Update";
                }
                if (ddlRole.SelectedItem.Value == "BranchAdmin")
                {
                    ddlAgentBranch.Enabled = false;
                }
                else
                {
                    ddlAgentBranch.Enabled = true;
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
        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                int id = Convert.ToInt32(HttpUtility.HtmlDecode(row.Cells[1].Text.Trim()));

                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster
                {
                    Id = id,
                    Type = "delete",
                    CreatedDate = DateTime.Now
                };

                var userResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMasterDetailsResponse>, 
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster>
                                 (BKIC.SellingPoint.DTO.Constants.AdminURI.UserOperation, details);

                if (userResult.StatusCode == 200 && userResult.Result.IsTransactionDone)
                {                   
                    LoadUserData(userInfo, service);
                    master.ShowErrorPopup("User details deleted successfully", "User");
                }
            }
        }
        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
            //dlist.DefaultView.Sort = e.SortExpression + " " + SortDir(e.SortExpression);
            //gvMotorInsurance.DataSource = dlist;
            //gvMotorInsurance.DataBind();
        }

        #endregion


        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlRole.SelectedItem.Value != BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin)
            {
                admindetails.Visible = false;
                userdetails.Visible = true;

                if (ddlRole.SelectedItem.Value == BKIC.SellingPoint.DL.Constants.Roles.User)
                {
                    rfvAgentBranch.Enabled = true;
                    ddlAgentBranch.SelectedIndex = 0;
                    ddlAgentBranch.Enabled = true;
                }
                else if (ddlRole.SelectedItem.Value == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
                {
                    rfvAgentBranch.Enabled = false;
                    ddlAgentBranch.SelectedIndex = 0;
                    ddlAgentBranch.Enabled = false;
                }
            }

            if (ddlRole.SelectedItem.Value == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin)
            {               
                userdetails.Visible = false;
                admindetails.Visible = true;
            }
            
        }

        protected void btnAdminSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string opertaion = string.Empty;

                    master.IsSessionAvailable();                    
                    var service = CommonMethods.GetLogedInService();

                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminRegister
                    {
                        UserName = txtAdUserName.Text.ToString(),
                        Password = txtAdPassword.Text.ToString(),
                        EmailAddress = txtAdEmail.Text.ToString()
                    };

                    opertaion = (sender as Button).Text;
                    //if (opertaion == "Update")
                    //{
                    //    details.Id = Convert.ToInt32(ViewState["UserId"].ToString());
                    //    details.Type = "edit";
                    //}
                    //else
                    //{
                    //    details.Type = "insert";
                    //}

                    var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.PostAdminUserResult>, 
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminRegister>
                                 (BKIC.SellingPoint.DTO.Constants.UserURI.RegisterAdminUser, details);
                    if (results.StatusCode == 200 && results.Result.IsTransactionDone)
                    {
                        //LoadUserData();
                        ClearAdminControl();
                        userdetails.Visible = true;
                        admindetails.Visible = false;
                        //btnSubmit.Text = (branchdetails.Type == "edit") ? "Save" : "Update";
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
            ClearControl();
            Response.Redirect("Homepage.aspx");
        }

        protected void btnAdCancel_Click(object sender, EventArgs e)
        {
            btnAdminSave.Text = "Save";
            ClearAdminControl();
            Response.Redirect("Homepage.aspx");
        }

        private void ClearAdminControl()
        {
            txtAdEmail.Text = string.Empty;
            txtAdPassword.Text = string.Empty;
            txtAdConfirmPwd.Text = string.Empty;
            txtAdUserName.Text = string.Empty;
        }

        protected void gvUserMaster_RowDataBound(object sender, EventArgs e)
        {
            //Change the Index number as per your Grid Column
            foreach (GridViewRow row in gvUserMaster.Rows)
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
            }

        }

        protected void txtSearch_User(object sender, EventArgs e)
        {
            try
            {
                GetUserByUserName();
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

        private void GetUserByUserName()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            ClearControl();
            ClearAdminControl();
            btnSubmit.Text = "Save";

            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyUserRequest
            {
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode,
                UserName = txtSearchUser.Text.Trim()
            };


            var userResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyUserResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyUserRequest>
                                (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyUsers, req);

            if (userResult.StatusCode == 200 && userResult.Result.IsTransactionDone 
                && userResult.Result.AgencyUsers.Count > 0)
            {
                var user = userResult.Result.AgencyUsers[0];

                if (ViewState["UserId"] != null)
                {
                    ViewState["UserId"] = string.Empty;
                }
                ViewState["UserId"] = user.Id;

                ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(user.Agency));
                ddlAgentBranch.SelectedIndex = ddlAgentBranch.Items.IndexOf(ddlAgentBranch.Items.FindByValue(user.AgentBranch));
                ddlAgentCode.SelectedIndex = ddlAgentCode.Items.IndexOf(ddlAgentCode.Items.FindByText(user.AgentCode));
                ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(user.Role));
                txtStaffNo.Text = user.StaffNo.ToString();
                txtUserId.Text = user.UserId;
                txtMobile.Text = user.Mobile;

                if (user.Role == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin)
                {
                    txtAdEmail.Text = user.Email;
                    txtAdPassword.Text = string.Empty;
                    txtAdConfirmPwd.Text = string.Empty;
                    txtAdUserName.Text = user.UserName;
                }
                else
                {
                    txtEmail.Text = user.Email;
                    txtPassword.Text = string.Empty;
                    txtConfirmPwd.Text = string.Empty;
                    txtUserName.Text = user.UserName;
                }
                if (ddlRole.SelectedItem.Value == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin
                                                || ddlRole.SelectedItem.Value == BKIC.SellingPoint.DL.Constants.Roles.User)
                {
                    admindetails.Visible = false;
                    userdetails.Visible = true;
                }
                if (ddlRole.SelectedItem.Value == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin)
                {
                    userdetails.Visible = false;
                    admindetails.Visible = true;
                }               
                btnSubmit.Text = "Update";
            }
            else
            {
                master.ShowErrorPopup("User not found", "User");
                ClearControl();
                btnSubmit.Text = "Save";
            }
        }

    }
}