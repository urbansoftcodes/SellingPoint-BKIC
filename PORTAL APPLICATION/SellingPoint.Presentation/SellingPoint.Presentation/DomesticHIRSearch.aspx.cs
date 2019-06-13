using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class DomesticHIRSearch : System.Web.UI.Page
    {
        General master;

        public DomesticHIRSearch()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!IsPostBack)
            {               
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                BindAgency(userInfo, service);
                BindDropDowns(userInfo, service);
                LoadData(userInfo, service);               
            }
        }

        private void BindAgency(OAuthTokenResponse userInfo, DataServiceManager service)
        { 
            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}", 
                                BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.UserMaster));

            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropDownResult.Result.dropdownresult);
                DataTable AgencyDt = dropdownds.Tables["AgentCodeDD"];

                ddlAgency.DataValueField = "AgentCode";
                ddlAgency.DataTextField = "Agency";
                ddlAgency.DataSource = AgencyDt;
                ddlAgency.DataBind();
                ddlAgency.Items.Insert(0, new ListItem("--Please Select--", ""));

                ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));
                ddlAgency.Enabled = false;

            }
        }       

        public void BindDropDowns(OAuthTokenResponse userInfo, DataServiceManager service)
        {           

            var dropdown = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                           (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}",
                           BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.InsurancePortal));

            if (dropdown.StatusCode == 200 && dropdown.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(dropdown.Result.dropdownresult);
                DataTable hirdt = dropdownds.Tables["HIRStatus"];
                if (hirdt.Rows.Count > 0)
                {
                    ddlStatus.DataValueField = "StatusID";
                    ddlStatus.DataTextField = "HIRStatus";
                    ddlStatus.DataSource = hirdt;
                    ddlStatus.DataBind();
                    ddlStatus.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
            }

        }
        public void LoadData(OAuthTokenResponse userInfo, DataServiceManager service)
        {          
            var fetchdetailsrequest = new AdminFetchDomesticDetailsRequest();

            fetchdetailsrequest.DocumentNo = "";
            fetchdetailsrequest.Type = "HIR";
            fetchdetailsrequest.AgencyCode = ddlAgency.SelectedItem.Value.Trim();
            fetchdetailsrequest.All = true;

            var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchDomesticDetailsResponse>, 
                         BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchDomesticDetailsRequest>
                         (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchDomesticPolicyDetails, fetchdetailsrequest);           

            if (result.StatusCode == 200 && result.Result.IsTransactionDone)
            {
                gvDomesticInsurance.DataSource = result.Result.DomesticDetails;
                gvDomesticInsurance.DataBind();
            }
        }

        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string domesticID = (row.FindControl("lblDomesticID") as Label).Text.Trim();
                string InsuredCode = (row.FindControl("lblInsuredCode") as Label).Text.Trim();

                Response.Redirect("DomesticHelpEdit.aspx?Ref=" + domesticID + "&InsuredCode=" + InsuredCode);


            }
        }
        protected void lnkbtnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    string domesticID = (row.FindControl("lblDomesticID") as Label).Text.Trim();
                    string InsuredCode = (row.FindControl("lblInsuredCode") as Label).Text.Trim();

                    //string InsuredCode = row.Cells[1].Text.Trim();
                    string PolicyNo = HttpUtility.HtmlDecode(row.Cells[2].Text.Trim());
                    string CPR = HttpUtility.HtmlDecode(row.Cells[3].Text.Trim());
                    string InsuredName = HttpUtility.HtmlDecode(row.Cells[4].Text.Trim());

                    Response.Redirect("DomesticHelp.aspx?InsuredCode=" + InsuredCode + "&InsuredName=" +
                                   InsuredName + "&CPR=" + CPR  +"&PolicyNo=" + PolicyNo + "&IncludeHIR=" + true);

                }
            }
            catch(Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
           
        }

        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {

                //string a = (row.FindControl("lblveid") as Label).Text.Trim();
                //lblcid.Text = a;
                //lblmessage.Text = "Are you sure want to delete this ?";
                //divThankYou.Visible = true;

            }
        }
        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //bindgridview will get the data source and bind it again           
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            gvDomesticInsurance.PageIndex = e.NewPageIndex;
            LoadData(userInfo, service); 
        }

        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
            //dlist.DefaultView.Sort = e.SortExpression + " " + SortDir(e.SortExpression);
            //gvMotorInsurance.DataSource = dlist;
            //gvMotorInsurance.DataBind();
        }

        private string SortDir(string sField)
        {
            string sDir = "asc";
            string sPrevField = (ViewState["SortField"] != null ? ViewState["SortField"].ToString() : "");
            if (sPrevField == sField)
                sDir = (ViewState["SortDir"].ToString() == "asc" ? "desc" : "asc");
            else
                ViewState["SortField"] = sField;

            ViewState["SortDir"] = sDir;
            return sDir;
        }
        protected void btndelconf_Click(object sender, EventArgs e)
        {

            var service = CommonMethods.GetLogedInService();

            string type = Constants.DomesticHelp;
            string HIR = "0";
            string url = BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.FetchDetails.Replace("{HIR}", type);
            url = url.Replace("HIR", HIR);
            var result = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchPolicyDetailsResponse>>
                        (url);

            if (result.StatusCode == 200)
            {
                if (result.Result.IsTransactionDone)
                {

                    lbler.Text = "Travel Insurance Period has been Deleted";
                }
                else
                {
                    lbler.Text = result.Result.TransactionErrorMessage;
                }

            }
            else
            {
                lbler.Text = result.ErrorMessage;
            }

           // loadd();

            lbler.ForeColor = System.Drawing.Color.Maroon;
            lbler.Text = "Travel Insurance Period has been deleted";           
        }
        protected void btndelcan_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    string domesticID = (row.FindControl("lblDomesticID") as Label).Text.Trim();
                    string InsuredCode = (row.FindControl("lblInsuredCode") as Label).Text.Trim();
                    string documentId = row.Cells[2].Text.Trim();
                    string linkID = (row.FindControl("lblLinkID") as Label).Text.Trim();

                 
                    var service = CommonMethods.GetLogedInService();

                    var request = new UpdateHIRStatusRequest
                    {
                        HIRStatusCode = Constants.ApproveHIRStatus,
                        InsuranceType = Constants.DomesticHelp,
                        ID = Convert.ToInt32(domesticID),
                        DocumentNo = documentId,
                        InsuredCode = InsuredCode,
                        LinkId = linkID
                    };


                    var approvedresponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                            <BKIC.SellingPoint.DTO.RequestResponseWrappers.UpdateHIRStatusResponse>, 
                                            BKIC.SellingPoint.DTO.RequestResponseWrappers.UpdateHIRStatusRequest>
                                           (BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.UpdateHIRStatus, request);

                    if (approvedresponse.Result.IsTransactionDone && approvedresponse.StatusCode == 200)
                    {                      
                        Response.Redirect(Request.RawUrl);

                    }
                }
            }
            catch(Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }           
        }  
        protected void btnRejected_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    string domesticID = (row.FindControl("lblDomesticID") as Label).Text.Trim();
                    string InsuredCode = (row.FindControl("lblInsuredCode") as Label).Text.Trim();
                    string linkId = (row.FindControl("lblLinkID") as Label).Text.Trim();
                    string documentId = row.Cells[2].Text.Trim();

                   
                    var service = CommonMethods.GetLogedInService();

                    var request = new UpdateHIRStatusRequest();
                    request.HIRStatusCode = 2;
                    request.InsuranceType = Constants.DomesticHelp;
                    request.ID = Convert.ToInt32(domesticID);
                    request.DocumentNo = documentId;
                    request.InsuredCode = InsuredCode;
                    request.LinkId = linkId;

                    var approvedresponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.UpdateHIRStatusResponse>,
                                            BKIC.SellingPoint.DTO.RequestResponseWrappers.UpdateHIRStatusRequest>
                                            (BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.UpdateHIRStatus, request);

                    if (approvedresponse.Result.IsTransactionDone && approvedresponse.StatusCode == 200)
                    {                       
                        Response.Redirect(Request.RawUrl);
                    }

                }
            }
            catch(Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                master.ShowLoading = false;
            }
        }

        protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
        {

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();           

            foreach (GridViewRow row in gvDomesticInsurance.Rows)
            {
                string HIRStatusCode = (row.FindControl("lblHIRStatusCode") as Label).Text.Trim();
                string IsMessage = (row.FindControl("lblIsMessage") as Label).Text.Trim();
                string IsDocuments = (row.FindControl("IsDocument") as Label).Text.Trim();

                if (userInfo.Roles == "BranchAdmin" || userInfo.Roles == "User")
                {
                    var btnapproved = row.FindControl("btnApproved") as LinkButton;
                    btnapproved.Visible = false;
                    var btnRejected = row.FindControl("btnRejected") as LinkButton;
                    btnRejected.Visible = false;
                    var btnActivate = row.FindControl("btnActivate") as LinkButton;
                    btnActivate.Visible = false;                    
                }
                else
                {
                    if (HIRStatusCode == "1")
                    {
                        var btnapproved = row.FindControl("btnApproved") as LinkButton;
                        btnapproved.Visible = true;
                        var btnRejected = row.FindControl("btnRejected") as LinkButton;
                        btnRejected.Visible = true;

                    }
                    if (HIRStatusCode == "2")
                    {
                        var btnapproved = row.FindControl("btnApproved") as LinkButton;
                        btnapproved.Visible = false;
                        var btnRejected = row.FindControl("btnRejected") as LinkButton;
                        btnRejected.Visible = false;
                        var btnActivate = row.FindControl("btnActivate") as LinkButton;
                        btnActivate.Visible = true;

                    }
                    if (HIRStatusCode == "8")
                    {
                        var btnapproved = row.FindControl("btnApproved") as LinkButton;
                        btnapproved.Visible = false;
                        var btnRejected = row.FindControl("btnRejected") as LinkButton;
                        btnRejected.Visible = false;
                        var btnActivate = row.FindControl("btnActivate") as LinkButton;
                        btnActivate.Visible = false;

                    }
                }
               


            }
        }     
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var fetchdetailsrequest = new AdminFetchDomesticDetailsRequest
                {
                    DocumentNo = txtSearchKey.Text.Trim(),
                    Type = "HIR",
                    AgencyCode = ddlAgency.SelectedItem.Value,
                    HIRStatus = string.IsNullOrEmpty(ddlStatus.SelectedItem.Value) ? 0 : Convert.ToInt32(ddlStatus.SelectedItem.Value)
                };

                if (string.IsNullOrEmpty(fetchdetailsrequest.DocumentNo) && string.IsNullOrEmpty(fetchdetailsrequest.AgencyCode) && fetchdetailsrequest.HIRStatus == 0)
                {
                    LoadData(userInfo, service);
                }
                else
                {                    
                    var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchDomesticDetailsResponse>,
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchDomesticDetailsRequest>
                                 (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchDomesticPolicyDetails, fetchdetailsrequest);

                    if (result.StatusCode == 200 && result.Result.IsTransactionDone)
                    {
                        gvDomesticInsurance.DataSource = result.Result.DomesticDetails;
                        gvDomesticInsurance.DataBind();

                    }
                }
            }
            catch (Exception ex)
            {
                ////throw ex;
            }
            finally
            {
                 master.ShowLoading = false;
            }
        }
    }
}