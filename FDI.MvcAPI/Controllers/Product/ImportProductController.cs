using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class ImportProductController : BaseApiController
    {
        //
        // GET: /DNActive/
        
        private readonly ImportProductDA _dlDa = new ImportProductDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelImportProductItem()
                : new ModelImportProductItem { ListItem = _dlDa.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _dlDa.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<ImportProductItem>() : _dlDa.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, Guid id)
        {
            var obj = key != Keyapi ? new DN_ImportProduct() : _dlDa.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, Guid id)
        {
            var obj = key != Keyapi ? new ImportProductItem() : _dlDa.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<ImportProductItem>() : _dlDa.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var importProduct = JsonConvert.DeserializeObject<ImportProductItem>(json);
                var obj = new DN_ImportProduct();
                UpdateBase(obj, importProduct);
                _dlDa.Add(obj);
                _dlDa.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, Guid id)
        {
            if (key == Keyapi)
            {
                var importProduct = JsonConvert.DeserializeObject<ImportProductItem>(json);
                var obj = _dlDa.GetById(id);
                UpdateBase(obj, importProduct);
                _dlDa.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public DN_ImportProduct UpdateBase(DN_ImportProduct importProduct, ImportProductItem importProductItem)
        {
            importProduct.Quantity = importProductItem.Quantity;
            //importProduct.ProductID = importProductItem.ProductID;
            importProduct.Price = importProductItem.Price;
            importProduct.IsDelete = importProductItem.IsDelete;
            return importProduct;
        }
    }
}
