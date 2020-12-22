$("a.tool").click(function () {
    if ($(this).parent().parent().hasClass("unselect")) {
        $(".filetree li").addClass("unselect").removeClass("select");
        $(this).parent().parent().addClass("select").removeClass("unselect");
    }
    else {
        $(this).parent().parent().addClass("unselect").removeClass("select");
    }
    return false;
});

$("#btn_add").click(function (e) {
    e.preventDefault();
    var urlRequest = urlForm + "?do=Add";
    $("#dialog-form").css("height", formHeight);
    $.post(urlRequest, function (data) {
        $("#dialog-form .modal-title").html("Thêm mới");
        $("#dialog-form #dialog-form-ajax").html(data);
    });
    $("#dialog-form").modal("show");
});

// Image popups
$("img.lazyImg").lazyload({
    effect: "fadeIn"
});



$(document).ajaxStop($.unblockUI);

function FdiDialog(url, title, height) {
    $("#dialog-form").css("height", height);
    $('body').modalmanager('loading');
    $.post(url, function (data) {
        $("#dialog-form .modal-title").html(title);
        $("#dialog-form #dialog-form-ajax").html(data);
        $('#dialog-form').modal('show');
    });   
    $("#dialog-form.in #close").click(function () {
        $('#dialog-form').modal('hide').html("");
    });
}

function FdiDialogView(url, title, height) {
    $("#dialog-form").css("height", height);
    $('body').modalmanager('loading');
    $.post(url, function (data) {
        $("#dialog-form .modal-title").html(title);
        $("#dialog-form #dialog-form-ajax").html(data);
        $('#dialog-form').modal('show');
    });
    $("#dialog-form.in #close").click(function () {
        $('#dialog-form').modal('hide').html("");
    });
}


String.prototype.changeParam = String.prototype.changeParam || function (key, value) {
    var temp = "(.*[\\?&#]" + key + "=)([^&#]*)";
    var regex = new RegExp(temp);
    if (this.length <= 0 || !regex.exec(this)) {
        if (this.length <= 0)
            return "#" + key + "=" + value;
        else
            return this + "&" + key + "=" + value;
    } else {
        return this.replace(regex, "$1" + value);
    }
};
/***
* Get parameters from string (?id=2&page=3) => id = 2; page = 3
*/
String.prototype.getParamFromUrl = String.prototype.getParamFromUrl || function (name) {
    var temp = "[\\?&#]" + name + "=([^&#]*)";
    var regex = new RegExp(temp);
    var results = regex.exec(this);
    if (!results)
        return "";
    else
        return results[1];
};
