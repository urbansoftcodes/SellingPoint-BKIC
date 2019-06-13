<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="ViewInsuranceMessage.aspx.cs" Inherits="BKIC.SellingPoint.Portal.ViewInsuranceMessage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="panel">
        <header class="panel-heading tab-bg-dark-navy-blue fcolorwhite ">
            Insurance Messages
        </header>
        <div class="panel-body">
            <div id="insuranceMessages">
                <asp:Repeater ID="rtInsuranceMessages" runat="server">
                    <ItemTemplate>
                        <div class="col-lg-12 border-style">
                            <div class="col-lg-3">
                                <%# Eval("Message Key") %>
                            </div>
                            <div class="col-lg-6">
                                <%--<customHtmlEditor:HTMLEditor ID="messageEditor" Width="450px"
                                    Height="200px"
                                    runat="server"
                                    Value='<%# Eval("Message") %>'></customHtmlEditor:HTMLEditor>--%>
                                <%# Eval("Message") %>
                            </div>
                            <div class="col-lg-3">
                                <%# Eval("Send Date") %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </section>
    <style>
        .col-lg-12.border-style {
            border-bottom: 1px solid;
            padding-bottom: 30px;
            margin-bottom: 20px;
        }
    </style>
</asp:Content>