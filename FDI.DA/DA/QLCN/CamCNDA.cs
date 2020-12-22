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
   public class CamCNDA:BaseDA
    {
       #region Contruction
       public CamCNDA()
        {
        }
        public CamCNDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public CamCNDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
       #endregion
        public List<CamCNItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Cam_CN
                        orderby o.ID descending
                        select new CamCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Catename = o.Cate_CN.Name,
                            Price = o.Price,
                            HSD = o.HSD
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CamCNItem> GetList()
        {
            var query = from o in FDIDB.Cam_CN
                        orderby o.ID descending
                        select new CamCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            return query.ToList();
        }
        public CamCNItem GetCamCNItem(int id)
        {
            var query = from o in FDIDB.Cam_CN
                        where o.ID == id
                        orderby o.ID descending
                        select new CamCNItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Catename = o.Cate_CN.Name,
                            Price = o.Price,
                            HSD = o.HSD,
                            CateId = o.CateID,
                            LstCamNguyenlieuCnItem = o.NguyenLieu_Cam_CN.Where(a=>a.IsDeleted == false).Select(v=> new CamNguyenlieuCNItem
                            {
                                Nguyenlieu = v.NguyenLieu_CN.Name,
                                ID = v.ID,
                                IdNguyenlieu = v.IdNguyenlieu,
                                Quantity = v.Quantity,
                            })
                        };
            return query.FirstOrDefault();
        }
        public Cam_CN GetById(int id)
        {
            var query = from o in FDIDB.Cam_CN where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<Cam_CN> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.Cam_CN where lst.Contains(o.ID) select o;
            return query.ToList();
        }


        public void Add(Cam_CN item)
        {
            FDIDB.Cam_CN.Add(item);
        }
        public void Delete(Cam_CN item)
        {
            FDIDB.Cam_CN.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
