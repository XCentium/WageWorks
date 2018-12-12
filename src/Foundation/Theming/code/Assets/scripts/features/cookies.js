

var WageWorks = WageWorks || {};
WageWorks.Cookies = (function ($) {

    function checkCookies() {
        var dissmissSections = $(".dismissable .dismiss-close");
        $.each(dissmissSections, function () {
            var dissminS = $(this).attr("data-dismissable-cookie");
            var cookieArr = Cookies.get();
            var parent = $(this).parent();


            var parent = $(this).parent();

            $.each(cookieArr, function (name, value) {
                //console.log(name + ',' + value)
                if (name == dissminS) {
                    parent.hide();
                    //console.log('yes - ' + name + ',' + dissminS);
                    return false;
                } else {
                    parent.show();
                    //console.log('no - ' + name + ',' + dissminS);
                }
            });
        });
    };

    function dissmissSection() {
        $(".dismissable .dismiss-close").on("click", function () {
            //console.log('start create cookie');
            var Number = $(this).attr("data-dismissable-period");
            var cookieName = $(this).attr("data-dismissable-cookie");
            var parent = $(this).parent(".dismissable");
            //console.log(Number  + " , " + cookieName);
            parent.hide();
            Cookies.set(cookieName, 'active', { expires: parseInt(Number)});
        });
    };



    function init() {
        checkCookies();
        dissmissSection();
    }

    return {
        init: init
    }
})(jQuery);

$(function () {
    WageWorks.Cookies.init();
});