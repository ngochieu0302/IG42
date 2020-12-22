using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class BiasProduceDA : BaseDA
    {
        public BiasProduceDA()
        {
        }
        public BiasProduceDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public BiasProduceDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        public List<BiasProduceItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.BiasProduces
                        where c.AgencyID == agencyid && c.IsDeleted == false
                        orderby c.ID descending
                        select new BiasProduceItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            EndDate = c.EndDate,
                            StartDate = c.StartDate,
                            Quantity = c.Quantity,
                            ProductName = c.Shop_Product.Shop_Product_Detail.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<ProductCodeItem> GetListProductCode(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var itemid = httpRequest["codeId"] ?? "0";
            var id = int.Parse(itemid);
            var query = from c in FDIDB.ProductCodes
                        where c.BiasProduceID == id
                        orderby c.ID
                        select new ProductCodeItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            EndDate = c.EndDate,
                            StartDate = c.StartDate,
                            DateCreated = c.DateCreated,
                            Note = c.Note,
                            Status = c.Status,
                            BiasProduceID = c.BiasProduceID
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CostProductUserItem> GetListCostProductUser(int biasId)
        {
            var query = from c in FDIDB.Cost_Product_User
                        where c.BiasProduceID == biasId
                        orderby c.ID
                        select new CostProductUserItem
                        {
                            ID = c.ID,
                            UserId = c.UserId,
                            UserName = c.DN_Users.LoweredUserName,
                            SetupProductName = c.SetupProduction.Name,
                            BiasProduceID = c.BiasProduceID
                        };
            //query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<CostProductCostUserItem> GetListEvaluate(int id)
        {
            var query = from c in FDIDB.ProductCode_CostUser
                        where c.ProductCodeID == id
                        orderby c.ID
                        select new CostProductCostUserItem
                        {
                            ID = c.ID,
                            Status = c.Status,
                            DateCreated = c.DateCreated,
                            UserName = c.Cost_Product_User.DN_Users.LoweredUserName,
                            SetupProductName = c.Cost_Product_User.SetupProduction.Name,
                            UserCreatedName = c.DN_Users.LoweredUserName,
                            Note = c.Note
                        };
            return query.ToList();
        }
        public BiasProduceItem GetBiasProduceItem(int id)
        {
            var query = from c in FDIDB.BiasProduces where c.ID == id select new BiasProduceItem
            {
                ID = c.ID,
                Name = c.Name,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ProductID = c.ProductID,
                ProductName = c.Shop_Product.Shop_Product_Detail.Name,
                Quantity = c.Quantity,
                LstCostProducts = from u in c.Cost_Product
                                  select new CostProductItem
                                  {
                                      BiasProduceID = u.BiasProduceID,
                                      Percent = u.Percent,
                                      SetupProductID = u.SetupProductId,
                                      SetupProductName = u.SetupProduction.Name
                                  },
                LstCostProductUserItems = from u in c.Cost_Product_User
                                          select new CostProductUserItem
                                          {
                                              BiasProduceID = u.BiasProduceID,
                                              Percent = u.Percent,
                                              UserName = u.DN_Users.UserName,
                                              FullName = u.DN_Users.LoweredUserName,                                              
                                              SetupProductID = u.SetupProductId,
                                              SetupProductName = u.SetupProduction.Name,
                                              UserId = u.UserId
                                          }
            };
            return query.FirstOrDefault();
        }
        public BiasProduce GetById(int id)
        {
            var query = from o in FDIDB.BiasProduces where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<BiasProduce> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.BiasProduces where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public List<ProductCode> GetListProductCode(List<int> lst)
        {
           var query = from c in FDIDB.ProductCodes where lst.Contains(c.ID)select c;
            return query.ToList();
        }
        public void Add(ProductCode_CostUser item)
        {
            FDIDB.ProductCode_CostUser.Add(item);
        }
        public void Add(BiasProduce item)
        {
            FDIDB.BiasProduces.Add(item);
        }
        public void Delete(Cost_Product_User item)
        {
            FDIDB.Cost_Product_User.Remove(item);
        }public void Delete(Cost_Product item)
        {
            FDIDB.Cost_Product.Remove(item);
        }public void Delete(BiasProduce item)
        {
            FDIDB.BiasProduces.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
