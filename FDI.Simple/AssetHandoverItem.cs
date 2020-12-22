using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class AssetHandoverItem : BaseSimple
    {
        public string Code { get; set; }
        public decimal? DateDelivery { get; set; }
        public Guid UserDelivery { get; set; }
        public string DeliveryName { get; set; }
        public Guid? UserReceiver { get; set; }
        public string ReceiverName { get; set; }
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class ModelAssetHandoverItem : BaseModelSimple
    {
        public List<AssetHandoverItem> ListItems { get; set; }
    }
}
