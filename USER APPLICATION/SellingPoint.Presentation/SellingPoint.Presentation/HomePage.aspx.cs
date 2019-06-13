using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using BKIC.SellingPoint.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SellingPoint.Presentation
{
    public partial class HomePage : System.Web.UI.Page
    {
        General master;
        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!IsPostBack)
            {
                BindDropdown();
                loadCounts();
            }
        }

        public HomePage()
        {
            master = Master as General;
        }

        public void loadCounts()
        {            
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();            

            string currentdate = Convert.ToString(DateTime.Now);
            var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.DashboardRequest
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(1),
                Agent = userInfo.Agency,
                AgencyCode = userInfo.AgentCode,
                BranchCode = userInfo.AgentBranch
            };

            var Results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                          <BKIC.SellingPoint.DTO.RequestResponseWrappers.DashboardResponse>, BKIC.SellingPoint.DTO.RequestResponseWrappers.DashboardRequest>
                         (BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.FetchDashboard, details);

            if(Results.StatusCode==200 && Results.Result.IsTransactionDone)
            {                
                List<int> activepolicy = new List<int>();
                List<int> renewpolicy = new List<int>();
                foreach (var count in Results.Result.ActiveList)
                {
                    if (count.InsuranceType == Constants.Travel)
                    {
                        travelcount.InnerText = Convert.ToString(count.ActiveCount);
                    }
                    if (count.InsuranceType == Constants.Motor)
                    {
                        motorcount.InnerText = Convert.ToString(count.ActiveCount);
                    }
                    if (count.InsuranceType == Constants.DomesticHelp)
                    {
                        domesticcount.InnerText = Convert.ToString(count.ActiveCount);
                    }
                    if (count.InsuranceType == Constants.Home)
                    {
                        homecount.InnerText = Convert.ToString(count.ActiveCount);
                    }
                }
            }
        }
        public void BindDropdown()
        {            
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();           

            if (userInfo.Agency.ToLower() == "bbk")
                securaLogo.Visible = true;
            if (userInfo.Agency.ToLower() == "tisco")
                tiscoLogo.Visible = true;            
        }
   

        protected void Search_Policy(object sender, EventArgs e)
        {

            try
            {
                gvDocuments.DataSource = null;
                gvDocuments.DataBind();
                GetInsuredPolicies();
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
                gvDocuments.DataSource = null;
                gvDocuments.DataBind();
                GetInsuredPolicies();
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

        private void GetInsuredPolicies()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            string uri = "api/admin/getdocumentsbycpr/{cpr}/{agentcode}";
            
            uri = uri.Replace("{cpr}", txtCPRSearch.Text.Trim()).Replace("{agentcode}", userInfo.AgentCode);

            var AllDocuments = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers
                               .ApiResponseWrapper<BKIC.SellingPoint.DTO.RequestResponseWrappers.DocumentDetailsResult>>(uri);


            if (AllDocuments != null && AllDocuments.Result != null && AllDocuments.StatusCode == 200 && AllDocuments.Result.IsTransactionDone)
            {
                gvDocuments.DataSource = AllDocuments.Result.DocumentDetails;
                gvDocuments.DataBind();

            }

            string uri1 = "api/user/fetchdetailscprinsuredCode";

            BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest request = new InsuredRequest
            {                
                CPR = txtCPRSearch.Text.Trim(),
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode
            };

            var postData = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                          <BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredResponse>,
                          BKIC.SellingPoint.DTO.RequestResponseWrappers.InsuredRequest>(uri1, request);

            if (postData.StatusCode == 200 && postData.Result != null && postData.Result.IsTransactionDone)
            {
                InsuredCode.Value = postData.Result.InsuredDetails.InsuredCode;
                InsuredName.Value = postData.Result.InsuredDetails.LastName;
            }
        }

        protected void lnkbtnNew_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string policyType = (row.FindControl("lblPolicyType") as Label).Text.Trim();
                string policyno = (row.FindControl("lblPolicyNo") as Label).Text.Trim();
                (row.FindControl("lnkbtnNew") as LinkButton).Attributes.Add("target", "_blank");
                
                switch (policyType)
                {
                    case "DomesticHelp":                       
                        Response.Redirect("DomesticHelp.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim());                      
                        break;
                    case "Travel":
                        Response.Redirect("Travelnsurance.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim());
                        break;
                    case "Home":
                        Response.Redirect("HomeInsurancePage.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim());
                        break;
                    case "Motor":
                        Response.Redirect("MotorInsurance.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim());
                        break;

                } 
            }
        }
        protected void lnkbtnRenew_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string policyType = (row.FindControl("lblPolicyType") as Label).Text.Trim();
                string policyno = (row.FindControl("lblPolicyNo") as Label).Text.Trim();

                switch (policyType)
                {
                    case "DomesticHelp":
                        Response.Redirect("DomesticHelp.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim() + "&PolicyNo=" + policyno);
                        break;
                    case "Travel":
                        Response.Redirect("Travelnsurance.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim() + "&PolicyNo=" + policyno);
                        break;
                    case "Home":
                        Response.Redirect("HomeInsurancePage.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim() + "&PolicyNo=" + policyno);
                        break;
                    case "Motor":
                        Response.Redirect("MotorInsurance.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim() + "&PolicyNo=" + policyno);                       
                        break;                       
                }
            }
        }
        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string policyType = (row.FindControl("lblPolicyType") as Label).Text.Trim();
                string policyno = (row.FindControl("lblPolicyNo") as Label).Text.Trim();
                string renewalCount  = (row.FindControl("lblRenewalCount") as Label).Text.Trim();

                switch (policyType)
                {
                    case "DomesticHelp":
                         Response.Redirect("DomesticHelp.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim() + "&PolicyNo=" + policyno + "&RenewalCount=" + renewalCount);                       
                        break;
                    case "Travel":
                        Response.Redirect("Travelnsurance.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim() + "&PolicyNo=" + policyno + "&RenewalCount=" + renewalCount);                        
                        break;
                    case "Home":
                        Response.Redirect("HomeInsurancePage.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim() + "&PolicyNo=" + policyno + "&RenewalCount=" + renewalCount);                      
                        break;
                    case "Motor":                      
                        Response.Redirect("MotorInsurance.aspx?InsuredCode=" + InsuredCode.Value + "&InsuredName=" + InsuredName.Value + "&CPR=" + txtCPRSearch.Text.Trim() + "&PolicyNo=" + policyno + "&RenewalCount=" + renewalCount);                      
                        break;

                }
            }
        }
        protected void Gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var uu = ((Label)e.Row.FindControl("lblPolicyType")).Text;           
                
            }
        }
    }
}