using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using BKIC.SellingPoint.Presentation;
using KBIC.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SellingPoint.Presentation
{
    public partial class MotorProductMaster : System.Web.UI.Page
    {
        General master;
        public static string MainClass { get; set; }
        public MotorProductMaster()
        {
            master = Master as General;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            master = Master as General;
            if (!Page.IsPostBack)
            {
                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                LoadMotorProductData(userInfo, service);
                btnSubmit.Text = "Save";
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string opertaion = string.Empty;

                master.IsSessionAvailable();
                var userInfo = CommonMethods.GetUserDetails();
                var service = CommonMethods.GetLogedInService();

                var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductMaster
                {
                    Agency = userInfo.Agency,
                    AgentCode = userInfo.AgentCode,
                    MainClass = txtMainClass.Text.Trim(),
                    SubClass = txtSubClass.Text.Trim(),

                    AgeLoadingPercent = string.IsNullOrEmpty(txtAgeLoadingPercent.Text) ? decimal.Zero :
                                            Convert.ToDecimal(txtAgeLoadingPercent.Text),

                    ExcessAmount = string.IsNullOrEmpty(txtExcessAmount.Text) ? decimal.Zero :
                                           Convert.ToDecimal(txtExcessAmount.Text),

                    GCCCoverRangeInYears = string.IsNullOrEmpty(txtGCCCoverRangeInYears.Text) ? 0 :
                                           Convert.ToInt32(txtGCCCoverRangeInYears.Text),

                    LastSeries = string.IsNullOrEmpty(txtLastSeries.Text) ? 0 :
                                           Convert.ToInt64(txtLastSeries.Text),

                    MaximumVehicleAge = string.IsNullOrEmpty(txtMaximumVehicleAge.Text) ? 0 :
                                            Convert.ToInt32(txtMaximumVehicleAge.Text),

                    MaximumVehicleValue = string.IsNullOrEmpty(txtMaximumVehicleValue.Text) ? 0 :
                                           Convert.ToDecimal(txtMaximumVehicleValue.Text),

                    MinimumPremium = string.IsNullOrEmpty(txtMinimumPremium.Text) ? 0 :
                                           Convert.ToDecimal(txtMinimumPremium.Text),

                    Rate = string.IsNullOrEmpty(txtRate.Text) ? decimal.Zero :
                                           Convert.ToDecimal(txtRate.Text),

                    SeriesFormatLength = string.IsNullOrEmpty(txtSeriesFormatLength.Text) ? 0 :
                                           Convert.ToInt32(txtSeriesFormatLength.Text),

                    UnderAge = string.IsNullOrEmpty(txtUnderAge.Text) ? 0 :
                                           Convert.ToInt32(txtUnderAge.Text),

                    UnderAgeExcessAmount = string.IsNullOrEmpty(txtUnderAgeExcessAmount.Text) ? 0 :
                                           Convert.ToDecimal(txtUnderAgeExcessAmount.Text),

                    UnderAgeminPremium = string.IsNullOrEmpty(txtUnderAgeMinimumPremium.Text) ? 0 :
                                           Convert.ToDecimal(txtUnderAgeMinimumPremium.Text),

                    GulfAssitAmount = string.IsNullOrEmpty(txtGulfAssitAmount.Text) ? 0 :
                                          Convert.ToDecimal(txtGulfAssitAmount.Text),

                    UnderAgeToHIR = chkUnderAgeToHIR.Checked,
                    HasAdditionalDays = chkHasAdditionalDays.Checked,
                    HasAgeLoading = chkHasAgeLoading.Checked,
                    HasGCC = chkHasGCC.Checked,
                    AllowUnderAge = chkAllowUnderAge.Checked,
                    AllowMaxVehicleAge = chkAllowMaximumVehicleAge.Checked,
                    Description = txtDescription.Text.Trim(),
                    PolicyCode = txtPolicyCode.Text.Trim(),
                    AllowUsedVehicle = chkAllowUsedVehicle.Checked
                };

                opertaion = (sender as Button).Text;
                if (opertaion == "Update")
                {
                   // request.ID = Convert.ToInt32(ViewState["Id"].ToString());
                    request.Type = "edit";
                }
                else
                {
                    request.Type = "insert";
                }

                var motorProduct = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                       <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductMasterResponse>,
                                       BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductMaster>
                                       (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorProductOperation, request);

                if (motorProduct != null && motorProduct.StatusCode == 200 && motorProduct.Result.IsTransactionDone)
                {
                    ClearProduct();
                    if (request.Type == "insert")
                    {
                        master.ShowErrorPopup("Motor product added successfully", "Motor Product");
                    }
                    else if(request.Type == "edit")
                    {
                        master.ShowErrorPopup("Motor product updated successfully", "Motor Product");
                    }
                   
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "Save";
            Response.Redirect("HomePage.aspx");

        }

        public void LoadMotorProductData(OAuthTokenResponse userInfo, DataServiceManager service)
        {
            var productCode = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                             <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchProductCodeResponse>>(
                             BKIC.SellingPoint.DTO.Constants.DropDownURI.GetInsuranceProductCode
                             .Replace("{agency}", userInfo.Agency)
                             .Replace("{agencyCode}", userInfo.AgentCode)
                             .Replace("{insurancetypeid}", "4"));

            MainClass = productCode.Result.productCode;

            if (productCode != null && productCode.StatusCode == 200 && productCode.Result.IsTransactionDone)
            {
                var products = service.GetData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                              <BKIC.SellingPoint.DTO.RequestResponseWrappers.FetchDropDownsResponse>>(
                              BKIC.SellingPoint.DTO.Constants.DropDownURI.GetAgencyProducts
                              .Replace("{agency}", userInfo.Agency)
                              .Replace("{agencyCode}", userInfo.AgentCode)
                              .Replace("{mainclass}", productCode.Result.productCode)
                              .Replace("{page}", BKIC.SellingPoint.DTO.RequestResponseWrappers.PageType.MotorInsurance));


                if (products != null && products.StatusCode == 200 && products.Result.IsTransactionDone)
                {
                    DataSet dropdownds = JsonConvert.DeserializeObject<DataSet>(products.Result.dropdownresult);
                    DataTable prods = dropdownds.Tables["Products"];
                    ddlCover.DataValueField = "SUBCLASS";
                    ddlCover.DataTextField = "DESCRIPTION";
                    ddlCover.DataSource = prods;
                    ddlCover.DataBind();
                    ddlCover.Items.Insert(0, new ListItem("Add New", ""));
                }
            }

        }
        protected void MotorProduct_changed(object sender, EventArgs e)
        {
            try
            {
                if(ddlCover.SelectedIndex > 0)
                {
                     LoadMotorProduct();
                }
                else
                {
                    ClearProduct();
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
        public void LoadMotorProduct()
        {
            master = Master as General;

            master.IsSessionAvailable();
            var userInfo = CommonMethods.GetUserDetails();
            var service = CommonMethods.GetLogedInService();

            var request = new BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductMaster
            {
                Agency = userInfo.Agency,
                AgentCode = userInfo.AgentCode,
                Type = "fetch",
                MainClass = MainClass,
                SubClass = ddlCover.SelectedItem.Value.Trim()
            };

            var motorProduct = service.PostData<BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper
                                   <BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductMasterResponse>,
                                   BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductMaster>
                                   (BKIC.SellingPoint.DTO.Constants.AdminURI.MotorProductOperation, request);

            if(motorProduct != null && motorProduct.StatusCode == 200 && motorProduct.Result.IsTransactionDone)
            {
                if(motorProduct.Result.motorProductMaster.Count > 0)
                {
                    UpdateProduct(motorProduct.Result.motorProductMaster[0]);
                }
            }

        }

        public void UpdateProduct(BKIC.SellingPoint.DTO.RequestResponseWrappers.MotorProductMaster motorProductMaster)
        {
            txtDescription.Text = motorProductMaster.Description;
            txtExcessAmount.Text = Convert.ToString(motorProductMaster.ExcessAmount);
            txtMainClass.Text = motorProductMaster.MainClass;
            txtSubClass.Text = motorProductMaster.SubClass;
            txtGCCCoverRangeInYears.Text = Convert.ToString(motorProductMaster.GCCCoverRangeInYears);
            txtMaximumVehicleAge.Text = Convert.ToString(motorProductMaster.MaximumVehicleAge);
            txtMaximumVehicleValue.Text = Convert.ToString(motorProductMaster.MaximumVehicleValue);
            txtMinimumPremium.Text = Convert.ToString(motorProductMaster.MinimumPremium);
            txtPolicyCode.Text = Convert.ToString(motorProductMaster.PolicyCode);
            txtRate.Text = Convert.ToString(motorProductMaster.Rate);
            txtSeriesFormatLength.Text = Convert.ToString(motorProductMaster.SeriesFormatLength);
            txtUnderAge.Text = Convert.ToString(motorProductMaster.UnderAge);
            txtUnderAgeExcessAmount.Text = Convert.ToString(motorProductMaster.UnderAgeExcessAmount);
            txtUnderAgeMinimumPremium.Text = Convert.ToString(motorProductMaster.UnderAgeminPremium);
            chkAllowMaximumVehicleAge.Checked = Convert.ToBoolean(motorProductMaster.AllowMaxVehicleAge);
            txtGulfAssitAmount.Text = Convert.ToString(motorProductMaster.GulfAssitAmount);
            chkAllowUnderAge.Checked = Convert.ToBoolean(motorProductMaster.AllowUnderAge);
            chkHasAdditionalDays.Checked = Convert.ToBoolean(motorProductMaster.HasAdditionalDays);
            chkHasAgeLoading.Checked = Convert.ToBoolean(motorProductMaster.HasAgeLoading);
            chkHasGCC.Checked = Convert.ToBoolean(motorProductMaster.HasGCC);           
            chkUnderAgeToHIR.Checked = Convert.ToBoolean(motorProductMaster.UnderAgeToHIR);
            chkAllowUsedVehicle.Checked = Convert.ToBoolean(motorProductMaster.AllowUsedVehicle);
            txtAgeLoadingPercent.Text = Convert.ToString(motorProductMaster.AgeLoadingPercent);
            txtLastSeries.Text = Convert.ToString(motorProductMaster.LastSeries);

            btnSubmit.Text = "Update";

        }

        public void ClearProduct()
        {
            txtDescription.Text = string.Empty;
            txtExcessAmount.Text = string.Empty;
            txtMainClass.Text = string.Empty;
            txtSubClass.Text = string.Empty;
            txtGCCCoverRangeInYears.Text = string.Empty;
            txtMaximumVehicleAge.Text = string.Empty;
            txtMaximumVehicleValue.Text = string.Empty;
            txtMinimumPremium.Text = string.Empty;
            txtPolicyCode.Text = string.Empty;
            txtRate.Text = string.Empty;
            txtLastSeries.Text = string.Empty;
            txtAgeLoadingPercent.Text = string.Empty;
            txtSeriesFormatLength.Text = string.Empty;
            txtUnderAge.Text = string.Empty;
            txtUnderAgeExcessAmount.Text = string.Empty;
            txtUnderAgeMinimumPremium.Text = string.Empty;
            txtGulfAssitAmount.Text = string.Empty;
            chkAllowMaximumVehicleAge.Checked = false;
            chkAllowUnderAge.Checked = false;
            chkHasAdditionalDays.Checked = false;
            chkHasAgeLoading.Checked = false;
            chkHasGCC.Checked = false;            
            chkUnderAgeToHIR.Checked = false;
            chkAllowUsedVehicle.Checked = false;
        }
    }
    
}