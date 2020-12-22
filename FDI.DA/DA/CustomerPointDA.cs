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
    public class CustomerPointDA:BaseDA
    {
        #region Constructer
        public CustomerPointDA()
        {
        }

        public CustomerPointDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerPointDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CustomerPointItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customer_Point
                        where c.AgencyID == agencyId && c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerPointItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            AgencyID = c.AgencyID,
                            Point = c.Point
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public CustomerPointItem GetCustomerPointItem(int id)
        {
            var query = from c in FDIDB.Customer_Point
                        where c.ID == id && c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerPointItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            AgencyID = c.AgencyID,
                            Point = c.Point
                        };
            return query.FirstOrDefault();
        }
        public List<Customer_Point> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Customer_Point where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public Customer_Point GetById(int customerId)
        {
            var query = from c in FDIDB.Customer_Point where c.ID == customerId select c;
            return query.FirstOrDefault();
        }
        public void Add(Customer_Point item)
        {
            FDIDB.Customer_Point.Add(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
