﻿<%@ Page Title="" Language="C#" MasterPageFile="~/v2/MasterPages/Payment.master" AutoEventWireup="true" CodeFile="Pay120280.aspx.cs" Inherits="v2_Deposit_Pay120280" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PaymentMainContent" Runat="Server">
        <div class="form-group">
        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtAmount" />
        <asp:TextBox ID="txtAmount" runat="server" type="number" step="any" min="1" CssClass="form-control" />
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsHolder" Runat="Server">
     <script type="text/javascript" src="/_static/v2/assets/js/gateways/alipay.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    
        <script type="text/javascript">
        $(document).ready(function () {
            _w88_paymentSvcV2.setPaymentTabs("<%=base.PaymentType %>", "<%=base.PaymentMethodId %>");
            _w88_paymentSvcV2.DisplaySettings("<%=base.PaymentMethodId %>", { type: "<%=base.PaymentType %>" });

             window.w88Mobile.Gateways.AlipayV2.init();

             $('#form1').submit(function (e) {
                 e.preventDefault();
                 var data = {
                     Amount: $('input[id$="txtAmount"]').val(),
                     ThankYouPage: location.protocol + "//" + location.host + "/Index",
                     MethodId: "<%=base.PaymentMethodId%>"
                };

                var params = decodeURIComponent($.param(data));
                window.open(_w88_paymentSvcV2.payRoute + "?" + params, "<%=base.PageName%>");
                _w88_paymentSvcV2.onTransactionCreated($(this));
                return;
            });

         });

    </script>
</asp:Content>

