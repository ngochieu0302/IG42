using System.Collections.Generic;
using System.Linq;
using FDI.Simple;
using System.Web;
using FDI.Base;
using FDI.Utils;
using System;
using FDI.CORE;

namespace FDI.DA.DA
{
    public class DiscountDA : BaseDA
    {
        public DiscountDA()
        {

        }
        public DiscountDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DiscountDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<DiscountItem> GetListSimple(HttpRequestBase httpRequest, int AgencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Discounts where o.AgencyID == AgencyId
                        orderby o.ID descending
                        select new DiscountItem
                        {
                            ID = o.ID,
                            DateEnd = o.DateEnd,
                            DateStart = o.DateStart,
                            IsActive = o.IsActive,
                            IsAll = o.IsAll,
                            Percent = o.Percent,
                            PriceE = o.PriceE,
                            PriceS = o.PriceS                           
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DiscountItem GetItemById(int id)
        {
            var query = from o in FDIDB.Discounts
                        where o.ID == id
                        select new DiscountItem
                        {
                            ID = o.ID,
                            DateEnd = o.DateEnd,
                            DateStart = o.DateStart,
                            IsActive = o.IsActive,
                            IsAll = o.IsAll,
                            Type = o.Type,
                            Percent = o.Percent,
                            PriceE = o.PriceE,
                            PriceS = o.PriceS
                        };
            return query.FirstOrDefault();
        }

        public List<DiscountItem> GetDiscountItem(int type, int agencyId)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.Discounts
                        where o.Type == type && o.AgencyID == agencyId && o.DateStart <= date && o.DateEnd >= o.DateEnd
                        orderby o.Percent descending
                        select new DiscountItem
                        {
                            ID = o.ID,
                            IsAll = o.IsAll,                       
                            Percent = o.Percent,
                            PriceE = o.PriceE,
                            PriceS = o.PriceS,
                        };
            return query.ToList();
        }

        public Discount GetById(int id)
        {
            var query = from c in FDIDB.Discounts where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<DiscountItem> GetByAllKHDiscount()
        {
            //var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.Discounts
                        //where c.DateStart < date && c.DateEnd > date
                        select new DiscountItem
                        {
                            Percent = c.Percent,
                            PriceS = c.PriceS,
                            PriceE = c.PriceE
                        };
            return query.ToList();
        }
        public List<Discount> GetListByArrId(string ltsArrID)
        {
            var arrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from c in FDIDB.Discounts where arrId.Contains(c.ID) select c;
            return query.ToList();
        }
       
        public void Add(Discount item)
        {
            FDIDB.Discounts.Add(item);
        }
    }
}
