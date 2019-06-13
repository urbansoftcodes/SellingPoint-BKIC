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
    public partial class MotorAgeReport : System.Web.UI.Page
    {
        private General master;

        public MotorAgeReport()
        {
            master = Master as General;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;

            try
            {
                if (!Page.IsPostBack)
                {
                    BindAgency();
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

        public void SetDefaultAgency()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();          
            if (userInfo != null)
            {
                ddlAgency.SelectedIndex = ddlAgency.Items.IndexOf(ddlAgency.Items.FindByText(userInfo.Agency));
                ddlAgency.Enabled = false;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
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

        protected void btnSearch_Click(object sender, EventArgs e)
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

        public void Bind()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var reportRequest = new AdminFetchReportRequest
            {
                Agency = ddlAgency.SelectedItem.Text,
                AgentCode = ddlAgency.SelectedItem.Value,
                InsuranceType = "MotorInsurance",
                ReportType = "MotorAgeReport",
                AgeFrom = string.IsNullOrEmpty(txtAgeFrom.Text) ? 0 : Convert.ToInt32(txtAgeFrom.Text),
                AgeTo = string.IsNullOrEmpty(txtAgeTo.Text) ? 0 : Convert.ToInt32(txtAgeTo.Text),
                DateFrom = txtDateFrom.Text.CovertToCustomDateTime(),
                DateTo = txtDateTo.Text.CovertToCustomDateTime()
            };

            var motorAgeResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorReportResponse>,
                                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchReportRequest>
                                                   (BKIC.SellingPoint.DTO.Constants.ReportURI.GetMotorReport, reportRequest);

            if (motorAgeResponse.StatusCode == 200 && motorAgeResponse.Result.IsTransactionDone)
            {
                gvMotorAgeReport.DataSource = motorAgeResponse.Result.MotorReportDetails;
                gvMotorAgeReport.DataBind();
            }
        }

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMotorAgeReport.PageIndex = e.NewPageIndex;
            loadd(); //bindgridview will get the data source and bind it again
        }

        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        protected void gvMotorAgeReport_DataBound(object sender, EventArgs e)
        {
        }

        private void BindAgency()
        {
            master.IsSessionAvailable();           
            var service = CommonMethods.GetLogedInService();

            var dropDownResult = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
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

        protected void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            string FileName = "MotorAgeReport" + DateTime.Now + ".xls";
            Response.AddHeader("content-disposition", "attachment; filename = " + FileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvMotorAgeReport.AllowPaging = false;
                this.Bind();

                gvMotorAgeReport.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvMotorAgeReport.HeaderRow.Cells)
                {
                    cell.BackColor = gvMotorAgeReport.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvMotorAgeReport.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvMotorAgeReport.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvMotorAgeReport.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvMotorAgeReport.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
    }
}