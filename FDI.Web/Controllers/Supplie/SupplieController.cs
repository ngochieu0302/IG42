using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class SupplieController : BaseController
    {
        //
        // GET: /Supplie/
        readonly SupplieAPI _api = new SupplieAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new DNSupplierItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetItemById(UserItem.AgencyID,ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            return View(model);
        }
        
        //public ActionResult ExportExcel()
        //{
        //    var model = _api.GetListExport(UserItem.AgencyID, Request.Url.Query);
        //    var fileName = "Danh_sach_danh_gia_nv.xlsx";
        //    var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
        //    var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
        //    if (!Directory.Exists(folder))
        //    {
        //        Directory.CreateDirectory(folder);
        //    }
        //    var di = new DirectoryInfo(folder);
        //    foreach (var file in di.GetFiles())
        //    {
        //        file.Delete();
        //    }
        //    foreach (var dir in di.GetDirectories())
        //    {
        //        dir.Delete(true);
        //    }
        //    Excels.ExportListCard(filePath, model);
        //    var bytes = System.IO.File.ReadAllBytes(filePath);
        //    return File(bytes, "text/xls", fileName);
        //}
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            var lstID = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(UserItem.AgencyID,url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    if (_api.Update(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Show:
                    if (_api.Show(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    _api.Show(lstID);
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Hide:
                    if (_api.Hide(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                        break;
                    }
                    _api.Hide(lstID);
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Delete:
                    _api.Delete(lstID);
                    msg.Message = "Cập nhật thành công.";
                    break;
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}