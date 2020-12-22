using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Admin.Models;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class GalleryPictureController : BaseController
    {
        //
        // GET: /Admin/GalleryPicture/

        readonly Gallery_PictureDA _pictureDa = new Gallery_PictureDA("#");
        private readonly CategoryDA _categoryDa;

        public GalleryPictureController()
        {
            _categoryDa = new CategoryDA("#");
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
            var msg = new JsonMessage { Erros = false };
            var intTotalFile = Convert.ToInt32(Request["NumberOfImage"]);
            var folder = date.Year + "\\" + date.Month + "\\" + date.Day + "\\";
            var fileinsert = date.Year + "/" + date.Month + "/" + date.Day + "/";
            var folderinsert = fileinsert;

            for (var idx = 0; idx < intTotalFile; idx++)
            {
                var fileNameLocal = Request["ImageFile_" + idx + ""];
                var file = fileNameLocal.Split('.');
                var nameslug = FomatString.Slug(file[0]);
                var fileName = nameslug + "-" + date.ToString("HHmmss") + "." + file[1];
                if (!nameslug.Contains(ConfigData.WebTitle))
                {
                    fileName = ConfigData.WebTitle + nameslug + "-" + date.ToString("HHmmss") + "." + file[1];
                }
                
                var fileTemp = nameslug + "." + file[1];
                var imageSource = Image.FromFile(ConfigData.TempFolder + fileTemp);
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
                        System.IO.File.Copy(ConfigData.TempFolder + fileTemp, ConfigData.OriginalFolder + folder + fileName);
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
                        System.IO.File.Copy(ConfigData.TempFolder + fileTemp, ConfigData.ImageFolder + folder + fileName);
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
                        System.IO.File.Copy(ConfigData.TempFolder + fileTemp, ConfigData.ImageUploadMediumFolder + folder + fileName);
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
                var picture = new Gallery_Picture
                {
                    Type = !string.IsNullOrEmpty(Request["type"]) ? Convert.ToInt32(Request["type"]) : 0,
                    //CategoryID =
                    //    !string.IsNullOrEmpty(Request["CategoryID"]) ? Convert.ToInt32(Request["CategoryID"]) : 1,
                    LanguageId = Fdisystem.LanguageId,
                    DateCreated = date.TotalSeconds(),
                    Folder = folderinsert,
                    Name = Request["ImageName_" + idx],
                    IsShow = true,
                    Url = fileName,
                    IsDeleted = false,
                    CreateBy = User.Identity.Name,
                    UpdateBy = User.Identity.Name
                };
                _pictureDa.Add(picture);
                msg.Message += string.Format("Đã thêm hình ảnh: <b>{0}</b><br/>", picture.Name);
            }
            _pictureDa.Save();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sau khi upload xong, cập nhật thông tin cho picture
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxFormPictureUpdate()
        {
            var arrFile = Request["fileData"];
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var ltsFileobj = oSerializer.Deserialize<List<FileObj>>(arrFile);
            var type = Request["Type"];
            var categoryId = Request["CategoryID"];
            var model = new ModelFileObj
            {
                ListCategoryItem = _categoryDa.GetChildByParentId(false),
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
            var type = Request["Type"];
            var categoryId = Request["CategoryID"];
            var model = new ModelPictureItem
            {
                Type = !string.IsNullOrEmpty(type) ? Convert.ToInt32(type) : 0,
                CategoryID = !string.IsNullOrEmpty(categoryId) ? Convert.ToInt32(categoryId) : 0,
                Action = DoAction.ToString(),
            };
            return View(model);
        }
        #endregion

        public ActionResult Index()
        {
            var ltsAllCategory = _categoryDa.GetChildByParentId(true);
            return View(ltsAllCategory);
        }

        public ActionResult ListItems()
        {
            var listPictureItem = _pictureDa.GetListSimpleByRequest(Request,1,0);
            var model = new ModelPictureItem
            {
                ListItem = listPictureItem,
                PageHtml = _pictureDa.GridHtmlPage
            };
            return PartialView(model);
        }

        public ActionResult AjaxView()
        {
            var pictureModel = _pictureDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = pictureModel;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var pictureModel = new Gallery_Picture
            {
                IsShow = true,
            };

            if (DoAction == ActionType.Edit)
                pictureModel = _pictureDa.GetById(ArrId.FirstOrDefault());
            ViewBag.PictureCategoryID = _categoryDa.GetChildByParentId(false);
            ViewData.Model = pictureModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var picture = new Gallery_Picture();
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(picture);
                        picture.LanguageId = Fdisystem.LanguageId;
                        picture.DateCreated = DateTime.Now.TotalSeconds();
                        picture.CreateBy = User.Identity.Name;
                        picture.UpdateBy = User.Identity.Name;
                        picture.IsDeleted = false;
                        picture.Type = !string.IsNullOrEmpty(Request["Type"]) ? Convert.ToInt32(Request["Type"]) : 0;
                        picture.CategoryID = !string.IsNullOrEmpty(Request["CategoryID"]) ? Convert.ToInt32(Request["CategoryID"]) : 0;
                        _pictureDa.Add(picture);
                        _pictureDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = picture.ID.ToString(),
                            Message = string.Format("Đã thêm mới hình ảnh: <b>{0}</b>", Server.HtmlEncode(picture.Name))
                        };
                    }
                    catch (Exception ex)
                    {

                        LogHelper.Instance.LogError(GetType(), ex);
                    }

                    break;

                case ActionType.Edit:
                    try
                    {
                        picture = _pictureDa.GetById(ArrId.FirstOrDefault());
                        picture.UpdateBy = User.Identity.Name;
                        UpdateModel(picture);
                        picture.Type = !string.IsNullOrEmpty(Request["Type"]) ? Convert.ToInt32(Request["Type"]) : 0;
                        picture.CategoryID = !string.IsNullOrEmpty(Request["CategoryID"]) ? Convert.ToInt32(Request["CategoryID"]) : 0;
                        _pictureDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = picture.ID.ToString(),
                            Message = string.Format("Đã cập nhật hình ảnh: <b>{0}</b>", Server.HtmlEncode(picture.Name))
                        };
                    }
                    catch (Exception ex)
                    {

                        LogHelper.Instance.LogError(GetType(), ex);
                    }

                    break;

                case ActionType.Delete:
                    //ltsPictureItems = _pictureDa.GetListByArrId(ArrId);
                    //stbMessage = new StringBuilder();
                    //foreach (var item in ltsPictureItems)
                    //{
                    //    try
                    //    {
                    //        item.IsDeleted = true;
                    //        stbMessage.AppendFormat("Đã xóa hình ảnh <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LogHelper.Instance.LogError(GetType(), ex);
                    //    }

                    //}
                    //msg.ID = string.Join(",", ArrId);
                    //_pictureDa.Save();
                    //msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    //ltsPictureItems = _pictureDa.GetListByArrId(ArrId).Where(o => o.IsShow == false).ToList();
                    //stbMessage = new StringBuilder();
                    //foreach (var item in ltsPictureItems)
                    //{
                    //    try
                    //    {
                    //        item.IsShow = true;
                    //        stbMessage.AppendFormat("Đã hiển thị hình ảnh <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LogHelper.Instance.LogError(GetType(), ex);
                    //    }

                    //}
                    //_pictureDa.Save();
                    //msg.ID = string.Join(",", ltsPictureItems.Select(o => o.ID));
                    //msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    //ltsPictureItems = _pictureDa.GetListByArrId(ArrId).Where(o => o.IsShow == true).ToList();
                    //stbMessage = new StringBuilder();
                    //foreach (var item in ltsPictureItems)
                    //{
                    //    try
                    //    {
                    //        item.IsShow = false;
                    //        stbMessage.AppendFormat("Đã ẩn hình ảnh <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LogHelper.Instance.LogError(GetType(), ex);
                    //    }

                    //}
                    //_pictureDa.Save();
                    //msg.ID = string.Join(",", ltsPictureItems.Select(o => o.ID));
                    //msg.Message = stbMessage.ToString();
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _pictureDa.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }

    }
}
