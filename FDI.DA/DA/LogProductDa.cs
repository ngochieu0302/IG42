using System;
using System.Linq;
using FDI.Base;

namespace FDI.DA
{
    public class LogProductDa : BaseDA
    {
        #region contructor

        public LogProductDa()
        {
        }

        public LogProductDa(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public LogProductDa(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        #region select
        public Shop_Product GetShopProductById(int id)
        {
            var products = from product in FDIDB.Shop_Product
                           where product.ID == id
                           select product;
            return products.FirstOrDefault();
        }
        
       #endregion

        #region insert
        /// <summary>
        /// Insert Log
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="userEdited"></param>
        /// <param name="propertiesChanged"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="actionTable"></param>
        //public void InsertLog(int productId, string userEdited, string propertiesChanged, string oldValue, string newValue, string actionTable)
        //{
        //    try
        //    {
        //        var logProduct = new Log_Shop_Product()
        //        {
        //            ProductId = productId,
        //            UserEdited = userEdited,
        //            PropertiesChanged = propertiesChanged,
        //            OldValue = oldValue,
        //            NewValue = newValue,
        //            DateChanged = DateTime.Now,
        //            TypeActionName = actionTable
        //        };
        //        FDIDB.Log_Shop_Product.Add(logProduct);
        //        Save();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        /// <summary>
        /// Nghiatc2.
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
            try
            {
                FDIDB.SaveChanges();

            }
            catch (Exception)
            {

            }
        }
        #endregion

    }
}
