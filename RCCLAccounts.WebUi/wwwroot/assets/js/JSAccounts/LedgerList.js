var dataTable;

$(document).ready(function () {
    $("#tbLedger").dataTable().fnDestroy();
    loadDataTable("Asset");
    $("input[type=radio][name=assetName]").change(function () {
        var optType = $("input[name='assetName']:checked").val();
        if (optType != "") {
            $("#tbLedger").dataTable().fnDestroy();
            loadDataTable(optType);
        }
    });
});


function loadDataTable(type) {
    dataTable = $('#tbLedger').DataTable({
        "pageLength": 100,
        "columnDefs": [{ targets: 'no-sort', orderable: false }],
        "ajax": {
            "url": "/Ledger/GetData?type="+type
        },
        "columns": [
            { "data": "type" },
            { "data": "primaryGroupName" },
            { "data": "mainGroupName" },
            { "data": "subGroupName" },
            /*{ "data": "ledgerCode" },*/
            { "data": "ledgerName" },
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
                }, "width": "5%"
            }
        ]
    });
}