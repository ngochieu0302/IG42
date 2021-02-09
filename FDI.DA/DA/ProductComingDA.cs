using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class ProductComingDA : BaseDA
    {
        #region Constructer
        public ProductComingDA()
        {
        }

        public ProductComingDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ProductComingDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<ProductComingItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.Shop_Product_Comingsoon
                orderby c.ID descending
                        select new ProductComingItem
                        {
                                       ID = c.ID,
                                       Productname = c.Shop_Product_Detail.Name,
                                   };
            return query.ToList();
        }

        public List<ProductComingItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Shop_Product_Comingsoon
                orderby c.ID descending 
                        select new ProductComingItem
                        {
                            ID = c.ID,
                            Productname = c.Shop_Product_Detail.Name,
                            Catename = c.Shop_Product_Detail.Category.Name,
                            Ncc = c.SupplierAmountProduct.DN_Supplier.Name,
                            DateEx = c.DateEx,
                            Quantity = c.Quantity,
                            QuantityOut = c.QuantityOut,
                            Price = c.Price
                           
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="id">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Shop_Product_Comingsoon GetById(int id)
        {
            var query = from c in FDIDB.Shop_Product_Comingsoon where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<Shop_Product_Comingsoon> GetById(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Shop_Product_Comingsoon where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(Shop_Product_Comingsoon item)
        {
            FDIDB.Shop_Product_Comingsoon.Add(item);
        }

        public void Delete(Shop_Product_Comingsoon item)
        {
            FDIDB.Shop_Product_Comingsoon.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion
    }
}
