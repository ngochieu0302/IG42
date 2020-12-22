using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNExchangeDA : BaseDA
    {
        #region Constructer
        public DNExchangeDA()
        {
        }

        public DNExchangeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNExchangeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNExchangeItem> GetListByNow(int agencyid)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Exchange
                        where c.StartDate <= date && c.EndDate >= date && c.AgencyID == agencyid
                        orderby c.StartDate 
                        select new DNExchangeItem
                        {
                            BedDeskID = c.BedDeskID,
                            BedDeskExID = c.BedDeskExID,
                            EndDate = c.EndDate,
                            StartDate = c.StartDate,
                        };
            return query.ToList();
        }

        public int GetBedIDByName(string name, int agencyid)
        {
            var query = from c in FDIDB.DN_Bed_Desk
                where c.Name.ToLower() == name.ToLower() && c.AgencyId == agencyid
                select c.ID;
            return query.FirstOrDefault();
        }

        public void Add(DN_Exchange exchange)
        {
            FDIDB.DN_Exchange.Add(exchange);
        }

        public void Delete(DN_Exchange exchange)
        {
            FDIDB.DN_Exchange.Remove(exchange);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
