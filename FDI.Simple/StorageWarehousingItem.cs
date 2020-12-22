using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.CORE;

namespace FDI.Simple
{
    public class StorageWarehousingItem:BaseSimple
    {
        public string Code { get; set; }
        public decimal? DateCreated { get; set; }
        public decimal? DateRecive { get; set; }
        public int? HoursRecive { get; set; }
        public Guid? UserID { get; set; }
        public string Note { get; set; }
        public int? AgencyId { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsDelete { get; set; }
        public decimal? DateImport { get; set; }
        public decimal? Payment { get; set; }
        public Guid? UserActive { get; set; }
        public decimal? DateActive { get; set; }
        public bool? IsActive { get; set; }
        public int Status { get; set; }
        public string UsernameCreate { get; set; }
        public string UsernameActive { get; set; }
        public bool? IsOrder { get; set; }
        public Guid? KeyGuid { get; set; }
        public string Mobile { get; set; }
        public decimal? PrizeMoney { get; set; }
        public int? MarketID { get; set; }
        public string Martketname { get; set; }
        public string Areaname { get; set; }
        public  string UrlConfirm { get; set; }
        public  string CustomerName { get; set; }
        public string AgencyName { get; set; }
        public int? CustomerId { get; set; }

        public string StatusTxt
        {
            get
            {
                var b = (StatusWarehouse) Status;
                return b.GetDisplayName();
            }
        }
        public IEnumerable<UserItem> Users { get; set; }
        public IEnumerable<DNRequestWareHouseItem> LstImport { get; set; }
        public IEnumerable<DNRequestWareHouseActiveItem> LstImportActive { get; set; }
    }
    
    public class ModelStorageWarehousingItem : BaseModelSimple
    {
        public IEnumerable<StorageWarehousingItem> ListItems { get; set; }

    }

    public class ModelStorageWarehousingActiveItem:BaseModelSimple
    {
        public IEnumerable<ListCateActiveItem> ListCateValueItems { get; set; }
    }

    public class ListCateActiveItem
    {
        public int? ID { get; set; }
        public string Productname { get; set; }
        public decimal? DateCreate { get; set; }
        public int? Hours { get; set; }
        public string Fullname { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? DateActive { get; set; }
        public decimal? DateRecive { get; set; }
        public string Useractive { get; set; }
        public decimal? Prizemoney { get; set; }
    }
    public class StorageWarehousingItemNew:BaseSimple
    {
        public string Note { get; set; }
    }
}
