using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class InsuredMaster : System.Web.UI.Page
    {
        private General master;
        public static DataTable Nationalitydt;
        public static DataTable Area;        
        public static int PageType { get; set; }
        public static long _InsuredID { get; set; }     

        public InsuredMaster()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!Page.IsPostBack)
            {
                btnSubmit.Text = "Save";               
                ClearControl();
                BindDropdown();
                if (Request.QueryString["type"] != null)
                {
                    PageType = Convert.ToInt32(Request.QueryString["type"]);
                }
                _InsuredID = 0;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string opertaion = string.Empty; 

                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();
                    var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails
                    {
                        CPR = HttpUtility.HtmlDecode(txtCPR.Text),                       
                        FirstName = HttpUtility.HtmlDecode(txtFirstName.Text),
                        MiddleName = HttpUtility.HtmlDecode(txtMiddleName.Text),
                        LastName = HttpUtility.HtmlDecode(txtLastName.Text),
                        Gender = HttpUtility.HtmlDecode(ddlGender.SelectedValue),
                        Flat = HttpUtility.HtmlDecode(txtFlat.Text),
                        Building = HttpUtility.HtmlDecode(txtBuilding.Text),
                        Road = HttpUtility.HtmlDecode(txtRoad.Text),
                        Block = HttpUtility.HtmlDecode(txtBlock.Text),
                        Area = HttpUtility.HtmlDecode(ddlArea.SelectedValue),
                        Mobile = HttpUtility.HtmlDecode(txtMobile.Text),
                        Email = HttpUtility.HtmlDecode(txtEmail.Text),
                        Nationality = HttpUtility.HtmlDecode(ddlNationality.SelectedValue),
                        Occupation = HttpUtility.HtmlDecode(txtOccupation.Text),
                        DateOfBirth = txtDateOfBirth.Text.CovertToCustomDateTime(),
                        PassportNo = HttpUtility.HtmlDecode(txtPassport.Text),

                        IsActive = true
                    };

                    opertaion = HttpUtility.HtmlDecode((sender as Button).Text);

                    if (opertaion == "Update")
                    {
                        details.InsuredId = _InsuredID;
                        details.Type = "edit";
                    }
                    else
                    {
                       if(ValidateCPR(details.CPR, userInfo, service))
                        {
                            return;
                        }
                        details.Type = "insert";
                    }
                    details.Agency = userInfo.Agency;
                    details.AgentCode = userInfo.AgentCode;

                    var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                  <BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetailsResponse>, 
                                  BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredMasterDetails>
                                  (BKIC.SellingPoint.DTO.Constants.AdminURI.InsuredMasterOperation, details);

                    if (results.StatusCode == 200 && results.Result.IsTransactionDone)
                    {
                        master.ShowLoading = false;
                        if (details.Type == "insert")
                        {                            
                            if (PageType == 1)
                            {
                                Response.Redirect("DomesticHelp.aspx?CPR=" + details.CPR + "&InsuredCode=" + results.Result.InsuredCode + "&InsuredName=" + results.Result.InsuredName);
                            }
                            else if (PageType == 2)
                            {
                                Response.Redirect("Travelnsurance.aspx?CPR=" + details.CPR + "&InsuredCode=" + results.Result.InsuredCode + "&InsuredName=" + results.Result.InsuredName + "&DOB=" + details.DateOfBirth.ConvertToLocalFormat());
                            }
                            else if (PageType == 3)
                            {
                                Response.Redirect("HomeInsurancePage.aspx?CPR=" + details.CPR + "&InsuredCode=" + results.Result.InsuredCode + "&InsuredName=" + results.Result.InsuredName);
                            }
                            else if (PageType == 4)
                            {
                                Response.Redirect("MotorInsurance.aspx?CPR=" + details.CPR + "&InsuredCode=" + results.Result.InsuredCode + "&InsuredName=" + results.Result.InsuredName + "&DOB="+ details.DateOfBirth.ConvertToLocalFormat());
                            }
                            else if (PageType == 5)
                            {
                                Response.Redirect("MotorTransferEndorsement.aspx?CPR=" + details.CPR + "&InsuredCode=" + results.Result.InsuredCode + "&InsuredName=" + results.Result.InsuredName + "&DOB=" + details.DateOfBirth.ConvertToLocalFormat());
                            }
                            else
                            {
                                master.ShowErrorPopup("Insured Details Saved Successfully", "Insured");
                            }
                        } 
                        else if(details.Type == "edit")
                        {
                            master.ShowErrorPopup("Insured Details Updated Successfully", "Insured");
                        }
                        ClearControl();                       
                        btnSubmit.Text = "Save";                       
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
        protected void txtCPR_Changed(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                ValidateCPR(txtCPR.Text, userInfo, service);
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
        public bool ValidateCPR(string CPR, OAuthTokenResponse userInfo, DataServiceManager service)
        {
            bool isCPRExist = false;            

            if (!string.IsNullOrEmpty(CPR))
            {
                var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyUserRequest
                {
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    CPR = CPR
                };

                var insuredResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                    <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredResponse>,
                                    BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyUserRequest>
                                   (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyInsured, req);

                if (insuredResult.StatusCode == 200 && insuredResult.Result.IsTransactionDone
                    && insuredResult.Result.AgencyInsured.Count > 0)
                {
                    master.ShowErrorPopup("The CPR is already exists !!!", "CPR");
                    isCPRExist = true;                   
                }
                return isCPRExist;
            }
            return isCPRExist;
        }
        protected void txtSearch_ByCPR(object sender, EventArgs e)
        {
            try
            {
                GetInsuredUserByCPR();                
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

        private void GetInsuredUserByCPR()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var req = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest
            {
                AgentBranch = userInfo.AgentBranch,
                AgentCode = userInfo.AgentCode,
                Agency = userInfo.Agency,
                CPR = txtSearchByCPR.Text.Trim()
            };

            var insuredResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyInsuredRequest>
                                (BKIC.SellingPoint.DTO.Constants.AdminURI.GetAgencyInsured, req);

            if (insuredResult.StatusCode == 200 && insuredResult.Result.IsTransactionDone && insuredResult.Result.AgencyInsured.Count > 0)
            {
                var insured = insuredResult.Result.AgencyInsured[0];
                txtCPR.Text = insured.CPR;
                txtPassport.Text = insured.PassportNo;
                txtFirstName.Text = insured.FirstName;
                txtMiddleName.Text = insured.MiddleName;
                txtLastName.Text = insured.LastName;
                txtFlat.Text = insured.Flat;
                ddlGender.SelectedIndex = insured.Gender == "Male" ? 1 : 2;
                txtRoad.Text = insured.Road;
                txtBuilding.Text = insured.Building;
                txtBlock.Text = insured.Block;
                SetAreaBlock();
                txtEmail.Text = insured.Email;
                txtMobile.Text = insured.Mobile;
                txtDateOfBirth.Text = insured.DateOfBirth.ConvertToLocalFormat();
                ddlNationality.SelectedIndex = ddlNationality.Items.IndexOf(ddlNationality.Items.FindByValue(insured.Nationality));
                txtOccupation.Text = insured.Occupation;
                btnSubmit.Text = HttpUtility.HtmlDecode("Update");
                _InsuredID = insured.InsuredId;

            }
            else
            {
                ClearControl();
                btnSubmit.Text = "Save";
            }
        }       

        private void GetUserforEdit(string insuredCd, string cpr)
        {

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var insured = new InsuredRequest
            {
                CPR = cpr,
                InsuredCode = insuredCd,
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode
            };

            var serviceResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredResponse>,
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest>
                               (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchUserDetailsByCPRInsuredCode, insured);

            if (serviceResult.StatusCode == 200 && serviceResult.Result.IsTransactionDone)
            {
                var userInformation = serviceResult.Result.InsuredDetails;
                if (userInformation != null)
                {                  
                    txtCPR.Text = userInformation.CPR;
                    txtFirstName.Text = userInformation.FirstName;
                    txtMiddleName.Text = userInformation.MiddleName;
                    txtLastName.Text = userInformation.LastName;                   
                    ddlGender.SelectedIndex = ddlGender.Items.IndexOf(ddlGender.Items.FindByValue(userInformation.Gender));
                    txtFlat.Text = userInformation.Flat;
                    txtBuilding.Text = userInformation.Building;
                    txtRoad.Text = userInformation.Road;
                    txtBlock.Text = userInformation.Block;
                    ddlArea.SelectedIndex = ddlArea.Items.IndexOf(ddlArea.Items.FindByValue(userInformation.Area));
                    txtMobile.Text = userInformation.Mobile;
                    txtEmail.Text = userInformation.Email;
                    txtDateOfBirth.Text = userInformation.DateOfBirth.ConvertToLocalFormat();
                    ddlNationality.SelectedIndex = ddlNationality.Items.IndexOf(ddlNationality.Items.FindByValue(userInformation.Nationality));
                    txtOccupation.Text = userInformation.Occupation;
                }
            }
        }    

        protected void btn_CancelClick(object sender, EventArgs e)
        {
            Response.Redirect("Homepage.aspx");
        }

        private void ClearControl()
        {
            txtCPR.Text = string.Empty;           
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            ddlGender.SelectedIndex = 0;
            txtFlat.Text = string.Empty;
            txtBuilding.Text = string.Empty;
            txtRoad.Text = string.Empty;
            txtBlock.Text = string.Empty;
            ddlArea.SelectedIndex = 0;
            txtMobile.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlNationality.SelectedIndex = 0;
            txtOccupation.Text = string.Empty;
            txtDateOfBirth.Text = string.Empty;
            txtPassport.Text = string.Empty;
        }

        private void BindDropdown()
        {
            master.IsSessionAvailable();         
            var service = CommonMethods.GetLogedInService();

            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                 (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns
                                 .Replace("{type}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.Travelnsurance));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                Nationalitydt = dropdownds.Tables["Nationality"];
                var area = dropdownds.Tables["AreaMaster"];
                Area = dropdownds.Tables["AreaMaster"];               
                if (Nationalitydt.Rows.Count > 0)
                {
                    ddlNationality.DataValueField = "Code";
                    ddlNationality.DataTextField = "Description";
                    ddlNationality.DataSource = Nationalitydt;
                    ddlNationality.DataBind();
                    ddlNationality.Items.Insert(0, new ListItem("--Please Select--", ""));
                    ddlNationality.Items[0].Selected = true;
                    ddlNationality.Items[1].Selected = false;
                }
                if (area != null && area.Rows.Count > 0)
                {
                    ddlArea.DataValueField = "Area";
                    ddlArea.DataTextField = "Area";
                    ddlArea.DataSource = ExtensionMethod.GetDistinctArea(area);
                    ddlArea.DataBind();
                    ddlArea.Items.Insert(0, new ListItem("--Please Select--", ""));
                    ddlArea.Items[0].Selected = true;
                    ddlArea.Items[1].Selected = false;
                }
            }
        }
        protected void BlockNumber_Changed(object sender, EventArgs e)
        {

            SetAreaBlock();
           
        }
        public void SetAreaBlock()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtBlock.Text))
                {
                    if (Area != null && Area.Rows.Count > 0)
                    {
                        var AreaList = from row in Area.AsEnumerable()
                                       where row.Field<string>("AreaCode") == txtBlock.Text.Trim()
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
       
    }
}