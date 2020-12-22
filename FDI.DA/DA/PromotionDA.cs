using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class PromotionDA : BaseDA
    {
        public PromotionDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public PromotionDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<PromotionItem> GetList()
        {
            var query = from c in FDIDB.Promotions
                        where c.IsDelete == false
                        select new PromotionItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            Quantity = c.Quantity,
                            UserCreated = c.UserCreated,
                            Content = c.Content,
                            ShopId = c.ShopId,
                            Type = c.Type,
                            FromPrice = c.FromPrice,
                            ToPrice = c.ToPrice,
                            Mode = c.Mode,
                            Sale = c.Sale
                        };
            return query.ToList();
        }

        public List<PromotionItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Promotions
                        where c.IsDelete == false
                        orderby c.ID descending 
                        select new PromotionItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            Quantity = c.Quantity,
                            UserCreated = c.UserCreated,
                            Content = c.Content,
                            ShopId = c.ShopId,
                            Type = c.Type,
                            FromPrice = c.FromPrice,
                            ToPrice = c.ToPrice,
                            Mode = c.Mode,
                            Sale = c.Sale,
                            Status = c.StartDate < DateTime.Now && c.EndDate > DateTime.Now
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.OrderByDescending(c => c.ID).ToList();
        }

        public Promotion GetById(int id)
        {
            var query = from c in FDIDB.Promotions where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DiscountCodeItem GetByCode(string shopid, string code)
        {
            var query = from c in FDIDB.DiscountCodes 
                        where c.Promotion.ShopId.Contains(shopid) && c.Code == code && c.IsComplete == false 
                        && c.Promotion.StartDate <= DateTime.Now && c.Promotion.EndDate >= DateTime .Now && c.Promotion.IsDelete == false
                        select new DiscountCodeItem
                        {
                            Sale = c.Promotion.Sale,
                        };
            return query.FirstOrDefault();
        }

        public Promotion GetByType(int type)
        {
            var query = from c in FDIDB.Promotions where c.ID == type select c;
            return query.FirstOrDefault();
        }

        public List<Promotion> GetListByArrId(List<int> lst)
        {
            var query = from c in FDIDB.Promotions where lst.Contains(c.ID) select c;
            return query.ToList();
        }

        #region Change

        public void Add(Promotion item)
        {
            FDIDB.Promotions.Add(item);
        }

        public void Delete(Promotion item)
        {
            FDIDB.Promotions.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

        #endregion
    }
}