using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA.AppCustomer
{
    public class ProductCusAppDA : BaseDA
    {
        #region Contruction

        public ProductCusAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ProductCusAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public List<ProductAppItem> GetListProduct()
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where (!c.IsDelete.HasValue || c.IsDelete == false) && c.IsShow24hApp == true && c.IsShow == true
                        orderby c.Sort
                        select new ProductAppItem
                        {
                            Name = c.Name,
                            ID = c.ID,
                            Pu = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Q = 1,
                            D = c.DateCreate,
                            Pr = c.Price,
                            PrD = c.Shop_Product.FirstOrDefault().Product_Size.Value * c.Price / 1000,
                        };
            return query.ToList();
        }
        public ProductAppItem GetProductItem(int id)
        {
            var model = new ProductAppItem();

            var query = from c in FDIDB.Shop_Product_Detail
                        where (!c.IsDelete.HasValue || c.IsDelete == false) && c.IsShow24hApp == true && c.IsShow == true && c.ID == id
                        orderby c.Sort
                        select new ProductAppItem
                        {
                            Name = c.Name,
                            ID = c.ID,
                            Pu = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Q = 1,
                            D = c.DateCreate,
                            Pr = c.Price,
                            detail = c.Details,
                            PrD = c.Shop_Product.FirstOrDefault().Product_Size.Value * c.Price / 1000,
                            Size = c.Shop_Product.FirstOrDefault().Product_Size.Name,
                            SizeValue = c.Shop_Product.FirstOrDefault().Product_Size.Value, //TODO: thiếu where
                            des = c.Description,
                            ledge = c.Knowledge,
                            procces = c.Proccess,
                            lstS = c.Shop_Product.Where(a => a.TypeID != null).GroupBy(a => a.TypeID).Select(a => new TypeAppItem
                            {
                                ID = a.FirstOrDefault().Shop_Product_Type.ID,
                                Name = a.FirstOrDefault().Shop_Product_Type.Name,
                                Price = a.FirstOrDefault().Shop_Product_Detail.Price * (a.FirstOrDefault().Product_Size == null ? 1000 : a.FirstOrDefault().Product_Size.Value) / 1000
                            }),
                            lstId = c.Shop_Product.Where(a => a.IsDelete == false).Select(v => new PtemAppItem
                            {
                                ID = v.ID
                            })
                        };
            return query.FirstOrDefault();
        }

        public List<ImgAppItem> GetListIMGApp(int id)
        {
            var query = from c in FDIDB.Shop_Product_Picture
                        where c.ProductID == id
                        select new ImgAppItem
                        {
                            ID = c.Gallery_Picture.ID,
                            Url = c.Gallery_Picture.Folder + c.Gallery_Picture.Url
                        };
            return query.ToList();
        }
        public List<ProductItem> GetListProduct24H(int categoryId)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        join cate in FDIDB.Categories on c.CateID equals cate.Id
                        where (!c.IsDelete.HasValue || c.IsDelete == false) && c.IsShow24hApp == true && c.IsShow == true && (cate.Id == categoryId || cate.ParentId == categoryId)
                        select new ProductItem
                        {
                            Name = c.Name,
                            ID = c.ID,
                            UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            PriceUnit = c.Price,
                        };
            return query.Distinct().Take(10).ToList();
        }
        public List<ProductItem> GetListProduct24H()
        {
            var query = from c in FDIDB.Shop_Product_Detail
                where (!c.IsDelete.HasValue || c.IsDelete == false) && c.IsShow24hApp == true && c.IsShow == true
                select new ProductItem
                {
                    Name = c.Name,
                    ID = c.ID,
                    UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                    PriceUnit = c.Price,
                    Description = c.Description
                };
            return query.Take(50).ToList();
        }

        public List<ProductItem> GetListByCategoryId(int categoryId)
        {
            var query = from c in FDIDB.Shop_Product_Detail
                where (!c.IsDelete.HasValue || c.IsDelete == false) && c.CateID == categoryId
                orderby c.Sort
                select new ProductItem
                {
                    Name = c.Name,
                    ID = c.ID,
                    UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                    PriceUnit = c.Price,
                    Description = c.Description
                };
            return query.ToList();
        }
    }
}
