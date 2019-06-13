using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class General : System.Web.UI.MasterPage
    {
       
        public string lang;      

        public bool ShowLoading
        {
            set
            {
                this.loadPageUC.ShowLoading = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                if (userInfo.Agency.ToLower() == "bbk")
                    {
                        clientSecura.Visible = true;
                    }
                    if (userInfo.Agency.ToLower() == "tisco")
                    {
                        clientTisco.Visible = true;
                    }
                    if (userInfo.Roles.ToLower() == "user")
                    {
                        addNewUser.Visible = false;
                        lstReports.Visible = false;

                        lstAgents.Visible = false;
                        lstProduct.Visible = false;
                    }
                    else if (userInfo.Roles.ToLower() == "branchadmin")
                    {
                        addNewUser.Visible = true;
                        lstReports.Visible = true;

                        lstAgents.Visible = false;
                        lstProduct.Visible = false;
                    }
                    else if (userInfo.Roles.ToLower() == "superadmin")
                    {
                        addNewUser.Visible = true;
                        lstReports.Visible = true;

                        lstAgents.Visible = true;
                        lstProduct.Visible = true;
                    }
                    AgencyHidden.Value = userInfo.Agency;
                    AgentCodeHidden.Value = userInfo.AgentCode;
                    AgentBranch.Value = userInfo.AgentBranch;
                    WebApiUrlHidden.Value = ConfigurationManager.AppSettings["WebApiUri"].ToString();                
            }
        }

        protected void btnRenewLogin_Click(object sender, EventArgs e)
        {
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
        }

        public InsuredMasterDetails GetInsured(string CPR, string InsuredCode, string Agency, string AgentCode)
        {
            this.IsSessionAvailable();
            var service = CommonMethods.GetLogedInService();

            var insured = new InsuredRequest
            {
                CPR = CPR,
                InsuredCode = "",
                Agency = Agency,
                AgentCode = AgentCode
            };
            var serviceResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                               <BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredResponse>,
                               BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest>
               (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchUserDetailsByCPRInsuredCode, insured);

            if (serviceResult.StatusCode == 200 && serviceResult.Result.IsTransactionDone)
            {
                return serviceResult.Result.InsuredDetails;
            }
            else
            {
                ShowErrorPopup("Insured not found", "Insured");
                return null;
            }           
        }

        public int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = 0;
            if (birthDate != null)
            {
                age = now.Year - birthDate.Year;
                if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                    age--;

                return age;
            }
            return age;
        }

        public decimal NearestOneHalf(decimal TotalPremium)
        {
            decimal d1 = TotalPremium * 2;
            decimal d2 = Math.Round(TotalPremium * 2, 2) / 2;
            decimal d3 = Math.Round(d2, 3);
            string ThreeDigitDecimal = d3.ToString("#.000");
            return Convert.ToDecimal(ThreeDigitDecimal);
        }

        public void ShowErrorPopup(string message, string title)
        {
            this.modelTitle.InnerText = title;
            this.errorBodyText.InnerText = message;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "error", "ShowErrorMessage();", true);
        }

        public void RedirectToLogin()
        {
            string OriginalUrl = HttpContext.Current.Request.RawUrl;
            string LoginPageUrl = "/login.aspx";
            Session["ReturnUrl"] = OriginalUrl;

            if (!OriginalUrl.Contains("UserRegistration.aspx"))
            {
                HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", LoginPageUrl, OriginalUrl));
            }
            else
            {
                HttpContext.Current.Response.Redirect("login.aspx");
            }
        }

        public void RedirectToUserRegistration()
        {
            Response.Redirect("UserRegistration.aspx");
        }

        public void TransactionFailed(string errorMessage)
        {
            Response.Redirect("Login.aspx");
        }       

        public string DateFormate(string formate, DateTime dateTime)
        {
            return string.Format(formate, dateTime);
        }

        //public DataServiceManager GetService()
        //{
        //    return new DataServiceManager(ClientUtility.WebApiUri, "", false);
        //}

        //public DataServiceManager GetLoggedInService()
        //{
        //    if (Session["UserInfo"] != null)
        //    {
        //        var userInfo = Session["UserInfo"] as OAuthTokenResponse;
        //        return new DataServiceManager(ClientUtility.WebApiUri, userInfo.AccessToken, false);
        //    }
        //    else
        //    {
        //        return new DataServiceManager(ClientUtility.WebApiUri, "", false);
        //    }
        //}

       

        public void IsSessionAvailable(string key = "", bool pCheckFurtherAction = true)
        {
            key = string.IsNullOrEmpty(key) ? CommonMethods.UserInfoKey : key;
            if (CommonMethods.IsCookieAvailable(key))
            {
                var userInfo = CommonMethods.GetLogedInService(key);

                string value = "";
                value = HttpContext.Current.Request.Cookies[key].Value;
                value = value.Decrypt();
                OAuthTokenResponse info = JsonConvert.DeserializeObject<OAuthTokenResponse>(value);

                if (info == null || string.IsNullOrEmpty(value))
                {
                    Response.Redirect("Login.aspx");
                }                

                DateTime iKnowThisIsUtc = info.ExpiresDate;
                DateTime runtimeKnowsThisIsUtc = DateTime.SpecifyKind(
                    iKnowThisIsUtc,
                    DateTimeKind.Utc);
                DateTime localVersion = runtimeKnowsThisIsUtc.ToLocalTime();
                if (localVersion < DateTime.Now)
                {
                    Response.Redirect("Login.aspx");
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        public bool IsEmailValid(string emailInput)
        {
            bool isEmail = Regex.IsMatch(emailInput, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            return isEmail;
        }

        public bool IsPasswordStrengthValid(string password)
        {
            return Regex.Match(password, @"^.{5,}$").Success;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {            
            Response.Redirect("Logout.aspx");
        }

        public void ShowHideErrorSpacingSpan()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "HideShowHiddenRequiredSpan", "HideShowHiddenRequiredSpan();", true);
        }

        public void ShowErrorPopup(string message)
        {
            this.errorBodyText.InnerText = message;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "error", "ShowErrorMessage();", true);
        }
        public void ShowClaimPopup(string message)
        {
            this.txtClaimBody.InnerText = message;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "error", "ShowClaimPopup();", true);
        }

        public void SetCancelDateDate()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setCancelDateDate", "setCancelDateDate();", true);
        }

        public void SetExtendDate()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setExtendDate", "setExtendDate();", true);
        }

        public void SetRenewalDate()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "setRenewalDate", "setRenewalDate();", true);
        }

        public void ClearControls(Control c)
        {
            if (c != null)
            {
                foreach (Control childControl in c.Controls)
                {
                    if (childControl.GetType() == typeof(TextBox))
                    {
                        ((TextBox)childControl).Text = string.Empty;
                        ((TextBox)childControl).Enabled = true;
                    }
                    else if (childControl.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)childControl).Checked = false;
                        ((CheckBox)childControl).Enabled = true;
                    }
                    else if (childControl.GetType() == typeof(DropDownList))
                    {
                        if (((DropDownList)childControl).SelectedIndex != -1)
                        {
                            ((DropDownList)childControl).SelectedIndex = 0;
                        }
                        ((DropDownList)childControl).Enabled = true;
                    }
                    else if (childControl.GetType() == typeof(Button))
                    {
                        ((Button)childControl).Enabled = true;
                    }
                    if (childControl.Controls.Count > 0)
                    {
                        ClearControls(childControl);
                    }
                }
            }
        }

        public void makeReadOnly(Control c, bool enable)
        {
            if (c != null)
            {
                foreach (Control childControl in c.Controls)
                {
                    if (childControl.GetType() == typeof(TextBox))
                    {
                        ((TextBox)childControl).Enabled = enable;
                    }
                    else if (childControl.GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)childControl).Enabled = enable;
                    }
                    else if (childControl.GetType() == typeof(DropDownList))
                    {
                        ((DropDownList)childControl).Enabled = enable;
                    }
                    else if (childControl.GetType() == typeof(Button))
                    {
                        ((Button)childControl).Enabled = enable;
                    }
                    if (childControl.Controls.Count > 0)
                    {
                        makeReadOnly(childControl, enable);
                    }
                }
            }
        }

        public bool IsQuickRenewUser()
        {
            if (Session["QuicRenewUserInfo"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get vat amount for the premium.
        /// </summary>
        /// <param name="Premium">Policy premium.</param>
        /// <param name="Commission">Policy commission.</param>
        /// <returns>Vat amount, commission amount.</returns>
        public VatResponse GetVat(decimal Premium, decimal Commission)
        {
            try
            {
                var service = new DataServiceManager(ClientUtility.WebApiUri, "", false);
                var vatRequest = new DTO.RequestResponseWrappers.VatRequest();
                vatRequest.PremiumAmount = Premium;
                vatRequest.CommissionAmount = Commission;

                decimal Vat = 0;
                decimal VatCommission = 0;

                var vatresult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.VatResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.VatRequest>
                                       (BKIC.SellingPoint.DTO.Constants.VatURI.CalculateVat, vatRequest);

                if (vatresult.StatusCode == 200 && vatresult.Result.IsTransactionDone && vatresult.Result.VatAmount >= 0)
                {
                    Vat = vatresult.Result.VatAmount;
                    VatCommission = vatresult.Result.VatCommissionAmount;
                }
                return new VatResponse
                {
                    IsTransactionDone = true,
                    VatAmount = Vat,
                    VatCommissionAmount = VatCommission
                };
            }
            catch (Exception ex)
            {
                return new VatResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        public DTO.RequestResponseWrappers.MotorProduct GetProduct(string mainClass, string subClass)
        {
            DTO.RequestResponseWrappers.MotorProduct product = null;
            var motorProduct = (List<DTO.RequestResponseWrappers.MotorProduct>)Session["MotorProducts"];
            if (motorProduct != null)
            {
                product = motorProduct.Find(x => x.MainClass == mainClass
                                         && x.SubClass == subClass);
            }
            else
            {
                this.IsSessionAvailable();
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

                        product = productResponse.Result.MotorProducts.Find(x => x.MainClass == mainClass
                                         && x.SubClass == subClass);
                    }
                }
            }
            return product;
        }

        public DTO.RequestResponseWrappers.HomeProduct GetHomeProduct(string mainClass, string subClass)
        {
            DTO.RequestResponseWrappers.HomeProduct product = null;
            var homeProduct = (List<DTO.RequestResponseWrappers.HomeProduct>)Session["HomeProducts"];
            if (homeProduct != null)
            {
                product = homeProduct.Find(x => x.MainClass == mainClass
                                         && x.SubClass == subClass);
            }
            else
            {
                this.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var productRequest = new AgecyProductRequest();
                productRequest.Agency = userInfo.Agency;
                productRequest.AgentCode = userInfo.AgentCode;
                productRequest.MainClass = mainClass;
                productRequest.SubClass = subClass;
                productRequest.Type = Constants.Home;

                var productResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                        <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyProductResponse>,
                        BKIC.SellingPoint.DTO.RequestResponseWrappers.AgecyProductRequest>
                        (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchAgencyProductByType, productRequest);

                if (productResponse.StatusCode == 200 && productResponse.Result.IsTransactionDone)
                {
                    if (productResponse.Result.HomeProducts != null && productResponse.Result.HomeProducts.Count > 0)
                    {
                        Session["HomeProducts"] = productResponse.Result.HomeProducts;

                        product = productResponse.Result.HomeProducts.Find(x => x.MainClass == mainClass
                                         && x.SubClass == subClass);
                    }
                }
            }
            return product;
        }
    }
}