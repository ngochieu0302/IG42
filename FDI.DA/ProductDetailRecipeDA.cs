using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ProductDetailRecipeDA : BaseDA
    {
        #region Constructer
        public ProductDetailRecipeDA()
        {
        }

        public ProductDetailRecipeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ProductDetailRecipeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<ProductDetailRecipeItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.ProductDetail_Recipe
                        where (!o.IsDeleted.HasValue || o.IsDeleted == false)
                        orderby o.ID descending
                        select new ProductDetailRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            ProductDetailId = o.ProductDetailId,
                            TimeM = o.TimeM,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            ProductName = o.Shop_Product_Detail.Name,
                            Username = o.DN_Users.UserName,
                            IsUse = o.IsUse,
                            ProductPrice = o.ProductPrice,
                            ValuePrice = o.ValuePrice,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public ProductDetailRecipeItem GetItemById(int id)
        {
            var query = from o in FDIDB.ProductDetail_Recipe
                        where o.ID == id
                        select new ProductDetailRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            ProductDetailId = o.ProductDetailId,
                            TimeM = o.TimeM,
                            DateCreate = o.DateCreate,
                            IsUse = o.IsUse,
                            DateUpdate = o.DateUpdate,
                            ProductName = o.Shop_Product_Detail.Name,
                            Username = o.DN_Users.UserName,
                            ProductPrice = o.ProductPrice,
                            ValuePrice = o.ValuePrice,
                            LstRecipeProductValues = o.ProductValue_Recipe.Where(a => a.IsDeleted == false).Select(c => new RecipeProductValue
                            {
                                ValueName = c.Shop_Product_Value.Name,
                                Quantity = c.Quantity,
                                ProductValueId = c.ProductValueId,
                                ValuePrice = c.Shop_Product_Value.Price,
                                TotalPrice = (c.Shop_Product_Value.Price ?? 0) * c.Quantity,
                                UnitName = c.Shop_Product_Value.DN_Unit.Name,
                            }),
                            LstRecipeProductDetails = o.Mapping_ProductDetail_Recipe.Where(a => a.IsDeleted == false).Select(c => new RecipeProductDetail
                            {
                                ProductName = c.Shop_Product_Detail.Name,
                                Quantity = c.Quantity,
                                DetailID = c.DetailID,
                                ProductPrice = c.Shop_Product_Detail.PriceCost,
                                TotalPrice = (c.Shop_Product_Detail.PriceCost ?? 0) * c.Quantity,
                                UnitName = c.Shop_Product_Detail.DN_Unit.Name,
                            })
                        };
            return query.FirstOrDefault();
        }
        public List<ProductDetailRecipeItem> GetAll()
        {
            var query = from o in FDIDB.ProductDetail_Recipe
                        where o.IsDeleted == false || !o.IsDeleted.HasValue
                        select new ProductDetailRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            ProductName = o.Shop_Product_Detail.Name,
                        };
            return query.ToList();
        }
        public ProductDetail_Recipe GetById(int id)
        {
            var query = from c in FDIDB.ProductDetail_Recipe where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<ProductDetail_Recipe> ListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.ProductDetail_Recipe
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }
        public List<ProductDetail_Recipe> GetListByArrCateId(int id)
        {
            var query = from o in FDIDB.ProductDetail_Recipe
                        where o.ProductDetailId == id
                        select o;
            return query.ToList();
        }

        public List<RecipeProductDetail> GetRecipeDetails(int recipeId)
        {
            var query = from d in FDIDB.Mapping_ProductDetail_Recipe
                        where d.RecipeID == recipeId && (d.IsDeleted == null || d.IsDeleted == false)
                        select new RecipeProductDetail
                        {
                            Quantity = d.Quantity,
                            ProductName = d.Shop_Product_Detail.Name,
                            DetailID = d.DetailID,

                        };

            return query.ToList();
        }
        public ProductDetailRecipeItem GetProductRecipe(int id)
        {
            var query = from c in FDIDB.ProductDetail_Recipe
                        where c.ProductDetailId == id && (c.IsDeleted == false || c.IsDeleted == null) && c.IsUse == true && c.Mapping_ProductDetail_Recipe.Any()
                        select new ProductDetailRecipeItem
                        {
                            ID = c.ID,

                        };
            return query.FirstOrDefault();
        }

        public List<RecipeProductDetail> GetProductDetailRecipe(int productId)
        {
            var query = from c in FDIDB.ProductDetail_Recipe
                        join a in FDIDB.Mapping_ProductDetail_Recipe
                            on c.ID equals a.RecipeID
                        where (c.IsDeleted == false || c.IsDeleted == null) && c.IsUse == true
                        && (a.IsDeleted == false || a.IsDeleted == null)

                        select new RecipeProductDetail()
                        {
                            DetailID = a.DetailID,
                            Quantity = a.Quantity
                        };


            return query.ToList();
        }

        public void Add(ProductDetail_Recipe productSize)
        {
            FDIDB.ProductDetail_Recipe.Add(productSize);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }

}
