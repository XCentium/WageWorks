$(function () {
    var consentLabels = $('form label.accept-terms');
    consentLabels.each(function (index,elem) {
        var consentLabel = $(elem);
        var html = consentLabel.text().replace("Privacy Policy", "<a target='_blank' href='http://www.Wageworksoutdoor.com/privacy-policy/'>Privacy Policy</a>")
            .replace('Terms & Conditions', "<a target='_blank' href='http://www.Wageworksoutdoor.com/terms-conditions/'>Terms & Conditions</a>")
            .replace('Terms and Conditions', "<a target='_blank' href='http://www.Wageworksoutdoor.com/terms-conditions/'>Terms and Conditions</a>");
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