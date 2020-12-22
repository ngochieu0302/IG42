using System.Linq;
using System;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public partial class DNLoginDA : BaseDA
    {
        #region User

        public DNLoginDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNLoginDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public DNUserItem GetUserItemByCode(string code)
        {
            var date = DateTime.Now;
            var time = date.TotalSeconds();
            var query = from c in FDIDB.DN_Login
                        where c.Code == code && c.IsOut == false && c.DN_Users.DN_UsersInRoles.Any(v => !v.IsDelete.HasValue ||v.IsDelete == false) && c.DateCreated < time && c.DateEnd > time
                        orderby c.ID descending
                        select new DNUserItem
                        {
                            UserId = c.DN_Users.UserId,
                            UserName = c.DN_Users.UserName,
                            listRole = c.DN_Users.DN_UsersInRoles.Where(v => (!v.IsDelete.HasValue || v.IsDelete == false) && (!v.DN_Roles.IsDeleted.HasValue || v.DN_Roles.IsDeleted == false)).Select(o => o.DN_Roles.RoleName),
                            listRoleID = c.DN_Users.DN_UsersInRoles.Where(v => (!v.IsDelete.HasValue || v.IsDelete == false) && (!v.DN_Roles.IsDeleted.HasValue || v.DN_Roles.IsDeleted == false)).Select(o => o.DN_Roles.ID),
                            EnterprisesID = c.DN_Users.DN_Agency.EnterpriseID,
                            AgencyID = c.DN_Users.AgencyID ?? 0,
                            AreaID = c.DN_Users.DN_Agency.Areas.FirstOrDefault() != null ? c.DN_Users.DN_Agency.Areas.FirstOrDefault().ID : (c.DN_Users.DN_Agency.Market.AreaID ?? 0),
                            MarketID = c.DN_Users.DN_Agency.MarketID ?? 0,
                            LoweredUserName = c.DN_Users.LoweredUserName,
                            AgencyName = c.DN_Users.DN_Agency.Name,
                            AgencyDeposit = c.DN_Users.DN_Agency.Documents.Sum(a=>a.Deposit),
                            AgencyAddress = c.DN_Users.DN_Agency.Address,
                            AgencyWallet = c.DN_Users.DN_Agency.WalletValue,
                            RoleId = c.DN_Users.DN_UsersInRoles.Where(v => !v.IsDelete.HasValue || v.IsDelete == false).Select(m => m.RoleId).FirstOrDefault()
                        };
            return query.FirstOrDefault();
        }
        public CustomerItem GetCustomerByCode(string code)
        {
            var date = DateTime.Now;
            var time = date.TotalSeconds();
            var query = from c in FDIDB.DN_Login
                        where c.Code.ToLower() == code && c.IsOut == false && c.DateEnd > time
                        orderby c.ID descending
                        select new CustomerItem
                        {
                            ID = c.Customer.ID,
                            UserName = c.Customer.UserName,
                            FullName = c.Customer.FullName,
                            Password = c.Customer.PassWord,
                            PasswordSalt = c.Customer.PasswordSalt
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
            var time =date.TotalSeconds();
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