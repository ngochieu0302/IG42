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
    $('body').modalmanager('loading');
    $.post(urlRequest, function (data) {
        $("#dialog-form .modal-title").html("Thêm mới");
        $("#dialog-form #dialog-form-ajax").html(data);
        $("#dialog-form").modal("show");
    });
    
});

function FdiDialog(url, title) {
    $('body').modalmanager('loading');
    $.post(url, function (data) {
        $("#dialog-form .modal-title").html(title);
        $("#dialog-form #dialog-form-ajax").html(data);
        $('#dialog-form').modal('show');
    });
}

function FdiOpenDialog(attr, url, title) {
    $('body').modalmanager('loading');
    $.post(url, function (data) {
        $(attr + " .modal-title").html(title);
        $(attr + " #dialog-form-ajax").html(data);
        $(attr).modal('show');
    });
}

function FdiDialogView(url, title) {
    $('body').modalmanager('loading');
    $.post(url, function (data) {
        $("#dialog-form .modal-title").html(title);
        $("#dialog-form #dialog-form-ajax").html(data);
        $('#dialog-form').modal('show');
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

function secondsToHms(day) {
    var hh = Math.floor(day / 3600);
    var mm = Math.floor(day % 3600 / 60);
    return ((hh < 10 ? "0" : "") + hh + ":" + (mm < 10 ? "0" : "") + mm);
}

function showddMMyyyyhhmmss(totals, id) {
    var date = toDateTime(totals);
    var y = date.getFullYear();
    var mm = date.getMonth() + 1;
    var d = date.getDate();
    var hh = date.getHours();
    var m = date.getMinutes();
    var s = date.getSeconds();
    $(id).html((d < 10 ? "0" : "") + d + "/" + (mm < 10 ? " 0" : "") + mm + "/" + y + " " + (hh < 10 ? "0" : "") + hh + ":" + (m < 10 ? " 0" : "") + m + ":" + (s < 10 ? "0" : "") + s + " ");
}

function ddMMyyyyhhmmss(totals) {
    var date = toDateTime(totals);
    var y = date.getFullYear();
    var mm = date.getMonth() + 1;
    var d = date.getDate();
    var hh = date.getHours();
    var m = date.getMinutes();
    return (d < 10 ? "0" : "") + d + "/" + (mm < 10 ? " 0" : "") + mm + "/" + y + " - " + (hh < 10 ? "0" : "") + hh + ":" + (m < 10 ? " 0" : "") + m;
}

function hhmmss(totals) {
    var date = toDateTime(totals);
    var hh = date.getHours();
    var m = date.getMinutes();
    var s = date.getSeconds();
    return (hh < 10 ? "0" : "") + hh + ":" + (m < 10 ? " 0" : "") + m;
}

function showhhmmss(date, id) {
    var hh = parseInt(date / 3600);
    var m = Math.floor(date % 3600 / 60);
    var s = Math.floor(date % 60);
    $(id).html((hh < 10 ? "0" : "") + hh + ":" + (m < 10 ? " 0" : "") + m + ":" + (s < 10 ? "0" : "") + s + " ");
}

function secondsToMs(ts, id, mm, st, td, dns) {
    if (st <= dns) {
        var m = parseInt((ts - dns) / 60);
        var s = parseInt((ts - dns) % 60);
        $("#counterOrder-" + id).html((m < 10 ? " 0" : "") + m + ":" + (s < 10 ? "0" : "") + s + " ");
    }
    $("#counterOrder-" + id + "-").html(secondsToHms(st - td) + "(" + mm + "')");
}

function secondsToMsidentity(id, st, td, dns) {
    if (st <= dns) {
        var m = parseInt((dns - st) / 60);
        //var s = parseInt((dns - st) % 60);
        $("#counterOrder-" + id).html((m < 10 ? " 0" : "") + m + "'");
    }
    //$("#counterOrder-" + id + "-").html(secondsToHms(st - td));
}

function countElement(item, array) {
    var count = 0;
    $.each(array, function (i, v) { if (v.BedDeskID === item) count++; });
    return count;
}

function deleteclass(id) {
    $(id).removeClass("red");
    $(id).removeClass("blue");
    $(id).removeClass("yellow");
    $(id).removeClass("default");
    $(id).removeClass("gold");
}

function deleteclassCalender(id) {
    $(id).removeClass("status-normal");
    $(id).removeClass("status-fromexchange");
    $(id).removeClass("status-toexchange");
    $(id).removeClass("status-off");
}
function deleteclassnh(id) {
    $(id).removeClass("serving-desk");
    $(id).removeClass("booking-desk");
    $(id).removeClass("free-desk");
    $(id).removeClass("processing-desk");
}

function bincountrestaurant(c, idc, list, td, dns, timedo, bedid) {
    list = listbybed(list, bedid);
    var count = list.length;
    if (list.length > 0) {
        list.sort(function (a, b) {
            return a.StartDate - b.StartDate;
        });
        var date = list[0].StartDate - td;
        if (count > 1 && list[0].StartDate - timedo < dns) {
            date = list[1].StartDate - td;
            count--;
        }
        $(c).html(" " + secondsToHms(date));
    }
}

function listbybed(list, bedid) {
    var listc = [];
    for (var i = 0; i < list.length; i++) {
        if (list[i].BedDeskID == bedid) listc.push(list[i]);
    }
    return listc;
}

function bincount(c, idc, list, td, dns, timedo, bedid) {
    var listc = listbybed(list, bedid);
    var count = listc.length;
    if (listc.length > 0) {
        listc.sort(function (a, b) {
            return a.StartDate - b.StartDate;
        });
        var m = listc[0].Minute;
        var date = listc[0].StartDate - td;
        if (count > 1 && listc[0].StartDate - timedo < dns) {
            date = listc[1].StartDate - td;
            m = listc[1].Minute;
            count--;
        }
        $(idc).html(count);
        $(c).html(" " + secondsToHms(date) + "(" + m + "')");
    } else {
        $(c).html("");
        $(idc).html(count);
    }
}
function deleteselectedbed(bedid) {
    var lstP = JSON.parse(localStorage.getItem("lstGroupTable"));
    for (var m = 0; m < lstP.length; m++) {
        var listselect = lstP[m].list;
        var a = -1;
        for (var i = 0; i < listselect.length; i++) {
            if (listselect[i] == bedid) {
                a = m;
                break;
            }
        }
        if (a >= 0) {
            listselect = lstP[a].list;
            for (i = 0; i < listselect.length; i++) {
                var id = "#dashboard-" + listselect[i];
                $(id).removeClass("selected-desk");
            }
            lstP.splice(a, 1);
            break;
        }

    }
    localStorage.setItem("lstGroupTable", JSON.stringify(lstP));
}
function printDiv(divId) {
    var printContents = document.getElementById(divId).innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = "<html><head><title></title></head><body>" + printContents + "</body>";
    window.print();
    document.body.innerHTML = originalContents;
}