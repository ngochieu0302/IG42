using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;
using System;
using FDI.CORE;

namespace FDI.DA
{
    public class SupplierDA : BaseDA
    {
        #region Constructer

        public SupplierDA()
        {
        }

        public SupplierDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public SupplierDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion

        public List<DNSupplierItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int Agencyid)
        {

            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Supplier
                        orderby o.ID descending
                        select new DNSupplierItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Mobile = o.Mobile,
                            Address = o.Address,
                            Email = o.Email,
                            IsLook = o.IsLook,
                            DateCreate = o.DateCreate,
                            //DateImport = o.Supplier_Product.Select(m=>m.DateCreated).FirstOrDefault()
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<DNSupplierItem> GetListSimpleStaticByRequest(HttpRequestBase httpRequest, int areaId)
        {

            Request = new ParramRequest(httpRequest);
            var daten = DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Supplier
                        where o.StorageProducts.Any(c => c.SupID == o.ID && c.DateImport > daten && c.AreaID == areaId)
                        orderby o.ID descending
                        select new DNSupplierItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Mobile = o.Mobile,
                            Address = o.Address,
                            Email = o.Email,
                            IsLook = o.IsLook,
                            DateCreate = o.DateCreate,
                            LstStorageProductItems = o.StorageProducts
                                .Where(a => a.IsDelete == false && a.AreaID == areaId && a.DateImport > daten).Select(v =>
                                    new StorageProductItem
                                    {
                                        Catename = v.Category.Name,
                                        Quantity = v.Quantity,
                                        DateImport = v.DateImport,
                                        HourImport = v.HoursImport,
                                        Price = v.Price,
                                        TotalPrice = v.TotalPrice
                                    })
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<DNSupplierItem> GetListSimpleGeneralDebt(HttpRequestBase httpRequest, int Agencyid)
        {

            Request = new ParramRequest(httpRequest);
            var id = httpRequest.QueryString["SupplierID"];
            var query = from o in FDIDB.DN_Supplier
                        orderby o.ID descending
                        select new DNSupplierItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Mobile = o.Mobile,
                            Address = o.Address,
                            //TotalPriceOrder = o.Order_Debt.Sum(c=>c.Pricetotal),
                            //TotalRecive = o.Debts.Where(a=>a.Type == 1).Sum(c=>c.Price),
                            //TotalRewar = o.Debts.Where(a=>a.Type == 2).Sum(c=>c.Price),
                            //TotalDebt = o.Debts.Sum(c => c.Price),
                        };
            if (!string.IsNullOrEmpty(id))
            {
                var idsup = int.Parse(id);
                query = query.Where(c => c.ID == idsup);
            }

            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<SupplieItem> GetList()
        {
            var date = DateTime.Today.TotalSeconds();
            var query = from o in FDIDB.DN_Supplier
                        where !o.IsLook.HasValue || !o.IsLook.Value
                        //&& o.Supplier_Product.All(m => m.DateCreated >= date)
                        orderby o.ID descending
                        select new SupplieItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Mobile = o.Mobile,
                            Address = o.Address
                        };
            return query.ToList();
        }

        public DN_Supplier GetById(int id)
        {
            var query = from c in FDIDB.DN_Supplier where c.ID == id select c;
            return query.FirstOrDefault();
        }

        //public void DeleteAll(int id)
        //{
        //    FDIDB.sp_DeleteSupplierProduct(id);
        //}
        public DNSupplierItem GetItemById(int id)
        {
            var daten = DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Supplier
                        where o.ID == id
                        select new DNSupplierItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Mobile = o.Mobile,
                            Note = o.Note,
                            Address = o.Address,
                            Description = o.Description,
                            Email = o.Email,
                            IsLook = o.IsLook,
                            LstStorageProductItems = o.StorageProducts.Where(a => a.IsDelete == false && a.DateImport > daten)
                                .Select(v => new StorageProductItem
                                {
                                    Catename = v.Category.Name,
                                    Quantity = v.Quantity,
                                    DateImport = v.DateImport,
                                    HourImport = v.HoursImport,
                                    Price = v.Price,
                                    TotalPrice = v.TotalPrice
                                })
                        };
            return query.FirstOrDefault();
        }

        public List<DNSupplierItem> GetListSimpleByRequestExcel(HttpRequestBase httpRequest, int agencyid)
        {

            Request = new ParramRequest(httpRequest);

            var query = from o in FDIDB.DN_Supplier
                        orderby o.ID descending
                        select new DNSupplierItem
                        {
                            ID = o.ID,
                            Name = o.Name + " " + o.Mobile,
                            Mobile = o.Mobile,
                            Address = o.Address,
                            Email = o.Email,
                            IsLook = o.IsLook,
                            DateCreate = o.DateCreate,
                            //DateImport = o.Supplier_Product.Select(m=>m.DateCreated).FirstOrDefault()
                        };
            return query.ToList();
        }

        public List<DN_Supplier> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Supplier
                        where ltsArrId.Contains(o.ID)
                        select o;
            return query.ToList();
        }

        public List<SupplieItem> GetByName(string name)
        {
            return FDIDB.DN_Supplier.Where(m => m.Name.Contains(name))
                .Select(m => new SupplieItem() { ID = m.ID, Name = m.Name }).ToList();
        }

        public void AddProduct(DN_SupplierProduct data)
        {
            FDIDB.DN_SupplierProduct.Add(data);
        }

        public void Add(DN_Supplier obj)
        {
            FDIDB.DN_Supplier.Add(obj);
        }

        public void Delete(DN_Supplier obj)
        {
            FDIDB.DN_Supplier.Remove(obj);
        }

        public List<SupplieProductItem> GetListSupplierProductById(HttpRequestBase httpRequest, int id)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_SupplierProduct
                        where o.SupplierId == id
                        orderby o.Id
                        select new SupplieProductItem
                        {
                            ID = o.Id,
                            CategoryName = o.Category.Name,
                            Quantity = o.Amount
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public SupplieProductItem GetSupplierProductById(int id)
        {
            var query = from o in FDIDB.DN_SupplierProduct
                        where o.Id == id
                        orderby o.Id
                        select new SupplieProductItem
                        {
                            ID = o.Id,
                            CategoryName = o.Category.Name,
                            Quantity = o.Amount,
                            SupplierId = o.SupplierId,
                            CateId = o.ProductId
                        };

            return query.FirstOrDefault();
        }

        public bool CheckExistSupplierProduct(int categoryId, int supplierId)
        {
            return FDIDB.DN_SupplierProduct.Any(m =>
                m.ProductId == categoryId && m.SupplierId == supplierId && !m.IsDelete);
        }

        public List<DNSupplierItem> GetList(IList<int> ids)
        {
            return FDIDB.DN_Supplier.Where(m => ids.Contains(m.ID)).Select(m => new DNSupplierItem()
            {
                ID = m.ID,
                Mobile = m.Mobile,
                Name = m.Name,
                Address = m.Address
            }).ToList();
        }

        public List<SupplieItem> GetSupplierByProductIds(IList<int> productIds)
        {
            var query = from o in FDIDB.DN_Supplier
                        where o.SupplierAmountProducts.Any(m => productIds.Any(n => n == m.ProductID))
                        orderby o.ID
                        select new SupplieItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Address = o.Address,
                            Mobile = o.Mobile
                        };

            return query.ToList();
        }

        public List<SupplieItem> GetItems(int enterpriseId)
        {
            var daten = DateTime.Now.TotalSeconds();
            var query = from o in FDIDB.DN_Supplier
                where o.EnterpriseID == enterpriseId && (o.IsDeleted == null || o.IsDeleted == false)
                        select new SupplieItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            Mobile = o.Mobile,
                            Address = o.Address,
                            Email = o.Email,
                        };
            return query.ToList();
        }
    }
}
