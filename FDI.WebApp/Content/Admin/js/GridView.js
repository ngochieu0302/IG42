var imageLoading = "<div class='loading-panel'><div class='icon-loading-panel'></div></div>";
var urlLists = "";
var urlForm = "";
var urlViewRole = "";
var urlShow = "";
var urlFormGroup = "";
var urlPostAction = "";
var urlPostActionGroup = "";
var urlPostRatingAction = "";
var urlEditRating = "";
var urlSort = "";
var urlSortGroup = "";
var urlView = "";
var urlHistory = "";
var urlEditPoint = "";
var urlFormRoleA = "";
var urlFormModule = "";
var urlViewGroup = "";
var formHeight = "auto";
var formWidth = "600";

function initAjaxLoad(urlListLoad, container, callback) {
    $.address.init().change(function (event) {
        var urlTransform = urlListLoad;
        var urlHistory = event.value;
        if (urlHistory.length > 0) {
            urlHistory = urlHistory.substring(1, urlHistory.length);
            if (urlTransform.indexOf("?") > 0)
                urlTransform = urlTransform + "&" + urlHistory;
            else
                urlTransform = urlTransform + "?" + urlHistory;
        }
        $(container).html(imageLoading);
        $.post(urlTransform, function (data) {
            $(container).html(data);
        }).complete(function () {
            if (callback && typeof (callback) === "function") {
                callback();
            }
        });
    });
}

//TreeView
function AjaxLoadTreeView(list, idcss) {
    function quickToolvalue(id, title, b, pid) {
        if ($("#quickToolvalue").length !== 0) {
            var html;
            if (b) html = $("#quickToolShow").html();
            else html = $("#quickToolHide").html();
            html += $("#quickToolvalue").html();
            html = html.replace(new RegExp("titlevalue", "g"), title);
            html = html.replace(new RegExp("idvalue", "g"), id);
            html = html.replace(new RegExp("idpvalue", "g"), pid);
            return "<div class='quickTool'>" + html + "</div>";
        }
        return "";
    }
    $(idcss).html("");
    $.each(list, function (a, b) {
        var data = "<li title='" + b.Name + "' class='unselect'>" +
            "<span class='file'>" +
            (b.IsShow ? "<a class='tool' id='" + b.ID + "' href='javascript:;'>" + b.Name + "</a>" : "<a class='tool' href='avascript:;'><strike>" + b.Name + "</strike></a>") +
            "<i>(" + b.Count + ")</i>" +
            quickToolvalue(b.ID, b.Name, b.IsShow, b.ParentId) + "</span>" +
            (b.Count > 0 ? " <ul id ='id" + b.ID + "'></ul>" : "") +
            "</li>";
        if ($("#id" + b.ParentId).length === 0) {
            $(idcss).append(data);
        } else {
            $("#id" + b.ParentId).append(data);
        }
    });
    TreeviewDiv(idcss);
};

function AjaxLoadTreeViewCheckBox(list, idcss) {
    $(idcss).html("");
    $.each(list, function (a, b) {
        var data = "<li class='unselect'  id='" + b.ID + "'>" +
            "<span class='folder'>" +
            "<input id='Category_" + b.ID + "' name='Category_" + b.ID + "' value='" + b.ID + "' type='checkbox' title='" + b.Name + "' " + (b.IsShow ? "checked" : "") + " />" + b.Name + "</span>" +
            (b.Count > 0 ? " <ul id ='id" + b.ID + "'></ul>" : "") +
            "</li>";
        if ($("#id" + b.ParentId).length === 0) {
            $(idcss).append(data);
        } else {
            $("#id" + b.ParentId).append(data);
        }
    });
    TreeviewDiv(idcss);
};

function TreeviewDiv(idcss) {
    $(idcss).treeview({
        collapsed: true,
        animated: "medium",
        control: "#treecontrol",
        persist: "location"
    });
}
function changeHashValue(key, value, source) {
    value = encodeURIComponent(value);
    var currentLink = source.substring(1);
    var returnLink = "#";
    var exits = false;
    if (currentLink.indexOf("&") > 0) { // lớn hơn 1
        var tempLink = currentLink.split("&");
        for (var idx = 0; idx < tempLink.length; idx++) {
            if (key === tempLink[idx].split("=")[0]) { //check Exits
                returnLink += key + "=" + value;
                exits = true;
            }
            else {
                returnLink += tempLink[idx];
            }
            if (idx < tempLink.length - 1)
                returnLink += "&";
        }
        if (!exits)
            returnLink += "&" + key + "=" + value;
    } else if (currentLink.indexOf("=") > 0) { //Chỉ 1
        if (currentLink.match(/Page/)) {
            returnLink = "#" + key + "=" + value;
        } else {
            returnLink = "#" + currentLink + "&" + key + "=" + value;
        }
    }
    else {
        returnLink = "#" + key + "=" + value;
    }
    return returnLink;
}

//Chuyển trang với value mới
function changeHashUrl(key, value) {
    var currentLink = $.address.value();
    return changeHashValue(key, value, currentLink);
}

function registerGridView(selector) {
    //Sắp xếp các cột
    //TúNT      22/11/2013      Update sort dont' lose param search, page
    debugger;
    $(selector + " .gridView th a").each(function () {
        var link = $(this).attr("href");
        link = link.substring(1, link.length);
        var newLink = "";
        var currentLink = $.address.value();
        //Trường hợp ghi đè mọi thuộc tính khác trên grid với có số trang, thông tin tìm kiếm
        if (currentLink.indexOf("&") > 1) {
            var re = /(?:\x26)?(Field\x3D[^\x26]+)\x26/;
            if (currentLink.match(/\x26(Field\x3D[^\x26]+)\x26/))
                newLink = currentLink.replace(re, link + "&");
            if (currentLink.match(/(?:\x26)?(Field\x3D[^\x26]+)\x26/))
                newLink = currentLink.replace(re, "&" + link + "&");
            if (newLink.match(/^\x2F/))
                newLink = newLink.substring(1, newLink.length);
            if (newLink.match(/^\x26/))
                newLink = newLink.substring(1, newLink.length);
            $(this).attr("href", "#" + newLink);
        }
        //Trường hợp đã sort
        if ($.address.value().indexOf(link) > 0) {
            var tempLink;
            if ($.address.value().indexOf("FieldOption=1") > 0) { //Tăng dần
                newLink = newLink.replace("/", "");
                tempLink = newLink;
                $(this).addClass("desc");
                var re2 = /\x26(FieldOption\x3D\d+)/;
                newLink = newLink.replace(re2, "&FieldOption=0");
                if (newLink === tempLink) {
                    var re4 = /\x26(x26FieldOption\x3D[^\x26]+)\x26/;
                    newLink = newLink.replace(re4, "&FieldOption=0");
                }
                newLink = newLink.replace("/", "");
                if (newLink.match(/^\x26/))
                    newLink = newLink.substring(1, newLink.length);
                $(this).attr("href", "#" + newLink);
            }
            else { //Giảm dần
                $(this).addClass("asc");
                tempLink = newLink;
                var re5 = /\x26(FieldOption\x3D\d+)/;
                newLink = newLink.replace("/", "");
                newLink = newLink.replace(re5, "&FieldOption=1");
                if (newLink === tempLink) {
                    var re6 = /\x26(x26FieldOption\x3D[^\x26]+)\x26/;
                    newLink = newLink.replace(re6, "FieldOption=1");
                }
                if (newLink === "")
                    newLink = link + "&FieldOption=1";
                newLink = newLink.replace("/", "");
                $(this).attr("href", "#" + newLink);
            }
        }
    });

    //khi người dùng click trên 1 row
    $(selector + " .gridView tr").not("first").click(function () {
        $(selector + " .gridView tr").removeClass("hightlight");
        $(this).addClass("hightlight");
    });

    //checkall
    $(selector + " .checkAll").click(function () {
        var checkboxes = $(this).closest(selector).find(":checkbox");
        if ($(this).is(":checked")) {
            checkboxes.prop("checked", true);
        } else {
            checkboxes.prop("checked", false);
        }
    });

    //Nhảy trang
    $(selector + " .bottom-pager input").change(function () {
        var cPage = trim12($(this).val());
        var maxPage = $(selector + " .bottom-pager input[type=hidden]").val();
        if (cPage.length === 0)
            createMessage("Thông báo", "Yêu cầu nhập trang cần chuyển đến");
        else if (isNaN(cPage))
            createMessage("Thông báo", "trang chuyển đến phải là kiểu số");
        else if (parseInt(cPage) > maxPage)
            createMessage("Thông báo", "trang không được lớn hơn " + maxPage + "");
        else if (parseInt(cPage) <= 0) {
            createMessage("Thông báo", "trang phải lớn hơn 0");
        }
        else {
            window.location.href = changeHashUrl("Page", cPage);;
        }
    });

    //Thay đổi số bản ghi trên trang
    $(selector + " .bottom-pager select").change(function () {
        var urlFWs = $.address.value();
        urlFWs = changeHashValue("Page", 1, urlFWs); //Replace  &Page=.. => Page=1
        urlFWs = changeHashValue("RowPerPage", $(this).val(), urlFWs); //Replace  &TenDonVi=.. => TenDonVi=donViNhan
        window.location.href = urlFWs;
    });

    //Đăng ký xóa nhiều
    $("[data-event=\"deleteAll\"][data-grid=\"" + selector + "\"]").click(function () {
        var arrRowId = "";
        var rowTitle = "";
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
            rowTitle += "<li>" + $(this).parent().parent().attr("title") + "</li>";
        });
        rowTitle = "<ul>" + rowTitle + "</ul>";

        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowDelete(urlPostAction, arrRowId, rowTitle, linkFw);
        return false;
    });

    //Đăng ký Hiển thị nhiều
    $("[data-event=\"showAll\"][data-grid=\"" + selector + "\"]").click(function () {
        var arrRowId = "";
        var rowTitle = "";
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
            rowTitle += "<li>" + $(this).parent().parent().attr("title") + "</li>";
        });
        rowTitle = "<ul>" + rowTitle + "</ul>";

        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowShow(urlPostAction, arrRowId, rowTitle, linkFw);
        return false;
    });


    //Đăng ký ẩn nhiều
    $("[data-event=\"hideAll\"][data-grid=\"" + selector + "\"]").click(function () {

        var arrRowId = "";
        var rowTitle = "";
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
            rowTitle += "<li>" + $(this).parent().parent().attr("title") + "</li>";
        });
        rowTitle = "<ul>" + rowTitle + "</ul>";

        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowHide(urlPostAction, arrRowId, rowTitle, linkFw);
        return false;
    });

    //Active row
    $(selector + " .gridView [data-event=\"active\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        rowActive(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });
    $(selector + " .gridView [data-event=\"notActive\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        rowNotActive(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });

    //Đăng ký button xóa row
    $(selector + " .gridView [data-event=\"delete\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        rowDelete(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });

    //Đăng ký button xóa row, co su dung url
    $(selector + " .gridView [data-event=\"deleteurl\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        rowDelete(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw + "&" + $(this).attr("data-url"));
        return false;
    });

    //Đăng ký button hiển thị
    $(selector + " .gridView [data-event=\"show\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        rowShow(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });

    //Đăng ký button ẩn
    $(selector + " .gridView [data-event=\"hide\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 50);
        rowHide(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });

    //đăng ký Thêm row
    $(selector + " .gridView [data-event=\"add\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Thêm mới bản ghi";
        var urlRequest;
        if (urlForm.indexOf("?") > 0)
            urlRequest = urlForm + "&do=add&ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + "?do=add&ItemId=" + $(this).attr("href").substring(1);
        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);
        return false;
    });

    $(selector + " .gridView [data-event=\"addMenu\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Thêm mới bản ghi";

        var urlRequest;
        if (urlForm.indexOf("?") > 0)
            urlRequest = urlForm + "&do=add&ItemId=" + $(this).attr("href").substring(1) + "&groupId=" + $(this).attr("data-groupid");
        else
            urlRequest = urlForm + "?do=add&ItemId=" + $(this).attr("href").substring(1) + "&groupId=" + $(this).attr("data-groupid");
        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);
        return false;
    });

    $(selector + " .gridView [data-event=\"showModule\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Xem thông tin";

        var urlRequest;
        if (urlShow.indexOf("?") > 0)
            urlRequest = urlShow + "&do=showModule&ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlShow + "?do=showModule&ItemId=" + $(this).attr("href").substring(1);

        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);
        return false;
    });

    //đăng ký sửa row
    $(selector + " .gridView [data-event=\"edit\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Sửa thông tin bản ghi";
        var urlRequest;
        if (urlForm.indexOf("?") > 0)
            urlRequest = urlForm + "&do=edit&ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + "?do=edit&ItemId=" + $(this).attr("href").substring(1);
        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);
        return false;
    });

    // cập nhật menu 
    $(selector + " .gridView [data-event=\"editMenu\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Sửa thông tin bản ghi";

        var urlRequest;
        if (urlForm.indexOf("?") > 0)
            urlRequest = urlForm + "&do=edit&ItemId=" + $(this).attr("href").substring(1) + "&groupId=" + $(this).attr("data-groupId");
        else
            urlRequest = urlForm + "?do=edit&ItemId=" + $(this).attr("href").substring(1) + "&groupId=" + $(this).attr("data-groupId");

        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);

        return false;
    });


    // Gán User cho Module
    $(selector + " .gridView [data-event=\"usermodule\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Gán User cho Module";

        var urlRequest;
        if (urlViewRole.indexOf("?") > 0)
            urlRequest = urlViewRole + "&do=userModule&ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlViewRole + "?do=userModule&ItemId=" + $(this).attr("href").substring(1);

        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);

        return false;
    });

    // Gán Role cho Module
    $(selector + " .gridView [data-event=\"rolemodule\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Gán Role cho Module";

        var urlRequest;
        if (urlFormModule.indexOf("?") > 0)
            urlRequest = urlFormModule + "?id=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlFormModule + "?id=" + $(this).attr("href").substring(1);

        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);

        return false;
    });

    //đăng ký sắp xếp
    $(selector + " .gridView [data-event=\"sort\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Sắp xếp thứ tự hiển thị";

        var urlRequest;
        if (urlSort.indexOf("?") > 0)
            urlRequest = urlSort + "&do=sort&ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlSort + "?do=sort&ItemId=" + $(this).attr("href").substring(1);

        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);

        return false;
    });

    $(selector + " .gridView [data-event=\"sortMenu\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Sắp xếp thứ tự hiển thị";

        var urlRequest;
        if (urlSort.indexOf("?") > 0)
            urlRequest = urlSort + "&do=sort&ItemId=" + $(this).attr("href").substring(1) + "&groupId=" + $(this).attr("rel");
        else
            urlRequest = urlSort + "?do=sort&ItemId=" + $(this).attr("href").substring(1) + "&groupId=" + $(this).attr("rel");

        FdiDialog(urlRequest, titleDiag, formHeight, formWidth);

        return false;
    });

    //đăng ký xem row
    $(selector + " .gridView [data-event=\"view\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Xem thông tin bản ghi";

        var urlRequest;
        if (urlView.indexOf("?") > 0)
            urlRequest = urlView + "&itemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlView + "?itemId=" + $(this).attr("href").substring(1);

        FdiDialogView(urlRequest, titleDiag);

        return false;
    });

    $(selector + " .gridView [data-event=\"complete\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Xem thông tin bản ghi";

        var urlRequest;
        if (urlFormRoleA.indexOf("?") > 0)
            urlRequest = urlFormRoleA + "?id=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlFormRoleA + "?id=" + $(this).attr("href").substring(1);

        FdiDialogView(urlRequest, titleDiag);

        return false;
    });

    //đăng ký xem xem và chỉnh sửa đánh giá sản phẩm
    $(selector + " .gridView [data-event=\"rating\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Xem thông tin";

        var urlRequest;
        if (urlView.indexOf("?") > 0)
            urlRequest = urlEditRating + "&do=edit&itemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlEditRating + "?do=edit&itemId=" + $(this).attr("href").substring(1);

        FdiDialogView(urlRequest, titleDiag, viewHeight, viewWidth);

        return false;
    });
}

//Hiển thị row tren grid
function rowShow(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Hiển thị các bản ghi đã chọn";
        else
            titleDia = "Hiển thị bản ghi đã chọn";
        swal({
            title: titleDia,
            text: rowTitle,
            type: "info",
            showCancelButton: true,
            confirmButtonClass: "btn-success",
            confirmButtonText: "Hiển thị",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },
        function (isConfirm) {
            if (isConfirm) {
                $.post(encodeURI(urlPost), { "do": "show", "itemId": "" + arrRowId + "" }, function (data) {
                    if (data.Erros) {
                        createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                    } else {
                        toastr["success"](data.Message);
                        window.location.href = urlFw + "&type=show&idShow=" + arrRowId;
                    }
                });
            }
            swal.close();
        });
    }
}

//Duyệt row tren grid
function rowActive(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Duyệt các bản ghi đã chọn";
        else
            titleDia = "Duyệt bản ghi đã chọn";

        swal({
            title: titleDia,
            text: rowTitle,
            type: "info",
            showCancelButton: true,
            confirmButtonClass: "btn-success",
            confirmButtonText: "Duyệt",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },
       function (isConfirm) {
           if (isConfirm) {
               $.post(encodeURI(urlPost), { "do": "Active", "itemId": "" + arrRowId + "" }, function (data) {
                   if (data.Erros) {
                       createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                   }
                   else {
                       toastr["success"](data.Message);
                       window.location.href = urlFw + "&type=Active&idActive=" + arrRowId;
                   }
               });
           }
           swal.close();
       });
    }
}

function rowNotActive(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Duyệt các bản ghi đã chọn";
        else
            titleDia = "Duyệt bản ghi đã chọn";

        swal({
            title: titleDia,
            text: rowTitle,
            type: "info",
            showCancelButton: true,
            confirmButtonClass: "btn-success",
            confirmButtonText: "Hiển thị",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },
     function (isConfirm) {
         if (isConfirm) {
             $.post(encodeURI(urlPost), { "do": "Complete", "itemId": "" + arrRowId + "" }, function (data) {
                 if (data.Erros) {
                     createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                 }
                 else {
                     toastr["success"](data.Message);
                     var url = urlFw + "&type=LockAccount&idActive=" + arrRowId;
                     window.location.href = url;
                 }
             });
         }
         swal.close();
     });
    }
}

//ẩn row tren grid
function rowHide(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Ẩn các bản ghi đã chọn";
        else
            titleDia = "Ẩn bản ghi đã chọn";

        swal({
            title: titleDia,
            text: rowTitle,
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-warning",
            confirmButtonText: "Ẩn",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },
        function (isConfirm) {
            if (isConfirm) {
                $.post(encodeURI(urlPost), { "do": "hide", "itemId": "" + arrRowId + "" }, function (data) {
                    if (data.Erros) {
                        createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                    }
                    else {
                        swal.close();
                        toastr["success"](data.Message);
                        window.location.href = urlFw + "&type=hide&idHide=" + arrRowId;
                    }
                });
            } else {
                swal.close();
            }
        });
    }
}


//xoa row tren grid
function rowDelete(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Xóa các bản ghi đã chọn";
        else
            titleDia = "Xóa bản ghi đã chọn";

        swal({
            title: titleDia,
            text: rowTitle,
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-warning",
            confirmButtonText: "Xóa",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },
      function (isConfirm) {
          if (isConfirm) {
              $.post(encodeURI(urlPost), { "do": "delete", "itemId": "" + arrRowId + "" }, function (data) {
                  if (data.Erros) {
                      createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                  } else {
                      swal.close();
                      toastr["success"](data.Message);
                      window.location.href = urlFw + "&type=delete&idDelete=" + arrRowId;
                  }
              });
          } else {
              swal.close();
          }
      });
    }
}

// Add row
function rowAddShowpage(urlPost, arrRowId, rowTitle) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Thêm các bản ghi đã chọn";
        else
            titleDia = "Thêm bản ghi đã chọn";

        swal({
            title: titleDia,
            text: rowTitle,
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-warning",
            confirmButtonText: "Tiếp tục",
            cancelButtonText: "Hủy lệnh thêm",
            closeOnConfirm: false,
            closeOnCancel: false
        },
   function (isConfirm) {
       if (isConfirm) {
           $.post(encodeURI(urlPost), { "do": "Add", "itemId": "" + arrRowId + "" }, function (data) {
               if (data.Erros) {
                   createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
               }
               else {
                   toastr["success"](data.Message);
                   $("#brandGriditemsHomePage").load("/Admin/ProductTopOrder/ListItemsNotInHomePage?");
               }
           });
       } else {
           swal.close();
       }
   });
    }
}

//Thêm mới row cho các bảng maping không hiện thi dialog
function rowAdd(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Thêm các bản ghi đã chọn";
        else
            titleDia = "Thêm bản ghi đã chọn";
        $("#dialog-confirm").attr(titleDia);
        $("#dialog-confirm").html("<p><b>Bạn có chắc chắn muốn thêm mới:</b><br />" + rowTitle + "</p>");
        $("#dialog-confirm").dialog({
            title: titleDia,
            resizable: false,
            height: "auto",
            width: "auto",
            modal: true,
            buttons: {
                "Tiếp tục": function () {
                    $(this).dialog("close");
                    $.post(encodeURI(urlPost), { "do": "add", "itemId": "" + arrRowId + "" }, function (data) {
                        if (data.Erros) {
                            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                        }
                        else {
                            toastr["success"](data.Message);
                            window.location.href = urlFw + "&type=add&idAdd=" + arrRowId;
                        }
                    });
                },
                "Hủy lệnh thêm mới": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
}

function escapeHTML(str) {
    var div = document.createElement("div");
    var text = document.createTextNode(str);
    div.appendChild(text);
    return div.innerHTML;
}


function trim12(str) {
    str = str.replace(/^\s\s*/, "");
    var ws = /\s/,
		i = str.length;
    while (ws.test(str.charAt(--i)));
    return str.slice(0, i + 1);
}

function createCloseMessage(title, message, urlFw) {
    var linkFW = (!window.location.hash || window.location.hash == undefined || window.location.hash == "" || window.location.hash.length == 0) ? linkFW = window.location.href : window.location.hash;
    toastr["success"](message);
    window.location.href = linkFW;
}
function CloseMessage(title, message, urlFw) {
    urlFw = urlFw.replace(/<\/?\w(?:[^"'>]|"[^"]*"|'[^']*')*>/g, "");
    toastr["success"](message);
    window.location.href = urlFw;
}

function createAutoTag(tagControls, urlRouters) {
    $("#" + tagControls + "_Input").keypress(function (e) {
        if (e.keyCode === 13) {
            var valuesAdd = trim12($(this).val());
            if (valuesAdd === "")
                createMessage("Đã có lỗi xảy ra.", "Bạn phải nhập vào từ khóa tìm kiếm");
            else {
                addValues(tagControls, valuesAdd, urlRouters + "&do=Add", "");
            }
            return false;
        }
        return false;
    });

    $("#" + tagControls + "_Input").autocomplete({
        serviceUrl: urlRouters,
        minChars: 1,
        delimiter: /(;)\s*/, // regex or character
        maxHeight: 400,
        width: 500,
        zIndex: 9999,
        deferRequestBy: 0 //miliseconds
    });
}

function getParameters(name, valueDefault) {
    var page = valueDefault;
    var url = window.location.href;
    if (url.indexOf("?") > 0) {
        url = url.replace("#", "&");
    } else {
        url = url.replace("#", "?");
    }
    var obj = $.url(url).param();
    if (obj[name] != null) {
        page = obj[name];
    }
    return page;
}
