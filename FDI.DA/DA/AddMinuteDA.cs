using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class AddMinuteDA : BaseDA
    {
        #region Constructer
        public AddMinuteDA()
        {
        }

        public AddMinuteDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public AddMinuteDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion


        public AddMinuteItem GetItemById(int id)
        {
            var query = from c in FDIDB.AddMinutes
                        orderby c.ID descending
                        where (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && c.ID == id
                        select new AddMinuteItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Minute = c.Minute,
                            Price = c.Price??0,
                            IsShow = c.IsShow,
                            Type = c.Type,
                            DateCreated = c.DateCreated
                        };
            return query.FirstOrDefault();
        }
        
        public List<AddMinuteItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.AddMinutes
                        orderby c.ID
                        where (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                        select new AddMinuteItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Minute = c.Minute,
                            Price = c.Price ?? 0,
                            IsShow = c.IsShow,
                            Type = c.Type,
                            DateCreated = c.DateCreated
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<AddMinuteItem> ListAllItems()
        {
            var query = from c in FDIDB.AddMinutes
                        orderby c.ID
                        where (!c.IsDeleted.HasValue || !c.IsDeleted.Value)&&
                        (c.IsShow.HasValue && c.IsShow.Value)
                        select new AddMinuteItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Minute = c.Minute,
                            Price = c.Price ?? 0,
                            Type = c.Type
                        };
            return query.ToList();
        }
        public AddMinute GetById(int advertisingId)
        {
            var query = from c in FDIDB.AddMinutes where c.ID == advertisingId select c;
            return query.FirstOrDefault();
        }

        public List<AddMinute> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.AddMinutes where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(AddMinute items)
        {
            FDIDB.AddMinutes.Add(items);
        }

        public void Delete(AddMinute items)
        {
            FDIDB.AddMinutes.Remove(items);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
