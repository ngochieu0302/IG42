using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class Advertising_PositionDA : BaseDA
    {
        #region Constructer
        public Advertising_PositionDA()
        {
        }

        public Advertising_PositionDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public Advertising_PositionDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<AdvertisingPositionItem> GetAllListSimple(int agencyId)
        {
            var query = from c in FDIDB.Advertising_Position
                        where !c.IsDeleted.Value && c.AgencyID == agencyId
                        orderby c.Name
                        select new AdvertisingPositionItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name
                                   };
            return query.ToList();
        }

        public AdvertisingPositionItem GetItemById(int id)
        {
            var query = from c in FDIDB.Advertising_Position
                        where !c.IsDeleted.Value && c.ID == id
                        orderby c.Name
                        select new AdvertisingPositionItem
                        {
                            ID = c.ID,
                            Name = c.Name

                        };
            return query.FirstOrDefault();
        }
        
        public List<AdvertisingPositionItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Advertising_Position
                        orderby c.Name
                        where c.Name.StartsWith(keyword) && !c.IsDeleted.Value
                        select new AdvertisingPositionItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name
                                   };
            return query.Take(showLimit).ToList();
        }

        public List<AdvertisingPositionItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Advertising_Position
                        orderby o.ID descending 
                        where o.IsDeleted == false && o.AgencyID == agencyId
                        select new AdvertisingPositionItem
                        {
                            ID = o.ID,
                            Name = o.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        
        public Advertising_Position GetById(int positionId)
        {
            var query = from c in FDIDB.Advertising_Position where c.ID == positionId select c;
            return query.FirstOrDefault();
        }

        public List<Advertising_Position> GetListByArrId(List<int> ltArrID)
        {
            var query = from c in FDIDB.Advertising_Position where ltArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(Advertising_Position template)
        {
            FDIDB.Advertising_Position.Add(template);
        }

        public void Delete(Advertising_Position template)
        {
            FDIDB.Advertising_Position.Remove(template);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
