using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Presentation
{
    public partial class MotorCoverMaster : System.Web.UI.Page
    {
        General master;
        public MotorCoverMaster()
        {
            master = Master as General;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                btnSubmit.Text = "Save";
                LoadMotorMasterData();
                ClearControl();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string opertaion = string.Empty;

                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCoverMaster();

                details.CoversDescription = txtCoverDescription.Text.ToString();
                details.CoversCode = txtCoverCode.Text.ToString();

                opertaion = (sender as Button).Text;

                if (opertaion == "Update")
                {
                    details.Type = "edit";
                }
                else
                {
                    details.Type = "insert";

                }

                var results = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                            <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCoverMasterResponse>,
                            BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCoverMaster>
                            (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorCoverMasterOperation, details);

                if (results.StatusCode == 200 && results.Result.IsTransactionDone)
                {
                    LoadMotorMasterData();
                    ClearControl();
                    btnSubmit.Text = "Save";
                }
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "Save";
            ClearControl();
        }

        private void ClearControl()
        {
            txtCoverDescription.Text = string.Empty;
        }
        public void LoadMotorMasterData()
        {
            master.IsSessionAvailable();
            var service = CommonMethods.GetLogedInService();

            var response = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                            <BKIC.SellingPoint.DTO.RequestResponseWrappers.MasterTableResult<BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorCoverMaster>>>
                           (BKIC.SellingPoint.DTO.Constants.AdminURI.GetMasterTableByTableName.Replace("{tableName}",
                           BKIC.SellingPoint.DTO.RequestResponseWrappers.MasterTable.MotorCoverMaster));

            if (response.StatusCode == 200)
            {
                if (response.Result.IsTransactionDone)
                {
                    gvMotorMaster.DataSource = response.Result.TableRows;
                    gvMotorMaster.DataBind();
                }
            }
            else
            {
                // lbler.Text = result.ErrorMessage;
            }
        }

        #region Test_1
        protected void gv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMotorMaster.PageIndex = e.NewPageIndex;
            LoadMotorMasterData(); //bindgridview will get the data source and bind it again
        }
        protected void gvMotorInsurance_DataBound(object sender, EventArgs e)
        {
            
        }
        protected void lnkbtnEdit_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                if (ViewState["MoterMasterId"] != null)
                {
                    ViewState["MoterMasterId"] = string.Empty;
                }
                string id = row.Cells[1].Text.Trim();
                ViewState["MoterMasterId"] = id;
                txtCoverCode.Text = row.Cells[0].Text.Trim();
                txtCoverDescription.Text = row.Cells[3].Text.Trim();
                btnSubmit.Text = "Update";
            }
        }
        protected void lnkbtnDelete_Click(object sender, EventArgs e)
        {
            using (GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent)
            {
                master.IsSessionAvailable();

                var service = CommonMethods.GetLogedInService();

                int id = Convert.ToInt32(row.Cells[1].Text.Trim());

                var details = new BKIC.SellingPoint.DTO.RequestResponseWrappers.AgentMaster
                {
                    Id = id,
                    Type = "delete"
                };

                var branchResult = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.AgentMasterResponse>,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.AgentMaster>
                                   (BKIC.SellingPoint.DTO.Constants.AdminURI.AgentOperation, details);
                if (branchResult.StatusCode == 200)
                {
                    LoadMotorMasterData();
                }
            }
        }
        protected void gv_Sorting(object sender, GridViewSortEventArgs e)
        {
            //dlist.DefaultView.Sort = e.SortExpression + " " + SortDir(e.SortExpression);
            //gvMotorInsurance.DataSource = dlist;
            //gvMotorInsurance.DataBind();
        }

        #endregion
    }
}