using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BKIC.SellingPoint.Presentation;
using BKIC.SellingPoint.DTO.RequestResponseWrappers;


namespace BKIC.SellingPoint.Presentation
{

    public partial class ViewHIRDocuments : System.Web.UI.Page
    {
        General master;
        public ViewHIRDocuments()
        {
            master = Master as General;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;

            if (Request.QueryString["InsuredCode"] != null && Request.QueryString["PolicyNo"] != null && Request.QueryString["LinkID"] != null)
            {
                string InsuredCode = Request.QueryString["InsuredCode"];
                string PolicyNo = Request.QueryString["PolicyNo"];
                string LinkID = Request.QueryString["LinkID"];
                BindDocuments(InsuredCode, PolicyNo, LinkID);
            }
        }

        public void BindDocuments(string insuredCode, string policyNo, string linkId)
        {
            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var docrequest = new FetchDocumentsRequest
            {
                InsuredCode = insuredCode,
                DocumentNo = policyNo,
                LinkID = linkId
            };
            var response = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                           <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDocumentsResponse>,
                           BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDocumentsRequest>
                           (BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.FetchDocuments, docrequest);
            if (response.StatusCode == 200 && response.Result.IsTransactionDone)
            {
                DataTable docdt = new DataTable();
                docdt.Columns.Add("FileName");
                docdt.Columns.Add("FileURL");
                docdt.Columns.Add("CreatedDate");

                foreach (var list in response.Result.FilesDocuments)
                {
                    docdt.Rows.Add(list.FileName, ClientUtility.WebApiUri + list.FileURL, list.CreatedDate.ToString("MMM-dd-yyyy hh:mm:ss tt"));
                }

                rptHIRDocuments.DataSource = docdt;
                rptHIRDocuments.DataBind();
            }


        }
    }
}