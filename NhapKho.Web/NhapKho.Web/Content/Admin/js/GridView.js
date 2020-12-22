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
var urlFormRoleA = "";
var urlFormModule = "";
var urlViewGroup = "";

var datedefault = new Date(2016, 0, 1);
//var datedefaultg = new Date(2016, 0, 1);

function intGetTime(date, time) {
    //intGetTime("29/12/2017", "18:30")
    var from = date.split("/");
    var t = time.split(":");
    var f = new Date(from[2], from[1] - 1, from[0], t[0], t[1]);
    return parseInt((f.getTime() - datedefault.getTime()) / 1000);
}

function toDateTime(secs) {
    var t = new Date(2016, 0, 1); // Epoch
    t.setSeconds(secs);
    return t;
}

function intGetTimeByDate(date, h, m) {
    //intGetTime("29/12/2017", 18,30)
    var from = date.split("/");
    var f = new Date(from[2], from[1] - 1, from[0], h, m);
    var kq = parseInt((f.getTime() - datedefault.getTime()) / 1000);
    return kq;
}

function intDateNow() {
    var f = parseInt((new Date().getTime() - datedefault.getTime()) / 1000);
    return f;
}
function loadHtml(urlListLoad, container, callback) {
    $.post(urlListLoad,
        function(data) {
            $(container).html(data);
        }).complete(function () {
        if (callback && typeof (callback) === "function") {
            callback();
        }
    });;
}
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
    function quickToolvalue(id, title, b, parent) {
        if ($("#quickToolvalue").length !== 0) {
            var html;
            if (b) {
                html = $("#quickToolShow").html();
            } else {
                html = $("#quickToolHide").html();
            }
            html += $("#quickToolvalue").html();
            html = html.replace(new RegExp("titlevalue", "g"), title);
            html = html.replace(new RegExp("idvalue", "g"), id);
            html = html.replace(new RegExp("parentvalue", "g"), parent);
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
    $(selector + " .gridView th a").each(function () {
        var link = $(this).attr("href");
        var currentLink = $.address.value().replace("/", "#");
        var option = Parameters(currentLink, "FieldOption", 1);
        var field1 = Parameters(link, "Field", "");
        var field2 = Parameters(currentLink, "Field", "");
        var href = "#Field=" + field1 + "&FieldOption=1";;
        if (field1 === field2) {
            if (option === "1") {
                href = "#Field=" + field1 + "&FieldOption=0";
            }
        }
        $(this).attr("href", href);
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
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
        });
        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowDelete(urlPostAction, arrRowId, rowTitle, linkFw);
        return false;
    });

    //Đăng ký Hiển thị nhiều
    $("[data-event=\"showAll\"][data-grid=\"" + selector + "\"]").click(function () {
        var arrRowId = "";
        var rowTitle = "";
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
        });
        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowShow(urlPostAction, arrRowId, rowTitle, linkFw);
        return false;
    });
    //Đăng ký Hiển thị nhiều
    $("[data-event=\"activeAll\"][data-grid=\"" + selector + "\"]").click(function () {
        var arrRowId = "";
        var rowTitle = "";
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
        });
        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowActive(urlPostAction, arrRowId, rowTitle, linkFw);
        return false;
    });
    //Đăng ký ẩn nhiều
    $("[data-event=\"hideAll\"][data-grid=\"" + selector + "\"]").click(function () {
        var arrRowId = "";
        var rowTitle = "";
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        $(selector + " input.check[type='checkbox']:checked").not("#checkAll").not(".checkAll").each(function () {
            arrRowId += $(this).val() + ",";
        });
        arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
        rowHide(urlPostAction, arrRowId, rowTitle, linkFw);
        return false;
    });

    //Active row
    $(selector + "  [data-event=\"active\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        rowActive(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });
    $(selector + " .gridView [data-event=\"notActive\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        rowNotActive(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });

    //Đăng ký button xóa row
    $(selector + " [data-event=\"delete\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        rowDelete(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });

    //Đăng ký button xóa row, co su dung url
    $(selector + " .gridView [data-event=\"deleteurl\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        rowDelete(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw + "&" + $(this).attr("data-url"));
        return false;
    });

    //Đăng ký button hiển thị
    $(selector + " [data-event=\"show\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        rowShow(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });

    function escapeHTML(str) {
        var div = document.createElement("div");
        var text = document.createTextNode(str);
        div.appendChild(text);
        return div.innerHTML;
    }
    //Đăng ký button ẩn
    $(selector + " [data-event=\"hide\"]").click(function () {
        var linkFw = "#Page=" + getParameters("Page", 1) + "&RowPerPage=" + getParameters("RowPerPage", 10);
        rowHide(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });

    //đăng ký Thêm row
    $(selector + " [data-event=\"add\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Thêm mới bản ghi";
        var urlRequest;
        if (urlForm.indexOf("?") > 0)
            urlRequest = urlForm + "&do=add&ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + "?do=add&ItemId=" + $(this).attr("href").substring(1);
        FdiDialog(urlRequest, titleDiag);
        return false;
    });

    $(selector + " [data-event=\"addMenu\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Thêm mới bản ghi";

        var urlRequest;
        if (urlForm.indexOf("?") > 0)
            urlRequest = urlForm + "&do=add&ItemId=" + $(this).attr("href").substring(1) + "&groupId=" + $(this).attr("data-groupid");
        else
            urlRequest = urlForm + "?do=add&ItemId=" + $(this).attr("href").substring(1) + "&groupId=" + $(this).attr("data-groupid");
        FdiDialog(urlRequest, titleDiag);
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

        FdiDialog(urlRequest, titleDiag);
        return false;
    });

    //đăng ký sửa row
    $(selector + " [data-event=\"edit\"]").click(function () {

        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Sửa thông tin bản ghi";
        var urlRequest;
        if (urlForm.indexOf("?") > 0)
            urlRequest = urlForm + "&do=edit&ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + "?do=edit&ItemId=" + $(this).attr("href").substring(1);
        FdiDialog(urlRequest, titleDiag);
        return false;
    });
    //đăng ký sửa row
    $(selector + " [data-event=\"pay\"]").click(function () {

        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Thanh toán";
        var urlRequest;
        if (urlForm.indexOf("?") > 0)
            urlRequest = urlForm + "&do=add&ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlForm + "?do=add&ItemId=" + $(this).attr("href").substring(1);
        FdiDialog(urlRequest, titleDiag);
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

        FdiDialog(urlRequest, titleDiag);

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

        FdiDialog(urlRequest, titleDiag);

        return false;
    });

    // Gán Role cho Module
    $(selector + " .gridView [data-event=\"rolemodule\"]").click(function () {
        var titleDiag = $(this).attr("title");
        if (titleDiag === "")
            titleDiag = "Gán Role cho Module";

        var urlRequest;
        if (urlFormModule.indexOf("?") > 0)
            urlRequest = urlFormModule + "?ItemId=" + $(this).attr("href").substring(1);
        else
            urlRequest = urlFormModule + "?ItemId=" + $(this).attr("href").substring(1);

        FdiDialog(urlRequest, titleDiag);

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

        FdiDialog(urlRequest, titleDiag);

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

        FdiDialog(urlRequest, titleDiag);

        return false;
    });

    //đăng ký xem row
    $(selector + " [data-event=\"view\"]").click(function () {
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

        FdiDialogView(urlRequest, titleDiag);

        return false;
    });
    $(selector + ' .gridView [data-event="restop"]').click(function () {
        var linkFw = '#Page=' + getParameters("Page", 1) + '&RowPerPage=' + getParameters("RowPerPage", 10);
        rowReStop(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });
    //dừng kinh doanh
    $(selector + ' .gridView [data-event="stop"]').click(function () {
        var linkFw = '#Page=' + getParameters("Page", 1) + '&RowPerPage=' + getParameters("RowPerPage", 10);
        rowStop(urlPostAction, $(this).attr("href").substring(1), escapeHTML($(this).attr("title")), linkFw);
        return false;
    });
}

function rowReStop(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        titleDia = "Cho phép kinh doanh";
        swal({
            title: titleDia,
            text: rowTitle,
            type: "info",
            showCancelButton: true,
            confirmButtonClass: "btn-success",
            confirmButtonText: "Xác nhận",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    $.post(encodeURI(urlPost), { "do": "show", "ShopId": $("#selectShop").val(), "itemId": "" + arrRowId + "" }, function (data) {
                        if (data.Erros) {
                            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                        } else {
                            createCloseMessage("Thông báo", data.Message, "");
                        }
                    });
                }
                swal.close();
            });
    }
}

function rowStop(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        titleDia = "Bạn có chắc chắn dừng kinh doanh";
        swal({
            title: titleDia,
            text: rowTitle,
            type: "info",
            showCancelButton: true,
            confirmButtonClass: "btn-success",
            confirmButtonText: "Xác nhận",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    $.post(encodeURI(urlPost), { "do": "hide", "ShopId": $("#selectShop").val(), "itemId": "" + arrRowId + "" }, function (data) {
                        if (data.Erros) {
                            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                        } else {
                            createCloseMessage("Thông báo", data.Message, "");
                        }
                    });
                }
                swal.close();
            });
    }
}

function rowHide(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Ẩn các bản ghi đã chọn";
        else
            titleDia = "Ẩn các bản ghi đã chọn";
        swal({
            title: titleDia,
            text: "",
            type: "info",
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
                        } else {
                            createCloseMessage("Thông báo", data.Message, "");
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
                            createCloseMessage("Thông báo", data.Message, "");
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
                            createCloseMessage("Thông báo", data.Message, "");
                        }
                    });
                }
                swal.close();
            });
    }
}

function rowShow(urlPost, arrRowId, rowTitle, urlFw) {
    var titleDia;
    if (arrRowId === "")
        createMessage("Thông báo", "Bạn chưa chọn bản ghi nào");
    else {
        if (arrRowId.indexOf(",") > 0)
            titleDia = "Hiện thị các bản ghi đã chọn";
        else
            titleDia = "Hiện thị các bản ghi đã chọn";

        swal({
            title: titleDia,
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-success",
            confirmButtonText: "Hiện thị",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },
            function (isConfirm) {
                if (isConfirm) {
                    $.post(encodeURI(urlPost), { "do": "show", "itemId": "" + arrRowId + "" }, function (data) {
                        if (data.Erros) {
                            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
                        }
                        else {
                            swal.close();
                            createCloseMessage("Thông báo", data.Message, "");
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
            text: "",
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
                            createCloseMessage("Thông báo", data.Message, "");
                        }
                    });
                } else {
                    swal.close();
                }
            });
    }
}

function trim12(str) {
    str = str.replace(/^\s\s*/, "");
    var ws = /\s/,
        i = str.length;
    while (ws.test(str.charAt(--i)));
    return str.slice(0, i + 1);
}
function createCloseMessage(title, message, urlFw) {
    var linkFW = '#Keyword=' + getParameters("Keyword", "") + '&SearchIn=' + getParameters("SearchIn", "") + '&Page=' + getParameters("Page", 1) + '&RowPerPage=' + getParameters("RowPerPage", 50) + '&temp=' + Math.random().toString(36).substring(7) + urlFw;
    linkFW = linkFW.replace(/<\/?\w(?:[^"'>]|"[^"]*"|'[^']*')*>/g, "");
    toastr["success"](message);
    window.location.href = linkFW;
}
function redirect(urlFw) {
    var linkFW = '#Keyword=' + getParameters("Keyword", "") + '&SearchIn=' + getParameters("SearchIn", "") + '&Page=' + getParameters("Page", 1) + '&RowPerPage=' + getParameters("RowPerPage", 50) + '&temp=' + Math.random().toString(36).substring(7) + urlFw;
    linkFW = linkFW.replace(/<\/?\w(?:[^"'>]|"[^"]*"|'[^']*')*>/g, "");
    window.location.href = linkFW;
}

function createShowMessage(title, message) {
    toastr["success"](message);
}

function ErrorMessage(message) {
    toastr["error"](message);
}

function Parameters(url, name, valueDefault) {
    var page = valueDefault;
    if (url.indexOf("?") > 0) {
        url = url.replace("#", "&");
    } else {
        url = url.replace("#", "?");
    }
    var obj = $.url(url).param();
    if (obj[name] !== null) {
        page = obj[name];
    }
    return page;
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
    if (obj[name] !== null) {
        page = obj[name];
    }
    return page;
}

function callSelect2(id, w) {
    $(id).select2({
        width: w,
        maximumSelectionLeng: 1
    });
}

function PostAction(id) {
    $("#btnSave").prop('disabled', true);
    $.post(urlPostAction, $(id).serialize(), function (data) {
        $("#btnSave").prop('disabled', false);
        if (data.Erros)
            createMessage("Đã có lỗi xảy ra", data.Message); // Tạo thông báo lỗi
        else {
            $("#dialog-form").modal('hide'); //Đóng form thêm mới - sửa            
            createCloseMessage("Thông báo", data.Message, ""); // Tạo thông báo khi click đóng thì chuyển đến url đích
        }
    });
    return false;
};

function PostActionUserid(id) {
    $("#btnSave").prop('disabled', true);
    $.post(urlPostAction, $(id).serialize(), function (data) {
        $("#btnSave").prop('disabled', false);
        if (data.Erros)
            createMessage("Đã có lỗi xảy ra", data.Message); // Tạo thông báo lỗi
        else {
            $("#dialog-form").modal('hide'); //Đóng form thêm mới - sửa
            createCloseMessage("Thông báo", data.Message, "&userid=" + data.ID); // Tạo thông báo khi click đóng thì chuyển đến url đích
        }
    });
    return false;
};

function ChartsLine(id, name, xlable, bindData) {
    var chartline = document.getElementById(id).getContext('2d');
    var chart = new Chart(chartline, {
        type: 'line',
        data: {
            labels: xlable,
            datasets: [
                {
                    label: name,
                    backgroundColor: 'rgba(255, 185, 0, 0.3)',
                    borderColor: 'rgba(255, 185, 0, 1)',
                    data: bindData,
                    lineTension: 0,
                }]
        },
        options: {
        }
    });
}

function ChartsLineMutil(id, xlable, bindData) {
    var chartline = document.getElementById(id).getContext('2d');
    var dataset = bindData;
    var chart = new Chart(chartline, {
        type: 'line',
        data: {
            labels: xlable,
            datasets: dataset,
        },
        options: {}
    });
}

function ChartsBar(id, name, xlable, bindData) {
    var chartBar = document.getElementById(id).getContext('2d');
    var myChart = new Chart(chartBar, {
        type: 'bar',
        data: {
            labels: xlable,
            datasets: [{
                label: name,
                data: bindData,
                backgroundColor: "rgba(54, 162, 235,0.8)",
                borderColor: "rgb(54, 162, 235)",
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}
function chartsPie(id, name, xlable, bindData) {
    var ctx = document.getElementById(id).getContext("2d");
    var config = {
        type: 'pie',
        data: {
            datasets: [{
                data: bindData,
                backgroundColor: [
                    window.chartColors.blue,
                    window.chartColors.orange,
                    window.chartColors.red,
                    window.chartColors.green,
                    window.chartColors.yelloworange,
                    window.chartColors.purple,
                    window.chartColors.yellow,
                    window.chartColors.grey,
                    window.chartColors.redorange,
                    window.chartColors.violet,
                    window.chartColors.yellowgreen,
                    window.chartColors.bluewgreen,

                ],
                label: name,
            }],
            labels: xlable,
        },
        options: {
            responsive: true
        }
    };
    window.myPie = new Chart(ctx, config);
}