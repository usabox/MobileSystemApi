﻿using System;
using System.Web;
using System.Web.UI;
using W88.BusinessLogic.Accounts.Helpers;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities;


public class BasePage : Page
{
    protected bool HasSession = false;
    protected MemberSession MemberSession = null;
    protected UserSessionInfo UserSessionInfo = null;
    protected MemberRewardsInfo MemberRewardsInfo = null;
    protected Members MembersHelper = new Members();
    protected RewardsHelper RewardsHelper = new RewardsHelper();
    protected string Language = string.Empty;

    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            if (bool.Parse(Common.GetAppSetting<string>("ClearWebCache")))
            {
                foreach (System.Collections.DictionaryEntry deCache in HttpContext.Current.Cache)
                {
                    HttpContext.Current.Cache.Remove(Convert.ToString(deCache.Key));
                }
            }
        }
        finally
        {
            var language = HttpContext.Current.Request.QueryString.Get("lang");
            Language = !string.IsNullOrEmpty(language) ? language : LanguageHelpers.SelectedLanguage;
            CheckSession();
        }
    }

    protected async void CheckSession()
    {
        try
        {
            var token = HttpContext.Current.Request.QueryString.Get("token");
            if (!string.IsNullOrEmpty(token))
            {                
                token = Encryption.Decrypt(W88.Utilities.Constant.EncryptionType.TripleDESCS, token);               
            }

            if (string.IsNullOrEmpty(token))
            {
                token = HttpContext.Current.Request.Headers.Get("token");
                if (string.IsNullOrEmpty(token))
                {
                    var cookie = HttpContext.Current.Request.Cookies["user"];
                    if (cookie == null) return;
                    var user = Common.DeserializeObject<MemberSession>(cookie.Value);
                    if (user == null) return;
                    token = user.Token;     
                }
            }

            if (string.IsNullOrEmpty(token)) return;
            var process = await MembersHelper.MembersSessionCheck(token);
            HasSession = process.Code == 1 && !string.IsNullOrEmpty(process.Data.Token);
            if (!HasSession) return;
            MemberSession = process.Data;
            UserSessionInfo = await MembersHelper.GetMemberInfo(token);
            SetMemberRewardsInfo();
        }
        catch (Exception)
        {

        }
    }

    protected async void SetMemberRewardsInfo()
    {
        if (MemberSession == null || UserSessionInfo == null) return;
        MemberRewardsInfo = new MemberRewardsInfo();
        MemberRewardsInfo.CurrentPoints = await MembersHelper.GetRewardsPoints(UserSessionInfo);
        MemberRewardsInfo.CurrentPointLevel = await RewardsHelper.GetPointLevel(MemberSession.MemberId);
    }

    protected bool IsVip
    {     
        get
        {
            var vipCookie = CookieHelpers.CookieVip;
            bool isVip;
            bool.TryParse(vipCookie, out isVip);
            return isVip;
        }
    }

    protected string GetTranslation(string key, string fileName = "")
    {
        return CultureHelpers.GetTranslation(key, 
            string.IsNullOrEmpty(Language) ? LanguageHelpers.SelectedLanguage : Language, 
            string.Format("contents/{0}", string.IsNullOrEmpty(fileName) ? "translations" : fileName));
    }
}