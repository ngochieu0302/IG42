using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class CustomerPolicyAppIG4DA : BaseDA
    {
        #region Constructer
        public CustomerPolicyAppIG4DA()
        {
        }

        public CustomerPolicyAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerPolicyAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CustomerPolicyAppIG4Item> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Customer_Policy
                        where o.IsShow == true && (!o.IsDeleted.HasValue || o.IsDeleted == false)
                        orderby o.ID descending
                        select new CustomerPolicyAppIG4Item
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            IsDeleted = o.IsDeleted,
                            IsShow = o.IsShow,
                            Number = o.Number,
                            //StageName = o.Stage.Name,
                            StartMoney = o.StartMoney,
                            EndMoney = o.EndMoney,

                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CustomerPolicyAppIG4Item> GetListAll()
        {
            var query = from o in FDIDB.Customer_Policy
                        where o.IsShow == true && (!o.IsDeleted.HasValue || o.IsDeleted == false)
                        orderby o.ID descending
                        select new CustomerPolicyAppIG4Item
                        {
                            ID = o.ID,
                            StartMoney = o.StartMoney,
                            EndMoney = o.EndMoney,
                            Name = o.Name,
                        };
            return query.ToList();
        }
        public CustomerPolicyAppIG4Item GetItemById(int id)
        {
            var query = from o in FDIDB.Customer_Policy
                        where o.ID == id
                        select new CustomerPolicyAppIG4Item
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            IsDeleted = o.IsDeleted,
                            IsShow = o.IsShow,
                            //StageName = o.Stage.Name,
                            StartMoney = o.StartMoney,
                            EndMoney = o.EndMoney,

                        };
            return query.FirstOrDefault();
        }
        public CustomerPolicyAppIG4Item GetItemByPrice(decimal price)
        {
            var query = from o in FDIDB.Customer_Policy
                        where o.StartMoney <= price && o.EndMoney >= price
                        select new CustomerPolicyAppIG4Item
                        {
                            ID = o.ID,

                        };
            return query.FirstOrDefault();
        }
        public Customer_Policy GetById(int id)
        {
            var query = from c in FDIDB.Customer_Policy where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<Customer_Policy> GetListByArrId(List<int> ltsArrID)
        {
            //var arrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from c in FDIDB.Customer_Policy where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        public void Add(Customer_Policy Stage)
        {
            FDIDB.Customer_Policy.Add(Stage);
        }
        public void Delete(Customer_Policy Stages)
        {
            FDIDB.Customer_Policy.Remove(Stages);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
