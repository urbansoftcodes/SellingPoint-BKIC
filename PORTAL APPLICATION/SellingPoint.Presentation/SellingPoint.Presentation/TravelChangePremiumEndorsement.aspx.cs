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
    public partial class TravelChangePremiumEndorsement : System.Web.UI.Page
    {
        private General master;
        public static DataTable Genderdt;
        public static DataTable Nationalitydt;
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelPolicy> policyList;
        public static DataTable Relationdt;

        public static long _TravelEndorsementID { get; set; }
        public static bool AjdustedPremium { get; set; }
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails> InsuredNames { get; set; }
        public static string MainClass { get; set; }
        public static string SubClass { get; set; }
        public static decimal OriginalPremium { get; set; }
        public static string PackageName { get; set; }

        public TravelChangePremiumEndorsement()
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
                _TravelEndorsementID = 0;
                btnSubmit.Visible = false;
                divPaymentSection.Visible = userInfo.IsShowPayments;
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

            if (insuredResult.StatusCode == 200 && insuredResult.Result.IsTransactionDone
                && insuredResult.Result.AgencyInsured.Count > 0)
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
                txtTravelEndorsementSearch.Text = string.Empty;
                GetTravelPoliciesByCPR();
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

        private void GetTravelPoliciesByCPR()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var travelreq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                AgentBranch = userInfo.AgentBranch,                
                CPR = txtCPRSearch.Text.Trim(),
                Type = Constants.Travel,
                isEndorsement = true
            };

            //Get PolicyNo by Agency
            var travelPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelPolicyResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelRequest>
                                (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetTravelPoliciesByCPR, travelreq);

            ddlTravelPolicies.Items.Clear();

            if (travelPolicies.StatusCode == 200 && travelPolicies.Result.AgencyTravelPolicies.Count > 0)
            {
                policyList = travelPolicies.Result.AgencyTravelPolicies;

                ddlTravelPolicies.DataSource = travelPolicies.Result.AgencyTravelPolicies;
                ddlTravelPolicies.DataTextField = "DOCUMENTNO";
                ddlTravelPolicies.DataValueField = "DOCUMENTNO";
                ddlTravelPolicies.DataBind();
                ddlTravelPolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
            }         
            ClearControls();
        }

        protected void Changed_TravelPolicy(object sender, EventArgs e)
        {
            try
            {
                //Travel Insurance don't have renewal passed renewal count to Zero.
                var policyRenewalCount = ddlTravelPolicies.SelectedItem.Value.Substring(0, ddlTravelPolicies.SelectedValue.IndexOf("-", 0));
                UpdatePolicyDetails(ddlTravelPolicies.SelectedItem.Text.Trim(), Convert.ToString(0), false);
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
                //Travel Insurance don't have renewal passed renewal count to Zero.
                //var policyRenewalCount = string.IsNullOrEmpty(renewalCount.Value) ? Convert.ToString(0) : renewalCount.Value;
                //UpdatePolicyDetails(txtTravelEndorsementSearch.Text.Trim(), Convert.ToString(0), true);

                UpdatePolicyDetails(txtTravelEndorsementSearch.Text.Trim(), Convert.ToString(0), true);
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


        private void UpdatePolicyDetails(string DocumentNo, string RenewalCount, bool SearchByDocument)
        {
            ClearControls();            

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            //Get saved policy details by document(policy) number.
            var url = BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetSavedQuoteDocumentNo
                      .Replace("{documentNo}", DocumentNo)
                      .Replace("{agentCode}", userInfo.AgentCode)
                      .Replace("{isendorsement}", "true")
                      .Replace("{endorsementid}", "0");

            var travelDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelSavedQuotationResponse>>(url);

            //Update policy details on current page for dispaly the details.
            if (travelDetails.StatusCode == 200 && travelDetails.Result.IsTransactionDone)
            {
                var response = travelDetails.Result.TravelInsurancePolicyDetails;

                txtOldClientCode.Text = response.InsuredCode;
                txtOldInsuredName.Text = response.InsuredName;
                txtEffectiveFromDate.Text = response.InsuranceStartDate.ConvertToLocalFormat();
                txtEffectiveToDate.Text = response.ExpiryDate.ConvertToLocalFormat();
                paidPremium.Value = Convert.ToString(response.PremiumAfterDiscount);
                subClass.Value = response.SubClass;
                SubClass = response.SubClass;
                MainClass = response.MainClass;
                expireDate.Value = response.ExpiryDate.ConvertToLocalFormat();
                txtActualPremium.Text = Convert.ToString(response.PremiumAfterDiscount);
                IsCancelled(response.IsCancelled);
                if (SearchByDocument)
                {
                    ddlTravelPolicies.Items.Clear();
                    txtCPRSearch.Text = response.CPR;
                    ddlTravelPolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
                    ddlTravelPolicies.Items.Insert(1, new ListItem(txtTravelEndorsementSearch.Text.Trim(), RenewalCount + "-" + txtTravelEndorsementSearch.Text.Trim()));
                    ddlTravelPolicies.DataBind();
                    ddlTravelPolicies.SelectedIndex = 1;
                }
                //List the previous endorsements for the policy.
                ListEndorsements(service, userInfo);
            }
            else
            {
                master.ShowErrorPopup(travelDetails.Result.TransactionErrorMessage, "Request failed!");
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
                //throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        public void EndorsementOperation(object sender, string type)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsementOperation
                {
                    TravelEndorsementID = Convert.ToInt32((row.FindControl("lblTravelEndorsementID") as Label).Text.Trim()),
                    TravelID = Convert.ToInt32((row.FindControl("lblTravelID") as Label).Text.Trim()),
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    Type = type,
                    UpdatedBy = Convert.ToInt32(userInfo.ID)
                };

                var endoResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsementOperationResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsementOperation>
                                 (BKIC.SellingPoint.DTO.Constants.TravelEndorsementURI.EndorsementOperation, details);

                if (endoResult.StatusCode == 200 && endoResult.Result.IsTransactionDone)
                {
                    ListEndorsements(service, userInfo);
                    if (type == "delete")
                    {
                        master.ShowErrorPopup("Your endorsement deleted successfully", "Travel Endorsement");
                    }
                    else if (type == "authorize")
                    {
                        master.ShowErrorPopup("Your endorsement authorized successfully", "Travel Endorsement");
                    }
                }
            }
        }

        private void ListEndorsements(DataServiceManager service, OAuthTokenResponse userInfo)
        {
            if (ddlTravelPolicies.SelectedIndex > 0)
            {
                var travelEndoRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndoRequest();
                if (userInfo == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    travelEndoRequest.Agency = userInfo.Agency;
                    travelEndoRequest.AgentCode = userInfo.AgentCode;
                    travelEndoRequest.InsuranceType = Constants.Travel;
                    travelEndoRequest.DocumentNo = ddlTravelPolicies.SelectedItem.Text.Trim();

                    var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndoResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndoRequest>
                                 (BKIC.SellingPoint.DTO.Constants.TravelEndorsementURI.GetAllEndorsements, travelEndoRequest);

                    if (result.StatusCode == 200 && result.Result.IsTransactionDone)
                    {
                        gvTravelEndorsement.DataSource = result.Result.TravelEndorsements;
                        gvTravelEndorsement.DataBind();

                        if (result.Result.TravelEndorsements.Count > 0)
                        {
                            _TravelEndorsementID = result.Result.TravelEndorsements[result.Result.TravelEndorsements.Count - 1].TravelEndorsementID;
                        }
                        else
                        {
                            _TravelEndorsementID = 0;
                        }
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
                DisablePaymentValidator();
                Page.Validate();            
                if (Page.IsValid)
                {
                    if (ddlTravelPolicies.SelectedIndex > 0)
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

            if (string.IsNullOrEmpty(txtNewPremium.Text))
            {
                master.ShowErrorPopup("Please enter premium value !!", "Change Premium");
                return;
            }

            bool premiumDeduct = false;
            decimal newPremium = Convert.ToDecimal(txtNewPremium.Text);
            if (Convert.ToDecimal(txtNewPremium.Text) < 0)
            {
                newPremium = Math.Abs(Convert.ToDecimal(txtNewPremium.Text));
                premiumDeduct = true;
            }

            var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest();
            commisionRequest.AgentCode = userInfo.AgentCode;
            commisionRequest.Agency = userInfo.Agency;
            commisionRequest.SubClass = subClass.Value;
            commisionRequest.PremiumAmount = newPremium;
            commisionRequest.IsDeductable = true;

            //Get commision for the endorsement premium.
            var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>
                                       ("api/insurance/Commission", commisionRequest);

            if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone
                && commissionresult.Result.CommissionAmount >= 0)
            {
                //commission.Text = Convert.ToString(commissionresult.Result.CommissionAmount);

                if (premiumDeduct)
                {
                    calculatedPremium.Value = Convert.ToString(newPremium * -1);
                    adjustedPremium.Value = Convert.ToString(newPremium * -1);
                    calculatedCommission.Value = Convert.ToString(commissionresult.Result.CommissionAmount * -1);
                    adjustedCommission.Value = Convert.ToString(commissionresult.Result.CommissionAmount * -1);
                    ShowPremium(userInfo, newPremium * -1, commissionresult.Result.CommissionAmount * -1);
                }
                else
                {
                    calculatedPremium.Value = Convert.ToString(newPremium);
                    adjustedPremium.Value = Convert.ToString(newPremium);
                    calculatedCommission.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                    adjustedCommission.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                    ShowPremium(userInfo, newPremium, commissionresult.Result.CommissionAmount);
                }
                btnSubmit.Visible = true;
            }
        }


        protected void NewPremiun_Changed(object sender, EventArgs e)
        {
            calculatedPremium.Value = txtNewPremium.Text;
            btnSubmit.Visible = false;
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
                return "Your travel endorsement saved and moved into HIR :" + docNo;
            }
            else
            {
                return "Your travel endorsement has been saved sucessfully :" + docNo;
            }
        }

        public void HidePremium()
        {
            amtDisplay.Visible = false;

            premiumAmount.Text = Convert.ToString(0);
            commission.Text = Convert.ToString(0);
            includeDisc.Visible = false;

            premiumAmount1.Text = Convert.ToString(0);
            commission1.Text = Convert.ToString(0);
            excludeDisc.Visible = false;
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
                btnSubmit.Enabled = true;
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
                //throw ex;
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

            var req = new TravelEndorsementPreCheckRequest
            {
                DocNo = ddlTravelPolicies.SelectedIndex > 0 ? ddlTravelPolicies.SelectedItem.Text : string.Empty
            };

            var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsementPreCheckResponse>,
                         BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsementPreCheckRequest>
                         (BKIC.SellingPoint.DTO.Constants.TravelEndorsementURI.EndorsementPreCheck, req);

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
                    master.ShowErrorPopup("Your travel policy already have saved endorsement !", "Travel Endorsement");
                    return;
                }
                var postTravelEndorsement = new BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsement
                {
                    Agency = userInfo.Agency,
                    AgencyCode = userInfo.AgentCode,
                    AgentBranch = userInfo.AgentBranch,
                    IsSaved = isSave,
                    IsActivePolicy = !isSave,
                    PremiumAmount = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value),
                    CreatedBy = Convert.ToInt32(userInfo.ID),
                    NewPremium = Convert.ToDecimal(calculatedPremium.Value),
                    DocumentNo = ddlTravelPolicies.SelectedItem.Text.Trim()
                };

                //Get saved policy details by document(policy) number.
                var url = BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetSavedQuoteDocumentNo
                          .Replace("{documentNo}", ddlTravelPolicies.SelectedItem.Text.Trim())
                          .Replace("{agentCode}", userInfo.AgentCode)
                          .Replace("{isendorsement}", "true")
                          .Replace("{endorsementid}", "0");

                var travelDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (travelDetails.StatusCode == 200 && travelDetails.Result.IsTransactionDone)
                {
                    SetEndorsementType(postTravelEndorsement, travelDetails.Result.TravelInsurancePolicyDetails);
                }
                //if (Convert.ToDecimal(txtNewPremium.Text) < Convert.ToDecimal(calculatedPremium.Value))
                //{
                //    postTravelEndorsement.UserChangedPremium = true;
                //    postTravelEndorsement.PremiumAfterDiscount = Convert.ToDecimal(txtNewPremium.Text);
                //    var diff = postTravelEndorsement.PremiumBeforeDiscount - postTravelEndorsement.PremiumAfterDiscount;
                //    postTravelEndorsement.CommissionAfterDiscount = Convert.ToDecimal(calculatedCommission.Value) - diff;
                //}

                var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsementResponse>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsement>
                             (BKIC.SellingPoint.DTO.Constants.TravelEndorsementURI.PostTravelEndorsement, postTravelEndorsement);

                if (response.Result != null && response.StatusCode == 200 && response.Result.IsTransactionDone)
                {
                    _TravelEndorsementID = response.Result.TravelEndorsementID;
                    ListEndorsements(service, userInfo);
                    btnSubmit.Visible = false;
                    master.ShowErrorPopup("Your travel endorsement has been saved sucessfully :" + response.Result.EndorsementNo, "Travel Endorsement");
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

        public void SetEndorsementType(BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsement travelEndorsement, TravelInsurancePolicy travelPolicyDetails)
        {
            travelEndorsement.TravelID = travelPolicyDetails.TravelID;
            travelEndorsement.Mainclass = travelPolicyDetails.MainClass;
            travelEndorsement.PolicyCommencementDate = travelPolicyDetails.InsuranceStartDate ?? DateTime.Now;
            travelEndorsement.ExpiryDate = txtEffectiveToDate.Text.CovertToCustomDateTime();
            travelEndorsement.Subclass = travelPolicyDetails.SubClass;
            travelEndorsement.CancelDate = null;
            travelEndorsement.PaymentType = ddlPaymentMethods.SelectedIndex > 0 ? ddlPaymentMethods.SelectedItem.Text : string.Empty;
            travelEndorsement.Remarks = txtRemarks.Text;
            travelEndorsement.AccountNumber = txtAccountNumber.Text;
            travelEndorsement.EndorsementType = "ChangePremium";
            travelEndorsement.InsuredCode = travelPolicyDetails.InsuredCode;
            travelEndorsement.InsuredName = travelPolicyDetails.InsuredName;
            travelEndorsement.PremiumBeforeDiscount = string.IsNullOrEmpty(calculatedPremium.Value) ? decimal.Zero : Convert.ToDecimal(calculatedPremium.Value);
            travelEndorsement.PremiumAfterDiscount = string.IsNullOrEmpty(adjustedPremium.Value) ? decimal.Zero : Convert.ToDecimal(adjustedPremium.Value);
            travelEndorsement.CommisionBeforeDiscount = string.IsNullOrEmpty(calculatedCommission.Value) ? decimal.Zero : Convert.ToDecimal(calculatedCommission.Value);
            travelEndorsement.CommissionAfterDiscount = string.IsNullOrEmpty(adjustedCommission.Value) ? decimal.Zero : Convert.ToDecimal(adjustedCommission.Value);
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

        protected void gvTravelEndorsement_DataBound(object sender, EventArgs e)
        {            

            foreach (GridViewRow row in gvTravelEndorsement.Rows)
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

        protected void gvTravelEndorsement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlAnchor lnkSchedule = e.Row.FindControl("downloadschedule") as HtmlAnchor;
                var btnAuthorize = e.Row.FindControl("lnkbtnAuthorize") as LinkButton;
                var endorsementID = e.Row.FindControl("lblTravelEndorsementID") as Label;
                var DocumentNo = e.Row.FindControl("lblDocumentNo") as Label;

                long id = 0;
                if (endorsementID != null)
                {
                    id = Convert.ToInt64(endorsementID.Text);
                }
                lnkSchedule.HRef = ClientUtility.WebApiUri + BKIC.SellingPoint.DTO.Constants.ScheduleURI.downloadschedule
                                   .Replace("{insuranceType}", Constants.Travel)
                                   .Replace("{agentCode}", userInfo.AgentCode)
                                   .Replace("{documentNo}", DocumentNo.Text)
                                   .Replace("{isEndorsement}", "true")
                                   .Replace("{endorsementID}", id.ToString())
                                   .Replace("{renewalCount}", "0");

                bool IsActive = Convert.ToBoolean((e.Row.FindControl("lblIsActive") as Label).Text.Trim());

                if (IsActive)
                    lnkSchedule.Visible = true;
                else
                    lnkSchedule.Visible = false;
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

        public void ClearControls()
        {
            txtEffectiveFromDate.Text = string.Empty;
            txtEffectiveToDate.Text = string.Empty;
            txtOldClientCode.Text = string.Empty;
            txtOldInsuredName.Text = string.Empty;
            txtAccountNumber.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            txtActualPremium.Text = string.Empty;
            txtNewPremium.Text = string.Empty;
            premiumAmount.Text = string.Empty;
            premiumAmount1.Text = string.Empty;
            commission.Text = string.Empty;
            commission1.Text = string.Empty;
            btnSubmit.Visible = false;
            gvTravelEndorsement.DataSource = null;
            gvTravelEndorsement.DataBind();
            includeDisc.Visible = false;
            excludeDisc.Visible = false;
            ddlPaymentMethods.SelectedIndex = 0;
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

                //var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest();
                //commisionRequest.AgentCode = userInfo.AgentCode;
                //commisionRequest.Agency = userInfo.Agency;
                //commisionRequest.SubClass = subClass.Value;
                //commisionRequest.PremiumAmount = string.IsNullOrEmpty(premiumAmount.Text) ? decimal.Zero : Convert.ToDecimal(premiumAmount.Text);

                ////Get commision for the endorsement premium.
                //var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                //                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                //                       BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>("api/insurance/Commission", commisionRequest);

                //if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone)
                //{
                //    adjustedCommission.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                //    commission.Text = Convert.ToString(commissionresult.Result.CommissionAmount);
                //}
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

        protected void Commission_Changed(object sender, EventArgs e)
        {
            try
            {
                adjustedCommission.Value = commission.Text.Trim();
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