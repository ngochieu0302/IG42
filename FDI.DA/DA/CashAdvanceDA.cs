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
    public partial class CashAdvanceDA : BaseDA
    {
        #region Constructer
        public CashAdvanceDA()
        {
        }

        public CashAdvanceDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CashAdvanceDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CashAdvanceItem> GetListByRequest(HttpRequestBase httpRequest, int agencyid, out decimal total, out decimal totalActive, out decimal totalDelete)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.CashAdvances
                        where c.AgencyId == agencyid
                        orderby c.ID descending
                        select new CashAdvanceItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            DateActive = c.DateActive,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserID,
                            UserActive = c.UserActive,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            UserName = c.DN_Users1.UserName,
                            FullNameActive = c.DN_Users2.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            UsernameActive = c.DN_Users2.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            IsActive = c.IsActive,
                            IsDelete = c.IsDelete
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal();
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateReturn >= fromDate && c.DateReturn < toDate);
            }
            var IsActive = httpRequest["IsActive"];
            if (!string.IsNullOrEmpty(IsActive))
            {
                if (IsActive == "0")
                {
                    query = query.Where(c => c.IsActive == false && c.IsDelete == false);
                }
                else if (IsActive == "1")
                {
                    query = query.Where(c => c.IsActive == true);
                }
                else if (IsActive == "2")
                {
                    query = query.Where(c => c.IsDelete == true);
                }
            }
            query = query.SelectByRequest(Request);
            total = query.Sum(m => m.Price ?? 0);
            totalActive = query.Any(c => c.IsActive == true) ? query.Where(c => c.IsActive == true).Sum(m => m.Price ?? 0) : 0;
            totalDelete = query.Any(c => c.IsDelete == true) ? query.Where((c => c.IsDelete == true)).Sum(m => m.Price ?? 0) : 0;
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public CashAdvanceItem GetCashAdvanceItem(int id)
        {
            var query = from c in FDIDB.CashAdvances
                        where c.ID == id
                        select new CashAdvanceItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserID,
                            PaymentMethodId = c.PaymentMethodId,
                            UserCashier = c.UserCashier,
                            UserName = c.DN_Users.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            IsActive = c.IsActive
                        };
            return query.FirstOrDefault();
        }

        public CashAdvance GetById(int id)
        {
            var query = from c in FDIDB.CashAdvances where c.ID == id && c.IsDelete == false && c.IsActive == false select c;
            return query.FirstOrDefault();
        }

        public List<CashAdvance> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.CashAdvances
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false
                        select c;
            return query.ToList();
        }
        public void Add(CashAdvance item)
        {
            FDIDB.CashAdvances.Add(item);
        }

        /// <summary>
        /// Danh sách trả ứng
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<CashAdvanceItem> GetListRepayByRequest(HttpRequestBase httpRequest, int agencyid, out decimal total, out decimal totalActive, out decimal totalDelete)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.Repays
                        where c.AgencyId == agencyid
                        orderby c.ID descending
                        select new CashAdvanceItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            DateActive = c.DateActive,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserID,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            UserName = c.DN_Users1.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            IsActive = c.IsActive,
                            IsDelete = c.IsDelete
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal();
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateReturn >= fromDate && c.DateReturn < toDate);
            }
            var IsActive = httpRequest["IsActive"];
            if (!string.IsNullOrEmpty(IsActive))
            {
                if (IsActive == "0")
                {
                    query = query.Where(c => c.IsActive == false && c.IsDelete == false);
                }
                else if (IsActive == "1")
                {
                    query = query.Where(c => c.IsActive == true);
                }
                else if (IsActive == "2")
                {
                    query = query.Where(c => c.IsDelete == true);
                }
            }
            query = query.SelectByRequest(Request);
            total = query.Sum(m => m.Price ?? 0);
            totalActive = query.Any(c => c.IsActive == true) ? query.Where(c => c.IsActive == true).Sum(m => m.Price ?? 0) : 0;
            totalDelete = query.Any(c => c.IsDelete == true) ? query.Where((c => c.IsDelete == true)).Sum(m => m.Price ?? 0) : 0;
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Đối tượng trả ứng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CashAdvanceItem GetRepayItem(int id)
        {
            var query = from c in FDIDB.Repays
                        where c.ID == id
                        select new CashAdvanceItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserID,
                            PaymentMethodId = c.PaymentMethodId,
                            UserCashier = c.UserCashier,
                            UserName = c.DN_Users.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            IsActive = c.IsActive
                        };
            return query.FirstOrDefault();
        }
        public Repay GetRepayById(int id)
        {
            var query = from c in FDIDB.Repays where c.ID == id && c.IsDelete == false && c.IsActive == false select c;
            return query.FirstOrDefault();
        }
        public List<Repay> GetRepayListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Repays
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false
                        select c;
            return query.ToList();
        }
        public void AddRepay(Repay item)
        {
            FDIDB.Repays.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }

        /// <summary>
        /// Tổng quát ứng
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <returns></returns>
        public List<CashAdvanceItem> GetListGeneralCash(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);

            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyid
                        && (c.CashAdvances1.Any(v => v.IsActive == true) || c.Repays1.Any(v => v.IsActive == true)) && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        orderby c.UserName
                        select new CashAdvanceItem
                        {
                            Name = c.LoweredUserName,
                            UserName = c.UserName,
                            TotalCash = c.CashAdvances1.Where(v => v.IsActive == true).Sum(v => v.Price),
                            TotalRepay = c.Repays1.Where(v => v.IsActive == true).Sum(v => v.Price),
                        };
            query = query.SelectByRequest(Request);
            return query.ToList();
        }
    }
}

