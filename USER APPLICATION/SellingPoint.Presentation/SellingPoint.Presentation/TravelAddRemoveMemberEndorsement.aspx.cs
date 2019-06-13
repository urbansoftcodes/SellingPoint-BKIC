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
    public partial class TravelAddRemoveMemberEndorsement : System.Web.UI.Page
    {
        public static DataTable Genderdt;
        public static DataTable Nationalitydt;
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelPolicy> policyList;
        public static DataTable Relationdt;
        private General master;

        public TravelAddRemoveMemberEndorsement()
        {
            master = Master as General;
        }

        public static long _TravelEndorsementID { get; set; }
        public static bool AjdustedPremium { get; set; }
        public static List<BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails> InsuredNames { get; set; }
        public static string MainClass { get; set; }
        public static string SubClass { get; set; }
        public static decimal OriginalPremium { get; set; }
        public static string PackageName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;

            if (!Page.IsPostBack)
            {
                depentdetails.Visible = false;
                SetInitialRow();
                BindAgencyClientCodeDropdown();
                PackageName = string.Empty;
            }
        }

        protected void btnAuthorize_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
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

        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewRowToGrid();
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
                //travelreq.CPR = ddlCPR.SelectedIndex > 0 ? ddlCPR.SelectedItem.Text.Trim() : string.Empty;
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

        public void IsCancelledFamilyPolicy(bool isCancelled)
        {
            if (isCancelled)
            {
                depentdetails.Visible = false;
                btnSubmit.Visible = false;
                master.ShowErrorPopup("This policy is already cancelled", "Policy Cancelled");
            }
            else
            {
                depentdetails.Visible = true;
                btnSubmit.Visible = true;
            }
        }
        public void IsCancelledIndividualPolicy(bool isCancelled)
        {
            depentdetails.Visible = false;
            btnSubmit.Visible = false;
            if (isCancelled)
            {
                master.ShowErrorPopup("This policy is already cancelled", "Policy Cancelled");
            }
        }

        protected void Changed_EndorsementType(object sender, EventArgs e)
        {
            if (ddlTravelPolicies.SelectedIndex > 0)
            {
                SetTravelEndorsement();
                depentdetails.Visible = true;
            }
            else
            {
                depentdetails.Visible = false;
            }
        }

        public void SetTravelEndorsement()
        {
            //if (PackageName == "family" && ddlEndorsementType.SelectedItem.Value == "ChangeMemberDetails")
            //{
            //    CorrectPremiumDiv.Visible = false;
            //    txtEffectiveToDate.Enabled = false;
            //    PaymentSection.Visible = false;
            //    btnCalculate.Visible = false;
            //    EnableGrid(true);
            //}
            //else if (PackageName == "family" && ddlEndorsementType.SelectedItem.Value == "AddRemoveFamilyMember")
            //{
            //    CorrectPremiumDiv.Visible = false;
            //    txtEffectiveToDate.Enabled = false;
            //    PaymentSection.Visible = false;
            //    btnCalculate.Visible = false;
            //    EnableGrid(false);
            //}
            //else if (ddlEndorsementType.SelectedItem.Value == "CorrectPremium")
            //{
            //    CorrectPremiumDiv.Visible = true;
            //    changeAddMemberDeatilsDiv.Visible = false;
            //    txtEffectiveToDate.Enabled = false;
            //    PaymentSection.Visible = true;
            //    btnCalculate.Visible = true;
            //    txtOldPremium.Text = OriginalPremium.ToString();
            //}
            //else if (ddlEndorsementType.SelectedItem.Value == "CancelPolicy")
            //{
            //    CorrectPremiumDiv.Visible = false;
            //    changeAddMemberDeatilsDiv.Visible = false;
            //    PaymentSection.Visible = true;
            //    btnCalculate.Visible = true;
            //    txtEffectiveToDate.Enabled = true;
            //}
        }

        protected void Changed_TravelPolicy(object sender, EventArgs e)
        {
            try
            {
                if (ddlTravelPolicies.SelectedIndex > 0)
                {
                    // TravelInsurancePolicy policy = GetPolicyInfo();
                    var policyRenewalCount = ddlTravelPolicies.SelectedItem.Value.Substring(0, ddlTravelPolicies.SelectedValue.IndexOf("-", 0));
                    UpdatePolicyDetails(ddlTravelPolicies.SelectedItem.Text.Trim(), policyRenewalCount, false);
                }
                else
                {
                    depentdetails.Visible = false;
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
                //UpdatePolicyDetails(txtTravelEndorsementSearch.Text.Trim(), policyRenewalCount, true);

                //var renewalCount = GetPolicyRenewalCount();
                //UpdatePolicyDetails(txtTravelEndorsementSearch.Text.Trim(), renewalCount.ToString(), true);

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


        public int GetPolicyRenewalCount()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            int renewalCount = 0;

            var travelreq = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelRequest
            {
                AgentCode = userInfo.AgentCode,
                AgentBranch = userInfo.AgentBranch,
                includeHIR = false,
                IsRenewal = false
            };

            //Get PolicyNo by Agency
            var travelPolicies = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelPolicyResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelRequest>
                                 (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetAgencyPolicy, travelreq);

            if (travelPolicies.StatusCode == 200 && travelPolicies.Result.IsTransactionDone && travelPolicies.Result.AgencyTravelPolicies.Count > 0)
            {
                renewalCount = travelPolicies.Result.AgencyTravelPolicies[0].RenewalCount;               
            }
            return renewalCount;
        }

        public void UpdatePolicyDetails(string DocumentNo, string RenewalCount, bool SearchByDocument)
        {
            ClearControls();            

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyTravelRequest
            {
                AgentBranch = userInfo.AgentBranch,
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency
            };


            //Get saved policy details by document(policy) number.
            var url = BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetSavedQuoteDocumentNo.
                          Replace("{documentNo}", DocumentNo)
                          .Replace("{agentCode}", request.AgentCode)
                          .Replace("{isendorsement}", "true")
                          .Replace("{endorsementid}", "0"); 

            var travelDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelSavedQuotationResponse>>(url);

            TravelInsurancePolicy res = null;
            //Update policy details on current page for dispaly the details.
            if (travelDetails.StatusCode == 200 && travelDetails.Result.IsTransactionDone)
            {
                res = travelDetails.Result.TravelInsurancePolicyDetails;
                OriginalPremium = travelDetails.Result.TravelInsurancePolicyDetails.PremiumAfterDiscount;
                txtEffectiveFromDate.Text = travelDetails.Result.TravelInsurancePolicyDetails.InsuranceStartDate.ConvertToLocalFormat();
                txtEffectiveToDate.Text = travelDetails.Result.TravelInsurancePolicyDetails.ExpiryDate.ConvertToLocalFormat();
                txtOldClientCode.Text = travelDetails.Result.TravelInsurancePolicyDetails.InsuredCode;
                txtOldInsuredName.Text = travelDetails.Result.TravelInsurancePolicyDetails.InsuredName;

                PackageName = res.PackageName.ToLower();
                if (travelDetails.Result.TravelMembers != null &&
                   travelDetails.Result.TravelMembers.Count > 0 &&
                   res.PackageName.ToLower() == "family")
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
                        //if (travelDetails.Result.TravelMembers[i].ItemSerialNo != 1)
                        //{

                            drCurrentRow = dtCurrentTable.NewRow();
                            dtCurrentTable.Rows.Add(drCurrentRow);
                            dtCurrentTable.Rows[memberIndex]["Insured Name"] = travelDetails.Result.TravelMembers[i].ItemName;
                            dtCurrentTable.Rows[memberIndex]["Relationship"] = travelDetails.Result.TravelMembers[i].Category;
                            dtCurrentTable.Rows[memberIndex]["CPR"] = travelDetails.Result.TravelMembers[i].CPR;
                            dtCurrentTable.Rows[memberIndex]["Date Of Birth"] = travelDetails.Result.TravelMembers[i].DateOfBirth.ConvertToLocalFormat();
                            dtCurrentTable.Rows[memberIndex]["Passport No"] = travelDetails.Result.TravelMembers[i].Passport;
                            dtCurrentTable.Rows[memberIndex]["Nationality"] = travelDetails.Result.TravelMembers[i].Make;
                            dtCurrentTable.Rows[memberIndex]["Occupation"] = travelDetails.Result.TravelMembers[i].OccupationCode;
                            dtCurrentTable.Rows[memberIndex]["ItemSerial No"] = travelDetails.Result.TravelMembers[i].ItemSerialNo;
                            memberIndex++;
                        //}
                    }
                    ViewState["CurrentTable"] = dtCurrentTable;
                    Gridview1.DataSource = dtCurrentTable;
                    Gridview1.DataBind();                    
                    SetPreviousData();
                    IsCancelledFamilyPolicy(res.IsCancelled);
                }
                else
                {
                    ViewState["CurrentTable"] = null;
                    Gridview1.DataSource = null;
                    Gridview1.DataBind();
                    IsCancelledIndividualPolicy(res.IsCancelled);
                }
                if (SearchByDocument)
                {
                    ddlTravelPolicies.Items.Clear();
                    txtCPRSearch.Text = res.CPR;
                    ddlTravelPolicies.Items.Insert(0, new ListItem("--Please Select--", "none"));
                    ddlTravelPolicies.Items.Insert(1, new ListItem(txtTravelEndorsementSearch.Text.Trim(), RenewalCount + "-" + txtTravelEndorsementSearch.Text.Trim()));
                    ddlTravelPolicies.DataBind();
                    ddlTravelPolicies.SelectedIndex = 1;
                }
                ListEndorsements(service, userInfo);
                SetDependantDOB();
            }
            else
            {
                master.ShowErrorPopup(travelDetails.Result.TransactionErrorMessage, "Request failed!");
            }
        }

        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlPaymentMethods.SelectedIndex == 1)
            //{
            //    //txtAccountNumber.Text = "";
            //    //txtAccountNumber.Enabled = false;
            //}
            //else
            //{
            //    //txtAccountNumber.Enabled = true;
            //}
            
        }

        protected void ddlRelation_Changed(object sender, EventArgs e)
        {
            SetDependantDOB();
            DropDownList ddlRelation = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlRelation.NamingContainer;
            int statusID = Convert.ToInt32(ddlRelation.SelectedIndex);
            //Give an ID to the Hyperlink Control and find it here

            TextBox txtBox = (TextBox)row.FindControl("txtDOB");
            txtBox.Enabled = true;
            txtBox.ToolTip = "";
            string dob = txtBox.Text;
            //txtBox.Text = DateTime.Now.CovertToLocalFormat();

            var id = txtBox.ClientID;
            txtBox.Text = "";
            if (ddlRelation != null && txtBox != null && (ddlRelation.SelectedIndex == 1 || ddlRelation.SelectedIndex == 2))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "Set21Years(" + id + ");", true);
            }
            else if (ddlRelation != null && txtBox != null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "Set100Years(" + id + ");", true);
            }           
            txtBox.Text = dob;

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
            }
        }

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
            //dlist.DefaultView.Sort = e.SortExpression + " " + SortDir(e.SortExpression);
            //gvMotorInsurance.DataSource = dlist;
            //gvMotorInsurance.DataBind();
        }

        protected void lnkbtnAuthorize_Click(object sender, EventArgs e)
        {
            try
            {
                EndorsementOperation(sender, "authorize");
            }
            catch (Exception ex)
            {
            }
            finally
            {
                master.ShowLoading = false;
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
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void lnkbtnMemberDelete_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void Gridview1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.RowIndex);
                ViewState["CurrentTable"] = GetTraveMembersDataTable();
                DataTable dt = ViewState["CurrentTable"] as DataTable;
                dt.Rows[index].Delete();
                ViewState["CurrentTable"] = dt;
                Gridview1.DataSource = dt;
                Gridview1.DataBind();
                SetPreviousData();
                SetDependantDOB();
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



        protected void validate_Premium(object sender, EventArgs e)
        {
            //var Premium = Convert.ToDecimal(calculatedPremium.Value);
            //var Commision = Convert.ToDecimal(calculatedCommision.Value);
            //decimal Discount = string.IsNullOrEmpty(txtDiscount.Text) ? decimal.Zero : Convert.ToDecimal(txtDiscount.Text);
            //var reduceablePremium = Premium - Commision;
            //var premiumDiff = Premium - Discount;

            //if (premiumDiff < reduceablePremium)
            //{
            //    premiumAmount.Text = Convert.ToString(reduceablePremium);
            //    txtDiscount.Text = Convert.ToString(calculatedCommision.Value);
            //    commission.Text = Convert.ToString(0);
            //}
            //else if (Discount > Premium)
            //{
            //    premiumAmount.Text = Convert.ToString(reduceablePremium);
            //    txtDiscount.Text = Convert.ToString(calculatedCommision.Value);
            //    commission.Text = Convert.ToString(0);
            //}
            //else
            //{
            //    premiumAmount.Text = Convert.ToString(premiumDiff);
            //    commission.Text = Convert.ToString(Commision - Discount);
            //    btnSubmit.Enabled = true;
            //}
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
                        dtCurrentTable.Rows[i - 1]["Nationality"] = ddlNation.SelectedItem.Value;
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
            SetDependantDOB();
        }

        private void BindAgencyClientCodeDropdown()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}",
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.Travelnsurance));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                Nationalitydt = dropdownds.Tables["Nationality"];
                Relationdt = dropdownds.Tables["FamilyRelationShip"];
                DataTable paymentTypes = dropdownds.Tables["PaymentType"];
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

            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest();
            req.AgentBranch = userInfo.AgentBranch;
            req.AgentCode = userInfo.AgentCode;

            var insuredResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest>
                               (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyInsured, req);

            if (insuredResult.StatusCode == 200 && insuredResult.Result.AgencyInsured.Count > 0)
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
            dt.Columns.Add(new DataColumn("ItemSerial No", typeof(int)));

            dr = dt.NewRow();

            dr["Insured Name"] = string.Empty;
            dr["Relationship"] = string.Empty;
            dr["CPR"] = string.Empty;
            dr["Date Of Birth"] = string.Empty;
            dr["Passport No"] = string.Empty;
            dr["Nationality"] = string.Empty;
            dr["Occupation"] = string.Empty;
            dr["ItemSerial No"] = 0;

            dt.Rows.Add(dr);

            //dr = dt.NewRow();

            //Store the DataTable in ViewState
            ViewState["CurrentTable"] = dt;

            Gridview1.DataSource = dt;
            Gridview1.DataBind();
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
                        LinkButton lnkBtn = (LinkButton)Gridview1.Rows[rowIndex].Cells[6].FindControl("lnkbtnDelete");
                        // DropDownList ddlOccupation = (DropDownList)Gridview1.Rows[rowIndex].Cells[6].FindControl("ddlTravelOccupation");                       
                        if (!dt.Rows[i].IsNull("ItemSerial No") && Convert.ToInt32(dt.Rows[i]["ItemSerial No"].ToString()) == 1)
                        {
                            ddlRelation.Items.Add(new ListItem("self", "SELF"));
                            ddlRelation.SelectedIndex = 4;
                            ddlRelation.Enabled = false;
                            lnkBtn.Visible = false;
                        }
                        else
                        {
                            ddlRelation.SelectedIndex = ddlRelation.Items.IndexOf(ddlRelation.Items.FindByText(dt.Rows[i]["Relationship"].ToString()));
                        }
                        txName.Text = dt.Rows[i]["Insured Name"].ToString();
                        txDOB.Text = dt.Rows[i]["Date Of Birth"].ToString();
                        txPassport.Text = dt.Rows[i]["Passport No"].ToString();
                        ddlNation.SelectedIndex = ddlNation.Items.IndexOf(ddlNation.Items.FindByValue(dt.Rows[i]["Nationality"].ToString()));
                        txtOccupation.Text = dt.Rows[i]["Occupation"].ToString();                      

                        rowIndex++;
                    }
                }
            }
        }

        private void EnableGrid(bool isEnable)
        {
            for (int row = 1; row <= Gridview1.Rows.Count; row++)
            {
                //DataRow TempRow = TempTable.NewRow();
                var obj = new TravelMembers();
                obj.ItemSerialNo = row;
                obj.ForeignSumInsured = 50000;
                obj.SumInsured = 18900;

                for (int col = 0; col < Gridview1.Columns.Count; col++)
                {
                    if (Gridview1.Columns[col].Visible)
                    {
                        if (String.IsNullOrEmpty(Gridview1.Rows[row - 1].Cells[col].Text))
                        {
                            if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("Label"))
                            {
                                ((Label)Gridview1.Rows[row - 1].Cells[col].Controls[1]).Enabled = isEnable;
                            }
                            else if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("LinkButton"))
                            {
                                var cc = ((LinkButton)Gridview1.Rows[row - 1].Cells[col].Controls[1]);
                                cc.Visible = !isEnable;
                            }
                            else if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("TextBox"))
                            {
                                ((TextBox)Gridview1.Rows[row - 1].Cells[col].Controls[1]).Enabled = isEnable;
                            }
                            else if (Gridview1.Rows[row - 1].Cells[col].Controls[1].GetType().ToString().Contains("DropDownList"))
                            {
                                ((DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1]).Enabled = isEnable;
                            }
                        }
                    }
                }
            }
            if (!isEnable)
            {
                //ButtonAdd.Visible = false;
            }
            else
            {
                // ButtonAdd.Visible = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSave"></param>
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
                if (!ValidateDependant(userInfo))
                {
                    return;
                }

                var postTravelEndorsement = new BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsement
                {
                    Agency = userInfo.Agency,
                    AgencyCode = userInfo.AgentCode,
                    AgentBranch = userInfo.AgentBranch,
                    IsSaved = isSave,
                    IsActivePolicy = !isSave
                };                
                postTravelEndorsement.PremiumAmount = string.IsNullOrEmpty(paidPremium.Value) ? decimal.Zero : Convert.ToDecimal(paidPremium.Value);
                postTravelEndorsement.CreatedBy = Convert.ToInt32(userInfo.ID);
                postTravelEndorsement.DocumentNo = ddlTravelPolicies.SelectedItem.Text.Trim();

                //Get saved policy details by document(policy) number.
                var url = BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetSavedQuoteDocumentNo.
                          Replace("{documentNo}", ddlTravelPolicies.SelectedItem.Text.Trim()).Replace("{agentCode}", userInfo.AgentCode)
                          .Replace("{isendorsement}", "true")
                          .Replace("{endorsementid}", "0"); ;

                var motorDetails = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelSavedQuotationResponse>>(url);

                //Update policy details on current page for dispaly the details.
                if (motorDetails.StatusCode == 200 && motorDetails.Result.IsTransactionDone)
                {
                    var response = motorDetails.Result.TravelInsurancePolicyDetails;
                    SetEndorsementType(postTravelEndorsement, response);
                }               

                var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsementResponse>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsement>
                            (BKIC.SellingPoint.DTO.Constants.TravelEndorsementURI.PostTravelEndorsement, postTravelEndorsement);

                if (result.Result != null && result.StatusCode == 200 && result.Result.IsTransactionDone)
                {
                    _TravelEndorsementID = result.Result.TravelEndorsementID;
                    ListEndorsements(service, userInfo);
                    btnSubmit.Visible = false;
                    master.ShowErrorPopup("Your travel endorsement has been saved sucessfully :" + result.Result.EndorsementNo, "Travel Endorsement");
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

        private void SetEndorsementType(BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelEndorsement postTravelEndorsement, BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsurancePolicy response)
        {

            postTravelEndorsement.Mainclass = response.MainClass;
            postTravelEndorsement.Subclass = response.SubClass;
            postTravelEndorsement.TravelID = response.TravelID;
            postTravelEndorsement.InsuredName = response.InsuredName;
            postTravelEndorsement.InsuredCode = response.InsuredCode;
            postTravelEndorsement.PolicyCommencementDate = response.InsuranceStartDate ?? DateTime.Now;
            postTravelEndorsement.ExpiryDate = response.ExpiryDate ?? DateTime.Now;
            postTravelEndorsement.Remarks = "";
            postTravelEndorsement.AccountNumber = "";
            postTravelEndorsement.EndorsementType = "AddRemoveFamilyMember";
            postTravelEndorsement.PaymentType = "";
            postTravelEndorsement.TravelMembers = GetTravelMembers();
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


        public List<TravelMembers> GetTravelMembers()
        {
            var objs = new List<TravelMembers>();

            for (int row = 1; row <= Gridview1.Rows.Count; row++)
            {
                //DataRow TempRow = TempTable.NewRow();
                var obj = new TravelMembers();
                obj.ItemSerialNo = row;
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
                        //if (colName == "Sex")
                        //{
                        //    DropDownList txtValue = (DropDownList)Gridview1.Rows[row - 1].Cells[col].Controls[1];
                        //    obj.Sex = txtValue.SelectedValue.ToString();
                        //}

                    }
                }
                objs.Add(obj);
            }
            //}

            return objs;
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
            table.Columns.Add("ItemSerial No");

            for (int row = 1; row <= Gridview1.Rows.Count; row++)
            {
                //DataRow TempRow = TempTable.NewRow();
                var obj = new TravelMembers();
                obj.ItemSerialNo = row;
                obj.ForeignSumInsured = 50000;
                obj.SumInsured = 18900;
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

                table.Rows.Add(obj.ItemName, obj.Category, obj.CPR, dob, obj.Passport, obj.Make, obj.OccupationCode, obj.ItemSerialNo);
            }

            return table;
        }

        public void SetDependantDOB()
        {
            foreach (GridViewRow row in Gridview1.Rows)
            {
                DropDownList relation = (DropDownList)row.FindControl("ddlRelation");

                if (relation.SelectedIndex > 0)
                {
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

        public void Reset_Content(object sender, EventArgs e)
        {
            modalBodyText.InnerText = "Your you sure want authorize this endorsement ?";
            btnOK.Text = "No";
            btnYes.Visible = true;
        }

        protected void gvTravelEndorsement_DataBound(object sender, EventArgs e)
        {
            int scheduleRowIndex = 1;
            int index = 1;

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
                    scheduleRowIndex++;

                    var btnAuthorize = row.FindControl("lnkbtnAuthorize") as LinkButton;
                    btnAuthorize.Visible = true;

                    var btnDelete = row.FindControl("lnkbtnDelete") as LinkButton;
                    btnDelete.Visible = true;

                    HtmlAnchor lnkSchedule = row.FindControl("downloadschedule") as HtmlAnchor;
                    lnkSchedule.Visible = false;

                    //if (!IsActive)
                    //{
                    //    btnAuthorize.Visible = true;
                    //    btnDelete.Visible = true;
                    //}

                }
                index++;
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
        public void ClearControls()
        {
            btnSubmit.Visible = false;
            depentdetails.Visible = false;
            txtOldClientCode.Text = string.Empty;
            txtOldInsuredName.Text = string.Empty;
            txtEffectiveFromDate.Text = string.Empty;
            txtEffectiveToDate.Text = string.Empty;
            gvTravelEndorsement.DataSource = null;
            gvTravelEndorsement.DataBind();
            Gridview1.DataSource = null;
            Gridview1.DataBind();
            ViewState["CurrentTable"] = null;

        }

        public bool ValidateDependant(OAuthTokenResponse userInfo)
        {
            int spouseCount = 0;
            bool isValid = true;
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
                                master.ShowErrorPopup("Spouse age is greater than 65 years", "Can't pass an endorsement");
                            }
                            //More than one spouse is not allowed for TISCO.
                            if (spouseCount > 1)
                            {
                                isValid = false;
                                master.ShowErrorPopup("More than one spouse not allowed !", "Can't pass an endorsement");
                            }
                        }
                        else if (relation.SelectedItem.Text == "son" || relation.SelectedItem.Text == "daughter")
                        {
                            //Kid age should not be above 18 years for TISCO.
                            if (memberAge > 18)
                            {
                                isValid = false;
                                master.ShowErrorPopup("Kid age should be less than 18 years !", "Can't pass an endorsement");
                            }
                            //Kid age should be above 3 months for TISCO.
                            var ThreeMonthsDeduct = DateTime.Now.AddMonths(-3);
                            if (ThreeMonthsDeduct < dob.Text.CovertToCustomDateTime())
                            {
                                isValid = false;
                                master.ShowErrorPopup("Kid age should be greater than 3 months", "Can't pass an endorsement");
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
                                master.ShowErrorPopup("Kid age should be less than 21 years !", "Can't pass an endorsement");
                            }
                        }
                    }
                }
            }
            return isValid;
        }
    }
}