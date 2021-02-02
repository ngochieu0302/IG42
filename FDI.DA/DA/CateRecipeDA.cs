using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.DA
{
    public class CateRecipeDA : BaseDA
    {
        private readonly ProductDetailRecipeDA _productDetailRecipeDa = new ProductDetailRecipeDA();
        #region Constructer
        public CateRecipeDA()
        {
        }

        public CateRecipeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CateRecipeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CateRecipeItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Category_Recipe
                        orderby o.CategoryID descending
                        select new CateRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            CategoryID = o.CategoryID,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            CateName = o.Category.Name,
                            Username = o.DN_Users.UserName,
                            TotalPrice = o.TotalPrice,
                            TotalPercent = o.TotalPercent,
                            IsUse = o.IsUse,
                            Totalkg = o.Totalkg
                        };
            var cate = httpRequest["cate"];
            if (Equals(!string.IsNullOrEmpty(cate)))
            {
                var id = int.Parse(cate);
                query = query.Where(a => a.CategoryID == id);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public CateRecipeItem GetItemById(int id)
        {
            var query = from o in FDIDB.Category_Recipe
                        where o.ID == id
                        select new CateRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            CategoryID = o.CategoryID,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            CateName = o.Category.Name,
                            Username = o.DN_Users.UserName,
                            TotalPrice = o.TotalPrice,
                            TotalPriceFinal = o.TotalPriceFinal,
                            TotalIncurred = o.TotalIncurred,
                            Weight = o.Weight,
                            IsUse = o.IsUse,
                            Loss = o.Category.PercentLoss ?? 0,
                            TotalPercent = o.TotalPercent,
                            Totalkg = o.Totalkg,
                            Price = o.Category.Price,
                            PriceFinal = o.Category.PriceFinal,
                            LstCategoryRecipeItems = o.Category_Product_Recipe.Where(c => c.IsDeleted == false).Select(c => new CategoryRecipeItem
                            {
                                ID = c.ID,
                                ProductName = c.Shop_Product_Detail.Name,
                                Quantity = c.Quantity,
                                Price = c.Price,
                                IsCheck = c.IsCheck,
                                PriceProduct = c.PriceProduct,
                                DateCreate = c.DateCreate,
                                PercentProduct = o.Category.Percent,
                                Incurred = c.Incurred,
                                UnitName = c.Shop_Product_Detail.DN_Unit.Name,
                                ProductId = c.ProductId,
                                TotalPrice = c.Price * c.Quantity,
                                Percent = c.Percent,
                                UnitId = c.Shop_Product_Detail.UnitID
                            }),
                            LstMappingCategoryRecipeItems = o.Mapping_Category_Recipe.Where(c => c.IsDeleted == false).Select(c => new MappingCategoryRecipeItem
                            {
                                ID = c.ID,
                                ProductName = c.Category.Name,
                                Quantity = c.Quantity,
                                Price = c.Price,
                                IsCheck = c.IsCheck,
                                PriceProduct = c.PriceProduct,
                                UnitId = c.Category.UnitID,
                                DateCreate = c.DateCreate,
                                PercentProduct = o.Category.Percent,
                                Incurred = c.Incurred,
                                UnitName = c.Category.DN_Unit.Name,
                                CategoryID = c.CategoryID,
                                TotalPrice = c.Price,
                                Percent = c.Percent,
                                Sl = c.Sl,
                            })
                        };
            return query.FirstOrDefault();
        }
        public CateRecipeItem GetItemByCateIdUser(int id)
        {
            var query = from o in FDIDB.Category_Recipe
                        where o.CategoryID == id &&(o.IsDeleted.HasValue || o.IsDeleted == false) && o.IsUse == true
                        select new CateRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            CategoryID = o.CategoryID,
                            DateCreate = o.DateCreate,
                            DateUpdate = o.DateUpdate,
                            CateName = o.Category.Name,
                            Username = o.DN_Users.UserName,
                            TotalPrice = o.TotalPrice,
                            TotalPriceFinal = o.TotalPriceFinal,
                            TotalIncurred = o.TotalIncurred,
                            Weight = o.Weight,
                            IsUse = o.IsUse,
                            Loss = o.Category.PercentLoss ?? 0,
                            TotalPercent = o.TotalPercent,
                            Totalkg = o.Totalkg,
                            Price = o.Category.Price,
                            PriceFinal = o.Category.PriceFinal,
                            LstCategoryRecipeItems = o.Category_Product_Recipe.Where(c => c.IsDeleted == false).Select(c => new CategoryRecipeItem
                            {
                                ID = c.ID,
                                ProductName = c.Shop_Product_Detail.Name,
                                Quantity = c.Quantity,
                                Price = c.Price,
                                IsCheck = c.IsCheck,
                                PriceProduct = c.PriceProduct,
                                DateCreate = c.DateCreate,
                                PercentProduct = o.Category.Percent,
                                Incurred = c.Incurred,
                                UnitName = c.Shop_Product_Detail.DN_Unit.Name,
                                ProductId = c.ProductId,
                                TotalPrice = c.Price * c.Quantity,
                                Percent = c.Percent,
                                UnitId = c.Shop_Product_Detail.UnitID
                            }),
                            LstMappingCategoryRecipeItems = o.Mapping_Category_Recipe.Where(c => c.IsDeleted == false).Select(c => new MappingCategoryRecipeItem
                            {
                                ID = c.ID,
                                ProductName = c.Category.Name,
                                Quantity = c.Quantity,
                                Price = c.Price,
                                IsCheck = c.IsCheck,
                                PriceProduct = c.PriceProduct,
                                UnitId = c.Category.UnitID,
                                DateCreate = c.DateCreate,
                                PercentProduct = o.Category.Percent,
                                Incurred = c.Incurred,
                                UnitName = c.Category.DN_Unit.Name,
                                CategoryID = c.CategoryID,
                                TotalPrice = c.Price,
                                Percent = c.Percent,
                                Sl = c.Sl,
                            })
                        };
            return query.FirstOrDefault();
        }
        public List<CateRecipeItem> GetAll()
        {
            var query = from o in FDIDB.Category_Recipe
                        where !o.IsDeleted.HasValue || o.IsDeleted == false
                        select new CateRecipeItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            CateName = o.Category.Name,
                        };
            return query.ToList();
        }
        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="categoryId">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Category_Recipe GetById(int categoryId)
        {
            var query = from c in FDIDB.Category_Recipe where c.ID == categoryId select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrId">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Category_Recipe> GetListByArrId(List<int> lstId)
        {

            var query = from c in FDIDB.Category_Recipe where lstId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<Category_Recipe> GetListByCateId(int id)
        {

            var query = from c in FDIDB.Category_Recipe where c.CategoryID == id select c;
            return query.ToList();
        }
        public void Add(Category_Recipe category)
        {
            FDIDB.Category_Recipe.Add(category);
        }
        public void Delete(Category_Recipe category)
        {
            FDIDB.Category_Recipe.Remove(category);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion

        public List<CategoryRecipeItem> GetProduct(int categoryId)
        {
            //FDIDB.Category_Recipe.Where(m=>m.CategoryID == categoryId && m.IsUse==true && (m.IsDeleted ==null || m.IsDeleted==false))
            var query = from c in FDIDB.Category_Recipe
                        where c.CategoryID == categoryId && c.IsUse == true && (c.IsDeleted == null || c.IsDeleted == false)
                        select c;
            var lst = query.SelectMany(m => m.Category_Product_Recipe.Where(n => n.IsDeleted == false).Select(c => new CategoryRecipeItem()
            {
                ID = c.ID,
                ProductName = c.Shop_Product_Detail.Name,
                Quantity = c.Quantity,
                Price = c.Price,
                IsCheck = c.IsCheck,
                DateCreate = c.DateCreate,
                UnitName = c.Shop_Product_Detail.DN_Unit.Name,
                ProductId = c.ProductId,
                TotalPrice = c.Price * c.Quantity,
                Percent = c.Percent,
            }));
            return lst.ToList();
        }

        public IList<int> GetCategoryChild(int categoryId)
        {
            var query = from c in FDIDB.Category_Recipe
                        where c.CategoryID == categoryId && (c.IsDeleted == false || c.IsDeleted == null)
                        select c;
            var response = new List<int>() { categoryId };
            var categoryChild = query.SelectMany(m => m.Mapping_Category_Recipe.Select(n => n.CategoryID));

            response.AddRange(categoryChild.Where(m => m != null).Select(m => m.Value).ToList());
            return response;
        }

        public List<OrderDetailProductItem> GetProduct(IList<int> categoryIds)
        {
            var query = from categoryRecipe in FDIDB.Category_Recipe
                        join categoryProductRecipe in FDIDB.Category_Product_Recipe
                            on categoryRecipe.ID equals categoryProductRecipe.RecipeID
                        join shopProductDetail in FDIDB.Shop_Product_Detail on categoryProductRecipe.ProductId equals shopProductDetail.ID
                        where categoryRecipe.CategoryID != null && categoryIds.Contains(categoryRecipe.CategoryID.Value)
                        && (categoryRecipe.IsDeleted == null || categoryRecipe.IsDeleted == false)
                        && categoryRecipe.IsUse == true
                        && (categoryProductRecipe.IsDeleted == null || categoryProductRecipe.IsDeleted == false)
                        && (shopProductDetail.IsDelete == null || shopProductDetail.IsDelete == false)
                        select new OrderDetailProductItem
                        {
                            WeightRecipe = categoryProductRecipe.Quantity ?? 0,
                            ProductName = shopProductDetail.Name,
                            ProductId = shopProductDetail.ID,
                            PriceUnit = categoryProductRecipe.Price??0
                        };
            return query.ToList();
        }

        public List<ProduceProductDetailItem> GetProductFinal(IList<OrderDetailProductItem> products)
        {
            var lst = new List<ProduceProductDetailItem>();
            foreach (var product in products)
            {
                var productRecipe = _productDetailRecipeDa.GetProductRecipe(product.ProductId);
                if (productRecipe != null)
                {
                    var productsRecipe = _productDetailRecipeDa.GetRecipeDetails(productRecipe.ID);
                    foreach (var recipeProductDetail in productsRecipe)
                    {
                        if (recipeProductDetail.DetailID != null)
                            lst.Add(new ProduceProductDetailItem()
                            {
                                Quantity = recipeProductDetail.Quantity ?? 0,
                                ProductId = recipeProductDetail.DetailID.Value,
                                ProductName = recipeProductDetail.ProductName,
                                Weight = recipeProductDetail.Quantity ?? 0 * product.Quantity,
                                ProductParentId = productRecipe.ID
                            });
                        else
                        {
                            throw new Exception("ProductDetail_Recipe");
                        }
                    }
                }
                else
                {
                    lst.Add(new ProduceProductDetailItem()
                    {
                        Quantity = product.Quantity,
                        ProductId = product.CateID,
                        ProductName = product.ProductName,
                        SizeId = product.SizeId,
                        UnitName = product.UnitName,
                        Weight = product.Weight 
                    });
                }
            }

            return lst;
        }

        public List<MappingCategoryRecipeItem> GetMappingCategoryChild(int recipeId)
        {
            var query = from c in FDIDB.Mapping_Category_Recipe
                        where c.RecipeID == recipeId
                        && c.IsCheck == true
                        && c.IsDeleted.IsNotDelete()
                        select new MappingCategoryRecipeItem()
                        {
                            CategoryID = c.CategoryID,
                            Quantity = c.Quantity
                        };
            return query.ToList();
        }
    }
}
