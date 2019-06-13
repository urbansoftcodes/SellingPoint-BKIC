<%@ Page Title="" Language="C#" MasterPageFile="~/General.Master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="BKIC.SellingPoint.Presentation.UserMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateKeyHide {
            display: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container container_top_margin">
        <asp:UpdatePanel runat="server" ID="upnlUserMaster">
            <ContentTemplate>               
                <div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>User Search :</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>
                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="form-horizontal form-label-left col-md-6">
                                <asp:TextBox ID="txtSearchUser" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-horizontal form-label-left col-md-6">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary onlynumber" OnClientClick="showPageLoader();" OnClick="txtSearch_User" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                    <div class="x_panel">
                        <div class="x_title">
                            <h2>User Master :</h2>
                            <ul class="nav navbar-right panel_toolbox">
                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                </li>

                            </ul>
                            <div class="clearfix"></div>
                        </div>
                        <div class="x_content">
                            <div class="form-horizontal form-label-left col-md-12 user-master-form">
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Role:</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <div class="control-label ">
                                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control col-md-10" AutoPostBack="True" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlRole" ErrorMessage="Please select user role" ControlToValidate="ddlRole" runat="server" ValidationGroup="UserMasterValidation" />
                                        </div>
                                    </div>
                                </div>
                                <div id="userdetails" class="col-md-12 padding_zero" runat="server">
                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Agency:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlAgency" runat="server" CssClass="form-control col-md-10" AutoPostBack="true" OnSelectedIndexChanged="agency_changed"></asp:DropDownList>
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvddlAgency" ErrorMessage="Please select agency" ControlToValidate="ddlAgency" runat="server" ValidationGroup="UserMasterValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Agent Code:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlAgentCode" runat="server" CssClass="form-control col-md-10"></asp:DropDownList>
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvAgentCode" runat="server" ControlToValidate="ddlAgentCode" ErrorMessage="Please select agency code" ValidationGroup="UserMasterValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Agent Branch:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlAgentBranch" runat="server" CssClass="form-control col-md-10"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvAgentBranch" CssClass="err" runat="server" ControlToValidate="ddlAgentBranch" ErrorMessage="Please select agent branch" ValidationGroup="UserMasterValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">UserId:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control col-md-10" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvUserid" runat="server" ControlToValidate="txtUserId" ErrorMessage="Please enter userId" ValidationGroup="UserMasterValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">UserName:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control col-md-10" />
                                                <asp:RequiredFieldValidator ID="rfvUserName" CssClass="err" runat="server" ControlToValidate="txtUserName" ErrorMessage="Please enter username" ValidationGroup="UserMasterValidation" />
                                            </div>
                                        </div>
                                    </div>


                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Password:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control col-md-10" />
                                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Please enter password" ValidationGroup="UserMasterValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Confirm Password:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtConfirmPwd" TextMode="Password" runat="server" CssClass="form-control col-md-10" />
                                                <asp:RequiredFieldValidator ID="rfvConfirmPwd" runat="server" ControlToValidate="txtConfirmPwd" ErrorMessage="Please enter confirm password" ValidationGroup="UserMasterValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Mobile No:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control col-md-10" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Email:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control col-md-10" />
                                            </div>
                                        </div>
                                    </div>


                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Staff No:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtStaffNo" runat="server" CssClass="form-control col-md-10 onlynumber" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvStaffNo" runat="server" ControlToValidate="txtStaffNo" ErrorMessage="Please enter staffNo" ValidationGroup="UserMasterValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>
                                    <div class="ln_solid"></div>
                                    <div class="form-group">
                                        <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                            <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSubmit_Click" OnClientClick="showPageLoader('UserMasterValidation');" ValidationGroup="UserMasterValidation" />
                                            <asp:Button ID="btnCancel" CssClass="btn btn-warning" runat="server" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                                        </div>
                                    </div>
                                </div>

                                <div id="admindetails" class="col-md-12 padding_zero" runat="server">

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">User Name:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtAdUserName" runat="server" CssClass="form-control col-md-10"></asp:TextBox>
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtAdUserName" runat="server" ControlToValidate="txtAdUserName" ErrorMessage="Please enter UserName" ValidationGroup="AdminValidation" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label col-md-3 col-sm-3 col-xs-12">Password:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtAdPassword" runat="server" TextMode="Password" CssClass="form-control col-md-10" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvAdPassword" runat="server" ControlToValidate="txtAdPassword" ErrorMessage="Please enter Password" ValidationGroup="AdminValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Confirm Password:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtAdConfirmPwd" runat="server" TextMode="Password" CssClass="form-control col-md-10" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvAdConfirmPwd" runat="server" ControlToValidate="txtAdConfirmPwd" ErrorMessage="Please enter Confirm Password" ValidationGroup="AdminValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group col-md-6">
                                        <div class="col-md-4 page-label">
                                            <label class="control-label">Email:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label">
                                                <asp:TextBox ID="txtAdEmail" runat="server" CssClass="form-control col-md-10" />
                                                <asp:RequiredFieldValidator CssClass="err" ID="rfvAdEmail" runat="server" ControlToValidate="txtAdEmail" ErrorMessage="Please enter Email" ValidationGroup="AdminValidation" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="clearfix"></div>
                                    <div class="ln_solid"></div>
                                    <div class="form-group">
                                        <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                            <asp:Button ID="btnAdminSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnAdminSave_Click" OnClientClick="showPageLoader('AdminValidation');" ValidationGroup="AdminValidation" />
                                            <asp:Button ID="btnAdCancel" CssClass="btn btn-warning" runat="server" Text="Cancel" OnClick="btnAdCancel_Click" CausesValidation="false" />


                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div>
                        <asp:GridView ID="gvUserMaster" runat="server" DataKeyNames="Id" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10"
                            OnPageIndexChanging="gv_PageIndexChanging"
                            OnDataBound="gvUserMaster_RowDataBound">
                            <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide" />
                                <asp:BoundField DataField="Agency" HeaderText="Agency" SortExpression="Name" />
                                <asp:BoundField DataField="AgentCode" HeaderText="Agent Code" SortExpression="Name" />
                                <asp:BoundField DataField="AgentBranch" HeaderText="Agent Branch" SortExpression="Name" />
                                <asp:BoundField DataField="Role" HeaderText="Agent Role" SortExpression="Name" />
                                <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="Name" />
                                <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="Name" />
                                <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Name" />
                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Name" />
                                <asp:BoundField DataField="StaffNo" HeaderText="StaffNo" SortExpression="Name" />

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnEdit" runat="server" ToolTip="Edit" CssClass="fsize fcolorred" OnClick="lnkbtnEdit_Click" OnClientClick="showPageLoader()"><i class="fa  fa-edit"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" CssClass="fsize fcolorred" OnClick="lnkbtnDelete_Click" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?');"><i class="fa  fa-trash-o"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>                  
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
