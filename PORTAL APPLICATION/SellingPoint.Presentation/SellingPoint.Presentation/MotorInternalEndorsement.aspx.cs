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

namespace SellingPoint.Presentation.assets.customjs.MasterPage
{
    public partial class MotorInternalEndorsement : System.Web.UI.Page
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
        public static DataTable GeneralMake { get; set; }     

        public MotorInternalEndorsement()
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
                BindDropdown(userInfo, service);
                _MotorEndorsementID = 0;
                btnSubmit.Enabled = false;               
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
                            ddlModel.DataSource = ExtensionMethod.GetDistictModel(SubClass, vehicleModeldt);
                            ddlModel.DataValueField = "Model";
                            ddlModel.DataTextField = "Model";
                            ddlModel.DataBind();
                            ddlModel.Items.Insert(0, new ListItem("--Please Select--", ""));
                        }
                        ddlBodyType.SelectedItem.Text = string.Empty;
                    }
                    else
                    {
                        ddlModel.SelectedIndex = -1;
                    }
                }                
               // DisableControls();
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
                GetBodyType(); 
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

        public void GetBodyType()
        {
            try
            {
                master.IsSessionAvailable();                
                var service = CommonMethods.GetLogedInService();

                string VehicleMake = ddlMake.SelectedItem.Value;
                string VehicleModel = ddlModel.SelectedItem.Value;

                var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorBodyRequest
                {
                    VehicleMake = VehicleMake,
                    VehicleModel = VehicleModel
                };

                var vehicleModel = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.VehicleBodyResponse>,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorBodyRequest>(
                                   BKIC.SellingPoint.DTO.Constants.DropDownURI.GetVehicleBody, request);

                if (ddlMake.SelectedIndex > 0)
                {
                    DataTable vehicleBodydt = JsonConvert.DeserializeObject<DataTable>(vehicleModel.Result.VehicleBodydt);
                    DataTable vehicleEnginCCdt = JsonConvert.DeserializeObject<DataTable>(vehicleModel.Result.VehicleEngineCCdt);

                    if (vehicleBodydt != null && vehicleBodydt.Rows.Count == 1)
                    {
                        ddlBodyType.Items.Clear();
                        ddlBodyType.Items.Add(new ListItem("select", "-1"));
                        ddlBodyType.DataValueField = "BodyType";
                        ddlBodyType.DataTextField = "BodyType";
                        ddlBodyType.DataSource = vehicleBodydt;
                        ddlBodyType.DataBind();
                        ddlBodyType.SelectedIndex = 0;
                    }
                    else if (vehicleBodydt != null && vehicleBodydt.Rows.Count > 1)
                    {
                        ddlBodyType.Items.Clear();
                        ddlBodyType.Items.Add(new ListItem("select", "-1"));
                        ddlBodyType.DataValueField = "BodyType";
                        ddlBodyType.DataTextField = "BodyType";
                        ddlBodyType.DataSource = vehicleBodydt;
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

            //var policyRenewalCount = ddlMotorPolicies.SelectedItem.Value.Substring(0, ddlMotorPolicies.SelectedValue.IndexOf("-", 0));

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
                ddlMake.SelectedIndex = ddlMake.Items.IndexOf(ddlMake.Items.FindByText(response.VehicleMake));
                ddlManufactureYear.SelectedIndex = ddlManufactureYear.Items.IndexOf(ddlManufactureYear.Items.FindByText(response.YearOfMake.ToString()));
                SetVehicleMake(service, response);
                GetBodyType();
                ddlBodyType.SelectedIndex = ddlBodyType.Items.IndexOf(ddlBodyType.Items.FindByText(response.vehicleBodyType));
                ddlEnginecc.SelectedIndex = ddlEnginecc.Items.IndexOf(ddlEnginecc.Items.FindByValue(response.EngineCC.ToString()));
                txtRegistration.Text = response.RegistrationNumber;
                txtChassis.Text = response.ChassisNo;
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

        private void SetVehicleMake(DataServiceManager service, BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsurancePolicy response)
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
                    ddlModel.DataSource = ExtensionMethod.GetDistictModel(response.Subclass, vehicleModeldt);
                    ddlModel.DataBind();
                    ddlModel.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
                ddlModel.SelectedIndex = ddlModel.Items.IndexOf(ddlModel.Items.FindByText(response.VehicleModel));
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
            mtorEndorsement.Remarks = //txtRemarks.Text;
            mtorEndorsement.AccountNumber = "";
            mtorEndorsement.EndorsementType = "InternalEndorsement";
            mtorEndorsement.PaymentType =  string.Empty;
            mtorEndorsement.InsuredCode = mtorPolicyDetails.InsuredCode;
            mtorEndorsement.InsuredName = mtorPolicyDetails.InsuredName;            
            mtorEndorsement.FinancierCompanyCode = mtorPolicyDetails.FinancierCompanyCode;
            mtorEndorsement.CPR = mtorPolicyDetails.CPR;
            mtorEndorsement.NewExcess = mtorPolicyDetails.ExcessAmount;
            mtorEndorsement.PremiumBeforeDiscount = 0;
            mtorEndorsement.PremiumAfterDiscount = 0;
            mtorEndorsement.CommisionBeforeDiscount = 0;
            mtorEndorsement.CommissionAfterDiscount = 0;            
            mtorEndorsement.RenewalCount = mtorPolicyDetails.RenewalCount;

            //InternalEndorsement
            mtorEndorsement.ChassisNo = txtChassis.Text.Trim();
            mtorEndorsement.RegistrationNo = txtRegistration.Text.Trim();
            mtorEndorsement.VehicleMake = ddlMake.SelectedItem.Text.Trim();
            mtorEndorsement.VehicleModel = ddlModel.SelectedItem.Text.Trim();
            mtorEndorsement.EngineCC = Convert.ToInt32(ddlEnginecc.SelectedItem.Value);
            mtorEndorsement.VehicleYear = Convert.ToInt32(ddlManufactureYear.SelectedItem.Text.Trim());
            mtorEndorsement.VehicleBodyType = ddlBodyType.SelectedItem.Text.Trim();
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
            btnSubmit.Enabled = false;
            txtEffectiveFromDate.Text = string.Empty;
            txtEffectiveToDate.Text = string.Empty;
            txtOldClientCode.Text = string.Empty;
            txtOldInsuredName.Text = string.Empty;
                     
            gvMotorEndorsement.DataSource = null;
            gvMotorEndorsement.DataBind();            
            //txtRemarks.Text = string.Empty;
            
        }
        
    }
}