﻿using System;

public partial class v2_Deposit_Pay1202134 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.AifuAlipay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AifuAlipay);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}