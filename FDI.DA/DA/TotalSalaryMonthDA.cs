using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class TotalSalaryMonthDA : BaseDA
    {
        #region Constructer
        public TotalSalaryMonthDA()
        {
        }

        public TotalSalaryMonthDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public TotalSalaryMonthDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<TotalSalaryMonthItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Total_SalaryMonth
                        orderby c.ID
                        select new TotalSalaryMonthItem
                                   {
                                       ID = c.ID,
                                       Month = c.Month,
                                       Year = c.Year,
                                       FixedSalary = c.FixedSalary,
                                       TotalMuon = c.TotalMuon,
                                       TotalDateCC = c.TotalDateCC,
                                       TotalSchedule = c.TotalSchedule,
                                       TotalSom = c.TotalSom,
	                                   SalaryAward = c.SalaryAward,
                                       Note = c.Note,
                                       DN_Users = new DNUserItem
                                       {
                                           UserName = c.DN_Users.UserName,
                                           LoweredUserName = c.DN_Users.LoweredUserName
                                       }
                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        
        public DN_Total_SalaryMonth GetById(int id)
        {
            var query = from c in FDIDB.DN_Total_SalaryMonth
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }
        public void AddTotal_SalaryMonth(int month, int year)
        {
            FDIDB.AddTotal_SalaryMonth(month, year);
        }
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <returns>Bản ghi</returns>
        public TotalSalaryMonthItem GetRoleItemById(int id)
        {
            var query = from c in FDIDB.DN_Total_SalaryMonth
                        where c.ID == id
                        select new TotalSalaryMonthItem
                        {
                            ID = c.ID,
                            Month = c.Month,
                            Year = c.Year,
                            FixedSalary = c.FixedSalary,
                            TotalMuon = c.TotalMuon,
                            TotalDateCC = c.TotalDateCC,
                            TotalSchedule = c.TotalSchedule,
                            TotalSom = c.TotalSom,
	                        SalaryAward = c.SalaryAward,
                            Note = c.Note,
                            DN_Users = new DNUserItem
                            {
                                UserName = c.DN_Users.UserName,
                                LoweredUserName = c.DN_Users.LoweredUserName
                            }
                        };
            var result = query.FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<DN_Total_SalaryMonth> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DN_Total_SalaryMonth where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="dnTotalSalaryMonth">bản ghi cần thêm</param>
        public void Add(DN_Total_SalaryMonth dnTotalSalaryMonth)
        {
            FDIDB.DN_Total_SalaryMonth.Add(dnTotalSalaryMonth);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        public void Delete(DN_Total_SalaryMonth dnTotalSalaryMonth)
        {
            FDIDB.DN_Total_SalaryMonth.Remove(dnTotalSalaryMonth);
        }

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
