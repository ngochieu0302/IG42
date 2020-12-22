using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers.Product
{
    public class ImportWarehouseController : BaseApiController
    {
        // GET: /ImportWarehouse/
        private readonly CateValueDA _dlDa = new CateValueDA();
        private readonly ProductDA _da = new ProductDA();
        public static List<CateValueAddItem> Cates = new List<CateValueAddItem>();
        public ActionResult GetListSimpleProductDetail(string codec)
        {
            if (Request["key"] == Keyapi)
            {
                var obj = Cates.FirstOrDefault(m => m.Barcode == codec);
                if (obj == null)
                {
                    var stt = 1;
                    var listItemDetail = _da.GetListSimpleProductDetail();
                    foreach (var item in listItemDetail)
                    {
                        item.Stt = stt;
                        item.Barcode = FDIUtils.RandomCode(8);
                        item.Code = "f" + stt;
                        stt++;
                        var objinew = new ImportProductAddItem
                        {
                            Stt = 1,
                            BarCode = FDIUtils.RandomCode(10),
                            Price = item.Price,
                            Value = 0,
                            PriceNew = 0,
                            Quantity = 1,
                        };
                        item.ListImportProductItems = new List<ImportProductAddItem> { objinew };
                    }
                    obj = new CateValueAddItem
                    {
                        Stt = 1,
                        Barcode = codec,
                        PriceNew = 0,
                        Pi = 0,
                        ListProductValueItems = listItemDetail,
                    };
                    Cates.Add(obj);
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            return Json(new CateValueAddItem(), JsonRequestBehavior.AllowGet);
        }

        public decimal Date(out decimal dateEnd)
        {
            var today = DateTime.Today;
            var h = DateTime.Now.Hour;
            var time = 18;
            if (h < 4) time = 4;
            else if (h < 8) time = 8;
            else if (h < 15) time = 15;
            var date = today.AddHours(time);
            dateEnd = date.AddDays(3).TotalSeconds();
            return date.TotalSeconds();
        }

        public ActionResult Actions(string key, string codec, string codep, string codei, decimal value, int type, int pi)
        {
            if (key == Keyapi)
            {
                decimal dateEnd;
                var date = Date(out dateEnd);
                var obj = Cates.FirstOrDefault(m => m.Barcode == codec);
                if (obj != null)
                {
                    obj.Pi = pi;
                    var objp = obj.ListProductValueItems.FirstOrDefault(m => m.Barcode == codep);
                    if (objp != null)
                    {
                        var a = (objp.Price * value) ?? 0;
                        var pricetotal = Math.Round(a / 500000, 0) * 500;
                        var obji = objp.ListImportProductItems.FirstOrDefault(m => m.BarCode == codei);
                        if (obji != null)
                        {
                            if (type == 2)
                            {
                                obj.Stt = obji.Stt;
                                obji.Value = value;
                                obji.PriceNew = pricetotal;
                                obji.IsIn = true;
                                obji.Date = date;
                                obji.DateEnd = dateEnd;
                                return Json(obji, JsonRequestBehavior.AllowGet);
                            }
                            if (type == 3) objp.ListImportProductItems.Remove(obji);
                        }
                        else
                        {
                            obji = new ImportProductAddItem
                            {
                                Stt = objp.ListImportProductItems.OrderByDescending(m => m.Stt).Select(m => m.Stt).FirstOrDefault() + 1,
                                BarCode = codei,
                                Price = objp.Price,
                                Value = value,
                                PriceNew = pricetotal,
                                Quantity = 1,
                                IsIn = value > 0,
                                Date = date,
                                DateEnd = dateEnd
                            };
                            obj.Stt = obji.Stt;
                            objp.ListImportProductItems.Add(obji);
                            return Json(obji, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return Json(new ImportProductAddItem(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActionsDeleteAll(string key, string codec)
        {
            if (key == Keyapi)
            {
                var obj = Cates.FirstOrDefault(m => m.Barcode == codec);
                if (obj == null)
                    return Json(1, JsonRequestBehavior.AllowGet);
                Cates.Remove(obj);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save(string key, string codec, int agencyid, int cateid, int unit, int areaid)
        {
            if (key == Keyapi)
            {
                var objorder = _dlDa.GetObjOrder(areaid);
                var date = DateTime.Now.TotalSeconds();
                var obja = Cates.FirstOrDefault(m => m.Barcode == codec);
                if (obja != null)
                {
                    var list = (from productValue in obja.ListProductValueItems
                                let listimport = productValue.ListImportProductItems.Where(m => m.IsIn && m.Value > 0).Select(importproduct => new DN_ImportProduct
                                {
                                    GID = Guid.NewGuid(),
                                    Date = importproduct.Date,
                                    Value = importproduct.Value / 1000,
                                    Price = importproduct.Price,
                                    PriceNew = importproduct.PriceNew,
                                    DateEnd = importproduct.DateEnd,
                                    Quantity = 1,
                                    QuantityOut = 0,
                                    IsDelete = false,
                                    BarCode = importproduct.BarCode,
                                    AgencyId = objorder != null  ? objorder.AgencyID : agencyid,
                                    //ProductValueID = 0
                                }).ToList()
                                select new Product_Value
                                {
                                    DateCreated = date,
                                    DateImport = date,
                                    Barcode = productValue.Barcode,
                                    PriceNew = productValue.ListImportProductItems.Where(m => m.IsIn && m.Value > 0).Sum(l => l.PriceNew),
                                    Value = productValue.ListImportProductItems.Where(m => m.IsIn && m.Value > 0).Sum(l => l.Value) / 1000,
                                    PriceCost = productValue.Price,
                                    Quantity = 1,
                                    QuantityOut = 0,
                                    IsDelete = false,
                                    UnitID = 1,
                                    AgencyId = objorder != null ? objorder.AgencyID : agencyid,
                                    ProductID = productValue.ID,
                                    DN_ImportProduct = listimport
                                }).ToList();
                    var obj = new Cate_Value
                    {
                        CateID = cateid,
                        UnitID = unit,
                        IsDelete = false,
                        Barcode = obja.Barcode,
                        Quantity = 1,
                        QuantityOut = 0,
                        AgencyId = agencyid,
                        DateCreated = date,
                        PriceNew = obja.ListProductValueItems.Sum(m => m.ListImportProductItems.Where(n => n.IsIn).Sum(l => l.PriceNew)),
                        DateImport = date,
                        Value = obja.ListProductValueItems.Sum(m => m.ListImportProductItems.Where(n => n.IsIn).Sum(l => l.Value)) / 1000,
                        Product_Value = list,
                    };
                    if (objorder != null) obj.RequestWareID = objorder.GID;
                    _dlDa.Add(obj);
                    _dlDa.Save();
                    Cates.Remove(obja);
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}