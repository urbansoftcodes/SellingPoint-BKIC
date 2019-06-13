<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/General.Master" CodeBehind="TravelChangeMemberEndorsement.aspx.cs" Inherits="SellingPoint.Presentation.TravelChangeMemberEndorsement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function ShowPopup() {
            if (checkPageIsValid()) {
                $('#myModal').modal('show');
            }
            else {
                showPageLoader('TravelEndorsementValidation');
                $('#myModal').modal('hide');
                $(".modal-backdrop").remove();
            }
        }
        function closePopup() {
            $('#myModal').modal('hide');
            $(".modal-backdrop").remove();
        }
        function checkPageIsValid() {
            return Page_ClientValidate('TravelEndorsementValidation');
        }
        $(function () {
            autocompleteCPR();
            getTravelEndorsementPolicies();
        }); 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="travelEndorsementUpdatePanel">
        <ContentTemplate>
            <div class="adv-table editable-table ">
                <asp:GridView ID="gvTravelEndorsement" runat="server" OnDataBound="gvTravelEndorsement_DataBound" OnRowDataBound="gvTravelEndorsement_RowDataBound" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10">
                    <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                 <asp:Label ID="lblDocumentNo" runat="server" Text='<%# Eval("DocumentNo") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTravelID" runat="server" Text='<%# Eval("TravelID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTravelEndorsementID" runat="server" Text='<%# Eval("TravelEndorsementID") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblIsSaved" runat="server" Text='<%# Eval("IsSaved") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblIsActive" runat="server" Text='<%# Eval("IsActivePolicy") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="MotorEndorsementID" HeaderText="EndorsementId" />
                     <asp:BoundField DataField="MotorID" HeaderText="MotorId" /> --%>
                        <asp:BoundField DataField="EndorsementNo" HeaderText="EndorsementNo" ItemStyle-CssClass="DateKeyHide" HeaderStyle-CssClass="DateKeyHide" />
                         <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkbtnAuthorize" runat="server" ToolTip="Authorize" CssClass="fsize fcolorred" OnClientClick="return ShowAuthorize();" OnClick="lnkbtnAuthorize_Click" CommandName="Authorize"><i class="fa fa-check"></i></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnDelete" runat="server" ToolTip="Delete" OnClientClick="return ShowDelete();" OnClick="lnkbtnDelete_Click" CssClass="fsize fcolorred" CommandName="Delete"><i class="fa  fa-trash-o"></i></asp:LinkButton>
                                 <a runat="server" id="downloadschedule" class="fsize fcolorred" title="Schedule" onclick="btnPrint_Click"><i class="fa fa-download" aria-hidden="true"></i></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Endorsementtype" HeaderText="Type" SortExpression="Name" />
                        <asp:BoundField DataField="PremiumBeforeDiscount" HeaderText="PremiumBeforeDiscount" SortExpression="Name" />
                        <asp:BoundField DataField="PremiumAfterDiscount" HeaderText="Premium" SortExpression="Name" />
                        <asp:BoundField DataField="CommisionBeforeDiscount" HeaderText="CommisionBeforeDiscount" SortExpression="Name" />
                        <asp:BoundField DataField="CommissionAfterDiscount" HeaderText="Commission" SortExpression="Name" />
                        <asp:BoundField DataField="PolicyCommencementDate" HeaderText="COMMENCEDATE" SortExpression="Name" />
                        <asp:BoundField DataField="EXPIRYDATE" HeaderText="EXPIRYDATE" SortExpression="Name" />                       
                    </Columns>
                </asp:GridView>
            </div>
             <div class="x_panel">               
                <div class="x_title">
                    <div class="col-md-2 motor-search-lable">
                        <h2>Search :</h2>
                    </div>
                    <div class="form-group col-md-4 motor-search-control">
                        <div class="col-md-4 page-label">
                            <label class="control-label">Policy Number :</label>
                        </div>
                        <div class="col-md-8 page-control">
                            <asp:TextBox ID="txtTravelEndorsementSearch" runat="server" CssClass="form-control" AutoPostBack="true" autocomplete="off" Width="300" OnTextChanged="PolicySearch_Changed" onChange="showPageLoader();" />
                            <%--<asp:HiddenField ID="renewalCount" runat="server" />--%>
                        </div>
                    </div>
                    <div class="form-group col-md-4 motor-search-control">
                        <div class="col-md-4 page-label">
                            <label class="control-label">CPR: *</label>
                        </div>
                        <asp:TextBox ID="txtCPRSearch" runat="server" CssClass="form-control onlynumber" autocomplete="off" Width="300" AutoPostBack="true" OnTextChanged="txtCPR_Changed" onChange="showPageLoader();" />
                        <asp:RequiredFieldValidator ID="rfvtxtCPRSearch" CssClass="err" ErrorMessage="Please select CPR" SetFocusOnError="true" ControlToValidate="txtCPRSearch" runat="server" ValidationGroup="TravelEndorsementValidation" />
                    </div>
                    <div class="col-md-2 motor-search-arrow">                    
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                </div>
                <div class="clearfix"></div>
                </div>
                <div class="x_content">
                    <div class="form-horizontal form-label-left col-md-12">
                        <div class="form-group col-md-4">
                            <div class="col-md-4 page-label">
                                <label class="control-label">Introduced by :</label>
                            </div>
                            <div class="col-md-8 page-control">
                                <asp:TextBox ID="txtIndroducedBy" runat="server" CssClass="form-control col-md-10 readonly-sty" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="form-group col-md-4">
                            <div class="col-md-4 page-label">
                                <label class="control-label">Branch :</label>
                            </div>
                            <div class="col-md-8 page-control">
                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control col-md-10 chzn-select">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="err" SetFocusOnError="true" ID="rfvddlBranch" ErrorMessage="Please select branch" ControlToValidate="ddlBranch" runat="server" ValidationGroup="MotorEndorsementValidation" />
                            </div>
                        </div>                                             
                        <div class="clearfix"></div>                       
                        <div class="form-group col-md-4">
                            <div class="col-md-4 page-label">
                                <label class="control-label">Policy Number: *</label>
                            </div>
                            <div class="col-md-8 page-control">
                                <asp:DropDownList ID="ddlTravelPolicies" runat="server" CssClass="form-control col-md-10 chzn-select" onChange="showPageLoader();" OnSelectedIndexChanged="Changed_TravelPolicy" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlTravelPolicies" ErrorMessage="Please select policy" ControlToValidate="ddlTravelPolicies" runat="server" ValidationGroup="TravelEndorsementValidation" />
                            </div>
                        </div>                        
                        <div>
                        </div>
                        <div class="clearfix"></div>
                        <div class="ln_solid"></div>
                        <div class="divider-20"></div>
                    </div>
                    <div runat="server" id="mainDiv">
                        <div class="trf_type typ_wrapper" id="commonDiv" runat="server">

                            <div class="form-horizontal form-label-left col-md-12">
                                <div class="clearfix"></div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Effective From :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtEffectiveFromDate" runat="server" CssClass="form-control col-md-10 datepicker" />
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Efecctive To :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtEffectiveToDate" runat="server" CssClass="form-control col-md-10 datepicker" />
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Insured code :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtOldClientCode" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                    </div>
                                </div>

                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Insurance Name :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtOldInsuredName" runat="server" CssClass="form-control col-md-10" Enabled="false" />
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>                           
                    </div>
                </div>
            </div>
                    <!-- Type 1 -->
                    <div class="trf_type typ_wrapper" id="changeAddMemberDeatilsDiv" runat="server">
                        <div class="form-horizontal form-label-left col-md-12">
                            <%--<div class="form-group col-md-6">
                        </div>
                         <div class="form-group col-md-6">
                        </div>--%>
                            <div id="depentdetails" runat="server">
                                <div class="form-group col-md-12 append-rows">
                                    <h2>Dependent details:</h2>
                                    <div class="table-append-rows">
                                        <asp:GridView ID="Gridview1" Width="100%"
                                            OnRowDataBound="Gridview1_RowDataBound" runat="server" OnRowDeleting="Gridview1_RowDeleting" ShowFooter="true" AutoGenerateColumns="false">
                                            <Columns>
                                                <%--<asp:BoundField DataField="RowNumber" HeaderText="Row Number" />--%>
                                                <asp:TemplateField HeaderText="Insured Name">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMemberName" CssClass="uppercase" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter name" ControlToValidate="txtMemberName" runat="server" ValidationGroup="travelValidation" />
                                                    </ItemTemplate>                                                  
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Relationship">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlRelation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRelation_Changed">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please select relationship" ControlToValidate="ddlRelation" runat="server" ValidationGroup="travelValidation" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date Of Birth">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDOB" runat="server" CssClass="dateofbirth"></asp:TextBox>
                                                        <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter date of birth" ControlToValidate="txtDOB" runat="server" ValidationGroup="travelValidation" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Passport No">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPassport" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter CPR no" ControlToValidate="txtPassport" runat="server" ValidationGroup="travelValidation" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nationality">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlNational" runat="server" CssClass="form-control col-md-10 chzn-select" >
                                                            <asp:ListItem Text="--Please Select--" Value="-1" />
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please select national" ControlToValidate="ddlNational" runat="server" ValidationGroup="travelValidation" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Occupation">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOccupation" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator CssClass="err" ErrorMessage="Please enter occupation" ControlToValidate="txtOccupation" runat="server" ValidationGroup="travelValidation" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div> 
                <div class="x_panel">            
                    <div class="ln_solid"></div>
                    <div class="btn-wrap-row text-center">                        
                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="showPageLoader('TravelEndorsementValidation');" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-primary" OnClick="btnBack_Click" ValidationGroup="TravelEndorsementValidation" />                       
                        <asp:HiddenField ID="calculatedPremium" runat="server" />
                        <asp:HiddenField ID="calculatedCommision" runat="server" />
                        <asp:HiddenField ID="insuredDOB" runat="server" />
                        <asp:HiddenField ID="paidPremium" runat="server" />
                        <asp:HiddenField ID="subClass" runat="server" />
                        <asp:HiddenField ID="expireDate" runat="server" />
                        <asp:HiddenField ID="endorsementSubmitted" Value="false" runat="server" />
                    </div>
                </div>           

            <asp:Panel runat="server">
                <div class="container" runat="server">
                    <div class="row">
                        <div class="modal" id="myModal">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span></button>
                                        <h4 class="modal-title">Travel Endorsement</h4>
                                    </div>
                                    <div class="modal-body" runat="server" id="modalBodyText">
                                        Your you sure want authorize this endorsement ?
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnYes" type="button" Text="Yes" OnClientClick="showPageLoader();" OnClick="PopUpCon" runat="server" CssClass="btn btn-primary" />
                                        <%--<button type="button" runat="server" id="btnClose"  data-dismiss="modal" Class="btn btn-primary">Close</button>--%>
                                        <asp:Button ID="btnOK" type="button" OnClientClick="closePopup();" OnClick="Reset_Content" Text="No" runat="server" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                                <!-- /.modal-content -->
                            </div>
                            <!-- /.modal-dialog -->
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
