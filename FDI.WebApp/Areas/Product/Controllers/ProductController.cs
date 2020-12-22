using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using FDI.Base;
using FDI.GetAPI;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using FDI.CORE;

namespace FDI.Web.Areas.Product.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /ModuleProducts/
        readonly ProductBL _bl = new ProductBL();
        private const int TotalpageH = 30;
        readonly CategoryBL _categoryBl = new CategoryBL();
        readonly ProductDetailBL _detailBl = new ProductDetailBL();
        readonly ProductDA _productDa = new ProductDA("#");
        readonly OrdersDA _ordersDa = new OrdersDA("#");
        readonly System_ColorDA _systemColorDa = new System_ColorDA("#");
        readonly ProductSizeDA _productSizeDa = new ProductSizeDA("#");
        readonly NewsBL _newsBl = new NewsBL();
        /// <summary>
        /// Trang chủ
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="ctrId"></param>
        /// <param name="url"></param>
        /// <param name="cateId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PartialViewResult Index(string slug, int ctrId, string url, int cateId = 0, int pageId = 0, int page = 1)
        {
            var stt = (page - 1) * TotalpageH;
            const int rowPage = 15;
            var total = 0;
            var keyword = "";
            var text = "";
            var search = slug.Contains("tim-kiem-san-pham");
            if (search)
            {
                text = Request["keyword"];
                keyword = FomatString.Slug(text);
            }
            var pages = string.Format("/{0}-p{1}c{2}p", slug, pageId, cateId);
            var color = Request["color"];
            var size = Request["size"];
            var sort = Request["sort"];
            var model = new ModelShopProductDetailItem
            {
                CtrId = ctrId,
                CtrUrl = url,
                ListItem = !search ? _detailBl.GetList(slug, page, rowPage, ref total, color, size, sort) : _detailBl.GetListByKeyword(keyword, page, rowPage, ref total, color, size, sort),
                PageHtml = search ? Paging.GetPage(pages, 2, page, rowPage, total, "keyword", text) : Paging.GetPage(pages, 2, page, rowPage, total),
                
            };
            return PartialView(model);
        }

        public PartialViewResult Breadcrumb(string slug, int cateId = 0, int pageId = 0)
        {
            var search = slug.Contains("tim-kiem-san-pham");
            var obj = new BreadcrumbItem();
            obj.Breadcrumb = _categoryBl.GetByid(cateId).Name;
            if (search)
            {
                obj.Breadcrumb = "Tìm kiếm sản phẩm";
            }
            if (pageId == 9)
            {
                obj.Breadcrumb = _productDa.GetProductItem(cateId).Name;
            }
            if (pageId == 6)
            {
                obj.Breadcrumb = _newsBl.GetNewsTitleAssci(slug).Title;
            }
            return PartialView(obj);
        }
        //#region danh gia
        //public ActionResult aggregateRating(int id, string type)
        //{
        //    var model = new List<RatingSiteItem>();
        //    if (type == "product")
        //    {
        //        model = _siteBl.GetListbyProductId(id);
        //    }
        //    if (type == "news")
        //    {
        //        model = _siteBl.GetListbyNewsId(id);
        //    }
        //    if (type == "home")
        //    {
        //        model = _siteBl.GetListHome();
        //    }

        //    return View(model);
        //}

        //public ActionResult AddRating(int id, string type, int star)
        //{
        //    var msg = new JsonMessage();
        //    var rate = new RatingSite();
        //    try
        //    {
        //        if (type == "product")
        //        {
        //            rate.Count = 1;
        //            rate.Star = star;
        //            rate.ProductId = id;
        //            _ratingSiteDl.Add(rate);
        //            _ratingSiteDl.Save();
        //        }
        //        if (type == "news")
        //        {
        //            rate.Count = 1;
        //            rate.Star = star;
        //            rate.NewsID = id;
        //            _ratingSiteDl.Add(rate);
        //            _ratingSiteDl.Save();
        //        }
        //        if (type == "home")
        //        {
        //            rate.Count = 1;
        //            rate.Star = star;
        //            _ratingSiteDl.Add(rate);
        //            _ratingSiteDl.Save();
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return Json(msg, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(msg, JsonRequestBehavior.AllowGet);
        //}
        //#endregion
        public JsonResult ProductItem(int id)
        {
            var model = _bl.GetProductId(id);
            return Json(model);
        }

        public PartialViewResult Detail(int ctrId, string url, int cateId = 0)
        {
            var model = new ModelShopProductDetailItem
            {
                Items = _bl.GetProductId(cateId),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }
        public PartialViewResult CommentFacebook(int ctrId, string url, int cateId = 0)
        {
            var model = new ModelShopProductDetailItem
            {
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }
        //public PartialViewResult ProductInCategory(int ctrId, string url, int cateId = 0)
        //{
        //    var temp = _bl.GetProductId(cateId);
        //    var lstString = string.Join(",", temp.ListCategoryItem.Select(c => c.ID));
        //    var lst = FDIUtils.StringToListInt(lstString);
        //    var model = _bl.GetListCateId(lst, cateId);
        //    return PartialView(model);
        //}
        public PartialViewResult ProductHot(int ctrId, string url)
        {
            var model = new ModelCategoryItem { CtrId = ctrId, CtrUrl = url };
            model.ListItem = _categoryBl.GetlistCateShowhome((int)ModuleType.Product);
            return PartialView(model);
        }

        public ActionResult ListProductHome(string listId)
        {
            var model = _detailBl.GetListProductbylstId(listId);
            return View(model);
        }
        public ActionResult ProductHome()
        {
            var model = _bl.GetListHome();
            return PartialView(model);
        }
        public ActionResult ProductOther(int cateId, int ortherId)
        {
            var model = _bl.GetListOther(cateId, ortherId);
            return PartialView(model);
        }
        public PartialViewResult ListCategoryHome(int ctrId, string url)
        {
            var model = new ModelCategoryItem { CtrId = ctrId, CtrUrl = url };
            model.ListItem = _categoryBl.GetlistCateShowhome((int)ModuleType.Product);
            return PartialView(model);
        }
        public PartialViewResult ProductInCategoryHome(int ctrId, string url)
        {
            var model = new ModelCategoryItem { CtrId = ctrId, CtrUrl = url };
            model.ListItem = _categoryBl.GetlistCateShowhome((int)ModuleType.Product);
            return PartialView(model);
        }
        public PartialViewResult ListProductHotInHome(int ctrId, string url)
        {
            var model = new ModelShopProductDetailItem { CtrId = ctrId, CtrUrl = url };
            model.ListItem = _detailBl.GetListHot();
            return PartialView(model);
        }
        public ActionResult ProductInCategoryHotHome()
        {
            var model = _categoryBl.GetCateHot((int)ModuleType.Product);
            return PartialView(model);
        }
        public ActionResult ProductRelate(int lstCateId)
        {
            var model = _detailBl.GetListProductbylstCateId(lstCateId);
            return PartialView(model);
        }
        public JsonResult ChangeProductSize(int productId, int sizeId = 0, int colorId = 0)
        {
            var model = _productDa.ChangeProductSize(productId, sizeId, colorId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Addtocart(int productId, int quantity, int shopId, int sizeId = 0)
        {
            var msg = new JsonMessage { Erros = false, Message = "Bạn đã thêm sản phẩm vào giỏ hàng" };
            if (quantity <= 0)
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Số lượng phải >= 1.",
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            if (shopId == 0)
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Không thể đặt sản phẩm này.",
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            var obj = new List<ShopProductDetailCartItem>();
            var codeCookie = HttpContext.Request.Cookies["addtocart"];
            if (codeCookie == null)
            {
                var item = new ShopProductDetailCartItem
                {
                    ProductId = productId,
                    SizeID = sizeId,
                    Quantity = quantity,
                    ShopID = shopId,
                };
                obj.Add(item);
                var val = new JavaScriptSerializer().Serialize(obj);
                codeCookie = new HttpCookie("addtocart") { Value = val, Expires = DateTime.Now.AddHours(6) };
                Response.Cookies.Add(codeCookie);
            }
            else
            {
                
                var temp = codeCookie.Value;
                var lst = new JavaScriptSerializer().Deserialize<List<ShopProductDetailCartItem>>(temp);
                var check = lst.Any(c => c.ProductId == productId  && c.SizeID == sizeId);
                if (check)
                {
                    foreach (var item in lst.Where(item => item.ProductId == productId  && item.SizeID == sizeId))
                    {
                        item.Quantity = (item.Quantity ?? 0) + quantity;
                        item.ShopID = shopId;
                        break;
                    }
                    var val = new JavaScriptSerializer().Serialize(lst);
                    codeCookie = new HttpCookie("addtocart") { Value = val, Expires = DateTime.Now.AddHours(6) };
                    Response.Cookies.Add(codeCookie);
                }
                else
                {
                    var item = new ShopProductDetailCartItem
                    {
                        ProductId = productId,
                        SizeID = sizeId,
                        Quantity = quantity,
                        ShopID = shopId,
                    };
                    lst.Add(item);
                    var val = new JavaScriptSerializer().Serialize(lst);
                    codeCookie = new HttpCookie("addtocart") { Value = val, Expires = DateTime.Now.AddHours(6) };
                    Response.Cookies.Add(codeCookie);
                }
                //codeCookie.Value = val;
                codeCookie.Expires = DateTime.Now.AddHours(6);
                Response.Cookies.Add(codeCookie);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ListCart()
        {
            var codeCookie = HttpContext.Request.Cookies["addtocart"];
            var list = new List<ShopProductCartItem>();
            if (codeCookie != null)
            {
                var temp = codeCookie.Value;
                var lst = new JavaScriptSerializer().Deserialize<List<ShopProductDetailCartItem>>(temp);
                list = (from item in lst
                        let model = _productDa.GetProductCart(item.ProductId ?? 0, item.SizeID ?? 0)
                        where model != null
                        select new ShopProductCartItem
                        {
                            Name = model.Name,
                            ColorName = model.ColorName,
                            ColorCode = model.ColorCode,
                            UrlPicture = model.UrlPicture,
                            ID = model.ID,
                            Size = model.Size,
                            Price = model.Price,
                            Total = model.Price * item.Quantity,
                            Quantity = item.Quantity,
                            ProductId = item.ProductId,
                            SizeId = item.SizeID,
                            ShopID = item.ShopID
                        }).ToList();
            }
            return PartialView(list);
        }

        public ActionResult UpdateCart(int productId, int type, int quantity = 0)
        {
            var msg = new JsonMessage();
            var codeCookie = HttpContext.Request.Cookies["addtocart"];
            if (codeCookie != null)
            {
                var temp = codeCookie.Value;
                var lst = new JavaScriptSerializer().Deserialize<List<ShopProductDetailCartItem>>(temp);
                var check = lst.Any(c => c.ShopID == productId);
                if (check)
                {
                    switch (type)
                    {
                        case 1:
                            foreach (var item in lst.Where(item => item.ShopID == productId))
                            {
                                item.Quantity = quantity;
                                break;
                            }
                            break;
                        case 2:
                            var itemremove = lst.SingleOrDefault(item => item.ShopID == productId);
                            if (itemremove != null) lst.Remove(itemremove);
                            break;
                    }
                    var val = new JavaScriptSerializer().Serialize(lst);
                    codeCookie = new HttpCookie("addtocart") { Value = val, Expires = DateTime.Now.AddHours(6) };
                    Response.Cookies.Add(codeCookie);
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CheckOutOrder()
        {
            var codeCookie = HttpContext.Request.Cookies["addtocart"];
            var list = new List<ShopProductCartItem>();
            if (codeCookie != null)
            {
                var temp = codeCookie.Value;
                var lst = new JavaScriptSerializer().Deserialize<List<ShopProductDetailCartItem>>(temp);
                if (lst.Any())
                {
                    list = (from item in lst
                            let model = _productDa.GetProductCart(item.ProductId ?? 0, item.SizeID ?? 0)
                            where model != null
                            select new ShopProductCartItem
                            {
                                Name = model.Name,
                                ColorName = model.ColorName,
                                ColorCode = model.ColorCode,
                                UrlPicture = model.UrlPicture,
                                ID = model.ID,
                                Price = model.Price,
                                Total = model.Price * item.Quantity,
                                Quantity = item.Quantity,
                                ProductId = item.ProductId,
                                SizeId = item.SizeID,
                                ShopID = item.ShopID
                            }).ToList();
                }
            }
            return PartialView(list);
        }

        public ActionResult SendOrder()
        {
            var msg = new JsonMessage
            {
                Erros = false,
                Message = "Cảm ơn bạn đã đặt hàng, Chúng tôi sẽ liện hệ với bạn sớm nhất.!"
            };
            try
            {
                var codeCookie = HttpContext.Request.Cookies["addtocart"];
                if (codeCookie != null)
                {
                    var temp = codeCookie.Value;
                    var lst = new JavaScriptSerializer().Deserialize<List<ShopProductDetailCartItem>>(temp);
                    var order = new Shop_Orders();
                    if (lst.Any())
                    {
                        var listDetail = (from item in lst
                                          let product = _productDa.GetProductItem(item.ShopID ?? 0)
                                          select new Shop_Order_Details
                                          {
                                              ProductID = item.ShopID,
                                              Quantity = item.Quantity,
                                              DateCreated = DateTime.Now.TotalSeconds(),
                                              Status = 2,
                                              Price = product.PriceNew,
                                          }).ToList();
                        if (listDetail.Any())
                        {
                            var total = listDetail.Sum(c => c.Price);
                            order.AgencyId = 2010;
                            order.CustomerName = Request["fullname"];
                            order.Mobile = Request["phone"];
                            order.Address = Request["address"];
                            order.Note = Request["ordernote"];
                            order.DateCreated = DateTime.Now.TotalSeconds();
                            order.Status = 2;
                            order.Shop_Order_Details = listDetail;
                            order.TotalPrice = total;
                            order.Payments = total;
                            order.IsDelete = false;
                            _ordersDa.Add(order);
                            _ordersDa.Save();
                            Response.Cookies["addtocart"].Expires = DateTime.Now.AddDays(-1);
                        }
                        else
                        {
                            msg = new JsonMessage
                            {
                                Erros = true,
                                Message = "Giỏ hàng của bạn chưa có sản phẩm nào.!"
                            };
                        }
                    }
                    else
                    {
                        msg = new JsonMessage
                        {
                            Erros = true,
                            Message = "Giỏ hàng của bạn chưa có sản phẩm nào.!"
                        };
                    }
                }
                else
                {
                    msg = new JsonMessage
                    {
                        Erros = true,
                        Message = "Giỏ hàng của bạn chưa có sản phẩm nào.!"
                    };
                }
            }
            catch (Exception)
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Có lỗi xảy ra.!"
                };
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListcolor()
        {
            var model = _systemColorDa.GetAll(2010);
            return PartialView(model);
        }
        public ActionResult GetListProductSize()
        {
            var model = _productSizeDa.GetAll(2010);
            return PartialView(model);
        }
    }

}
