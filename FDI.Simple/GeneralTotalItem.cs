using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    [Serializable]
    public class GeneralTotalItem
    {
        // Thu từ kế toán
        public int Month { get; set; }
        public decimal TotalReceipt { get; set; }
        //Thu DH thu ngân
        public decimal TotalOrder { get; set; }
        //Chi
        public decimal TotalPayment { get; set; }
        //Ứng
        public decimal TotalCash { get; set; }
        public decimal TotalRepay { get; set; }
    }

    public class GeneralItem
    {
        public decimal? Price { get; set; }
        public decimal? Date { get; set; }
    }

    public class GeneralVoteItem
    {
        public decimal? Value { get; set; }
        public decimal? Date { get; set; }
        public Guid? GuiId { get; set; }
    }
    public class ModelGeneralVoteItem
    {
        // Thu từ kế toán
        public List<GeneralVoteItem> ListItems { get; set; }
        public List<MonthItem> LstMonthItem { get; set; }
        public List<DNUserSimpleItem> ListUser { get; set; }        
    }
}
