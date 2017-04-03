﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using W88.BusinessLogic.Rewards.Helpers;

public partial class _Lang : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var divBuilder = new StringBuilder();
        var enabledLanguages = OperatorSettings.Get("EnabledLanguages");
        var availableLanguages = OperatorSettings.Get("LanguageSelection").Split('|');
        foreach (var language in availableLanguages)
        {
            divBuilder.Append(@"<div class='col-xs-6'>")
                .Append(@"<span id='")
                .Append(language)
                .Append("' class='flags'></span>")
                .Append(@"</div>");
        }
        divLanguageContainer.InnerHtml = divBuilder.ToString();
    }

    private NameValueCollection OperatorSettings
    {
        get
        {
            return ConfigurationManager.GetSection("OperatorGroupSettings/W88") as NameValueCollection;
        }
    }
}