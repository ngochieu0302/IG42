using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;

using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class SystemFileController : BaseController
    {
        //
        // GET: /Admin/SystemFile/

        readonly System_FileDA _fileDa = new System_FileDA("#");

        /// <summary>
        /// Trang chủ, index. Load ra grid dưới dạng ajax
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            var model = new ModelFileItem
            {
                ListItem = _fileDa.GetListSimpleByRequest(Request),
                PageHtml = _fileDa.GridHtmlPage
            };
            return View(model);
        }


        /// <summary>
        /// Tải file đính kèm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FileDownload(int id)
        {
            var file = _fileDa.GetById(id);
            if (file.Data == null)
            {
                file.Data = new byte[762];
            }
            return File(file.Data, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
        }

        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var fileModel = _fileDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = fileModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>

        public ActionResult AjaxForm()
        {
            var fileModel = new System_File();
            if (DoAction == ActionType.Edit)
                fileModel = _fileDa.GetById(ArrId.FirstOrDefault());

            ViewData.Model = fileModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }


        /// <summary>
        /// Hứng các giá trị, phục vụ cho thêm, sửa, xóa, ẩn, hiện
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var file = new System_File();

            switch (DoAction)
            {
                case ActionType.Add:
                    file.CreatedDate = DateTime.Now;
                    file.TypeID = 1;
                    UpdateModel(file);
                    _fileDa.Add(file);
                    _fileDa.Save();
                    System.IO.File.Move(Server.MapPath(@"~/Uploads/Temp/" + file.Name), Server.MapPath(@"~/Uploads/files/" + file.Name));
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = file.ID.ToString(),
                        Message = string.Format("Đã thêm mới file đính kèm: <b>{0}</b>", Server.HtmlEncode(file.Name))
                    };
                    break;

                case ActionType.Edit:
                    file = _fileDa.GetById(ArrId.FirstOrDefault());
                    UpdateModel(file);
                    _fileDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = file.ID.ToString(),
                        Message = string.Format("Đã cập nhật file đính kèm: <b>{0}</b>", Server.HtmlEncode(file.Name))
                    };
                    break;

                case ActionType.Delete:
                    var ltsFileItems = _fileDa.GetListByArrID(ArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in ltsFileItems)
                    {
                        if (item.News_News.Any() ||
                            item.FAQ_Answer.Any()
                        )
                        {
                            stbMessage.AppendFormat("File <b>{0}</b>đã được sử dụng.<br />", Server.HtmlEncode(item.Name));
                        }
                        else
                        {
                            var name = item.Name;
                            _fileDa.Delete(item);
                            System.IO.File.Delete(Server.MapPath(@"~/Uploads/files/" + item.Name));
                            stbMessage.AppendFormat("Đã xóa file đính kèm <b>{0}</b>.<br />", Server.HtmlEncode(name));
                        }

                    }
                    msg.ID = string.Join(",", ArrId);
                    _fileDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Dùng cho tra cứu nhanh
        /// </summary>
        /// <returns></returns>
        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _fileDa.GetListSimpleByAutoComplete(term, 10);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }

    }
}
