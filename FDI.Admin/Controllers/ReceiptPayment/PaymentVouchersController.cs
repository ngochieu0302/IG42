using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class PaymentVouchersController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ReceiptPaymentDA _da = new ReceiptPaymentDA();
        private readonly CostTypeDA _costTypeda = new CostTypeDA();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new ModelReceiptPaymentItem
            {
                ListItem = _da.GetListByRequestAdmin(Request, (int)Vouchers.Payment),
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = _da.GetReceiptPaymentItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new ReceiptPaymentItem();
            if (DoAction == ActionType.Edit)
                model = _da.GetReceiptPaymentItem(ArrId.FirstOrDefault());
            ViewBag.CostType = _costTypeda.GetList(1, (int)Vouchers.Payment);
            ViewBag.Action = DoAction;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công !" };
            var model = new ReceiptPayment();
            var lst = new List<ReceiptPayment>();
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);
                        model.IsDelete = false;
                        model.IsActive = false;
                        var startDate = Request["DateReturn_"];
                        var date = ConvertUtil.ToDateTime(startDate);
                        model.DateReturn = ConvertDate.TotalSeconds(date);
                        model.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                        _da.Add(model);
                        _da.Save();
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        model = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(model);
                        var startDate = Request["DateReturn_"];
                        var date = ConvertUtil.ToDateTime(startDate);
                        model.DateReturn = ConvertDate.TotalSeconds(date);
                        _da.Save();
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    break;

                case ActionType.Active:
                    try
                    {

                        lst = _da.GetListArrId(ArrId);
                        foreach (var item in lst)
                        {
                            item.IsActive = true;
                        }
                        _da.Save();
                        msg.Message = "Đơn đặt hàng đã được duyệt !";
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Duyệt đơn hàng thất bại !";
                    }
                    break;
                case ActionType.Delete:

                    lst = _da.GetListArrId(ArrId);
                    foreach (var item in lst)
                    {
                        item.IsDelete = true;
                    }
                    _da.Save();
                    return Json(1, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
