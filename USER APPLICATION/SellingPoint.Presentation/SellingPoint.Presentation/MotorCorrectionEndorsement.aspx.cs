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
    public partial class MotorCorrectionEndorsement : System.Web.UI.Page
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

        public MotorCorrectionEndorsement()
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
                    ddlBranch.DataSource = branches;
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
                ddlCPR.DataSource = insuredResult.Result.AgencyInsured;
                ddlCPR.DataTextField = "CPR";
                ddlCPR.DataValueField = "InsuredCode";
                ddlCPR.DataBind();
                ddlCPR.Items.Insert(0, new ListItem("--Please Select--", ""));
                InsuredNames = insuredResult.Result.AgencyInsured;
            }
            txtIndroducedBy.Text = userInfo.UserName;
            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(userInfo.AgentBranch));
            txtIndroducedBy.Text = userInfo.UserName;
        }

        protected void Changed_CPR(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var motorreq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest
                {
                    AgentCode = userInfo.AgentCode,
                    Agency = userInfo.Agency,
                    AgentBranch = userInfo.AgentBranch,
                    CPR = ddlCPR.SelectedIndex > 0 ? ddlCPR.SelectedItem.Text.Trim() : string.Empty,
                    Type = Constants.Motor,
                    isEndorsement = true
                };

                //Get PolicyNo by Agency
                var motorPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                    <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorPolicyResponse>,
                                    BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyMotorRequest>
                                    (BKIC.SellingPoint.DTO.Constants.MotorURI.GetMotorPoliciesByTypeByCPR, motorreq);

                ddlMotorPolicies.Items.Clear();
                if (motorPolicies.StatusCode == 200 && motorPolicies.Result.IsTransactionDone && motorPolicies.Result.AgencyMotorPolicies.Count > 0)
                {
                    policyList = motorPolicies.Result.AgencyMotorPolicies;

                    ddlMotorPolicies.DataSource = motorPolicies.Result.AgencyMotorPolicies;
                    ddlMotorPolicies.DataTextField = "DOCUMENTNO";
                    ddlMotorPolicies.DataValueField = "DOCUMENTNO";
                    ddlMotorPolicies.DataBind();
                    ddlMotorPolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
                }
                Page_CustomValidate();
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

        protected void Changed_MotorPolicy(object sender, EventArgs e)
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

                //List the previous endorsements for the policy.
                ListEndorsements(service, userInfo);

                //Get saved policy details by document(policy) number.
                var url = BKIC.SellingPoint.DTO.Constants.MotorURI.GetSavedQuoteDocumentNo
                          .Replace("{documentNo}", ddlMotorPolicies.SelectedItem.Text.Trim())
                          .Replace("{agentCode}", request.AgentCode)
                          .Replace("{isendorsement}", "true")
                          .Replace("{endorsementid}", "0");

                var motorDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (motorDetails.StatusCode == 200 && motorDetails.Result.IsTransactionDone)
                {
                    var response = motorDetails.Result.MotorPolicyDetails;

                    txtOldClientCode.Text = response.InsuredCode;
                    txtEffectiveFromDate.Text = response.PolicyCommencementDate.CovertToLocalFormat();
                    txtEffectiveToDate.Text = response.ExpiryDate.CovertToLocalFormat();
                    paidPremium.Value = Convert.ToString(response.PremiumAfterDiscount);
                    subClass.Value = response.Subclass;
                    SubClass = response.Subclass;
                    MainClass = response.Mainclass;
                    expireDate.Value = response.ExpiryDate.CovertToLocalFormat();
                    txtInsuredName.Text = response.InsuredName;
                    txtChassesNumber.Text = response.ChassisNo;
                    txtRegistrationNumber.Text = response.RegistrationNumber;
                }
                Page_CustomValidate();
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

                    btnOK.Text = "OK";
                    btnYes.Visible = false;
                    if (type == "delete")
                    {
                        modalBodyText.InnerText = "Your endorsement deleted successfully";
                    }
                    else if (type == "authorize")
                    {
                        modalBodyText.InnerText = "Your endorsement authorized successfully";
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowMessage();", true);
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
                Page_CustomValidate();
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

            bool sumInsuredDeduct = false;
            decimal newSumInsured = 0;

            if (newSumInsured < 0)
            {
                newSumInsured = Math.Abs(newSumInsured);
                sumInsuredDeduct = true;
            }

            var motorEndorementQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementQuote
            {
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode,
                MainClass = MainClass,
                SubClass = SubClass,
                EffectiveFromDate = txtEffectiveFromDate.Text.CovertToCustomDateTime(),
                EffectiveToDate = txtEffectiveToDate.Text.CovertToCustomDateTime(),
                PaidPremium = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value),
                EndorsementType = "ChangeSumInsured",
                CancelationDate = txtEffectiveToDate.Text.CovertToCustomDateTime(),
                NewInsuredCode = string.Empty,
                NewSumInsured = newSumInsured
            };


            //Calculate the motor endorsement premium.
            var motorEndoQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementQuoteResult>,
                                              BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementQuote>
                                              (BKIC.SellingPoint.DTO.Constants.MotorEndorsementURI.GetMotorEndorsementQuote, motorEndorementQuote);

            if (motorEndoQuoteResult.StatusCode == 200 && motorEndoQuoteResult.Result.IsTransactionDone)
            {

                var endorsementPremium = motorEndoQuoteResult.Result.EndorsementPremium;
                calculatedPremium.Value = endorsementPremium.ToString();

                var commisionRequest = new BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest();
                commisionRequest.AgentCode = userInfo.AgentCode;
                commisionRequest.Agency = userInfo.Agency;
                commisionRequest.SubClass = subClass.Value;
                commisionRequest.PremiumAmount = endorsementPremium;

                //Get commision for the endorsement premium.
                var commissionresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.CommissionRequest>("api/insurance/Commission", commisionRequest);

                if (commissionresult.StatusCode == 200 && commissionresult.Result.IsTransactionDone && commissionresult.Result.CommissionAmount >= 0)
                {
                    //commission.Text = Convert.ToString(commissionresult.Result.CommissionAmount);
                    calculatedCommision.Value = Convert.ToString(commissionresult.Result.CommissionAmount);
                    if (sumInsuredDeduct)
                    {
                       // ShowPremium(userInfo, -endorsementPremium, -commissionresult.Result.CommissionAmount);
                    }
                    else
                    {
                       // ShowPremium(userInfo, endorsementPremium, commissionresult.Result.CommissionAmount);
                    }
                }

            }
        }
        public void Page_CustomValidate()
        {
            if (endorsementSubmitted.Value == "true")
            {
                Validate("MotorEndorsementValidation");
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
                Page_CustomValidate();
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
                    modalBodyText.InnerText = "Your motor policy already have saved endorsement";
                    btnYes.Visible = false;
                    btnOK.Text = "OK";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "ShowPopup();", true);
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

                //Get saved policy details by document(policy) number.
                var url = BKIC.SellingPoint.DTO.Constants.MotorURI.GetSavedQuoteDocumentNo
                          .Replace("{documentNo}", ddlMotorPolicies.SelectedItem.Text.Trim())
                          .Replace("{agentCode}", userInfo.AgentCode)
                          .Replace("{isendorsement}", "true")
                          .Replace("{endorsementid}", "0");

                var motorDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (motorDetails.StatusCode == 200 && motorDetails.Result.IsTransactionDone)
                {
                    SetEndorsementType(postMotorEndorsement, motorDetails.Result.MotorPolicyDetails);
                }
                //if (Convert.ToDecimal(premiumAmount.Text) < Convert.ToDecimal(calculatedPremium.Value))
                //{
                //    postMotorEndorsement.UserChangedPremium = true;
                //    postMotorEndorsement.PremiumAfterDiscount = Convert.ToDecimal(premiumAmount.Text);
                //    var diff = Convert.ToDecimal(calculatedPremium.Value) - postMotorEndorsement.PremiumAfterDiscount;
                //    postMotorEndorsement.CommissionAfterDiscount = Convert.ToDecimal(calculatedCommision.Value) - diff;
                //}

                var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsementResult>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsement>
                             (BKIC.SellingPoint.DTO.Constants.MotorEndorsementURI.PostMotorEndorsement, postMotorEndorsement);

                if (response.Result != null && response.StatusCode == 200 && response.Result.IsTransactionDone)
                {
                    _MotorEndorsementID = response.Result.MotorEndorsementID;
                    ListEndorsements(service, userInfo);
                    modalBodyText.InnerText = GetMessageText(false, response.Result.EndorsementNo);
                    //SetScheduleCertificateHref(userInfo);
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

        public void SetEndorsementType(BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorEndorsement mtorEndorsement, BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy mtorPolicyDetails)
        {
            //mtorEndorsement.OldInsuredCode = mtorPolicyDetails.InsuredCode;
            //mtorEndorsement.OldInsuredName = mtorPolicyDetails.InsuredName;
            //mtorEndorsement.OldChassisNo = mtorPolicyDetails.ChassisNo;
            //mtorEndorsement.OldRegistrationNo = mtorPolicyDetails.RegistrationNumber;
            mtorEndorsement.VehicleValue = mtorPolicyDetails.VehicleValue;
            mtorEndorsement.Mainclass = mtorPolicyDetails.Mainclass;
            mtorEndorsement.Subclass = mtorPolicyDetails.Subclass;
            mtorEndorsement.MotorID = mtorPolicyDetails.MotorID;
            mtorEndorsement.PolicyCommencementDate = mtorPolicyDetails.PolicyCommencementDate;
            mtorEndorsement.ExpiryDate = mtorPolicyDetails.ExpiryDate;
            mtorEndorsement.Remarks = "";
            mtorEndorsement.AccountNumber = "";
            mtorEndorsement.EndorsementType = "Correction";
            mtorEndorsement.PaymentType = "";
            mtorEndorsement.InsuredCode = mtorPolicyDetails.InsuredCode;
            mtorEndorsement.InsuredName = mtorPolicyDetails.InsuredName;
            mtorEndorsement.RegistrationNo = !string.IsNullOrEmpty(txtNewRegistrationNumber.Text) ? txtNewRegistrationNumber.Text.Trim() : mtorPolicyDetails.RegistrationNumber;
            mtorEndorsement.ChassisNo = !string.IsNullOrEmpty(txtNewChassesNumber.Text) ? txtNewChassesNumber.Text.Trim() : mtorPolicyDetails.ChassisNo;
            mtorEndorsement.CPR = mtorPolicyDetails.CPR;
            mtorEndorsement.FinancierCompanyCode = mtorPolicyDetails.FinancierCompanyCode;
            mtorEndorsement.NewExcess = mtorPolicyDetails.ExcessAmount;

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
                Page_CustomValidate();
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
    }
}