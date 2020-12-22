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
    public class CustomerQuestionDA:BaseDA
    {
        #region Constructer
        public CustomerQuestionDA()
        {
        }

        public CustomerQuestionDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerQuestionDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CustomerQuestionItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customer_question
                        where c.AgencyID == agencyId && c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerQuestionItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            AgencyID = c.AgencyID,
                            NamePoint = c.Customer_Point.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public CustomerQuestionItem GetCustomerQuestionItem(int id)
        {
            var query = from c in FDIDB.Customer_question
                        where c.ID == id && c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerQuestionItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            AgencyID = c.AgencyID,
                            NamePoint = c.Customer_Point.Name
                        };
            return query.FirstOrDefault();
        }
        public List<Customer_question> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Customer_question where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public Customer_question GetById(int customerId)
        {
            var query = from c in FDIDB.Customer_question where c.ID == customerId select c;
            return query.FirstOrDefault();
        }
        public void Add(Customer_question item)
        {
            FDIDB.Customer_question.Add(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
