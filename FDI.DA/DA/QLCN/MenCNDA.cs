using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple.QLCN;

namespace FDI.DA.DA.QLCN
{
   public class MenCNDA:BaseDA
    {
        #region Contruction
       public MenCNDA()
        {
        }
        public MenCNDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public MenCNDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
       #endregion
        public List<MenCNItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Men_CN
                        orderby o.ID descending
                        select new MenCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Unitname = o.DN_Unit.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<MenCNItem> GetList()
        {
            var query = from o in FDIDB.Men_CN
                        orderby o.ID descending
                        select new MenCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            return query.ToList();
        }
        public MenCNItem GetMenCNItem(int id)
        {
            var query = from o in FDIDB.Men_CN
                        where o.ID == id
                        orderby o.ID descending
                        select new MenCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Unitname = o.DN_Unit.Name,
                            UnitID = o.UnitID,
                            LstNguyenlieuCnItems = o.Men_Nguyenlieu_CN.Where(a=>a.IsDeleted == false).Select(v=> new MenNguyenlieuCNItem
                            {
                                IdNguyenlieu = v.IdNguyenlieu,
                                Quantity = v.Quantity,
                                Nguyenlieu = v.NguyenLieu_CN.Name,
                            })
                        };
            return query.FirstOrDefault();
        }
        public Men_CN GetById(int id)
        {
            var query = from o in FDIDB.Men_CN where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<Men_CN> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.Men_CN where lst.Contains(o.ID) select o;
            return query.ToList();
        }

        public void Add(Men_CN item)
        {
            FDIDB.Men_CN.Add(item);
        }
        public void Delete(Men_CN item)
        {
            FDIDB.Men_CN.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
