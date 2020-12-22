using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA;
using FDI.DA.DA.AppCustomer;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers.AppCustomer
{
    public class ProductCusAppController : BaseApiAuthController
    {
        //
        // GET: /ProductCusApp/
        private readonly ProductCusAppDA _da = new ProductCusAppDA("#");
        private readonly CategoryDA _categoryDa = new CategoryDA();
        public ActionResult GetListProductApp(string key)
        {
            var obj = key != Keyapi ? new List<ProductAppItem>() : _da.GetListProduct();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductItem(string key,int id)
        {
            var model = new ProductAppItem();
           
                model = _da.GetProductItem(id);
            var lstImg = new List<ImgAppItem>();
            foreach (var item in model.lstId)
            {
                var a = _da.GetListIMGApp(item.ID);
                lstImg.AddRange(a);
            }
            lstImg = lstImg.GroupBy(c => c.ID).Select(a=> new ImgAppItem
            {
                ID = a.FirstOrDefault().ID,
                Url = a.FirstOrDefault().Url,
            }).ToList();
            model.lstImg = lstImg;
            
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetListOfDay(int categoryId)
        {
            if (categoryId == 0)
            {
                var productsTmp = _da.GetListProduct24H();
                foreach (var productItem in productsTmp)
                {
                    productItem.Description = productItem.Description.TruncateHtml(75, "...");
                }
                return Json(productsTmp);
            }

            var products = _da.GetListProduct24H(categoryId);
            foreach (var productItem in products)
            {
                productItem.Description = productItem.Description.TruncateHtml(75,"...");
            }
            
            return Json(products);
        }

        [HttpPost]
        public ActionResult GetListByCategoryId(int categoryId)
        {
            var products = _da.GetListByCategoryId(categoryId);
            return Json(products);
        }


    }
}
