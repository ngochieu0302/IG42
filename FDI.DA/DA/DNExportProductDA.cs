using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNExportProductDA : BaseDA
    {
        public DNExportProductDA()
        {
        }
        public DNExportProductDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNExportProductDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNExportProductItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.DN_ExportProduct
                        where o.AgencyId == agencyId
                        orderby o.ID descending
                        select new DNExportProductItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            TotalPrice = o.TotalPrice,
                            DateCreated = o.DateCreated,
                            DateExport = o.DateExport,
                            UserID = o.UserID,
                            Note = o.Note
                        };
            return query.ToList();
        }

        public List<DNExportProductItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDate().TotalSeconds() : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDate().AddDays(1).TotalSeconds() : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_ExportProduct
                        where o.AgencyId == agencyId && o.DateExport >= fromDate && o.DateExport <= toDate
                        orderby o.ID descending
                        select new DNExportProductItem
                        {
                            ID = o.ID,
                            Code = o.Code,
                            IsDeleted = o.IsDeleted,
                            TotalPrice = o.TotalPrice,
                            DateCreated = o.DateCreated,
                            DateExport = o.DateExport,
                            UserID = o.UserID,
                            Note = o.Note,
                        };
            var isdelete = httpRequest["IsDelete"];
            if (isdelete == "1")
            {
                query = query.Where(c => c.IsDeleted == true);
            }
            else if (isdelete == "0")
            {
                query = query.Where(c => c.IsDeleted == false);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DNExportProductItem GetItemById(int id)
        {
            var query = from c in FDIDB.DN_ExportProduct
                        where c.ID == id
                        select new DNExportProductItem
                            {
                                ID = c.ID,
                                Note = c.Note,
                                DateCreated = c.DateCreated,
                                DateExport = c.DateExport,
                                Code = c.Code,
                                TotalPrice = c.TotalPrice,
                                UserName = c.DN_Users1.UserName,
                                IsDeleted = c.IsDeleted,
                                UserGet = c.UserGet,
                                UserGetName = c.DN_Users.UserName,
                                IsOrder = c.IsOrder,
                            };
            return query.FirstOrDefault();
        }
        public DN_ExportProduct GetById(int id)
        {
            var query = from o in FDIDB.DN_ExportProduct where o.ID == id select o;
            return query.FirstOrDefault();
        }

        public List<DN_ExportProduct> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.DN_ExportProduct where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(DN_ExportProduct item)
        {
            FDIDB.DN_ExportProduct.Add(item);
        }
        public void Delete(DN_ExportProduct item)
        {
            FDIDB.DN_ExportProduct.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
