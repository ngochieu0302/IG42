using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class CompanyItem:BaseSimple
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string TaxCode { get; set; }
        public string NameRepresent { get; set; }
        public string Bank { get; set; }
        public string NumberBank { get; set; }
        public decimal? DateCreate { get; set; }
        public Guid? UserID { get; set; }
        public decimal? DateUpdate { get; set; }
        public string UserName { get; set; }
        public IEnumerable<WorkShopItem> WorkShopItems { get; set; }
    }

    public class ModelCompanyItem:BaseModelSimple
    {
        public IEnumerable<CompanyItem> ListItems { get; set; }
    }
}
