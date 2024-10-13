

var x = 0;
$(document).ready(function () {
    
    $("#btnAdd").click(function () {
        init();
        buttonSet(false);
        $("#cashPayment").modal('show');
       
    });
    $("#btnSave").click(function () {
        saveWork(false, false);
    });
    $("#btnEdit").click(function () {
        saveWork(false, true);
    });
    $("#btnSaveNew").click(function () {
        saveWork(true, false);
    });
    $("#btnCancel").click(function () {
        $('#tbCashPayment').DataTable().ajax.reload();
    });
    $('#cashPayment').on('hidden.bs.modal', function () {
        $('#tbCashPayment').DataTable().ajax.reload();
    })
    $('#tbCashPayment').on('click', '.tbEdit', function () {
        clear();
        buttonSet(true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbCashPayment").dataTable().fnGetData(tr);

        var url = "/CashPayment/findData?id=" + data.transactionId;
        x = 0;
        tableClear(1);
        if (isData(url)) {
            findWork(url);
            $("#cashPayment").modal('show');
            attachmentLoad();
        }
        else {
            errorNotify("Data not found");
        }
    });
    $("#cash").change(function () {
        var cashId = $("#cash").val();
        //budgetBalanceSet(cashId, "#budget", "#balance");
        
    });
    $("#btnAddRow").click(function () {
        x = x + 1;
        addRow(x);
        ledger("#ledgerId"+x);
    });
    $("#btnPreview").click(function () {
        var voucherNo = localStorage.getItem("voucher");
        reportPreview(voucherNo);
    });
    $("#btnPreviewEdit").click(function () {
        var voucherNo = $("#voucherNo").val();
        reportPreview(voucherNo);
    });
    $("#btnClear").click(function () {
        clear();
    });
   
    
});


function reportPreview(voucherNo) {
    //var today = getCDay() + '-' + getCMonth() + '-' + getCYear();
    var today = new Date();
    var voucherType = "CP";
    var findType = "Voucher";
    var fromDate = getBdToDbFormat(today);
    var toDate = getBdToDbFormat(today);
    var url = "/Accounts/AccountsReport/VoucherAll?vType=" + voucherType + "&findType=" +
        findType + "&fromDate=" + fromDate + "&toDate=" + toDate + "&voucherNo=" + voucherNo;
    console.log(url);
    if (voucherNo != "" && voucherNo != null && voucherNo != undefined) {
        window.open(url, "_blank");
    }
    else {
        warningNotify("Unable to preview");
    }
}
function buttonSet(isEdit) {
    if (isEdit) {
        document.getElementById('add').style.display = "none";
        document.getElementById('edit').style.display = "block";
    }
    else {
        document.getElementById('add').style.display = "block";
        document.getElementById('edit').style.display = "none";
    }
}
function isData(url) {
    var ret = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            ret = res.isFind;
            console.log("Find:" + res.isFind);
        }
    });
    return ret;
}
function init() {
    clear();
}
function tableClear(start) {
    var table = document.getElementById('gridCashPayment');
    var rowCount = table.rows.length;
    for (var i = start; i < rowCount; i++) {
        table.deleteRow(start);
    }
    $("#gridCashPayment tr td select").val("0").trigger('change');
    $(".debitAmount").val("0");
    $(".budget").text("");
    $(".balance").text("");
}
function findWork(url) {
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            var d = res.data;
            console.log(d);
            for (var i = 0; i < d.length; i++) {
                if (parseFloat(d[i].drAmt) == 0) {
                    $("#upload").val(d[i].attachment);
                    $("#paidTo").val(d[i].transactionWith);
                    $("#date").val(d[i].voucherDate);
                    $("#voucherNo").val(d[i].voucherNo);
                    $("#transactionId").val(d[i].transactionId);
                    $("#autoId").val(d[i].id);
                    $("#description").val(d[i].narration);
                   // budgetBalanceSet(d[i].ledgerUniqueId,"#budget","#balance");
                    ledgerListCash("#cash", d[i].ledgerUniqueId);
                }
                else {
                    console.log("Cash");
                    x = x + 1;
                    addRow(x);
                    ledger("#ledgerId" + x, d[i].ledgerUniqueId);
                    //budgetBalanceSet(d[i].ledgerUniqueId, "#lbBudget" + x, "#lbBalance" + x);
                    $("#debitAmount" + x).val(amountShowWithComma(d[i].drAmt));
                }
            }
        }
    });
}

function saveWork(isNew, isEdit) {
    if (checkValidation()) {
        submit(isNew,isEdit);
    }
}
function clear() {
    clearAttachment();
    //document.getElementById("SenctionDate").valueAsDate = new Date();
    //var today = getCDay() + '-' + getCMonth() + '-' + getCYear();
    var today = new Date();
    $("#paidTo").val("");
    $("#date").val(today);
    //$("#cash").select2('data', { id: "0", text:"Choose a Ledger" });
    $("#cash").val("0");
    $("#voucherNo").val("");
    //$("#budget").val("");
    //$("#balance").val("");
    $("#description").val("");
    var ledgerUniqeId = getLedgerUniqueId();
    localStorage.clear();
    ledgerListCash("#cash", ledgerUniqeId);
    //budgetBalanceSet(ledgerUniqeId, "#budget", "#balance");
    tableClear(1);
    x = x + 1;
    addRow(x);
    ledger("#ledgerId" + x);
    autocompleteText();
}
function newclear() {
    clearAttachment();
    $("#paidTo").val("");
    $("#voucherNo").val("");
    $("#description").val("");
    //$("#budget").val("");
    //$("#balance").val("");
    var ledgerUniqeId = $("#cash").val();
    localStorage.clear();
    //budgetBalanceSet(ledgerUniqeId, "#budget", "#balance");
    tableClear(1);
    x = x + 1;
    addRow(x);
    ledger("#ledgerId" + x);
    autocompleteText();
}

function checkValidation() {
    var date = $("#date").val();
    var cashInfo = $("#cash").val();
    var voucherNo = $("#voucherNo").val();
    var totalAmount = 0;
    var paidTo = $("#paidTo").val();
    var tableHead = "#gridCashPayment tr td ";
    var tbElements = document.querySelectorAll(tableHead + "select");
    var tbAmount = document.querySelectorAll(tableHead + ".debitAmount");
    var count = 0;
    console.log(tbElements);
    for (var i = 0; i < tbElements.length; i++) {
        var ledger = tbElements[i].getAttribute('id');
        console.log(ledger);
        var amount = tbAmount[i].getAttribute('id');
        var lId = document.getElementById(ledger).value;
        console.log(lId);
        var amtId = document.getElementById(amount).value;
        amtId = amtId.replace(/,/g, "");
        if (lId != "0" && parseFloat(amtId) > 0) {
            totalAmount += parseFloat(amtId);
            count++;
        }
    }
    //console.log(parseFloat(totalAmount));
    //console.log(parseFloat(balance));
    console.log(cashInfo);
   
    if (paidTo != "") { 
        if (date != "") {
            if (cashInfo != "") {
                if (count > 0) {
                    if ($("#description").val() != "") {
                        //if (document.getElementById("attachmentUP").files.length != 0 || $("#upload").val() != "") {
                            return true;
                        /*}
                        else {
                            warningNotify("Attachment not found!");
                            return false;
                        }*/
                    } else {
                        warningNotify("Description couldn't be empty!");
                        return false;
                    }
                }
                else {
                    warningNotify("Please give at least one ledger name and debit amount");
                    return false;
                }
            }
            else {
                warningNotify("Please select cash a/c");
                return false;
            }
        }
        else {
            warningNotify("Please give date");
            return false;
        }
    }
    else {
        warningNotify("Please entire Paid to!");
        return false;
    }

}
function budgetBalanceSet(id,budgetId,balanceId) {
    var url = "/CashPayment/LedgerBudgetBalance";
    var date = getBdToDbFormat($("#date").val());
    if (date == undefined) {
       // date = getCDay() + '-' + getCMonth() + '-' + getCYear();
        date = new Date();
    }
    $.ajax({
        url: url,
        data: {id:id,date:date},
        async: false,
        success: function (res) {
            console.log(JSON.stringify(res));
            if (budgetId.substring(1, 3) == "lb") {
                $(budgetId).text(amountShowWithComma(res.budget));
                $(balanceId).text(amountShowWithComma(res.balance));
            }
            else {
                $(budgetId).val(amountShowWithComma(res.budget));
                $(balanceId).val(amountShowWithComma(res.balance));
            }
        }
    });
}
function addRow(x) {
    var tableArray = new Array();
    tableArray = ['Ledger Code Name', 'Budget', 'Balance', 'Debit Amount',''];
    var gridCashPayment = document.getElementById('gridCashPayment');
    gridCashPayment.style.backgroundColor = "green";
    var rowCount = gridCashPayment.rows.length;    
    var tr = gridCashPayment.insertRow(rowCount); 
    tr = gridCashPayment.insertRow(rowCount);
    
    
    for (var c = 0; c < tableArray.length; c++) {
        var td = document.createElement('td');
        
        td = tr.insertCell(0);
        if (parseInt(x) % 2 == 0) {
            td.setAttribute('style', 'background-color:#fff;color:#000');
        }
        else {
            td.setAttribute('style', 'background-color:#eee;color:#000');
        }
        if (c == 4) {

            var select = document.createElement('select');
            
            select.setAttribute('class', 'form-control input-sm');
            select.setAttribute('id', 'ledgerId' + x);
            select.setAttribute('onchange', 'ledgerAction(' + x + ')');
            td.appendChild(select);
        }
        if (c == 3) {
            var ele = document.createElement('label');
            ele.setAttribute('class', 'col-sm-2 control-label budget');
            //ele.setAttribute('id', 'lbBudget' + x);
            td.appendChild(ele);
        }
        if (c == 2) {

            var ele = document.createElement('label');
            ele.setAttribute('class', 'col-sm-2 control-label balance');
            //ele.setAttribute('id', 'lbBalance'+x);
            td.appendChild(ele);
        }
        if (c == 1) {

            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm amount debitAmount');
            ele.setAttribute('type', 'text');
            ele.setAttribute('id', 'debitAmount'+x);
            ele.setAttribute('onkeyup', 'amountWithComma("#debitAmount"+' + x +')');
            ele.setAttribute('onchange', '$("#description").focus()');
            td.appendChild(ele);
        }
        if (c == 0) {
            var button = document.createElement('input');
            button.setAttribute('type', 'button');
            button.setAttribute('value', 'Remove');
            button.setAttribute('class', 'btn btn-default form-control input-sm');
            button.setAttribute('onclick', 'removeRow(this)');
            td.appendChild(button);
        }
        $('#ledgerId' + x).select2();
        $("#ledgerId" + x).focus();
    }
}
function ledgerAction(a) {
    var ledger = $("#ledgerId" + a).val();
    $("#debitAmount" + a).focus();
    //budgetBalanceSet(ledger, "#lbBudget"+a, "#lbBalance"+a);

    var counts = 0;
    var elements = document.querySelectorAll("#gridCashPayment tr td select");
    for (var i = 0; i < elements.length;i++) {
        var lId = $("#" + elements[i].id).val();
        if (lId == ledger) {
            counts++;
        }
    }
    if (counts === 2) {
        warningNotify("Already selected!");
        $("#ledgerId" + a).val("0").trigger('change');
        $("#debitAmount" + a).val("");
        $("#lbBudget" + a).text("");
        $("#lbBalance" + a).text("");
        $("#ledgerId" + a).focus();
    }
    
}
function addRowByAmount(debitAmount, ledgerId) {
    var elements = document.querySelectorAll("#gridCashPayment tr td select");
    var count = 0;
    for (var i = 0; i < elements.length; i++) {
        var getLedId = "#" + elements[i].getAttribute('id');
        if (getLedId == ledgerId) {
            count = i+1;
            break;
        }
    }
    if (elements.length==count) {
        if ($(ledgerId).val() != "0" && $(ledgerId).val() != "") {
            if (parseFloat($(debitAmount).val()) > 0) {
                x = x + 1;
                addRow(x);
                ledger("#ledgerId" + x);
            }
        }
        else {
            $(debitAmount).val("");
            $(debitAmount).focus();
        }
    }
}
function removeRow(oButton) {
    var tableId = "gridCashPayment";
    var empTab = document.getElementById(tableId);
    var elements = document.querySelectorAll("#" + tableId + " tr td select");
    if (elements.length > 1) {
        empTab.deleteRow(oButton.parentNode.parentNode.rowIndex); 
    }
}

function initFs(fs) {
    console.log(fs);
    fs.root.getFile('logFile.txt', { create: true }, function (fileEntry) {

        fileEntry.createWriter(function (writer) {  // FileWriter

            writer.onwrite = function (e) {
                console.log('Write completed.');
            };

            writer.onerror = function (e) {
                console.log('Write failed: ' + e.toString());
            };

            var bb = new BlobBuilder();
            bb.append('Lorem ipsum');
            writer.write(bb.getBlob('text/plain'));

        }, errorHandler);
    }
    )
};

function onResolveSuccess(fileEntry) {
    console.log(fileEntry.name);
}

function fail(error) {
    console.log(error.code);
}


async function submit(isNew, isEdit) {
    localStorage.clear();
    var time = new Date();
    time = (moment(time).format('HH:mm:ss'))
    var date = getBdToDbFormat($("#date").val());
    var id = 0;
    var transactionId="";
    var voucherNo="";
    if (isEdit) {
        id = $("#autoId").val();
        transactionId = $("#transactionId").val();
        voucherNo = $("#voucherNo").val();
    } else {
        transactionId = getTransactionId(date);
        voucherNo = getVoucherNo(date);
    }

    var paidTo = $("#paidTo").val();
    var cashId = getLedgerId($("#cash").val());
    var cashInfo = $("#cash option:selected").text();
    //var cashNameInfo = cashInfo.substring(cashInfo.indexOf('-') + 1);

    var narration = $("#description").val();

    var tableHead = "#gridCashPayment tr td ";
    var elements = document.querySelectorAll(tableHead +" select");
    var amt = document.querySelectorAll(tableHead + " .debitAmount");

    var uploadFile = $("#upload").val();
    var fileUpload = $("#attachmentUP").get(0);
    var files = fileUpload.files;
     
    // Create FormData object  
    var formData = new FormData();
    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        formData.append(files[i].name, files[i]);
    }

    var data = {
        objList: [],
        cashHeadId:cashId,
        cashHead: cashInfo 
    }

    
  
    for (var i = 0; i < elements.length; i++) {
        var ledgerId = elements[i].getAttribute('id');
        var amountId = amt[i].getAttribute('id');

        var lId = $("#" + ledgerId).val();
        var ln = $("#" + ledgerId + " option:selected").text();
        var ledgerName = ln;
        var amtId = $("#" + amountId).val().replace(/,/g, "");
        var ledger = "";
        if (lId != "0") {
            ledger = getLedgerId(lId);
        }
        if (lId != "0" && lId != "") {
            data.objList.push({
                "Id":id,"TransactionId":transactionId,"LedgerId": ledger, "LedgerName": ledgerName, "DrAmount": amtId,
                "TransactionWith": paidTo, "VoucherDate": date + " " + time, "VoucherNo": voucherNo, "Narration": narration,
                "AttachBill": uploadFile
            });
           
        }
    }
    // Adding one more key to FormData object  
    formData.append("cashHeadId", data.cashHeadId);
    formData.append("cashHead", data.cashHead);
    for (var i = 0; i < data.objList.length; i++) {
        formData.append("objList[" + i + "].id", data.objList[i].Id);
        formData.append("objList[" + i + "].transactionId", data.objList[i].TransactionId);
        formData.append("objList[" + i + "].ledgerId", data.objList[i].LedgerId);
        formData.append("objList[" + i + "].ledgerName", data.objList[i].LedgerName);
        formData.append("objList[" + i + "].drAmount", data.objList[i].DrAmount);
        formData.append("objList[" + i + "].transactionWith", data.objList[i].TransactionWith);
        formData.append("objList[" + i + "].voucherDate", data.objList[i].VoucherDate);
        formData.append("objList[" + i + "].voucherNo", data.objList[i].VoucherNo);
        formData.append("objList[" + i + "].narration", data.objList[i].Narration);
        formData.append("objList[" + i + "].attachBill", data.objList[i].AttachBill);
    }
    
    $.ajax({
        url: "/CashPayment/cashPaymentSave",
        data: formData,
        type: 'POST',
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        success: function (res) {
            if (res.success) {
                successNotify(res.message);
                localStorage.setItem("voucher", voucherNo);
                if (!isNew || isEdit) {
                    clear();
                    $("#cashPayment").modal('hide');
                } else {
                    newclear();
                }
                $('#tbCashPayment').DataTable().ajax.reload();
                $("#paidTo").focus();
                /*var urls = "/Accounts/CashPayment/FileRemove";
                $.getJSON(urls, function (data) {
                       console.log(data) 
                });*/
            } else {
                errorNotify(res.message);
            }
        }
    });


}
function getLedgerId(ledId) {
    var ret = "";
    var url = "/CashPayment/GetLedgerId";
    $.ajax({
        url: url,
        data: { id: ledId },
        async: false,
        success: function (res) {
            ret = res.ledgerId;
        }

    });
    return ret;
}
function getLedgerUniqueId() {
    var ret = "";
    var url = "/CashPayment/UniqueLedgerId";
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            ret = res.data;
        }

    });
    return ret;
}
function getTransactionId(date) {
    var ret = "";
    var url = "/CashPayment/TransactionId";
    $.ajax({
        url: url,
        data: { date: date },
        async: false,
        success: function (res) {
            ret = res.data;
        }

    });
    return ret;
}
function getVoucherNo(date) {
    var ret = "";
    var url = "/CashPayment/VoucherNo";
    $.ajax({
        url: url,
        data: {date:date},
        async: false,
        success: function (res) {
            ret = res.data;
        }

    });
    return ret;
}
function ledger(ledgerId,setValue) {
    var url = "/CashPayment/LedgerInfo";
    $.getJSON(url, function (data) {
        var item = "";
        $(ledgerId).empty();
        item += '<option value="' + 0 + '">Choose a ledger</option>'
        $(ledgerId).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(ledgerId).html(item);
        if (setValue != undefined) {
            $(ledgerId).val(setValue).trigger('change');
        }
    });
}
function ledgerListCash(ledgerId, setValue) {
    var url = "/CashPayment/LedgerInfoCash";
    $.getJSON(url, function (data) {
        var item = "";
        $(ledgerId).empty();
        item += '<option value="' + 0 + '">Choose a ledger</option>'
        $(ledgerId).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(ledgerId).html(item);
        if (setValue != undefined) {
            $(ledgerId).val(setValue).trigger('change');
        }
    });
}


function autocompleteText() {
    var ret = [];
    var url = "/CashPayment/Narrations";
    try {
        $.getJSON(url, function (data) {
            //console.log(data.data)
            ret = data.data;
            autocomplete(document.getElementById("description"), ret);
        });
    } catch (e) { console.log(e) }
}