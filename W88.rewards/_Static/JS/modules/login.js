﻿var Login = function(translations, elems, redirectUri, isVipLogin) {
    this.elems = elems;
    this.translations = translations;
    this.counter = 0;
    this.redirectUri = redirectUri;
    this.elems.captchaDiv.hide();
    this.isVipLogin = isVipLogin;
};

Login.prototype.initializeButtons = function() {
    var self = this,
        submitButton = self.elems.submitButton;
    submitButton.click(function(e) {
        submitButton.attr('disabled', true);
        var username = self.elems.username.val().trim(),
            password = self.elems.password.val().trim(),
            captcha = self.elems.captcha.val().trim(),
            hasError = false,
            message = '<ul>';

        if (username.length == 0) {
            message += '<li>' + self.translations.MissingUsername + '</li>';
            hasError = true;
        }
        if (password.length == 0) {
            message += '<li>' + self.translations.MissingPassword + '</li>';
            hasError = true;
        }
        if (!/^[a-zA-Z0-9]+$/.test(username)) {
            message += '<li>' + self.translations.InvalidUsernamePassword + '</li>';
            hasError = true;
        }
        if (captcha.length == 0 && self.counter >= 3) {
            message += '<li>' + self.translations.IncorrectVCode + '</li>';
            hasError = true;
        }

        if (hasError) {
            message += '</ul>';
            submitButton.attr('disabled', false);
            e.preventDefault();
            self.showMessage(message);
            return;
        }

        e.preventDefault();
        self.initiateLogin();
    });
};


Login.prototype.initiateLogin = function () {
    var self = this;
    $.ajax({
        type: 'POST',
        contentType: 'application/json',
        url: '/api/user/login',
        beforeSend: function() {
            GPINTMOBILE.ShowSplash();
        },
        timeout: function() {
            self.elems.submitButton.attr('disabled', false);
            self.showMessage(self.translations.Exception);
            window.location.href = '/Default.aspx';
        },
        data: JSON.stringify({           
            Username: self.elems.username.val(),
            Password: self.elems.password.val(),
            CaptchaCode: _.isEmpty(self.elems.captcha.val()) ? '' : self.elems.captcha.val(),
            VerificationCode: '',
            HasVCode: false        
        }),
        success: function(response) {
            if (!response || response.ResponseCode == undefined) {
                self.initiateLogin();
                return;
            }

            var message = response.ResponseMessage;
            switch (response.ResponseCode) {
                case 1:
                    window.user = (new User()).setProperties(response.ResponseData);
                    window.user.save();

                    $.ajax({
                        type: 'POST',
                        contentType: 'text/html',
                        url: '/_Secure/AjaxHandlers/Login.ashx',
                        beforeSend: function(request) {
                            request.setRequestHeader('token', window.user.Token);
                            request.setRequestHeader('isVipLogin', self.isVipLogin);
                        },
                        success: function (res) {
                            switch (res.Code) {
                                case 1:
                                    if (response.ResponseData.ResetPassword) {
                                        window.location.href = '/_Secure/ChangePassword.aspx';
                                        return;
                                    }
                                    if (!_.isEmpty(self.redirectUri)) {
                                        frsm_code = window.user.MemberId;
                                        window.location.href = self.redirectUri;
                                        return;
                                    }
                                    window.location.reload();
                                    break;
                                default:
                                    GPINTMOBILE.HideSplash();
                                    self.elems.submitButton.attr('disabled', false);
                                    setTimeout(function () {
                                        sessionId = window.user.Token;
                                        logout();
                                    }, 1500);
                                    if(self.isVipLogin)
                                        self.notAllowed();
                                    break;
                            }
                        },
                        error: function (err) {
                            GPINTMOBILE.HideSplash();
                            self.elems.submitButton.attr('disabled', false);
                            self.showMessage(self.translations.Exception);
                        }
                    });
                    break;
                default:
                    self.counter += 1;
                    if (self.counter >= 3) {
                        self.elems.captchaDiv.show();
                        self.elems.captchaImg.attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());
                        self.elems.captcha.val('');
                        self.elems.password.val('');
                    }
                    self.elems.submitButton.attr('disabled', false);
                    GPINTMOBILE.HideSplash();

                    if (response.ResponseCode == 21 || response.ResponseCode == 23) 
                        message = self.translations.InvalidUsernamePassword;
                    else if (response.ResponseCode == 22) 
                        message = self.translations.InactiveAccount;
                  
                    self.showMessage(message);
                    break;
            }
        },
        error: function(err) {
            GPINTMOBILE.HideSplash();
            self.showMessage(self.translations.Exception);
            self.elems.submitButton.attr('disabled', false);
        }
    });
};

Login.prototype.notAllowed = function () {
    var self = this;
    self.showMessage(self.translations.VipOnly);
};

Login.prototype.showMessage = function(message) {
    if (_.isEmpty(message)) return;
    window.w88Mobile.Growl.shout('<div>' + message + '</div>');
};