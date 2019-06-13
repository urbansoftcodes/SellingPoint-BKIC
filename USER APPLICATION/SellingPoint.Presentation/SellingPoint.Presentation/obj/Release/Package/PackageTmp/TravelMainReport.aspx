<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.master" CodeBehind="TravelMainReport.aspx.cs"
    Inherits="SellingPoint.Presentation.TravelMainReport" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="panel">
        <header class="panel-heading tab-bg-dark-navy-blue fcolorwhite ">
            Travel Main Report
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
            <div class="col-md-3">
                <div class="col-md-3 pad label">
                    <asp:Label ID="lblDateFrom" runat="server" Text="Date From:"></asp:Label>
                </div>
                <div class="col-md-9 pad">
                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control fromdate" />
                    <asp:RequiredFieldValidator ID="rfvtxtDateFrom" CssClass="err" ErrorMessage="Please enter date from" SetFocusOnError="true" ControlToValidate="txtDateFrom" runat="server" ValidationGroup="MotorBranchValidation" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="col-md-3 pad label">
                    <asp:Label ID="lblDateTo" runat="server" Text="Date To:"></asp:Label>
                </div>
                <div class="col-md-9 pad">
                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control todate" />
                    <asp:RequiredFieldValidator ID="rfvtxtDateTo" CssClass="err" ErrorMessage="Please enter date to" SetFocusOnError="true" ControlToValidate="txtDateTo" runat="server" ValidationGroup="MotorBranchValidation" />
                </div>
            </div>
            <div class="col-md-2 form-group pull-right" style="text-align: left;">
                <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary" OnClientClick="showPageLoader('MotorBranchValidation');" />
                <asp:Button runat="server" ID="btnExport" Text="Export" OnClick="btnExport_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="panel-body">
        <div class="adv-table editable-table">
            <asp:GridView ID="gvTravelMainReport" runat="server" CssClass="table table-bordered"
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
                    <asp:BoundField DataField="BranchName" HeaderText="Branch Name" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="PolicyNo" HeaderText="Policy No" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="EndorsementNo" HeaderText="EndorsementNo" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="Subclass" HeaderText="Subclass" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="InsuredName" HeaderText="Insured Name" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="AuthorizedUser" HeaderText="Authorized User" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="AuthorizedDate" HeaderText="Authorized Date" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" dataformatstring="{0:M-dd-yyyy}" />                   
                    <asp:BoundField DataField="HandledBy" HeaderText="Handled By" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                     <asp:BoundField DataField="CommenceDate" HeaderText="Commence Date" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="PaymentMethod" HeaderText="Payment Method" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                     <asp:BoundField DataField="Commission" HeaderText="Commission" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                     <asp:BoundField DataField="RefundCommision" HeaderText="Refund Commission" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                     <asp:BoundField DataField="Discount" HeaderText="Discount" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="NewPremium" HeaderText="NewPremium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="RenewalPremium" HeaderText="RenewalPremium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="AdditionalPremium" HeaderText="AdditionalPremium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                    <asp:BoundField DataField="RefundPremium" HeaderText="RefundPremium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                     <%--<asp:BoundField DataField="PremiumLessCredit" HeaderText="PremiumLessCredit" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                     <asp:BoundField DataField="PremiumReference" HeaderText="PremiumReference" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                     <asp:BoundField DataField="CommisionReference" HeaderText="CommisionReference" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                     <asp:BoundField DataField="BatchDate" HeaderText="BatchDate" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />--%>
                     <asp:BoundField DataField="CPR" HeaderText="CPR" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
