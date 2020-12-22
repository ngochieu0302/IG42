using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNProductSizeController : BaseApiController
    {
        //
        // GET: /DNActive/
        
        private readonly ProductSizeDA _da = new ProductSizeDA();
        
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelProductSizeItem()
                : new ModelProductSizeItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<ProductSizeItem>() : _da.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new Product_Size() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new ProductSizeItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<ProductSizeItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<ProductSizeItem>(json);
                var obj = new Product_Size();
                UpdateBase(obj, dayOff);
                _da.Add(obj);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<ProductSizeItem>(json);
                var obj = _da.GetById(id);
                UpdateBase(obj, dayOff);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, string listint)
        {
            if (key == Keyapi)
            {
                var list = _da.ListByArrId(listint);
                foreach (var item in list)
                {
                    _da.Delete(item);
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public Product_Size UpdateBase(Product_Size productSize, ProductSizeItem productSizeItem)
        {
            productSize.Name = productSizeItem.Name;
            productSize.Value = productSizeItem.Value;
            //productSize.AgencyID = productSizeItem.AgencyID;

            return productSize;
        }
    }
}
