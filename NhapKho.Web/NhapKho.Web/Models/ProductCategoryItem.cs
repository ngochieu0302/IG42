using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.CORE;
using FDI.Utils;

namespace NhapKho.Web.Models
{
    public class ProductCales
    {
        public int ProductId { get; set; }
        public decimal Weight { get; set; }
        public string Code { get; set; }
        public decimal DateCreate { get; set; }

        public string DateStr { get; set; }
        public decimal DateExpire { get; set; }
        public string DateExpireStr { get; set; }
        public string Name { get; set; }
        public decimal PriceUnit { get; set; }

        public string PriceStr => Price.Money();

        public decimal Price => Weight/1000 * PriceUnit;

        public override bool Equals(Object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            var temp = (ProductCales)obj;
            return temp.Code == Code;
        }
    }

    public class ProductCalesModel
    {
        public string Code { get; set; }
        public string IdLog { get; set; }
        public List<ProductCales> Items { get; set; }
    }
}