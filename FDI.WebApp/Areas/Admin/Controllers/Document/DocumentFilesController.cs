using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Web.Script.Serialization;

namespace FDI.Areas.Admin.Controllers
{
    public class DocumentFilesController : BaseController
    {
        readonly DocumentFilesDA _da = new DocumentFilesDA("#");
        readonly CategoryDA _categoryDa = new CategoryDA("#");

        //#region Dùng trong Upload ảnh
        ///// <summary>
        ///// Dùng khi submit
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult AjaxFormPictureSubmit()
        //{
        //    var date = DateTime.Now;
        //    var msg = new JsonMessage(false, "Thêm mới hình ảnh thành công.");
        //    var folder = date.Year + "\\" + date.Month + "\\" + date.Day + "\\";
        //    var fileinsert = date.Year + "/" + date.Month + "/" + date.Day + "/";
        //    var folderinsert = fileinsert;
        //    var urlFolder = ConfigData.TempFolder;
        //    var lstFile = Request["lstFile"];
        //    var lstP = JsonConvert.DeserializeObject<List<FileUploadItem>>(lstFile);
        //    try
        //    {
        //        foreach (var item in lstP)
        //        {
        //            var fileName = item.Url;
        //            var imageSource = Image.FromFile(urlFolder + fileName);
        //            var checkfolder = false;
        //            if (Request["ckImage_" + (int)FolderImage.Originals] != null)
        //            {
        //                checkfolder = true;
        //                ImageProcess.CreateForder(ConfigData.OriginalFolder); // tạo forder Năm / Tháng / Ngày
        //                if (imageSource.Width > ConfigData.ImageFullHdFile.Width)
        //                {
        //                    var image = ImageProcess.ResizeImage(imageSource, ConfigData.ImageFullHdFile);
        //                    ImageProcess.SaveJpeg(ConfigData.OriginalFolder + folder + fileName, new Bitmap(image), 92L); // Save file Original
        //                }
        //                else
        //                {
        //                    System.IO.File.Copy(urlFolder + fileName, ConfigData.OriginalFolder + folder + fileName);
        //                }
        //                folderinsert = "Originals/" + fileinsert;
        //            }

        //            if (Request["ckImage_" + (int)FolderImage.Images] != null)
        //            {
        //                checkfolder = true;
        //                ImageProcess.CreateForder(ConfigData.ImageFolder); // tạo forder Năm / Tháng / Ngày
        //                if (imageSource.Width > ConfigData.ImageHdFile.Width)
        //                {
        //                    var image = ImageProcess.ResizeImage(imageSource, ConfigData.ImageHdFile);
        //                    ImageProcess.SaveJpeg(ConfigData.ImageFolder + folder + fileName, new Bitmap(image), 92L); // Save file Images
        //                }
        //                else
        //                {
        //                    System.IO.File.Copy(ConfigData.ImageFolder + fileName, ConfigData.ImageFolder + folder + fileName);
        //                }
        //                folderinsert = "Images/" + fileinsert;
        //            }

        //            //Resize ảnh 640
        //            if (Request["ckImage_" + (int)FolderImage.Mediums] != null)
        //            {
        //                checkfolder = true;
        //                ImageProcess.CreateForder(ConfigData.ImageUploadMediumFolder); // tạo forder Năm / Tháng / Ngày
        //                if (imageSource.Width > ConfigData.ImageMediumFile.Width)
        //                {
        //                    var image = ImageProcess.ResizeImage(imageSource, ConfigData.ImageFullHdFile);
        //                    ImageProcess.SaveJpeg(ConfigData.ImageUploadMediumFolder + folder + fileName, new Bitmap(image), 92L); // Save file Medium
        //                }
        //                else
        //                {
        //                    System.IO.File.Copy(urlFolder + fileName, ConfigData.ImageUploadMediumFolder + folder + fileName);
        //                }
        //                folderinsert = "Mediums/" + fileinsert;
        //            }

        //            if (!checkfolder)
        //            {
        //                folderinsert = "Thumbs/" + fileinsert;
        //            }

        //            if (Request["ckImage_" + (int)FolderImage.Thumbs] != null)
        //            {
        //                ImageProcess.CreateForder(ConfigData.ThumbsFolder);
        //            }

        //            if (imageSource.Width < ConfigData.ImageThumbsSize.Width)
        //            {
        //                ImageProcess.SaveJpeg(ConfigData.ThumbsFolder + folder + fileName, new Bitmap(imageSource), 92L); // Save file Thumbs
        //            }
        //            else
        //            {
        //                imageSource = ImageProcess.ResizeImage(imageSource, ConfigData.ImageThumbsSize);
        //                ImageProcess.SaveJpeg(ConfigData.ThumbsFolder + folder + fileName, new Bitmap(imageSource), 92L); // Save file Thumbs                   
        //            }
        //            imageSource.Dispose();
        //            //Lấy thông tin cần thiết
        //            var picture = new Gallery_Picture
        //            {
        //                Type = !string.IsNullOrEmpty(Request["type"]) ? Convert.ToInt32(Request["type"]) : 0,
        //                CategoryID =
        //                    !string.IsNullOrEmpty(Request["CategoryID"]) ? Convert.ToInt32(Request["CategoryID"]) : 1,

        //                Folder = folderinsert,
        //                Name = item.Name,
        //                DateCreated = DateTime.Now,
        //                IsShow = true,
        //                Url = fileName,
        //                IsDeleted = false
        //            };
        //            _da.Add(picture);
        //        }
        //        _da.Save();
        //        try
        //        {
        //            var di = new DirectoryInfo(urlFolder);
        //            foreach (var file in di.GetFiles())
        //            {
        //                file.Delete();
        //            }
        //            foreach (var dir in di.GetDirectories())
        //            {
        //                dir.Delete(true);
        //            }
        //        }
        //        catch { }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg.Erros = true;
        //        msg.Message = "Thêm mới hình ảnh thất bại.";
        //    }
        //    return Json(msg, JsonRequestBehavior.AllowGet);
        //}

        ///// <summary>
        ///// Sau khi upload xong, cập nhật thông tin cho picture
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult AjaxFormPictureUpdate()
        //{
        //    var arrFile = Request["fileData"];
        //    var oSerializer = new JavaScriptSerializer();
        //    var ltsFileobj = oSerializer.Deserialize<List<FileObj>>(arrFile);
        //    var type = Request["Type"];
        //    var categoryId = Request["CategoryID"];
        //    var model = new ModelFileObj
        //    {
        //        ListCategoryItem = _categoryDa.GetChildByParentId(false),
        //        ListItem = ltsFileobj,
        //        Type = !string.IsNullOrEmpty(type) ? Convert.ToInt32(type) : 0,
        //        CategoryId = !string.IsNullOrEmpty(categoryId) ? Convert.ToInt32(categoryId) : 0,
        //        Action = DoAction.ToString()
        //    };
        //    return View(model);
        //}

        public ActionResult AjaxFormFiles()
        {
            var model = _categoryDa.GetChildByParentId(false, (int)ModuleType.Document);
            ViewBag.Action = DoAction;
            return View(model);
        }
        //#endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelDocumentFilesItem
            {
                ListItem = _da.GetListByRequest(Request),
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult ListNewItems()
        {
            var model = new ModelDocumentFilesItem
            {
                ListItem = _da.GetListByRequest(Request),
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var model = new DocumentFilesItem();
            if (DoAction == ActionType.Edit)
                model = _da.GetDocumentFilesItem(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }

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
                        //if (!lst.Contains(fileLocal[1].ToLower()))
                        //{
                        //    item.Error = true;
                        //    continue;
                        //}
                        var fileName = FDIUtils.Slug(fileLocal[0]) + "-" + DateTime.Now.ToString("MMddHHmmss") + "." + fileLocal[1];
                        var savedFileName = Path.Combine((urlFolder), Path.GetFileName(fileName));
                        hpf.SaveAs(savedFileName);
                        item = new FileObj
                        {
                            Name = fileName,
                            NameRoot = fileLocal[0],
                            Forder = ConfigData.Temp,
                            Icon = "/Images/Icons/file/" + fileLocal[1] + ".png",
                            Error = false
                        };
                    }
                }
            }
            return Json(item);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();            
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = new JsonMessage(false, "Cập nhât dữ liệu thành công.");
                    var lstFile = Request["lstFile"];
                    var lstP = JsonConvert.DeserializeObject<List<FileUploadItem>>(lstFile);
                    try
                    {
                        if (lstP.Count > 0)
                        {
                            foreach (var item in lstP)
                            {
                                var folder = DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day + "\\";
                                var folderinsert = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                                var urlFolder = ConfigData.DocumentFolder + folder;
                                if (!Directory.Exists(urlFolder))
                                    Directory.CreateDirectory(urlFolder);
                                if (item.Url.Length > 1)
                                {
                                    var fileLocal = item.Url.Split('.');
                                    var fileName = FDIUtils.Slug(fileLocal[0]) + "." + fileLocal[1];
                                    System.IO.File.Copy(ConfigData.TempFolder + fileName, urlFolder + fileName);
                                    var fileItem = new DocumentFile
                                    {
                                        Folder = folderinsert,
                                        FileUrl = fileName,
                                        DateCreated = DateTime.Now,
                                        TypeFile = fileLocal[1],
                                        Status = true,
                                        IsDeleted = false,
                                        Name = item.Name
                                    };
                                    _da.Add(fileItem);
                                }
                            }
                        }
                        _da.Save();
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        Log2File.LogExceptionToFile(ex);
                        msg.Message = "Dữ liệu chưa được cập nhật.";
                    }
                    break;
                case ActionType.Edit:
                    msg = new JsonMessage(false, "Cập nhât dữ liệu thành công.");
                    try
                    {
                        var model = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(model);
                        _da.Save();
                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        Log2File.LogExceptionToFile(ex);
                        msg.Message = "Dữ liệu chưa được cập nhật.";
                    }
                    break;
                case ActionType.Delete:
                    msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
                    try
                    {
                        var lst = _da.GetListByArrId(ArrId);
                        foreach (var item in lst)
                        {
                            item.IsDeleted = true;
                        }
                        _da.Save();

                    }
                    catch (Exception ex)
                    {
                        msg.Erros = true;
                        msg.Message = "Dữ liệu chưa được xóa";
                    }
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
