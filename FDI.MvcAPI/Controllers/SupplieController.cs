using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class SupplieController : BaseApiController
    {
        //
        // GET: /Supplie/

        private readonly SupplierDA _da = new SupplierDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNSupplierItem()
                : new ModelDNSupplierItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsStatic(int areaId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNSupplierItem()
                : new ModelDNSupplierItem { ListItem = _da.GetListSimpleStaticByRequest(Request, areaId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsGeneralDebt()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNSupplierItem()
                : new ModelDNSupplierItem { ListItem = _da.GetListSimpleGeneralDebt(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetList(string key)
        {
            var obj = _da.GetList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNSupplierItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListExport(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNSupplierItem()
                : new ModelDNSupplierItem
                {
                    ListItem = _da.GetListSimpleByRequestExcel(Request, agencyId),
                };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key)
        {
            var model = new DN_Supplier();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsDeleted = false;
                model.DateCreate = DateTime.Now.TotalSeconds();
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Address = HttpUtility.UrlDecode(model.Address);
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.Note = HttpUtility.UrlDecode(model.Note);
                _da.Add(model);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.DateCreate = DateTime.Now.TotalSeconds();
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Address = HttpUtility.UrlDecode(model.Address);
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.Note = HttpUtility.UrlDecode(model.Note);
                //if (model.IsLook.HasValue && model.IsLook.Value) _da.DeleteAll(ItemId);
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
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lst = _da.GetListByArrId(lstArrId);
            foreach (var item in lst)
                item.IsDeleted = true;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetByName(string name)
        {
            return Json(_da.GetByName(name), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddProduct(SupplieProductItem request)
        {
            //kiem tra da ton tai trong he thong chua
            var product = _da.CheckExistSupplierProduct(request.CateId, request.SupplierId);
            if (product)
            {
                return Json(new JsonMessage() { Erros = true, Message = "Sản phẩm đã tồn tại trong hệ thống" }, JsonRequestBehavior.AllowGet);
            }

            var obj = new DN_SupplierProduct()
            {
                ProductId = request.CateId,
                Amount = request.Quantity,
                SupplierId = request.SupplierId,
                IsDelete = false,
            };

            _da.AddProduct(obj);
            var result = _da.Save();

            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListSupplierProductById(int id)
        {

            var obj = new SupplieProductResponse { ListItem = _da.GetListSupplierProductById(Request, id), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSupplierProductById(int id)
        {
            var obj = _da.GetSupplierProductById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}
