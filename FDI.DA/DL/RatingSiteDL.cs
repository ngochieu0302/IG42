using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DL
{
    public class RatingSiteDL : BaseDA
    {
        public List<RatingSiteItem> GetListHome()
        {
            var query = from c in FDIDB.RatingSites
                        where c.NewsID == null && c.ProductId == null
                        orderby c.ID 
                        select new RatingSiteItem
                        {
                            ID = c.ID,
                            Count = c.Count,
                            Star = c.Star,
                            NewId = c.NewsID,
                            ProductId = c.ProductId
                        };
            return query.ToList();
        }
        public List<RatingSiteItem> GetListbyNewsId(int newsId)
        {
            var query = from c in FDIDB.RatingSites
                        where c.NewsID == newsId && c.ProductId == null
                        orderby c.ID
                        select new RatingSiteItem
                        {
                            ID = c.ID,
                            Count = c.Count,
                            Star = c.Star,
                            NewId = c.NewsID,
                            ProductId = c.ProductId
                        };
            return query.ToList();
        }
        public List<RatingSiteItem> GetListbyProductId(int productId)
        {
            var query = from c in FDIDB.RatingSites
                        where c.NewsID == null && c.ProductId == productId
                        orderby c.ID
                        select new RatingSiteItem
                        {
                            ID = c.ID,
                            Count = c.Count,
                            Star = c.Star,
                            NewId = c.NewsID,
                            ProductId = c.ProductId
                        };
            return query.ToList();
        }
        public List<RatingSite> GetListbyProductIdBase(int productId)
        {
            var query = from c in FDIDB.RatingSites
                where c.NewsID == null && c.ProductId == productId
                orderby c.ID
                select c;
            return query.ToList();
        }
        public List<RatingSite> GetListbyNewsIdBase(int productId)
        {
            var query = from c in FDIDB.RatingSites
                        where c.NewsID == null && c.ProductId == productId
                        orderby c.ID
                        select c;
            return query.ToList();
        }
        public List<RatingSite> GetListHomeBase(int productId)
        {
            var query = from c in FDIDB.RatingSites
                        where c.NewsID == null && c.ProductId == productId
                        orderby c.ID
                        select c;
            return query.ToList();
        }

        public void Add(RatingSite item)
        {
            FDIDB.RatingSites.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
