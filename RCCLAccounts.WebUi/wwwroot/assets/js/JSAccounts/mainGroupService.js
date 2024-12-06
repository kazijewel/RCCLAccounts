
$(document).ready(function () {
    primaryGroupData();

    $('#primaryGroupId').select2({
        dropdownParent: $('#mainGroup')
        });


    $("#btnAdd").click(function () {
        clear();
        init();
        groupPathAction(true);
        document.getElementById('add').style.display = "block";
        document.getElementById('edit').style.display = "none";
        $("#mainGroup").modal('show');

    });
    $('#tbMainGroup').on('click', '.tbEdit', function () {
        groupPathAction(false);
        document.getElementById('add').style.display = "none";
        document.getElementById('edit').style.display = "block";
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbMainGroup").dataTable().fnGetData(tr);

        if (isData(data.id)) {
            $("#primaryGroupId").prop('disabled', true);
            findWork(data.id);
            $("#mainGroup").modal('show');
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
    //    $('#tbMainGroup').DataTable().ajax.reload();
    //});
    $(document).on('click', '#btnCancel', function () {
        $('#tbMainGroup').DataTable().ajax.reload();
        $('#mainGroup').modal('hide');
    });
    $("#mainGroup").on('hidden.bs.model', function () {
        $('#tbMainGroup').DataTable().ajax.reload();
    });

    $("#primaryGroupId").change(function () {
        var id = $("#primaryGroupId").val();
       // var data = $('#primaryGroupId').select2('data');

        var data = $(this).find("option:selected").text(); 
        console.log(data);

        var MainGroupId = maxId();
        var mId = MainGroupId.substring(0, 2);

        var pri = "";
        var path = "";
        var str = data.substring(0, 1);
        pri = getType(str);
        var mCode = codeWork(str, mId, pri);

        $("#mainGroupId").val(MainGroupId);
        $("#mainGroupCode").val(mCode);

        path = pri + " / " + data + " / " + $("#nameId").val();
        $("#mainGroupPath").val(path);
    });
    /*$("#nameId").change(function () {
        if (nameCheck($(this).val())) {
            $("#nameId").val("");
            $("#nameId").focus();
            warningNotify("Main group name already taken! try new one.");
        }
        else {
        }
    });*/
});
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
    var mId = maxId();
    $("#mainGroupId").val(mId);
}
function codeWork(type, mId, pri) {

    var code = (type == 'L' ? '100' : (type == 'A' ? '200' : (type == 'E' ? '300' : (type == 'I' ? '400' : ''))));
    var isCode = maxCode(mId, code, pri);
    return isCode;
}
function maxCode(mId, code, pri) {
    var url = "/MainGroup/getMaxCode?group=" + mId + "&code=" + code + "&type=" + pri;
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
function maxId() {
    var url = "/MainGroup/getMaxId";
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
            url: "/MainGroup/findData",
            data: { id: id },
            async: false,
            success: function (res) {
                if (res.isFind) {
                    if (!isEmpty(res.data)) {
                        console.log(res.data)
                        var pgId = res.data.primaryGroupId;
                        var pgCaption = res.data.primaryGroupId;
                        var pgCode = res.data.primaryGroupCode;
                        var pgCaption2 = res.data.primaryGroupName;

                        $("#autoId").val(res.data.id);
                     //  $('#primaryGroupId').select2('data', { id: pgId, text: pgCaption + "-" + pgCode + "-" + pgCaption2 });
             
                        var newOption = new Option(pgCaption + "-" + pgCode + "-" + pgCaption2, pgId, true, true);
                        $('#primaryGroupId').empty().append(newOption).trigger('change');

                        $("#mainGroupId").val(res.data.mainGroupId);
                        $("#mainGroupCode").val(res.data.mainGroupCode);
                        $("#nameId").val(res.data.mainGroupName);
                        var path = getType(res.data.primaryGroupId.substring(0, 1)) +
                            " / " + pgCaption + "-" + pgCode  +"-" + pgCaption2 + " / " + $("#nameId").val();

                        $("#mainGroupPath").val(path);
                        if (res.data.active == "True") {
                            $('#chkActive').prop('checked', true);
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
        url: "/MainGroup/findData",
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
    var mainGroupId = $("#mainGroupId").val();
    var mainGroupCode = $("#mainGroupCode").val();
    var name = $("#nameId").val();
    //var active = $('#chkActive').prop('checked');
    var active = $('#chkActive').prop('checked') ? 1 : 0;
    var pId = $("#primaryGroupId").val();
  


    if (!isEdit) {
        mainGroupId = maxId();
        mainGroupCode = $("#mainGroupCode").val();
    }

    var jsonData = {
        MainId: id,  PrimaryGroupId: pId, MainGroupId: mainGroupId, MainGroupName: name,
        MainGroupCode: mainGroupCode, Active: active, EntryFrom: 'Main Group'
    }

    $.ajax({
        type: "POST",
        url: "/MainGroup/mainSave",
        data: jsonData,
        async: false,
        success: function (res) {
            if (res.success) {
                clear();
                init();
                if (!isNew) {
                    $("#mainGroup").modal('hide');
                }
                $("#nameId").focus();
                $('#tbMainGroup').DataTable().ajax.reload();
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
    $("#autoId").val(0);
    $("#mainGroupId").val("");
    $("#mainGroupCode").val("");
    $("#nameId").val("");
    $("#mainGroupPath").val("");
    $('#primaryGroupId').select2('data', { id: '0', text: 'Choose a Primary Group' });
}
function checkValidation() {
    if ($("#primaryGroupId").val() != "0") {
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
        warningNotify("Please give primary group ");
        return false;
    }
}
function primaryGroupData() {
    var url = "/MainGroup/getPrimaryGroup";

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