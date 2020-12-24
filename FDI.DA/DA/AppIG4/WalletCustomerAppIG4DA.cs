using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class WalletCustomerAppIG4DA : BaseDA
    {
        #region Constructer
        public WalletCustomerAppIG4DA()
        {
        }

        public WalletCustomerAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public WalletCustomerAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<WalletCustomerAppIG4Item> GetListSimpleRequest(HttpRequestBase httpRequest, out decimal? total)
        {

            Request = new ParramRequest(httpRequest);
            var cusId = int.Parse(httpRequest["cusId"] ?? "0");
            var from = httpRequest["fromDate"];
            var to = httpRequest["toDate"];
            var fromDate = !string.IsNullOrEmpty(from) ? from.StringToDecimal() : 0;
            var toDate = !string.IsNullOrEmpty(to) ? to.StringToDecimal() : DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.WalletCustomers
                        where c.IsActive == true
                              && (!c.IsDelete.HasValue || c.IsDelete == false)
                              && (cusId == 0 || c.CustomerID == cusId)
                              && c.DateCreate >= fromDate && c.DateCreate <= toDate
                        orderby c.ID
                        select new WalletCustomerAppIG4Item
                        {
                            ID = c.ID,
                            IsActive = c.IsActive,
                            Code = c.Code,
                            CustomerName = c.Customer.FullName,
                            TransactionNo = c.Transaction_no,
                            Email = c.Customer.Email,
                            Totalprice = c.TotalPrice ?? 0,
                            Mobile = c.Customer.Mobile,
                            Type = c.Type,
                            DateCreate = c.DateCreate,
                        };
            total = query.Sum(c => c.Totalprice);
            query = query.SelectByRequest(Request, ref TotalRecord);

            return query.ToList();
        }
        public List<WalletCustomerAppIG4Item> GetListWalletCustomer()
        {
            var query = from c in FDIDB.WalletCustomers
                        select new WalletCustomerAppIG4Item
                        {
                            ID = c.ID,
                            IsActive = c.IsActive,
                            Code = c.Code,
                            CustomerName = c.Customer.FullName,
                            TransactionNo = c.Transaction_no,
                            Email = c.Customer.Email,
                            Mobile = c.Customer.Mobile,
                            DateCreate = c.DateCreate,
                        };
            return query.ToList();
        }
        public List<WalletCustomerAppIG4Item> GetListWalletCustomerbyId(int cusId)
        {
            var query = from c in FDIDB.WalletCustomers
                        where c.CustomerID == cusId
                              && (c.IsDelete == false || !c.IsDelete.HasValue) && c.IsActive == true
                              orderby c.ID descending 
                        select new WalletCustomerAppIG4Item
                        {
                            ID = c.ID,
                            IsActive = c.IsActive,
                            Code = c.Code,
                            Totalprice = c.TotalPrice,
                            CustomerName = c.Customer.FullName,
                            TransactionNo = c.Transaction_no,
                            Email = c.Customer.Email,
                            Mobile = c.Customer.Mobile,
                            DateCreate = c.DateCreate,
                            TypeWalet = c.Type,
                            Type = 1
                        };
            return query.ToList();
        }
        public List<RewardHistoryAppIG4Item> GetListWalletReward(int cusId)
        {
            var query = from c in FDIDB.RewardHistories
                        where c.CustomerID == cusId
                              && (c.IsDeleted == false || !c.IsDeleted.HasValue) && c.IsActive == true
                        orderby c.ID descending
                        select new RewardHistoryAppIG4Item()
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Price = c.Price,
                            DateCreate = c.Date,
                            BonustypeName = c.BonusType.Name,
                            Type = c.Type,
                            Percent = c.Percent,
                        };
            return query.ToList();
        }
        public List<RewardHistoryAppIG4Item> GetListWalletRecive(int cusId)
        {
            var query = from c in FDIDB.ReceiveHistories
                        where c.CustomerID == cusId
                              && (c.IsDeleted == false || !c.IsDeleted.HasValue) && c.IsActive == true
                        orderby c.ID descending
                        select new RewardHistoryAppIG4Item()
                        {
                            ID = c.ID,
                            CustomerName = c.Customer.FullName,
                            Price = c.Price,
                            DateCreate = c.DateCreate,
                            Type = c.Type,
                            Query = c.Query,
                        };
            return query.ToList();
        }
        public WalletCustomerAppIG4Item GetWalletCustomerAppIG4ItemById(int cusId)
        {
            var query = from c in FDIDB.WalletCustomers
                        where c.CustomerID == cusId
                        select new WalletCustomerAppIG4Item
                        {
                            ID = c.ID,
                            IsActive = c.IsActive,
                            Code = c.Code,
                            CustomerName = c.Customer.FullName,
                            TransactionNo = c.Transaction_no,
                            Email = c.Customer.Email,
                            Mobile = c.Customer.Mobile,
                            DateCreate = c.DateCreate,
                        };
            return query.FirstOrDefault();
        }

        public WalletCustomer GetbyId(int id)
        {
            var query = from c in FDIDB.WalletCustomers
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public ConfigExchange GetConfig()
        {
            var query = from c in FDIDB.ConfigExchanges
                        where c.IsActive == true
                        select c;
            return query.FirstOrDefault();
        }
        public ConfigItemAppIG4 GetConfigItem()
        {
            var query = from c in FDIDB.ConfigExchanges
                where c.IsActive == true
                select new ConfigItemAppIG4
                {
                    FeeShip = c.FeeShip,
                    Discount = c.DiscountOrder,
                };
            return query.FirstOrDefault();
        }
        public void Add(WalletCustomer obj)
        {
            FDIDB.WalletCustomers.Add(obj);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
