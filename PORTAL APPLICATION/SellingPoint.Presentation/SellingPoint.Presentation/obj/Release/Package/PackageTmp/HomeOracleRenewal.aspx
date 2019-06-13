<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="HomeOracleRenewal.aspx.cs" Inherits="SellingPoint.Presentation.HomeOracleRenewal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90) !important;
            opacity: 0.6 !important;
            z-index: 20;
        }

        .modalpopup {
            padding: 10px;
            position: relative;
            width: 52%;
            height: auto;
            background-color: white;
            border: 1px solid black;
        }
    </style>
    <script>
        $(document).on('click', '.modalpopup .model-header-title span', function () {
            $('.modalpopup, .modalBackground').hide();
        });
    </script>
    <script>
        function ShowPopup() {
            if (checkPageIsValid()) {
                $('#myModal').modal('show');
            }
            else {
                showPageLoader('HomeCalculationValidation,HomeAuthValidation');
                focusErrorElement();
                $('#myModal').modal('hide');
                $(".modal-backdrop").remove();

            }
        }
        function closePopup() {
            $('#myModal').modal('hide');
            $(".modal-backdrop").remove();
        }
        function checkPageIsValid() {
            //var formgroup = 'HomeCalculationValidation,HomeAuthValidation';
            //var listGroups = formgroup.split(',');
            //return validateGroups(listGroups);
            // return Page_ClientValidate('HomeCalculationValidation,HomeCalculationValidation');

            var valid = false;
            if (Page_ClientValidate('HomeCalculationValidation') && Page_ClientValidate('HomeAuthValidation')) {
                valid = true;
            }
            return valid;
        }
        function ShowEndorsementPopup() {
            $('#endorsementWarning').modal('show');
        }
        function closeEndorsementPopup() {
            $('#endorsementWarning').modal('hide');
            $(".modal-backdrop").remove();
        }
        $(function () {
            autocompleteCPR();
            getHomeOracleRenewalPolicies();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container container_top_margin height-common">
        <asp:UpdatePanel runat="server" ID="upnlHomeInsurance">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="HomeID" />
                <asp:HiddenField runat="server" ID="CPR" />
                <div class="page-header">
                    <h1>Home Oracle Renewal</h1>
                </div>
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Home Policy Search :</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content form-grid-3">
                        <div class="form-horizontal form-label-left col-md-12">
                            <div class="form-group col-md-4">
                                <div class="col-md-6 page-label form-grid-3-label">
                                    <label class="control-label">Policy Number:</label>
                                </div>
                                <div class="col-md-6 page-label form-grid-3-input">
                                    <asp:TextBox ID="txtHomeOracleRenewalPolicySearch" runat="server" CssClass="form-control" autocomplete="off" Width="300" />
                                </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-12 pull-left form-grid-3-btn form-grid-3-btn-small">
                                    <asp:Button ID="btnPolicy" runat="server" Text="Renew" CssClass="btn btn-primary" OnClientClick="showPageLoader();" OnClick="btnPolicy_Click" ValidationGroup="PolicyCheck" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClientClick="showPageLoader();" OnClick="btnClear_Click" CssClass="btn btn-primary" ValidationGroup="PolicyCheck" />
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="divider-20"></div>
                            <div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Branch:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control col-md-10 chzn-select" SetFocusOnError="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvddlBranch" ErrorMessage="Please select branch" ControlToValidate="ddlBranch" runat="server" ValidationGroup="HomeCalculationValidation" />
                                    </div>
                                </div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Introduced By:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control col-md-10 chzn-select">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvddlUsers" ErrorMessage="Please select introducedby" ControlToValidate="ddlUsers" runat="server" ValidationGroup="HomeCalculationValidation" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                    </div>
                    <div id="subpanel" runat="server">
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Client:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>
                                </ul>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-12">
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">CPR:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox ID="txtCPR" runat="server" CssClass="form-control col-md-10" OnClick="populate_fileds" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Client Code: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox ID="txtClientCode" runat="server" CssClass="form-control col-md-10" OnClick="populate_fileds" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Insured Name: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox ID="txtInsuredName" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Issue Date: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control col-md-10 policydate" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtIssueDate" ErrorMessage="Please enter issue date" SetFocusOnError="true" ControlToValidate="txtIssueDate" runat="server" ValidationGroup="HomeCalculationValidation" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- House details--%>
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>House Insurance:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>
                                </ul>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-12">
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Building Type: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:DropDownList ID="ddlBuildingType" runat="server" onChange="showPageLoader();" CssClass="form-control col-md-10 chzn-select" OnSelectedIndexChanged="enable_Fields" AutoPostBack="True">
                                                <asp:ListItem Text="Select" Value="-1" />
                                                <asp:ListItem Value="1">House</asp:ListItem>
                                                <asp:ListItem Value="2">Flat</asp:ListItem>
                                                <asp:ListItem Value="3">Contents</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="-1" ID="rfvddlBuildingType" CssClass="err" ErrorMessage="Please Select Building Type" ControlToValidate="ddlBuildingType" runat="server" ValidationGroup="HomeCalculationValidation" />
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">House No:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtHouseNo" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Flat No:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtFlatNo" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Building No:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtBuildingNo" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                                <%-- <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtBuildingNo" runat="server" ControlToValidate="txtBuildingNo" ErrorMessage="Please enter block No" ValidationGroup="HomeCalculationValidation" />--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Road No:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtRoadNo" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtRoadNo" runat="server" ControlToValidate="txtRoadNo" ErrorMessage="Please enter road No" ValidationGroup="HomeCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Block No:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtBlockNo" runat="server" CssClass="form-control col-md-10" Onchange="showPageLoader()" OnTextChanged="BlockNumber_Changed" AutoPostBack="true" Enabled="false" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtBlockNo" runat="server" ControlToValidate="txtBlockNo" ErrorMessage="Please enter block No" ValidationGroup="HomeCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Area:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                    <asp:ListItem Text="Select" Value="-1" />
                                                    <asp:ListItem>Manama</asp:ListItem>
                                                    <asp:ListItem>Budhaiya</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlArea" CssClass="err" ErrorMessage="Please Select Area" ControlToValidate="ddlArea" runat="server" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Age of Building:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:DropDownList ID="ddlBuildingAge" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                    <asp:ListItem Text="--Please Select--" Value="-1" />
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>7</asp:ListItem>
                                                    <asp:ListItem>8</asp:ListItem>
                                                    <asp:ListItem>9</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                    <asp:ListItem>24</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>26</asp:ListItem>
                                                    <asp:ListItem>27</asp:ListItem>
                                                    <asp:ListItem>28</asp:ListItem>
                                                    <asp:ListItem>29</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                </asp:DropDownList>
                                                <span class="err">
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvBuildingAge" CssClass="err" SetFocusOnError="true" ControlToValidate="ddlBuildingAge" ValidationGroup="HomeCalculationValidation" ErrorMessage="Please Select the building age"></asp:RequiredFieldValidator>
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">No. of Floors:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtNoOfFloor" runat="server" CssClass="form-control col-md-10 onlynumber" Enabled="false" />
                                                <asp:RangeValidator ID="rfvtxtNoOfFloor" runat="server" ControlToValidate="txtNoOfFloor" ErrorMessage="No of floors can't be exceed 3" MaximumValue="3" MinimumValue="0"></asp:RangeValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Building Sum Insured: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtBuildingValue" runat="server" CssClass="form-control col-md-10 onlynumber" onChange="showPageLoader();" OnTextChanged="txtBuildingValue_TextChanged" AutoPostBack="true" />
                                                <asp:RequiredFieldValidator CssClass="err" runat="server" ID="rfvtxtBuildingValue" SetFocusOnError="true" ControlToValidate="txtBuildingValue" ValidationGroup="HomeCalculationValidation" ErrorMessage="Please enter building value"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Content Sum Insured: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtContentValue" runat="server" CssClass="form-control col-md-10 onlynumber" onChange="showPageLoader();" AutoPostBack="true" OnTextChanged="txtContentValueQuotation_TextChanged" />
                                                <asp:RequiredFieldValidator CssClass="err" runat="server" ID="rfvtxtContentValue" SetFocusOnError="true" ControlToValidate="txtContentValue" ValidationGroup="HomeCalculationValidation" ErrorMessage="Please enter content value"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Jewellery Sum Insured: </label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtJewelleryValue" runat="server" onChange="showPageLoader();" CssClass="form-control col-md-10 onlynumber" AutoPostBack="true" OnTextChanged="txtJewelleryValueQuotation_TextChanged" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Insurance period from:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtInsurancePeriodFrom" runat="server" CssClass="form-control col-md-10 policydate" OnTextChanged="calculate_expiredate" AutoPostBack="true" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rvftxtInsurancePeriodFrom" runat="server" ControlToValidate="txtInsurancePeriodFrom" ErrorMessage="Please enter insurance period from" ValidationGroup="HomeCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Insurance period to:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtInsurancePeriodTo" runat="server" CssClass="form-control col-md-10 datepicker" Enabled="false" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Domestic Cover:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlRequireDomesticHelpCover" CssClass="form-control col-md-10 chzn-select" OnSelectedIndexChanged="ddlRequireDomesticHelpCover_SelectedIndexChanged" runat="server" AutoPostBack="true">
                                                    <asp:ListItem>Yes</asp:ListItem>
                                                    <asp:ListItem>No</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Total SumInsured:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtTotalSumInsured" runat="server" CssClass="form-control" Enabled="false" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Remarks:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control col-md-10" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <div class="control-label page-control align-center">
                                            <asp:Button ID="btnQuestions" runat="server" Text="Questions" CssClass="col-md-2 btn btn-primary" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="x_panel" runat="server" id="divJointOwner" visible="false">
                            <div class="x_title">
                                <h2>Name of the joint Owner:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>
                                </ul>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-12">
                                    <asp:TextBox ID="txtJointOwnerName" runat="server" CssClass="form-control col-md-10 input-field-size"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtJointOwnerName" runat="server" ControlToValidate="txtJointOwnerName" ErrorMessage="Please enter joint owner name" ValidationGroup="HomeCalculationValidation" />
                                </div>
                            </div>
                        </div>
                        <div class="x_panel" runat="server" id="divBankName" visible="false">
                            <div class="x_title">
                                <h2>Bank Name:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>
                                </ul>
                                <div class="clearfix"></div>
                            </div>
                            <%--ddlbank_select--%>
                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-4 select_fullwidth">
                                    <asp:DropDownList ID="ddlBanks" runat="server" CssClass="  col-md-6 form-control input-field-size  chzn-select" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlBanks" runat="server" ControlToValidate="ddlBanks" ErrorMessage="Please select bank" ValidationGroup="HomeCalculationValidation" />
                                </div>
                            </div>
                        </div>
                        <div class="x_panel" runat="server" id="divInsurar" visible="false">
                            <div class="x_title">
                                <h2>Name of the insurer, policy period and reasons seeking insurance :</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>
                                </ul>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-12">
                                    <asp:TextBox ID="txtNameSeekingReasons" runat="server" CssClass="form-control col-md-10 input-field-size"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtNameSeekingReasons" runat="server" ControlToValidate="txtNameSeekingReasons" ErrorMessage="Please enter reason" ValidationGroup="HomeCalculationValidation" />
                                </div>
                            </div>
                        </div>
                        <div class="x_panel" runat="server" id="mainDomesticWorker" visible="false">
                            <div class="x_title">
                                <h2>Domestic Workers:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>
                                </ul>
                                <div class="clearfix"></div>
                            </div>

                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-12">
                                    <div id="divDetailedDomesticWorkers" runat="server" visible="false">
                                        <div class=" col-md-12 append-rows">
                                            <div class="table-append-rows">
                                                <asp:GridView ID="Gridview1" Width="100%" OnRowDataBound="Gridview1_RowDataBound" OnRowDeleting="Gridview1_RowDeleting" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDomesticName" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter name" ControlToValidate="txtDomesticName" runat="server" ValidationGroup="HomeCalculationValidation" />
                                                            </ItemTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                            <FooterTemplate>
                                                                <asp:Button ID="ButtonAdd" runat="server" OnClientClick="showPageLoader();" Text="Add New Dependant" OnClick="ButtonAdd_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sex">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlGender" runat="server">
                                                                    <asp:ListItem Text="--Select--" Value="-1" />
                                                                    <asp:ListItem Text="Male" Value="Male" />
                                                                    <asp:ListItem Text="Female" Value="Female" />
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please select gender" ControlToValidate="ddlGender" runat="server" ValidationGroup="HomeCalculationValidation" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Date Of Birth">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDOB" runat="server" CssClass="datepickerAge18to55"></asp:TextBox>
                                                                <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter date of birth" ControlToValidate="txtDOB" runat="server" ValidationGroup="HomeCalculationValidation" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Nationality">
                                                            <ItemTemplate>
                                                                <asp:DropDownList Width="70px" ID="ddlNational" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                                    <%-- <asp:ListItem Text="Select" Value="-1" />--%>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please select nationality" ControlToValidate="ddlNational" runat="server" ValidationGroup="HomeCalculationValidation" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="CPR / Passport No">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPassport" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter passportNo" ControlToValidate="txtPassport" runat="server" ValidationGroup="HomeCalculationValidation" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Occupation">
                                                            <ItemTemplate>
                                                                <asp:DropDownList Width="70px" ID="ddlDomesticOccupation" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                                    <%--<asp:ListItem Text="Select" Value="-1" />--%>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter occupation" ControlToValidate="ddlDomesticOccupation" runat="server" ValidationGroup="HomeCalculationValidation" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?');" CssClass="fsize fcolorred" CommandName="Delete"><i class="fa  fa-trash-o"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="x_panel" runat="server" id="mainAboveBD" visible="false">
                            <div class="x_title">
                                <h2>Above 2000BD Items:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>
                                </ul>
                                <div class="clearfix"></div>
                            </div>
                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-12">
                                    <div id="divSingleItemAboveBD" runat="server" visible="false">
                                        <asp:Repeater ID="rtSingleItemAboveBD" runat="server" OnItemDataBound="rtSingleItemAboveBD_DataBinding">
                                            <ItemTemplate>
                                                <div class="col-md-12">
                                                    <div class="form-group col-md-4">
                                                        <div class="col-md-4 page-label">
                                                            <label class="control-label">Item :</label>
                                                        </div>
                                                        <div class="col-md-8 page-control">
                                                            <div class="control-label ">
                                                                <asp:TextBox ID="txtCategory" Text='<%# Eval("Category") %>' CssClass="form-control" runat="server"></asp:TextBox>
                                                            </div>
                                                            <%--<span class="err"> <asp:RequiredFieldValidator ID="rfvCategoryForAboveBD" SetFocusOnError="true" ControlToValidate="ddlCategoryForAboveBD" runat="server" ValidationGroup ="HomeCalculationValidation" ErrorMessage="This field is required"></asp:RequiredFieldValidator>
                                                       </span>--%>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <div class="col-md-4 page-label">
                                                            <label class="control-label">Amount of the item :</label>
                                                        </div>
                                                        <div class="col-md-8 page-control">
                                                            <div class="control-label ">
                                                                <asp:TextBox ID="txtAmountOfItem" CssClass="form-control onlynumber" AutoPostBack="true" onchange="showPageLoader();" OnTextChanged="txtAmountOfItem_TextChanged" CausesValidation="true" runat="server" Text='<%# Eval("AmountOfItem") %>'></asp:TextBox>
                                                            </div>
                                                            <span class="err">
                                                                <asp:RequiredFieldValidator ID="rfvAmountOfItem" SetFocusOnError="true" ControlToValidate="txtAmountOfItem" runat="server" ValidationGroup="PolicyDetails" ErrorMessage="This field is required"></asp:RequiredFieldValidator>
                                                            </span>
                                                            <span class="err">
                                                                <asp:CustomValidator ID="cvAmountOfItem" SetFocusOnError="true" ControlToValidate="txtAmountOfItem" runat="server" ValidationGroup="PolicyDetails"></asp:CustomValidator>
                                                            </span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-md-4">
                                                        <div class="f1-buttons firstproceed Removebtn">
                                                            <asp:Button runat="server" ID="btnRemoveSingleItem" Text="Remove" OnClick="btnRemoveSingleItem_Click" OnClientClick="showPageLoader();" Visible="true" CssClass="col-md-2 btn btn-primary" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <div class="col-md-12">
                                            <div class="f1-buttons firstproceed Removebtn">
                                                <asp:Button runat="server" ID="btnAddMoreItem" Text="ADD More" OnClick="btnAddMoreItem_Click" OnClientClick="showPageLoader();" Visible="true" CssClass="col-md-2 btn btn-primary" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Payment Details--%>
                        <div class="x_panel" runat="server" id="divPaymentSection">
                            <div class="x_title">
                                <h2>Payment Details:</h2>
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
                                            <label class="control-label">Payment Methods:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged">
                                                    <asp:ListItem Text="--Please Select--" Value="-1" />
                                                    <asp:ListItem Value="0" Text="Cash"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="cheque"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Debit Card"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Credit Card"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator InitialValue="-1" CssClass="err" ID="rfvddlPaymentMethod" runat="server" ControlToValidate="ddlPaymentMethod" ErrorMessage="Please select payment method" ValidationGroup="HomeAuthValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Account Number:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control col-md-10" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtAccountNo" ErrorMessage="Please enter account number" ControlToValidate="txtAccountNo" runat="server" ValidationGroup="HomeAuthValidation" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="x_panel">
                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-12">
                                    <div class="clearfix"></div>
                                    <div class="form-group">
                                        <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                            <div runat="server" id="amtDisplay" class="calculate-amount">
                                                <div class="dsc-overallwrapper vat_calcuation" id="includeDisc" runat="server" visible="false">
                                                    <div class="dsc-wrapper">
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Premium Amount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="premiumAmount" Enabled="false" AutoPostBack="true" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Commission : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="commission" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="dsc-wrapper">
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">VAT Amount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtVATAmount" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12" runat="server" visible="false">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">VAT Commission : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtVATCommission" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="dsc-wrapper">
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Total Premium : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtTotalPremium" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12" runat="server" visible="false">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Total Commission : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtTotalCommission" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="dsc-wrapper">
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Discount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtDiscount" AutoPostBack="true" onChange="showPageLoader();" OnTextChanged="validate_Premium" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="excludeDisc" class="dsc-overallwrapper vat_calcuation" runat="server" visible="false">
                                                    <div class="dsc-wrapper">
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Premium Amount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="premiumAmount1" Enabled="false" AutoPostBack="true" runat="server" OnTextChanged="validate_Premium" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Commission : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="commission1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="dsc-wrapper">
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">VAT Amount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtVATAmount1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12" runat="server" visible="false">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">VAT Commission : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtVATCommission1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="dsc-wrapper">
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Total Premium : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtTotalPremium1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-12" runat="server" visible="false">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Total Commission : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtTotalCommission1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                         <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Discount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtDiscount1" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" Enabled="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                        <asp:Button ID="btnCalculate" runat="server" AutoPostBack="true" Text="Calculate" CssClass="btn btn-primary" OnClientClick="showPageLoader('HomeCalculationValidation');" OnClick="Calculate_Click" ValidationGroup="HomeCalculationValidation" />
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSubmit_Click" OnClientClick="showPageLoader('HomeCalculationValidation,HomeAuthValidation');" ValidationGroup="HomeCalculationValidation" />
                                        <asp:Button ID="btnAuthorize" runat="server" data-target="#myModal" OnClientClick="ShowPopup();" Text="Authorize" CssClass="btn btn-primary" OnClick="btnAuthorize_Click" ValidationGroup="HomeCalculationValidation" Enabled="false" />
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-primary" OnClick="btnBack_Click" ValidationGroup="HomeCalculationValidation" />
                                        <%--  <asp:Button ID="btnPrint" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnPrint_Click" />--%>
                                        <a runat="server" id="downloadschedule" class="btn btn-primary" title="Schedule" onclick="btnPrint_Click"><i class="fa fa-download" aria-hidden="true"></i>Schedule</a>
                                        <asp:HiddenField ID="calculatedPremium" runat="server" />
                                        <asp:HiddenField ID="calculatedCommision" runat="server" />
                                        <asp:HiddenField ID="actualRenewalDate" runat="server" />
                                        <asp:HiddenField ID="formHomeSubmitted" Value="false" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                    <%-- </div>--%>

                    <asp:Panel runat="server" ID="Panel1" CssClass="modalpopup">
                        <%--  <div class="form-horizontal form-label-left col-md-12" CssClass="modalpopup">--%>
                        <div class="col-md-12 model-container">
                            <div class="model-header-title">
                                <h3>Questions</h3>
                                <span><i class="fa fa-close"></i></span>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-6 page-label">
                                    <label class="control-label">Is the property mortgaged? *</label>
                                </div>
                                <div class="col-md-6 page-control">
                                    <asp:DropDownList ID="ddlMortgaged" CssClass="form-control col-md-10" runat="server" OnSelectedIndexChanged="ddlMortaged_Changed">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlMortgaged" ErrorMessage="Please select this field" ControlToValidate="ddlMortgaged" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-6 page-label">
                                    <label class="control-label">Is there a safe in the property to be insured? *</label>
                                </div>
                                <div class="col-md-6 page-control">
                                    <asp:DropDownList ID="ddlPropertyInsured" CssClass="form-control col-md-10" runat="server" OnChange="if(!validateJewellery()){return false;};" OnSelectedIndexChanged="ddlProperty_Changed">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlPropertyInsured" ErrorMessage="Please select this field" ControlToValidate="ddlPropertyInsured" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-6 page-label">
                                    <label class="control-label">Jewellery Cover within the contents? *</label>
                                </div>
                                <div class="col-md-6 page-control">
                                    <asp:DropDownList ID="ddlJewelleryCoverWithinContents" CssClass="form-control col-md-10" runat="server" OnSelectedIndexChanged="ddlJewellery_Changed">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem Value="NO COVER">No Cover</asp:ListItem>
                                        <asp:ListItem Value="STANDARD">15% of Contents value up to BD 2,500</asp:ListItem>
                                        <asp:ListItem Value="EXTENDED">25% of contents value up to BD 5,000</asp:ListItem>
                                        <asp:ListItem Value="EXTREME">Contents above BD 5,000</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlJewelleryCoverWithinContents" ErrorMessage="Please select this field" ControlToValidate="ddlJewelleryCoverWithinContents" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-6 page-label">
                                    <label class="control-label">Would you like to add riot, strike and malicious damage cover? *</label>
                                </div>
                                <div class="col-md-6 page-control">
                                    <asp:DropDownList ID="ddlMaliciousDamageCover" CssClass="form-control col-md-10" runat="server" OnSelectedIndexChanged="ddlMaliciousDamage_Changed">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlMaliciousDamageCover" ErrorMessage="Please select this field" ControlToValidate="ddlMaliciousDamageCover" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-6 page-label">
                                    <label class="control-label">Does the property have a joint ownership? *</label>
                                </div>
                                <div class="col-md-6 page-control">
                                    <asp:DropDownList ID="ddlPropertyJointOwnership" OnSelectedIndexChanged="ddlPropertyJointOwnership_SelectedIndexChanged" CssClass="form-control col-md-10" runat="server">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlPropertyJointOwnership" ErrorMessage="Please select this field" ControlToValidate="ddlPropertyJointOwnership" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-6 page-label">
                                    <label class="control-label">Is the insured property used in connection with any trade, business or profession? *</label>
                                </div>
                                <div class="col-md-6 page-control">
                                    <asp:DropDownList ID="ddlConnectionWithAnyTrade" CssClass="form-control col-md-10" runat="server" OnSelectedIndexChanged="ddlConnectionWithAnyTrade_Changed">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlConnectionWithAnyTrade" ErrorMessage="Please select this field" ControlToValidate="ddlConnectionWithAnyTrade" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-6 page-label">
                                    <label class="control-label">Is the property to be insured covered under any other insurance? *</label>
                                </div>
                                <div class="col-md-6 page-control">
                                    <asp:DropDownList ID="ddlInsuredCoverByOtherInsurance" OnSelectedIndexChanged="ddlInsuredCoverByOtherInsurance_SelectedIndexChanged" CssClass="form-control col-md-10" runat="server">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlInsuredCoverByOtherInsurance" ErrorMessage="Please select this field" ControlToValidate="ddlInsuredCoverByOtherInsurance" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-6 page-label">
                                    <label class="control-label">Has the property to be insured sustained any loss or damage (whether covered by insurance or not) during the last 5 years? *</label>
                                </div>
                                <div class="col-md-6 page-control">
                                    <asp:DropDownList ID="ddlPropertyInsuredSustainedAnyLossOrDamage" CssClass="form-control col-md-10" runat="server" OnSelectedIndexChanged="ddlPropertyInsuredSustained">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlPropertyInsuredSustainedAnyLossOrDamage" ErrorMessage="Please select this field" ControlToValidate="ddlPropertyInsuredSustainedAnyLossOrDamage" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">

                                <div class="col-md-6 page-label">
                                    <label class="control-label">Do you have any single item within the contents above BD 2,000? *</label>
                                </div>
                                <div class="col-md-6  page-control">
                                    <asp:DropDownList ID="ddlSingleItemAboveBD" CssClass="form-control col-md-10" OnSelectedIndexChanged="ddlSingleItemAboveBD_SelectedIndexChanged" runat="server">
                                        <asp:ListItem Text="Select" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlSingleItemAboveBD" ErrorMessage="Please select this field" ControlToValidate="ddlSingleItemAboveBD" runat="server" ValidationGroup="questionsValidation" />
                                </div>
                            </div>
                        </div>
                        <div class="model-footer-contanier">
                            <div class="page-button-wrap">
                                <asp:Button ID="btnSave" Text="Save" runat="server" ValidationGroup="questionsValidation" CssClass="btn btn-primary" OnClientClick="if (!validate()) {return false;}" OnClick="btnSavePopUp" UseSubmitBehavior="false" />
                            </div>
                            <div class="page-button-wrap">
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancelPopUp" />
                            </div>
                        </div>
                </div>
                </asp:Panel>
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID="ModalBehaviour" runat="server" CancelControlID="btnCancel" TargetControlID="btnQuestions" PopupControlID="Panel1" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server">
                    <div class="container" runat="server">
                        <div class="row">
                            <div class="modal" id="myModal">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span></button>
                                            <h4 class="modal-title">Home Insurance</h4>
                                        </div>
                                        <div class="modal-body" runat="server" id="modalBodyText">
                                            Are you sure want to authorize this policy?
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnYes" type="button" Text="Yes" OnClientClick="showPageLoader();" OnClick="Auth" runat="server" CssClass="btn btn-primary" />
                                            <%--<button type="button" runat="server" id="btnClose"  data-dismiss="modal" Class="btn btn-primary">Close</button>--%>
                                            <asp:Button ID="btnNo" type="button" OnClientClick="closePopup();" OnClick="Reset_Content" Text="No" runat="server" CssClass="btn btn-primary" />
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server">
                    <div class="container" runat="server">
                        <div class="row">
                            <div class="modal" id="endorsementWarning">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span></button>
                                            <h4 class="modal-title">Home Insurance</h4>
                                        </div>
                                        <div class="modal-body" runat="server" id="Div3">
                                            This policy is endorsend,for more details see any endorsement page.
                                        </div>
                                        <div class="modal-footer">
                                            <%--  <asp:Button ID="Button2" type="button" Text="Yes" OnClientClick="showPageLoader();" OnClick="Auth" runat="server" CssClass="btn btn-primary" />--%>
                                            <%--<button type="button" runat="server" id="btnClose"  data-dismiss="modal" Class="btn btn-primary">Close</button>--%>
                                            <asp:Button ID="Button2" type="button" OnClientClick="closeEndorsementPopup();" Text="OK" runat="server" CssClass="btn btn-primary" />
                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">

        function validate() {
            var filled = true;

            if ($('#<%= ddlMaliciousDamageCover.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if ($('#<%= ddlMortgaged.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if ($('#<%= ddlConnectionWithAnyTrade.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if ($('#<%= ddlInsuredCoverByOtherInsurance.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if ($('#<%= ddlJewelleryCoverWithinContents.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if ($('#<%= ddlPropertyJointOwnership.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if ($('#<%= ddlSingleItemAboveBD.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if ($('#<%= ddlPropertyInsured.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if ($('#<%= ddlPropertyInsuredSustainedAnyLossOrDamage.ClientID %>')[0].selectedIndex == '0') {
            filled = false;
        }
        if (!filled) {
            alert('Please fill all the fields');
        }
        else {
            $('#<%= ModalPopupExtender1.ClientID %>').hide();
                $find("ModalBehaviour").hide();

            }
            return filled;
        };
        function validateJewellery() {
            if ($('#<%= ddlPropertyInsured.ClientID %>')[0].selectedIndex == '1') {
            if ($('#ContentPlaceHolder1_txtContentValue').val() > 0) {
                $('#<%= ddlJewelleryCoverWithinContents.ClientID %>').prop('disabled', false);
            }

        }
        else if ($('#<%= ddlPropertyInsured.ClientID %>')[0].selectedIndex == '2') {
            $('#<%= ddlJewelleryCoverWithinContents.ClientID %>')[0].selectedIndex = 1;
            $('#<%= ddlJewelleryCoverWithinContents.ClientID %>').prop('disabled', true);
        }
        else {
            $('#<%= ddlJewelleryCoverWithinContents.ClientID %>')[0].selectedIndex = 0;
            $('#<%= ddlJewelleryCoverWithinContents.ClientID %>').prop('disabled', false);
            }

        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            // validateJewellery();

        });
    </script>
</asp:Content>
