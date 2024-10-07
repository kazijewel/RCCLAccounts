var dataTable;

$(document).ready(function () {
    $("#tbSubGroup").dataTable().fnDestroy();
    loadDataTable("Asset");
    $("input[type=radio][name=assetName]").change(function () {
        var optType = $("input[name='assetName']:checked").val();
        if (optType != "") {
            $("#tbSubGroup").dataTable().fnDestroy();
            loadDataTable(optType);
        }
    });
});


function loadDataTable(type) {
    dataTable = $('#tbSubGroup').DataTable({
        paging: false,
        "ajax": {
            "url": "/SubGroup/GetAll?type=" + type
        },
        "columns": [
            { "data": "type", "width": "10%"},
           
            {
                "data": "primaryGroupName", "width": "25%",
            },
            {
                "data": "mainGroupName", "width": "25%",
            },
            { "data": "subGroupCode", "width": "10%", className: "text-center" },
            { "data": "subGroupName", "width": "20%" },
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