<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="MotorInsurance.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.MotorInsurance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--      <style>
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
    </script>--%>
    <script>
        function ShowPopup() {
            if (checkPageIsValid()) {              
                $('#myModal').modal('show');
            }
            else {
                showPageLoader('MotorCalcualtionValidation,MotorAuthValidation');
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

            //var formgroup = 'MotorCalculationValidation,MotorAuthValidation';
            //var listGroups = formgroup.split(',');
            //return validateGroups(listGroups);

            // return Page_ClientValidate('MotorCalcualtionValidation');

            var valid = false;
            if (Page_ClientValidate('MotorCalcualtionValidation') && Page_ClientValidate('MotorAuthValidation')) {
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
              getMotorPolicies();
        }); 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="upnlHomeInsurance">
            <ContentTemplate>
                <div class="page-header">
                    <h1>Motor Insurance</h1>
                </div>
                    <%-- Policy details--%>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Motor Policy Search :</h2>
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
                                        <asp:TextBox ID="txtMotorPolicySearch" runat="server" CssClass="form-control" autocomplete="off" Width="300" TabIndex="1"/> 
                                        <%--<asp:HiddenField  ID="renewalCount"  runat="server"  />--%>
                                    </div>
                                </div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-12 pull-left form-grid-3-btn form-grid-3-btn-small ">                                      
                                        <asp:Button ID="btnPolicy" runat="server" Text="Review" OnClientClick="showPageLoader();" OnClick="btnPolicy_Click" CssClass="btn btn-primary" TabIndex="2" />                                        
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClientClick="showPageLoader();" OnClick="btnClear_Click" CssClass="btn btn-primary" ValidationGroup="PolicyCheck"  TabIndex="3"/>
                                        <asp:Button ID="btnDummy" runat="server" Visible="false"/>                                       
                                    </div>                                    
                                </div>
                                <div class="form-group col-md-4">
                                     <div class="col-md-12 page-button-wrap form-grid-3-btn form-grid-3-btn-right">
                                          <asp:Button ID="Button5" type="button" Text="Add Insured" CssClass="btn btn-info btn-link-button" runat="server" OnClick="insured_Master" TabIndex="4" />
                                      </div>
                                </div>
                         <div class="clearfix"></div>
                          <div class="divider-20"></div>
                          <div>
                          <div class="form-group col-md-4">
                            <div class="col-md-6 page-label form-grid-3-label">
                                <label class="control-label">CPR: *</label>
                            </div>                          
                            <div class="col-md-6 page-label form-grid-3-input">
                               <asp:TextBox ID="txtCPRSearch" runat="server" CssClass="form-control onlynumber" autocomplete="off" Width="300" AutoPostBack="true" OnTextChanged="txtCPR_Changed" onChange="showPageLoader();" TabIndex="5"/>
                               <asp:RequiredFieldValidator ID="rfvtxtCPRSearch" CssClass="err" ErrorMessage="Please enter CPR" SetFocusOnError="true" ControlToValidate="txtCPRSearch" runat="server" ValidationGroup="MotorCalculationValidation" /> 
                            </div>                              
                        </div>
                         <div class="form-group col-md-4">
                            <div class="col-md-6 page-label form-grid-3-label">
                                <label class="control-label">Branch:</label>
                            </div>
                            <div class="col-md-6 page-control form-grid-3-input">
                              <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control col-md-10 chzn-select"  SetFocusOnError="true" TabIndex="6">
                                </asp:DropDownList>
                                 <asp:RequiredFieldValidator CssClass="err" ID="rfvddlBranch" ErrorMessage="Please select branch" ControlToValidate="ddlBranch" runat="server" ValidationGroup="MotorInsuranceValidation" />
                            </div>
                        </div>
                        <div class="form-group col-md-4">
                            <div class="col-md-6 page-label form-grid-3-label">
                                <label class="control-label">Introduced By:</label>
                            </div>
                             <div class="col-md-6 page-control form-grid-3-input">
                              <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control col-md-10 chzn-select" TabIndex="7" >
                               </asp:DropDownList>
                              <asp:RequiredFieldValidator CssClass="err" ID="rfvddlUsers" ErrorMessage="Please select introducedby" ControlToValidate="ddlUsers" runat="server" ValidationGroup="MotorCalculationValidation" />
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
                                            <asp:TextBox ID="txtCPR" runat="server" CssClass="form-control col-md-10" Enabled="false" TabIndex="8" />
                                            <%--<asp:RequiredFieldValidator CssClass="err" ID="RequiredFieldValidator4" ErrorMessage="Please select Physical Defect" ControlToValidate="ddlPhydefect" runat="server" ValidationGroup="domesticValidation" />--%>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Client Code: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox ID="txtClientCode" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                            <%--<asp:RequiredFieldValidator ID="rfvtxtClientCode" ErrorMessage="Please enter client code" SetFocusOnError="true" ControlToValidate="txtClientCode" runat="server" ValidationGroup ="MotorCalculationValidation" />--%>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Insured Name: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox ID="txtInsuredName" runat="server" CssClass="form-control col-md-10" Enabled="false"/>
                                            <%--<asp:RequiredFieldValidator ID="rfvtxtInsuredName" SetFocusOnError="true" ErrorMessage="Please enter insured name" ControlToValidate="txtInsuredName" runat="server" ValidationGroup ="MotorCalculationValidation" />--%>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Age</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control col-md-10" Enabled="false"/>
                                            <%--<asp:RequiredFieldValidator ID="rfvtxtDOB" SetFocusOnError="true" ErrorMessage="Please enter date of birth" ControlToValidate="txtDOB" runat="server" ValidationGroup ="MotorCalculationValidation" />--%>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Issue Date: *</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control col-md-10 datepicker" TabIndex="9" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtIssueDate" ErrorMessage="Please enter issue date" SetFocusOnError="true" ControlToValidate="txtIssueDate" runat="server" ValidationGroup="MotorInsuranceValidation" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- Insurance / Bank Details--%>
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Insurance / Bank Details:</h2>
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
                                            <label class="control-label">Under Loan:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlUnderLoan" runat="server" onChange="showPageLoader();" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" OnSelectedIndexChanged="vehicle_financed" TabIndex="10">
                                                    <asp:ListItem Text="Select" Value="-1" />
                                                    <asp:ListItem>Yes</asp:ListItem>
                                                    <asp:ListItem>No</asp:ListItem>
                                                </asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="rfvddlUnderLoan" InitialValue="-1" ErrorMessage="Please select under loan" ControlToValidate="ddlUnderLoan" runat="server" ValidationGroup ="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Bank Name:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlBanks" runat="server" CssClass="form-control col-md-10 chzn-select" onChange="showPageLoader();" AutoPostBack="True" OnSelectedIndexChanged="update_BankCode">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvddlBanks" ErrorMessage="Please select bank" ControlToValidate="ddlBanks" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Bank Code:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtBankCode" runat="server" CssClass="form-control col-md-10" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Cover:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlCover" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" onChange="showPageLoader();" OnSelectedIndexChanged="MotorProduct_changed" >
                                                    <asp:ListItem Text="Select" Value="-1" />
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvddlCover" ErrorMessage="Please select cover" ControlToValidate="ddlCover" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                     <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Used / New:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlVehicleType" runat="server" onchange="showPageLoader();" AutoPostBack="true" OnSelectedIndexChanged="VehicleType_Changed" CssClass="form-control col-md-10 chzn-select" >
                                                    <asp:ListItem Text="Select" Value="-1" />
                                                    <asp:ListItem>Used</asp:ListItem>
                                                    <asp:ListItem>New</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlVehicleType" InitialValue="-1" ErrorMessage="Please select vehicle type" ControlToValidate="ddlVehicleType" runat="server" ValidationGroup ="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                     <div class="form-group col-md-4" id="underBCFC" runat="server">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Policy Under BCFC:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                               <asp:CheckBox ID="ChkPolicyUnderBCFC" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                        </div>
                         <%-- Vehicle details--%>
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Vehicle Details:</h2>
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
                                            <label class="control-label">Make:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlMake" runat="server" CssClass="form-control col-md-10 chzn-select" OnChange="showPageLoader();" AutoPostBack="True"  OnSelectedIndexChanged="ddlMake_SelectedIndexChanged" >
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlMake" CssClass="err" ErrorMessage="Please select vehicle make" ControlToValidate="ddlMake" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Model:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control col-md-10 chzn-select" OnChange="showPageLoader();" OnSelectedIndexChanged="ddlModel_SelectedIndexChanged" AutoPostBack="True">
                                                    <asp:ListItem Text="Select" Value="-1" />
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlModel" CssClass="err" ErrorMessage="Please select vehicle model" ControlToValidate="ddlModel" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Type of Body:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                 <%--<asp:TextBox ID="txtBodyType" runat="server" CssClass="form-control col-md-10" TabIndex="23" ReadOnly="true" />--%>
                                                <asp:DropDownList ID="ddlBodyType" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                 <asp:ListItem Text="Select" Value="-1" />
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlBodyType" CssClass="err" ErrorMessage="Please select vehicle body" ControlToValidate="ddlBodyType" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Year of Manufacture:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:DropDownList ID="ddlManufactureYear" runat="server" CssClass="form-control col-md-10 chzn-select" OnSelectedIndexChanged="ddlManufactureYear_Changed" AutoPostBack="True" >                                               
                                                </asp:DropDownList>
                                                 <asp:RequiredFieldValidator ID="rfvddlManufactureYear" CssClass="err" ErrorMessage="Please select vehicle manufacture year" ControlToValidate="ddlManufactureYear" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Engine CC:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:DropDownList ID="ddlEnginecc" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlEnginecc" CssClass="err" ErrorMessage="Please select engineCC" ControlToValidate="ddlEnginecc" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                      <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Excess Value(BHD):</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox runat="server" ID="txtExcessValue" CssClass="form-control col-md-10" Enabled="false"></asp:TextBox>

                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Please Select Building Type" ControlToValidate="ddlBuildingType" runat="server" ValidationGroup ="UserMasterValidation" />--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Excess:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlExcess" runat="server" CssClass="form-control col-md-10 chzn-select" onchange="showPageLoader();" OnSelectedIndexChanged="ddlExcess_Changed" AutoPostBack="True"  Enabled="false">
                                                    <asp:ListItem Text="Select" Value="-1" />
                                                    <asp:ListItem Value="Standard" Selected="True">Standard</asp:ListItem>
                                                    <asp:ListItem Value="Twice">Twice (Applies Discount)</asp:ListItem>
                                                    <asp:ListItem Value="4 Times">4 Times (Applies Greater Discount)</asp:ListItem>
                                                    <asp:ListItem Value="None">None (Removes Discount)</asp:ListItem>
                                                </asp:DropDownList>

                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ErrorMessage="Please Select Building Type" ControlToValidate="ddlBuildingType" runat="server" ValidationGroup ="UserMasterValidation" />--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Registration:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtRegistration" runat="server" CssClass="form-control col-md-10 onlynumber" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtRegistration" ErrorMessage="Please enter registration no" ControlToValidate="txtRegistration" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                      <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Chassis:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtChassis" runat="server" MaxLength="17" CssClass="form-control col-md-10"  ValidationGroup="MotorCalculationValidation" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtChassis" ErrorMessage="Please enter chasses no" ControlToValidate="txtChassis" runat="server" ValidationGroup="MotorCalculationValidation" />
                                                <asp:RegularExpressionValidator CssClass="err" Display = "Dynamic" ControlToValidate = "txtChassis" ID="revtxtChassis" ValidationExpression = "^[\s\S]{17,17}$" runat="server" ErrorMessage="Minimum 17 characters required." ValidationGroup="MotorCalculationValidation" ></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                      <div class="clearfix"></div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Sum Insured:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtSumInsured" runat="server" CssClass="form-control col-md-10 onlynumber" AutoPostBack ="true" onChange="showPageLoader();" OnTextChanged="txtSumInsured_Changed"  />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtSumInsured" ErrorMessage="Please enter suminsured" ControlToValidate="txtSumInsured" runat="server" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>

                                       <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Insurance period from:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtInsuredPeriodFrom" runat="server" CssClass="form-control col-md-10 policydate" onchange="showPageLoader();"  OnTextChanged="calculate_expiredate" AutoPostBack="true"  />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtInsuredPeriodFrom" runat="server" ControlToValidate="txtInsuredPeriodFrom" ErrorMessage="Please enter policy start date" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Insurance period to:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtInsuredPeriodTo" runat="server" CssClass="form-control col-md-10 datepicker" AutoPostBack="true" onChange="showPageLoader();"  OnTextChanged="expireDate_Changed"  />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtInsuredPeriodTo" runat="server" ControlToValidate="txtInsuredPeriodTo" ErrorMessage="Please enter policy end date" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                     <div class="clearfix"></div>
                                     <div class="form-group col-md-4" runat="server" id="divSeatingCapcity" visible="false">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Seating Capacity:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtSeatingCapcity" runat="server" CssClass="form-control col-md-10 onlynumber" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvxtSeatingCapcity" runat="server" ControlToValidate="txtSeatingCapcity" ErrorMessage="Please enter seating capcity" ValidationGroup="MotorCalculationValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-4" runat="server">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Remarks:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="control-label page-control">
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control col-md-10" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Button ID="ButtonAddNewCover" runat="server" Text="Optional Cover" OnClientClick="showPageLoader();" OnClick="ButtonAddNewCover_Click" CssClass="btn btn-primary" />
                           <div id="newadmindetails" runat="server">
                            <div class="form-group col-md-12 append-rows">
                                <div class="table-append-rows">
                                    <asp:GridView ID="Gridview1" Width="100%" OnRowDeleting="Gridview1_RowDeleting" OnRowDataBound="Gridview1_RowDataBound" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                                        <Columns>
                                            <%--<asp:BoundField DataField="RowNumber" HeaderText="Row Number" />--%>
                                            <asp:TemplateField HeaderText="Cover Code">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlNewCover" runat="server" onChange="showPageLoader();" AutoPostBack="true" OnSelectedIndexChanged="ddlCover_Changed">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" />
                                                <FooterTemplate>
                                                    <asp:Button ID="ButtonAdd" runat="server" Text="Add New Cover" OnClientClick="showPageLoader();" OnClick="ButtonAdd_Click" CssClass="btn btn-primary" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cover Description">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNewCoverDescription" Enabled="false" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cover Amount">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNewCoverAmount" CssClass="onlynumber" runat="server" Enabled="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" OnClientClick="showPageLoader();" CssClass="fsize fcolorred" CommandName="Delete"><i class="fa  fa-trash-o"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                           </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <%-- Payment Details--%>
                         <div class="x_panel"  runat="server" id="divPaymentSection">
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
                                                <asp:DropDownList ID="ddlPaymentMethods" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged" >
                                                   <asp:ListItem Text="--Please Select--" Value="-1" />
                                                    <asp:ListItem Value="0" Text="Cash"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="cheque"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Debit Card"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Credit Card"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator InitialValue="-1" CssClass="err" ID="rfvddlPaymentMethods" runat="server" ControlToValidate="ddlPaymentMethods" ErrorMessage="Please select payment method" ValidationGroup="MotorAuthValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Account Number:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control col-md-10" />
                                                  <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtAccountNo" ErrorMessage="Please enter account number" ControlToValidate="txtAccountNumber" runat="server" ValidationGroup="MotorAuthValidation" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                              </div>
                          </div>
                        <div class="x_panel">
                            <div class="x_content">
                                <div class="form-horizontal form-label-left col-md-12">
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
                                                             <asp:TextBox ID="txtVATAmount" Enabled="false"  runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
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
                                                             <asp:TextBox ID="txtTotalPremium" Enabled="false"  runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
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
                                                        <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Load Amount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtLoadAmount" AutoPostBack="true" onChange="showPageLoader();" OnTextChanged="txtLoad_AmountChanged" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="excludeDisc" class="dsc-overallwrapper vat_calcuation"  runat="server" visible="false">
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
                                                         </div>
                                                      <div class="dsc-wrapper"> 
                                                           <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Discount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtDiscount1" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>  
                                                          <div class="form-group col-md-12">
                                                            <div class="col-md-5 page-label">
                                                                <label class="control-label">Load Amount : </label>
                                                            </div>
                                                            <div class="col-md-7 page-control">
                                                                <div class="control-label">
                                                                    <asp:TextBox ID="txtLoadAmount1"  runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" Enabled="false"></asp:TextBox>
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
                                            <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CssClass="btn btn-primary" OnClientClick="showPageLoader('MotorCalculationValidation');" OnClick="Calculate_Click" TabIndex="-1" />
                                            <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="showPageLoader('MotorCalculationValidation');" OnClick="btnSubmit_Click"  TabIndex="-1" />
                                            <asp:Button ID="btnAuthorize" runat="server" Text="Authorize" data-target="#myModal" OnClientClick="ShowPopup();" CssClass="btn btn-primary" OnClick="btnAuthorize_Click" ValidationGroup="MotorInsuranceValidation" Enabled="false"  TabIndex="-1" />
                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-primary" OnClick="btnBack_Click" TabIndex="-1" />
                                            <a runat="server" id="downloadproposal" class="btn btn-primary" title="Proposal"><i class="fa fa-download" aria-hidden="true" tabindex="-1"></i>Proposal</a>
                                            <a runat="server" id="downloadschedule" class="btn btn-primary" title="Schedule"><i class="fa fa-download" aria-hidden="true" tabindex="-1"></i>Schedule</a>
                                            <a runat="server" id="downloadCertificate" class="btn btn-primary" title="Certificate"><i class="fa fa-download" aria-hidden="true" tabindex="-1"></i>Certificate</a>
                                            <asp:HiddenField ID="calculatedPremium" runat="server" />
                                            <asp:HiddenField ID="calculatedCommision" runat="server" />
                                            <asp:HiddenField ID="insuredDOB" runat="server" />
                                            <asp:HiddenField ID="formMotorCalculated" Value="false" runat="server" />
                                            <asp:HiddenField ID="formMotorSubmitted" Value="false" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>                       
                    </div>
                <%-- <asp:Panel runat="server" ID="Panel1" CssClass="modalpopup"> 
                    <div class="col-md-12 model-container" style="max-height: 500px; overflow: auto;">
                     <div class="model-header-title">
                        <h3>Policy Details</h3>
                         <span><i class="fa fa-close"></i></span>
                     </div>
                         <div class="form-group col-md-12">
                       <asp:GridView ID="gvPolicyDetails" runat="server" OnDataBound="gvPolicyDetails_DataBound" OnRowDataBound="gvPolicyDetails_RowDataBound" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="100">
                     <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>                                                           
                            </ItemTemplate>
                        </asp:TemplateField>                      
                        <asp:BoundField DataField="DocumentNo" HeaderText="DocumentNo" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide" />
                        <asp:BoundField DataField="MotorID"  Visible="false" />
                        <asp:BoundField DataField="PolicyStartDate" HeaderText="Start Date" SortExpression="Name" />
                        <asp:BoundField DataField="PolicyEndDate" HeaderText="End Date" SortExpression="Name" />
                        <asp:BoundField DataField="RenewalCount" HeaderText="Renewal Count" SortExpression="Name" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnReview" runat="server" ToolTip="Review" CssClass="fsize fcolorred" 
                                OnClientClick="showPageLoader();" OnClick="View_PolicyByRenewalCount"
                                CommandName="Authorize"><i class="fa fa-eye"></i></asp:LinkButton>                               
                            </ItemTemplate>
                        </asp:TemplateField>                        
                    </Columns>
                   </asp:GridView>
                  </div>                
                 </div>
                 </asp:Panel>--%>
                 <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" BehaviorID ="ModalBehaviour" runat="server" TargetControlID="btnBack" PopupControlID="Panel1"  BackgroundCssClass="modalBackground" >
                 </ajaxToolkit:ModalPopupExtender>--%> 
                <asp:Panel runat="server">
                        <div class="container" runat="server">
                            <div class="row">
                                <div class="modal" id="myModal" data-backdrop="static">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                    <span aria-hidden="true">&times;</span></button>
                                                <h4 class="modal-title">Motor Insurance</h4>
                                            </div>
                                            <div class="modal-body" runat="server" id="modalBodyText">
                                                Are you sure want to authorize this policy?
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnYes" type="button" Text="Yes" OnClientClick="showPageLoader('MotorCalculationValidation');" OnClick="Auth" runat="server" CssClass="btn btn-primary" />                                             
                                                <asp:Button ID="btnOK" type="button" OnClientClick="closePopup();" OnClick="Reset_Content" Text="No" runat="server" CssClass="btn btn-primary" />
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
                                                <h4 class="modal-title">Motor Insurance</h4>
                                            </div>
                                            <div class="modal-body" runat="server" id="Div1">
                                                This policy is endorsend,for more details see any endorsement page.
                                            </div>
                                            <div class="modal-footer">                                             
                                                <asp:Button ID="Button4" type="button" OnClientClick="closeEndorsementPopup();" Text="OK" runat="server" CssClass="btn btn-primary" />
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
</asp:Content>