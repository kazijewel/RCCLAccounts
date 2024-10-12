﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblNarration').DataTable({
        "ajax": {
            "url": "/NarrationInfo/GetAll"
        },
        "columns": [
            { "data": "voucherType", "width": "25%" },
            { "data": "narrationName", "width": "33%" },
            {
                "data": "narrationId",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a class="btn btn-sm btn-primary text-white tbEdit">
                                    <i class="fa fa-edit"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "10%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}