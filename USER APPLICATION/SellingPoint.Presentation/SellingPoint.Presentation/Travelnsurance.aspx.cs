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
    public partial class Travelnsurance : System.Web.UI.Page
    {
        private General master;
        public static DataTable Nationalitydt;
        public static DataTable Relationdt;
        public static DataTable Genderdt;

        public static string _InsuredCode;
        public static string _InsuredName;
        public static string _CPR;
        public static string _DOB;
        public static long _TravelId = 0;
        public bool UserChangedPremium = false;
        public static List<InsuredMasterDetails> InsuredNames { get; set; }
        public static string MainClass { get; set; }
        public static bool AjdustedPremium { get; set; }

        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelPolicy> policyList = new List<AgencyTravelPolicy>();

        public Travelnsurance()
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

                SetInitialRow();
                DisableDefaultControls(userInfo, service);
                BindDropdown(userInfo, service);
                LoadUsers(userInfo, service);
                LoadAgencyClientCode(userInfo, service);
                QueryStringMethods(userInfo, service);
            }
        }

        public void DisableDefaultControls(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            _TravelId = 0;
            AjdustedPremium = false;
            //userdetails.Visible = true;
            amtDisplay.Visible = false;
            admindetails.Visible = false;
            phyDefect.Visible = false;
            successDiv.Visible = false;
            downloadschedule.Visible = false;
            btnAuthorize.Visible = false;
            btnTravelSave.Visible = false;
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

            LoadAgencyClientPolicyInsuredCode(userInfo, service, includeHIR != null ? Convert.ToBoolean(includeHIR) : false);

            if (cpr != null)
            {
                string CPR = Convert.ToString(cpr);               
                txtCPRSearch.Text = CPR;
                txtCPR.Text = CPR;
                var insured = master.GetInsured(CPR, string.Empty, userInfo.Agency, userInfo.AgentCode);
                if(insured != null)
                {
                    txtInsuredAge.Text = Convert.ToString(master.CalculateAgeCorrect(insured.DateOfBirth.Value, DateTime.Now));
                    if (!ValidateInsured(insured))
                        return;
                }                
            }
            if (Request.QueryString["PolicyNo"] != null)
            {                
                txtTravelPolicySearch.Text = Convert.ToString(Request.QueryString["PolicyNo"]);
                GetPolicyInfo();
            }
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

        public void Enable_controls()
        {
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

        public bool ValidateInsured(InsuredMasterDetails insured)
        {
            bool isValid = true;
            if (insured != null)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();                

                int insuredAge = master.CalculateAgeCorrect(insured.DateOfBirth.Value, DateTime.Now);

                if (insured.DateOfBirth == null)
                {
                    isValid = false;
                    master.ShowErrorPopup("Insured date of birth is mandatory !!", "Insured");
                }
                if (21 > insuredAge && userInfo.Agency == "BBK")
                {
                    isValid = false;
                    master.ShowErrorPopup("Insured age should be above 21 years !!", "Insured age is :" + insuredAge);
                }
                if (string.IsNullOrEmpty(insured.PassportNo))
                {
                    isValid = false;
                    master.ShowErrorPopup("Insured must required a passport !!", "Insured");
                }
            }
            return isValid;
        }

        public bool ValidateAge(InsuredMasterDetails insured)
        {
            bool isValid = true;            
            if (insured != null)
            {
                int insuredAge = master.CalculateAgeCorrect(insured.DateOfBirth.Value, DateTime.Now);

                if (insuredAge > 79 && (ddlPackage.SelectedItem.Value == "STS" || ddlPackage.SelectedItem.Value == "STI"))
                {
                    isValid = false;
                    master.ShowErrorPopup("Can't issue a individual policy!", "Insured over age :" + insuredAge);
                   
                }
                if (insuredAge > 65 && ddlPackage.SelectedItem.Value == "STP")
                {
                    isValid = false;
                    master.ShowErrorPopup("Can't issue a family policy!", "Insured over age :" + insuredAge);                    
                }
            }
            return isValid;
        }
        public bool ValidateDependant(OAuthTokenResponse userInfo)
        {
            int spouseCount = 0;
            bool isValid = true;
            if(ddlPackage.SelectedItem.Text.ToLower() == "family")
            {
                foreach (GridViewRow row in Gridview1.Rows)
                {
                    DropDownList relation = (DropDownList)row.FindControl("ddlRelation");
                    if (relation.SelectedIndex > 0)
                    {
                        TextBox dob = (TextBox)row.FindControl("txtDOB");
                        int memberAge = master.CalculateAgeCorrect(dob.Text.CovertToCustomDateTime(), DateTime.Now);
                        if (userInfo.Agency == "TISCO")
                        {
                            if (relation.SelectedItem.Text == "spouse")
                            {
                                spouseCount++;
                                //spouse age should not be greater than 65 for TISCO.
                                if (memberAge > 65)
                                {
                                    isValid = false;
                                    master.ShowErrorPopup("Spouse age is greater than 65 years", "Can't Issue a Policy");
                                }
                                //More than one spouse is not allowed for TISCO.
                                if (spouseCount > 1)
                                {
                                    isValid = false;
                                    master.ShowErrorPopup("More than one spouse not allowed !", "Can't Issue a Policy");
                                }
                            }
                            else if (relation.SelectedItem.Text == "son" || relation.SelectedItem.Text == "daughter")
                            {
                                //Kid age should not be above 18 years for TISCO.
                                if (memberAge > 18)
                                {
                                    isValid = false;
                                    master.ShowErrorPopup("Kid age should be less than 18 years !", "Can't Issue a Policy");
                                }
                                //Kid age should be above 3 months for TISCO.
                                var ThreeMonthsDeduct = DateTime.Now.AddMonths(-3);
                                if (ThreeMonthsDeduct < dob.Text.CovertToCustomDateTime())
                                {
                                    isValid = false;
                                    master.ShowErrorPopup("Kid age should be greater than 3 months", "Can't Issue a Policy");
                                }
                            }
                        }
                        else if (userInfo.Agency == "BBK")
                        {
                            if (relation.SelectedItem.Text == "son" || relation.SelectedItem.Text == "daughter")
                            {
                                //Kid age should not be above 21 years for SECURA.
                                if (memberAge > 21)
                                {
                                    isValid = false;
                                    master.ShowErrorPopup("Kid age should be less than 21 years !", "Can't Issue a Policy");
                                }
                            }
                        }
                    }
                }
            }          
            return isValid;
        }

        protected void insured_Master(object sender, EventArgs e)
        {
            Response.Redirect("InsuredMaster.aspx?type=" + 2);
        }

        private void LoadAgencyClientPolicyInsuredCode(OAuthTokenResponse userInfo, DataServiceManager service, bool includeHIR = false)
        {
            var travelreq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelRequest
            {
                AgentCode = userInfo.AgentCode,
                AgentBranch = userInfo.AgentBranch,
                includeHIR = includeHIR
            };

            //Get PolicyNo by Agency
            var travelPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelPolicyResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelRequest>
                                 (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetAgencyPolicy, travelreq);

            if (travelPolicies.StatusCode == 200 && travelPolicies.Result.AgencyTravelPolicies.Count > 0)
            {
                policyList = travelPolicies.Result.AgencyTravelPolicies;
                //ddlTravelPolicies.DataSource = travelPolicies.Result.AgencyTravelPolicies;
                //ddlTravelPolicies.DataTextField = "DOCUMENTNO";
                //ddlTravelPolicies.DataValueField = "DOCUMENTNO";
                //ddlTravelPolicies.DataBind();
                //ddlTravelPolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
            }
        }

        protected void btnInsuredPage_Click(object source, EventArgs args)
        {
        }

        private void LoadAgencyClientCode(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest
            {
                AgentBranch = userInfo.AgentBranch,
                AgentCode = userInfo.AgentCode
            };

            var travelResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest>
                               (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyInsured, req);

            if (travelResult.StatusCode == 200 && travelResult.Result.IsTransactionDone && travelResult.Result.AgencyInsured.Count > 0)
            {
                //ddlCPR.DataSource = travelResult.Result.AgencyInsured;
                //ddlCPR.DataTextField = "CPR";
                //ddlCPR.DataValueField = "InsuredCode";
                //ddlCPR.DataBind();
                //ddlCPR.Items.Insert(0, new ListItem("--Please Select--", ""));
                InsuredNames = travelResult.Result.AgencyInsured;
            }
            ddlUsers.SelectedIndex = ddlUsers.Items.IndexOf(ddlUsers.Items.FindByText(userInfo.UserName));
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(userInfo.AgentBranch));

            if (userInfo.Roles == "BranchAdmin" || userInfo.Roles == "User")
            {
                ddlUsers.Enabled = false;
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
                //throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void ddlCPR_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                SetCPR();
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

        private void SetCPR()
        {
            txtCPR.Text = txtCPRSearch.Text.Trim();
            if (InsuredNames != null && InsuredNames.Count > 0)
            {
                var insured = InsuredNames.Find(c => c.CPR == txtCPRSearch.Text.Trim());
                if (insured != null)
                {
                    txtInsuredName.Text = insured.FirstName + " " + insured.MiddleName + " " + insured.LastName;
                    txtClientCode.Text = insured.InsuredCode;
                    _DOB = insured.DateOfBirth.ConvertToLocalFormat();
                    txtInsuredAge.Text = Convert.ToString(master.CalculateAgeCorrect(insured.DateOfBirth.Value, DateTime.Now));
                    ValidateInsured(insured);
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
            DisableControls();
        }

        public void DisableControls()
        {
            btnTravelSave.Visible = false;
            //txtPhysicalDesc.Text = string.Empty;
            //phyDefect.Visible = false;
            btnAuthorize.Visible = false;
            premiumAmount.Text = string.Empty;
            premiumAmount1.Text = string.Empty;
            commission.Text = string.Empty;
            commission1.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            includeDisc.Visible = false;
            excludeDisc.Visible = false;
        }

        public void SetCoverage(bool isActive)
        {
            if (ddlJourney.SelectedIndex == 0 || ddlPackage.SelectedIndex == 3)
            {
                ddlJourney.Enabled = false;
                rfvddlJourney.Enabled = false;
            }
            else
            {
                if (!isActive)
                {
                    ddlJourney.Enabled = true;
                    rfvddlJourney.Enabled = true;
                }
            }
        }

        #region CustomValidations
        #endregion CustomValidations

        private void BindDropdown(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns
                                .Replace("{type}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.Travelnsurance));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable Packagedt = dropdownds.Tables["TravelInsurancePackage"];
                DataTable Peroiddt = dropdownds.Tables["TravelInsurancePeroid"];
                Nationalitydt = dropdownds.Tables["Nationality"];
                Relationdt = dropdownds.Tables["FamilyRelationShip"];
                DataTable travelCoveragedt = dropdownds.Tables["TravelCoverage"];
                DataTable introducedByDt = dropdownds.Tables["Introducedby"];
                DataTable InsuredDt = dropdownds.Tables["InsuredMasterDD"];
                DataTable paymentTypes = dropdownds.Tables["PaymentType"];

                DataTable branches = dropdownds.Tables["BranchMaster"];

                if (Packagedt.Rows.Count > 0)
                {
                    ddlPackage.DataValueField = "Code";
                    ddlPackage.DataTextField = "Name";
                    ddlPackage.DataSource = Packagedt;
                    ddlPackage.DataBind();
                    ddlPackage.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
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
                if (Peroiddt.Rows.Count > 0)
                {
                    ddlPeriod.DataValueField = "Code";
                    ddlPeriod.DataTextField = "Name";
                    ddlPeriod.DataSource = Peroiddt;
                    ddlPeriod.DataBind();
                    ddlPeriod.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
                if (travelCoveragedt.Rows.Count > 0)
                {
                    ddlJourney.DataValueField = "Code";
                    ddlJourney.DataTextField = "CoverageType";
                    ddlJourney.DataSource = travelCoveragedt;
                    ddlJourney.DataBind();
                    ddlJourney.Items.Insert(0, new ListItem("--Please Select--", ""));
                }

                if (paymentTypes.Rows.Count > 0)
                {
                    ddlPaymentMethod.DataValueField = "Code";
                    ddlPaymentMethod.DataTextField = "Value";
                    ddlPaymentMethod.DataSource = paymentTypes;
                    ddlPaymentMethod.DataBind();
                    ddlPaymentMethod.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
            }

            var productResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchProductCodeResponse>>(
                                BKIC.SellingPoint.DTO.Constants.DropDownURI.GetInsuranceProductCode
                                .Replace("{agency}", userInfo.Agency)
                                .Replace("{agencyCode}", userInfo.AgentCode)
                                .Replace("{insurancetypeid}", "2"));

            if (productResult != null && productResult.StatusCode == 200 && productResult.Result.IsTransactionDone)
            {
                var products = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>(
                               BKIC.SellingPoint.DTO.Constants.DropDownURI.GetAgencyProducts
                               .Replace("{agency}", userInfo.Agency)
                               .Replace("{agencyCode}", userInfo.AgentCode)
                               .Replace("{mainclass}", productResult.Result.productCode)
                               .Replace("{page}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.TravelInsurance));

                MainClass = productResult.Result.productCode;
                if (products != null && products.StatusCode == 200 && products.Result.IsTransactionDone)
                {
                    DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(products.Result.dropdownresult);
                    DataTable prods = dropdownds.Tables["Products"];
                    ddlPackage.DataValueField = "SUBCLASS";
                    ddlPackage.DataTextField = "DESCRIPTION";
                    ddlPackage.DataSource = prods;
                    ddlPackage.DataBind();
                    ddlPackage.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
            }
            //txtIssueDate.Text = DateTime.Now.CovertToLocalFormat();
        }      

        

        public string getExpiryDate(string peroid, string commencedate)
        {
            master = Master as General;

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            DTO.RequestResponseWrappers.TravelInsuranceExpiryDate input = new TravelInsuranceExpiryDate();
            input.PackageCode = peroid;
            input.CommenceDate = commencedate.CovertToCustomDateTime();

            var expiryDateResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                     <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceExpiryDateResponse>,
                                     BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceExpiryDate>
                                     (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetPolicyExpirtyDate, input);

            DateTime ExpiryDate;

            if (expiryDateResponse != null && expiryDateResponse.StatusCode == 200 && expiryDateResponse.Result != null
                && expiryDateResponse.Result.IsTransactionDone)
            {
                ExpiryDate = expiryDateResponse.Result.ExpiryDate.Value;
                return ExpiryDate.CovertToLocalFormat();
            }

            return "";
        }

        private List<TravelMembers> GetFamilyDetails()
        {
            #region test_1

            var objs = new List<TravelMembers>();

            for (int row = 1; row <= Gridview1.Rows.Count; row++)
            {
                //DataRow TempRow = TempTable.NewRow();
                var obj = new TravelMembers();
                obj.ItemSerialNo = row + 1;
                obj.ForeignSumInsured = 50000;
                obj.SumInsured = 18900;

                for (int col = 0; col < Gridview1.Columns.Count; col++)
                {
                    if (Gridview1.Columns[col].Visible)
                    {
                        var colName = Gridview1.Columns[col].ToString();

                        if (colName == "Insured Name")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.ItemName = txtValue.Text.ToString();
                        }

                        if (colName == "Date Of Birth")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.DateOfBirth = txtValue.Text.CovertToCustomDateTime();
                        }
                        if (colName == "Passport No")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.Passport = txtValue.Text.ToString();
                        }

                        if (colName == "Occupation")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.OccupationCode = txtValue.Text.ToString();
                        }

                        if (colName == "CPR")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.CPR = txtValue.Text.ToString();
                        }

                        if (colName == "Nationality")
                        {
                            DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.Make = txtValue.SelectedValue.ToString();
                        }

                        if (colName == "Relationship")
                        {
                            DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.Category = txtValue.SelectedValue.ToString();

                            obj.Sex = obj.Category.ToLower() == "son" ? "M" : "F";
                        }
                    }
                }
                objs.Add(obj);
            }
            //}

            return objs;

            #endregion test_1
        }



        #region Travel
        protected void btnPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                   // if (ddlTravelPolicies.SelectedIndex > 0)
                   if(!string.IsNullOrEmpty(txtTravelPolicySearch.Text))
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


                var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyDomesticRequest
                {
                    AgentBranch = userInfo.AgentBranch,
                    AgentCode = userInfo.AgentCode,
                    Agency = userInfo.Agency
                };
                //Get saved policy details by document(policy) number.
                //var docNo = ddlTravelPolicies.SelectedItem.Text.Trim();

                var docNo = txtTravelPolicySearch.Text.Trim();

                var url = BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetSavedQuoteDocumentNo.Replace("{documentNo}", docNo)
                         .Replace("{type}", "portal")
                         .Replace("{agentCode}", userInfo.AgentCode)
                         .Replace("{isendorsement}", "false")
                         .Replace("{endorsementid}", "0");

                var travelDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                    <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (travelDetails.StatusCode == 200 && travelDetails.Result.IsTransactionDone)
                {
                    Update(userInfo, travelDetails);
                }
                else
                {
                    master.ShowErrorPopup(travelDetails.Result.TransactionErrorMessage, "Policy not found !");
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

        private void Update(OAuthTokenResponse userInfo, ApiResponseWrapper<TravelSavedQuotationResponse> travelDetails)
        {
            var res = travelDetails.Result.TravelInsurancePolicyDetails;
            if (res.EndorsementCount > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "error", "ShowEndorsementPopup();", true);
            }
            txtClientCode.Text = res.InsuredCode;
            _TravelId = res.TravelID;
            ddlJourney.SelectedIndex = ddlJourney.Items.IndexOf(ddlJourney.Items.FindByText(res.CoverageType));
            ddlPackage.SelectedIndex = ddlPackage.Items.IndexOf(ddlPackage.Items.FindByText(res.PackageName.ToUpper()));
            ddlPaymentMethod.SelectedIndex = ddlPaymentMethod.Items.IndexOf(ddlPaymentMethod.Items.FindByText(res.PaymentType));
            ddlPeriod.SelectedIndex = ddlPeriod.Items.IndexOf(ddlPeriod.Items.FindByText(res.PolicyPeroidName));
            ddlPhydefect.SelectedIndex = ddlPhydefect.Items.IndexOf(ddlPhydefect.Items.FindByText(res.IsPhysicalDefect));
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(res.AgentBranch));
            //ddlCPR.SelectedIndex = ddlCPR.Items.IndexOf(ddlCPR.Items.FindByText(res.CPR));
            txtCPRSearch.Text = res.CPR;
            txtCPR.Text = res.CPR;
            txtInsuranceFrom.Text = res.InsuranceStartDate.ConvertToLocalFormat();
            txtInsuranceTo.Text = res.ExpiryDate.ConvertToLocalFormat();
            txtInsuredName.Text = res.InsuredName;
            txtPhysicalDesc.Text = res.PhysicalStateDescription;
            txtAccountNo.Text = res.AccountNumber;
            txtRemarks.Text = res.Remarks;

            if (ddlPhydefect.SelectedIndex == 1 || !string.IsNullOrEmpty(res.PhysicalStateDescription))
            {
                phyDefect.Visible = true;
            }
            else
            {
                phyDefect.Visible = false;
            }
            var insured = master.GetInsured(res.CPR, string.Empty, userInfo.Agency, userInfo.AgentCode);

            if (insured != null)
            {
                _DOB = insured.DateOfBirth.ConvertToLocalFormat();
                txtInsuredAge.Text = Convert.ToString(master.CalculateAgeCorrect(insured.DateOfBirth.Value, DateTime.Now));
            }
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
            ShowDiscount(userInfo, res);


            txtDiscount.Text = Convert.ToString(res.CommisionBeforeDiscount - res.CommissionAfterDiscount);

            //Update travel members details on the page.
            if (travelDetails.Result.TravelMembers != null && travelDetails.Result.TravelMembers.Count > 0 && res.PackageName.ToLower() == "family")
            {
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
                int memberIndex = 0;
                for (int i = 0; i < travelDetails.Result.TravelMembers.Count; i++)
                {
                    if (travelDetails.Result.TravelMembers[i].ItemSerialNo != 1)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        dtCurrentTable.Rows[memberIndex]["Insured Name"] = travelDetails.Result.TravelMembers[i].ItemName;
                        dtCurrentTable.Rows[memberIndex]["Relationship"] = travelDetails.Result.TravelMembers[i].Category;
                        dtCurrentTable.Rows[memberIndex]["CPR"] = travelDetails.Result.TravelMembers[i].CPR;
                        dtCurrentTable.Rows[memberIndex]["Date Of Birth"] = travelDetails.Result.TravelMembers[i].DateOfBirth.ConvertToLocalFormat();
                        dtCurrentTable.Rows[memberIndex]["Passport No"] = travelDetails.Result.TravelMembers[i].Passport;
                        dtCurrentTable.Rows[memberIndex]["Nationality"] = travelDetails.Result.TravelMembers[i].Make;
                        dtCurrentTable.Rows[memberIndex]["Occupation"] = travelDetails.Result.TravelMembers[i].OccupationCode;
                        memberIndex++;
                    }
                }
                ViewState["CurrentTable"] = dtCurrentTable;
                Gridview1.DataSource = dtCurrentTable;
                Gridview1.DataBind();
                admindetails.Visible = true;
                SetPreviousData();
            }
            else
            {
                ViewState["CurrentTable"] = null;
                Gridview1.DataSource = null;
                Gridview1.DataBind();
                admindetails.Visible = false;
            }
            EnableAuthorize(travelDetails.Result.TravelInsurancePolicyDetails.ISHIR, travelDetails.Result.TravelInsurancePolicyDetails.HIRStatus);
            if (res.IsActivePolicy)
            {
                SetScheduleHRef(txtTravelPolicySearch.Text.Trim(), Constants.Travel, userInfo);
                //If it is authorized policy need to disable all the page controls.
                master.makeReadOnly(GetContentControl(), false);
            }
            else
            {
                RemoveScheduleHRef();
                master.makeReadOnly(GetContentControl(), true);
            }
            SetReadOnlyControls();
           // SetDependantDOB();
            SetCoverage(res.IsActivePolicy);
            EnablePaymentValidator();
        }

        protected void ddlPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();                

                ValidateAge(master.GetInsured(txtCPR.Text, string.Empty, userInfo.Agency, userInfo.AgentCode));
                PackageDetailsChanges();
                DisableControls();               
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

        private void PackageDetailsChanges()
        {
            if (ddlPackage.SelectedItem.Text.ToLower() == "family")
            {
                SetInitialRow();
                admindetails.Visible = true;
                ddlJourney.SelectedIndex = 1;
                ddlJourney.Enabled = false;
                rfvddlJourney.Enabled = true;
            }
            else if (ddlPackage.SelectedItem.Text.ToLower() == "individual")
            {
                admindetails.Visible = false;
                ddlJourney.SelectedIndex = -1;
                ddlJourney.Enabled = true;
                rfvddlJourney.Enabled = true;
            }
            else if (ddlPackage.SelectedItem.Text.ToLower() == "schengen")
            {
                admindetails.Visible = false;
                ddlJourney.SelectedIndex = -1;
                ddlJourney.Enabled = false;
                rfvddlJourney.Enabled = false;
            }
        }    

        protected void ddlJourney_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisableControls();           
        }       

        protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtInsuranceFrom.Text))
                {
                    txtInsuranceTo.Text = getExpiryDate(ddlPeriod.SelectedItem.Value, txtInsuranceFrom.Text);
                }               
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

        protected void calculate_expiredate(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ddlPeriod.SelectedItem.Value))
                {
                    txtInsuranceTo.Text = getExpiryDate(ddlPeriod.SelectedItem.Value, txtInsuranceFrom.Text);
                }               
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

        protected void ddlPhydefect_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPhydefect.SelectedValue == "Yes")
                {
                    txtPhysicalDesc.Text = string.Empty;
                    phyDefect.Visible = true;
                }
                else
                {
                    phyDefect.Visible = false;
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

        protected void ddlTravelPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlTravelPolicies.SelectedIndex == 0)
            //{
            //    master.ClearControls(GetContentControl());
            //    SetReadOnlyControls();
            //}
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Insured Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Relationship", typeof(string)));
            dt.Columns.Add(new DataColumn("CPR", typeof(string)));
            dt.Columns.Add(new DataColumn("Date Of Birth", typeof(string)));

            dt.Columns.Add(new DataColumn("Passport No", typeof(string)));
            dt.Columns.Add(new DataColumn("Nationality", typeof(string)));
            dt.Columns.Add(new DataColumn("Occupation", typeof(string)));

            dr = dt.NewRow();

            dr["Insured Name"] = string.Empty;
            dr["Relationship"] = string.Empty;
            dr["CPR"] = string.Empty;
            dr["Date Of Birth"] = string.Empty;
            dr["Passport No"] = string.Empty;
            dr["Nationality"] = string.Empty;
            dr["Occupation"] = string.Empty;

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
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox txName = (TextBox)Gridview1.Rows[rowIndex].Cells[0].FindControl("txtMemberName");
                        DropDownList ddlRelation = (DropDownList)Gridview1.Rows[rowIndex].Cells[1].FindControl("ddlRelation");
                        TextBox txOccupation = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtOccupation");
                        TextBox txDOB = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("txtDOB");
                        TextBox txPassport = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtPassport");
                        DropDownList ddlNation = (DropDownList)Gridview1.Rows[rowIndex].Cells[5].FindControl("ddlNational");
                        //TextBox txOccupation = (TextBox)Gridview1.Rows[rowIndex].Cells[6].FindControl("txtOccupation");

                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i - 1]["Insured Name"] = txName.Text;
                        dtCurrentTable.Rows[i - 1]["Relationship"] = ddlRelation.SelectedItem.Text;
                        //dtCurrentTable.Rows[i - 1]["CPR"] = txCPR.Text;
                        dtCurrentTable.Rows[i - 1]["Date Of Birth"] = txDOB.Text;
                        dtCurrentTable.Rows[i - 1]["Passport No"] = txPassport.Text;
                        dtCurrentTable.Rows[i - 1]["Nationality"] = ddlNation.SelectedItem.Text;
                        dtCurrentTable.Rows[i - 1]["Occupation"] = txOccupation.Text;

                        rowIndex++;
                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    Gridview1.DataSource = dtCurrentTable;
                    Gridview1.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreviousData();
           // SetDependantDOB();
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
                        TextBox txName = (TextBox)Gridview1.Rows[rowIndex].Cells[0].FindControl("txtMemberName");
                        DropDownList ddlRelation = (DropDownList)Gridview1.Rows[rowIndex].Cells[1].FindControl("ddlRelation");
                        TextBox txtOccupation = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtOccupation");
                        TextBox txDOB = (TextBox)Gridview1.Rows[rowIndex].Cells[3].FindControl("txtDOB");
                        TextBox txPassport = (TextBox)Gridview1.Rows[rowIndex].Cells[4].FindControl("txtPassport");
                        DropDownList ddlNation = (DropDownList)Gridview1.Rows[rowIndex].Cells[5].FindControl("ddlNational");
                        // DropDownList ddlOccupation = (DropDownList)Gridview1.Rows[rowIndex].Cells[6].FindControl("ddlTravelOccupation");

                        txName.Text = dt.Rows[i]["Insured Name"].ToString();
                        ddlRelation.SelectedIndex = ddlRelation.Items.IndexOf(ddlRelation.Items.FindByText(dt.Rows[i]["Relationship"].ToString()));
                        //txCPR.Text=dt.Rows[i]["CPR"].ToString();
                        txDOB.Text = dt.Rows[i]["Date Of Birth"].ToString();
                        txPassport.Text = dt.Rows[i]["Passport No"].ToString();
                        ddlNation.SelectedIndex = ddlNation.Items.IndexOf(ddlNation.Items.FindByValue(dt.Rows[i]["Nationality"].ToString()));
                        txtOccupation.Text = dt.Rows[i]["Occupation"].ToString();
                        //  ddlOccupation.SelectedIndex = ddlOccupation.Items.IndexOf(ddlOccupation.Items.FindByText(dt.Rows[i]["Occupation"].ToString()));

                        rowIndex++;
                    }
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

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                DisablePaymentValidator();               
                if (Page.IsValid)
                {
                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    var travelQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuote();
                    var Insured = master.GetInsured(txtCPR.Text, string.Empty, userInfo.Agency, userInfo.AgentCode);

                    if (Insured != null && !ValidateInsured(Insured) || !ValidateAge(Insured) || !ValidateDependant(userInfo)
                      || ddlPackage.SelectedItem.Value == "-1" && ddlPeriod.SelectedItem.Value == "-1")
                    {
                        return;
                    }
                    if (Insured != null)
                    {
                        _DOB = Insured.DateOfBirth.ConvertToLocalFormat();
                    }

                    //Get travel quote for the given values.
                    travelQuote.Agency = userInfo.Agency;
                    travelQuote.AgentCode = userInfo.AgentCode;
                    travelQuote.MainClass = string.IsNullOrEmpty(MainClass) ? "MISC" : MainClass;
                    travelQuote.SubClass = ddlPackage.SelectedItem.Value == "STS" ? "STI" : ddlPackage.SelectedItem.Value;
                    travelQuote.DateOfBirth = _DOB.CovertToCustomDateTime();
                    travelQuote.PolicyPeriodCode = ddlPeriod.SelectedItem.Value;
                    travelQuote.PackageCode = ddlPackage.SelectedItem.Text.ToLower() == "individual" ?
                                              "IN001" : ddlPackage.SelectedItem.Text.ToLower() == "schengen" ?
                                              "SCHEN" : "FM001";

                    travelQuote.CoverageType = ddlPackage.SelectedItem.Text.ToLower() == "schengen" ?
                                               "SCHENGEN" : ddlJourney.SelectedItem.Value;

                    var travelQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                            <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuoteResponse>,
                                            BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuote>
                                            (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetQuote, travelQuote);

                    if (travelQuoteResult.StatusCode == 200 && travelQuoteResult.Result.IsTransactionDone)
                    {
                        calculatedPremium.Value = travelQuoteResult.Result.Premium.ToString();
                        var commisionRequest = new CommissionRequest
                        {
                            AgentCode = userInfo.AgentCode,
                            Agency = userInfo.Agency,
                            SubClass = ddlPackage.SelectedItem.Value == "STS" ? "STI" : ddlPackage.SelectedItem.Value,
                            PremiumAmount = travelQuoteResult.Result.Premium,
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
                            ShowPremium(userInfo, travelQuoteResult.Result.Premium, commissionresult.Result.CommissionAmount);
                        }
                        else
                        {
                            master.ShowLoading = false;
                            master.ShowErrorPopup(commissionresult.Result.TransactionErrorMessage, "Request Failed !");
                            return;
                        }
                    }
                    else
                    {
                        master.ShowLoading = false;
                        master.ShowErrorPopup(travelQuoteResult.Result.TransactionErrorMessage, "Request Failed !");
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                //throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void btnTravelSave_Click(object sender, EventArgs e)
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
            catch (System.Exception ex)
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
        protected void imgbtnNewClientCd_Click(object sender, EventArgs e)
        {
            Response.Redirect("InsuredMaster.aspx");
        }
        private TravelMembers GetIndividual(out string mobileNo)
        {

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var tmember = new TravelMembers();

            var insured = new BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest
            {
                CPR = txtCPRSearch.Text.Trim(),
                InsuredCode = txtClientCode.Text.Trim(),
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode
            };

            var travelResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest>
                               (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchUserDetailsByCPRInsuredCode, insured);

            tmember.TravelID = 0;
            tmember.DocumentNo = "";
            tmember.ItemSerialNo = 1;
            tmember.ItemName = travelResult.Result.InsuredDetails.FirstName + " " + travelResult.Result.InsuredDetails.MiddleName + " " + travelResult.Result.InsuredDetails.LastName;
            tmember.SumInsured = 0;
            tmember.ForeignSumInsured = 50000;
            tmember.Category = "";
            tmember.Title = "";
            tmember.Sex = travelResult.Result.InsuredDetails.Gender.ToLower() == "male" ? "M" : "F";
            tmember.DateOfBirth = travelResult.Result.InsuredDetails.DateOfBirth;
            tmember.PremiumAmount = 0;
            tmember.Make = travelResult.Result.InsuredDetails.Nationality;
            tmember.OccupationCode = travelResult.Result.InsuredDetails.Occupation;
            tmember.CPR = travelResult.Result.InsuredDetails.CPR;
            tmember.Passport = travelResult.Result.InsuredDetails.PassportNo;
            tmember.FirstName = travelResult.Result.InsuredDetails.FirstName;
            tmember.LastName = travelResult.Result.InsuredDetails.LastName;
            tmember.MiddleName = travelResult.Result.InsuredDetails.MiddleName;
            tmember.CreatedBy = 0;
            tmember.UpdatedBy = 0;
            mobileNo = travelResult.Result.InsuredDetails.Mobile;
            return tmember;
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

                if (Relationdt != null && Relationdt.Rows.Count > 0)
                {
                    DropDownList ddlRelationship = (DropDownList)e.Row.FindControl("ddlRelation");
                    ddlRelationship.DataValueField = "Relationship";
                    ddlRelationship.DataTextField = "Relationship";
                    ddlRelationship.DataSource = Relationdt;
                    ddlRelationship.DataBind();
                    ddlRelationship.Items.Insert(0, new ListItem("--Please Select--", ""));
                }

                //if (Genderdt != null && Genderdt.Rows.Count > 0)
                //{
                //    DropDownList ddlGender = (DropDownList)e.Row.FindControl("ddlDependentGender");
                //    ddlGender.DataValueField = "Value";
                //    ddlGender.DataTextField = "Text";
                //    ddlGender.DataSource = Genderdt;
                //    ddlGender.DataBind();
                //    ddlGender.Items.Insert(0, new ListItem("--Please Select--", ""));
                //}
            }

            //if (drv != null && !string.IsNullOrEmpty(drv.Row["MAKE"].ToString()))
            //{
            //    DropDownList ddlNationality = (DropDownList)e.Item.FindControl("ddlNationalityDependent");
            //    ddlNationality.SelectedValue = Convert.ToString(drv.Row["MAKE"]);

            //}
            //if (drv != null && !string.IsNullOrEmpty(drv.Row["CATEGORY"].ToString()))
            //{
            //    DropDownList ddlRelationship = (DropDownList)e.Item.FindControl("ddlRelationship");
            //    ddlRelationship.SelectedValue = Convert.ToString(drv.Row["CATEGORY"]);
            //}
            //if (drv != null && !string.IsNullOrEmpty(drv.Row["SEX"].ToString()))
            //{
            //    DropDownList ddlGender = (DropDownList)e.Item.FindControl("ddlDependentGender");
            //    ddlGender.SelectedValue = Convert.ToString(drv.Row["SEX"]);
            //}

            //}
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
                    btnTravelSave.Enabled = true;
                    btnAuthorize.Enabled = true;
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

        #endregion Travel

        public void SaveAuthorize(bool isSave)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var travelInsurnacePolicy = new TravelInsurancePolicy();
                var travelmembers = new TravelMembers();

                travelInsurnacePolicy.AgentBranch = userInfo.AgentBranch;
                travelInsurnacePolicy.AgentCode = userInfo.AgentCode;
                travelInsurnacePolicy.Agency = userInfo.Agency;
                
                var Insured = master.GetInsured(txtCPR.Text, string.Empty, userInfo.Agency, userInfo.AgentCode);

                if (Insured != null && !ValidateInsured(Insured) || !ValidateAge(Insured) || !ValidateDependant(userInfo)
                    || ddlPackage.SelectedItem.Value == "-1" && ddlPeriod.SelectedItem.Value == "-1")
                {
                    return;
                }

                //Get travel quote for the given values.
                var travelQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuote
                {
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    PolicyPeriodCode = ddlPeriod.SelectedItem.Value,
                    DateOfBirth = Insured.DateOfBirth.Value,
                    MainClass = string.IsNullOrEmpty(MainClass) ? "MISC" : MainClass,
                    SubClass = ddlPackage.SelectedItem.Value == "STS" ? "STI" : ddlPackage.SelectedItem.Value,

                    PackageCode = ddlPackage.SelectedItem.Text.ToLower() == "individual" ?
                                          "IN001" : ddlPackage.SelectedItem.Text.ToLower() == "schengen" ?
                                           "SCHEN" : "FM001",
                    CoverageType = ddlPackage.SelectedItem.Text.ToLower() == "schengen" ?
                                           "SCHENGEN" : ddlJourney.SelectedItem.Value
                };

                var travelQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuoteResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuote>
                                       (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetQuote, travelQuote);

                decimal PremiumBeforeDiscount = decimal.Zero;
                decimal PremiumAfterDiscount = decimal.Zero;
                if (travelQuoteResult.StatusCode == 200 && travelQuoteResult.Result.IsTransactionDone)
                {
                    PremiumBeforeDiscount = travelQuoteResult.Result.Premium;
                    PremiumAfterDiscount = travelQuoteResult.Result.DiscountPremium;

                    var commisionRequest = new CommissionRequest();
                    commisionRequest.AgentCode = userInfo.AgentCode;
                    commisionRequest.Agency = userInfo.Agency;
                    commisionRequest.SubClass = ddlPackage.SelectedItem.Value == "STS" ? "STI" : ddlPackage.SelectedItem.Value;
                    commisionRequest.PremiumAmount = travelQuoteResult.Result.Premium;
                    commisionRequest.IsDeductable = true;

                    var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                           BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>
                                           (BKIC.SellingPoint.DTO.Constants.CommissionURI.CalculateCommission, commisionRequest);

                    if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone
                        && commissionresult.Result.CommissionAmount >= 0)
                    {
                        // travelInsurnacePolicy.CommissionAmount = commissionresult.Result.CommissionAmount;
                        if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                            || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
                        {
                            if (Convert.ToDecimal(premiumAmount.Text) < PremiumBeforeDiscount || AjdustedPremium)
                            {
                                travelInsurnacePolicy.UserChangedPremium = true;
                                travelInsurnacePolicy.PremiumAfterDiscount = Convert.ToDecimal(premiumAmount.Text);
                                var diff = PremiumBeforeDiscount - travelInsurnacePolicy.PremiumAfterDiscount;
                                travelInsurnacePolicy.CommissionAfterDiscount = commissionresult.Result.CommissionAmount - diff;
                            }
                        }
                        else if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.User)
                        {
                            if (Convert.ToDecimal(premiumAmount1.Text) < PremiumBeforeDiscount || AjdustedPremium)
                            {
                                travelInsurnacePolicy.UserChangedPremium = true;
                                travelInsurnacePolicy.PremiumAfterDiscount = Convert.ToDecimal(premiumAmount1.Text);
                                var diff = PremiumBeforeDiscount - travelInsurnacePolicy.PremiumAfterDiscount;
                                travelInsurnacePolicy.CommissionAfterDiscount = commissionresult.Result.CommissionAmount - diff;
                            }
                        }
                    }
                }

                //Get travel member details.
                List<TravelMembers> members = new List<TravelMembers>();
                TravelMembers individualdt = GetIndividual(out string mobileNo);

                //Update the quote values to current instance.
                individualdt.PremiumAmount = PremiumBeforeDiscount;
                travelInsurnacePolicy.DOB = travelQuote.DateOfBirth;
                travelInsurnacePolicy.PackageCode = ddlPackage.SelectedItem.Text.ToLower() == "individual" ?
                                                    "IN001" : ddlPackage.SelectedItem.Text.ToUpper() == "SCHENGEN" ?
                                                    "SCHEN" : "FM001";
                //travelInsurnacePolicy.PolicyPeroidYears = ddlPeriod.SelectedItem.Value == "AN001" ? 1 : 2;
                travelInsurnacePolicy.PolicyPeroidYears = ddlPeriod.SelectedItem.Value == "AN001" ? 1 : 2;
                //travelInsurnacePolicy.CoverageType = ddlJourney.SelectedItem.Value == "SCHEN" ? "SCHENGEN" : ddlJourney.SelectedItem.Value;
                travelInsurnacePolicy.CoverageType = ddlPackage.SelectedItem.Text.ToLower() == "schengen" ?
                                                     "SCHENGEN" : ddlJourney.SelectedItem.Value;
                travelInsurnacePolicy.InsuredCode = txtClientCode.Text.Trim();
                travelInsurnacePolicy.InsuredName = txtInsuredName.Text;
                travelInsurnacePolicy.SumInsured = individualdt.SumInsured;
                travelInsurnacePolicy.PremiumAmount = PremiumBeforeDiscount;
                travelInsurnacePolicy.InsuranceStartDate = txtInsuranceFrom.Text.CovertToCustomDateTime();
                travelInsurnacePolicy.MainClass = MainClass;
                travelInsurnacePolicy.SubClass = ddlPackage.SelectedItem.Value;
                travelInsurnacePolicy.Passport = individualdt.Passport;
                travelInsurnacePolicy.Renewal = 'N';
                travelInsurnacePolicy.Occupation = individualdt.OccupationCode;
                travelInsurnacePolicy.PeroidOfCoverCode = ddlPeriod.SelectedItem.Value;
                travelInsurnacePolicy.DiscountAmount = PremiumAfterDiscount;
                travelInsurnacePolicy.CPR = txtCPRSearch.Text.Trim();//ddlCPR.SelectedItem.Text;
                travelInsurnacePolicy.Mobile = mobileNo;
                travelInsurnacePolicy.QuestionaireCode = "QST_STP_002";
                travelInsurnacePolicy.IsPhysicalDefect = ddlPhydefect.SelectedItem.Value;
                travelInsurnacePolicy.PhysicalStateDescription = ddlPhydefect.SelectedItem.Value == "Yes" ? txtPhysicalDesc.Text : string.Empty;
                travelInsurnacePolicy.PaymentType = ddlPaymentMethod.SelectedItem.Value;
                //travelInsurnacePolicy.CreatedBy = Convert.ToInt32(userInfo.UserId);
                travelInsurnacePolicy.Source = ddlBranch.SelectedItem.Value;
                travelInsurnacePolicy.AgentBranch = ddlBranch.SelectedItem.Value;
                travelInsurnacePolicy.PremiumBeforeDiscount = PremiumBeforeDiscount;
                travelInsurnacePolicy.PaymentType = ddlPaymentMethod.SelectedIndex > 0 ? ddlPaymentMethod.SelectedItem.Text : "";
                travelInsurnacePolicy.Remarks = txtRemarks.Text.Trim();
                travelInsurnacePolicy.AccountNumber = txtAccountNo.Text.Trim();
                travelInsurnacePolicy.IsSaved = isSave;
                travelInsurnacePolicy.IsActivePolicy = !isSave;
                
                if (ddlPackage.SelectedItem.Value == "FM001" || ddlPackage.SelectedItem.Value == "STP")
                {
                    //***** Get GetFamilyDependentDetails *****
                    members = GetFamilyDetails();
                }
                members.Add(individualdt);

                //Insert or update the travel policy.
                if (_TravelId > 0)
                    travelInsurnacePolicy.TravelID = _TravelId;

                var traveldetails = new BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelPolicy
                {
                    TravelInsurancePolicyDetails = travelInsurnacePolicy,
                    TravelMembers = members
                };
                traveldetails.TravelInsurancePolicyDetails.Agency = userInfo.Agency;
                traveldetails.TravelInsurancePolicyDetails.AgentCode = userInfo.AgentCode;                
                traveldetails.TravelInsurancePolicyDetails.CreatedBy = ddlUsers.SelectedIndex > 0 ?
                                                                       Convert.ToInt32(ddlUsers.SelectedItem.Value) : Convert.ToInt32(userInfo.ID);

                var postData = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelPolicyResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelPolicy>
                               (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.PostTravel, traveldetails);

                if (postData.StatusCode == 200 && postData.Result.IsTransactionDone)
                {
                    _TravelId = postData.Result.TravelId;
                    LoadAgencyClientPolicyInsuredCode(userInfo, service);                    
                    txtTravelPolicySearch.Text = postData.Result.DocumentNo;
                    modalBodyText.InnerText = GetMessageText(postData.Result.IsHIR, traveldetails.TravelInsurancePolicyDetails.IsActivePolicy, postData.Result.DocumentNo);
                    if (traveldetails.TravelInsurancePolicyDetails.IsActivePolicy)
                    {
                        SetScheduleHRef(postData.Result.DocumentNo, Constants.Travel, userInfo);
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowPopup();", true);                   
                }
                else
                {
                    master.ShowErrorPopup(postData.Result.TransactionErrorMessage, "Request Failed !");
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

        public void SetDependantDOB()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();           

            int spouseCount = 0;

            foreach (GridViewRow row in Gridview1.Rows)
            {
                DropDownList relation = (DropDownList)row.FindControl("ddlRelation");
                if (relation.SelectedIndex > 0)
                {
                    //More than one spouse is not allowed for TISCO..
                    if(userInfo.Agency == "TISCO")
                    {
                        if (relation.SelectedIndex == 3)
                        {
                            spouseCount++;
                        }
                        if (spouseCount > 1)
                        {
                            relation.SelectedIndex = 0;
                            master.ShowErrorPopup("More than one spouse not allowed !", "Validation Faild");
                            return;
                        }
                    }                   
                    TextBox dob = (TextBox)row.FindControl("txtDOB");
                    var id = dob.ClientID;
                    string selectedDOB = dob.Text;
                    dob.Text = "";
                    dob.Text = selectedDOB;
                    dob.Enabled = false;
                    dob.ToolTip = "For edit DOB, please change the relationship";
                }
            }
        }

        protected void Gridview1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (e.RowIndex > 0)
                {
                    int index = Convert.ToInt32(e.RowIndex);
                    ViewState["CurrentTable"] = GetTraveMembersDataTable();
                    DataTable dt = ViewState["CurrentTable"] as DataTable;
                    dt.Rows[index].Delete();
                    ViewState["CurrentTable"] = dt;
                    Gridview1.DataSource = dt;
                    Gridview1.DataBind();
                    SetPreviousData();                   
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

        public DataTable GetTraveMembersDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Insured Name");
            table.Columns.Add("Relationship");
            table.Columns.Add("CPR");
            table.Columns.Add("Date Of Birth");
            table.Columns.Add("Passport No");
            table.Columns.Add("Nationality");
            table.Columns.Add("Occupation");

            for (int row = 1; row <= Gridview1.Rows.Count; row++)
            {
                //DataRow TempRow = TempTable.NewRow();
                var obj = new TravelMembers
                {
                    ItemSerialNo = row + 1,
                    ForeignSumInsured = 50000,
                    SumInsured = 18900
                };
                string dob = "";

                for (int col = 0; col < Gridview1.Columns.Count; col++)
                {
                    if (Gridview1.Columns[col].Visible)
                    {
                        var colName = Gridview1.Columns[col].ToString();

                        if (colName == "Insured Name")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.ItemName = txtValue.Text.ToString();
                        }

                        if (colName == "Date Of Birth")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            dob = txtValue.Text;
                        }
                        if (colName == "Passport No")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.Passport = txtValue.Text.ToString();
                        }

                        if (colName == "Occupation")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.OccupationCode = txtValue.Text.ToString();
                        }

                        if (colName == "CPR")
                        {
                            TextBox txtValue = (TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.CPR = txtValue.Text.ToString();
                        }

                        if (colName == "Nationality")
                        {
                            DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.Make = txtValue.SelectedValue.ToString();
                        }

                        if (colName == "Relationship")
                        {
                            DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.Category = txtValue.SelectedValue.ToString();
                        }
                        if (colName == "Sex")
                        {
                            DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                            obj.Sex = txtValue.SelectedValue.ToString();
                        }
                    }
                }

                table.Rows.Add(obj.ItemName, obj.Category, obj.CPR, dob, obj.Passport, obj.Make, obj.OccupationCode);
            }

            return table;
        }

        public void ShowPremium(OAuthTokenResponse userInfo, decimal Premium, decimal Commission)
        {
            amtDisplay.Visible = true;
            btnTravelSave.Visible = true;
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
        }

        protected void ddlRelation_Changed(object sender, EventArgs e)
        {
            //SetDependantDOB();

            //DropDownList ddlRelation = (DropDownList)sender;
            //GridViewRow row = (GridViewRow)ddlRelation.NamingContainer;
            //int statusID = Convert.ToInt32(ddlRelation.SelectedIndex);
            ////Give an ID to the Hyperlink Control and find it here

            //TextBox txtBox = (TextBox)row.FindControl("txtDOB");
            //txtBox.Enabled = true;
            //txtBox.ToolTip = "";
            //string dob = txtBox.Text;
            ////txtBox.Text = DateTime.Now.CovertToLocalFormat();

            //var id = txtBox.ClientID;
            //txtBox.Text = "";
            //if (ddlRelation != null && txtBox != null && (ddlRelation.SelectedIndex == 1 || ddlRelation.SelectedIndex == 2))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "Set21Years(" + id + ");", true);
            //}
            //else if (ddlRelation != null && txtBox != null)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "Set100Years(" + id + ");", true);
            //}          
            //txtBox.Text = dob;
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
                btnOK.Text = "OK";
                btnAuthorize.Visible = false;
                return "Your travel policy is saved and moved into HIR: " + docNo;
            }
            else if (!isHIR && !isActivePolicy)
            {
                btnYes.Visible = false;
                btnOK.Text = "OK";
                btnAuthorize.Enabled = true;
                btnAuthorize.Visible = true;
                return "Your travel policy has been saved successfully: " + docNo;
            }
            else if (isActivePolicy)
            {
                master.makeReadOnly(GetContentControl(), false);
                btnCalculate.Enabled = false;
                btnTravelSave.Enabled = false;
                btnYes.Visible = false;
                btnOK.Text = "OK";
                btnAuthorize.Enabled = false;
                return "Your travel policy has been authorized successfully: " + docNo;
            }
            else
                return string.Empty;
        }

        public void SetReadOnlyControls()
        {
            txtCPR.Enabled = false;
            txtClientCode.Enabled = false;
            txtInsuredName.Enabled = false;           
            premiumAmount.Enabled = false;
            premiumAmount1.Enabled = false;
            commission.Enabled = false;
            commission1.Enabled = false;
            btnBack.Enabled = true;
            txtDiscount1.Enabled = false;
        }

        private void EnablePaymentValidator()
        {
            rfvddlPaymentMethod.Enabled = true;
            if (ddlPaymentMethod.SelectedIndex == 1)
            {
                txtAccountNo.Enabled = false;
                rfvtxtAccountNo.Enabled = false;
            }
            else
            {
                txtAccountNo.Enabled = true;
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
                //throw ex;
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
            // btnSubmit.Enabled = true;
            btnCalculate.Enabled = true;
            btnBack.Visible = true;
            btnTravelSave.Visible = false;
            btnCalculate.Visible = true;

            btnAuthorize.Visible = false;
            downloadschedule.Visible = false;

            //ddlCPR.SelectedIndex = 0;
            txtCPRSearch.Text = string.Empty;
            //ddlTravelPolicies.SelectedIndex = 0;
            txtTravelPolicySearch.Text = string.Empty;

            _TravelId = 0;

            ViewState["CurrentTable"] = null;
            Gridview1.DataSource = null;
            Gridview1.DataBind();
            admindetails.Visible = false;
        }

        public void ShowDiscount(OAuthTokenResponse userInfo, TravelInsurancePolicy policy)
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
    }
}