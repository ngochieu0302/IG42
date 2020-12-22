using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.StorageWarehouse;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.StorageWarehouse
{
    public class StorageFreightWarehouseController : BaseApiController
    {
        //
        // GET: /StorageFreightWarehouse/
        readonly StorageFreightWarehouseDA _da = new StorageFreightWarehouseDA();
        readonly DNUserDA _dnUserDa = new DNUserDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelStorageFreightWarehouseItem()
                : new ModelStorageFreightWarehouseItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsAll()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelStorageFreightWarehouseItem()
                : new ModelStorageFreightWarehouseItem { ListItems = _da.GetListSimpleAllByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsRecive()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelStorageFreightWarehouseItem()
                : new ModelStorageFreightWarehouseItem { ListItems = _da.GetListSimpleByRequestRecive(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListExcel(string key)
        {
            var obj = key != Keyapi ? new List<StorageFreightWarehouseItem>() : _da.GetListExcel(Request, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStorageFreightWarehousesItem(string key, int id)
        {
            var obj = key != Keyapi ? new StorageFreightWarehouseItem() : _da.GetStorageFreightWarehousesItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId, int type)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsNotActive(string key,bool active)
        {
            var obj = key != Keyapi ? new List<StorageFreightWarehouseItemNew>() : _da.ListItemsNotActive(active);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string codeLogin, string port = ":4000")
        {
            var model = new StorageFreightWarehouse();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                var dateCreated = Request["DateCreated_"];
                var date = ConvertDate.StringToDate(dateCreated);
                model.FreightWarehouses = GetListImportItem(codeLogin, date);
                //model.DateImport = date.TotalSeconds();
                model.Status = (int)StatusWarehouse.Pending;
                model.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                model.Code = DateTime.Now.ToString("yyMMddHHmm");
                model.AgencyId = Agencyid();
                model.IsDelete = false;
                _da.Add(model);
                _da.Save();
                var user = _dnUserDa.GetById(model.UserID ?? new Guid());
                var jsonnew = new StorageFreightWarehouseItemNew()
                {
                    ID = model.ID,
                    Note = model.Note,
                    Fullname = user.LoweredUserName,
                };
                var json = new JavaScriptSerializer().Serialize(jsonnew);
                Node(port + "/addnotify/" + json);
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
                model.IsActive = false;
                model.Status = (int)StatusWarehouse.Pending;
                var dateCreated = Request["DateCreated_"];
                var date = ConvertDate.StringToDate(dateCreated);
                var lst = model.FreightWarehouses.Where(c => c.IsDelete == false).ToList();
                var lstNew = GetListImportItem(codeLogin, date);
                //xóa
                var result1 = lst.Where(p => lstNew.All(p2 => p2.ProductID != p.ProductID)).ToList();
                foreach (var i in result1)
                {
                    i.IsDelete = true;
                }
                //sửa
                foreach (var i in lst)
                {
                    var j = lstNew.FirstOrDefault(c => c.ProductID == i.ProductID);
                    if (j == null) continue;
                    i.Quantity = j.Quantity;
                    i.Price = j.Price;
                    i.Date = j.Date;
                }
                //thêm mới
                var result2 = lstNew.Where(p => lst.All(p2 => p2.ProductID != p.ProductID)).ToList();
                model.FreightWarehouses.AddRange(result2);
                model.DateImport = ConvertDate.TotalSeconds(date);
                model.Note = HttpUtility.UrlDecode(model.Note);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ActiveFrei(string key, Guid userId, string lstArrId)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var model = _da.GetListArrId(lstInt);
               foreach (var item in model.Where(c => c.IsDelete == false))
                {
                    item.IsActive = true;
                    item.DateActive = ConvertDate.TotalSeconds(DateTime.Now);
                    item.Status = (int) StatusWarehouse.Waitting;
                    item.UserActive = userId;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateActive(string key, string codeLogin, Guid userIdActive)
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
                model.Status = (int)StatusWarehouse.Complete;
                model.IsActive = true;
                model.DateActive = ConvertDate.TotalSeconds(DateTime.Now);
                //model.Status = (int)StatusWarehouse.Waitting;
                model.UserActive = userIdActive;
                model.keyreq = Guid.NewGuid();
                var dateCreated = Request["DateCreated_"];
                var date = ConvertUtil.ToDateTime(dateCreated);
                var lst = model.FreightWareHouse_Active.Where(c => c.IsDelete == false).ToList();
                var lstNew = GetListImportItemActive(codeLogin, date);
                //xóa
                var result1 = lst.Where(p => lstNew.All(p2 => p2.ImportProductGID != p.ImportProductGID)).ToList();
                foreach (var i in result1)
                {
                    i.IsDelete = true;
                }
                //sửa
                //foreach (var i in lst)
                //{
                //    var j = lstNew.FirstOrDefault(c => c.ProductID == i.ProductID);
                //    if (j == null) continue;
                //    i.Price = j.Price;
                //    i.Quantity = j.Quantity;
                //    i.TotalPrice = j.TotalPrice;
                //    i.Date = j.Date;
                //}
                //thêm mới
                var result2 = lstNew.Where(p => lst.All(p2 => p2.ImportProductGID != p.ImportProductGID)).ToList();
                model.FreightWareHouse_Active.AddRange(result2);
                //model.DateImport = date.TotalSeconds();
                model.Note = HttpUtility.UrlDecode(model.Note);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public List<FreightWarehouse> GetListImportItem(string code, DateTime date)
        {
            const string url = "Utility/GetListImportFrei?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<FreightWarehouseNewItem>>(urlJson);
            return list.Select(item => new FreightWarehouse()
            {
                Quantity = item.Quantity,
                Price = item.Price,
                ProductID = item.ProductID,
                IsDelete = false,
                Date = ConvertDate.StringToDecimal(item.DateS),
                TotalPrice = item.Quantity * item.Price
            }).ToList();
        }
        public List<FreightWareHouse_Active> GetListImportItemActive(string code, DateTime date)
        {
            const string url = "Utility/GetListImportFreiActive?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<FreightWarehouseActiveNewItem>>(urlJson);
            return list.Select(item => new FreightWareHouse_Active()
            {
                Quantity = item.Quantity,
                Price = item.Price,
                //ProductID = item.ProductID,
                IsDelete = false,
                Date = ConvertDate.StringToDecimal(item.DateS),
                DateEnd = ConvertDate.StringToDecimal(item.DateE),
                BarCode = item.BarCode,
                ValueWeight = item.ValueWeight,
                ImportProductGID = item.Idimport,
            }).ToList();
        }
        public List<Guid> GetListImportedItem(string code)
        {
            const string url = "Utility/GetListImportFreiActive?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<FreightWarehouseActiveNewItem>>(urlJson);
             var lstArr = string.Join(",", list.Select(c => c.Idimport));
            var lstInt = FDIUtils.StringToListGuid(lstArr);
            return lstInt;
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
        public ActionResult Imported(string key, string codeLogin)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
               
                var storage = _da.GetById(ItemId);
                storage.Status = (int)StatusWarehouse.Imported;
                storage.DateImport = ConvertDate.TotalSeconds(DateTime.Now);
                var lstInt = GetListImportedItem(codeLogin);
                var temp = _da.GetListArrIdImport(lstInt);
                foreach (var dnImportProduct in temp)
                {
                    dnImportProduct.AgencyId = Agencyid();
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
