
$(document).ready(function () {
    $("#btnAdd").click(function () {
        clear();
        init();
        
        document.getElementById('add').style.display = "block";
        document.getElementById('edit').style.display = "none";
       
        $("#narration").modal('show');

    });
    $('#tblNarration').on('click', '.tbEdit', function () {
         document.getElementById('add').style.display = "none";
        document.getElementById('edit').style.display = "block";
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tblNarration").dataTable().fnGetData(tr);

        if (isData(data.narrationId)) {
            $("#voucherType").prop('disabled', true);
            findWork(data.narrationId);
            $("#narration").modal('show');
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
   
    $(document).on('click', '#btnCancel', function () {
        $('#tblNarration').DataTable().ajax.reload();
        $('#narration').modal('hide');
    });
    $("#narration").on('hidden.bs.model', function () {
        $('#tblNarration').DataTable().ajax.reload();
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

function init() {
    var NarrationCode = maxId();
    $("#narrationCode").val(NarrationCode);
}

function maxId() {
    var url = "/NarrationInfo/getMaxId";
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

function findWork(id) {
    if (id != "0" && id != "") {
        $.ajax({
            url: "/NarrationInfo/findData",
            data: { id: id },
            async: false,
            success: function (res) {
                if (res.isFind) {
                    if (!isEmpty(res.data)) {
                        console.log(res.data)
             
                        $("#autoId").val(res.data.narrationId);              
                        $("#voucherType").val(res.data.voucherType);
                        $("#narrationCode").val(res.data.narrationCode);
                        $("#narrationName").val(res.data.narrationName);
                        
                        if (res.data.active == 1) {
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
        url: "/NarrationInfo/findData",
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
    var voucherType = $("#voucherType").val();
    var narrationCode = $("#narrationCode").val();
    var narrationName = $("#narrationName").val();
    var active = $('#chkActive').prop('checked') ? 1 : 0;
 

    var jsonData = {
        NarrationId: id, NarrationCode: narrationCode, NarrationName: narrationName, VoucherType: voucherType,
        Active: active
    }

    $.ajax({
        type: "POST",
        url: "/NarrationInfo/narrationSave",
        data: jsonData,
        async: false,
        success: function (res) {
            if (res.success) {
                clear();
                init();
                if (!isNew) {
                    $("#narration").modal('hide');
                }
                $("#narrationName").focus();
                $('#tblNarration').DataTable().ajax.reload();
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }

    });
}
function clear() {
    $("#voucherType").prop('disabled', false);
    $("#autoId").val(0);
    $("#narrationCode").val("");
    $("#narrationName").val("");

}
function checkValidation() {
    if ($("#voucherType").val() != "") {
        
            if ($("#narrationCode").val() != "") {
                if ($("#narrationName").val() != "") {
                    return true;
                }
                else {
                    warningNotify("Please give Narration ");
                    return false;
                }
            }
            else {
                warningNotify("Please provide narration code ");
                return false;
            }     
    }
    else {
        warningNotify("Please Select Voucher Type ");
        return false;
    }
}
