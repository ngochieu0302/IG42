using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Simple.QLCN;

namespace FDI.DA.DA.QLCN
{
   public class NguyenlieuCNDA:BaseDA
   {
       #region Contruction
       public NguyenlieuCNDA()
        {
        }
        public NguyenlieuCNDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public NguyenlieuCNDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
       #endregion
        public List<NguyenlieuCNItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.NguyenLieu_CN
                        orderby o.ID descending
                        select new NguyenlieuCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Price = o.Price,
                            Unitname = o.DN_Unit.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<NguyenlieuCNItem> GetList()
        {
            var query = from o in FDIDB.NguyenLieu_CN
                        orderby o.ID descending
                        select new NguyenlieuCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            return query.ToList();
        }
        public NguyenlieuCNItem GetNguyenlieuCNItem(int id)
        {
            var query = from o in FDIDB.NguyenLieu_CN
                        where o.ID == id
                        orderby o.ID descending
                        select new NguyenlieuCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Price = o.Price,
                            Unitname = o.DN_Unit.Name,
                            UnitID = o.UnitID,
                        };
            return query.FirstOrDefault();
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit)
        {
            var query = from c in FDIDB.NguyenLieu_CN
                        where c.Name.Contains(keword)  && c.IsDeleted == false
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            value = c.Name,
                            title = c.Name,
                            
                        };
            return query.Take(showLimit).ToList();
        }
        public NguyenLieu_CN GetById(int id)
        {
            var query = from o in FDIDB.NguyenLieu_CN where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<NguyenLieu_CN> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.NguyenLieu_CN where lst.Contains(o.ID) select o;
            return query.ToList();
        }


        public void Add(NguyenLieu_CN item)
        {
            FDIDB.NguyenLieu_CN.Add(item);
        }
        public void Delete(NguyenLieu_CN item)
        {
            FDIDB.NguyenLieu_CN.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
   }
}
