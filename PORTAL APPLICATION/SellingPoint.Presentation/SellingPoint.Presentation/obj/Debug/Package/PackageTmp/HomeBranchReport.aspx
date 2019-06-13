﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="HomeBranchReport.aspx.cs"
    Inherits="BKIC.SellingPoint.Presentation.HomeBranchReport" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="panel">
        <header class="panel-heading tab-bg-dark-navy-blue fcolorwhite ">
            Home Branch Report
        </header>
    </section>
    <div class="form-group has-success search-header">
        <%--<label class="col-sm-2 control-label col-lg-2"></label>--%>
        <asp:Label runat="server" ID="lbler"></asp:Label>
        <div class="col-lg-12  title-content">
            <div class="col-md-2 form-group">
                <div class="col-md-3 pad label">
                    <asp:Label ID="lbl" runat="server" Text="Agency"></asp:Label>
                </div>
                <div class="col-md-9 pad">
                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlAgency"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2 form-group">
                <div class="col-md-3 pad label">
                    <asp:Label ID="Label1" runat="server" Text="Branch:"></asp:Label>
                </div>
                <div class="col-md-9 pad">
                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlBranch"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvddlBranch" CssClass="err" ErrorMessage="Please select Branch" SetFocusOnError="true" ControlToValidate="ddlBranch" runat="server" ValidationGroup="HomeBranchValidation" />
                </div>
            </div>
            <div class="col-md-3 form-group">
                <div class="col-md-3 pad label">
                    <asp:Label ID="lblDateFrom" runat="server" Text="Date From:"></asp:Label>
                </div>
                <div class="col-md-9 pad">
                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control fromdate" />
                    <asp:RequiredFieldValidator ID="rfvtxtDateFrom" CssClass="err" ErrorMessage="Please enter date from" SetFocusOnError="true" ControlToValidate="txtDateFrom" runat="server" ValidationGroup="HomeBranchValidation" />
                </div>
            </div>
            <div class="col-md-3 form-group">
                <div class="col-md-3 pad label">
                    <asp:Label ID="lblDateTo" runat="server" Text="Date To:"></asp:Label>
                </div>
                <div class="col-md-9 pad">
                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control todate" />
                    <asp:RequiredFieldValidator ID="rfvtxtDateTo" CssClass="err" ErrorMessage="Please enter date to" SetFocusOnError="true" ControlToValidate="txtDateTo" runat="server" ValidationGroup="HomeBranchValidation" />
                </div>
            </div>
            <div class="col-md-2 form-group pull-right" style="text-align: left;">
                <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" OnClientClick="showPageLoader('HomeBranchValidation');" />

                <asp:Button runat="server" ID="btnExport" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click" />
            </div>
        </div>
    </div>
    <div class="panel-body">
        <div class="adv-table editable-table">
            <asp:GridView ID="gvHomeBranchReport" runat="server" CssClass="table table-bordered"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" AllowSorting="True" Width="100%"
                OnSorting="gv_Sorting" OnPageIndexChanging="gv_PageIndexChanging">
                <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BranchCode" HeaderText="Branch Code" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />                   
                    <asp:BoundField DataField="PolicyNo" HeaderText="Policy No" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="EndorsementNo" HeaderText="EndorsementNo" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="Subclass" HeaderText="Subclass" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />                   
                    <asp:BoundField DataField="SumInsured" HeaderText="SumInsured" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="NewPremium" HeaderText="NewPremium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="RenewalPremium" HeaderText="RenewalPremium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="AdditionalPremium" HeaderText="AdditionalPremium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="RefundPremium" HeaderText="RefundPremium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="VAT" HeaderText="VAT" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>