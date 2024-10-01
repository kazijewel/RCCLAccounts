var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable(type) {
    dataTable = $('#tbPrimaryGroup').DataTable({
        paging: false,
        "ajax": {
            "url": "/PrimaryGroup/GetAll"
        },
        "columns": [
            { "data": "itemOf", "width": "20%", className: "text-center"},
            { "data": "primaryGroupCode", "width": "20%", className: "text-center"},
            { "data": "primaryGroupName", "width": "40%" },
            {
                "data": "primaryId",
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