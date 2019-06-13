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
    public partial class MotorChangeExcessEndorsement : System.Web.UI.Page
    {
        private General master;
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails> InsuredNames { get; set; }
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorPolicy> policyList;
        public static long _MotorEndorsementID { get; set; }
        public static string MainClass { get; set; }
        public static bool AjdustedPremium { get; set; }
        public static string SubClass { get; set; }
        public static string OldRegNumber { get; set; }
        public static string OldChassisNumber { get; set; }

        public MotorChangeExcessEndorsement()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!Page.IsPostBack)
            {
                BindAgencyClientCodeDropdown();
                _MotorEndorsementID = 0;
                btnSubmit.Enabled = false;
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
        protected void txtCPR_Changed(object sender, EventArgs e)
        {
            try
            {
                txtMotorEndorsementSearch.Text = string.Empty;
                GetMotorPoliciesByCPR();
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

        private void GetMotorPoliciesByCPR()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var motorreq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                AgentBranch = userInfo.AgentBranch,              
                CPR = txtCPRSearch.Text.Trim(),
                Type = Constants.Motor,
                isEndorsement = true
            };

            //Get PolicyNo by Agency
            var motorPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorPolicyResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest>
                                (BKIC.SellingPoint.DTO.Constants.MotorURI.GetMotorPoliciesByTypeByCPR, motorreq);

            ddlMotorPolicies.Items.Clear();
            if (motorPolicies.StatusCode == 200 && motorPolicies.Result.IsTransactionDone
                && motorPolicies.Result.AgencyMotorPolicies.Count > 0)
            {
                policyList = motorPolicies.Result.AgencyMotorPolicies;

                ddlMotorPolicies.DataSource = motorPolicies.Result.AgencyMotorPolicies;
                ddlMotorPolicies.DataTextField = "DOCUMENTNO";
                ddlMotorPolicies.DataValueField = "DOCUMENTRENEWALNO";
                ddlMotorPolicies.DataBind();
                ddlMotorPolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
            }           
            ClearControls();
        }

        protected void Changed_MotorPolicy(object sender, EventArgs e)
        {
            try
            {
                if (ddlMotorPolicies.SelectedIndex > 0)
                {
                    var policyRenewalCount = ddlMotorPolicies.SelectedItem.Value.Substring(0, ddlMotorPolicies.SelectedValue.IndexOf("-", 0));
                    UpdatePolicyDetails(ddlMotorPolicies.SelectedItem.Text.Trim(), policyRenewalCount, false);
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
                //UpdatePolicyDetails(txtMotorEndorsementSearch.Text.Trim(), policyRenewalCount, true);

                var renewalCount = GetPolicyRenewalCount();
                UpdatePolicyDetails(txtMotorEndorsementSearch.Text.Trim(), renewalCount.ToString(), true);
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

            var motorreq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest
            {
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                AgentBranch = userInfo.AgentBranch,
                IncludeHIR = false,
                IsRenewal = false,
                DocumentNo = txtMotorEndorsementSearch.Text.Trim()
            };

            //Get PolicyNo by Agency
            var motorPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorPolicyResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest>
                                (BKIC.SellingPoint.DTO.Constants.MotorURI.GetMotorPoliciesEndorsement, motorreq);

            if (motorPolicies.StatusCode == 200 && motorPolicies.Result.IsTransactionDone
                && motorPolicies.Result.AgencyMotorPolicies.Count > 0)
            {
                renewalCount = motorPolicies.Result.AgencyMotorPolicies[0].RenewalCount;
            }
            return renewalCount;

        }


        private void UpdatePolicyDetails(string DocumentNo, string RenewalCount, bool SearchByDocument)
        {
            ClearControls();           

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyDomesticRequest
            {
                AgentBranch = userInfo.AgentBranch,
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency
            };


            // var policyRenewalCount = ddlMotorPolicies.SelectedItem.Value.Substring(0, ddlMotorPolicies.SelectedValue.IndexOf("-", 0));

            //Get saved policy details by document(policy) number.
            var url = BKIC.SellingPoint.DTO.Constants.MotorURI.GetSavedQuoteDocumentNo
                      .Replace("{documentNo}", DocumentNo)
                      .Replace("{agentCode}", request.AgentCode)
                      .Replace("{isendorsement}", "true")
                      .Replace("{endorsementid}", "0")
                      .Replace("{renewalCount}", RenewalCount);

            var motorDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorSavedQuotationResponse>>(url);

            //Update policy details on current page for dispaly the details.
            if (motorDetails.StatusCode == 200 && motorDetails.Result.IsTransactionDone)
            {
                var response = motorDetails.Result.MotorPolicyDetails;

                txtOldClientCode.Text = response.InsuredCode;
                txtOldInsuredName.Text = response.InsuredName;
                txtEffectiveFromDate.Text = DateTime.Now.CovertToLocalFormat();//response.PolicyCommencementDate.CovertToLocalFormat();
                txtEffectiveToDate.Text = response.ExpiryDate.CovertToLocalFormat();
                paidPremium.Value = Convert.ToString(response.PremiumAfterDiscount);
                subClass.Value = response.Subclass;
                SubClass = response.Subclass;
                MainClass = response.Mainclass;
                expireDate.Value = response.ExpiryDate.CovertToLocalFormat();
                txtOldExcess.Text = Convert.ToString(response.ExcessAmount);
                IsCancelled(response.IsCancelled);
                if (SearchByDocument)
                {
                    ddlMotorPolicies.Items.Clear();
                    txtCPRSearch.Text = response.CPR;
                    ddlMotorPolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
                    ddlMotorPolicies.Items.Insert(1, new ListItem(txtMotorEndorsementSearch.Text.Trim(), RenewalCount + "-" + txtMotorEndorsementSearch.Text.Trim()));
                    ddlMotorPolicies.DataBind();
                    ddlMotorPolicies.SelectedIndex = 1;
                }
                //List the previous endorsements for the policy.
                ListEndorsements(service, userInfo);
            }
            else
            {
                master.ShowErrorPopup(motorDetails.Result.TransactionErrorMessage, "Request failed!");
            }            
        }

        public void IsCancelled(bool isCancelled)
        {
            if (isCancelled)
            {
                btnSubmit.Enabled = false;
                master.ShowErrorPopup("This policy is already cancelled", "Policy Cancelled");
            }
            else
            {
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

        public void EndorsementOperation(object sender, string type)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementOperation
                {
                    MotorEndorsementID = Convert.ToInt32((row.FindControl("lblMotorEndorsementID") as Label).Text.Trim()),
                    MotorID = Convert.ToInt32((row.FindControl("lblMotorID") as Label).Text.Trim()),
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    Type = type,
                    UpdatedBy = Convert.ToInt32(userInfo.ID)
                };

                var endoResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementOperationResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementOperation>
                                 (BKIC.SellingPoint.DTO.Constants.MotorEndorsementURI.EndorsementOperation, details);

                if (endoResult.StatusCode == 200 && endoResult.Result.IsTransactionDone)
                {
                    ListEndorsements(service, userInfo);
                    if (type == "delete")
                    {
                        master.ShowErrorPopup("Your endorsement deleted successfully", "Motor Endorsement");
                    }
                    else if (type == "authorize")
                    {
                        master.ShowErrorPopup("Your endorsement authorized successfully", "Motor Endorsement");
                    }
                }
            }
        }

        private void ListEndorsements(DataServiceManager service, OAuthTokenResponse userInfo)
        {
            if (userInfo == null)
                Response.Redirect("Login.aspx");

            if (ddlMotorPolicies.SelectedIndex > 0)
            {
                var motorEndoRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndoRequest();
                motorEndoRequest.Agency = userInfo.Agency;
                motorEndoRequest.AgentCode = userInfo.AgentCode;
                motorEndoRequest.InsuranceType = Constants.Motor;
                motorEndoRequest.DocumentNo = ddlMotorPolicies.SelectedItem.Text.Trim();

                var listEndoResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndoResult>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndoRequest>
                            (BKIC.SellingPoint.DTO.Constants.MotorEndorsementURI.GetAllEndorsements, motorEndoRequest);

                if (listEndoResponse.StatusCode == 200 && listEndoResponse.Result.IsTransactionDone)
                {
                    gvMotorEndorsement.DataSource = listEndoResponse.Result.MotorEndorsements;
                    gvMotorEndorsement.DataBind();

                    if (listEndoResponse.Result.MotorEndorsements.Count > 0)
                    {
                        _MotorEndorsementID = listEndoResponse.Result.MotorEndorsements[listEndoResponse.Result.MotorEndorsements.Count - 1].MotorEndorsementID;
                    }
                    else
                    {
                        _MotorEndorsementID = 0;
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
                    if (ddlMotorPolicies.SelectedIndex > 0)
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

            if (string.IsNullOrEmpty(txtNewExcess.Text))
            {
                master.ShowErrorPopup("Please enter new excess amount !!", "Change Excess");
                return;
            }
            var motorEndorementQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementQuote();
            motorEndorementQuote.Agency = userInfo.Agency;
            motorEndorementQuote.AgentCode = userInfo.AgentCode;
            motorEndorementQuote.MainClass = MainClass;
            motorEndorementQuote.SubClass = SubClass;
            motorEndorementQuote.EffectiveFromDate = txtEffectiveFromDate.Text.CovertToCustomDateTime();
            motorEndorementQuote.EffectiveToDate = txtEffectiveToDate.Text.CovertToCustomDateTime();
            motorEndorementQuote.PaidPremium = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value);
            motorEndorementQuote.EndorsementType = "ChangeExcess";
            motorEndorementQuote.CancelationDate = txtEffectiveToDate.Text.CovertToCustomDateTime();
            motorEndorementQuote.NewInsuredCode = string.Empty;

            //Calculate the motor endorsement premium.
            var motorEndoQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementQuoteResult>,
                                              BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementQuote>
                                              (BKIC.SellingPoint.DTO.Constants.MotorEndorsementURI.GetMotorEndorsementQuote, motorEndorementQuote);

            if (motorEndoQuoteResult.StatusCode == 200 && motorEndoQuoteResult.Result.IsTransactionDone)
            {
                var endoPremium = motorEndoQuoteResult.Result.RefundPremium;
                calculatedPremium.Value = endoPremium.ToString();


                var product = master.GetProduct(MainClass, SubClass);
                bool includeCommission = false;
                if (product != null)
                {
                    var mEndorsement = product.MotorEndorsementMaster.Find(c => c.EndorsementType == "ChangeExcess");
                    if (mEndorsement != null)
                    {
                        includeCommission = mEndorsement.HasCommission;
                    }
                }
                var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest();
                commisionRequest.AgentCode = userInfo.AgentCode;
                commisionRequest.Agency = userInfo.Agency;
                commisionRequest.SubClass = subClass.Value;
                commisionRequest.PremiumAmount = includeCommission ? endoPremium : decimal.Zero;
                // commisionRequest.PremiumAmount = decimal.Zero;

                //Get commision for the endorsement premium.
                var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>
                                       (BKIC.SellingPoint.DTO.Constants.CommissionURI.CalculateCommission, commisionRequest);

                if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone
                    && commissionresult.Result.CommissionAmount >= 0)
                {
                    //commission.Text = Convert.ToString(commissionresult.Result.CommissionAmount);
                    calculatedCommision.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                    if (showPremium)
                        ShowPremium(userInfo, endoPremium, commissionresult.Result.CommissionAmount);
                }
            }
        }
   

        public void ShowPremium(OAuthTokenResponse userInfo, decimal Premium, decimal Commission)
        {
            amtDisplay.Visible = true;
            if (userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.SuperAdmin
                || userInfo.Roles == BKIC.SellingPoint.DL.Constants.Roles.BranchAdmin)
            {
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

        protected void gvMotorEndorsement_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvMotorEndorsement.Rows)
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

                    HtmlAnchor lnkCertificate = row.FindControl("downloadcertificate") as HtmlAnchor;
                    lnkCertificate.Visible = true;
                }
                else
                {
                   
                    var btnAuthorize = row.FindControl("lnkbtnAuthorize") as LinkButton;
                    btnAuthorize.Visible = true;

                    var btnDelete = row.FindControl("lnkbtnDelete") as LinkButton;
                    btnDelete.Visible = true;

                    HtmlAnchor lnkSchedule = row.FindControl("downloadschedule") as HtmlAnchor;
                    lnkSchedule.Visible = false;

                    HtmlAnchor lnkCertificate = row.FindControl("downloadcertificate") as HtmlAnchor;
                    lnkCertificate.Visible = false;
                }
               
            }
        }

        public string GetMessageText(bool isHIR, string docNo)
        {
            btnYes.Visible = false;
            btnOK.Text = "OK";
            if (isHIR)
            {
                return "Your motor endorsement saved and moved into HIR :" + docNo;
            }
            else
            {
                return "Your motor endorsement has been saved sucessfully :" + docNo;
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
            }
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
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var req = new MotorEndorsementPreCheckRequest
            {
                DocNo = ddlMotorPolicies.SelectedIndex > 0 ? ddlMotorPolicies.SelectedItem.Text : string.Empty
            };

            var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementPreCheckResponse>,
                         BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementPreCheckRequest>
                         (BKIC.SellingPoint.DTO.Constants.MotorEndorsementURI.EndorsementPreCheck, req);

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
                    master.ShowErrorPopup("Your motor policy already have saved endorsement !", "Motor Endorsement");
                    return;
                }
                if (string.IsNullOrEmpty(txtNewExcess.Text))
                {
                    master.ShowErrorPopup("Please enter new excess amount !!", "Change Excess");
                    return;
                }

                var postMotorEndorsement = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsement
                {
                    Agency = userInfo.Agency,
                    AgencyCode = userInfo.AgentCode,
                    AgentBranch = userInfo.AgentBranch,
                    IsSaved = isSave,
                    IsActivePolicy = !isSave,
                    PremiumAmount = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value),
                    CreatedBy = Convert.ToInt32(userInfo.ID)
                };


                var policyRenewalCount = ddlMotorPolicies.SelectedItem.Value.Substring(0, ddlMotorPolicies.SelectedValue.IndexOf("-", 0));

                //Get saved policy details by document(policy) number.
                var url = BKIC.SellingPoint.DTO.Constants.MotorURI.GetSavedQuoteDocumentNo
                          .Replace("{documentNo}", ddlMotorPolicies.SelectedItem.Text.Trim())
                          .Replace("{agentCode}", userInfo.AgentCode)
                          .Replace("{isendorsement}", "true")
                          .Replace("{endorsementid}", "0")
                          .Replace("{renewalCount}", policyRenewalCount);

                var motorDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (motorDetails.StatusCode == 200 && motorDetails.Result.IsTransactionDone)
                {
                    SetEndorsementType(postMotorEndorsement, motorDetails.Result.MotorPolicyDetails);
                }

                var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementResult>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsement>
                             (BKIC.SellingPoint.DTO.Constants.MotorEndorsementURI.PostMotorEndorsement, postMotorEndorsement);

                if (response.Result != null && response.StatusCode == 200 && response.Result.IsTransactionDone)
                {
                    _MotorEndorsementID = response.Result.MotorEndorsementID;
                    ListEndorsements(service, userInfo);
                    btnSubmit.Enabled = false;
                    master.ShowErrorPopup("Your motor endorsement has been saved sucessfully :" + response.Result.EndorsementNo, "Motor Endorsement");
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

        public void SetEndorsementType(BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsement mtorEndorsement, BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy mtorPolicyDetails)
        {
            
            mtorEndorsement.VehicleValue = mtorPolicyDetails.VehicleValue;
            mtorEndorsement.Mainclass = mtorPolicyDetails.Mainclass;
            mtorEndorsement.Subclass = mtorPolicyDetails.Subclass;
            mtorEndorsement.MotorID = mtorPolicyDetails.MotorID;
            mtorEndorsement.PolicyCommencementDate = mtorPolicyDetails.PolicyCommencementDate;
            mtorEndorsement.ExpiryDate = mtorPolicyDetails.ExpiryDate;
            mtorEndorsement.Remarks = "";
            mtorEndorsement.AccountNumber = "";
            mtorEndorsement.EndorsementType = "ChangeExcess";
            mtorEndorsement.PaymentType = "";
            mtorEndorsement.InsuredCode = mtorPolicyDetails.InsuredCode;
            mtorEndorsement.InsuredName = mtorPolicyDetails.InsuredName;
            mtorEndorsement.RegistrationNo = mtorPolicyDetails.RegistrationNumber;
            mtorEndorsement.ChassisNo = mtorPolicyDetails.ChassisNo;
            mtorEndorsement.NewExcess = Convert.ToDecimal(txtNewExcess.Text);
            mtorEndorsement.CPR = mtorPolicyDetails.CPR;
            mtorEndorsement.FinancierCompanyCode = mtorPolicyDetails.FinancierCompanyCode;
            mtorEndorsement.RenewalCount = mtorPolicyDetails.RenewalCount;

            //InternalEndorsement           
            mtorEndorsement.VehicleMake = mtorPolicyDetails.VehicleMake;
            mtorEndorsement.VehicleModel = mtorPolicyDetails.VehicleModel;
            mtorEndorsement.EngineCC = mtorPolicyDetails.EngineCC;
            mtorEndorsement.VehicleYear = mtorPolicyDetails.YearOfMake;
            mtorEndorsement.VehicleBodyType = mtorPolicyDetails.vehicleBodyType;
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

        protected void gvMotorEndorsement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();            

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlAnchor lnkSchedule = e.Row.FindControl("downloadschedule") as HtmlAnchor;
                HtmlAnchor lnkCertificate = e.Row.FindControl("downloadcertificate") as HtmlAnchor;

                var btnAuthorize = e.Row.FindControl("lnkbtnAuthorize") as LinkButton;
                var endorsementID = e.Row.FindControl("lblMotorEndorsementID") as Label;
                var DocumentNo = e.Row.FindControl("lblDocumentNo") as Label;
                var renewalCount = e.Row.FindControl("lblRenewalCount") as Label;

                long id = 0;
                if (endorsementID != null)
                {
                    id = Convert.ToInt64(endorsementID.Text);
                }
                lnkSchedule.HRef = ClientUtility.WebApiUri + BKIC.SellingPoint.DTO.Constants.ScheduleURI.downloadschedule
                                   .Replace("{insuranceType}", Constants.Motor)
                                   .Replace("{agentCode}", userInfo.AgentCode)
                                   .Replace("{documentNo}", DocumentNo.Text)
                                   .Replace("{isEndorsement}", "true")
                                   .Replace("{endorsementID}", id.ToString())
                                   .Replace("{renewalCount}", renewalCount.Text.Trim());

                lnkCertificate.HRef = ClientUtility.WebApiUri + BKIC.SellingPoint.DTO.Constants.MotorURI.FetchInsuranceCertificate
                                   .Replace("{documentNo}", DocumentNo.Text)
                                   .Replace("{type}", "endorsement")
                                   .Replace("{agentCode}", userInfo.AgentCode)
                                   .Replace("{isEndorsement}", "true")
                                   .Replace("{endorsementID}", id.ToString())
                                   .Replace("{renewalCount}", renewalCount.Text.Trim());

                bool IsActive = Convert.ToBoolean((e.Row.FindControl("lblIsActive") as Label).Text.Trim());

                if (IsActive)
                {
                    lnkSchedule.Visible = true;
                    lnkCertificate.Visible = true;
                }
                else
                {
                    lnkSchedule.Visible = false;
                    lnkCertificate.Visible = false;
                }
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
            btnSubmit.Enabled = false;
            gvMotorEndorsement.DataSource = null;
            gvMotorEndorsement.DataBind();
            includeDisc.Visible = false;
            excludeDisc.Visible = false;
            txtNewExcess.Text = string.Empty;
            txtOldExcess.Text = string.Empty;
        }
    }
}