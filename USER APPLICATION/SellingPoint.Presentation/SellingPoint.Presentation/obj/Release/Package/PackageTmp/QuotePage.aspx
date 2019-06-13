<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/General.master" CodeBehind="QuotePage.aspx.cs" Inherits="SellingPoint.Presentation.QuotePage" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <asp:UpdatePanel runat="server" ID="quoteUpdatePanel">
        <ContentTemplate>
    <div class="col-md-12">
        <div class="col-md-4 column-field">
            <div class="column-field-inner">
                <h3>Motor</h3>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Sum Insured:</label>
                    </div>
                    <div class="col-md-8 page-control">
                        <div class="control-label ">
                            <asp:TextBox ID="txtSumInsured" runat="server" CssClass="form-control col-md-10 onlynumber" TabIndex="20" />
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtSumInsured" ErrorMessage="Please enter suminsured" ControlToValidate="txtSumInsured" runat="server" ValidationGroup="MotorCalculationValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                   <div class="col-md-4 page-label">
                                            <label class="control-label">Used / New:</label>
                                        </div>
                                        <div class="col-md-8 page-control">
                                            <div class="control-label ">
                                                <asp:DropDownList ID="ddlVehicleType" runat="server" onchange="showPageLoader();" AutoPostBack="true" OnSelectedIndexChanged="VehicleType_Changed" CssClass="form-control col-md-10 chzn-select" TabIndex="17">
                                                    <asp:ListItem Text="Select" Value="-1" />
                                                    <asp:ListItem>Used</asp:ListItem>
                                                    <asp:ListItem>New</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvddlVehicleType" InitialValue="-1" ErrorMessage="Please select vehicle type" ControlToValidate="ddlVehicleType" runat="server" ValidationGroup ="MotorCalculationValidation" />
                                            </div>
                                        </div>                     
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Product:</label>
                    </div>
                    <div class="col-md-8 page-control">
                        <div class="control-label ">
                            <asp:DropDownList ID="ddlCover" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" TabIndex="24">
                                <asp:ListItem Text="Select" Value="-1" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                 <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Date Of Birth:</label>
                    </div>
                    <div class="col-md-8 page-control">
                        <div class="control-label ">
                            <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control col-md-10 dateofbirth" />
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvDateOfBirth" runat="server" ControlToValidate="txtDateOfBirth" ErrorMessage="Please enter dateofbirth" ValidationGroup="InsuredMasterValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Insurance period from:</label>
                    </div>
                    <div class="col-md-8 page-control">
                        <div class="control-label">
                            <asp:TextBox ID="txtInsuredPeriodFrom" runat="server" CssClass="form-control col-md-10 policydate" OnTextChanged="Calculate_MotorExpireDate" AutoPostBack="true" TabIndex="27" />
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtInsuredPeriodFrom" runat="server" ControlToValidate="txtInsuredPeriodFrom" ErrorMessage="Please enter policy start date" ValidationGroup="MotorCalculationValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Insurance period to:</label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtInsuredPeriodTo" runat="server" CssClass="form-control col-md-10 datepicker" TabIndex="28" />
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtInsuredPeriodTo" runat="server" ControlToValidate="txtInsuredPeriodTo" ErrorMessage="Please enter policy end date" ValidationGroup="MotorCalculationValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label"><strong>Premium:</strong></label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtMotorPremium" runat="server" CssClass="form-control onlynumber" />
                        </div>
                    </div>
                </div>
                  <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label"><strong>VAT:</strong></label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtMotorVat" runat="server" CssClass="form-control onlynumber" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label"><strong>Total:</strong></label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtMotorTotal" runat="server" CssClass="form-control onlynumber" />
                        </div>
                    </div>
                </div>
                <div class="col-md-12 form-group textcenter">
                    <asp:Button runat="server" ID="btnMotorCalculate" Text="Calculate" OnClientClick="showPageLoader('MotorCalculationValidation');" OnClick="Calculate_Motor" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
        <div class="col-md-4 column-field">
            <div class="column-field-inner">
                <h3>Home</h3>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Building Value:</label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtBuildingValue" runat="server" CssClass="form-control col-md-10 onlynumber" />
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtBuildingValue" runat="server" ControlToValidate="txtBuildingValue" ErrorMessage="Please enter building value" ValidationGroup="HomeCalculationValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Content Value:</label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtContentValue" runat="server" CssClass="form-control col-md-10 onlynumber"/>
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtContentValue" runat="server" ControlToValidate="txtContentValue" ErrorMessage="Please enter content value" ValidationGroup="HomeCalculationValidation" />
                        </div>
                    </div>
                </div>
                 <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Jewellery Value:</label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtJewelleryValue" runat="server" CssClass="form-control col-md-10 onlynumber"/>
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtJewelleryValue" runat="server" ControlToValidate="txtJewelleryValue" ErrorMessage="Please enter jewellery value" ValidationGroup="HomeCalculationValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">DomesticHelp Worker:</label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtDomesticHelpWorkers" runat="server" CssClass="form-control col-md-10 onlynumber"/>
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvtxtDomesticHelpWorkers" runat="server" ControlToValidate="txtDomesticHelpWorkers" ErrorMessage="Please enter number of domestic helper" ValidationGroup="HomeCalculationValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">RSM:</label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:DropDownList ID="ddlMaliciousDamageCover" CssClass="form-control col-md-10" runat="server">
                                <asp:ListItem Text="Select" Value="-1" />
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlMaliciousDamageCover" ErrorMessage="Please select this field" ControlToValidate="ddlMaliciousDamageCover" runat="server" ValidationGroup="HomeCalculationValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Jewellery Added:</label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:DropDownList ID="ddlJewelleryCoverWithinContents" CssClass="form-control col-md-10" runat="server">
                                <asp:ListItem Text="Select" Value="-1" />
                                <asp:ListItem Value="NO COVER">No Cover</asp:ListItem>
                                <asp:ListItem Value="STANDARD">15% of Contents value up to BD 2,500</asp:ListItem>
                                <asp:ListItem Value="EXTENDED">25% of contents value up to BD 5,000</asp:ListItem>
                                <asp:ListItem Value="EXTREME">Contents above BD 5,000</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlJewelleryCoverWithinContents" ErrorMessage="Please select this field" ControlToValidate="ddlJewelleryCoverWithinContents" runat="server" ValidationGroup="HomeCalculationValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label"><strong>Premium:</strong></label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtHomePremium" runat="server" CssClass="form-control onlynumber" />
                        </div>
                    </div>
                </div>
                 <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label"><strong>VAT:</strong></label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtHomeVat" runat="server" CssClass="form-control onlynumber" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label"><strong>Total:</strong></label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtHomeTotal" runat="server" CssClass="form-control onlynumber" />
                        </div>
                    </div>
                </div>
                <div class="col-md-12 form-group textcenter">
                    <asp:Button runat="server" ID="btnHomeCalculate" Text="Calculate" OnClientClick="showPageLoader('HomeCalculationValidation');" OnClick="Calculate_Home" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
        <div class="col-md-4 column-field">
            <div class="column-field-inner">
                <h3>Travel</h3>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Package:</label>
                    </div>
                    <div class="col-md-8 page-control">
                        <div class="control-label ">
                            <asp:DropDownList ID="ddlPackage" runat="server" CssClass="form-control chzn-select col-md-10" AutoPostBack="True" OnSelectedIndexChanged="ddlPackage_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="err" ID="rfvddlPackage" ErrorMessage="Please select package" ControlToValidate="ddlPackage" runat="server" ValidationGroup="travelValidation" />
                        </div>                      
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Coverage(Journey) *</label>
                    </div>
                    <div class="col-md-8 page-control">
                        <asp:DropDownList ID="ddlJourney" runat="server" CssClass="form-control col-md-10 chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlJourney_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="err" ID="rfvddlJourney" ErrorMessage="Please select journey" ControlToValidate="ddlJourney" runat="server" ValidationGroup="travelValidation" />
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Period *</label>
                    </div>
                    <div class="col-md-8 page-control">
                        <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="form-control col-md-10 chzn-select">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="err" ID="rfvddlPeriod" ErrorMessage="Please select period" ControlToValidate="ddlPeriod" runat="server" ValidationGroup="travelValidation" />
                    </div>
                </div>
                <div class="form-group col-md-12" runat="server" id="txtDOBID">
                    <div class="col-md-4 page-label">
                        <label class="control-label">Date Of Birth:</label>
                    </div>
                    <div class="col-md-8 page-control">
                        <div class="control-label ">
                            <asp:TextBox ID="txtTravelDOB" runat="server" CssClass="form-control col-md-10 dateofbirth" />
                            <asp:RequiredFieldValidator CssClass="err" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTravelDOB" ErrorMessage="Please enter dateofbirth" ValidationGroup="travelValidation" />
                        </div>
                    </div>
                </div>
                <div class="form-group col-md-12">
                    <div class="col-md-4 page-label">
                        <label class="control-label"><strong>Premium:</strong></label>
                    </div>
                    <div class="col-md-8">
                        <div class="control-label page-control">
                            <asp:TextBox ID="txtTravelPremium" runat="server" CssClass="form-control onlynumber" />
                        </div>
                    </div>
                </div>
                <div class="col-md-12 form-group textcenter">
                    <asp:Button runat="server" ID="btnCalculate" Text="Calculate" OnClientClick="showPageLoader('travelValidation');" OnClick="Calculate_Travel" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>
  </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>