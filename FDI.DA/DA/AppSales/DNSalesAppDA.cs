using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNSalesAppDA : BaseDA
    {
        #region Constructer
        public DNSalesAppDA()
        {
        }

        public DNSalesAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNSalesAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public ImportProductItem GetObjOne(string keword, int agencyId)
        {
            var date = DateTime.Now.TotalSeconds();
            var name = FomatString.Slug(keword);
            var query = from c in FDIDB.DN_ImportProduct
                        where c.BarCode == name 
                        //&& c.AgencyId == agencyId 
                        && c.Quantity > c.QuantityOut
                        select new ImportProductItem
                        {
                            
                            GID = c.GID,
                            BarCode = c.BarCode,
                            PriceNew = c.PriceNew,
                            Name = c.Product_Value.Shop_Product_Detail.Name,
                            Price = c.Price,
                            Date = c.Date,
                            DateEnd = c.DateEnd,
                            Quantity = 1,
                            Value = c.Value,
                            IsDate = true,//c.Date < date && c.DateEnd >= date
                            UrlPicture = c.Product_Value.Shop_Product_Detail.Gallery_Picture.Folder + c.Product_Value.Shop_Product_Detail.Gallery_Picture.Url,
                        };
            return query.Any() ? query.FirstOrDefault() : new ImportProductItem();
        }

        public CustomerItem GetObjCustomerAuto(string keword, int agencyId)
        {
            const int stt = (int)Card.Released;
            var query = from c in FDIDB.Customers
                        where c.IsActive == true && c.IsDelete == false && ((c.DN_Card.Code == keword && c.DN_Card.Status == stt) || c.Phone.Contains(keword) || c.FullName.Contains(keword))
                        select new CustomerItem
                        {
                            ID = c.ID,
                            Address = c.Address,
                            FullName = c.FullName,
                            Phone = c.Phone,
                            Birthday = c.Birthday,
                            IsQa = c.DN_Card.Code == keword,
                        };
            return query.FirstOrDefault();
        }
        public List<ImportProductItem> GetListImportProductBarcode(IList<string> lstBarcode, int agencyid)
        {
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_ImportProduct
                        where lstBarcode.Contains(c.BarCode) && c.IsDelete == false
                        && c.Quantity > c.QuantityOut 
                        //&& c.Date <= date && c.DateEnd >= date
                        //&& c.AgencyId == agencyid
                        select new ImportProductItem
                        {
                            GID = c.GID,
                            BarCode = c.BarCode,
                            Quantity = c.Quantity,
                            Price = c.Price,
                            PriceNew = c.PriceNew,
                            Value = c.Value,
                            ProductValueID = c.Product_Value.ProductID
                        };
            return query.ToList();
        }
        public void AddOrder(Shop_Orders item)
        {
            FDIDB.Shop_Orders.Add(item);
        }
    }
}
