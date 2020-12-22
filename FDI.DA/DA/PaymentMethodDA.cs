using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
   public class PaymentMethodDA:BaseDA
    {
        #region Constructer
        public PaymentMethodDA()
        {
        }

        public PaymentMethodDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public PaymentMethodDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<PaymentMethodItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Payment_Method
                        where o.IsShow == true && (!o.IsDeleted.HasValue || o.IsDeleted == false)
                        orderby o.ID descending
                        select new PaymentMethodItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public Payment_Method GetById(int id)
        {
            var query = from c in FDIDB.Payment_Method where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public PaymentMethodItem GetItemById(int id)
        {
            var query = from o in FDIDB.Payment_Method
                        where o.ID == id
                        select new PaymentMethodItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description
                        };
            return query.FirstOrDefault();
        }
        public List<PaymentMethodItem> GetAll()
        {
            var query = from o in FDIDB.Payment_Method
                        where o.IsShow == true && (!o.IsDeleted.HasValue || o.IsDeleted == false)
                        select new PaymentMethodItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Description = o.Description
                        };
            return query.ToList();
        }
        public List<Payment_Method> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Payment_Method
                where ltsArrId.Contains(o.ID)
                select o;
            return query.ToList();
        }
        public void Add(Payment_Method productSize)
        {
            FDIDB.Payment_Method.Add(productSize);
        }
       
        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
