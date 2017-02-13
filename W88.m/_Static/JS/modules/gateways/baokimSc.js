﻿window.w88Mobile.Gateways.BaokimScratchCard = BaokimScratchCard();
var _w88_baokimSc = window.w88Mobile.Gateways.BaokimScratchCard;

function BaokimScratchCard() {

    var defaultSelect = "";
    var telcos = "";
    var baokimSc = Object.create(new w88Mobile.Gateway(_w88_paymentSvcV2));

    baokimSc.init = function () {
        setTranslations();
        function setTranslations() {
            if (_w88_contents.translate("LABEL_PAYMENT_NOTE") != "LABEL_PAYMENT_NOTE") {
                $('label[id$="lblDepositAmount"]').text(_w88_contents.translate("LABEL_CARD_AMOUNT"));
                $('label[id$="lblBanks"]').text(_w88_contents.translate("LABEL_TELCO_NAME"));
                $('label[id$="lblPin"]').text(_w88_contents.translate("LABEL_CARD_PIN"));
                $('label[id$="lblCardSerialNo"]').text(_w88_contents.translate("LABEL_CARD_SERIAL"));

                sessionStorage.setItem("indicator", _w88_contents.translate("LABEL_INDICATOR_MSG"));
                $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator"));

                defaultSelect = _w88_contents.translate("LABEL_SELECT_DEFAULT");

                getBanks();

            } else {
                window.setInterval(function () {
                    setTranslations();
                }, 500);
            }
        }
    };

    baokimSc.getBanks = function() {
        var url = w88Mobile.APIUrl + "/payments/baokindenom/";

        $.ajax({
            type: "GET",
            url: url,
            success: function(d) {
                telcos = d.ResponseData.Telcos;

                $('select[id$="drpBanks"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpBanks"]').val("-1").selectmenu("refresh");

                _.forOwn(d.ResponseData.Telcos, function(data) {
                    $('select[id$="drpBanks"]').append($('<option>').text(data.Name).attr('value', data.Id));
                });


                $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
                $('select[id$="drpAmount"]').val("-1").selectmenu("refresh");
            }
        });
    };

    baokimSc.setDenom = function(selectedValue) {
        var telco = _.find(telcos, function(data) {
            return data.Id == selectedValue;
        });

        $('select[id$="drpAmount"]').empty();

        $('select[id$="drpAmount"]').append($('<option>').text(defaultSelect).attr('value', '-1'));
        $('select[id$="drpAmount"]').val("-1").selectmenu("refresh");

        _.forOwn(telco.Denominations, function(data) {
            $('select[id$="drpAmount"]').append($('<option>').text(data.Text).attr('value', data.Value));
        });
    };

    baokimSc.setFee = function(selectedValue) {

        var fee = "";

        _.forEach(telcos, function(i) {
            if (i.Id == selectedValue) {
                fee = i.Fee;
            }
        });

        $('p[id$="IndicatorMsg"]').html(sessionStorage.getItem("indicator") + fee);
    };

    baokimSc.createDeposit = function() {
        var _self = this;
        var params = _self.getUrlVars();
        var data = {
            Amount: params.Amount,
            CardType: { Text: params.CardTypeTextText, Value: params.CardTypeTextValue },
            AccountName: params.AccountName,
            CardNumber: params.CardNumber,
            CardExpiryMonth: params.CardExpiryMonth,
            CardExpiryYear: params.CardExpiryYear,
            CCV: params.CCV
        };

        _self.methodId = params.MethodId;
        _self.changeRoute();
        _self.deposit(data, function(response) {
                switch (response.ResponseCode) {
                case 1:
                    w88Mobile.PostPaymentForm.createv2(response.ResponseData.FormData, response.ResponseData.PostUrl, "body");
                    w88Mobile.PostPaymentForm.submit();

                    $('#form1')[0].reset();
                    break;
                default:
                    if (_.isArray(response.ResponseMessage))
                        w88Mobile.Growl.shout(w88Mobile.Growl.bulletedList(response.ResponseMessage));
                    else
                        w88Mobile.Growl.shout(response.ResponseMessage);

                    break;
                }
            },
            function() {
                GPInt.prototype.HideSplash();
            });
    };

    return baokimSc;
}