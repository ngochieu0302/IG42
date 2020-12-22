using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CusContactItem : BaseSimple
    {
        public int? Id { get; set; }
        public int? AId { get; set; }
        public string N { get; set; }
        public decimal? T { get; set; }
        public int? S { get; set; }
        public string A { get; set; }
        public string M { get; set; }
        public string La { get; set; }
        public string Lo { get; set; }
        public IEnumerable<CusContactDetaiItem> LItem { get; set; }
    }
    public class CusContactDetaiItem
    {
        public int? Id { get; set; }
        public int? PId { get; set; }
        public string N { get; set; }
        public decimal? Q { get; set; }
        public decimal? P { get; set; }
        public int? S { get; set; }
    }
    public class ContactDetaiAppItem
    {
        public int? PId { get; set; }
        public decimal Q { get; set; }
        public decimal? P { get; set; }
    }
    public class ModelCusContactItem : BaseModelSimple
    {
        public IEnumerable<CusContactItem> ListItem { get; set; }
    }

}
