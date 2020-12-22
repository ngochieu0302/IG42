$(".packet").appendTo(".pull-left");
function MassageLoad(urlnode, nm, td, agencyid, timedo) {
    var lsto = [];
    var listrc = JSON.parse($('#quantityroomcount').val());
    //NodeJS
    var socket = io(urlnode);
    socket.emit("client-listorder-main", { id: agencyid });
    socket.on('server-listorder-main', function (data) {
        if (lsto.length == 0 && agencyid == data.aid)
            lsto = data.list;
    });
    socket.emit("client-load-main");
    $('#quantityroomcount').val("");
    socket.on('server-ready-main', function (data) {
        var id;
        if (data.ds == "red") {
            for (var i = 0; i < lsto.length ; i++) {
                if (lsto[i].ID == data.bid.ID) {
                    lsto.splice(i, 1);
                    var o = "#order-" + data.bid.BedDeskID;
                    $(o).html("");
                    $("#ordercount-" + data.bid.BedDeskID).html("");
                    $("#counterOrder-" + data.bid.BedDeskID).html("");
                    $("#counterOrder-" + data.bid.BedDeskID + "-").html("");
                    id = "#dashboard-" + data.bid.BedDeskID;
                    $("#btnResetPay-" + data.bid.BedDeskID).hide();
                    $("#btnAddMinute-" + data.bid.BedDeskID).hide();
                    $("#user_Bill_" + data.bid.BedDeskID).html("");
                    deleteclass(id);
                    $(id).addClass("blue");
                    $(id).appendTo("#content-blue");
                }
            }
        }
    });
    socket.on('server-count-main', function (data) {
        if (data.aid == "red") lsto.push(data.bedid);
    });
    socket.on('server-add-main', function (data) {
        if (data.aid == "red") {
            for (var a = 0; a < lsto.length; a++) {
                if (lsto[a].ID == data.bedid.ID) {
                    lsto[a].StartDate = data.bedid.StartDate;
                    lsto[a].EndDate = data.bedid.EndDate;
                    break;
                }
            }
        }
    });
    //Chuyển trạng thái
    setInterval(function () {
        var dns = intDateNow() - nm;
        showhhmmss(dns - td, "#timedatenow");
        if (!$(id).hasClass("not-active")) {
            var i = lsto.length;
            while (i--) {
                var c = "#order-" + lsto[i].BedDeskID;
                var oc = "#ordercount-" + lsto[i].BedDeskID;
                bincount(c, oc, lsto, td, dns, timedo, lsto[i].BedDeskID);
                var id = "#dashboard-" + lsto[i].BedDeskID;
                var userstring;
                // Đang có đơn hàng
                if (lsto[i].StartDate - timedo <= dns && lsto[i].StartDate >= dns && lsto[i].EndDate >= dns && lsto[i].EndDate - dns > lsto[i].TimeWait) {
                    if (!$(id).hasClass("red")) {
                        deleteclass(id);
                        $("#btnResetPay-" + lsto[i].BedDeskID).show();
                        $("#btnAddMinute-" + lsto[i].BedDeskID).data("ido", lsto[i].ID);
                        $("#btnAddMinute-" + lsto[i].BedDeskID).show();
                        $("#counterOrder-" + lsto[i].BedDeskID).html("");
                        userstring = '<i class="fa fa-user"></i><b>' + lsto[i].UserName + '</b>';
                        $("#user_Bill_" + lsto[i].BedDeskID).html(userstring);
                        $(id).addClass("red");
                        $(id).appendTo("#content-red");
                    }
                    secondsToMs(lsto[i].EndDate, lsto[i].BedDeskID, lsto[i].Minute, lsto[i].StartDate, td, dns);
                }
                // Bán sớm
                if (lsto[i].StartDate <= dns && lsto[i].EndDate >= dns) {
                    deleteclass(id);
                    $("#btnResetPay-" + lsto[i].BedDeskID).show();
                    $("#btnAddMinute-" + lsto[i].BedDeskID).data("ido", lsto[i].ID);
                    $("#btnAddMinute-" + lsto[i].BedDeskID).show();
                    $("#counterOrder-" + lsto[i].BedDeskID).html("");
                    userstring = '<i class="fa fa-user"></i><b>' + lsto[i].UserName + '</b>';
                    $("#user_Bill_" + lsto[i].BedDeskID).html(userstring);
                    if (lsto[i].IsEarly && lsto[i].EndDate - dns <= lsto[i].TimeWait) {
                        if (!$(id).hasClass("gold")) {
                            $(id).addClass("gold");
                            $(id).appendTo("#content-gold");
                        }
                    } else if (!$(id).hasClass("red")) {
                        $(id).addClass("red");
                        $(id).appendTo("#content-red");
                    }
                    secondsToMs(lsto[i].EndDate, lsto[i].BedDeskID, lsto[i].Minute, lsto[i].StartDate, td, dns);
                }
                if (lsto[i].EndDate <= dns) {
                    if (!$(id).hasClass("blue")) {
                        deleteclass(id);
                        $("#user_Bill_" + lsto[i].BedDeskID).html("");
                        $(id).addClass("blue");
                        $("#counterOrder-" + lsto[i].BedDeskID).html("");
                        $("#counterOrder-" + lsto[i].BedDeskID + "-").html("");
                        $("#btnAddMinute-" + lsto[i].BedDeskID).hide();
                        $("#btnAddMinute-" + lsto[i].BedDeskID).data("ido", 0);
                        $("#btnResetPay-" + lsto[i].BedDeskID).hide();
                        $(id).appendTo("#content-blue");
                        $(c).html("");
                        $(oc).html("");
                    }
                    lsto.splice(i, 1);
                }
            }
        }
    }, 1000);
    var list = JSON.parse($('#agencyid').val());
    $('#agencyid').val("");
    $("[data-event='Order']").click(function () {
        var listobj = [];
        var packId = $(".activebed").data("id");
        if (packId == undefined || packId == null || packId == "") {
            packId = 0;
        }
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
        FdiOpenDialog("#dialog-form-3", "/Massage/AjaxOrders?ItemId=" + $(this).data("id") + "&packetId=" + packId, "Trạng thái");
    });
    $("[data-event='ResetPay']").click(function () {
        var idBed = $(this).attr("data-id");
        rowDelete("/Massage/Actions", idBed, "Đơn hàng", "");
    });
    $("[data-event='Default']").click(function () {
        $(".packet").removeClass("active").removeClass("activebed");
        $(".LevelRoom").removeClass("active").removeClass("activeroom");
        $(".item-massage").show();
        $("#PacketItem").val(0).trigger("change");
        $("#valuepaketshow").html("");
        $("#objbed").show();
        $("#groupbed").html("");
        $(this).addClass("active");
        $(".checkboxbed").attr("checked", false);
        $(".checkboxbed").hide();
        $("#QuantityPacket").val("");
        $("#ValuePacket").val("");
        $("#Keywordbed").val("");
        $("#Keywordquantity").val("");
    });
    $("[data-event='Packet']").click(function () {
        var id = $(this).attr("data-id");
        if ($(".activebed").attr("data-id") != id) {
            $(".Default").removeClass("active");
            $(".item-massage").hide();
            $(".packet").removeClass("active").removeClass("activebed");
            $(".checkboxbed").attr("checked", false);
            $(".checkboxbed").hide();
            $(this).addClass("active").addClass("activebed");
            var val = $(this).attr("data-val");
            var pk = parseInt($("#PacketItem").val());
            if (pk == 0) {
                $("#ValuePacket").val(val);
                $("#valuepaketshow").html(val);
            }
            var toSearch = $("#Keywordbed").val();
            if (toSearch.length > 0) searchKeywordbed(toSearch);
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
    });
    $("#PacketItem").change(function () {
        var val = $(this).find(":selected").data("val");
        if (parseInt(val) == 0) {
            val = $(".activebed").attr("data-val");
        }
        $("#ValuePacket").val(val);
        $("#valuepaketshow").html(val);
    });
    $(".name-staff").click(function () {
        var i = $(this).data("id");
        var id = "checkbedid-" + i;
        var cid = "#dashboard-" + i;
        if ($("#" + id).is(":visible") && !$(cid).hasClass("not-active")) {
            var dns = intDateNow() - nm;
            var dates = dns + 300;
            if (dates < dns) ErrorMessage("Thời gian nhỏ hơn thời gian hiện tại!"); else {
                var count = parseInt($("#QuantityPacket").val());
                var a = $('.checkboxbed:checkbox:checked').length;
                if (count > a) document.getElementById(id).checked = !document.getElementById(id).checked;
                else if (document.getElementById(id).checked) document.getElementById(id).checked = false; else ErrorMessage(" Vượt quá số lượng ban đầu");
            }
        }
    });
    $("#QuantityPacket").keyup(function () {
        var count = parseInt($("#QuantityPacket").val());
        var a = $('.checkboxbed:checkbox:checked').length;
        var sl = $(".checkboxbed:checkbox[name=checkbedid]:visible").length;
        if (count > sl) {
            $("#QuantityPacket").val(sl);
            ErrorMessage(" Vượt quá số lượng giường hiện tại!");
        }

        if (count < a) {
            $("#QuantityPacket").val(a);
            ErrorMessage(" Vượt quá số lượng đã tích!");
        }
    });
    $("[data-event='AddMinute']").click(function () {
        FdiOpenDialog("#dialog-form-3", "/Massage/AjaxFormOrder?ItemId=" + $(this).data("ido"), "Trạng thái");
    });
    $("[data-event='OrderAll']").click(function () {
        var count = parseInt($("#QuantityPacket").val());
        if (count > 0)
            listbed("/Massage/AjaxOrders?ItemId=");
        else ErrorMessage("Bạn chưa chọn số lượng!");
    });
    function search() {
        var listnew = [];
        var a = $(".LevelRoom").hasClass("activeroom");
        var b = $(".packet").hasClass("activebed");
        var dns = intDateNow() - nm;
        var dates = dns + 300;
        if (dates <= dns) {
            ErrorMessage("Thời gian nhỏ hơn thời gian hiện tại!");
            if ($("#groupbed").html().length > 0) $("#groupbed .item-massage .checkboxbed").hide();
            else $("#objbed .item-massage .checkboxbed").hide();
        }
        var mp = parseInt($("#ValuePacket").val());
        var idp = $(".activebed").attr("data-id");
        var idr = $(".activeroom").attr("data-id");
        var t;
        if (a && b) {
            for (var i = 0; i < list.length; i++) {
                var temp1 = list[i].LstPacketItems;
                for (t = 0; t < temp1.length; t++) {
                    if (parseInt(temp1[t].ID) == parseInt(idp) && parseInt(list[i].LevelRoomId) == parseInt(idr)) {
                        listnew.push(list[i]);
                        if (dates > dns) datenowcheck(list[i].ID, dates, mp);
                    }
                }
            }
        }
        else if (a) {
            for (i = 0; i < list.length; i++) {
                if (list[i].LevelRoomId == parseInt(idr)) listnew.push(list[i]);
            }
        } else if (b) {
            for (i = 0; i < list.length; i++) {
                var temp = list[i].LstPacketItems;
                for (t = 0; t < temp.length; t++) {
                    if (temp[t].ID == parseInt(idp)) {
                        listnew.push(list[i]);
                        if (dates > dns) datenowcheck(list[i].ID, dates, mp);
                    }
                }
            }
        } else listnew = list;
        return listnew;
    }
    $("#Keywordbed").keyup(function () {
        $("#Keywordquantity").val("");
        var toSearch = $(this).val().toLowerCase();
        if (toSearch.length > 0) {
            $(".item-massage").hide();
            searchKeywordbed(toSearch);
        } else $(".item-massage").show();
    });
    $("#Keyword").keyup(function () {
        var toSearch = $(this).val().toLowerCase();
        if (toSearch.length > 0) {
            $(".item-massage").hide();
            searchKeyword(toSearch);
        } else $(".item-massage").show();
    });
    function searchKeywordbed(toSearch) {

        var listn = search();
        for (var i = 0; i < listn.length; i++) {
            if (listn[i].Name.indexOf(toSearch) != -1) $("#dashboard-" + listn[i].ID).show();
        }
    }
    function searchKeyword(toSearch) {
        for (var i = 0; i < lsto.length; i++) {
            if (lsto[i].UserName.length > 0 && lsto[i].UserName.indexOf(toSearch) != -1) $("#dashboard-" + lsto[i].BedDeskID).show();
        }
    }
    function searchKeywordquantity(toSearch) {
        $("#Keywordbed").val("");
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
            var dates = dns + 300;
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
        if (c < 0 && !$("#dashboard-" + bid).hasClass("not-active")) {
            if ($("#groupbed").html().length > 0) $("#groupbed .item-massage #checkbedid-" + bid).show();
            else $("#objbed .item-massage #checkbedid-" + bid).show();
        }
    }
    function listbed(url) {
        var packId = $(".activebed").data("id");
        if (packId == undefined || packId == null || packId == "") {
            packId = 0;
        }
        var yourArray = [];
        var lst = [];
        var lcheck = $('.checkboxbed:checkbox:checked');
        var count = parseInt($("#QuantityPacket").val());
        var ma = 0;
        var j = 0;
        if (lcheck.length > 0) {
            lcheck.each(function () {
                yourArray.push($(this).val());
                for (var i = 0; i < list.length; i++) {
                    if (list[i].ID == parseInt($(this).val())) {
                        if (lcheck.length == 1) {
                            for (j = 0; j < list.length; j++) {
                                if (lsto[j].BedDeskID == parseInt($(this).val()) && ma < lsto[i].EndDate)
                                    ma = lsto[i].EndDate;
                            }
                            list[i].Quantity = ma;
                        }
                        lst.push(list[i]);
                        break;
                    }
                }
            });
        } else {
            j = 0;
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
            FdiOpenDialog("#dialog-form-3", url + yourArray.join(",") + "&packetId=" + packId, "Trạng thái");
        } else ErrorMessage("Bạn chưa chọn giường!");

    }
    $("#Keywordquantity").keyup(function () {
        searchKeywordquantity($("#Keywordquantity").val());
    });
    function objcup(id, i, gid) {
        $(id).show();
        if ($(id).hasClass("blue")) $(id).clone().appendTo("#content-blueg-" + gid);
        if ($(id).hasClass("red")) $(id).clone().appendTo("#content-redg-" + gid);
        if ($(id).hasClass("gold")) $(id).clone().appendTo("#content-goldg-" + gid);
        if ($(id).hasClass("not-active")) $(id).clone().appendTo("#content-not-activeg-" + gid);
        else $(".groupbedshow #checkbedid-" + i).addClass('g-' + gid);
    }
}