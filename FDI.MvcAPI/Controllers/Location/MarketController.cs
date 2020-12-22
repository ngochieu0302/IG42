using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class MarketController : BaseApiController
    {
        //
        // GET: /Market/
        readonly MarketDA _da = new MarketDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelMarketItem()
                : new ModelMarketItem { ListItems = _da.GetListbyRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsStatic(int areaId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelMarketItem()
                : new ModelMarketItem { ListItems = _da.GetListStaticbyRequest(Request, areaId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMarketItem(string key, int id)
        {
            var obj = key != Keyapi ? new MarketItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<MarketItem>() : _da.GetListSimple();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string code)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new Market();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsShow = true;
                model.IsDeleted = false;
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu Chưa được thêm mới.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string code)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lst = _da.GetByListArrId(lstArrId);
                    foreach (var item in lst)
                    {
                        item.IsDeleted = true;
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
