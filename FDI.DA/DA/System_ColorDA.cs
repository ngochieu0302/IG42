using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class System_ColorDA : BaseDA
    {
        #region Constructer
        public System_ColorDA()
        {
        }

        public System_ColorDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public System_ColorDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        
        public List<ColorItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.System_Color
                        orderby o.ID
                        select new ColorItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name,
                                       Value = o.Value,
                                       IsShow = o.IsShow,
                                       Description = o.Description,
                                       LanguageId = o.LanguageId
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<ColorItem> GetAll(int agencyId)
        {
            var query = from o in FDIDB.System_Color
                        select new ColorItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Value = o.Value,
                            IsShow = o.IsShow,
                            Description = o.Description,
                            LanguageId = o.LanguageId
                        };
            return query.ToList();
        }
        
        public List<ColorItem> GetListByArrId(string ltsArrID)
        {
            var ltsArrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from o in FDIDB.System_Color
                        where ltsArrId.Contains(o.ID)
                        orderby o.ID descending
                        select new ColorItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name,
                                       Value = o.Value,
                                       IsShow = o.IsShow,
                                       Description = o.Description
                                   };
            return query.ToList();
        }

        public ColorItem GetItemById(int id)
        {
            var query = from o in FDIDB.System_Color
                        where o.ID == id
                        orderby o.ID descending
                        select new ColorItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Value = o.Value,
                            IsShow = o.IsShow,
                            Description = o.Description
                        };
            return query.FirstOrDefault();
        }
        public System_Color GetById(int colorID)
        {
            var query = from c in FDIDB.System_Color where c.ID == colorID select c;
            return query.FirstOrDefault();
        }

        public List<System_Color> GetListByArrID(string ltsArrID)
        {
            var ltsArrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from c in FDIDB.System_Color where ltsArrId.Contains(c.ID) && !c.Shop_Product.Any() select c;
            return query.ToList();
        }
        public List<System_Color> GetListByArrID(List<int> ltsArrID)
        {
            //var ltsArrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from c in FDIDB.System_Color where ltsArrID.Contains(c.ID) && !c.Shop_Product.Any() select c;
            return query.ToList();
        }
        
        public void Add(System_Color systemColor)
        {
            FDIDB.System_Color.Add(systemColor);
        }
        
        public void Delete(System_Color systemColor)
        {
            FDIDB.System_Color.Remove(systemColor);
        }
        
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
