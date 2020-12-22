using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ProductTypeDA : BaseDA
    {
        #region Constructer
        public ProductTypeDA()
        {
        }

        public ProductTypeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ProductTypeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<ProductTypeItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Shop_Product_Type
                        orderby o.ID descending
                        select new ProductTypeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public Shop_Product_Type GetById(int id)
        {
            var query = from c in FDIDB.Shop_Product_Type where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public ProductTypeItem GetItemById(int id)
        {
            var query = from o in FDIDB.Shop_Product_Type
                        where o.ID == id
                        select new ProductTypeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                        };
            return query.FirstOrDefault();
        }

        public List<ProductTypeItem> GetAll()
        {
            var query = from o in FDIDB.Shop_Product_Type
                where o.Delete == false
                        select new ProductTypeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                        };
            return query.ToList();
        }

        public List<Shop_Product_Type> ListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Shop_Product_Type
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }
        public List<ProductTypeItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Shop_Product_Type
                        where ltsArrId.Contains(o.ID)
                        select new ProductTypeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                        };
            return query.ToList();
        }

        public void Add(Shop_Product_Type productType)
        {
            FDIDB.Shop_Product_Type.Add(productType);
        }

        public void Delete(Shop_Product_Type productType)
        {
            FDIDB.Shop_Product_Type.Remove(productType);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
