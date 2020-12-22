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
    public class EnterprisesDA : BaseDA
    {
        #region Constructer
        public EnterprisesDA()
        {
        }

        public EnterprisesDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public EnterprisesDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<EnterprisesItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            var date = DateTime.Now.TotalSeconds();
            Request = new ParramRequest(httpRequest);
            var status = httpRequest["isstatus"];
            var query = from o in FDIDB.DN_Enterprises
                        where !o.IsDeleted.HasValue || o.IsDeleted == false
                        orderby o.ID descending
                        select new EnterprisesItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Email = o.Email,
                            Address = o.Address,
                            Phone = o.Phone,
                            CMTND = o.CMTND,
                            IsLocked = o.IsLocked,
                            UserName = o.UserName,
                            IsTest = o.IsTest,
                            Url = o.Url,
                            TotalPay = o.DN_Agency.Sum(m=>m.Customer_Reward.Sum(c=>c.PriceReward- c.PriceReceive)),
                            DateEnd = o.DateEnd,
                            DomainDN = o.DomainDN,
                            Percent = o.Percent
                        };
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "1")
                {
                    query = query.Where(m => m.DateEnd > date && (!m.IsTest.HasValue || m.IsTest.Value));
                }
                if (status == "2")
                {
                    query = query.Where(m => m.DateEnd < date);
                }
                if (status == "3")
                {
                    query = query.Where(m => m.IsTest == true);
                }
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Enterprises GetById(int id)
        {
            var query = from c in FDIDB.DN_Enterprises where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public bool CheckUrl(string url,int id)
        {
            return FDIDB.DN_Enterprises.Any(c=> c.ID != id 
            && (c.DomainDN.ToLower() == url.ToLower() || c.Url.ToLower() == url.ToLower()));
            
        }

        public DN_Enterprises GetPassByUserName(string userName, string domain)
        {
            var query = from c in FDIDB.DN_Enterprises
                        where c.IsLocked == false && c.IsDeleted == false && c.UserName.ToLower() == userName.ToLower()
                        //&& c.DomainDN.ToLower() == domain.ToLower()
                        select c;
            return query.FirstOrDefault();
        }

        public DN_Enterprises GetPassByUserName(string userName)
        {
            var query = from c in FDIDB.DN_Enterprises
                        where c.IsLocked == false && c.IsOut == false && (c.UserName.ToLower() == userName.ToLower() || c.CodeLogin.ToLower() == userName.ToLower())
                        select c;
            return query.FirstOrDefault();
        }

        public EnterprisesItem GetItemByCodeLogin(string code)
        {
            var datenow = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Enterprises
                        where c.CodeLogin == code && c.IsOnline == true && c.DateOut >= datenow && c.DateLogin <= datenow
                        select new EnterprisesItem
                        {
                            ID = c.ID,
                            UserName = c.UserName,
                            PasswordSalt = c.PasswordSalt,
                            Password = c.Password
                        };
            return query.FirstOrDefault();
        }

        public EnterprisesItem GetItem(int id)
        {
            var query = from c in FDIDB.DN_Enterprises
                        where c.ID == id
                        select new EnterprisesItem
                        {
                            Percent = c.Percent,
                            PercentOrder = c.PercentOrder
                        };
            return query.FirstOrDefault();
        }
        public EnterprisesItem GetItemByAgencyID(int id)
        {
            var query = from c in FDIDB.DN_Agency
                        where c.ID == id
                        select new EnterprisesItem
                        {
                            Percent = c.DN_Enterprises.Percent,
                            PercentOrder = c.DN_Enterprises.PercentOrder
                        };
            return query.FirstOrDefault();
        }
        public EnterprisesItem GetItemById(int id)
        {
            var query = from c in FDIDB.DN_Enterprises
                        where c.ID == id
                        select new EnterprisesItem
                            {
                                ID = c.ID,
                                PasswordSalt = c.PasswordSalt,
                                Password = c.Password
                            };
            return query.FirstOrDefault();
        }
        public List<EnterprisesItem> GetAll()
        {
            var date = DateTime.Now;
            var dtResult = new DateTime(date.Year, date.Month, 1);
            var fromDate = dtResult.TotalSeconds();
            var toDate = date.TotalSeconds();
            var query = from o in FDIDB.DN_Enterprises
                        where !o.IsTest.HasValue || !o.IsTest.Value
                        orderby FDIDB.Shop_Orders.Where(c => o.DN_Agency.Any(m => m.ID == c.AgencyId) && c.IsDelete == false && c.StartDate >= fromDate && c.StartDate <= toDate && c.EndDate <= toDate).Sum(m => m.TotalPrice)
                        select new EnterprisesItem
                        {
                            ID = o.ID,
                            Name = o.Name
                        };
            return query.ToList();
        }
        public EnterprisesItem GetContent(string domain)
        {
            domain = domain.ToLower();
            var query = from c in FDIDB.DN_Enterprises
                        where c.Url.ToLower() == domain || c.DomainDN == domain && (!c.IsDeleted.HasValue ||!c.IsDeleted.Value)
                        select new EnterprisesItem
                            {
                                PictureUrl = c.Urllogo,
                                Content = c.Content
                            };
            return query.Any()? query.FirstOrDefault(): new EnterprisesItem();
        }

        public List<DN_Enterprises> GetListAll()
        {
            var query = from c in FDIDB.DN_Enterprises where c.IsShow == true && c.IsLocked == false && c.IsOut == false select c;
            return query.ToList();
        }

        public List<DN_Enterprises> GetListByArrId(string ltsArrID)
        {
            var arrId = FDIUtils.StringToListInt(ltsArrID);
            var query = from c in FDIDB.DN_Enterprises where arrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<STGroupItem> GetListStGroupById(int enterprisesId)
        {
            var query = from c in FDIDB.ST_Group
                        //where c.DN_Enterprises.Any(m => m.ID == enterprisesId)
                        orderby c.Sort
                        select new STGroupItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                        };
            return query.ToList();
        }
        public ModelTotalItem GetStaticEnterprise(int enterprisesId)
        {
            var query = from c in FDIDB.DN_Enterprises
                        where c.ID == enterprisesId
                        select new ModelEnterprisesStaticItem
                        {
                            TotalPrice = (from a in FDIDB.Shop_Orders join b in FDIDB.DN_Agency on a.AgencyId equals b.ID
                                join d in FDIDB.DN_Enterprises on b.EnterpriseID equals d.ID where a.IsDelete == false && a.Status == 3 && d.ID == enterprisesId && a.EndDate != null && b.IsDelete == false select new OrderItem
                                {
                                    TotalPrice = a.TotalPrice
                                }).Sum(n=>n.TotalPrice),
                            TotalOrder = (from a in FDIDB.Shop_Orders
                                          join b in FDIDB.DN_Agency on a.AgencyId equals b.ID
                                          join d in FDIDB.DN_Enterprises on b.EnterpriseID equals d.ID
                                          where a.IsDelete == false && a.Status == 3 && d.ID == enterprisesId && a.EndDate != null && b.IsDelete == false
                                          select new OrderGetItem()
                                          {
                                              ID = a.ID
                                          }).Count(),
                            TotalAgent = (from a in FDIDB.DN_Agency join b in FDIDB.DN_Enterprises on a.EnterpriseID equals b.ID where a.IsDelete == false && b.ID == enterprisesId select new AgencyItem
                            {
                                ID = a.ID
                            }).Count(),
                            TotalCustomer = (from a in FDIDB.Customers
                                          where a.IsDelete == false &&  a.IsActive == true
                                          select new CustomerItem
                                          {
                                              ID = a.ID
                                          }).Count(),
                        };
            var query2 = from c in FDIDB.DN_Enterprises
                where c.ID == enterprisesId
                select new ModelEnterprisesTotalItem
                {
                    TotalReceipt = (from a in FDIDB.ReceiptVouchers
                                    join b in FDIDB.DN_Agency on a.AgencyId equals b.ID
                        join d in FDIDB.DN_Enterprises on b.EnterpriseID equals d.ID where a.IsActive == true && a.IsDelete == false && d.ID == enterprisesId select new ReceiptPaymentItem
                        {
                            ID = a.ID,
                            Price = a.Price
                        }).Sum(h=>h.Price),
                    TotalPayment = (from a in FDIDB.PaymentVouchers
                              join b in FDIDB.DN_Agency on a.AgencyId equals b.ID
                              join d in FDIDB.DN_Enterprises on b.EnterpriseID equals d.ID
                              where a.IsActive == true && a.IsDelete == false && d.ID == enterprisesId
                              select new RecipeItem()
                              {
                                  ID = a.ID,
                                  Price = a.Price
                              }).Sum(v => v.Price),
                    TotalCash = (from a in FDIDB.CashAdvances
                              join b in FDIDB.DN_Agency on a.AgencyId equals b.ID
                              join d in FDIDB.DN_Enterprises on b.EnterpriseID equals d.ID
                              where a.IsActive == true && a.IsDelete == false && d.ID == enterprisesId
                              select new CashAdvanceItem()
                              {
                                  ID = a.ID,
                                  Price = a.Price
                              }).Sum(n => n.Price),
                    TotalRepay = (from a in FDIDB.Repays
                              join b in FDIDB.DN_Agency on a.AgencyId equals b.ID
                              join d in FDIDB.DN_Enterprises on b.EnterpriseID equals d.ID
                              where a.IsActive == true && a.IsDelete == false && d.ID == enterprisesId
                              select new RewardHistoryItem()
                              {
                                  ID = a.ID,
                                  Price = a.Price
                              }).Sum(m => m.Price),

                };
            var model = new ModelTotalItem
            {
                Items = query.FirstOrDefault(),
                Item2 = query2.FirstOrDefault(),
            };

            return model;
        }

        #region tạo mã số doanh nghiệp
        public string GetCodeEnterprises()
        {
            const int maxCodeLength = 6;
            int countProduct = FDIDB.DN_Enterprises.OrderByDescending(m => m.ID).Select(m => m.ID).FirstOrDefault();
            string newCode = "";
            int nextNumber = countProduct + 1;
            for (int i = 0; i < maxCodeLength - countProduct.ToString().Length; i++)
            {
                newCode += "0";
            }
            return string.Concat(newCode, nextNumber.ToString());
        }
        #endregion

        public void Add(DN_Enterprises enterprises)
        {
            FDIDB.DN_Enterprises.Add(enterprises);
        }

        public List<ST_Group> GetListGroupByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.ST_Group where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Delete(DN_Enterprises enterprises)
        {
            FDIDB.DN_Enterprises.Remove(enterprises);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
