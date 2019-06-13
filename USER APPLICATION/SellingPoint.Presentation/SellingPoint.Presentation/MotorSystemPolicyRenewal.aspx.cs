using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using BKIC.SellingPoint.Presentation;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SellingPoint.Presentation
{
    public partial class MotorSystemPolicyRenewal : System.Web.UI.Page
    {
        private General master;
        public static long _MotorID { get; set; }
        public static decimal ClaimAmount { get; set; }
        public static int _RenewalCount { get; set; }
        public static string MainClass { get; set; }
        public static bool AjdustedPremium { get; set; }       
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails> InsuredNames { get; set; }
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorPolicy> policyList;
        public static DataTable GeneralMake { get; set; }
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCovers> OptionalCovers { get; set; }        

        public MotorSystemPolicyRenewal()
        {
            master = Master as General;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!Page.IsPostBack)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                SetInitialRow();
                DisableDefultControls(userInfo);
                BindDropdown(userInfo, service);
                LoadUsers(userInfo, service);
                LoadAgencyClientCode(userInfo, service);
                QueryStringMethods(userInfo, service);

                //ClaimService.GETCLAIMCOUNTPortTypeClient soapclient = new ClaimService.GETCLAIMCOUNTPortTypeClient("GETCLAIMCOUNTPort");
                //using (OperationContextScope scope = new OperationContextScope(soapclient.InnerChannel))
                //{
                //    var httpRequestProperty = new HttpRequestMessageProperty();
                //    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                //    Convert.ToBase64String(Encoding.ASCII.GetBytes("WEBSERVICEUSER" + ":" + "WEBSERVICEUSER"));
                //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                //    var result = new ClaimService.OCLAIMCOUNTNUMBEROUT();
                //    var claimCount = soapclient.GETCLAIMCOUNT(result, "PPRD22014-44", 0);
                //    //var claimCount = soapclient.GETCLAIMCOUNT(result, "PPRD22014-17", 0);
                //    if (claimCount > 0)
                //    {
                //        master.ShowClaimPopup("This policy already has a claim, Please contact GIG Admin !");
                //        return;
                //    }
                //}                
            }
        }

        public void DisableDefultControls(OAuthTokenResponse userInfo)
        {
            amtDisplay.Visible = false;
            downloadproposal.Visible = false;
            downloadschedule.Visible = false;
            downloadCertificate.Visible = false;
            btnAuthorize.Visible = false;
            btnSubmit.Visible = false;
            ButtonAddNewCover.Visible = false;
            newadmindetails.Visible = false;
            rfvxtSeatingCapcity.Enabled = false;
            _MotorID = 0;
            AjdustedPremium = false;
            txtIssueDate.Text = DateTime.Now.CovertToLocalFormat();
            divPaymentSection.Visible = userInfo.IsShowPayments;
            ClaimAmount = 0;
            _RenewalCount = 0;            
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

            LoadAgencyClientRenewalPolicy(userInfo, service, includeHIR != null ? Convert.ToBoolean(includeHIR) : false);

            if (cpr != null)
            {
                string CPR = Convert.ToString(cpr);               
                txtCPR.Text = CPR;
            }
            if (dob != null)
            {
                var dateofbirth = DateTime.ParseExact(dob, "dd/MM/yyyy", null);
                insuredDOB.Value = dateofbirth.CovertToLocalFormat();
                txtAge.Text = Convert.ToString(CalculateAgeCorrect(dateofbirth, DateTime.Now));
            }          
            if (userInfo.Agency == "BBK")
            {
                underBCFC.Visible = false;
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

        protected void insured_Master(object sender, EventArgs e)
        {
            Response.Redirect("InsuredMaster.aspx?type=" + 4);
        }

        protected void ddlMotorPolicy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlMotorPolicies.SelectedIndex == 0)
            //{
            //    master.ClearControls(GetContentControl());
            //    SetReadOnlyControls();
            //    HidePremium();
            //}
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

        protected void btnPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                   // if (ddlMotorPolicies.SelectedIndex > 0)
                   if(!string.IsNullOrEmpty(txtMotorRenewalPolicySearch.Text))
                    {

                        master.ClearControls(GetContentControl());
                        SetReadOnlyControls();
                        HidePremium();
                        GetRenewalPolicyData();
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

        public void GetRenewalPolicyData()
        {
            try
            {
                OAuthTokenResponse userInfo;
                DataServiceManager service;
                ApiResponseWrapper<MotorSavedQuotationResponse> motorDetails;
                GetRenewalPolicy(out userInfo, out service, out motorDetails);

                //Update policy details on current page for dispaly the details.
                if (motorDetails.StatusCode == 200 && motorDetails.Result.IsTransactionDone)
                {
                    decimal claimAmount = 0;
                    ClaimAmount = claimAmount;
                    //When application goes to live enable below lines..
                    //ClaimAmount.GETCLAIMAMOUNTPortTypeClient soapclient = new ClaimAmount.GETCLAIMAMOUNTPortTypeClient("GETCLAIMAMOUNTPort");
                    //using (OperationContextScope scope = new OperationContextScope(soapclient.InnerChannel))
                    //{
                    //    var httpRequestProperty = new HttpRequestMessageProperty();
                    //    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                    //    Convert.ToBase64String(Encoding.ASCII.GetBytes("WEBSERVICEUSER" + ":" + "WEBSERVICEUSER"));
                    //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    //    var result = new ClaimAmount.OCLAIMAMOUNTNUMBEROUT();

                    //    claimAmount = Convert.ToDecimal(soapclient.GETCLAIMAMOUNT(result, "PMCTR2017-11", 1));
                    //    ClaimAmount = claimAmount;
                    //    if (claimAmount > 0)
                    //    {
                    //        master.ShowClaimPopup("This policy already has a claim, The claim amount is :" + ClaimAmount);                           
                    //    }
                    //}
                    Update(userInfo, service, motorDetails, motorDetails.Result.MotorPolicyDetails, claimAmount);
                }
                else
                {
                    master.ShowErrorPopup(motorDetails.Result.TransactionErrorMessage, "Request Failed !");
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

        private void GetRenewalPolicy(out OAuthTokenResponse userInfo, out DataServiceManager service,
            out ApiResponseWrapper<MotorSavedQuotationResponse> motorDetails)
        {
            master.IsSessionAvailable();
            userInfo = CommonMethods.GetUserDetails();
            service = CommonMethods.GetLogedInService();

            var renewalCount = GetPolicyRenewalCount(userInfo, service);

            //If the policy is already renewed show to the information to the user.
            RenewalPreCheck(userInfo, service, renewalCount);


            //var policyRenewalCount = ddlMotorPolicies.SelectedItem.Value.Substring(0, ddlMotorPolicies.SelectedValue.IndexOf("-", 0));
            //var policyRenewalCount = string.IsNullOrEmpty(renewalCount.Value) ? Convert.ToString(0) : renewalCount.Value;
           

            //Get saved policy details by document(policy) number.
            var url = BKIC.SellingPoint.DTO.Constants
                     .MotorURI.GetMotorRenewalPolicyByDocNo
                     //.Replace("{documentNo}", ddlMotorPolicies.SelectedItem.Text.Trim())
                     .Replace("{documentNo}", txtMotorRenewalPolicySearch.Text.Trim())
                     .Replace("{agentCode}", userInfo.AgentCode)
                     .Replace("{agency}", userInfo.Agency)
                     .Replace("{renewalCount}", renewalCount.ToString());

            motorDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorSavedQuotationResponse>>(url);
        }


        public int GetPolicyRenewalCount(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            int renewalCount = 0;
            var motorreq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                AgentBranch = userInfo.AgentBranch,
                IncludeHIR = false,
                IsRenewal = true,
                DocumentNo = txtMotorRenewalPolicySearch.Text.Trim()
            };

            //Get PolicyNo by Agency
            var motorPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorPolicyResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest>
                                (BKIC.SellingPoint.DTO.Constants.MotorURI.GetMotorAgencyPolicy, motorreq);

            if (motorPolicies.StatusCode == 200 && motorPolicies.Result.IsTransactionDone
                && motorPolicies.Result.AgencyMotorPolicies.Count > 0)
            {
                renewalCount = motorPolicies.Result.AgencyMotorPolicies[0].RenewalCount;
            }
            return renewalCount;

        }


        public void RenewalPreCheck(OAuthTokenResponse userInfo, DataServiceManager service, int renewalCount)
        {
           // var policyRenewalCount = string.IsNullOrEmpty(renewalCount.Value) ? Convert.ToString(0) : renewalCount.Value;

            var preCheckReq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.RenewalPrecheckRequest();

            preCheckReq.InsuranceType = BKIC.SellingPoint.DL.Constants.Insurance.Motor;
            preCheckReq.Agency = userInfo.Agency;
            preCheckReq.AgentCode = userInfo.AgentCode;
            preCheckReq.DocumentNo = txtMotorRenewalPolicySearch.Text.Trim();
            preCheckReq.CurrentRenewalCount  = renewalCount;


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

        private void Update(OAuthTokenResponse userInfo, DataServiceManager service,
            ApiResponseWrapper<MotorSavedQuotationResponse> motorDetails,
            BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy response, decimal claimamount)
        {
            _MotorID = motorDetails.Result.MotorPolicyDetails.MotorID;
            _RenewalCount = motorDetails.Result.MotorPolicyDetails.RenewalCount;
            ddlMake.SelectedIndex = ddlMake.Items.IndexOf(ddlMake.Items.FindByText(response.VehicleMake));
            SetVehicleMake(service, motorDetails, response);
            ddlManufactureYear.SelectedIndex = ddlManufactureYear.Items.IndexOf(ddlManufactureYear.Items.FindByText(response.YearOfMake.ToString()));
            SetVehicleType(response);
            txtRegistration.Text = response.RegistrationNumber;
            txtChassis.Text = response.ChassisNo;
            txtSumInsured.Text = response.VehicleValue.ToString();
            ddlBanks.SelectedIndex = ddlBanks.Items.IndexOf(ddlBanks.Items.FindByValue(response.FinancierCompanyCode));
            txtBankCode.Text = response.FinancierCompanyCode;
            ChkPolicyUnderBCFC.Checked = response.IsUnderBCFC;
            txtSeatingCapcity.Text = Convert.ToString(response.SeatingCapacity);
            ddlCover.SelectedIndex = ddlCover.Items.IndexOf(ddlCover.Items.FindByValue(response.Subclass));
            txtExcessValue.Text = response.ExcessAmount.ToString();
            ddlExcess.SelectedIndex = ddlExcess.Items.IndexOf(ddlExcess.Items.FindByValue(response.ExcessType));
            ddlPaymentMethods.SelectedIndex = ddlPaymentMethods.Items.IndexOf(ddlPaymentMethods.Items.FindByText(response.PaymentType));
            txtAccountNumber.Text = response.IsSavedRenewal ? response.AccountNumber : string.Empty;
            txtRemarks.Text = response.IsSavedRenewal ? response.Remarks : string.Empty;
            txtCPR.Text = response.CPR;
            txtInsuredName.Text = response.InsuredName;
            txtClientCode.Text = response.InsuredCode;
            insuredDOB.Value = response.DOB.CovertToLocalFormat();
            txtAge.Text = Convert.ToString(CalculateAgeCorrect(response.DOB, DateTime.Now));
            GetBodyType();
            ddlBodyType.SelectedIndex = ddlBodyType.Items.IndexOf(ddlBodyType.Items.FindByText(response.vehicleBodyType));
            ddlEnginecc.SelectedIndex = ddlEnginecc.Items.IndexOf(ddlEnginecc.Items.FindByValue(response.EngineCC.ToString()));
            SetBank();
            SetInsured();
            SetRenewalDate(response);
            SetRenewalData(userInfo, service, response, claimamount);
            //Don't allow back date while renewal.
            master.SetRenewalDate();
            SetCover(userInfo);
        }


        private void SetVehicleType(BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy response)
        {
            //For renewal all vehicle should be used type.
            //ddlVehicleType.Items.IndexOf(ddlVehicleType.Items.FindByText(response.VehicleTypeCode)); 
            ddlVehicleType.SelectedIndex = 1;
            ddlVehicleType.Enabled = false;
            rfvtxtRegistration.Enabled = true;            
        }

        private void SetRenewalData(OAuthTokenResponse userInfo, DataServiceManager service,
            BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy response, decimal claimAmount)
        {            
            if(CheckClaim(userInfo, service, response, claimAmount))
            {
                if (response.IsSavedRenewal)
                {
                    if (response.IsSaved || response.IsActivePolicy || response.PremiumAfterDiscount > 0)
                    {
                        if (response.PremiumBeforeDiscount - response.PremiumAfterDiscount > 0)
                        {
                            calculatedPremium.Value = Convert.ToString(response.PremiumBeforeDiscount - response.LoadAmount);
                            calculatedCommision.Value = Convert.ToString(response.CommisionBeforeDiscount - response.LoadAmount);
                            AjdustedPremium = true;
                        }
                        else
                        {
                            calculatedPremium.Value = Convert.ToString(response.PremiumAfterDiscount - response.LoadAmount);
                            calculatedCommision.Value = Convert.ToString(response.CommissionAfterDiscount - response.LoadAmount);
                        }
                        txtLoadAmount.Text = Convert.ToString(response.LoadAmount);
                        txtLoadAmount1.Text = Convert.ToString(response.LoadAmount);

                        ShowPremium(userInfo, response.PremiumAfterDiscount, response.CommissionAfterDiscount);
                        ShowVAT(userInfo, response.TaxOnPremium, response.TaxOnCommission,
                                  (response.PremiumAfterDiscount + response.TaxOnPremium),
                                  (response.CommissionAfterDiscount + response.TaxOnCommission));
                        ShowDiscount(userInfo, response);

                        EnableAuthorize(response.IsHIR, response.HIRStatus);
                        if (response.IsActivePolicy)
                        {
                            SetScheduleHRef(txtMotorRenewalPolicySearch.Text.Trim(), Constants.Motor, userInfo, response.RenewalCount);
                            SetCertificateHRef(txtMotorRenewalPolicySearch.Text.Trim(), "Portal", userInfo, response.RenewalCount);
                            //If it is authorized policy need to disable all the page controls.
                            master.makeReadOnly(GetContentControl(), false);
                        }
                        else
                        {
                            RemoveScheduleHRef();
                            RemoveCertificateHref();
                            SetProposalHRef(txtMotorRenewalPolicySearch.Text.Trim(), Constants.Motor, userInfo, response.RenewalCount);
                            master.makeReadOnly(GetContentControl(), true);
                        }
                        SetReadOnlyControls();
                        GetOptionalCovers(service, userInfo.Agency, userInfo.AgentCode);
                        SetOptionalCover(response);
                    }
                }
                else
                {
                    _MotorID = 0;
                    GetOptionalCovers(service, userInfo.Agency, userInfo.AgentCode);
                    SetOptionalCover(response);
                }
            }            
        }

        private bool CheckClaim(OAuthTokenResponse userInfo, DataServiceManager service, 
                              BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy response, decimal claimAmount)
        {
            bool showPremium = true;
            var product = GetProduct();
            decimal maxAllowedClaimAmount = decimal.Zero;
            if (product != null && claimAmount > 0 && product.MotorClaim != null && product.MotorClaim.Count > 0)
            {
                maxAllowedClaimAmount = product.MotorClaim.FirstOrDefault().MaximumClaimAmount;
            }
            if (maxAllowedClaimAmount < claimAmount && (userInfo.Roles == "User" || userInfo.Roles == "BranchAdmin") && response.HIRStatus != 8)
            {
                if(response.IsSavedRenewal)
                {
                    _MotorID = response.MotorID;
                }
                else
                {
                    _MotorID = 0;
                }
                Calculate();            
                GetOptionalCovers(service, userInfo.Agency, userInfo.AgentCode);
                SetOptionalCover(response);
                btnAuthorize.Visible = false;
                btnCalculate.Visible = false;
                amtDisplay.Visible = false;
                btnSubmit.Visible = true;
                showPremium = false;                
            }
            return showPremium;
        }
        


        private void SetVehicleMake(DataServiceManager service, ApiResponseWrapper<MotorSavedQuotationResponse> motorDetails,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy response)
        {
            if (ddlMake.SelectedIndex > 0)
            {
                string VehicleMake = ddlMake.SelectedItem.Value;

                var vehicleModel = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.VehicleModelResponse>>(
                                   BKIC.SellingPoint.DTO.Constants.DropDownURI.GetVehicleModel.Replace("{vehicleMake}", VehicleMake));

                DataTable vehicleModeldt = JsonConvert.DeserializeObject<DataTable>(vehicleModel.Result.VehicleModeldt);

                if (vehicleModeldt.Rows.Count > 0)
                {
                    ddlModel.DataValueField = "Model";
                    ddlModel.DataTextField = "Model";
                    ddlModel.DataSource = ExtensionMethod.GetDistictModel(motorDetails.Result.MotorPolicyDetails.Subclass, vehicleModeldt);
                    ddlModel.DataBind();
                    ddlModel.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
                ddlModel.SelectedIndex = ddlModel.Items.IndexOf(ddlModel.Items.FindByText(response.VehicleModel));
            }
        }

        private void SetBank()
        {
            if (ddlBanks.SelectedIndex > 0)
            {
                ddlUnderLoan.SelectedIndex = 1;
                rfvddlBanks.Enabled = true;
            }
            else
            {
                ddlUnderLoan.SelectedIndex = 2;
                ddlBanks.SelectedIndex = -1;
                ddlBanks.Enabled = false;
                txtBankCode.Text = string.Empty;
                txtBankCode.Enabled = false;
                rfvddlBanks.Enabled = false;
            }
        }

        private void SetInsured()
        {
            if (InsuredNames != null && InsuredNames.Count > 0)
            {
                // var insured = InsuredNames.Find(c => c.CPR == ddlCPR.SelectedItem.Text.Trim());
                var insured = InsuredNames.Find(c => c.CPR == txtCPR.Text.Trim());
                if (insured != null)
                {
                    txtInsuredName.Text = insured.FirstName + " " + insured.MiddleName + " " + insured.LastName;
                    txtAge.Text = Convert.ToString(CalculateAgeCorrect(insured.DateOfBirth ?? DateTime.Now, DateTime.Now));
                    insuredDOB.Value = insured.DateOfBirth.ConvertToLocalFormat();
                }
            }
        }

        private void SetRenewalDate(BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy response)
        {
            if (!response.IsSavedRenewal)
            {
                if (response.ExpiryDate < DateTime.Now.Date)
                {
                    txtRenewalPeriodFrom.Text = DateTime.Now.Date.CovertToLocalFormat();
                }
                else
                {
                    txtRenewalPeriodFrom.Text = response.ExpiryDate.AddDays(1).CovertToLocalFormat();
                }
                txtRenewalPeriodTo.Text = response.ExpiryDate.AddYears(1).CovertToLocalFormat();
                actualRenewalDate.Value = Convert.ToString(response.ExpiryDate.AddDays(1).CovertToLocalFormat());
            }
            else
            {
                txtRenewalPeriodFrom.Text = response.PolicyCommencementDate.CovertToLocalFormat();
                txtRenewalPeriodTo.Text = response.ExpiryDate.CovertToLocalFormat();
                actualRenewalDate.Value = Convert.ToString(response.ActualRenewalStartDate.ConvertToLocalFormat());
            }
        }

        public void SetCover(OAuthTokenResponse userInfo)
        {
            if (userInfo.Agency == "TISCO")
            {
                ddlCover.Enabled = false;
                txtSumInsured.Enabled = false;
            }
        }

        public void GetSystemPolicyForRenewal()
        {
            OAuthTokenResponse userInfo;
            DataServiceManager service;
            ApiResponseWrapper<MotorSavedQuotationResponse> motorDetails;
            GetRenewalPolicy(out userInfo, out service, out motorDetails);

        }
        /// <summary>
        /// Set Optional covers if the policy have.
        /// </summary>
        /// <param name="response"></param>
        private void SetOptionalCover(BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy response)
        {
            if (response.OptionalCovers != null && response.OptionalCovers.Count > 0)
            {
               // OptionalCovers = response.OptionalCovers;
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
                for (int i = 0; i < response.OptionalCovers.Count; i++)
                {
                    drCurrentRow = dtCurrentTable.NewRow();
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    dtCurrentTable.Rows[i]["Cover Code"] = response.OptionalCovers[i].CoverCode;
                    dtCurrentTable.Rows[i]["Cover Description"] = response.OptionalCovers[i].CoverDescription;
                    dtCurrentTable.Rows[i]["Cover Amount"] = response.OptionalCovers[i].CoverAmount;
                    dtCurrentTable.Rows[i]["IsOptionalCover"] = true;
                }
                ViewState["CurrentTable"] = dtCurrentTable;
                Gridview1.DataSource = dtCurrentTable;
                Gridview1.DataBind();
                SetNewCoverData();
                //ButtonAddNewCover.Visible = false;
                newadmindetails.Visible = true;
            }
            else
            {
                //ViewState["CurrentTable"] = null;
                //Gridview1.DataSource = null;
                //Gridview1.DataBind();
                //newadmindetails.Visible = false;
            }
        }

        protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                master.IsSessionAvailable();               
                var service = CommonMethods.GetLogedInService();

                string VehicleMake = ddlMake.SelectedItem.Value;

                var vehicleModel = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.VehicleModelResponse>>(
                                   BKIC.SellingPoint.DTO.Constants.DropDownURI.GetVehicleModel
                                   .Replace("{vehicleMake}", VehicleMake));

                if (vehicleModel.StatusCode == 200 && vehicleModel.Result.IsTransactionDone)
                {
                    if (ddlMake.SelectedIndex > 0)
                    {
                        DataTable vehicleModeldt = JsonConvert.DeserializeObject<DataTable>(vehicleModel.Result.VehicleModeldt);

                        if (vehicleModeldt != null && vehicleModeldt.Rows.Count > 0)
                        {
                            ddlModel.DataValueField = "Model";
                            ddlModel.DataTextField = "Model";
                            ddlModel.DataSource = ExtensionMethod.GetDistictModel(ddlCover.SelectedItem.Value, vehicleModeldt);
                            ddlModel.DataBind();
                            ddlModel.Items.Insert(0, new ListItem("--Please Select--", ""));
                        }
                    }
                    else
                    {
                        ddlModel.SelectedIndex = -1;
                    }
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

        public void EnableAuthorize(bool isHIR, int HIRStatus)
        {
            if (isHIR && HIRStatus != 8)
                btnAuthorize.Visible = false;
            else
                btnAuthorize.Visible = true;
        }

        protected void ddlExcess_Changed(object sender, EventArgs e)
        {
            try
            {
                txtExcessValue.Text = GetExcess().ToString();
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

        protected void txtSumInsured_Changed(object sender, EventArgs e)
        {
            try
            {
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

        protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtExcessValue.Text = GetExcess().ToString();
                GetBodyType();
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

        public void DisableControls()
        {         
            btnSubmit.Visible = false;                        
            btnAuthorize.Visible = false;
            downloadproposal.Visible = false;
            premiumAmount.Text = string.Empty;
            premiumAmount1.Text = string.Empty;
            commission.Text = string.Empty;
            commission1.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            includeDisc.Visible = false;
            excludeDisc.Visible = false;
        }

        public void GetBodyType()
        {
            try
            {
                master.IsSessionAvailable();               
                var service = CommonMethods.GetLogedInService();

                string VehicleMake = ddlMake.SelectedItem.Value;
                string VehicleModel = ddlModel.SelectedItem.Value;

                var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorBodyRequest();
                request.VehicleMake = VehicleMake;
                request.VehicleModel = VehicleModel;

                var vehicleModel = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.VehicleBodyResponse>,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorBodyRequest>(
                                   BKIC.SellingPoint.DTO.Constants.DropDownURI.GetVehicleBody, request);

                if (ddlMake.SelectedIndex > 0)
                {
                    DataTable vehicleModeldt = JsonConvert.DeserializeObject<DataTable>(vehicleModel.Result.VehicleBodydt);
                    DataTable vehicleEnginCCdt = JsonConvert.DeserializeObject<DataTable>(vehicleModel.Result.VehicleEngineCCdt);

                    if (vehicleModeldt.Rows.Count == 1)
                    {
                        ddlBodyType.Items.Clear();
                        ddlBodyType.Items.Add(new ListItem("select", "-1"));
                        ddlBodyType.DataValueField = "BodyType";
                        ddlBodyType.DataTextField = "BodyType";
                        ddlBodyType.DataSource = vehicleModeldt;
                        ddlBodyType.DataBind();
                        ddlBodyType.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlBodyType.Items.Clear();
                        ddlBodyType.Items.Add(new ListItem("select", "-1"));
                        ddlBodyType.DataValueField = "BodyType";
                        ddlBodyType.DataTextField = "BodyType";
                        ddlBodyType.DataSource = vehicleModeldt;
                        ddlBodyType.DataBind();
                        // ddlBodyType.Items.Insert(0, new ListItem("--Please Select--", ""));
                    }
                    if (vehicleEnginCCdt != null && vehicleEnginCCdt.Rows.Count > 0)
                    {
                        ddlEnginecc.DataValueField = "Tonnage";
                        ddlEnginecc.DataTextField = "Capacity";
                        ddlEnginecc.DataSource = vehicleEnginCCdt;
                        ddlEnginecc.DataBind();
                        ddlEnginecc.Items.Insert(0, new ListItem("--Please Select--", ""));
                    }
                }
                else
                {
                    ddlModel.SelectedIndex = -1;
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

        public decimal GetExcess()
        {
            try
            {
               
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var product = GetProduct();
                var underAgeLimit = product != null ? product.UnderAge : 25;

                var excessAmountrequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.ExcessAmountRequest();
                excessAmountrequest.VehicleMake = ddlMake.SelectedItem.Value;
                excessAmountrequest.VehicleModel = ddlModel.SelectedItem.Value;
                // excessAmountrequest.VehicleType = ddlVehicleTyePolicyDetail.SelectedItem.Value;
                excessAmountrequest.ExcessType = ddlExcess.SelectedItem.Value;
                excessAmountrequest.Agency = userInfo.Agency;
                excessAmountrequest.AgentCode = userInfo.AgentCode;
                excessAmountrequest.MainClass = MainClass;
                excessAmountrequest.SubClass = ddlCover.SelectedItem.Value.Trim();
                excessAmountrequest.IsUnderAge = !string.IsNullOrEmpty(txtAge.Text)
                                                 && Convert.ToInt32(txtAge.Text) < underAgeLimit ? true : false;

                var excessAmount = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.ExcessAmountResponse>,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.ExcessAmountRequest>
                                   (BKIC.SellingPoint.DTO.Constants.MotorURI.GetExcessAmount, excessAmountrequest);

                if (excessAmount.Result.IsTransactionDone && excessAmount.StatusCode == 200)
                {
                    return excessAmount.Result.ExcessAmount;
                }
                return decimal.Zero;
            }
            catch (Exception ex)
            {
                return 0;
                //throw ex;
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
                txtLoadAmount.Text = string.Empty;
                txtLoadAmount1.Text = string.Empty;
                UpdateTotalWithLoad();
                UpdateTotal();
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

        private void UpdateTotal()
        {
            var Premium = Convert.ToDecimal(calculatedPremium.Value);
            var Commision = Convert.ToDecimal(calculatedCommision.Value);
            decimal Discount = string.IsNullOrEmpty(txtDiscount.Text) ? decimal.Zero : Convert.ToDecimal(txtDiscount.Text);
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
                btnSubmit.Enabled = true;
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

        protected void ddlCPR_SelectedIndexChanged(object sender, EventArgs e)
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

        public int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;

            return age;
        }

        protected void update_BankCode(object sender, EventArgs e)
        {
            try
            {
                txtBankCode.Text = ddlBanks.SelectedItem.Value;
                txtBankCode.Enabled = false;
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

        protected void vehicle_financed(object sender, EventArgs e)
        {
            try
            {
                if (ddlUnderLoan.SelectedIndex == 1)
                {
                    ddlBanks.Enabled = true;
                    rfvddlBanks.Enabled = true;
                }
                else
                {
                    ddlBanks.SelectedIndex = 0;
                    ddlBanks.Enabled = false;
                    txtBankCode.Text = string.Empty;
                    txtBankCode.Enabled = false;
                    rfvddlBanks.Enabled = false;
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

        public void SaveAuthorize(bool isSave)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                if (!ValidateProduct() || (newadmindetails.Visible && Gridview1.Rows.Count > 0 && !ValidateOptionalCover()))
                {
                    return;
                }               
                int RenewalDelayedDays = string.IsNullOrEmpty(actualRenewalDate.Value) ? 0 : (int)(txtRenewalPeriodFrom.Text.CovertToCustomDateTime() - actualRenewalDate.Value.CovertToCustomDateTime()).TotalDays;

                var motorQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuote();
                var postMotorPolicy = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy();

                motorQuote.Agency = userInfo.Agency;
                motorQuote.AgentCode = userInfo.AgentCode;
                motorQuote.YearOfMake = ddlManufactureYear.SelectedItem.Text;
                motorQuote.VehicleMake = ddlMake.SelectedItem.Value;
                motorQuote.VehicleModel = ddlModel.SelectedItem.Value;
                motorQuote.VehicleType = ddlVehicleType.SelectedItem.Value;
                motorQuote.VehicleSumInsured = Convert.ToDecimal(txtSumInsured.Text);
                motorQuote.TypeOfInsurance = ddlCover.SelectedItem.Value.Trim();
                motorQuote.ExcessType = ddlExcess.SelectedItem.Value;
                motorQuote.DOB = insuredDOB.Value.CovertToCustomDateTime();
                motorQuote.PolicyStartDate = txtRenewalPeriodFrom.Text.CovertToCustomDateTime();
                motorQuote.PolicyEndDate = txtRenewalPeriodTo.Text.CovertToCustomDateTime();
                motorQuote.RegistrationMonth = string.IsNullOrEmpty(txtRegistration.Text.Trim()) ?
                                               string.Empty : txtRegistration.Text.Trim();
                motorQuote.MainClass = MainClass;
                motorQuote.RenewalDelayedDays = RenewalDelayedDays;

                //If the user added the optional cover to the policy.
                decimal optionalCoverAmount = GetOptionalCoverAmount();
                motorQuote.OptionalCoverAmount = optionalCoverAmount;

                var motorQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuoteResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuote>
                                       (BKIC.SellingPoint.DTO.Constants.MotorURI.GetQuote, motorQuote);

                if (motorQuoteResult.StatusCode == 200 && motorQuoteResult.Result.IsTransactionDone)
                {

                    decimal loadamount = decimal.Zero;
                    //If there is load amount include that amount to total premium.
                    if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                      || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
                    {
                        loadamount = string.IsNullOrEmpty(txtLoadAmount.Text) ? decimal.Zero : Convert.ToDecimal(txtLoadAmount.Text);
                        postMotorPolicy.LoadAmount = loadamount;
                    }
                    else
                    {
                        loadamount = string.IsNullOrEmpty(txtLoadAmount1.Text) ? decimal.Zero : Convert.ToDecimal(txtLoadAmount1.Text);
                        postMotorPolicy.LoadAmount = loadamount;
                    }

                    motorQuoteResult.Result.TotalPremium = loadamount + motorQuoteResult.Result.TotalPremium;

                    //Include the claim amount also to the premium.
                    var product = GetProduct();
                    decimal claimLoad = decimal.Zero;
                    if (product != null && ClaimAmount > 0)
                    {
                        decimal claimPercent = decimal.Zero;
                        if (product.MotorClaim != null)
                        {
                            var claimRow = product.MotorClaim.Find(x => x.AmountFrom <= ClaimAmount && x.AmountTo >= ClaimAmount);
                            if (claimRow != null)
                            {
                                claimPercent = claimRow.Percentage;
                                claimLoad = claimPercent * motorQuoteResult.Result.TotalPremium / 100;
                            }
                        }
                    }
                    motorQuoteResult.Result.TotalPremium = motorQuoteResult.Result.TotalPremium + claimLoad;
                    postMotorPolicy.ClaimAmount = ClaimAmount;
                    postMotorPolicy.PremiumBeforeDiscount = motorQuoteResult.Result.TotalPremium;

                    var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest();
                    commisionRequest.AgentCode = userInfo.AgentCode;
                    commisionRequest.Agency = userInfo.Agency;
                    commisionRequest.SubClass = ddlCover.SelectedItem.Value.Trim();
                    commisionRequest.PremiumAmount = motorQuoteResult.Result.TotalPremium;
                    commisionRequest.IsDeductable = true;


                    var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                           BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>
                                           (BKIC.SellingPoint.DTO.Constants.CommissionURI.CalculateCommission, commisionRequest);

                    if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone
                          && commissionresult.Result.CommissionAmount >= 0)
                    {
                        postMotorPolicy.CommisionBeforeDiscount = commissionresult.Result.CommissionAmount;
                        if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                           || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
                        {
                            var Premium = Convert.ToDecimal(premiumAmount.Text);
                            if (Premium < postMotorPolicy.PremiumBeforeDiscount || AjdustedPremium)
                            {
                                postMotorPolicy.UserChangedPremium = true;
                                postMotorPolicy.PremiumAfterDiscount = Premium;
                                var diff = postMotorPolicy.PremiumBeforeDiscount - postMotorPolicy.PremiumAfterDiscount;
                                postMotorPolicy.CommissionAfterDiscount = commissionresult.Result.CommissionAmount - diff;
                            }
                        }
                        else if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.User)
                        {
                            var Premium = Convert.ToDecimal(premiumAmount1.Text);
                            if (Premium < postMotorPolicy.PremiumBeforeDiscount || AjdustedPremium)
                            {
                                postMotorPolicy.UserChangedPremium = true;
                                postMotorPolicy.PremiumAfterDiscount = Premium;
                                var diff = postMotorPolicy.PremiumBeforeDiscount - postMotorPolicy.PremiumAfterDiscount;
                                postMotorPolicy.CommissionAfterDiscount = commissionresult.Result.CommissionAmount - diff;
                            }
                        }

                    }
                }
                postMotorPolicy.Agency = userInfo.Agency;
                postMotorPolicy.AgencyCode = userInfo.AgentCode;
                postMotorPolicy.AgentBranch = ddlBranch.SelectedItem.Value.Trim();
                postMotorPolicy.IsSaved = isSave;
                postMotorPolicy.IsActivePolicy = !isSave;
                postMotorPolicy.ChassisNo = txtChassis.Text.Trim();
                postMotorPolicy.CPR = txtCPR.Text.Trim();
                postMotorPolicy.EngineCC = Convert.ToInt32(ddlEnginecc.SelectedItem.Value);
                postMotorPolicy.ExcessAmount = Convert.ToDecimal(txtExcessValue.Text);
                postMotorPolicy.ExcessType = ddlExcess.SelectedItem.Value;
                postMotorPolicy.FinancierCompanyCode = txtBankCode.Text.Trim();
                postMotorPolicy.InsuredCode = txtClientCode.Text.Trim();
                postMotorPolicy.InsuredName = txtInsuredName.Text.Trim();
                postMotorPolicy.Mainclass = MainClass;
                postMotorPolicy.Subclass = ddlCover.SelectedItem.Value.Trim();
                postMotorPolicy.VehicleMake = ddlMake.SelectedItem.Text.Trim();
                postMotorPolicy.VehicleModel = ddlModel.SelectedItem.Text.Trim();
                postMotorPolicy.VehicleTypeCode = ddlVehicleType.SelectedItem.Value;
                postMotorPolicy.VehicleValue = Convert.ToDecimal(txtSumInsured.Text);
                postMotorPolicy.vehicleBodyType = ddlBodyType.SelectedItem.Text.Trim();
                postMotorPolicy.YearOfMake = ddlManufactureYear.SelectedIndex > 0 ? Convert.ToInt32(ddlManufactureYear.SelectedItem.Text) : 0;
                postMotorPolicy.DOB = insuredDOB.Value.CovertToCustomDateTime();
                postMotorPolicy.PolicyCommencementDate = txtRenewalPeriodFrom.Text.CovertToCustomDateTime();
                postMotorPolicy.PolicyEndDate = txtRenewalPeriodTo.Text.CovertToCustomDateTime();
                postMotorPolicy.Remarks = txtRemarks.Text.Trim();
                postMotorPolicy.AccountNumber = txtAccountNumber.Text.Trim();
                postMotorPolicy.PaymentType = ddlPaymentMethods.SelectedItem.Text.Trim();
                postMotorPolicy.RegistrationNumber = txtRegistration.Text.Trim();
                postMotorPolicy.OptionalCovers = GetOptionalCoverDetails();
                postMotorPolicy.OptionalCoverAmount = optionalCoverAmount;
                postMotorPolicy.IsUnderBCFC = ChkPolicyUnderBCFC.Checked;
                postMotorPolicy.SeatingCapacity = string.IsNullOrEmpty(txtSeatingCapcity.Text) ? 0 :
                                                  Convert.ToInt32(txtSeatingCapcity.Text);
                postMotorPolicy.Createdby = ddlUsers.SelectedIndex > 0 ?
                                            Convert.ToInt32(ddlUsers.SelectedItem.Value) : Convert.ToInt32(userInfo.ID);
                postMotorPolicy.IsRenewal = true;
                postMotorPolicy.DocumentNo = txtMotorRenewalPolicySearch.Text.Trim();//ddlMotorPolicies.SelectedItem.Text.Trim();
                postMotorPolicy.OldDocumentNumber = String.Empty;
                postMotorPolicy.ClaimAmount = ClaimAmount;
                postMotorPolicy.RenewalDelayedDays = RenewalDelayedDays;
                postMotorPolicy.ActualRenewalStartDate = string.IsNullOrEmpty(actualRenewalDate.Value) ? (DateTime?)null : actualRenewalDate.Value.CovertToCustomDateTime();

                if (_MotorID > 0)
                    postMotorPolicy.MotorID = _MotorID;

                if (_RenewalCount > 0)
                    postMotorPolicy.RenewalCount = _RenewalCount;

                var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                     <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicyResponse>,
                                     BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy>
                                    (BKIC.SellingPoint.DTO.Constants.MotorURI.PostMotorPolicy, postMotorPolicy);

                if (result.Result != null && result.StatusCode == 200 && result.Result.IsTransactionDone)
                {
                    _MotorID = result.Result.MotorID;
                    LoadAgencyClientRenewalPolicy(userInfo, service);
                    txtMotorRenewalPolicySearch.Text = result.Result.DocumentNo;
                    modalBodyText.InnerHtml = GetMessageText(result.Result.IsHIR, postMotorPolicy.IsActivePolicy, result.Result.DocumentNo);
                    if (postMotorPolicy.IsActivePolicy)
                    {
                        SetScheduleHRef(result.Result.DocumentNo, Constants.Motor, userInfo, result.Result.RenewalCount);
                        SetCertificateHRef(result.Result.DocumentNo, "Portal", userInfo, result.Result.RenewalCount);
                    }
                    else
                    {
                        var product = GetProduct();
                        decimal maxAllowedClaimAmount = decimal.Zero;
                        if (product != null && ClaimAmount > 0 && product.MotorClaim != null && product.MotorClaim.Count > 0)
                        {
                            maxAllowedClaimAmount = product.MotorClaim.FirstOrDefault().MaximumClaimAmount;
                        }
                        if(maxAllowedClaimAmount < ClaimAmount && (userInfo.Roles == "User" || userInfo.Roles == "BranchAdmin"))
                        {
                           
                        } 
                        else
                        {
                            SetProposalHRef(result.Result.DocumentNo, Constants.Motor, userInfo, result.Result.RenewalCount);
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowPopup();", true);
                }
                else
                {
                    master.ShowErrorPopup(result.Result.TransactionErrorMessage, "Request Failed !");
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


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool ValidateProduct()
        {
            bool isvalid = true;
            var product = GetProduct();
            if (product != null)
            {
                if (!product.AllowUnderAge && !string.IsNullOrEmpty(txtAge.Text)
                    && Convert.ToInt32(txtAge.Text) < product.UnderAge)
                {
                    master.ShowErrorPopup("Insured is under age !", "Can't issue a policy !");
                    isvalid = false;
                }
                if (!product.AllowMaxVehicleAge && ddlManufactureYear.SelectedIndex > 0)
                {
                    int yearsDiffrent = DateTime.Now.Year - Convert.ToInt32(ddlManufactureYear.SelectedItem.Text);
                    if (yearsDiffrent > product.MaximumVehicleAge)
                    {
                        master.ShowErrorPopup("Vehicle age is excceed the limit maximum vehicle age is upto :" +
                                " " + product.MaximumVehicleAge + " Years", "Can't issue a policy !");
                        isvalid = false;
                    }
                }
            }
            return isvalid;
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
            }
        }

        protected void Calculate_Click(object sender, EventArgs e)
        {
            try
            {
                DisablePaymentValidator();
                if (Page.IsValid)
                {
                    Calculate();
                }
            }
            catch (Exception ex)
            {
                //throw ex; ;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        public void Calculate()
        {
            try
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    if (!ValidateProduct() || (newadmindetails.Visible && Gridview1.Rows.Count > 0 && !ValidateOptionalCover()))
                    {
                        return;
                    }
                    ClearLoadDiscount();                  

                    var motorQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuote
                    {
                        Agency = userInfo.Agency,
                        AgentCode = userInfo.AgentCode,
                        YearOfMake = ddlManufactureYear.SelectedItem.Text,
                        VehicleMake = ddlMake.SelectedItem.Value,
                        VehicleModel = ddlModel.SelectedItem.Value,
                        VehicleType = ddlVehicleType.SelectedItem.Value,
                        VehicleSumInsured = Convert.ToDecimal(txtSumInsured.Text),
                        TypeOfInsurance = ddlCover.SelectedItem.Value.Trim(),
                        ExcessType = ddlExcess.SelectedItem.Value,
                        DOB = insuredDOB.Value.CovertToCustomDateTime(),
                        PolicyStartDate = txtRenewalPeriodFrom.Text.CovertToCustomDateTime(),
                        PolicyEndDate = txtRenewalPeriodTo.Text.CovertToCustomDateTime(),
                        RegistrationMonth = txtRegistration.Text,
                        MainClass = MainClass,
                        RenewalDelayedDays = string.IsNullOrEmpty(actualRenewalDate.Value) ? 0 : (int)(txtRenewalPeriodFrom.Text.CovertToCustomDateTime() - actualRenewalDate.Value.CovertToCustomDateTime()).TotalDays

                    };
                    //Tisco have optinal cover.
                    //I have added the page optional cover amount to the total premium.
                    decimal optionalCoverAmount = GetOptionalCoverAmount();
                    motorQuote.OptionalCoverAmount = optionalCoverAmount;

                    var motorQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuoteResponse>,
                                           BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuote>
                                           (BKIC.SellingPoint.DTO.Constants.MotorURI.GetQuote, motorQuote);

                    if (motorQuoteResult.StatusCode == 200 && motorQuoteResult.Result.IsTransactionDone)
                    {
                        //Include the claim amount also to the premium.
                        var product = GetProduct();
                        decimal claimLoad = decimal.Zero;
                        if (product != null && ClaimAmount > 0)
                        {
                            decimal claimPercent = decimal.Zero;
                            if (product.MotorClaim != null)
                            {
                                var claimRow = product.MotorClaim.Find(x => x.AmountFrom <= ClaimAmount && x.AmountTo >= ClaimAmount);
                                if (claimRow != null)
                                {
                                    claimPercent = claimRow.Percentage;
                                    claimLoad = claimPercent * motorQuoteResult.Result.TotalPremium / 100;
                                }
                            }
                        }
                        motorQuoteResult.Result.TotalPremium = motorQuoteResult.Result.TotalPremium + claimLoad;

                        calculatedPremium.Value = Convert.ToString(motorQuoteResult.Result.TotalPremium);
                        var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest
                        {
                            AgentCode = userInfo.AgentCode,
                            Agency = userInfo.Agency,
                            SubClass = ddlCover.SelectedItem.Value.Trim(),
                            IsDeductable = true,
                            PremiumAmount = motorQuoteResult.Result.TotalPremium
                        };

                        var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                               BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>
                                               (BKIC.SellingPoint.DTO.Constants.CommissionURI.CalculateCommission, commisionRequest);

                        if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone
                            && commissionresult.Result.CommissionAmount >= 0)
                        {
                            calculatedCommision.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                            ShowPremium(userInfo, motorQuoteResult.Result.TotalPremium, commissionresult.Result.CommissionAmount);
                        }
                        else
                        {
                            master.ShowLoading = false;
                            master.ShowErrorPopup(commissionresult.Result.TransactionErrorMessage, "Request Failed !");
                            return;
                        }
                        //Calculate VAT.
                        var vatResponse = master.GetVat(motorQuoteResult.Result.TotalPremium, commissionresult.Result.CommissionAmount);
                        if (vatResponse != null && vatResponse.IsTransactionDone)
                        {
                            decimal TotalPremium = motorQuoteResult.Result.TotalPremium + vatResponse.VatAmount;
                            decimal TotalCommission = commissionresult.Result.CommissionAmount + vatResponse.VatCommissionAmount;
                            ShowVAT(userInfo, vatResponse.VatAmount, vatResponse.VatCommissionAmount, TotalPremium, TotalCommission);
                        }
                    }
                    else
                    {
                        master.ShowLoading = false;
                        master.ShowErrorPopup(motorQuoteResult.Result.TransactionErrorMessage, "Request Failed !");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex; ;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        public void ClearLoadDiscount()
        {
            txtDiscount.Text = string.Empty;
            txtLoadAmount.Text = string.Empty;
            txtLoadAmount1.Text = string.Empty;
        }

        private void BindDropdown(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}",
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.MotorInsurance));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable branches = dropdownds.Tables["BranchMaster"];
                DataTable YearOfMake = dropdownds.Tables["MotorYearOfMake"];
                DataTable MotorVehicle = dropdownds.Tables["MotorVehicle"];
                DataTable MotorFinancier = dropdownds.Tables["MotorFinancier"];
                GeneralMake = MotorVehicle;

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

                if (YearOfMake != null && YearOfMake.Rows.Count > 0)
                {
                    ddlManufactureYear.DataValueField = "ID";
                    ddlManufactureYear.DataTextField = "Year";
                    ddlManufactureYear.DataSource = YearOfMake;
                    ddlManufactureYear.DataBind();
                    ddlManufactureYear.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
                if (MotorVehicle != null && MotorVehicle.Rows.Count > 0)
                {
                    ddlMake.DataValueField = "Make";
                    ddlMake.DataTextField = "Make";
                    ddlMake.DataSource = ExtensionMethod.GetDistinctMake(string.Empty, MotorVehicle);
                    ddlMake.DataBind();
                    ddlMake.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
                if (MotorFinancier != null && MotorFinancier.Rows.Count > 0)
                {
                    ddlBanks.DataValueField = "Code";
                    ddlBanks.DataTextField = "Financier";
                    ddlBanks.DataSource = MotorFinancier;
                    ddlBanks.DataBind();
                    ddlBanks.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
            }

            var productCode = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchProductCodeResponse>>(
                              BKIC.SellingPoint.DTO.Constants.DropDownURI.GetInsuranceProductCode
                              .Replace("{agency}", userInfo.Agency)
                              .Replace("{agencyCode}", userInfo.AgentCode)
                              .Replace("{insurancetypeid}", "4"));

            if (productCode != null && productCode.StatusCode == 200 && productCode.Result.IsTransactionDone)
            {
                var products = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>(
                               BKIC.SellingPoint.DTO.Constants.DropDownURI.GetAgencyProducts
                               .Replace("{agency}", userInfo.Agency)
                               .Replace("{agencyCode}", userInfo.AgentCode)
                               .Replace("{mainclass}", productCode.Result.productCode)
                               .Replace("{page}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.MotorInsurance));

                MainClass = productCode.Result.productCode;

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

        private void LoadAgencyClientCode(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest
            {
                AgentBranch = userInfo.AgentBranch,
                AgentCode = userInfo.AgentCode
            };

            var motorResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredResponse>,
                              BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest>
                              (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyInsured, req);

            if (motorResult.StatusCode == 200 && motorResult.Result.IsTransactionDone && motorResult.Result.AgencyInsured.Count > 0)
            {
                //ddlCPR.DataSource = motorResult.Result.AgencyInsured;
                //ddlCPR.DataTextField = "CPR";
                //ddlCPR.DataValueField = "InsuredCode";
                //ddlCPR.DataBind();
                //ddlCPR.Items.Insert(0, new ListItem("--Please Select--", ""));
                InsuredNames = motorResult.Result.AgencyInsured;
            }
            ddlUsers.SelectedIndex = ddlUsers.Items.IndexOf(ddlUsers.Items.FindByText(userInfo.UserName));
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(userInfo.AgentBranch));

            if (userInfo.Roles == "BranchAdmin" || userInfo.Roles == "User")
            {
                ddlUsers.Enabled = false;
            }
        }

        private void LoadAgencyClientRenewalPolicy(OAuthTokenResponse userInfo, DataServiceManager service, bool IncludeHIR = false)
        {
            var motorreq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                AgentBranch = userInfo.AgentBranch,
                IncludeHIR = IncludeHIR,
                IsRenewal = true
            };

            //Get PolicyNo by Agency
            var motorPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorPolicyResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest>
                                (BKIC.SellingPoint.DTO.Constants.MotorURI.GetMotorAgencyPolicy, motorreq);

            if (motorPolicies.StatusCode == 200 && motorPolicies.Result.IsTransactionDone
                && motorPolicies.Result.AgencyMotorPolicies.Count > 0)
            {
                policyList = motorPolicies.Result.AgencyMotorPolicies;

                //ddlMotorPolicies.DataSource = motorPolicies.Result.AgencyMotorPolicies;
                //ddlMotorPolicies.DataTextField = "DOCUMENTNO";
                //ddlMotorPolicies.DataValueField = "DOCUMENTRENEWALNO";
                //ddlMotorPolicies.DataBind();
                //ddlMotorPolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
                //ddlMotorPolicies.Focus();
            }
        }

        protected void calculate_expiredate(object sender, EventArgs e)
        {
            try
            {
                SetExpireDate();
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

        protected void expireDate_Changed(object sender, EventArgs e)
        {
            try
            {
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

        protected void VehicleType_Changed(object sender, EventArgs e)
        {
            try
            {
                if (ddlVehicleType.SelectedIndex > 0)
                {
                    SetExpireDate();
                }
                if (ddlVehicleType.SelectedIndex == 2)
                {
                    rfvtxtRegistration.Enabled = false;
                }
                if (ddlVehicleType.SelectedIndex == 1)
                {
                    rfvtxtRegistration.Enabled = true;
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

        private void SetExpireDate()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtRenewalPeriodFrom.Text))
                {
                    if (ddlVehicleType.SelectedItem.Value == "New")
                    {
                        var newDate = Convert.ToDateTime(txtRenewalPeriodFrom.Text.CovertToCustomDateTime()).AddYears(1);
                        var startOfMonth = new DateTime(newDate.Year, newDate.Month, 1);
                        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                        txtRenewalPeriodTo.Text = endOfMonth.CovertToLocalFormat();
                    }
                    else if (ddlVehicleType.SelectedItem.Value == "Used")
                    {
                        txtRenewalPeriodTo.Text = Convert.ToDateTime(txtRenewalPeriodFrom.Text.CovertToCustomDateTime())
                                                  .AddYears(1)
                                                  .AddDays(-1)
                                                  .CovertToLocalFormat();
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

        public void ShowPremium(OAuthTokenResponse userInfo, decimal Premium, decimal Commission)
        {
            amtDisplay.Visible = true;
            btnSubmit.Visible = true;
            if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
            {
                premiumAmount.Text = Convert.ToString(0);
                commission.Text = Convert.ToString(0);
                premiumAmount.Text = Convert.ToString(Premium);
                commission.Text = Convert.ToString(Commission);
                includeDisc.Visible = true;
                if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin)
                {
                    txtLoadAmount.Enabled = true;
                }
                else
                {
                    txtLoadAmount.Enabled = false;
                }
                txtDiscount.Enabled = true;
            }
            else
            {
                premiumAmount1.Text = Convert.ToString(0);
                commission1.Text = Convert.ToString(0);
                premiumAmount1.Text = Convert.ToString(Premium);
                commission1.Text = Convert.ToString(Commission);
                excludeDisc.Visible = true;
                txtLoadAmount1.Enabled = false;
                txtDiscount1.Enabled = false;
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
            //btnSubmit.Enabled = f;
            btnCalculate.Enabled = true;
            btnBack.Visible = true;
            btnSubmit.Visible = false;
            btnCalculate.Visible = true;

            btnAuthorize.Visible = false;
            downloadproposal.Visible = false;
            downloadschedule.Visible = false;
            downloadCertificate.Visible = false;

            //ddlCPR.SelectedIndex = 0;
            //ddlMotorPolicies.SelectedIndex = 0;
            txtCPR.Text = string.Empty;

            _MotorID = 0;

            ClaimAmount = 0;
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

        public void ShowDiscount(OAuthTokenResponse userInfo, BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy policy)
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

        protected void txtChassisNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (formMotorSubmitted.Value == "true")
                {
                    Validate();
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
            //if (!string.IsNullOrEmpty(txtChassis.Text))
            //{
            //    cvChassisNo.IsValid = txtChassis.Text.Trim().Length == 17 ? true : false;
            //    //master.ShowHideErrorSpacingSpan();
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "HideShowHiddenRequiredSpan", "HideShowHiddenRequiredSpan();", true);
            //}
            //master.ShowLoading = false;
        }



        protected void MotorProduct_changed(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                if (!ValidateProduct())
                {
                    DisableControls();
                    return;
                }             

                var product = GetProduct();
                var isProductSport = product != null ? product.IsProductSport : false;
                var OriginalMakeIndex = ddlMake.SelectedIndex;
                var OriginalMake = ddlMake.SelectedItem.Text;


                ddlMake.DataSource = ExtensionMethod.GetDistinctMake(ddlCover.SelectedItem.Value, GeneralMake);
                ddlMake.DataTextField = "Make";
                ddlMake.DataValueField = "Make";
                ddlMake.DataBind();
                ddlMake.Items.Insert(0, new ListItem("--Please Select--", ""));

                //if (ddlCover.SelectedIndex > 0 && isProductSport)
                //{
                //    ddlMake.DataSource = SportsMake;
                //    ddlMake.DataTextField = "Make";
                //    ddlMake.DataValueField = "Make";
                //    ddlMake.DataBind();
                //    ddlMake.Items.Insert(0, new ListItem("--Please Select--", ""));
                //}
                //else
                //{
                //    ddlMake.DataSource = GeneralMake;
                //    ddlMake.DataTextField = "Make";
                //    ddlMake.DataValueField = "Make";
                //    ddlMake.DataBind();
                //    ddlMake.Items.Insert(0, new ListItem("--Please Select--", ""));
                //}
                ddlMake.SelectedIndex = OriginalMakeIndex;
                GetOptionalCovers(service, userInfo.Agency, userInfo.AgentCode);
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

        private void GetOptionalCovers(DataServiceManager service, string Agency, string AgentCode)
        {
            var OptionalCoverReq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.OptionalCoverRequest
            {
                Agency = Agency,
                AgentCode = AgentCode,
                MainClass = MainClass,
                SubClass = ddlCover.SelectedItem.Value.Trim()
            };

            //Get Optinal covers by product.
            var optionalCoverRes = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.OptionalCoverResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.OptionalCoverRequest>
                                (BKIC.SellingPoint.DTO.Constants.MotorURI.GetOptionalCover, OptionalCoverReq);

            if (optionalCoverRes.StatusCode == 200 && optionalCoverRes.Result.IsTransactionDone)
            {
                if (optionalCoverRes.Result.OptionalCovers.Count > 0)
                {
                    OptionalCovers = optionalCoverRes.Result.OptionalCovers;
                    ButtonAddNewCover.Visible = true;
                    newadmindetails.Visible = false;
                    if (ddlCover.SelectedItem.Value == "TMCTR")
                    {
                        divSeatingCapcity.Visible = true;
                    }
                }
                else
                {
                    ButtonAddNewCover.Visible = false;
                    newadmindetails.Visible = false;
                    divSeatingCapcity.Visible = false;
                }
            }
            else
            {
                ButtonAddNewCover.Visible = false;
                newadmindetails.Visible = false;
                divSeatingCapcity.Visible = false;
            }
        }

        protected void ddlManufactureYear_Changed(object sender, EventArgs e)
        {
            ValidateProduct();
        }

        public void SetScheduleHRef(string DocNo, string Insurancetype, OAuthTokenResponse UserInfo, int RenewalCount)
        {
            downloadproposal.Visible = false;
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

        public void RemoveCertificateHref()
        {
            downloadCertificate.Visible = false;
            downloadCertificate.HRef = string.Empty;
        }

        public void SetCertificateHRef(string DocNo, string type, OAuthTokenResponse UserInfo, int RenewalCount)
        {
            downloadproposal.Visible = false;
            downloadCertificate.Visible = true;
            downloadCertificate.HRef = ClientUtility.WebApiUri + BKIC.SellingPoint.DTO.Constants.MotorURI.FetchInsuranceCertificate
                                                              .Replace("{documentNo}", DocNo).Replace("{type}", type)
                                                              .Replace("{agentCode}", UserInfo.AgentCode)
                                                              .Replace("{isEndorsement}", "false")
                                                              .Replace("{endorsementID}", "0")
                                                              .Replace("{renewalCount}", Convert.ToString(RenewalCount));
        }

        public void SetProposalHRef(string DocNo, string Insurancetype, OAuthTokenResponse UserInfo, int RenewalCount)
        {
            downloadschedule.Visible = false;
            downloadCertificate.Visible = false;
            downloadproposal.Visible = true;
            downloadproposal.HRef = ClientUtility.WebApiUri + BKIC.SellingPoint.DTO.Constants.ScheduleURI.downloadproposal
                                    .Replace("{insuranceType}", Insurancetype)
                                    .Replace("{agentCode}", UserInfo.AgentCode)
                                    .Replace("{documentNo}", DocNo)
                                    .Replace("{renewalCount}", Convert.ToString(RenewalCount));
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
                return "Your motor renewal policy is saved and moved into HIR: " + docNo;
            }
            else if (!isHIR && !isActivePolicy)
            {
                btnYes.Visible = false;
                btnOK.Text = "OK";
                btnAuthorize.Enabled = true;
                btnAuthorize.Visible = true;
                return "Your motor renewal policy has been saved successfully: " + docNo;
            }
            else if (isActivePolicy)
            {
                master.makeReadOnly(GetContentControl(), false);
                btnCalculate.Enabled = false;
                btnSubmit.Enabled = false;
                btnYes.Visible = false;
                downloadproposal.Visible = false;
                btnOK.Text = "OK";
                btnAuthorize.Enabled = false;
                return "Your motor renewal policy has been authorized successfully: " + docNo;
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
            txtAge.Enabled = false;
            txtBankCode.Enabled = false;
            premiumAmount.Enabled = false;
            premiumAmount1.Enabled = false;
            commission.Enabled = false;
            commission1.Enabled = false;
            btnBack.Enabled = true;
            txtAccountNumber.Enabled = ddlPaymentMethods.SelectedIndex == 1 ? false : true;
            txtExcessValue.Enabled = false;
            ddlExcess.SelectedIndex = 1;
            ddlExcess.Enabled = false;
            txtVATAmount.Enabled = false;
            txtVATAmount1.Enabled = false;
            txtVATCommission.Enabled = false;
            txtVATCommission1.Enabled = false;
            txtTotalPremium.Enabled = false;
            txtTotalPremium1.Enabled = false;
            txtTotalCommission.Enabled = false;
            txtTotalCommission1.Enabled = false;
            txtLoadAmount1.Enabled = false;
            txtDiscount1.Enabled = false;

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();          

            if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin)
            {
                txtLoadAmount.Enabled = true;
            }
            else
            {
                txtLoadAmount.Enabled = false;
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

        protected void ButtonAddNewCover_Click(object sender, EventArgs e)
        {
            try
            {
                ///If the user entered the sum insured then only allow to add the cover
                ///Because the GCC cover amount is based on sum insured.
                if (!ValidateOptionalCover())
                {
                    return;
                }
                if (OptionalCovers != null && OptionalCovers.Count > 0)
                {
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
                        }
                        for (int i = 1; i <= OptionalCovers.Count; i++)
                        {
                            drCurrentRow = dtCurrentTable.NewRow();
                            dtCurrentTable.Rows.Add(drCurrentRow);
                            dtCurrentTable.Rows[i - 1]["Cover Code"] = OptionalCovers[i - 1].CoverCode;
                            dtCurrentTable.Rows[i - 1]["Cover Description"] = OptionalCovers[i - 1].CoverDescription;
                            dtCurrentTable.Rows[i - 1]["Cover Amount"] = OptionalCovers[i - 1].CoverAmount;
                            dtCurrentTable.Rows[i - 1]["IsOptionalCover"] = false;
                            break;
                        }
                        ViewState["CurrentTable"] = dtCurrentTable;
                        Gridview1.DataSource = dtCurrentTable;
                        Gridview1.DataBind();
                        newadmindetails.Visible = true;
                        DisableControls();
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

        private void SetNewCoverData()
        {
            int rowIndex = 0;

            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DropDownList ddlCover = (DropDownList)Gridview1.Rows[rowIndex].Cells[0].FindControl("ddlNewCover");
                        TextBox txtName = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("txtNewCoverDescription");
                        TextBox txtCoverAmt = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtNewCoverAmount");

                        txtName.Text = dt.Rows[i]["Cover Description"].ToString();
                        ddlCover.SelectedIndex = ddlCover.Items.IndexOf(ddlCover.Items.FindByText(dt.Rows[i]["Cover Code"].ToString()));
                        txtCoverAmt.Text = dt.Rows[i]["Cover Amount"].ToString();
                        rowIndex++;
                    }
                }
            }
        }

        private void SetInitialRow()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("Cover Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Cover Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Cover Amount", typeof(decimal)));
            dt.Columns.Add(new DataColumn("IsOptionalCover", typeof(bool)));

            dr = dt.NewRow();

            dr["Cover Code"] = string.Empty;
            dr["Cover Description"] = string.Empty;
            dr["Cover Amount"] = decimal.Zero;
            dr["IsOptionalCover"] = false;

            dt.Rows.Add(dr);

            //dr = dt.NewRow();

            //Store the DataTable in ViewState
            ViewState["CurrentTable"] = dt;
            Gridview1.DataSource = dt;
            Gridview1.DataBind();
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

        protected void ddlCover_Changed(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                List<string> SelectedCovers = new List<string>();
                if (OptionalCovers != null)
                {
                    GridViewRow row1 = ((DropDownList)sender).Parent.Parent as GridViewRow;
                    foreach (GridViewRow row in Gridview1.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            if (row1.RowIndex != row.RowIndex)
                            {
                                var previousCover = (DropDownList)row.FindControl("ddlNewCover");
                                SelectedCovers.Add(previousCover.SelectedItem.Text);
                            }
                            if (row1.RowIndex == row.RowIndex)
                            {
                                var newCover = (DropDownList)row.FindControl("ddlNewCover");
                                var newCoverDescription = (TextBox)row.FindControl("txtNewCoverDescription");
                                var newCoverAmount = (TextBox)row.FindControl("txtNewCoverAmount");

                                if (newCover.SelectedItem.Text == "PAL" && string.IsNullOrEmpty(txtSeatingCapcity.Text))
                                {
                                    master.ShowErrorPopup("Please enter seating capacity!", "Enter Seating Capacity");
                                    newCover.SelectedIndex = -1;
                                    return;
                                }
                                if (SelectedCovers.Contains(newCover.SelectedItem.Text))
                                {
                                    newCoverDescription.Text = string.Empty;
                                    newCoverAmount.Text = Convert.ToString(0);
                                    newCover.SelectedIndex = -1;
                                    return;
                                }

                                newCoverDescription.Text = newCover.SelectedValue.Trim();
                                var OptionalCover = OptionalCovers.Find(x => x.CoverCode == newCover.SelectedItem.Text.Trim());

                                var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CalculateCoverAmountRequest
                                {
                                    Agency = userInfo.Agency,
                                    AgentCode = userInfo.AgentCode,
                                    MainClass = MainClass,
                                    SubClass = ddlCover.SelectedItem.Value.Trim(),
                                    SumInsured = string.IsNullOrEmpty(txtSumInsured.Text) ? decimal.Zero : Convert.ToDecimal(txtSumInsured.Text),
                                    BaseCoverAmount = OptionalCover != null ? OptionalCover.CoverAmount : decimal.Zero,
                                    CoverCode = newCover.SelectedItem.Text.Trim(),
                                    NoOfSeats = string.IsNullOrEmpty(txtSeatingCapcity.Text) ? 0 : Convert.ToInt32(txtSeatingCapcity.Text)
                                };                              

                                var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.CalculateCoverAmountResponse>,
                                               BKIC.SellingPoint.DTO.RequestResponseWrappers.CalculateCoverAmountRequest>
                                               (BKIC.SellingPoint.DTO.Constants.MotorURI.CalculateOptionalCoverAmount, request);

                                if (response.StatusCode == 200 && response.Result.IsTransactionDone)
                                {
                                    newCoverAmount.Text = Convert.ToString(response.Result.CoverAmount);
                                }
                            }
                        }
                    }
                    DisableControls();
                    SetClaimOptions();
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

        private void SetClaimOptions()
        {
            //If there a cliam amount exceed the maximum allowed claim amount need to enable the save button, Disable the Calculate button//
            OAuthTokenResponse userInfo1;
            DataServiceManager service1;
            ApiResponseWrapper<MotorSavedQuotationResponse> motorDetails;
            GetRenewalPolicy(out userInfo1, out service1, out motorDetails);
            if (motorDetails.StatusCode == 200 && motorDetails.Result.IsTransactionDone)
            {
                var product = GetProduct();
                decimal maxAllowedClaimAmount = decimal.Zero;
                if (product != null && ClaimAmount > 0 && product.MotorClaim != null && product.MotorClaim.Count > 0)
                {
                    maxAllowedClaimAmount = product.MotorClaim.FirstOrDefault().MaximumClaimAmount;
                }
                if (maxAllowedClaimAmount < ClaimAmount && (userInfo1.Roles == "User" || userInfo1.Roles == "BranchAdmin")
                    && motorDetails.Result.MotorPolicyDetails.HIRStatus != 8)
                {
                    if (motorDetails.Result.MotorPolicyDetails.IsSavedRenewal)
                    {
                        _MotorID = motorDetails.Result.MotorPolicyDetails.MotorID;
                    }
                    else
                    {
                        _MotorID = 0;
                    }
                    Calculate();                   
                    btnAuthorize.Visible = false;
                    btnCalculate.Visible = false;
                    amtDisplay.Visible = false;
                    btnSubmit.Visible = true;
                }
            }
        }

        protected void NewCoverAmount_Changed(object sender, EventArgs e)
        {
            btnSubmit.Enabled = false;
        }

        protected void Gridview1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                var datatable = GetNewCoversDataTable();
                ViewState["CurrentTable"] = datatable;
                DataTable dt = ViewState["CurrentTable"] as DataTable;
                datatable.Rows[index].Delete();
                ViewState["CurrentTable"] = datatable;
                Gridview1.DataSource = datatable;
                Gridview1.DataBind();
                SetNewCoverData();
                DisableControls();
                SetClaimOptions();
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

        protected void Gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (OptionalCovers != null && OptionalCovers.Count > 0)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlNewCover");
                    ddl.DataValueField = "CoverDescription";
                    ddl.DataTextField = "CoverCode";
                    ddl.DataSource = OptionalCovers;
                    ddl.DataBind();
                    ddl.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
                var lnkBtnDelete = e.Row.FindControl("lnkbtnCoverDelete") as LinkButton;
                var lblAddedByEndorsement = e.Row.FindControl("lblAddedByEndorsement") as Label;
            }
        }

        public DataTable GetNewCoversDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Cover Code");
            table.Columns.Add("Cover Description");
            table.Columns.Add("Cover Amount");
            table.Columns.Add(new DataColumn("IsOptionalCover", typeof(bool)));

            for (int row = 0; row < Gridview1.Rows.Count; row++)
            {
                var obj = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCovers();

                for (int col = 0; col < Gridview1.Columns.Count; col++)
                {
                    if (Gridview1.Columns[col].Visible)
                    {
                        if (String.IsNullOrEmpty(Gridview1.Rows[row].Cells[col].Text))
                        {
                            var colName = Gridview1.Columns[col].ToString();
                            if (colName == "Cover Amount")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row].Cells[col].Controls[1];
                                obj.CoverAmount = string.IsNullOrEmpty(txtValue.Text) ? decimal.Zero : Convert.ToDecimal(txtValue.Text);
                            }
                            if (colName == "Cover Description")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row].Cells[col].Controls[1];
                                obj.CoverDescription = txtValue.Text;
                            }
                            if (colName == "Cover Code")
                            {
                                DropDownList txtValue = (DropDownList)Gridview1.Rows[row].Cells[col].Controls[1];
                                obj.CoverCode = txtValue.SelectedItem.Text;
                            }
                        }
                    }
                }
                table.Rows.Add(obj.CoverCode, obj.CoverDescription, obj.CoverAmount, true);
            }
            return table;
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;
            if (OptionalCovers.Count > Gridview1.Rows.Count)
            {
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    DataRow drCurrentRow = null;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            //extract the TextBox values
                            DropDownList ddlCover = (DropDownList)Gridview1.Rows[rowIndex].Cells[0].FindControl("ddlNewCover");
                            TextBox txtName = (TextBox)Gridview1.Rows[rowIndex].Cells[1].FindControl("txtNewCoverDescription");
                            TextBox txtCoverAmt = (TextBox)Gridview1.Rows[rowIndex].Cells[2].FindControl("txtNewCoverAmount");

                            drCurrentRow = dtCurrentTable.NewRow();

                            dtCurrentTable.Rows[i - 1]["Cover Description"] = txtName.Text;
                            dtCurrentTable.Rows[i - 1]["Cover Code"] = ddlCover.SelectedItem.Text;
                            dtCurrentTable.Rows[i - 1]["Cover Amount"] = txtCoverAmt.Text;
                            dtCurrentTable.Rows[i - 1]["IsOptionalCover"] = true;
                            rowIndex++;
                        }
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["CurrentTable"] = dtCurrentTable;
                        Gridview1.DataSource = dtCurrentTable;
                        Gridview1.DataBind();
                    }
                    DisableControls();
                    SetClaimOptions();
                }
                else
                {
                    Response.Write("ViewState is null");
                }
                //Set Previous Data on Postbacks
                SetNewCoverData();
            }
        }

        public decimal GetOptionalCoverAmount()
        {
            decimal OptionalCoverAmount = 0;
            if (ViewState["CurrentTable"] != null && newadmindetails.Visible)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int row = 0; row < Gridview1.Rows.Count; row++)
                    {
                        for (int col = 0; col < Gridview1.Columns.Count; col++)
                        {
                            if (Gridview1.Columns[col].Visible)
                            {
                                var colName = Gridview1.Columns[col].ToString();
                                if (colName == "Cover Amount")
                                {
                                    TextBox txtValue = (TextBox)Gridview1.Rows[row].Cells[col].Controls[1];
                                    OptionalCoverAmount += string.IsNullOrEmpty(txtValue.Text) ? 0 : Convert.ToDecimal(txtValue.Text);
                                }
                            }
                        }
                    }
                }
            }
            return OptionalCoverAmount;
        }

        private List<BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCovers> GetOptionalCoverDetails()
        {
            var objs = new List<BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCovers>();
            if (newadmindetails.Visible == true)
            {
                for (int row = 0; row < Gridview1.Rows.Count; row++)
                {
                    var obj = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCovers();
                    obj.AddedByEndorsement = true;

                    for (int col = 0; col < Gridview1.Columns.Count; col++)
                    {
                        if (Gridview1.Columns[col].Visible)
                        {
                            var colName = Gridview1.Columns[col].ToString();

                            if (colName == "Cover Code")
                            {
                                DropDownList ddlCoverCode = (DropDownList)Gridview1.Rows[row].Cells[col].Controls[1];
                                obj.CoverCode = ddlCoverCode.SelectedItem.Text;
                            }
                            if (colName == "Cover Description")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row].Cells[col].Controls[1];
                                obj.CoverDescription = txtValue.Text;
                            }
                            if (colName == "Cover Amount")
                            {
                                TextBox txtValue = (TextBox)Gridview1.Rows[row].Cells[col].Controls[1];
                                obj.CoverAmount = Convert.ToDecimal(txtValue.Text);
                            }
                        }
                    }
                    objs.Add(obj);
                }
            }
            return objs;
        }

        public bool ValidateOptionalCover()
        {
            bool IsValid = true;

            var product = GetProduct();
            if (product != null)
            {
                if (product.HasGCC && string.IsNullOrEmpty(txtSumInsured.Text))
                {
                    master.ShowErrorPopup("Please enter sum insured before addding the GCC cover", "Can't add Cover !");
                    IsValid = false;
                }
                if (product.HasGCC)
                {
                    if (ddlManufactureYear.SelectedIndex == 0)
                    {
                        master.ShowErrorPopup("Please select vehicle manufactured year", "Can't add Cover !");
                        IsValid = false;
                    }
                    else
                    {
                        var yearDifference = DateTime.Now.Year - Convert.ToInt32(ddlManufactureYear.SelectedItem.Text);
                        if (yearDifference > product.GCCCoverRangeInYears)
                        {
                            master.ShowErrorPopup("Vehicle age is exceed the limit. The maximum vehicle age for GGC is upto  :"
                                + product.GCCCoverRangeInYears + " Years", "Can't add Cover !");
                            IsValid = false;
                        }
                    }
                }
            }
            return IsValid;
        }

        public BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProduct GetProduct()
        {
            BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProduct product = null;
            var motorProduct = (List<BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProduct>)Session["MotorProducts"];
            if (motorProduct != null)
            {
                product = motorProduct.Find(x => x.MainClass == MainClass
                                         && x.SubClass == ddlCover.SelectedItem.Value);

                product.MotorClaim = product.MotorClaim.Where(c => c.MainClass == MainClass &&
                                          c.SubClass == ddlCover.SelectedItem.Value).ToList();
                
            }
            else
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var productRequest = new AgecyProductRequest
                {
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    MainClass = string.Empty,
                    SubClass = string.Empty,
                    Type = "MotorInsurance"
                };

                var productResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                        <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyProductResponse>,
                                        BKIC.SellingPoint.DTO.RequestResponseWrappers.AgecyProductRequest>
                                        (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchAgencyProductByType, productRequest);

                if (productResponse.StatusCode == 200 && productResponse.Result.IsTransactionDone)
                {
                    if (productResponse.Result.MotorProducts != null && productResponse.Result.MotorProducts.Count > 0)
                    {
                        Session["MotorProducts"] = productResponse.Result.MotorProducts;

                        product = motorProduct.Find(x => x.MainClass == MainClass
                                         && x.SubClass == ddlCover.SelectedItem.Value);

                        product.MotorClaim = product.MotorClaim.Where(c => c.MainClass == MainClass &&
                                          c.SubClass == ddlCover.SelectedItem.Value).ToList();
                    }
                }
            }
            return product;
        }

        protected void txtLoad_AmountChanged(object sender, EventArgs e)
        {
            try
            {
                txtDiscount.Text = string.Empty;
                UpdateTotal();
                UpdateTotalWithLoad();
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

        private void UpdateTotalWithLoad()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            decimal PremiumWithLoad = decimal.Zero;
            if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
            {
                var LoadAmount = string.IsNullOrEmpty(txtLoadAmount.Text) ? decimal.Zero : Convert.ToDecimal(txtLoadAmount.Text);
                PremiumWithLoad = Convert.ToDecimal(calculatedPremium.Value) + LoadAmount;
                premiumAmount.Text = Convert.ToString(PremiumWithLoad);
            }
            else
            {
                var LoadAmount = string.IsNullOrEmpty(txtLoadAmount1.Text) ? decimal.Zero : Convert.ToDecimal(txtLoadAmount1.Text);
                PremiumWithLoad = Convert.ToDecimal(calculatedPremium.Value) + LoadAmount;
                premiumAmount1.Text = Convert.ToString(PremiumWithLoad);
            }
            var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                SubClass = ddlCover.SelectedItem.Value.Trim(),
                IsDeductable = true,

                PremiumAmount = PremiumWithLoad
            };

            var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>
                                   (BKIC.SellingPoint.DTO.Constants.CommissionURI.CalculateCommission, commisionRequest);

            if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone
                && commissionresult.Result.CommissionAmount >= 0)
            {
                //calculatedCommision.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                ShowPremium(userInfo, PremiumWithLoad, commissionresult.Result.CommissionAmount);
            }
            //Calculate VAT.
            var vatResponse = master.GetVat(PremiumWithLoad, commissionresult.Result.CommissionAmount);
            if (vatResponse != null && vatResponse.IsTransactionDone)
            {
                decimal TotalPremium = PremiumWithLoad + vatResponse.VatAmount;
                decimal TotalCommission = commissionresult.Result.CommissionAmount + vatResponse.VatCommissionAmount;
                ShowVAT(userInfo, vatResponse.VatAmount, vatResponse.VatCommissionAmount, TotalPremium, TotalCommission);
            }
        }

        protected void LoadAgencyPolicy(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                ApiResponseWrapper<MotorSavedQuotationResponse> motorDetails;
                GetRenewalPolicy(out userInfo, out service, out motorDetails);
                Update(userInfo, service, motorDetails, motorDetails.Result.MotorPolicyDetails, motorDetails.Result.MotorPolicyDetails.ClaimAmount);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "CloseInfoPopup();", true);

            }
            catch
            {

            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void LoadOraclePolicy(object sender, EventArgs e)
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

    }
}