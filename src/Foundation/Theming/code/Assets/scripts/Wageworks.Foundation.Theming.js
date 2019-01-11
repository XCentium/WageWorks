var WageWorks = WageWorks || {};
WageWorks.Navigation = (function ($) {

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
    WageWorks.Navigation.init();
});
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
(function (window, $) {
    'use strict';

    var d;
    function getYTScript() {
        if (!d) {
            d = $.Deferred();

            $.getScript('https://www.youtube.com/iframe_api');

            window.onYouTubeIframeAPIReady = function () {
                d.resolve(window.YT);
            };
        }

        return d;
    }

    var ytPlayer = function ($player) {
        var elements = {};
        var player;
        var isPlayerLoaded = false;

        function _cacheElements() {
            elements.$video = $player.find('.yt__video');
            elements.$screenshot = $player.find('.yt__screenshot');
            elements.$playButton = $player.find('.yt__play');
        }

        function _createPlayer() {
            player = new window.YT.Player(elements.$video.get(0), {
                videoId: elements.$video.data('yt-id'),
                width: '100%',
                height: '100%',
                playerVars: { modestbranding: 1, rel: 0 },
                events: {
                    onReady: _onPlayerReady,
                    onStateChange: _onPlayerStateChange
                }
            });
            elements.$video = null;
        }

        function _handleClick(e) {
            e.preventDefault();
            elements.$screenshot
                .animate({ opacity: 0 }, 600, function () {
                    $(this).prev().css({ 'z-index': 1 });
                    player.playVideo();
                });

            return false;
        }

        function _onPlayerReady() {
            isPlayerLoaded = true;
            elements.$playButton
                .on('click', _handleClick);
        }

        function _onPlayerStateChange(event) {
            if (event.data == 0) {
                showScreenshot();
            }
        }

        function _killPlayback() {
            $('.video-modal .close').on('click', function () {
                stopPlayer();
            });
        }

        function _stopPlayback() {
            $('.video-modal').on('hidden.bs.modal', function () {
                stopPlayer();
            });
        }

        function init() {
            _cacheElements();
            _createPlayer();
            _killPlayback();
            _stopPlayback();
        }

        function showScreenshot() {
            stopPlayer();
            elements.$screenshot
                .prev().css('z-index', '')
                .end()
                .animate({ opacity: 1 }, 300);
        }

        function stopPlayer() {
            player.pauseVideo();
        }

        function videoLoaded() {
            return isPlayerLoaded;
        }

        return {
            init: init,
            showScreenshot: showScreenshot,
            stopPlayer: stopPlayer,
            videoLoaded: videoLoaded
        }
    }

    if ($('.yt__player').length) {
        getYTScript().done(
            function () {
                $('.yt__player').each(function () {
                    var elem = $(this);
                    if (!elem.data('ytPlayer')) {
                        var p = ytPlayer(elem);
                        elem.data('ytPlayer', p);
                        p.init();
                    }
                });
            });
    }

})(window, $);

var WageWorks = WageWorks || {};
WageWorks.Filters = (function ($) {

    function init() {
        //$('.card').each(function () {
        //    var limit = $(this).attr("data-facet-limit");
        //    if (limit) {
        //        var options = $(this).find('.card-body__toggle');
        //        //var hiddenOptions = $(this).find('.card-body__toggle:hidden');
        //        var showBtn = $(this).find('.js-showall');

        //        if ($(options).length > limit  && limit !== 0) {
        //            $(showBtn).addClass('show');
        //        }
        //        if (limit === 0)
        //            $(options).addClass('show');
        //        else
        //            $(options).slice(0, limit + 1).addClass('show');

        //        $(showBtn).on('click', function (e) {
        //            e.preventDefault();

        //            var parent = $(this).closest('.card'),
        //                options = $(parent).find('.card-body__toggle'),
        //                hiddenOptions = $(parent).find('.card-body__toggle:hidden');

        //            if (!$(hiddenOptions).length === 0) {
        //                $(hiddenOptions).slice(0, limit).addClass('show');
        //            } else {
        //                $(showBtn).removeClass('show'); 
        //            }
        //        });
        //    }
        //});
    }

    return {
        init: init
    }
})(jQuery);

$(function () {
    WageWorks.Filters.init();
});
$.fn.extend(
    {
        onCSSAnimationEnd: function (callback) {
            var $this = $(this).eq(0);
            var prefix = 'webkitAnimationEnd mozAnimationEnd oAnimationEnd oanimationend animationend'
            $this.on(prefix, function () {
                callback.call(this);
                $(this).off(prefix);
            });
            return this;
        },
        onCSSTransitionEnd: function (callback) {
            var $this = $(this).eq(0);
            var prefix = 'webkitTransitionEnd mozTransitionEnd oTransitionEnd otransitionend transitionend'
            $this.on(prefix, function () {
                callback.call(this);
                $(this).off(prefix);
            });
            return this;
        }
    });
(function ($) {
    'use strict';

    var menus = [];
    var EVENT = 'click.social';
    var slideOutMenu = function ($menuItem) {
        var elements = {};

        function _cacheElements() {
            elements.$icon = $menuItem.find('.icon');
            elements.$menu = $menuItem.find('.menu');
            elements.$links = elements.$menu.find('li');
        }

        function _cacheMenus() {
            menus.push(elements.$menu);
        }

        function handleIconClick(e) {
            e.preventDefault();
            e.stopPropagation();
            hideAll();
            if (!elements.$menu.hasClass('menu-visible')) {
                show(elements.$menu);
            } else {
                hide(elements.$menu);
            }
        }

        function _bindEvents() {
            elements.$icon.on(EVENT, handleIconClick);
            elements.$links.on(EVENT, function () { return false; });
        }

        function init() {
            if (!$menuItem.find('.menu').length) return;
            _cacheElements();
            _cacheMenus();
            _bindEvents();
        }

        return {
            init: init
        }
    }

    function hideAll() {
        var i = 0;
        var len = menus.length;

        for (; i < len; i++) {
            var $menu = menus[i];
            if (!$menu.hasClass('menu-visible')) continue;
            hide($menu);
        }
    }

    function hide($menu) {
        $menu
            .addClass('menu-slideIn')
            .onCSSAnimationEnd(function () {
                $(this).removeClass('menu-visible')
                    .removeClass('menu-slideIn')
                    .addClass('menu-hidden')
            });
    }

    function show($menu) {
        $menu
            .addClass('menu-slideOut')
            .onCSSAnimationEnd(function () {
                $(this).removeClass('menu-hidden')
                    .addClass('menu-visible');
                setTimeout(
             function () {
                   $menu.removeClass('menu-slideOut');
                }, 350);
            });
       
    }

    $(document).off(EVENT).on(EVENT, function (e) {
        hideAll();
    });

    $(function () {
        $('.navbar-sticky li, .social-share').each(function () {
            slideOutMenu($(this)).init();
        });
    })
})($);


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
/*! jQuery Validation Plugin - v1.16.0 - 12/2/2016
 * http://jqueryvalidation.org/
 * Copyright (c) 2016 JÃ¶rn Zaefferer; Licensed MIT */
!function (a) { "function" == typeof define && define.amd ? define(["jquery"], a) : "object" == typeof module && module.exports ? module.exports = a(require("jquery")) : a(jQuery) }(function (a) { a.extend(a.fn, { validate: function (b) { if (!this.length) return void (b && b.debug && window.console && console.warn("Nothing selected, can't validate, returning nothing.")); var c = a.data(this[0], "validator"); return c ? c : (this.attr("novalidate", "novalidate"), c = new a.validator(b, this[0]), a.data(this[0], "validator", c), c.settings.onsubmit && (this.on("click.validate", ":submit", function (b) { c.settings.submitHandler && (c.submitButton = b.target), a(this).hasClass("cancel") && (c.cancelSubmit = !0), void 0 !== a(this).attr("formnovalidate") && (c.cancelSubmit = !0) }), this.on("submit.validate", function (b) { function d() { var d, e; return !c.settings.submitHandler || (c.submitButton && (d = a("<input type='hidden'/>").attr("name", c.submitButton.name).val(a(c.submitButton).val()).appendTo(c.currentForm)), e = c.settings.submitHandler.call(c, c.currentForm, b), c.submitButton && d.remove(), void 0 !== e && e) } return c.settings.debug && b.preventDefault(), c.cancelSubmit ? (c.cancelSubmit = !1, d()) : c.form() ? c.pendingRequest ? (c.formSubmitted = !0, !1) : d() : (c.focusInvalid(), !1) })), c) }, valid: function () { var b, c, d; return a(this[0]).is("form") ? b = this.validate().form() : (d = [], b = !0, c = a(this[0].form).validate(), this.each(function () { b = c.element(this) && b, b || (d = d.concat(c.errorList)) }), c.errorList = d), b }, rules: function (b, c) { var d, e, f, g, h, i, j = this[0]; if (null != j && null != j.form) { if (b) switch (d = a.data(j.form, "validator").settings, e = d.rules, f = a.validator.staticRules(j), b) { case "add": a.extend(f, a.validator.normalizeRule(c)), delete f.messages, e[j.name] = f, c.messages && (d.messages[j.name] = a.extend(d.messages[j.name], c.messages)); break; case "remove": return c ? (i = {}, a.each(c.split(/\s/), function (b, c) { i[c] = f[c], delete f[c], "required" === c && a(j).removeAttr("aria-required") }), i) : (delete e[j.name], f) }return g = a.validator.normalizeRules(a.extend({}, a.validator.classRules(j), a.validator.attributeRules(j), a.validator.dataRules(j), a.validator.staticRules(j)), j), g.required && (h = g.required, delete g.required, g = a.extend({ required: h }, g), a(j).attr("aria-required", "true")), g.remote && (h = g.remote, delete g.remote, g = a.extend(g, { remote: h })), g } } }), a.extend(a.expr.pseudos || a.expr[":"], { blank: function (b) { return !a.trim("" + a(b).val()) }, filled: function (b) { var c = a(b).val(); return null !== c && !!a.trim("" + c) }, unchecked: function (b) { return !a(b).prop("checked") } }), a.validator = function (b, c) { this.settings = a.extend(!0, {}, a.validator.defaults, b), this.currentForm = c, this.init() }, a.validator.format = function (b, c) { return 1 === arguments.length ? function () { var c = a.makeArray(arguments); return c.unshift(b), a.validator.format.apply(this, c) } : void 0 === c ? b : (arguments.length > 2 && c.constructor !== Array && (c = a.makeArray(arguments).slice(1)), c.constructor !== Array && (c = [c]), a.each(c, function (a, c) { b = b.replace(new RegExp("\\{" + a + "\\}", "g"), function () { return c }) }), b) }, a.extend(a.validator, { defaults: { messages: {}, groups: {}, rules: {}, errorClass: "error", pendingClass: "pending", validClass: "valid", errorElement: "label", focusCleanup: !1, focusInvalid: !0, errorContainer: a([]), errorLabelContainer: a([]), onsubmit: !0, ignore: ":hidden", ignoreTitle: !1, onfocusin: function (a) { this.lastActive = a, this.settings.focusCleanup && (this.settings.unhighlight && this.settings.unhighlight.call(this, a, this.settings.errorClass, this.settings.validClass), this.hideThese(this.errorsFor(a))) }, onfocusout: function (a) { this.checkable(a) || !(a.name in this.submitted) && this.optional(a) || this.element(a) }, onkeyup: function (b, c) { var d = [16, 17, 18, 20, 35, 36, 37, 38, 39, 40, 45, 144, 225]; 9 === c.which && "" === this.elementValue(b) || a.inArray(c.keyCode, d) !== -1 || (b.name in this.submitted || b.name in this.invalid) && this.element(b) }, onclick: function (a) { a.name in this.submitted ? this.element(a) : a.parentNode.name in this.submitted && this.element(a.parentNode) }, highlight: function (b, c, d) { "radio" === b.type ? this.findByName(b.name).addClass(c).removeClass(d) : a(b).addClass(c).removeClass(d) }, unhighlight: function (b, c, d) { "radio" === b.type ? this.findByName(b.name).removeClass(c).addClass(d) : a(b).removeClass(c).addClass(d) } }, setDefaults: function (b) { a.extend(a.validator.defaults, b) }, messages: { required: "This field is required.", remote: "Please fix this field.", email: "Please enter a valid email address.", url: "Please enter a valid URL.", date: "Please enter a valid date.", dateISO: "Please enter a valid date (ISO).", number: "Please enter a valid number.", digits: "Please enter only digits.", equalTo: "Please enter the same value again.", maxlength: a.validator.format("Please enter no more than {0} characters."), minlength: a.validator.format("Please enter at least {0} characters."), rangelength: a.validator.format("Please enter a value between {0} and {1} characters long."), range: a.validator.format("Please enter a value between {0} and {1}."), max: a.validator.format("Please enter a value less than or equal to {0}."), min: a.validator.format("Please enter a value greater than or equal to {0}."), step: a.validator.format("Please enter a multiple of {0}.") }, autoCreateRanges: !1, prototype: { init: function () { function b(b) { !this.form && this.hasAttribute("contenteditable") && (this.form = a(this).closest("form")[0]); var c = a.data(this.form, "validator"), d = "on" + b.type.replace(/^validate/, ""), e = c.settings; e[d] && !a(this).is(e.ignore) && e[d].call(c, this, b) } this.labelContainer = a(this.settings.errorLabelContainer), this.errorContext = this.labelContainer.length && this.labelContainer || a(this.currentForm), this.containers = a(this.settings.errorContainer).add(this.settings.errorLabelContainer), this.submitted = {}, this.valueCache = {}, this.pendingRequest = 0, this.pending = {}, this.invalid = {}, this.reset(); var c, d = this.groups = {}; a.each(this.settings.groups, function (b, c) { "string" == typeof c && (c = c.split(/\s/)), a.each(c, function (a, c) { d[c] = b }) }), c = this.settings.rules, a.each(c, function (b, d) { c[b] = a.validator.normalizeRule(d) }), a(this.currentForm).on("focusin.validate focusout.validate keyup.validate", ":text, [type='password'], [type='file'], select, textarea, [type='number'], [type='search'], [type='tel'], [type='url'], [type='email'], [type='datetime'], [type='date'], [type='month'], [type='week'], [type='time'], [type='datetime-local'], [type='range'], [type='color'], [type='radio'], [type='checkbox'], [contenteditable], [type='button']", b).on("click.validate", "select, option, [type='radio'], [type='checkbox']", b), this.settings.invalidHandler && a(this.currentForm).on("invalid-form.validate", this.settings.invalidHandler), a(this.currentForm).find("[required], [data-rule-required], .required").attr("aria-required", "true") }, form: function () { return this.checkForm(), a.extend(this.submitted, this.errorMap), this.invalid = a.extend({}, this.errorMap), this.valid() || a(this.currentForm).triggerHandler("invalid-form", [this]), this.showErrors(), this.valid() }, checkForm: function () { this.prepareForm(); for (var a = 0, b = this.currentElements = this.elements(); b[a]; a++)this.check(b[a]); return this.valid() }, element: function (b) { var c, d, e = this.clean(b), f = this.validationTargetFor(e), g = this, h = !0; return void 0 === f ? delete this.invalid[e.name] : (this.prepareElement(f), this.currentElements = a(f), d = this.groups[f.name], d && a.each(this.groups, function (a, b) { b === d && a !== f.name && (e = g.validationTargetFor(g.clean(g.findByName(a))), e && e.name in g.invalid && (g.currentElements.push(e), h = g.check(e) && h)) }), c = this.check(f) !== !1, h = h && c, c ? this.invalid[f.name] = !1 : this.invalid[f.name] = !0, this.numberOfInvalids() || (this.toHide = this.toHide.add(this.containers)), this.showErrors(), a(b).attr("aria-invalid", !c)), h }, showErrors: function (b) { if (b) { var c = this; a.extend(this.errorMap, b), this.errorList = a.map(this.errorMap, function (a, b) { return { message: a, element: c.findByName(b)[0] } }), this.successList = a.grep(this.successList, function (a) { return !(a.name in b) }) } this.settings.showErrors ? this.settings.showErrors.call(this, this.errorMap, this.errorList) : this.defaultShowErrors() }, resetForm: function () { a.fn.resetForm && a(this.currentForm).resetForm(), this.invalid = {}, this.submitted = {}, this.prepareForm(), this.hideErrors(); var b = this.elements().removeData("previousValue").removeAttr("aria-invalid"); this.resetElements(b) }, resetElements: function (a) { var b; if (this.settings.unhighlight) for (b = 0; a[b]; b++)this.settings.unhighlight.call(this, a[b], this.settings.errorClass, ""), this.findByName(a[b].name).removeClass(this.settings.validClass); else a.removeClass(this.settings.errorClass).removeClass(this.settings.validClass) }, numberOfInvalids: function () { return this.objectLength(this.invalid) }, objectLength: function (a) { var b, c = 0; for (b in a) a[b] && c++; return c }, hideErrors: function () { this.hideThese(this.toHide) }, hideThese: function (a) { a.not(this.containers).text(""), this.addWrapper(a).hide() }, valid: function () { return 0 === this.size() }, size: function () { return this.errorList.length }, focusInvalid: function () { if (this.settings.focusInvalid) try { a(this.findLastActive() || this.errorList.length && this.errorList[0].element || []).filter(":visible").focus().trigger("focusin") } catch (b) { } }, findLastActive: function () { var b = this.lastActive; return b && 1 === a.grep(this.errorList, function (a) { return a.element.name === b.name }).length && b }, elements: function () { var b = this, c = {}; return a(this.currentForm).find("input, select, textarea, [contenteditable]").not(":submit, :reset, :image, :disabled").not(this.settings.ignore).filter(function () { var d = this.name || a(this).attr("name"); return !d && b.settings.debug && window.console && console.error("%o has no name assigned", this), this.hasAttribute("contenteditable") && (this.form = a(this).closest("form")[0]), !(d in c || !b.objectLength(a(this).rules())) && (c[d] = !0, !0) }) }, clean: function (b) { return a(b)[0] }, errors: function () { var b = this.settings.errorClass.split(" ").join("."); return a(this.settings.errorElement + "." + b, this.errorContext) }, resetInternals: function () { this.successList = [], this.errorList = [], this.errorMap = {}, this.toShow = a([]), this.toHide = a([]) }, reset: function () { this.resetInternals(), this.currentElements = a([]) }, prepareForm: function () { this.reset(), this.toHide = this.errors().add(this.containers) }, prepareElement: function (a) { this.reset(), this.toHide = this.errorsFor(a) }, elementValue: function (b) { var c, d, e = a(b), f = b.type; return "radio" === f || "checkbox" === f ? this.findByName(b.name).filter(":checked").val() : "number" === f && "undefined" != typeof b.validity ? b.validity.badInput ? "NaN" : e.val() : (c = b.hasAttribute("contenteditable") ? e.text() : e.val(), "file" === f ? "C:\\fakepath\\" === c.substr(0, 12) ? c.substr(12) : (d = c.lastIndexOf("/"), d >= 0 ? c.substr(d + 1) : (d = c.lastIndexOf("\\"), d >= 0 ? c.substr(d + 1) : c)) : "string" == typeof c ? c.replace(/\r/g, "") : c) }, check: function (b) { b = this.validationTargetFor(this.clean(b)); var c, d, e, f = a(b).rules(), g = a.map(f, function (a, b) { return b }).length, h = !1, i = this.elementValue(b); if ("function" == typeof f.normalizer) { if (i = f.normalizer.call(b, i), "string" != typeof i) throw new TypeError("The normalizer should return a string value."); delete f.normalizer } for (d in f) { e = { method: d, parameters: f[d] }; try { if (c = a.validator.methods[d].call(this, i, b, e.parameters), "dependency-mismatch" === c && 1 === g) { h = !0; continue } if (h = !1, "pending" === c) return void (this.toHide = this.toHide.not(this.errorsFor(b))); if (!c) return this.formatAndAdd(b, e), !1 } catch (j) { throw this.settings.debug && window.console && console.log("Exception occurred when checking element " + b.id + ", check the '" + e.method + "' method.", j), j instanceof TypeError && (j.message += ".  Exception occurred when checking element " + b.id + ", check the '" + e.method + "' method."), j } } if (!h) return this.objectLength(f) && this.successList.push(b), !0 }, customDataMessage: function (b, c) { return a(b).data("msg" + c.charAt(0).toUpperCase() + c.substring(1).toLowerCase()) || a(b).data("msg") }, customMessage: function (a, b) { var c = this.settings.messages[a]; return c && (c.constructor === String ? c : c[b]) }, findDefined: function () { for (var a = 0; a < arguments.length; a++)if (void 0 !== arguments[a]) return arguments[a] }, defaultMessage: function (b, c) { "string" == typeof c && (c = { method: c }); var d = this.findDefined(this.customMessage(b.name, c.method), this.customDataMessage(b, c.method), !this.settings.ignoreTitle && b.title || void 0, a.validator.messages[c.method], "<strong>Warning: No message defined for " + b.name + "</strong>"), e = /\$?\{(\d+)\}/g; return "function" == typeof d ? d = d.call(this, c.parameters, b) : e.test(d) && (d = a.validator.format(d.replace(e, "{$1}"), c.parameters)), d }, formatAndAdd: function (a, b) { var c = this.defaultMessage(a, b); this.errorList.push({ message: c, element: a, method: b.method }), this.errorMap[a.name] = c, this.submitted[a.name] = c }, addWrapper: function (a) { return this.settings.wrapper && (a = a.add(a.parent(this.settings.wrapper))), a }, defaultShowErrors: function () { var a, b, c; for (a = 0; this.errorList[a]; a++)c = this.errorList[a], this.settings.highlight && this.settings.highlight.call(this, c.element, this.settings.errorClass, this.settings.validClass), this.showLabel(c.element, c.message); if (this.errorList.length && (this.toShow = this.toShow.add(this.containers)), this.settings.success) for (a = 0; this.successList[a]; a++)this.showLabel(this.successList[a]); if (this.settings.unhighlight) for (a = 0, b = this.validElements(); b[a]; a++)this.settings.unhighlight.call(this, b[a], this.settings.errorClass, this.settings.validClass); this.toHide = this.toHide.not(this.toShow), this.hideErrors(), this.addWrapper(this.toShow).show() }, validElements: function () { return this.currentElements.not(this.invalidElements()) }, invalidElements: function () { return a(this.errorList).map(function () { return this.element }) }, showLabel: function (b, c) { var d, e, f, g, h = this.errorsFor(b), i = this.idOrName(b), j = a(b).attr("aria-describedby"); h.length ? (h.removeClass(this.settings.validClass).addClass(this.settings.errorClass), h.html(c)) : (h = a("<" + this.settings.errorElement + ">").attr("id", i + "-error").addClass(this.settings.errorClass).html(c || ""), d = h, this.settings.wrapper && (d = h.hide().show().wrap("<" + this.settings.wrapper + "/>").parent()), this.labelContainer.length ? this.labelContainer.append(d) : this.settings.errorPlacement ? this.settings.errorPlacement.call(this, d, a(b)) : d.insertAfter(b), h.is("label") ? h.attr("for", i) : 0 === h.parents("label[for='" + this.escapeCssMeta(i) + "']").length && (f = h.attr("id"), j ? j.match(new RegExp("\\b" + this.escapeCssMeta(f) + "\\b")) || (j += " " + f) : j = f, a(b).attr("aria-describedby", j), e = this.groups[b.name], e && (g = this, a.each(g.groups, function (b, c) { c === e && a("[name='" + g.escapeCssMeta(b) + "']", g.currentForm).attr("aria-describedby", h.attr("id")) })))), !c && this.settings.success && (h.text(""), "string" == typeof this.settings.success ? h.addClass(this.settings.success) : this.settings.success(h, b)), this.toShow = this.toShow.add(h) }, errorsFor: function (b) { var c = this.escapeCssMeta(this.idOrName(b)), d = a(b).attr("aria-describedby"), e = "label[for='" + c + "'], label[for='" + c + "'] *"; return d && (e = e + ", #" + this.escapeCssMeta(d).replace(/\s+/g, ", #")), this.errors().filter(e) }, escapeCssMeta: function (a) { return a.replace(/([\\!"#$%&'()*+,./:;<=>?@\[\]^`{|}~])/g, "\\$1") }, idOrName: function (a) { return this.groups[a.name] || (this.checkable(a) ? a.name : a.id || a.name) }, validationTargetFor: function (b) { return this.checkable(b) && (b = this.findByName(b.name)), a(b).not(this.settings.ignore)[0] }, checkable: function (a) { return /radio|checkbox/i.test(a.type) }, findByName: function (b) { return a(this.currentForm).find("[name='" + this.escapeCssMeta(b) + "']") }, getLength: function (b, c) { switch (c.nodeName.toLowerCase()) { case "select": return a("option:selected", c).length; case "input": if (this.checkable(c)) return this.findByName(c.name).filter(":checked").length }return b.length }, depend: function (a, b) { return !this.dependTypes[typeof a] || this.dependTypes[typeof a](a, b) }, dependTypes: { "boolean": function (a) { return a }, string: function (b, c) { return !!a(b, c.form).length }, "function": function (a, b) { return a(b) } }, optional: function (b) { var c = this.elementValue(b); return !a.validator.methods.required.call(this, c, b) && "dependency-mismatch" }, startRequest: function (b) { this.pending[b.name] || (this.pendingRequest++ , a(b).addClass(this.settings.pendingClass), this.pending[b.name] = !0) }, stopRequest: function (b, c) { this.pendingRequest-- , this.pendingRequest < 0 && (this.pendingRequest = 0), delete this.pending[b.name], a(b).removeClass(this.settings.pendingClass), c && 0 === this.pendingRequest && this.formSubmitted && this.form() ? (a(this.currentForm).submit(), this.formSubmitted = !1) : !c && 0 === this.pendingRequest && this.formSubmitted && (a(this.currentForm).triggerHandler("invalid-form", [this]), this.formSubmitted = !1) }, previousValue: function (b, c) { return c = "string" == typeof c && c || "remote", a.data(b, "previousValue") || a.data(b, "previousValue", { old: null, valid: !0, message: this.defaultMessage(b, { method: c }) }) }, destroy: function () { this.resetForm(), a(this.currentForm).off(".validate").removeData("validator").find(".validate-equalTo-blur").off(".validate-equalTo").removeClass("validate-equalTo-blur") } }, classRuleSettings: { required: { required: !0 }, email: { email: !0 }, url: { url: !0 }, date: { date: !0 }, dateISO: { dateISO: !0 }, number: { number: !0 }, digits: { digits: !0 }, creditcard: { creditcard: !0 } }, addClassRules: function (b, c) { b.constructor === String ? this.classRuleSettings[b] = c : a.extend(this.classRuleSettings, b) }, classRules: function (b) { var c = {}, d = a(b).attr("class"); return d && a.each(d.split(" "), function () { this in a.validator.classRuleSettings && a.extend(c, a.validator.classRuleSettings[this]) }), c }, normalizeAttributeRule: function (a, b, c, d) { /min|max|step/.test(c) && (null === b || /number|range|text/.test(b)) && (d = Number(d), isNaN(d) && (d = void 0)), d || 0 === d ? a[c] = d : b === c && "range" !== b && (a[c] = !0) }, attributeRules: function (b) { var c, d, e = {}, f = a(b), g = b.getAttribute("type"); for (c in a.validator.methods) "required" === c ? (d = b.getAttribute(c), "" === d && (d = !0), d = !!d) : d = f.attr(c), this.normalizeAttributeRule(e, g, c, d); return e.maxlength && /-1|2147483647|524288/.test(e.maxlength) && delete e.maxlength, e }, dataRules: function (b) { var c, d, e = {}, f = a(b), g = b.getAttribute("type"); for (c in a.validator.methods) d = f.data("rule" + c.charAt(0).toUpperCase() + c.substring(1).toLowerCase()), this.normalizeAttributeRule(e, g, c, d); return e }, staticRules: function (b) { var c = {}, d = a.data(b.form, "validator"); return d.settings.rules && (c = a.validator.normalizeRule(d.settings.rules[b.name]) || {}), c }, normalizeRules: function (b, c) { return a.each(b, function (d, e) { if (e === !1) return void delete b[d]; if (e.param || e.depends) { var f = !0; switch (typeof e.depends) { case "string": f = !!a(e.depends, c.form).length; break; case "function": f = e.depends.call(c, c) }f ? b[d] = void 0 === e.param || e.param : (a.data(c.form, "validator").resetElements(a(c)), delete b[d]) } }), a.each(b, function (d, e) { b[d] = a.isFunction(e) && "normalizer" !== d ? e(c) : e }), a.each(["minlength", "maxlength"], function () { b[this] && (b[this] = Number(b[this])) }), a.each(["rangelength", "range"], function () { var c; b[this] && (a.isArray(b[this]) ? b[this] = [Number(b[this][0]), Number(b[this][1])] : "string" == typeof b[this] && (c = b[this].replace(/[\[\]]/g, "").split(/[\s,]+/), b[this] = [Number(c[0]), Number(c[1])])) }), a.validator.autoCreateRanges && (null != b.min && null != b.max && (b.range = [b.min, b.max], delete b.min, delete b.max), null != b.minlength && null != b.maxlength && (b.rangelength = [b.minlength, b.maxlength], delete b.minlength, delete b.maxlength)), b }, normalizeRule: function (b) { if ("string" == typeof b) { var c = {}; a.each(b.split(/\s/), function () { c[this] = !0 }), b = c } return b }, addMethod: function (b, c, d) { a.validator.methods[b] = c, a.validator.messages[b] = void 0 !== d ? d : a.validator.messages[b], c.length < 3 && a.validator.addClassRules(b, a.validator.normalizeRule(b)) }, methods: { required: function (b, c, d) { if (!this.depend(d, c)) return "dependency-mismatch"; if ("select" === c.nodeName.toLowerCase()) { var e = a(c).val(); return e && e.length > 0 } return this.checkable(c) ? this.getLength(b, c) > 0 : b.length > 0 }, email: function (a, b) { return this.optional(b) || /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(a) }, url: function (a, b) { return this.optional(b) || /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})).?)(?::\d{2,5})?(?:[/?#]\S*)?$/i.test(a) }, date: function (a, b) { return this.optional(b) || !/Invalid|NaN/.test(new Date(a).toString()) }, dateISO: function (a, b) { return this.optional(b) || /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$/.test(a) }, number: function (a, b) { return this.optional(b) || /^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(a) }, digits: function (a, b) { return this.optional(b) || /^\d+$/.test(a) }, minlength: function (b, c, d) { var e = a.isArray(b) ? b.length : this.getLength(b, c); return this.optional(c) || e >= d }, maxlength: function (b, c, d) { var e = a.isArray(b) ? b.length : this.getLength(b, c); return this.optional(c) || e <= d }, rangelength: function (b, c, d) { var e = a.isArray(b) ? b.length : this.getLength(b, c); return this.optional(c) || e >= d[0] && e <= d[1] }, min: function (a, b, c) { return this.optional(b) || a >= c }, max: function (a, b, c) { return this.optional(b) || a <= c }, range: function (a, b, c) { return this.optional(b) || a >= c[0] && a <= c[1] }, step: function (b, c, d) { var e, f = a(c).attr("type"), g = "Step attribute on input type " + f + " is not supported.", h = ["text", "number", "range"], i = new RegExp("\\b" + f + "\\b"), j = f && !i.test(h.join()), k = function (a) { var b = ("" + a).match(/(?:\.(\d+))?$/); return b && b[1] ? b[1].length : 0 }, l = function (a) { return Math.round(a * Math.pow(10, e)) }, m = !0; if (j) throw new Error(g); return e = k(d), (k(b) > e || l(b) % l(d) !== 0) && (m = !1), this.optional(c) || m }, equalTo: function (b, c, d) { var e = a(d); return this.settings.onfocusout && e.not(".validate-equalTo-blur").length && e.addClass("validate-equalTo-blur").on("blur.validate-equalTo", function () { a(c).valid() }), b === e.val() }, remote: function (b, c, d, e) { if (this.optional(c)) return "dependency-mismatch"; e = "string" == typeof e && e || "remote"; var f, g, h, i = this.previousValue(c, e); return this.settings.messages[c.name] || (this.settings.messages[c.name] = {}), i.originalMessage = i.originalMessage || this.settings.messages[c.name][e], this.settings.messages[c.name][e] = i.message, d = "string" == typeof d && { url: d } || d, h = a.param(a.extend({ data: b }, d.data)), i.old === h ? i.valid : (i.old = h, f = this, this.startRequest(c), g = {}, g[c.name] = b, a.ajax(a.extend(!0, { mode: "abort", port: "validate" + c.name, dataType: "json", data: g, context: f.currentForm, success: function (a) { var d, g, h, j = a === !0 || "true" === a; f.settings.messages[c.name][e] = i.originalMessage, j ? (h = f.formSubmitted, f.resetInternals(), f.toHide = f.errorsFor(c), f.formSubmitted = h, f.successList.push(c), f.invalid[c.name] = !1, f.showErrors()) : (d = {}, g = a || f.defaultMessage(c, { method: e, parameters: b }), d[c.name] = i.message = g, f.invalid[c.name] = !0, f.showErrors(d)), i.valid = j, f.stopRequest(c, j) } }, d)), "pending") } } }); var b, c = {}; return a.ajaxPrefilter ? a.ajaxPrefilter(function (a, b, d) { var e = a.port; "abort" === a.mode && (c[e] && c[e].abort(), c[e] = d) }) : (b = a.ajax, a.ajax = function (d) { var e = ("mode" in d ? d : a.ajaxSettings).mode, f = ("port" in d ? d : a.ajaxSettings).port; return "abort" === e ? (c[f] && c[f].abort(), c[f] = b.apply(this, arguments), c[f]) : b.apply(this, arguments) }), a });
/*
** Unobtrusive validation support library for jQuery and jQuery Validate
** Copyright (C) Microsoft Corporation. All rights reserved.
*/
!function (a) { function e(a, e, n) { a.rules[e] = n, a.message && (a.messages[e] = a.message) } function n(a) { return a.replace(/^\s+|\s+$/g, "").split(/\s*,\s*/g) } function t(a) { return a.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1") } function r(a) { return a.substr(0, a.lastIndexOf(".") + 1) } function i(a, e) { return 0 === a.indexOf("*.") && (a = a.replace("*.", e)), a } function o(e, n) { var r = a(this).find("[data-valmsg-for='" + t(n[0].name) + "']"), i = r.attr("data-valmsg-replace"), o = i ? a.parseJSON(i) !== !1 : null; r.removeClass("field-validation-valid").addClass("field-validation-error"), e.data("unobtrusiveContainer", r), o ? (r.empty(), e.removeClass("input-validation-error").appendTo(r)) : e.hide() } function d(e, n) { var t = a(this).find("[data-valmsg-summary=true]"), r = t.find("ul"); r && r.length && n.errorList.length && (r.empty(), t.addClass("validation-summary-errors").removeClass("validation-summary-valid"), a.each(n.errorList, function () { a("<li />").html(this.message).appendTo(r) })) } function s(e) { var n = e.data("unobtrusiveContainer"); if (n) { var t = n.attr("data-valmsg-replace"), r = t ? a.parseJSON(t) : null; n.addClass("field-validation-valid").removeClass("field-validation-error"), e.removeData("unobtrusiveContainer"), r && n.empty() } } function l(e) { var n = a(this), t = "__jquery_unobtrusive_validation_form_reset"; if (!n.data(t)) { n.data(t, !0); try { n.data("validator").resetForm() } finally { n.removeData(t) } n.find(".validation-summary-errors").addClass("validation-summary-valid").removeClass("validation-summary-errors"), n.find(".field-validation-error").addClass("field-validation-valid").removeClass("field-validation-error").removeData("unobtrusiveContainer").find(">*").removeData("unobtrusiveContainer") } } function m(e) { var n = a(e), t = n.data(v), r = a.proxy(l, e), i = p.unobtrusive.options || {}, m = function (n, t) { var r = i[n]; r && a.isFunction(r) && r.apply(e, t) }; return t || (t = { options: { errorClass: i.errorClass || "input-validation-error", errorElement: i.errorElement || "span", errorPlacement: function () { o.apply(e, arguments), m("errorPlacement", arguments) }, invalidHandler: function () { d.apply(e, arguments), m("invalidHandler", arguments) }, messages: {}, rules: {}, success: function () { s.apply(e, arguments), m("success", arguments) } }, attachValidation: function () { n.off("reset." + v, r).on("reset." + v, r).validate(this.options) }, validate: function () { return n.validate(), n.valid() } }, n.data(v, t)), t } var u, p = a.validator, v = "unobtrusiveValidation"; p.unobtrusive = { adapters: [], parseElement: function (e, n) { var t, r, i, o = a(e), d = o.parents("form")[0]; d && (t = m(d), t.options.rules[e.name] = r = {}, t.options.messages[e.name] = i = {}, a.each(this.adapters, function () { var n = "data-val-" + this.name, t = o.attr(n), s = {}; void 0 !== t && (n += "-", a.each(this.params, function () { s[this] = o.attr(n + this) }), this.adapt({ element: e, form: d, message: t, params: s, rules: r, messages: i })) }), a.extend(r, { __dummy__: !0 }), n || t.attachValidation()) }, parse: function (e) { var n = a(e), t = n.parents().addBack().filter("form").add(n.find("form")).has("[data-val=true]"); n.find("[data-val=true]").each(function () { p.unobtrusive.parseElement(this, !0) }), t.each(function () { var a = m(this); a && a.attachValidation() }) } }, u = p.unobtrusive.adapters, u.add = function (a, e, n) { return n || (n = e, e = []), this.push({ name: a, params: e, adapt: n }), this }, u.addBool = function (a, n) { return this.add(a, function (t) { e(t, n || a, !0) }) }, u.addMinMax = function (a, n, t, r, i, o) { return this.add(a, [i || "min", o || "max"], function (a) { var i = a.params.min, o = a.params.max; i && o ? e(a, r, [i, o]) : i ? e(a, n, i) : o && e(a, t, o) }) }, u.addSingleVal = function (a, n, t) { return this.add(a, [n || "val"], function (r) { e(r, t || a, r.params[n]) }) }, p.addMethod("__dummy__", function (a, e, n) { return !0 }), p.addMethod("regex", function (a, e, n) { var t; return this.optional(e) ? !0 : (t = new RegExp(n).exec(a), t && 0 === t.index && t[0].length === a.length) }), p.addMethod("nonalphamin", function (a, e, n) { var t; return n && (t = a.match(/\W/g), t = t && t.length >= n), t }), p.methods.extension ? (u.addSingleVal("accept", "mimtype"), u.addSingleVal("extension", "extension")) : u.addSingleVal("extension", "extension", "accept"), u.addSingleVal("regex", "pattern"), u.addBool("creditcard").addBool("date").addBool("digits").addBool("email").addBool("number").addBool("url"), u.addMinMax("length", "minlength", "maxlength", "rangelength").addMinMax("range", "min", "max", "range"), u.addMinMax("minlength", "minlength").addMinMax("maxlength", "minlength", "maxlength"), u.add("equalto", ["other"], function (n) { var o = r(n.element.name), d = n.params.other, s = i(d, o), l = a(n.form).find(":input").filter("[name='" + t(s) + "']")[0]; e(n, "equalTo", l) }), u.add("required", function (a) { ("INPUT" !== a.element.tagName.toUpperCase() || "CHECKBOX" !== a.element.type.toUpperCase()) && e(a, "required", !0) }), u.add("remote", ["url", "type", "additionalfields"], function (o) { var d = { url: o.params.url, type: o.params.type || "GET", data: {} }, s = r(o.element.name); a.each(n(o.params.additionalfields || o.element.name), function (e, n) { var r = i(n, s); d.data[r] = function () { var e = a(o.form).find(":input").filter("[name='" + t(r) + "']"); return e.is(":checkbox") ? e.filter(":checked").val() || e.filter(":hidden").val() || "" : e.is(":radio") ? e.filter(":checked").val() || "" : e.val() } }), e(o, "remote", d) }), u.add("password", ["min", "nonalphamin", "regex"], function (a) { a.params.min && e(a, "minlength", a.params.min), a.params.nonalphamin && e(a, "nonalphamin", a.params.nonalphamin), a.params.regex && e(a, "regex", a.params.regex) }), a(function () { p.unobtrusive.parse(document) }) }(jQuery);
/* NUGET: BEGIN LICENSE TEXT
 *
 * Microsoft grants you the right to use these script files for the sole
 * purpose of either: (i) interacting through your browser with the Microsoft
 * website or online service, subject to the applicable licensing or use
 * terms; or (ii) using the files as included with a Microsoft product subject
 * to that product's license terms. Microsoft reserves all other rights to the
 * files not expressly granted by Microsoft, whether by implication, estoppel
 * or otherwise. Insofar as a script file is dual licensed under GPL,
 * Microsoft neither took the code under GPL nor distributes it thereunder but
 * under the terms set out in this paragraph. All notices and licenses
 * below are for informational purposes only.
 *
 * NUGET: END LICENSE TEXT */
/*
** Unobtrusive Ajax support library for jQuery
** Copyright (C) Microsoft Corporation. All rights reserved.
*/
(function (a) { var b = "unobtrusiveAjaxClick", d = "unobtrusiveAjaxClickTarget", h = "unobtrusiveValidation"; function c(d, b) { var a = window, c = (d || "").split("."); while (a && c.length) a = a[c.shift()]; if (typeof a === "function") return a; b.push(d); return Function.constructor.apply(null, b) } function e(a) { return a === "GET" || a === "POST" } function g(b, a) { !e(a) && b.setRequestHeader("X-HTTP-Method-Override", a) } function i(c, b, e) { var d; if (e.indexOf("application/x-javascript") !== -1) return; d = (c.getAttribute("data-ajax-mode") || "").toUpperCase(); a(c.getAttribute("data-ajax-update")).each(function (f, c) { var e; switch (d) { case "BEFORE": e = c.firstChild; a("<div />").html(b).contents().each(function () { c.insertBefore(this, e) }); break; case "AFTER": a("<div />").html(b).contents().each(function () { c.appendChild(this) }); break; case "REPLACE-WITH": a(c).replaceWith(b); break; default: a(c).html(b) } }) } function f(b, d) { var j, k, f, h; j = b.getAttribute("data-ajax-confirm"); if (j && !window.confirm(j)) return; k = a(b.getAttribute("data-ajax-loading")); h = parseInt(b.getAttribute("data-ajax-loading-duration"), 10) || 0; a.extend(d, { type: b.getAttribute("data-ajax-method") || undefined, url: b.getAttribute("data-ajax-url") || undefined, cache: (b.getAttribute("data-ajax-cache") || "").toLowerCase() === "true", beforeSend: function (d) { var a; g(d, f); a = c(b.getAttribute("data-ajax-begin"), ["xhr"]).apply(b, arguments); a !== false && k.show(h); return a }, complete: function () { k.hide(h); c(b.getAttribute("data-ajax-complete"), ["xhr", "status"]).apply(b, arguments) }, success: function (a, e, d) { i(b, a, d.getResponseHeader("Content-Type") || "text/html"); c(b.getAttribute("data-ajax-success"), ["data", "status", "xhr"]).apply(b, arguments) }, error: function () { c(b.getAttribute("data-ajax-failure"), ["xhr", "status", "error"]).apply(b, arguments) } }); d.data.push({ name: "X-Requested-With", value: "XMLHttpRequest" }); f = d.type.toUpperCase(); if (!e(f)) { d.type = "POST"; d.data.push({ name: "X-HTTP-Method-Override", value: f }) } a.ajax(d) } function j(c) { var b = a(c).data(h); return !b || !b.validate || b.validate() } a(document).on("click", "a[data-ajax=true]", function (a) { a.preventDefault(); f(this, { url: this.href, type: "GET", data: [] }) }); a(document).on("click", "form[data-ajax=true] input[type=image]", function (c) { var g = c.target.name, e = a(c.target), f = a(e.parents("form")[0]), d = e.offset(); f.data(b, [{ name: g + ".x", value: Math.round(c.pageX - d.left) }, { name: g + ".y", value: Math.round(c.pageY - d.top) }]); setTimeout(function () { f.removeData(b) }, 0) }); a(document).on("click", "form[data-ajax=true] :submit", function (e) { var g = e.currentTarget.name, f = a(e.target), c = a(f.parents("form")[0]); c.data(b, g ? [{ name: g, value: e.currentTarget.value }] : []); c.data(d, f); setTimeout(function () { c.removeData(b); c.removeData(d) }, 0) }); a(document).on("submit", "form[data-ajax=true]", function (h) { var e = a(this).data(b) || [], c = a(this).data(d), g = c && c.hasClass("cancel"); h.preventDefault(); if (!g && !j(this)) return; f(this, { url: this.action, type: this.method || "GET", data: e.concat(a(this).serializeArray()) }) }) })(jQuery);
(function ($jq) {
    var adapters = $jq.validator.unobtrusive.adapters;
    adapters.fxbAddNumberVal = function (adapterName, attribute, ruleName) {
        attribute = attribute || "val";
        ruleName = ruleName || adapterName;
        this.add(adapterName, [attribute], function (options) {
            var attrVal = options.params[attribute];
            if ((attrVal || attrVal === 0) && !isNaN(attrVal)) {
                options.rules[ruleName] = Number(attrVal);
            }
            if (options.message) {
                options.messages[ruleName] = options.message;
            }
        });
    };

    adapters.fxbAddMinMax = function (adapterName, minRuleName, maxRuleName, minAttribute, maxAttribute) {
        minAttribute = minAttribute || "min";
        maxAttribute = maxAttribute || "max";
        this.add(adapterName, [minAttribute, maxAttribute], function (options) {
            if (options.params[minAttribute] && options.params[maxAttribute]) {
                if (!options.rules.hasOwnProperty(minRuleName)) {
                    if (options.message) {
                        options.messages[minRuleName] = options.message;
                    }
                }
                if (!options.rules.hasOwnProperty(maxRuleName)) {
                    if (options.message) {
                        options.messages[maxRuleName] = options.message;
                    }
                }
            }
        });
    };

    adapters.addBool("ischecked", "required");

    $jq.validator.addMethod(
        "daterange",
        function (value, element, params) {
            return this.optional(element) || (value >= params.min && value <= params.max);
        });

    adapters.add(
        "daterange",
        ["min", "max"],
        function (options) {
            var params = {
                min: options.params.min,
                max: options.params.max
            };
            options.rules["daterange"] = params;
            options.messages["daterange"] = options.message;
        });

    adapters.fxbAddNumberVal("min");
    adapters.fxbAddNumberVal("max");
    adapters.fxbAddNumberVal("step");

    adapters.fxbAddMinMax("range", "min", "max");
    adapters.fxbAddMinMax("length", "minlength", "maxlength");
    adapters.fxbAddMinMax("daterange", "min", "max");

})(jQuery);
(function ($jq) {
    var eventIds = {
        fieldCompleted: "2ca692cb-bdb2-4c9d-a3b5-917b3656c46a",
        fieldError: "ea27aca5-432f-424a-b000-26ba5f8ae60a"
    };

    function endsWith(str, suffix) {
        return str.toLowerCase().indexOf(suffix.toLowerCase(), str.length - suffix.length) !== -1;
    }

    function getOwner(form, elementId) {
        var targetId = elementId.slice(0, -(elementId.length - elementId.lastIndexOf(".") - 1)) + "Value";
        return form.find("input[name=\"" + targetId + "\"]")[0];
    }

    function getSessionId(form) {
        var formId = form[0].id;
        var targetId = formId.slice(0, -(formId.length - formId.lastIndexOf("_") - 1)) + "FormSessionId";
        var element = form.find("input[type='hidden'][id=\"" + targetId + "\"]");
        return element.val();
    }

    function getElementName(element) {
        var fieldName = element.name;
        if (!endsWith(fieldName, "value")) {
            var searchPattern = "fields[";
            var index = fieldName.toLowerCase().indexOf(searchPattern);
            return fieldName.substring(0, index + searchPattern.length + 3) + "Value";
        }

        return fieldName;
    }

    function getElementValue(element) {
        var value;
        if (element.type === "checkbox" || element.type === "radio") {
            var form = $jq(element).closest("form");
            var checkboxList = form.find("input[name='" + element.name + "']");
            if (checkboxList.length > 1) {
                value = [];
                checkboxList = checkboxList.not(":not(:checked)");
                $jq.each(checkboxList, function () {
                    value.push($jq(this).val());
                });
            } else {
                value = element.checked ? "1" : "0";
            }
        } else {
            value = $jq(element).val();
        }

        if (value && Object.prototype.toString.call(value) === "[object Array]") {
            value = value.join(",");
        }

        return value;
    }

    function getFieldName(element) {
        return $jq(element).attr("data-sc-field-name");
    }

    $jq.fxbFormTracker = function (el, options) {
        this.el = el;
        this.$el = $jq(el);
        this.options = $jq.extend({}, $jq.fxbFormTracker.defaultOptions, options);
        this.init();
    },

        $jq.fxbFormTracker.parse = function (formId) {
            var $form = $jq(formId);
            $form.track_fxbForms();
        },

        $jq.extend($jq.fxbFormTracker,
            {
                defaultOptions: {
                    formId: null,
                    sessionId: null,
                    fieldId: null,
                    fieldValue: null,
                    duration: null
                },

                prototype: {
                    init: function () {
                        this.options.duration = 0;
                        this.options.formId = this.$el.attr("data-sc-fxb");
                    },

                    startTracking: function () {
                        this.options.sessionId = getSessionId(this.$el);

                        var self = this;
                        var inputs = this.$el.find("input:not([type='submit']), select, textarea");
                        var trackedInputs = inputs.filter("[data-sc-tracking='True'], [data-sc-tracking='true']");
                        if (trackedInputs.length) {
                            inputs.not(trackedInputs).bind("focus",
                                function () {
                                    self.onFocusField(this);
                                });
                            trackedInputs.bind("focus", function () {
                                self.onFocusField(this, true);
                            }).bind("blur change",
                                function () {
                                    self.onBlurField(this);
                                });
                        }
                    },

                    onFocusField: function (element, hasTracking) {
                        if (!hasTracking) {
                            this.options.fieldId = "";
                            return;
                        }

                        var fieldId = getElementName(element);

                        if (this.options.fieldId !== fieldId) {
                            this.options.fieldId = fieldId;
                            this.options.duration = $jq.now();
                            this.options.fieldValue = getElementValue(element);
                        }
                    },

                    onBlurField: function (element) {
                        var fieldId = getElementName(element);
                        var timeStamp = $jq.now();

                        if (!endsWith(fieldId, "value")) {
                            var owner = getOwner(this.$el, fieldId);
                            if (!owner) {
                                return;
                            }

                            element = owner;
                        }

                        var duration = this.options.duration ? Math.round((timeStamp - this.options.duration) / 1000) : 0;
                        var value = getElementValue(element);
                        var fieldChanged = this.options.fieldId !== fieldId;
                        if (fieldChanged) {
                            this.options.fieldId = fieldId;
                            this.options.duration = $jq.now();
                            duration = 0;
                        }
                        if (fieldChanged || this.options.fieldValue !== value) {
                            this.options.fieldValue = value;

                            var fieldName = getFieldName(element);
                            var clientEvent = this.buildEvent(fieldId, fieldName, eventIds.fieldCompleted, duration);

                            var validator = this.$el.data("validator");
                            var validationEvents = [];
                            if (validator && !validator.element(element)) {
                                validationEvents = this.checkClientValidation(element, fieldName, validator, duration);
                            }

                            this.trackEvents($jq.merge([clientEvent], validationEvents));
                        }
                    },

                    buildEvent: function (fieldId, fieldName, eventId, duration) {
                        var fieldIdHidden = fieldId.slice(0, -5) + "ItemId";
                        fieldId = $jq("input[name=\"" + fieldIdHidden + "\"]").val();

                        return {
                            'formId': this.options.formId,
                            'sessionId': this.options.sessionId,
                            'eventId': eventId,
                            'fieldId': fieldId,
                            'duration': duration,
                            'fieldName': fieldName
                        };
                    },

                    checkClientValidation: function (element, fieldName, validator, duration) {
                        var tracker = this;
                        var events = [];

                        $jq.each(validator.errorMap,
                            function (key) {
                                if (key === element.name) {
                                    var clientEvent = tracker.buildEvent(key, fieldName, eventIds.fieldError, duration);
                                    events.push(clientEvent);
                                }
                            });

                        return events;
                    },

                    trackEvents: function (events) {
                        $jq.ajax({
                            type: "POST",
                            url: "/fieldtracking/register",
                            data: JSON.stringify(events),
                            dataType: "json",
                            contentType: "application/json"
                        });
                    }
                }
            });

    $jq.fn.track_fxbForms = function (options) {
        return this.each(function () {
            var tracker = $jq.data(this, "fxbForms.tracking");
            if (tracker) {
                tracker.startTracking();
            } else {
                tracker = new $jq.fxbFormTracker(this, options);
                $jq.data(this, "fxbForms.tracking", tracker);
                tracker.startTracking();
            }
        });
    };

    $jq(document).ready(function () {
        $jq("form[data-sc-fxb]").track_fxbForms();
    });
})(jQuery);
$(function () {
    var consentLabels = $('form label.accept-terms');
    consentLabels.each(function (index,elem) {
        var consentLabel = $(elem);
        var html = consentLabel.text().replace("Privacy Policy", "<a target='_blank' href='http://www.WageWorksoutdoor.com/privacy-policy/'>Privacy Policy</a>")
            .replace('Terms & Conditions', "<a target='_blank' href='http://www.WageWorksoutdoor.com/terms-conditions/'>Terms & Conditions</a>")
            .replace('Terms and Conditions', "<a target='_blank' href='http://www.WageWorksoutdoor.com/terms-conditions/'>Terms and Conditions</a>");
        var temp = consentLabel.children();
        consentLabel.empty();
        consentLabel.append(temp)
        consentLabel.append(html);
    });


    var updateSelect = function (menu, text) {
        var emptyOption = menu.find("option:first-child");
        if ((emptyOption.val() === "") && emptyOption.text() === "") {
            emptyOption.attr("value", "");
            emptyOption.text(text);
            emptyOption.attr("label", text);
        }
    }

    var countryMenu = $(".country-menu");
    var statesMenu = $(".state-menu");
    var statesContainer = $(".state");
    var provincesContainer = $(".province")
    var provincesMenu = $(".province-menu");

    updateSelect(countryMenu, "Select a Country");
    updateSelect(statesMenu, "Select a State");
    updateSelect(provincesMenu, "Select a Province");


    var countryChanged = function () {
        // only change if there are states on the page
        if (!statesMenu) return;

        var selectedCountry = countryMenu.val();
        if (selectedCountry === "United States" || selectedCountry === "USA") {
            provincesContainer.hide();
            statesContainer.show();
        } else if (selectedCountry === "Canada" || selectedCountry === "CANADA") {
            statesContainer.hide();
            provincesContainer.show();
        } else {
            statesMenu.val("");
            statesContainer.hide();
            provincesContainer.hide();
        }
    }

    countryMenu.change(countryChanged);
    countryChanged();

    //$(".contact__fields input[type=submit]").on("click", function (e) {

    //})
});
$(function () {
    $('.nice-select').niceSelect();
});
(function ($, window) {
    'use-strict';

    var BSTabsToSelect = (function () {
        var ui = {};

        function handleClick(e) {
            var hash = $(this).val();
            ui.$tabs.filter('[href="' + hash + '"]').tab('show');
        }

        function buildSelect($tabs) {
            var selectHTML = '<select>';
            ui.$tabs.each(function () {
                var elem = $(this);
                selectHTML += '<option value="' + elem.attr('href') + '">' + elem.text(); + '</option>';
            });

            return selectHTML + '</select>';
        }

        function init($tabSet) {
            if (!$tabSet.length) return;
            ui.$tabs = $tabSet.find('.nav-link');
            var $select = $(buildSelect(ui.$tabs)).on('click change', handleClick);
            $select.insertBefore($tabSet);
        }

        return {
            init: init
        }
    })();

    window.WageWorks = window.WageWorks || {};
    window.WageWorks.BSTabsToSelect = BSTabsToSelect;
})($, window);