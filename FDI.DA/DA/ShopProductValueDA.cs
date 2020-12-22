using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class ShopProductValueDA : BaseDA
    {
        public ShopProductValueDA()
        {
        }
        public ShopProductValueDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public ShopProductValueDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<ShopProductValueItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agecncy)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Shop_Product_Value
                        where o.AgencyId == agecncy && o.IsDeleted == false
                        orderby o.ID descending
                        select new ShopProductValueItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            AgencyId = o.AgencyId,
                            UnitName = o.DN_Unit.Name,
                            QuantityDay = o.QuantityDay,
                            Price = o.Price
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<ShopProductValueItem> GetList(int agencyId)
        {
            var query = from o in FDIDB.Shop_Product_Value
                        where o.AgencyId == agencyId && o.IsDeleted == false
                        orderby o.ID descending
                        select new ShopProductValueItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            AgencyId = o.AgencyId,
                            UnitName = o.DN_Unit.Name,
                            Quantity = o.Quantity,
                            Price = o.Price,
                            QuantityOut = o.QuantityOut,
                        };
            return query.ToList();
        }
        public ShopProductValueItem GetProductValueItem(int id)
        {
            var query = from o in FDIDB.Shop_Product_Value
                        where o.ID == id
                        orderby o.ID descending
                        select new ShopProductValueItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            QuantityDay = o.QuantityDay,
                            AgencyId = o.AgencyId,
                            Price = o.Price,
                            UnitName = o.DN_Unit.Name,
                            UnitId = o.UnitId
                        };
            return query.FirstOrDefault();
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit, int agencyId)
        {
            var query = from c in FDIDB.Shop_Product_Value
                        where c.Name.Contains(keword) && c.AgencyId == agencyId && c.IsDeleted == false
                        select new SuggestionsProduct
                        {
                            ID = c.ID,
                            value = c.Name,
                            title = c.Name,
                            code = c.Code,
                            QuantityDay = c.QuantityDay ?? 0,
                            data = c.DN_Unit.Name,
                            pricenew = c.Price
                        };
            return query.Take(showLimit).ToList();
        }
        public bool CheckByName(string name, int id, int agencyId)
        {
            return FDIDB.Shop_Product_Value.Any(c => c.ID != id && c.NameAscii == name && c.IsDeleted == false && c.AgencyId == agencyId);

        }
        public Shop_Product_Value GetById(int id)
        {
            var query = from o in FDIDB.Shop_Product_Value where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<Shop_Product_Value> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.Shop_Product_Value where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(Shop_Product_Value item)
        {
            FDIDB.Shop_Product_Value.Add(item);
        }
        public void Delete(Shop_Product_Value item)
        {
            FDIDB.Shop_Product_Value.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
