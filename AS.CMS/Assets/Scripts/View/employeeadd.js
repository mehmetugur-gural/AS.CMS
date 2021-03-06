﻿$(document).ready(function () {
    $(".add-image").click(function () {
        selectFileWithCKFinder();
    });

    $(".add-employee-image").click(function () {
        selectEmployeeImageWithCKFinder();
    });

    if (!$('#saveEmployeeForm').length) {
        return false;
    }

    var loginValidationSettings = {
        rules: {
            FirstName: "required",
            LastName: "required",
            BirthDate: "required",
            Phone: "required"
        },
        messages: {
            FirstName: "Bu alan boş geçilemez",
            LastName: "Bu alan boş geçilemez",
            BirthDate: "Bu alan boş geçilemez",
            Phone: "Bu alan boş geçilemez"
        },
        invalidHandler: function () {
            animate({
                name: 'shake',
                selector: '.auth-container > .card'
            });
        }
    }

    $.extend(loginValidationSettings, config.validations);
    $('#saveEmployeeForm').validate(loginValidationSettings);

});

$('.images-container.gallery.cv').on({
    drop: function () {
        $("#CVFile").val("");

        $(".images-container.gallery.cv .image-container").each(function (item) {
            if ($(this).data("fileurl") != undefined) {
                $("#CVFile").val($("#CVFile").val() + $(this).data("fileurl") + ",");
            }
        });
    }
});

$('.images-container.gallery.employee').on({
    drop: function () {
        $("#Picture").val("");

        $(".images-container.gallery.employee .image-container").each(function (item) {
            if ($(this).data("fileurl") != undefined) {
                $("#Picture").val($("#Picture").val() + $(this).data("fileurl") + ",");
            }
        });
    }
});

$(document).on("click", ".employee-upload-remove", function () {
    var confirmDialog = confirm("Resmi Silmek İstediğinizden Emin Misiniz ?");
    if (confirmDialog) {

        $(this).parent().parent().remove();
        $("#Picture").val("");

        $(".images-container.gallery.employee .image-container").each(function (item) {
            if ($(this).data("fileurl") != undefined) {
                $("#Picture").val($("#Picture").val() + $(this).data("fileurl") + ",");
            }
        });

    }
});

$(document).on("click", ".upload-remove", function () {
    var confirmDialog = confirm("Resmi Silmek İstediğinizden Emin Misiniz ?");
    if (confirmDialog) {

        $(this).parent().parent().remove();
        $("#CVFile").val("");

        $(".images-container.gallery.cv .image-container").each(function (item) {
            if ($(this).data("fileurl") != undefined) {
                $("#CVFile").val($("#CVFile").val() + $(this).data("fileurl") + ",");
            }
        });

    }
});

$(document).on("click", ".upload-remove-single", function () {
    var confirmDialog = confirm("Resmi Silmek İstediğinizden Emin Misiniz ?");
    if (confirmDialog) {
        $(this).parent().parent().remove();
        var newImageItem = "<div class=\"image-container new\"><a href=\"#\" class=\"add-single-image\"><div class=\"image\"> <i class=\"fa fa-plus\"></i> </div></a></div>";
        $(".images-container.single").append(newImageItem);
        $("#picture").val("");
    }
});

$(document).on("click", ".add-single-image", function () {
    selectMainImageFileWithCKFinder();
});

function generatePermalinkFromText(text) {
    var maxLength = 100;

    var returnString = text.toLowerCase();
    returnString = returnString.replace(/ö/g, 'o');
    returnString = returnString.replace(/ç/g, 'c');
    returnString = returnString.replace(/ş/g, 's');
    returnString = returnString.replace(/ı/g, 'i');
    returnString = returnString.replace(/ğ/g, 'g');
    returnString = returnString.replace(/ü/g, 'u');
    returnString = returnString.replace(/[^a-z0-9\s-]/g, "");
    returnString = returnString.replace(/[\s-]+/g, " ");
    returnString = returnString.replace(/^\s+|\s+$/g, "");

    if (returnString.length > maxLength)
        returnString = returnString.substring(0, maxLength);

    returnString = returnString.replace(/\s/g, "-");

    return returnString;
}

function selectFileWithCKFinder() {
    CKFinder.modal({
        chooseFiles: true,
        width: 800,
        height: 600,
        onInit: function (finder) {
            finder.on('files:choose', function (evt) {
                var file = evt.data.files.toArray();
                for (var i = 0; i < file.length; i++) {
                    var uploadItem = "<div class=\"image-container\" data-fileurl=\"" + file[i].getUrl() + "\"><div class=\"controls\"><a href=\"#\" class=\"control-btn move\"><i class=\"fa fa-arrows\"></i></a><a href=\"#\" class=\"control-btn remove upload-remove\"><i class=\"fa fa-trash-o\"></i></a><a href=\"" + file[i].getUrl() + "\" class=\"control-btn download\"><i class=\"fa fa-download\"></i></a></div><div class=\"image\"\"><i class=\"fa fa-file-pdf-o\"></i></div></div>";
                    $(".images-container.gallery.cv").append(uploadItem);
                    $("#CVFile").val($("#CVFile").val() + file[i].getUrl() + ",");
                }
            });
        }
    });
}

function selectEmployeeImageWithCKFinder() {
    CKFinder.modal({
        chooseFiles: true,
        width: 800,
        height: 600,
        onInit: function (finder) {
            finder.on('files:choose', function (evt) {
                var file = evt.data.files.toArray();
                for (var i = 0; i < file.length; i++) {
                    var uploadItem = "<div class=\"image-container\" data-fileurl=\"" + file[i].getUrl() + "\"><div class=\"controls\"><a href=\"#\" class=\"control-btn move\"><i class=\"fa fa-arrows\"></i></a><a href=\"#\" class=\"control-btn remove employee-upload-remove\"><i class=\"fa fa-trash-o\"></i></a><a href=\"" + file[i].getUrl() + "\" download=\"" + file[i].getUrl() + "\" target=\"_blank\" class=\"control-btn download\"><i class=\"fa fa-download\"></i></a></div><div class=\"image\" style=\"background-image:url('" + file[i].getUrl() + "')\"></div></div>";
                    $(".images-container.gallery.employee").append(uploadItem);
                    $("#Picture").val($("#Picture").val() + file[i].getUrl() + ",");
                }
            });
        }
    });
}