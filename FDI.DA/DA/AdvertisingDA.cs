using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class AdvertisingDA : BaseDA
    {
        #region Constructer
        public AdvertisingDA()
        {
        }

        public AdvertisingDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public AdvertisingDA(string pathPaging, string pathPagingExt)
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

        public AdvertisingItem GetItemById(int id)
        {
            var query = from c in FDIDB.Advertisings
                        orderby c.ID descending
                        where  c.IsDeleted == false && c.ID == id
                        select new AdvertisingItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            PictureUrl = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Sort = c.Sort,
                            Link = c.Link,
                            Show = c.Show,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            Content = c.Content,
                            PositionName = c.Advertising_Position.Name,
                            PositionID = c.PositionID,
                            PictureId = c.PictureID,
                        };
            return query.FirstOrDefault();
        }

        public List<AdvertisingItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Advertisings
                        orderby o.ID
                        where o.IsDeleted == false && o.LanguageId == LanguageId
                        select new AdvertisingItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Show = o.Show,
                            Content = o.Content,
                            Width = o.Width,
                            Height = o.Height,
                            Link = o.Link,
                            PositionName = o.Advertising_Position.Name,
                            PictureUrl = (o.PictureID.HasValue) ? o.Gallery_Picture.Folder + o.Gallery_Picture.Url : string.Empty,
                            TypeId = o.TypeID,
                            TypeName = (o.TypeID.HasValue) ? o.Advertising_Type.Name : string.Empty,
                            //CreateOnUtc = o.CreateOnUtc
                        };

            var positionId = httpRequest.QueryString["PositionID"];
            if (!string.IsNullOrEmpty(positionId))
            {
                var id = Convert.ToInt32(positionId);
                query = query.Where(c => c.AdvertisingPosition.ID == id);
            }
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
        public void Add(Advertising advertising)
        {
            FDIDB.Advertisings.Add(advertising);
        }

        public void Delete(Advertising advertising)
        {
            FDIDB.Advertisings.Remove(advertising);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }

    }
}
