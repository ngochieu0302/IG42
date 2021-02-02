using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.Supplier
{
    public class SupplierAmountProductItem : BaseSimple
    {
        public int SupplierId { get; set; }
        public int? ProductID { get; set; }
        [DataType(DataType.DateTime)]
        public decimal PublicationDate { get; set; }
        [DataType(DataType.DateTime)]
        public decimal? ExpireDate { get; set; }
        public bool IsAlwayExist { get; set; }
        public int AmountEstimate { get; set; }
        public int AmountPayed { get; set; }
        [DataType(DataType.DateTime)]
        public decimal? CallDate { get; set; }
        public Guid? UserActiveId { get; set; }
        public string Note { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierAdress { get; set; }

    }
    public class SupplierAmountProductFormItem : BaseSimple
    {
        public int? SupplierId { get; set; }
        public int? ProductID { get; set; }
        public string _PublicationDate { get; set; }
        public string _ExpireDate { get; set; }
        public bool? IsAlwayExist { get; set; }
        public int? AmountEstimate { get; set; }
        public int? AmountPayed { get; set; }
        public string _CallDate { get; set; }
        public Guid? UserActiveId { get; set; }
        public string Note { get; set; }

    }


    public class SupplierAmountProductResponse : BaseModelSimple
    {
        public IEnumerable<SupplierAmountProductItem> ListItem { get; set; }
    }

    public class CssAttribute : Attribute
    {
        public CssAttribute(string value)
        {
            Value = value;
        }
        public string Value { get; set; }

        public string GetValue()
        {
            return Value;
        }
    }
}
