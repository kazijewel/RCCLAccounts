var dataTable;
$(document).ready(function () {
    //var today = getCDay() + '-' + getCMonth() + '-' + getCYear();
    //$("#fromDate").val(today);
    //$("#toDate").val(today);

    // Create a new Date object with the current date
    const currentDate = new Date();

    // Format the date as "YYYY-MM-DD" (required for the input type="date" element)
    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = String(currentDate.getDate()).padStart(2, '0');
    const formattedDate = `${year}-${month}-${day}`;

    $("#fromDate").val(formattedDate);
    $("#toDate").val(formattedDate);

    var fromdate = formattedDate;
    var todate = formattedDate;

    var url = "/JournalVoucher/GetJournalVoucher?fromDate=" + fromdate + "&toDate=" + todate;
    loadDataTable(url);
    reloadData();
    $("#fromDate").change(function () {
        var fromdate = $("#fromDate").val();
        var todate = $("#toDate").val();
        
        if (fromdate != undefined && fromdate != "" && todate != undefined && todate != "") {
            var url = "/JournalVoucher/GetJournalVoucher?fromDate=" + fromdate + "&toDate=" + todate;
            dataTable.ajax.url(url).load();
        }

    });
    $("#toDate").change(function () {
        var fromdate = $("#fromDate").val();
        var todate = $("#toDate").val();
        if (fromdate != undefined && fromdate != "" && todate != undefined && todate != "") {
            var url = "/JournalVoucher/GetJournalVoucher?fromDate=" + fromdate + "&toDate=" + todate;
            dataTable.ajax.url(url).load();
        }

    });

});

function reloadData() {
    //var fromdate = getCDay() + '-' + getCMonth() + '-' + getCYear();
    //var todate = getCDay() + '-' + getCMonth() + '-' + getCYear();

    const currentDate = new Date();

    // Format the date as "YYYY-MM-DD" (required for the input type="date" element)
    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = String(currentDate.getDate()).padStart(2, '0');
    const formattedDate = `${year}-${month}-${day}`; 


    $.ajax({
        url: "/FiscalYearInfo/GetFiscaleYearDate",
        async: false,
        success: function (res) {
            // console.log(res.opening);
            fromdate = res.opening;
            todate = res.closing;

        }
    });
    $("#fromDate").val(formattedDate);
    $("#toDate").val(formattedDate);

    var fromdate = formattedDate;
    var todate = formattedDate;

    var url = "/JournalVoucher/GetJournalVoucher?fromDate=" + fromdate + "&toDate=" + todate;
    dataTable.ajax.url(url).load();
}

function loadDataTable(url) {
    var element = document.getElementById("tbJournalVoucher");
    const isAdmin = element.dataset.admin;
    var isEdit = element.dataset.edit;
    dataTable = $('#tbJournalVoucher').DataTable({
        //"order": [[1, "desc"]],
        "targets": 'no-sort',
        "bSort": false,
        "autoWidth": true,
        "pageLength": 100,
        "ajax": {
            "url": url
        },
        "processing": true,
        "columns": [
            { "data": "voucherNo", "width": "10%" },
            {
                "data": "voucherDate",
                "render": function (data) {
                    return (moment(data).format("DD-MM-YYYY"));
                }, "width": "15%"
            },
            { "data": "narration"},
            { "data": "drAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
            { "data": "crAmount", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
            {
                "data": null,
                "render": function (data) {
                    if (data.attachment != null && data.attachment != '' && data.attachment != '0') {
                        return `
                            <div class="text-center">
                                <a href=${data.attachment} target="_blank" style="cursor: pointer;">
                                     <i style="font-size: 22px;color: '#0088cc';" class="fa fa-paperclip" ></i>
                                </a>
                            </div>
                           `;
                    } else {
                        return `
                            <div></div>
                           `;
                    }

                }, "width": "3%"
            },
            {
                "data": null,
                "render": function (data) {
                    if (data.auditApprove == '2') {
                        return `
                            <div class="text-center" style="color: green">
                                Approved
                            </div>
                           `;
                    } else {
                        return `
                             <div class="text-center" style="color: red">
                                Pendding
                            </div>
                           `;
                    }

                }, "width": "5%"
            },
            {
                "data": "approveBy", "width": "5%"
            },
            {
                "data": "approveTime",
                "render": function (data) {
                    return data == null ? "" : (moment(data).format("DD-MM-YYYY hh:mm a"));
                }, "width": "9%"
            },
            {
                "data": null,
                "render": function (data) {
                    if (data.auditApprove == '2') {
                       /* if (isAdmin == 'True') {*/
                            return `
                            <div class="text-center">
                                <a class="btn btn-sm btn-primary text-white tbEdit">
                                    <i class="fa fa-edit"></i> 
                                </a>
                            </div>
                            `;
                        //} else {
                        //    return `<div class="text-center"></div>`;
                        //}

                    } else {
                     /*   if (isEdit == 'True') {*/
                            return `
                            <div class="text-center">
                                <a class="btn btn-sm btn-primary text-white tbEdit">
                                    <i class="fa fa-edit"></i> 
                                </a>
                            </div>
                            `;
                        //} else {
                        //    return `<div class="text-center"></div>`;
                        //}
                    }

                }, "width": "5%"
            },
            {
                "data": "transactionId",
                "render": function (data) {
                    if (data.auditApprove == '2') {
                        /*if (isAdmin == 'True') {*/
                        return `
                            <div class="text-center">
                                <a class="btn btn-sm btn-primary text-white tbDelete">
                                    <i class="fa fa-trash-alt fa-lg"></i> 
                                </a>
                            </div>
                            `;
                        //} else {
                        //    return `<div class="text-center"></div>`;
                        //}

                    } else {
                        /* if (isEdit == 'True') {*/
                        return `
                            <div class="text-center">
                                <a class="btn btn-sm btn-primary text-white tbDelete">
                                    <i class="fa fa-trash-alt fa-lg"></i> 
                                </a>
                            </div>
                            `;
                        //} else {
                        //    return `<div class="text-center"></div>`;
                        //}
                    }
                }, "width": "5%"
            }
        ]
    });
}