
function registerGallery() {
    $('[data-type="gallery"]').click(function () {
        var multi = $(this).attr('data-multi');
        var value = $(this).attr('data-value');
        var types = $(this).data('type-value');
        var container = $(this).attr('data-container');
        selectPicture("/Admin/GalleryPictureSelect/?MutilFile=" + multi + "&Container=" + container + "&ValuesSelected=" + value + "&ModuleType=" + types);
    });

    $('body').on('click', '.deleteImg', function () {
        var id = $(this).data("id").toString();
        var ctn = $(this).data("ctn");
        var el = "#Value_" + ctn;
        var value = $(el).val();
        var arrRowId = value.split(',');
        var index = arrRowId.indexOf(id);
        if (index != -1)
            arrRowId.splice(index, 1);
        $(el).val(arrRowId);
        $(this).parent().parent().parent().remove();
    });

}

function registerDate() {
    !function (a) {
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
    $(".date-picker").datepicker({
        showOn: "top",
        buttonImageOnly: true,
        format: "dd/mm/yyyy",
        language: 'vi'

    }).mask('00/00/0000').on('changeDate', function (e) {
        $(this).datepicker('hide');
    });
};

function registerGalleryByModule() {
    // Đăng ký sự kiện gọi gallery picture dialog
    $('[data-type="gallery"]').click(function () {
        var multi = $(this).data('multi');
        var value = $(this).data('value');
        var types = $(this).data('type-value');
        var container = $(this).attr('data-container');
        selectPicture("/Admin/GalleryPictureSelectByModule/?MutilFile=" + multi + "&Container=" + container + "&ValuesSelected=" + value + "&ModuleType=" + types);
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

function createAutoTag(tagControls, urlRouters) {
    $("#" + tagControls).keypress(function (e) {
        if (e.keyCode == 13) {
            addValues(tagControls, $(this).val(), urlRouters + "?do=Add", '');
            return false;
        }
        return false;
    });
    $('#' + tagControls).autocomplete({
        serviceUrl: urlRouters,
        minChars: 1,
        delimiter: /(,|;)\s*/, // regex or character
        maxHeight: 400,
        width: 500,
        zIndex: 9999,
        deferRequestBy: 0, //miliseconds
    });
}

function createAutoTag(tagControls, urlRouters, labelKey) {
    $("#" + tagControls).keypress(function (e) {
        if (e.keyCode == 13) {
            addValues(tagControls, $(this).val(), urlRouters + "?do=Add&KeyID=" + labelKey, labelKey);
            return false;
        }
    });

    $('#' + tagControls).autocomplete({
        serviceUrl: urlRouters,
        minChars: 1,
        delimiter: /(,|;)\s*/, // regex or character
        maxHeight: 400,
        width: 500,
        zIndex: 9999,
        params: { KeyID: labelKey },
        deferRequestBy: 0, //miliseconds
    });
}

function addValues(container, value, urlRouters, key) {
    var controlsInput = $("#" + container);
    var controls = $("#" + container + "_Value");
    $.post(encodeURI(urlRouters), { "values": "" + value + "" }, function (data) {
        if (data.Erros) {
            createMessage("Có lỗi xảy ra", "<b>Lỗi được thông báo:</b><br/>" + data.Message);
        }
        else {
            $(controls).append("<li id=\"" + container + "_" + data.ID + "\" name=\"" + data.ID + "\" key=\"" + key + "\"><span>" + data.Message + "</span><a href=\"javascript:deletevalues('" + container + "_" + data.ID + "');\"><img border=\"0\" src=\"/Content/Admin/Images/gridview/act_filedelete.png\"></a></li>");
            $(controlsInput).val("");
        }
    });
}

function createCKFider(instance, imageWidth) {
    $("#" + instance + "Button").click(function () {
        var finder = new CKFinder();
        finder.selectActionFunction = function (fileUrl) {
            var htmlRespoint = "<input type=\"hidden\" name=\"" + instance + "\" value=\"" + fileUrl + "\" />";
            htmlRespoint += "<img src=\"" + fileUrl + "\" style=\"border:1px solid #ccc; width:" + imageWidth + "px; margin-top:2px;\" />";
            $("#" + instance + "Values").html(htmlRespoint);
        };
        finder.popup();
    });
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
        if ($(this).val() !== '' && $(this).val() !== 'Từ khóa tìm kiếm')
            arrParam += "&" + idMselect + "=" + $(this).val();
    });
    $("[data-id='SearchIn'].multiSelectSearch").each(function () {
        idMselect = $(this).attr("data-id");
        if (getValueMutilSelect(idMselect) !== '')
            arrParam += "&" + idMselect + "=" + getValueMutilSelect(idMselect);
    });
    if (arrParam !== '')
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
    //alert(arrParam);
    //$(".multiSelectSearch").each(function () {
    //    idMselect = $(this).attr("data-id");
    //    if (getValueMutilSelect(idMselect) != '')
    //        arrParam += "&" + idMselect + "=" + getValueMutilSelect(idMselect);
    //});
    //alert(arrParam);
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

function getUrlParemeter(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}

function getValueOrder(form, input) {
    var values = '';
    $("#" + form).find("select").each(function () {
        values += "|" + $(this).attr("id") + '_' + $(this).val();
    });
    values = values.substring(1);
    $("#" + input).val(values);
}

function getEditorContent(instanceName) {
    if (typeof (FCKeditorAPI) !== 'undefined') {
        var oEditor = FCKeditorAPI.GetInstance(instanceName);
        return oEditor.GetHTML(true);
    }
}

function updateEditor() {
    for (var name in CKEDITOR.instances)
        CKEDITOR.instances[name].updateElement();
}

function getValueFromAutoTag(classUL, controlsFillvalue) {
    var contentValues = '';
    $("." + classUL + " li").each(function () {
        if ($(this).attr("key") != 'undefined' && $(this).attr("key") != '')
            contentValues += "," + $(this).attr("key") + "_" + $(this).attr("name");
        else
            contentValues += "," + $(this).attr("name");
    });
    if (contentValues != '')
        contentValues = contentValues.substring(1);
    $("#" + controlsFillvalue).val(contentValues);
}

function ConvertCurrency(str) {
    return str.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
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

function xoa_dau(str) {
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
    str = str.replace(/Đ/g, "D");
    return str;
}