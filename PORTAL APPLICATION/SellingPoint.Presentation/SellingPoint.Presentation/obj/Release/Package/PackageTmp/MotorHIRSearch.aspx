<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.master" CodeBehind="MotorHIRSearch.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.MotorHIRSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .hiddencol { display: none; }
    </style>
<script type='text/javascript'>
                function openModal() {
                    $('[id*=myModal]').modal('show');
                } 
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="panel">
        <header class="panel-heading tab-bg-dark-navy-blue fcolorwhite ">
            Motor Insurance- HIR Policy                              
        </header>
        <div class="form-group has-success search-header">
            <asp:Label runat="server" ID="lbler"></asp:Label>
            <div class="col-lg-12  title-content">


                <div class="col-md-3  pull-left">
                    <div class="col-md-3 pad label">
                        <asp:Label ID="lblDocNumber" runat="server" Text="Key"></asp:Label>

                    </div>
                    <div class="col-md-9 pad">
                        <asp:TextBox runat="server" ID="txtSearch"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="col-md-3 pad label">
                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                    </div>
                    <div class="col-md-9 pad">
                        <asp:DropDownList runat="server" ID="ddlStatus"></asp:DropDownList>
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

                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="showPageLoader();" />

                </div>
            </div>
        </div>

        <div class="panel-body">
            <div class="adv-table editable-table ">
                <asp:GridView ID="gvMotorInsurance" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" OnPageIndexChanging="gv_PageIndexChanging" OnDataBound="gvMotorInsurance_DataBound">
                    <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                <asp:Label ID="lblMotorID" runat="server" Text='<%# Eval("MOTORID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblHIRStatusCode" runat="server" Text='<%# Eval("HIRStatusDesc") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblIsMessage" runat="server" Text='<%# Eval("IsMessageAvailable") %>' Visible="false"></asp:Label>
                                <asp:Label ID="IsDocument" runat="server" Text='<%# Eval("IsDocumentsAvailable") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblLinkID" runat="server" Text='<%# Eval("LinkID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblInsuredCode" runat="server" Text='<%# Eval("InsuredCode") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblRenewalCount" runat="server" Text='<%# Eval("RenewalCount") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="MOTORID" HeaderText="LINKID" SortExpression="UpdatedDate" HeaderStyle-ForeColor="White" Visible="false" />
                        <asp:BoundField DataField="DocumentNo" HeaderText="DOCUMENTNO" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="CPR" HeaderText="CPR" SortExpression="UpdatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="GrossPremium" HeaderText="GROSSPREMIUM" SortExpression="UpdatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="InsuredName" HeaderText="INSURED NAME" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="HIRReason" HeaderText="HIR Reason" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="HIRStatus" HeaderText="STATUS" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />                       
                        <asp:BoundField DataField="HIRRemarks" SortExpression="CreatedDate"
                            HeaderStyle-ForeColor="White" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnApproved" runat="server" CausesValidation="false" CommandName="Approved"
                                    CssClass="fsize fcolorgreen status" CommandArgument='<%# Eval("MOTORID") %>' OnClientClick="showPageLoader();" OnClick="btnApproved_Click" Visible="false" ToolTip="Approved"><i class="fa fa-check" aria-hidden="true"></i></asp:LinkButton>
                                <asp:LinkButton ID="btnRejected" runat="server" CausesValidation="false" CommandName="ReqDoc" CssClass="fsize fcolorred" ToolTip="Rejected"
                                    Text="Rejected status" CommandArgument='<%# Eval("MOTORID") %>' OnClientClick="showPageLoader();" OnClick="btnRejected_Click" Visible="false"><i class="fa fa-times" aria-hidden="true"></i>  </asp:LinkButton>
                                <asp:LinkButton ID="btnActivate" runat="server" CausesValidation="false" CommandName="Activate" CssClass="fsize fcolorred" ToolTip="Activate"
                                    Text="Rejected status" CommandArgument='<%# Eval("MOTORID") %>' OnClick="btnApproved_Click" OnClientClick="showPageLoader();" Visible="false"><i class="fa fa-archive" aria-hidden="true"></i>  </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnEdit" runat="server" Text="Remarks" CssClass="btn btn-info"
                                    OnClick="Display"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnViewDetails" runat="server" ToolTip="View Details" CssClass="fsize fcolorgreen" OnClick="lnkbtnViewDetails_Click"><i class="fa  fa-search"></i></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
        <div class="modal" id="myModal" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                            &times;</button>
                        <h4 class="modal-title">Policy Details</h4>
                    </div>
                    <div class="modal-body">
                        <div class="col-lg-12 col-sm-12 col-md-12 col-xs-12">
                            <div class="form-group">
                                <asp:Label ID="lblPolicyNo" runat="server" Text="Policy No:"></asp:Label>                               
                            </div>                            
                            <div class="form-group">
                                <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                                <asp:TextBox ID="txtRemarks"  TextMode="MultiLine" runat="server"  CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-info" />
                        <button type="button" class="btn btn-info" data-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
