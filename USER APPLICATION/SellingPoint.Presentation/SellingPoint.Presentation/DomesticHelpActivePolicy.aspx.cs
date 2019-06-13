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
    public partial class DomesticHelpActivePolicy : System.Web.UI.Page
    {
        General master; 

        public DomesticHelpActivePolicy()
        {
            master = Master as General;
        }

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
            var userInfo = CommonMethods.GetUserDetails();
            ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));
            ddlAgency.Enabled = false;
        }
        public void loadd()
        {
            var service = CommonMethods.GetLogedInService();

            var fetchdetailsrequest = new AdminFetchDomesticDetailsRequest();
            fetchdetailsrequest.AgencyCode = ddlAgency.SelectedItem.Value.Trim();
            fetchdetailsrequest.DocumentNo = "";
            fetchdetailsrequest.Type = "Active";
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
                string InsuredCode = row.Cells[1].Text.Trim();

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
                    string InsuredCode = HttpUtility.HtmlDecode(row.Cells[2].Text.Trim());
                    string PolicyNo = HttpUtility.HtmlDecode(row.Cells[4].Text.Trim());
                    string CPR = HttpUtility.HtmlDecode(row.Cells[5].Text.Trim());
                    string InsuredName = HttpUtility.HtmlDecode(row.Cells[6].Text.Trim());
                    Response.Redirect("DomesticHelp.aspx?InsuredCode=" + InsuredCode + "&InsuredName=" + InsuredName + "&CPR=" + CPR + "&PolicyNo=" + PolicyNo);


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
            gvDomesticInsurance.PageIndex = e.NewPageIndex;
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

            string type = "DomesticInsurance";
            string HIR = "0";
            string url = BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.FetchDetails.Replace("{HIR}", type);

            url = url.Replace("HIR", HIR);

            var result = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchPolicyDetailsResponse>>(url);

            if (result.StatusCode == 200 && result.Result.IsTransactionDone)
            {
                lbler.Text = "Travel Insurance Period has been Deleted";
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

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string domesticID = (row.FindControl("lblDomesticID") as Label).Text.Trim();
                string InsuredCode = row.Cells[1].Text.Trim();
               
                var service = CommonMethods.GetLogedInService();

                var request = new UpdateHIRStatusRequest();
                request.HIRStatusCode = 9;
                request.InsuranceType = "DomesticInsurance";
                request.ID = Convert.ToInt32(domesticID);

                var approvedresponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.UpdateHIRStatusResponse>, 
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.UpdateHIRStatusRequest>
                                      (BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.UpdateHIRStatus, request);

                if (approvedresponse.Result.IsTransactionDone && approvedresponse.StatusCode == 200)
                {                   
                    loadd();

                }
            }
        }

        protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvDomesticInsurance.Rows)
            {               
                string IsMessage = "true";
                string IsDocuments = "true";
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

        protected void btnDocument_Click(object sender, EventArgs e)
        {           
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string domesticID = (row.FindControl("lblDomesticID") as Label).Text.Trim();
                string InsuredCode = row.Cells[1].Text.Trim();
                string LinkID = (row.FindControl("lblLinkID") as Label).Text.Trim();
                string DocumentNo = row.Cells[2].Text.Trim();
                Response.Redirect("ViewHIRDocuments.aspx?InsuredCode=" + InsuredCode + "&PolicyNo=" + DocumentNo + "&LinkID=" + LinkID);
            }

        }

        protected void btnViewMail_Click(object sender, EventArgs e)
        {            
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string domesticID = (row.FindControl("lblDomesticID") as Label).Text.Trim();
                string InsuredCode = row.Cells[1].Text.Trim();
                string linkId = (row.FindControl("lblLinkID") as Label).Text.Trim();
                string documentId = row.Cells[2].Text.Trim();
                Response.Redirect("ViewInsuranceMessages.aspx?InsuredCode=" +
                    InsuredCode + "&PolicyNumber=" + documentId + "&LinkedId=" + linkId);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {             
                var service = CommonMethods.GetLogedInService();
                var fetchdetailsrequest = new AdminFetchDomesticDetailsRequest
                {
                    DocumentNo = txtSearchKey.Text.Trim(),
                    HIRStatus = 0,
                    Type = "Active",
                    AgencyCode = string.Empty
                };
                if (ddlAgency.SelectedIndex > 0)
                {
                    fetchdetailsrequest.AgencyCode = ddlAgency.SelectedItem.Value;
                }               
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
            catch (Exception ex)
            {
                ////throw ex;
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
        public void SetSearchType(AdminFetchDomesticDetailsRequest req)
        {
            if(!string.IsNullOrEmpty(txtSearchKey.Text.Trim()))
            {
                ddlAgency.SelectedIndex = 0;
                req.ByDocumentNo = true;
                
            }
            if(ddlAgency.SelectedIndex > 0)
            {
                req.ByAgencyCode = true;
            }
        }

        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "DomesticHelp" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gvDomesticInsurance.GridLines = GridLines.Both;
            gvDomesticInsurance.HeaderStyle.Font.Bold = true;
            gvDomesticInsurance.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }
    }
}