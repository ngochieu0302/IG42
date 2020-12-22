using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class CustomerGroupDA : BaseDA
    {
        #region Constructer
        public CustomerGroupDA()
        {

        }

        public CustomerGroupDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerGroupDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CustomerGroupItem> GetList(int agencyId)
        {
            var query = from c in FDIDB.Customer_Groups
                        where c.IsDelete == false && c.AgencyId == agencyId
                        select new CustomerGroupItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name,
                                       Description = c.Description,
                                       Discount = c.Discount
                                   };
            return query.ToList();
        }

        public List<CustomerGroupItem> GetListRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customer_Groups
                        where c.IsDelete == false && c.AgencyId == agencyId
                        orderby c.ID descending
                        select new CustomerGroupItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name,
                                       Description = c.Description,
                                       Discount = c.Discount,
                                       TotalPrice = c.TotalPrice,
                                       Level = c.Level
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public CustomerGroupItem GetCustomerGroupItem(int id)
        {
            var query = from c in FDIDB.Customer_Groups
                        where c.IsDelete == false && c.ID == id
                        select new CustomerGroupItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            Discount = c.Discount,
                            TotalPrice = c.TotalPrice,
                            Level = c.Level
                        };
            return query.FirstOrDefault();
        }
        public List<Customer_Groups> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Customer_Groups where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public Customer GetCustomerId(int id)
        {
            var query = from c in FDIDB.Customers where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public Customer_Groups GetById(int id)
        {
            var query = from c in FDIDB.Customer_Groups where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public void Add(Customer_Groups item)
        {
            FDIDB.Customer_Groups.Add(item);
        }
    }
}
