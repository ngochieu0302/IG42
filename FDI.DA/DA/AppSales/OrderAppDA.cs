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

namespace FDI.DA
{
    public class OrderAppDA : BaseDA
    {
        #region Constructer
        public OrderAppDA()
        {
        }

        public OrderAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public OrderAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CategoryAppItem> ListByParentID(int cateid = 2)
        {
            var query = from c in FDIDB.Categories
                        where c.ParentId == cateid
                        select new CategoryAppItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Price = c.Price
                        };
            return query.ToList();
        }

        public List<CategoryAppItem> ListByListID(List<int?> listid)
        {
            var query = from c in FDIDB.Categories
                        where listid.Contains(c.Id)
                        select new CategoryAppItem
                        {
                            ID = c.Id,
                            Price = c.Price
                        };
            return query.ToList();
        }
        public AgencyItem GetbyID(int id)
        {
            var date = ConvertDate.TotalSeconds(DateTime.Now);
            var query = from c in FDIDB.DN_Agency
                        where c.ID == id
                        //&& c.MarketID.HasValue && c.MarketID > 0
                        select new AgencyItem
                        {
                            MarketID = c.MarketID,
                            AreaID = c.Market.AreaID,
                            TotalDeposit = c.WalletValue
                        };
            return query.FirstOrDefault();
        }
        #region UserLogin
        public List<StorageWarehousingItem> GetStorageByUser(int agencyId, int page, int rowPerPage, int status, decimal startDate, decimal endDate, bool? orderbyPrice, ref int total)
        {
            //var date = DateTime.Now.AddHours(12).TotalSeconds();
            var query = from o in FDIDB.StorageWarehousings
                        where o.AgencyId == agencyId && (o.IsDelete == null || o.IsDelete.Value == false)
                         && o.DN_RequestWare.Any()
                         && o.DN_RequestWare.Sum(m => m.TotalPrice)>0
                        orderby o.ID descending
                        select new StorageWarehousingItem
                        {
                            ID = o.ID,
                            Status = o.Status,
                            Code = o.Code,
                            DateRecive = o.DateRecive ?? 0,
                            TotalPrice = o.DN_RequestWare.Sum(m=>m.TotalPrice)

                        };
            query = query.Where(m =>
                (m.Status == status || status == 0)
                && m.DateRecive >= startDate && m.DateRecive <= endDate
            );

            query = query.Paging(page, rowPerPage, ref total);


            return query.ToList();
        }
        public SWItem GetStorageByID(int agencyId, int id)
        {
            var date = ConvertDate.TotalSeconds(DateTime.Now.AddHours(12));
            var query = from o in FDIDB.StorageWarehousings
                        where o.AgencyId == agencyId && (!o.IsDelete.HasValue || !o.IsDelete.Value) && o.ID == id
                        select new SWItem
                        {
                            id = o.ID,
                            n = o.Name,
                            c = o.Code,
                            d = o.DateRecive ?? 0,
                            LstItem = o.DN_RequestWare.Select(m => new DNRWHItem
                            {
                                n = m.Category.Name,
                                h = m.Hour ?? 0,
                                c = m.CateID,
                                q = m.Quantity ?? 0,
                                s = date < m.DateEnd,
                                t = m.Type ?? 0
                            })
                        };
            var obj = query.FirstOrDefault() ?? new SWItem { d = ConvertDate.TotalSeconds(DateTime.Today.AddDays(1)) };
            obj.LstTimes = TypeTime.Hours(obj.d, date);
            return obj;
        }
        public StorageWarehousing GetObjByID(int agencyId, int id)
        {
            var date = ConvertDate.TotalSeconds(DateTime.Now.AddHours(12));
            var query = from o in FDIDB.StorageWarehousings
                        where o.AgencyId == agencyId && (!o.IsDelete.HasValue || !o.IsDelete.Value) && o.ID == id && o.DN_RequestWare.Any(m => date < m.DateEnd)
                        select o;
            return query.FirstOrDefault();
        }
        public List<SWItem> ListStorageByUser(int rowPerPage, int page, int agencyId, int status, ref int total)
        {
            var date = ConvertDate.TotalSeconds(DateTime.Now.AddHours(12));
            var query = from o in FDIDB.StorageWarehousings
                        where o.AgencyId == agencyId && (!o.IsDelete.HasValue || !o.IsDelete.Value)
                        && o.Status == status
                        orderby o.ID descending
                        select new SWItem
                        {
                            id = o.ID,
                            n = o.Name,
                            c = o.Code,
                            d = o.DateRecive ?? 0,
                            s = o.DN_RequestWare.Any(m => m.DateEnd > date)
                        };
            return query.Paging(page, rowPerPage, ref total).ToList();
        }
        #endregion
        public void Add(StorageWarehousing item)
        {
            FDIDB.StorageWarehousings.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
