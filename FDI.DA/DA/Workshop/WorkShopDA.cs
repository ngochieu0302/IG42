using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class WorkShopDA : BaseDA
    {
        #region Constructer
        public WorkShopDA()
        {
        }

        public WorkShopDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public WorkShopDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<WorkShopItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.P_Workshop
                        where (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        orderby c.ID descending
                        select new WorkShopItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Address = c.Address,
                            IsActive = c.IsActive,
                            CompanyName = c.Company.Name,
                            DateActive = c.DateActive,
                            UserName = c.DN_Users.UserName,
                            DateCreated = c.DateCreated,
                            Latitute = c.Latitute,
                            Longitude = c.Longitude,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public WorkShopItem GetItembyId(int id)
        {
            var query = from c in FDIDB.P_Workshop
                        where (!c.IsDeleted.HasValue || c.IsDeleted == false) && c.ID == id
                        select new WorkShopItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Address = c.Address,
                            IsActive = c.IsActive,
                            CompanyName = c.Company.Name,
                            DateActive = c.DateActive,
                            UserName = c.DN_Users.UserName,
                            DateCreated = c.DateCreated,
                            Latitute = c.Latitute,
                            Longitude = c.Longitude,
                            CateRecipeItems = c.Category_Recipe.Where(a => a.IsDeleted == false).Select(v => new CateRecipeItem
                            {
                                ID = v.ID,
                                CateName = v.Category.Name
                            }),
                            ProductDetailRecipeItems = c.ProductDetail_Recipe.Where(a => a.IsDeleted == false).Select(v => new ProductDetailRecipeItem
                            {
                                ID = v.ID,
                                ProductName = v.Shop_Product_Detail.Name,
                            })
                        };
            return query.FirstOrDefault();
        }
        public P_Workshop GetbyId(int id)
        {
            var query = from c in FDIDB.P_Workshop
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }

        public List<P_Workshop> GetListArrId(string lst)
        {
            var lstArr = FDIUtils.StringToListInt(lst);
            var query = from c in FDIDB.P_Workshop
                        where lstArr.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public List<Category_Recipe> GetListArrIdCateRecipe(string lst)
        {
            var lstArr = FDIUtils.StringToListInt(lst);
            var query = from c in FDIDB.Category_Recipe
                        where lstArr.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public List<ProductDetail_Recipe> GetListArrIdDetailRecipe(string lst)
        {
            var lstArr = FDIUtils.StringToListInt(lst);
            var query = from c in FDIDB.ProductDetail_Recipe
                        where lstArr.Contains(c.ID)
                        select c;
            return query.ToList();
        }

        public List<WorkShopItem> GetAll()
        {
            return FDIDB.P_Workshop.Where(m => (!m.IsDeleted.HasValue || m.IsDeleted == false)).Select(c =>
               new WorkShopItem()
               {
                   ID = c.ID,
                   Address = c.Address,
                   IsActive = c.IsActive,
                   CompanyName = c.Company.Name,
                   DateActive = c.DateActive,
                   UserName = c.DN_Users.UserName,
                   DateCreated = c.DateCreated,
                   Latitute = c.Latitute,
                   Longitude = c.Longitude,
                   Name = c.Name
               }).ToList();
        }
        public void Add(P_Workshop obj)
        {
            FDIDB.P_Workshop.Add(obj);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
