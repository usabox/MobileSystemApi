﻿using w88Mobile;

namespace Helpers
{
    /// <summary>
    /// Summary description for BusinessRules
    /// </summary>
    public static class BusinessRules
    {

        public static void SetRules()
        {
            CheckClubApollo();
        }

        private static void CheckClubApollo()
        {
            var currency = commonVariables.GetSessionVariable(MemberInfo.CurrencyCode);
            if (currency.Contains("KRW") || currency.Contains("VND") || currency.Contains("IDR"))
                commonVariables.SetSessionVariable("clubapollo", "0");
            else
                commonVariables.SetSessionVariable("clubapollo", "1");
        }

    }
}