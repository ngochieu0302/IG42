using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA.CheckOrigin
{
    public class SourceDA :BaseDA
    {
        #region Constructer
        public SourceDA()
        {
        }

        public SourceDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public SourceDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<SourceItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Sources
                        where o.IsDeleted != true && o.IsShow == true
                        orderby o.ID descending
                        select new SourceItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            SupplierName = o.StorageProduct.DN_Supplier.Name,
                            Description = o.Description,
                            IsShow = o.IsShow,
                            Code = o.Code,
                            DateCreate = o.DateCreate,
                            
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<SourceItem> GetList()
        {
            var query = from c in FDIDB.Sources
                        select new SourceItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                        };
            return query.ToList();
        }
        public SourceItem GetItemById(int id)
        {
            var query = from c in FDIDB.Sources
                        where c.ID == id
                        select new SourceItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Description = c.Description,
                            Details = c.Details,
                            IsShow = c.IsShow,
                            SourceVideo = c.Gallery_Video.Select(v => new GalleryVideoItem
                            {
                                ID = v.ID,
                                Name = v.Name,
                                Url = v.UrlYoutube,
                                PictureUrl = v.Gallery_Picture.Folder + v.Gallery_Picture.Url,
                            }),
                            PictureUrl = c.Gallery_Picture.Folder  + c.Gallery_Picture.Url,
                            IsDeleted = c.IsDeleted,
                            AgencyID = c.AgencyID,
                            DateCreate = c.DateCreate,
                        };
            return query.FirstOrDefault();
        }
        public Source GetById(int id)
        {
            var query = from c in FDIDB.Sources where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<Source> GetListByArrId(string ltsArrID)
        {
            var arrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from c in FDIDB.Sources where arrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public void Add(Source agency)
        {
            FDIDB.Sources.Add(agency);
        }
        public void Deletevideo(Source_Video agency)
        {
            FDIDB.Source_Video.Remove(agency);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
