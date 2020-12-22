using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class SendCardDA : BaseDA
    {
        #region Constructer
        public SendCardDA()
        {
        }

        public SendCardDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public SendCardDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<SendCardItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.Send_Card
                        orderby c.ID descending
                        select new SendCardItem
                        {
                            ID = c.ID,
                            CustomerID = c.CustomerID,
                            DateCreate = c.DateCreate
                        };
            return query.ToList();
        }

        public List<SendCardItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Send_Card
                        orderby c.ID
                        select new SendCardItem
                        {
                            ID = c.ID,
                            CustomerID = c.CustomerID,
                            CustomerName = c.Customer.FullName,
                            DateCreate = c.DateCreate
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public Send_Card GetById(int id)
        {
            var query = from c in FDIDB.Send_Card where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<Send_Card> GetById(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Send_Card where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public void Add(Send_Card item)
        {
            FDIDB.Send_Card.Add(item);
        }
        public void Delete(Send_Card item)
        {
            FDIDB.Send_Card.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
