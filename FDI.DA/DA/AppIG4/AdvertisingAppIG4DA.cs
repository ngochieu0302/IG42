using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class AdvertisingAppIG4DA : BaseDA
    {
        #region Constructer
        public AdvertisingAppIG4DA()
        {
        }

        public AdvertisingAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public AdvertisingAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<AdvertisingItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Advertisings
                        orderby c.ID descending
                        where c.Show == isShow
                        && c.Name.StartsWith(keyword) && !c.IsDeleted.Value

                        select new AdvertisingItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name
                                   };
            return query.Take(showLimit).ToList();
        }

        public List<AdvertisingItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Advertisings
                        orderby o.Sort, o.ID descending 
                        where o.IsDeleted == false && o.LanguageId == LanguageId
                        select new AdvertisingItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Show = o.Show,
                            Content = o.Content,
                            Width = o.Width,
                            Height = o.Height,
                            PositionID = o.PositionID,
                            PositionName = o.Advertising_Position.Name,
                            PictureUrl = (o.PictureID.HasValue) ? o.Gallery_Picture.Folder + o.Gallery_Picture.Url : string.Empty,
                            TypeId = o.TypeID,
                            TypeName = (o.TypeID.HasValue) ? o.Advertising_Type.Name : string.Empty,
                            CreateOnUtc = o.CreateOnUtc
                        };

            var positionId = httpRequest.QueryString["PositionID"];
            if (!string.IsNullOrEmpty(positionId))
            {
                var id = Convert.ToInt32(positionId);
                query = query.Where(c => c.PositionID == id);
            }
            //var name = httpRequest.QueryString["SearchIn"];
            //var keyword = httpRequest.QueryString["Keyword"];
            //if (name == "Name" && !string.IsNullOrEmpty(keyword))
            //{
            //    var txt = FDIUtils.Slug(keyword);
            //    query = query.Where(c => c.NameAscii.Contains(txt));
            //    return query.ToList();
            //}
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public Advertising GetById(int advertisingId)
        {
            var query = from c in FDIDB.Advertisings where c.ID == advertisingId select c;
            return query.FirstOrDefault();
        }

        public List<Advertising> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Advertisings where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(Advertising advertising)
        {
            FDIDB.Advertisings.Add(advertising);
        }

        public void Delete(Advertising advertising)
        {
            FDIDB.Advertisings.Remove(advertising);
        }

        public List<AdvertisingItem> GetByPosition(int id)
        {
            var query = from c in FDIDB.Advertisings
                        where c.PositionID == id && (c.IsDeleted != null && c.IsDeleted.Value == false)
                        select new AdvertisingItem()
                        {
                            ID = c.ID,
                            PictureUrl = (c.PictureID.HasValue) ? c.Gallery_Picture.Folder + c.Gallery_Picture.Url : string.Empty
                        };
            return query.ToList();
        }
    }
}
