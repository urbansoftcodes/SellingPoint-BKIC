<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="MotorMakeYear.aspx.cs" Inherits="SellingPoint.Presentation.MotorMakeYear" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateKeyHide {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="upnlMotorVehicle">
            <ContentTemplate>
                 <div class="page-header">
							<h1>Motor Vehicle Year</h1>
			    </div>
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Motor Vehicle Year :</h2>
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
                                        <label class="control-label">Manufacturar YEAR:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                            <asp:TextBox ID="txtManufacturarYear" runat="server" CssClass="form-control col-md-10 onlynumber" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtManufacturarYear" runat="server" ControlToValidate="txtManufacturarYear" ErrorMessage="Please enter manufacturer year" ValidationGroup="MotorVehicleValidation" />
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

                    <div>
                        <div class="x_panel">
                            <div class="x_title">
                                <h2>Manage Motor Year:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>

                                </ul>
                                <div class="clearfix"></div>                                
                            </div>
                            <div class="panel-body">
                                <div class="adv-table editable-table">
                                    <div class="x_content">    
                                        <asp:GridView ID="gvMotorYear" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10"
                                            OnPageIndexChanging="gv_PageIndexChanging"
                                            OnDataBound="gvMotorVehicle_DataBound">
                                            <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide" />
                                                <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Name"/>
                                                </Columns>
                                            </asp:GridView>
                                    </div>
                                </div>
                                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
