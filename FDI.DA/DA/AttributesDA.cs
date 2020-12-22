using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class AttributesDA : BaseDA
    {
        public AttributesDA()
        {
        }
        public AttributesDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public AttributesDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<AttributeDynamicItem> GetList(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_AttributeDynamic
                        orderby c.ID descending
                        select new AttributeDynamicItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Description = c.Description,
                            LstInt = c.Categories.Select(p => p.Id)
                        };
            var cateId = httpRequest.QueryString["CateID"];
            if (!string.IsNullOrEmpty(cateId))
            {
                var id = int.Parse(cateId);
                query = query.Where(c => c.LstInt.Contains(id));
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<AttributeDynamicItem> GetAttribute(string lstInts, int id)
        {
            var query = from c in FDIDB.GetAttribute(lstInts, id)
                        select new AttributeDynamicItem
                        {
                            ID = c.AttributeID,
                            ProductId = c.ProductId,
                            Name = c.Name,
                            Value = c.Values
                        };
            return query.ToList();
        }
        public DN_AttributeDynamic GetById(int id)
        {
            var query = from c in FDIDB.DN_AttributeDynamic where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public List<SuggestionsItem> GetListAuto(string keyword, int showLimit)
        {
            var query = from c in FDIDB.DN_AttributeDynamic
                        orderby c.Name
                        where c.Name.Contains(keyword)
                        select new SuggestionsItem
                        {
                            ID = c.ID,
                            value = c.Name,
                            data = c.Description
                        };
            return query.Take(showLimit).ToList();
        }
        public List<DN_AttributeDynamic> GetById(List<int> lst)
        {
            var query = from c in FDIDB.DN_AttributeDynamic where lst.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<AttributeOption> GetAttrValue(List<int> lst)
        {
            var query = from c in FDIDB.AttributeOptions where lst.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<Category> GetListCategory(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Categories where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }
        public void Add(DN_AttributeDynamic item)
        {
            FDIDB.DN_AttributeDynamic.Add(item);
        }
        public void Add(AttributeOption item)
        {
            FDIDB.AttributeOptions.Add(item);
        }
        public void Delete(DN_AttributeDynamic shop)
        {
            FDIDB.DN_AttributeDynamic.Remove(shop);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}