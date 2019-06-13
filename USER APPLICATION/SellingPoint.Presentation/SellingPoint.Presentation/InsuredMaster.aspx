<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="InsuredMaster.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.InsuredMaster" %>

<%@ MasterType VirtualPath="~/General.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function ShowPopup() {           
                $(".modal-backdrop").remove();
                $('#cprWarning').modal('show');
            }
 function closePopup() {
            $('#cprWarning').modal('hide');
            $(".modal-backdrop").remove();
        }
</script>
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="upnlInsuredMaster">
            <ContentTemplate>
                <%-- <div class="page-header">
                    <h1>Insured Master </h1>
                </div>--%>
                 <div class="x_panel">
                     <div class="x_title">
                         <h2>Insured Search :</h2>
                         <ul class="nav navbar-right panel_toolbox">
                             <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                             </li>
                         </ul>
                         <div class="clearfix"></div>
                     </div>
                     <div class="x_content">
                         <div class="form-horizontal form-label-left col-md-6">
                             <asp:TextBox ID="txtSearchByCPR" runat="server"></asp:TextBox>
                         </div>
                         <div class="form-horizontal form-label-left col-md-6">
                               <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary onlynumber" OnClientClick="showPageLoader();" OnClick="txtSearch_ByCPR" CausesValidation="false" />    
                         </div>
                     </div>
                 </div>

                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Insured Master :</h2>
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
                                        <label class="control-label">CPR :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtCPR" runat="server" CssClass="form-control col-md-10 onlynumber" Onchange ="showPageLoader();" OnTextChanged="txtCPR_Changed" AutoPostBack="true" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtCPR" runat="server" ControlToValidate="txtCPR" ErrorMessage="Please enter cpr" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Passport No:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtPassport" runat="server" CssClass="form-control col-md-10" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">FirstName:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control col-md-10 oneName" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="Please enter first name" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">MiddleName:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtMiddleName" runat="server" CssClass="form-control col-md-10 oneName" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtMiddleName" runat="server" ControlToValidate="txtMiddleName" ErrorMessage="Please enter middle name" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">LastName:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control col-md-10 oneName" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Please enter last name" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Gender:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                <asp:ListItem Text="Select" Value="-1" />
                                                <asp:ListItem>Male</asp:ListItem>
                                                <asp:ListItem>Female</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="-1" CssClass="err" ID="rfvddlGender" runat="server" ControlToValidate="ddlGender" ErrorMessage="Please enter gender" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Flat:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtFlat" runat="server" CssClass="form-control col-md-10" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Building / House No:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtBuilding" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtBuilding" runat="server" ControlToValidate="txtBuilding" ErrorMessage="Please enter building or house no" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Road:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtRoad" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtRoad" runat="server" ControlToValidate="txtRoad" ErrorMessage="Please enter road" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Block:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtBlock" runat="server" CssClass="form-control col-md-10" Onchange ="showPageLoader()" OnTextChanged="BlockNumber_Changed" AutoPostBack="true" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtBlock"  runat="server" ControlToValidate="txtBlock" ErrorMessage="Please enter block" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Area:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control col-md-10 chzn-select">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlArea" CssClass="err" ErrorMessage="Please select area" ControlToValidate="ddlArea" runat="server" ValidationGroup="InsuredMasterValidation"  />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Mobile:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control col-md-10" />
                                            <%--<asp:RequiredFieldValidator CssClass="err" ID="rfvMobile" runat="server" ControlToValidate="txtMobile" ErrorMessage="Please enter mobile" ValidationGroup="InsuredMasterValidation" />--%>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Email:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control col-md-10 email" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter email" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">DateOfBirth:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control col-md-10 dateofbirth" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtDateOfBirth" runat="server" ControlToValidate="txtDateOfBirth" ErrorMessage="Please enter date of birth" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Nationality:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:DropDownList ID="ddlNationality" runat="server" CssClass="form-control col-md-10 chzn-select">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlNationality" runat="server" ControlToValidate="ddlNationality" ErrorMessage="Please enter nationality" ValidationGroup="InsuredMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Occupation:</label>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                            <asp:TextBox ID="txtOccupation" runat="server" CssClass="form-control col-md-10"  MaxLength="15"/>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtOccupation" runat="server" ControlToValidate="txtOccupation" ErrorMessage="Please enter occupation" ValidationGroup="InsuredMasterValidation" />
                                            </div>
                                        </div>
                                    </div>
                                <div class="form-group">
                                <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="showPageLoader('InsuredMasterValidation');" OnClick="btnSubmit_Click"/>    
                                     <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-warning" OnClick="btn_CancelClick" />
                                </div>
                            </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="ln_solid"></div>                           
                        </div>
                    </div>
                </div>                   
            </ContentTemplate>
        </asp:UpdatePanel>  
         
    </div>
</asp:Content>