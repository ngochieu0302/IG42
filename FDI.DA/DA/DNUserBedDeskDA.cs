using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNUserBedDeskDA : BaseDA
    {
        public DNUserBedDeskDA()
        {
        }
        public DNUserBedDeskDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNUserBedDeskDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        
        public DNUserBedDeskItem GetItemById(int id)
        {
            var query = from c in FDIDB.DN_User_BedDesk where c.ID == id select new DNUserBedDeskItem
            {
                ID = c.ID,
                UserID = c.UserID,
                BedDeskID = c.BedDeskID,
                MWSID = c.MWSID
            };
            return query.FirstOrDefault();
        }

        public DN_User_BedDesk GetById(int id)
        {
            var query = from c in FDIDB.DN_User_BedDesk
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }

        public List<DN_Bed_Desk> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Bed_Desk where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public void Add(DN_User_BedDesk item)
        {
            FDIDB.DN_User_BedDesk.Add(item);
        }
        public void Delete(DN_User_BedDesk item)
        {
            FDIDB.DN_User_BedDesk.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
