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
    public class DNDocumentDA : BaseDA
    {
        public DNDocumentDA()
        {
        }
        public DNDocumentDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public DNDocumentDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        public List<DocumentItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Documents
                        where o.IsCancel != true
                        orderby o.ID descending
                        select new DocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            ShortDescription = o.ShortDescription,
                            NguoiKy = o.NguoiKy,
                            DrawerID = o.DrawerID,
                            NoiBanHanh = o.NoiBanHanh,
                            Description = o.Description,
                            IsShow = o.IsShow,
                            Address = o.Address,
                            MobileB = o.MobieB,
                            Numberbill = o.NumberBill,
                            Status = o.Status,
                            DateStart = o.DateStart,
                            DateEnd = o.DateEnd,
                            DrawerName = o.DN_Drawer.Name,
                            NameB = o.NameB,
                            Value = o.Value,
                            Type = o.Type,
                            Deposit = o.Deposit,
                        };
            var drawerId = Convert.ToInt32(httpRequest.QueryString["DrawerID"]);
            if (drawerId > 0)
                query = query.Where(m => m.DrawerID == drawerId);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DocumentItem> GetListWarningSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            var date = DateTime.Today.TotalSeconds();
            var datet = DateTime.Today.AddMonths(2).TotalSeconds();
            var month = datet - date;
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Documents
                        where (date >= (o.DateEnd - month)) && date <= o.DateEnd
                        orderby o.ID descending
                        select new DocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            ShortDescription = o.ShortDescription,
                            NguoiKy = o.NguoiKy,
                            DrawerID = o.DrawerID,
                            NoiBanHanh = o.NoiBanHanh,
                            Description = o.Description,
                            IsShow = o.IsShow,
                            Address = o.Address,
                            MobileB = o.MobieB,
                            Numberbill = o.NumberBill,
                            Status = o.Status,
                            DateStart = o.DateStart,
                            DateEnd = o.DateEnd,
                            DrawerName = o.DN_Drawer.Name,
                            NameB = o.NameB,
                            Value = o.Value,
                            Type = o.Type,
                            Deposit = o.Deposit,
                        };
            var drawerId = Convert.ToInt32(httpRequest.QueryString["DrawerID"]);
            if (drawerId > 0)
                query = query.Where(m => m.DrawerID == drawerId);
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DocumentItem> ExcelGetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Documents
                        orderby o.ID descending
                        select new DocumentItem
                        {
                            Name = o.Name,
                            ShortDescription = o.ShortDescription,
                            NguoiKy = o.NguoiKy,
                            DrawerID = o.DrawerID,
                            NoiBanHanh = o.NoiBanHanh,
                            Description = o.Description,
                            IsShow = o.IsShow,
                            DrawerName = o.DN_Drawer.Name,
                        };
            var drawerId = Convert.ToInt32(httpRequest.QueryString["DrawerID"]);
            
            if (drawerId > 0)
                query = query.Where(m => m.DrawerID == drawerId);
            return query.ToList();
        }
        public List<DocumentItem> GetListSimple(int agencyId)
        {
            var query = from o in FDIDB.Documents
                        where  o.IsShow == true 
                        orderby o.ID descending
                        select new DocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                        };
            return query.ToList();
        }
        public List<StaticDocumentItem> GeneralListStatic(int year, int areaId)
        {
            var query = FDIDB.StaticDocumentYear(year, areaId).Select(m => new StaticDocumentItem
            {
                Month = m.I ?? 0,
                TotalDoc = m.totadocument,
                TotalValue = m.totalvalue,
                TotalDeposit = m.totaldep
            }).ToList();
            return query;
        }
        public DocumentItem GetItemsByID(int id)
        {
            var query = from o in FDIDB.Documents
                        where o.ID == id
                        orderby o.ID descending
                        select new DocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            DrawerID = o.DrawerID,
                            ShortDescription = o.ShortDescription,
                            NguoiKy = o.NguoiKy,
                            NoiBanHanh = o.NoiBanHanh,
                            Description = o.Description,
                            IsShow = o.IsShow,
                            DrawerName = o.DN_Drawer.Name,
                            NameB = o.NameB,
                            Type = o.Type,
                            Code = o.Code,
                            NameCompany = o.NameCompany,
                            Address = o.Address,
                            BankName = o.BankName,
                            Department = o.Department,
                            MST = o.MST,
                            STK = o.STK,
                            AgencyId = o.AgencyId,
                            DateEnd = o.DateEnd,
                            DateStart = o.DateStart,
                            Value = o.Value,
                            IsCancel = o.IsCancel,
                            Isbill = o.IsBill,
                            UserId = o.UserId,
                            SupId = o.SupId,
                            Numberbill = o.NumberBill,
                            MobileB = o.MobieB,
                            Deposit = o.Deposit,
                            DocumentFilesItem = o.DocumentFiles.Select(d=> new DocumentFilesItem
                            {
                                ID = d.ID,
                                Name = d.Name,
                                UrlDocument = d.Folder + d.FileUrl,
                                FileSize = d.FileSize,
                                FileUrl = d.FileUrl
                            })
                        };
            return query.FirstOrDefault();
        }

        public List<Category> GetListCateByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Categories where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }

        public List<Document> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Documents where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<DocumentFile> GetListFileByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DocumentFiles where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        public Document GetById(int id)
        {
            var query = from c in FDIDB.Documents where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public void Add(Document item)
        {
            FDIDB.Documents.Add(item);
        }
        public void Delete(Document item)
        {
            FDIDB.Documents.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
