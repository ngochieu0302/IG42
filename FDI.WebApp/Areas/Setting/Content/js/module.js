var layoutName = "";
var sessionValue = 0;
var pageId = 0;
var url = "";


$(document).ready(function () {
    $(".dropdown-settinh-module").parent().addClass("box-edit-module");
    $("#start").prev().css("background", "#f99");


    $("[data-dismiss='modal']").click(function () {
        $("#modal-content").html("");
    });

   
    $("#btnAddModule").click(function () {       
        $.post("/Setting/Modules/EditModule", { doAction: "add", pageId: pageId, layout: layoutName }, function (data) {
            $("#modalSettingModule .modal-title").html("Thêm mới module");
            $(".modal-dialog").css({ "max-width": "350px" });
            $("#modal-content").html(data);
            $("#modalSettingModule .btn-primary").html("Thêm");
        });
    });
    $("#btnSaveLayout").click(function () {
        $.post("/Setting/Modules/SaveModule" + "?doAction=layout", { LayoutNew: $("#LayoutNew").val(), pageId: pageId }, function () {
            window.location.reload();
        });
    });

    $("#btnEditPage").click(function () {
        $.post("/Setting/Modules/editPage", { name: "edit" }, function () {
            window.location.reload();
        });
    });

    $("#btnViewPage").click(function () {
        $.post("/Setting/Modules/editPage", { name: "view" }, function () {
            window.location.reload();
        });
    });

    $(".btn-show-pagesetting").click(function () {
        $("#top-header-setting").toggleClass("active");
    });

    $(".btn-setting-edithtml").click(function () {
        var id = $(this).data("id");
        $(".modal-title").html("Chỉnh sửa nội dung module HTML");
        $(".modal-dialog").css({ "max-width": "1200px" });
        $.post("/Html/Html/EditHtml", { id: id }, function (data) {
            $("#modal-content").html(data);
        });
    });

});
function ModuleSetting(name) {
    $(name + ".edit-module").click(function () {
        var id = $(this).data("id");
        $(".modal-title").html("Chỉnh sửa module");
        $(".modal-dialog").css({ "max-width": "350px" });
        $.post("/Setting/Modules/EditModule", { doAction: "edit", ctrId: id, layout: layoutName }, function (data) {
            
            $("#modal-content").html(data);
        });
    });

    $(name + ".delete-module").click(function () {
        var id = $(this).data("id");
        $(".modal-title").html("Bạn có chắc chắn muốn xóa module này ?");
        $(".modal-dialog").css({ "max-width": "420px" });
        $.post("/Setting/Modules/EditModule", { doAction: "delete", ctrId: id }, function (data) {
            $("#modal-content").html(data);
            $("#modalSettingModule .btn-primary").html("Xóa");
        });
    });

    $(name + ".copy-module").click(function () {
        var id = $(this).data("id");
        $(".modal-title").html("Sao chép module");
        $(".modal-dialog").css({ "max-width": "350px" });
        $.post("/Setting/Modules/ModuleCopy", { doAction: "copy", ctrId: id }, function (data) {
            $("#modal-content").html(data);
        });
    });

    $(name + ".setting-module").click(function () {
        var id = $(this).data("id");
        var url = $(this).data("url");
        $(".modal-title").html("Cài đặt module");
        $(".modal-dialog").css({ "max-width": "1200px" });
        $.post(url, { doAction: "setting", ctrId: id }, function (data) {
            $("#modal-content").html(data);
        });
    });
}
var util = {};
document.addEventListener("keydown", function (e) {
    var key = util.key[e.which];
    if (key === "F1") {
        e.preventDefault();
        if (sessionValue === 1) {
            $("#btnViewPage").click();
        } else if (sessionValue === 2) {
            $("#btnEditPage").click();
        }

    }
    if (key === "F2") {
        if (sessionValue === 1) {
            $("#btnAddModule").click();
        }
    }
    if (key === "esc") {
        $(".close").click();
    }

    if (key === "F4") {
        window.open("/Admin", "_blank");
    }

});
util.key = {
    9: "tab",
    13: "enter",
    16: "shift",

    18: "alt",
    27: "esc",
    33: "rePag",
    34: "avPag",
    35: "end",
    36: "home",
    37: "left",
    38: "up",
    39: "right",
    40: "down",
    112: "F1",
    113: "F2",
    114: "F3",
    115: "F4",
    116: "F5",
    117: "F6",
    118: "F7",
    119: "F8",
    120: "F9",
    121: "F10",
    122: "F11",
    123: "F12"
}
