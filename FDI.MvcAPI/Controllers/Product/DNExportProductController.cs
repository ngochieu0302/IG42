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
    public class DNExportProductController : BaseApiController
    {
        //
        // GET: /DNExport/

        readonly DNExportProductDA _da = new DNExportProductDA();
        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNExportProductItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNExportProductItem()
                : new ModelDNExportProductItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNExportProductItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string codeLogin)
        {
            var model = new DN_ExportProduct();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                var dateCreated = Request["DateCreated_"];
                model.Export_Product = GetListExportItem(codeLogin);
                model.DateExport = dateCreated.StringToDate().TotalSeconds();
                model.DateCreated = DateTime.Now.TotalSeconds();
                model.AgencyId = Agencyid();
                model.Code = DateTime.Now.ToString("yyMMddHHmm");
                model.IsDeleted = false;
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
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
                var lst = model.Export_Product.Where(c=>c.IsDelete==false).ToList();
                var lstNew = GetListExportItem(codeLogin);
                //xóa
                var result1 = lst.Where(p => lstNew.All(p2 => p2.InportProductID != p.InportProductID));
                foreach (var i in result1)
                {
                    i.IsDelete = true;
                    //i.DN_ImportProduct.QuantityOut = i.DN_ImportProduct.QuantityOut - i.Quantity;
                }
                foreach (var i in lst)
                {
                    var j = lstNew.FirstOrDefault(c => c.InportProductID == i.InportProductID);
                    if (j == null) continue;
                    //i.DN_ImportProduct.QuantityOut = i.DN_ImportProduct.QuantityOut - i.Quantity + j.Quantity;
                    i.Quantity = j.Quantity;                    
                }
                //thêm mới
                var result2 = lstNew.Where(p => lst.All(p2 => p2.InportProductID != p.InportProductID)).ToList();
                model.Export_Product.AddRange(result2);
                model.DateExport = dateCreated.StringToDate().TotalSeconds();
                model.Note = HttpUtility.UrlDecode(model.Note);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public List<Export_Product> GetListExportItem(string code)
        {
            const string url = "Utility/GetListExportValue?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ExportProductNewItem>>(urlJson);
            return list.Select(item => new Export_Product
            {
                InportProductID = item.ImportID,
                Quantity = item.Quantity,
                Price = item.Price,
                Date = DateTime.Now.TotalSeconds(),
                IsDelete = false
            }).ToList();

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
