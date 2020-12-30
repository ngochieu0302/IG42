using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class Shop_ProductAppIG4DA : BaseDA
    {
        #region Constructer

        public Shop_ProductAppIG4DA()
        {
        }

        public Shop_ProductAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public Shop_ProductAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion

        public List<ProductAppIG4Item> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false //&& c.LanguageId == LanguageId
                        orderby c.ID
                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            Code = c.Code,
                            NameCustomer = c.CustomerID.HasValue ? c.Customer.FullName : "G_Store",
                            CodeCustomer = c.CustomerID.HasValue ? c.Customer.UserName : "GStore",
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Author = c.Author,
                            PriceNew = c.PriceNew,
                            Format = c.Format,
                            IsShow = c.IsShow,
                            Quantity = 0,
                            Weight = c.Weight,
                            QuantityOut = 0,
                            YearOfManufacture = c.YearOfManufacture,
                            IsHot = c.IsHot,
                            FileReadId = c.FileBookId,
                            FileReadTryId = c.FIleReadtryId,
                            DateCreated = c.DateCreated,
                            CreateBy = c.CreateBy,
                            Sort = c.Sort,
                            LstInt = c.Categories.Select(p => p.Id),
                            CateId = c.ID,
                            BookOld = c.BookOld,
                        };
            var cateId = httpRequest.QueryString["CateID"];
            if (!string.IsNullOrEmpty(cateId))
            {
                var id = int.Parse(cateId);
                query = query.Where(c => c.LstInt.Contains(id));
            }

            //var show = httpRequest.QueryString["Isshow"];
            //if (!string.IsNullOrEmpty(show))
            //{
            //    var id = show == "1" ? true : false;
            //    query = query.Where(c => c.BookOld == id);
            //}
            var name = httpRequest.QueryString["SearchIn"];
            var keyword = httpRequest.QueryString["Keyword"];
            if (!string.IsNullOrEmpty(keyword) && name == "Name")
            {
                //var txt = FDIUtils.Slug(keyword);
                query = query.Where(c => c.Name.Contains(keyword));
                //return query.ToList();
            }
            if (!string.IsNullOrEmpty(keyword) && name == "Code")
            {
                //var txt = FDIUtils.Slug(keyword);
                query = query.Where(c => c.Code.Contains(keyword));
                //return query.ToList();
            }
            if (!string.IsNullOrEmpty(keyword) && name == "NameCustomer")
            {
                //var txt = FDIUtils.Slug(keyword);
                query = query.Where(c => c.NameCustomer.Contains(keyword));
                //return query.ToList();
            }
            if (!string.IsNullOrEmpty(keyword) && name == "CodeCustomer")
            {
                //var txt = FDIUtils.Slug(keyword);
                query = query.Where(c => c.CodeCustomer.Contains(keyword));
                //return query.ToList();
            }

            query = query.SelectByRequest(Request, ref TotalRecord);

            return query.ToList();
        }

        public List<SizeItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.Sizes
                        where c.IsShow == true
                        orderby c.Sort
                        select new SizeItem
                        {
                            ID = c.ID,
                            Name = c.Name
                        };
            return query.ToList();
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int type = 1)
        {
            var name = FDIUtils.Slug(keword);
            var query = FDIDB.AutoSelectProduct(name, showLimit).Select(c => new SuggestionsProduct
            {
                ID = c.ID,
                UrlImg = c.UrlImg ?? "/Content/Admin/images/auto-default.jpg",
                name = "",
                code = c.BarCode ?? "",
                value = c.BarCode ?? "",
                data = "<span class='maskPrice'>" + c.PriceNew + "</span> đ",
                title = c.Name,
                pricenew = c.PriceNew,
                Type = type
            });
            return query.ToList();
        }
        public ProductAppIG4Item GetProductItem(int id, double Latitude = 0, double Longitude = 0)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.ID == id
                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            CategoryName = c.Category.Name,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            PriceNew = c.PriceNew,
                            CateId = c.CategoryId,
                            LstPictures = c.Shop_Product_Picture.Select(m => new PictureAppIG4Item
                            {
                                ID = m.Gallery_Picture.ID,
                                Folder = m.Gallery_Picture.Folder,
                                Url = m.Gallery_Picture.Url
                            }),
                            Description = c.Description,
                            CustomerId = c.CustomerID,
                            CustomerId1 = c.CustomerID1,
                            Quantity = c.Quantity,
                            QuantityOut = c.QuantityOut,
                            Type = c.Type,
                            HasTransfer = c.HasTransfer,
                            Ratings = c.Ratings ?? 0,
                            AvgRating = c.AvgRating ?? 0,
                            IsPrestige = c.CustomerID1.HasValue,
                            CustomerItem = new Customer1Item
                            {
                                Fullname = c.Customer.FullName,
                                Mobile = c.Customer.Mobile,
                                IsPrestige = c.Customer.IsPrestige,
                                TypePoint = c.Customer.IsPrestige ? 10000 : c.Customer.Order_Package.Where(a => a.DateStart >= date && a.DateEnd < date).Select(a => a.Customer_Type.Type).FirstOrDefault(),
                                CustomerAddressItem = c.Customer.CustomerAddresses.Where(m => m.IsDefault).Select(m => new CustomerAddressAppIG4Item
                                {
                                    Address = m.Address,
                                    Longitude = m.Longitude,
                                    Latitude = m.Latitude,
                                    Km = ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude)
                                }).FirstOrDefault()
                            },
                            CustomerItem1 = new Customer2Item
                            {
                                Fullname = c.Customer.FullName,
                                Mobile = c.Customer.Mobile,
                                IsPrestige = true,
                                CustomerAddressItem = c.Customer.CustomerAddresses.Where(m => m.IsDefault).Select(m => new CustomerAddressAppIG4Item
                                {
                                    Address = m.Address,
                                    Longitude = m.Longitude,
                                    Latitude = m.Latitude,
                                    Km = ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude)
                                }).FirstOrDefault()
                            }
                        };
            return query.FirstOrDefault();
        }
        public List<ProductApiItem> GetAllList()
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.LanguageId == LanguageId && c.IsShow == true
                        orderby c.ID descending
                        select new ProductApiItem()
                        {
                            Name = c.Name,
                            Code = c.Code,
                            Author = c.Author,

                        };
            return query.ToList();
        }
        public List<Select2Model> GetAllListSelect2(string key, int page, ref int total)
        {
            var keyword = FDIUtils.Slug(key);
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.NameAscii.Contains(keyword) && c.IsShow == true
                        orderby c.ID descending
                        select new Select2Model()
                        {
                            text = c.Name,
                            id = c.Name,
                        };
            total = query.Count();
            query = query.Skip((page - 1) * 10).Take(10);
            return query.ToList();
        }

        public List<ProductAppIG4Item> GetAuthor()
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false
                        select new ProductAppIG4Item()
                        {
                            Author = c.Author,
                            ID = c.ID
                        };
            return query.ToList();
        }

        public List<Shop_Product> GetListProductEbook()
        {
            var query = from c in FDIDB.Shop_Product
                        where c.IsDelete == false && c.IsShow == true && c.FileBookId != null
                        select c;
            return query.ToList();
        }
        public ProductCartItem GetProductCart(int id)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.ID == id && c.IsShow == true && (c.IsDelete == false)
                        select new ProductCartItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Price = c.PriceNew,
                            QuantityStorage = c.Quantity ?? 0,
                            QuantityOut = c.QuantityOut ?? 0,
                        };
            return query.FirstOrDefault();
        }
        public List<ProductTypeItem> GetListProductType()
        {
            return FDIDB.Shop_Product_Type.Select(m => new ProductTypeItem
            {
                ID = m.ID,
                Name = m.Name
            }).ToList();
        }

        public bool CheckName(string name, int productId)
        {
            var query = (from c in FDIDB.Shop_Product
                         where c.Name.ToLower() == name.ToLower() && c.ID != productId && c.IsDelete == false
                         select c).Count();

            return query > 0;
        }

        public bool CheckNameAscii(string name, int productId)
        {
            var query = (from c in FDIDB.Shop_Product
                         where c.NameAscii == name && c.ID != productId && c.IsDelete == false
                         select c).Count();

            return query > 0;
        }
        public bool CheckCode(string code, int productId)
        {
            var query = (from c in FDIDB.Shop_Product
                         where c.Code == code && c.ID != productId && c.IsDelete == false
                         select c).Count();

            return query > 0;
        }
        public Shop_Product GetById(int productId)
        {
            var query = from c in FDIDB.Shop_Product where c.ID == productId select c;
            return query.FirstOrDefault();
        }
        public List<Shop_Product> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Shop_Product
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false
                        select c;
            return query.ToList();
        }
        public List<Shop_Product> GetListBase()
        {
            var query = from c in FDIDB.Shop_Product select c;
            return query.ToList();
        }
        public bool GetByCode(string code)
        {
            var query = (from c in FDIDB.Shop_Product where c.Code == code select c).Count();
            return query > 0;
        }
        public List<Category> GetListCategoryByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Categories where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }

        public List<Gallery_Picture> GetListPictureByArrId(List<int> arrId)
        {
            var query = from c in FDIDB.Gallery_Picture where arrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<Shop_Product> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Shop_Product where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<System_Tag> GetListIntTagByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.System_Tag where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(Shop_Product shopProduct)
        {
            try
            {
                FDIDB.Shop_Product.Add(shopProduct);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                ex.StackTrace.ToString();
            }
        }

        public void Delete(Shop_Product shopProduct)
        {
            FDIDB.Shop_Product.Remove(shopProduct);
        }


        public void InsertAuthorProduct(Author_Product author_Product)
        {
            FDIDB.Author_Product.Add(author_Product);
        }
        public void InsertAuthor(Author author)
        {
            FDIDB.Authors.Add(author);
        }

        public List<ProductAppIG4Item> ProductGetbyShop(int cateid, int shopid, bool IsAll, DateTime dates, DateTime datee, int page, int pagesize)
        {
            var query = FDIDB.ProductGetbyShop(cateid, shopid, IsAll, dates.TotalSeconds(), datee.TotalSeconds(), page, pagesize).Select(m =>
                new ProductAppIG4Item
                {
                    ID = m.ID,
                    Name = m.Name,
                    PriceNew = m.PriceNew ?? 0,
                    DateCreated = m.DateCreated,
                    Quantity = m.Quantity,
                    UrlPicture = m.Folder + m.Url,
                });
            return query.ToList();
        }
        public List<ProductAppIG4Item> ProductGetOrderShop(int cateid, int shopid, bool IsAll, decimal dates, decimal datee, int page, int pagesize)
        {
            var query = FDIDB.ProductGetOrderShop(cateid, shopid, IsAll, dates, datee, page, pagesize).Select(m =>
                 new ProductAppIG4Item
                 {
                     ID = m.ID,
                     Name = m.Name,
                     PriceNew = m.PriceNew ?? 0,
                     DateCreated = m.DateCreated,
                     Quantity = m.Quantity,
                     UrlPicture = m.Folder + m.Url,
                 });
            return query.ToList();
        }
        public List<DashboardItem> GroupProductGetOrderShop(int shopid, bool IsAll, decimal dates, decimal datee, int cateId)
        {
            var query = FDIDB.GroupProductGetOrderShop(shopid, IsAll, dates, datee, cateId).Select(m =>
                new DashboardItem
                {
                    CategoryId = m.id,
                    Name = m.name,
                    TotalPrice = m.totalprice,
                    totalquantity = m.totalproduct,
                });
            return query.ToList();
        }
        public List<ProductAppIG4Item> GetProductNearPosition(int km, double latitude, double longitude, int page, int pagesize)
        {
            var query = FDIDB.ProductGetNearPosition(km, latitude, longitude, page, pagesize).Select(m =>
            new ProductAppIG4Item
            {
                ID = m.ID,
                Name = m.Name,
                PriceNew = m.PriceNew,
                Ratings = m.ratings ?? 0,
                AvgRating = m.avgrating ?? 0,
                UrlPicture = m.Folder + m.Url,
                HasTransfer = m.HasTransfer,
                Km = m.Km
            });
            return query.ToList();
        }
        public List<ProductAppIG4Item> GetItemByCategoryId(int id, int page, int pagesize)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.CategoryId == id && c.IsShow == true && c.IsDelete == false
                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PriceNew = c.PriceNew,
                            Ratings = c.Ratings ?? 0,
                            AvgRating = c.AvgRating ?? 0,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude
                        };
            query = query.OrderByDescending(m => m.AvgRating).ThenByDescending(m => m.Ratings);
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }
        public List<ProductAppIG4Item> GetItemByCategoryId(List<int> ids, int maxKm, int minPrice, int maxPrice, double Latitude, double Longitude, int page, int pagesize)
        {
            var query = from c in FDIDB.Shop_Product
                        where ids.Contains(c.CategoryId) && c.IsShow == true && (c.IsDelete == false)
                        && (minPrice == 0 || c.PriceNew >= minPrice)
                        && (maxPrice == 0 || c.PriceNew <= maxPrice)
                          && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) <= maxKm)
                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PriceNew = c.PriceNew,
                            Ratings = c.Ratings ?? 0,
                            AvgRating = c.AvgRating ?? 0,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude
                        };
            query = query.OrderByDescending(m => m.AvgRating).ThenByDescending(m => m.Ratings);
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }
        public List<ProductAppIG4Item> GetProductSampleShop(int id, int page, int pagesize)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.CustomerID == id && c.IsShow == true && (c.IsDelete == false)
                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PriceNew = c.PriceNew,
                            Ratings = c.Ratings ?? 0,
                            HasTransfer = c.HasTransfer,
                            AvgRating = c.AvgRating ?? 0,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude
                        };
            query = query.OrderByDescending(m => m.AvgRating).ThenByDescending(m => m.Ratings);
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }
        public List<RatingAppIG4Item> GetCommentRatings(int id)
        {
            var query = FDIDB.ProductRatings.Where(m => m.ProductId == id)
                .Select(m => new RatingAppIG4Item
                {
                    TypeRating = m.TypeRating,
                    Title = m.Title,
                    Comment = m.Comment,
                    CustomerName = m.Customer.FullName,
                    DateCreated = m.DateCreated,
                    Gallery_Picture = m.Gallery_Picture.OrderBy(g => g.ID).Select(g => new PictureItem
                    {
                        Url = g.Folder + g.Url
                    })
                });
            return query.ToList();
        }
        public List<RatingAppIG4Item> GetRatings(int id)
        {
            var query = FDIDB.ProductRatings.Where(m => m.ProductId == id)
                .GroupBy(m => new { m.ProductId, m.TypeRating })
                .Select(m => new RatingAppIG4Item() { TypeRating = m.Key.TypeRating, Quantity = m.Count() });
            return query.ToList();
        }
        public List<RatingAppIG4Item> GetRatingsCustomer(int id)
        {
            var query = FDIDB.ProductRatings.Where(m => m.CustomerId == id)
                .GroupBy(m => new { m.CustomerId, m.TypeRating })
                .Select(m => new RatingAppIG4Item() { TypeRating = m.Key.TypeRating, Quantity = m.Count() });
            return query.ToList();
        }
        public void AddComment(ProductRating data)
        {
            FDIDB.ProductRatings.Add(data);
        }

        public List<RatingAppIG4Item> GetTotalRating(int productId)
        {

            return FDIDB.ProductRatings.Where(m => m.ProductId == productId).GroupBy(m => new { m.ProductId, m.TypeRating }).Select(m => new RatingAppIG4Item() { TypeRating = m.Key.TypeRating, Quantity = m.Count() }).ToList();

        }
        public ProductRating GetComment(int productid, int customerId)
        {
            return FDIDB.ProductRatings.FirstOrDefault(m => m.CustomerId == customerId && m.ProductId == productid);
        }
        public SaleCode GetSaleCodeUseByCode(string code)
        {
            var todayCode = DateTime.Now.TotalSeconds();
            return FDIDB.SaleCodes.FirstOrDefault(m => m.Code == code && (m.IsUse == null || m.IsUse == false) && m.DN_Sale.DateEnd >= todayCode && m.DN_Sale.DateStart <= todayCode);
        }

        public async Task<int> TotalNearProductByCategoryId(int categoryId, string name, double minKm, double maxKm, int minPrice, int maxPrice, double Latitude, double Longitude)
        {
            var query = FDIDB.Shop_Product.Where(m => m.CategoryId == categoryId && m.IsShow == true && m.IsDelete == false
            && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude) <= maxKm)
            && (minKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude) >= minKm)
            && (maxPrice == 0 || m.PriceNew <= maxPrice)
            && (minPrice == 0 || m.PriceNew >= minPrice)
            && m.Name.Contains(name)
            ).CountAsync();

            return await query;
        }

        public async Task<int> TotalNearProductByCategoryId(bool hasTransfer, int categoryId, string name, double minKm, double maxKm, int minPrice, int maxPrice, double Latitude, double Longitude)
        {
            var query = FDIDB.Shop_Product.Where(m => m.CategoryId == categoryId && m.HasTransfer == hasTransfer && m.IsDelete == false
             && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude) <= maxKm)
            && (minKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude) >= minKm)
            && (maxPrice == 0 || m.PriceNew <= maxPrice)
            && (minPrice == 0 || m.PriceNew >= minPrice)
            && m.Name.Contains(name)
            ).CountAsync();

            return await query;
        }
        public async Task<int> ProductsHot(int categoryId, string name, double minKm, double maxKm, int minPrice, int maxPrice, double Latitude, double Longitude)
        {
            var query = FDIDB.Shop_Product.Where(m => m.CategoryId == categoryId && m.IsDelete == false && m.IsHot == true
             && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude) <= maxKm)
            && (minKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude) >= minKm)
            && (maxPrice == 0 || m.PriceNew <= maxPrice)
            && (minPrice == 0 || m.PriceNew >= minPrice)
            && m.Name.Contains(name)
            ).CountAsync();

            return await query;
        }
        public async Task<int> ProductsIsReview(int categoryId, string name, double minKm, double maxKm, int minPrice, int maxPrice, double Latitude, double Longitude)
        {
            var query = FDIDB.Shop_Product.Where(m => m.CategoryId == categoryId && m.IsDelete == false && m.IsUpcoming == true
             && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude) <= maxKm)
            && (minKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)m.Latitude, (float)m.Longitude) >= minKm)
            && (maxPrice == 0 || m.PriceNew <= maxPrice)
            && (minPrice == 0 || m.PriceNew >= minPrice)
            && m.Name.Contains(name)
            ).CountAsync();

            return await query;
        }

        public List<ProductAppIG4Item> GetNearItemByCategoryId(int id, string name, double minKm, double maxKm, int minPrice, int maxPrice, double Latitude, double Longitude, int page, int pagesize)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Product
                        where c.CategoryId == id && c.IsShow == true && (c.IsDelete == false)
                              && (maxKm == 0 ||
                                  ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) <= maxKm ||
                                  ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) <= maxKm)
                        && (minKm == 0 ||
                            ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) >= minKm
                            || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) >= minKm)

                              && (maxPrice == 0 || c.PriceNew <= maxPrice)
                        && (minPrice == 0 || c.PriceNew >= minPrice)
                        && c.Name.Contains(name)
                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PriceNew = c.PriceNew,
                            Ratings = c.Ratings ?? 0,
                            AvgRating = c.AvgRating ?? 0,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                            HasTransfer = c.HasTransfer,
                            Sort = !c.CustomerID1.HasValue ? (c.Customer.Order_Package.Where(m => m.DateStart < date && m.DateEnd > date).Select(m => m.Customer_Type.Sort).FirstOrDefault() ?? 10000) : 0,
                        };
            query = query.OrderBy(m => m.Sort).OrderByDescending(m => m.AvgRating).ThenByDescending(m => m.Ratings);
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }

        public List<ProductAppIG4Item> GetHasTransferItemByCategoryId(int id, string name, double minKm, double maxKm, int minPrice, int maxPrice, double Latitude, double Longitude, int page, int pagesize)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Product
                        where (c.CategoryId == id || id == 0) && c.IsShow == true && c.IsDelete == false && c.HasTransfer == true
                         && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) <= maxKm)
                        && (minKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) >= minKm)
                        && (maxPrice == 0 || c.PriceNew <= maxPrice)
                        && (minPrice == 0 || c.PriceNew >= minPrice)
                        && c.Name.Contains(name)
                        select
            new ProductAppIG4Item()
            {
                ID = c.ID,
                Name = c.Name,
                PriceNew = c.PriceNew,
                Ratings = c.Ratings ?? 0,
                AvgRating = c.AvgRating ?? 0,
                UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                HasTransfer = c.HasTransfer,
                Sort = !c.CustomerID1.HasValue ? (c.Customer.Order_Package.Where(m => m.DateStart < date && m.DateEnd > date).Select(m => m.Customer_Type.Sort).FirstOrDefault() ?? 10000) : 0,
            };
            query = query.OrderBy(m => m.Sort).OrderByDescending(m => m.AvgRating).ThenByDescending(m => m.Ratings);
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }
        public List<ProductAppIG4Item> GetIsHotItemByCategoryId(int id, string name, double minKm, double maxKm, int minPrice, int maxPrice, double Latitude, double Longitude, int page, int pagesize)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Product
                        where c.CategoryId == id && c.IsShow == true && c.IsDelete == false && c.IsHot == true
                          && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) <= maxKm)
                        && (minKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) >= minKm)
                        && (maxPrice == 0 || c.PriceNew <= maxPrice)
                        && (minPrice == 0 || c.PriceNew >= minPrice)
                        && c.Name.Contains(name)
                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PriceNew = c.PriceNew,
                            Ratings = c.Ratings ?? 0,
                            AvgRating = c.AvgRating ?? 0,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                            HasTransfer = c.HasTransfer,
                            Sort = !c.CustomerID1.HasValue ? (c.Customer.Order_Package.Where(m => m.DateStart < date && m.DateEnd > date).Select(m => m.Customer_Type.Sort).FirstOrDefault() ?? 10000) : 0,
                        };
            query = query.OrderBy(m => m.Sort).OrderByDescending(m => m.AvgRating).ThenByDescending(m => m.Ratings);
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }

        public List<ProductAppIG4Item> GetIsReviewItemByCategoryId(int id, string name, double minKm, double maxKm, int minPrice, int maxPrice, double Latitude, double Longitude, int page, int pagesize)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Product
                        where c.CategoryId == id && c.IsShow == true && c.IsDelete == false && c.IsUpcoming == true
                          && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) <= maxKm)
                        && (minKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) >= minKm)
                        && (maxPrice == 0 || c.PriceNew <= maxPrice)
                        && (minPrice == 0 || c.PriceNew >= minPrice)
                        && c.Name.Contains(name)
                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PriceNew = c.PriceNew,
                            Ratings = c.Ratings ?? 0,
                            AvgRating = c.AvgRating ?? 0,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                            HasTransfer = c.HasTransfer,
                            Sort = !c.CustomerID1.HasValue ? (c.Customer.Order_Package.Where(m => m.DateStart < date && m.DateEnd > date).Select(m => m.Customer_Type.Sort).FirstOrDefault() ?? 10000) : 0,
                        };
            query = query.OrderBy(m => m.Sort).OrderByDescending(m => m.AvgRating).ThenByDescending(m => m.Ratings);
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }
        public List<ProductAppIG4Item> GetBestBuy(double Latitude, double Longitude, int met, int page, int pagesize)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Product
                        where c.IsShow == true && c.IsDelete == false && c.HasTransfer == true
                        && ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) < met

                        select new ProductAppIG4Item
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PriceNew = c.PriceNew,
                            Ratings = c.Ratings ?? 0,
                            AvgRating = c.AvgRating ?? 0,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Latitude = c.Latitude,
                            HasTransfer = c.HasTransfer,
                            Longitude = c.Longitude,
                            QuantityOut = c.QuantityOut,
                            Sort = !c.CustomerID1.HasValue ? (c.Customer.Order_Package.Where(m => m.DateStart < date && m.DateEnd > date).Select(m => m.Customer_Type.Sort).FirstOrDefault() ?? 10000) : 0,
                            Km = ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) / 1000
                        };

            query = query.OrderBy(m => m.Sort).OrderBy(m => m.Km).ThenByDescending(m => m.QuantityOut);
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();
        }

        public List<ProductAppIG4Item> GetCategoryForMap(string name, double minKm, double maxKm, int minPrice, int maxPrice, int categoryId, double Latitude, double Longitude)
        {
            if (!string.IsNullOrEmpty(name)) name = String.Join(",", FDIUtils.Slug(FDIUtils.NewUnicodeToAscii(name)).Split('_').ToList());
            var query = from p in FDIDB.ShopMap(name, maxKm, minKm, categoryId, Latitude, Longitude, minPrice, maxPrice)

                        select new ProductAppIG4Item
                        {
                            Longitude = p.Longitude ?? 0,
                            Latitude = p.Latitude ?? 0,
                            CustomerId = p.CustomerId,
                            IsPrestige = p.IsPrestige,
                            UrlPicture = p.UrlIcon,
                            Sort = p.sortut
                        };
            return query.ToList();

        }
        public List<ProductAppIG4Item> GetProductForMap(string name, double minKm, double maxKm, int cateid, int minPrice, int maxPrice, double Latitude, double Longitude, int page, int pagesize, bool HasTransfer, int shopid)
        {
            if (!string.IsNullOrEmpty(name)) name = String.Join(",", FDIUtils.Slug(FDIUtils.NewUnicodeToAscii(name)).Split('_').ToList());
            var query = FDIDB.ProductFull(name, maxKm, minKm, cateid, Latitude, Longitude, minPrice, maxPrice, page, pagesize, HasTransfer, shopid)
                .Select(l => new ProductAppIG4Item
                {
                    ID = l.id ?? 0,
                    Name = l.Name,
                    PriceNew = l.PriceNew,
                    Ratings = l.Ratings ?? 0,
                    AvgRating = l.AvgRating ?? 0,
                    UrlPicture = l.Folder + l.Url,
                    kc = l.kc,
                    kc1 = l.kc1,
                    IsPrestige = l.IsPrestige,
                    IsPrestige1 = l.IsPrestige1,
                    CateId = l.CateId,
                    Km = l.km,
                    Sort = l.sort
                });
            return query.ToList();
        }

        public List<ProductAppIG4Item> GetMyProduct(int customerId, int categoryId, string name, int maxKm, int minPrice, int maxPrice, double Latitude, double Longitude, int page, int pagesize)
        {

            var query = from c in FDIDB.Shop_Product
                        where (c.CategoryId == categoryId || categoryId == 0) && c.IsShow == true && c.IsDelete == false && (c.CustomerID == customerId || c.CustomerID1 == customerId) && c.Name.Contains(name)
                         && (minPrice == 0 || c.PriceNew >= minPrice)
                        && (maxPrice == 0 || c.PriceNew <= maxPrice)
                          && (maxKm == 0 || ConvertUtil.DistanceBetween((float)Latitude, (float)Longitude, (float)c.Latitude, (float)c.Longitude) <= maxKm)
                        orderby c.ID descending
                        select
                        new ProductAppIG4Item()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PriceNew = c.PriceNew,
                            Ratings = c.Ratings ?? 0,
                            AvgRating = c.AvgRating ?? 0,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                            QuantityOut = c.QuantityOut ?? 0,
                            Quantity = c.Quantity,
                            HasTransfer = c.HasTransfer != null && c.HasTransfer.Value
                        };
            query = query.Skip(pagesize * (page - 1)).Take(pagesize);

            return query.ToList();
        }
        public List<ProductAppIG4Item> GetProductByPosition(double latitude, double longitude, int page, int pagesize)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Shop_Product
                        where c.IsShow == true && c.IsDelete == false && c.Latitude == latitude && c.Longitude == longitude
                        orderby c.ID descending
                        select
            new ProductAppIG4Item()
            {
                ID = c.ID,
                Name = c.Name,
                PriceNew = c.PriceNew,
                Ratings = c.Ratings ?? 0,
                AvgRating = c.AvgRating ?? 0,
                UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                QuantityOut = c.QuantityOut ?? 0,
                Quantity = c.Quantity,
                HasTransfer = c.HasTransfer,
                Sort = !c.CustomerID1.HasValue ? (c.Customer.Order_Package.Where(m => m.DateStart < date && m.DateEnd > date).Select(m => m.Customer_Type.Sort).FirstOrDefault() ?? 10000) : 0,
            };
            query = query.OrderBy(m => m.Sort).Skip(pagesize * (page - 1)).Take(pagesize);
            return query.ToList();

        }

    }
}
