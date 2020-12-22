using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class BankDA : BaseDA
    {
        #region Constructer
        public BankDA()
        {
        }

        public BankDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public BankDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<BankItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.Banks
                        orderby c.ID descending
                        where c.IsDelete == false
                        select new BankItem
                                   {
                                       ID = c.ID,
                                       Name = c.Name,
                                       AccountNumber = c.AccountNumber,
                                       BankName = c.BankName,
                                   };
            return query.ToList();
        }

        public List<BankItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Banks
                        select new BankItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            AccountNumber = c.AccountNumber,
                            BankName = c.BankName,
                           
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="id">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Bank GetById(int id)
        {
            var query = from c in FDIDB.Banks where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<Bank> GetById(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Banks where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(Bank item)
        {
            FDIDB.Banks.Add(item);
        }

        public void Delete(Bank item)
        {
            FDIDB.Banks.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion
    }
}
