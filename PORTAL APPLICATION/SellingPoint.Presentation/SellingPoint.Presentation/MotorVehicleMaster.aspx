<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="MotorVehicleMaster.aspx.cs" Inherits="SellingPoint.Presentation.MotorVehicleMaster" %>

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
							<h1>Motor Vehicle </h1>
			    </div>
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Motor Vehicle :</h2>
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
                                        <label class="control-label">Make :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                            <asp:TextBox ID="txtMake" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtMake" runat="server" ControlToValidate="txtMake" ErrorMessage="Please enter make" ValidationGroup="MotorVehicleValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Model :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtModel" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtModel" runat="server" ControlToValidate="txtModel" ErrorMessage="Please enter model" ValidationGroup="MotorVehicleValidation" />
                                        </div>
                                    </div>
                                </div>
                              <%--    <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Type:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control col-md-10 chzn-select">
                                                <asp:ListItem Text="Select" Value="-1" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlType" ErrorMessage="Please select type" ControlToValidate="ddlType" runat="server" ValidationGroup="MotorVehicleValidation" />
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Capacity:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtCapacity" runat="server" ControlToValidate="txtCapacity" ErrorMessage="Please enter cover amount" ValidationGroup="MotorVehicleValidation" />
                                        </div>
                                    </div>
                                </div>    
                                 
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Vehicle Value:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtVehicleValue" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtVehicleValue" runat="server" ControlToValidate="txtVehicleValue" ErrorMessage="Please enter vehicle value" ValidationGroup="MotorVehicleValidation" />
                                        </div>
                                    </div>
                                </div> 
                                <div class="clearfix"></div>
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Excess:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtExcess" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtExcess" runat="server" ControlToValidate="txtExcess" ErrorMessage="Please enter excess" ValidationGroup="MotorVehicleValidation" />
                                        </div>
                                    </div>
                                </div>
                                    <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Body Type:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtBodyType" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtBodyType" runat="server" ControlToValidate="txtBodyType" ErrorMessage="Please enter body type" ValidationGroup="MotorVehicleValidation" />
                                        </div>
                                    </div>
                                </div>
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Year:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtYear" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBodyType" ErrorMessage="Please enter body type" ValidationGroup="MotorVehicleValidation" />
                                        </div>
                                    </div>
                                </div>                              
                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Seating Capacity:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtSeatingCapacity" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBodyType" ErrorMessage="Please enter body type" ValidationGroup="MotorVehicleValidation" />
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
                                <h2>Manage Motor Vehicle:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>

                                </ul>
                                <div class="clearfix"></div>
                                <div class="manage-motor-vehicle-row col-md-12">
                                    <asp:TextBox ID="txtMakeSearch" runat="server" CssClass="col-md-4" />
                                    <asp:Button ID="btnMakeSearch" runat="server" Class="btn btn-primary" Text="Search" OnClientClick="showPageLoader();" OnClick="btnMakeSearch_Click" />
                                </div>
                            </div>

                            <div class="panel-body">
                                <div class="adv-table editable-table">
                                    <div class="x_content">

                                        <asp:GridView ID="gvMotorVehicle" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10"
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
                                                <asp:BoundField DataField="Make" HeaderText="Make" SortExpression="Name"/>
                                                <asp:BoundField DataField="Model" HeaderText="Model" SortExpression="Name" />
                                                <asp:BoundField DataField="Tonnage" HeaderText="Capacity" SortExpression="Name" />
                                                <asp:BoundField DataField="VehicleValue" HeaderText="Vehicle Value" SortExpression="Name"/>
                                                <asp:BoundField DataField="NewExcessAmount" HeaderText="Excess" SortExpression="Name" />
                                                <asp:BoundField DataField="Body" HeaderText="Body Type" SortExpression="Name" />
                                                <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Name" />
                                                <asp:BoundField DataField="SeatingCapacity" HeaderText="Seating Capacity" SortExpression="Name" />
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkbtnEdit" runat="server" ToolTip="Edit" CssClass="fsize fcolorred" OnClientClick="showPageLoader();" OnClick="lnkbtnEdit_Click"><i class="fa  fa-edit"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" CssClass="fsize fcolorred"  OnClick="lnkbtnDelete_Click" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?');"><i class="fa  fa-trash-o"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

