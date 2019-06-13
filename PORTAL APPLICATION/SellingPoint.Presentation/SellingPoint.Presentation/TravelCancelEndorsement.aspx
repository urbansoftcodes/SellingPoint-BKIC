﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.Master" CodeBehind="TravelCancelEndorsement.aspx.cs" Inherits="SellingPoint.Presentation.TravelCancelEndorsement" %>

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
        function ShowMessage() {
            $('#myModal').modal('show');
        }
        //function showDeletePopup() {
        //    $('#myModalDelete').modal('show');

        //}
        // function closeDeletePopup() {
        //    $('#myModalDelete').modal('hide');
        //      $("#myModalDelete .modal-backdrop").remove();
        //}

        $(function () {
            autocompleteCPR();
            getTravelEndorsementPolicies();
        }); 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" ID="motorEndorsementUpdatePanel">
        <ContentTemplate>
            <div class="adv-table editable-table ">
                <asp:GridView ID="gvTravelEndorsement" runat="server" OnDataBound="gvTravelEndorsement_DataBound" OnRowDataBound="gvTravelEndorsement_RowDataBound" CssClass="table table-bordered" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" Width="100%" OnSorting="gv_Sorting" PageSize="10">
                    <HeaderStyle CssClass="bcolorhead fcolorwhite" />
                    <Columns>
                       <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSNo" runat="server" Text=" <%# Container.DataItemIndex + 1 %>"></asp:Label>
                                <asp:Label ID="lblTravelID" runat="server" Text='<%# Eval("TravelID") %>' Visible="false"></asp:Label>
                                 <asp:Label ID="lblDocumentNo" runat="server" Text='<%# Eval("DocumentNo") %>' Visible="false"></asp:Label>
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
                           <%-- <asp:HiddenField ID="renewalCount" runat="server" />--%>
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
                                <asp:RequiredFieldValidator CssClass="err" SetFocusOnError="true" ID="rfvddlBranch" ErrorMessage="Please select branch" ControlToValidate="ddlBranch" runat="server" ValidationGroup="TravelEndorsementValidation" />
                            </div>
                        </div>                                            
                        <div class="clearfix"></div>                       
                        <div class="form-group col-md-4">
                            <div class="col-md-4 page-label">
                                <label class="control-label">Policy Number: *</label>
                            </div>
                            <div class="col-md-8 page-control">
                                <asp:DropDownList ID="ddlTravelPolicies" runat="server"  CssClass="form-control col-md-10 chzn-select" onChange="showPageLoader();" OnSelectedIndexChanged="Changed_TravelPolicy" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlTravelPolicies" ErrorMessage="Please select policy" ControlToValidate="ddlTravelPolicies" runat="server" ValidationGroup="TravelEndorsementValidation" />
                            </div>
                        </div> 
                         <div class="form-group col-md-4">
                            <div class="col-md-4 page-label">
                                <label class="control-label">Refund Type: *</label>
                            </div>
                            <div class="col-md-8 page-control">
                                <asp:DropDownList ID="ddlRefundType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RefundType_Changed" CssClass="form-control col-md-10 chzn-select">
                                    <asp:ListItem Text="Select" Value="-1" />
                                    <asp:ListItem>Full Amount</asp:ListItem>
                                    <asp:ListItem>Pro Rate</asp:ListItem>
                                </asp:DropDownList>   
                                 <asp:RequiredFieldValidator InitialValue="-1" ID="rfvddlRefundType" ErrorMessage="Please select refund type" ControlToValidate="ddlRefundType" runat="server" ValidationGroup="TravelEndorsementValidation" />
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
                                <div class="form-group col-md-6" id="divEffectiveFrom" runat="server" visible="false">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Effective From :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtEffectiveFromDate" runat="server" CssClass="form-control col-md-10 datepicker" />
                                    </div>
                                </div>
                                <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Efecctive From :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtEffectiveToDate" runat="server" CssClass="form-control col-md-10 cancel-date" />
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
                                   <div class="form-group col-md-6">
                                    <div class="col-md-4 page-label">
                                        <label class="control-label">Remarks :</label>
                                    </div>
                                    <div class="col-md-8 page-control">
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control col-md-10" />
                                    </div>
                                </div>
                            </div>
                        </div>                           
                    </div>
                </div>
            </div>
            <div class="x_panel">                
                <div class="x_content">  
                     <div class="col-md-12 form-group calc-wrapper ">
                    <div runat="server" id="amtDisplay" class="calculate-amount">
                        <div class="dsc-overallwrapper vat_calcuation" id="includeDisc" runat="server" visible="false">
                            <div class="dsc-wrapper">
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">PremiumAmount : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="premiumAmount" Enabled="false" AutoPostBack="true" onChange="showPageLoader();" OnTextChanged="Premium_Changed" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">Commission : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="commission" Enabled="false" AutoPostBack="true" onChange="showPageLoader();" OnTextChanged="Commission_Changed" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="dsc-wrapper">
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">VAT Amount : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtVATAmount" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">VAT Commission : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtVATCommission" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="dsc-wrapper">
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">Total Premium : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtTotalPremium" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">Total Commission : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtTotalCommission" Enabled="false" runat="server" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="dsc-wrapper">
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">Discount : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtDiscount" AutoPostBack="true" OnTextChanged="validate_Premium" runat="server" CssClass="form-control col-md-10 onlynumber" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="dsc-overallwrapper vat_calcuation" id="excludeDisc" runat="server" visible="false">
                            <div class="dsc-wrapper">
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">PremiumAmount : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="premiumAmount1" Enabled="false" AutoPostBack="true" runat="server" OnTextChanged="validate_Premium" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">Commission : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="commission1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="dsc-wrapper">
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">VAT Amount : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtVATAmount1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">VAT Commission : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtVATCommission1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="dsc-wrapper">
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">Total Premium : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtTotalPremium1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-5 page-label">
                                        <label class="control-label">Total Commission : </label>
                                    </div>
                                    <div class="col-md-7 page-control">
                                        <div class="control-label">
                                            <asp:TextBox ID="txtTotalCommission1" runat="server" Enabled="false" CssClass="form-control col-md-10 onlynumber"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                    <div class="btn-wrap-row text-center">
                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CssClass="btn btn-primary" OnClientClick="showPageLoader('TravelEndorsementValidation');" OnClick="Calculate_Click" />
                        <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="showPageLoader('TravelEndorsementValidation');" OnClick="btnSubmit_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-primary" OnClick="btnBack_Click" ValidationGroup="TravelEndorsementValidation" />
                        <%-- <a runat="server" id="downloadschedule" class="btn btn-primary" title="Schedule" onclick="btnPrint_Click"><i class="fa fa-download" aria-hidden="true"></i>Schedule</a>
                        <a runat="server" id="downloadCertificate" class="btn btn-primary" title="Certificate" onclick="btnPrint_Click"><i class="fa fa-download" aria-hidden="true"></i>Certificate</a>--%>
                        <asp:HiddenField ID="calculatedPremium" runat="server" />
                        <asp:HiddenField ID="calculatedCommission" runat="server" />
                        <asp:HiddenField ID="adjustedPremium" runat="server" />
                        <asp:HiddenField ID="adjustedCommission" runat="server" />
                        <asp:HiddenField ID="insuredDOB" runat="server" />
                        <asp:HiddenField ID="paidPremium" runat="server" />
                        <asp:HiddenField ID="subClass" runat="server" />
                         <asp:HiddenField ID="todayDate" runat="server" />
                        <asp:HiddenField ID="expireDate" runat="server" />
                        <asp:HiddenField ID="endorsementSubmitted" Value="false" runat="server" />
                    </div>
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
                                        <asp:Button ID="btnYes" type="button" Text="Yes" OnClientClick="showPageLoader();" OnClick="btnAuthorize_Click" runat="server" CssClass="btn btn-primary" />
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
