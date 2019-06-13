<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="AgentMaster.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.AgentMaster" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateKeyHide {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="upnlAgentMaster">
            <ContentTemplate>
                <%--  <div class="page-header">
                    <h1>Agent Master </h1>
                </div>--%>

                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Agent Master:</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="form-horizontal form-label-left col-md-12 margin-bott-fields">

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Agency:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                            <asp:TextBox ID="txtAgency" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvAgency" ErrorMessage="Please enter Agency" ControlToValidate="txtAgency" runat="server" ValidationGroup="AgentMasterValidation" />
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Agent Code:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtAgentCode" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvAgentCode" runat="server" ControlToValidate="txtAgentCode" ErrorMessage="Please enter Agency Code" ValidationGroup="AgentMasterValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Customer Code:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvCustomercode" runat="server" ControlToValidate="txtCustomerCode" ErrorMessage="Please enter Customer Code" ValidationGroup="AgentMasterValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="ln_solid"></div>
                                <div class="form-group">
                                    <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSubmit_Click" OnClientClick="showPageLoader('AgentMasterValidation');" ValidationGroup="AgentMasterValidation" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-warning" OnClick="btnCancel_Click" />
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>


                </div>

                <%--Grid--%>
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Manage Agents:</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <asp:GridView ID="gvAgentMaster" runat="server" DataKeyNames="Id" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10"
                                OnPageIndexChanging="gv_PageIndexChanging"
                                OnDataBound="gvMotorInsurance_DataBound" CssClass="table table-striped">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide" />
                                    <asp:BoundField DataField="Agency" HeaderText="Agency" SortExpression="Name" />
                                    <asp:BoundField DataField="AgentCode" HeaderText="Agent Code" SortExpression="Name" />
                                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" SortExpression="Name" />
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnEdit" runat="server" ToolTip="Edit" CssClass="fsize fcolorred" OnClientClick="showPageLoader();" OnClick="lnkbtnEdit_Click"><i class="fa  fa-edit"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" CssClass="fsize fcolorred" OnClick="lnkbtnDelete_Click" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?');"><i class="fa  fa-trash-o"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

<%--                <div class="x_panel">
                    <div class="x_title">
                        <h2>Category Master :</h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>

                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="x_content">
                        <div class="form-horizontal form-label-left col-md-12 margin-bott-fields">
                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Agency:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label ">
                                        <asp:DropDownList ID="ddlAgency" runat="server" CssClass="form-control col-md-10">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvddlAgency" ErrorMessage="Please select agency" ControlToValidate="ddlAgency" runat="server" ValidationGroup="categoryValidation" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Main Class:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox ID="txtMainclass" runat="server" CssClass="form-control col-md-10" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvMainclass" runat="server" ControlToValidate="txtMainclass" ErrorMessage="Please enter Main class" ValidationGroup="categoryValidation" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Sub Class:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox ID="txtSubClass" runat="server" CssClass="form-control col-md-10" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvSubClass" runat="server" ControlToValidate="txtSubClass" ErrorMessage="Please enter Sub Class" ValidationGroup="categoryValidation" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Category:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control col-md-10" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvCategory" runat="server" ControlToValidate="txtCategory" ErrorMessage="Please enter Category" ValidationGroup="categoryValidation" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Code:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control col-md-10" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvCode" runat="server" ControlToValidate="txtCode" ErrorMessage="Please enter Code" ValidationGroup="categoryValidation" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Value Type:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox ID="txtValueType" runat="server" CssClass="form-control col-md-10" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvValueType" runat="server" ControlToValidate="txtValueType" ErrorMessage="Please enter ValueType" ValidationGroup="categoryValidation" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Value:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control col-md-10  numbersOnly" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvValue" runat="server" ControlToValidate="txtValue" ErrorMessage="Please enter Value" ValidationGroup="categoryValidation" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Effective From:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox runat="server" ID="txtEffectiveFrom" DataFormatString="{0:dd/MM/yyyy}" CssClass="datepicker form-control col-md-10" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvEffectiveFrom" ControlToValidate="txtEffectiveFrom" ValidationGroup="categoryValidation" runat="server" ErrorMessage="Please select the EffectiveFrom Date"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <div class="col-md-4 page-label">
                                    <label class="control-label">Effective To:</label>
                                </div>
                                <div class="col-md-8 page-control">
                                    <div class="control-label">
                                        <asp:TextBox runat="server" ID="txtEffectiveTo" DataFormatString="{0:dd/MM/yyyy}" CssClass="datepicker form-control col-md-10" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvEffectiveTo" ControlToValidate="txtEffectiveTo" ValidationGroup="categoryValidation" runat="server" ErrorMessage="Please select the EffectiveTo Date"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="clearfix"></div>
                            <div class="ln_solid"></div>
                            <div class="form-group">
                                <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                    <asp:Button ID="btnCategorySubmit" runat="server" Text="Save" Class="btn btn-primary" OnClientClick="showPageLoader('categoryValidation');" OnClick="btnCategorySubmit_Click" ValidationGroup="CategoryValidation" />
                                    <asp:Button ID="btnCategoryCancel" runat="server" Text="Cancel" Class="btn btn-warning" OnClick="btnCategoryCancel_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <div>
                   <%-- <div class="x_panel">
                        <div class="x_title">
                            <h2>Manage Category:</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

                            <asp:GridView ID="gvCategoryMaster" runat="server" DataKeyNames="Id" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10"
                                OnPageIndexChanging="gv_CategoryPageIndexChanging"
                                OnDataBound="gvMotorInsurance_DataBound">
                                <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="id" HeaderText="Id" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide" />
                                    <asp:BoundField DataField="Agency" HeaderText="Agency" SortExpression="Name" />                                   
                                    <asp:BoundField DataField="MainClass" HeaderText="Main Class" SortExpression="Name" />
                                    <asp:BoundField DataField="SubClass" HeaderText="Sub Class" SortExpression="Name" />
                                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Name" />
                                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Name" />
                                    <asp:BoundField DataField="ValueType" HeaderText="Value Type" SortExpression="Name" />
                                    <asp:BoundField DataField="Value" HeaderText="Value" SortExpression="Name" />                                   

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnCategoryEdit" runat="server" ToolTip="Edit" CssClass="fsize fcolorred" OnClick="lnkbtnCategoryEdit_Click" OnClientClick="showPageLoader();"><i class="fa  fa-edit"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkbtnCategoryDelete" runat="server" ToolTip="Delete" CssClass="fsize fcolorred" OnClick="lnkbtnCategoryDelete_Click" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?');"><i class="fa  fa-trash-o"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>--%>
                </div>               
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
