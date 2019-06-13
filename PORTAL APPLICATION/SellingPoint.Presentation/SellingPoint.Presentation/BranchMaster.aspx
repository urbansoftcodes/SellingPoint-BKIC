<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="BranchMaster.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.BranchMaster" %>
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
        <asp:UpdatePanel runat="server" ID="upnlBranchMaster">
            <ContentTemplate>
               <%-- <div class="page-header">
							<h1>Branch Master </h1>
			    </div>--%>
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Branch Master:</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="form-horizontal form-label-left col-md-12 BranchMaster-fields ">
                                <div class="form-group col-md-6">
                                     <div class="col-md-4 page-label">
                                    <label class="control-label">Agency:</label>
                                     </div>

                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <asp:DropDownList ID="ddlAgency" runat="server" CssClass="form-control col-md-10" onselectedindexchanged ="ddlAgency_Changed" AutoPostBack="true"> </asp:DropDownList>
                                         <%--<asp:TextBox ID="txtAgency" runat="server" CssClass="form-control col-md-10" />--%>
                                         <asp:RequiredFieldValidator Class="err" ID="rfvddlAgency" ErrorMessage="Please select agency" ControlToValidate="ddlAgency"  runat="server" ValidationGroup ="branchMasterValidation" />
                                        </div>
                                            </div>
                                    </div>                  
                                

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Branch Code:</label>
                                        </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                            <%--<asp:DropDownList ID="ddlAgentBranch" runat="server" CssClass="form-control col-md-10"> </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtAgentBranch" runat="server" CssClass="form-control col-md-10"/>
                                        <asp:RequiredFieldValidator Class="err"  ID="rfvddlAgentBranch" runat="server" ControlToValidate="txtAgentBranch" ErrorMessage="Please enter Agency Branch" ValidationGroup ="branchMasterValidation"/>
                                    </div>
                                        </div>
                                </div>

                                <div class="form-group col-md-6">
                                     <div class="col-md-4 page-label">
                                    <label class="control-label ">Branch Name:</label>
                                     </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                         <asp:TextBox ID="txtBranchName" runat="server" CssClass="form-control col-md-10"/>
                    <asp:RequiredFieldValidator ID="rfvBranchName" Class="err" runat="server" ControlToValidate="txtBranchName" ErrorMessage="Please enter Branch Name" ValidationGroup ="branchMasterValidation"/>
                                    </div>
                                </div>
                                    </div>                     

                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Phone:</label>
                                        </div>
                                     <div class="col-md-8">
                                        <div class="control-label page-control">
                                        <asp:TextBox ID="txtPhone" runat="server" NullDisplayText=" " CssClass="form-control col-md-10"/>                                       
                                    </div>
                                </div>
                               </div>
                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Incharge:</label>
                                        </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                        <asp:TextBox  ID="txtIncharge" runat="server" NullDisplayText=" " CssClass="form-control col-md-10"/>                                       
                                    </div>
                                </div>
                                </div>


                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Email:</label>
                                        </div>
                                    <div class="col-md-8">
                                        <div class="control-label page-control">
                                        <asp:TextBox ID="txtEmail" runat="server" NullDisplayText=" " CssClass="form-control col-md-10"/>
                                        <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                     </div>


                                <div class="clearfix"></div>
                                <div class="ln_solid"></div>
                                <div class="form-group">
                                    <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" Class="btn btn-primary" OnClientClick="showPageLoader('branchMasterValidation')" OnClick="btnSubmit_Click" ValidationGroup ="branchMasterValidation"/> 
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Class="btn btn-warning" OnClick="btnCancel_Click"  />
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
                            <h2>Manage Branch:</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

         <asp:GridView ID="gvBranch" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10" 
             OnPageIndexChanging="gv_PageIndexChanging"
             OnDataBound="gvBranch_RowDataBound">
                            <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                    <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide"/>
                    <asp:BoundField DataField="Agency" HeaderText="Agency" SortExpression="Name" />
                    <%--<asp:BoundField DataField="AgentCode" HeaderText="Agent Code" SortExpression="Name" />--%>
                    <asp:BoundField DataField="AgentBranch" HeaderText="Agent Branch" SortExpression="Name" />
                    <asp:BoundField DataField="BranchName" HeaderText="Branch Name" SortExpression="Name" />
                   <%-- <asp:BoundField DataField="BranchAddress" HeaderText="Branch Address" SortExpression="Name" />--%>
                    <asp:BoundField DataField="Telephoneno" HeaderText="Telephone NO" SortExpression="Name" />
                    <asp:BoundField DataField="Incharge" HeaderText="Incharge" SortExpression="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Name" />

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnEdit" runat="server" ToolTip="Edit" CssClass="fsize fcolorred" OnClientClick="showPageLoader()" OnClick="lnkbtnEdit_Click"><i class="fa  fa-edit"></i></asp:LinkButton>
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
