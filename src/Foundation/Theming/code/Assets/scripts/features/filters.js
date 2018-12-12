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