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
    public class DNExportDA : BaseDA
    {
        public DNExportDA()
        {
        }
        public DNExportDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNExportDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<DNExportItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.DN_Export
                        where o.AgencyId == agencyId
                        orderby o.ID descending
                        select new DNExportItem
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

        public List<DNExportItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from)? from.StringToDate().TotalSeconds() : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDate().AddDays(1).TotalSeconds() : DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Export
                        where o.AgencyId == agencyId && o.DateExport >= fromDate && o.DateExport <= toDate
                        orderby o.ID descending
                        select new DNExportItem
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

        public DNExportItem GetItemById(int id)
        {
            var query = from c in FDIDB.DN_Export
                        where c.ID == id
                        select new DNExportItem
                            {
                                ID = c.ID,
                                Note = c.Note,
                                DateCreated = c.DateCreated,
                                DateExport = c.DateExport,
                                Code = c.Code,
                                IsOrder = c.IsOrder,
                                TotalPrice = c.TotalPrice,
                                UserName = c.DN_Users.UserName,
                                IsDeleted = c.IsDeleted,
                                UserGet = c.UserGet,
                                UserGetName = c.DN_Users1.UserName,
                                Export_Product_Value = c.Export_Product_Value.Select(v => new ExportProductValueItem
                                                {
                                                    ID = v.ID,
                                                    ImportID = v.ImportID,
                                                    ValueName = v.DN_Import.Shop_Product_Value.Name,
                                                    Quantity = v.Quantity,
                                                    Price = v.Price,
                                                    PriceExport = v.PriceExport,
                                                    Date = v.DN_Import.Date,
                                                    DateEnd = v.DN_Import.DateEnd,
                                                    UnitName = v.DN_Import.Shop_Product_Value.DN_Unit.Name,
                                                    IsDelete = v.IsDelete
                                                })
                            };
            return query.FirstOrDefault();
        }
        public DN_Export GetById(int id)
        {
            var query = from o in FDIDB.DN_Export where o.ID == id select o;
            return query.FirstOrDefault();
        }

        public List<DN_Export> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.DN_Export where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(DN_Export item)
        {
            FDIDB.DN_Export.Add(item);
        }
        public void Delete(DN_Export item)
        {
            FDIDB.DN_Export.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
