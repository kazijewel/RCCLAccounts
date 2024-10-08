$(document).ready(function () {
    primaryGroupData();
    mainGroupData("%");
    $("#btnAdd").click(function () {
        clear();
        init();
        groupPathAction(true);
        document.getElementById('add').style.display = "block";
        document.getElementById('edit').style.display = "none";
        $("#subGroup").modal('show');
    });
    $('#tbSubGroup').on('click', '.tbEdit', function () {
        groupPathAction(false);
        document.getElementById('add').style.display = "none";
        document.getElementById('edit').style.display = "block";
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbSubGroup").dataTable().fnGetData(tr);

        if (isData(data.id)) {
           //$("#primaryGroupId").prop('disabled', true);
            //$("#mainGroupId").prop('disabled', true);
            findWork(data.id);
            $("#subGroup").modal('show');
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
    //    $("tbSubGroup").DataTable().ajax.reload();
    //});

    $(document).on('click', '#btnCancel', function () {
        $('#tbSubGroup').DataTable().ajax.reload();
        $('#subGroup').modal('hide');
    });
    $("#subGroup").on('hidden.bs.model', function () {
        $("tbSubGroup").DataTable().ajax.reload();
    });

    $("#primaryGroupId").change(function () {
        pathSet();
       // var data = $('#primaryGroupId').select2('data');
        var data = $('#primaryGroupId').val();
        mainGroupData(data);
        $('#subGroupCode').val(setCode());
    });
    $("#mainGroupId").change(function () {
        pathSet();
        $('#subGroupCode').val(setCode());
    });
    /*$("#nameId").change(function () {
        if (nameCheck($(this).val())) {
            $("#nameId").val("");
            $("#nameId").focus();
            warningNotify("Sub group name already taken! try new one.");
        }
        else {
        }
    });*/

});
function getAsset(id) {
    var url = "/SubGroup/getPrimaryGroupId?id=" + id;
    var result = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.itemOf;
        }
    });
    return result;
}
function setCode() {
    //var data = $('#primaryGroupId').select2('data');
    var data = $("#primaryGroupId").val();
    console.log("dd "+data);
   // var mainData = $('#mainGroupId').select2('data');
    var mainData = $('#mainGroupId').val();
    var subGroupId = maxId();
    var sId = subGroupId.substring(0, 2);

    var type = getAsset(data);


    var Code = "";
    if (
        (data != "" && data!= "0")
        && (mainData!= "" && mainData!= "0")
        && (subGroupId != "" && subGroupId != "0")
    ) {
        Code = codeWork(type.substring(0, 1), sId, type);
    }
    return Code;
}
function codeWork(subtype, sId, type) {

    var code = (subtype == 'L' ? '100' : (subtype == 'A' ? '200' : (subtype == 'E' ? '300' : (subtype == 'I' ? '400' : ''))));
    console.log("sId= " + sId + " code= " + code + " type= " + type)
    var isCode = maxCode(sId, code, type);
    return isCode;
}
function pathSet() {

    var path = "";
    var primary = "";
    var main = "";

    //var data = $('#primaryGroupId').select2('data');
    var data = $('#primaryGroupId').val();
    //var mainData = $('#mainGroupId').select2('data');
    var mainData = $('#mainGroupId').val();
    var sId = maxId();
    if (data != "" && data != "0") {
        primary = $('#primaryGroupId').find("option:selected").text();  //data.text;
    }
    if (mainData != "" && mainData != "0") {
        main = $('#mainGroupId').find("option:selected").text();  //mainData.text;
    }
    var type = getAsset(data);
    $("#subGroupId").val(sId);

    path = type + " / " + primary + " / " + main + " / " + $("#nameId").val();
    $("#subGroupPath").val(path);
}
function nameCheck(name) {
    var url = "/SubGroup/nameCheck?name=" + name;
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
    var mId = maxId();
    $("#subGroupId").val(mId);
}
function maxCode(sId, code, type) {
    var url = "/SubGroup/getMaxCode?group=" + sId + "&code=" + code + "&type=" + type;
    var result = "";
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            console.log("max:" + JSON.stringify(res));
            result = res.maxData;
        }
    });
    console.log("Result : " + result);
    return result;
}
function maxId() {
    var url = "/SubGroup/getMaxId";
    console.log("Console , Url : " + url);

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
        $("#subGroupPath").mouseover(function () {
            $("#subGroupPath").prop('readonly', true);
        });
        $("#subGroupPath").mouseout(function () {
            $("#subGroupPath").prop('readonly', true);
        });
    }
    else {
        $("#subGroupPath").mouseover(function () {
            $("#subGroupPath").prop('readonly', false);
        });
        $("#subGroupPath").mouseout(function () {
            $("#subGroupPath").prop('readonly', true);
        });
    }
}
function findWork(id) {
    if (id != "0" && id != "") {
        $.ajax({
            url: "/SubGroup/findData",
            data: { id: id },
            async: false,
            success: function (res) {
                if (res.isFind) {
                    //if (!isEmpty(res.data)) {
                        var pgId = res.data.primaryGroupId;
                        var pgCaption = res.data.primaryGroupCode;
                        var pgCaption2 = res.data.primaryGroupName;
                        var mgId = res.data.mainGroupId;
                        var mgCaption = res.data.mainGroupCode;
                        var mgCaption2 = res.data.mainGroupName;

                        $("#autoId").val(res.data.id);
                        //$('#primaryGroupId').select2('data', { id: pgId, text: pgCaption + "-" + pgCaption2 });
                        //var newOption = new Option(pgCaption + "-"  + pgCaption2, pgId, true, true);
                        //$('#primaryGroupId').empty().append(newOption).trigger('change');

                        ////$('#mainGroupId').select2('data', { id: mgId, text: mgCaption + "-" + mgCaption2 });
                        //var newOption = new newOption(mgCaption + "-" + mgCaption2, mgId, true, true);
                        //$('#mainGroupId').empty().append(newOption).trigger('change');

                    var primaryGroupOption = new Option(pgCaption + "-" + pgCaption2, pgId, true, true);
                    $('#primaryGroupId').empty().append(primaryGroupOption).trigger('change');

                    // Corrected the issue with creating a new option for main group
                    //var mainGroupOption = new Option(mgCaption + "-" + mgCaption2, mgId, true, true);
                    //$('#mainGroupId').empty().append(mainGroupOption).trigger('change');
                                      
                        $("#subGroupId").val(res.data.subGroupId);
                        $("#subGroupCode").val(res.data.subGroupCode);
                        $("#nameId").val(res.data.subGroupName);
                        var path = getType(res.data.primaryGroupId.substring(0, 1)) +
                            " / " + pgCaption + "-" + pgCaption2 + " / " + mgCaption + "-" + mgCaption2 + " / " + $("#nameId").val();

                        $("#subGroupPath").val(path);
                        if (res.data.active) {
                            console.log("Active:" + res.data.active);
                            $('#chkActive').prop('checked', true);
                        }
                        else {
                            console.log("Active2:" + res.data.active);
                            $('#chkActive').prop('checked', false);
                        }
                    console.log("Active3:" + res.data.active);

                    var mainGroupOption = new Option(mgCaption + "-" + mgCaption2, mgId, true, true);
                    $('#mainGroupId').empty().append(mainGroupOption).trigger('change');

                   // }
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
        url: "/SubGroup/findData",
        data: { id: id },
        async: false,
        success: function (res) {
            ret = res.isFind;
        }
    });
    return ret;
}
function saveWork(isNew, isEdit) {
    var id = $("#autoId").val();
    //var primaryData = $('#primaryGroupId').select2('data');
    var primaryData = $("#primaryGroupId").val();
   // var mainData = $('#mainGroupId').select2('data');
    var mainData = $("#mainGroupId").val();
    var name = $("#nameId").val();
    var subGroupId = $("#subGroupId").val();
    var subGroupCode = $("#subGroupCode").val();
    //var active = $('#chkActive').prop('checked');
    var active = $('#chkActive').prop('checked') ? 1 : 0;

    var pId = primaryData;
    var mId = mainData;
    var type = getAsset(primaryData);
    console.log("primary id:" + pId + " type:" + type);



    if (!isEdit) {
        subGroupId = maxId();
        subGroupCode = setCode();
    }

    var jsonData = {
        SubId: id, PrimaryGroupId: pId, MainGroupId: mId, SubGroupName: name, SubGroupId: subGroupId,
        SubGroupCode: subGroupCode, Active: active, EntryFrom: 'Sub Group'
    }

    $.ajax({
        type: "POST",
        url: "/SubGroup/subSave",
        data: jsonData,
        async: false,
        success: function (res) {
            if (res.success) {
                clear();
                init();
                if (!isNew) {
                    $("#subGroup").modal('hide');
                }
                $("#nameId").focus();
                $('#tbSubGroup').DataTable().ajax.reload();
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
    $("#autoId").val(0);
    $("#subGroupId").val("");
    $("#subGroupCode").val("");
    $("#nameId").val("");
    $("#subGroupPath").val("");
    $('#primaryGroupId').select2('data', { id: '0', text: 'Choose a Primary Group' });
    $('#mainGroupId').select2('data', { id: '0', text: 'Choose a Main Group' });
}
function checkValidation() {
    if ($("#primaryGroupId").val() != "0") {
        if ($("#mainGroupId").val() != "0") {
            if ($("#mainGroupId").val() != "") {
                if ($("#mainGroupCode").val() != "") {
                    if ($("#nameId").val() != "") {
                        return true;
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
                warningNotify("Please main group id ");
                return false;
            }
        }
        else {
            warningNotify("Please give Main group ");
            return false;
        }
    }
    else {
        warningNotify("Please give primary group ");
        return false;
    }
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
function primaryGroupData() {
    var url = "/SubGroup/getPrimaryGroup";

    $.getJSON(url, function (data) {
        var item = "";
        $("#primaryGroupId").empty();
        item += '<option value="' + 0 + '">Choose a Primary Group</option>'
        $("#primaryGroupId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#primaryGroupId").html(item);
    });
}
function mainGroupData(id) {
    
    var url = "/SubGroup/getMainGroup?id=" + id;
    //$('#mainGroupId').select2('data', { id: '0', text: 'Choose a Main Group' });

   
    if ($("#autoId").val() == 0) {
        $.getJSON(url, function (data) {
            var item = "";
            $("#mainGroupId").empty();
            item += '<option value="' + 0 + '">Choose a Main Group</option>'
            $("#mainGroupId").html(item);
            $.each(data, function (i, opt) {
                item += '<option value="' + opt.value + '">' + opt.text + '</option>'
            });
            $("#mainGroupId").html(item);
        });
    }
   
}
