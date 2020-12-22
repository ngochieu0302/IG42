using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using FDI.Web.Models;
using Newtonsoft.Json;

namespace FDI.Web.Controllers
{
    public class GalleryPictureController : BaseController
    {
        readonly GalleryPictureAPI _galleryPictureApi;
        readonly CategoryAPI _categoryApi;

        public GalleryPictureController()
        {
            _categoryApi = new CategoryAPI();
            _galleryPictureApi = new GalleryPictureAPI();
        }

        #region Dùng trong Upload ảnh
        /// <summary>
        /// Dùng khi submit
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AjaxFormPictureSubmit()
        {
            var date = DateTime.Now;
            var msg = new JsonMessage(true, "Không có hình ảnh nào được thêm mới");
            var folder = date.Year + "\\" + date.Month + "\\" + date.Day + "\\";
            var fileinsert = date.Year + "/" + date.Month + "/" + date.Day + "/";
            var folderinsert = fileinsert;

            var urlFolder = ConfigData.TempFolder;
            var lstFile = Request["lstFile"];
            var lstP = JsonConvert.DeserializeObject<List<FileUploadItem>>(lstFile);
            try
            {
                foreach (var item in lstP)
                {
                    var fileName = item.Url;
                    var imageSource = Image.FromFile(urlFolder + fileName);
                    var checkfolder = false;
                    if (Request["ckImage_" + (int)FolderImage.Originals] != null)
                    {
                        checkfolder = true;
                        ImageProcess.CreateForder(ConfigData.OriginalFolder); // tạo forder Năm / Tháng / Ngày
                        if (imageSource.Width > ConfigData.ImageFullHdFile.Width)
                        {
                            var image = ImageProcess.ResizeImage(imageSource, ConfigData.ImageFullHdFile);
                            ImageProcess.SaveJpeg(ConfigData.OriginalFolder + folder + fileName, new Bitmap(image), 92L); // Save file Original
                        }
                        else
                        {
                            System.IO.File.Copy(urlFolder + fileName, ConfigData.OriginalFolder + folder + fileName);
                        }
                        folderinsert = "Originals/" + fileinsert;
                    }

                    if (Request["ckImage_" + (int)FolderImage.Images] != null)
                    {
                        checkfolder = true;
                        ImageProcess.CreateForder(ConfigData.ImageFolder); // tạo forder Năm / Tháng / Ngày
                        if (imageSource.Width > ConfigData.ImageHdFile.Width)
                        {
                            var image = ImageProcess.ResizeImage(imageSource, ConfigData.ImageHdFile);
                            ImageProcess.SaveJpeg(ConfigData.ImageFolder + folder + fileName, new Bitmap(image), 92L); // Save file Images
                        }
                        else
                        {
                            System.IO.File.Copy(ConfigData.ImageFolder + fileName, ConfigData.ImageFolder + folder + fileName);
                        }
                        folderinsert = "Images/" + fileinsert;
                    }

                    //Resize ảnh 640
                    if (Request["ckImage_" + (int)FolderImage.Mediums] != null)
                    {
                        checkfolder = true;
                        ImageProcess.CreateForder(ConfigData.ImageUploadMediumFolder); // tạo forder Năm / Tháng / Ngày
                        if (imageSource.Width > ConfigData.ImageMediumFile.Width)
                        {
                            var image = ImageProcess.ResizeImage(imageSource, ConfigData.ImageFullHdFile);
                            ImageProcess.SaveJpeg(ConfigData.ImageUploadMediumFolder + folder + fileName, new Bitmap(image), 92L); // Save file Medium
                        }
                        else
                        {
                            System.IO.File.Copy(urlFolder + fileName, ConfigData.ImageUploadMediumFolder + folder + fileName);
                        }
                        folderinsert = "Mediums/" + fileinsert;
                    }

                    if (!checkfolder)
                    {
                        folderinsert = "Thumbs/" + fileinsert;
                    }

                    if (Request["ckImage_" + (int)FolderImage.Thumbs] != null)
                    {
                        ImageProcess.CreateForder(ConfigData.ThumbsFolder);
                    }

                    if (imageSource.Width < ConfigData.ImageThumbsSize.Width)
                    {
                        ImageProcess.SaveJpeg(ConfigData.ThumbsFolder + folder + fileName, new Bitmap(imageSource), 92L); // Save file Thumbs
                    }
                    else
                    {
                        imageSource = ImageProcess.ResizeImage(imageSource, ConfigData.ImageThumbsSize);
                        ImageProcess.SaveJpeg(ConfigData.ThumbsFolder + folder + fileName, new Bitmap(imageSource), 92L); // Save file Thumbs                   
                    }
                    imageSource.Dispose();
                    //Lấy thông tin cần thiết
                    var picture = new PictureItem
                    {
                        Type = !string.IsNullOrEmpty(Request["type"]) ? Convert.ToInt32(Request["type"]) : 0,
                        CategoryID =
                            !string.IsNullOrEmpty(Request["CategoryID"]) ? Convert.ToInt32(Request["CategoryID"]) : 1,
                        AgencyId = UserItem.AgencyID,
                        Folder = folderinsert,
                        Name = item.Name,
                        DateCreated = date.TotalSeconds(),
                        IsShow = true,
                        Url = fileName,
                        IsDeleted = false
                    };
                    var json = new JavaScriptSerializer().Serialize(picture);
                    msg = _galleryPictureApi.Add(json);
                }
                try
                {
                    var di = new DirectoryInfo(urlFolder);
                    foreach (var file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (var dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Thêm mới hình ảnh thất bại.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sau khi upload xong, cập nhật thông tin cho picture
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxFormPictureUpdate()
        {
            var arrFile = Request["fileData"];
            var oSerializer = new JavaScriptSerializer();
            var ltsFileobj = oSerializer.Deserialize<List<FileObj>>(arrFile);
            var type = Request["Type"];
            var categoryId = Request["CategoryID"];
            var model = new ModelFileObj
            {
                ListCategoryItem = _categoryApi.GetChildByParentId(false),
                ListItem = ltsFileobj,
                Type = !string.IsNullOrEmpty(type) ? Convert.ToInt32(type) : 0,
                CategoryId = !string.IsNullOrEmpty(categoryId) ? Convert.ToInt32(categoryId) : 0,
                Action = DoAction.ToString()
            };
            return View(model);
        }

        /// <summary>
        /// Load ra khung upload mutils
        /// </summary>
        public ActionResult AjaxFormPicture()
        {
            var model = _categoryApi.GetChildByParentId(false);
            ViewBag.Action = DoAction;
            return View(model);
        }
        #endregion

        public ActionResult Index()
        {
            var ltsAllCategory = _categoryApi.GetChildByParentId(true);
            return View(ltsAllCategory);
        }

        public ActionResult ListItems()
        {
            return View(_galleryPictureApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult ListNewItems()
        {
            return View(_galleryPictureApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var pictureModel = _galleryPictureApi.GetPictureItem(ArrId.FirstOrDefault());
            ViewData.Model = pictureModel;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var pictureModel = new PictureItem();
            if (DoAction == ActionType.Edit)
                pictureModel = _galleryPictureApi.GetPictureItem(ArrId.FirstOrDefault());
            ViewBag.PictureCategoryID = _categoryApi.GetChildByParentId(false);
            ViewData.Model = pictureModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        public ActionResult UploadFiles()
        {
            var lst = new string[] { "jpg", "png" };
            var item = new FileObj();
            var urlFolder = ConfigData.TempFolder;
            if (!Directory.Exists(urlFolder)) Directory.CreateDirectory(urlFolder);
            foreach (string file in Request.Files)
            {
                var hpf = Request.Files[file];
                var fileNameRoot = hpf != null ? hpf.FileName : string.Empty;
                if (hpf != null && hpf.ContentLength == 0)
                    continue;
                if (hpf != null && fileNameRoot.Length > 1)
                {
                    var fileLocal = fileNameRoot.Split('.');
                    if (!lst.Contains(fileLocal[1].ToLower()))
                    {
                        item.Error = true;
                        continue;
                    }
                    var fileName = FomatString.Slug(fileLocal[0]) + "-" + DateTime.Now.ToString("MMddHHmmss") + "." + fileLocal[1];
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
            return Json(item);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            var lstArrId = "";
            switch (DoAction)
            {
                case ActionType.Edit:
                    msg = _galleryPictureApi.Update(url);
                    break;
                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    msg = _galleryPictureApi.Delete(lst1);
                    break;
                case ActionType.Show:
                    lstArrId = string.Join(",", ArrId);
                    msg = _galleryPictureApi.ShowHide(lstArrId,true);
                    break;
                case ActionType.Hide:
                    lstArrId = string.Join(",", ArrId);
                    msg = _galleryPictureApi.ShowHide(lstArrId, false);
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
