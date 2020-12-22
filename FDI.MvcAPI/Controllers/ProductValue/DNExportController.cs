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
    public class DNExportController : BaseApiController
    {
        //
        // GET: /DNExport/

        readonly DNExportDA _da = new DNExportDA();
        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DNExportItem>() : _da.GetListSimple(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNExportItem()
                : new ModelDNExportItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNExportItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string codeLogin)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new DN_Export();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                var dateCreated = Request["DateCreated_"] ?? DateTime.Now.ToString("dd/MM/yyyy");
                model.DateExport = dateCreated.StringToDecimal();
                model.Export_Product_Value = GetListExportItem(codeLogin, model.DateExport);   
                
                model.DateCreated = DateTime.Now.TotalSeconds();
                model.AgencyId = Agencyid();
                model.IsDeleted = false;
                model.Code = DateTime.Now.ToString("yyMMddHHmm");
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string codeLogin)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
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
                model.DateExport = dateCreated.StringToDecimal();
                var lst = model.Export_Product_Value.Where(c => c.IsDelete == false).ToList();
                var lstNew = GetListExportItem(codeLogin, model.DateExport);
                //xóa
                var result1 = lst.Where(p => lstNew.All(p2 => p2.ImportID != p.ImportID));
                foreach (var i in result1)
                {
                    i.IsDelete = true;
                    i.DN_Import.QuantityOut = i.DN_Import.QuantityOut - i.Quantity;
                }
                //Chỉnh sửa
                foreach (var i in lst)
                {
                    var j = lstNew.FirstOrDefault(c => c.ImportID == i.ImportID);
                    if (j == null) continue;
                    i.DN_Import.QuantityOut = i.DN_Import.QuantityOut - i.Quantity + j.Quantity;
                    i.Quantity = j.Quantity;
                    i.PriceExport = j.PriceExport;
                }
                //thêm mới
                var result2 = lstNew.Where(p => lst.All(p2 => p2.ImportID != p.ImportID)).ToList();
                model.Export_Product_Value.AddRange(result2);                
                model.Note = HttpUtility.UrlDecode(model.Note);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public List<Export_Product_Value> GetListExportItem(string code,decimal? date)
        {
            const string url = "Utility/GetListExportValue?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<ExportNewItem>>(urlJson);
            return list.Select(item => new Export_Product_Value
            {
                ImportID = item.ImportID,
                Quantity = item.Quantity,
                Price = item.Price,
                PriceExport = item.PriceExport,
                Date = DateTime.Now.TotalSeconds(),
                IsDelete = false
            }).ToList();
        }

        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsDeleted = true;
                        foreach (var i in item.Export_Product_Value)
                        {
                            i.DN_Import.QuantityOut = i.DN_Import.QuantityOut - i.Quantity;
                        }
                    }
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


    }
}
