using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA.AppCustomer
{
    public class AgencyAppDA : BaseDA
    {
        #region Contruction

        public AgencyAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public AgencyAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public List<DNAgencyAppItem> ListAgencyByKm(double x, double y, int id, int aid, int km)
        {
            var date = DateTime.Now.TotalSeconds();
            if (aid > 0)
            {
                var query1 = from c in FDIDB.DN_Agency
                             where c.ID == aid
                             select new DNAgencyAppItem
                             {
                                 ID = c.ID,
                                 Name = c.Name,
                                 Latitute = c.Latitute,
                                 Longitude = c.Longitude,
                                 Address = c.Address,
                             };
                return query1.ToList();
            }

            var query = from c in FDIDB.ListAgencyByKm(km, x, y, id, date)
                        select new DNAgencyAppItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Latitute = c.Latitute,
                            Longitude = c.Longitude,
                            Address = c.Address,
                            Km = c.Km
                        };
            return query.ToList();
        }
        public DNAgencyAppItem GetAgencyItemByPhone(string phone)
        {
            var query = from c in FDIDB.DN_Agency
                        where (!c.IsDelete.HasValue || !c.IsDelete.Value) && c.Phone == phone
                        orderby c.ID descending
                        select new DNAgencyAppItem
                        {
                            ID = c.ID,
                            Name = c.FullName,
                            Mobile = c.Phone,
                            Address = c.Address,
                            Latitute = c.Latitute,
                            Longitude = c.Longitude
                        };
            return query.FirstOrDefault();
        }
        public DNAgencyAppItem GetById(int id)
        {
            var _dimg = Utility._dimg;
            var date = DateTime.Now.TotalSeconds();
            var query = from c in FDIDB.DN_Agency
                        where c.ID == id && (!c.IsDelete.HasValue || !c.IsDelete.Value)
                        select new DNAgencyAppItem
                        {
                            ID = c.ID,
                            Name = c.FullName,
                            Mobile = c.Phone,
                            Latitute = c.Latitute,
                            Longitude = c.Longitude,
                            Address = c.Address,
                            ListItem = FDIDB.Shop_Product_Detail.Where(m => m.Product_Value.Any(p => p.AgencyId == id && p.DN_ImportProduct.Any(i => i.DateEnd > date && i.Quantity > i.QuantityOut))).Select(m => new ProductValueAppItem
                            {
                                ID = m.ID,
                                Name = m.Name,
                                UrlImage = _dimg + m.Gallery_Picture.Folder + m.Gallery_Picture.Url,
                                Price = m.Price,
                                TotalQuantity = m.Product_Value.Where(p => p.AgencyId == id).Sum(p => p.DN_ImportProduct.Where(i => i.AgencyId == id && i.DateEnd > date && i.Quantity > i.QuantityOut).Sum(i => (i.Quantity - i.QuantityOut) * i.Value))
                            })
                        };
            return query.FirstOrDefault();
        }
        public List<ProductAppItem> GetAll()
        {
            var query = from c in FDIDB.Shop_Product_Detail
                        where (!c.IsDelete.HasValue || !c.IsDelete.Value)
                        select new ProductAppItem
                        {
                            
                            Name = c.Name,
                        };
            return query.ToList();
        }
        //public void Add(DN_Login dnLogin)
        //{
        //    FDIDB.DN_Login.Add(dnLogin);
        //}
        //public void Save()
        //{
        //    FDIDB.SaveChanges();
        //}
    }
}
