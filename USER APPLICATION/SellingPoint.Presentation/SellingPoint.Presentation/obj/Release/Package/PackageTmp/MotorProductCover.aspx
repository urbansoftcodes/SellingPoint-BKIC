<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="MotorProductCover.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.MotorProductCover" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateKeyHide {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="upnlMotorProductCover">
            <ContentTemplate>
                <%-- <div class="page-header">
							<h1>Motor Product Cover </h1>
			    </div>--%>
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Motor Product Cover :</h2>
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
                                            <asp:DropDownList ID="ddlCover" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" onChange="showPageLoader();" OnSelectedIndexChanged="MotorProduct_changed" TabIndex="16">
                                                <asp:ListItem Text="Select" Value="-1" />
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlCover" ErrorMessage="Please select cover" ControlToValidate="ddlCover" runat="server" ValidationGroup="MotorCalculationValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Cover Code:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtCoverCode" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtCoverCode" runat="server" ControlToValidate="txtCoverCode" ErrorMessage="Please enter cover code" ValidationGroup="MotorProductValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Cover Description:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtCoverDescription" runat="server" CssClass="form-control col-md-10" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtCoverDescription" runat="server" ControlToValidate="txtCoverDescription" ErrorMessage="Please enter cover description" ValidationGroup="MotorProductValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Cover amount:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtCoverAmount" runat="server" CssClass="form-control col-md-10 onlynumbernotallowminus" />
                                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtCoverAmount" runat="server" ControlToValidate="txtCoverAmount" ErrorMessage="Please enter cover amount" ValidationGroup="MotorProductValidation" />
                                        </div>
                                    </div>
                                </div>    
                                 <div class="clearfix"></div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Optional Cover:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                           <asp:CheckBox  ID="chkIsOptionalCover" runat="server" CssClass="col-md-10"/>
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
                                <h2>Manage Motor Covers:</h2>
                                <ul class="nav navbar-right panel_toolbox">
                                    <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                    </li>

                                </ul>
                                <div class="clearfix"></div>
                            </div>

                            <div class="panel-body">
                                <div class="adv-table editable-table">
                                    <div class="x_content">

                                        <asp:GridView ID="gvMotorProductCover" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10"
                                            OnPageIndexChanging="gv_PageIndexChanging"
                                            OnDataBound="gvMotorInsurance_DataBound">
                                            <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ID" HeaderText="CoverId" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide" />
                                                <asp:BoundField DataField="CoverCode" HeaderText="Cover Code" SortExpression="Name" />
                                                <asp:BoundField DataField="CoverDescription" HeaderText="Cover Description" SortExpression="Name" />
                                                <asp:BoundField DataField="CoverAmount" HeaderText="Cover Amount" SortExpression="Name"/>
                                                <asp:BoundField DataField="IsOptional" HeaderText="Optional Cover" SortExpression="Name" />
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
