using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ImportProductDA : BaseDA
    {
        #region Constructer
        public ImportProductDA()
        {
        }

        public ImportProductDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ImportProductDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<ImportProductItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int Agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_ImportProduct
                        where o.IsDelete == false
                        orderby o.Date descending
                        select new ImportProductItem
                        {
                            GID = o.GID,
                            //ProductID = o.ProductID,
                            Name = o.Product_Value.Shop_Product_Detail.Name,
                            //ColorName = o.Shop_Product.System_Color.Value,
                            //SizeName = o.Shop_Product.Product_Size.Name,
                            Quantity = o.Quantity,
                            Price = o.Price
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_ImportProduct GetById(Guid id)
        {
            var query = from c in FDIDB.DN_ImportProduct where c.GID == id select c;
            return query.FirstOrDefault();
        }

        public ImportProductItem GetItemById(Guid id)
        {
            var query = from o in FDIDB.DN_ImportProduct
                        where o.GID == id
                        select new ImportProductItem
                        {
                            GID = o.GID,
                            //ProductID = o.ProductID,
                            Name = o.Product_Value.Shop_Product_Detail.Name,
                            Quantity = o.Quantity,
                            Price = o.Price,
                            BarCode = o.BarCode,
                            PriceNew = o.PriceNew,
                            Value = o.Value
                        };
            return query.FirstOrDefault();
        }

        public List<ImportProductItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_ImportProduct
                        where  o.IsDelete == false
                        orderby o.Product_Value.Shop_Product_Detail.Name descending
                        select new ImportProductItem
                        {
                            GID = o.GID,
                            Name = o.Product_Value.Shop_Product_Detail.Name,
                            Quantity = o.Quantity,
                            Price = o.Price,
                            BarCode = o.BarCode,
                            PriceNew = o.PriceNew,
                            Value = o.Value
                        };
            return query.ToList();
        }


        public List<ImportProductItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListGuid(lstId);
            var query = from o in FDIDB.DN_ImportProduct
                        where o.IsDelete == false && ltsArrId.Contains(o.GID)
                        select new ImportProductItem
                        {
                            GID = o.GID,
                            //ProductID = o.ProductID,
                            Quantity = o.Quantity,
                            Price = o.Price
                        };
            return query.ToList();
        }

        public void Add(DN_ImportProduct importProduct)
        {
            FDIDB.DN_ImportProduct.Add(importProduct);
        }

        public void Delete(DN_ImportProduct importProduct)
        {
            FDIDB.DN_ImportProduct.Remove(importProduct);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
