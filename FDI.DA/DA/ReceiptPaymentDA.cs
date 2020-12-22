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
    public partial class ReceiptPaymentDA : BaseDA
    {
        #region Constructer
        public ReceiptPaymentDA()
        {
        }

        public ReceiptPaymentDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public ReceiptPaymentDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        /// <summary>
        /// Phiếu chuyển
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ReceiptPaymentItem> GetListByRequest(HttpRequestBase httpRequest, int agencyid, out decimal total, out decimal totalActive, out decimal totalDelete)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.ReceiptPayments
                        where c.AgencyId == agencyid
                        orderby c.ID descending
                        select new ReceiptPaymentItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            DateActive = c.DateActive,
                            Type = c.Type,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserId,
                            UserCashier = c.UserCashier,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            UserName = c.DN_Users1.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            CostTypeID = c.CostTypeId,
                            CostTypeName = c.CostType.Name,
                            IsActive = c.IsActive,
                            IsDelete = c.IsDelete
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal(0);
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateReturn >= fromDate && c.DateReturn < toDate);
            }
            var CostTypeID = httpRequest["CostTypeID"];
            if (!string.IsNullOrEmpty(CostTypeID))
            {
                var t = int.Parse(CostTypeID);
                query = query.Where(c => c.CostTypeID == t);
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
        public List<ReceiptPaymentItem> GetListByRequestAdmin(HttpRequestBase httpRequest, int Paymethod)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.ReceiptPayments
                        where c.PaymentMethodId == Paymethod
                        orderby c.ID descending
                        select new ReceiptPaymentItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            DateActive = c.DateActive,
                            Type = c.Type,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserId,
                            UserCashier = c.UserCashier,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            UserName = c.DN_Users1.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            CostTypeID = c.CostTypeId,
                            CostTypeName = c.CostType.Name,
                            IsActive = c.IsActive,
                            IsDelete = c.IsDelete
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal(0);
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateReturn >= fromDate && c.DateReturn < toDate);
            }
            var CostTypeID = httpRequest["CostTypeID"];
            if (!string.IsNullOrEmpty(CostTypeID))
            {
                var t = int.Parse(CostTypeID);
                query = query.Where(c => c.CostTypeID == t);
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
            decimal total = query.Sum(m => m.Price ?? 0); 
            query = query.SelectByRequest(Request);           
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        /// <summary>
        /// Phiếu chuyển theo user
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="user"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ReceiptPaymentItem> GetListByUserRequest(HttpRequestBase httpRequest, int agencyid, Guid user, out decimal total, out decimal totalActive, out decimal totalDelete)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];

            var query = from c in FDIDB.ReceiptPayments
                        where c.AgencyId == agencyid
                        orderby c.ID descending
                        select new ReceiptPaymentItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            DateActive = c.DateActive,
                            Type = c.Type,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserId,
                            UserCashier = c.UserCashier,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            UserName = c.DN_Users1.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            CostTypeID = c.CostTypeId,
                            CostTypeName = c.CostType.Name,
                            IsActive = c.IsActive,
                            IsDelete = c.IsDelete
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal(0);
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateCreated >= fromDate && c.DateCreated < toDate);
            }
            var CostTypeID = httpRequest["CostTypeID"];
            if (!string.IsNullOrEmpty(CostTypeID))
            {
                var t = int.Parse(CostTypeID);
                query = query.Where(c => c.CostTypeID == t);
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
        /// Phiếu chi
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total">Trả về tổng tiền chi</param>
        /// <returns></returns>
        public List<ReceiptPaymentItem> GetListPayment(HttpRequestBase httpRequest, int agencyid, out decimal total, out decimal totalActive, out decimal totalDelete)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.PaymentVouchers
                        where c.AgencyId == agencyid
                        orderby c.ID descending
                        select new ReceiptPaymentItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            DateActive = c.DateActive,
                            Type = c.Type,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserID,
                            UserCashier = c.UserCashier,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            UserName = c.DN_Users1.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            CostTypeID = c.CostTypeId,
                            CostTypeName = c.CostType.Name,
                            IsActive = c.IsActive,
                            IsDelete = c.IsDelete
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal(0);
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateCreated >= fromDate && c.DateCreated < toDate);
            }
            var CostTypeID = httpRequest["CostTypeID"];
            if (!string.IsNullOrEmpty(CostTypeID))
            {
                var t = int.Parse(CostTypeID);
                query = query.Where(c => c.CostTypeID == t);
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
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            totalActive = query.Any(c => c.IsActive == true) ? query.Where(c => c.IsActive == true).Sum(m => m.Price ?? 0) : 0;
            totalDelete = query.Any(c => c.IsDelete == true) ? query.Where((c => c.IsDelete == true)).Sum(m => m.Price ?? 0) : 0;
            return query.ToList();
        }
        /// <summary>
        /// Phiếu thu
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <param name="total">Trả về tổng tiền thu</param>
        /// <returns></returns>
        public List<ReceiptPaymentItem> GetListReceipt(HttpRequestBase httpRequest, int agencyid, out decimal total)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var query = from c in FDIDB.ReceiptVouchers
                        where c.AgencyId == agencyid && (!c.IsDelete.HasValue || c.IsDelete == false)
                        orderby c.ID descending
                        select new ReceiptPaymentItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserCashier = c.UserCashier,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            CostTypeID = c.CostTypeId,
                            CostTypeName = c.CostType.Name,
                        };
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                var fromDate = from.StringToDecimal();
                var toDate = to.StringToDecimal(1);
                query = query.Where(c => c.DateCreated >= fromDate && c.DateCreated < toDate);
            }
            var CostTypeID = httpRequest["CostTypeID"];
            if (!string.IsNullOrEmpty(CostTypeID))
            {
                var t = int.Parse(CostTypeID);
                query = query.Where(c => c.CostTypeID == t);
            }

            query = query.SelectByRequest(Request);
            total = query.Sum(m => m.Price ?? 0);
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        /// <summary>
        /// Tổng quát thu chi
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <returns></returns>
        public List<ReceiptPaymentItem> GetListUserRp(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyid
                        && (c.PaymentVouchers.Any(v => v.IsActive == true) || c.ReceiptVouchers.Any(v => v.IsActive == true)) && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        orderby c.UserName
                        select new ReceiptPaymentItem
                        {
                            Name = c.LoweredUserName,
                            UserName = c.UserName,
                            TotalReceip = c.ReceiptVouchers.Where(v => v.IsActive == true).Sum(v => v.Price),
                            TotalOrder = c.ReceiptPayments.Where(v => v.IsActive == true).Sum(v => v.Price),
                            TotalPayment = c.PaymentVouchers.Where(v => v.IsActive == true).Sum(v => v.Price),
                            TotalCashAdvance = c.CashAdvances.Where(v => v.IsActive == true).Sum(v => v.Price)
                        };
            query = query.SelectByRequest(Request);
            return query.ToList();
        }
        /// <summary>
        /// Tổng quát chuyển
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <returns></returns>
        public List<ReceiptPaymentItem> GetListGeneralTrip(HttpRequestBase httpRequest, int agencyid)
        {
              var date = DateTime.Today.TotalSeconds();
            Request = new ParramRequest(httpRequest);
            var t = (int)FDI.CORE.OrderStatus.Complete;
            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyid
                        && (c.Shop_Orders.Any(v => v.Status == t || (v.Status < t && v.DateCreated < date)) || c.ReceiptPayments.Any(v => v.IsActive == true))
                        orderby c.UserName
                        select new ReceiptPaymentItem
                        {
                            Name = c.LoweredUserName,
                            UserName = c.UserName,
                            TotalReceip = c.Shop_Orders.Where(v => v.Status == t || (v.Status < t && v.DateCreated < date)).Sum(v => v.PriceReceipt),
                            TotalPayment = c.ReceiptPayments.Where(v => v.IsActive == true).Sum(v => v.Price),
                        };
            query = query.SelectByRequest(Request);
            return query.ToList();
        }
        /// <summary>
        /// Doanh số thu tiền theo đơn hàng
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <returns></returns>
        public List<ReceiptPaymentItem> GetListGeneralOrder(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var from = httpRequest["fromDate"];
            var date = DateTime.Now;
            var to = httpRequest["toDate"];
            var t = (int)FDI.CORE.OrderStatus.Complete;
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(from) ? to.StringToDecimal(1) : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where c.AgencyID == agencyid
                        && (c.Shop_Orders.Any(v => v.DateCreated >= fromDate && v.DateCreated <= toDate))
                        orderby c.UserName
                        select new ReceiptPaymentItem
                        {
                            Name = c.LoweredUserName,
                            UserName = c.UserName,
                            TotalReceip = c.Shop_Orders.Where(v => v.DateCreated >= fromDate && v.DateCreated <= toDate && v.Status == t).Sum(v => v.PriceReceipt)
                        };
            return query.ToList();
        }

        public int Count(int agencyId, int type)
        {
            var count = FDIDB.ReceiptPayments.Count(c => c.AgencyId == agencyId && c.Type == type);
            return count;
        }
        public PaymentVoucher GetPaymentById(int id)
        {
            var query = from c in FDIDB.PaymentVouchers where c.ID == id && c.IsDelete == false && c.IsActive == false select c;
            return query.FirstOrDefault();
        }
        public ReceiptVoucher GetReceiptById(int id)
        {
            var query = from c in FDIDB.ReceiptVouchers where c.ID == id && c.IsDelete == false && c.IsActive == false select c;
            return query.FirstOrDefault();
        }
        public ReceiptPayment GetById(int id)
        {
            var query = from c in FDIDB.ReceiptPayments where c.ID == id && c.IsDelete == false && c.IsActive == false select c;
            return query.FirstOrDefault();
        }
        public ReceiptPaymentItem GetPaymentItem(int id)
        {
            var query = from c in FDIDB.PaymentVouchers
                        where c.ID == id
                        select new ReceiptPaymentItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            Type = c.Type,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserID,
                            UserCashier = c.UserCashier,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            UserName = c.DN_Users.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            CostTypeID = c.CostTypeId,
                            CostTypeName = c.CostType.Name,
                            IsActive = c.IsActive
                        };
            return query.FirstOrDefault();
        }
        public ReceiptPaymentItem GetReceiptItem(int id)
        {
            var query = from c in FDIDB.ReceiptVouchers
                        where c.ID == id
                        select new ReceiptPaymentItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            OrderId = c.OrderId,
                            Type = c.Type,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserCashier = c.UserCashier,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            CostTypeID = c.CostTypeId,
                            CostTypeName = c.CostType.Name,
                            IsActive = c.IsActive
                        };
            return query.FirstOrDefault();
        }
        public ReceiptPaymentItem GetReceiptPaymentItem(int id)
        {
            var query = from c in FDIDB.ReceiptPayments
                        where c.ID == id
                        select new ReceiptPaymentItem
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateReturn = c.DateReturn,
                            OrderId = c.OrderId,
                            Type = c.Type,
                            Code = c.Code,
                            Note = c.Note,
                            Price = c.Price,
                            UserID = c.UserId,
                            UserCashier = c.UserCashier,
                            PaymentMethodId = c.PaymentMethodId,
                            UserNameCashier = c.DN_Users.UserName,
                            UserName = c.DN_Users1.UserName,
                            FullNameCashier = c.DN_Users.LoweredUserName,
                            FullNameReceipt = c.DN_Users1.LoweredUserName,
                            CostTypeID = c.CostTypeId,
                            CostTypeName = c.CostType.Name,
                            IsActive = c.IsActive
                        };
            return query.FirstOrDefault();
        }
        public List<ReceiptPayment> GetById(List<int> lst)
        {
            var query = from c in FDIDB.ReceiptPayments where lst.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<PaymentVoucher> GetListPaymentArrId(List<int> ltsArrId, Guid userid)
        {
            var query = from c in FDIDB.PaymentVouchers
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false && c.UserID == userid
                        select c;
            return query.ToList();
        }
        public List<PaymentVoucher> GetListPaymentArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.PaymentVouchers
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false
                        select c;
            return query.ToList();
        }
        public List<ReceiptVoucher> GetListReceiptArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.ReceiptVouchers
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false
                        select c;
            return query.ToList();
        }
        public List<ReceiptPayment> GetListArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.ReceiptPayments
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false
                        select c;
            return query.ToList();
        }
        public List<ReceiptPayment> GetListArrId(List<int> ltsArrId, Guid userid)
        {
            var query = from c in FDIDB.ReceiptPayments
                        where ltsArrId.Contains(c.ID) && c.IsDelete == false && c.UserId == userid
                        select c;
            return query.ToList();
        }
        public void Add(PaymentVoucher item)
        {
            FDIDB.PaymentVouchers.Add(item);
        }
        public void Add(ReceiptVoucher item)
        {
            FDIDB.ReceiptVouchers.Add(item);
        }
        public void Add(ReceiptPayment item)
        {
            FDIDB.ReceiptPayments.Add(item);
        }

        public void Delete(ReceiptPayment item)
        {
            FDIDB.ReceiptPayments.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }


        /// <summary>
        /// Tính toán
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="agencyid"></param>
        /// <returns></returns>
        public int GetListGeneral(int agencyid, int mounth, int year)
        {
            var from = (new DateTime(year, mounth, 1)).TotalSeconds();
            var to = (new DateTime(year, mounth + 1, 1)).TotalSeconds();
            var now = DateTime.Now;

            var query = FDIDB.DN_Total_SalaryMonth.ToList();
            //where c.AgencyID == agencyid
            //select new TotalSalaryMonthItem
            //{
            //    UserId = c.UserId,
            //    UngMonney = c.DN_Users.Where(v => v.IsDelete == false).Sum(v => v.Price),
            //    TraUngMoney = c.Repays1.Where(v => v.IsDelete == false).Sum(v => v.Price),
            //    TotalAwardKH = c.Customer.RewardHistories.Where(v=>v.IsDeleted == false).Sum(v=>v.Price),
            //    TotalReceiptKH = c.Customer.ReceiveHistories.Where(v => v.IsDeleted == false).Sum(v => v.Price),
            //};


            return 1;
        }

        public List<GeneralTotalItem> GeneralListTotal(int year, int agencyId)
        {
            var query = FDIDB.GeneralYear(year, agencyId).Select(m => new GeneralTotalItem
                            {
                                Month = m.I ?? 0,
                                TotalOrder = m.TotalPriceO ?? 0,
                                TotalCash = m.TotalPriceCA ?? 0,
                                TotalPayment = m.TotalPricePV ?? 0,
                                TotalReceipt = m.TotalPriceRV ?? 0,
                                TotalRepay = m.TotalPriceR ?? 0,
                            }).ToList();
            return query;
        }
    }
}
