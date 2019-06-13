using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class DomesticHelp : System.Web.UI.Page
    {
        private General master;
        public static DataTable Nationalitydt;
        public static DataTable domesticHelpOccupationDt;
        public static List<AgencyDomesticPolicy> policyList;
        public static long _DomesticId;
        public static bool AjdustedPremium { get; set; }
        public static List<InsuredMasterDetails> InsuredNames { get; set; }
        // public static DataServiceManager ClientServiceManager { get; set; }

        //As of now domesticHelp insurance have only one product, in future it may come.
        public static string MainClass { get; set; }

        public static string SubClass { get; set; }

        public DomesticHelp()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Page.Validate();

            master = Master as General;
            if (!Page.IsPostBack)
            {               
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                BindDropdown(userInfo, service);
                SetInitialRow();
                DisableDefaultControls(userInfo, service);                
                LoadUsers(userInfo, service);
                LoadAgencyClientCode(userInfo, service);
                QueryStringMethods(userInfo, service);
            }
        }

        public void DisableDefaultControls(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            _DomesticId = 0;
            AjdustedPremium = false;
            phyDefect.Visible = false;
            amtDisplay.Visible = false;            
            downloadschedule.Visible = false;
            btnDomesticSave.Visible = false;
            btnAuthorize.Visible = false;
            admindetails.Visible = true;
            divPaymentSection.Visible = userInfo.IsShowPayments;
        }

        private void BindDropdown(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                 (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}",
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.DomesticHelp));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                Nationalitydt = dropdownds.Tables["Nationality"];
                domesticHelpOccupationDt = dropdownds.Tables["DomesticWorkerOccupation"];
                DataTable branches = dropdownds.Tables["BranchMaster"];

                if (branches != null && branches.Rows.Count > 0)
                {                   
                    ddlBranch.DataValueField = "AGENTBRANCH";
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataSource = branches.AsEnumerable()
                                            .Where(row => row.Field<string>("Agency") == userInfo.Agency)
                                            .CopyToDataTable();
                    ddlBranch.DataBind();
                    ddlBranch.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
            }

            var productResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchProductCodeResponse>>(
                                BKIC.SellingPoint.DTO.Constants.DropDownURI.GetInsuranceProductCode.
                                Replace("{agency}", userInfo.Agency)
                                .Replace("{agencyCode}", userInfo.AgentCode)
                                .Replace("{insurancetypeid}", "1"));

            if (productResult != null && productResult.StatusCode == 200 && productResult.Result.IsTransactionDone)
            {
                var products = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>(
                               BKIC.SellingPoint.DTO.Constants.DropDownURI.GetAgencyProducts
                              .Replace("{agency}", userInfo.Agency).Replace("{agencyCode}", userInfo.AgentCode)
                              .Replace("{mainclass}", productResult.Result.productCode)
                              .Replace("{page}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.DomesticHelp));

                MainClass = productResult.Result.productCode;
                if (products != null && products.StatusCode == 200 && products.Result.IsTransactionDone)
                {
                    DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(products.Result.dropdownresult);
                    DataTable prods = dropdownds.Tables["Products"];
                    //In future product may be increase. Now it has only one product.
                    if (prods != null && prods.Rows.Count > 0)
                    {
                        SubClass = prods.Rows[0]["SubClass"].ToString();
                    }
                }
            }
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(userInfo.AgentBranch));
            ddlUsers.SelectedIndex = ddlUsers.Items.IndexOf(ddlUsers.Items.FindByText(userInfo.UserName));
            txtIssueDate.Text = DateTime.Now.CovertToLocalFormat();
        }

        public void LoadUsers(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster
            {
                Type = "fetch",
                CreatedDate = DateTime.Now
            };

            var userResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMasterDetailsResponse>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster>
                            (BKIC.SellingPoint.DTO.Constants.AdminURI.UserOperation, details);

            if (userResult.Result.IsTransactionDone && userResult.StatusCode == 200)
            {
                ddlUsers.DataValueField = "ID";
                ddlUsers.DataTextField = "UserName";
                ddlUsers.DataSource = userResult.Result.UserMaster.Where(x => x.Agency == userInfo.Agency);
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, new ListItem("--Please Select--", ""));
            }
        }

        private void LoadAgencyClientCode(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest
            {
                AgentBranch = userInfo.AgentBranch,
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency
            };

            var Results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredResponse>,
                         BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest>
                         (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyInsured, req);

            if (Results.StatusCode == 200 && Results.Result.IsTransactionDone && Results.Result.AgencyInsured.Count > 0)
            {
                //ddlCPR.DataSource = Results.Result.AgencyInsured;
                //ddlCPR.DataTextField = "CPR";
                //ddlCPR.DataValueField = "InsuredCode";
                //ddlCPR.DataBind();
                //ddlCPR.Items.Insert(0, new ListItem("--Please Select--", ""));
                InsuredNames = Results.Result.AgencyInsured;
            }
            ddlUsers.SelectedIndex = ddlUsers.Items.IndexOf(ddlUsers.Items.FindByText(userInfo.UserName));
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(userInfo.AgentBranch));

            if (userInfo.Roles == "BranchAdmin" || userInfo.Roles == "User")
            {
                ddlUsers.Enabled = false;
            }
        }

        private void LoadAgencyClientPolicyInsuredCode(OAuthTokenResponse userInfo, DataServiceManager service, bool includeHIR = false)
        {
            var domesticReq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyDomesticRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                AgentBranch = userInfo.AgentBranch,
                IncludeHIR = includeHIR
            };

            //Get PolicyNo by Agency
            var domesticPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyDomesticPolicyResponse>,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyDomesticRequest>
                                  (BKIC.SellingPoint.DTO.Constants.DomesticURI.GetDomesticAgencyPolicy, domesticReq);

            if (domesticPolicies.StatusCode == 200 && domesticPolicies.Result.IsTransactionDone
                && domesticPolicies.Result.DomesticAgencyPolicies.Count > 0)
            {
                policyList = domesticPolicies.Result.DomesticAgencyPolicies;                
            }
        }

        public void QueryStringMethods(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var includeHIR = Request.QueryString["IncludeHIR"];
            var cpr = Request.QueryString["CPR"];
            var dob = Request.QueryString["DOB"];
            var insuredName = Request.QueryString["InsuredName"];
            var insuredCode = Request.QueryString["InsuredCode"];
            var policyNo = Request.QueryString["PolicyNo"];

            txtInsuredName.Text = insuredName != null ? Convert.ToString(insuredName) : string.Empty;
            txtClientCode.Text = insuredCode != null ? Convert.ToString(insuredCode) : string.Empty;

            LoadAgencyClientPolicyInsuredCode(userInfo, service, includeHIR != null ? Convert.ToBoolean(includeHIR) : false);

            if (cpr != null)
            {
                var CPR = Convert.ToString(cpr);              
                txtCPRSearch.Text = CPR;
                txtCPR.Text = CPR;
            }
            if (policyNo != null)
            {               
                txtDomesticPolicySearch.Text = policyNo;
                GetPolicyInfo();
            }
        }

        protected void insured_Master(object sender, EventArgs e)
        {
            Response.Redirect("InsuredMaster.aspx?type=" + 1);
        }

        //protected void txtCPR_Changed(object sender, EventArgs e)
        //{
        //    SetCPR();
        //}

        protected void ddlCPR_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetCPR();
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

        protected void txtCPR_Changed(object sender, EventArgs e)
        {
            try
            {
                SetCPR();
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

        private void SetCPR()
        {           
            txtCPR.Text =  txtCPRSearch.Text.Trim();
            if (InsuredNames != null && InsuredNames.Count > 0)
            {
                var insured = InsuredNames.Find(c => c.CPR == txtCPRSearch.Text.Trim());
                if (insured != null)
                {
                    txtInsuredName.Text = insured.FirstName + " " + insured.MiddleName + " " + insured.LastName;
                    txtClientCode.Text = insured.InsuredCode;
                }
                else
                {
                    master.ShowErrorPopup("Please enter valid CPR", "Insured");
                    txtInsuredName.Text = string.Empty;
                    txtClientCode.Text = string.Empty;
                    txtCPR.Text = string.Empty;
                    txtCPRSearch.Text = string.Empty;
                }
            }
            Page_CustomValidate();
            DisableControls();
        }

        #region Grid events

        protected void Gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Nationalitydt != null && Nationalitydt.Rows.Count > 0)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlNational");
                    ddl.DataValueField = "Code";
                    ddl.DataTextField = "Description";
                    ddl.DataSource = Nationalitydt;
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("--Please Select--", ""));
                }

                if (domesticHelpOccupationDt != null && domesticHelpOccupationDt.Rows.Count > 0)
                {
                    DropDownList ddlRelationship = (DropDownList)e.Row.FindControl("ddlDomesticOccupation");
                    ddlRelationship.DataValueField = "ID";
                    ddlRelationship.DataTextField = "Occupation";
                    ddlRelationship.DataSource = domesticHelpOccupationDt;
                    ddlRelationship.DataBind();
                    ddlRelationship.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
            }
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();

            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Sex", typeof(string)));
            dt.Columns.Add(new DataColumn("Date Of Birth", typeof(string)));

            dt.Columns.Add(new DataColumn("Nationality", typeof(string)));
            dt.Columns.Add(new DataColumn("Passport No", typeof(string)));
            dt.Columns.Add(new DataColumn("Occupation", typeof(string)));

            dt.Columns.Add(new DataColumn("Flat No", typeof(string)));
            dt.Columns.Add(new DataColumn("Building No", typeof(string)));
            dt.Columns.Add(new DataColumn("Block No", typeof(string)));

            dt.Columns.Add(new DataColumn("Road No", typeof(string)));
            dt.Columns.Add(new DataColumn("Town", typeof(string)));

            dr = dt.NewRow();

            //dr["RowNumber"] = 1;
            dr["Name"] = string.Empty;
            dr["Sex"] = string.Empty;
            dr["Date Of Birth"] = string.Empty;

            dr["Nationality"] = string.Empty;
            dr["Passport No"] = string.Empty;
            dr["Occupation"] = string.Empty;

            dr["Flat No"] = string.Empty;
            dr["Building No"] = string.Empty;
            dr["Block No"] = string.Empty;

            dr["Road No"] = string.Empty;
            dr["Town"] = string.Empty;

            dt.Rows.Add(dr);
            //dr = dt.NewRow();

            //Store the DataTable in ViewState
            ViewState["CurrentTable"] = dt;

            Gridview1.DataSource = dt;
            Gridview1.DataBind();
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                try
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    DataRow drCurrentRow = null;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        //for (int i = dtCurrentTable.Rows.Count; i > 0; i--)
                        //{
                        //    dtCurrentTable.Rows[i - 1].Delete();
                        //    dtCurrentTable.AcceptChanges();
                        //}
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            ////extract the TextBox values

                            TextBox txName = (TextBox)Gridview1.Rows[rowIndex].Cells[0].FindControl("txtDomesticName");
                            DropDownList ddlSex = (DropDownList)Gridview1.Rows[rowIndex].Cells[1].FindControl("ddlGender");
                            TextBox txDOB = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtDOB");
                            DropDownList ddlNationality = (DropDownList)Gridview1.Rows[rowIndex].Cells[3].FindControl("ddlNational");
                            TextBox txPassPort = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtPassport");
                            DropDownList ddlOccupation = (DropDownList)Gridview1.Rows[rowIndex].Cells[5].FindControl("ddlDomesticOccupation");

                            //TextBox txFlatNo = (TextBox)Gridview1.Rows[rowIndex].Cells[6].FindControl("txtFlatNo");
                            //TextBox txBuildingNo = (TextBox)Gridview1.Rows[rowIndex].Cells[7].FindControl("txtBuildingNo");
                            //TextBox txBlockNo = (TextBox)Gridview1.Rows[rowIndex].Cells[8].FindControl("txtBlockNo");
                            //TextBox txRoadNo = (TextBox)Gridview1.Rows[rowIndex].Cells[9].FindControl("txtRoadNo");
                            //TextBox txTown = (TextBox)Gridview1.Rows[rowIndex].Cells[10].FindControl("txtTown");

                            drCurrentRow = dtCurrentTable.NewRow();
                            dtCurrentTable.Rows[i - 1]["Name"] = txName.Text;
                            dtCurrentTable.Rows[i - 1]["Sex"] = ddlSex.SelectedItem.Text;
                            dtCurrentTable.Rows[i - 1]["Date Of Birth"] = txDOB.Text;

                            dtCurrentTable.Rows[i - 1]["Nationality"] = ddlNationality.SelectedItem.Text;
                            dtCurrentTable.Rows[i - 1]["Passport No"] = txPassPort.Text;
                            dtCurrentTable.Rows[i - 1]["Occupation"] = ddlOccupation.SelectedItem.Text;

                            //dtCurrentTable.Rows[i - 1]["Flat No"] = txFlatNo.Text;
                            //dtCurrentTable.Rows[i - 1]["Building No"] = txBuildingNo.Text;
                            //dtCurrentTable.Rows[i - 1]["Block No"] = txBlockNo.Text;

                            //dtCurrentTable.Rows[i - 1]["Road No"] = txRoadNo.Text;
                            //dtCurrentTable.Rows[i - 1]["Town"] = txTown.Text;

                            rowIndex++;
                        }

                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["CurrentTable"] = dtCurrentTable;
                        Gridview1.DataSource = dtCurrentTable;
                        Gridview1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    ////throw ex;
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //Set Previous Data on Postbacks
            SetPreviousData();
        }

        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox txName = (TextBox)Gridview1.Rows[rowIndex].Cells[0].FindControl("txtDomesticName");
                        DropDownList ddlSex = (DropDownList)Gridview1.Rows[rowIndex].Cells[1].FindControl("ddlGender");
                        TextBox txDOB = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtDOB");
                        DropDownList ddlNationality = (DropDownList)Gridview1.Rows[rowIndex].Cells[3].FindControl("ddlNational");
                        TextBox txPassPort = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtPassport");
                        DropDownList ddlOccupation = (DropDownList)Gridview1.Rows[rowIndex].Cells[5].FindControl("ddlDomesticOccupation");

                        txName.Text = dt.Rows[i]["Name"].ToString();
                        ddlSex.SelectedIndex = dt.Rows[i]["Sex"].ToString().ToUpper() == "M" ? 1 : 2;
                        txDOB.Text = dt.Rows[i]["Date Of Birth"].ToString();
                        ddlNationality.SelectedIndex = ddlNationality.Items.IndexOf(ddlNationality.Items.FindByValue(dt.Rows[i]["Nationality"].ToString()));
                        txPassPort.Text = dt.Rows[i]["Passport No"].ToString();
                        ddlOccupation.SelectedIndex = ddlOccupation.Items.IndexOf(ddlOccupation.Items.FindByText(dt.Rows[i]["Occupation"].ToString()));

                        rowIndex++;
                    }
                }
            }
        }

        #endregion Grid events

        #region DropDown

        protected void ddlPhydefect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPhydefect.SelectedValue == "Yes")
            {
                phyDefect.Visible = true;
            }
            else
            {
                phyDefect.Visible = false;
            }
            Page_CustomValidate();          
        }

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentMethod.SelectedIndex == 1)
            {
                txtAccountNo.Text = "";
                txtAccountNo.Enabled = false;
                rfvtxtAccountNo.Enabled = false;
            }
            else
            {
                rfvtxtAccountNo.Enabled = true;
                txtAccountNo.Enabled = true;
            }
            Page_CustomValidate();
        }

        protected void ddlNoOfDomesticWorker_SelectedIndexChanged(object sender, EventArgs e)
        {
            admindetails.Visible = true;            
            //AddNewRowToGrid();
        }

        protected void ddlDomesticPolicyNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlDomesticPolicies.SelectedIndex == 0)
            //{
            //    master.ClearControls(GetContentControl());
            //    SetReadOnlyControls();
            //}
           
        }
        #endregion DropDown
        #region Button
        protected void btnPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {                   
                    if (!string.IsNullOrEmpty(txtDomesticPolicySearch.Text.Trim()))
                    {
                        GetPolicyInfo();
                    }
                    else
                    {
                        master.ClearControls(GetContentControl());
                        SetReadOnlyControls();
                        HidePremium();
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

        public void GetPolicyInfo()
        {           
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyDomesticRequest();
            request.AgentBranch = userInfo.AgentBranch;
            request.AgentCode = userInfo.AgentCode;
            request.Agency = userInfo.Agency;

            //Get saved policy details by document(policy) number.
            var url = BKIC.SellingPoint.DTO.Constants.DomesticURI.GetSavedQuoteDocumentNo
                     //.Replace("{documentNo}", ddlDomesticPolicies.SelectedItem.Text.Trim())
                     .Replace("{documentNo}", txtDomesticPolicySearch.Text.Trim())
                     .Replace("{agentCode}", request.AgentCode);

            var domesticDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticHelpSavedQuotationResponse>>(url);

            //Update policy details on current page for dispaly the details.
            if (domesticDetails.StatusCode == 200 && domesticDetails.Result.IsTransactionDone)
            {
                Update(userInfo, domesticDetails);
            }
            else
            {
                master.ShowErrorPopup(domesticDetails.Result.TransactionErrorMessage, "Policy not found !");
            }
        }

        private void Update(OAuthTokenResponse userInfo, ApiResponseWrapper<DomesticHelpSavedQuotationResponse> domesticDetails)
        {
            var response = domesticDetails.Result.DomesticHelp;

            CPR.Value = response.CPR;
            DomesticID.Value = response.DomesticID.ToString();
            _DomesticId = response.DomesticID;
            txtClientCode.Text = response.InsuredCode;
            txtInsuredName.Text = response.FullName;
            txtPolicyStartDate.Text = response.PolicyStartDate.CovertToLocalFormat();
            txtPolicyEndDate.Text = response.PolicyExpiryDate.CovertToLocalFormat();
            txtAccountNo.Text = response.AccountNumber;
            txtIssueDate.Text = response.PolicyIssueDate.CovertToLocalFormat();
            ddlPaymentMethod.SelectedIndex = ddlPaymentMethod.Items.IndexOf(ddlPaymentMethod.Items.FindByText(response.PaymentType));
            ddlPhydefect.SelectedIndex = ddlPhydefect.Items.IndexOf(ddlPhydefect.Items.FindByText(response.IsPhysicalDefect));           
            txtCPRSearch.Text = response.CPR;
            txtCPR.Text = response.CPR;
            txtRemarks.Text = response.Remarks;
            ddlNoOfYears.SelectedIndex = ddlNoOfYears.Items.IndexOf(ddlNoOfYears.Items.FindByValue(CalculateYears().ToString()));
            if (!string.IsNullOrEmpty(response.IsPhysicalDefect) && response.IsPhysicalDefect.ToLower() == "yes")
            {
                txtPhysicalDesc.Text = domesticDetails.Result.DomesticHelp.PhysicalDefectDescription;
                ddlPhydefect.SelectedIndex = 1;
                phyDefect.Visible = true;
            }
            else
            {
                ddlPhydefect.SelectedIndex = 2;
                phyDefect.Visible = false;
            }
            if (response.PremiumBeforeDiscount - response.PremiumAfterDiscount > 0)
            {
                calculatedPremium.Value = Convert.ToString(response.PremiumBeforeDiscount);
                calculatedCommision.Value = Convert.ToString(response.CommisionBeforeDiscount);
                AjdustedPremium = true;
            }
            else
            {
                calculatedPremium.Value = Convert.ToString(response.PremiumAfterDiscount);
                calculatedCommision.Value = Convert.ToString(response.CommissionAmount);
            }
            ShowPremium(userInfo, response.PremiumAfterDiscount, response.CommissionAmount);
            ShowVAT(userInfo, response.TaxOnPremium, response.TaxOnCommission,
                    (response.PremiumAfterDiscount + response.TaxOnPremium),
                    (response.CommissionAfterDiscount + response.TaxOnCommission));

            ShowDiscount(userInfo, response); 

            //Update domestic members details on the page.
            if (domesticDetails.Result.DomesticHelpMemberList != null && domesticDetails.Result.DomesticHelpMemberList.Count > 0)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                Gridview1.DataSource = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = dtCurrentTable.Rows.Count; i > 0; i--)
                    {
                        dtCurrentTable.Rows[i - 1].Delete();
                        dtCurrentTable.AcceptChanges();
                    }
                }
                for (int i = 0; i < domesticDetails.Result.DomesticHelpMemberList.Count; i++)
                {
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    dtCurrentTable.Rows[i]["Name"] = domesticDetails.Result.DomesticHelpMemberList[i].Name;
                    dtCurrentTable.Rows[i]["Sex"] = domesticDetails.Result.DomesticHelpMemberList[i].Sex;
                    dtCurrentTable.Rows[i]["Date Of Birth"] = domesticDetails.Result.DomesticHelpMemberList[i].DOB.CovertToLocalFormat();

                    dtCurrentTable.Rows[i]["Nationality"] = domesticDetails.Result.DomesticHelpMemberList[i].Nationality;
                    dtCurrentTable.Rows[i]["Passport No"] = domesticDetails.Result.DomesticHelpMemberList[i].Passport;
                    dtCurrentTable.Rows[i]["Occupation"] = domesticDetails.Result.DomesticHelpMemberList[i].Occupation;
                }
                ViewState["CurrentTable"] = dtCurrentTable;
                Gridview1.DataSource = dtCurrentTable;
                Gridview1.DataBind();
                admindetails.Visible = true;
                SetPreviousData();
            }
            EnableAuthorize(domesticDetails.Result.DomesticHelp.IsHIR, domesticDetails.Result.DomesticHelp.HIRStatus);
            if (response.IsActivePolicy)
            {
                //SetScheduleHRef(ddlDomesticPolicies.SelectedItem.Text.Trim(), Constants.DomesticHelp, userInfo);
                SetScheduleHRef(txtDomesticPolicySearch.Text.Trim(), Constants.DomesticHelp, userInfo);
                master.makeReadOnly(GetContentControl(), false);
            }
            else
            {
                RemoveScheduleHRef();
                master.makeReadOnly(GetContentControl(), true);
            }
            SetReadOnlyControls();
            formDomesticSubmitted.Value = "false";
        }

        public void EnableAuthorize(bool isHIR, int HIRStatus)
        {
            if (isHIR && HIRStatus != 8)
                btnAuthorize.Visible = false;
            else
                btnAuthorize.Visible = true;
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }

        protected void ddlNoYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPolicyStartDate.Text))
            {
                int Periods = Convert.ToInt32(ddlNoOfYears.SelectedItem.Value);
                txtPolicyEndDate.Text = Convert.ToDateTime(txtPolicyStartDate.Text.CovertToCustomDateTime())
                    .AddYears(Periods).AddDays(-1).CovertToLocalFormat();
            }
            Page_CustomValidate();
            DisableControls();
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                DisablePaymentValidator();
                Page.Validate();
                Page_CustomValidate();
                if (Page.IsValid)
                {                   
                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    var DomesticQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticHelpQuote();                  

                    var insuranceDuration = CalculateYears();
                    if (insuranceDuration <= 0)
                    {
                        insuranceDuration = 1;
                    }
                    DomesticQuote.InsurancePeroid = insuranceDuration;
                    DomesticQuote.NumberOfDomesticWorkers = Gridview1.Rows.Count;

                    var DomesticQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticHelpQuoteResponse>,
                                              BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticHelpQuote>
                                             (BKIC.SellingPoint.DTO.Constants.DomesticURI.GetQuote, DomesticQuote);

                    if (DomesticQuoteResult.StatusCode == 200 && DomesticQuoteResult.Result.IsTransactionDone)
                    {
                        //premiumAmount.Text = Convert.ToString(DomesticQuoteResult.Result.PremiumBeforeDiscount);
                        calculatedPremium.Value = Convert.ToString(DomesticQuoteResult.Result.PremiumBeforeDiscount);

                        var commisionRequest = new CommissionRequest
                        {
                            AgentCode = userInfo.AgentCode,
                            Agency = userInfo.Agency,
                            SubClass = SubClass,
                            PremiumAmount = DomesticQuoteResult.Result.PremiumBeforeDiscount,
                            IsDeductable = true
                        };

                        var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                               BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>
                                               (BKIC.SellingPoint.DTO.Constants.CommissionURI.CalculateCommission, commisionRequest);

                        if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone
                            && commissionresult.Result.CommissionAmount >= 0)
                        {
                            //commission.Text = Convert.ToString(commissionresult.Result.CommissionAmount);
                            calculatedCommision.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                            ShowPremium(userInfo, DomesticQuoteResult.Result.PremiumBeforeDiscount, commissionresult.Result.CommissionAmount);
                        }
                        else
                        {
                            master.ShowLoading = false;
                            master.ShowErrorPopup(commissionresult.Result.TransactionErrorMessage, "Request Failed !");
                            return;
                        }
                        //Calculate VAT.
                        var vatResponse = master.GetVat(DomesticQuoteResult.Result.PremiumBeforeDiscount, commissionresult.Result.CommissionAmount);
                        if (vatResponse != null && vatResponse.IsTransactionDone)
                        {
                            decimal TotalPremium = DomesticQuoteResult.Result.PremiumBeforeDiscount + vatResponse.VatAmount;
                            decimal TotalCommission = commissionresult.Result.CommissionAmount + vatResponse.VatCommissionAmount;
                            ShowVAT(userInfo, vatResponse.VatAmount, vatResponse.VatCommissionAmount, TotalPremium, TotalCommission);
                        }
                        btnDomesticSave.Visible = true;
                    }
                    else
                    {
                        master.ShowLoading = false;
                        master.ShowErrorPopup(DomesticQuoteResult.Result.TransactionErrorMessage, "Request Failed !");
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void validate_Premium(object sender, EventArgs e)
        {
            try
            {
                var Premium = Convert.ToDecimal(calculatedPremium.Value);
                var Commision = Convert.ToDecimal(calculatedCommision.Value);
                var Discount = string.IsNullOrEmpty(txtDiscount.Text) ? decimal.Zero : Convert.ToDecimal(txtDiscount.Text);
                var reduceablePremium = Premium - Commision;
                var premiumDiff = Premium - Discount;

                if (premiumDiff < reduceablePremium)
                {
                    premiumAmount.Text = Convert.ToString(reduceablePremium);
                    txtDiscount.Text = Convert.ToString(calculatedCommision.Value);
                    commission.Text = Convert.ToString(0);
                }
                else if (Discount > Premium)
                {
                    premiumAmount.Text = Convert.ToString(reduceablePremium);
                    txtDiscount.Text = Convert.ToString(calculatedCommision.Value);
                    commission.Text = Convert.ToString(0);
                }
                else
                {
                    premiumAmount.Text = Convert.ToString(premiumDiff);
                    commission.Text = Convert.ToString(Commision - Discount);
                    btnDomesticSave.Enabled = true;
                    btnAuthorize.Enabled = true;
                }
                var vatResponse = master.GetVat(string.IsNullOrEmpty(premiumAmount.Text) ? 0 : Convert.ToDecimal(premiumAmount.Text),
                                string.IsNullOrEmpty(commission.Text) ? 0 : Convert.ToDecimal(commission.Text));

                if (vatResponse != null && vatResponse.IsTransactionDone)
                {
                    txtVATAmount.Text = Convert.ToString(vatResponse.VatAmount);
                    txtVATCommission.Text = Convert.ToString(vatResponse.VatCommissionAmount);
                    txtTotalPremium.Text = Convert.ToString(string.IsNullOrEmpty(premiumAmount.Text) ? 0 : Convert.ToDecimal(premiumAmount.Text) + vatResponse.VatAmount);
                    txtTotalCommission.Text = Convert.ToString(string.IsNullOrEmpty(commission.Text) ? 0 : Convert.ToDecimal(commission.Text) + vatResponse.VatCommissionAmount);
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

        protected void btnDomesticSave_Click(object sender, EventArgs e)
        {
            try
            {
                EnablePaymentValidator();
                Page.Validate();
                Page_CustomValidate();
                if (Page.IsValid)
                {
                    SaveAuthorize(true);
                    btnAuthorize.Enabled = true;
                }
            }
            catch (System.Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void btnAuthorize_Click(object sender, EventArgs e)
        {
            try
            {
                EnablePaymentValidator();
                Page.Validate();
                Page_CustomValidate();
                if (Page.IsValid)
                {
                    Reset();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowPopup();", true);
                }
            }
            catch (System.Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void Auth(object sender, EventArgs e)
        {
            try
            {
                EnablePaymentValidator();
                Page.Validate();
                Page_CustomValidate();
                if (Page.IsValid)
                {
                    SaveAuthorize(false);                    
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Homepage.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
        }

        protected void imgbtnNewClientCd_Click(object sender, EventArgs e)
        {
            Response.Redirect("InsuredMaster.aspx");
        }

        private DomesticHelpMember GetIndividual()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var tmember = new DomesticHelpMember();         

            var insured = new BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest
            {
                CPR = txtCPR.Text.Trim(),
                InsuredCode = txtClientCode.Text.Trim(),
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode
            };

            var travelResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest>
                              (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchUserDetailsByCPRInsuredCode, insured);

            return tmember;
        }

        #endregion Button

        #region Method

        public DateTime getexpiryDate(int years)
        {
            DateTime ExpiryDate;
            DateTime date = DateTime.Now.AddYears(years);
            ExpiryDate = date.AddDays(-1);
            return ExpiryDate;
        }

        private List<DomesticHelpMember> GetDomesticDetails()
        {
            #region test_1

            var objs = new List<DomesticHelpMember>();
            DateTime expirydate = Convert.ToDateTime(txtPolicyEndDate.Text.ConvertToDateTimeNull()); 
            //getexpiryDate(Convert.ToInt32(ddlInsurancePeriod.SelectedItem.Value));

            for (int row = 1; row <= Gridview1.Rows.Count; row++)
            {
                var obj = new DomesticHelpMember();
                obj.ItemserialNo = row;
                obj.SumInsured = 0;// 18900;
                obj.ExpiryDate = expirydate;

                for (int col = 0; col < Gridview1.Columns.Count; col++)
                {
                    if (Gridview1.Columns[col].Visible)
                    {
                        if (String.IsNullOrEmpty(Gridview1.Rows[row - 1].Cells[col].Text))
                        {
                            if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("Label"))
                            {
                                Label LB = (Label)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            }
                            else if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("LinkButton"))
                            {
                                LinkButton LB = (LinkButton)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            }
                            else if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("TextBox"))
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];

                                var colName = Gridview1.Columns[col].ToString();

                                if (colName == "Name")
                                {
                                    obj.Name = txtValue.Text.ToString();
                                    if (String.IsNullOrEmpty(obj.Name))
                                    {
                                        objs = new List<DomesticHelpMember>();
                                        break;
                                    }
                                }

                                if (colName == "Date Of Birth")
                                {
                                    obj.DOB = txtValue.Text.CovertToCustomDateTime();
                                }
                                if (colName == "CPR / Passport No")
                                {
                                    obj.Passport = txtValue.Text.ToString();
                                    if (String.IsNullOrEmpty(obj.Passport))
                                    {
                                        objs = new List<DomesticHelpMember>();
                                        break;
                                    }
                                }

                                if (colName == "Occupation")
                                {
                                    obj.Occupation = txtValue.Text.ToString();
                                    if (String.IsNullOrEmpty(obj.Occupation))
                                    {
                                        objs = new List<DomesticHelpMember>();
                                        break;
                                    }
                                }
                                if (colName == "Flat No")
                                {
                                    obj.AddressType = txtValue.Text.ToString();
                                    if (String.IsNullOrEmpty(obj.AddressType))
                                    {
                                        objs = new List<DomesticHelpMember>();
                                        break;
                                    }
                                }
                                if (colName == "Building No")
                                {
                                    obj.AddressType = obj.AddressType + "," + txtValue.Text.ToString();
                                }

                                if (colName == "Block No")
                                {
                                    obj.AddressType = obj.AddressType + "," + txtValue.Text.ToString();
                                }

                                if (colName == "Road No")
                                {
                                    obj.AddressType = obj.AddressType + "," + txtValue.Text.ToString();
                                }
                                if (colName == "Town")
                                {
                                    obj.AddressType = obj.AddressType + "," + txtValue.Text.ToString();
                                }
                            }
                            else if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("DropDownList"))
                            {
                                DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];

                                var colName = Gridview1.Columns[col].ToString();

                                if (colName == "Sex")
                                {
                                    //obj.Sex = txtValue.SelectedValue == "Male" ? "M" : "F";
                                    obj.Sex = Convert.ToChar(txtValue.SelectedValue == "Male" ? "M" : "F");
                                }
                                if (colName == "Nationality")
                                {
                                    obj.Nationality = txtValue.SelectedValue.ToString();
                                }

                                if (colName == "Occupation")
                                {
                                    obj.Occupation = txtValue.SelectedItem.Text.Trim();
                                }
                            }
                            else if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("LinkButton"))
                            {
                                LinkButton LB = (LinkButton)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            }
                        }
                        else
                        {
                        }
                    }
                }
                objs.Add(obj);
            }
            //}

            return objs;

            #endregion test_1
        }

        #endregion Method

        public void SaveAuthorize(bool isSave)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var domesticHelp = new DomesticHelpPolicy();
            domesticHelp.AgentBranch = userInfo.AgentBranch;
            domesticHelp.AgentCode = userInfo.AgentCode;
            domesticHelp.Agency = userInfo.Agency;

            //Get Quote for the given values.
            var DomesticQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticHelpQuote();
            var insuranceDuration = CalculateYears();
            if (insuranceDuration <= 0)
            {
                insuranceDuration = 1;
            }
            DomesticQuote.InsurancePeroid = insuranceDuration;
            DomesticQuote.NumberOfDomesticWorkers = Gridview1.Rows.Count;

            var DomesticQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                      <BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticHelpQuoteResponse>,
                                      BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticHelpQuote>
                                     (BKIC.SellingPoint.DTO.Constants.DomesticURI.GetQuote, DomesticQuote);

            //Update the quote values to current instance.
            if (DomesticQuoteResult.StatusCode == 200 && DomesticQuoteResult.Result.IsTransactionDone)
            {
                domesticHelp.PremiumBeforeDiscount = DomesticQuoteResult.Result.PremiumBeforeDiscount;
                domesticHelp.PremiumAfterDiscount = DomesticQuoteResult.Result.PremiumAfterDiscount;

                var commisionRequest = new CommissionRequest
                {
                    AgentCode = userInfo.AgentCode,
                    Agency = userInfo.Agency,
                    SubClass = SubClass,
                    PremiumAmount = DomesticQuoteResult.Result.PremiumBeforeDiscount,
                    IsDeductable = true
                };

                var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                      <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                      BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>
                                      (BKIC.SellingPoint.DTO.Constants.CommissionURI.CalculateCommission, commisionRequest);

                if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone
                    && commissionresult.Result.CommissionAmount >= 0)
                {
                    domesticHelp.CommissionAmount = commissionresult.Result.CommissionAmount;
                    if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                        || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
                    {
                        if (Convert.ToDecimal(premiumAmount.Text) < domesticHelp.PremiumBeforeDiscount || AjdustedPremium)
                        {
                            domesticHelp.UserChangedPremium = true;
                            domesticHelp.PremiumAfterDiscount = Convert.ToDecimal(premiumAmount.Text);
                            var diff = domesticHelp.PremiumBeforeDiscount - domesticHelp.PremiumAfterDiscount;
                            domesticHelp.CommissionAfterDiscount = domesticHelp.CommissionAmount - diff;
                        }
                    }
                    else if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.User)
                    {
                        if (Convert.ToDecimal(premiumAmount1.Text) < domesticHelp.PremiumBeforeDiscount || AjdustedPremium)
                        {
                            domesticHelp.UserChangedPremium = true;
                            domesticHelp.PremiumAfterDiscount = Convert.ToDecimal(premiumAmount1.Text);
                            var diff = domesticHelp.PremiumBeforeDiscount - domesticHelp.PremiumAfterDiscount;
                            domesticHelp.CommissionAfterDiscount = domesticHelp.CommissionAmount - diff;
                        }
                    }
                }
            }
            domesticHelp.InsurancePeroid = insuranceDuration;
            domesticHelp.NoOfDomesticWorkers = Gridview1.Rows.Count;
            domesticHelp.PolicyStartDate = txtPolicyStartDate.Text.CovertToCustomDateTime();
            domesticHelp.PolicyExpiryDate = txtPolicyEndDate.Text.CovertToCustomDateTime();
            domesticHelp.IsPhysicalDefect = ddlPhydefect.SelectedItem.Value;
            domesticHelp.Remarks = txtRemarks.Text.Trim();
            domesticHelp.AccountNumber = txtAccountNo.Text.Trim();
            domesticHelp.PaymentType = ddlPaymentMethod.SelectedItem.Text.Trim();
            domesticHelp.PolicyIssueDate = txtIssueDate.Text.CovertToCustomDateTime();
            domesticHelp.MainClass = MainClass;
            domesticHelp.SubClass = SubClass;

            domesticHelp.IsSaved = isSave;
            domesticHelp.IsActivePolicy = !isSave;
      

            if (domesticHelp.IsPhysicalDefect == "Yes")
            {
                domesticHelp.PhysicalDefectDescription = txtPhysicalDesc.Text.Trim();
            }
            //Get Insured Person details by CPR or InsuredCode.
            var insured = new BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest
            {
                CPR = txtCPR.Text.Trim(),
                InsuredCode = txtClientCode.Text.Trim(),
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode

            };

            var insuredDetails = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest>
                                 (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchUserDetailsByCPRInsuredCode, insured);

            if (insuredDetails.StatusCode == 200 && insuredDetails.Result.IsTransactionDone)
            {
                domesticHelp.InsuredCode = insuredDetails.Result.InsuredDetails.InsuredCode;
                domesticHelp.InsuredName = insuredDetails.Result.InsuredDetails.FirstName + " " + insuredDetails.Result.InsuredDetails.MiddleName + " " + insuredDetails.Result.InsuredDetails.LastName;
                domesticHelp.CPR = insuredDetails.Result.InsuredDetails.CPR;
                domesticHelp.DOB = insuredDetails.Result.InsuredDetails.DateOfBirth ?? DateTime.Now;

                domesticHelp.Mobile = insuredDetails.Result.InsuredDetails.Mobile;
                domesticHelp.FFPNumber = "";
            }

            //Get Domestichelp members details.
            List<DomesticHelpMember> members = new List<DomesticHelpMember>();
            members = GetDomesticDetails();

            //Insert or update the domestic policy.
            var authenticatedservice = new DataServiceManager(ClientUtility.WebApiUri, userInfo.AccessToken, false);

            if (_DomesticId > 0)
                domesticHelp.DomesticID = _DomesticId;

            var postPolicyDetails = new DomesticPolicyDetails();
            postPolicyDetails.DomesticHelp = domesticHelp;
            postPolicyDetails.DomesticHelpMemberList = members;
            postPolicyDetails.DomesticHelp.Agency = userInfo.Agency;
            postPolicyDetails.DomesticHelp.AgentCode = userInfo.AgentCode;
            postPolicyDetails.DomesticHelp.CreatedBy = ddlUsers.SelectedIndex > 0 ?
                                       Convert.ToInt32(ddlUsers.SelectedItem.Value) : Convert.ToInt32(userInfo.ID);

            var postData = authenticatedservice.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticHelpPolicyResponse>,
                           BKIC.SellingPoint.DTO.RequestResponseWrappers.DomesticPolicyDetails>
                           (BKIC.SellingPoint.DTO.Constants.DomesticURI.PostQuote, postPolicyDetails);

            if (postData.StatusCode == 200 && postData.Result.IsTransactionDone)
            {
                _DomesticId = postData.Result.DomesticID;
                LoadAgencyClientPolicyInsuredCode(userInfo, service);
                //ddlDomesticPolicies.SelectedIndex = ddlDomesticPolicies.Items.IndexOf(ddlDomesticPolicies.Items.FindByText(postData.Result.DocumentNo));
                txtDomesticPolicySearch.Text = postData.Result.DocumentNo;
                modalBodyText.InnerText = GetMessageText(postData.Result.IsHIR, postPolicyDetails.DomesticHelp.IsActivePolicy, postData.Result.DocumentNo);
                if (postPolicyDetails.DomesticHelp.IsActivePolicy)
                {
                    SetScheduleHRef(postData.Result.DocumentNo, Constants.DomesticHelp, userInfo);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowPopup();", true);
            }
            else
            {
                master.ShowErrorPopup(postData.Result.TransactionErrorMessage, "Request Failed !");
            }
        }

        public int CalculateYears()
        {
            var ToDate = Convert.ToDateTime(txtPolicyEndDate.Text.CovertToCustomDateTime()).CovertToLocalFormat();
            var FromDate = Convert.ToDateTime(txtPolicyStartDate.Text.CovertToCustomDateTime()).CovertToLocalFormat();
            var years = (Convert.ToDateTime(ToDate.CovertToCustomDateTime()) - Convert.ToDateTime(FromDate.CovertToCustomDateTime())).Days / 365;
            if (years <= 0)
            {
                years = 1;
            }
            return years;
        }

        protected void calculate_expiredate(object sender, EventArgs e)
        {
            if (ddlNoOfYears.SelectedIndex > 0)
            {
                int Periods = Convert.ToInt32(ddlNoOfYears.SelectedItem.Value);
                txtPolicyEndDate.Text = Convert.ToDateTime(txtPolicyStartDate.Text.CovertToCustomDateTime())
                                        .AddYears(Periods).AddDays(-1).CovertToLocalFormat();
            }
            Page_CustomValidate();
           
        }

        public void ShowPremium(OAuthTokenResponse userInfo, decimal Premium, decimal Commission)
        {
            amtDisplay.Visible = true;
            btnDomesticSave.Visible = true;
            if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
            {
                premiumAmount.Text = Convert.ToString(0);
                commission.Text = Convert.ToString(0);
                txtDiscount.Text = Convert.ToString(0);
                premiumAmount.Text = Convert.ToString(Premium);
                commission.Text = Convert.ToString(Commission);
                includeDisc.Visible = true;
                txtDiscount.Enabled = true;
            }
            else
            {
                premiumAmount1.Text = Convert.ToString(0);
                commission1.Text = Convert.ToString(0);
                premiumAmount1.Text = Convert.ToString(Premium);
                commission1.Text = Convert.ToString(Commission);
                excludeDisc.Visible = true;
                txtDiscount1.Enabled = false;
            }
        }

        public void SetScheduleHRef(string DocNo, string Insurancetype, OAuthTokenResponse UserInfo)
        {
            downloadschedule.Visible = true;
            downloadschedule.HRef = ClientUtility.WebApiUri + BKIC.SellingPoint.DTO.Constants.ScheduleURI.downloadschedule
                                    .Replace("{insuranceType}", Insurancetype)
                                    .Replace("{agentCode}", UserInfo.AgentCode)
                                    .Replace("{documentNo}", DocNo)
                                    .Replace("{isEndorsement}", "false")
                                    .Replace("{endorsementID}", "0")
                                    .Replace("{renewalCount}", "0");
        }

        public void RemoveScheduleHRef()
        {
            downloadschedule.Visible = false;
            downloadschedule.HRef = string.Empty;
        }

        protected void Reset_Content(object sender, EventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            modalBodyText.InnerText = "Are you sure want to authorize this policy?";
            btnOK.Text = "No";
            btnYes.Visible = true;
        }

        public void Page_CustomValidate()
        {
            if (formDomesticSubmitted.Value == "true")
            {
                // Validate("domesticValidation");
            }
        }

        public Control GetContentControl()
        {
            MasterPage ctl00 = FindControl("ctl00") as MasterPage;
            ContentPlaceHolder MainContent = ctl00.FindControl("ContentPlaceHolder1") as ContentPlaceHolder;
            return MainContent.FindControl("subpanel");
        }

        public string GetMessageText(bool isHIR, bool isActivePolicy, string docNo)
        {
            if (isHIR && !isActivePolicy)
            {
                // master.makeReadOnly(GetContentControl(), true);
                btnYes.Visible = false;
                btnOK.Text = "OK";
                btnAuthorize.Visible = false;
                return "Your domestic policy is saved and moved into HIR: " + docNo;
            }
            else if (!isHIR && !isActivePolicy)
            {
                // master.makeReadOnly(GetContentControl(), true);
                btnYes.Visible = false;
                btnOK.Text = "OK";
                btnAuthorize.Enabled = true;
                btnAuthorize.Visible = true;
                return "Your domestic policy has been saved successfully: " + docNo;
            }
            else if (isActivePolicy)
            {
                master.makeReadOnly(GetContentControl(), false);
                btnCalculate.Enabled = false;
                btnYes.Visible = false;
                btnOK.Text = "OK";
                btnAuthorize.Enabled = false;
                btnDomesticSave.Enabled = false;
                return "Your domestic policy has been authorized successfully: " + docNo;
            }
            else
                return string.Empty;
        }

        public void SetReadOnlyControls()
        {
            txtCPR.Enabled = false;
            txtClientCode.Enabled = false;
            txtInsuredName.Enabled = false;
            txtIssueDate.Text = DateTime.Now.CovertToLocalFormat();
            txtIssueDate.Enabled = false;
            premiumAmount.Enabled = false;
            premiumAmount1.Enabled = false;
            commission.Enabled = false;
            commission1.Enabled = false;
            btnBack.Enabled = true;
            txtVATAmount.Enabled = false;
            txtVATAmount1.Enabled = false;
            txtVATCommission.Enabled = false;
            txtVATCommission1.Enabled = false;
            txtTotalPremium.Enabled = false;
            txtTotalPremium1.Enabled = false;
            txtTotalCommission.Enabled = false;
            txtTotalCommission1.Enabled = false;
            txtDiscount1.Enabled = false;
        }

        private void EnablePaymentValidator()
        {
            rfvddlPaymentMethod.Enabled = true;
            if (ddlPaymentMethod.SelectedIndex == 1)
            {
                rfvtxtAccountNo.Enabled = false;
            }
            else
            {
                rfvtxtAccountNo.Enabled = true;
            }
        }

        private void DisablePaymentValidator()
        {
            rfvddlPaymentMethod.Enabled = false;
            rfvtxtAccountNo.Enabled = false;
        }

        protected void btnClear_Click(object sener, EventArgs e)
        {
            try
            {
                master.ClearControls(GetContentControl());
                SetReadOnlyControls();
                HidePremium();
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

        public void HidePremium()
        {
            amtDisplay.Visible = false;

            premiumAmount.Text = string.Empty;
            commission.Text = string.Empty;
            includeDisc.Visible = false;

            premiumAmount1.Text = string.Empty;
            commission1.Text = string.Empty;
            excludeDisc.Visible = false;

            btnBack.Enabled = true;
            btnDomesticSave.Enabled = true;
            btnCalculate.Enabled = true;
            btnBack.Visible = true;
            btnDomesticSave.Visible = true;
            btnCalculate.Visible = true;

            btnAuthorize.Visible = false;
            downloadschedule.Visible = false;

            //ddlCPR.SelectedIndex = 0;
            txtCPRSearch.Text = string.Empty;
            //ddlDomesticPolicies.SelectedIndex = 0;
            txtDomesticPolicySearch.Text = string.Empty;
            _DomesticId = 0;
        }

        public void DisableControls()
        {
            btnDomesticSave.Visible = false;
            btnAuthorize.Visible = false;
            premiumAmount.Text = string.Empty;
            premiumAmount1.Text = string.Empty;
            commission.Text = string.Empty;
            commission1.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            includeDisc.Visible = false;
            excludeDisc.Visible = false;
        }

        public void ShowVAT(OAuthTokenResponse userInfo, decimal vatAmount, decimal vatCommission, decimal totalPremium,
                 decimal totalCommission)
        {
            if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
            {
                txtVATAmount.Text = Convert.ToString(0);
                txtVATCommission.Text = Convert.ToString(0);
                txtTotalPremium.Text = Convert.ToString(0);
                txtTotalCommission.Text = Convert.ToString(0);
                txtVATAmount.Text = Convert.ToString(vatAmount);
                txtVATCommission.Text = Convert.ToString(vatCommission);
                txtTotalPremium.Text = Convert.ToString(totalPremium);
                txtTotalCommission.Text = Convert.ToString(totalCommission);
            }
            else
            {
                txtVATAmount1.Text = Convert.ToString(0);
                txtVATCommission1.Text = Convert.ToString(0);
                txtTotalPremium1.Text = Convert.ToString(0);
                txtTotalCommission1.Text = Convert.ToString(0);
                txtVATAmount1.Text = Convert.ToString(vatAmount);
                txtVATCommission1.Text = Convert.ToString(vatCommission);
                txtTotalPremium1.Text = Convert.ToString(totalPremium);
                txtTotalCommission1.Text = Convert.ToString(totalCommission);
            }
        }


        public void ShowDiscount(OAuthTokenResponse userInfo, DomesticHelpPolicy policy)
        {
            txtDiscount.Text = Convert.ToString(policy.PremiumBeforeDiscount - policy.PremiumAfterDiscount);
            if(userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.User)
            {
                if(policy.PremiumBeforeDiscount - policy.PremiumAfterDiscount > 0)
                {
                    txtDiscount1.Text = Convert.ToString(policy.PremiumBeforeDiscount - policy.PremiumAfterDiscount);
                    txtDiscount1.Enabled = false;                    
                }
                else
                {                    
                    txtDiscount1.Enabled = false;
                }
            }
        }
    }
}