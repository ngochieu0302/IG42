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
    public class CustomerReviewDA : BaseDA
    {
        #region Constructer
        public CustomerReviewDA()
        {
        }

        public CustomerReviewDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerReviewDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CustomerReviewItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Customer_Review
                        where c.AgencyID == agencyId && c.IsDelete == false
                        orderby c.ID descending
                        select new CustomerReviewItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            Note = c.Note,
                            DateCreate = c.DateCreate,
                            QuestionID = c.QuestionID,
                            PointID = c.PointID,
                            Rep = c.Rep,
                            PointName = c.Customer_Point.Name,
                            RepNote = c.RepNote,
                            Username = c.Shop_Orders.DN_Users2.LoweredUserName,
                            AgencyID = c.AgencyID
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public CustomerReviewItem GetCustomerReviewItem(int id)
        {
            var query = from c in FDIDB.Customer_Review
                        where c.ID == id
                        select new CustomerReviewItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            Note = c.Note,
                            PointName = c.Customer_Point.Name,
                            DateCreate = c.DateCreate,
                            Username = c.Shop_Orders.DN_Users2.LoweredUserName,
                            AgencyID = c.AgencyID,
                            QuestionID = c.QuestionID,
                            PointID = c.PointID,
                            Rep = c.Rep,
                            RepNote = c.RepNote,
                            CustomerReviewDeltails = c.Customer_Review_Deltails.Where(a => a.IsDelete == false).Select(z => new CustomerReviewDetailsItem
                            {
                                Point = z.Customer_Point.Point,
                                Question = z.Customer_question.Name,
                                Rep = z.Rep,
                                Note = z.Note
                            })
                        };

            return query.FirstOrDefault();
        }
        public List<Customer_Review> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Customer_Review where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public Customer_Review GetById(int customerId)
        {
            var query = from c in FDIDB.Customer_Review where c.ID == customerId select c;
            return query.FirstOrDefault();
        }
        public void Add(Customer_Review item)
        {
            FDIDB.Customer_Review.Add(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
