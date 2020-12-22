using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA.DA.AppSales
{
    public class UserAppDA : BaseDA
    {
        #region Contruction

        public UserAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public UserAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public DNUserAppItem GetItemByUsername(string username)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserName == username
                        select new DNUserAppItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            Gender = c.Gender == true ? 1 : 0,
                            Address = c.Address,
                            Birthday = c.BirthDay,
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
       
        public DN_Users GetByUsername(string username)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserName == username
                        select c;
            return query.FirstOrDefault();
        }
        public DN_Agency GetAgencyById(int id)
        {
            var query = from c in FDIDB.DN_Agency
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public DNUserAppItem GetItemById(Guid id)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserId == id
                        select new DNUserAppItem
                        {
                            UserId = c.UserId,
                            UserName = c.UserName,
                            Gender = c.Gender == true ? 1 : 0,
                            Address = c.Address,
                            Birthday = c.BirthDay,
                            FullName = c.LoweredUserName,
                            Depart = c.DN_Agency.Department,
                            Company = c.DN_Agency.Company,
                            MST = c.DN_Agency.MST,
                            STK = c.DN_Agency.STK,
                            BankName = c.DN_Agency.BankName,
                            AgencyID = c.AgencyID.Value,
                            GroupID = c.DN_Agency.GroupID
                        };
            return query.FirstOrDefault();
        }

    }
}
