using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.Simple
{
    public class DebtItem : BaseSimple
    {
        public int ID { get; set; }
        public int? SupplierID { get; set; }
        public decimal? Price { get; set; }
        public decimal? DateCreated { get; set; }
        public Guid? UserCreated { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public string SupplieName { get; set; }
        public int? Type { get; set; }
        public int? AgencyId { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Supplier DN_Supplie { get; set; }
    }
    public class ModelDebtItem : BaseModelSimple
    {
        public IEnumerable<DebtItem> ListItem { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class FormDebtItem : BaseModelSimple
    {
        public DebtItem ObjItem { get; set; }
        public bool IsAdmin { get; set; }
        public int AgencyId { get; set; }
        public Guid UserId { get; set; }
        public List<SupplieItem> DNSuppliers { get; set; }
        //public List<CostTypeItem> CostTypeItems { get; set; }
    }
}
