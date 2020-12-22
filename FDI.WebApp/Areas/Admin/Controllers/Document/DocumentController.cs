using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class DocumentController : BaseController
    {
        // GET: /Admin/Document/
        private readonly DocumentsDA _da;
        private readonly DocumentFilesDA _documentFilesDa;
        private readonly CategoryDA _categoryDa;

        public DocumentController()
        {
            _da = new DocumentsDA("#");
            _categoryDa = new CategoryDA("#");
            _documentFilesDa = new DocumentFilesDA();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelDocumentItem
            {
                ListItem = _da.GetListSimpleByRequest(Request),
                PageHtml = _da.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var language = _da.GetById(ArrId.FirstOrDefault());
            ViewData.Model = language;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var document = new DocumentItem();
            if (DoAction == ActionType.Edit)
                document = _da.GetDocumentItem(ArrId.FirstOrDefault());
            ViewBag.CategoryId = _categoryDa.GetChildByParentId(false, (int)ModuleType.Document);
            ViewData.Model = document;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        private void UploadDocument(string fileNameLocal, int idDocument)
        {
            if (!string.IsNullOrEmpty(fileNameLocal))
            {
                var arrDocument = fileNameLocal.Split(',');
                foreach (var item in arrDocument)
                {
                    var arrDocumentChild = item.Split(':');
                    var folder = DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day + "\\";
                    var folderinsert = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                    var urlFolder = ConfigData.DocumentFolder + folder;
                    if (!Directory.Exists(urlFolder))
                        Directory.CreateDirectory(urlFolder);
                    if (arrDocumentChild.Length > 1)
                    {
                        var fileLocal = arrDocumentChild[0].Split('.');
                        var fileName = FDIUtils.Slug(fileLocal[0]) + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + fileLocal[1];
                        var fileTemp = FDIUtils.Slug(fileLocal[0]) + "." + fileLocal[1];
                        System.IO.File.Copy(ConfigData.TempFolder + fileTemp, ConfigData.DocumentFolder + folder + fileName);

                        var documentFile = new DocumentFile
                        {
                            Folder = folderinsert,
                            FileUrl = fileName,
                            DateCreated = DateTime.Now,
                            FileSize = Convert.ToInt32(arrDocumentChild[1]),
                            TypeFile = fileLocal[1],
                            Status = true,
                        };

                        var documentFilesDa = new DocumentFilesDA();                        
                        documentFile.Name = fileName;
                        documentFilesDa.Add(documentFile);
                        documentFilesDa.Save();
                    }
                }
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage(false,"Cập nhật dữ liệu thành công.");
            var document = new Document();
            List<Document> ltsDocuments;
            StringBuilder stbMessage;
            List<int> idValues;
            List<int> lstFileOld;
            List<int> lstFileNews;            
            var lstFiles = Request["getFiles"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(document);

                        document.Slug = FDIUtils.Slug(document.Name);
                        document.LanguageId = Fdisystem.LanguageId;
                        _da.Add(document);
                        _da.Save();
                        if (!string.IsNullOrEmpty(Request["lstFile"]))
                        {
                            UploadDocument(Request["lstFile"], document.Id);
                        }                        
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được cập nhật";
                        Log2File.LogExceptionToFile(ex);
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        document = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(document);
                        document.Slug = FDIUtils.Slug(document.Name);
                        _da.Save();                        
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được cập nhật";
                        Log2File.LogExceptionToFile(ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsDocuments = _da.GetListByArrId(ArrId);                    
                    foreach (var item in ltsDocuments)
                    {
                        item.IsDeleted = true;                     
                    }
                    
                    _da.Save();                    
                    break;
                case ActionType.Show:
                    ltsDocuments = _da.GetListByArrId(ArrId);                  
                    foreach (var item in ltsDocuments)
                    {
                        item.IsShow = true;                        
                    }
                    _da.Save();                   
                    break;

                case ActionType.Hide:
                    ltsDocuments = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsDocuments)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn tài liệu <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();                    
                    break;
                default:
                    msg.Message = "Không có hành động nào được thực hiện.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
