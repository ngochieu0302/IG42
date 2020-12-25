using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Simple.StorageWarehouse;

namespace FDI.DA.DA.StorageWarehouse
{
    public class ProduceDa : BaseDA
    {
        public void Add(Produce item)
        {
            FDIDB.Produces.Add(item);
        }

        public void DeleteMapDnRequest(MapProduceRequestWare mapping)
        {
            FDIDB.Entry(mapping).State = (System.Data.Entity.EntityState)EntityState.Deleted;
        }
        public void DeleteMapDnRequest(IList<MapProduceRequestWare> mappings)
        {
            for (int i = 0; i < mappings.Count; i++)
            {
                FDIDB.MapProduceRequestWares.Remove(mappings[0]);
            }
        }

        public List<DN_RequestWare> GetDNRequest(int produceId)
        {
            return FDIDB.DN_RequestWare.Where(m => m.MapProduceRequestWares.Any(n => n.ProduceID == produceId)).ToList();
        }

        public List<ProduceItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Produces
                        where o.Isdelete == false
                        orderby o.ID descending
                        select new ProduceItem()
                        {
                            ID = o.ID,
                            ProductId = o.ProductId,
                            Quantity = o.Quantity,
                            Status = (ProduceStatus)o.Status,
                            ProductName = o.Category.Name,
                            DateProduce = o.DateProduce
                        };

            query = query.SelectByRequest(Request, ref TotalRecord);

            return query.ToList();
        }

        public Produce GetById(int Id)
        {
            return FDIDB.Produces.FirstOrDefault(m => m.ID == Id);
        }
        public ProduceItem GetItemById(int Id)
        {
            var query = FDIDB.Produces.Where(m => m.ID == Id).Select(o =>
                  new ProduceItem()
                  {
                      ID = o.ID,
                      ProductId = o.ProductId,
                      Quantity = o.Quantity,
                      Status = (ProduceStatus)o.Status,
                      ProductName = o.Category.Name,
                      DateProduce = o.DateProduce
                  });
            return query.FirstOrDefault();
        }

        public List<CategoryRecipeItem> GetCategoryRecipe(int productId)
        {
            var query = from c in FDIDB.Category_Product_Recipe
                        where (c.IsDeleted == null || c.IsDeleted == false) && c.Category_Recipe.CategoryID == productId
                        select new CategoryRecipeItem()
                        {
                            ProductId = c.ProductId,
                            ProductName = c.Shop_Product_Detail.Name,
                            Quantity = c.Quantity
                        };
            return query.ToList();
        }

        public List<OrderDetailProductItem> GetDetail(int produceId)
        {
            var query = from c in FDIDB.ProduceProductDetails
                        where c.ProduceId == produceId
                        select new OrderDetailProductItem()
                        {
                            Quantity = c.Quantity,
                            ProductId = c.ProductId,
                            ProductName = c.Shop_Product_Detail.Name,
                            UnitName = c.Product_Size.Name,
                            Weight = c.Weight,
                            SizeId = c.SizeId
                        };
            return query.ToList();
        }
        public List<OrderDetailProductItem> GetDetail(int produceId, int productparent)
        {
            var query = from c in FDIDB.ProduceProductDetails
                        where c.ProduceId == produceId && c.ProductParentId == productparent

                        select new OrderDetailProductItem()
                        {
                            Quantity = c.Quantity,
                            ProductId = c.ProductId,
                            ProductName = c.Shop_Product_Detail.Name,
                            UnitName = c.Product_Size.Name,
                            Weight = c.Weight,
                            SizeId = c.SizeId,
                        };
            return query.ToList();
        }

        public ProduceItem GetProduceDetail(string code)
        {
            var query = from c in FDIDB.ProduceProductPrepares
                        where c.Isdelete == false && c.Code == code
                        select new ProduceItem()
                        {
                            ProductId = c.ProductId,
                            ID = c.ProduceId,

                        };
            return query.FirstOrDefault();
        }

        public ProduceItem GetProduceDetailByIdLog(string idlog)
        {
            return FDIDB.Product_Value.Where(m => m.IdLog == idlog).Select(m => new ProduceItem()
            {
                ID = m.ProduceId ?? 0,
                ProductId = m.ProductID ?? 0
            }).FirstOrDefault();
        }
    }
}
