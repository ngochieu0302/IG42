using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class ProductDLController : BaseApiController
    {
        readonly ProductDL _dl = new ProductDL();
        public JsonResult GetList(int cateId, int page)
        {
            var total = 0;
            var obj = Request["key"] != Keyapi
                ? new ModelProductItem() : new ModelProductItem { ListItem = _dl.GetList(cateId, page, ref total), Total = total };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductId(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new ProductItem() : _dl.GetProductId(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult CategoryItem(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new CategoryItem() : _dl.CategoryItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCateId(int id)
        {
            var obj = Request["key"] != Keyapi
               ? new CategoryItem() : _dl.CategoryItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListOther(int cateId, int ortherId)
        {
            var obj = Request["key"] != Keyapi
               ? new List<ProductItem>() : _dl.GetListOther(cateId, ortherId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}