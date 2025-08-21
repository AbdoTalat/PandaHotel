// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function loadSelect(selector, url, textField, valueField) {
    var $select = $(selector);
    var selectedValue = $select.data("selected"); // read from data-selected attr

    $select.empty().append('<option value="">Loading...</option>').prop("disabled", true);

    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            $select.empty().append('<option value=""></option>');

            $.each(response, function (_, item) {
                var option = $('<option>', {
                    value: item[valueField],
                    text: item[textField]
                });

                if (selectedValue && item[valueField].toString() === selectedValue.toString()) {
                    option.prop("selected", true);
                }

                $select.append(option);
            });

            $select.prop("disabled", false);

            // If using select2, refresh it
            if ($select.hasClass("select2-hidden-accessible")) {
                $select.trigger("change");
            }
        },
        error: function (xhr) {
            console.error("Error loading " + url, xhr.responseText);
            $select.empty().append('<option value="">Error loading</option>').prop("disabled", false);
        }
    });
}
