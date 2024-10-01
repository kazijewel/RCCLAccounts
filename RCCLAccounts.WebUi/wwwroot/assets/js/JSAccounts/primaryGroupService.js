$(document).ready(function () {
    assetSet();
    $('input[type=radio][name=assetName]').change(function () {
        if ($(this).val() == "Asset") {
            assetSet();
        }
        if ($(this).val() == "Liability") {
            liabilitySet();
        }
        if ($(this).val() == "Income") {
            incomeSet();
        }
        if ($(this).val() == "Expense") {
            expenseSet();
        }
    });
   /* $("#nameId").change(function () {
        if (nameCheck($(this).val())) {
            $("#nameId").val("");
            $("#nameId").focus();
            warningNotify("Primary group name already taken! try new one.");
        }
        else {
        }
    });*/
});
function maxId(id) {
    var url = "/PrimaryGroup/getMax?id=" + id;
    var result = "";
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
function nameCheck(name) {
    var url = "/PrimaryGroup/nameCheck?name=" + name;
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
function maxPCode(id, isCode) {
    var url = "/PrimaryGroup/getMaxPrimaryCode";
    console.log("Console , Url : " + url);

    var result = "";
    var data = { group: id, code: isCode };
    $.ajax({
        url: url,
        data: data,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    return result;
}
function liabilitySet() {
    var id2 = $("#radio2").val();
    var maxL = maxId(id2.substring(0, 1));
    $("#primaryGroupId").val(maxL);

    var code = "100";
    var isCode = maxPCode(id2.substring(0, 1), code);
    $("#PCode").val(isCode);
}
function expenseSet() {
    var id4 = $("#radio4").val();
    var maxL = maxId(id4.substring(0, 1));
    $("#primaryGroupId").val(maxL);

    var code = "300";
    var isCode = maxPCode(id4.substring(0, 1), code);
    $("#PCode").val(isCode);

}
function assetSet() {
    var id = $("#radio1").val();
    var maxL = maxId(id.substring(0, 1));
    $("#primaryGroupId").val(maxL);
    var code = "200";
    var isCode = maxPCode(id.substring(0, 1), code);
    $("#PCode").val(isCode);
}
function incomeSet() {
    var id3 = $("#radio3").val();
    var maxL = maxId(id3.substring(0, 1));
    $("#primaryGroupId").val(maxL);

    var code = "400";
    var isCode = maxPCode(id3.substring(0, 1), code);
    $("#PCode").val(isCode);
}
$(document).ready(function () {
    $("#btnAdd").click(function () {
        clear();
        $('input[name=assetName]').attr("disabled", false);
        document.getElementById('add').style.display = "block";
        document.getElementById('edit').style.display = "none";
        $("#primaryGroup").modal('show');
    });
    $('#tbPrimaryGroup').on('click', '.tbEdit', function () {
        document.getElementById('add').style.display = "none";
        document.getElementById('edit').style.display = "block";
        $('input[name=assetName]').attr("disabled", true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbPrimaryGroup").dataTable().fnGetData(tr);
        
        if (isData(data.id)) {
            findWork(data.id);
            $("#primaryGroup").modal('show');
        }
        else {
            errorNotify("Data not found");
        }
    });
    $("#btnSave").click(function () {
        if (checkValidation()) {
            saveWork(false,false);
        }
    });
    $("#btnEdit").click(function () {
        if (checkValidation()) {
            saveWork(false,true);
        }
    });
    $("#btnSaveNew").click(function () {
        if (checkValidation()) {
            saveWork(true,false);
        }
    });
    $("#btnCancel").click(function () {
        $('#tbPrimaryGroup').DataTable().ajax.reload();
        $('#primaryGroup').modal('hide');
    });
    $("#primaryGroup").on('hidden.bs.model', function () {
        $('#tbPrimaryGroup').DataTable().ajax.reload();
    });
});
function findWork(id) {
    if (id != "0" && id != "") {
        $.ajax({
            url: "/PrimaryGroup/findData",
            data: { id: id },
            async: false,
            success: function (res) {
                if (res.isFind) {
                    if (!isEmpty(res.data)) {
                        $("#autoId").val(res.data.id);
                        $('#itemTypeId').select2('data', { id: res.data.groupType, text: res.data.groupType });
                        $("#PCode").val(res.data.code);
                        $("#nameId").val(res.data.name);
                        $("#noteNoId").val(res.data.noteNo);
                        $("#primaryGroupId").val(res.data.primaryGroupId);
                        $("input[name=assetName][value=" + res.data.type + "]").prop('checked', true);
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
        url: "/PrimaryGroup/findData",
        data: { id: id },
        async: false,
        success: function (res) {
            ret = res.isFind;
        }
    });
    return ret;
}
function saveWork(isNew,isEdit) {
    var id = $("#autoId").val();
    var groupType = $("#itemTypeId").val();
    var code = $("#PCode").val();
    var name = $("#nameId").val();
    var noteNo = $("#noteNoId").val();
    var primaryGroupId = $("#primaryGroupId").val();
    var type = $("input[name='assetName']:checked").val();

    if (!isEdit) {
        primaryGroupId = maxId(type.substring(0, 1));
        if (type == "Asset") {
            code = maxPCode(type.substring(0, 1), "200");
        }
        else if (type == "Liability") {
            code = maxPCode(type.substring(0, 1), "100");
        }
        else if (type == "Income") {
            code = maxPCode(type.substring(0, 1), "400");
        }
        else if (type == "Expense") {
            code = maxPCode(type.substring(0, 1), "300");
        }
    }

    var jsonData = {
        PrimaryId: id, ItemOf: type, PrimaryGroupCode: code, PrimaryGroupName: name,
        GroupName: groupType, NoteNo: noteNo, PrimaryGroupId: primaryGroupId
    }
    $.ajax({
        type:"POST",
        url: "/PrimaryGroup/primarySave",
        data: jsonData,
        async: false,
        success: function (res) {
            if (res.success) {
                clear();
                if (!isNew) {
                    $("#primaryGroup").modal('hide');
                }
                $("#nameId").focus();
                $('#tbPrimaryGroup').DataTable().ajax.reload();
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }

    });
}
function clear() {
    $("#autoId").val(0);
    $("#itemTypeId").val("Current Assets");
    $("#PCode").val("");
    $("#nameId").val("");
    $("#noteNoId").val("");
    $("#primaryGroupId").val("");
    $("input[name=assetName][value='Asset']").prop("checked", true);
    $('#itemTypeId').select2('data', { id: 'Current Assets', text: 'Current Assets' });
    assetSet();
}
function checkValidation() {
    if ($("#primaryGroupId").val()!="") {
        if ($("#PCode").val() != "") {
            if ($("#nameId").val() != "") {
                if ($("#itemTypeId").val() != "") {
                    return true;
                }
                else {
                    warningNotify("Please give item type ");
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
    else {
       
        warningNotify("Please primary group id ");
        return false;
    }
}