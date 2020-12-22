$(".packet").appendTo(".pull-left");
$(".class-room").appendTo("#lroom");
function RestaurantLoad(urlnode, nm, td, agencyid, timedo, stp) {
    
    var lsto = [];
    var lstc = [];
    var listselected = [];
    var socket = io(urlnode);
    socket.emit("client-listorder-main", { id: agencyid });
    socket.emit("client-listcontact-main", { id: agencyid });
    
    socket.on('server-listorder-main', function (data) {
        if (lsto.length == 0 && agencyid == data.aid)
            lsto = data.list;
    });
    socket.on('server-listcontact-main', function (data) {
        if (lstc.length == 0 && agencyid == data.aid)
            lstc = data.list;
    });
    socket.emit("client-load-main");
    $.contextMenu({
        selector: '.free-desk .box-item-desk',
        callback: function (key, options) {
            var a = parseInt($(this).data("id"));
            var ocid = $(this).data("ocid");
            if (key == "add") add(a, ocid);
            if (key == "edit") edit(a);
        },
        items: {
            "add": { name: "Gọi món", icon: "fa-plus", accesskey: "a" },
            "edit": { name: "Đặt bàn", icon: "fa-shopping-cart", accesskey: "e" },
        }
    });
    $.contextMenu({
        selector: '.serving-desk .box-item-desk',
        callback: function (key, options) {
            var a = parseInt($(this).data("id"));
            var ocid = $(this).data("ocid");
            if (key == "add") add(a, ocid);
            if (key == "edit") edit(a);
            if (key == "delete") deleted(ocid.toString());
            if (key == "pay") {
                pay(ocid);
            }
        },
        items: {
            "add": { name: "Gọi món", icon: "fa-plus", accesskey: "a" },
            "edit": { name: "Đặt bàn", icon: "fa-shopping-cart", accesskey: "e" },
            "pay": { name: "In, thanh toán", icon: "fa-credit-card", accesskey: "t" },
            "delete": { name: "Hủy", icon: "delete" }
        }
    });
    $.contextMenu({
        selector: '.processing-desk .box-item-desk',
        callback: function (key, options) {
            var a = parseInt($(this).data("id"));
            var ocid = $(this).data("ocid");
            if (key == "add") add(a, ocid);
            if (key == "edit") edit(a);
            if (key == "delete") deleted(ocid.toString());
            if (key == "pay") pay(ocid);
        },
        items: {
            "add": { name: "Gọi món", icon: "fa-plus", accesskey: "a" },
            "edit": { name: "Đặt bàn", icon: "fa-shopping-cart", accesskey: "e" },
            "pay": { name: "In, thanh toán", icon: "fa-credit-card", accesskey: "t" },
            "delete": { name: "Hủy", icon: "delete" }
        }
    });

    $.contextMenu({
        selector: '.booking-desk .box-item-desk',
        callback: function (key, options) {
            var a = parseInt($(this).data("id"));
            var ocid = $(this).data("ocid");
            if (key == "view") view(ocid);
            if (key == "cto") cto(ocid);
            if (key == "edit") edit(a);
            if (key == "delete") deletedcontact(ocid.toString());
        },
        items: {
            "view": { name: "Thông tin ĐĐH", icon: "fa-credit-card", accesskey: "a" },
            "cto": { name: "Xử lý ĐĐH", icon: "fa-plus", accesskey: "a" },
            "edit": { name: "Đặt bàn", icon: "fa-shopping-cart", accesskey: "e" },
            "delete": { name: "Hủy", icon: "delete" }
        }
    });
    setInterval(function () {
        var dns = intDateNow() - nm;
        showhhmmss(dns - td, "#timedatenow");
        var i = lsto.length;
        while (i--) {
           var ocid = "#context-" + lsto[i].BedDeskID;
           var id = "#dashboard-" + lsto[i].BedDeskID;
           bincountrestaurant(c, oc, lstc, td, dns, timedo, lsto[i].BedDeskID);
            // báo sớm cho order
            if (lsto[i].StartDate - timedo <= dns && lsto[i].StartDate >= dns && lsto[i].EndDate >= dns) {
                if (!$(id).hasClass("serving-desk")) {
                    deleteclassnh(id);
                    $(ocid).data("ocid", lsto[i].ID);
                    $("#btnResetPay-" + lsto[i].BedDeskID).show();
                    $(id).addClass("serving-desk");
                }
                secondsToMsidentity(lsto[i].BedDeskID, lsto[i].StartDate, td, dns);
            }
            if (lsto[i].StartDate <= dns && lsto[i].EndDate >= dns) {
                secondsToMsidentity(lsto[i].BedDeskID, lsto[i].StartDate, td, dns);
                if (lsto[i].Status == stp) {
                    if (!$(id).hasClass("processing-desk")) {
                        deleteclassnh(id);
                        $(ocid).data("ocid", lsto[i].ID);
                        $(id).addClass("processing-desk");
                        $("#btnResetPay-" + lsto[i].BedDeskID).show();
                    }
                } else if (!$(id).hasClass("serving-desk")) {
                    deleteclassnh(id);
                    $(ocid).data("ocid", lsto[i].ID);
                    $(id).addClass("serving-desk");
                    $("#btnResetPay-" + lsto[i].BedDeskID).show();
                }
            } else if (lsto[i].EndDate < dns) {
                var c = "#contact-" + lsto[i].BedDeskID;
                $(c).html("");
                var oc = "#contactcount-" + lsto[i].BedDeskID;
                if (!$(id).hasClass("free-desk")) {
                    $(ocid).data("ocid", 0);
                    deleteclassnh(id);
                    $("#counterOrder-" + lsto[i].BedDeskID).html("");
                    $(id).addClass("free-desk");
                    $("#btnResetPay-" + lsto[i].BedDeskID).hide();
                    
                }
                bincountrestaurant(c, oc, lstc, td, dns, timedo, lsto[i].BedDeskID);
                lsto.splice(i, 1);                
            }
        }
        i = lstc.length;
        while (i--) {
            ocid = "#context-" + lstc[i].BedDeskID;
            id = "#dashboard-" + lstc[i].BedDeskID;
            bincountrestaurant(c, oc, lstc, td, dns, timedo, lstc[i].BedDeskID);
            // báo sớm cho order
            if (lstc[i].StartDate - timedo <= dns && lstc[i].StartDate >= dns && lstc[i].EndDate >= dns) {
                if (!$(id).hasClass("booking-desk")) {
                    deleteclassnh(id);
                    $(ocid).data("ocid", lstc[i].ID);
                    $("#counterOrder-" + lstc[i].BedDeskID).html("");
                    $("#btnResetPay-" + lstc[i].BedDeskID).show();
                    $(id).addClass("booking-desk");
                }
                secondsToMsidentity(lstc[i].BedDeskID, lstc[i].StartDate, td, dns);
               }
            if (!$(id).hasClass("processing-desk") && !$(id).hasClass("serving-desk") && lstc[i].StartDate <= dns && lstc[i].EndDate >= dns) {
                if (!$(id).hasClass("booking-desk")) {
                    deleteclassnh(id);
                    $(ocid).data("ocid", lstc[i].ID);
                    $(id).addClass("booking-desk");
                    $("#btnResetPay-" + lstc[i].BedDeskID).show();
                }
                secondsToMsidentity(lstc[i].BedDeskID, lstc[i].StartDate, td, dns);
            }
            if (lstc[i].EndDate < dns) {
                if (!$(id).hasClass("processing-desk") && !$(id).hasClass("serving-desk")) {
                    c = "#contact-" + lstc[i].BedDeskID;
                    $(c).html("");
                    oc = "#contactcount-" + lstc[i].BedDeskID;
                    if (!$(id).hasClass("free-desk")) {
                        $(ocid).data("ocid", 0);
                        deleteclassnh(id);
                        $("#counterOrder-" + lstc[i].BedDeskID).html("");
                        $(id).addClass("free-desk");
                        $("#btnResetPay-" + lstc[i].BedDeskID).hide();
                    }
                }                
                bincountrestaurant(c, oc, lstc, td, dns, timedo, lstc[i].BedDeskID);
                lstc.splice(i, 1);
            }
        }
    }, 1000);
    socket.on('server-ready-main', function (data) {
        if (data.ds == "serving-desk") {
            for (var j = 0; j < lsto.length; j++) {
                if (lsto[j].BedDeskID == data.bid.BedDeskID) {
                    lsto[j].EndDate = intDateNow() - nm;
                    break;
                }
            };
        }
        if (data.ds == "booking-desk") {
            for (j = 0; j < lstc.length; j++) {
                if (lstc[j].BedDeskID == data.bid.BedDeskID) {
                    lstc[j].EndDate = intDateNow() - nm;
                    break;
                }
            };
        }
    });
    socket.on('server-count-main', function (data) {
        if (data.aid == "serving-desk") {
            var check = false;
            for (var j = 0; j < lsto.length; j++) {
                if (lsto[j].ID == data.bedid.ID && lsto[j].BedDeskID == data.bedid.BedDeskID) {
                    lsto[j].Status = 1;
                    check = true;
                    break;
                }
            }
            if (!check) lsto.push(data.bedid);
        }
        if (data.aid == "booking-desk") {
            lstc.push(data.bedid);
        }
    });
    socket.on('server-status-main', function (data) {
       if (data.aid == "serving-desk") {
            for (var j = 0; j < lsto.length; j++) {
                if (lsto[j].ID == data.bedid.ID && lsto[j].BedDeskID == data.bedid.BedDeskID) {
                    lsto[j].Status = data.bedid.Status;
                    break;
                }
            }
        }
        if (data.aid == "booking-desk") {
            for (j = 0; j < lstc.length; j++) {
                if (lstc[j].ID == data.bedid.ID) {
                    lstc[j].Status = data.bedid.Status;
                    break;
                }
            }
        }
    });
    function cto(ocid) {
        FdiOpenDialog("#dialog-form-3", "/BookATable/AjaxContactToOrder?itemId=" + ocid, "Gọi món");
    };
    function add(id, ocid) {
        FdiOpenDialog("#dialog-form-4", "/BookATable/AjaxOrders?ItemId=" + id + "&id=" + ocid, "Gọi món");
    };
    function edit(id) {
        FdiOpenDialog("#dialog-form-3", "/BookATable/AjaxView?ItemId=" + id, "Đặt bàn");
    };
    function view(id) {
        FdiOpenDialog("#dialog-form-3", "/BookATable/AjaxForm?ItemId=" + id, "Đặt bàn");
    };
    function deleted(id) {
        rowDelete("/BookATable/Actions", id, "Đơn hàng", "");
    };
    function deletedcontact(id) {
        rowDelete("/BookATable/StopOrder", id, "Đơn hàng", "");
    };
    function pay(id) {
        FdiOpenDialog("#dialog-form-4", "/BookATable/AjaxPrint?ItemId=" + id, "Thanh toán");
    };
    var list = JSON.parse($('#agencyid').val());
    $('#agencyid').val("");
    $("[data-event='Default']").click(function () {
        $(".LevelRoom").removeClass("active").removeClass("activeroom");
        $(".item-desk").show();
        $(".gdesk").show();
        $(this).addClass("active");
        $("#Keywordbed").val("");
        var h = $("#HoursStartAll").val();
        var dateh = new Date();
        $(".item-desk").removeClass("selected-desk");
        listselected = [];
        dateh.setSeconds(300 - nm);
        if (dateh.getHours() != h) {
            $("#HoursStartAll").val(dateh.getHours()).trigger("change");
        }
        $("#MinuteStartAll").val(dateh.getMinutes()).trigger("change");
    });
    $("[data-event='LevelRoom']").click(function () {
        var id = $(this).attr("data-id");
        $(".gdesk").hide();
        $(".glevel-" + id).show();
        if ($(".activeroom").attr("data-id") != id) {
            $(".item-desk").hide();
            $(".LevelRoom").removeClass("active").removeClass("activeroom");
            $(this).addClass("active").addClass("activeroom");
            var toSearch = $("#Keywordbed").val();
            if (toSearch.length > 0) searchKeywordbed(toSearch);
            else {
                var listn = search();
                for (var i = 0; i < listn.length; i++) {
                    $("#dashboard-" + listn[i].ID).show();
                }
            }
        }
    });
    $("#HoursStartAll").change(function () {
        search();
    });
    $("#MinuteStartAll").change(function () {
        search();
    });
    $(".item-desk").click(function () {
        var id = $(this).data("id");
        if (!$(this).hasClass("booking-desk")) {
            if ($(this).hasClass("selected-desk")) {
                $(this).removeClass("selected-desk");
                for (var i = 0; i < listselected.length; i++) {
                    if (listselected[i] == id) {
                        listselected.splice(i, 1);
                        break;
                    }
                }
            } else {
                listselected.push(id);
                $(this).addClass("selected-desk");
            }
        } else ErrorMessage("Bàn đang được đặt!");
    });
    $("[data-event='PayAll']").click(function () {
        var dns = intDateNow() - nm;
        var ma = parseInt($("#MinuteStartAll").val());
        var ha = parseInt($("#HoursStartAll").val());
        var dates = td + ha * 3600 + ma * 60;
        if (dates < dns) {
            var datenow5 = new Date();
            datenow5.setMinutes(datenow5.getMinutes() + 5);
            $("#HoursStartAll").val(datenow5.getHours());
            $("#MinuteStartAll").val(datenow5.getMinutes());
        }

        listbed("/BookATable/AjaxView?ItemId=");
    });
    $("[data-event='Exchange']").click(function () {
        var obj = [];
        for (var i = 0; i < list.length; i++) {
            if (list[i].ID == parseInt($(this).data("id"))) {
                obj = list[i];
                break;
            }
        }
        FdiOpenDialog("#dialog-form-3", "/BookATable/AjaxExchange?id=" + $(this).data("id") + "&end=" + obj.DN_User_BedDesk.DN_Weekly_Schedule.ScheduleTimeEnd, "Đổi bàn" + obj.Name);
    });
    $("[data-event='OrderAll']").click(function () {
        listbed("/BookATable/AjaxOrders?ItemId=");
    });
    function search() {
        var listnew = [];
        var ha = $("#HoursStartAll").val();
        var ma = $("#MinuteStartAll").val();
        var dates = td + ha * 3600 + ma * 60;
        var dns = intDateNow() - nm;
        if (dates <= dns) {
            var datenow5 = new Date();
            datenow5.setMinutes(datenow5.getMinutes() + 5);
            $("#HoursStartAll").val(datenow5.getHours());
            $("#MinuteStartAll").val(datenow5.getMinutes());
        };
        var idr = $(".activeroom").attr("data-id");
        if ($(".LevelRoom").hasClass("activeroom")) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].LevelRoomId == parseInt(idr)) listnew.push(list[i]);
            }
        } else listnew = list;
        return listnew;
    }
    $("#Keywordbed").keyup(function () {
        var toSearch = $(this).val().toLowerCase();
        if (toSearch.length > 0) {
            $(".item-desk").hide();
            searchKeywordbed(toSearch);
        } else $(".item-desk").show();
    });
    function searchKeywordbed(toSearch) {
        var listn = search();
        for (var i = 0; i < listn.length; i++) {
            if (listn[i].Name.indexOf(toSearch) != -1) $("#dashboard-" + listn[i].ID).show();
        }
    }
    function listbed(url) {
        var id = 0;
        if (listselected.length > 0) {
            for (var i = 0; i < listselected.length; i++) {
                id = parseInt($("#context-" + listselected[i]).data("ocid"));
                if (id > 0) {
                    break;
                }
            }
            FdiOpenDialog("#dialog-form-4",  url + listselected + "&id=" + id, "Trạng thái");
        } else ErrorMessage("Bạn chưa chọn bàn!");
    }
}
function TotalSeconds(date) {
    return parseInt((date.getTime() - datedefault.getTime()) / 1000);
}
function Notifycation(urlnode, userid,agencid,codelogin) {
    var socket = io(urlnode);
    socket.on('server-approved-main', function (data, uid) {
        if (userid == uid){
           $("#header_inbox_bar .badge-default").html(parseInt($("#header_inbox_bar .badge-default").html()) + data.count);
        }
    });
    socket.emit("client-approved-main", userid, agencid);
    socket.emit("client-userId-online", userid, agencid, codelogin);
    socket.on('server-warehouse-online', function (data) {
        var obj = JSON.parse(data.obj);
        debugger;
        if (userid == data.uid) {
            var html = '<a href="/SaleSim/p2154c2159?id=' + obj.ID + '">'
                + '<span class="photo">' +
                +'<img src="/Content/Admin/images/no_image.gif" class="img-circle" alt="">' +
                '</span>'
                + '<span class="subject">'
                + '<span class="from"> ' + obj.Fullname + '</span>'
                + '<span class="time">vài giây trước</span>'
                + '</span><span class="message">' + obj.Note + '</span></a>';
            $(".vieworder").html(html + $(".vieworder").html());
            //var notify;
            //notify = new Notification(
            //            'Bạn có một thông báo mới từ ' +obj.Fullname , // Tiêu đề thông báo
            //            {
            //                body: 'Freetuts vừa đăng một bài viết mới.', // Nội dung thông báo
            //                icon: 'https://freetuts.net/public/logo/icon.png', // Hình ảnh
            //                tag: 'https://freetuts.net/' // Đường dẫn 
            //            }
            //    );
            //notify.onclick = function() {
            //    window.location.href = this.tag; // Di chuyển đến trang cho url = tag
            //};
        }

    });
}