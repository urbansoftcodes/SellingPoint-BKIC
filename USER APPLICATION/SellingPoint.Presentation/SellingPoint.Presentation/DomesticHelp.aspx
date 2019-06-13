<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="DomesticHelp.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.DomesticHelp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowPopup() {
            if (checkPageIsValid()) {
                $(".modal-backdrop").remove();
                $('#myModal').modal('show');
            }
            else {
                showPageLoader('domesticValidation,domesticAuthValidation');
                $('#myModal').modal('hide');
                $(".modal-backdrop").remove();
            }
        }
        function checkPageIsValid() {
            var valid = false;
            if (Page_ClientValidate('domesticValidation') && Page_ClientValidate('domesticAuthValidation')) {
                valid = true;
            }
            return valid;
        }
        function closePopup() {
            $('#myModal').modal('hide');
            $(".modal-backdrop").remove();
        }
         $(function () {
             autocompleteCPR();
             getDomesticPolicies();
        });        
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="domesticsUpdatePanel">
        <ContentTemplate>
            <div class="x_panel">
                <div class="x_title">
                    <h2>Domestic Policy Search :</h2>
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
                               <asp:TextBox ID="txtDomesticPolicySearch" runat="server" CssClass="form-control" autocomplete="off" Width="300"/>   
                            </div>
                        </div> 
                        <div class="form-group col-md-4">
                            <div class="col-md-12 pull-left form-grid-3-btn form-grid-3-btn-small">
                                <asp:Button ID="btnSearch" runat="server" Text="Review" CssClass="btn btn-primary" OnClientClick="showPageLoader();" OnClick="btnPolicy_Click" ValidationGroup="PolicyCheck" TabIndex="2" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClientClick="showPageLoader();" OnClick="btnClear_Click" CssClass="btn btn-primary" ValidationGroup="PolicyCheck" TabIndex="3" />
                            </div>
                            </div>
                             <div class="form-group col-md-4">
                                    <div class="col-md-12 page-button-wrap form-grid-3-btn form-grid-3-btn-right">
                                 <asp:Button ID="linkButton" type="button" Text="Add Insured" CssClass="btn btn-info btn-link-button" runat="server" OnClick="insured_Master" TabIndex="5" />
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
                                <asp:TextBox ID="txtCPRSearch" runat="server" CssClass="form-control onlynumber" autocomplete="off" Width="300" AutoPostBack="true" OnTextChanged="txtCPR_Changed" onChange="showPageLoader();"/>
                                 <asp:RequiredFieldValidator ID="rfvtxtCPRSearch" CssClass="err" ErrorMessage="Please select CPR" SetFocusOnError="true" ControlToValidate="txtCPRSearch" runat="server" ValidationGroup="domesticValidation" />                                
                            </div>
                               </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-6 page-label form-grid-3-label">
                                    <label class="control-label">Branch: *</label>
                                </div>
                                <div class="col-md-6 page-control form-grid-3-input">
                                    <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control col-md-10 chzn-select" TabIndex="6">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" SetFocusOnError="true" ID="rfvddlBranch" ErrorMessage="Please select branch" ControlToValidate="ddlBranch" runat="server" ValidationGroup="domesticValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-6 page-label form-grid-3-label">
                                    <label class="control-label">Introduced By:</label>
                                </div>
                                <div class="col-md-6 page-control form-grid-3-input">
                                    <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control col-md-10 chzn-select" TabIndex="7">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvddlUsers" ErrorMessage="Please select introducedby" ControlToValidate="ddlUsers" runat="server" ValidationGroup="domesticValidation" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="subpanel" runat="server">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Domestic Insurance :</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <asp:HiddenField runat="server" ID="DomesticID" />
                        <asp:HiddenField runat="server" ID="CPR" />
                        <div class="form-horizontal form-label-left col-md-12">
                            <div class="form-group col-md-4">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">CPR:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <asp:TextBox ID="txtCPR" runat="server" CssClass="form-control col-md-10" OnClick="populate_fileds" Enabled="false" />
                                    <asp:RequiredFieldValidator ID="rfvtxtCPR" ErrorMessage="Please enter CPR" SetFocusOnError="true" ControlToValidate="txtCPR" runat="server" ValidationGroup ="domesticValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Client Code: *</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <asp:TextBox runat="server" ID="txtClientCode" CssClass="form-control col-md-10" Enabled="false"/>
                                    <asp:RequiredFieldValidator CssClass="err" SetFocusOnError="true" ID="rfvtxtClientCode" ErrorMessage="Please enter client code" ControlToValidate="txtClientCode" runat="server" ValidationGroup="domesticValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Insured Name: *</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <asp:TextBox ID="txtInsuredName" runat="server" CssClass="form-control col-md-10" Enabled="false"/>
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtInsuredName"  SetFocusOnError="true" ErrorMessage="Please enter insured name" ControlToValidate="txtInsuredName" runat="server" ValidationGroup="domesticValidation" />
                                </div>
                            </div>
                            <div class="clearfix"></div>                         
                            <div class="form-group col-md-4">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Number Of Years: *</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <asp:DropDownList ID="ddlNoOfYears" runat="server" CssClass="form-control col-md-10 chzn-select" CauseValidation="false" AutoPostBack="True" OnSelectedIndexChanged="ddlNoYears_SelectedIndexChanged" TabIndex="11">
                                        <asp:ListItem Text="--Select--" Value="-1" />
                                        <asp:ListItem Value="1">1 Year</asp:ListItem>
                                        <asp:ListItem Value="2">2 Years</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvddlNoOfYears" InitialValue="-1" CssClass="err" ErrorMessage="Please select number of years" SetFocusOnError="true" ControlToValidate="ddlNoOfYears" runat="server" ValidationGroup="domesticValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Insured Period From: *</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <asp:TextBox ID="txtPolicyStartDate" runat="server" CssClass="form-control col-md-10 policydate" CauseValidation="false" OnTextChanged="calculate_expiredate" AutoPostBack="true" TabIndex="12" />
                                    <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtPolicyStartDate" ErrorMessage="Please select policy start date" ControlToValidate="txtPolicyStartDate" runat="server" ValidationGroup="domesticValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-4">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Insured Period To: *</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <asp:TextBox runat="server" ID="txtPolicyEndDate" CssClass="form-control col-md-10 datepicker" Enabled="false" TabIndex="13" />
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="form-group col-md-4">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Issue Date:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control col-md-10 datepicker" Enabled="false" TabIndex="14" />
                                    <asp:RequiredFieldValidator CssClass="err" ID="RequiredFieldValidator6" ErrorMessage="Please enter issue date" ControlToValidate="txtIssueDate" runat="server" ValidationGroup="domesticValidation" />
                                </div>
                            </div>
                            <div class="form-group col-md-8">
                                <div class="col-md-8 page-label">
                                    <label class="control-label">Do any of the persons to be insured have any Physical Defect, Infirmity, Abnormality, or Medical Condition?</label>
                                </div>

                                <div class="col-md-4 page-control">
                                    <asp:DropDownList ID="ddlPhydefect" runat="server" CssClass="form-control col-md-10 chzn-select" CauseValidation="false" AutoPostBack="True" OnSelectedIndexChanged="ddlPhydefect_SelectedIndexChanged" TabIndex="15">
                                        <asp:ListItem Text="--Select--" Value="-1" />
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="-1" CssClass="err" ID="rfvddlPhydefect" ErrorMessage="Please select physical defect" ControlToValidate="ddlPhydefect" runat="server" ValidationGroup="domesticValidation" />
                                </div>
                            </div>

                            <div runat="server" id="phyDefect">
                                <div class="form-group col-md-4">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">If Yes, Please enter details</label>
                                    </div>
                                    <div class="col-md-8 page-control full-width-textarea">
                                        <asp:TextBox TextMode="MultiLine" runat="server" ID="txtPhysicalDesc" class="form-control col-md-10" TabIndex="18"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtPhysicalDesc" ErrorMessage="Please select enter defect description" ControlToValidate="txtPhysicalDesc" runat="server" ValidationGroup="travelValidation" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Remarks:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control col-md-10" TabIndex="32" />
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>

                        <div id="admindetails" runat="server">
                            <div class="form-group col-md-12 append-rows domestic_workers">
                                <h2>Domestic Worker details:</h2>
                                <div class="table-append-rows">
                                    <asp:GridView ID="Gridview1" Width="100%" OnRowDataBound="Gridview1_RowDataBound" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Name">

                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDomesticName" runat="server" TabIndex="17"></asp:TextBox>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter name" ControlToValidate="txtDomesticName" runat="server" ValidationGroup="domesticValidation" />
                                                </ItemTemplate>
                                                <%--<FooterStyle HorizontalAlign="Left" />--%>
                                                <%--<FooterTemplate>
                                            <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" OnClick="ButtonAdd_Click" CssClass="btn btn-primary" />
                                        </FooterTemplate>--%>
                                    </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sex">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlGender" runat="server" TabIndex="18">
                                                        <asp:ListItem Text="--Select--" Value="-1" />
                                                        <asp:ListItem Text="Male" Value="Male" />
                                                        <asp:ListItem Text="Female" Value="Female" />
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator CssClass="err" InitialValue="-1" ErrorMessage="Please select gender" ControlToValidate="ddlGender" runat="server" ValidationGroup="domesticValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Date Of Birth">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="datepickerAge18to55" TabIndex="19"></asp:TextBox>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter date of birth" ControlToValidate="txtDOB" runat="server" ValidationGroup="domesticValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Nationality">
                                                <ItemTemplate>
                                                    <asp:DropDownList Width="70px" ID="ddlNational" runat="server" CssClass="form-control col-md-10 chzn-select" TabIndex="20">
                                                        <%-- <asp:ListItem Text="Select" Value="-1" />--%>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please select nationality" ControlToValidate="ddlNational" runat="server" ValidationGroup="domesticValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CPR / Passport No">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPassport" runat="server" TabIndex="21"></asp:TextBox>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter passport No" ControlToValidate="txtPassport" runat="server" ValidationGroup="domesticValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Occupation">
                                                <ItemTemplate>
                                                    <asp:DropDownList Width="70px" ID="ddlDomesticOccupation" runat="server" CssClass="form-control col-md-10 chzn-select" TabIndex="22">
                                                        <%--<asp:ListItem Text="Select" Value="-1" />--%>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter occupation" ControlToValidate="ddlDomesticOccupation" runat="server" ValidationGroup="domesticValidation" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
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
                                    <label class="control-label">Payment Methods:</label>
                                </div>
                                <div class="col-md-8">
                                    <div class="control-label page-control">
                                        <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentMethod_SelectedIndexChanged" TabIndex="30">
                                            <asp:ListItem Text="--Please Select--" Value="-1" />
                                            <asp:ListItem Value="0" Text="Cash"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="cheque"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Debit Card"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Credit Card"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="-1" CssClass="err" ID="rfvddlPaymentMethod" runat="server" ControlToValidate="ddlPaymentMethod" ErrorMessage="Please select payment method" ValidationGroup="domesticAuthValidation" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Account Number:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label ">
                                        <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control col-md-10" TabIndex="31" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtAccountNo" ErrorMessage="Please enter account number" ControlToValidate="txtAccountNo" runat="server" ValidationGroup="domesticAuthValidation" />
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
                                                            <asp:TextBox ID="txtDiscount" AutoPostBack="true" onChange="showPageLoader();" OnTextChanged="validate_Premium" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
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
                                                            <asp:TextBox ID="txtDiscount1" runat="server" CssClass="form-control col-md-10 onlynumber" Enabled="false"></asp:TextBox>
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
                                <asp:Button ID="btnCalculate" CssClass="btn btn-primary" runat="server" TabIndex="100" Text="Calculate" ValidationGroup="domesticValidation" OnClientClick="showPageLoader('domesticValidation');" OnClick="btnCalculate_Click" />
                                <asp:Button ID="btnDomesticSave" CssClass="btn btn-primary" runat="server" TabIndex="101" Text="Save" ValidationGroup="domesticValidation" OnClientClick="showPageLoader('domesticValidation,domesticAuthValidation');" OnClick="btnDomesticSave_Click" />
                                <asp:Button ID="btnAuthorize" data-target="#myModal" CssClass="btn btn-primary" TabIndex="102" OnClientClick=" return ShowPopup();" runat="server" OnClick="btnAuthorize_Click" Text="Authorize" Enabled="false" />
                                <asp:Button ID="btnBack" CssClass="btn btn-primary" runat="server" Text="Back" TabIndex="103" OnClick="btnBack_Click" />
                                <a runat="server" id="downloadschedule" class="btn btn-primary" title="Schedule" tabindex="104" onclick="btnPrint_Click"><i class="fa fa-download" aria-hidden="true"></i>Schedule</a>
                                <asp:HiddenField ID="calculatedPremium" runat="server" />
                                <asp:HiddenField ID="calculatedCommision" runat="server" />
                                <asp:HiddenField ID="formDomesticSubmitted" Value="false" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div> 
            <asp:Panel runat="server">
                <div class="container" runat="server">
                    <div class="row">
                        <div class="modal" id="myModal">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">Domestic Help</h4>
                                    </div>
                                    <div class="modal-body" runat="server" id="modalBodyText">
                                        Are you sure want to authorize this policy?
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnYes" type="button" Text="Yes" OnClientClick="showPageLoader();" OnClick="Auth" runat="server" CssClass="btn btn-primary" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
