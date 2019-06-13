<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="MotorProductMaster.aspx.cs" Inherits="SellingPoint.Presentation.MotorProductMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateKeyHide {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="upnlMotorProductMaster">
            <ContentTemplate>
                <%-- <div class="page-header">
							<h1>Motor Product Cover </h1>
			    </div>--%>
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Motor Product</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="form-horizontal form-label-left col-md-12">
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Cover:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                            <asp:DropDownList ID="ddlCover"  runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" onChange="showPageLoader();" OnSelectedIndexChanged="MotorProduct_changed">
                                                <asp:ListItem Text="Select" Value="-1"/>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlCover" ErrorMessage="Please select cover" ControlToValidate="ddlCover" runat="server" ValidationGroup="MotorCalculationValidation" />
                                        </div>
                                    </div>
                                </div>
                                 <div class="form-group col-md-6">                                   
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">MainClass:</label>
                                    </div>
                                     <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtMainClass" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtMainClass" runat="server" ControlToValidate="txtMainClass" ErrorMessage="Please enter main class" ValidationGroup="MotorProductValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">SubClass:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtSubClass" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtSubClass" runat="server" ControlToValidate="txtSubClass" ErrorMessage="Please enter sub class" ValidationGroup="MotorProductValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Description:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Please enter  description" ValidationGroup="MotorProductValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Rate:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtRate" runat="server" ControlToValidate="txtRate" ErrorMessage="Please enter cover rate" ValidationGroup="MotorProductValidation" />
                                        </div>
                                    </div>
                                </div> 
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Minimum Premium:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtMinimumPremium" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtMinimumPremium" runat="server" ControlToValidate="txtMinimumPremium" ErrorMessage="Please enter minium premium" ValidationGroup="MotorProductValidation" />
                                        </div>
                                    </div>
                                </div>  
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Under Age Minimum Premium:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtUnderAgeMinimumPremium" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div> 
                                    <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Maximum Vehicle Age:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtMaximumVehicleAge" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div> 
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Maximum Vehicle Value:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtMaximumVehicleValue" runat="server" CssClass=" form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div>
                                   <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Allow GCC Cover Upto(Years):</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtGCCCoverRangeInYears" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div>
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Excess Amount:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtExcessAmount" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div> 
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Under Age Excess Amount:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtUnderAgeExcessAmount" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div>
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Policy Code:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtPolicyCode" runat="server" CssClass="form-control col-md-10"/>
                                        </div>
                                    </div>
                                </div> 
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Last Series No:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtLastSeries" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div> 
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Series Format(In digits):</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtSeriesFormatLength" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div> 
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">UnderAge:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtUnderAge" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div>
                                   <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Age Loading Percent:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtAgeLoadingPercent" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div>
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Gulf Assit Amount:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:TextBox  ID="txtGulfAssitAmount" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"/>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Allow UnderAge:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:CheckBox  ID="chkAllowUnderAge" runat="server" CssClass="col-md-10"/>
                                        </div>
                                    </div>
                                </div>                                 
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Has AgeLoading:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:CheckBox  ID="chkHasAgeLoading" runat="server" CssClass="col-md-10"/>
                                        </div>
                                    </div>
                                </div> 
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Has Additional Days:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:CheckBox  ID="chkHasAdditionalDays" runat="server" CssClass="col-md-10"/>
                                        </div>
                                    </div>
                                </div> 
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Allow Maximum Vehicle Age:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:CheckBox  ID="chkAllowMaximumVehicleAge" runat="server" CssClass="col-md-10"/>
                                        </div>
                                    </div>
                                </div>
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Has GCC:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:CheckBox  ID="chkHasGCC" runat="server" CssClass="col-md-10"/>
                                        </div>
                                    </div>
                                </div>                          
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Under Age To HIR:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:CheckBox  ID="chkUnderAgeToHIR" runat="server" CssClass="col-md-10"/>
                                        </div>
                                    </div>
                                </div>
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Allow Used Vehicle:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:CheckBox  ID="chkAllowUsedVehicle" runat="server" CssClass="col-md-10"/>
                                        </div>
                                    </div>
                                </div> 
                                <div class="clearfix"></div>
                                <div class="ln_solid"></div>
                                <div class="form-group">
                                    <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" Class="btn btn-primary" OnClientClick="showPageLoader();" OnClick="btnSubmit_Click" ValidationGroup="MotorProductValidation" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Class="btn btn-warning" OnClick="btnCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>   
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

