using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FDI.Simple.Order
{
    public class StorageWarehousingRequest
    {
        public string Code { get; set; }
        public int AgencyId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal ReceiveDate { get; set; }
        public string UrlConfirm { get; set; }

        public string CustomerCode { get; set; }
        public ICollection<RequestWare> RequestWares { get; set; }

    }

    public class RequestWare
    {
        public int CateId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? QuantityActive { get; set; }
        public IList<RequestWareDetail> Details { get; set; }

    }

    public class RequestWareDetail
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Weight { get; set; }
    }
}

