var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable(type) {
    dataTable = $('#tbPrimaryGroup').DataTable({
        paging: false,
        "ajax": {
            "url": "/Accounts/PrimaryGroup/GetAll?type=" + type
        },
        "columns": [
            { "data": "type", "width": "20%", className: "text-center"},
            { "data": "code", "width": "20%", className: "text-center"},
            { "data": "name", "width": "40%" },
            {
                "data": "id",
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