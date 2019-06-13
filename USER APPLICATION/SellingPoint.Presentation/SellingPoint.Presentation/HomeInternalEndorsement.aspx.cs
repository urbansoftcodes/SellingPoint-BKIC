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
    public partial class HomeInternalEndorsement : System.Web.UI.Page
    {
        private General master;
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails> InsuredNames { get; set; }
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyHomePolicy> policyList;
        public static long _HomeEndorsementID { get; set; }
        public static string MainClass { get; set; }
        public static bool AjdustedPremium { get; set; }
        public static string SubClass { get; set; }
        public static DataTable Area;
        public static DataTable Nationalitydt;
        public static DataTable domesticHelpOccupationDt;

        public HomeInternalEndorsement()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!Page.IsPostBack)
            {
                BindAgencyClientCodeDropdown();
                SetInitialRow();
                _HomeEndorsementID = 0;
                btnSubmit.Enabled = false;
                ChangeAddressDiv.Visible = true;
            }
        }

        private void BindAgencyClientCodeDropdown()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                 (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}",
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.HomeInsurance));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable branches = dropdownds.Tables["BranchMaster"];
                DataTable area = dropdownds.Tables["AreaMaster"];
                Area = dropdownds.Tables["AreaMaster"];

                Nationalitydt = dropdownds.Tables["Nationality"];
                domesticHelpOccupationDt = dropdownds.Tables["DomesticWorkerOccupation"];

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
                    ddlArea.DataValueField = "AreaCode";
                    ddlArea.DataTextField = "Description";
                    ddlArea.DataSource = area;
                    ddlArea.DataBind();
                    ddlArea.Items.Insert(0, new ListItem("--Please Select--", ""));
                    // ddlArea.Items[0].Selected = true;
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

        protected void BlockNumber_Changed(object sender, EventArgs e)
        {
            try
            {
                SetArea();
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

        public void SetArea()
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

        protected void txtCPR_Changed(object sender, EventArgs e)
        {
            try
            {
                txtHomeEndorsementSearch.Text = string.Empty;
                GetHomePoliciesByCPR();
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
                //throw ex;
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
                //throw ex;
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
                txtEffectiveFromDate.Text = response.PolicyStartDate.CovertToLocalFormat();
                txtEffectiveToDate.Text = response.PolicyExpiryDate.CovertToLocalFormat();
                paidPremium.Value = Convert.ToString(response.PremiumAfterDiscount);
                subClass.Value = response.SubClass;
                SubClass = response.SubClass;
                MainClass = response.MainClass;
                ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(userInfo.AgentBranch));
                ddlBuildingAge.SelectedIndex = ddlBuildingAge.Items.IndexOf(ddlBuildingAge.Items.FindByText(Convert.ToString(response.BuildingAge)));
                ddlBuildingType.SelectedIndex = ddlBuildingType.Items.IndexOf(ddlBuildingType.Items.FindByValue(Convert.ToString(response.BuildingType)));
                txtFlatNo.Text = response.FlatNo;
                txtRoadNo.Text = response.RoadNo;
                txtHouseNo.Text = response.HouseNo;
                txtNoOfFloor.Text = Convert.ToString(response.NoOfFloors);
                txtBuildingNo.Text = response.BuildingNo;
                txtBlockNo.Text = response.BlockNo;
                SetArea();
                EnableAddressfields();
                txtBuildingValue.Text = Convert.ToString(response.BuildingValue);
                txtContentValue.Text = Convert.ToString(response.ContentValue);
                txtJewelleryValue.Text = Convert.ToString(response.JewelleryValue);
                UpdateDomesticHelp(homeDetails.Result.DomesticHelp);

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
                ChangeAddressDiv.Visible = true;
                btnSubmit.Enabled = false;
                master.ShowErrorPopup("This policy is already cancelled", "Policy Cancelled");
            }
            else
            {
                ChangeAddressDiv.Visible = true;
                btnSubmit.Enabled = true;
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
            }
            else if (ddlBuildingType.SelectedIndex == 2)
            {
                txtFlatNo.Text = string.Empty;
                txtFlatNo.Enabled = true;
                txtHouseNo.Text = string.Empty;
                txtHouseNo.Enabled = false;
                txtNoOfFloor.Enabled = true;
                txtRoadNo.Enabled = true;
                txtBlockNo.Enabled = true;
                txtBuildingNo.Enabled = true;
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
                //throw ex;
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
                //throw ex;
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
                //throw ex;
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
                //throw ex;
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

            var homeEndorementQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementQuote
            {
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode,
                MainClass = MainClass,
                SubClass = SubClass,
                EffectiveFromDate = txtEffectiveFromDate.Text.CovertToCustomDateTime(),
                EffectiveToDate = txtEffectiveToDate.Text.CovertToCustomDateTime(),
                PaidPremium = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value),
                EndorsementType = "ChangeAddress",
                CancelationDate = DateTime.Now
            };

            //Calculate the home endorsement premium.
            var homeEndoQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementQuoteResponse>,
                                              BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsementQuote>
                                              (BKIC.SellingPoint.DTO.Constants.HomeEndorsementURI.GetHomeEndorsementQuote, homeEndorementQuote);

            if (homeEndoQuoteResult.StatusCode == 200 && homeEndoQuoteResult.Result.IsTransactionDone)
            {
                var endoresementPremium = homeEndoQuoteResult.Result.EndorsementPremium;
                calculatedPremium.Value = endoresementPremium.ToString();


                var product = master.GetHomeProduct(MainClass, SubClass);
                bool includeCommission = false;
                if (product != null)
                {
                    var hEndorsement = product.HomeEndorsementMaster.Find(c => c.EndorsementType == "ChangeAddress");
                    if (hEndorsement != null)
                    {
                        includeCommission = hEndorsement.HasCommission;
                    }
                }

                var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest();
                commisionRequest.AgentCode = userInfo.AgentCode;
                commisionRequest.Agency = userInfo.Agency;
                commisionRequest.SubClass = string.IsNullOrEmpty(subClass.Value) ? "SH" : subClass.Value;
                commisionRequest.PremiumAmount = includeCommission ? endoresementPremium : decimal.Zero;

                //Get commision for the endorsement premium.
                var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>("api/insurance/Commission", commisionRequest);

                if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone && commissionresult.Result.CommissionAmount > 0)
                {
                    //commission.Text = Convert.ToString(commissionresult.Result.CommissionAmount);
                    calculatedCommision.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                    //ShowPremium(userInfo, endoresementPremium, commissionresult.Result.CommissionAmount);
                }
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

        public bool EndorsementPrecheck()
        {           

            master.IsSessionAvailable();      
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

                var postHomeEndorsement = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeEndorsement
                {
                    Agency = userInfo.Agency,
                    AgencyCode = userInfo.AgentCode,
                    AgentBranch = userInfo.AgentBranch,
                    IsSaved = isSave,
                    IsActivePolicy = !isSave,
                    PremiumAmount = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value),
                    CreatedBy = Convert.ToInt32(userInfo.ID)
                };

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
                    btnSubmit.Enabled = false;
                    master.ShowErrorPopup("Your home endorsement has been saved sucessfully :" + response.Result.EndorsementNo, "Home Endorsement");
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
            homeEndorsement.Remarks = "";
            homeEndorsement.AccountNumber = "";
            homeEndorsement.EndorsementType = "InternalEndorsement";
            homeEndorsement.FinancierCompanyCode = homePolicyDetails.FinancierCode;
            homeEndorsement.BlockNo = txtBlockNo.Text;
            homeEndorsement.BuildingAge = ddlBuildingAge.SelectedIndex > 0 ? Convert.ToInt32(ddlBuildingAge.SelectedItem.Value) : 0;
            homeEndorsement.BuildingNo = txtBuildingNo.Text;
            homeEndorsement.Area = ddlArea.SelectedIndex > 0 ? ddlArea.SelectedItem.Text : "";
            homeEndorsement.NoOfFloors = string.IsNullOrEmpty(txtNoOfFloor.Text) ? 0 : Convert.ToInt32(txtNoOfFloor.Text);
            homeEndorsement.HouseNo = txtHouseNo.Text.Trim();
            homeEndorsement.FlatNo = txtFlatNo.Text.Trim();
            homeEndorsement.RoadNo = txtRoadNo.Text.Trim();
            homeEndorsement.BuildingType = ddlBuildingType.SelectedItem.Text.ToLower() == "flat" ? 2 : ddlBuildingType.SelectedItem.Text.ToLower() == "contents" ? 3 : 1;
            homeEndorsement.BuildingSumInsured = string.IsNullOrEmpty(txtBuildingValue.Text) ? 0 : Convert.ToDecimal(txtBuildingValue.Text);
            homeEndorsement.ContentSumInsured = string.IsNullOrEmpty(txtContentValue.Text) ? 0 : Convert.ToDecimal(txtContentValue.Text);
            homeEndorsement.HomeDomesticHelp = GetHomeDomesticHelps();
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
                //throw ex;
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
            btnSubmit.Enabled = false;
            txtOldClientCode.Text = string.Empty;
            txtOldInsuredName.Text = string.Empty;
            txtEffectiveFromDate.Text = string.Empty;
            txtEffectiveToDate.Text = string.Empty;
            gvHomeEndorsement.DataSource = null;
            gvHomeEndorsement.DataBind();
            txtBlockNo.Text = string.Empty;
            txtBuildingNo.Text = string.Empty;
            txtFlatNo.Text = string.Empty;
            txtHouseNo.Text = string.Empty;
            txtNoOfFloor.Text = string.Empty;
            txtRoadNo.Text = string.Empty;
            ddlArea.SelectedIndex = 0;
            ddlBuildingAge.SelectedIndex = 0;
            ddlBuildingType.SelectedIndex = 0;
            ChangeAddressDiv.Visible = false;
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

        protected void ddlRequireDomesticHelpCover_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRequireDomesticHelpCover.SelectedIndex == 0 && ddlRequireDomesticHelpCover.SelectedValue.ToLower() != "no")
                {
                    SetInitialRow();
                    mainDomesticWorker.Visible = true;
                    divDetailedDomesticWorkers.Visible = true;
                    if (ViewState["CurrentTable"] != null)
                    {
                        DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];                      
                        ViewState["CurrentTable"] = dtCurrentTable;
                        Gridview1.DataSource = dtCurrentTable;
                        Gridview1.DataBind();
                    }
                }
                else
                {                   
                    ResetDependant();                  
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

        public void ResetDependant()
        {
            ViewState["CurrentTable"] = null;
            Gridview1.DataSource = null;
            Gridview1.DataBind();
            mainDomesticWorker.Visible = false;
            divDetailedDomesticWorkers.Visible = false;
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
            //DisableControls();
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
            //DisableControls();
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
        private void UpdateDomesticHelp(List<HomeDomesticHelp> domesticHelpDetails)
        {
            if (domesticHelpDetails != null && domesticHelpDetails.Count > 0)
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
                ddlRequireDomesticHelpCover.SelectedIndex = 0;
            }
            else
            {
                ViewState["CurrentTable"] = null;
                Gridview1.DataSource = null;
                Gridview1.DataBind();
                mainDomesticWorker.Visible = false;
                divDetailedDomesticWorkers.Visible = false;
                ddlRequireDomesticHelpCover.SelectedIndex = 1;

            }
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
    }
}