<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="CategoryMaster.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.CategoryMaster" %>
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
        <asp:UpdatePanel runat="server" ID="upnlInsuranceMaster">
            <ContentTemplate>
               <%-- <div class="page-header">
							<h1>Category Master </h1>
			    </div>--%>
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Category Master :</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="form-horizontal form-label-left col-md-12">

                             <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Agency:</label>
                                        </div>
                                      <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                       <asp:TextBox ID="txtAgency" runat="server" CssClass="form-control col-md-10"/>
                                       <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtAgency" runat="server" ControlToValidate="txtAgency" ErrorMessage="Please enter Agency" ValidationGroup ="CategoryValidation"/>
                                    </div>
                                </div>
                                 </div>
                                    <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Agent Code:</label>
                                        </div>
                                      <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                       <asp:TextBox ID="txtAgentCode" runat="server" CssClass="form-control col-md-10"/>
                                       <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtAgentCode" runat="server" ControlToValidate="txtAgentCode" ErrorMessage="Please enter Agenct Code" ValidationGroup ="CategoryValidation"/>
                                    </div>
                                </div>
                                 </div>
                                 

                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Main Class:</label>
                                        </div>

                                      <div class="col-md-8 page-control">
                                        <div class="control-label">
                                        <asp:TextBox ID="txtMainclass" runat="server" CssClass="form-control col-md-10"/>
                    <asp:RequiredFieldValidator CssClass="err" ID="rfvMainclass" runat="server" ControlToValidate="txtMainclass" ErrorMessage="Please enter main class" ValidationGroup ="CategoryValidation"/>
                                    </div>
                                </div>
                                     </div>


                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label" >Sub Class:</label>
                                        </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                         <asp:TextBox ID="txtSubClass" runat="server" CssClass="form-control col-md-10" />
                    <asp:RequiredFieldValidator CssClass="err" ID="rfvSubClass" runat="server" ControlToValidate="txtSubClass" ErrorMessage="Please enter sub class" ValidationGroup ="CategoryValidation"/>
                                    </div>
                                </div>
                                     </div>


                                 <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Category:</label>
                                        </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label">
                                         <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control col-md-10" />
                    <asp:RequiredFieldValidator CssClass="err" ID="rfvCategory" runat="server" ControlToValidate="txtCategory" ErrorMessage="Please enter category" ValidationGroup ="CategoryValidation"/>
                                    </div>
                                </div>
                                </div>

                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Code:</label>
                                        </div>

                                     <div class="col-md-8 page-control">
                                        <div class="control-label">
                                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control col-md-10" />
                    <asp:RequiredFieldValidator CssClass="err" ID="rfvCode" runat="server" ControlToValidate="txtCode" ErrorMessage="Please enter code" ValidationGroup ="CategoryValidation"/>
                                    </div>
                                </div>
                                      </div>

                                  <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Value Type:</label>
                                        </div>
                                       <div class="col-md-8 page-control">
                                        <div class="control-label">
                                         <asp:TextBox ID="txtValueType" runat="server" CssClass="form-control col-md-10" />
                                         <asp:RequiredFieldValidator CssClass="err" ID="rfvValueType" runat="server" ControlToValidate="txtValueType" ErrorMessage="Please enter value type" ValidationGroup ="CategoryValidation"/>
                                    </div>
                                </div>
</div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                    <label class="control-label">Value:</label>
                                     </div>
                                       <div class="col-md-8 page-control">
                                        <div class="control-label">
                                        <asp:TextBox ID="txtValue" runat="server" CssClass="form-control col-md-10 onlynumber" />
                                        <asp:RequiredFieldValidator CssClass="err" ID="rfvValue" runat="server" ControlToValidate="txtValue" ErrorMessage="Please enter value" ValidationGroup ="CategoryValidation"/>
                                    </div>
                                </div>
                                    </div>                          

                                <div class="clearfix"></div>
                                <div class="ln_solid"></div>
                                <div class="form-group">
                                    <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Save" Class="btn btn-primary" OnClick="btnSubmit_Click" OnClientClick="showPageLoader('CategoryValidation');"/>
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Class="btn btn-warning" OnClick="btnCancel_Click"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                  
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>Manage Category:</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">

         <asp:GridView ID="gvCategoryMaster" runat="server" DataKeyNames="Id" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10" 
             OnPageIndexChanging="gv_PageIndexChanging"
             OnDataBound="gvMotorInsurance_DataBound">
                            <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                    <asp:BoundField DataField="id" HeaderText="Id" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide"/>
                    <asp:BoundField DataField="Agency" HeaderText="Agency" SortExpression="Name" />
                    <asp:BoundField DataField="AgentCode" HeaderText="Agent Code" SortExpression="Name" />
                    <asp:BoundField DataField="MainClass" HeaderText="Main Class" SortExpression="Name" />
                    <asp:BoundField DataField="SubClass" HeaderText="Sub Class" SortExpression="Name" />
                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Name" />
                    <asp:BoundField DataField="Code" HeaderText="Code" SortExpression="Name" />
                    <asp:BoundField DataField="ValueType" HeaderText="Value Type" SortExpression="Name" />
                    <asp:BoundField DataField="Value" HeaderText="Value" SortExpression="Name" />                   

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnEdit" runat="server" ToolTip="Edit" CssClass="fsize fcolorred" OnClick="lnkbtnEdit_Click" OnClientClick="showPageLoader();"><i class="fa  fa-edit"></i></asp:LinkButton>
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
