using BKIC.SellingPoint.Presentation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKIC.SellingPoint.Portal
{
    public partial class ViewInsuranceMessage : System.Web.UI.Page
    {
        General master;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                string userInsuredCode = !string.IsNullOrEmpty(Request.QueryString["InsuredCode"]) ? Request.QueryString["InsuredCode"] : "";
                string policyDocumentId = !string.IsNullOrEmpty(Request.QueryString["PolicyNumber"]) ? Request.QueryString["PolicyNumber"] : "";
                string linkedId = !string.IsNullOrEmpty(Request.QueryString["LinkedId"]) ? Request.QueryString["LinkedId"] : "";

                DataTable messages = new DataTable();
                messages.Columns.Add("Message Key");
                messages.Columns.Add("Message");
                messages.Columns.Add("Send Date");

                if (!string.IsNullOrEmpty(userInsuredCode) && !string.IsNullOrEmpty(policyDocumentId) && !string.IsNullOrEmpty(linkedId))
                {
                    var result = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                 <BKIC.SellingPoint.DTO.RequestResponseWrappers.EmailMessageAuditResult>>
                                 (BKIC.SellingPoint.DTO.Constants.InsurancePortalURI.GetEmailMessageForRecord.Replace("{insuredCode}", userInsuredCode)
                                 .Replace("{policyNo}", policyDocumentId).Replace("{linkId}", linkedId));

                    if (result.StatusCode == 200 && result.Result != null && result.Result.IsTransactionDone)
                    {
                        result.Result.EmailMessage.ForEach(x => {
                            messages.Rows.Add(x.MessageKey, x.Message, x.CreatedDate.Value.ToString("MMM-dd-yyyy hh:mm:ss tt"));
                        });
                    }

                    rtInsuranceMessages.DataSource = messages;
                    rtInsuranceMessages.DataBind();
                }

            }
        }
    }
}