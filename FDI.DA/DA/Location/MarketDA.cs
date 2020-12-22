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

namespace FDI.DA.DA
{
    public class MarketDA : BaseDA
    {
        public MarketDA()
        {
        }

        public MarketDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public MarketDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<MarketItem> GetListbyRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Markets
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new MarketItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            WardsID = c.WardsID,
                            AreaID = c.AreaID
                        };
            var ward = httpRequest["WardsID"];
            if (!string.IsNullOrEmpty(ward))
            {
                var wId = int.Parse(ward);
                query = query.Where(c => c.WardsID == wId);
            }
            var area = httpRequest["AreaID"];
            if (!string.IsNullOrEmpty(ward))
            {
                var aId = int.Parse(area);
                query = query.Where(c => c.AreaID == aId);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<MarketItem> GetListStaticbyRequest(HttpRequestBase httpRequest, int areaId)
        {
            var daten = DateTime.Today.TotalSeconds();
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Markets
                        where c.IsDeleted != true && c.IsShow == true
                        && c.AreaID == areaId
                        && c.DN_RequestWare.Any(a => a.MarketID == c.ID && a.Today > daten)
                        orderby c.Name
                        select new MarketItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            WardsID = c.WardsID,
                            AreaID = c.AreaID,
                            Areaname = c.Area.Name,
                            Address =c.Address,
                            ListDnRequestWareHouseItems = c.DN_RequestWare.Where(a => a.IsDelete != true && a.Today > daten).Select(v => new DNRequestWareHouseItem
                            {
                                QuantityActive = v.QuantityActive,
                                ProductName = v.Category.Name,
                                Agencyname = v.DN_Agency.Name,
                                Mobile = v.DN_Agency.Phone,
                                Today = v.Today,
                                Hours = v.Hour,
                            }),
                            ListDnRequestWareHouseGroupItems = c.DN_RequestWare.GroupBy(g => g.CateID).Where(a => a.Select(h => h.IsDelete).FirstOrDefault() != true && a.Select(h => h.Today).FirstOrDefault() > daten).Select(z => new DNRequestWareHouseActiveItem
                            {
                                ProductName = z.Select(d=>d.Category.Name).FirstOrDefault(),
                                Quantity = z.Sum(d => d.QuantityActive)
                            })
                        };
            var ward = httpRequest["WardsID"];
            if (!string.IsNullOrEmpty(ward))
            {
                var wId = int.Parse(ward);
                query = query.Where(c => c.WardsID == wId);
            }
            var area = httpRequest["AreaID"];
            if (!string.IsNullOrEmpty(ward))
            {
                var aId = int.Parse(area);
                query = query.Where(c => c.AreaID == aId);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public MarketItem GetItemById(int id)
        {
            var daten = DateTime.Today.TotalSeconds();
            var query = from c in FDIDB.Markets
                        where c.IsDeleted != true && c.IsShow == true
                        && c.ID == id
                        orderby c.Name
                        select new MarketItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim(),
                            Coordinates = c.Coordinates,
                            WardsID = c.WardsID,
                            AreaID = c.AreaID,
                            Address = c.Address,
                            Areaname = c.Area.Name,
                            ListDnRequestWareHouseItems = c.DN_RequestWare.Where(a => a.IsDelete != true && a.Today > daten).Select(v => new DNRequestWareHouseItem
                            {
                                QuantityActive = v.QuantityActive,
                                ProductName = v.Category.Name,
                                Agencyname = v.DN_Agency.Name,
                                Mobile = v.DN_Agency.Phone,
                                Today = v.Today,
                                Hours = v.Hour,
                            }),
                            ListDnRequestWareHouseGroupItems = c.DN_RequestWare.Where(a => a.IsDelete != true && a.Today > daten).GroupBy(g => g.CateID).Select(z => new DNRequestWareHouseActiveItem
                            {
                                ProductName = z.Select(d => d.Category.Name).FirstOrDefault(),
                                Quantity = z.Sum(d => d.QuantityActive)
                            })
                        };
            return query.FirstOrDefault();
        }
        public List<MarketItem> GetListSimple()
        {
            var query = from c in FDIDB.Markets
                        where c.IsDeleted != true && c.IsShow == true
                        orderby c.Name
                        select new MarketItem
                        {
                            ID = c.ID,
                            Name = c.Name.Trim()
                        };
            return query.ToList();
        }

        public Market GetById(int id)
        {
            var query = from c in FDIDB.Markets
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public List<Market> GetByListArrId(string lstInt)
        {
            var lst = FDIUtils.StringToListInt(lstInt);
            var query = from c in FDIDB.Markets
                        where lst.Contains(c.ID)
                        select c;
            return query.ToList();
        }
        public void Add(Market market)
        {
            FDIDB.Markets.Add(market);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
