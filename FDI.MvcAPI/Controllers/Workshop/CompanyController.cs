using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class CompanyController : BaseApiAuthController
    {
        //
        // GET: /Company/
        readonly CompanyDA _da = new CompanyDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCompanyItem()
                : new ModelCompanyItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new CompanyItem() : _da.GetItembyId(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<CompanyItem>() : _da.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key,Guid userId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = new Company();
                    UpdateModel(model);
                    var date = DateTime.Now.TotalSeconds();
                    model.UserID = userId;
                    model.IsDeleted = false;
                    model.DateCreate = date;
                    _da.Add(model);
                    _da.Save();
                }
                else
                {
                    msg = new JsonMessage(true, "Có lỗi xảy ra!.");
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, Guid userId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetbyId(ItemId);
                    UpdateModel(model);
                    var date = DateTime.Now.TotalSeconds();
                    model.UserID = userId;
                    model.DateUpdate = date;
                    _da.Save();
                }
                else
                {
                    msg = new JsonMessage(true, "Có lỗi xảy ra!.");
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            if (key != Keyapi) return Json(msg, JsonRequestBehavior.AllowGet);
            var lst = _da.GetListArrId(lstArrId);
            foreach (var item in lst)
                item.IsDeleted = true;
            _da.Save();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
