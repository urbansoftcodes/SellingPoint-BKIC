using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using BKIC.SellingPoint.Presentation;
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

namespace SellingPoint.Presentation
{
    public partial class HomeAddRemoveDomesticHelpEndorsement : System.Web.UI.Page
    {
        private General master;
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails> InsuredNames { get; set; }
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomePolicy> policyList;
        public static long _HomeEndorsementID { get; set; }
        public static string MainClass { get; set; }
        public static bool AjdustedPremium { get; set; }
        public static string SubClass { get; set; }
        public static DataTable Nationalitydt;
        public static DataTable domesticHelpOccupationDt;
        public static bool isRiotAdded { get; set; }

        public HomeAddRemoveDomesticHelpEndorsement()
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

                BindAgencyClientCodeDropdown(userInfo, service);
                SetInitialRow();
                _HomeEndorsementID = 0;
                btnSubmit.Visible = false;
                divPaymentSection.Visible = userInfo.IsShowPayments;
            }
        }

        private void BindAgencyClientCodeDropdown(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns
                                .Replace("{type}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.HomeInsurance));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable AgencyData = dropdownds.Tables["AgentCodeDD"];
                //DataTable propertyInsured = dropdownds.Tables["BK_PropertyInsured"];
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

            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest
            {
                AgentBranch = userInfo.AgentBranch,
                AgentCode = userInfo.AgentCode
            };

            var insuredResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest>
                               (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyInsured, req);

            if (insuredResult.StatusCode == 200 && insuredResult.Result.IsTransactionDone && insuredResult.Result.AgencyInsured.Count > 0)
            {
                //ddlCPR.DataSource = insuredResult.Result.AgencyInsured;
                //ddlCPR.DataTextField = "CPR";
                //ddlCPR.DataValueField = "InsuredCode";
                //ddlCPR.DataBind();
                //ddlCPR.Items.Insert(0, new ListItem("--Please Select--", ""));
                InsuredNames = insuredResult.Result.AgencyInsured;
            }
            txtIndroducedBy.Text = userInfo.UserName;
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(userInfo.AgentBranch));
            txtIndroducedBy.Text = userInfo.UserName;
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();

            DataRow dr = null;

            dt.Columns.Add(new DataColumn("ITEMNAME", typeof(string)));
            dt.Columns.Add(new DataColumn("SEX", typeof(string)));
            dt.Columns.Add(new DataColumn("DATEOFBIRTH", typeof(string)));

            dt.Columns.Add(new DataColumn("NATIONALITY", typeof(string)));
            dt.Columns.Add(new DataColumn("CPR", typeof(string)));
            dt.Columns.Add(new DataColumn("OCCUPATION", typeof(string)));

            dt.Columns.Add(new DataColumn("Flat No", typeof(string)));
            dt.Columns.Add(new DataColumn("Building No", typeof(string)));
            dt.Columns.Add(new DataColumn("Block No", typeof(string)));

            dt.Columns.Add(new DataColumn("Road No", typeof(string)));
            dt.Columns.Add(new DataColumn("Town", typeof(string)));

            dr = dt.NewRow();

            //dr["RowNumber"] = 1;
            dr["ITEMNAME"] = string.Empty;
            dr["SEX"] = string.Empty;
            dr["DATEOFBIRTH"] = string.Empty;

            dr["NATIONALITY"] = string.Empty;
            dr["CPR"] = string.Empty;
            dr["OCCUPATION"] = string.Empty;

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
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            ////extract the TextBox values
                            TextBox txName = (TextBox)Gridview1.Rows[rowIndex].Cells[0].FindControl("txtDomesticName");
                            DropDownList ddlSex = (DropDownList)Gridview1.Rows[rowIndex].Cells[1].FindControl("ddlGender");
                            TextBox txDOB = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtDOB");
                            DropDownList ddlNationality = (DropDownList)Gridview1.Rows[rowIndex].Cells[3].FindControl("ddlNational");
                            TextBox txPassPort = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtPassport");
                            DropDownList ddlOccupation = (DropDownList)Gridview1.Rows[rowIndex].Cells[5].FindControl("ddlDomesticOccupation");

                            drCurrentRow = dtCurrentTable.NewRow();
                            dtCurrentTable.Rows[i - 1]["ITEMNAME"] = txName.Text;
                            dtCurrentTable.Rows[i - 1]["SEX"] = ddlSex.SelectedItem.Value;
                            dtCurrentTable.Rows[i - 1]["DATEOFBIRTH"] = txDOB.Text;

                            dtCurrentTable.Rows[i - 1]["NATIONALITY"] = ddlNationality.SelectedItem.Value;
                            dtCurrentTable.Rows[i - 1]["CPR"] = txPassPort.Text;
                            dtCurrentTable.Rows[i - 1]["OCCUPATION"] = ddlOccupation.SelectedItem.Text;

                            rowIndex++;
                        }

                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["CurrentTable"] = dtCurrentTable;
                        Gridview1.DataSource = dtCurrentTable;
                        Gridview1.DataBind();
                        btnSubmit.Visible = false;
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

                        txName.Text = dt.Rows[i]["ITEMNAME"].ToString();
                        ddlSex.SelectedIndex = ddlSex.Items.IndexOf(ddlSex.Items.FindByValue(dt.Rows[i]["SEX"].ToString().ToUpper()));
                        txDOB.Text = dt.Rows[i]["DATEOFBIRTH"].ToString();
                        txPassPort.Text = dt.Rows[i]["CPR"].ToString();
                        ddlNationality.SelectedIndex = ddlNationality.Items.IndexOf(ddlNationality.Items.FindByValue(dt.Rows[i]["NATIONALITY"].ToString()));
                        ddlOccupation.SelectedIndex = ddlOccupation.Items.IndexOf(ddlOccupation.Items.FindByText(dt.Rows[i]["OCCUPATION"].ToString()));

                        rowIndex++;
                    }
                }
            }
        }

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

        protected void Gridview1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                ViewState["CurrentTable"] = GetDomesticHelperForUI();
                DataTable dt = ViewState["CurrentTable"] as DataTable;
                dt.Rows[index].Delete();
                ViewState["CurrentTable"] = dt;
                Gridview1.DataSource = dt;
                Gridview1.DataBind();
                SetPreviousData();
                btnSubmit.Visible = false;
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
                txtHomeEndorsementSearch.Text = string.Empty;
                GetHomePoliciesByCPR();
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

        private void GetHomePoliciesByCPR()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var homereq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest();
            homereq.AgentCode = userInfo.AgentCode;
            homereq.Agency = userInfo.Agency;
            homereq.AgentBranch = userInfo.AgentBranch;
            // homereq.CPR = ddlCPR.SelectedIndex > 0 ? ddlCPR.SelectedItem.Text.Trim() : string.Empty;
            homereq.CPR = txtCPRSearch.Text.Trim();
            homereq.Type = Constants.Home;

            //Get PolicyNo by Agency
            var homePolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomePolicyResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest>
                                (BKIC.SellingPoint.DTO.Constants.HomeURI.GetHomePoliciesByCPR, homereq);

            ddlHomePolicies.Items.Clear();
            if (homePolicies.StatusCode == 200 && homePolicies.Result.IsTransactionDone
                && homePolicies.Result.AgencyHomePolicies.Count > 0)
            {
                policyList = homePolicies.Result.AgencyHomePolicies;

                ddlHomePolicies.DataSource = homePolicies.Result.AgencyHomePolicies;
                ddlHomePolicies.DataTextField = "DOCUMENTNO";
                ddlHomePolicies.DataValueField = "DOCUMENTRENEWALNO";
                ddlHomePolicies.DataBind();
                ddlHomePolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
            }           
            ClearControls();
        }

        protected void Changed_HomePolicy(object sender, EventArgs e)
        {
            try
            {
                if (ddlHomePolicies.SelectedIndex > 0)
                {
                    var policyRenewalCount = ddlHomePolicies.SelectedItem.Value.Substring(0, ddlHomePolicies.SelectedValue.IndexOf("-", 0));
                    UpdatePolicyDetails(ddlHomePolicies.SelectedItem.Text.Trim(), policyRenewalCount, false);
                }
                else
                {
                    ClearControls();
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

        protected void PolicySearch_Changed(object sender, EventArgs e)
        {
            try
            {
                //var policyRenewalCount = string.IsNullOrEmpty(renewalCount.Value) ? Convert.ToString(0) : renewalCount.Value;
                //UpdatePolicyDetails(txtHomeEndorsementSearch.Text.Trim(), policyRenewalCount, true);

                var renewalCount = GetPolicyRenewalCount();
                UpdatePolicyDetails(txtHomeEndorsementSearch.Text.Trim(), renewalCount.ToString(), true);
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


        public int GetPolicyRenewalCount()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            int renewalCount = 0;

            var homereq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest();
            homereq.AgentCode = userInfo.AgentCode;
            homereq.Agency = userInfo.Agency;
            homereq.AgentBranch = userInfo.AgentBranch;
            homereq.IncludeHIR = false;
            homereq.IsRenewal = false;
            homereq.DocumentNo = txtHomeEndorsementSearch.Text.Trim();

            //Get PolicyNo by Agency
            var homePolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomePolicyResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest>
                              (BKIC.SellingPoint.DTO.Constants.HomeURI.GetHomePoliciesEndorsement, homereq);

            if (homePolicies.StatusCode == 200 && homePolicies.Result.IsTransactionDone
                && homePolicies.Result.AgencyHomePolicies.Count > 0)
            {
                renewalCount = homePolicies.Result.AgencyHomePolicies[0].RenewalCount;
            }
            return renewalCount;
        }

        private void UpdatePolicyDetails(string DocumentNo, string RenewalCount, bool SearchByDocument)
        {
                ClearControls(); 

                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

            // var policyRenewalCount = ddlHomePolicies.SelectedItem.Value.Substring(0, ddlHomePolicies.SelectedValue.IndexOf("-", 0));

            //Get saved policy details by document(policy) number.
            var url = BKIC.SellingPoint.DTO.Constants.HomeURI.GetHomeSavedQuoteDocumentNo
                          .Replace("{documentNo}", DocumentNo)
                          .Replace("{agentCode}", userInfo.AgentCode)
                          .Replace("{isendorsement}", "true")
                          .Replace("{endorsementid}", "0")
                          .Replace("{renewalCount}", RenewalCount);

                var homeDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (homeDetails.StatusCode == 200 && homeDetails.Result.IsTransactionDone)
                {
                    var response = homeDetails.Result.HomeInsurancePolicy;

                    txtOldClientCode.Text = response.InsuredCode;
                    txtOldInsuredName.Text = response.InsuredName;
                    txtEffectiveFromDate.Text = response.PolicyStartDate.CovertToLocalFormat();
                    txtEffectiveToDate.Text = response.PolicyExpiryDate.CovertToLocalFormat();
                    paidPremium.Value = Convert.ToString(response.PremiumAfterDiscount);
                    subClass.Value = response.SubClass;
                    SubClass = response.SubClass;
                    MainClass = response.MainClass;
                    expireDate.Value = response.PolicyExpiryDate.CovertToLocalFormat();
                    isRiotAdded = response.IsRiotStrikeDamage.ToString().ToUpper() == "Y" ? true : false;

                    //Update travel members details on the page.
                    if (homeDetails.Result.DomesticHelp != null && homeDetails.Result.DomesticHelp.Count > 0)
                    {
                        SetInitialRow();
                        // ddlPDomesticWorksCover.SelectedIndex = domesticHelpDetails.Count;
                        if (ViewState["CurrentTable"] != null)
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
                                for (int i = 0; i < homeDetails.Result.DomesticHelp.Count; i++)
                                {
                                    drCurrentRow = dtCurrentTable.NewRow();
                                    dtCurrentTable.Rows.Add(drCurrentRow);

                                    dtCurrentTable.Rows[i]["ITEMNAME"] = homeDetails.Result.DomesticHelp[i].Name;
                                    dtCurrentTable.Rows[i]["SEX"] = homeDetails.Result.DomesticHelp[i].Sex;
                                    dtCurrentTable.Rows[i]["DATEOFBIRTH"] = homeDetails.Result.DomesticHelp[i].DOB.CovertToLocalFormat();

                                    dtCurrentTable.Rows[i]["NATIONALITY"] = homeDetails.Result.DomesticHelp[i].Nationality;
                                    dtCurrentTable.Rows[i]["CPR"] = homeDetails.Result.DomesticHelp[i].CPR;
                                    dtCurrentTable.Rows[i]["OCCUPATION"] = homeDetails.Result.DomesticHelp[i].Occupation;
                                }
                                ViewState["CurrentTable"] = dtCurrentTable;
                                Gridview1.DataSource = dtCurrentTable;
                                Gridview1.DataBind();
                                SetPreviousData();
                            }
                        }
                        //divNoOfDomesticWorkersDetails.Visible = true;
                        divDetailedDomesticWorkers.Visible = true;                       
                    }
                    else
                    {
                        SetInitialRow();
                        if (ViewState["CurrentTable"] != null)
                        {
                            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                            //for (int i = dtCurrentTable.Rows.Count; i > NoOfHelpers; i--)
                            //{
                            //    dtCurrentTable.Rows[i - 1].Delete();
                            //}
                            ViewState["CurrentTable"] = dtCurrentTable;
                            Gridview1.DataSource = dtCurrentTable;
                            Gridview1.DataBind();
                        }
                        divDetailedDomesticWorkers.Visible = true;
                    }
                    IsCancelled(response.IsCancelled);
                    if (SearchByDocument)
                    {
                        ddlHomePolicies.Items.Clear();
                        txtCPRSearch.Text = response.CPR;
                        ddlHomePolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
                        ddlHomePolicies.Items.Insert(1, new ListItem(txtHomeEndorsementSearch.Text.Trim(), RenewalCount + "-" + txtHomeEndorsementSearch.Text.Trim()));
                        ddlHomePolicies.DataBind();
                        ddlHomePolicies.SelectedIndex = 1;
                    }
                    //List the previous endorsements for the policy.
                    ListEndorsements(service, userInfo);
                }
                else
                {
                    master.ShowErrorPopup(homeDetails.Result.TransactionErrorMessage, "Request failed!");
                }            
        }

        public void IsCancelled(bool isCancelled)
        {
            if (isCancelled)
            {
                btnCalculate.Visible = false;
                btnSubmit.Visible = false;
                master.ShowErrorPopup("This policy is already cancelled", "Policy Cancelled");
            }
            else
            {
                btnCalculate.Visible = true;
            }
        }

        private void EnablePaymentValidator()
        {
            rfvddlPaymentMethods.Enabled = true;
            if (ddlPaymentMethods.SelectedIndex == 1)
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
            rfvddlPaymentMethods.Enabled = false;
            rfvtxtAccountNo.Enabled = false;
        }

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewRowToGrid();
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

        protected void lnkbtnAuthorize_Click(object sender, EventArgs e)
        {
            try
            {
                EndorsementOperation(sender, "authorize");
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

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentMethods.SelectedIndex == 1)
            {
                txtAccountNumber.Text = "";
                txtAccountNumber.Enabled = false;
            }
            else
            {
                txtAccountNumber.Enabled = true;
            }            
        }

        public void EndorsementOperation(object sender, string type)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementOperation
                {
                    HomeEndorsementID = Convert.ToInt32((row.FindControl("lblHomeEndorsementID") as Label).Text.Trim()),
                    HomeID = Convert.ToInt32((row.FindControl("lblHomeID") as Label).Text.Trim()),
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    Type = type,
                    UpdatedBy = Convert.ToInt32(userInfo.ID)
                };

                var endoResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementOperationResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementOperation>
                                 (BKIC.SellingPoint.DTO.Constants.HomeEndorsementURI.EndorsementOperation, details);

                if (endoResult.StatusCode == 200 && endoResult.Result.IsTransactionDone)
                {
                    ListEndorsements(service, userInfo);
                    if (type == "delete")
                    {
                        master.ShowErrorPopup("Your endorsement deleted successfully", "Home Endorsement");
                    }
                    else if (type == "authorize")
                    {
                        master.ShowErrorPopup("Your endorsement authorized successfully", "Home Endorsement");
                    }
                }
            }
        }

        private void ListEndorsements(DataServiceManager service, OAuthTokenResponse userInfo)
        {
            if (userInfo == null)
                Response.Redirect("Login.aspx");

            if (ddlHomePolicies.SelectedIndex > 0)
            {
                var homeEndoRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndoRequest();
                homeEndoRequest.Agency = userInfo.Agency;
                homeEndoRequest.AgentCode = userInfo.AgentCode;
                homeEndoRequest.InsuranceType = Constants.Home;
                homeEndoRequest.DocumentNo = ddlHomePolicies.SelectedItem.Text.Trim();

                var listEndoResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndoResponse>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndoRequest>
                            (BKIC.SellingPoint.DTO.Constants.HomeEndorsementURI.GetAllEndorsements, homeEndoRequest);

                if (listEndoResponse.StatusCode == 200 && listEndoResponse.Result.IsTransactionDone)
                {
                    gvHomeEndorsement.DataSource = listEndoResponse.Result.HomeEndorsements;
                    gvHomeEndorsement.DataBind();

                    if (listEndoResponse.Result.HomeEndorsements.Count > 0)
                    {
                        _HomeEndorsementID = listEndoResponse.Result.HomeEndorsements[listEndoResponse.Result.HomeEndorsements.Count - 1].HomeEndorsementID;
                    }
                    else
                    {
                        _HomeEndorsementID = 0;
                    }
                }
            }
        }

        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                EndorsementOperation(sender, "delete");
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

        protected void lnkbtnSchedule_Click(object sender, EventArgs e)
        {
            try
            {
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

        protected void lnkbtnCertificate_Click(object sender, EventArgs e)
        {
            try
            {
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

        protected void Calculate_Click(object sender, EventArgs e)
        {
            try
            {
                DisablePaymentValidator();
                Page.Validate();                
                if (Page.IsValid)
                {
                    if (ddlHomePolicies.SelectedIndex > 0)
                    {
                        CalculateEndorsementQuote(true);
                    }
                    else
                    {
                        return;
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

        public void CalculateEndorsementQuote(bool showPremium)
        {
            
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var homeEndorementQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementDomesticHelpQuote
            {
                Domestichelp = GetHomeDomesticHelps(),
                DocumentNumber = ddlHomePolicies.SelectedItem.Text.Trim(),
                MainClass = MainClass,
                SubClass = subClass.Value,
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode,
                //DomesticHelp Calculation not included in the SRCC, riot is always false.
                IsRiotAdded = false
            };

            var policyRenewalCount = ddlHomePolicies.SelectedItem.Value.Substring(0, ddlHomePolicies.SelectedValue.IndexOf("-", 0));

            homeEndorementQuote.RenewalCount = string.IsNullOrEmpty(policyRenewalCount) ? 0 : Convert.ToInt32(policyRenewalCount);

            //Calculate the home endorsement premium.
            var homeEndoQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementQuoteResponse>,
                                              BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementDomesticHelpQuote>
                                              (BKIC.SellingPoint.DTO.Constants.HomeEndorsementURI.GetHomeDomesticHelpEndorsementQuote,
                                              homeEndorementQuote);

            if (homeEndoQuoteResult.StatusCode == 200 && homeEndoQuoteResult.Result.IsTransactionDone)
            {
                var endoresementPremium = homeEndoQuoteResult.Result.EndorsementPremium;
                calculatedPremium.Value = endoresementPremium.ToString();
                adjustedPremium.Value = endoresementPremium.ToString();

                var product = master.GetHomeProduct(MainClass, SubClass);
                bool includeCommission = false;
                if (product != null)
                {
                    var hEndorsement = product.HomeEndorsementMaster.Find(c => c.EndorsementType == "AddRemoveDomesticHelp");
                    if (hEndorsement != null)
                    {
                        includeCommission = hEndorsement.HasCommission;
                    }
                }

                var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeCommissionRequest();
                commisionRequest.AgentCode = userInfo.AgentCode;
                commisionRequest.Agency = userInfo.Agency;
                commisionRequest.SubClass = subClass.Value;
                commisionRequest.PremiumAmount = includeCommission ? endoresementPremium : decimal.Zero;
                //DomesticHelp Calculation not included in the SRCC, riot is always false.
                commisionRequest.IsRoitAdded = false;
                
                
                

                //Get commision for the endorsement premium.
                var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeCommissionResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeCommissionRequest>
                                       ("api/insurance/HomeCommission", commisionRequest);

                if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone)
                {
                    var ToatlCommission = commissionresult.Result.BasicCommission + commissionresult.Result.SRCCCommission;
                    calculatedCommission.Value = Convert.ToString(ToatlCommission);
                    adjustedCommission.Value = Convert.ToString(ToatlCommission);
                    ShowPremium(userInfo, endoresementPremium, ToatlCommission);
                }
                else
                {
                    master.ShowLoading = false;
                    master.ShowErrorPopup(commissionresult.Result.TransactionErrorMessage, "Request Failed !");
                    return;
                }
                //Calculate VAT.
                var vatResponse = master.GetVat(homeEndoQuoteResult.Result.EndorsementPremium,
                                    (commissionresult.Result.BasicCommission + commissionresult.Result.SRCCCommission));

                if (vatResponse != null && vatResponse.IsTransactionDone)
                {
                    decimal TotalPremium = homeEndoQuoteResult.Result.EndorsementPremium + vatResponse.VatAmount;
                    decimal TotalCommission = (commissionresult.Result.BasicCommission + commissionresult.Result.SRCCCommission) + vatResponse.VatCommissionAmount;
                    ShowVAT(userInfo, vatResponse.VatAmount, vatResponse.VatCommissionAmount, TotalPremium, TotalCommission);
                }
                btnSubmit.Visible = true;
            }
        }

        public void ShowPremium(OAuthTokenResponse userInfo, decimal Premium, decimal Commission)
        {
            amtDisplay.Visible = true;
            if (userInfo.Roles == "SuperAdmin" || userInfo.Roles == "BranchAdmin")
            {
                if (userInfo.Roles == "SuperAdmin")
                {
                    premiumAmount.Enabled = true;
                    commission.Enabled = true;
                }
                premiumAmount.Text = Convert.ToString(Premium);
                commission.Text = Convert.ToString(Commission);
                includeDisc.Visible = true;
            }
            else
            {
                premiumAmount1.Text = Convert.ToString(Premium);
                commission1.Text = Convert.ToString(Commission);
                excludeDisc.Visible = true;
            }
        }

        public string GetMessageText(bool isHIR, string docNo)
        {
            btnYes.Visible = false;
            btnOK.Text = "OK";
            if (isHIR)
            {
                return "Your home endorsement saved and moved into HIR :" + docNo;
            }
            else
            {
                return "Your home endorsement has been saved sucessfully :" + docNo;
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
        }

        protected void validate_Premium(object sender, EventArgs e)
        {
            var Premium = Convert.ToDecimal(calculatedPremium.Value);
            var Commision = Convert.ToDecimal(calculatedCommission.Value);
            decimal Discount = string.IsNullOrEmpty(txtDiscount.Text) ? decimal.Zero : Convert.ToDecimal(txtDiscount.Text);
            var reduceablePremium = Premium - Commision;
            var premiumDiff = Premium - Discount;

            if (premiumDiff < reduceablePremium)
            {
                premiumAmount.Text = Convert.ToString(reduceablePremium);
                txtDiscount.Text = Convert.ToString(calculatedCommission.Value);
                commission.Text = Convert.ToString(0);
            }
            else if (Discount > Premium)
            {
                premiumAmount.Text = Convert.ToString(reduceablePremium);
                txtDiscount.Text = Convert.ToString(calculatedCommission.Value);
                commission.Text = Convert.ToString(0);
            }
            else
            {
                premiumAmount.Text = Convert.ToString(premiumDiff);
                commission.Text = Convert.ToString(Commision - Discount);
                btnSubmit.Visible = true;
            }
            //Calculate VAT.
            var vatResponse = master.GetVat(string.IsNullOrEmpty(premiumAmount.Text) ? 0 : Convert.ToDecimal(premiumAmount.Text),
                              string.IsNullOrEmpty(commission.Text) ? 0 : Convert.ToDecimal(commission.Text));

            if (vatResponse != null && vatResponse.IsTransactionDone)
            {
                txtVATAmount.Text = Convert.ToString(vatResponse.VatAmount);
                txtVATCommission.Text = Convert.ToString(vatResponse.VatCommissionAmount);
                txtTotalPremium.Text = Convert.ToString(string.IsNullOrEmpty(premiumAmount.Text) ? 0 :
                                       master.NearestOneHalf(Convert.ToDecimal(premiumAmount.Text) + vatResponse.VatAmount));
                txtTotalCommission.Text = Convert.ToString(string.IsNullOrEmpty(commission.Text) ? 0 : Convert.ToDecimal(commission.Text) + vatResponse.VatCommissionAmount);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsRefundPremium())
                {
                    EnablePaymentValidator();
                }
                else
                {
                    DisablePaymentValidator();
                }
                Page.Validate();               
                if (Page.IsValid)
                {
                    SaveAuthorize(true);
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

        public bool IsRefundPremium()
        {
            if (!string.IsNullOrEmpty(adjustedPremium.Value) && Convert.ToDecimal(adjustedPremium.Value) < 0)
            {
                return true;
            }
            return false;
        }

        public bool EndorsementPrecheck()
        {
            

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var req = new HomeEndorsementPreCheckRequest();
            req.DocNo = ddlHomePolicies.SelectedIndex > 0 ? ddlHomePolicies.SelectedItem.Text : string.Empty;

            var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementPreCheckResponse>,
                         BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementPreCheckRequest>
                         (BKIC.SellingPoint.DTO.Constants.HomeEndorsementURI.EndorsementPreCheck, req);

            if (result.StatusCode == 200 && result.Result.IsTransactionDone)
            {
                return result.Result.IsAlreadyHave;
            }
            return false;
        }

        public void SaveAuthorize(bool isSave)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                if (isSave && EndorsementPrecheck())
                {
                    master.ShowErrorPopup("Your home policy already have saved endorsement !", "Home Endorsement");
                    return;
                }

                var postHomeEndorsement = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsement();
                postHomeEndorsement.Agency = userInfo.Agency;
                postHomeEndorsement.AgencyCode = userInfo.AgentCode;
                postHomeEndorsement.AgentBranch = userInfo.AgentBranch;
                postHomeEndorsement.IsSaved = isSave;
                postHomeEndorsement.IsActivePolicy = !isSave;
                postHomeEndorsement.PremiumAmount = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value);
                postHomeEndorsement.CreatedBy = Convert.ToInt32(userInfo.ID);

                var policyRenewalCount = ddlHomePolicies.SelectedItem.Value.Substring(0, ddlHomePolicies.SelectedValue.IndexOf("-", 0));

                //Get saved policy details by document(policy) number.
                var url = BKIC.SellingPoint.DTO.Constants.HomeURI.GetHomeSavedQuoteDocumentNo
                          .Replace("{documentNo}", ddlHomePolicies.SelectedItem.Text.Trim())
                          .Replace("{agentCode}", userInfo.AgentCode)
                          .Replace("{isendorsement}", "true")
                          .Replace("{endorsementid}", "0")
                          .Replace("{renewalCount}", policyRenewalCount);

                var homeDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (homeDetails.StatusCode == 200 && homeDetails.Result.IsTransactionDone)
                {
                    SetEndorsementType(postHomeEndorsement, homeDetails.Result.HomeInsurancePolicy);
                }

                var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementResponse>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsement>
                             (BKIC.SellingPoint.DTO.Constants.HomeEndorsementURI.PostHomeEndorsement, postHomeEndorsement);

                if (response.Result != null && response.StatusCode == 200 && response.Result.IsTransactionDone)
                {
                    _HomeEndorsementID = response.Result.HomeEndorsementID;
                    ListEndorsements(service, userInfo);
                    btnSubmit.Visible = false;
                    master.ShowErrorPopup("Your home endorsement has been saved sucessfully :" + response.Result.EndorsementNo, "Home Endorsement");
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

        public void SetEndorsementType(BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsement homeEndorsement, HomeInsurancePolicy homePolicyDetails)
        {
            homeEndorsement.RenewalCount = homePolicyDetails.RenewalCount;
            homeEndorsement.InsuredCode = homePolicyDetails.InsuredCode;
            homeEndorsement.InsuredName = homePolicyDetails.InsuredName;
            homeEndorsement.Mainclass = homePolicyDetails.MainClass;
            homeEndorsement.Subclass = homePolicyDetails.SubClass;
            homeEndorsement.HomeID = homePolicyDetails.HomeID;
            homeEndorsement.PolicyCommencementDate = homePolicyDetails.PolicyStartDate;
            homeEndorsement.ExpiryDate = homePolicyDetails.PolicyExpiryDate;
            homeEndorsement.Remarks = txtRemarks.Text;
            homeEndorsement.AccountNumber = txtAccountNumber.Text;
            homeEndorsement.PaymentType = ddlPaymentMethods.SelectedIndex > 0 ? ddlPaymentMethods.SelectedItem.Text : string.Empty;
            homeEndorsement.EndorsementType = "AddRemoveDomesticHelp";
            homeEndorsement.FinancierCompanyCode = homePolicyDetails.FinancierCode;
            homeEndorsement.HomeDomesticHelp = GetHomeDomesticHelps();
            homeEndorsement.BlockNo = homePolicyDetails.BlockNo;
            homeEndorsement.BuildingAge = homePolicyDetails.BuildingAge;
            homeEndorsement.BuildingNo = homePolicyDetails.BuildingNo;
            homeEndorsement.Area = homePolicyDetails.Area;
            homeEndorsement.NoOfFloors = homePolicyDetails.NoOfFloors;
            homeEndorsement.HouseNo = homePolicyDetails.HouseNo;
            homeEndorsement.FlatNo = homePolicyDetails.FlatNo;
            homeEndorsement.RoadNo = homePolicyDetails.RoadNo;
            homeEndorsement.BuildingType = homePolicyDetails.BuildingType;
            homeEndorsement.BuildingSumInsured = homePolicyDetails.BuildingValue;
            homeEndorsement.ContentSumInsured = homePolicyDetails.ContentValue;
            homeEndorsement.PremiumBeforeDiscount = string.IsNullOrEmpty(calculatedPremium.Value) ? decimal.Zero : Convert.ToDecimal(calculatedPremium.Value);
            homeEndorsement.PremiumAfterDiscount = string.IsNullOrEmpty(adjustedPremium.Value) ? decimal.Zero : Convert.ToDecimal(adjustedPremium.Value);
            homeEndorsement.CommisionBeforeDiscount = string.IsNullOrEmpty(calculatedCommission.Value) ? decimal.Zero : Convert.ToDecimal(calculatedCommission.Value);
            homeEndorsement.CommissionAfterDiscount = string.IsNullOrEmpty(adjustedCommission.Value) ? decimal.Zero : Convert.ToDecimal(adjustedCommission.Value);
        }

        private DataTable GetDomesticWorkersForDB()
        {
            DataTable domesticWorkerDetails = new DataTable();
            domesticWorkerDetails.Columns.Add("ITEMNAME", typeof(string));
            domesticWorkerDetails.Columns.Add("DATEOFBIRTH", typeof(string));
            domesticWorkerDetails.Columns.Add("CPR", typeof(string));
            domesticWorkerDetails.Columns.Add("SEX", typeof(string));
            domesticWorkerDetails.Columns.Add("NATIONALITY", typeof(string));
            domesticWorkerDetails.Columns.Add("OCCUPATION", typeof(string));
            domesticWorkerDetails.Columns.Add("Count", typeof(Int32));

            int count = 0;

            for (int row = 1; row <= Gridview1.Rows.Count; row++)
            {
                count++;
                var obj = new DomesticHelpMember();

                for (int col = 0; col < Gridview1.Columns.Count; col++)
                {
                    if (Gridview1.Columns[col].Visible)
                    {
                        if (String.IsNullOrEmpty(Gridview1.Rows[row - 1].Cells[col].Text))
                        {
                            var colName = Gridview1.Columns[col].ToString();

                            if (colName == "Name")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Name = txtValue.Text.ToString();
                            }

                            if (colName == "Date Of Birth")
                            {
                                //var ss = "04/07/1990";
                                //obj.DOB = ss.CovertToCustomDateTime();
                                TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.DOB = txtValue.Text.CovertToCustomDateTime();
                            }
                            if (colName == "CPR / Passport No")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Passport = txtValue.Text.ToString();
                            }

                            if (colName == "Sex")
                            {
                                //obj.Sex = txtValue.SelectedValue == "Male" ? "M" : "F";
                                DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Sex = Convert.ToChar(txtValue.SelectedItem.Text == "Male" ? "M" : "F");
                            }
                            if (colName == "Nationality")
                            {
                                DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Nationality = txtValue.SelectedValue.ToString();
                            }

                            if (colName == "Occupation")
                            {
                                DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Occupation = txtValue.SelectedItem.Text.Trim();
                            }
                        }
                    }
                }
                domesticWorkerDetails.Rows.Add(obj.Name, obj.DOB, obj.Passport, obj.Sex, obj.Nationality, obj.Occupation, count);
            }

            return domesticWorkerDetails;
        }

        public DataTable GetDomesticHelperForUI()
        {
            DataTable domesticWorkerDetails = new DataTable();
            domesticWorkerDetails.Columns.Add("ITEMNAME", typeof(string));
            domesticWorkerDetails.Columns.Add("DATEOFBIRTH", typeof(string));
            domesticWorkerDetails.Columns.Add("CPR", typeof(string));
            domesticWorkerDetails.Columns.Add("SEX", typeof(string));
            domesticWorkerDetails.Columns.Add("NATIONALITY", typeof(string));
            domesticWorkerDetails.Columns.Add("OCCUPATION", typeof(string));
            domesticWorkerDetails.Columns.Add("Count", typeof(Int32));

            int count = 0;

            for (int row = 1; row <= Gridview1.Rows.Count; row++)
            {
                count++;
                var obj = new DomesticHelpMember();
                string dob = "";

                for (int col = 0; col < Gridview1.Columns.Count; col++)
                {
                    if (Gridview1.Columns[col].Visible)
                    {
                        if (String.IsNullOrEmpty(Gridview1.Rows[row - 1].Cells[col].Text))
                        {
                            var colName = Gridview1.Columns[col].ToString();

                            if (colName == "Name")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Name = txtValue.Text.ToString();
                            }

                            if (colName == "Date Of Birth")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                dob = txtValue.Text;
                            }
                            if (colName == "CPR / Passport No")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Passport = txtValue.Text.ToString();
                            }

                            if (colName == "Sex")
                            {
                                //obj.Sex = txtValue.SelectedValue == "Male" ? "M" : "F";
                                DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Sex = Convert.ToChar(txtValue.SelectedItem.Text == "Male" ? "M" : "F");
                            }
                            if (colName == "Nationality")
                            {
                                DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Nationality = txtValue.SelectedValue.ToString();
                            }

                            if (colName == "Occupation")
                            {
                                DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                                obj.Occupation = txtValue.SelectedItem.Text.Trim();
                            }
                        }
                    }
                }
                domesticWorkerDetails.Rows.Add(obj.Name, dob, obj.Passport, obj.Sex, obj.Nationality, obj.Occupation, count);
            }

            return domesticWorkerDetails;
        }

        public List<HomeDomesticHelp> GetHomeDomesticHelps()
        {
            List<HomeDomesticHelp> homedomesticList = new List<HomeDomesticHelp>();

            DataTable homedomesticdt = GetDomesticWorkersForDB();
            if (homedomesticdt != null && homedomesticdt.Rows.Count > 0)
            {
                homedomesticList = (from DataRow dr in homedomesticdt.Rows
                                    select new HomeDomesticHelp
                                    {
                                        MemberSerialNo = Convert.ToInt32(dr["Count"]),
                                        Name = Convert.ToString(dr["ITEMNAME"]),
                                        DOB = Convert.ToString(dr["DATEOFBIRTH"]).CovertToCustomDateTime2(),
                                        CPR = Convert.ToString(dr["CPR"]),
                                        Nationality = Convert.ToString(dr["Nationality"]),
                                        Sex = Convert.ToChar(dr["Sex"].ToString()),
                                        Occupation = Convert.ToString(dr["Occupation"])
                                    }).ToList();
            }

            return homedomesticList;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Homepage.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
        }

        protected void btnAuthorize_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate();               
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

        public void Reset_Content(object sender, EventArgs e)
        {
            modalBodyText.InnerText = "Your you sure want authorize this endorsement ?";
            btnOK.Text = "No";
            btnYes.Visible = true;
        }

        protected void gvHomeEndorsement_DataBound(object sender, EventArgs e)
        {
            
            foreach (GridViewRow row in gvHomeEndorsement.Rows)
            {
                bool IsSaved = Convert.ToBoolean((row.FindControl("lblIsSaved") as Label).Text.Trim());
                bool IsActive = Convert.ToBoolean((row.FindControl("lblIsActive") as Label).Text.Trim());

                if (IsActive)
                {
                    var btnAuthorize = row.FindControl("lnkbtnAuthorize") as LinkButton;
                    btnAuthorize.Visible = false;

                    var btnDelete = row.FindControl("lnkbtnDelete") as LinkButton;
                    btnDelete.Visible = false;

                    HtmlAnchor lnkSchedule = row.FindControl("downloadschedule") as HtmlAnchor;
                    lnkSchedule.Visible = true;
                }
                else
                {
                    
                    var btnAuthorize = row.FindControl("lnkbtnAuthorize") as LinkButton;
                    btnAuthorize.Visible = true;

                    var btnDelete = row.FindControl("lnkbtnDelete") as LinkButton;
                    btnDelete.Visible = true;

                    HtmlAnchor lnkSchedule = row.FindControl("downloadschedule") as HtmlAnchor;
                    lnkSchedule.Visible = false;
                }
               
            }
        }

        protected void gvHomeEndorsement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
         
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlAnchor lnkSchedule = e.Row.FindControl("downloadschedule") as HtmlAnchor;
                var btnAuthorize = e.Row.FindControl("lnkbtnAuthorize") as LinkButton;
                var endorsementID = e.Row.FindControl("lblHomeEndorsementID") as Label;
                var DocumentNo = e.Row.FindControl("lblDocumentNo") as Label;
                var renewalCount = e.Row.FindControl("lblRenewalCount") as Label;

                long id = 0;
                if (endorsementID != null)
                {
                    id = Convert.ToInt64(endorsementID.Text);
                }
                lnkSchedule.HRef = ClientUtility.WebApiUri + BKIC.SellingPoint.DTO.Constants.ScheduleURI.downloadschedule
                                   .Replace("{insuranceType}", Constants.Home).Replace("{agentCode}", userInfo.AgentCode)
                                   .Replace("{documentNo}", DocumentNo.Text)
                                   .Replace("{isEndorsement}", "true")
                                   .Replace("{endorsementID}", id.ToString())
                                   .Replace("{renewalCount}", renewalCount.Text.Trim());

                bool IsActive = Convert.ToBoolean((e.Row.FindControl("lblIsActive") as Label).Text.Trim());
                if (IsActive)
                    lnkSchedule.Visible = true;
                else
                    lnkSchedule.Visible = false;
            }
        }

        public void ClearControls()
        {
            txtEffectiveFromDate.Text = string.Empty;
            txtEffectiveToDate.Text = string.Empty;
            txtOldClientCode.Text = string.Empty;
            txtOldInsuredName.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            premiumAmount.Text = string.Empty;
            premiumAmount1.Text = string.Empty;
            commission.Text = string.Empty;
            commission1.Text = string.Empty;
            btnSubmit.Visible = false;
            gvHomeEndorsement.DataSource = null;
            gvHomeEndorsement.DataBind();
            includeDisc.Visible = false;
            excludeDisc.Visible = false;
            Gridview1.DataSource = null;
            Gridview1.DataBind();
            ViewState["CurrentTable"] = null;
        }

        protected void Premium_Changed(object sender, EventArgs e)
        {
            try
            {
                //Akbari said don't need to calculate the commission for changed premium.

                //var userInfo = Session["UserInfo"] as OAuthTokenResponse;
                //if (userInfo == null)
                //{
                //    Response.Redirect("Login.aspx");
                //}
                //var service = master.GetService();

                adjustedPremium.Value = premiumAmount.Text.Trim();

                //Calculate VAT.
                var vatResponse = master.GetVat(string.IsNullOrEmpty(premiumAmount.Text) ? 0 : Convert.ToDecimal(premiumAmount.Text),
                                  string.IsNullOrEmpty(commission.Text) ? 0 : Convert.ToDecimal(commission.Text));

                if (vatResponse != null && vatResponse.IsTransactionDone)
                {
                    txtVATAmount.Text = Convert.ToString(vatResponse.VatAmount);
                    txtVATCommission.Text = Convert.ToString(vatResponse.VatCommissionAmount);
                    txtTotalPremium.Text = Convert.ToString(string.IsNullOrEmpty(premiumAmount.Text) ? 0 : Convert.ToDecimal(premiumAmount.Text) + vatResponse.VatAmount);
                    txtTotalCommission.Text = Convert.ToString(string.IsNullOrEmpty(commission.Text) ? 0 : Convert.ToDecimal(commission.Text) + vatResponse.VatCommissionAmount);
                }

                //var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeCommissionRequest();
                //commisionRequest.AgentCode = userInfo.AgentCode;
                //commisionRequest.Agency = userInfo.Agency;
                //commisionRequest.SubClass = subClass.Value;
                //commisionRequest.PremiumAmount = string.IsNullOrEmpty(premiumAmount.Text) ? decimal.Zero : Convert.ToDecimal(premiumAmount.Text);
                ////DomesticHelp Calculation not included in the SRCC, riot is always false.
                //commisionRequest.IsRoitAdded = false;

                ////Get commision for the endorsement premium.
                //var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                //                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeCommissionResponse>,
                //                       BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeCommissionRequest>("api/insurance/HomeCommission", commisionRequest);

                //if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone)
                //{
                //    var ToatlCommission = commissionresult.Result.BasicCommission + commissionresult.Result.SRCCCommission;
                //    adjustedCommission.Value = Convert.ToString(ToatlCommission);
                //    commission.Text = Convert.ToString(ToatlCommission);
                //}
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

        protected void Commission_Changed(object sender, EventArgs e)
        {
            try
            {
                adjustedCommission.Value = commission.Text.Trim();
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
                txtTotalPremium.Text = Convert.ToString(master.NearestOneHalf(totalPremium));
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
                txtTotalPremium1.Text = Convert.ToString(master.NearestOneHalf(totalPremium));
                txtTotalCommission1.Text = Convert.ToString(totalCommission);
            }
        }
    }
}