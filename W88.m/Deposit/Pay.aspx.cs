﻿using System;

public partial class Deposit_Pay : PaymentBasePage
{
    public string GatewayFile;

    protected void Page_Load(object sender, EventArgs e)
    {
        var methodId = Request.QueryString["MethodId"];

        if (string.IsNullOrWhiteSpace(methodId))
            return;

        commonVariables.DepositMethod PaymentMethodId;
        if (!Enum.TryParse(methodId, out PaymentMethodId))
            return;

        switch (PaymentMethodId)
        {
            case commonVariables.DepositMethod.PaySec:
                GatewayFile = "paysec";
                break;
            case commonVariables.DepositMethod.TongHuiPay:
            case commonVariables.DepositMethod.TongHuiAlipay:
            case commonVariables.DepositMethod.TongHuiWeChat:
                GatewayFile = "tonghui";
                break;
            case commonVariables.DepositMethod.NineVPayAlipay:
                GatewayFile = "ninevpay";
                break;
            case commonVariables.DepositMethod.JuyPayAlipay:
            case commonVariables.DepositMethod.SDPay:
                GatewayFile = "juypay";
                break;
            case commonVariables.DepositMethod.JTPayWeChat:
            case commonVariables.DepositMethod.JTPayAliPay:
                GatewayFile = "jtpay";
                break;
            case commonVariables.DepositMethod.ECPSS:
                GatewayFile = "ecpss";
                break;
            case commonVariables.DepositMethod.DinPayTopUp:
                GatewayFile = "dinpay";
                break;
            case commonVariables.DepositMethod.IWallet:
                GatewayFile = "iwallet";
                break;
            case commonVariables.DepositMethod.KexunPayWeChat:
            case commonVariables.DepositMethod.AllDebitWeChat:
            case commonVariables.DepositMethod.HebaoWeChat:
                GatewayFile = "kexunpay";
                break;
            case commonVariables.DepositMethod.KDPayWeChat:
                GatewayFile = "kdpay";
                break;
            case commonVariables.DepositMethod.Help2Pay:
                GatewayFile = "help2pay";
                break;
            case commonVariables.DepositMethod.ShengPayAliPay:
            case commonVariables.DepositMethod.AllDebitAlipay:
                GatewayFile = "shengpay";
                break;
            case commonVariables.DepositMethod.Cubits:
                GatewayFile = "cubits";
                break;
            case commonVariables.DepositMethod.NextPay:
                GatewayFile = "nextpay";
                break;
            case commonVariables.DepositMethod.NextPayGV:
                GatewayFile = "nextpaygv";
                break;
            case commonVariables.DepositMethod.EGHL:
                GatewayFile = "eghl";
                break;
            case commonVariables.DepositMethod.AifuAlipay:
            case commonVariables.DepositMethod.AifuWeChat:
                GatewayFile = "aifu";
                break;
            case commonVariables.DepositMethod.PayTrust:
                GatewayFile = "paytrust";
                break;
            case commonVariables.DepositMethod.AloGatewayWeChat:
                GatewayFile = "alogateway";
                break;
            case commonVariables.DepositMethod.AllDebitB2C:
            case commonVariables.DepositMethod.HebaoB2C:
                GatewayFile = "quickonline";
                break;
        }

        commonVariables.AutoRouteMethod autoRouteId;
        if (!Enum.TryParse(methodId, out autoRouteId))
            return;

        switch (autoRouteId)
        {
            case commonVariables.AutoRouteMethod.QuickOnline:
                GatewayFile = "quickonline";
                break;
            case commonVariables.AutoRouteMethod.WeChat:
                GatewayFile = "wechatpay";
                break;
            case commonVariables.AutoRouteMethod.AliPay:
                GatewayFile = "alipay2";
                break;
        }

    }
}