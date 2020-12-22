$(".packet").appendTo(".pull-left");
function SpaLoad(urlnode, nm, td, agencyid, timedo) {
    var lstc = [];
    var lsto = [];
    var listrc = JSON.parse($('#quantityroomcount').val());
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
    $('#quantityroomcount').val("");

    socket.on('server-schedulein-main', function (data) {
        // Check in
        if (data.ds.length > 0) {
            if (data.ds[1] == "1") {
                deleteclass(data.ds[0]);
                $("." + data.ds[0]).removeClass("not-active");
                $("." + data.ds[0]).addClass("blue");
                $("." + data.ds[0]).appendTo("#content-blue");
            } else {
                deleteclass("." + data.ds[0]);
                $("." + data.ds[0]).addClass("not-active");
                $("." + data.ds[0]).appendTo("#content-not-active");
            }
        }
        // Active Schedule
        if (data.aid === agencyid) {
            $.post("/Spa/KeyOrder?agencyid=" + agencyid, function () { });
            window.location.reload();
        }
    });
    socket.on('server-exchange-main', function (data) {
        var a = -1, b = -1;
        for (var i = 0; i < list.length; i++) {
            if (list[i].ID == parseInt(data.bedid)) {
                a = i;
                break;
            }
        }
        for (i = 0; i < list.length; i++) {
            if (list[i].ID == parseInt(data.bedeid)) {
                b = i;
                break;
            }
        }
        if (a >= 0 && b >= 0) {
            $("#" + list[a].DN_User_BedDesk.UserID).appendTo(".detailu-" + list[b].ID);
            $("#" + list[b].DN_User_BedDesk.UserID).appendTo(".detailu-" + list[a].ID);
            $("#dashboard-" + list[a].ID).addClass(list[b].DN_User_BedDesk.UserID);
            $("#dashboard-" + list[a].ID).removeClass(list[a].DN_User_BedDesk.UserID);
            $("#dashboard-" + list[b].ID).removeClass(list[b].DN_User_BedDesk.UserID);
            $("#dashboard-" + list[b].ID).addClass(list[a].DN_User_BedDesk.UserID);
            var obj = list[a].DN_User_BedDesk;
            list[a].DN_User_BedDesk = list[b].DN_User_BedDesk;
            list[b].DN_User_BedDesk = obj;
        }
    });
    setInterval(function () {
        var dns = intDateNow() - nm;
        showhhmmss(dns - td, "#timedatenow");
        if (!$(id).hasClass("not-active")) {
            var i = lsto.length;
            while (i--) {
                var c = "#order-" + lsto[i].BedDeskID;
                var oc = "#ordercount-" + lsto[i].BedDeskID;
                //var idBed = $("#Idbed-" + lsto[i].BedDeskID).val();
                bincount(c, oc, lsto, td, dns, timedo, lsto[i].BedDeskID);
                var id = "#dashboard-" + lsto[i].BedDeskID;
                if (lsto[i].StartDate - timedo <= dns && lsto[i].StartDate >= dns && lsto[i].EndDate >= dns) {
                    if (!$(id).hasClass("red")) {
                        deleteclass(id);
                        $("#btnResetPay-" + lsto[i].BedDeskID).show();
                        $("#counterOrder-" + lsto[i].BedDeskID).html("");
                        $(id).addClass("red");
                        $(id).appendTo("#content-red");
                    }
                    secondsToMs(lsto[i].EndDate, lsto[i].BedDeskID, lsto[i].Minute, lsto[i].StartDate, td, dns);
                }
                if (lsto[i].StartDate <= dns && lsto[i].EndDate >= dns) {

                    secondsToMs(lsto[i].EndDate, lsto[i].BedDeskID, lsto[i].Minute, lsto[i].StartDate, td, dns);
                    if (lsto[i].IsEarly && lsto[i].EndDate - dns < timedo) {
                        if (!$(id).hasClass("yellow")) {
                            deleteclass(id);
                            $(id).addClass("yellow");
                        }
                    } else if (!$(id).hasClass("red")) {

                        deleteclass(id);
                        $(id).addClass("red");
                        $("#btnResetPay-" + lsto[i].BedDeskID).show();
                        $(id).appendTo("#content-red");
                    }
                }
                if (lsto[i].EndDate <= dns) {
                    if (!$(id).hasClass("blue")) {

                        deleteclass(id);
                        $(id).addClass("blue");
                        $("#counterOrder-" + lsto[i].BedDeskID).html("");
                        $("#counterOrder-" + lsto[i].BedDeskID + "-").html("");
                        $("#btnResetPay-" + lsto[i].BedDeskID).hide();
                        $(id).appendTo("#content-blue");
                        $(c).html("");
                        $(oc).html("");
                    }
                    lsto.splice(i, 1);
                }
            }
            i = lstc.length;
            while (i--) {
                c = "#contact-" + lstc[i].BedDeskID;
                oc = "#contactcount-" + lstc[i].BedDeskID;
                var idBeds = $("#Idbed-" + lstc[i].BedDeskID).val();
                bincount(c, oc, lstc, td, dns, timedo, lstc[i].BedDeskID);
                id = "#dashboard-" + lstc[i].BedDeskID;
                if (lstc[i].StartDate - timedo <= dns && lstc[i].StartDate >= dns && lstc[i].EndDate >= dns) {

                    if (!$(id).hasClass("gold")) {
                        deleteclass(id);
                        $("#counterOrder-" + lstc[i].BedDeskID).html("");
                        $("#btnResetPay-" + lstc[i].BedDeskID).show();
                        $(id).addClass("gold");
                        $(id).appendTo("#content-gold");
                    }
                    secondsToMs(lstc[i].EndDate, lstc[i].BedDeskID, lstc[i].Minute, lstc[i].StartDate, td, dns);
                }

                if (lstc[i].StartDate <= dns && lstc[i].EndDate >= dns) {

                    if (!$(id).hasClass("gold") && !$(id).hasClass("red")) {
                        $(id).addClass("gold");
                        $("#btnResetPay-" + lstc[i].BedDeskID).show();
                        $(id).appendTo("#content-gold");
                    }
                    secondsToMs(lstc[i].EndDate, lstc[i].BedDeskID, lstc[i].Minute, lstc[i].StartDate, td, dns);
                } else if (lstc[i].EndDate <= dns) {
                    if (!$(id).hasClass("blue") && !$(id).hasClass("yellow") && !$(id).hasClass("red")) {

                        deleteclass(id);
                        $(id).addClass("blue");
                        $("#counterOrder-" + lstc[i].BedDeskID).html("");
                        $("#counterOrder-" + lstc[i].BedDeskID + "-").html("");
                        $("#btnResetPay-" + lstc[i].BedDeskID).hide();
                        $(c).html("");
                        $(oc).html("");
                        $(id).appendTo("#content-blue");
                    }
                    lstc.splice(i, 1);
                }
            }
        }
    }, 1000);
    socket.on('server-ready-main', function (data) {
        if (data.ds == "red") {
            for (var i = 0; i < lsto.length ; i++) {
                if (lsto[i].ID == data.bid.ID) {
                    lsto.splice(i, 1);
                    var o = "#order-" + data.bid.BedDeskID;
                    $(o).html("");
                    $("#ordercount-" + data.bid.BedDeskID).html("");
                    $("#counterOrder-" + data.bid.BedDeskID).html("");
                    $("#counterOrder-" + data.bid.BedDeskID + "-").html("");
                    var id = "#dashboard-" + data.bid.BedDeskID;
                    deleteclass(id);
                    $(id).addClass("blue");
                }
            }
        }
        var c = "#contact-" + data.bid.BedDeskID;
        if (data.ds == "gold") {
            for (var j = 0; j < lstc.length; j++) {
                if (lstc[j].ID == data.bid.ID) {
                    $(c).html("");
                    $("#contactcount-" + data.bid.BedDeskID).html("");
                    $("#counterOrder-" + data.bid.BedDeskID).html("");
                    $("#counterOrder-" + data.bid.BedDeskID + "-").html("");
                    var id = "#dashboard-" + lstc[j].BedDeskID;
                    deleteclass(id);
                    $(id).addClass("blue");
                    lstc.splice(j, 1);
                    break;
                }
            }
        }
        if (data.ds == "blue") {
            for (var k = 0; k < lstc.length; k++) {
                if (lstc[k].ID == data.bid.ID) {
                    $(c).html("");
                    $("#contactcount-" + data.bid.BedDeskID).html("");
                    $("#counterOrder-" + data.bid.BedDeskID).html("");
                    $("#counterOrder-" + data.bid.BedDeskID + "-").html("");
                    var id = "#dashboard-" + lstc[k].BedDeskID;
                    deleteclass(id);
                    $(id).addClass("blue");
                    lstc.splice(k, 1);
                    break;
                }
            }
        }

    });
    socket.on('server-count-main', function (data) {
        if (data.aid == "red") {
            lsto.push(data.bedid);
        }
        if (data.aid == "gold") {
            lstc.push(data.bedid);
        }
    });
    var list = JSON.parse($('#agencyid').val());
    $('#agencyid').val("");
    $("[data-event='Order']").click(function () {
        var listobj = [];
        for (var i = 0; i < list.length; i++) {
            if (list[i].ID == parseInt($(this).data("id"))) {
                if ($("#ValuePacket").val().length == 0) {
                    $("#ValuePacket").val($(this).data("packet"));
                }
                listobj.push(list[i]);
                $("#listbed").val(JSON.stringify(listobj));
                break;
            }
        }
        FdiOpenDialog("#dialog-form-3", "/Spa/AjaxOrders?ItemId=" + $(this).data("id"), "Trạng thái");
    });
    $("[data-event='Exchange']").click(function () {
        var obj = [];
        for (var i = 0; i < list.length; i++) {
            if (list[i].ID == parseInt($(this).data("id"))) {
                obj = list[i];
                break;
            }
        }
        FdiOpenDialog("#dialog-form-3", "/Spa/AjaxExchange?id=" + $(this).data("id") + "&end=" + obj.DN_User_BedDesk.DN_Weekly_Schedule.ScheduleTimeEnd, "Đổi giường" + obj.Name);
    });
    $("[data-event='Pay']").click(function () {
        var listobj = [];
        for (var i = 0; i < list.length; i++) {
            if (list[i].ID == parseInt($(this).data("id"))) {
                if ($("#ValuePacket").val().length == 0) {
                    $("#ValuePacket").val($(this).data("packet"));
                }
                listobj.push(list[i]);
                $("#listbed").val(JSON.stringify(listobj));
                break;
            }
        }
        FdiOpenDialog("#dialog-form-3", "/Spa/AjaxView?ItemId=" + $(this).data("id"), "Trạng thái");
    });
    $("[data-event='ResetPay']").click(function () {
        var idBed = $(this).attr("data-id");
        rowDelete("/Spa/Actions", idBed, "Đơn hàng", "");
    });


    $("[data-event='LevelRoom']").click(function () {
        var id = $(this).attr("data-id");
        if ($(".activeroom").attr("data-id") != id) {
            $(".item-massage").hide();
            $(".LevelRoom").removeClass("active").removeClass("activeroom");
            $(this).addClass("active").addClass("activeroom");
            var toSearch = $("#Keywordbed").val();
            if (toSearch.length > 0) searchKeywordbed(toSearch);
            else {
                toSearch = $("#Keyword").val();
                if (toSearch.length > 0) searchKeyword(toSearch);
                else {
                    toSearch = $("#Keywordquantity").val();
                    if (toSearch.length > 0) searchKeywordquantity(toSearch);
                    else {
                        var listn = search();
                        for (var i = 0; i < listn.length; i++) {
                            $("#dashboard-" + listn[i].ID).show();
                        }
                    }
                }
            }
        }
    });
    $("#QuantityPacket").keyup(function () {
        var count = parseInt($("#QuantityPacket").val());
        var a = $('.checkboxbed:checkbox:checked').length;
        var sl = $(".checkboxbed:checkbox[name=checkbedid]:visible").length;
        if (count < a) {
            $("#QuantityPacket").val(a);
            ErrorMessage(" Vượt quá số lượng đã tích!");
        }
    });
    $("[data-event='PayAll']").click(function () {
        var count = parseInt($("#QuantityPacket").val());
        if (count > 0) listbed("/Spa/AjaxView?ItemId=");
        else ErrorMessage("Bạn chưa chọn số lượng!");
    });
    $("[data-event='OrderAll']").click(function () {
        var count = parseInt($("#QuantityPacket").val());
        if (count > 0)
            listbed("/Spa/AjaxOrders?ItemId=");
        else ErrorMessage("Bạn chưa chọn số lượng!");
    });
    function search() {
        var listnew = [];
        var a = $(".LevelRoom").hasClass("activeroom");
        var b = $(".packet").hasClass("activebed");
        var ha = $("#HoursStartAll").val();
        var ma = $("#MinuteStartAll").val();
        var dates = td + ha * 3600 + ma * 60;
        var dns = intDateNow() - nm;
        if (dates <= dns) {
            ErrorMessage("Thời gian nhỏ hơn thời gian hiện tại!");
            if ($("#groupbed").html().length > 0) $("#groupbed .item-massage .checkboxbed").hide();
            else $("#objbed .item-massage .checkboxbed").hide();
        }
        var mp = parseInt($("#ValuePacket").val());
        var idp = $(".activebed").attr("data-id");
        var idr = $(".activeroom").attr("data-id");
        if (a && b) {
            for (var i = 0; i < list.length; i++) {
                if (list[i].PacketId == parseInt(idp) && list[i].LevelRoomId == parseInt(idr)) {
                    listnew.push(list[i]);
                    if (dates > dns) datenowcheck(list[i].ID, dates, mp);
                }
            }
        }
        else if (a) {
            for (i = 0; i < list.length; i++) {
                if (list[i].LevelRoomId == parseInt(idr)) listnew.push(list[i]);
            }
        } else if (b) {
            for (i = 0; i < list.length; i++) {
                if (list[i].PacketId == parseInt(idp)) {
                    listnew.push(list[i]);
                    if (dates > dns) datenowcheck(list[i].ID, dates, mp);
                }
            }
        } else listnew = list;
        return listnew;
    }
    $("#Keyword").keyup(function () {
        $("#Keywordbed").val("");

        var toSearch = $(this).val().toLowerCase();
        if (toSearch.length > 0) {
            $(".item-massage").hide();
            searchKeyword(toSearch);
        } else $(".item-massage").show();
    });
    $("#Keywordbed").keyup(function () {
        $("#Keyword").val("");

        var toSearch = $(this).val().toLowerCase();
        if (toSearch.length > 0) {
            $(".item-massage").hide();
            searchKeywordbed(toSearch);
        } else $(".item-massage").show();
    });
    function searchKeywordbed(toSearch) {
        var listn = search();
        for (var i = 0; i < listn.length; i++) {
            if (listn[i].Name.indexOf(toSearch) != -1) $("#dashboard-" + listn[i].ID).show();
        }
    }
    function searchKeyword(toSearch) {
        var listn = search();
        for (var i = 0; i < listn.length; i++) {
            if (listn[i].DN_User_BedDesk != null && (listn[i].DN_User_BedDesk.UserName.toLowerCase().indexOf(toSearch) != -1 || listn[i].DN_User_BedDesk.FullName.toLowerCase().indexOf(toSearch) != -1)) $("#dashboard-" + listn[i].ID).show();
        }
    }
    function searchKeywordquantity(toSearch) {
        $("#Keywordbed").val("");
        $("#Keyword").val("");
        $("#groupbed").html("");
        var listn = search();
        if (toSearch.length == 0) {
            $("#objbed").show();
            for (var i = 0; i < listn.length; i++) {
                $("#dashboard-" + listn[i].ID).show();
            }
        } else {
            $("#objbed").hide();
            var count = parseInt(toSearch);
            for (i = 0; i < listrc.length; i++) {
                if (listrc[i].Count == count) {
                    var gid = listrc[i].ID;
                    var check = true;
                    var l = listrc[i].LiInts;
                    for (var j = 0; j < l.length; j++) {
                        for (var n = 0; n < listn.length; n++) {
                            if (listn[n].ID == l[j]) {
                                if (check) {
                                    var group = '<div class="groupbedshow show-' + (count > 5 ? 6 : count) + '" id="groupbedshow-' + gid + '" data-id="' + gid + '"><div id="content-blueg-' + gid + '"></div><div id="content-goldg-' + gid + '"></div><div id="content-redg-' + gid + '"></div><div id="content-not-activeg-' + gid + '"></div></div>';
                                    $("#groupbed").append(group);
                                    check = false;
                                }
                                objcup("#dashboard-" + l[j], l[j], gid);
                            }
                        }
                    }
                }
            }
        }
        $(".groupbedshow").click(function () {
            var dns = intDateNow() - nm;
            var ma = parseInt($("#MinuteStartAll").val());
            var ha = parseInt($("#HoursStartAll").val());
            var dates = td + ha * 3600 + ma * 60;
            if (dates < dns) ErrorMessage("Thời gian nhỏ hơn thời gian hiện tại!");
            else {
                var ct = parseInt($("#QuantityPacket").val());
                if (ct > 0) {
                    var idg = $(this).data("id");
                    var c = ".g-" + idg + ":checkbox";
                    var a = $('.checkboxbed:checkbox:checked').length;
                    $(c).each(function () {
                        var ic = c + "[id=" + $(this).attr('id') + "]";
                        $(ic).prop("checked", a < ct && !$(ic).prop("checked"));
                        a++;
                    });
                } else ErrorMessage("Bạn chưa chọn số lượng!");
            }
        });
    }
    function datenowcheck(bid, dates, mp) {
        var datae = dates + mp * 60;
        var c = -1;
        var ltso = listbybed(lsto, bid);
        for (var n = 0; n < ltso.length; n++) {
            if (ltso[n].StartDate <= dates && (ltso[n].IsEarly ? ltso[n].EndDate - timedo : ltso[n].EndDate) >= dates || ltso[n].StartDate <= datae && (ltso[n].IsEarly ? ltso[n].EndDate - timedo : ltso[n].EndDate) >= datae || ltso[n].StartDate >= dates && ltso[n].StartDate <= datae || (ltso[n].IsEarly ? ltso[n].EndDate - timedo : ltso[n].EndDate) >= dates && (ltso[n].IsEarly ? ltso[n].EndDate - timedo : ltso[n].EndDate) <= datae) {
                c = n;
                break;
            }
        }
        if (c < 0) {
            var ltsc = listbybed(lstc, bid);;
            for (n = 0; n < ltsc.length; n++) {
                if (ltsc[n].StartDate <= dates && (ltsc[n].IsEarly ? ltsc[n].EndDate - timedo : ltsc[n].EndDate) >= dates || ltsc[n].StartDate <= datae && (ltsc[n].IsEarly ? ltsc[n].EndDate - timedo : ltsc[n].EndDate) >= datae || ltsc[n].StartDate >= dates && ltsc[n].StartDate <= datae || (ltsc[n].IsEarly ? ltsc[n].EndDate - timedo : ltsc[n].EndDate) >= dates && (ltsc[n].IsEarly ? ltsc[n].EndDate - timedo : ltsc[n].EndDate) <= datae) {
                    c = n;
                    break;
                }
            }
        }
        if (c < 0 && !$("#dashboard-" + bid).hasClass("not-active")) {
            if ($("#groupbed").html().length > 0) $("#groupbed .item-massage #checkbedid-" + bid).show();
            else $("#objbed .item-massage #checkbedid-" + bid).show();
        }
    }
    function listbed(url) {
        var yourArray = [];
        var lst = [];
        var lcheck = $('.checkboxbed:checkbox:checked');
        var count = parseInt($("#QuantityPacket").val());
        if (lcheck.length > 0) {
            lcheck.each(function () {
                yourArray.push($(this).val());
                for (var i = 0; i < list.length; i++) {
                    if (list[i].ID == parseInt($(this).val())) {
                        lst.push(list[i]);
                        break;
                    }
                }
            });
        } else {
            var j = 0;
            $(".checkboxbed:checkbox[name=checkbedid]:visible").each(function () {
                if (j < count) {
                    j++;
                    yourArray.push($(this).val());
                    for (var i = 0; i < list.length; i++) {
                        if (list[i].ID == parseInt($(this).val())) {
                            lst.push(list[i]);
                            break;
                        }
                    }
                }
            });
        }
        if (yourArray.length > 0) {
            $("#listbed").val(JSON.stringify(lst));
            FdiOpenDialog("#dialog-form-3", url + yourArray.join(","), "Trạng thái");
        } else ErrorMessage("Bạn chưa chọn giường!");
    }
    function objcup(id, i, gid) {
        $(id).show();
        if ($(id).hasClass("blue")) $(id).clone().appendTo("#content-blueg-" + gid);
        if ($(id).hasClass("red")) $(id).clone().appendTo("#content-redg-" + gid);
        if ($(id).hasClass("gold")) $(id).clone().appendTo("#content-goldg-" + gid);
        if ($(id).hasClass("not-active")) $(id).clone().appendTo("#content-not-activeg-" + gid);
        else $(".groupbedshow #checkbedid-" + i).addClass('g-' + gid);
    }
}