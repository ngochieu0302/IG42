using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNMailSSCDA : BaseDA
    {
        #region Constructer
        public DNMailSSCDA()
        {
        }

        public DNMailSSCDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNMailSSCDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNMailSSCItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int type, int agencyId, Guid userId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.AgencyID == agencyId && o.UserSendId == userId && o.IsDelete == false && o.Type == type
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            IsSpam = o.IsSpam,
                            IsDraft = o.IsDraft,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            //query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Mail_SSC GetById(int id)
        {
            var query = from c in FDIDB.DN_Mail_SSC where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNMailSSCItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.ID == id
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            Type = o.Type,
                            Status = o.Status,
                            AgencyID = o.AgencyID,
                            CreateDate = o.CreateDate,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName,
                            UserSendEmail = o.DN_Users.Email,
                            UserReceiveEmail = o.DN_Users1.Email,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            Content = o.Content,
                            ListDNFileMailItem = o.DN_File_Mail.Where(m=> m.IsDeleted == false && m.IsShow == true).Select(m=> new DNFileMailItem
                            {
                                ID = m.ID,
                                Url = m.Url,
                                Folder = m.Folder
                            })
                        };
            return query.FirstOrDefault();
        }

        public List<DNMailSSCItem> CountInboxNew(int type, int agencyId, Guid userId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.Type == type && o.AgencyID == agencyId && o.UserReceiveId == userId && o.Status == 0 && o.IsDelete == false && o.IsSpam == false && o.IsRecycleBin == false && o.IsDraft == false
                        orderby o.ID descending 
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsImportant,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CountInbox(int type, int agencyId, Guid userId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.Type == type && o.AgencyID == agencyId && o.UserReceiveId == userId && o.IsDelete == false && o.IsSpam == false && o.IsRecycleBin == false && o.IsDraft == false
                        orderby o.ID descending 
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsImportant,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> SentMail(int agencyId, Guid userId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.AgencyID == agencyId && o.UserSendId == userId && o.IsDelete == false && o.Type == 1 && o.IsSpam == false && o.IsRecycleBin == false && o.IsDraft == false
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsImportant,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CountDrafts(int agencyId, Guid userId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where  o.AgencyID == agencyId && o.UserSendId == userId && o.IsDelete == false && o.IsSpam == false && o.IsRecycleBin == false && o.IsDraft == true
                        orderby o.ID descending 
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsSpam = o.IsSpam,
                            IsDraft = o.IsDraft,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsImportant,
                            Content = o.Content,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CountSpam(int type, int agencyId, Guid userId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where  o.IsDelete == false && o.IsSpam == true && o.IsRecycleBin == false && o.IsDraft == false && (o.UserReceiveId == userId || o.UserSendId == userId)
                        orderby o.ID descending 
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam =  o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CountRecycleBin(Guid userId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.IsDelete == false  && o.IsRecycleBin == true && (o.UserReceiveId == userId || o.UserSendId == userId)
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        #region Customer
        public List<DNMailSSCItem> CustomerCountInboxNew(int type, int agencyId, int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.Type == type && o.CustomerReceiveId == customerId && o.StatusEmail == false && o.Status == 0 && o.IsDelete == false && o.IsSpam == false && o.IsRecycleBin == false && o.IsDraft == false
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsImportant,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CustomerCountInbox(int type, int agencyId, int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.Type == type  && o.CustomerReceiveId == customerId && o.StatusEmail == false && o.IsDelete == false && o.IsSpam == false && o.IsRecycleBin == false && o.IsDraft == false
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsImportant,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CustomerSentMail(int agencyId, int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where  o.CustomerSendId == customerId && o.StatusEmail == true && o.IsDelete == false && o.Type == 1 && o.IsSpam == false && o.IsRecycleBin == false && o.IsDraft == false
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsImportant,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }


        public List<DNMailSSCItem> CustomerCountDrafts(int agencyId, int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where  o.CustomerSendId == customerId && o.StatusEmail == true && o.IsDelete == false && o.IsSpam == false && o.IsRecycleBin == false && o.IsDraft == true
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsSpam = o.IsSpam,
                            IsDraft = o.IsDraft,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsImportant,
                            Content = o.Content,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CustomerCountSpam(int type, int agencyId, int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.IsDelete == false && o.IsSpam == true && o.DN_StatusEmail.Any(m=> m.CustomerId == customerId)
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CustomerCountSpamSend(int type, int agencyId, int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.IsDelete == false && o.IsSpam == true &&  o.CustomerSendId == customerId && o.StatusEmail == true
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CustomerCountSpamReceive(int type, int agencyId, int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.IsDelete == false && o.IsSpam == true && o.CustomerReceiveId == customerId && o.StatusEmail == false
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }


        public List<DNMailSSCItem> CustomerCountRecycleBin(int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.IsDelete == false && o.IsRecycleBin == true && o.IsSpam == false && o.DN_StatusEmail.Any(m => m.CustomerId == customerId)
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CustomerCountRecycleBinSend(int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.IsDelete == false && o.IsRecycleBin == true && o.IsSpam == false &&  o.CustomerSendId == customerId && o.StatusEmail == true
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }

        public List<DNMailSSCItem> CustomerCountRecycleBinReceive(int customerId)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.IsDelete == false && o.IsRecycleBin == true && o.IsSpam == false && o.CustomerReceiveId == customerId && o.StatusEmail == false
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsImportant = o.IsImportant,
                            IsDelete = o.IsDelete,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            return query.ToList();
        }
        #endregion


        public List<DNMailSSCItem> GetListDelete(int agencyId, Guid userId, int type)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.AgencyID == agencyId && o.IsDelete == false && o.IsRecycleBin == true && o.IsSpam == false && o.IsDraft == false
                        orderby o.ID descending
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                            IsImportant = o.IsImportant,
                            Type = o.Type,
                            IsDraft = o.IsDraft,
                            IsSpam = o.IsSpam,
                            IsDelete = o.IsDelete,
                            UserReceiveId = o.UserReceiveId,
                            UserSendId = o.UserSendId,
                            Content = o.Content,
                            CreateDate = o.CreateDate,
                            Status = o.Status,
                            CustomerSendName = o.Customer.UserName,
                            CustomerReceiveName = o.Customer1.UserName,
                            UserSendName = o.DN_Users.UserName,
                            UserReceiveName = o.DN_Users1.UserName
                        };
            query = type == 1 ? query.Where(m => m.UserReceiveId == userId) : query.Where(m => m.UserSendId == userId);
            return query.ToList();
        }

        public List<DNMailSSCItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_Mail_SSC
                        where o.AgencyID == agencyid
                         select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                        };
            return query.ToList();
        }


        public List<DNMailSSCItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Mail_SSC
                        where  ltsArrId.Contains(o.ID)
                        select new DNMailSSCItem
                        {
                            ID = o.ID,
                            Title = o.Title.Trim(),
                        };
            return query.ToList();
        }

        public List<DN_File_Mail> GetFileMailArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_File_Mail
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }

        public void Add(DN_Mail_SSC mailSsc)
        {
            FDIDB.DN_Mail_SSC.Add(mailSsc);
        }

        public void Delete(DN_Mail_SSC mailSsc)
        {
            FDIDB.DN_Mail_SSC.Remove(mailSsc);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
