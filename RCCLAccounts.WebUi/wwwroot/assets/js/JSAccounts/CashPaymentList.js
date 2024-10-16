var dataTable;
$(document).ready(function () {
   // var fromdate = getCDay() + '-' + getCMonth() + '-' + getCYear();

    var today = new Date();
    var formattedDate = today.toISOString().split('T')[0]; // Format the date as YYYY-MM-DD
    $("#fromDate").val(formattedDate); 
    $("#toDate").val(formattedDate); 


    //var fromdate = new Date();
    //var todate = getCDay() + '-' + getCMonth() + '-' + getCYear();
    //var todate = new Date();
    var url = url = "/CashPayment/GetCashPaymentAll?fromDate=" + getBdToDbFormat(fromdate) + "&toDate=" + getBdToDbFormat(todate);
    loadDataTable(url);
    reloadData();
    $("#fromDate").change(function () {
        console.log("fromDate");
        console.log(fromdate);
        var fromdate = $("#fromDate").val();
        var todate = $("#toDate").val();
        if (fromdate != undefined && fromdate != "" && todate != undefined && todate != "") {
            var url = "/CashPayment/GetCashPaymentAll?fromDate=" + getBdToDbFormat(fromdate) + "&toDate=" + getBdToDbFormat(todate);
            dataTable.ajax.url(url).load();
        }

    });
    $("#toDate").change(function () {
        console.log(fromdate);
        console.log("toDate");
        var fromdate = $("#fromDate").val();
        var todate = $("#toDate").val();
        if (fromdate != undefined && fromdate != "" && todate != undefined && todate != "") {
            var url = "/CashPayment/GetCashPaymentAll?fromDate=" + getBdToDbFormat(fromdate) + "&toDate=" + getBdToDbFormat(todate);
            dataTable.ajax.url(url).load();
        }
    });

});
function reloadData() {
    //var fromdate = getCDay() + '-' + getCMonth() + '-' + getCYear();
    //var todate = getCDay() + '-' + getCMonth() + '-' + getCYear();
    var today = new Date();
    var formattedDate = today.toISOString().split('T')[0]; // Format the date as YYYY-MM-DD
   

    $.ajax({
        url: "/Accounts/FiscalYearInfo/GetFiscaleYearDate",
        async: false,
        success: function (res) {
           // console.log(res.opening);
            fromdate = res.opening;
            todate = res.closing;

        }
    });
    //$("#fromDate").val(fromdate);
    //$("#toDate").val(todate);

    $("#fromDate").val(formattedDate);
    $("#toDate").val(formattedDate); 
    var url = "/CashPayment/GetCashPaymentAll?fromDate=" + getBdToDbFormat(fromdate) + "&toDate=" + getBdToDbFormat(todate)  ;
    dataTable.ajax.url(url).load();
}
function loadDataTable(url) {
    var element = document.getElementById("tbCashPayment");
    const isAdmin = element.dataset.admin;
    var isEdit = element.dataset.edit;
    //var isDelete = element.dataset.delete;
    dataTable = $('#tbCashPayment').DataTable({
        //"order": [[1, "desc"]],
        "ordering": false,
        "targets": 'no-sort',
        "bSort": false,
        "autoWidth": true,
        "pageLength": 100,
        "ajax": {
            "url": url
        },
        "columns": [
            { "data": "voucherNo", "width": "10%"},
            {
                "data": "voucherDate",
                "render": function (data) {
                    return (moment(data).format("DD-MM-YYYY"));
                }, "width": "9%" 
            },
            { "data": "ledgerName"},
            { "data": "narration"},
            { "data": "drAmt", className: "text-right", render: $.fn.dataTable.render.number(',', '.', 0, '') },
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
                    return data==null?"":(moment(data).format("DD-MM-YYYY hh:mm a"));
                },"width": "9%"
            },
            {
                "data": null,
                "render": function (data) {
                    if (data.auditApprove == '2') {
                        if (isAdmin == 'True') {
                            return `
                            <div class="text-center">
                                <a class="btn btn-sm btn-primary text-white tbEdit">
                                    <i class="fa fa-edit"></i> 
                                </a>
                            </div>
                            `;
                        } else {
                            return `<div class="text-center"></div>`;
                        }

                    } else {
                        if (isEdit == 'True') {
                            return `
                            <div class="text-center">
                                <a class="btn btn-sm btn-primary text-white tbEdit">
                                    <i class="fa fa-edit"></i> 
                                </a>
                            </div>
                            `;
                        } else {
                            return `<div class="text-center"></div>`;
                        }
                    }

                }, "width": "5%"
            }
        ]
    });
}