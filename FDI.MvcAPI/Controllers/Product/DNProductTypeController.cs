using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNProductTypeController : BaseApiController
    {
        //
        // GET: /DNActive/
        
        private readonly ProductTypeDA _da = new ProductTypeDA("#");
        
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelProductTypeItem()
                : new ModelProductTypeItem { ListItem = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<ProductTypeItem>() : _da.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new Shop_Product_Type()  : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new ProductTypeItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<ProductTypeItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<ProductTypeItem>(json);
                var obj = new Shop_Product_Type();
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
                var dayOff = JsonConvert.DeserializeObject<ProductTypeItem>(json);
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

        public Shop_Product_Type UpdateBase(Shop_Product_Type productType, ProductTypeItem productTypeItem)
        {
            productType.Name = productTypeItem.Name;
            //productType.AgencyID = productTypeItem.AgencyID;

            return productType;
        }
    }
}
