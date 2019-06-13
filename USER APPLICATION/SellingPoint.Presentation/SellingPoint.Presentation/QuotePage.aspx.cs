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
using System.Web.UI.WebControls;

namespace SellingPoint.Presentation
{
    public partial class QuotePage : System.Web.UI.Page
    {
        private General master;
        public static string MotorMainClass { get; set; }
        public static string HomeMainClass { get; set; }

        public QuotePage()
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
                BindDropdown(userInfo, service);                             
            }
        }

        private void BindDropdown(OAuthTokenResponse userInfo, DataServiceManager service)
        {            

            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                 (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns
                                 .Replace("{type}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.Travelnsurance));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdowns = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);

                DataTable Packagedt = dropdowns.Tables["TravelInsurancePackage"];
                DataTable Peroiddt = dropdowns.Tables["TravelInsurancePeroid"];
                DataTable travelCoveragedt = dropdowns.Tables["TravelCoverage"];

                if (Packagedt.Rows.Count > 0)
                {
                    ddlPackage.DataValueField = "Code";
                    ddlPackage.DataTextField = "Name";
                    ddlPackage.DataSource = Packagedt;
                    ddlPackage.DataBind();
                    ddlPackage.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
                if (travelCoveragedt.Rows.Count > 0)
                {
                    ddlJourney.DataValueField = "Code";
                    ddlJourney.DataTextField = "CoverageType";
                    ddlJourney.DataSource = travelCoveragedt;
                    ddlJourney.DataBind();
                    ddlJourney.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
                if (Peroiddt.Rows.Count > 0)
                {
                    ddlPeriod.DataValueField = "Code";
                    ddlPeriod.DataTextField = "Name";
                    ddlPeriod.DataSource = Peroiddt;
                    ddlPeriod.DataBind();
                    ddlPeriod.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
            }

            var productCode = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchProductCodeResponse>>
                              (BKIC.SellingPoint.DTO.Constants.DropDownURI.GetInsuranceProductCode
                              .Replace("{agency}", userInfo.Agency)
                              .Replace("{agencyCode}", userInfo.AgentCode)
                              .Replace("{insurancetypeid}", "4"));

            if (productCode != null && productCode.StatusCode == 200 && productCode.Result.IsTransactionDone)
            {
                var products = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                               (BKIC.SellingPoint.DTO.Constants.DropDownURI.GetAgencyProducts
                               .Replace("{agency}", userInfo.Agency)
                               .Replace("{agencyCode}", userInfo.AgentCode)
                               .Replace("{mainclass}", productCode.Result.productCode)
                               .Replace("{page}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.MotorInsurance));

                if (products != null && products.StatusCode == 200 && products.Result.IsTransactionDone)
                {
                    DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(products.Result.dropdownresult);
                    DataTable prods = dropdownds.Tables["Products"];
                    ddlCover.DataValueField = "SUBCLASS";
                    ddlCover.DataTextField = "DESCRIPTION";
                    ddlCover.DataSource = prods;
                    ddlCover.DataBind();
                    ddlCover.Items.Insert(0, new ListItem("--Please Select--", ""));

                    MotorMainClass = productCode.Result.productCode;
                }
            }
            var productResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchProductCodeResponse>>(
                                BKIC.SellingPoint.DTO.Constants.DropDownURI.GetInsuranceProductCode
                                .Replace("{agency}", userInfo.Agency)
                                .Replace("{agencyCode}", userInfo.AgentCode)
                                .Replace("{insurancetypeid}", "3"));

            if (productResult != null && productResult.StatusCode == 200 && productResult.Result.IsTransactionDone)
            {
                var products = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>(
                               BKIC.SellingPoint.DTO.Constants.DropDownURI.GetAgencyProducts
                               .Replace("{agency}", userInfo.Agency)
                               .Replace("{agencyCode}", userInfo.AgentCode)
                               .Replace("{mainclass}", productResult.Result.productCode)
                               .Replace("{page}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.HomeInsurance));

                HomeMainClass = productResult.Result.productCode;
                if (products != null && products.StatusCode == 200 && products.Result.IsTransactionDone)
                {
                    DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(products.Result.dropdownresult);
                    DataTable prods = dropdownds.Tables["Products"];

                    //In future product may be increase. Now it has only one product.
                    //if (prods != null && prods.Rows.Count > 0)
                    //{
                    //    //SubClass = prods.Rows[0]["SubClass"].ToString();
                    //    ddlCover.DataValueField = "SUBCLASS";
                    //    ddlCover.DataTextField = "SUBCLASS";
                    //    ddlCover.DataSource = prods;
                    //    ddlCover.DataBind();
                    //    ddlCover.Items.Insert(0, new ListItem("--Please Select--", ""));
                    //}
                }
            }
        }

        protected void Calculate_MotorExpireDate(object sender, EventArgs e)
        {
            SetExpireDate();
        }

        private void SetExpireDate()
        {
            if (!string.IsNullOrEmpty(txtInsuredPeriodFrom.Text))
            {
                if (ddlVehicleType.SelectedItem.Value == "New")
                {
                    var newDate = Convert.ToDateTime(txtInsuredPeriodFrom.Text.CovertToCustomDateTime()).AddYears(1);
                    var startOfMonth = new DateTime(newDate.Year, newDate.Month, 1);
                    var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
                    txtInsuredPeriodTo.Text = endOfMonth.CovertToLocalFormat();
                }
                else if (ddlVehicleType.SelectedItem.Value == "Used")
                {
                    txtInsuredPeriodTo.Text = Convert.ToDateTime(txtInsuredPeriodFrom.Text.CovertToCustomDateTime())
                                              .AddYears(1)
                                              .AddDays(-1)
                                              .CovertToLocalFormat();
                }
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

        public string getExpiryDate(string peroid, string commencedate)
        {

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceExpiryDate input = new TravelInsuranceExpiryDate
            {
                PackageCode = peroid,
                CommenceDate = commencedate.CovertToCustomDateTime()
            };

            var expiryDateResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers
                                     .ApiResponseWrapper<BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceExpiryDateResponse>,
                                     BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceExpiryDate>
                                    (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetPolicyExpirtyDate, input);

            if (expiryDateResponse != null && expiryDateResponse.StatusCode == 200
                && expiryDateResponse.Result != null && expiryDateResponse.Result.IsTransactionDone)
            {
                return expiryDateResponse.Result.ExpiryDate.Value.CovertToLocalFormat();
               
            }

            return "";
        }

        protected void ddlPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            PackageDetailsChanges();
        }

        private void PackageDetailsChanges()
        {
            try
            {                
                if (ddlPackage.SelectedItem.Text.ToLower() == "family")
                {
                    ddlJourney.SelectedIndex = 1;
                    ddlJourney.Enabled = false;
                    rfvddlJourney.Enabled = true;
                }
                else if (ddlPackage.SelectedItem.Text.ToLower() == "individual")
                {
                    ddlJourney.SelectedIndex = -1;
                    ddlJourney.Enabled = true;
                    rfvddlJourney.Enabled = true;
                }
                else if (ddlPackage.SelectedItem.Text.ToLower() == "schengen")
                {
                    ddlJourney.SelectedIndex = -1;
                    ddlJourney.Enabled = false;
                    rfvddlJourney.Enabled = false;
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

        protected void ddlCoverage_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlJourney_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void Calculate_Motor(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                if (!ValidateProduct())
                {
                    return;
                }
        
                var motorQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuote
                {
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    VehicleSumInsured = Convert.ToDecimal(txtSumInsured.Text),
                    TypeOfInsurance = ddlCover.SelectedItem.Value.Trim(),
                    VehicleType = ddlVehicleType.SelectedItem.Value
                };
                motorQuote.Agency = userInfo.Agency;
                motorQuote.AgentCode = userInfo.AgentCode;
                motorQuote.PolicyStartDate = txtInsuredPeriodFrom.Text.CovertToCustomDateTime();
                motorQuote.PolicyEndDate = txtInsuredPeriodTo.Text.CovertToCustomDateTime();
                motorQuote.DOB = txtDateOfBirth.Text.CovertToCustomDateTime();
                motorQuote.MainClass = MotorMainClass;

                var motorQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuoteResponse>, 
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorInsuranceQuote>
                                       (BKIC.SellingPoint.DTO.Constants.MotorURI.GetQuote, motorQuote);

                if (motorQuoteResult.StatusCode == 200 && motorQuoteResult.Result.IsTransactionDone)
                {
                    txtMotorPremium.Text = motorQuoteResult.Result.TotalPremium.ToString();
                    var vatResponse = master.GetVat(Convert.ToDecimal(txtMotorPremium.Text), 0);
                    if (vatResponse != null && vatResponse.IsTransactionDone)
                    {
                        txtMotorVat.Text = Convert.ToString(vatResponse.VatAmount);
                        txtMotorTotal.Text = Convert.ToString(Convert.ToDecimal(txtMotorPremium.Text) + Convert.ToDecimal(vatResponse.VatAmount));
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


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool ValidateProduct()
        {
            bool isvalid = true;
            var product = master.GetProduct(MotorMainClass, ddlCover.SelectedItem.Value);
            if (product != null)
            {
                if (!product.AllowUnderAge && CalculateAgeCorrect(txtDateOfBirth.Text.CovertToCustomDateTime(), DateTime.Now) < product.UnderAge)
                {
                    master.ShowErrorPopup("Insured is under age !", "Can't issue a policy !");
                    isvalid = false;
                } 
                if(!string.IsNullOrEmpty(txtSumInsured.Text) && product.MaximumVehicleValue < Convert.ToDecimal(txtSumInsured.Text))
                {
                    master.ShowErrorPopup("Maximum vehicle value is" + product.MaximumVehicleValue+ "! " + "and the policy will go to admin referal", "Motor");
                    isvalid = false;
                }
                if (!product.AllowUsedVehicle && (ddlVehicleType.SelectedItem.Text == "Used" || ddlVehicleType.SelectedIndex == 1))
                {
                    master.ShowErrorPopup("Used vehicle not eligible for this product", "Can't issue a policy !");
                    isvalid = false;
                }
            }
            return isvalid;
        }


        public int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;

            return age;
        }

        protected void Calculate_Home(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var homeQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuote
                {
                    BuildingValue = !string.IsNullOrEmpty(txtBuildingValue.Text.Trim()) ? Convert.ToDecimal(txtBuildingValue.Text.Trim()) : 0,
                    ContentValue = !string.IsNullOrEmpty(txtContentValue.Text.Trim()) ? Convert.ToDecimal(txtContentValue.Text.Trim()) : 0,
                    JewelleryValue = !string.IsNullOrEmpty(txtJewelleryValue.Text.Trim()) ? Convert.ToDecimal(txtJewelleryValue.Text.Trim()) : 0,
                    // homeQuote.IsPropertyToBeInsured = ddlPropertyInsured.SelectedIndex > 0 ? (ddlPropertyInsured.SelectedIndex == 1 ? true : false) : false;
                    IsRiotStrikeAdded = ddlMaliciousDamageCover.SelectedIndex > 0 ? (ddlMaliciousDamageCover.SelectedIndex == 1 ? true : false) : false,
                    JewelleryCover = ddlJewelleryCoverWithinContents.SelectedItem.Value,
                    NumberOfDomesticWorker = Convert.ToInt32(txtDomesticHelpWorkers.Text),
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    MainClass = HomeMainClass,
                    SubClass = userInfo.Agency == "BBK" ? "SH" : "TSHO"
                };

                var homeQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                     <BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuoteResponse>,
                                     BKIC.SellingPoint.DTO.RequestResponseWrappers.HomeInsuranceQuote>
                                     (BKIC.SellingPoint.DTO.Constants.HomeURI.GetQuote, homeQuote);

                if (homeQuoteResult.StatusCode == 200 && homeQuoteResult.Result.IsTransactionDone)
                {
                    txtHomePremium.Text = Convert.ToString(homeQuoteResult.Result.TotalPremium);
                    var vatResponse = master.GetVat(Convert.ToDecimal(txtHomePremium.Text), 0);
                    if (vatResponse != null && vatResponse.IsTransactionDone)
                    {
                        txtHomeVat.Text = Convert.ToString(vatResponse.VatAmount);
                        txtHomeTotal.Text = Convert.ToString(Convert.ToDecimal(txtHomePremium.Text) + Convert.ToDecimal(vatResponse.VatAmount));
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

        protected void Calculate_Travel(object sender, EventArgs e)
        {
            try
            {
                 
                if (Page.IsValid)
                {
                    master.IsSessionAvailable();
                    var userInfo = CommonMethods.GetUserDetails();
                    var service = CommonMethods.GetLogedInService();

                    if (ddlPackage.SelectedItem.Value == "" || ddlPeriod.SelectedItem.Value == "")
                    {
                        return;
                    }
                    if (21 > master.CalculateAgeCorrect(txtTravelDOB.Text.CovertToCustomDateTime(), DateTime.Now)
                        && userInfo.Agency == "BBK")
                    {                        
                        master.ShowErrorPopup("Insured age should be above 21 years !!", "Insured");
                        return;
                    }
                    if(!ValidateAge(txtTravelDOB.Text.CovertToCustomDateTime()))
                    {
                        return;
                    }                   

                    var travelQuote = new BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuote
                    {

                        //Get travel quote for the given values.
                        Agency = userInfo.Agency,
                        AgentCode = userInfo.AgentCode,
                        MainClass = "MISC",
                        SubClass = ddlPackage.SelectedItem.Text.ToLower() == "individual" ? "STI" : ddlPackage.SelectedItem.Text.ToLower() == "schengen" ? "STI" : "STP",
                        DateOfBirth = txtTravelDOB.Text.CovertToCustomDateTime(),

                        PackageCode = ddlPackage.SelectedItem.Text.ToLower() == "individual" ?
                                              "IN001" : ddlPackage.SelectedItem.Text.ToLower() == "schengen" ?
                                              "SCHEN" : "FM001",

                        PolicyPeriodCode = ddlPeriod.SelectedItem.Value,

                        CoverageType = ddlPackage.SelectedItem.Text.ToLower() == "schengen" ?
                                               "SCHENGEN" : ddlJourney.SelectedItem.Value
                    };


                    var travelQuoteResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                            <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuoteResponse>, 
                                            BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelInsuranceQuote>
                                            (BKIC.SellingPoint.DTO.Constants.TravelInsuranceURI.GetQuote, travelQuote);

                    if (travelQuoteResult.StatusCode == 200 && travelQuoteResult.Result.IsTransactionDone)
                    {                        
                        txtTravelPremium.Text = travelQuoteResult.Result.Premium.ToString();
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

        public bool ValidateAge(DateTime DateOfBirth)
        {
            bool isValid = true;
            int insuredAge = master.CalculateAgeCorrect(DateOfBirth, DateTime.Now);

            if (insuredAge > 79 && (ddlPackage.SelectedItem.Value == "IN001" || ddlPackage.SelectedItem.Value == "SCHEN"))
            {
                isValid = false;
                master.ShowErrorPopup("Can't issue a individual policy!", "Insured over age :" + insuredAge);

            }
            if (insuredAge > 65 && ddlPackage.SelectedItem.Value == "FM001")
            {
                isValid = false;
                master.ShowErrorPopup("Can't issue a family policy!", "Insured over age :" + insuredAge);
            }
            return isValid;
        }
    }
}