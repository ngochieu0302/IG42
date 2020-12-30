using FDI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.DA.DA.AppIG4
{
    public class CustomerAppSMSDA : BaseDA
    {
        #region Constructer
        public CustomerAppSMSDA()
        {
        }

        public CustomerAppSMSDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerAppSMSDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public void Add(SM customer)
        {
            FDIDB.SMS.Add(customer);
        }

        public void Delete(SM customer)
        {
            customer.IsDelete = true;
            FDIDB.SMS.Attach(customer);
            var entry = FDIDB.Entry(customer);
            entry.Property(e => e.IsDelete).IsModified = true;
            // DB.Customers.Remove(customer);
        }
    }
}
