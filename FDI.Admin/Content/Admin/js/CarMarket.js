function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function btnDisabled(name) {
    $(name).prop("disabled", true);
}
function btnEnable(name) {
    $(name).prop("disabled", false);
}

function registerDate() {
    !function(a) {
        a.fn.datepicker.dates.vi = {
            days: ["Chủ nhật", "Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy"],
            daysShort: ["CN", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7"],
            daysMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"],
            months: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"],
            monthsShort: ["Th1", "Th2", "Th3", "Th4", "Th5", "Th6", "Th7", "Th8", "Th9", "Th10", "Th11", "Th12"],
            today: "Hôm nay",
            clear: "Xóa",
            format: "dd/mm/yyyy"
        }
    }(jQuery);
    $('.time-picker').clockface().mask('00:00');
    $(".date-picker").datepicker({
        showOn: "top",
        buttonImageOnly: true,
        format: "dd/mm/yyyy",
        language: 'vi'

    }).mask('00/00/0000').on('changeDate', function(e) {
        $(this).datepicker('hide');
    });
};
function registerGallery() {
    $('[data-type="gallery"]').click(function () {
        var multi = $(this).attr('data-multi');
        var value = $(this).attr('data-value');
        var types = $(this).data('type-value');
        var container = $(this).attr('data-container');
        selectPicture("/GalleryPictureSelect/?MutilFile=" + multi + "&Container=" + container + "&ValuesSelected=" + value + "&ModuleType=" + types);
    });
    $('body').on('click', '.deleteImg', function () {
        var id = $(this).data("id").toString();
        var ctn = $(this).data("ctn");
        var el = "#Value_" + ctn;
        var value = $(el).val();
        var arrRowId = value.split(',');
        var index = arrRowId.indexOf(id);
        if (index != -1) {
            arrRowId.splice(index, 1);
        }
        $(el).val(arrRowId);
        $(this).parent().parent().parent().remove();
    });
}

function getValuePicture(container) {
    var obj = container;
    if (obj.indexOf(',') > 0) {
        container = obj.split(',')[0];
    }
    var arrRowId = '';
    $("#Text_" + container + " tr").each(function () {
        arrRowId += $(this).attr("id") + ",";
    });
    arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
    if (typeof selectMutil != "undefined" && selectMutil == false) {
        var strArr = arrRowId.split(',');
        arrRowId = strArr[strArr.length - 1];
    }
    $("#Value_" + container + "").val(arrRowId);
    return arrRowId;
}

function getValueVideo(container) {
    var arrRowId = '';
    $("#Text_" + container + " tr").each(function () {
        arrRowId += $(this).attr("id") + ",";
    });
    arrRowId = (arrRowId.length > 0) ? arrRowId.substring(0, arrRowId.length - 1) : arrRowId;
    if (typeof selectMutil != "undefined" && selectMutil == false) {
        var strArr = arrRowId.split(',');
        arrRowId = strArr[strArr.length - 1];
    }
    $("#Value_" + container).val(arrRowId);
    return arrRowId;
}
function selectPicture(urlSelectImage) {
    $('body').modalmanager('loading');
    $("#dialog-form-2 .modal-title").html("Chọn ảnh");
    $("#dialog-form-2 #dialog-form-ajax").load(encodeURI(urlSelectImage));
    $('#dialog-form-2').modal('show');
    return false;
}


var config_description = {
    height: 200
};
var config_content = {
    toolbar: 'Full',
    height: 250
};

function LoadCKEDITOR(instanceName, fullEditor) {
    if (fullEditor)
        CKEDITOR.replace(instanceName, config_content);
    else
        CKEDITOR.replace(instanceName, config_description);
}

function getValueMutilSelect() {
    var arrId = '';
    $.each($('select.mutil').multiSelect("getSelects"), function (key, value) {
        arrId += value + ",";
    });
    arrId = (arrId.length > 0) ? arrId.substring(0, arrId.length - 1) : arrId;
    return arrId;
}

function getValueFormMutilSelect(form) {
    var arrParam = '';
    var idMselect;
    $(form).find("input,textarea,hidden,select").not("input[type='checkbox'], input[type='radio']:checked, input[name='selectItem'], .ms-search input, .mutil").each(function () {
        idMselect = $(this).attr("name");
        if ($(this).val() != '' && $(this).val() != 'Từ khóa tìm kiếm')
            arrParam += "&" + idMselect + "=" + $(this).val();
    });
    $("[data-id='SearchIn'].multiSelectSearch").each(function () {
        idMselect = $(this).attr("data-id");
        if (getValueMutilSelect(idMselect) != '')
            arrParam += "&" + idMselect + "=" + getValueMutilSelect(idMselect);
    });
    if (arrParam != '')
        arrParam = arrParam.substring(1);
    return arrParam;
}

function getValueGalaryFormMutilSelect(form) {
    var arrParam = '';
    var idMselect;
    $(form).find("input,textarea,hidden,select").not("input[type='checkbox'], input[type='radio']:checked").each(function () {
        idMselect = $(this).attr("name");
        if ($(this).val() != '' && $(this).val() != 'Từ khóa tìm kiếm')
            arrParam += "&" + idMselect + "=" + $(this).val();
    });
  
    if (arrParam != '')
        arrParam = arrParam.substring(1);
    return arrParam;
}

function createMessage(title, message) {
    toastr["error"](message);
}

function Reorder(eSelect, iCurrentField, numSelects) {
    var iNewOrder = eSelect.selectedIndex + 1;
    var iPrevOrder;
    var positions = new Array(numSelects);
    var ix;
    for (ix = 0; ix < numSelects; ix++) {
        positions[ix] = 0;
    }
    for (ix = 0; ix < numSelects; ix++) {
        positions[eSelect.form["ViewOrder" + ix].selectedIndex] = 1;
    }
    for (ix = 0; ix < numSelects; ix++) {
        if (positions[ix] == 0) {
            iPrevOrder = ix + 1;
            break;
        }
    }
    if (iNewOrder != iPrevOrder) {
        var iInc = iNewOrder > iPrevOrder ? -1 : 1;
        var iMin = Math.min(iNewOrder, iPrevOrder);
        var iMax = Math.max(iNewOrder, iPrevOrder);
        for (var iField = 0; iField < numSelects; iField++) {
            if (iField != iCurrentField) {
                if (eSelect.form["ViewOrder" + iField].selectedIndex + 1 >= iMin &&
					eSelect.form["ViewOrder" + iField].selectedIndex + 1 <= iMax) {
                    eSelect.form["ViewOrder" + iField].selectedIndex += iInc;
                }
            }
        }
    }
}


function getValueOrder(form, input) {
    var values = '';
    $("#" + form).find("select").each(function () {
        values += "|" + $(this).attr("id") + '_' + $(this).val();
    });
    values = values.substring(1);
    $("#" + input).val(values);
}


function updateEditor() {
    for (var name in CKEDITOR.instances)
        CKEDITOR.instances[name].updateElement();
}




function RemoveUnicode(str) {

    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$–|”|“|`/g, "-");
    str = str.replace(/-+-/g, "-"); //thay thế 2- thành 1- 
    str = str.replace(/^\-+|\-+$/g, "");
    return str;
}

function DocSo3ChuSo(baso) {
    var chuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
    var ketQua = "";
    var tram = parseInt(baso / 100);
    var chuc = parseInt((baso % 100) / 10);
    var donvi = baso % 10;
    if (tram === 0 && chuc === 0 && donvi === 0) return "";
    if (tram !== 0) {
        ketQua += chuSo[tram] + " trăm ";
        if ((chuc === 0) && (donvi !== 0)) ketQua += " linh ";
    }
    if ((chuc !== 0) && (chuc !== 1)) {
        ketQua += chuSo[chuc] + " mươi";
        if ((chuc === 0) && (donvi !== 0)) ketQua = ketQua + " linh ";
    }
    if (chuc === 1) ketQua += " mười ";
    switch (donvi) {
        case 1:
            if ((chuc !== 0) && (chuc !== 1)) {
                ketQua += " mốt ";
            }
            else {
                ketQua += chuSo[donvi];
            }
            break;
        case 5:
            if (chuc === 0) {
                ketQua += chuSo[donvi];
            }
            else {
                ketQua += " lăm ";
            }
            break;
        default:
            if (donvi !== 0) {
                ketQua += chuSo[donvi];
            }
            break;
    }
    return ketQua;
}
function DocTienBangChu(soTien) {
    var tien = new Array("", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ");
    var lan = 0;
    var i = 0;
    var so = 0;
    var ketQua = "";
    var tmp = "";
    var viTri = new Array();
    if (soTien < 0) return "Số tiền âm !";
    if (soTien === 0) return "Không đồng !";
    if (soTien > 0) {
        so = soTien;
    }
    else {
        so = -soTien;
    }
    if (soTien > 8999999999999999) {
        return "Số quá lớn!";
    }
    viTri[5] = Math.floor(so / 1000000000000000);
    if (isNaN(viTri[5]))
        viTri[5] = "0";
    so = so - parseFloat(viTri[5].toString()) * 1000000000000000;
    viTri[4] = Math.floor(so / 1000000000000);
    if (isNaN(viTri[4]))
        viTri[4] = "0";
    so = so - parseFloat(viTri[4].toString()) * 1000000000000;
    viTri[3] = Math.floor(so / 1000000000);
    if (isNaN(viTri[3]))
        viTri[3] = "0";
    so = so - parseFloat(viTri[3].toString()) * 1000000000;
    viTri[2] = parseInt(so / 1000000);
    if (isNaN(viTri[2]))
        viTri[2] = "0";
    viTri[1] = parseInt((so % 1000000) / 1000);
    if (isNaN(viTri[1]))
        viTri[1] = "0";
    viTri[0] = parseInt(so % 1000);
    if (isNaN(viTri[0]))
        viTri[0] = "0";
    if (viTri[5] > 0) {
        lan = 5;
    }
    else if (viTri[4] > 0) {
        lan = 4;
    }
    else if (viTri[3] > 0) {
        lan = 3;
    }
    else if (viTri[2] > 0) {
        lan = 2;
    }
    else if (viTri[1] > 0) {
        lan = 1;
    }
    else {
        lan = 0;
    }
    for (i = lan; i >= 0; i--) {
        tmp = DocSo3ChuSo(viTri[i]);
        ketQua += tmp;
        if (viTri[i] > 0) ketQua += tien[i];
        if ((i > 0) && (tmp.length > 0)) ketQua += '';//&& (!string.IsNullOrEmpty(tmp))
    }
    if (ketQua.substring(ketQua.length - 1) === ',') {
        ketQua = ketQua.substring(0, ketQua.length - 1);
    }
    ketQua = ketQua.substring(1, 2).toUpperCase() + ketQua.substring(2);
    return ketQua + " đồng.";//.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
}