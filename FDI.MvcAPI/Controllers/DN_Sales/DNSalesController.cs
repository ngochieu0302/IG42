using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA.DN_Sales;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.DN_Sales
{
    public class DNSalesController : BaseApiController
    {
        //
        // GET: /DNSales/
        readonly DNSalesDA _da = new DNSalesDA("#");
        public ActionResult ListItems(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNSalesItem()
                : new ModelDNSalesItem { ListItems = _da.GetListSimpleByRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsCode(int agencyId, int id)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelSaleCodeItem()
                : new ModelSaleCodeItem { ListItems = _da.GetListSimpleByRequestCode(Request, agencyId,id), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDNSalesItem(string key, int id)
        {
            var obj = key != Keyapi ? new DNSalesItem() : _da.GetDNSalesItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDNSalesItembyCode(string key, string code, int agencyId)
        {
            var obj = key != Keyapi ? new SaleCodeItem() : _da.GetDNSalesItembyCode(code, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, int agencyId, Guid userId)
        {
            var model = new DN_Sale();
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                var lstproduct = Request["values-arr-product"];
                var dateE = Request["DateEnd_"];
                var dateS = Request["DateStart_"];
                var lstCate = Request["ListCateId"];
                UpdateModel(model);
                model.IsDeleted = false;
                model.IsShow = true;
                model.AgencyId = agencyId;
                model.IsAgency = false;
                model.UserCreate = userId;
                model.DateEnd = !string.IsNullOrEmpty(dateE)
                    ? ConvertUtil.ToDateTime(dateE).TotalSeconds()
                    : DateTime.Now.TotalSeconds();
                model.DateStart = !string.IsNullOrEmpty(dateS)
                    ? ConvertUtil.ToDateTime(dateS).TotalSeconds()
                    : DateTime.Now.TotalSeconds();
                if ((!model.QuantityCode.HasValue || model.QuantityCode == 0) && model.IsAll != true && (!model.TotalOrder.HasValue || model.TotalOrder == 0))
                {
                        if (string.IsNullOrEmpty(lstCate))
                        {
                            model.Categories = _da.GetListCateByArrId(lstCate);
                            model.IsAll = false;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(lstproduct))
                            {
                                var lstInt = FDIUtils.StringToListInt(lstproduct);
                                model.Shop_Product_Detail = _da.GetListIntProductByArrId(lstInt);
                                model.IsAll = false;
                            }
                        }
                }
                else
                {
                    model.IsAll = false;
                }
               if (model.IsAll == true)
                {
                    model.QuantityCode = 0;
                }
               if (model.QuantityCode > 0)
               {
                   model.TotalOrder = 0;
               }
               if (model.TotalOrder > 0)
               {
                   model.QuantityCode = 0;
               }
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, Guid userId)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstproduct = Request["values-arr-product"];
                var dateE = Request["DateEnd_"];
                var dateS = Request["DateStart_"];
                var lstCate = Request["ListCateId"];
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.UserUpdate = userId;
                model.DateEnd = !string.IsNullOrEmpty(dateE)
                    ? ConvertUtil.ToDateTime(dateE).TotalSeconds()
                    : DateTime.Now.TotalSeconds();
                model.DateStart = !string.IsNullOrEmpty(dateS)
                    ? ConvertUtil.ToDateTime(dateS).TotalSeconds()
                    : DateTime.Now.TotalSeconds();
                model.Shop_Product_Detail.Clear();
                if ((!model.QuantityCode.HasValue || model.QuantityCode == 0) && model.IsAll != true)
                {
                    if (string.IsNullOrEmpty(lstCate))
                    {
                        model.Categories = _da.GetListCateByArrId(lstCate);
                        model.IsAll = false;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(lstproduct))
                        {
                            var lstInt = FDIUtils.StringToListInt(lstproduct);
                            model.Shop_Product_Detail = _da.GetListIntProductByArrId(lstInt);
                            model.IsAll = false;
                        }
                    }
                }
                else
                {
                    model.IsAll = false;
                }
                if (model.IsAll == true)
                {
                    model.QuantityCode = 0;
                    model.TotalOrder = 0;
                }
                if (model.QuantityCode > 0)
                {
                    model.TotalOrder = 0;
                }
                if (model.TotalOrder > 0)
                {
                    model.QuantityCode = 0;
                }
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListByArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsDeleted = true;
                    }
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Truy cập thất bại.";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
