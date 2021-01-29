using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DL
{
    public class ProductDetailDL : BaseDA
    {
        public List<ShopProductDetailItem> GetListProductbylstId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.IsDelete == false  && c.IsShow == true && (c.Categories.Any(a => ltsArrId.Contains(a.Id)) || c.Categories.Any(b => ltsArrId.Contains(b.ParentId ?? 3))) && c.IsHot == true
                        orderby c.ID
                        select new ShopProductDetailItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Description = c.Description,
                            ListGalleryPictureItems = c.Gallery_Picture2.Where(a => a.IsDeleted == false && a.IsShow == true).Select(z => new GalleryPictureItem
                            {
                                Url = z.Folder + z.Url
                            }),
                            ListProductItem = c.Shop_Product.Where(a => a.IsDelete != true).Select(v => new ProductItem
                            {
                                ID = v.ID,
                                PriceNew = (v.Shop_Product_Detail.Price * v.Product_Size.Value / 1000) ?? 0,
                                //PriceOld = v.PriceOld
                            }),
                            //ListColorProductItem = from a in c.Shop_Product
                            //                       group a by a.ColorID into g
                            //                       select new ColorItem
                            //                       {
                            //                           ID = g.FirstOrDefault().System_Color.ID,
                            //                           Name = g.FirstOrDefault().System_Color.Name,
                            //                           Value = g.FirstOrDefault().System_Color.Value
                            //                       },
                            ListSizeProductItem = from a in c.Shop_Product
                                                  group a by a.SizeID into g
                                                  select new ProductSizeItem
                                                  {
                                                      ID = g.FirstOrDefault().Product_Size.ID,
                                                      Name = g.FirstOrDefault().Product_Size.Name,
                                                      Value = g.FirstOrDefault().Product_Size.Value
                                                  }
                        };
            return query.ToList();
        }
        public List<ProductDetailsItem> ListAll()
        {
            var query = from c in FDIDB.Shop_Product_Detail
                where (!c.IsDelete.HasValue || !c.IsDelete.Value) && c.Shop_Product.Any(m => (!m.IsDelete.HasValue || !m.IsDelete.Value) &&m.SizeID.HasValue)
                select
                    new ProductDetailsItem
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Slug = c.NameAscii,
                        SlugCate = c.Category.Slug,
                        PriceNew = c.Price,
                        NameUnit = c.UnitID.HasValue ? c.DN_Unit.Name : null,
                        UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                        Description = c.Description,
                        DateSale = c.StartDate,
                        //freeShipFor = c.d
                        PAppItems = c.Shop_Product.Where(m => !m.IsDelete.HasValue || !m.IsDelete.Value).Select(m => new PAppItem
                        {
                            ID = m.ID,
                            Name = m.SizeID.HasValue ? m.Product_Size.Name : null,
                            Value = m.Product_Size.Value,
                        }),
                        CateId = c.CateID
                    };
            return query.ToList();
        }
        public ProductDetailsItem GetById(int id)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                where c.ID == id && (!c.IsDelete.HasValue || !c.IsDelete.Value) && c.Shop_Product.Any(m => (!m.IsDelete.HasValue || !m.IsDelete.Value) && m.SizeID.HasValue)
                select
                    new ProductDetailsItem
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Slug = c.NameAscii,
                        SlugCate = c.Category.Slug,
                        PriceNew = c.Price,
                        NameUnit = c.UnitID.HasValue ? c.DN_Unit.Name : null,
                        UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                        Pictures = c.Gallery_Picture2.Select(m=>m.Folder+m.Url),
                        Description = c.Description,
                        Details = c.Details,
                        DateSale = c.StartDate,
                        PAppItems = c.Shop_Product.Where(m => !m.IsDelete.HasValue || !m.IsDelete.Value).Select(m => new PAppItem
                        {
                            ID = m.ID,
                            Name = m.SizeID.HasValue ? m.Product_Size.Name : null,
                            Value = m.Product_Size.Value,
                        }),
                        //Pictures = c.Gallery_Picture2.Where(a => a.IsDeleted == false && a.IsShow == true).Select(z => z.Folder + z.Url),
                        CateId = c.CateID
                    };
            return query.FirstOrDefault();
        }
        public List<ShopProductDetailItem> GetListProductbylstCateId(int lstId)
        {

            var query = from c in FDIDB.Shop_Product_Detail
                        where c.IsDelete == false  && c.IsShow == true && (c.Categories.Any(a => a.Id == lstId))
                        orderby c.ID
                        select new ShopProductDetailItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Description = c.Description,
                            Sort = c.Sort,
                            ListGalleryPictureItems = c.Gallery_Picture2.Where(a => a.IsDeleted == false && a.IsShow == true).Select(z => new GalleryPictureItem
                            {
                                Url = z.Folder + z.Url
                            }),
                            ListProductItem = c.Shop_Product.Where(a => a.IsDelete != true).Select(v => new ProductItem
                            {
                                ID = v.ID,
                                PriceNew = (v.Shop_Product_Detail.Price * v.Product_Size.Value / 1000) ?? 0,
                                //PriceOld = v.PriceOld
                            }),
                            //ListColorProductItem = from a in c.Shop_Product
                            //                       group a by a.ColorID into g
                            //                       select new ColorItem
                            //                       {
                            //                           ID = g.FirstOrDefault().System_Color.ID,
                            //                           Name = g.FirstOrDefault().System_Color.Name,
                            //                           Value = g.FirstOrDefault().System_Color.Value
                            //                       },
                            ListSizeProductItem = from a in c.Shop_Product
                                                  group a by a.SizeID into g
                                                  select new ProductSizeItem
                                                  {
                                                      ID = g.FirstOrDefault().Product_Size.ID,
                                                      Name = g.FirstOrDefault().Product_Size.Name,
                                                      Value = g.FirstOrDefault().Product_Size.Value
                                                  }
                        };
            return query.ToList();
        }
        public ShopProductDetailItem GetProductbySlug(string slug)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.IsDelete == false  && c.IsShow == true && c.NameAscii == slug
                        select new ShopProductDetailItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                        };
            return query.FirstOrDefault();
        }
        public List<ShopProductDetailItem> GetListHot()
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where c.IsDelete != true  && c.IsShow == true && c.IsHot == true
                        orderby c.ID descending
                        select new ShopProductDetailItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            NameAscii = c.NameAscii,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            UrlPictureMap = c.Gallery_Picture1.Folder + c.Gallery_Picture1.Url,
                            Sale = c.Sale,
                            ListProductItem = c.Shop_Product.Where(a => a.IsDelete != true).Select(v => new ProductItem
                            {
                                ID = v.ID,
                                PriceNew = (v.Shop_Product_Detail.Price * v.Product_Size.Value / 1000) ?? 0,
                                //PriceOld = v.PriceOld
                            }),
                            //ListColorProductItem = from a in c.Shop_Product
                            //                       group a by a.ColorID into g
                            //                       select new ColorItem
                            //                       {
                            //                           ID = g.FirstOrDefault().System_Color.ID,
                            //                           Name = g.FirstOrDefault().System_Color.Name,
                            //                           Value = g.FirstOrDefault().System_Color.Value
                            //                       },
                            ListSizeProductItem = from a in c.Shop_Product
                                                  where a.IsDelete != true
                                                  group a by a.SizeID into g
                                                  select new ProductSizeItem
                                                  {
                                                      ID = g.FirstOrDefault().Product_Size.ID,
                                                      Name = g.FirstOrDefault().Product_Size.Name,
                                                      Value = g.FirstOrDefault().Product_Size.Value
                                                  }
                        };
            return query.ToList();
        }
        public List<ShopProductDetailItem> GetList(string slug, int page, int rowPage, ref int total, string color, string size, string sort)
        {
            var query = from n in FDIDB.Shop_Product_Detail
                        where n.IsDelete == false && n.IsShow == true 
                        //&& n.Categories.Any(a => a.Slug == slug)
                        orderby n.ID descending
                        select new ShopProductDetailItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            NameAscii = n.NameAscii,
                            Sale = n.Sale,
                            DateCreate = n.DateCreate,
                            UrlPicture = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            UrlPictureMap = n.Gallery_Picture1.IsDeleted != true ? n.Gallery_Picture1.Folder + n.Gallery_Picture1.Url : "",
                            Description = n.Description,
                            ListProductItem = n.Shop_Product.Where(c => c.IsDelete != true).Select(v => new ProductItem
                            {
                                ID = v.ID,
                                PriceNew = (v.Shop_Product_Detail.Price * v.Product_Size.Value / 1000) ?? 0,
                                //PriceOld = v.PriceOld
                            }),
                            //ListColorProductItem = from a in n.Shop_Product
                            //                       group a by a.ColorID into g
                            //                       select new ColorItem
                            //                       {
                            //                           ID = g.FirstOrDefault().System_Color.ID,
                            //                           Name = g.FirstOrDefault().System_Color.Name,
                            //                           Value = g.FirstOrDefault().System_Color.Value
                            //                       },
                            ListSizeProductItem = from a in n.Shop_Product
                                                  group a by a.SizeID into g
                                                  select new ProductSizeItem
                                                  {
                                                      ID = g.FirstOrDefault().Product_Size.ID,
                                                      Name = g.FirstOrDefault().Product_Size.Name,
                                                      Value = g.FirstOrDefault().Product_Size.Value
                                                  }
                        };
            if (!string.IsNullOrEmpty(color))
            {
                var lstArr = FDIUtils.StringToListInt(color);
                query = query.Where(c => c.ListColorProductItem.Any(a => lstArr.Contains(a.ID)));
            }
            if (!string.IsNullOrEmpty(size))
            {
                var lstArr = FDIUtils.StringToListInt(size);
                query = query.Where(c => c.ListSizeProductItem.Any(a => lstArr.Contains(a.ID)));
            }
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "az")
                {
                    query = query.OrderBy(c => c.Name);
                }
                if (sort == "za")
                {
                    query = query.OrderByDescending(c => c.Name);
                }
                if (sort == "lh")
                {
                    query = query.OrderBy(c => c.ListProductItem.Select(a => a.PriceNew).FirstOrDefault());
                }
                if (sort == "hl")
                {
                    query = query.OrderByDescending(c => c.ListProductItem.Select(a => a.PriceNew).FirstOrDefault());
                }
            }
            query = query.Paging(page, rowPage, ref total);
            return query.ToList();
        }
        public List<ShopProductDetailItem> GetListBykeyword(string keyword, int page, int rowPage, ref int total, string color, string size, string sort)
        {
            var query = from n in FDIDB.Shop_Product_Detail
                        where n.IsDelete == false && n.IsShow == true 
                        && n.NameAscii.Contains(keyword)
                        orderby n.ID descending
                        select new ShopProductDetailItem
                        {
                            ID = n.ID,
                            Name = n.Name,
                            NameAscii = n.NameAscii,
                            Sale = n.Sale,
                            UrlPicture = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            UrlPictureMap = n.Gallery_Picture1.IsDeleted != true ? n.Gallery_Picture1.Folder + n.Gallery_Picture1.Url : "",
                            Description = n.Description,
                            ListProductItem = n.Shop_Product.Where(c => c.IsDelete != true).Select(v => new ProductItem
                            {
                                ID = v.ID,
                                PriceNew = (v.Shop_Product_Detail.Price * v.Product_Size.Value / 1000) ?? 0,
                                //PriceOld = v.PriceOld
                            }),
                            //ListColorProductItem = from a in n.Shop_Product
                            //                       group a by a.ColorID into g
                            //                       select new ColorItem
                            //                       {
                            //                           ID = g.FirstOrDefault().System_Color.ID,
                            //                           Name = g.FirstOrDefault().System_Color.Name,
                            //                           Value = g.FirstOrDefault().System_Color.Value
                            //                       },
                            ListSizeProductItem = from a in n.Shop_Product
                                                  group a by a.SizeID into g
                                                  select new ProductSizeItem
                                                  {
                                                      ID = g.FirstOrDefault().Product_Size.ID,
                                                      Name = g.FirstOrDefault().Product_Size.Name,
                                                      Value = g.FirstOrDefault().Product_Size.Value
                                                  }
                        };
            if (!string.IsNullOrEmpty(color))
            {
                var lstArr = FDIUtils.StringToListInt(color);
                query = query.Where(c => c.ListColorProductItem.Any(a => lstArr.Contains(a.ID)));
            }
            if (!string.IsNullOrEmpty(size))
            {
                var lstArr = FDIUtils.StringToListInt(size);
                query = query.Where(c => c.ListSizeProductItem.Any(a => lstArr.Contains(a.ID)));
            }
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "az")
                {
                    query = query.OrderBy(c => c.Name);
                }
                if (sort == "za")
                {
                    query = query.OrderByDescending(c => c.Name);
                }
                if (sort == "lh")
                {
                    query = query.OrderBy(c => c.ListProductItem.Select(a => a.PriceNew).FirstOrDefault());
                }
                if (sort == "hl")
                {
                    query = query.OrderByDescending(c => c.ListProductItem.Select(a => a.PriceNew).FirstOrDefault());
                }
            }
            query = query.Paging(page, rowPage, ref total);
            return query.ToList();
        }
    }
}
