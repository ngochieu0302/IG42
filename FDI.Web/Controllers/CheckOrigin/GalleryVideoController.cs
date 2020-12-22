using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.GetAPI;
using FDI.GetAPI.CheckOrigin;
using FDI.Simple;
using FDI.Utils;


namespace FDI.Web.Controllers
{
    public class GalleryVideoController : BaseController
    {
        readonly GalleryVideoAPI _api = new GalleryVideoAPI();
        readonly Gallery_VideoDA _da = new Gallery_VideoDA("#");
        readonly CategoryAPI _categoryApi = new CategoryAPI();
        readonly SourceAPI _sourceApi = new SourceAPI();
        static string _fileName = "";
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new Gallery_Video();
            if (DoAction == ActionType.Edit)
                model = _da.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.CategoryID = _categoryApi.GetAll();
            ViewBag.lstsource = _sourceApi.GetList();
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var model = _api.GetItemById(ArrId.FirstOrDefault());
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);

            var lstID = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    if (_api.Update(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Show:
                    if (_api.Show(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    _api.Show(lstID);
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Hide:
                    if (_api.Hide(url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
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


        //public ActionResult AjaxFormFiles()
        //{
        //    var model = _categoryApi.GetChildByParentId(false);
        //    ViewBag.Action = DoAction;
        //    return View(model);
        //}
        public ActionResult UploadFiles()
        {
            //string[] lst = new string[] { "jpg", "png" };

            var item = new FileObj();
            var urlFolder = ConfigData.TempFolder;
            if (!Directory.Exists(urlFolder))
                Directory.CreateDirectory(urlFolder);
            foreach (string file in Request.Files)
            {
                var hpf = Request.Files[file];
                var fileNameRoot = hpf != null ? hpf.FileName : string.Empty;
                if (hpf != null && hpf.ContentLength == 0)
                    continue;
                if (hpf != null)
                {
                    if (fileNameRoot.Length > 1)
                    {
                        var fileLocal = fileNameRoot.Split('.');
                        var name = "";
                        var index = fileLocal.Length - 2;
                        for (var i = 0; i <= index; i++)
                        {
                            name = name.Trim() + " " + fileLocal[i];
                        }
                        _fileName = FDIUtils.Slug(name) + "-" + DateTime.Now.ToString("MMddHHmmss") + "." + fileLocal[fileLocal.Length - 1];
                        var savedFileName = Path.Combine((urlFolder), Path.GetFileName(_fileName));
                        hpf.SaveAs(savedFileName);
                        item = new FileObj
                        {
                            Name = _fileName,
                            NameRoot = name,
                            Forder = ConfigData.Temp,
                            Icon = "/Images/Icons/file/" + fileLocal[1] + ".png",
                            Error = false
                        };
                    }
                }
            }
            return Json(item);
        }
    }
}
