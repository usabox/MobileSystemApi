﻿using System;
 using System.Data;
 using System.Web;

namespace Helpers
{
    /// <summary>
    /// Summary description for Members
    /// </summary>
    public class Members
    {
        public int CheckMemberSession(string memberSessionId = null, string password = null)
        {
            try
            {
                using (var svcInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    var id = memberSessionId ?? commonVariables.CurrentMemberSessionId;
                    var dsMember = svcInstance.MemberSessionCheck(id, commonIp.UserIP);

                    if (dsMember != null)
                    {
                        if (dsMember.Tables[0].Rows.Count > 0)
                        {
                            SetSessions(dsMember.Tables[0], password);
                        }
                    }

                    return dsMember == null ? 0 : Convert.ToInt32(dsMember.Tables[0].Rows[0]["RETURN_VALUE"]);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void SetSessions(DataTable dTable, string password)
        {
            var memberSessionId = dTable.Rows[0]["memberSessionId"];
            commonVariables.SetSessionVariable("MemberSessionId", memberSessionId.ToString());
            commonVariables.SetSessionVariable("MemberId", dTable.Rows[0]["memberId"].ToString());
            commonVariables.SetSessionVariable("MemberCode", dTable.Rows[0]["memberCode"].ToString());
            commonVariables.SetSessionVariable("CountryCode", dTable.Rows[0]["countryCode"].ToString());
            commonVariables.SetSessionVariable("CurrencyCode", dTable.Rows[0]["currency"].ToString());
            commonVariables.SetSessionVariable("LanguageCode", dTable.Rows[0]["languageCode"].ToString());
            commonVariables.SetSessionVariable("RiskId", dTable.Rows[0]["riskId"].ToString());
            commonVariables.SetSessionVariable("PaymentGroup", dTable.Rows[0]["paymentGroup"].ToString());
            commonVariables.SetSessionVariable("PartialSignup", dTable.Rows[0]["partialSignup"].ToString());
            commonVariables.SetSessionVariable("ResetPassword", dTable.Rows[0]["resetPassword"].ToString());
            commonVariables.SetSessionVariable("MemberName", Convert.ToString(dTable.Rows[0]["Lastname"]) + Convert.ToString(dTable.Rows[0]["Firstname"]));

            commonCookie.CookieS = Convert.ToString(memberSessionId);
            commonCookie.CookieG = Convert.ToString(memberSessionId);

            if (password != null)
                commonCookie.CookiePalazzo = password;
        }
    }
}