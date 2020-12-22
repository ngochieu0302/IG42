using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple.StorageWarehouse
{
    public class ProduceCatogoryItem
    {
        public int ProduceId { get; set; }
        public string ProductOriginalCode { get; set; }
        public int ProductId { get; set; }
        public decimal Weight { get; set; }
        public  string Code { get; set; }
        public string IdLog { get; set; }
    }
}
