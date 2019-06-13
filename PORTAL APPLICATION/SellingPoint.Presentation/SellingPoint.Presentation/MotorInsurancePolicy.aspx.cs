using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class MotorInsurancePolicy : System.Web.UI.Page
    {
        General master;

        public MotorInsurancePolicy()
        {
            master = Master as General;
        }

       // CommonMethods methods = new CommonMethods();
        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!IsPostBack)
            {                
                BindAgency();
                loadd();               
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        private void BindAgency()
        {
            master.IsSessionAvailable();          
            var service = CommonMethods.GetLogedInService();


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
                SetDefaultAgency();

            }
        }

        public void SetDefaultAgency()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();           
            ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));
            ddlAgency.Enabled = false;            
        }   
    
        public void loadd()
        {
            master.IsSessionAvailable();         
            var service = CommonMethods.GetLogedInService();

            var fetchdetailsrequest = new AdminFetchMotorDetailsRequest
            {
                DocumentNo = "",
                Type = "Active",
                AgencyCode = ddlAgency.SelectedItem.Value,
                All = true
            };

            var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchMotorDetailsResponse>, 
                         BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchMotorDetailsRequest>
                        (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchMotorPolicyDetails, fetchdetailsrequest);           

            if (result.StatusCode == 200 && result.Result.IsTransactionDone && result.Result.IsTransactionDone)
            {
                gvMotorInsurance.DataSource = result.Result.MotorDetails;
                gvMotorInsurance.DataBind();
            }           
        }

        protected void lnkbtnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    string motorID = (row.FindControl("lblMotorID") as Label).Text.Trim();
                    string InsuredCode = HttpUtility.HtmlDecode(row.Cells[1].Text.Trim());
                    string PolicyNo = HttpUtility.HtmlDecode(row.Cells[2].Text.Trim());
                    string CPR = HttpUtility.HtmlDecode(row.Cells[3].Text.Trim());
                    string InsuredName = HttpUtility.HtmlDecode(row.Cells[4].Text.Trim());
                    Response.Redirect("MotorInsurance.aspx?InsuredCode=" + InsuredCode + "&InsuredName=" + InsuredName + "&CPR=" + CPR + "&PolicyNo=" + PolicyNo);
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

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMotorInsurance.PageIndex = e.NewPageIndex;
            loadd(); //bindgridview will get the data source and bind it again
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

            master.IsSessionAvailable();           
            var service = CommonMethods.GetLogedInService();

            string type = "MotorInsurance";
            string HIR = "0";
            string url = BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.FetchDetails.Replace("{HIR}", type);
            url = url.Replace("HIR", HIR);
            var result = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchMotorDetailsResponse>>(url);

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

            loadd();

            lbler.ForeColor = System.Drawing.Color.Maroon;
            lbler.Text = "Travel Insurance Period has been deleted";
           

        }
        protected void btndelcan_Click(object sender, EventArgs e)
        {
            
        }


        protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvMotorInsurance.Rows)
            {               
                string IsMessage = (row.FindControl("lblIsMessage") as Label).Text.Trim();
                string IsDocuments = (row.FindControl("IsDocument") as Label).Text.Trim();

                if (IsDocuments == "True")
                {
                    var btnHIRFiles = row.FindControl("btnDocument") as LinkButton;
                    btnHIRFiles.Visible = true;
                }
                else
                {
                    var btnHIRFiles = row.FindControl("btnDocument") as LinkButton;
                    btnHIRFiles.Visible = false;
                }

                if (IsMessage == "True")
                {
                    var btnDocFiles = row.FindControl("btnViewMail") as LinkButton;
                    btnDocFiles.Visible = true;
                }
                else
                {
                    var btnDocFiles = row.FindControl("btnViewMail") as LinkButton;
                    btnDocFiles.Visible = false;
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                master.IsSessionAvailable();               
                var service = CommonMethods.GetLogedInService();

                var fetchdetailsrequest = new AdminFetchMotorDetailsRequest
                {
                    DocumentNo = txtSearchKey.Text.Trim(),
                    HIRStatus = 0,
                    Type = "Active",
                    AgencyCode = ""
                };
                if (ddlAgency.SelectedIndex > 0)
                {
                    fetchdetailsrequest.AgencyCode = ddlAgency.SelectedItem.Value;
                }           

                var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchMotorDetailsResponse>,
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchMotorDetailsRequest>
                            (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchMotorPolicyDetails, fetchdetailsrequest);
                if (result.StatusCode == 200 && result.Result.IsTransactionDone)
                {
                    gvMotorInsurance.DataSource = result.Result.MotorDetails;
                    gvMotorInsurance.DataBind();

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
        protected void txtSearch_Changed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchKey.Text))
            {
                ddlAgency.SelectedIndex = 0;
                ddlAgency.Enabled = false;
               
            }
            else
            {
                ddlAgency.Enabled = true;
               
            }
        }
        public void SetSearchType(AdminFetchMotorDetailsRequest req)
        {
            if (!string.IsNullOrEmpty(txtSearchKey.Text.Trim()))
            {
                ddlAgency.SelectedIndex = 0;
                req.ByDocumentNo = true;

            }
            if (ddlAgency.SelectedIndex > 0)
            {
                req.ByAgencyCode = true;
            }
        }

        protected void btnDocument_Click(object sender, EventArgs e)
        { 
            master.IsSessionAvailable();
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string domesticID = (row.FindControl("lblMotorID") as Label).Text.Trim();
                string InsuredCode = row.Cells[1].Text.Trim();
                string LinkID = (row.FindControl("lblLinkID") as Label).Text.Trim();
                string DocumentNo = row.Cells[2].Text.Trim();
                Response.Redirect("ViewHIRDocuments.aspx?InsuredCode=" + InsuredCode + "&PolicyNo=" + DocumentNo + "&LinkID=" + LinkID);
            }

        }

        protected void btnViewMail_Click(object sender, EventArgs e)
        {            
            master.IsSessionAvailable();
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string motorID = (row.FindControl("lblMotorID") as Label).Text.Trim();
                string InsuredCode = row.Cells[1].Text.Trim();
                string linkId = (row.FindControl("lblLinkID") as Label).Text.Trim();
                string documentId = row.Cells[2].Text.Trim();
                Response.Redirect("ViewInsuranceMessages.aspx?InsuredCode=" +
                    InsuredCode + "&PolicyNumber=" + documentId + "&LinkedId=" + linkId);
            }
        }
    }
}