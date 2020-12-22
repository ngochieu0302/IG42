using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNTotalSalaryMonthDA : BaseDA
    {
        #region Constructer
        public DNTotalSalaryMonthDA()
        {
        }

        public DNTotalSalaryMonthDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNTotalSalaryMonthDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNTotalSalaryMonthItem> GetListByMonthYear(int month, int year)
        {
            var query = FDIDB.DN_Total_SalaryMonth.Where(c => c.Month == month && c.Year == year)
                .Select(c => new DNTotalSalaryMonthItem
                {
                    ID = c.ID,
                    UserId = c.UserId,
                    Username = c.DN_Users.UserName,
                    LowerUsername = c.DN_Users.LoweredUserName,
                    Month = c.Month,
                    Year = c.Year,
                    DateUpdate = c.DateUpdate,
                    FixedSalary = c.FixedSalary,
                    Note = c.Note,
                    SalaryAward = c.SalaryAward,
                    TotalDateCC = c.TotalDateCC,
                    TotalDateNLV = c.TotalDateNLV,
                    TotalMuon = c.TotalMuon,
                    TotalSchedule = c.TotalSchedule,
                    TotalSom = c.TotalSom,
                    UserUpdate = c.UserUpdate
                });
            return query.ToList();
        }

        public DN_Total_SalaryMonth GetById(int id)
        {
            var query = from o in FDIDB.DN_Total_SalaryMonth where o.ID == id select o;
            return query.FirstOrDefault();
        }

        public DNTotalSalaryMonthItem GetByUseridMonthYear(Guid userId, int month, int year)
        {
            var query = from o in FDIDB.DN_Total_SalaryMonth
                        where o.UserId == userId && o.Month == month && o.Year == year
                        select new DNTotalSalaryMonthItem
                        {
                            ID = o.ID,
                            UserId = o.UserId,
                            Username = o.DN_Users.UserName,
                            LowerUsername = o.DN_Users.LoweredUserName,
                            Month = o.Month,
                            Year = o.Year,
                            AgencyID = o.AgencyID,
                            DateUpdate = o.DateUpdate,
                            UserUpdate = o.UserUpdate,
                            TotalDateCC = o.TotalDateCC,
                            FixedSalary = o.FixedSalary,
                            Note = o.Note,
                            SalaryAward = o.SalaryAward,
                            TotalSom = o.TotalSom,
                            TotalMuon = o.TotalMuon,
                            TotalDateNLV = o.TotalDateNLV,
                            TotalSchedule = o.TotalSchedule,
                            Status = o.Status,
                        };
            return query.FirstOrDefault();
        }

        public List<DNUserSimpleItem> GetSimpleListUserSalary(int agencyId)
        {
            var query = from o in FDIDB.DN_Total_SalaryMonth
                        where o.AgencyID == agencyId
                        select new DNUserSimpleItem
                        {
                            //ID = o.ID,
                            UserId = o.UserId,
                            UserName = o.DN_Users.UserName,
                            LoweredUserName = o.DN_Users.LoweredUserName
                        };
            return query.Distinct().ToList();
        }

        public DNTotalSalaryMonthItem GetItemByID(int id)
        {
           var query = from c in FDIDB.DN_Total_SalaryMonth
                        where c.ID == id
                       select new DNTotalSalaryMonthItem
                        {
                            ID = c.ID,
                            UserId = c.UserId,
                            Month = c.Month,
                            Username = c.DN_Users.UserName,
                            LowerUsername = c.DN_Users.LoweredUserName,
                            Year = c.Year,
                            FixedSalary = c.FixedSalary,
                            TotalDateCC = c.TotalDateCC,
                            TotalMuon = c.TotalMuon,
                            TotalSom = c.TotalSom,
                            SalaryAward = c.SalaryAward,
                            TotalSchedule = c.TotalSchedule,
                            Note = c.Note,
                        };
            return query.FirstOrDefault();
        }

        public DN_Total_SalaryMonth GetByUseridMonthYearDefault(Guid userId, int month, int year)
        {
            var query = from o in FDIDB.DN_Total_SalaryMonth
                        where o.UserId == userId && o.Month == month && o.Year == year
                        select o;
            return query.FirstOrDefault();
        }

        public void Add(DN_Total_SalaryMonth item)
        {
            FDIDB.DN_Total_SalaryMonth.Add(item);
        }

        public void Delete(DN_Total_SalaryMonth item)
        {
            FDIDB.DN_Total_SalaryMonth.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
