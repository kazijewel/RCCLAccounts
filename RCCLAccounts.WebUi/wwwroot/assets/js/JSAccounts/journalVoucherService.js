var x = 0;
var baseFile;
$(document).ready(function () {
    
    $("#btnAdd").click(function () {
        init();
        buttonSet(false);
        $("#journalVoucher").modal('show');
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
    //$("#btnCancel").click(function () {
    //    $('#tbJournalVoucher').DataTable().ajax.reload();
    //});
    $(document).on('click', '#btnCancel', function () {
        $('#tbJournalVoucher').DataTable().ajax.reload();
        $('#journalVoucher').modal('hide');
    });

    $('#journalVoucher').on('hidden.bs.modal', function () {
        $('#tbJournalVoucher').DataTable().ajax.reload();
    })

    $('#tbJournalVoucher').on('click', '.tbEdit', function () {
        clear();
        buttonSet(true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbJournalVoucher").dataTable().fnGetData(tr);

        var url = "/JournalVoucher/findData?id=" + data.transactionId + "&date=" + (moment(data.voucherDate).format('YYYY-MM-DD'));
        x = 0;
        tableClear(1);
        if (isData(url)) {
            findWork(url);
            $("#journalVoucher").modal('show');
            attachmentLoad();
        }
        else {
            errorNotify("Data not found");
        }
    });
    $("#btnAddRow").click(function () {
        x = x + 1;
        addRow(x);
        ledger("#ledgerId"+x);
    });
    $("#debitHead").change(function () {
        //budgetBalanceSet($(this).val(), "#balance");
        $(".ledgerselect").focus();
    });

    $("input[name=type]").change(function () {
        debitCreditOption($(this).val());
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
    var voucherType = "JV";
    var findType = "Voucher";
    var fromDate = getBdToDbFormat(today);
    var toDate = getBdToDbFormat(today);
    var url = "/Report/VoucherAll?vType=" + voucherType + "&findType=" +
        findType + "&fromDate=" + fromDate + "&toDate=" + toDate + "&voucherNo=" + voucherNo;
    //console.log(url);
    if (voucherNo != "" && voucherNo != null && voucherNo != undefined) {
        window.open(url, "_blank");
    }
    else {
        warningNotify("Unable to preview");
    }
}
function debitCreditOption(val) {
    var element = document.querySelectorAll("#gridJournalVoucher th");
    if (val == "Debit") {
        element[2].textContent = "Credit Amount";
        document.getElementById('drHead').innerHTML = "Dr. Head:";
    } else {
        element[2].textContent = "Debit Amount";
        document.getElementById('drHead').innerHTML = "Cr. Head:";
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
            //console.log("Find:" + res.isFind);
        }
    });
    return ret;
}
function init() {
    clear();
}
function tableClear(start) {
    var table = document.getElementById('gridJournalVoucher');
    var rowCount = table.rows.length;
    for (var i = start; i < rowCount; i++) {
        table.deleteRow(start);
    }
    $("#gridJournalVoucher tr td select").val("0").trigger('change');
    $(".debitAmount").val("0");
    //$(".budget").text("");
    //$(".balance").text("");
}
function findWork(url) {
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            var d = res.data;
            //console.log(d);
            $("#upload").val(d[0].attachment);

            
            for (var i = 0; i < d.length; i++) {
                if (d[i].jvType == "Debit") {
                    if (parseFloat(d[i].drAmount) > 0) {
                        //$("#debitHead").val(d[i].ledgerUniqueId).trigger('change');
                        LedgerInfoDrCr("#debitHead", d[i].ledgerUniqueId);
                       // $("#date").val(d[i].voucherDate);


                        console.log("Voucher Date: ", d[i].voucherDate);

                        // Format the date if necessary, depending on the format you need
                        var formattedDate = new Date(d[i].voucherDate).toISOString().split('T')[0];  // Adjust format as needed
                        $("#date").val(formattedDate);

                        $("#voucherNo").val(d[i].voucherNo);
                        $("#transactionId").val(d[i].transactionId);
                        $("#autoId").val(d[i].id);
                        $("#description").val(d[i].narration);
                        //budgetBalanceSet(d[i].ledgerUniqueId, "#balance");
                    }
                    else {
                        x = x + 1;
                        addRow(x);
                        ledger("#ledgerId" + x, d[i].ledgerUniqueId);
                        //budgetBalanceSet(d[i].ledgerUniqueId, "#lbBalance" + x);
                        $("#debitAmount" + x).val(amountShowWithComma(d[i].crAmount));
                    }
                }
                else {
                    if (parseFloat(d[i].crAmount) > 0) {
                        LedgerInfoDrCr("#debitHead", d[i].ledgerUniqueId);
                        //$("#date").val(d[i].voucherDate);

                        console.log("Voucher Date: ", d[i].voucherDate);

                        // Format the date if necessary, depending on the format you need
                        var formattedDate = new Date(d[i].voucherDate).toISOString().split('T')[0];  // Adjust format as needed
                        $("#date").val(formattedDate);

                        $("#voucherNo").val(d[i].voucherNo);
                        $("#transactionId").val(d[i].transactionId);
                        $("#autoId").val(d[i].id);
                        $("#description").val(d[i].narration);
                        //budgetBalanceSet(d[i].ledgerUniqueId, "#balance");
                    }
                    else {
                        x = x + 1;
                        addRow(x);
                        ledger("#ledgerId" + x, d[i].ledgerUniqueId);
                        //budgetBalanceSet(d[i].ledgerUniqueId, "#lbBalance" + x);
                        $("#debitAmount" + x).val(amountShowWithComma(d[i].drAmount));
                    }
                }
            }
            if (d[0].jvType == "Debit") {
                document.getElementById('debit').checked = true;
            }
            else {
                document.getElementById('credit').checked = true;
            }
            debitCreditOption(d[0].jvType);
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
    //var Day = getCDay();
    //var Month = getCMonth();
    //var Year = getCYear();
    //var Date = Day + "-" + Month + "-" + Year

    var today = new Date();
    var formattedDate = today.toISOString().split('T')[0]; // Format the date as YYYY-MM-DD

    $("#date").val(formattedDate);
    //$("#balance").val("");
    $("#voucherNo").val("");
    $("#description").val("");
    $("#debitHead").select2('data', { id: "0", text: "Choose a Dr/Cr Head" });
    localStorage.clear();
    LedgerInfoDrCr("#debitHead");
    tableClear(1);
    $('#date').val(formattedDate);
    x = x + 1;
    addRow(x);
    ledger("#ledgerId" + x);
    autocompleteText();
}
function newclear() {
    clearAttachment();
    //$("#balance").val("");
    $("#voucherNo").val("");
    $("#description").val("");
    $("#debitHead").select2('data', { id: "0", text: "Choose a Dr/Cr Head" });
    localStorage.clear();
    LedgerInfoDrCr("#debitHead");
    tableClear(1);
    x = x + 1;
    addRow(x);
    ledger("#ledgerId" + x);
    autocompleteText();
}

function checkValidation() {
    var bankHead = $("#debitHead").val();
    var date = $("#date").val();

    var tableHead = "#gridJournalVoucher tr td ";
    var elements = document.querySelectorAll(tableHead + "select");
    var amt = document.querySelectorAll(tableHead + ".debitAmount");

    var count = 0,headCount=0;
    for (var i = 0; i < elements.length; i++) {
        var ledger = elements[i].getAttribute('id');
        var amount = amt[i].getAttribute('id');

        var lId = document.getElementById(ledger).value;
        var amtId = document.getElementById(amount).value;
        amtId = amtId.replace(/,/g, "");
        if (lId != "0" && parseFloat(amtId)>0) {
            count++;
        }
        if (lId == bankHead) {
            headCount++;
        }
    }
    if (bankHead != "0" && bankHead !="") {
        if (date != "") {
            if (count > 0) {
                if (headCount===0) {
                    return true;
                }
                else {
                    warningNotify("Same head is not allowed!");
                    return false;
                }
            }
            else {
                warningNotify("Please entire at least one ledger name and debit amount");
                return false;
            }
        }
        else {
            warningNotify("Please entire date");
            return false;
        }
    }
    else {
        warningNotify("Please entire dr.head/cr.head");
        return false;
    }
}
function budgetBalanceSet(id,balanceId) {
    var url = "/JournalVoucher/LedgerBudgetBalance";
    var date = $("#date").val();
    if (date == undefined) {
        date = new Date();
    }
    
    $.ajax({
        url: url,
        data: {id:id,date:date},
        async: false,
        success: function (res) {
            console.log(JSON.stringify(res));
            if (balanceId.substring(1, 3) == "lb") {
                $(balanceId).text(amountShowWithComma(res.balance));
            }
            else {
                $(balanceId).val(amountShowWithComma(res.balance));
            }
        }
    });
}
function addRow(x) {
    var tableArray = new Array();
    tableArray = ['Ledger Code Name','Balance', 'Debit Amount',''];
    var gridJournalVoucher = document.getElementById('gridJournalVoucher');
    gridJournalVoucher.style.backgroundColor = "green";
    var rowCount = gridJournalVoucher.rows.length;    
    var tr = gridJournalVoucher.insertRow(rowCount); 
    tr = gridJournalVoucher.insertRow(rowCount);
    
    
    for (var c = 0; c < tableArray.length; c++) {
        var td = document.createElement('td');
        
        td = tr.insertCell(0);
        if (parseInt(x) % 2 == 0) {
            td.setAttribute('style', 'background-color:#fff;color:#000');
        }
        else {
            td.setAttribute('style', 'background-color:#eee;color:#000');
        }
        if (c == 3) {

            var select = document.createElement('select');
            
            select.setAttribute('class', 'form-control input-sm account-head');
           // select.style.width = '200px';
            select.setAttribute('id', 'ledgerId' + x);
            select.setAttribute('onchange', 'ledgerAction('+x+')');
            td.appendChild(select);
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
            //ele.setAttribute('onchange', 'addRowByAmount("#debitAmount"+'+x+', "#ledgerId" + '+x+')');
            ele.setAttribute('onchange', '$("#description").focus()');
            td.appendChild(ele);
        }
        if (c == 0) {
            var button = document.createElement('input');
            button.setAttribute('type', 'button');
            button.setAttribute('value', 'Remove');
            button.setAttribute('class', 'btn btn-secondary btn-sm');
            button.setAttribute('onclick', 'removeRow(this)');
            td.appendChild(button);
        }
        //$("#ledgerId" + x).select2();
        //$("#ledgerId" + x).focus();
       

    }
}
function ledgerAction(a) {
    var ledger = $("#ledgerId" + a).val();
    $("#debitAmount" + a).focus();
    //budgetBalanceSet(ledger, "#lbBalance"+a);

    var counts = 0;
    var elements = document.querySelectorAll("#gridJournalVoucher tr td select");
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
       // $("#lbBalance" + a).text("");
        $("#ledgerId" + a).focus();
    }
}
function addRowByAmount(debitAmount, ledgerId) {
    var elements = document.querySelectorAll("#gridJournalVoucher tr td select");
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
    var tableId = "gridJournalVoucher";
    var empTab = document.getElementById(tableId);
    var elements = document.querySelectorAll("#" + tableId + " tr td select");
    if (elements.length > 1) {
        empTab.deleteRow(oButton.parentNode.parentNode.rowIndex); 
    }
}

function submit(isNew, isEdit) {
    localStorage.clear();
    var time = new Date();
    time = (moment(time).format('HH:mm:ss'));
    var id = 0;
    //var date = getBdToDbFormat($("#date").val());
    var date = $("#date").val();
    var transactionId = "";
    var voucherNo = "";
    if (isEdit) {
        id = $("#autoId").val();
        transactionId = $("#transactionId").val();
        voucherNo = $("#voucherNo").val();
    } else {
        transactionId = getTransactionId(date);
        voucherNo = getVoucherNo(date);
    }

    var bankHead = $("#debitHead").val();
    var bankHeadId = getLedgerId(bankHead);
    var bankHeadName = $("#debitHead option:selected").text();

    var narration = $("#description").val();
    var voucherType = $("input[name=type]:checked").val();

    var tableHead = "#gridJournalVoucher tr td ";
    var elements = document.querySelectorAll(tableHead+"select");
    var amt = document.querySelectorAll(tableHead + ".debitAmount");

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
        bankHeadId: bankHeadId,
        bankHeadName: bankHeadName,
        voucherType: voucherType,
        objList: []
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
                "AutoId":id,"TransactionId":transactionId,"LedgerId": ledger, "LedgerName": ledgerName, "DrAmount": amtId,
                "VoucherDate": date + " " + time, "VoucherNo": voucherNo, "Narration": narration, "AttachBill": uploadFile
            });
        }
    }
    formData.append("bankHeadId", data.bankHeadId);
    formData.append("bankHeadName", data.bankHeadName);
    formData.append("voucherType", data.voucherType);

    for (var i = 0; i < data.objList.length; i++) {
        formData.append("objList[" + i + "].autoId", data.objList[i].AutoId);
        formData.append("objList[" + i + "].transactionId", data.objList[i].TransactionId);
        formData.append("objList[" + i + "].ledgerId", data.objList[i].LedgerId);
        formData.append("objList[" + i + "].ledgerName", data.objList[i].LedgerName);
        formData.append("objList[" + i + "].drAmount", data.objList[i].DrAmount);
        formData.append("objList[" + i + "].voucherDate", data.objList[i].VoucherDate);
        formData.append("objList[" + i + "].voucherNo", data.objList[i].VoucherNo);
        formData.append("objList[" + i + "].narration", data.objList[i].Narration);
        formData.append("objList[" + i + "].attachBill", data.objList[i].AttachBill);
    }
    $.ajax({
        url: "/JournalVoucher/journalVoucherSave",
        data: formData,
        type: 'POST',
        processData: false,
        contentType: false,
        async: false,
        success: function (res) {
            if (res.success) {
                successNotify(res.message);
                localStorage.setItem("voucher", voucherNo);
               
                if (!isNew || isEdit) {
                    clear();
                    $("#journalVoucher").modal('hide');
                } else {
                    newclear();
                }
                $('#tbJournalVoucher').DataTable().ajax.reload();
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
    var url = "/JournalVoucher/GetLedgerId";
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
function getVoucherNo(date) {
    var ret = "";
    var url = "/JournalVoucher/GetVoucherNo";
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
function getTransactionId(date) {
    var ret = "";
    var url = "/JournalVoucher/GetTransactionId";
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
function ledger(ledgerId,setValue) {
    var url = "/JournalVoucher/LedgerInfo";
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
function LedgerInfoDrCr(ledgerId, setValue) {
    var url = "/JournalVoucher/LedgerInfoDrCr";
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
           // console.log(data.data)
            ret = data.data;
            autocomplete(document.getElementById("description"), ret);
        });
    } catch (e) { console.log(e) }
}