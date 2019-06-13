<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="DomesticHelpActivePolicy.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.DomesticHelpActivePolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="panel">
        <header class="panel-heading tab-bg-dark-navy-blue fcolorwhite ">
            DomesticHelp Insurance
                               <%--<span style="float: right;">Motor Insurance
                               </span>--%>
        </header>
        <div class="panel-body">
            <div class="row">
                <%--   <div class="col-lg-3">
                    <asp:LinkButton ID="lbtnnew" runat="server" OnClick="lbtnnew_Click" CssClass="btn btn-round btn-info"><i class="fa  fa-plus-square"></i>  Add New</asp:LinkButton>
                    <asp:Label ID="lblcid" runat="server" Text="" Visible="false"></asp:Label>
                </div>
                <div class="col-lg-9">
                </div>--%>
            </div>

        </div>
        <div class="form-group has-success search-header">          
            <asp:Label runat="server" ID="lbler"></asp:Label>
            <div class="col-lg-12  title-content">


                <div class="col-md-3  pull-left search_filed">
                    <div class="col-md-3 pad label">
                      <asp:Label ID="lblDocNumber" runat="server" Text="Key"></asp:Label>                       

                    </div>
                    <div class="col-md-9 pad">
                        <asp:TextBox runat="server" ID="txtSearchKey"></asp:TextBox>
                    </div>
                </div>


                <div class="col-md-3 search_filed">
                    <div class="col-md-3 pad label">
                        <asp:Label ID="lbl" runat="server" Text="Agency"></asp:Label>
                    </div>
                    <div class="col-md-9 pad">
                         <asp:DropDownList runat="server" ID="ddlAgency" CssClass="col-md-12"></asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-3  submit-search">
                   <asp:Button runat="server" ID="btnSearch" Text="Search"  OnClick="btnSearch_Click" OnClientClick="showPageLoader();" />                  
                </div>
            </div>
            </div>
            <div class="panel-body">
                
                    <div class="adv-table editable-table ">
                        <asp:GridView ID="gvDomesticInsurance" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10" OnPageIndexChanging="gv_PageIndexChanging" OnDataBound="gvMotorInsurance_DataBound">
                            <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                        <asp:Label ID="lblDomesticID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                        <%--<asp:Label ID="lblHIRStatusCode" runat="server" Text='<%# Eval("HIRStatusCode") %>' Visible="false"></asp:Label>--%>
                                        <asp:Label ID="lblIsMessage" runat="server" Text='<%# Eval("IsMessageAvailable") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="IsDocument" runat="server" Text='<%# Eval("IsDocumentsAvailable") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblLinkID" runat="server" Text='<%# Eval("LinkID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AgentCode" HeaderText="AGENTCODE" SortExpression="CreatedDate" />
                                <asp:BoundField DataField="InsuredCode" HeaderText="InsuredCode"/>
                                <asp:BoundField DataField="LinkID" HeaderText="LINKID" SortExpression="UpdatedDate" />
                                <asp:BoundField DataField="DocumentNo" HeaderText="DOCUMENTNO" SortExpression="CreatedDate" />
                                <asp:BoundField DataField="CPR" HeaderText="CPR" SortExpression="UpdatedDate" />
                                <asp:BoundField DataField="InsuredName" HeaderText="INSURED NAME" SortExpression="CreatedDate" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDocument" runat="server" CausesValidation="false" CommandName="Documents" CssClass="fsize fcolorred" ToolTip="Documents"
                                            Text="Documents" CommandArgument='<%# Eval("ID") %>' OnClick="btnDocument_Click" Visible="true"><i class="fa fa-paperclip" aria-hidden="true"></i>  </asp:LinkButton>
                                        <asp:LinkButton ID="btnViewMail" runat="server" CausesValidation="false" CommandName="Documents" CssClass="fsize fcolorred" ToolTip="ViewMessage"
                                            Text="Documents" CommandArgument='<%# Eval("ID") %>' OnClick="btnViewMail_Click" Visible="true"><i class="fa fa-envelope" aria-hidden="true"></i>  </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>                               
                                <asp:BoundField DataField="TRANSACTIONDATE" HeaderText="Commence Date" SortExpression="CreatedDate"  DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="NETPREMIUM" HeaderText="Premium" SortExpression="UpdatedDate"  />  
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnViewDetails" runat="server" OnClientClick="showPageLoader();" ToolTip="View Details" CssClass="fsize fcolorgreen" OnClick="lnkbtnViewDetails_Click"><i class="fa  fa-search"></i></asp:LinkButton>
                                        <%--<asp:LinkButton ID="lnkbtnEdit" runat="server" ToolTip="Edit" CssClass="fsize fcolorred" OnClick="lnkbtnEdit_Click"><i class="fa  fa-edit"></i></asp:LinkButton>
                                    <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" CssClass="fsize fcolorred" OnClick="lnkbtnDelete_Click"><i class="fa  fa-trash-o"></i></asp:LinkButton>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
            </div>
    </section>
</asp:Content>
