var dataTable;

$(document).ready(function () {
    $("#tbMainGroup").dataTable().fnDestroy();
    loadDataTable("Asset");
    $("input[type=radio][name=assetName]").change(function () {
        var optType = $("input[name='assetName']:checked").val();
        if (optType != "") {
            $("#tbMainGroup").dataTable().fnDestroy();
            loadDataTable(optType);
        }
    });
});


function loadDataTable(type) {
    dataTable = $('#tbMainGroup').DataTable({
        paging: false,
        "ajax": {
            "url": "/MainGroup/GetAll?type="+type
        },
        "columns": [
            { "data": "type", "width": "10%" },
            { "data": "primaryGroupCode", "width": "15%", className: "text-center" },
            { "data": "primaryGroupName", "width": "25%" },
            { "data": "mainGroupCode", "width": "15%", className: "text-center"},
            { "data": "mainGroupName", "width": "25%" },
            
            
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