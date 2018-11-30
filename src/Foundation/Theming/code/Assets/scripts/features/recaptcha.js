function recaptchaCallback() {
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
}$(function () {    // injust necessary span tag to render psuedo controls    $('.psuedo-checkbox--inject, .psuedo-radio--inject').append('<span></span>');    $('.warranty__country label:has(input)').append('<span></span>');});