﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class _Secure_Login : BasePage
{
    protected XElement xeErrors = null;
    protected string strRedirect = string.Empty;
    protected string urlRedirect = string.Empty;
    protected bool isSlotRedirect = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);

        strRedirect = Request.QueryString.Get("redirect");
        urlRedirect = Request.QueryString.Get("url");

        if (!string.IsNullOrWhiteSpace(strRedirect) && (strRedirect.ToLower().Contains("deposit") || strRedirect.ToLower().Contains("withdraw")))
            strRedirect = strRedirect.Replace("_app", "");

        commonCookie.CookieIsApp = null;
        if (!string.IsNullOrEmpty(Request.QueryString.Get("token")))
        {
            try
            {
                commonCookie.CookieIsApp = "1";
                var cipherKey = commonEncryption.Decrypt(ConfigurationManager.AppSettings.Get("PrivateKeyToken"));
                string strSessionId = commonEncryption.DecryptToken(Request.QueryString.Get("token"), cipherKey);

                var loginCode = UserSession.checkSession(strSessionId);

                if (loginCode != 1)
                {
                    UserSession.ClearSession();
                }
                else
                {
                    if (!string.IsNullOrEmpty(strRedirect))
                    {
                        Response.Redirect(strRedirect, false);
                    }
                    else
                    {
                        Response.Redirect("/Funds.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                UserSession.ClearSession();
            }
        }
        else
        {
            if (string.IsNullOrEmpty(strRedirect))
            {
                if (!string.IsNullOrEmpty(urlRedirect))
                {
                    isSlotRedirect = true;

                    if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                    {
                        try
                        {
                            var link = new Uri(Server.UrlDecode(urlRedirect));
                            NameValueCollection nvc = HttpUtility.ParseQueryString(link.Query);

                            var tokenArray = new string[] { "token", "s" };
                            bool isEmpty = true;

                            foreach (var item in tokenArray)
                            {
                                if (!string.IsNullOrEmpty(nvc[item]))
                                {
                                    isEmpty = false;

                                    if (nvc.AllKeys.Contains(item))
                                    {
                                        nvc.Remove(item);
                                        nvc.Add(item, commonVariables.CurrentMemberSessionId);
                                    }
                                }
                            }

                            if (isEmpty)
                            {
                                nvc.Add("s", commonVariables.CurrentMemberSessionId);
                            }

                            var domainArray = new string[] { "domainlink", "domain" };

                            foreach (var item in domainArray)
                            {
                                if (nvc.AllKeys.Contains(item))
                                {
                                    nvc.Remove(item);
                                    nvc.Add(item, (commonIp.DomainName).Trim(new char[] { '.' }));
                                }
                            }
                            if (link.Query.Length > 0)
                            {
                                link = new Uri(link.ToString().Replace(link.Query, ""));
                            }

                            Response.Redirect(link.ToString() + "?" + nvc.ToString(), false);
                            Response.End();
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        Response.Redirect("/", true);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(urlRedirect))
                        {
                            strRedirect = urlRedirect;
                        }
                    }
                }
                else
                {
                    strRedirect = "/Index.aspx?lang=" + commonVariables.SelectedLanguage;
                }
            }
        }

        if (!Page.IsPostBack)
        {
            lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
            lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
            lblCaptcha.Text = commonCulture.ElementValues.getResourceString("lblCaptcha", xeResources);
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnLogin", xeResources);

            txtUsername.Focus();

            lblRegister.Text = commonCulture.ElementValues.getResourceString("btnRegister", xeResources);
        }
    }
}
