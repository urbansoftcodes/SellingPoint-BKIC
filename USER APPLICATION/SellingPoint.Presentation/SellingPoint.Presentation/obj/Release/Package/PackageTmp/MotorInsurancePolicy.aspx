<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.master" CodeBehind="MotorInsurancePolicy.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.MotorInsurancePolicy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="panel">
        <header class="panel-heading tab-bg-dark-navy-blue fcolorwhite ">
            Motor Insurance- Active Policy                              
        </header>
        <div class="panel-body">
            <div class="row">               
            </div>
        </div>

        <div class="form-group Ahas-success">   
        <label class="col-sm-2 control-label col-lg-2"></label>
        <asp:Label runat="server" ID="lbler"></asp:Label>
            <div class="form-group has-success search-header">
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
                        <asp:DropDownList runat="server" ID="ddlAgency"></asp:DropDownList>
                    </div>
                </div>

                <div class="col-md-3 submit-search">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click"  OnClientClick="showPageLoader();"/>                   
                </div>
            </div>
            </div>



        <div class="panel-body">
            <div class="adv-table editable-table ">
                <asp:GridView ID="gvMotorInsurance" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" OnPageIndexChanging="gv_PageIndexChanging" OnDataBound="gvMotorInsurance_DataBound">
                    <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                <asp:Label ID="lblMotorID" runat="server" Text='<%# Eval("MotorID") %>' Visible="false"></asp:Label>
                              <%--  <asp:Label ID="lblHIRStatusCode" runat="server" Text='<%# Eval("HIRStatusCode") %>' Visible="false"></asp:Label>--%>
                                <asp:Label ID="lblIsMessage" runat="server" Text='<%# Eval("IsMessageAvailable") %>' Visible="false"></asp:Label>
                                <asp:Label ID="IsDocument" runat="server" Text='<%# Eval("IsDocumentsAvailable") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblLinkID" runat="server" Text='<%# Eval("LinkID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="InsuredCode" HeaderText="INSUREDCODE" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />                     
                        <asp:BoundField DataField="DocumentNo" HeaderText="DOCUMENTNO" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="CPR" HeaderText="CPR" SortExpression="UpdatedDate" HeaderStyle-ForeColor="White" />
                        <asp:BoundField DataField="InsuredName" HeaderText="INSURED NAME" SortExpression="CreatedDate" HeaderStyle-ForeColor="White" />                      

                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDocument" runat="server" CausesValidation="false" CommandName="Documents" CssClass="fsize fcolorred" ToolTip="Documents"
                                    Text="Documents" CommandArgument='<%# Eval("MotorID") %>' OnClick="btnDocument_Click" Visible="true"><i class="fa fa-paperclip" aria-hidden="true"></i>  </asp:LinkButton>
                                <asp:LinkButton ID="btnViewMail" runat="server" CausesValidation="false" CommandName="Documents" CssClass="fsize fcolorred" ToolTip="ViewMessage"
                                    Text="Documents" CommandArgument='<%# Eval("MotorID") %>' OnClick="btnViewMail_Click" Visible="true"><i class="fa fa-envelope" aria-hidden="true"></i>  </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="GROSSPREMIUM" HeaderText="Premium" SortExpression="UpdatedDate" HeaderStyle-ForeColor="White" />                      
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnViewDetails" runat="server" ToolTip="View Details" CssClass="fsize fcolorgreen" OnClick="lnkbtnViewDetails_Click"><i class="fa  fa-search"></i></asp:LinkButton>                             
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>         
        </div>
        </div>
    </section>
</asp:Content>

