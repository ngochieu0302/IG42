using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.Simple;
using FDI.DA;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class ProductValueController : BaseApiController
    {
        private readonly ShopProductValueDA _da = new ShopProductValueDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModuleShopProductValueItem()
                : new ModuleShopProductValueItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetList(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<ShopProductValueItem>() : _da.GetList(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductValueItem(string key, int id)
        {
            var obj = key != Keyapi ? new ShopProductValueItem() : _da.GetProductValueItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword,showLimit,agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckByName(string key, string name, int id, int agencyId)
        {            
            var b = key == Keyapi && _da.CheckByName(name, id, agencyId);
            return Json(b ? 1 : 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var model = new Shop_Product_Value();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.NameAscii = FomatString.Slug(model.Name);
                model.IsDeleted = false;
                model.Quantity = 0;
                model.QuantityOut = 0;
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key, string json)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.NameAscii = FomatString.Slug(model.Name);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListArrId(lstInt);
                foreach (var item in lst)
                {
                    item.IsDeleted = true;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
