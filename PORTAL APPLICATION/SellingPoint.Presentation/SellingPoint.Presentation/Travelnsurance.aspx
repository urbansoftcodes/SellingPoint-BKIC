<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" Async="true" CodeBehind="Travelnsurance.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.Travelnsurance" %>

<%@ MasterType VirtualPath="~/General.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowPopup() {
            if (checkPageIsValid()) {
                 $(".modal-backdrop").remove();
                $('#myModal').modal('show');
            }
            else {
                showPageLoader('travelValidation,travelAuthValidation');
                $('#myModal').modal('hide');
                $(".modal-backdrop").remove();
            }
        }
        function closePopup() {
            $('#myModal').modal('hide');
            $(".modal-backdrop").remove();
        }
        function checkPageIsValid() {
            // return Page_ClientValidate('travelValidation');
            var valid = false;
            if (Page_ClientValidate('travelValidation') && Page_ClientValidate('travelAuthValidation')) {
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
             getTravelPolicies();
        });  

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="TravelUpdatePanel">
            <ContentTemplate>
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Travel Policy Search :</h2>
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
                                 <asp:TextBox ID="txtTravelPolicySearch" runat="server" CssClass="form-control" autocomplete="off" Width="300"/> 
                                 </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-12 pull-left form-grid-3-btn form-grid-3-btn-small">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Review" CssClass="btn btn-primary" OnClick="btnPolicy_Click" ValidationGroup="PolicyCheck" OnClientClick="showPageLoader();" />
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClientClick="showPageLoader();" OnClick="btnClear_Click" CssClass="btn btn-primary" ValidationGroup="PolicyCheck"/>                                    
                                </div>
                                </div>
                            <div class="form-group col-md-4">
                                 <div class="col-md-12 page-button-wrap form-grid-3-btn form-grid-3-btn-right">
                                    <asp:Button ID="linkButton" type="button" Text="Add Insured" CssClass="btn btn-info btn-link-button" runat="server" OnClick="insured_Master" />
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
                                    <asp:TextBox id="txtCPRSearch" runat="server" CssClass="form-control onlynumber" autocomplete="off" Width="300" OnTextChanged="txtCPR_Changed" AutoPostBack="true" onChange="showPageLoader()"/>
                                     <asp:RequiredFieldValidator ID="rfvtxtCPRSearch" CssClass="err" ErrorMessage="Please enter CPR" SetFocusOnError="true" ControlToValidate="txtCPRSearch" runat="server" ValidationGroup="travelValidation" /> 
                                </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-6 page-label form-grid-3-label">
                                    <label class="control-label">Branch:</label>
                                </div>
                                <div class="col-md-6 page-label form-grid-3-input">
                                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control col-md-10 chzn-select">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlBranch" ErrorMessage="Please select branch" ControlToValidate="ddlBranch" runat="server" ValidationGroup="travelValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-6 page-label form-grid-3-label">
                                    <label class="control-label">Introduced By:</label>
                                </div>
                                <div class="col-md-6 page-label form-grid-3-input">
                                    <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control col-md-10 chzn-select">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlUsers" ErrorMessage="Please select introducedby" ControlToValidate="ddlUsers" runat="server" ValidationGroup="travelValidation" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="subpanel" runat="server">
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Travel Insurance :</h2>
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
                                        <asp:TextBox ID="txtCPR" runat="server" CssClass="form-control col-md-10" OnClick="populate_fileds" Enabled="false"/>
                                        <asp:RequiredFieldValidator ID="rfvtxtCPR" ErrorMessage="Please enter CPR" SetFocusOnError="true" ControlToValidate="txtCPR" runat="server" ValidationGroup="travelValidation" />
                                    </div>
                                </div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Client Code:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtClientCode" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="RequiredFieldValidator2" ErrorMessage="Please enter client code" ControlToValidate="txtClientCode" runat="server" ValidationGroup="travelValidation" />
                                    </div>
                                </div>

                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Insured Name:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtInsuredName" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvInsuredName" ErrorMessage="Please select insuredname" ControlToValidate="txtInsuredName" runat="server" ValidationGroup="travelValidation" />
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Insured Age:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtInsuredAge" runat="server" CssClass="form-control col-md-10 policydate" Enabled="false"/>                                       
                                    </div>
                                </div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Package:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                            <asp:DropDownList ID="ddlPackage" runat="server" CssClass="form-control chzn-select col-md-10" onChange="showPageLoader();" AutoPostBack="True" OnSelectedIndexChanged="ddlPackage_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlPackage" ErrorMessage="Please select package" ControlToValidate="ddlPackage" runat="server" ValidationGroup="travelValidation" />
                                        </div>                                      
                                    </div>
                                </div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Coverage(Journey) *</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:DropDownList ID="ddlJourney" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlJourney_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvddlJourney" ErrorMessage="Please select journey" ControlToValidate="ddlJourney" runat="server" ValidationGroup="travelValidation" />
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Period *</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" onChange="showPageLoader();" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvddlPeriod" ErrorMessage="Please select period" ControlToValidate="ddlPeriod" runat="server" ValidationGroup="travelValidation" />                                     
                                    </div>
                                </div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Insurance From *</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox type="text" runat="server" ID="txtInsuranceFrom" CssClass="form-control col-md-10 policydate" AutoPostBack="true" onChange="showPageLoader();" OnTextChanged="calculate_expiredate"/>
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvInsuranceFrom" ControlToValidate="txtInsuranceFrom" ValidationGroup="travelValidation" runat="server" ErrorMessage="Please select insurance from date"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Insurance To:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox runat="server" ID="txtInsuranceTo" CssClass="form-control col-md-10 datepicker" Enabled="false" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvInsuranceTo" ErrorMessage="Please select insurance to date" ControlToValidate="txtInsuranceTo" runat="server" ValidationGroup="travelValidation" />
                                    </div>
                                </div>

                                <div class="clearfix"></div>
                                <div class="form-group col-md-8">
                                    <div class="col-md-6 page-label">
                                        <label class="control-label">Do any of the persons to be insured have any Physical Defect, Infirmity, Abnormality, or Medical Condition?</label>
                                    </div>

                                    <div class="col-md-6 page-control pull-left">
                                        <asp:DropDownList ID="ddlPhydefect" runat="server" onChange="showPageLoader();" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlPhydefect_SelectedIndexChanged">
                                            <asp:ListItem Text="--Select--" Value="-1" />
                                            <asp:ListItem>Yes</asp:ListItem>
                                            <asp:ListItem>No</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="err" InitialValue="-1" ID="rfvddlPhydefect" ErrorMessage="Please select physical defect" ControlToValidate="ddlPhydefect" runat="server" ValidationGroup="travelValidation" />
                                    </div>
                                </div>

                                <div class="form-group col-md-4">
                                    <div runat="server" id="phyDefect">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">If Yes, Please enter details</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <asp:TextBox TextMode="MultiLine" runat="server" ID="txtPhysicalDesc" class="form-control col-md-10" ></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtPhysicalDesc" ErrorMessage="Please select enter defect description" ControlToValidate="txtPhysicalDesc" runat="server" ValidationGroup="travelValidation" />
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
                            </div>
                            <div class="clearfix"></div>
                        </div>

                        <div id="admindetails" runat="server">
                            <div class="form-group col-md-12 append-rows">
                                <h2>Dependent details:</h2>
                                <div class="table-append-rows">
                                    <asp:GridView ID="Gridview1" Width="100%" OnRowDataBound="Gridview1_RowDataBound" OnRowDeleting="Gridview1_RowDeleting" runat="server" ShowFooter="true" AutoGenerateColumns="false" CssClass="table-striped">
                                        <Columns>
                                            <%--<asp:BoundField DataField="RowNumber" HeaderText="Row Number" />--%>
                                            <asp:TemplateField HeaderText="Insured Name">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtMemberName" CssClass="uppercase" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter name" ControlToValidate="txtMemberName" runat="server" ValidationGroup="travelValidation" />
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Left" />
                                                <FooterTemplate>
                                                    <asp:Button ID="ButtonAdd" runat="server" OnClientClick="showPageLoader();" Text="Add New Dependent" OnClick="ButtonAdd_Click" CssClass="btn btn-primary" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Relationship">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlRelation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRelation_Changed">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please select relationship" ControlToValidate="ddlRelation" runat="server" ValidationGroup="travelValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="dateofbirth"></asp:TextBox>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter date of birth" ControlToValidate="txtDOB" runat="server" ValidationGroup="travelValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Passport No">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPassport" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter CPR no" ControlToValidate="txtPassport" runat="server" ValidationGroup="travelValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nationality">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlNational" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                        <asp:ListItem Text="--Please Select--" Value="-1" />
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please select national" ControlToValidate="ddlNational" runat="server" ValidationGroup="travelValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Occupation">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOccupation" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter occupation" ControlToValidate="txtOccupation" runat="server" ValidationGroup="travelValidation" />
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
                    </div>
                    <div class="x_panel" runat="server" id="divPaymentSection">
                        <div class="x_title">
                            <h2>Payment Details: :</h2>
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
                                        <label class="control-label">Payment Method:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged">
                                                <asp:ListItem Text="--Please Select--" Value="-1" />
                                                <asp:ListItem Value="0" Text="Cash"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="cheque"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Debit Card"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Credit Card"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlPaymentMethod" ErrorMessage="Please select payment method" ControlToValidate="ddlPaymentMethod" runat="server" ValidationGroup="travelAuthValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Account No:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control col-md-10"/>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtAccountNo" ErrorMessage="Please enter account number" ControlToValidate="txtAccountNo" runat="server" ValidationGroup="travelAuthValidation" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="x_panel">
                        <div class="x_content">
                            <div class="form-horizontal form-label-left col-md-12">
                                <div class="col-md-12 form-group">
                                    <div runat="server" id="amtDisplay" class="calculate-amount">
                                        <div class="dsc-overallwrapper" id="includeDisc" runat="server" visible="false">
                                            <div class="dsc-wrapper">
                                                <div class="col-md-2"></div>
                                                <div class="form-group col-md-10">
                                                    <div class="col-md-4 page-label">
                                                        <label class="control-label">PremiumAmount : </label>
                                                    </div>
                                                    <div class="col-md-7 page-control">
                                                        <div class="control-label">
                                                            <asp:TextBox ID="premiumAmount" Enabled="false" AutoPostBack="true" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1"></div>
                                                </div>
                                                <div class="clearfix"></div>
                                                <div class="col-md-2"></div>
                                                <div class="form-group col-md-10">
                                                    <div class="col-md-4 page-label">
                                                        <label class="control-label">Commission : </label>
                                                    </div>
                                                    <div class="col-md-7 page-control">
                                                        <div class="control-label">
                                                            <asp:TextBox ID="commission" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1"></div>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>
                                            <div class="dsc-wrapper">
                                                <div class="form-group col-md-10">
                                                    <div class="col-md-4 page-label">
                                                        <label class="control-label">Discount : </label>
                                                    </div>
                                                    <div class="col-md-7 page-control">
                                                        <div class="control-label">
                                                            <asp:TextBox ID="txtDiscount" AutoPostBack="true" onChange="showPageLoader();" OnTextChanged="validate_Premium" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1"></div>
                                                </div>
                                                <div class="col-md-2"></div>
                                                <div class="clearfix"></div>
                                            </div>
                                        </div>
                                        <div id="excludeDisc" runat="server" visible="false">
                                            <div class="col-md-3"></div>
                                            <div class="form-group col-md-6">
                                                <div class="col-md-4 page-label">
                                                    <label class="control-label">PremiumAmount : </label>
                                                </div>
                                                <div class="col-md-4 page-control">
                                                    <div class="control-label">
                                                        <asp:TextBox ID="premiumAmount1" Enabled="false" AutoPostBack="true" runat="server" OnTextChanged="validate_Premium" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-4"></div>
                                            </div>
                                            <div class="col-md-3"></div>
                                            <div class="clearfix"></div>
                                            <div class="col-md-3"></div>
                                            <div class="form-group col-md-6">
                                                <div class="col-md-4 page-label">
                                                    <label class="control-label">Commission : </label>
                                                </div>
                                                <div class="col-md-4 page-control">
                                                    <div class="control-label">
                                                        <asp:TextBox ID="commission1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-4"></div>
                                            </div>
                                             <div class="form-group col-md-6">
                                                <div class="col-md-4 page-label">
                                                    <label class="control-label">Discount : </label>
                                                </div>
                                                <div class="col-md-4 page-control">
                                                    <div class="control-label">
                                                        <asp:TextBox ID="txtDiscount1" runat="server" CssClass="form-control col-md-10 onlynumber" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-4"></div>
                                            </div>
                                            <div class="col-md-3"></div>
                                            <div class="clearfix"></div>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>

                                    <div class="form-group">
                                        <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                            <div runat="server" id="successDiv">
                                                <h4><span id="successMsg" runat="server"></span></h4>
                                            </div>
                                            <asp:Button ID="btnCalculate" CssClass="btn btn-primary" runat="server" Text="Calculate" ValidationGroup="travelValidation" OnClientClick="showPageLoader('travelValidation');" OnClick="btnCalculate_Click" />
                                            <asp:Button ID="btnTravelSave" CssClass="btn btn-primary" runat="server" Text="Save" ValidationGroup="travelValidation" OnClientClick="showPageLoader('travelValidation,travelAuthValidation');" OnClick="btnTravelSave_Click" />
                                            <asp:Button ID="btnAuthorize" data-target="#myModal" CssClass="btn btn-primary" OnClientClick="ShowPopup();" runat="server" Text="Authorize" OnClick="btnAuthorize_Click" Enabled="false" />
                                            <asp:Button ID="btnBack" CssClass="btn btn-primary" runat="server" Text="Back" OnClick="btnBack_Click" />
                                            <a runat="server" id="downloadschedule" class="btn btn-primary" title="Schedule" onclick="btnPrint_Click"><i class="fa fa-download" aria-hidden="true"></i>Schedule</a>
                                            <asp:HiddenField ID="calculatedPremium" runat="server" />
                                            <asp:HiddenField ID="calculatedCommision" runat="server" />
                                            <asp:HiddenField ID="formTravelSubmitted" Value="false" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:HiddenField runat="server" ID="TravelID" />
                <asp:Panel runat="server">
                    <div class="container" runat="server">
                        <div class="row">
                            <div class="modal" id="myModal">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span></button>
                                            <h4 class="modal-title">Travel Insurance</h4>
                                        </div>
                                        <div class="modal-body" runat="server" id="modalBodyText">
                                            Are you sure want to authorize this policy?
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnYes" type="button" Text="Yes" OnClientClick="showPageLoader();" OnClick="Auth" runat="server" CssClass="btn btn-primary" />
                                            <%--<button type="button" runat="server" id="btnClose"  data-dismiss="modal" Class="btn btn-primary">Close</button>--%>
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
                                            <h4 class="modal-title">Travel Insurance</h4>
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
</asp:Content>