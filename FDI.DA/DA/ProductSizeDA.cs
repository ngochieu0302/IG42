using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class ProductSizeDA : BaseDA
    {
        #region Constructer
        public ProductSizeDA()
        {
        }

        public ProductSizeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ProductSizeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<ProductSizeItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int Agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Product_Size
                        orderby o.ID descending
                        select new ProductSizeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Value = o.Value
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public Product_Size GetById(int id)
        {
            var query = from c in FDIDB.Product_Size where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public ProductSizeItem GetItemById(int id)
        {
            var query = from o in FDIDB.Product_Size
                        where o.ID == id
                        select new ProductSizeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Value = o.Value
                        };
            return query.FirstOrDefault();
        }

        public List<ProductSizeItem> GetAll(int agencyId)
        {
            var query = from o in FDIDB.Product_Size
                        select new ProductSizeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Value = o.Value
                        };
            return query.ToList();
        }
        public List<ProductSizeItem> GetAllByUnitID(int agencyId, int? unitID)
        {
            var query = from o in FDIDB.Product_Size
                        where o.UnitID == unitID
                        select new ProductSizeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Value = o.Value
                        };
            return query.ToList();
        }

        public List<Product_Size> ListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Product_Size
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }
        public List<ProductSizeItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Product_Size
                        where ltsArrId.Contains(o.ID)
                        select new ProductSizeItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Value = o.Value
                        };
            return query.ToList();
        }

        public void Add(Product_Size productSize)
        {
            FDIDB.Product_Size.Add(productSize);
        }

        public void Delete(Product_Size productSize)
        {
            FDIDB.Product_Size.Remove(productSize);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
