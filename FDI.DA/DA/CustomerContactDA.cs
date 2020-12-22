using System;
using System.Collections.Generic;
using System.Linq;
using FDI.Base;
using FDI.Simple;
using System.Web;

namespace FDI.DA
{
    public partial class CustomerContactDA : BaseDA
    {
        #region Constructer
        public CustomerContactDA()
        {
        }

        public CustomerContactDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CustomerContactDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CustomerContactItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.CustomerContacts
                        orderby c.Id descending
                        where c.IsShow == isShow
                        && c.Name.StartsWith(keyword) && !c.IsDelete.Value

                        select new CustomerContactItem
                        {
                            ID = c.Id,
                            Name = c.Name,
                            Email = c.Email,
                            Subject = c.Subject,
                            Message = c.Message,
                            IsShow = c.IsShow.Value,
                            IsDelete = c.IsDelete.Value,
                            Status = c.Status,
                            CreatedOnUtc = c.CreatedOnUtc,
                            Phone = c.Phone,
                            TypeContact = c.TypeContact,

                        };
            return query.Take(showLimit).ToList();
        }

        public CustomerContact GetById(int id)
        {
            var query = from c in FDIDB.CustomerContacts where c.Id == id select c;
            return query.FirstOrDefault();
        }

        public List<CustomerContact> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.CustomerContacts where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }

        public List<CustomerContactItem> GetListRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.CustomerContacts
                orderby o.Id descending
                where o.IsDelete == false 
                select o;

            var typeContact = httpRequest["TypeContact"];
            if (!string.IsNullOrEmpty(typeContact))
            {
                var type = Convert.ToInt32(typeContact);
                query = query.Where(o => o.TypeContact == type);
            }
            var status = httpRequest["Status"];
            if (!string.IsNullOrEmpty(status))
            {
                var _bool = status == "1";
                query = query.Where(o => o.Status == _bool); 
            }
            
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.Select(t => new CustomerContactItem
            {
                ID = t.Id,
                Name = t.Name,
                Email = t.Email,
                Subject = t.Subject,
                Message = t.Message,
                IsShow = t.IsShow.Value,
                IsDelete = t.IsDelete.Value,
                Status = t.Status,
                CreatedOnUtc = t.CreatedOnUtc,
                Phone = t.Phone,
                TypeContact = t.TypeContact,
            }).ToList();
        }

        public void Add(CustomerContact customerContact)
        {
            FDIDB.CustomerContacts.Add(customerContact);
        }

        public void Delete(CustomerContact customerContact)
        {
            FDIDB.CustomerContacts.Remove(customerContact);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
