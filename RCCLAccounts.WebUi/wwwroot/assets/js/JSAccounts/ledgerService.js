
$(document).ready(function () {
    primaryGroupData();
    mainGroupData("%");
    subGroupData("%", "%");
    $("#primaryGroupId").change(function () {
        //var primaryData = $('#primaryGroupId').select2('data');
        var primaryData = $('#primaryGroupId').val();
        mainGroupData(primaryData);
        path_Id_Code();
    });
    $("#mainGroupId").change(function () {
        //var primaryData = $('#primaryGroupId').select2('data');
        var primaryData = $('#primaryGroupId').val();
       // var mainData = $('#mainGroupId').select2('data');
        var mainData = $('#mainGroupId').val();
        subGroupData(primaryData, mainData);
        path_Id_Code();
    });
    $("#subGroupId").change(function () {
        path_Id_Code();
    });
    /*$("#nameId").change(function () {
        if (nameCheck($(this).val())) {
            $("#nameId").val("");
            $("#nameId").focus();
            warningNotify("Ledger name already taken! try new one.");
        }
        else {
        }
    });*/
    $("#debit").keyup(function () {
        $("#credit").val('0');
    });
    $("#credit").keyup(function () {
        $("#debit").val('0');
    });
    $("#btnAdd").click(function () {
        clear();
        init();
        groupPathAction(true);
        buttonSet(false);
        $("#ledger").modal('show');
    });
    $('#tbLedger').on('click', '.tbEdit', function () {
        groupPathAction(false);
        buttonSet(true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbLedger").dataTable().fnGetData(tr);
        if (isData(data.id)) {
            $("#primaryGroupId").prop('disabled', true);
            $("#mainGroupId").prop('disabled', true);
            $("#subGroupId").prop('disabled', true);
            findWork(data.id);
            $("#ledger").modal('show');
        }
        else {
            errorNotify("Data not found");
        }
    });
    $("#btnSave").click(function () {
        if (checkValidation()) {
            saveWork(false, false);
        }
    });
    $("#btnEdit").click(function () {
        if (checkValidation()) {
            saveWork(false, true);
        }
    });
    $("#btnSaveNew").click(function () {
        if (checkValidation()) {
            saveWork(true, false);
        }
    });

    //$("#btnCancel").click(function () {
    //    $('#tbLedger').DataTable().ajax.reload();
    //});
    $(document).on('click', '#btnCancel', function () {
        $('#tbLedger').DataTable().ajax.reload();
        $('#ledger').modal('hide');
    });
    $("#ledger").on('hidden.bs.model', function () {
        $("tbLedger").DataTable().ajax.reload();
    });
});
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
function path_Id_Code() {
    var pri = "", path = "", primary = "", main = "", sub = "", name = $("#nameId").val();
   // var primaryData = $('#primaryGroupId').select2('data');
    var primaryData = $("#primaryGroupId").val();
    //var mainData = $('#mainGroupId').select2('data');

    var mainData = $('#mainGroupId').val();
  //  var subData = $('#subGroupId').select2('data');
    var subData = $('#subGroupId').val();

  //  var lId = getMaxId(primaryData.id);
    var lId = getMaxId(primaryData);

    if (primaryData != "") {
        primary = $('#primaryGroupId').find("option:selected").text();
    }
    if (mainData!= "") {
        main = $('#mainGroupId').find("option:selected").text();
    }
    if (subData != "") {
        sub = $('#subGroupId').find("option:selected").text();
    }
    if (name == undefined) {
        name = "";
    }
    var code = maxCode(primaryData, mainData, subData);
    var str = primaryData.substring(0, 1);
    pri = getType(str);

    path = pri + " / " + primary + " / " + main + " / " + sub + " / " + name;

    console.log("Id:" + lId);

    $("#ledgerId").val(lId);
    $("#ledgerCode").val(code);
    $("#ledgerPath").val(path);
}
function getPrimaryGroupId(id) {
    var url = "/Ledger/getPrimaryGroupId?id=" + id;
    var result = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    return result;
}
function getMainGroupId(id) {
    var url = "/Ledger/getMainGroupId?id=" + id;
    var result = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    return result;
}
function getSubGroupId(id) {
    var url = "/Ledger/getSubGroupId?id=" + id;
    var result = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    return result;
}
function getMaxId(id) {
    var url = "/Ledger/getMaxId?pId=" + id;
    var result = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    return result;
}
function nameCheck(name) {
    var url = "/MainGroup/nameCheck?name=" + name;
    var result = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.isFind;
        }
    });
    return result;
}
function getType(str) {
    var pri = "";
    if (str == "A") {
        pri = "Asset";
    }
    else if (str == "L") {
        pri = "Liability";
    }
    else if (str == "I") {
        pri = "Income";
    }
    else if (str == "E") {
        pri = "Expense";
    }
    return pri;
}
function init() {
    primaryGroupData();
    mainGroupData("%");
    subGroupData("%", "%");
/*    var opdate = getCDay() + '-' + getCMonth() + '-' + getCYear();*/
    $.ajax({
        url: "/Ledger/GetFiscaleYearDate",
        async: false,
        success: function (res) {
            // console.log(res.opening);
            opdate = res.opening;
        }
    });
    //$("#openingDate").val(opdate);
    $("#openingDate").val(new Date());
}
function maxCode(pId, mId, sId) {
    var url = "/Ledger/getMaxCode?pId=" + pId + "&mId=" + mId + "&sId=" + sId;
    var result = "";
    console.log("Url:" + url);
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    console.log("Result : " + result);
    return result;
}
function maxId() {
    var url = "/Ledger/getMaxId";
    console.log("Url : " + url);

    var result = "";
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    return result;
}
function groupPathAction(isNew) {
    if (isNew) {
        $("#mainGroupPath").mouseover(function () {
            $("#mainGroupPath").prop('readonly', true);
        });
        $("#mainGroupPath").mouseout(function () {
            $("#mainGroupPath").prop('readonly', true);
        });
    }
    else {
        $("#mainGroupPath").mouseover(function () {
            $("#mainGroupPath").prop('readonly', false);
        });
        $("#mainGroupPath").mouseout(function () {
            $("#mainGroupPath").prop('readonly', true);
        });
    }
}
function findWork(id) {
    if (id != "0" && id != "") {
        $.ajax({
            url: "/Ledger/findData",
            data: { id: id },
            async: false,
            success: function (res) {
                if (res.isFind) {
                    if (!isEmpty(res.data)) {
                        var jsonData = res.data

                        var pgId = res.data.pId;
                        var pgCaption = res.data.primaryData;
                        var mgId = res.data.mId;
                        var mgCaption = res.data.mainData;
                        var sgId = res.data.sId;
                        var sgCaption = res.data.subData;
                        console.log(pgId);
                        console.log(pgCaption);
                        $("#autoId").val(res.data.autoId);
                        //$('#primaryGroupId').select2('data', { id: pgId, text: pgCaption });
                        //$('#mainGroupId').select2('data', { id: mgId, text: mgCaption });
                        //$('#subGroupId').select2('data', { id: sgId, text: sgCaption });

                        $('#primaryGroupId').val(pgId).trigger('change');
                        $('#mainGroupId').val(mgId).trigger('change');
                        $('#subGroupId').val(sgId).trigger('change');

                        $("#ledgerId").val(res.data.ledgerId);
                        $("#ledgerCode").val(res.data.ledgerCode);
                        $("#ledgerName").val(res.data.ledgerName);
                        $("#debit").val(res.data.drAmount);
                        $("#credit").val(res.data.crAmount);
                        $("#openingDate").val(res.data.openingDate);

                        $("#openingId").val(res.data.openingId);
                        $("#companyId").val(res.data.companyId);
                        $("#branchId").val(res.data.branchId);
                        $("input[name=ledgerType][value=" + res.data.ledgerType + "]").prop('checked', true);

                        var str = pgId.substring(0, 1);
                        var path = getType(str) + " / " + pgCaption + " / " + mgCaption + " / " + sgCaption + " / " + res.data.ledgerName;

                        $("#ledgerPath").val(path);
                        if (res.data.active == 1) {
                            $('#chkActive').prop('checked', true);
                        }
                        else {
                            $('#chkActive').prop('checked', false);
                        }
                    }
                }
                else {
                    errorNotify("Data not found!");
                }
            }
        });
    }
}
function isData(id) {
    var ret = false;
    $.ajax({
        url: "/Ledger/findData",
        data: { id: id },
        async: false,
        success: function (res) {
            ret = res.isFind;
        }
    });
    return ret;
}
function saveWork(isNew, isEdit) {
    var companyId = "B-1";//$("#companyId").val();
    var branchId = $("#branchId").val();
    var parentId = "", createForm = "";
    var id = $("#autoId").val();
    var ledgerId = $("#ledgerId").val();
    var openingId = $("#openingId").val();
    var ledgerCode = $("#ledgerCode").val();
    var name = $("#ledgerName").val();
    var debit = $("#debit").val();
    var credit = $("#credit").val();
   // var active = $('#chkActive').prop('checked');
    var active = $('#chkActive').prop('checked') ? 1 : 0;
    var openingDate = $("#openingDate").val();

   // var dbOpening = getBdToDbFormat(openingDate);

    //var primaryData = $('#primaryGroupId').select2('data');
    var primaryData = $('#primaryGroupId').val();
    //var mainData = $('#mainGroupId').select2('data
    var mainData = $('#mainGroupId').val();
    //var subData = $('#subGroupId').select2('data');
    var subData = $('#subGroupId').val();

    var str = primaryData.substring(0, 1);

    var type = getType(str);

    if (primaryData != "") {
        parentId = primaryData;
        createForm = parentId;
    }
    if (mainData != "") {
        parentId = primaryData;
        createForm = parentId + "-" + mainData;
    }
    if (subData.id != "") {
        parentId = primaryData;
        createForm = parentId + "-" + mainData + "-" + subData;
    }
    console.log("Parent:" + parentId + " create form:" + createForm);

    var ledgerType = $("input[name='ledgerType']:checked").val();


    if (!isEdit) {
        ledgerId = getMaxId(primaryData);
        ledgerCode = maxCode(primaryData, mainData, subData);
    }
    //     , Type: type,  Type: type,    , BranchId: branchId
    var jsonData = {
        AutoId: id, OpeningDate: openingDate,  PrimaryGroupId: primaryData, MainGroupId: mainData, SubGroupId: subData,
        LedgerId: ledgerId, LedgerCode: ledgerCode, LedgerName: name, ParentId: parentId, CreateFrom: createForm, LedgerType: ledgerType,
        Active: active, EntryFrom: 'Ledger Information' ,CompanyId: companyId
    }

    $.ajax({
        type: "POST",
        url: "/Ledger/ledgerSave",
        data: { obj: jsonData, debit: debit, credit: credit, openingId: openingId },
        async: false,
        success: function (res) {
            if (res.success) {
                clear();
                init();
                if (!isNew) {
                    $("#ledger").modal('hide');
                }
                $("#ledgerName").focus();
                $('#tbLedger').DataTable().ajax.reload();
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}
function clear() {
    $("#primaryGroupId").prop('disabled', false);
    $("#mainGroupId").prop('disabled', false);
    $("#subGroupId").prop('disabled', false);
 /*   var today = getCDay() + '-' + getCMonth() + '-' + getCYear();*/
    $("#companyId").val("0");
    $("#branchId").val("");
    $("#autoId").val(0);
    $("#ledgerId").val("");
    $("#ledgerCode").val("");
    $("#ledgerName").val("");
    $("#ledgerPath").val("");
    $("#debit").val("0");
    $("#credit").val("0");
    $("#openingId").val("");
    //$("#openingDate").val(today);
    //document.getElementById("LastTrDate").valueAsDate = new Date();
  //  $("#openingDate").val(new Date());
    document.getElementById("openingDate").valueAsDate = new Date();
    $('#chkActive').prop('checked', true);
    $('#primaryGroupId').select2('data', { id: '', text: 'Choose a Primary Group' });
    $('#mainGroupId').select2('data', { id: '', text: 'Choose a Main Group' });
    $('#subGroupId').select2('data', { id: '', text: 'Choose a Sub Group' });
}
function checkValidation() {
    if ($("#primaryGroupId").val() != "" && $("#primaryGroupId").val() != "0") {
        if (isMainGroup($("#primaryGroupId").val())) {
            if ($("#mainGroupId").val() != "" && $("#mainGroupId").val() != "0") {
                if (isSubGroup($("#primaryGroupId").val(), $("#mainGroupId").val())) {
                    if ($("#subGroupId").val() != "" && $("#subGroupId").val() != "0") {
                        if (checkSubValidation()) {
                            return true;
                        }
                    }
                    else {
                        warningNotify("Please give sub group ");
                        return false;
                    }
                }
                else {
                    if (checkSubValidation()) {
                        return true;
                    }
                }
            }
            else {
                warningNotify("Please give main group ");
                return false;
            }
        }
        else {
            if (checkSubValidation()) {
                return true;
            }
        }
    }
    else {
        warningNotify("Please give primary group ");
        return false;
    }
}
function checkSubValidation() {
    if ($("#ledgerCode").val() != "") {
        if ($("#ledgerName").val() != "") {
            if (parseFloat($("#debit").val()) >= 0 || parseFloat($("#credit").val()) >= 0) {
                if ($("input[name='ledgerType']:checked").val() != '') {
                    if ($("#openingDate").val() != '') {
                        return true;
                    }
                    else {
                        warningNotify("Please give opening date");
                        return false;
                    }
                }
                else {
                    warningNotify("Please give ledger type");
                    return false;
                }
            }
            else {
                warningNotify("Please give debit/credit ");
                return false;
            }
        }
        else {
            warningNotify("Please give name ");
            return false;
        }
    }
    else {
        warningNotify("Please primary code ");
        return false;
    }
}
function isMainGroup(primaryId) {
    var url = "/Ledger/isMainGroup";
    var ret = false;
    $.ajax({
        url: url,
        data: { id: primaryId },
        async: false,
        success: function (res) {
            if (res.isFind) {
                ret = true;
            }
        }
    });
    return ret;
}
function isSubGroup(primaryId, mainId) {
    var url = "/Ledger/isSubGroup";
    var ret = false;
    $.ajax({
        url: url,
        data: { pId: primaryId, mId: mainId },
        async: false,
        success: function (res) {
            if (res.isFind) {
                ret = true;
            }
        }
    });
    return ret;
}
function primaryGroupData() {
    var url = "/Ledger/getPrimaryGroup";
    //$('#primaryGroupId').select2('data', { id: '', text: 'Choose a Primary Group' });
    if ($("#autoId").val() == 0) {
        $.getJSON(url, function (data) {
            var item = "";
            $("#primaryGroupId").empty();
            item += '<option value="">Choose a Primary Group</option>'
            $("#primaryGroupId").html(item);
            $.each(data, function (i, opt) {
                item += '<option value="' + opt.value + '">' + opt.text + '</option>'
            });
            $("#primaryGroupId").html(item);
        });
    }
}
function mainGroupData(pId) {
    var url = "/Ledger/getMainGroup?pId=" + pId;
    //$('#mainGroupId').select2('data', { id: '', text: 'Choose a Main Group' });
    //$('#subGroupId').select2('data', { id: '', text: 'Choose a Sub Group' });

    if ($("#autoId").val() == 0) {
        $.getJSON(url, function (data) {
            var item = "";
            $("#mainGroupId").empty();
            item += '<option value="">Choose a Main Group</option>'
            $("#mainGroupId").html(item);
            $.each(data, function (i, opt) {
                item += '<option value="' + opt.value + '">' + opt.text + '</option>'
            });
            $("#mainGroupId").html(item);
        });
    }
}
function subGroupData(pId, mId) {
    var url = "/Ledger/getSubGroup?pId=" + pId + "&mId=" + mId;
    //$('#subGroupId').select2('data', { id: '', text: 'Choose a Sub Group' });
    if ($("#autoId").val() == 0) {
        $.getJSON(url, function (data) {
            var item = "";
            $("#subGroupId").empty();
            item += '<option value="">Choose a Sub Group</option>'
            $("#subGroupId").html(item);
            $.each(data, function (i, opt) {
                item += '<option value="' + opt.value + '">' + opt.text + '</option>'
            });
            $("#subGroupId").html(item);
        });
    }
}