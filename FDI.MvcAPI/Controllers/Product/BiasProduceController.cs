using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;
using DotNetOpenAuth.Messaging;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class BiasProduceController : BaseApiController
    {
        private readonly BiasProduceDA _da = new BiasProduceDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelBiasProduceItem()
                : new ModelBiasProduceItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListProductCode()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelProductCodeItem()
                : new ModelProductCodeItem { ListItems = _da.GetListProductCode(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetListCostProductUser(int biasId)
        //{
        //    var obj = Request["key"] != Keyapi
        //        ? new List<CostProductUserItem>()
        //        : _da.GetListCostProductUser(ItemId,biasId);
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetListEvaluate(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new List<CostProductCostUserItem>()
                : _da.GetListEvaluate(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBiasProduceItem(string key, int id)
        {
            var obj = key != Keyapi ? new BiasProduceItem() : _da.GetBiasProduceItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEvaluate(string key, int status, string itemId, string lstRet)
        {
            var now = ConvertDate.TotalSeconds(DateTime.Now);
            var lstInt = FDIUtils.StringToListInt(itemId);
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstProductCode = _da.GetListProductCode(lstInt);
                var lst = JsonConvert.DeserializeObject<List<ProductCode_CostUser>>(lstRet);
                foreach (var item in lst)
                {
                    item.UserCreated = UserId();
                    item.DateCreated = now;
                }
                var json = new JavaScriptSerializer().Serialize(lst);

                foreach (var item in lstProductCode)
                {
                    item.ProductCode_CostUser = JsonConvert.DeserializeObject<List<ProductCode_CostUser>>(json);
                    item.Status = status;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Add(string key, string code)
        {
            var msg = new JsonMessage(false,"Thêm mới dữ liệu thành công.");
            var model = new BiasProduce();
            var lst = new List<ProductCode>();
            var now = ConvertDate.TotalSeconds(DateTime.Now);
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                var itemId = Request["ItemID"];
                var start = Request["StartDate_"];
                var end = Request["EndDate_"];
                var startDate = start.StringToDecimal();
                var endDate = end.StringToDecimal();
                model.StartDate = startDate;
                model.EndDate = endDate;
                model.Name = HttpUtility.UrlDecode(model.Name);
                for (var i = 0; i <= model.Quantity; i++)
                {
                    var item = new ProductCode
                    {
                        Code = "ssc-" + itemId + "-" + model.ProductID + "-" + i,
                        Status = 0,
                        DateCreated = now,
                        StartDate = startDate,
                        EndDate = endDate
                    };
                    lst.Add(item);
                }
                model.ProductCodes = lst;
                var lstcpu = GetListItem(code, model.ProductID);
                model.Cost_Product_User = lstcpu;
                model.IsDeleted = false;
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
        public ActionResult Update(string key, string code, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                var lstInt2 = model.Cost_Product_User.Select(c => c.SetupProductId).ToList();
                var lstcpu = GetListItem(code, model.ProductID);
                var lst = model.Cost_Product_User.ToList();
                var result2 = lstcpu.Where(p => lst.All(p2 => p2.SetupProductId != p.SetupProductId)).ToList();
                model.Cost_Product_User.AddRange(result2);
                foreach (var i in lst)
                {
                    var j = lstcpu.FirstOrDefault(c => c.ProductID == i.ProductID);
                    if (j == null)
                    {
                        i.UserId = null;
                    }
                    else
                    {
                        i.UserId = j.UserId;
                    }
                }

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

        public List<Cost_Product_User> GetListItem(string code, int? productId)
        {
            const string url = "Utility/GetListCostProductUser?key=";
            var urlJson = string.Format("{0}{1}", UrlG + url, code);
            var list = Utility.GetObjJson<List<CostProductUserItem>>(urlJson);
            return list.Select(item => new Cost_Product_User
            {
                UserId = item.UserId,
                SetupProductId = item.SetupProductID,
                ProductID = productId
            }).ToList();

        }
    }
}
