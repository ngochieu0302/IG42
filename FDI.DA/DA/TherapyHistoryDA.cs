using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA.DA
{
    public class TherapyHistoryDA:BaseDA
    {
        #region Constructer
        public TherapyHistoryDA()
        {
        }

        public TherapyHistoryDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public TherapyHistoryDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<TherapyHistoryItem> GetListbyCustomerID( int cusId,string phone)
        {
            var query = from o in FDIDB.Therapy_History
                        where o.CustomerID == cusId || o.Phone == phone && o.IsDelete == false
                        orderby o.ID descending
                        select new TherapyHistoryItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Address = o.Address,
                            Phone = o.Phone,
                            IsDelete = o.IsDelete,
                            DateCreate = o.DateCreate,
                            Note = o.Note,
                            Gender = o.Gender,
                            CustomerID = o.CustomerID,
                            IsShow = o.IsShow
                        };
            return query.Take(5).ToList();
        }

        public void Add(Therapy_History item)
        {
            FDIDB.Therapy_History.Add(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
