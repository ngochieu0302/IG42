using FDI.Base;
using FDI.DA;
using FDI.MvcAPI.Common;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.DA.DA;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    //[CustomerAuthorize]
    public class ProductAppController : BaseAppApiController
    {
        readonly Shop_ProductAppIG4DA _productDa = new Shop_ProductAppIG4DA();
        readonly ProductDetailBL _detailBl = new ProductDetailBL();
        readonly CustomerAddressAppIG4DA customerAddressDA = new CustomerAddressAppIG4DA();
        readonly WalletCustomerAppIG4DA _walletCustomerDa = new WalletCustomerAppIG4DA("");
        readonly CustomerAppIG4DA _customerDa = new CustomerAppIG4DA();
        readonly Gallery_PictureDA _da = new Gallery_PictureDA();
        readonly OrderPackageAppIG4DA _orderPackageDa = new OrderPackageAppIG4DA();
        readonly CustomerTypeAppIG4DA _customerTypeDa = new CustomerTypeAppIG4DA();
        string[] lst = new string[] { "jpg", "png", "bmp", "dib", "gif", "jpeg", "jpe", "jfif", "tif", "tiff" };
        int kmNearYou = 15000;
        readonly CategoryAppIG4DA _categoryDa = new CategoryAppIG4DA();
        readonly CashOutWalletAppIG4DA _cashOutWalletDa = new CashOutWalletAppIG4DA("#");
        public async Task<ActionResult> Add()
        {
            var model = new ProductAppIG4Item();
            UpdateModel(model);

            List<Gallery_Picture> images = new List<Gallery_Picture>();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var img = await UploadImage(i);
                if (img.Code != 200)
                {
                    return Json(img);
                }
                var picture = new Gallery_Picture
                {
                    Type = !string.IsNullOrEmpty(Request["type"]) ? Convert.ToInt32(Request["type"]) : 0,
                    CategoryID = model.CateId,
                    Folder = img.Data.Folder,
                    Name = img.Data.Name,
                    DateCreated = DateTime.Now.TotalSeconds(),
                    IsShow = true,
                    Url = img.Data.Url,
                    IsDeleted = false,
                };
                images.Add(picture);
                _da.Add(picture);
            }
            if (images.Count == 0)
            {
                return Json(new JsonMessage(1000, "Ảnh sản phẩn không được để trống."));
            }
            await _da.SaveAsync();

            //lay address
            var address = customerAddressDA.GetById(model.AddressId, CustomerId);
            if (address == null)
            {
                return Json(new JsonMessage(1000, "Địa chỉ không tồn tại."));
            }
            var product = new Shop_Product
            {
                Name = model.Name,
                NameAscii = FDIUtils.Slug(FDIUtils.NewUnicodeToAscii(model.Name)),
                Quantity = model.Quantity,
                Description = model.Description,
                PriceNew = model.PriceNew,
                PictureID = images[0].ID,
                CustomerID = CustomerId,
                freeShipFor = model.freeShipFor,
                HasTransfer = model.HasTransfer,
                Type = model.Type,
                IsShow = true,
                IsDelete = false,
                AddressId = address.ID,
                Latitude = address.Latitude.Value,
                CategoryId = model.CateId.Value,
                Longitude = address.Longitude.Value,
                DateCreated = DateTime.Now.TotalSeconds(),
                CustomerID1 = model.CustomerId1,
                //Gallery_Picture1 = _productDa.GetListPictureByArrId(images.Select(m => m.ID).ToList()),
            };

            //product.Categories = _productDa.GetListCategoryByArrId(new List<int>() { model.CateId.Value });

            _productDa.Add(product);
            await _productDa.SaveAsync();

            return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Update()
        {
            var model = new ProductAppIG4Item();
            UpdateModel(model);
            var product = _productDa.GetById(model.ID);
            if (product.CustomerID != CustomerId)
            {
                return Json(new JsonMessage(4001, "Bạn không có quyển xóa sản phẩm này"));
            }
            var files = Request.Files;
            if (files.Count == 0 && model.LstPictures != null && model.LstPictures.Count() >= product.Shop_Product_Picture.Count())
            {
                return Json(new JsonMessage(1000, "Ảnh sản phẩn không được để trống."));
            }

            var date = DateTime.Now;
            var folder = date.Year + "\\" + date.Month + "\\" + date.Day + "\\";
            var fileinsert = date.Year + "/" + date.Month + "/" + date.Day + "/";

            var urlFolder = ConfigData.OriginalFolder;

            ImageProcess.CreateForder(ConfigData.OriginalFolder);
            ImageProcess.CreateForder(ConfigData.ThumbsFolder);
            ImageProcess.CreateForder(ConfigData.MediumsFolder);

            List<Gallery_Picture> images = new List<Gallery_Picture>();

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var img = await UploadImage(i);
                if (img.Code != 200)
                {
                    return Json(img);
                }

                var picture = new Gallery_Picture
                {
                    Type = !string.IsNullOrEmpty(Request["type"]) ? Convert.ToInt32(Request["type"]) : 0,
                    CategoryID =
                          model.CateId,
                    Folder = img.Data.Folder,
                    Name = img.Data.Name,
                    DateCreated = DateTime.Now.TotalSeconds(),
                    IsShow = true,
                    Url = img.Data.Url,
                    IsDeleted = false,
                };
                images.Add(picture);
                _da.Add(picture);
            }

            await _da.SaveAsync();
            if (model.AddressId != product.AddressId)
            {
                //lay address
                var address = customerAddressDA.GetById(model.AddressId, CustomerId);
                if (address == null)
                {
                    return Json(new JsonMessage(1000, "Địa chỉ không tồn tại."));
                }
                product.AddressId = model.AddressId;
                product.Latitude = address.Latitude.Value;
                product.Longitude = address.Longitude.Value;
            }
            product.Name = model.Name;
            product.Quantity = model.Quantity;
            product.Description = model.Description;
            product.PriceNew = model.PriceNew;
            product.CustomerID = CustomerId;
            product.HasTransfer = model.HasTransfer;
            product.Type = model.Type;
            product.CategoryId = model.CateId.Value;
            product.CustomerID1 = model.CustomerId1;

            var idImages = product.Shop_Product_Picture.Where(m => model.PictureDeleteIds == null || !model.PictureDeleteIds.Any(n => n == m.Gallery_Picture.ID)).Select(m => m.Gallery_Picture.ID).ToList();
            idImages.AddRange(images.Select(m => m.ID).ToList());
            //product.Shop_Product_Picture = _productDa.GetListPictureByArrId(idImages);

            //if (!product.Categories.Any(m => m.Id == model.CateId))
            //{
            //    product.Categories.Clear();
            //    product.Categories = _productDa.GetListCategoryByArrId(new List<int>() { model.CateId.Value });
            //}

            await _productDa.SaveAsync();

            return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var product = _productDa.GetById(id);
            if (product == null) return Json(new JsonMessage(1004, "Sản phẩm không tồn tại"));
            if (product.CustomerID != CustomerId) return Json(new JsonMessage(4001, "Bạn không có quyển xóa sản phẩm này"));
            product.IsDelete = true;
            await _productDa.SaveAsync();
            return Json(new JsonMessage(200, ""));
        }
        //[AllowAnonymous]
        public ActionResult Product_Sales()
        {
            var product = _productDa.GetListProductSales();
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Erros = false, Data = product }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Product_Incoming()
        {
            var product = _productDa.GetListProductIncoming();
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Erros = false, Data = product }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBestProductForYou(int km, double latitude, double longitude, int page, int pagesize)
        {
            pagesize = pagesize > 15 ? 15 : pagesize;
            var lst = _productDa.GetProductNearPosition(km, latitude, longitude, page, pagesize);

            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Erros = false, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult ProductGetOrderShop(int cateid, int shopid, bool IsAll, int type, DateTime date, int page, int pagesize)
        //{
        //    pagesize = pagesize > 15 ? 15 : pagesize;
        //    var model = new List<ProductAppIG4Item>();
        //    if (type == 1)
        //    {
        //        var firstDay = new DateTime(date.Year, 1, 1).TotalSeconds();
        //        var lastDay = new DateTime(date.Year, 12, 31).TotalSeconds();
        //        model = _productDa.ProductGetOrderShop(cateid, shopid, IsAll, firstDay, lastDay, page, pagesize);
        //    }
        //    if (type == 2)
        //    {
        //        var startDate = new DateTime(date.Year, date.Month, 1);
        //        var endDate = startDate.AddMonths(1).AddDays(-1);
        //        model = _productDa.ProductGetOrderShop(cateid, shopid, IsAll, startDate.TotalSeconds(), endDate.TotalSeconds(), page, pagesize);
        //    }
        //    if (type == 3)
        //    {
        //        var startweek = date.ThisWeekStart();
        //        var endweek = date.ThisWeekEnd();
        //        model = _productDa.ProductGetOrderShop(cateid, shopid, IsAll, startweek.TotalSeconds(), endweek.TotalSeconds(), page, pagesize);

        //    }
        //    return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Erros = false, Data = model }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ProductGetbyShop(int cateid, int shopid, bool IsAll, DateTime dates, DateTime datee, int page, int pagesize)
        {
            pagesize = pagesize > 15 ? 15 : pagesize;
            var lst = _productDa.ProductGetbyShop(cateid, shopid, IsAll, dates, datee, page, pagesize);
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Erros = false, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GroupProductGetOrderShop(int shopid, bool IsAll, int type, DateTime date, int cateId = 0)
        {
            var model = new List<DashboardItem>();
            if (type == 1)
            {
                var firstDay = new DateTime(date.Year, 1, 1).TotalSeconds();
                var lastDay = new DateTime(date.Year, 12, 31).AddDays(1).TotalSeconds();
                model = _productDa.GroupProductGetOrderShop(shopid, IsAll, firstDay, lastDay, cateId);
            }
            if (type == 2)
            {
                var startDate = new DateTime(date.Year, date.Month, 1);
                var endDate = startDate.AddMonths(1);
                model = _productDa.GroupProductGetOrderShop(shopid, IsAll, startDate.TotalSeconds(), endDate.TotalSeconds(), cateId);
            }
            if (type == 3)
            {
                var startweek = date.ThisWeekStart();
                var endweek = date.ThisWeekEnd();
                model = _productDa.GroupProductGetOrderShop(shopid, IsAll, startweek.TotalSeconds(), endweek.TotalSeconds(), cateId);
            }

            return Json(new BaseResponse<List<DashboardItem>> { Code = 200, Erros = false, Data = model }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetProductByCategoryId(int id, int maxKm, int minPrice, int maxPrice, int page, int pagesize)
        {
            pagesize = pagesize > 15 ? 15 : pagesize;
            maxKm *= 1000;

            // get parents id
            var list = new List<int>();
            var model = _categoryDa.GetAllListSimpleByParentId(id);
            if (model.Any())
            {
                list.Add(id);
                var ids = model.Select(m => m.ID).ToList();
                list.AddRange(ids);
                foreach (var item in model)
                {
                    list.AddRange(item.lstInt);
                }
            }

            var lst = _productDa.GetItemByCategoryId(list, maxKm, minPrice, maxPrice, Latitude, Longitude, page, pagesize);

            foreach (var product in lst)
            {
                product.Km = ConvertUtil.distance(Latitude, Longitude, product.Latitude, product.Longitude, 'K');
            }
            return Json(new BaseResponse<List<ProductAppIG4Item>>() { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetById(int id)
        {
            var model = _detailBl.GetById(id);
            return Json(new BaseResponse<ProductDetailsItem> { Code = 200, Erros = false, Data = model }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult ListProducComingsoonAll()
        {
            var date = DateTime.Now.TotalSeconds();
            var model = _detailBl.ListProducComingsoonAll(date);
            return Json(new BaseResponse<List<ProducComingsoonItem>> { Code = 200, Erros = false, Data = model }, JsonRequestBehavior.AllowGet);
        }
        
        [AllowAnonymous]
        public ActionResult ListAll()
        {
            var model = _detailBl.ListAll();
            return Json(new BaseResponse<List<ProductDetailsItem>> { Code = 200, Erros = false, Data = model }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductSampleShop(int id, double latitude, double longitude, int page, int pagesize)
        {
            pagesize = pagesize > 15 ? 15 : pagesize;
            var lst = _productDa.GetProductSampleShop(id, page, pagesize);
            foreach (var product in lst)
            {
                product.Km = ConvertUtil.distance(latitude, longitude, product.Latitude, product.Longitude, 'K');
            }
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetRatings(int id)
        {
            var data = _productDa.GetRatings(id);
            return Json(new BaseResponse<List<RatingAppIG4Item>> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRatingsCustomer(int id)
        {
            var data = _productDa.GetRatingsCustomer(id);
            return Json(new BaseResponse<List<RatingAppIG4Item>> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCommentRatings(int id)
        {
            var data = _productDa.GetCommentRatings(id);
            return Json(new BaseResponse<List<RatingAppIG4Item>> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddComment(RatingAppIG4Item data)
        {
            var comment = _productDa.GetComment(data.ProductId.Value, CustomerId);
            if (comment != null)
            {
                comment.TypeRating = data.TypeRating;
                comment.Title = data.Title;
                comment.Comment = data.Comment;
                comment.DateCreated = DateTime.Now;
                _productDa.Save();
                CaculateRating(data);
                return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
            }
            List<Gallery_Picture> imgs = new List<Gallery_Picture>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var img = await UploadImage(i);
                if (!img.Erros)
                {
                    var picture = new Gallery_Picture
                    {
                        Type = !string.IsNullOrEmpty(Request["type"]) ? Convert.ToInt32(Request["type"]) : 0,

                        Folder = img.Data.Folder,
                        Name = img.Data.Name,
                        DateCreated = DateTime.Now.TotalSeconds(),
                        IsShow = true,
                        Url = img.Data.Url,
                        IsDeleted = false,
                    };
                    imgs.Add(picture);
                }

            }
            var item = new ProductRating
            {
                ProductId = data.ProductId,
                CustomerId = CustomerId,
                TypeRating = data.TypeRating,
                DateCreated = DateTime.Now,
                Gallery_Picture = imgs,
                Title = data.Title,
                Comment = data.Comment
            };
            _productDa.AddComment(item);
            _productDa.Save();
            CaculateRating(data);
            return Json(new JsonMessage(200, ""), JsonRequestBehavior.AllowGet);
        }

        private void CaculateRating(RatingAppIG4Item data)
        {
            var rating = _productDa.GetTotalRating(data.ProductId.Value);
            var product = _productDa.GetById(data.ProductId.Value);
            if (product != null)
            {
                product.Ratings = rating.Sum(m => m.Quantity);
                var totalrating = rating.Sum(m => m.Quantity * m.TypeRating);
                if (product.Ratings != 0) product.AvgRating = totalrating / product.Ratings;
                else product.AvgRating = 0;
                _productDa.Save();
            }
        }

        private async Task<BaseResponse<GalleryPictureItem>> UploadImage(int i)
        {
            var file = Request.Files[i];
            using (HttpClient client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    byte[] fileBytes = new byte[file.InputStream.Length + 1]; file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                    content.Add(fileContent);
                    var result = await client.PostAsync("http://imggstore.fditech.vn/home/upload", content);
                    if (result.IsSuccessStatusCode)
                    {
                        var datas = await result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<BaseResponse<GalleryPictureItem>>(datas);
                    }
                    return new BaseResponse<GalleryPictureItem>() { Code = 1000 };
                }
            }
        }

        public async Task<ActionResult> GetSummaryTotalByCategoryId(int id, string name, double minKm, double maxKm, int minPrice, int maxPrice)
        {
            minKm *= 1000;
            maxKm *= 1000;
            //get total san pham gan toi
            var nearTotal = await _productDa.TotalNearProductByCategoryId(id, name, minKm, maxKm, minPrice, maxPrice, Latitude, Longitude);
            var hasTransferTotal = await _productDa.TotalNearProductByCategoryId(true, id, name, minKm, maxKm, minPrice, maxPrice, Latitude, Longitude);
            var hotTotal = await _productDa.ProductsHot(id, name, minKm, maxKm, minPrice, maxPrice, Latitude, Longitude);
            var totalReview = await _productDa.ProductsIsReview(id, name, minKm, maxKm, minPrice, maxPrice, Latitude, Longitude);
            return Json(new BaseResponse<object>
            {
                Code = 200,
                Data = new { nearTotal, hasTransferTotal, hotTotal, totalReview }
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNearProductByCategoryId(int id, string name, double minKm, double maxKm, int minPrice, int maxPrice, int page, int pagesize)
        {
            minKm *= 1000;
            maxKm *= 1000;
            pagesize = pagesize > 15 ? 15 : pagesize;
            var lst = _productDa.GetNearItemByCategoryId(id, name, minKm, maxKm, minPrice, maxPrice, Latitude, Longitude, page, pagesize);
            foreach (var product in lst)
            {
                product.Km = ConvertUtil.distance(Latitude, Longitude, product.Latitude, product.Longitude, 'K');
            }
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductHasTranferByCategoryId(int id, string name, double minKm, double maxKm, int minPrice, int maxPrice, int page, int pagesize)
        {
            minKm *= 1000;
            maxKm *= 1000;
            pagesize = pagesize > 15 ? 15 : pagesize;
            var lst = _productDa.GetHasTransferItemByCategoryId(id, name, minKm, maxKm, minPrice, maxPrice, Latitude, Longitude, page, pagesize);
            foreach (var product in lst)
            {
                product.Km = ConvertUtil.distance(Latitude, Longitude, product.Latitude, product.Longitude, 'K');
            }
            return Json(new BaseResponse<List<ProductAppIG4Item>>() { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductIsHotByCategoryId(int cateId, string name, double minKm, double maxKm, int minPrice, int maxPrice, int page, int pagesize)
        {
            minKm *= 1000;
            maxKm *= 1000;
            pagesize = pagesize > 15 ? 15 : pagesize;
            var lst = _productDa.GetIsHotItemByCategoryId(cateId, name, minKm, maxKm, minPrice, maxPrice, Latitude, Longitude, page, pagesize);
            foreach (var product in lst)
            {
                product.Km = ConvertUtil.distance(Latitude, Longitude, product.Latitude, product.Longitude, 'K');
            }
            return Json(new BaseResponse<List<ProductAppIG4Item>>() { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductIsReviewByCategoryId(int id, string name, double minKm, double maxKm, int minPrice, int maxPrice, int page, int pagesize)
        {
            minKm *= 1000;
            maxKm *= 1000;
            pagesize = pagesize > 15 ? 15 : pagesize;
            var lst = _productDa.GetIsReviewItemByCategoryId(id, name, minKm, maxKm, minPrice, maxPrice, Latitude, Longitude, page, pagesize);
            foreach (var product in lst)
            {
                product.Km = ConvertUtil.distance(Latitude, Longitude, product.Latitude, product.Longitude, 'K');
            }
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBestBuy(int page, int pagesize)
        {
            pagesize = pagesize > 15 ? 15 : pagesize;
            var lst = _productDa.GetBestBuy(Latitude, Longitude, kmNearYou, page, pagesize);

            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetCategoryForMap(string name, double minKm, double maxKm, int minPrice, int maxPrice, int categoryId, double latitude, double longitude)
        {
            var lst = _productDa.GetCategoryForMap(name, minKm, maxKm, minPrice, maxPrice, categoryId, latitude, longitude);
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult GetProductForMap(string name = "", double minKm = 0, double maxKm = 0, int minPrice = 0, int maxPrice = 0, int cateId = 0, double latitude = 0, double longitude = 0, int page = 1, int pagesize = 10, bool HasTransfer = false, int shopid = 0)
        {
            var lst = _productDa.GetProductForMap(name, minKm, maxKm, cateId, minPrice, maxPrice, latitude, longitude, page, pagesize, HasTransfer, shopid);
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMyProduct(string name, int page, int maxKm, int minPrice, int maxPrice, int pagesize, int categoryId = 0)
        {
            maxKm *= 1000;
            var lst = _productDa.GetMyProduct(CustomerId, categoryId, name, maxKm, minPrice, maxPrice, Latitude, Longitude, page, pagesize);
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMyProduct1(int page, int pagesize, int categoryId = 0)
        {
            var lst = _productDa.GetMyProduct1(CustomerId, Latitude, Longitude, page, pagesize);
            return Json(new BaseResponse<List<CategoryAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductById(int productId)
        {
            var data = _productDa.GetById(productId);
            return Json(new BaseResponse<Shop_Product> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductByPosition(double latitude, double longitude, int page, int pagesize)
        {
            var lst = _productDa.GetProductByPosition(latitude, longitude, page, pagesize);
            return Json(new BaseResponse<List<ProductAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCustomerForMap(double km, double latitude, double longitude, int page, int pagesize)
        {
            km *= 1000;
            //var cate = categoryIds == null ? new int[] { } : categoryIds.Select(m => m.Value).ToArray();
            var lst = _customerDa.GetCustomerForMap(km, latitude, longitude, page, pagesize);
            return Json(new BaseResponse<List<CustomerAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrderPackage(int customerid)
        {
            var lst = _orderPackageDa.GetOrderPackage(customerid);
            return Json(new BaseResponse<OrderPackageAppIG4Item> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListCustomerType()
        {
            var lst = _customerTypeDa.GetListCustomerTypes();
            return Json(new BaseResponse<List<CustomerTypeAppIG4Item>> { Code = 200, Data = lst }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddOrderPackage(int typeid, int customerid)
        {
            try
            {
                var cus = _customerDa.GetItemByID(customerid);
                var config = _walletCustomerDa.GetConfig();
                var type = _customerTypeDa.GetById(typeid);
                if (cus.Wallets >= type.Price)
                {
                    var dateStart = _orderPackageDa.GetDateStartByCustomerID(customerid);

                    var date = DateTime.Now;
                    var item = new Order_Package
                    {
                        TypeID = type.ID,
                        Price = type.Price,
                        CustomerID = customerid,
                        Datecreate = date.TotalSeconds(),
                        DateStart = dateStart ?? date.TotalSeconds(),
                        DateEnd = date.AddMonths(type.Month ?? 0).AddDays(type.Day ?? 0).TotalSeconds(),
                    };

                    _orderPackageDa.Add(item);
                    var cashout = new CashOutWallet
                    {
                        CustomerID = customerid,
                        DateCreate = DateTime.Now.TotalSeconds(),
                        TotalPrice = type.Price,
                        OrderPaketID = item.ID,
                        Type = 2,
                    };
                    _cashOutWalletDa.Add(cashout);

                    await _orderPackageDa.SaveAsync();

                    //var walletcus = new WalletCustomer
                    //{
                    //    CustomerID = 1,
                    //    TotalPrice = type.Price*(100 - config.DiscountOrderPacket) ?? 0,
                    //    DateCreate = DateTime.Now.TotalSeconds(),
                    //    IsActive = true,
                    //    IsDelete = false,
                    //    Type = 3,
                    //    //Transaction_no = data.Code,
                    //    CustomerIDR = cus.ID,
                    //};
                    //_walletCustomerDa.Add(walletcus);
                    //_walletCustomerDa.Save();
                    _cashOutWalletDa.Save();
                    //var total = type.Price * config.DiscountOrderPacket / 100;
                    var bonusItems = _customerDa.ListBonusTypeItems();

                    InsertRewardOrderPacket(cus, config, type.Price, item.ID, bonusItems);
                    return Json(new BaseResponse<JsonMessage> { Code = 200, Message = "Mua gói dịch vụ thành công." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new BaseResponse<OrderPacketAppAppIG4Item> { Code = -1, Message = "Ví tiền không đủ." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new BaseResponse<OrderPacketAppAppIG4Item> { Code = -2, Message = e.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}