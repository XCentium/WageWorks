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



