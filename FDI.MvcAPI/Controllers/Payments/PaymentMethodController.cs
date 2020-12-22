using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class PaymentMethodController : BaseApiController
    {
        //
        // GET: /PaymentMethod/
        private readonly PaymentMethodDA _da = new PaymentMethodDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelPaymentMethodItem()
                : new ModelPaymentMethodItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<PaymentMethodItem>() : _da.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new PaymentMethodItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var msg = new JsonMessage { Erros = false, Message = "Thêm mới dữ liệu thành công.!" };
            if (key == Keyapi)
            {
               
                var model = new Payment_Method();
                UpdateModel(model);
                _da.Add(model);
                _da.Save();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Có lỗi xảy ra.!"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key, string json, int id)
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công.!" };
            if (key == Keyapi)
            {
                var obj = _da.GetById(id);
                UpdateModel(obj);
                _da.Save();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Có lỗi xảy ra.!"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult Delete(string key, string listint)
        {
            var msg = new JsonMessage { Erros = false, Message = "Xóa dữ liệu thành công.!" };
            if (key == Keyapi)
            {
                var list = _da.GetListByArrId(listint);
                foreach (var item in list)
                {
                    item.IsDeleted = true;
                }
                _da.Save();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Có lỗi xảy ra.!"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
