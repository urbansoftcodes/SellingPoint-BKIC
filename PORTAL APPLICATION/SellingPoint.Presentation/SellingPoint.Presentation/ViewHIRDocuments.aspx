<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="ViewHIRDocuments.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.ViewHIRDocuments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="field-wrap package col-sm-12">
        <div class="col-md-12 pull-left forms_colon">
            <div class="col-md-10">
                <asp:Repeater runat="server" ID="rptHIRDocuments">
                    <ItemTemplate>
                        <div class="col-md-12 inbox">
                            <div class="testclass">
                                <div class="col-md-12 border-style inbox-content">
                                    <div class="col-md-2">
                                        <img src="aimages/FileImage.png" style="width:50px;height:50px" />
                                    </div>
                                    <div class="col-md-4 documentname">
                                        <lable runat="server" id="FileName"><%# Eval("FileName") %></lable>
                                    </div>
                                    <div class="col-md-3 documentlink">
                                        <lable runat="server" id="CreateDate"><%# Eval("CreatedDate") %></lable>
                                    </div>
                                    <div class="col-md-3 documentlink">
                                       <lable><i class="fa fa-download"></i><a href='<%# Eval("FileURL") %>' target="_blank">Download</a></lable>  
                                    </div>  
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>
