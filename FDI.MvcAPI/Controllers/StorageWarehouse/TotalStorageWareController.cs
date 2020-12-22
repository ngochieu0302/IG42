using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA.StorageWarehouse;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.StorageWarehouse
{
    public class TotalStorageWareController : BaseApiController
    {
        //
        // GET: /TotalStorageWare/
        readonly TotalStorageWareDA _da = new TotalStorageWareDA("#");
        public ActionResult ListItems(int areaID)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelTotalStorageWareItem()
                : new ModelTotalStorageWareItem { ListItems = _da.GetListSimpleByRequest(Request, areaID), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalStorageWareItem(string key, int id)
        {
            var obj = key != Keyapi ? new TotalStorageWareItem() : _da.GetTotalStorageWareItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddStorage(string key, string json, Guid userId)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var ncc = _da.GetbyId(ItemId);
                var totalquantity = 0;
                var stt = ConvertUtil.ToInt32(Request["do_stt"]);
                for (var i = 1; i <= stt; i++)
                {
                    var name = Request["SalaryType_" + i];
                    if (!string.IsNullOrEmpty(name))
                    {
                        var quantity = ConvertUtil.ToInt32(Request["QuantityActive_add_" + i] ?? "0");
                        var price = ConvertUtil.ToDecimal(Request["Price_add_" + i] ?? "0");
                        var supId = ConvertUtil.ToInt32(Request["DNSupplie_" + i] ?? "0");
                        var today = ConvertUtil.ToDecimal(Request["today"] ?? "0");
                        var hours = ConvertUtil.ToInt32(Request["hourstoday"] ?? "0");
                        var cateId = ConvertUtil.ToInt32(Request["CateId"] ?? "0");
                        var hourI = Request["Hours_add_" + i] ?? "0";
                        var dateI = ConvertUtil.ToDateTime(Request["Date_add_" + i]).TotalSeconds();
                        totalquantity += quantity;
                        var obj = new StorageProduct
                        {
                            UserID = userId,
                            AgencyId = Agencyid(),
                            IsDelete = false,
                            DateCreated = DateTime.Now.TotalSeconds(),
                            Code = DateTime.Now.Millisecond.ToString(),
                            Quantity = quantity,
                            Price = price,
                            SupID = supId,
                            Today = today,
                            Hour = hours,
                            TotalID = ncc.ID,
                            CateID = cateId,
                            DateImport = dateI,
                            HoursImport = int.Parse(hourI)
                        };
                        _da.AddStora(obj);
                    }
                }
                if (totalquantity > (ncc.Quantity - ncc.QuantityOut))
                {
                    msg = new JsonMessage
                    {
                        Erros = true,
                        Message = "Bạn đã xuất quá số lượng thực tế.!"
                    };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ncc.QuantityOut += totalquantity;
                    _da.Save();
                }
                
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
