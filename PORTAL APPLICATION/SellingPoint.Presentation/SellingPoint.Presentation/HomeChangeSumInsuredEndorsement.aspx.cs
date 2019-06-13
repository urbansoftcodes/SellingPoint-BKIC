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
    public partial class HomeChangeSumInsuredEndorsement : System.Web.UI.Page
    {
        private General master;
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails> InsuredNames { get; set; }
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomePolicy> policyList;
        public static long _HomeEndorsementID { get; set; }
        public static string MainClass { get; set; }
        public static bool AjdustedPremium { get; set; }
        public static string SubClass { get; set; }
        public static decimal BuildingValue { get; set; }
        public static decimal ContentValue { get; set; }
        public static bool isRiotAdded { get; set; }

        public HomeChangeSumInsuredEndorsement()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            if (!Page.IsPostBack)
            {
                BindAgencyClientCodeDropdown(userInfo, service);
                _HomeEndorsementID = 0;
                btnSubmit.Visible = false;
            }
        }

        private void BindAgencyClientCodeDropdown(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                 (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}",
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.DomesticHelp));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable branches = dropdownds.Tables["BranchMaster"];
                DataTable Financier = dropdownds.Tables["Financier"];

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
            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest();
            req.AgentBranch = userInfo.AgentBranch;
            req.AgentCode = userInfo.AgentCode;

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

            var homereq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                AgentBranch = userInfo.AgentBranch,             
                CPR = txtCPRSearch.Text.Trim(),
                Type = Constants.Home
            };

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

            var homereq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomeRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                AgentBranch = userInfo.AgentBranch,
                IncludeHIR = false,
                IsRenewal = false,
                DocumentNo = txtHomeEndorsementSearch.Text.Trim()
            };

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
                txtEffectiveFromDate.Text = DateTime.Now.CovertToLocalFormat();// response.PolicyStartDate.CovertToLocalFormat();
                txtEffectiveToDate.Text = response.PolicyExpiryDate.CovertToLocalFormat();
                paidPremium.Value = Convert.ToString(response.PremiumAfterDiscount);
                subClass.Value = response.SubClass;
                SubClass = response.SubClass;
                MainClass = response.MainClass;
                expireDate.Value = response.PolicyExpiryDate.CovertToLocalFormat();
                BuildingValue = response.BuildingValue;
                ContentValue = response.ContentValue;
                txtOldBuildingSumInsured.Text = Convert.ToString(response.BuildingValue);
                txtOldContentSumInsured.Text = Convert.ToString(response.ContentValue);

                isRiotAdded = response.IsRiotStrikeDamage.ToString().ToUpper() == "Y" ? true : false;

                var productRequest = new AgecyProductRequest();
                productRequest.Agency = userInfo.Agency;
                productRequest.AgentCode = userInfo.AgentCode;
                productRequest.MainClass = response.MainClass;
                productRequest.SubClass = response.SubClass;
                productRequest.Type = Constants.Home;

                var productResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                        <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyProductResponse>,
                        BKIC.SellingPoint.DTO.RequestResponseWrappers.AgecyProductRequest>
                        (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchAgencyProductByType, productRequest);

                if (productResponse.StatusCode == 200 && productResponse.Result.IsTransactionDone)
                {
                    if (productResponse.Result.HomeProducts != null && productResponse.Result.HomeProducts.Count > 0)
                    {
                        maxBuildingValue.Value = Convert.ToString(productResponse.Result.HomeProducts[0].MaximumBuildingValue);
                        maxContentValue.Value = Convert.ToString(productResponse.Result.HomeProducts[0].MaximumContentValue);
                    }
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

        protected void Changed_BuildingSumInsured(object sender, EventArgs e)
        {
            try
            {
                ValidateBuildingSumInsured();
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

        protected void Changed_ContentSumInsured(object sender, EventArgs e)
        {
            try
            {
                ValidateContentSumInsured();
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

        public bool ValidateBuildingSumInsured()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();        

            btnSubmit.Visible = false;

            if (!string.IsNullOrEmpty(txtNewBuildingSumInsured.Text) &&
               !string.IsNullOrEmpty(maxBuildingValue.Value)
               && Convert.ToDecimal(maxBuildingValue.Value) < Convert.ToDecimal(txtNewBuildingSumInsured.Text)
               && (userInfo.Roles == "User" || userInfo.Roles == "BranchAdmin"))
            {
                master.ShowErrorPopup("Building sum insured value is greater " +
                    "than product maximum Value and the maximum value is :" + maxBuildingValue.Value, "Validation Error");
                return false;
            }
            return true;
        }

        public bool ValidateContentSumInsured()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
          
            btnSubmit.Visible = false;

            if (!string.IsNullOrEmpty(txtNewContentSumInsured.Text) &&
                   !string.IsNullOrEmpty(maxContentValue.Value)
                   && Convert.ToDecimal(maxContentValue.Value) < Convert.ToDecimal(txtNewContentSumInsured.Text)
                   && (userInfo.Roles == "User" || userInfo.Roles == "BranchAdmin"))
            {
                master.ShowErrorPopup("Content sum insured value is greater than product maximum Value " +
                    "and the maximum value is :" + maxContentValue.Value, "Validation Error");
                return false;
            }
            return true;
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

        //protected void Set_SumInsured(object sender, EventArgs e)
        //{
        //    if(ddlType.SelectedIndex > 0)
        //    {
        //        if(ddlType.SelectedIndex == 1)
        //        {
        //            txtOldSumInsured.Text = Convert.ToString(BuildingValue);
        //        }
        //        else if (ddlType.SelectedIndex == 2)
        //        {
        //            txtOldSumInsured.Text = Convert.ToString(ContentValue);
        //        }
        //        else
        //        {
        //            txtOldSumInsured.Text = "";
        //        }
        //    }
        //}

        protected void Calculate_Click(object sender, EventArgs e)
        {
            try
            {
                DisablePaymentValidator();
                Page.Validate();              
                if (Page.IsValid)
                {
                    if (ddlHomePolicies.SelectedIndex > 0 && ValidateBuildingSumInsured() && ValidateContentSumInsured())
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

            if (string.IsNullOrEmpty(txtNewBuildingSumInsured.Text) && string.IsNullOrEmpty(txtNewContentSumInsured.Text))
            {
                master.ShowErrorPopup("Please enter building value or content value !!", "Change Sum Insured");
                return;
            }

            bool sumInsuredDeduct = false;
            decimal NewBuildingSumInsured = 0;
            decimal NewContentSumInsured = 0;

            if (!string.IsNullOrEmpty(txtNewBuildingSumInsured.Text))
            {
                NewBuildingSumInsured = Convert.ToDecimal(txtNewBuildingSumInsured.Text) - Convert.ToDecimal(txtOldBuildingSumInsured.Text);
            }
            if (!string.IsNullOrEmpty(txtNewContentSumInsured.Text))
            {
                NewContentSumInsured = Convert.ToDecimal(txtNewContentSumInsured.Text) - Convert.ToDecimal(txtOldContentSumInsured.Text);
            }
            decimal newSumInsured = NewBuildingSumInsured + NewContentSumInsured;

            if (newSumInsured < 0)
            {
                newSumInsured = Math.Abs(newSumInsured);
                sumInsuredDeduct = true;
            }

            var homeEndorementQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementQuote();
            homeEndorementQuote.Agency = userInfo.Agency;
            homeEndorementQuote.AgentCode = userInfo.AgentCode;
            homeEndorementQuote.MainClass = MainClass;
            homeEndorementQuote.SubClass = SubClass;
            homeEndorementQuote.EffectiveFromDate = txtEffectiveFromDate.Text.CovertToCustomDateTime();
            homeEndorementQuote.EffectiveToDate = txtEffectiveToDate.Text.CovertToCustomDateTime();
            homeEndorementQuote.PaidPremium = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value);
            homeEndorementQuote.EndorsementType = "ChangeSumInsured";
            homeEndorementQuote.CancelationDate = DateTime.Now;
            homeEndorementQuote.NewSumInsured = newSumInsured;
            homeEndorementQuote.DocumentNumber = ddlHomePolicies.SelectedItem.Text.Trim();

            //Calculate the home endorsement premium.
            var homeEndoQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementQuoteResponse>,
                                              BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementQuote>
                                              (BKIC.SellingPoint.DTO.Constants.HomeEndorsementURI.GetHomeEndorsementQuote,
                                              homeEndorementQuote);

            if (homeEndoQuoteResult.StatusCode == 200 && homeEndoQuoteResult.Result.IsTransactionDone)
            {
                var endoresementPremium = homeEndoQuoteResult.Result.EndorsementPremium;
                if (sumInsuredDeduct)
                {
                    calculatedPremium.Value = Convert.ToString(endoresementPremium * -1);
                    adjustedPremium.Value = Convert.ToString(endoresementPremium * -1);
                    calculatedCommission.Value = Convert.ToString(homeEndoQuoteResult.Result.Commission * -1);
                    adjustedCommission.Value = Convert.ToString(homeEndoQuoteResult.Result.Commission * -1);
                    ShowPremium(userInfo, endoresementPremium * -1, homeEndoQuoteResult.Result.Commission * -1);
                }
                else
                {
                    calculatedPremium.Value = Convert.ToString(endoresementPremium);
                    adjustedPremium.Value = Convert.ToString(endoresementPremium);
                    calculatedCommission.Value = Convert.ToString(homeEndoQuoteResult.Result.Commission);
                    adjustedCommission.Value = Convert.ToString(homeEndoQuoteResult.Result.Commission);
                    ShowPremium(userInfo, endoresementPremium, homeEndoQuoteResult.Result.Commission);
                }
                btnSubmit.Visible = true;
            }
            else
            {
                master.ShowLoading = false;
                master.ShowErrorPopup(homeEndoQuoteResult.Result.TransactionErrorMessage, "Request Failed !");
                return;
            }
            //Calculate VAT.
            var vatResponse = master.GetVat(homeEndoQuoteResult.Result.EndorsementPremium, homeEndoQuoteResult.Result.Commission);
            if (vatResponse != null && vatResponse.IsTransactionDone)
            {
                decimal TotalPremium = homeEndoQuoteResult.Result.EndorsementPremium + vatResponse.VatAmount;
                decimal TotalCommission = homeEndoQuoteResult.Result.Commission + vatResponse.VatCommissionAmount;
                if(sumInsuredDeduct)
                {
                    ShowVAT(userInfo, vatResponse.VatAmount * -1, vatResponse.VatCommissionAmount * -1, TotalPremium * -1, TotalCommission * -1);
                }
                else
                {
                    ShowVAT(userInfo, vatResponse.VatAmount, vatResponse.VatCommissionAmount, TotalPremium, TotalCommission);
                }
               
            }
        }

        public void ShowPremium(OAuthTokenResponse userInfo, decimal Premium, decimal Commission)
        {
            amtDisplay.Visible = true;
            if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
            {
                if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin)
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

            var req = new HomeEndorsementPreCheckRequest
            {
                DocNo = ddlHomePolicies.SelectedIndex > 0 ? ddlHomePolicies.SelectedItem.Text : string.Empty
            };

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
                if (!ValidateBuildingSumInsured() || !ValidateContentSumInsured())
                {
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
                //if (Convert.ToDecimal(premiumAmount.Text) < Convert.ToDecimal(calculatedPremium.Value))
                //{
                //    postHomeEndorsement.UserChangedPremium = true;
                //    postHomeEndorsement.PremiumAfterDiscount = Convert.ToDecimal(premiumAmount.Text);
                //    var diff = Convert.ToDecimal(calculatedPremium.Value) - postHomeEndorsement.PremiumAfterDiscount;
                //    postHomeEndorsement.CommissionAfterDiscount = Convert.ToDecimal(calculatedCommision.Value) - diff;
                //}

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
            homeEndorsement.EndorsementType = "ChangeSumInsured";
            homeEndorsement.FinancierCompanyCode = homePolicyDetails.FinancierCode;
            homeEndorsement.NewSumInsured = 0;
            homeEndorsement.BlockNo = homePolicyDetails.BlockNo;
            homeEndorsement.BuildingAge = homePolicyDetails.BuildingAge;
            homeEndorsement.BuildingNo = homePolicyDetails.BuildingNo;
            homeEndorsement.Area = homePolicyDetails.Area;
            homeEndorsement.NoOfFloors = homePolicyDetails.NoOfFloors;
            homeEndorsement.HouseNo = homePolicyDetails.HouseNo;
            homeEndorsement.FlatNo = homePolicyDetails.FlatNo;
            homeEndorsement.RoadNo = homePolicyDetails.RoadNo;
            homeEndorsement.BuildingType = homePolicyDetails.BuildingType;
            decimal NewBuildingSumInsured = 0;
            decimal NewContentSumInsured = 0;
            if (!string.IsNullOrEmpty(txtNewBuildingSumInsured.Text))
            {
                NewBuildingSumInsured = Convert.ToDecimal(txtNewBuildingSumInsured.Text) - Convert.ToDecimal(txtOldBuildingSumInsured.Text);
            }

            if (!string.IsNullOrEmpty(txtNewContentSumInsured.Text))
            {
                NewContentSumInsured = Convert.ToDecimal(txtNewContentSumInsured.Text) - Convert.ToDecimal(txtOldContentSumInsured.Text);
            }

            homeEndorsement.NewSumInsured = NewBuildingSumInsured + NewContentSumInsured;
            homeEndorsement.BuildingSumInsured = string.IsNullOrEmpty(txtNewBuildingSumInsured.Text) ? Convert.ToDecimal(txtOldBuildingSumInsured.Text) : Convert.ToDecimal(txtNewBuildingSumInsured.Text);
            homeEndorsement.ContentSumInsured = string.IsNullOrEmpty(txtNewContentSumInsured.Text) ? Convert.ToDecimal(txtOldContentSumInsured.Text) : Convert.ToDecimal(txtNewContentSumInsured.Text);
            homeEndorsement.PremiumBeforeDiscount = string.IsNullOrEmpty(calculatedPremium.Value) ? decimal.Zero : Convert.ToDecimal(calculatedPremium.Value);
            homeEndorsement.PremiumAfterDiscount = string.IsNullOrEmpty(adjustedPremium.Value) ? decimal.Zero : Convert.ToDecimal(adjustedPremium.Value);
            homeEndorsement.CommisionBeforeDiscount = string.IsNullOrEmpty(calculatedCommission.Value) ? decimal.Zero : Convert.ToDecimal(calculatedCommission.Value);
            homeEndorsement.CommissionAfterDiscount = string.IsNullOrEmpty(adjustedCommission.Value) ? decimal.Zero : Convert.ToDecimal(adjustedCommission.Value);
            homeEndorsement.IsRiotStrikeDamage = homePolicyDetails.IsRiotStrikeDamage.ToString();
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
            btnSubmit.Visible = false;
            txtOldClientCode.Text = string.Empty;
            txtOldInsuredName.Text = string.Empty;
            txtEffectiveFromDate.Text = string.Empty;
            txtEffectiveToDate.Text = string.Empty;
            txtAccountNumber.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            premiumAmount.Text = string.Empty;
            premiumAmount1.Text = string.Empty;
            commission.Text = string.Empty;
            commission1.Text = string.Empty;
            includeDisc.Visible = false;
            excludeDisc.Visible = false;
            ddlPaymentMethods.SelectedIndex = 0;
            amtDisplay.Visible = false;
            gvHomeEndorsement.DataSource = null;
            gvHomeEndorsement.DataBind();
            txtOldBuildingSumInsured.Text = string.Empty;
            txtNewBuildingSumInsured.Text = string.Empty;
            txtOldContentSumInsured.Text = string.Empty;
            txtNewContentSumInsured.Text = string.Empty;
        }

        protected void Premium_Changed(object sender, EventArgs e)
        {
            try
            {
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