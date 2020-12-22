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

namespace FDI.DA.DA.DN_Sales
{
    public class DNSalesDA : BaseDA
    {
        #region Constructer
        public DNSalesDA()
        {
        }

        public DNSalesDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNSalesDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<DNSalesItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Sale
                        where c.AgencyId == agencyId && (!c.IsDeleted.HasValue || c.IsDeleted == false) && c.IsShow == true
                        orderby c.ID descending
                        select new DNSalesItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            DateEnd = c.DateEnd,
                            DateStart = c.DateStart,
                            Price = c.Price,
                            Percent = c.Percent,
                            AgencyId = c.AgencyId,
                            Username = c.DN_Users.UserName,
                            IsAgency = c.IsAgency,
                            IsAll = c.IsAll,
                            QuantityCode = c.QuantityCode,

                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<SaleCodeItem> GetListSimpleByRequestCode(HttpRequestBase httpRequest, int agencyId, int id)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.SaleCodes
                        where c.SaleID == id
                        orderby c.ID descending
                        select new SaleCodeItem
                        {
                            ID = c.ID,
                            DateUser = c.DateUse,
                            IsUser = c.IsUse,
                            Code = c.Code,
                        };
            var use = httpRequest["IsUser_"];
            if (!string.IsNullOrEmpty(use))
            {
                var isUse = use != "0";
                query = query.Where(c => c.IsUser == isUse);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public DNSalesItem GetDNSalesItem(int id)
        {
            var query = from c in FDIDB.DN_Sale
                        where c.ID == id && c.IsDeleted == false
                        orderby c.ID descending
                        select new DNSalesItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            DateEnd = c.DateEnd,
                            DateStart = c.DateStart,
                            Price = c.Price,
                            Percent = c.Percent,
                            AgencyId = c.AgencyId,
                            Username = c.DN_Users.UserName,
                            IsAgency = c.IsAgency,
                            IsAll = c.IsAll,
                            QuantityCode = c.QuantityCode,
                            IsMonth = c.IsMonth,
                            IsDay = c.IsDay,
                            PriceLimit = c.PriceLimit,
                            TotalOrder = c.TotalOrder,
                            TotalUse = c.TotalUse,
                            Type = c.Type,
                            ListProductDetailItems = c.Shop_Product_Detail.Where(a => !a.IsDelete.HasValue || a.IsDelete == false && a.IsShow == true).Select(v => new ShopProductDetailItem
                            {
                                ID = v.ID,
                                Name = v.Name,
                                Code = v.Code,
                                Price = v.Price,
                                UrlPicture = v.Gallery_Picture.Folder + v.Gallery_Picture.Url
                            })
                        };
            return query.FirstOrDefault();
        }
        public List<Shop_Product_Detail> GetListIntProductByArrId(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Shop_Product_Detail where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        #region Get Sale
        public ModelPSaleItem GetSaleProduct(int id, int agencyid)
        {
            const int type = (int)TypeEnumSale.Product;
            var date = DateTime.Now.TotalSeconds();
            var modelS = new ModelPSaleItem
            {
                Sale = from o in FDIDB.DN_Sale
                       where (!o.IsEnd.HasValue || !o.IsEnd.Value) && o.IsShow == true && (!o.IsDeleted.HasValue || !o.IsDeleted.Value)
                       && (!o.PriceLimit.HasValue || o.PriceLimit == 0 || (o.PriceLimit - (o.TotalUse ?? 0) > 0)) && o.Type == type
                       && (!o.IsAgency.HasValue || !o.IsAgency.Value || o.AgencyId == agencyid) && o.DateStart < date && o.DateEnd > date &&
                       (o.IsAll == true || o.Shop_Product_Detail.Any(k => k.ID == id) || o.Categories.Any(k => k.Shop_Product_Detail.Any(p => p.ID == id)) || o.Categories.Any(k => k.Category1.Any(c => c.Shop_Product_Detail.Any(p => p.ID == id))))
                       select new Sale
                       {
                           Price = o.Price,
                           PercentSale = o.Percent,
                           IsAll = o.IsAll,
                           QuantityCode = o.QuantityCode
                       }
            };
            return modelS;
        }
        public ModelPSaleItem GetSaleBirthday(decimal birthday, int agencyid)
        {
            var date = DateTime.Today;
            var dateN = date.TotalSeconds();
            var dateNow = DateTime.Now.TotalSeconds();
            var dateNe = date.AddDays(1).TotalSeconds();
            var dateM = new DateTime(date.Year, date.Month, 1);
            var dateMs = dateM.TotalSeconds();
            var dateMe = dateM.AddMonths(1).TotalSeconds();
            bool checkd = dateN >= birthday && dateNe < birthday;
            bool checkm = dateMs >= birthday && dateMe < birthday;
            const int type = (int)TypeEnumSale.Birthday;
            var modelS = new ModelPSaleItem
            {
                Sale = from o in FDIDB.DN_Sale
                       where (!o.IsEnd.HasValue || !o.IsEnd.Value) && o.IsShow == true && (!o.IsDeleted.HasValue || !o.IsDeleted.Value)
                       && (!o.PriceLimit.HasValue || o.PriceLimit == 0 || (o.PriceLimit - (o.TotalUse ?? 0) > 0))
                       && (!o.IsAgency.HasValue || !o.IsAgency.Value || o.AgencyId == agencyid) && o.DateStart < dateNow && o.DateEnd > dateNow
                       && ((o.IsMonth == true && checkm) || (o.IsDay == true && checkd)) && o.Type == type
                       select new Sale
                       {
                           Price = o.Price,
                           PercentSale = o.Percent,
                       }
            };
            return modelS;
        }
        public List<Sale> GetSaleByTotalOrderBirthday(decimal totalorder, decimal birthday, int agencyid)
        {
            var date = DateTime.Today;
            var dateN = date.TotalSeconds();
            var dateNow = DateTime.Now.TotalSeconds();
            var dateNe = date.AddDays(1).TotalSeconds();
            var dateM = new DateTime(date.Year, date.Month, 1);
            var dateMs = dateM.TotalSeconds();
            var dateMe = dateM.AddMonths(1).TotalSeconds();
            bool checkd = dateN >= birthday && dateNe < birthday;
            bool checkm = dateMs >= birthday && dateMe < birthday;
            const int typeb = (int)TypeEnumSale.Birthday;
            const int type = (int)TypeEnumSale.Order;
            var models = from o in FDIDB.DN_Sale
                         where (!o.IsEnd.HasValue || !o.IsEnd.Value) && o.IsShow == true && (!o.IsDeleted.HasValue || !o.IsDeleted.Value)
                         && (!o.PriceLimit.HasValue || o.PriceLimit == 0 || (o.PriceLimit - (o.TotalUse ?? 0) > 0))
                         && (!o.IsAgency.HasValue || !o.IsAgency.Value || o.AgencyId == agencyid) && o.DateStart < dateNow && o.DateEnd > dateNow
                         && ((o.TotalOrder <= totalorder && o.Type == type) || (((o.IsMonth == true && checkm) || (o.IsDay == true && checkd)) && o.Type == typeb))
                         select new Sale
                         {
                             Name = o.Name,
                             Price = o.Price,
                             PercentSale = o.Percent,
                         };
            return models.ToList();
        }
        public List<Sale> GetSaleByTotalOrder(decimal totalorder, int agencyid)
        {
            var dateNow = DateTime.Now.TotalSeconds();
            const int type = (int)TypeEnumSale.Order;
            var models = from o in FDIDB.DN_Sale
                         where (!o.IsEnd.HasValue || !o.IsEnd.Value) && o.IsShow == true && (!o.IsDeleted.HasValue || !o.IsDeleted.Value)
                         && (!o.PriceLimit.HasValue || o.PriceLimit == 0 || (o.PriceLimit - (o.TotalUse ?? 0) > 0))
                         && (!o.IsAgency.HasValue || !o.IsAgency.Value || o.AgencyId == agencyid) && o.DateStart < dateNow && o.DateEnd > dateNow
                         && ((o.TotalOrder <= totalorder && o.Type == type))
                         select new Sale
                         {
                             Name = o.Name,
                             Price = o.Price,
                             PercentSale = o.Percent,
                         };
            return models.ToList();
        }
        public SaleCodeItem CheckSaleCode(string code, int agencyid)
        {
            var datetime = DateTime.Now.TotalSeconds();
            const int type = (int)TypeEnumSale.Voucher;
            var query = from o in FDIDB.SaleCodes
                        where (!o.DN_Sale.IsEnd.HasValue || !o.DN_Sale.IsEnd.Value) && o.DN_Sale.IsShow == true && (!o.DN_Sale.IsDeleted.HasValue || !o.DN_Sale.IsDeleted.Value)
                       && (!o.DN_Sale.PriceLimit.HasValue || o.DN_Sale.PriceLimit == 0 || (o.DN_Sale.PriceLimit - (o.DN_Sale.TotalUse ?? 0) > 0)) && o.DN_Sale.Type == type
                       && (!o.DN_Sale.IsAgency.HasValue || !o.DN_Sale.IsAgency.Value || o.DN_Sale.AgencyId == agencyid) && o.DN_Sale.DateStart < datetime && o.DN_Sale.DateEnd > datetime
                       && o.Code == code && (!o.IsUse.HasValue || o.IsUse == false)
                        select new SaleCodeItem
                        {
                            Percent = o.DN_Sale.Percent,
                            PriceSale = o.DN_Sale.Price,
                        };
            return query.FirstOrDefault();
        }
        public SaleCodeItem GetDNSalesItembyCode(string code, int agencyid)
        {
            var datetime = DateTime.Now.TotalSeconds();
            const int type = (int)TypeEnumSale.Voucher;
            var query = from o in FDIDB.SaleCodes
                        where (!o.DN_Sale.IsEnd.HasValue || !o.DN_Sale.IsEnd.Value) 
                        && o.DN_Sale.IsShow == true 
                        && (!o.DN_Sale.IsDeleted.HasValue || !o.DN_Sale.IsDeleted.Value)
                       && (!o.DN_Sale.PriceLimit.HasValue || o.DN_Sale.PriceLimit == 0 || (o.DN_Sale.PriceLimit - (o.DN_Sale.TotalUse ?? 0) > 0)) 
                       && o.DN_Sale.Type == type
                       && (o.DN_Sale.IsAgency == true || o.DN_Sale.AgencyId == agencyid) 
                       && o.DN_Sale.DateStart < datetime 
                       && o.DN_Sale.DateEnd > datetime
                       && o.Code == code 
                       && (!o.IsUse.HasValue || o.IsUse == false)
                        select new SaleCodeItem
                        {
                            Percent = o.DN_Sale.Percent,
                            PriceSale = o.DN_Sale.Price,
                        };
            return query.FirstOrDefault();
        }
        #endregion
        public SaleCode GetSaleCodebyCode(string code)
        {
            var query = from c in FDIDB.SaleCodes
                        where c.Code == code
                        select c;
            return query.FirstOrDefault();
        }
        public List<DN_Sale> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Sale where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public DN_Sale GetById(int promotion)
        {
            var query = from c in FDIDB.DN_Sale where c.ID == promotion select c;
            return query.FirstOrDefault();
        }
        public List<Category> GetListCateByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.Categories
                        where ltsArrId.Contains(o.Id)
                        select o;
            return query.ToList();
        }
        public void Add(DN_Sale item)
        {
            FDIDB.DN_Sale.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
