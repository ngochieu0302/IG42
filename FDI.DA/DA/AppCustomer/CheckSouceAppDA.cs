using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.DA.DA.AppCustomer
{
    public class CheckSouceAppDA:BaseDA
    {
        #region Contruction

        public CheckSouceAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CheckSouceAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public Storage GetById(int id)
        {
            var query = from o in FDIDB.Storages where o.ID == id && o.IsDelete == false select o;
            return query.FirstOrDefault();
        }
        public DN_ImportProduct GetByBarcode(string barcode)
        {
            var query = from o in FDIDB.DN_ImportProduct where o.BarCode == barcode && (!o.IsDelete.HasValue || !o.IsDelete.Value) && (!o.Ischeck.HasValue || !o.Ischeck.Value) select o;
            return query.FirstOrDefault();
        }
    }
}
