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
    public partial class MotorBranchReport : System.Web.UI.Page
    {
        private General master;

        public MotorBranchReport()
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
                    BindAgencyBranch();
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

        protected void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            string FileName = "MotorBranchReport" + DateTime.Now + ".xls";
            Response.AddHeader("content-disposition", "attachment; filename = " + FileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvMotorBranchReport.AllowPaging = false;
                this.Bind();

                gvMotorBranchReport.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvMotorBranchReport.HeaderRow.Cells)
                {
                    cell.BackColor = gvMotorBranchReport.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvMotorBranchReport.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvMotorBranchReport.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvMotorBranchReport.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvMotorBranchReport.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        private void BindAgencyBranch()
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var result = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                         <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>
                         (BKIC.SellingPont.DTO.Constants.DropdownURI.GetPageDropDowns.Replace("{type}", 
                         BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.Reports));

            if (result.StatusCode == 200 && result.Result.IsTransactionDone)
            {
                DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(result.Result.dropdownresult);

                DataTable Agency = dropdownds.Tables["AgentCodeDD"];
                DataTable branches = dropdownds.Tables["AgentBranchDD"];

                if (Agency != null && Agency.Rows.Count > 0)
                {
                    ddlAgency.DataValueField = "AgentCode";
                    ddlAgency.DataTextField = "Agency";
                    ddlAgency.DataSource = Agency;
                    ddlAgency.DataBind();
                    ddlAgency.Items.Insert(0, new ListItem("--Please Select--", ""));
                    SetDefaultAgency();
                }
                if (branches != null && branches.Rows.Count > 0)
                {
                    ddlBranch.DataValueField = "AGENTBRANCH";
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataSource = branches.AsEnumerable()
                                            .Where(row => row.Field<string>("Agency") == userInfo.Agency)
                                            .CopyToDataTable();
                    ddlBranch.DataBind();
                    ddlBranch.Items.Insert(0, new ListItem("--Please Select--", ""));
                }
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

        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMotorBranchReport.PageIndex = e.NewPageIndex;
            loadd(); //bindgridview will get the data source and bind it again
        }

        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
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
                Agency = ddlAgency.SelectedItem.Text,
                AgentCode = ddlAgency.SelectedItem.Value,
                BranchCode = ddlBranch.SelectedItem.Value,
                DateFrom = txtDateFrom.Text.CovertToCustomDateTime(),
                DateTo = txtDateTo.Text.CovertToCustomDateTime(),
                InsuranceType = "MotorInsurance",
                ReportType = "MotorBranchReport"
            };

            var motorBranchResponse = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorReportResponse>,
                                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.AdminFetchReportRequest>
                                                   (BKIC.SellingPoint.DTO.Constants.ReportURI.GetMotorReport, reportRequest);


            if (motorBranchResponse.StatusCode == 200 && motorBranchResponse.Result.IsTransactionDone)
            {
                gvMotorBranchReport.DataSource = motorBranchResponse.Result.MotorReportDetails;
                gvMotorBranchReport.DataBind();
            }
        }
    }
}