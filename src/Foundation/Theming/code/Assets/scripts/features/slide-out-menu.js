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