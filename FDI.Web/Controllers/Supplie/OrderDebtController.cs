using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Supplie
{
    public class OrderDebtController : BaseController
    {
        //
        // GET: /OrderDebt/
        readonly OrderDebtAPI _api = new OrderDebtAPI();
        readonly SupplieAPI _supplie = new SupplieAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new ModelOrderDebtItem();
            var temp = new OrderDebtItem();
            if (DoAction == ActionType.Edit)
                temp = _api.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            model.ID = temp.ID;
            model.SupplieId = temp.SupplieId;
            model.UserName = !string.IsNullOrEmpty(temp.UserName) ? temp.UserName : UserItem.UserName;
            model.Note = temp.Note;
            model.UserID = UserItem.UserId;
            model.AgencyId = UserItem.AgencyID;
            model.Pricetotal = temp.Pricetotal;
            model.ListNcc = _supplie.GetList(UserItem.AgencyID);
            ViewBag.Action = DoAction;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            var lstID = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    if (_api.Update(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Show:
                    if (_api.Show(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    _api.Show(lstID);
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Hide:
                    if (_api.Hide(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    _api.Hide(lstID);
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Delete:
                    _api.Delete(lstID);
                    msg.Message = "Cập nhật thành công.";
                    break;
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
