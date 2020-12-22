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
    public class CateCNDA:BaseDA
    {
        #region Contruction
       public CateCNDA()
        {
        }
        public CateCNDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public CateCNDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
       #endregion
        public List<CateCNItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Cate_CN
                        orderby o.ID descending
                        select new CateCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CateCNItem> GetList()
        {
            var query = from o in FDIDB.Cate_CN
                        orderby o.ID descending
                        select new CateCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            return query.ToList();
        }
        public CateCNItem GetCateCNItem(int id)
        {
            var query = from o in FDIDB.Cate_CN
                        where o.ID == id
                        orderby o.ID descending
                        select new CateCNItem
                        {
                            ID = o.ID,
                            Name = o.Name
                        };
            return query.FirstOrDefault();
        }
        public Cate_CN GetById(int id)
        {
            var query = from o in FDIDB.Cate_CN where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<Cate_CN> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.Cate_CN where lst.Contains(o.ID) select o;
            return query.ToList();
        }


        public void Add(Cate_CN item)
        {
            FDIDB.Cate_CN.Add(item);
        }
        public void Delete(Cate_CN item)
        {
            FDIDB.Cate_CN.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
