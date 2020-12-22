using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class System_TagDA : BaseDA
    {
        #region Constructer
        public System_TagDA()
        {
        }

        public System_TagDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public System_TagDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<TagItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.System_Tag
                        where c.LanguageId == LanguageId && c.IsDelete == false
                        orderby c.ID descending 
                        select new TagItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            IsShow = c.IsShow,
                            IsDeleted = c.IsDelete
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<TagItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in FDIDB.System_Tag
                        orderby c.Name
                        where c.Name.StartsWith(keyword) && c.IsDelete == false && c.IsShow.Value
                        select new TagItem
                                   {
                            ID = c.ID,
                            Name = c.Name
                        };
            return query.Take(showLimit).ToList();
        }
        
        public System_Tag GetById(int tagID)
        {
            var query = from c in FDIDB.System_Tag where c.ID == tagID select c;
            return query.FirstOrDefault();
        }

        public List<System_Tag> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.System_Tag where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public System_Tag GetByName(string tagName)
        {
            var query = from c in FDIDB.System_Tag where ((c.Name == tagName)) select c;
            return query.FirstOrDefault();
        }

        public System_Tag AddOrGet(string tagName)
        {
            var systemTag = GetByName(tagName);
            if (systemTag == null)
            {
                var newTag = new System_Tag
                                 {
                    Name = tagName
                };
                FDIDB.System_Tag.Add(newTag);
                FDIDB.SaveChanges();
                return newTag;
            }
            return systemTag;
        }

        public void AddTagToProduct(int tagId, int productId)
        {
            var product = FDIDB.Shop_Product.Find(productId);
            var tag = FDIDB.System_Tag.Find(tagId);
            if (product == null || tag == null) return;
            //product.System_Tag.Add(tag);
            FDIDB.SaveChanges();

        }

        public void RemoveTagFrompProduct(int tagId, int productId)
        {
            var product = FDIDB.Shop_Product.Find(productId);
            var tag = FDIDB.System_Tag.Find(tagId);
            if (product == null || tag == null) return;
            //product.System_Tag.Remove(tag);
            FDIDB.SaveChanges();
        }

        public void Add(System_Tag systemTag)
        {
            FDIDB.System_Tag.Add(systemTag);
        }

        public void Delete(System_Tag systemTag)
        {
            FDIDB.System_Tag.Remove(systemTag);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
