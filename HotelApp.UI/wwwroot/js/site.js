// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Create a global SweetAlert instance with Bootstrap button styles


// Generic showAlert using SweetAlert2 with Bootstrap button colors
function showAlert(type, title, text) {
    let icon = 'info';
    let confirmClass = 'btn btn-info';

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-primary',
            cancelButton: 'btn btn-default custom-cancel-btn'
        },
        buttonsStyling: false
    });

    switch (type) {
        case 'success':
            icon = 'success';
            confirmClass = 'btn btn-success';
            break;
        case 'error':
            icon = 'error';
            confirmClass = 'btn btn-danger';
            break;
        case 'warning':
            icon = 'warning';
            confirmClass = 'btn btn-warning';
            break;
        case 'info':
        default:
            icon = 'info';
            confirmClass = 'btn btn-info';
            break;
    }

    swalWithBootstrapButtons.fire({
        position: "top",
        icon: icon,
        title: title || '',
        text: text || '',
        confirmButtonText: 'OK',
        buttonsStyling: false,
        customClass: {
            confirmButton: confirmClass
        }
    });
}


// Global toastr config (shared for all calls)
toastr.options = {
    closeButton: true,
    progressBar: true,
    positionClass: "toast-top-right",
    timeOut: 5000,
    extendedTimeOut: 3000,
    preventDuplicates: true
};

function showToast(type, title, message) {
    switch (type) {
        case 'success':
            toastr.success(message || '', title || '');
            break;
        case 'error':
            toastr.error(message || '', title || '');
            break;
        case 'warning':
            toastr.warning(message || '', title || '');
            break;
        case 'info':
            toastr.info(message || '', title || '');
            break;
        default:
            console.warn('Unknown toast type:', type);
            toastr.info(message || '', title || '');
            break;
    }
}






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



//Delete Entity
$(document).ready(function () {

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-danger',
            cancelButton: 'btn btn-default custom-cancel-btn'
        },
        buttonsStyling: false
    });

    // Delegate delete button clicks globally
    $(document).on('click', '.btn-delete', function () {
        let button = $(this);
        let itemId = button.data("id");
        let row = button.closest("tr");
        let url = button.data("url"); // ✅ get the delete url dynamically

        swalWithBootstrapButtons.fire({
            position: "top",
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            iconColor: "red",
            showCancelButton: true,
            confirmButtonText: "Delete!",
            cancelButtonText: "Cancel",
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (response) {
                        if (response.success) {
                            swalWithBootstrapButtons.fire({
                                position: "top",
                                title: "Deleted!",
                                text: response.message,
                                icon: "success",
                                customClass: { confirmButton: 'btn btn-success' }
                            }).then(() => {
                                row.fadeOut(400, function () {
                                    $(this).remove();
                                });
                            });
                        } else {
                            swalWithBootstrapButtons.fire({
                                position: "top",
                                title: "Warning!",
                                text: response.message,
                                icon: "warning",
                                customClass: { confirmButton: 'btn btn-warning' }
                            });
                        }
                    },
                    error: function () {
                        swalWithBootstrapButtons.fire({
                            position: "top",
                            title: "Error!",
                            text: "Something went wrong!",
                            icon: "error"
                        });
                    }
                });
            }
        });
    });
});



$(document).ready(function () {

    // Generic DataTable loader
    function initEntityTable(config) {
        let dataTable = $(config.tableSelector).DataTable();

        function loadData(filters = {}, isRefresh = false) {
            $.ajax({
                url: config.url,
                type: 'GET',
                dataType: 'json',
                data: filters,
                success: function (response) {
                    if (response.success) {
                        let items = response.data;
                        dataTable.clear();

                        $.each(items, function (index, item) {
                            let rowData = config.mapRow(item);
                            dataTable.row.add(rowData);
                        });

                        dataTable.draw();
                    } else {
                        console.error("Failed to load data from: " + config.url);
                    }
                },
                error: function () {
                    console.error("Error while fetching data from: " + config.url);
                }
            });
        }

        // Filter submit
        $(config.filterFormSelector).submit(function (e) {
            e.preventDefault();
            let filters = {};

            // collect filters dynamically from inputs inside the form
            $(this).serializeArray().forEach(x => filters[x.name] = x.value);

            loadData(filters, false);
        });

        // Reset button
        if (config.refreshBtnSelector) {
            $(config.refreshBtnSelector).click(function () {
                $(config.filterFormSelector)[0].reset();
                loadData({}, true);
            });
        }

        // Initial load
        //loadData(config.defaultFilters || {}, true);

        return {
            reload: loadData
        };
    }

    // Expose globally
    window.initEntityTable = initEntityTable;
});
