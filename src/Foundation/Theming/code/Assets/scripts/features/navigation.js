var Wageworks = Wageworks || {};
Wageworks.Navigation = (function ($) {

    function stickyNav() {
        var stickyNav = $('.navbar-sticky');
        var didScroll;
        var lastScrollTop = 0;
        var delta = 5;

        if ($(window).width() > 992) {
            
            $(window).scroll(function (e) {
                userScrolled = true;

                setInterval(function () {
                    if (userScrolled) {
                        hasScrolled();
                        didScroll = false;
                    }
                }, 250);
            });

            function hasScrolled() {
                var top = $(this).scrollTop();

                if (Math.abs(lastScrollTop - top) <= delta)
                    return;

                if (top > lastScrollTop && top > 100) {
                    $('.navbar-sticky').hide();
                } else {
                    if (top <= 10) {
                        $('.navbar-sticky').show();
                    }
                }
                lastScrollTop = top;
            }
        }
    };

    function externalLinks() {
        $('a').each(function () {
            var a = new RegExp('/' + window.location.host + '/');
            if (!a.test(this.href)) {
                if (this.href.indexOf("javascript:") === -1)
                    $(this).attr("target", "_blank");
            }
        });
    };

    function init() {
        stickyNav();
        externalLinks();
    }

    return {
        init: init
    }
})(jQuery);

$(function () {
    Wageworks.Navigation.init();
});