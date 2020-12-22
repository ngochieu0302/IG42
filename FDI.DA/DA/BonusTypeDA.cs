using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class BonusTypeDA : BaseDA
    {
        #region Constructer
        public BonusTypeDA()
        {
        }

        public BonusTypeDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public BonusTypeDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<BonusTypeItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.BonusTypes
                        orderby c.ID
                        select new BonusTypeItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            RootID = c.RootID,
                            Percent = c.Percent,
                            Description = c.Description,
                            PercentParent = c.PercentParent,
                            PercentRoot = c.PercentRoot,
                            DateCreate = c.DateCreate
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public BonusType GetById(int id)
        {
            var query = from c in FDIDB.BonusTypes where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public BonusTypeItem GetItemTop()
        {
            var query = from c in FDIDB.BonusTypes select new BonusTypeItem
            {
                ID = c.ID,
                Percent = c.Percent,
                PercentParent = c.PercentParent
            };
            return query.FirstOrDefault();
        }

        public List<BonusType> GetById(List<int> ltsArrId)
        {
            var query = from c in FDIDB.BonusTypes where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(BonusType item)
        {
            FDIDB.BonusTypes.Add(item);
        }

        public void Delete(BonusType item)
        {
            FDIDB.BonusTypes.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
