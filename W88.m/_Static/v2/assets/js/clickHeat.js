﻿
$(document).ready(function() {
    var url = window.location.href;

    if (url.indexOf("/Slots.aspx") > 0) {
        clickHeatGroup = 'W88MobileSlotsHome';
    } else if (url.indexOf("/Index.aspx") > 0) {
        clickHeatGroup = 'W88MobileIndex';
    }

    if (clickHeatGroup.length > 0) {
        clickHeatSite = 'W88Mobile';
        clickHeatServer = 'https://clickheat.liveperson88.com/clickheat/clickempty.html';
        initClickHeat();
    }
});