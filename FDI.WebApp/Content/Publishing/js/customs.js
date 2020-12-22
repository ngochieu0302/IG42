/// <reference path="numeral.min.js" />
/// <reference path="numeral.min.js" />
//---------- Detect Device
var isTouchDevice = 'ontouchstart' in window || 'onmsgesturechange' in window;
var isDesktop = $(window).width() != 0 && !isTouchDevice ? true : false;
var isiPad = navigator.userAgent.indexOf('iPad') != -1;
var isiPhone = navigator.userAgent.indexOf('iPhone') != -1;

// ----------- Check Device customs
$(document).ready(function () {
    if (!isTouchDevice) {

    }
    if (isDesktop) {

    }
    if (isiPhone) {

    }
    if (isiPad) {

    }
});
function searchMobie() {
    let overlayPage = $('#overlay');
    $(document).ready(function () {
        $('.button-call-search').on('click', function () {
            $(this).addClass("active");
            $(".boxMobieSearch").addClass("open");
            overlayPage.fadeIn();
            $('#iptSearchMobie').blur(function () {
                $('#iptSearchMobie').focus();
            });
        });
        $('.cogLangguage').on('click', function () {
            $(".cogLangguage").addClass("active").find(".head-lang").addClass("open");
            overlayPage.fadeIn();
        });
        overlayPage.on('click', function () {
            $(".button-call-search").removeClass("active");
            $(".boxMobieSearch").removeClass("open");
            $(".cogLangguage").removeClass("active").find(".head-lang").removeClass("open");
            overlayPage.fadeOut();
        });

    });
};

//Load inline mobie - tablet
const Xwidth = $(window).width();
if (Xwidth < 800) {
    if ($(".js-mmenu").length = 1) {
        function mMenu() {
            let $menu = $("#mainMenu").clone();
            $menu.attr("id", "my-mobile-menu");
            $menu.mmenu({});
        };
        mMenu();
    };
    searchMobie();
}


function ResizeWindows() {
    let Yheight = $(window).height();
    let Xwidth = $(window).width();

    if (Xwidth < 800) {
        $(document).ready(function () {

        });
    };

    if (Xwidth > 800) {
        $(function () {
            var shrinkHeader = 300;
            $(window).scroll(function () {
                var scroll = getCurrentScroll();
                if (scroll >= shrinkHeader) {
                    $('#header-main').addClass('fixed');
                    $('.page-stick-header').addClass('active');
                }
                else {
                    $('#header-main').removeClass('fixed');
                    $('.page-stick-header').removeClass('active');
                }
            });
            function getCurrentScroll() {
                return window.pageYOffset || document.documentElement.scrollTop;
            }
        });
    };
};

$(function cusScrollTop() {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 600) {
            $('#scrollTop').fadeIn(200);
        } else {
            $('#scrollTop').fadeOut(200);
        }
    });
    $('#scrollTop').click(function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: 0 }, 300);

    });
});

window.onorientationchange = ResizeWindows;
$(window).resize(function () {
    ResizeWindows();
});
ResizeWindows();

function Done() {
    ResizeWindows()
};

$(document).ready(function () {
    Done();
});
$(function () {
    $("#frmContact").validate({
        rules: {
            Name: {
                required: true
            },
            Message: {
                required: true
            },
            Email: {
                required: true,
                email: true
            },
            Phone: {
                required: true,
                number: true,
                minlength: 10,
                maxlength: 11
            },
            Capcha: {
                required: true
            },
        },
        messages: {
            Name: {
                required: "Mời nhập tên của bạn !"
            },
            Message: {
                required: "Hãy để lại lời nhắn !"
            },
            Email: {
                required: "Mời nhập Email của bạn !",
                email: "Không đúng định dạng !"
            },
            Phone: {
                required: "Mời nhập số điện thoại của bạn !",
                number: "Không đúng định dạng !",
                minlength: "Không đúng định dạng !",
                maxlength: "Không đúng định dạng !"
            },
            Capcha: {
                required: "Bạn chưa nhập mã xác nhận !"
            },
        },
        submitHandler: function () { //onSubmit
            $("#btn-send").disabled = true;
            $.post("/Contact/Contact/SendContact2", $("#frmContact").serialize(), function (msg) {
                if (!msg.Erros) {
                    swal({
                        title: "Thành công !",
                        text: msg.Message,
                        type: "success",
                        confirmButtonText: "Ok"
                    }, function () {
                        window.location.reload();
                        $(".fancybox-close-small").click();
                    });

                } else {
                    swal({ title: "Có lỗi xảy ra !", text: msg.Message, type: "error", confirmButtonText: "Ok" });
                }
            });
        }
    });
});
function ChangeRandom() {
    $("#imageRandom").attr("src", "contact/contact/showCaptchaImage?width=120&height=28&t=" + new Date().getMilliseconds());
}

$(document).ready(function() {
    $(".header__lang a").click(function() {
        $.post("/Menus/Menus/Changelanguage", { lang: $(this).data("lang") }, function (data) {
            if (!data.Erros) {
                window.location.href = window.location.href;
                location.reload(true);
            }
        });
    });
});
$(function () {
    $("#frmSendEmail").validate({
        rules: {
            Email: {
                required: true,
                email: true
            }
        },
        messages: {
            Email: {
                required: "Mời nhập Email của bạn !",
                email: "Không đúng định dạng !"
            }
        },
        submitHandler: function () { //onSubmit
            $.post("/Contact/Contact/SendEmail", $("#frmSendEmail").serialize(), function (msg) {
                if (!msg.Erros) {
                    swal({
                        title: "Thành công !",
                        text: msg.Message,
                        type: "success",
                        confirmButtonText: "Ok"
                    }, function () {
                        window.location.reload();
                        $(".fancybox-close-small").click();
                    });

                } else {
                    swal({ title: "Có lỗi xảy ra !", text: msg.Message, type: "error", confirmButtonText: "Ok" });
                }
            });
        }
    });
});

$(document).ready(function() {
    $("#ChangeSize_Product").change(function() {
        var id = $(this).val();
        var productId = $("#ChangeSize_Product option:selected").data("id");
        $.post("/Product/Product/ChangeProductSize", { productId: productId, sizeId: id }, function(data) {
            $(".price-regular").html(numeral(data.PriceNew).format("0,0"));
            $("#ShopproducId").val(data.ID);
            if (data.PriceNew < data.PriceOld) {
                $(".price-old").html("<del>" + numeral(data.PriceOld).format("0,0") + "</del>");
            } else {
                $(".price-old").html("");
            }
        });
    });
    $("#Color_Product li").click(function() {
        $("#Color_Product li").each(function(e) {
            e.removeClass("active");
        });
        var colorId = $(this).data("id");
        var productId = $(this).data("pId");
        var sizeId = $("#ChangeSize_Product option:selected").val();
        $.post("/Product/Product/ChangeProductSize", { productId: productId, sizeId: sizeId ,colorId:colorId}, function(data) {
            $(".price-regular").html(numeral(data.PriceNew).format("0,0"));
            $("#ShopproducId").val(data.ID);
            if (data.PriceNew < data.PriceOld) {
                $(".price-old").html("<del>" + numeral(data.PriceOld).format("0,0") + "</del>");
            } else {
                $(".price-old").html("");
            }
        });
        $(this).addClass("active");
    });
    $("#Addtocart").click(function() {
        debugger;
        var productId = $(this).data("id");
        var sizeId = $("#ChangeSize_Product option:selected").val();
        var colorId = $("#Color_Product li.active").data("id");
        var pId = $("#ShopproducId").val();
        if (!sizeId) {
            toastr["error"]("Hãy chọn size.!");
            return false;
        }
        if (!colorId) {
            toastr["error"]("Hãy chọn màu.!");
            return false;
        }
        var quantity = $("#Quantity").val();
        $.post("/Product/Product/Addtocart", { productId: productId,quantity: quantity,shopId: pId, sizeId: sizeId ,colorId:colorId}, function(msg) {
            if (msg.Erros == false) {
                toastr["success"](msg.Message);
            }
            if (msg.Erros == true) {
                toastr["error"](msg.Message);
            }
        });
    });
    $(".Updateq").on("change", function() {
        
        var pId = parseInt($(this).attr("data-id"));
        var quantity = parseInt($(this).val());
        if (quantity <=0 ) {
            toastr["error"]("Số lượng phải > 0.!");
            return false;
        }
        $.post("/Product/Product/UpdateCart", { productId: pId,type:1,quantity: quantity}, function(msg) {
            
        });
    });
    $('.qtytn-cart').on('click', function () {
        debugger;
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        if ($button.hasClass('inc')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        $button.parent().find('input').val(newVal);
        var pId = parseInt($button.parent().find('input').attr("data-id"));
        if (newVal <=0 ) {
            toastr["error"]("Số lượng phải > 0.!");
            return false;
        } else {
            $.post("/Product/Product/UpdateCart", { productId: pId,type:1,quantity: newVal}, function(msg) {
            
            });
        }
    });
    $(".btnDelete").click(function() {
        var pId = $(this).data("id");
        swal({
            title: "Xóa sản phẩm đã chọn",
            text: "Xóa sản phẩm",
            type: "warning",
            showCancelButton: true,
            confirmButtonClass: "btn-warning",
            confirmButtonText: "Xóa",
            cancelButtonText: "Hủy",
            closeOnConfirm: false,
            closeOnCancel: false
        },function (isConfirm) {
            if (isConfirm) {
                $.post("/Product/Product/UpdateCart", { productId: pId,type:2}, function (msg) {
                    swal.close();
                    window.location.reload();
                });
            } else {
                swal.close();
            }
        });
        //$.post("/Product/Product/UpdateCart", { productId: pId,type:2}, function(msg) {
            
        //});
    });
    $("#frm-Order").validate({
        rules: {
            email: {
                required: true,
                email: true
            },
            fullname: {
                required: true,
            },
            phone: {
                required: true,
                number: true
            }
            ,
            address: {
                required: true,
            }
        },
        messages: {
            email: {
                required: "Mời nhập Email của bạn !",
                email: "Không đúng định dạng !"
            },
            fullname: {
                required: "Mời nhập tên của bạn !",
            },
            phone: {
                required: "Mời nhập số điện thoại của bạn !",
                number: "Số điện thoại kiểu số"
            },
            address: {
                required: "Mời nhập địa chỉ của bạn !",
            }
        },
        submitHandler: function () { //onSubmit
            $.post("/Product/Product/SendOrder", $("#frm-Order").serialize(), function (msg) {
                if (!msg.Erros) {
                    swal({
                        title: "Thành công !",
                        text: msg.Message,
                        type: "success",
                        confirmButtonText: "Ok"
                    }, function () {
                        window.location.href="/";
                        $(".fancybox-close-small").click();
                    });

                } else {
                    swal({ title: "Có lỗi xảy ra !", text: msg.Message, type: "error", confirmButtonText: "Ok" });
                }
            });
        }
    });
    $("#lst-color input").click(function() {
        var names = '';
        debugger;
        $(this).parent().parent().parent().find("input:checked").each(function() {
            names = names + $(this).val() +",";
        });
        var url = changeURLParameter("color", names);
        if (url) {
            window.location.href = url;
        }
        
        
    });
    $("#lst-productsize input").click(function() {
        var names = '';
        $(this).parent().parent().parent().find("input:checked").each(function() {
            names = names + $(this).val() +",";
        });
        
        var url = changeURLParameter("size", names);
        if (url) {
            window.location.href = url;
        }
        
    });
    $("#sortby").change(function() {
        var val = $(this).val();
        var url = changeURLParameter("sort", val);
        if (url) {
            window.location.href = url;
        }
        
    });
    function changeURLParameter(sVariable, sNewValue) {
        debugger;
        var aURLParams = [];
        var aParts;
        var aParams = (window.location.search).substring(1, (window.location.search).length).split('&');

        for (var i = 0; i < aParams.length; i++)
        {
            aParts = aParams[i].split('=');
            aURLParams[aParts[0]] = aParts[1];
        }
        var sNewURL = '';
        if (aURLParams[sVariable] != sNewValue)
        {
            if (sNewValue.toUpperCase() == "ALL")
                aURLParams[sVariable] = null;
            else
                aURLParams[sVariable] = sNewValue;

            sNewURL = window.location.origin + window.location.pathname;
            var bFirst = true;

            for (var sKey in aURLParams)
            {
                if (aURLParams[sKey])
                {
                    if (bFirst)
                    {
                        sNewURL += "?" + sKey + "=" + aURLParams[sKey];
                        bFirst = false;
                    }
                    else
                        sNewURL += "&" + sKey + "=" + aURLParams[sKey];
                }
            }
        }
        return sNewURL;
    }

    $(".btn-cart").click(function() {
        debugger;
        var pId = $(this).data("id");
        var shopId = $(".ShopId-" + pId).val();
        var sizeId = $(".sizeId-" + pId).val();
        $.post("/Product/Product/Addtocart", { productId: pId,quantity: 1,shopId: shopId, sizeId: sizeId }, function(msg) {
            if (msg.Erros == false) {
                toastr["success"](msg.Message);
            }
            if (msg.Erros == true) {
                toastr["error"](msg.Message);
            }
        });
    });
    $("body").on("click",".cui-toolbar-toolbar a img",function() {
        debugger;
        toastr["success"]("ok");
    });
    // Loaded via <script> tag, create shortcut to access PDF.js exports.
   
});