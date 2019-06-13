using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class Login : System.Web.UI.Page
    {
        private General master;

        public Login()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                master = Master as General;
                CommonMethods.DeleteCookie();               
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                master = Master as General;              
                var authentication = new BKIC.SellingPoint.DTO.RequestResponseWrappers.OAuthRequest();
                authentication.UserName = txtUserName.Text.Trim();
                authentication.Password = txtLoginPassword.Value;
                authentication.GrantType = "";

                var client = new BKIC.SellingPoint.Presentation.ClientUtility
                {
                    serviceManger = new KBIC.Utility.DataServiceManager(BKIC.SellingPoint.Presentation.ClientUtility.WebApiUri, "", true)
                };

                client.UserInfo = client.serviceManger.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.OAuthTokenResponse,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.OAuthRequest>
                                 (BKIC.SellingPoint.DTO.Constants.UserURI.Authentication, authentication);

                if (client.UserInfo.StatusCode == 200)
                {
                    client.serviceManger = new KBIC.Utility.DataServiceManager(BKIC.SellingPoint.Presentation.ClientUtility.WebApiUri, client.UserInfo.AccessToken, false);

                    // Session["UserInfo"] = client.UserInfo;
                    //  Session.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(client.UserInfo.ExpiresIn).TotalMinutes);
                    SetUserInfoKey(client);
                    // createa a new GUID and save into the session
                    string guid = Guid.NewGuid().ToString();
                    // now create a new cookie with this guid value
                    //Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", guid));

                    if (Session["ReturnUrl"] != null)
                    {
                        this.Response.Redirect(Session["ReturnUrl"].ToString());
                    }
                    else
                    {
                        var productRequest = new AgecyProductRequest();
                        productRequest.Agency = client.UserInfo.Agency;
                        productRequest.AgentCode = client.UserInfo.AgentCode;
                        productRequest.MainClass = string.Empty;
                        productRequest.SubClass = string.Empty;
                        productRequest.Type = "MotorInsurance";

                        var productResponse = client.serviceManger.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                                <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgencyProductResponse>,
                                                BKIC.SellingPoint.DTO.RequestResponseWrappers.AgecyProductRequest>
                                                (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchAgencyProductByType, productRequest);

                        if (productResponse.StatusCode == 200 && productResponse.Result.IsTransactionDone)
                        {
                            if (productResponse.Result.MotorProducts != null && productResponse.Result.MotorProducts.Count > 0)
                            {
                                Session["MotorProducts"] = productResponse.Result.MotorProducts;
                            }
                        }
                        this.Response.Redirect("HomePage.aspx");
                    }
                }
                else if(client.UserInfo.StatusCode == 404)
                {
                    ErrorMessage.Text = "Service not available!";
                    //  Session.Remove("UserInfo");
                    CommonMethods.DeleteCookie();
                }
                else
                {
                    ErrorMessage.Text = "Invalid credentials";
                  //  Session.Remove("UserInfo");
                    CommonMethods.DeleteCookie();
                }
               
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
        }

        public void SetUserInfoKey(ClientUtility client)
        {
            string userInformation = JsonConvert.SerializeObject(client.UserInfo);
            HttpCookie cookie = new HttpCookie(CommonMethods.UserInfoKey)
            {
                Value = userInformation.Encrypt(),
                Expires = client.UserInfo.ExpiresDate
            };
            Response.Cookies.Add(cookie);
        }

        public void SetUserTempKey(ClientUtility client)
        {
            string userInformation = JsonConvert.SerializeObject(client.UserInfo);
            HttpCookie cookie = new HttpCookie(CommonMethods.UserTempInfoKey);
            cookie.Value = userInformation.Encrypt();
            cookie.Expires = client.UserInfo.ExpiresDate;
            Response.Cookies.Add(cookie);
        }
    }
}