<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/General.master" CodeBehind="HomeHIRSearch.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.HomeHIRSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="panel">
        <header class="panel-heading tab-bg-dark-navy-blue fcolorwhite ">
            Home Insurance- HIR Policy                              
        </header>
        <div class="panel-body">
            <div class="row">               
            </div>

        </div>
        <div class="form-group has-success search-header">           
            <asp:Label runat="server" ID="lbler"></asp:Label>
            <div class="col-lg-12  title-content">


                <div class="col-md-3  pull-left">
                    <div class="col-md-3 pad label">
                        <asp:Label ID="lblDocNumber" runat="server" Text="Key"></asp:Label>
                    </div>                   
                    <div class="col-md-9 pad">
                        <asp:TextBox runat="server" ID="txtSearchKey"></asp:TextBox>
                    </div>
                </div>
                 <div class="col-md-3">
                    <div class="col-md-3 pad label">
                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-9 pad">
                        <asp:DropDownList runat="server" ID="ddlStatus" CssClass="col-md-12"></asp:DropDownList>
                    </div>
                </div>
                  <div class="col-md-3">
                    <div class="col-md-3 pad label">
                        <asp:Label ID="lbl" runat="server" Text="Agency"></asp:Label>
                    </div>
                    <div class="col-md-9 pad">
                        <asp:DropDownList runat="server" ID="ddlAgency"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-1 submit-search">
                    <asp:Button runat="server" ID="btnSearch" OnClientClick="showPageLoader();" Text="Search" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
        <div class="panel-body">
            <div class="adv-table editable-table ">
                <asp:GridView ID="gvHomeInsurance" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" OnPageIndexChanging="gv_PageIndexChanging" OnDataBound="gvHomeInsurance_DataBound">
                    <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                <asp:Label ID="lblHomeID" runat="server" Text='<%# Eval("HomeID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblHIRStatusCode" runat="server" Text='<%# Eval("HIRStatusDesc") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblIsMessage" runat="server" Text='<%# Eval("IsMessageAvailable") %>' Visible="false"></asp:Label>
                                <asp:Label ID="IsDocument" runat="server" Text='<%# Eval("IsDocumentsAvailable") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblLinkID" runat="server" Text='<%# Eval("LinkID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblInsuredCode" runat="server" Text='<%# Eval("InsuredCode") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblRenewalCount" runat="server" Text='<%# Eval("RenewalCount") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="LinkID" HeaderText="LINKID" SortExpression="UpdatedDate" HeaderStyle-ForeColor="White" Visible="false" />
                        <asp:BoundField DataField="DocumentNo" HeaderText="DOCUMENTNO" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="CPR" HeaderText="CPR" SortExpression="UpdatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="InsuredName" HeaderText="INSURED NAME" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="HIRReason" HeaderText="HIR Reason" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="HIRStatus" HeaderText="STATUS" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />

                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>

                                <asp:LinkButton ID="btnApproved" runat="server" CausesValidation="false" CommandName="Approved"
                                    CssClass="fsize fcolorgreen status" CommandArgument='<%# Eval("HomeID") %>' OnClientClick="showPageLoader();" OnClick="btnApproved_Click" Visible="false" ToolTip="Approved"><i class="fa fa-check" aria-hidden="true"></i></asp:LinkButton>                            
                                <asp:LinkButton ID="btnRejected" runat="server" CausesValidation="false" CommandName="ReqDoc" CssClass="fsize fcolorred" ToolTip="Rejected"
                                    Text="Rejected status" CommandArgument='<%# Eval("HomeID") %>' OnClientClick="showPageLoader();" OnClick="btnRejected_Click" Visible="false"><i class="fa fa-times" aria-hidden="true"></i>  </asp:LinkButton>
                                <asp:LinkButton ID="btnActivate" runat="server" CausesValidation="false" CommandName="Activate" CssClass="fsize fcolorred" ToolTip="Activate"
                                    Text="Rejected status" CommandArgument='<%# Eval("HomeID") %>' OnClientClick="showPageLoader();" OnClick="btnApproved_Click" Visible="false"><i class="fa fa-archive" aria-hidden="true"></i>  </asp:LinkButton>                              
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="GrossPremium" HeaderText="Net Premium" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                       <asp:BoundField DataField="CreatedDate" HeaderText="Triggered Date" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" DataFormatString="{0:dd/MM/yyyy}" />                     

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnViewDetails" runat="server" ToolTip="View Details" OnClientClick="showPageLoader();" CssClass="fsize fcolorgreen" OnClick="lnkbtnViewDetails_Click"><i class="fa  fa-search"></i></asp:LinkButton>                             
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </section>
</asp:Content>
