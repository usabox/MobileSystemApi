﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Dashboard : BasePage
{
    public string BannerDiv;
    public string FishingLink;

    protected override void OnLoad(EventArgs e)
    {
        Page.Title = "Dashboard";
        var deviceId = commonFunctions.getMobileDevice(Request);
        FishingLink = (deviceId == 2 || deviceId == 4) ? "https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/FishingMasterEN.apk" : "itms-services://?action=download-manifest&url=https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/manifest.plist";
        base.OnLoad(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        var banner = new Banner(commonFunctions.getMobileDevice(Request));
        BannerDiv = banner.GetBanners();
    }
}