using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class CompanyDA:BankDA
    {
        #region Constructer
        public CompanyDA()
        {
        }

        public CompanyDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CompanyDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<CompanyItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Companies
                where (!c.IsDeleted.HasValue || c.IsDeleted == false)
                orderby c.ID descending 
                        select new CompanyItem
                {
                    ID = c.ID,
                    Name = c.Name,
                    Address = c.Address,
                    DateCreate = c.DateCreate,
                    DateUpdate = c.DateUpdate,
                    UserName = c.DN_Users.UserName,
                    NameRepresent = c.NameRepresent,
                    NumberBank = c.NumberBank,
                    TaxCode = c.TaxCode,
                    Bank = c.Bank,
                };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CompanyItem> GetAll()
        {
            var query = from c in FDIDB.Companies
                where (!c.IsDeleted.HasValue || c.IsDeleted == false)
                select new CompanyItem
                {
                    ID = c.ID,
                    Name = c.Name,
                };
            return query.ToList();
        }
        public CompanyItem GetItembyId(int id)
        {
            var query = from c in FDIDB.Companies
                where (!c.IsDeleted.HasValue || c.IsDeleted == false) && c.ID == id
                select new CompanyItem
                {
                    ID = c.ID,
                    Name = c.Name,
                    Address = c.Address,
                    DateCreate = c.DateCreate,
                    DateUpdate = c.DateUpdate,
                    UserName = c.DN_Users.UserName,
                    NameRepresent = c.NameRepresent,
                    NumberBank = c.NumberBank,
                    TaxCode = c.TaxCode,
                    Bank = c.Bank,
                    Phone = c.Phone,
                };
            return query.FirstOrDefault();
        }

        public Company GetbyId(int id)
        {
            var query = from c in FDIDB.Companies
                where c.ID == id
                select c;
            return query.FirstOrDefault();
        }

        public List<Company> GetListArrId(string lstarr)
        {
            var arr = FDIUtils.StringToListInt(lstarr);
            var query = from c in FDIDB.Companies
                where arr.Contains(c.ID)
                select c;
            return query.ToList();
        }

        public void Add(Company obj)
        {
            FDIDB.Companies.Add(obj);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
