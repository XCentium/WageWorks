﻿function recaptchaCallback() {
    var responseInput = $('.recapcha-response');
    if (responseInput.length != 0) {
        responseInput.val('success');
    }
}
function recaptchaDataExpiredCallback() {
    var responseInput = $('.recapcha-response');
    if (responseInput.length != 0) {
        responseInput.val('');
    }
}