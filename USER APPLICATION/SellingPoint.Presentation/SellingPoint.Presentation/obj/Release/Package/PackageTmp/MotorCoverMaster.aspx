<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="MotorCoverMaster.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.MotorCoverMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
    .DateKeyHide
    {
        display: none;
    }
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="upnlMotorProductMaster">
            <ContentTemplate>
                <%--<div class="page-header">
							<h1>Motor Product Master </h1>
			    </div>--%>
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Motor Cover Master:</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="form-horizontal form-label-left">

                                <div class="form-group">
                                    <label class="control-label col-md-1 col-sm-3 col-xs-12">Cover Code:</label>
                                    <div class="col-md-3 col-sm-9 col-xs-12">
                                         <asp:TextBox ID="txtCoverCode" runat="server" CssClass="form-control col-md-10"/>
                                         <asp:RequiredFieldValidator CssClass="err" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCoverDescription" ErrorMessage="Please enter txtCoverDescription" ValidationGroup ="MotorProductMasterValidation"/>
                                    </div>
                                    <label class="control-label col-md-2 col-sm-3 col-xs-12">Cover Description:</label>
                                    <div class="col-md-6 col-sm-9 col-xs-12">
                                         <asp:TextBox ID="txtCoverDescription" runat="server" CssClass="form-control col-md-10"/>
                                         <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtCoverDescription" runat="server" ControlToValidate="txtCoverDescription" ErrorMessage="Please enter txtCoverDescription" ValidationGroup ="MotorProductMasterValidation"/>
                                    </div>

                                </div>

                              
                                <div class="clearfix"></div>
                                <div class="ln_solid"></div>
                                <div class="form-group">
                                    <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" Class="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="MotorProductMasterValidation" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Class="btn btn-warning" OnClick="btnCancel_Click"/>
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
                            <h2>Manage Motor Cover:</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
         <asp:GridView ID="gvMotorMaster" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10" 
             OnPageIndexChanging="gv_PageIndexChanging"
             OnDataBound="gvMotorInsurance_DataBound">
                            <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                <asp:BoundField DataField="CoverId" HeaderText="Cover Id" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide"/>
                                <asp:BoundField DataField="CoversCode" HeaderText="Cover Code" SortExpression="Name" />
                                <asp:BoundField DataField="CoversDescription" HeaderText="Cover Description" SortExpression="Name" />

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnEdit" runat="server" ToolTip="Edit" CssClass="fsize fcolorred" OnClick="lnkbtnEdit_Click"><i class="fa  fa-edit"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" CssClass="fsize fcolorred" OnClick="lnkbtnDelete_Click" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?');"><i class="fa  fa-trash-o"></i></asp:LinkButton>             
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
