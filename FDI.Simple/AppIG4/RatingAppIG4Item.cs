using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class RatingAppIG4Item : BaseSimple
    {
        public int? ProductId { get; set; }
        public int ShopId { get; set; }
        public int? CustomerId { get; set; }
        public int Quantity { get; set; }
        public int TypeRating { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual IEnumerable<PictureItem> Gallery_Picture { get; set; }
        
    }
  
}
