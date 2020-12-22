using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    [Serializable]
    public class StackItem
    {
        public int ID { get; set; }
        public decimal? Date { get; set; }
        public string Json { get; set; }
        public decimal? AgencyID { get; set; }
    }
    public class StackValueItem
    {
        public int I { get; set; }
        public int U { get; set; }
    }
    public class ModelStackItem
    {
        public IEnumerable<AnalysisItem> ListItem { get; set; }
    }
}
