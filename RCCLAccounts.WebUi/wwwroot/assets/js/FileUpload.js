// Attachment


$("#attachmentUP").change(function (event) {
    fileUpload(this.files, $("#fileName"), $(".attachmentlink"), $(".attachmentshow"));
});

function fileUpload(File, filenameelement, filelinkelement, fileshowelement) {
    //var File = me.files;
    var file = File[0];
    // debugger;

    if (File && File[0]) {
        console.log("size " + file.size);
        if (file.size < 5242881) {
            var name = file.name;
            console.log("name " + name);
            var ext = name.split(".");
            ext = ext[ext.length - 1].toLowerCase();
            var arrayExtensions = ["jpg", "jpeg", "png", "bmp", "pdf"];
            var reader = new FileReader();
            reader.readAsDataURL(file)
            reader.onload = function (_file) {
                filenameelement.text(name);
                filelinkelement.attr('href', _file.target.result);
                if (ext === 'pdf') {
                    fileshowelement.attr('src', '/images/default-pdf.png');
                } else {
                    fileshowelement.attr('src', _file.target.result);
                }
            }
            document.querySelector('#attachmentUP').files = File;
        } else {
            new PNotify({
                title: 'Warning!',
                text: 'File size should be small then 5MB.',
                type: 'warning',
            });
        }
    }
}

var clearAttachment = function () {
    $("#upload").val('');
    $("#attachmentUP").val('');
    $("#fileName").text('');
    $(".attachmentlink").attr('href', '');
    $(".attachmentshow").attr('src', '/images/default-image.png');
}


function attachmentLoad() {
    var uploadId = $("#upload").val();
    //console.log(uploadId);
    if (uploadId != null && uploadId != "") {
        $(".attachmentlink").attr('href', uploadId);
        var filenameext = uploadId.split("\\");
        filenameext = filenameext[filenameext.length - 1];
        $("#fileName").text(filenameext);
        var ext = filenameext.split(".");
        ext = ext[ext.length - 1].toLowerCase();
        if (ext === 'pdf') {
            $(".attachmentshow").attr('src', '/images/default-pdf.png');
        } else {
            $(".attachmentshow").attr('src', uploadId);
        }

    }
}
function allowDrop(e) {
    e.stopPropagation();
    e.preventDefault();
    e.dataTransfer.dropEffect = 'Drop me';
    $("#dropthumbnail").addClass("drag-drop-hover");
}

function leaveDrop(e) {
    $("#dropthumbnail").removeClass("drag-drop-hover");
}

function drag(ev) {
    ev.dataTransfer.setData("text", ev.target.id);
}

function drop(e) {
    e.stopPropagation();
    e.preventDefault();
    $("#dropthumbnail").removeClass("drag-drop-hover");
    var files = e.dataTransfer.files; // Array of all files
    fileUpload(files, $("#fileName"), $(".attachmentlink"), $(".attachmentshow"));
}