using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using BKIC.SellingPoint.Presentation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class TravelUserReport : System.Web.UI.Page
    {
        private General master;

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            try
            {
                if (!Page.IsPostBack)
                {
                    BindAgency();
                    LoadUsers();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            master = Master as General;
            try
            {
                this.Bind();
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

        public void loadd()
        {
            try
            {
                this.Bind();
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

        private void BindAgency()
        {
            var client = new BKIC.SellingPoint.Presentation.ClientUtility();
            client.serviceManger = new KBIC.Utility.DataServiceManager(BKIC.SellingPoint.Presentation.ClientUtility.WebApiUri, "", false);

            var dropDownResult = client.serviceManger.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                                 (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}",
                                 BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.Reports));

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

        public void LoadUsers()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster
            {
                Type = "fetch",
                CreatedDate = DateTime.Now
            };

            var userResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                            <BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMasterDetailsResponse>,
                            BKIC.SellingPoint.DTO.RequestResponseWrappers.UserMaster>
                            (BKIC.SellingPoint.DTO.Constants.AdminURI.UserOperation, details);

            if (userResult.Result.IsTransactionDone && userResult.StatusCode == 200)
            {
                ddlUsers.DataValueField = "ID";
                ddlUsers.DataTextField = "UserName";
                ddlUsers.DataSource = userResult.Result.UserMaster.AsEnumerable()
                                      .Where(row => row.Agency == userInfo.Agency);
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, new ListItem("--Please Select--", ""));
            }
        }

        public void SetDefaultAgency()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();           
            ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));
            ddlAgency.Enabled = false;
        }

        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTravelUserReport.PageIndex = e.NewPageIndex;
            loadd(); //bindgridview will get the data source and bind it again
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportGridToExcel();
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

        //private void ExportGridToExcel()
        //{
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.Charset = "";
        //    string FileName = "TravelUserReport" + DateTime.Now + ".xls";
        //    System.IO.StringWriter strwritter = new StringWriter();
        //    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
        //    gvTravelUserReport.GridLines = GridLines.Both;
        //    gvTravelUserReport.HeaderStyle.Font.Bold = true;
        //    gvTravelUserReport.RenderControl(htmltextwrtter);
        //    Response.Write(strwritter.ToString());
        //    Response.End();
        //}

        protected void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            string FileName = "TravelUserReport" + DateTime.Now + ".xls";
            Response.AddHeader("content-disposition", "attachment; filename = " + FileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvTravelUserReport.AllowPaging = false;
                this.Bind();

                gvTravelUserReport.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvTravelUserReport.HeaderRow.Cells)
                {
                    cell.BackColor = gvTravelUserReport.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvTravelUserReport.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvTravelUserReport.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvTravelUserReport.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvTravelUserReport.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
        public void Bind()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var reportRequest = new AdminFetchReportRequest
            {
                InsuranceType = "TravelInsurance",
                ReportType = "TravelUserReport",
                Agency = ddlAgency.SelectedItem.Text,
                AgentCode = ddlAgency.SelectedItem.Value,
                DateFrom = txtDateFrom.Text.CovertToCustomDateTime(),
                DateTo = txtDateTo.Text.CovertToCustomDateTime(),
                AuthorizedUserID = Convert.ToInt32(ddlUsers.SelectedItem.Value)
            };

            var travelUserResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.TravelHomeReportResponse>,
                                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchReportRequest>
                                                   (BKIC.SellingPoint.DTO.Constants.ReportURI.GetTravelReport, reportRequest);

            if (travelUserResponse.StatusCode == 200 && travelUserResponse.Result.IsTransactionDone)
            {
                gvTravelUserReport.DataSource = travelUserResponse.Result.TravelHomeReportDetails;
                gvTravelUserReport.DataBind();
            }
        }
    }
}