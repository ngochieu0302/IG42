using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class CustomerTypeAppIG4DA : BaseDA
    {
        #region Constructer
        public CustomerTypeAppIG4DA()
        {

        }

        public CustomerTypeAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerTypeAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CustomerTypeAppIG4Item> GetAll(bool isDelete = false)
        {
            var query = from t in FDIDB.Customer_Type
                        where t.IsDelete == isDelete
                        select new CustomerTypeAppIG4Item
                        {
                            ID = t.ID,
                            Name = t.Name,
                            Price = t.Price,

                        };
            return query.ToList();
        }

        public List<CustomerTypeAppIG4Item> GetListRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from t in FDIDB.Customer_Type
                        orderby t.ID descending
                        where t.IsDelete == false
                        select new CustomerTypeAppIG4Item
                        {
                            ID = t.ID,
                            Name = t.Name,
                            TypeName = t.Customer_TypeGroup.Name,
                            IsDeleted = t.IsDelete,
                            Month = t.Month,
                            Type = t.Type,
                            Day = t.Day,
                            Price = t.Price,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<Customer_Type> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Customer_Type where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public Customer_Type GetById(int typeId)
        {
            var query = from c in FDIDB.Customer_Type where c.ID == typeId select c;
            return query.FirstOrDefault();
        }
        public List<CustomerTypeAppIG4Item> GetListCustomerTypes()
        {
            var query = (from c in FDIDB.Customer_Type
                         orderby c.Sort
                         select new CustomerTypeAppIG4Item
                         {
                             ID = c.ID,
                             Name = c.Name,
                             //IsDeleted = c.IsDelete,
                             //Month = c.Month,
                             //Type = c.Type,
                             //Day = c.Day,
                             //PictureId = c.PictureId,
                             //Price = c.Price,
                             //Border = c.Border,
                             //Color = c.Color,
                             Sort = c.Sort
                         }).ToList();
            return query.ToList();
        }

        public void Add(Customer_Type customerType)
        {
            FDIDB.Customer_Type.Add(customerType);
        }

        public void Delete(Customer_Type customerType)
        {
            FDIDB.Customer_Type.Remove(customerType);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
