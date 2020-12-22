using System.Collections.Generic;
using System.Linq;
using System;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;
using FDI.CORE;

namespace FDI.DA
{
    public partial class DNLoginAppDA : BaseDA
    {
        #region User

        public DNLoginAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNLoginAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public DNUserAppItem GetUserItemByCode(string code)
        {
            //var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && c.DN_Login.Any(m => m.Code == code 
                            //&& m.DateCreated < date && m.DateEnd > date
                             && m.IsOut == false)
                        select new DNUserAppItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            Gender = c.Gender == true ? 1 : 0,
                            Address = c.Address,
                            Birthday = c.BirthDay,
                            AgencyWallet = c.DN_Agency.WalletValue,
                            FullName = c.LoweredUserName,
                            Password = c.Password,
                            Depart = c.DN_Agency.Department,
                            Company = c.DN_Agency.Company,
                            MST = c.DN_Agency.MST,
                            STK = c.DN_Agency.STK,
                            BankName = c.DN_Agency.BankName,
                            GroupID = c.DN_Agency.GroupID
                        };
            return query.FirstOrDefault();
        }

        public List<GroupAgencyItem> GetAllGroup()
        {
            //var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_GroupAgency
                        where (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                        select new GroupAgencyItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                        };
            return query.ToList();
        }

        public DNUserAppItem GetPassByUserName(string userName)
        {
            //var s = Utility.S;
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                        where c.UserName == userName && (!c.IsLockedOut.HasValue || c.IsLockedOut == false) && (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                            //&& c.AgencyID.HasValue && c.DN_Agency.DN_Enterprises.DateStart < time && c.DN_Agency.DN_Enterprises.DateEnd > time &&
                            //c.DN_Agency.DN_Enterprises.IsLocked == false && c.DN_Agency.DN_Enterprises.IsDeleted == false
                        //&& c.DN_Agency.IsLock == false
                        //&& c.DN_Agency.DN_Enterprises.Url.ToLower() == domain.ToLower()
                        select new DNUserAppItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            EnterprisesID = c.DN_Agency.EnterpriseID,
                            AgencyID = c.AgencyID ?? 0,
                            AreaID = c.DN_Agency.Areas.FirstOrDefault() != null ? c.DN_Agency.Areas.FirstOrDefault().ID : (c.DN_Agency.Market.AreaID ?? 0),
                            MarketID = c.DN_Agency.MarketID ?? 0,
                            Password = c.Password,
                            PasswordSalt = c.PasswordSalt,
                            AgencyDeposit = c.DN_Agency.Documents.Where(m => m.DateStart <= date && m.DateEnd >= date).Sum(a => a.Deposit),
                            AgencyAddress = c.DN_Agency.Address,
                            AgencyWallet = c.DN_Agency.WalletValue,
                            GroupID = c.DN_Agency.GroupID
                        };
            return query.FirstOrDefault();
        }
        public DNUserAppItem GetPassByUserName(string userName, bool isAgency)
        {
            //var s = Utility.S;
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Users
                where c.UserName == userName && (!c.IsLockedOut.HasValue || c.IsLockedOut == false) && (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                && (c.IsAgency == isAgency || (c.IsAgency==null && !isAgency))
                
                select new DNUserAppItem
                {
                    UserId = c.UserId,
                    UserName = c.UserName,
                    EnterprisesID = c.DN_Agency.EnterpriseID,
                    AgencyID = c.AgencyID ?? 0,
                    AreaID = c.DN_Agency.Areas.FirstOrDefault() != null ? c.DN_Agency.Areas.FirstOrDefault().ID : (c.DN_Agency.Market.AreaID ?? 0),
                    MarketID = c.DN_Agency.MarketID ?? 0,
                    Password = c.Password,
                    PasswordSalt = c.PasswordSalt,
                    AgencyDeposit = c.DN_Agency.Documents.Where(m => m.DateStart <= date && m.DateEnd >= date).Sum(a => a.Deposit),
                    AgencyAddress = c.DN_Agency.Address,
                    AgencyWallet = c.DN_Agency.WalletValue,
                    GroupID = c.DN_Agency.GroupID,
                    AgencyLevelId = c.DN_Agency.AgencyLevelId,
                    isLock = c.DN_Agency.IsLock
                };
            return query.FirstOrDefault();
        }
        public bool Logout(string code)
        {
            var query = from c in FDIDB.DN_Login
                        where c.Code.ToLower() == code.ToLower()
                        select c;
            var obj = query.FirstOrDefault();
            var date = DateTime.Now;
            var time = date.TotalSeconds();
            if (obj != null)
            {
                obj.DateEnd = time;
                obj.IsOut = true;
                FDIDB.SaveChanges();
            }
            return true;
        }
        public void Add(DN_Login dnLogin)
        {
            FDIDB.DN_Login.Add(dnLogin);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}