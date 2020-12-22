using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class SetupProductionController : BaseApiController
    {
        //
        // GET: /CostType/
        //readonly CostTypeBL _bl = new CostTypeBL();
        readonly SetupProductionDA _da = new SetupProductionDA();
        public ActionResult GetListSimple(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelSetupProductionItem()
                : new ModelSetupProductionItem { ListItem = _da.GetListSimple(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(string key, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new List<SetupProductionItem>() : _da.GetList(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSetupProductionItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new SetupProductionItem() : _da.GetSetupProductionItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAuto(string key, string keword, int showLimit, int agencyId, int type)
        {
            var obj = key != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, agencyId, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new SetupProduction();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.DateCreate = DateTime.Now.TotalSeconds();
                model.IsDelete = false;
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
        public ActionResult Update(string key, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetById(ItemId);
                    UpdateModel(model);
                    model.Name = HttpUtility.UrlDecode(model.Name);
                    model.Description = HttpUtility.UrlDecode(model.Description);
                    _da.Save();                   
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu Chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
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
