<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="HomePage.aspx.cs" Inherits="SellingPoint.Presentation.HomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false);
				function hideURLbar(){ window.scrollTo(0,1); } 
        $(function () {
               autocompleteCPR();
        }); 
    </script>
    <!-- //custom-theme -->
    <link href="assets/css/component.css" rel="stylesheet" type="text/css" media="all" />
    <link href="assets/css/export.css" rel="stylesheet" type="text/css" media="all" />
    <link href="assets/css/flipclock.css" rel="stylesheet" type="text/css" media="all" />
    <link href="assets/css/circles.css" rel="stylesheet" type="text/css" media="all" />
    <link href="assets/css/style_grid.css" rel="stylesheet" type="text/css" media="all" />
    <link href="assets/css/style.css" rel="stylesheet" type="text/css" media="all" />

    <!-- page custom main styles -->
    <link rel="stylesheet" href="assets/css/main-style.css" />
    <link rel="stylesheet" href="assets/css/chart.css" />

    <!-- font-awesome-icons -->
    <link href="assets/css/font-awesome.css" rel="stylesheet" />
    <!-- //font-awesome-icons -->    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="homePagePanel">
        <ContentTemplate>

            <!-- banner -->
            <div class="wthree_agile_admin_info">

                <div class="clearfix"></div>
                <!-- //w3_agileits_top_nav-->
                <!-- /inner_content-->
                <div class="dashboard_content">
                    <!-- /inner_content_w3_agile_info-->
                    <div class="dashboard_content_w3_agile_info">
                        <!-- /agile_top_w3_grids-->
                        <div class="agile_top_w3_grids">
                            <!------ breadcrumb ----->
                            <div class="row page-titles">
                                <div class="col-md-5 align-left">
                                    <h3>Dashboard</h3>
                                </div>
                                <div class="col-md-7 align-right">
                                    <ol class="breadcrumb">
                                        <li class="breadcrumb-item"><a href="javascript:void(0)">Home</a></li>
                                        <li class="breadcrumb-item active">Dashboard</li>
                                    </ol>
                                </div>
                            </div>
                            <!------ End breadcrumb ----->
                            <!------ insurance grid ----->
                            <div class="grid-wrapper">
                                <div class="grid-item grid-1">
                                    <div class="grid-img">
                                        <img src="assets/images/travel-icon.png" alt="travel insurance">
                                    </div>
                                    <div class="count-container">
                                        <h2 runat="server" id="travelcount">0</h2>
                                        <a href="#" class="insurance-link">TRAVEL INSURANCE</a>
                                    </div>
                                    <ul>
                                        <li><a href="Travelnsurance.aspx">New</a></li>
                                        <li><a href="Travelnsurance.aspx">Renewal</a></li>
                                        <li><a href="Travelnsurance.aspx">View</a></li>
                                          <li><a href="#"></a></li>
                                    </ul>
                                </div>
                                <div class="grid-item grid-2">
                                    <div class="grid-img">
                                        <img src="assets/images/motor-icon.png" alt="MOTOR INSURANCE">
                                    </div>
                                    <div class="count-container">
                                        <h2 runat="server" id="motorcount">0</h2>
                                        <a href="./motor-insurance.html" class="insurance-link">MOTOR INSURANCE</a>
                                    </div>
                                    <ul>
                                        <li><a href="MotorInsurance.aspx">New</a></li>
                                        <li><a href="MotorSystemRenewal.aspx">Renewal</a></li>
                                        <li><a href="MotorInsurance.aspx">View</a></li>
                                          <li><a href="#"></a></li>
                                    </ul>
                                </div>
                                <div class="grid-item grid-3">
                                    <div class="grid-img">
                                        <img src="assets/images/domestic-icon.png" alt="DOMESTIC HELP">
                                    </div>
                                    <div class="count-container">
                                        <h2 runat="server" id="domesticcount">0</h2>
                                        <a href="#" class="insurance-link">DOMESTIC HELP</a>
                                    </div>
                                    <ul>
                                        <li><a href="DomesticHelp.aspx">New</a></li>
                                        <li><a href="DomesticHelp.aspx">Renewal</a></li>
                                        <li><a href="DomesticHelp.aspx">View</a></li>
                                          <li><a href="#"></a></li>
                                    </ul>
                                </div>
                                <div class="grid-item grid-4">
                                    <div class="grid-img">
                                        <img src="assets/images/home-icon.png" alt="HOME INSURANCE">
                                    </div>
                                    <div class="count-container">
                                        <h2 runat="server" id="homecount">0</h2>
                                        <a href="#" class="insurance-link">HOME INSURANCE</a>
                                    </div>
                                    <ul>
                                        <li><a href="HomeInsurancePage.aspx">New</a></li>
                                        <li><a href="HomeSystemRenewal.aspx">Renewal</a></li>
                                        <li><a href="HomeInsurancePage.aspx">View</a></li>
                                          <li><a href="#"></a></li>
                                    </ul>
                                </div>
                            </div>
                            <!------ End insurance grid ----->
                        </div>
                        <!-- //agile_top_w3_grids-->
                        <div class="col-md-12 graph-container padding_zero">
                            <div class="col-md-6 graph-row ">
                                <h3 class=" w3_inner_tittle two">SELLING POINT</h3>                           
                                  <div class="col-md-12 selling_logo" runat="server" id="securaLogo" visible="false">
                                    <img src="assets/images/SecuraLogo.jpg" class="img-responsive" alt="selling_logo" />
                                </div>
                                 <div class="col-md-12 selling_logo" runat="server" id ="tiscoLogo" visible="false">
                                    <img src="assets/images/TiscoLogo.jpg" class="img-responsive" alt="selling_logo" style="width: 65%;margin: 0 auto;" />
                                </div>
                            </div>                           
                            <div class="col-md-6  prograc-blocks_agileits">
                                <h3 class="w3_inner_tittle two">Policy Search</h3>
                                <div class="col-md-6 bars_agileits agile_info_shadow">
                                    <div class='bar_group'>
                                        <div class="col-md-2 page-label">
                                            <label class="control-label">CPR: *</label>
                                        </div>
                                        <div class="col-md-5  page-control">
                                        <asp:TextBox ID="txtCPRSearch" runat="server" CssClass="form-control onlynumber" autocomplete="off" Width="300" AutoPostBack="true" onChange="showPageLoader();" OnTextChanged="txtCPR_Changed"/>                                                                             
                                         <asp:Button ID="btnCPRSearch" Text="Find" runat="server" OnClientClick="showPageLoader();" OnClick="Search_Policy" CssClass="btn btn-primary search-btn"></asp:Button>
                                        <asp:HiddenField runat="server" ID="InsuredCode" />
                                        <asp:HiddenField runat="server" ID="InsuredName" />
                                    </div>
                                    <asp:GridView ID="gvDocuments" OnRowDataBound="Gridview1_RowDataBound" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False"
                                        AllowPaging="True" AllowSorting="True" Width="100%" PageSize="10">                                      
                                        <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                                    <asp:Label ID="lblPolicyType" runat="server" Text='<%# Eval("PolicyType") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblPolicyNo" runat="server" Text='<%# Eval("DocumentNo") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblRenewalCount" runat="server" Text='<%# Eval("RenewalCount") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DocumentNo" HeaderText="Policy No" SortExpression="Name" />
                                            <asp:BoundField DataField="ExpireDate" HeaderText="Expire Date" SortExpression="Name" />
                                            <asp:BoundField DataField="PolicyType" HeaderText="Insurance" SortExpression="Name" />
                                            <asp:BoundField DataField="RenewalCount" HeaderText="Renewal Count" SortExpression="Name" />
                                            <asp:TemplateField HeaderText="Review">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnView" runat="server" ToolTip="Edit Policy Details" CssClass="fsize fcolorgreen" OnClick="lnkbtnEdit_Click" OnClientClick="document.forms[0].target ='_blank';"><i class="fa  fa-edit"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Renew" Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkbtnRenew" runat="server" ToolTip="Renew Policy" CssClass="fsize fcolorgreen" OnClick="lnkbtnRenew_Click" OnClientClick="document.forms[0].target ='_blank';"><i class="fa fa-refresh"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <!--//prograc-blocks_agileits-->
                        </div>
                        <!-- /w3ls_agile_circle_progress-->
                    </div>
                    <!-- //inner_content_w3_agile_info-->
                </div>
                <!-- //inner_content-->
            </div>
            <!-- banner -->
            <!--copy rights start here-->

            <!--copy rights end here-->                
                      
            <script src="assets/customjs/gnmenu.js"></script>
            <script>
                new gnMenu(document.getElementById('gn-menu'));
            </script>           
            <script src="assets/customjs/jquery.nicescroll.js"></script>
            <script src="assets/customjs/scripts.js"></script>  
            <%--</body>
           </html>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>