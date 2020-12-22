using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class StorageProductController : BaseApiController
    {
        //
        // GET: /StorageProduct/
        readonly StorageProductDA _da = new StorageProductDA();
        public ActionResult GetListSimple(string key)
        {
            var obj = key != Keyapi ? new List<StorageProductItem>() : _da.GetListSimple();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelStorageProductItem()
                : new ModelStorageProductItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListProductByRequest(int agencyId)
        {
            decimal? total;
            decimal? totalold;
            int? quantity;
            var obj = Request["key"] != Keyapi
                ? new ModelImportProductItem()
                : new ModelImportProductItem { ListItem = _da.GetListProductByRequest(Request, agencyId, out total, out quantity), PageHtml = _da.GridHtmlPage, Total = total ?? 0, Quantity = quantity ?? 0 };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListProductValueByRequest(int agencyId)
        {
            decimal? total;
            decimal? totalold;
            int? quantity;
            var obj = Request["key"] != Keyapi
                ? new ModelProductItem()
                : new ModelProductItem { ListItem = _da.GetListProductValueByRequest(Request, agencyId, out total, out totalold, out quantity), PageHtml = _da.GridHtmlPage, Total = total ?? 0, TotalOld = totalold ?? 0, Quantity = quantity ?? 0 };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListCateValueByRequest(int agencyId)
        {
            decimal? total;
            decimal? totalold;
            decimal? quantity;
            var obj = Request["key"] != Keyapi
                ? new ModelCateValueItem()
                : new ModelCateValueItem { LisItems = _da.GetListCateValueByRequest(Request, agencyId, out total, out totalold, out quantity), PageHtml = _da.GridHtmlPage, Total = total ?? 0, TotalOld = totalold ?? 0, Quantity = quantity ?? 0 };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListProductLater(int agencyId)
        {
            decimal? total;
            decimal? totalold;
            int? quantity;
            var obj = Request["key"] != Keyapi
                ? new ModelImportProductItem()
                : new ModelImportProductItem { ListItem = _da.GetListProductLater(Request, agencyId, out total,  out quantity), PageHtml = _da.GridHtmlPage, Total = total ?? 0, Quantity = quantity ?? 0 };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListExcel(string key)
        {
            var obj = key != Keyapi ? new List<StorageProductItem>() : _da.GetListExcel(Request, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStorageProductItem(string key, int id)
        {
            var obj = key != Keyapi ? new StorageProductItem() : _da.GetStorageProductItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCateProductValueItem(string key, double quantity,int skip)
        {
            var obj = key != Keyapi ? new List<CateValueItem>() : _da.GetCateProductValueItem(quantity,skip);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStorageProductValueItem(string key, int id)
        {
            var obj = key != Keyapi ? new ProductValueItem() : _da.GetStorageProductValueItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCateValueItem(string key, int id)
        {
            var obj = key != Keyapi ? new CateValueItem() : _da.GetCateValueItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId, int type)
        //{
        //    var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId, type);
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Add(string key, string codeLogin)
        {
            var model = new StorageProduct();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                var dateCreated = Request["DateCreated_"];
                var date = dateCreated.StringToDate();
                //model.DN_ImportProduct = GetListImportItem(codeLogin, date,Agencyid());
                model.DateImport = DateTime.Now.TotalSeconds();
                model.DateCreated = DateTime.Now.TotalSeconds();
                model.Code = DateTime.Now.ToString("yyMMddHHmm");
                model.AgencyId = Agencyid();
                model.IsDelete = false;
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public List<DN_ImportProduct> GetListImportItem(string code, DateTime date,int agencyId)
        {
            const string url = "Utility/GetListImportPs?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ImportProductNewItem>>(urlJson);
            return list.Select(item => new DN_ImportProduct
            {
                Quantity = item.Quantity,
                Value = item.Value,
                QuantityOut = 0,
                Price = item.Price,
                //ProductID = item.ProductID,
                IsDelete = false,
                Date = item.DateS.StringToDecimal(),
                BarCode = item.BarCode,
                DateEnd = item.DateE.StringToDecimal(),
                PriceNew = item.Value * item.Price * item.Quantity,
                AgencyId = agencyId
            }).ToList();
        }
        public ActionResult Update(string key, string codeLogin)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                if (model == null)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                UpdateModel(model);
                var dateCreated = Request["DateCreated_"];
                var date = dateCreated.StringToDate();
                //var lst = model.DN_ImportProduct.Where(c => c.IsDelete == false).ToList();
                //var lstNew = GetListImportItem(codeLogin, date, Agencyid());

                ////xóa
                //var result1 = lst.Where(p => lstNew.All(p2 => p2.ProductID != p.ProductID)).ToList();
                //foreach (var i in result1)
                //{
                //    i.IsDelete = true;
                //}

                ////sửa
                //foreach (var i in lst)
                //{
                //    var j = lstNew.FirstOrDefault(c => c.ProductID == i.ProductID);
                //    if (j == null) continue;
                //    i.Quantity = j.Quantity;
                //    i.DateEnd = j.DateEnd;
                //    i.Date = j.Date;
                //    i.BarCode = j.BarCode;
                //    i.Value = j.Value;
                //}

                ////thêm mới
                //var result2 = lstNew.Where(p => lst.All(p2 => p2.ProductID != p.ProductID)).ToList();
                //model.DN_ImportProduct.AddRange(result2);
                model.DateImport = DateTime.Now.TotalSeconds();
                model.Note = HttpUtility.UrlDecode(model.Note);
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
                    item.IsDelete = true;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

    }
}
