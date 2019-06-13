<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageLoader.ascx.cs" Inherits="BKIC.SellingPoint.Presentation.PageLoader" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:ModalPopupExtender runat="server" ID="MPE" BehaviorID="PleaseWaitPopup" BackgroundCssClass="BGCss" TargetControlID="loading" PopupControlID="loading">
</asp:ModalPopupExtender>

<asp:Panel runat="server" ID="loading">
    <style>
        #pageLoadUpdatePanel {
                                position: fixed;
                                top: 0;
                                left: 0;
                                width: 100%;
                                height: 100%;
                                z-index: 999999999;
                                background: rgba(53, 53, 53, 0.84);
                               }
    </style>

   <%-- <img src="img/ui2images/loading.png" border="0" />--%>
    <div class="middle-wrap-animator">
                <div class="text-center ld ld-flip-h">
                   <%-- <img src="img/animator.gif" class="animation-image loader-image" />--%>
                    <img src="assets/images/loading.png" class="animation-image loader-image" style="width: 60px;"/>
                    <link href="assets/css/loading.css" rel="stylesheet" />
                </div>
            </div>
</asp:Panel>