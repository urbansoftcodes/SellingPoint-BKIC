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
    public partial class HomeInsuranceActivePolicy : System.Web.UI.Page
    {
        General master;

        public HomeInsuranceActivePolicy()
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
            var client = new BKIC.SellingPoint.Presentation.ClientUtility();
            client.serviceManger = new KBIC.Utility.DataServiceManager(BKIC.SellingPoint.Presentation.ClientUtility.WebApiUri, "", false);


            var dropDownResult = client.serviceManger.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper<BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                  (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.UserMaster));


            if (dropDownResult.StatusCode == 200 && dropDownResult.Result.IsTransactionDone == true)
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
            if (userInfo == null)
            {
                Response.Redirect("Login.aspx");
            }
            ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));
            ddlAgency.Enabled = false;
        }
        public void loadd()
        {
            master.IsSessionAvailable();
            var service = CommonMethods.GetLogedInService();

            var fetchdetailsrequest = new AdminFetchHomeDetailsRequest
            {
                DocumentNo = "",
                Type = "Active",
                AgencyCode = ddlAgency.SelectedItem.Value,
                All = true
            };

            var result = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchHomeDetailsResponse>, 
                         BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchHomeDetailsRequest>
                         (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchHomePolicyDetails, fetchdetailsrequest);      

            if (result.StatusCode == 200 && result.Result.IsTransactionDone)
            {
                gvHomeInsurance.DataSource = result.Result.HomeDetails;
                gvHomeInsurance.DataBind();
            }          
        }

        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string motorID = (row.FindControl("lblHomeID") as Label).Text.Trim();
                string InsuredCode = row.Cells[1].Text.Trim();

                Response.Redirect("HomeInsuranceEdit.aspx?Ref=" + motorID + "&InsuredCode=" + InsuredCode);


            }
        }
        protected void lnkbtnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
                {
                    string homeID = (row.FindControl("lblHomeID") as Label).Text.Trim();
                    string InsuredCode = HttpUtility.HtmlDecode(row.Cells[1].Text.Trim());
                    string PolicyNo = HttpUtility.HtmlDecode(row.Cells[2].Text.Trim());
                    string CPR = HttpUtility.HtmlDecode(row.Cells[3].Text.Trim());
                    string InsuredName = HttpUtility.HtmlDecode(row.Cells[4].Text.Trim());
                    Response.Redirect("HomeInsurancePage.aspx?InsuredCode=" + InsuredCode + "&InsuredName=" + InsuredName + "&CPR=" + CPR + "&PolicyNo=" + PolicyNo);
                }
            }
            catch(Exception ex)
            {

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
            gvHomeInsurance.PageIndex = e.NewPageIndex;
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

            string type = "HomeInsurance";
            string HIR = "0";
            string url = BKIC.SellingPoint.DTO.Constants.AdminURI.FetchHomePolicyDetails.Replace("{HIR}", type);
            url = url.Replace("HIR", HIR);
            var result = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchHomeDetailsResponse>>(url);

            if (result.StatusCode == 200 && result.Result.IsTransactionDone)
            {
                lbler.Text = "Home Insurance Period has been Deleted";
                lbler.Text = result.Result.TransactionErrorMessage;
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


        protected void gvHomeInsurance_DataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvHomeInsurance.Rows)
            {
                string HIRStatusCode = (row.FindControl("lblHIRStatusCode") as Label).Text.Trim();
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

        protected void btnDocument_Click(object sender, EventArgs e)
        {           

            master.IsSessionAvailable();
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                string homeID = (row.FindControl("lblHomeID") as Label).Text.Trim();
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
                string homeID = (row.FindControl("lblHomeID") as Label).Text.Trim();
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
                master.IsSessionAvailable();               
                var service = CommonMethods.GetLogedInService();

                var fetchdetailsrequest = new AdminFetchHomeDetailsRequest
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
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchHomeDetailsResponse>, 
                             BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchHomeDetailsRequest>
                             (BKIC.SellingPoint.DTO.Constants.AdminURI.FetchHomePolicyDetails, fetchdetailsrequest);
                if (result.StatusCode == 200 && result.Result.IsTransactionDone)
                {
                    gvHomeInsurance.DataSource = result.Result.HomeDetails;
                    gvHomeInsurance.DataBind();

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
        public void SetSearchType(AdminFetchHomeDetailsRequest req)
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportGridToExcel();
        }

        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "HomePolicy" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            gvHomeInsurance.GridLines = GridLines.Both;
            gvHomeInsurance.HeaderStyle.Font.Bold = true;
            gvHomeInsurance.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();

        }
    }
}