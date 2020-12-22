using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA.StorageWarehouse
{
    public class TotalStorageWareDA : BaseDA
    {
        #region Contruction
        public TotalStorageWareDA()
        {
        }
        public TotalStorageWareDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public TotalStorageWareDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<TotalStorageWareItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int area)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var query = from o in FDIDB.TotalStorageWares
                        where o.AreaID == area

                        orderby o.ID descending
                        select new TotalStorageWareItem()
                        {
                            ID = o.ID,
                            Catename = o.Category.Name,
                            Cateprice = o.Category.Price,
                            Hour = o.Hour,
                            QuantityOut = o.QuantityOut,
                            Today = o.Today,
                            Quantity = o.Quantity
                        };
            if (!string.IsNullOrEmpty(from))
            {
                var fromDate = ConvertUtil.ToDateTime(from).TotalSeconds();
                query = query.Where(c => c.Today == fromDate);
            }
            var hours = httpRequest["hours"];
            if (!string.IsNullOrEmpty(hours))
            {
                var h = int.Parse(hours);
                query = query.Where(c => c.Hour == h);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public TotalStorageWareItem GetTotalStorageWareItem(int id)
        {
            var query = from c in FDIDB.TotalStorageWares
                        where c.ID == id
                        select new TotalStorageWareItem
                        {
                            ID = c.ID,
                            Catename = c.Category.Name,
                            Quantity = c.Quantity,
                            Cateprice = c.Category.Price,
                            CateID = c.CateID,
                            QuantityOut = c.QuantityOut,
                            Today = c.Today,
                            Hour = c.Hour,
                            StorageProducts = c.StorageProducts.Where(a => a.IsDelete != true).Select(v => new StorageProductItem
                            {
                                ID = v.ID,
                                TotalID = v.TotalID,
                                Quantity = v.Quantity,
                                Hour = v.Hour,
                                HourImport = v.HoursImport,
                                CateID = v.CateID,
                                DateImport = v.DateImport,
                                SupID = v.SupID,
                                Price = v.Price,
                            })
                        };
            return query.FirstOrDefault();
        }

        public TotalStorageWare GetbyId(int id)
        {
            var query = from c in FDIDB.TotalStorageWares
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public List<StorageProduct> GetListStorabyId(int id)
        {
            var query = from c in FDIDB.StorageProducts
                        where c.TotalID == id
                        select c;
            return query.ToList();
        }
        public void AddStora(StorageProduct item)
        {
            FDIDB.StorageProducts.Add(item);
        }
        public void DeleteStora(StorageProduct item)
        {
            FDIDB.StorageProducts.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
