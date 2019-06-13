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
using System.Web.UI.WebControls;

namespace SellingPoint.Presentation
{
    public partial class HomeSystemRenewal : System.Web.UI.Page
    {
        public static long _HomeID = 0;
        public bool _isSavedQuestions = false;
        private static DataTable singleItemCategories;
        public static DataTable Nationalitydt;
        public static DataTable domesticHelpOccupationDt;
        public bool UserChangedPremium = false;
        public static List<InsuredMasterDetails> InsuredNames { get; set; }
        public static List<AgencyHomePolicy> policyList;
        public static bool AjdustedPremium { get; set; }
        public static DataTable Area;
        public static bool _policyFetched { get; set; }
        public static bool _questionarieChanged { get; set; }
        public static bool _calculatedSuccessful { get; set; }

        public static int _isMortgagedPreviousIndex { get; set; }
        public static int _isSafePropertyPreviousIndex { get; set; }
        public static int _jewelleryCoverPreviousIndex { get; set; }
        public static int _isRiotStrikePreviousIndex { get; set; }
        public static int _isJointOwenerPreviousIndex { get; set; }
        public static int _isAnyOtherTradePreviousIndex { get; set; }
        public static int _isAnyOtherInsurancePreviousIndex { get; set; }
        public static int _isSustainedPreviousIndex { get; set; }
        public static int _isAboveBDPreviousIndex { get; set; }

        //As of now home insurance have only one product, in future it may come
        public static string MainClass { get; set; }

        public static string SubClass { get; set; }

        public static int _RenewalCount { get; set; }

        private General master;

        public HomeSystemRenewal()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!IsPostBack)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                SetInitialRow();
                DisableDefaultControls(userInfo);
                BindDropdown(userInfo, service);
                LoadUsers(userInfo, service);
                LoadAgencyClientCode(userInfo, service);
                QueryStringMethods(userInfo, service);
            }
        }

        public override void Validate(string group)
        {
            base.Validate(group);

            // get the first validator that failed
            var validator = GetValidators(group).OfType<BaseValidator>().FirstOrDefault(v => !v.IsValid);

            // set the focus to the control
            // that the validator targets
            if (validator != null)
            {
                Control target = validator.NamingContainer.FindControl(validator.ControlToValidate);

                if (target != null)
                    target.Focus();
            }
        }

        protected void ddlHomePolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlHomePolicies.SelectedIndex == 0)
            //{
            //    master.ClearControls(GetContentControl());
            //    SetReadOnlyControls();
            //}
        }

        public void DisableDefaultControls(OAuthTokenResponse userInfo)
        {
            amtDisplay.Visible = false;
            downloadschedule.Visible = false;
            btnSubmit.Visible = false;
            btnAuthorize.Visible = false;
            _HomeID = 0;
            AjdustedPremium = false;
            txtIssueDate.Text = DateTime.Now.CovertToLocalFormat();
            _calculatedSuccessful = false;
            _questionarieChanged = false;
            _policyFetched = false;
            divPaymentSection.Visible = userInfo.IsShowPayments;
        }

        public void QueryStringMethods(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var includeHIR = Request.QueryString["IncludeHIR"];
            var cpr = Request.QueryString["CPR"];
            var insuredName = Request.QueryString["InsuredName"];
            var insuredCode = Request.QueryString["InsuredCode"];
            var policyNo = Request.QueryString["PolicyNo"];

            txtInsuredName.Text = insuredName != null ? Convert.ToString(insuredName) : string.Empty;
            txtClientCode.Text = insuredCode != null ? Convert.ToString(insuredCode) : string.Empty;

            LoadAgencyClientPolicyRenewal(userInfo, service, includeHIR != null ? Convert.ToBoolean(includeHIR) : false);

            if (cpr != null)
            {
                string CPR = Convert.ToString(cpr);
                //ddlCPR.SelectedIndex = ddlCPR.Items.IndexOf(ddlCPR.Items.FindByText(CPR));
                txtCPR.Text = CPR;
            }
            if (policyNo != null)
            {
                //ddlHomePolicies.SelectedIndex = ddlHomePolicies.Items.IndexOf(ddlHomePolicies.Items.FindByText
                //                                (Convert.ToString(policyNo)));
                //GetPolicyInfo();
            }
        }

        public void LoadUsers(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster();
            details.Type = "fetch";
            details.CreatedDate = DateTime.Now;

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

        protected void insured_Master(object sender, EventArgs e)
        {
            Response.Redirect("InsuredMaster.aspx?type=" + 3);
        }

        private void BindDropdown(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                 (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}",
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.HomeInsurance));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone == true)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable AgencyData = dropdownds.Tables["AgentCodeDD"];
                //DataTable propertyInsured = dropdownds.Tables["BK_PropertyInsured"];
                Nationalitydt = dropdownds.Tables["Nationality"];
                domesticHelpOccupationDt = dropdownds.Tables["DomesticWorkerOccupation"];

                ddlBranch.DataValueField = "Agency";
                ddlBranch.DataTextField = "AgentCode";
                ddlBranch.DataSource = AgencyData;
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, new ListItem("--Please Select--", ""));

                DataSet insuranceDDl = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable yesOrNo = insuranceDDl.Tables["BK_YesOrNo"];
                DataTable buildingAge = insuranceDDl.Tables["BK_BuildingAge"];
                DataTable residentialType = insuranceDDl.Tables["BK_ResidentialType"];
                //singleItemCategories = insuranceDDl.Tables["BK_HomeInsuranceCategory"];
                DataTable financier = insuranceDDl.Tables["MotorFinancier"];
                DataTable residenceType = insuranceDDl.Tables["BK_ResidentialType"];
                DataTable branches = dropdownds.Tables["BranchMaster"];
                DataTable area = dropdownds.Tables["AreaMaster"];
                Area = dropdownds.Tables["AreaMaster"];

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
                if (area != null && area.Rows.Count > 0)
                {
                    ddlArea.DataValueField = "Area";
                    ddlArea.DataTextField = "Area";
                    ddlArea.DataSource = ExtensionMethod.GetDistinctArea(area);
                    ddlArea.DataBind();
                    ddlArea.Items.Insert(0, new ListItem("--Please Select--", ""));
                    // ddlArea.Items[0].Selected = true;
                }

                //ddlBuildingAge.DataValueField = "Value";
                //ddlBuildingAge.DataTextField = "Text";
                //ddlBuildingAge.DataSource = buildingAge;
                //ddlBuildingAge.DataBind();
                //ddlBuildingAge.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlMortgaged.DataValueField = "Value";
                //ddlMortgaged.DataTextField = "Text";
                //ddlMortgaged.DataSource = yesOrNo;
                //ddlMortgaged.DataBind();
                //ddlMortgaged.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlFinancier.DataValueField = "Code";
                //ddlFinancier.DataTextField = "Financier";
                //ddlFinancier.DataSource = financier;
                //ddlFinancier.DataBind();
                //ddlFinancier.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlPropertyInsured.DataValueField = "Value";
                //ddlPropertyInsured.DataTextField = "Text";
                //ddlPropertyInsured.DataSource = yesOrNo;
                //ddlPropertyInsured.DataBind();
                //ddlPropertyInsured.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlMaliciousDamageCover.DataValueField = "Value";
                //ddlMaliciousDamageCover.DataTextField = "Text";
                //ddlMaliciousDamageCover.DataSource = yesOrNo;
                //ddlMaliciousDamageCover.DataBind();
                //ddlMaliciousDamageCover.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlPropertyJointOwnership.DataValueField = "Value";
                //ddlPropertyJointOwnership.DataTextField = "Text";
                //ddlPropertyJointOwnership.DataSource = yesOrNo;
                //ddlPropertyJointOwnership.DataBind();
                //ddlPropertyJointOwnership.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlConnectionWithAnyTrade.DataValueField = "Value";
                //ddlConnectionWithAnyTrade.DataTextField = "Text";
                //ddlConnectionWithAnyTrade.DataSource = yesOrNo;
                //ddlConnectionWithAnyTrade.DataBind();
                //ddlConnectionWithAnyTrade.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlInsuredCoverByOtherInsurance.DataValueField = "Value";
                //ddlInsuredCoverByOtherInsurance.DataTextField = "Text";
                //ddlInsuredCoverByOtherInsurance.DataSource = yesOrNo;
                //ddlInsuredCoverByOtherInsurance.DataBind();
                //ddlInsuredCoverByOtherInsurance.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlPropertyInsuredSustainedAnyLossOrDamage.DataValueField = "Value";
                //ddlPropertyInsuredSustainedAnyLossOrDamage.DataTextField = "Text";
                //ddlPropertyInsuredSustainedAnyLossOrDamage.DataSource = yesOrNo;
                //ddlPropertyInsuredSustainedAnyLossOrDamage.DataBind();
                //ddlPropertyInsuredSustainedAnyLossOrDamage.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlSingleItemAboveBD.DataValueField = "Value";
                //ddlSingleItemAboveBD.DataTextField = "Text";
                //ddlSingleItemAboveBD.DataSource = yesOrNo;
                //ddlSingleItemAboveBD.DataBind();
                //ddlSingleItemAboveBD.Items.Insert(0, new ListItem("--Please Select--", ""));

                //ddlBuildingType.DataValueField = "Value";
                //ddlBuildingType.DataTextField = "Text";
                //ddlBuildingType.DataSource = residenceType;
                //ddlBuildingType.DataBind();
                //ddlBuildingType.Items.Insert(0, new ListItem("--Please Select--", ""));

                ddlBanks.DataValueField = "Code";
                ddlBanks.DataTextField = "Financier";
                ddlBanks.DataSource = financier;
                ddlBanks.DataBind();
                ddlBanks.Items.Insert(0, new ListItem("--Please Select--", ""));

                ddlRequireDomesticHelpCover.DataValueField = "Value";
                ddlRequireDomesticHelpCover.DataTextField = "Text";
                ddlRequireDomesticHelpCover.DataSource = yesOrNo;
                ddlRequireDomesticHelpCover.DataBind();
                ddlRequireDomesticHelpCover.Items.Insert(0, new ListItem("--Please Select--", ""));
            }

            var productResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchProductCodeResponse>>(
                                BKIC.SellingPoint.DTO.Constants.DropDownURI.GetInsuranceProductCode
                                .Replace("{agency}", userInfo.Agency)
                                .Replace("{agencyCode}", userInfo.AgentCode)
                                .Replace("{insurancetypeid}", "3"));

            if (productResult != null && productResult.StatusCode == 200 && productResult.Result.IsTransactionDone)
            {
                var products = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>(
                               BKIC.SellingPoint.DTO.Constants.DropDownURI.GetAgencyProducts
                               .Replace("{agency}", userInfo.Agency)
                               .Replace("{agencyCode}", userInfo.AgentCode)
                               .Replace("{mainclass}", productResult.Result.productCode)
                               .Replace("{page}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.HomeInsurance));

                MainClass = productResult.Result.productCode;
                if (products != null && products.StatusCode == 200 && products.Result.IsTransactionDone)
                {
                    DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(products.Result.dropdownresult);
                    DataTable prods = dropdownds.Tables["Products"];

                    //In future product may be increase. Now it has only one product.
                    if (prods != null && prods.Rows.Count > 0)
                    {
                        SubClass = prods.Rows[0]["SubClass"].ToString();
                        //ddlCover.DataValueField = "SUBCLASS";
                        //ddlCover.DataTextField = "SUBCLASS";
                        //ddlCover.DataSource = prods;
                        //ddlCover.DataBind();
                        //ddlCover.Items.Insert(0, new ListItem("--Please Select--", ""));
                    }
                }
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void rtDetailedDomesticWorkers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        private void LoadAgencyClientCode(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest();
            req.AgentBranch = userInfo.AgentBranch;
            req.AgentCode = userInfo.AgentCode;
            //req.Agency = userInfo.;

            var homeResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest>
                               (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyInsured, req);

            if (homeResult.StatusCode == 200 && homeResult.Result.IsTransactionDone && homeResult.Result.AgencyInsured.Count > 0)
            {
                //ddlCPR.DataSource = homeResult.Result.AgencyInsured;
                //ddlCPR.DataTextField = "CPR";
                //ddlCPR.DataValueField = "InsuredCode";
                //ddlCPR.DataBind();
                //ddlCPR.Items.Insert(0, new ListItem("--Please Select--", ""));
                InsuredNames = homeResult.Result.AgencyInsured;
            }
            ddlUsers.SelectedIndex = ddlUsers.Items.IndexOf(ddlUsers.Items.FindByText(userInfo.UserName));
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(userInfo.AgentBranch));

            if (userInfo.Roles == "BranchAdmin" || userInfo.Roles == "User")
            {
                ddlUsers.Enabled = false;
            }
        }

        private void LoadAgencyClientPolicyRenewal(OAuthTokenResponse userInfo, DataServiceManager service, bool includeHIR = false)
        {
            var homereq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest();
            homereq.AgentCode = userInfo.AgentCode;
            homereq.Agency = userInfo.Agency;
            homereq.AgentBranch = userInfo.AgentBranch;
            homereq.IncludeHIR = includeHIR;
            homereq.IsRenewal = true;

            //Get PolicyNo by Agency
            var homePolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomePolicyResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest>
                              (BKIC.SellingPoint.DTO.Constants.HomeURI.GetHomeAgencyPolicy, homereq);

            if (homePolicies.StatusCode == 200 && homePolicies.Result.IsTransactionDone && homePolicies.Result.AgencyHomePolicies.Count > 0)
            {
                policyList = homePolicies.Result.AgencyHomePolicies;

                //ddlHomePolicies.DataSource = homePolicies.Result.AgencyHomePolicies;
                //ddlHomePolicies.DataTextField = "DOCUMENTNO";
                //ddlHomePolicies.DataValueField = "DOCUMENTRENEWALNO";
                //ddlHomePolicies.DataBind();
                //ddlHomePolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
            }
        }

        protected void ddlCPR_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    txtClientCode.Text = ddlCPR.SelectedItem.Value;
            //    txtCPR.Text = ddlCPR.SelectedItem.Text;

            //    if (InsuredNames != null && InsuredNames.Count > 0)
            //    {
            //        var insured = InsuredNames.Find(c => c.CPR == ddlCPR.SelectedItem.Text.Trim());
            //        if (insured != null)
            //        {
            //            txtInsuredName.Text = insured.FirstName + " " + insured.MiddleName + " " + insured.LastName;
            //        }
            //    }
            //    Page_CustomValidate();
            //    DisableControls();
            //}
            //catch (Exception ex)
            //{
            //    //throw ex;
            //}
            //finally
            //{
            //    master.ShowLoading = false;
            //}
        }

        protected void enable_Fields(object sender, EventArgs e)
        {
            try
            {
                EnableAddressfields();                
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

        public void EnableAddressfields()
        {
            if (ddlBuildingType.SelectedIndex == 0)
            {
                txtFlatNo.Text = string.Empty;
                txtFlatNo.Enabled = false;
                txtHouseNo.Text = string.Empty;
                txtHouseNo.Enabled = false;
                txtNoOfFloor.Text = string.Empty;
                txtNoOfFloor.Enabled = false;
                txtRoadNo.Text = string.Empty;
                txtRoadNo.Enabled = false;
                txtBlockNo.Text = string.Empty;
                txtBlockNo.Enabled = false;
                txtBuildingNo.Text = string.Empty;
                txtBuildingNo.Enabled = false;
                txtBuildingValue.Enabled = true;
                txtJewelleryValue.Enabled = true;
            }
            else if (ddlBuildingType.SelectedIndex == 1)
            {
                txtFlatNo.Text = string.Empty;
                txtFlatNo.Enabled = false;
                //txtHouseNo.Text = string.Empty;
                txtHouseNo.Enabled = true;
                txtNoOfFloor.Enabled = true;
                txtRoadNo.Enabled = true;
                txtBlockNo.Enabled = true;
                txtBuildingNo.Enabled = false;
                txtBuildingValue.Enabled = true;
                txtJewelleryValue.Enabled = true;
            }
            else if (ddlBuildingType.SelectedIndex == 2)
            {
               // txtFlatNo.Text = string.Empty;
                txtFlatNo.Enabled = true;
                txtHouseNo.Text = string.Empty;
                txtHouseNo.Enabled = false;
                txtNoOfFloor.Enabled = true;
                txtRoadNo.Enabled = true;
                txtBlockNo.Enabled = true;
                txtBuildingNo.Enabled = true;

                txtBuildingValue.Enabled = true;
                txtJewelleryValue.Enabled = true;
            }
            else if (ddlBuildingType.SelectedIndex == 3)
            {
                txtFlatNo.Enabled = true;
                txtHouseNo.Enabled = true;
                txtNoOfFloor.Enabled = true;
                txtRoadNo.Enabled = true;
                txtBlockNo.Enabled = true;
                txtBuildingNo.Enabled = true;

                txtBuildingValue.Enabled = false;
                txtJewelleryValue.Enabled = false;
            }
        }

        protected void calculate_expiredate(object sender, EventArgs e)
        {
            txtInsurancePeriodTo.Text = Convert.ToDateTime(txtInsurancePeriodFrom.Text.CovertToCustomDateTime())
                                        .AddYears(1).AddDays(-1).CovertToLocalFormat();            
        }

        protected void ddlPDomesticWorksCover_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlPDomesticWorksCover.SelectedIndex > 0)
            //{
            //    divDetailedDomesticWorkers.Visible = true;
            //    var NoOfHelpers = Convert.ToInt32(ddlPDomesticWorksCover.SelectedValue);

            //    if (ViewState["CurrentTable"] != null)
            //    {
            //        DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            //        if (NoOfHelpers < dtCurrentTable.Rows.Count)
            //        {
            //            for (int i = dtCurrentTable.Rows.Count; i > NoOfHelpers; i--)
            //            {
            //                dtCurrentTable.Rows[i - 1].Delete();
            //                //dtCurrentTable.Rows[i -1].AcceptChanges();
            //            }
            //            ViewState["CurrentTable"] = dtCurrentTable;
            //            Gridview1.DataSource = dtCurrentTable;
            //            Gridview1.DataBind();
            //        }

            //    }

            //    for (int i = 0; i < Convert.ToInt32(ddlPDomesticWorksCover.SelectedValue) - 1; i++)
            //    {
            //        AddNewRowToGrid();
            //    }

            //}
            //else
            //{
            //    divDetailedDomesticWorkers.Visible = false;
            //}
            DisableControls();
            master.ShowLoading = false;
        }

        public DataTable CreateDependents(int count)
        {
            DataTable domesticWorkerDetails = new DataTable();
            domesticWorkerDetails.Columns.Add("ITEMNAME", typeof(string));
            domesticWorkerDetails.Columns.Add("DATEOFBIRTH", typeof(string));
            domesticWorkerDetails.Columns.Add("CPR", typeof(string));
            domesticWorkerDetails.Columns.Add("Count", typeof(Int32));
            for (int i = 1; i <= count; i++)
            {
                domesticWorkerDetails.Rows.Add("", "", "", i);
            }

            return domesticWorkerDetails;
        }

        protected void ddlRequireDomesticHelpCover_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRequireDomesticHelpCover.SelectedIndex > 0 && ddlRequireDomesticHelpCover.SelectedValue.ToLower() != "no")
                {
                    SetInitialRow();
                    mainDomesticWorker.Visible = true;
                    divDetailedDomesticWorkers.Visible = true;
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
                }
                else
                {
                    HidePremium(true);
                    ResetDependant();
                    // divNoOfDomesticWorkersDetails.Visible = false;
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

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewRowToGrid();
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

            //dt.Columns.Add(new DataColumn("Flat No", typeof(string)));
            //dt.Columns.Add(new DataColumn("Building No", typeof(string)));
            //dt.Columns.Add(new DataColumn("Block No", typeof(string)));

            //dt.Columns.Add(new DataColumn("Road No", typeof(string)));
            //dt.Columns.Add(new DataColumn("Town", typeof(string)));

            dr = dt.NewRow();

            //dr["RowNumber"] = 1;
            dr["ITEMNAME"] = string.Empty;
            dr["SEX"] = string.Empty;
            dr["DATEOFBIRTH"] = string.Empty;

            dr["NATIONALITY"] = string.Empty;
            dr["CPR"] = string.Empty;
            dr["OCCUPATION"] = string.Empty;

            //dr["Flat No"] = string.Empty;
            //dr["Building No"] = string.Empty;
            //dr["Block No"] = string.Empty;

            //dr["Road No"] = string.Empty;
            //dr["Town"] = string.Empty;

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
                            dtCurrentTable.Rows[i - 1]["SEX"] = ddlSex.SelectedItem.Text;
                            dtCurrentTable.Rows[i - 1]["DATEOFBIRTH"] = txDOB.Text;

                            dtCurrentTable.Rows[i - 1]["NATIONALITY"] = ddlNationality.SelectedItem.Text;
                            dtCurrentTable.Rows[i - 1]["CPR"] = txPassPort.Text;
                            dtCurrentTable.Rows[i - 1]["OCCUPATION"] = ddlOccupation.SelectedItem.Text;

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
                    //throw ex;
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            //Set Previous Data on Postbacks
            SetPreviousData();
            DisableControls();
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
                        ddlSex.SelectedIndex = ddlSex.Items.IndexOf(ddlSex.Items.FindByText(dt.Rows[i]["SEX"].ToString())) > 0 ?
                                               ddlSex.Items.IndexOf(ddlSex.Items.FindByText(dt.Rows[i]["SEX"].ToString())) :
                                               ddlSex.Items.IndexOf(ddlSex.Items.FindByValue(dt.Rows[i]["SEX"].ToString()));
                        if (ddlSex.SelectedIndex == 0)
                        {
                            ddlSex.SelectedIndex = dt.Rows[i]["SEX"].ToString() == "F" ? 1 : 2;
                        }
                        txDOB.Text = dt.Rows[i]["DATEOFBIRTH"].ToString();
                        txPassPort.Text = dt.Rows[i]["CPR"].ToString();

                        ddlNationality.SelectedIndex = ddlNationality.Items.IndexOf(ddlNationality.Items.FindByText(dt.Rows[i]["NATIONALITY"].ToString())) > 0 ?
                                                       ddlNationality.Items.IndexOf(ddlNationality.Items.FindByText(dt.Rows[i]["NATIONALITY"].ToString())) :
                                                       ddlNationality.Items.IndexOf(ddlNationality.Items.FindByValue(dt.Rows[i]["NATIONALITY"].ToString()));

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

        public void EnableAuthorize(bool isHIR, int HIRStatus)
        {
            if (isHIR && HIRStatus != 8)
                btnAuthorize.Visible = false;
            else
                btnAuthorize.Visible = true;
        }

        protected void btnSavePopUp(object sender, EventArgs e)
        {
            if (!QuestionsAnswered())
            {
                master.ShowErrorPopup("Please answer the all the questions before caluculate !!", "Answer All Questions");
                return;
            }
            if (!IsEligible())
            {
                return;
            }
            else
            {
                SavedQuetionsChanged();
                _isSavedQuestions = true;
            }
        }

        protected void btnCancelPopUp(object sender, EventArgs e)
        {
            ddlMortgaged.SelectedIndex = -1;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                EnablePaymentValidator();
                Page.Validate();               
                if (Page.IsValid)
                {
                    SaveAuthorize(true);
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

        protected void btnInsuredDetail_Click(object sender, EventArgs e)
        {
            //Response.Redirect("");
        }

        protected void btnInsuredPage_Click(object source, EventArgs args)
        {
        }

        protected void btnAuthorize_Click(object sender, EventArgs e)
        {
            try
            {
                EnablePaymentValidator();
                Page.Validate();               
                if (Page.IsValid)
                {
                    Reset();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowPopup();", true);
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

        protected void Auth(object sender, EventArgs e)
        {
            try
            {
                EnablePaymentValidator();
                Page.Validate();               
                if (Page.IsValid)
                {
                    SaveAuthorize(false);
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Homepage.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Panel1.Visible = true;
            }
        }

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentMethod.SelectedIndex == 1)
            {
                txtAccountNo.Text = "";
                txtAccountNo.Enabled = false;
            }
            else
            {
                txtAccountNo.Enabled = true;
            }            
        }

        protected void Calculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    if (!QuestionsAnswered())
                    {
                        master.ShowErrorPopup("Please answer the all the questions before caluculate !!", "Answer All Questions");
                        return;
                    }                

                    var homeQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuote
                    {
                        BuildingValue = !string.IsNullOrEmpty(txtBuildingValue.Text.Trim()) ? Convert.ToDecimal(txtBuildingValue.Text.Trim()) : 0,
                        ContentValue = !string.IsNullOrEmpty(txtContentValue.Text.Trim()) ? Convert.ToDecimal(txtContentValue.Text.Trim()) : 0,
                        JewelleryValue = !string.IsNullOrEmpty(txtJewelleryValue.Text.Trim()) ? Convert.ToDecimal(txtJewelleryValue.Text.Trim()) : 0,
                        IsPropertyToBeInsured = ddlPropertyInsured.SelectedIndex > 0 ? (ddlPropertyInsured.SelectedIndex == 1 ? true : false) : false,
                        IsRiotStrikeAdded = ddlMaliciousDamageCover.SelectedIndex > 0 ? (ddlMaliciousDamageCover.SelectedIndex == 1 ? true : false) : false,
                        JewelleryCover = ddlJewelleryCoverWithinContents.SelectedItem.Value,
                        NumberOfDomesticWorker = Gridview1.Rows.Count,
                        Agency = userInfo.Agency,
                        AgentCode = userInfo.AgentCode,
                        MainClass = MainClass,
                        SubClass = SubClass,//ddlCover.SelectedItem.Value.Trim();
                        RenewalDelayedDays = string.IsNullOrEmpty(actualRenewalDate.Value) ? 0 : (int)(txtInsurancePeriodFrom.Text.CovertToCustomDateTime() - actualRenewalDate.Value.CovertToCustomDateTime()).TotalDays

                    };

                    var homeQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                          <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuoteResponse>,
                                           BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuote>
                                           (BKIC.SellingPoint.DTO.Constants.HomeURI.GetQuote, homeQuote);

                    //Home insurence calculated commission as the time of calculating the premium.
                    if (homeQuoteResult.StatusCode == 200 && homeQuoteResult.Result != null && homeQuoteResult.Result.IsTransactionDone)
                    {
                        _calculatedSuccessful = true;
                        calculatedPremium.Value = Convert.ToString(homeQuoteResult.Result.TotalPremium);
                        calculatedCommision.Value = Convert.ToString(homeQuoteResult.Result.TotalCommission);
                        ShowPremium(userInfo, homeQuoteResult.Result.TotalPremium, homeQuoteResult.Result.TotalCommission);

                        //Calculate VAT.
                        var vatResponse = master.GetVat(homeQuoteResult.Result.TotalPremium, homeQuoteResult.Result.TotalCommission);
                        if (vatResponse != null && vatResponse.IsTransactionDone)
                        {
                            decimal TotalPremium = homeQuoteResult.Result.TotalPremium + vatResponse.VatAmount;
                            decimal TotalCommission = homeQuoteResult.Result.TotalCommission + vatResponse.VatCommissionAmount;
                            ShowVAT(userInfo, vatResponse.VatAmount, vatResponse.VatCommissionAmount, TotalPremium, TotalCommission);
                        }
                    }
                    else
                    {
                        master.ShowLoading = false;
                        master.ShowErrorPopup(homeQuoteResult.Result.TransactionErrorMessage, "Request Failed !");
                        return;
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

        public bool QuestionsAnswered()
        {
            if (ddlConnectionWithAnyTrade.SelectedIndex == 0)
                return false;
            if (ddlInsuredCoverByOtherInsurance.SelectedIndex == 0)
                return false;
            if (ddlJewelleryCoverWithinContents.SelectedIndex == 0)
                return false;
            if (ddlMaliciousDamageCover.SelectedIndex == 0)
                return false;
            if (ddlMortgaged.SelectedIndex == 0)
                return false;
            if (ddlPropertyInsured.SelectedIndex == 0)
                return false;
            if (ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedIndex == 0)
                return false;
            if (ddlPropertyJointOwnership.SelectedIndex == 0)
                return false;
            if (ddlSingleItemAboveBD.SelectedIndex == 0)
                return false;
            else
                return true;
        }

        public bool IsEligible()
        {
            bool isValid = true;
            if (ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedIndex == 1)
            {
                btnCalculate.Visible = false;
                btnAuthorize.Visible = false;
                btnSubmit.Visible = false;
                master.ShowErrorPopup("Has the property to be insured sustained any loss or damage (whether covered by insurance or not) during the last 5 years?", "Can't Issue a policy");
                isValid = false;
            }
            if (ddlConnectionWithAnyTrade.SelectedIndex == 1)
            {
                btnCalculate.Visible = false;
                btnAuthorize.Visible = false;
                btnSubmit.Visible = false;
                master.ShowErrorPopup("Is the insured property used in connection with any trade, business or profession?", "Can't Issue a policy");
                isValid = false;
            }
            else
            {
                btnCalculate.Visible = true;
            }
            return isValid;
        }

        public void SavedQuetionsChanged()
        {
            if (_questionarieChanged)
            {
                //Need to calculate the premium again

                HidePremium(true);
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
                    btnSave.Enabled = true;
                    btnAuthorize.Enabled = true;
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
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void btnPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    //if (ddlHomePolicies.SelectedIndex > 0)
                    if(!string.IsNullOrEmpty(txtHomeRenewalPolicySearch.Text))
                    {
                        GetPolicyInfo();
                    }
                    else
                    {
                        master.ClearControls(GetContentControl());
                        SetReadOnlyControls();
                        HidePremium(false);
                        ResetJoint();
                        ResetBank();
                        ResetInsurar();
                        ResetSingleItem();
                        ResetDependant();
                        ResetQuestions();
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

        public void GetPolicyInfo()
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var renewalCount = GetPolicyRenewalCount(userInfo, service);

                //If the policy is already renewed show to the information to the user.
                RenewalPreCheck(userInfo, service, renewalCount);

                //Get saved policy details by document(policy) number.
                //var docNo = ddlHomePolicies.SelectedItem.Text.Trim();
                //var policyRenewalCount = ddlHomePolicies.SelectedItem.Value.Substring(0, ddlHomePolicies.SelectedValue.IndexOf("-", 0));
                //var policyRenewalCount = string.IsNullOrEmpty(renewalCount.Value) ? Convert.ToString(0) : renewalCount.Value;


                var url = BKIC.SellingPoint.DTO.Constants.HomeURI.GetHomeRenewalPolicyByDocNo
                        .Replace("{documentNo}", txtHomeRenewalPolicySearch.Text.Trim())
                        .Replace("{type}", "portal")
                        .Replace("{agentCode}", userInfo.AgentCode)
                        .Replace("{renewalCount}", renewalCount.ToString());

                var homeDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (homeDetails.StatusCode == 200 && homeDetails.Result.IsTransactionDone)
                {
                    Update(userInfo, homeDetails);
                }
                else
                {
                    master.ShowErrorPopup(homeDetails.Result.TransactionErrorMessage, "Request Failed !");
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

        public int GetPolicyRenewalCount(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            int renewalCount = 0;
            var homereq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest();
            homereq.AgentCode = userInfo.AgentCode;
            homereq.Agency = userInfo.Agency;
            homereq.AgentBranch = userInfo.AgentBranch;
            homereq.IncludeHIR = false;
            homereq.IsRenewal = true;
            homereq.DocumentNo = txtHomeRenewalPolicySearch.Text.Trim();

            //Get PolicyNo by Agency
            var homePolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomePolicyResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest>
                              (BKIC.SellingPoint.DTO.Constants.HomeURI.GetHomeAgencyPolicy, homereq);

            if (homePolicies.StatusCode == 200 && homePolicies.Result.IsTransactionDone
                && homePolicies.Result.AgencyHomePolicies.Count > 0)
            {
                renewalCount = homePolicies.Result.AgencyHomePolicies[0].RenewalCount;
            }
            return renewalCount;
        }


        public void RenewalPreCheck(OAuthTokenResponse userInfo, DataServiceManager service, int renewalCount)
        {
            //var policyRenewalCount = string.IsNullOrEmpty(renewalCount.Value) ? Convert.ToString(0) : renewalCount.Value;

            var preCheckReq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.RenewalPrecheckRequest();

            preCheckReq.InsuranceType = BKIC.SellingPoint.DL.Constants.Insurance.Home;
            preCheckReq.Agency = userInfo.Agency;
            preCheckReq.AgentCode = userInfo.AgentCode;
            preCheckReq.DocumentNo = txtHomeRenewalPolicySearch.Text.Trim();
            preCheckReq.CurrentRenewalCount = Convert.ToInt32(renewalCount);


            var checkResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.RenewalPrecheckResponse>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.RenewalPrecheckRequest>
                             (BKIC.SellingPoint.DTO.Constants.AdminURI.RenewalPrecheckByInsuranceType, preCheckReq);

            if (checkResult.StatusCode == 200 && checkResult.Result.IsTransactionDone)
            {
                if (checkResult.Result.IsAlreadyRenewed)
                {
                    master.ShowErrorPopup("This policy is already renewed", "Renewal");
                }
            }

        }

        private void Update(OAuthTokenResponse userInfo, ApiResponseWrapper<HomeSavedQuotationResponse> homeDetails)
        {
            var res = homeDetails.Result.HomeInsurancePolicy;
            _HomeID = res.HomeID;
            _RenewalCount = res.RenewalCount;         

            txtClientCode.Text = res.InsuredCode;
            txtHomeRenewalPolicySearch.Text = res.DocumentNo;          
            txtCPR.Text = res.CPR;          
            UpdateQuestoinareIndex(res);
            txtBlockNo.Text = res.BlockNo;
            txtBuildingNo.Text = res.BuildingNo;
            txtBuildingValue.Text = Convert.ToString(res.BuildingValue);
            txtClientCode.Text = res.InsuredCode;
            txtContentValue.Text = Convert.ToString(res.ContentValue);
            txtJewelleryValue.Text = Convert.ToString(res.JewelleryValue);
            txtTotalSumInsured.Text = Convert.ToString(res.BuildingValue + res.ContentValue + res.JewelleryValue);            
            txtInsuredName.Text = res.InsuredName;
            txtRemarks.Text = res.IsSavedRenewal ? res.Remarks : string.Empty;
            txtAccountNo.Text = res.IsSavedRenewal ? res.AccountNumber : string.Empty;
            txtRoadNo.Text = res.RoadNo;
            txtHouseNo.Text = res.HouseNo;
            txtFlatNo.Text = res.FlatNo;
            txtNoOfFloor.Text = res.NoOfFloors.ToString();
            ddlBuildingType.SelectedIndex = ddlBuildingType.Items.IndexOf(ddlBuildingType.Items.FindByValue(res.BuildingType.ToString()));
            ddlArea.SelectedIndex = ddlArea.Items.IndexOf(ddlArea.Items.FindByText(res.Area));
            UpdateDomesticHelp(homeDetails.Result.DomesticHelp);
            UpdateHomeSubItems(homeDetails.Result.homesubitems);
            if (res.BuildingType > 0)
            {
                EnableAddressfields();
            }
            SetRenewalDate(res);
            SetRenewalData(userInfo, res);
            UpdateJewelleryCover();
        }

        private void SetRenewalData(OAuthTokenResponse userInfo, HomeInsurancePolicy res)
        {
            if (res.IsSavedRenewal)
            {
                if (res.PremiumBeforeDiscount - res.PremiumAfterDiscount > 0)
                {
                    calculatedPremium.Value = Convert.ToString(res.PremiumBeforeDiscount);
                    calculatedCommision.Value = Convert.ToString(res.CommisionBeforeDiscount);
                    AjdustedPremium = true;
                }
                else
                {
                    calculatedPremium.Value = Convert.ToString(res.PremiumAfterDiscount);
                    calculatedCommision.Value = Convert.ToString(res.CommissionAfterDiscount);
                }
                ShowPremium(userInfo, res.PremiumAfterDiscount, res.CommissionAfterDiscount);
                ShowVAT(userInfo, res.TaxOnPremium, res.TaxOnCommission, (res.PremiumAfterDiscount + res.TaxOnPremium), (res.CommissionAfterDiscount + res.TaxOnCommission));

                ShowDiscount(userInfo, res);
                EnableAuthorize(res.IsHIR, res.HIRStatus);
                if (res.IsActivePolicy)
                {
                    SetScheduleHRef(txtHomeRenewalPolicySearch.Text.Trim(), Constants.Home, userInfo, res.RenewalCount);
                    //If it is authorized policy need to disable all the page controls.
                    master.makeReadOnly(GetContentControl(), false);
                }
                else
                {
                    RemoveScheduleHRef();
                    master.makeReadOnly(GetContentControl(), true);
                }
                SetReadOnlyControls();
                GetPreviousQuestionsIndexs();
                _policyFetched = true;
                if (res.BuildingType > 0 && res.IsSaved)
                {
                    EnableAddressfields();
                }
            }
            else
            {
                _HomeID = 0;
            }
        }

        private void SetRenewalDate(HomeInsurancePolicy res)
        {           
            if(!res.IsSavedRenewal)
            {
                if(res.PolicyExpiryDate < DateTime.Now.Date)
                {
                    txtInsurancePeriodFrom.Text = DateTime.Now.Date.CovertToLocalFormat();
                }
                else
                {
                    txtInsurancePeriodFrom.Text = res.PolicyExpiryDate.AddDays(1).CovertToLocalFormat();
                }
                txtInsurancePeriodTo.Text = res.PolicyExpiryDate.AddYears(1).CovertToLocalFormat();
                actualRenewalDate.Value = Convert.ToString(res.PolicyExpiryDate.AddDays(1).CovertToLocalFormat());
            }
            else
            {
                txtInsurancePeriodFrom.Text = DateTime.Now.Date.CovertToLocalFormat();
                txtInsurancePeriodTo.Text = res.PolicyExpiryDate.AddYears(1).CovertToLocalFormat();
                actualRenewalDate.Value = Convert.ToString(res.ActualRenewalStartDate.ConvertToLocalFormat());
            }            
        }

        private void UpdateHomeSubItems(List<HomeSubItems> homeItemList)
        {
            if (homeItemList != null && homeItemList.Count > 0)
            {
                mainAboveBD.Visible = true;
                divSingleItemAboveBD.Visible = true;

                DataTable singleItemDt = new DataTable();
                singleItemDt.Columns.Add("Category", typeof(string));
                singleItemDt.Columns.Add("MoreDescription", typeof(string));
                singleItemDt.Columns.Add("AmountOfItem", typeof(string));
                singleItemDt.Columns.Add("SingleItemCVStatus", typeof(bool));
                singleItemDt.Columns.Add("SingleItemCVErrorMessage", typeof(string));
                singleItemDt.Columns.Add("Index", typeof(Int32));

                int count = 0;
                foreach (var i in homeItemList)
                {
                    singleItemDt.Rows.Add(i.SubItemName, i.Description, i.SumInsured, false, "", count);
                    count++;
                }
                rtSingleItemAboveBD.DataSource = singleItemDt;
                rtSingleItemAboveBD.DataBind();
            }
            else
            {
                mainAboveBD.Visible = false;
                divSingleItemAboveBD.Visible = false;
            }
        }

        private void UpdateDomesticHelp(List<HomeDomesticHelp> domesticHelpDetails)
        {
            if (domesticHelpDetails != null && domesticHelpDetails.Count > 0)
            {

                /*If they did some endorsemnts for the policy(add the domestic helper to the policy), at the time of endorsment
                  the questionarie not updated here we check that the policy have the domestic helper select the question to YES */
                ddlRequireDomesticHelpCover.SelectedIndex = 1;

                SetInitialRow();
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                Gridview1.DataSource = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = dtCurrentTable.Rows.Count; i > 0; i--)
                    {
                        dtCurrentTable.Rows[i - 1].Delete();
                        dtCurrentTable.AcceptChanges();
                    }
                }
                DataRow drCurrentRow = null;
                for (int i = 0; i < domesticHelpDetails.Count; i++)
                {
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);

                    dtCurrentTable.Rows[i]["ITEMNAME"] = domesticHelpDetails[i].Name;
                    dtCurrentTable.Rows[i]["SEX"] = domesticHelpDetails[i].Sex;
                    dtCurrentTable.Rows[i]["DATEOFBIRTH"] = domesticHelpDetails[i].DOB.CovertToLocalFormat();

                    dtCurrentTable.Rows[i]["NATIONALITY"] = domesticHelpDetails[i].Nationality;
                    dtCurrentTable.Rows[i]["CPR"] = domesticHelpDetails[i].CPR;
                    dtCurrentTable.Rows[i]["OCCUPATION"] = domesticHelpDetails[i].Occupation;
                }
                ViewState["CurrentTable"] = dtCurrentTable;
                Gridview1.DataSource = dtCurrentTable;
                Gridview1.DataBind();
                SetPreviousData();
                mainDomesticWorker.Visible = true;
                divDetailedDomesticWorkers.Visible = true;
            }
            else
            {
                ViewState["CurrentTable"] = null;
                Gridview1.DataSource = null;
                Gridview1.DataBind();
                mainDomesticWorker.Visible = false;
                divDetailedDomesticWorkers.Visible = false;
            }
        }

        private void UpdateQuestoinareIndex(HomeInsurancePolicy res)
        {
            ddlBuildingAge.SelectedIndex = ddlBuildingAge.Items.IndexOf(ddlBuildingAge.Items.FindByText(res.BuildingAge.ToString()));
            ddlJewelleryCoverWithinContents.SelectedIndex = ddlJewelleryCoverWithinContents.Items.IndexOf(ddlJewelleryCoverWithinContents.Items.FindByValue(res.JewelleryCoverType));
            ddlMaliciousDamageCover.SelectedIndex = ddlMaliciousDamageCover.Items.IndexOf(ddlMaliciousDamageCover.Items.FindByText(res.IsRiotStrikeDamage.ToString().ToLower() == "y" ? "Yes" : "No"));
            ddlMortgaged.SelectedIndex = ddlMortgaged.Items.IndexOf(ddlMortgaged.Items.FindByText(res.IsPropertyMortgaged.ToString().ToLower() == "y" ? "Yes" : "No"));
            ddlPaymentMethod.SelectedIndex = ddlPaymentMethod.Items.IndexOf(ddlPaymentMethod.Items.FindByText(res.PaymentType));
            ddlRequireDomesticHelpCover.SelectedIndex = ddlRequireDomesticHelpCover.Items.IndexOf(ddlRequireDomesticHelpCover.Items.FindByText(res.IsRequireDomestic.ToString().ToLower() == "y" ? "Yes" : "No"));
            ddlPropertyInsured.SelectedIndex = ddlPropertyInsured.Items.IndexOf(ddlPropertyInsured.Items.FindByText(res.IsSafePropertyInsured.ToString().ToLower() == "y" ? "Yes" : "No"));
            ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedIndex = ddlPropertyInsuredSustainedAnyLossOrDamage.Items.IndexOf(ddlPropertyInsuredSustainedAnyLossOrDamage.Items.FindByText(res.IsPropertyInsuredSustainedAnyLoss.ToString().ToLower() == "y" ? "Yes" : "No"));
            ddlPropertyJointOwnership.SelectedIndex = ddlPropertyJointOwnership.Items.IndexOf(ddlPropertyJointOwnership.Items.FindByText(res.IsJointOwnership.ToString().ToLower() == "y" ? "Yes" : "No"));
            ddlSingleItemAboveBD.SelectedIndex = ddlSingleItemAboveBD.Items.IndexOf(ddlSingleItemAboveBD.Items.FindByText(res.IsSingleItemAboveContents.ToString().ToLower() == "y" ? "Yes" : "No"));
            ddlConnectionWithAnyTrade.SelectedIndex = ddlConnectionWithAnyTrade.Items.IndexOf(ddlConnectionWithAnyTrade.Items.FindByText(res.IsPropertyInConnectionTrade.ToString().ToLower() == "y" ? "Yes" : "No"));
            ddlInsuredCoverByOtherInsurance.SelectedIndex = ddlInsuredCoverByOtherInsurance.Items.IndexOf(ddlInsuredCoverByOtherInsurance.Items.FindByText(res.IsPropertyCoveredOtherInsurance.ToString().ToLower() == "y" ? "Yes" : "No"));

            if (res.IsJointOwnership.ToString().ToLower() == "y")
            {
                divJointOwner.Visible = true;
                txtJointOwnerName.Text = string.IsNullOrEmpty(res.JointOwnerName) ? string.Empty : res.JointOwnerName;
            }
            if (res.IsPropertyMortgaged.ToString().ToLower() == "y")
            {
                divBankName.Visible = true;
                ddlBanks.SelectedIndex = ddlBanks.Items.IndexOf(ddlBanks.Items.FindByValue(res.FinancierCode));
            }
            if (res.IsPropertyCoveredOtherInsurance.ToString().ToLower() == "y")
            {
                divInsurar.Visible = true;
                txtNameSeekingReasons.Text = res.NamePolicyReasonSeekingReasons;
            }
        }

        private void SetDomesticHelp(HomeInsurancePolicyDetails homePolicy)
        {
            List<HomeDomesticHelp> homedomesticList = new List<HomeDomesticHelp>();

            if (ddlRequireDomesticHelpCover.SelectedItem.Value.ToLower() == "yes")
            {
                DataTable homedomesticdt = GetDomesticWorkers();
                if (homedomesticdt != null && homedomesticdt.Rows.Count > 0)
                {
                    homedomesticList = (from DataRow dr in homedomesticdt.Rows
                                        select new HomeDomesticHelp
                                        {
                                            MemberSerialNo = Convert.ToInt32(dr["Count"]),
                                            Name = Convert.ToString(dr["ITEMNAME"]),
                                            DOB = Convert.ToString(dr["DATEOFBIRTH"]).CovertToCustomDateTime(),
                                            CPR = Convert.ToString(dr["CPR"]),
                                            Nationality = Convert.ToString(dr["Nationality"]),
                                            Sex = Convert.ToChar(dr["Sex"].ToString()[0]),
                                            Occupation = Convert.ToString(dr["Occupation"])
                                        }).ToList();
                }
            }
            homePolicy.HomeDomesticHelpList = homedomesticList;
        }

        private void SetHomeSubItems(HomeInsurancePolicyDetails homePolicy)
        {
            DataTable subitemsdt = GetSingleItems();
            List<HomeSubItems> homesubitemsList = new List<HomeSubItems>();
            if (ddlSingleItemAboveBD.SelectedItem.Value.ToLower() == "yes" && subitemsdt != null && subitemsdt.Rows.Count > 0)
            {
                homesubitemsList = (from DataRow dr in subitemsdt.Rows
                                    select new HomeSubItems
                                    {
                                        SubItemCode = Convert.ToString(dr["Category"]),
                                        SubItemName = Convert.ToString(dr["Category"]),
                                        Description = Convert.ToString(dr["MoreDescription"]),
                                        SubItemSerialNo = Convert.ToInt32(dr["Index"]),
                                        SumInsured = Convert.ToDecimal(dr["AmountOfItem"])
                                    }).ToList();
            }
            homePolicy.HomeSubItemsList = homesubitemsList;
        }

        private void SetQuestionaire(HomeInsurancePolicyDetails homepolicy)
        {
            homepolicy.HomeInsurancePolicy.IsPropertyMortgaged = ddlMortgaged.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.IsSafePropertyInsured = ddlPropertyInsured.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.JewelleryCover = ddlJewelleryCoverWithinContents.SelectedItem.Value;
            homepolicy.HomeInsurancePolicy.IsRiotStrikeDamage = ddlMaliciousDamageCover.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.IsJointOwnership = ddlPropertyJointOwnership.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.IsPropertyInConnectionTrade = ddlConnectionWithAnyTrade.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.IsPropertyCoveredOtherInsurance = ddlInsuredCoverByOtherInsurance.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.IsPropertyInsuredSustainedAnyLoss = ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.IsPropertyUndergoingConstruction = ddlInsuredCoverByOtherInsurance.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.IsSingleItemAboveContents = ddlSingleItemAboveBD.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';
            homepolicy.HomeInsurancePolicy.IsRequireDomestic = ddlRequireDomesticHelpCover.SelectedItem.Value.ToLower() == "yes" ? 'Y' : 'N';

            if (ddlPropertyJointOwnership.SelectedItem.Value.ToLower() == "yes")
            {
                homepolicy.HomeInsurancePolicy.JointOwnerName = string.IsNullOrEmpty(txtJointOwnerName.Text) ? string.Empty : txtJointOwnerName.Text.Trim();
            }
            if (ddlMortgaged.SelectedItem.Value.ToLower() == "yes")
            {
                homepolicy.HomeInsurancePolicy.FinancierCode = ddlBanks.SelectedItem.Text.Trim();
            }
            if (ddlInsuredCoverByOtherInsurance.SelectedItem.Value.ToLower() == "yes")
            {
                homepolicy.HomeInsurancePolicy.NamePolicyReasonSeekingReasons = txtNameSeekingReasons.Text.Trim();
            }
        }

        public void GetPreviousQuestionsIndexs()
        {
            _isMortgagedPreviousIndex = ddlMortgaged.SelectedIndex;
            _isSafePropertyPreviousIndex = ddlPropertyInsured.SelectedIndex;
            _jewelleryCoverPreviousIndex = ddlJewelleryCoverWithinContents.SelectedIndex;
            _isRiotStrikePreviousIndex = ddlMaliciousDamageCover.SelectedIndex;
            _isJointOwenerPreviousIndex = ddlPropertyJointOwnership.SelectedIndex;
            _isAnyOtherTradePreviousIndex = ddlConnectionWithAnyTrade.SelectedIndex;
            _isAnyOtherInsurancePreviousIndex = ddlInsuredCoverByOtherInsurance.SelectedIndex;
            _isSustainedPreviousIndex = ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedIndex;
            _isAboveBDPreviousIndex = ddlSingleItemAboveBD.SelectedIndex;
        }

        private DataTable GetDomesticWorkers()
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
                domesticWorkerDetails.Rows.Add(obj.Name, dob, obj.Passport, obj.Sex, obj.Nationality, obj.Occupation, count);
            }

            return domesticWorkerDetails;
        }

        public void ClearHiddenValueItems()
        {
            ddlConnectionWithAnyTrade.SelectedIndex = 0;
            ddlInsuredCoverByOtherInsurance.SelectedIndex = 0;
            ddlJewelleryCoverWithinContents.SelectedIndex = 0;
            ddlMaliciousDamageCover.SelectedIndex = 0;
            ddlMortgaged.SelectedIndex = 0;
            ddlPropertyInsured.SelectedIndex = 0;
            ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedIndex = 0;
            ddlPropertyJointOwnership.SelectedIndex = 0;
            ddlRequireDomesticHelpCover.SelectedIndex = 0;
            ddlSingleItemAboveBD.SelectedIndex = 0;
        }

        public void SaveAuthorize(bool isSave)
        {
            try
            {    
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                if (!QuestionsAnswered())
                {
                    master.ShowErrorPopup("Please answer the all the questions before caluculate !!", "Answer All Questions");
                    return;
                }               

                int RenewalDelayedDays = string.IsNullOrEmpty(actualRenewalDate.Value) ? 0 : (int)(txtInsurancePeriodFrom.Text.CovertToCustomDateTime() - actualRenewalDate.Value.CovertToCustomDateTime()).TotalDays;

                var homeQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuote();
                var insured = new BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest();
                var homepolicy = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsurancePolicyDetails();

                //Get Insured Person details
                insured.CPR = txtCPR.Text.Trim();
                insured.InsuredCode = txtClientCode.Text.Trim();
                insured.Agency = userInfo.Agency;
                insured.AgentCode = userInfo.AgentCode;

                var insuredDetails = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                     <BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredResponse>,
                                     BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest>
                                    (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchUserDetailsByCPRInsuredCode, insured);

                if (insuredDetails.StatusCode == 200 && insuredDetails.Result.IsTransactionDone)
                {
                    homepolicy.HomeInsurancePolicy.InsuredCode = insuredDetails.Result.InsuredDetails.InsuredCode ?? string.Empty;
                    homepolicy.HomeInsurancePolicy.InsuredName = string.Concat(insuredDetails.Result.InsuredDetails.FirstName, " ", insuredDetails.Result.InsuredDetails.MiddleName, " ", insuredDetails.Result.InsuredDetails.LastName);
                    homepolicy.HomeInsurancePolicy.Mobile = insuredDetails.Result.InsuredDetails.Mobile;
                    homepolicy.HomeInsurancePolicy.CPR = insuredDetails.Result.InsuredDetails.CPR;
                }

                //Get Quote for the given values.
                homeQuote.Agency = userInfo.Agency;
                homeQuote.AgentCode = userInfo.AgentCode;
                homeQuote.BuildingValue = !string.IsNullOrEmpty(txtBuildingValue.Text.Trim()) ? Convert.ToDecimal(txtBuildingValue.Text.Trim()) : 0;
                homeQuote.ContentValue = !string.IsNullOrEmpty(txtContentValue.Text.Trim()) ? Convert.ToDecimal(txtContentValue.Text.Trim()) : 0;
                homeQuote.IsPropertyToBeInsured = ddlPropertyInsured.SelectedIndex > 0 ? (ddlPropertyInsured.SelectedIndex == 1 ? true : false) : false;
                homeQuote.IsRiotStrikeAdded = ddlMaliciousDamageCover.SelectedIndex > 0 ? (ddlMaliciousDamageCover.SelectedIndex == 1 ? true : false) : false;
                homeQuote.JewelleryCover = ddlJewelleryCoverWithinContents.SelectedItem.Value;
                homeQuote.NumberOfDomesticWorker = Gridview1.Rows.Count;
                homeQuote.MainClass = MainClass;
                homeQuote.SubClass = SubClass;//ddlCover.SelectedItem.Value.Trim();
                homeQuote.RenewalDelayedDays = RenewalDelayedDays;

                var homeQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                     <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuoteResponse>,
                                     BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuote>
                                     (BKIC.SellingPoint.DTO.Constants.HomeURI.GetQuote, homeQuote);

                if (homeQuoteResult.StatusCode == 200 && homeQuoteResult.Result.IsTransactionDone)
                {
                    homepolicy.HomeInsurancePolicy.Agency = userInfo.Agency;
                    homepolicy.HomeInsurancePolicy.AgentCode = userInfo.AgentCode;
                    homepolicy.HomeInsurancePolicy.AgentBranch = ddlBranch.SelectedItem.Value.Trim();
                    homepolicy.HomeInsurancePolicy.PolicyStartDate = txtInsurancePeriodFrom.Text.CovertToCustomDateTime();
                    homepolicy.HomeInsurancePolicy.PolicyExpiryDate = txtInsurancePeriodTo.Text.CovertToCustomDateTime();
                    homepolicy.HomeInsurancePolicy.BuildingValue = string.IsNullOrEmpty(txtBuildingValue.Text) ? 0 : Convert.ToDecimal(txtBuildingValue.Text);
                    homepolicy.HomeInsurancePolicy.ContentValue = string.IsNullOrEmpty(txtContentValue.Text) ? 0 : Convert.ToDecimal(txtContentValue.Text);
                    homepolicy.HomeInsurancePolicy.JewelleryValue = !string.IsNullOrEmpty(txtJewelleryValue.Text.Trim()) ? Convert.ToDecimal(txtJewelleryValue.Text.Trim()) : 0;
                    homepolicy.HomeInsurancePolicy.PremiumAfterDiscount = homeQuoteResult.Result.TotalPremium;
                    homepolicy.HomeInsurancePolicy.PremiumBeforeDiscount = homeQuoteResult.Result.TotalPremium;
                    homepolicy.HomeInsurancePolicy.BuildingNo = txtBuildingNo.Text.Trim();
                    homepolicy.HomeInsurancePolicy.HouseNo = txtHouseNo.Text.Trim();
                    homepolicy.HomeInsurancePolicy.RoadNo = txtRoadNo.Text.Trim();
                    homepolicy.HomeInsurancePolicy.BlockNo = txtBlockNo.Text.Trim();
                    homepolicy.HomeInsurancePolicy.BuildingAge = ddlBuildingAge.SelectedIndex > 0 ? Convert.ToInt32(ddlBuildingAge.SelectedItem.Text) : 0;
                    homepolicy.HomeInsurancePolicy.FlatNo = txtFlatNo.Text.Trim();
                    homepolicy.HomeInsurancePolicy.Area = ddlArea.SelectedIndex > 0 ? ddlArea.SelectedItem.Text.Trim() : string.Empty;
                    homepolicy.HomeInsurancePolicy.NoOfFloors = string.IsNullOrEmpty(txtNoOfFloor.Text) ? 0 : Convert.ToInt32(txtNoOfFloor.Text);
                    homepolicy.HomeInsurancePolicy.BuildingType = ddlBuildingType.SelectedItem.Text.ToLower() == "flat" ? 2 : ddlBuildingType.SelectedItem.Text.ToLower() == "contents" ? 3 : 1;
                    homepolicy.HomeInsurancePolicy.ResidanceTypeCode = homepolicy.HomeInsurancePolicy.BuildingType == 1 ? "H" : "F";
                    homepolicy.HomeInsurancePolicy.PaymentType = ddlPaymentMethod.SelectedIndex > 0 ? ddlPaymentMethod.SelectedItem.Text.Trim() : string.Empty;
                    homepolicy.HomeInsurancePolicy.AccountNumber = txtAccountNo.Text.Trim();
                    homepolicy.HomeInsurancePolicy.Remarks = txtRemarks.Text.Trim();
                    homepolicy.HomeInsurancePolicy.IsSaved = isSave;
                    homepolicy.HomeInsurancePolicy.IsActivePolicy = !isSave;
                    homepolicy.HomeInsurancePolicy.MainClass = MainClass;
                    homepolicy.HomeInsurancePolicy.SubClass = SubClass;//ddlCover.SelectedItem.Value.Trim();
                    homepolicy.HomeInsurancePolicy.NoOfDomesticWorker = Gridview1.Rows.Count;
                    homepolicy.HomeInsurancePolicy.IsRenewal = true;

                    //var commisionRequest = new CommissionRequest();
                    //commisionRequest.AgentCode = userInfo.AgentCode;
                    //commisionRequest.Agency = userInfo.Agency;
                    //commisionRequest.SubClass = SubClass;
                    //commisionRequest.PremiumAmount = homeQuoteResult.Result.TotalPremium;
                    //var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper<BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                    //                 BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>("api/insurance/Commission", commisionRequest);

                    if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                            || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
                    {
                        if (Convert.ToDecimal(premiumAmount.Text) < homepolicy.HomeInsurancePolicy.PremiumBeforeDiscount || AjdustedPremium)
                        {
                            homepolicy.HomeInsurancePolicy.UserChangedPremium = true;
                            homepolicy.HomeInsurancePolicy.PremiumAfterDiscount = Convert.ToDecimal(premiumAmount.Text);
                            var diff = homepolicy.HomeInsurancePolicy.PremiumBeforeDiscount - homepolicy.HomeInsurancePolicy.PremiumAfterDiscount;
                            homepolicy.HomeInsurancePolicy.CommissionAfterDiscount = homeQuoteResult.Result.TotalCommission - diff;
                        }
                    }
                    else if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.User)
                    {
                        if (Convert.ToDecimal(premiumAmount1.Text) < homepolicy.HomeInsurancePolicy.PremiumBeforeDiscount || AjdustedPremium)
                        {
                            homepolicy.HomeInsurancePolicy.UserChangedPremium = true;
                            homepolicy.HomeInsurancePolicy.PremiumAfterDiscount = Convert.ToDecimal(premiumAmount1.Text);
                            var diff = homepolicy.HomeInsurancePolicy.PremiumBeforeDiscount - homepolicy.HomeInsurancePolicy.PremiumAfterDiscount;
                            homepolicy.HomeInsurancePolicy.CommissionAfterDiscount = homeQuoteResult.Result.TotalCommission - diff;
                        }
                    }
                }
                SetQuestionaire(homepolicy);
                SetHomeSubItems(homepolicy);
                SetDomesticHelp(homepolicy);

                if (_HomeID > 0)
                    homepolicy.HomeInsurancePolicy.HomeID = _HomeID;

                if (_RenewalCount > 0)
                    homepolicy.HomeInsurancePolicy.RenewalCount = _RenewalCount;

                homepolicy.HomeInsurancePolicy.CreatedBy = ddlUsers.SelectedIndex > 0 ?
                                            Convert.ToInt32(ddlUsers.SelectedItem.Value) : Convert.ToInt32(userInfo.ID);

                homepolicy.HomeInsurancePolicy.DocumentNo = txtHomeRenewalPolicySearch.Text.Trim(); //ddlHomePolicies.SelectedItem.Text.Trim();

                homepolicy.HomeInsurancePolicy.RenewalDelayedDays = RenewalDelayedDays;
                homepolicy.HomeInsurancePolicy.ActualRenewalStartDate = string.IsNullOrEmpty(actualRenewalDate.Value) ? (DateTime?)null : actualRenewalDate.Value.CovertToCustomDateTime();

                var postData = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsurancePolicyResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsurancePolicyDetails>
                               (BKIC.SellingPoint.DTO.Constants.HomeURI.PostPolicy, homepolicy);

                if (postData != null && postData.StatusCode == 200 && postData.Result.IsTransactionDone)
                {
                    _HomeID = postData.Result.HomeId;
                    LoadAgencyClientPolicyRenewal(userInfo, service);
                    //ddlHomePolicies.SelectedIndex = ddlHomePolicies.Items.IndexOf(ddlHomePolicies.Items.FindByText(postData.Result.DocumentNo));
                    txtHomeRenewalPolicySearch.Text = postData.Result.DocumentNo;
                    modalBodyText.InnerText = GetMessageText(postData.Result.IsHIR, homepolicy.HomeInsurancePolicy.IsActivePolicy, postData.Result.DocumentNo);
                    if (homepolicy.HomeInsurancePolicy.IsActivePolicy)
                    {
                        SetScheduleHRef(postData.Result.DocumentNo, Constants.Home, userInfo, postData.Result.RenewalCount);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowPopup();", true);
                }
                else
                {
                    master.ShowErrorPopup(postData.Result.TransactionErrorMessage, "Request Failed !");
                    //return;
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

        protected void txtBuildingValue_TextChanged(object sender, EventArgs e)
        {
            try
            {               
                UpdateTotalSumInsured();
                txtPolicyBuildingValueChanged();
                UpdateJewelleryCover();
                DisableControls();
                master.ShowHideErrorSpacingSpan();
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

        protected void txtContentValueQuotation_TextChanged(object sender, EventArgs e)
        {
            try
            {                
                UpdateTotalSumInsured();
                txtContentValueQuotationChanged();
                UpdateJewelleryCover();
                DisableControls();
                master.ShowHideErrorSpacingSpan();
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

        protected void txtJewelleryValueQuotation_TextChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateJewelleryCover();
                UpdateTotalSumInsured();
                DisableControls();
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

        public void UpdateJewelleryCover()
        {
            if (string.IsNullOrEmpty(txtJewelleryValue.Text))
            {
                ddlPropertyInsured.SelectedIndex = 2;
                ddlPropertyInsured.Enabled = false;
                ddlJewelleryCoverWithinContents.SelectedIndex = 1;
                ddlJewelleryCoverWithinContents.Enabled = false;
            }
            else
            {
                decimal JewlleryAmount = Convert.ToDecimal(txtJewelleryValue.Text);
                if (JewlleryAmount == 0)
                {
                    ddlPropertyInsured.SelectedIndex = 2;
                    ddlPropertyInsured.Enabled = false;
                    ddlJewelleryCoverWithinContents.SelectedIndex = 1;
                    ddlJewelleryCoverWithinContents.Enabled = false;
                }
                if (JewlleryAmount > 0 && JewlleryAmount <= 2500)
                {
                    ddlPropertyInsured.SelectedIndex = 1;
                    ddlPropertyInsured.Enabled = false;
                    ddlJewelleryCoverWithinContents.SelectedIndex = 2;
                    ddlJewelleryCoverWithinContents.Enabled = false;
                }
                else if (JewlleryAmount > 2500 && JewlleryAmount <= 5000)
                {
                    ddlPropertyInsured.SelectedIndex = 1;
                    ddlPropertyInsured.Enabled = false;
                    ddlJewelleryCoverWithinContents.SelectedIndex = 3;
                    ddlJewelleryCoverWithinContents.Enabled = false;
                }
                else if (JewlleryAmount > 5000)
                {
                    ddlPropertyInsured.SelectedIndex = 1;
                    ddlPropertyInsured.Enabled = false;
                    ddlJewelleryCoverWithinContents.SelectedIndex = 4;
                    ddlJewelleryCoverWithinContents.Enabled = false;
                }

            }

        }

        public void UpdateTotalSumInsured()
        {
            decimal contentvalue = 0;
            decimal buildingvalue = 0;
            decimal jewellvalue = 0;

            if (string.IsNullOrEmpty(txtContentValue.Text))
                contentvalue = 0;
            else
                contentvalue = Convert.ToDecimal(txtContentValue.Text);
            if (string.IsNullOrEmpty(txtBuildingValue.Text))
                buildingvalue = 0;
            else
                buildingvalue = Convert.ToDecimal(txtBuildingValue.Text);
            if (string.IsNullOrEmpty(txtJewelleryValue.Text))
                jewellvalue = 0;
            else
                jewellvalue = Convert.ToDecimal(txtJewelleryValue.Text);

            txtTotalSumInsured.Text = Convert.ToString(contentvalue + buildingvalue + jewellvalue);
        }

        private void txtContentValueQuotationChanged()
        {
            int contentValue = 0;
            if (Int32.TryParse(txtContentValue.Text, out contentValue))
            {
                if (contentValue > 0)
                {
                    rfvtxtBuildingValue.Enabled = false;
                    ddlJewelleryCoverWithinContents.Enabled = true;
                    ddlJewelleryCoverWithinContents.SelectedIndex = 0;
                }
                else
                {
                    rfvtxtBuildingValue.Enabled = true;
                    ddlJewelleryCoverWithinContents.Enabled = false;
                    ddlJewelleryCoverWithinContents.SelectedIndex = 1;
                }
            }
            else
            {
                rfvtxtBuildingValue.Enabled = true;
                ddlJewelleryCoverWithinContents.Enabled = false;
                ddlJewelleryCoverWithinContents.SelectedIndex = 1;
            }
        }

        private void SetJewelleryConverToNoCoverOnlyInQuotation(bool enable)
        {
            if (!enable)
            {
                ddlJewelleryCoverWithinContents.Enabled = false;
                ddlJewelleryCoverWithinContents.SelectedIndex = 1;
            }
            else
            {
                ddlJewelleryCoverWithinContents.Enabled = true;
                ddlJewelleryCoverWithinContents.SelectedIndex = 0;
            }
        }

        private void txtPolicyBuildingValueChanged()
        {
            int buildingValue = 0;
            if (Int32.TryParse(txtBuildingValue.Text, out buildingValue))
            {
                if (buildingValue > 0)
                {
                    rfvtxtContentValue.Enabled = false;
                    ddlJewelleryCoverWithinContents.Enabled = false;
                    ddlJewelleryCoverWithinContents.SelectedIndex = 1;
                }
                else
                {
                    rfvtxtContentValue.Enabled = true;
                    ddlJewelleryCoverWithinContents.Enabled = true;
                    ddlJewelleryCoverWithinContents.SelectedIndex = 0;
                }
            }
            else
            {
                rfvtxtContentValue.Enabled = true;
                ddlJewelleryCoverWithinContents.Enabled = true;
                ddlJewelleryCoverWithinContents.SelectedIndex = 0;                
            }
        }

        private void SetJewelleryCoverToNoCoverOnly(bool enable)
        {
            if (!enable)
            {
                ddlJewelleryCoverWithinContents.Enabled = false;
                ddlJewelleryCoverWithinContents.SelectedIndex = 1;
            }
            else
            {
                ddlJewelleryCoverWithinContents.Enabled = true;
                ddlJewelleryCoverWithinContents.SelectedIndex = 0;
            }
        }

        protected void txtAmountOfItem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal contentValue = 0;

                if (!string.IsNullOrEmpty(txtContentValue.Text))
                {
                    contentValue = Convert.ToDecimal(txtContentValue.Text);
                }

                TextBox text = sender as TextBox;
                decimal singleItemValue = Convert.ToDecimal(text.Text);
                int repeaterItem = ((RepeaterItem)text.NamingContainer).ItemIndex;
                decimal totalSingleItemValue = 0;

                foreach (RepeaterItem i in rtSingleItemAboveBD.Items)
                {
                    TextBox itemValue = (TextBox)i.FindControl("txtAmountOfItem");

                    if (!string.IsNullOrEmpty(itemValue.Text))
                    {
                        totalSingleItemValue = totalSingleItemValue + Convert.ToDecimal(itemValue.Text);
                    }
                }

                foreach (RepeaterItem i in rtSingleItemAboveBD.Items)
                {
                    if (i.ItemIndex == repeaterItem)
                    {
                        CustomValidator cvalidation = (CustomValidator)i.FindControl("cvAmountOfItem");

                        if (singleItemValue > contentValue || singleItemValue < 2001 || totalSingleItemValue > contentValue)
                        {
                            cvalidation.IsValid = false;

                            if (singleItemValue > contentValue || totalSingleItemValue > contentValue)
                            {
                                cvalidation.ErrorMessage = "Total shouldn't exceed content value";
                            }
                            else
                            {
                                cvalidation.ErrorMessage = "Minimum amount should be 2001";
                            }
                        }
                        else
                        {
                            cvalidation.IsValid = true;
                            cvalidation.ErrorMessage = "";
                        }
                    }
                }

                DataTable singleItemDetails = GetSingleItems();
                rtSingleItemAboveBD.DataSource = singleItemDetails;
                rtSingleItemAboveBD.DataBind();

                ViewState["SingleItemContents"] = singleItemDetails;
                master.ShowHideErrorSpacingSpan();
                DisableControls();
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

        protected void btnRemoveSingleItem_Click(object sender, EventArgs e)
        {
            try
            {
               
                Button removeItem = sender as Button;
                int repeaterItemIndex = ((RepeaterItem)removeItem.NamingContainer).ItemIndex;

                DataTable singleItemDetails = GetSingleItems();

                if (singleItemDetails != null)
                {
                    singleItemDetails.Rows.RemoveAt(repeaterItemIndex);
                }

                rtSingleItemAboveBD.DataSource = singleItemDetails;
                rtSingleItemAboveBD.DataBind();

                ViewState["SingleItemContents"] = singleItemDetails;
                DisableControls();
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

        protected void rtSingleItemAboveBD_DataBinding(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = e.Item.DataItem as DataRowView;
                int currentRepeaterIndex = e.Item.ItemIndex;

                if (currentRepeaterIndex == 0)
                {
                    Button removeItem = (Button)e.Item.FindControl("btnRemoveSingleItem");
                    removeItem.Visible = false;
                }

                if (singleItemCategories != null && singleItemCategories.Rows.Count > 0)
                {
                    DropDownList ddl = (DropDownList)e.Item.FindControl("ddlCategoryForAboveBD");
                    ddl.DataValueField = "Title";
                    ddl.DataTextField = "Title";
                    ddl.DataSource = singleItemCategories;
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("--Please Select--", "-1"));

                    if (drv != null && !string.IsNullOrEmpty(drv.Row["Category"].ToString()))
                    {
                        ddl.SelectedItem.Text = drv.Row["Category"].ToString();
                    }
                }

                if (drv != null && !string.IsNullOrEmpty(drv.Row["MoreDescription"].ToString()))
                {
                    TextBox moreInfo = (TextBox)e.Item.FindControl("txtCategoryDescription");
                    moreInfo.Text = drv.Row["MoreDescription"].ToString();
                }

                if (drv != null && !string.IsNullOrEmpty(drv.Row["AmountOfItem"].ToString()))
                {
                    TextBox itemValue = (TextBox)e.Item.FindControl("txtAmountOfItem");
                    itemValue.Text = drv.Row["AmountOfItem"].ToString();

                    CustomValidator cvItemValue = (CustomValidator)e.Item.FindControl("cvAmountOfItem");
                    cvItemValue.IsValid = Convert.ToBoolean(drv.Row["SingleItemCVStatus"]);
                    cvItemValue.ErrorMessage = Convert.ToString(drv.Row["SingleItemCVErrorMessage"]);
                }
            }
        }

        private DataTable CreateSingleItemAboveBDDetails(int index)
        {
            DataTable singleItemDt = new DataTable();
            singleItemDt.Columns.Add("Category");
            singleItemDt.Columns.Add("MoreDescription");
            singleItemDt.Columns.Add("AmountOfItem");
            singleItemDt.Columns.Add("Index");

            singleItemDt.Rows.Add("", "", "", index);
            return singleItemDt;
        }

        protected void btnAddMoreItem_Click(object sender, EventArgs e)
        {
            try
            {               
                DataTable existSingleItems = GetSingleItems();
                existSingleItems.Rows.Add("", "", "", existSingleItems.Rows.Count + 1);
                rtSingleItemAboveBD.DataSource = existSingleItems;
                rtSingleItemAboveBD.DataBind();
                ViewState["SingleItemContents"] = existSingleItems;
                DisableControls();
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

        private DataTable GetSingleItems()
        {
            DataTable singleItemDt = new DataTable();
            singleItemDt.Columns.Add("Category", typeof(string));
            singleItemDt.Columns.Add("MoreDescription", typeof(string));
            singleItemDt.Columns.Add("AmountOfItem", typeof(string));
            singleItemDt.Columns.Add("SingleItemCVStatus", typeof(bool));
            singleItemDt.Columns.Add("SingleItemCVErrorMessage", typeof(string));
            singleItemDt.Columns.Add("Index", typeof(Int32));
            int index = 0;

            foreach (RepeaterItem i in rtSingleItemAboveBD.Items)
            {
                index++;
                TextBox category = (TextBox)i.FindControl("txtCategory");
                // TextBox description = (TextBox)i.FindControl("txtCategoryDescription");
                TextBox itemValue = (TextBox)i.FindControl("txtAmountOfItem");
                CustomValidator cvSingleItem = (CustomValidator)i.FindControl("cvAmountOfItem");

                if (!string.IsNullOrEmpty(cvSingleItem.ErrorMessage))
                {
                    cvSingleItem.IsValid = false;
                }

                singleItemDt.Rows.Add(category.Text, "", itemValue.Text, cvSingleItem.IsValid, cvSingleItem.ErrorMessage, index);
            }

            return singleItemDt;
            //master.ShowHideErrorSpacingSpan();
        }

        protected void ddlPropertyInsured_Changed(object sender, EventArgs e)
        {
        }

        public void ShowPremium(OAuthTokenResponse userInfo, decimal Premium, decimal Commission)
        {
            amtDisplay.Visible = true;
            btnSubmit.Visible = true;
            if (userInfo.Roles == "SuperAdmin" || userInfo.Roles == "BranchAdmin")
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
            btnAuthorize.Enabled = true;
            btnSubmit.Enabled = true;
        }

        public void HidePremium(bool isQuestionChanged)
        {
            amtDisplay.Visible = false;

            premiumAmount.Text = string.Empty;
            commission.Text = string.Empty;
            includeDisc.Visible = false;

            premiumAmount1.Text = string.Empty;
            commission1.Text = string.Empty;
            excludeDisc.Visible = false;

            btnAuthorize.Visible = false;
            btnSubmit.Visible = false;
            downloadschedule.Visible = false;

            if (!isQuestionChanged)
            {
                //ddlCPR.SelectedIndex = 0;
                //ddlHomePolicies.SelectedIndex = 0;
                txtHomeRenewalPolicySearch.Text = string.Empty;
                _HomeID = 0;
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

        public void ShowDiscount(OAuthTokenResponse userInfo, HomeInsurancePolicy policy)
        {
            txtDiscount.Text = Convert.ToString(policy.PremiumBeforeDiscount - policy.PremiumAfterDiscount);
            if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.User)
            {
                if (policy.PremiumBeforeDiscount - policy.PremiumAfterDiscount > 0)
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

        public void ResetSingleItem()
        {
            mainAboveBD.Visible = false;
            divSingleItemAboveBD.Visible = false;
        }

        public void ResetBank()
        {
            divBankName.Visible = false;
        }

        public void ResetInsurar()
        {
            divInsurar.Visible = false;
        }

        public void ResetJoint()
        {
            divJointOwner.Visible = false;
        }

        public void ResetDependant()
        {
            ViewState["CurrentTable"] = null;
            Gridview1.DataSource = null;
            Gridview1.DataBind();
            mainDomesticWorker.Visible = false;
            divDetailedDomesticWorkers.Visible = false;
        }

        public void ResetQuestions()
        {
            ddlConnectionWithAnyTrade.SelectedIndex = 0;
            ddlInsuredCoverByOtherInsurance.SelectedIndex = 0;
            ddlJewelleryCoverWithinContents.SelectedIndex = 0;
            ddlMaliciousDamageCover.SelectedIndex = 0;
            ddlMortgaged.SelectedIndex = 0;
            ddlPropertyInsured.SelectedIndex = 0;
            ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedIndex = 0;
            ddlPropertyJointOwnership.SelectedIndex = 0;
            ddlSingleItemAboveBD.SelectedIndex = 0;
        }

        public void SetScheduleHRef(string DocNo, string Insurancetype, OAuthTokenResponse UserInfo, int RenewalCount)
        {
            downloadschedule.Visible = true;
            downloadschedule.HRef = ClientUtility.WebApiUri + BKIC.SellingPoint.DTO.Constants.ScheduleURI.downloadschedule
                                    .Replace("{insuranceType}", Insurancetype)
                                    .Replace("{agentCode}", UserInfo.AgentCode)
                                    .Replace("{documentNo}", DocNo)
                                    .Replace("{isEndorsement}", "false")
                                    .Replace("{endorsementID}", "0")
                                    .Replace("{renewalCount}", Convert.ToString(RenewalCount));
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
            btnNo.Text = "No";
            btnYes.Visible = true;
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
                btnYes.Visible = false;
                btnNo.Text = "OK";
                btnAuthorize.Visible = false;
                return "Your home renewal policy is saved and moved into HIR: " + docNo;
            }
            else if (!isHIR && !isActivePolicy)
            {
                btnYes.Visible = false;
                btnNo.Text = "OK";
                btnAuthorize.Enabled = true;
                btnAuthorize.Visible = true;
                return "Your home renewal policy has been saved successfully: " + docNo;
            }
            else if (isActivePolicy)
            {
                master.makeReadOnly(GetContentControl(), false);
                btnCalculate.Enabled = false;
                btnSubmit.Enabled = false;
                btnYes.Visible = false;
                btnNo.Text = "OK";
                btnAuthorize.Enabled = false;
                return "Your home renewal policy has been authorized successfully: " + docNo;
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

        protected void BlockNumber_Changed(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtBlockNo.Text))
                {
                    if (Area != null && Area.Rows.Count > 0)
                    {
                        var AreaList = from row in Area.AsEnumerable()
                                       where row.Field<string>("AreaCode") == txtBlockNo.Text.Trim()
                                       select row;
                        if (AreaList != null && AreaList.Count() > 0)
                        {
                            var description = AreaList.ElementAt(0).Field<string>("Description");
                            ddlArea.SelectedIndex = ddlArea.Items.IndexOf(ddlArea.Items.FindByText(description));
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

        protected void ddlInsuredCoverByOtherInsurance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlInsuredCoverByOtherInsurance.SelectedIndex > 0 && ddlInsuredCoverByOtherInsurance.SelectedValue.ToLower() != "no")
            {
                divInsurar.Visible = true;
            }
            else
            {
                divInsurar.Visible = false;
            }
            if (_policyFetched && ddlMortgaged.SelectedIndex > 0 && ddlMortgaged.SelectedIndex != _isMortgagedPreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }
        }

        protected void ddlPropertyJointOwnership_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPropertyJointOwnership.SelectedIndex > 0 && ddlPropertyJointOwnership.SelectedValue.ToLower() != "no")
            {
                divJointOwner.Visible = true;
            }
            else
            {
                divJointOwner.Visible = false;
            }
            if (_policyFetched && ddlPropertyJointOwnership.SelectedIndex > 0 && ddlPropertyJointOwnership.SelectedIndex != _isJointOwenerPreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }
        }

        protected void ddlMortaged_Changed(object sender, EventArgs e)
        {
            if (ddlMortgaged.SelectedIndex > 0 && ddlMortgaged.SelectedValue.ToLower() != "no")
            {
                divBankName.Visible = true;
            }
            else
            {
                divBankName.Visible = false;
            }
            if (_policyFetched && ddlMortgaged.SelectedIndex > 0 && ddlMortgaged.SelectedIndex != _isMortgagedPreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }
        }

        protected void ddlProperty_Changed(object sender, EventArgs e)
        {
            if (_policyFetched && ddlPropertyInsured.SelectedIndex > 0 && ddlPropertyInsured.SelectedIndex != _isSafePropertyPreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }
        }

        protected void ddlJewellery_Changed(object sender, EventArgs e)
        {
            if (_policyFetched && ddlJewelleryCoverWithinContents.SelectedIndex > 0 && ddlJewelleryCoverWithinContents.SelectedIndex != _jewelleryCoverPreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }
        }

        protected void ddlMaliciousDamage_Changed(object sender, EventArgs e)
        {
            if (_policyFetched && ddlMaliciousDamageCover.SelectedIndex > 0
                && ddlMaliciousDamageCover.SelectedIndex != _isRiotStrikePreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }
        }

        protected void ddlConnectionWithAnyTrade_Changed(object sender, EventArgs e)
        {
            if (_policyFetched && ddlConnectionWithAnyTrade.SelectedIndex > 0
                && ddlConnectionWithAnyTrade.SelectedIndex != _isAnyOtherTradePreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }
        }

        protected void ddlPropertyInsuredSustained(object sender, EventArgs e)
        {
            if (_policyFetched && ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedIndex > 0
                && ddlPropertyInsuredSustainedAnyLossOrDamage.SelectedIndex != _isSustainedPreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }
        }

        protected void ddlSingleItemAboveBD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSingleItemAboveBD.SelectedIndex > 0 && ddlSingleItemAboveBD.SelectedValue.ToLower() != "no")
            {
                mainAboveBD.Visible = true;
                divSingleItemAboveBD.Visible = true;

                rtSingleItemAboveBD.DataSource = CreateSingleItemAboveBDDetails(1);
                rtSingleItemAboveBD.DataBind();
            }
            else
            {
                mainAboveBD.Visible = false;
                divSingleItemAboveBD.Visible = false;
            }
            if (_policyFetched && ddlSingleItemAboveBD.SelectedIndex > 0
                && ddlSingleItemAboveBD.SelectedIndex != _isAboveBDPreviousIndex)
            {
                _questionarieChanged = true;
            }
            if (_calculatedSuccessful)
            {
                _questionarieChanged = true;
            }

            master.ShowLoading = false;
        }

        protected void btnClear_Click(object sener, EventArgs e)
        {
            try
            {
                master.ClearControls(GetContentControl());
                SetReadOnlyControls();
                HidePremium(false);
                ResetDependant();
                ResetJoint();
                ResetBank();
                ResetInsurar();
                ResetSingleItem();
                ResetQuestions();
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

        protected void Gridview1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);
            ViewState["CurrentTable"] = GetDomesticWorkers();
            DataTable dt = ViewState["CurrentTable"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["CurrentTable"] = dt;
            Gridview1.DataSource = dt;
            Gridview1.DataBind();
            SetPreviousData();
            DisableControls();
        }

        public void DisableControls()
        {
            btnSubmit.Visible = false;
            btnAuthorize.Visible = false;
            premiumAmount.Text = string.Empty;
            premiumAmount1.Text = string.Empty;
            commission.Text = string.Empty;
            commission1.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            includeDisc.Visible = false;
            excludeDisc.Visible = false;
        }

        protected void HomeProduct_changed(object sender, EventArgs e)
        {
            try
            {
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