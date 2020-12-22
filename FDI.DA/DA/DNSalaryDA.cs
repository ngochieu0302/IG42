using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNSalaryDA : BaseDA
    {
        #region Constructer
        public DNSalaryDA()
        {
        }

        public DNSalaryDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNSalaryDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNSalaryItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Salary
                        where o.AgencyID == agencyid
                        orderby o.ID descending
                        select new DNSalaryItem
                        {
                            ID = o.ID,
                            Salary = o.Salary,
                            UserName = o.DN_Users.UserName,
                            DateCreated = o.DateCreated,
                            OrderId = o.OrderId ?? 0,
                            DNCriteriaItem = new DNCriteriaItem
                            {
                                Name = o.DN_Criteria.Name
                            },
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Salary GetById(int id)
        {
            var query = from c in FDIDB.DN_Salary where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public DNSalaryItem GetItemById(int id)
        {
            var query = from o in FDIDB.DN_Salary
                        where o.ID == id
                        select new DNSalaryItem
                        {
                            ID = o.ID,
                            Salary = o.Salary,
                            UserName = o.DN_Users.UserName,
                            //Month = o.Month,
                            //Year = o.Year
                        };
            return query.FirstOrDefault();
        }

        public List<DNSalaryItem> GetAll(int agencyid)
        {
            var query = from o in FDIDB.DN_Salary
                        where o.AgencyID == agencyid
                        select new DNSalaryItem
                        {
                            ID = o.ID,
                            Salary = o.Salary,
                            UserName = o.DN_Users.UserName,
                            //Month = o.Month,
                            //Year = o.Year
                        };
            return query.ToList();
        }


        public List<DNSalaryItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Salary
                        where ltsArrId.Contains(o.ID)
                        select new DNSalaryItem
                        {
                            ID = o.ID,
                            Salary = o.Salary,
                            UserName = o.DN_Users.UserName,
                            //Month = o.Month,
                            //Year = o.Year
                        };
            return query.ToList();
        }

        public void Add(DN_Salary dnSalary)
        {
            FDIDB.DN_Salary.Add(dnSalary);
        }

        public void Delete(DN_Salary dnSalary)
        {
            FDIDB.DN_Salary.Remove(dnSalary);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
