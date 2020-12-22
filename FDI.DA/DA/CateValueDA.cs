using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class CateValueDA : BaseDA
    {
        #region Constructer
        public CateValueDA()
        {
        }

        public CateValueDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CateValueDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<CateValueItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int Agencyid)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Cate_Value
                        where o.IsDelete == false && o.AgencyId == Agencyid
                        orderby o.ID descending
                        select new CateValueItem
                        {
                            ID = o.ID,
                            //Name = o.Name.Trim(),
                            //Date = o.Date,
                            //Quantity = o.Quantity,
                            //IsShow = o.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public DNRequestWareItem GetObjOrder(int areaID)
        {
            var h = DateTime.Now.Hour;
            var time = 18;
            if (h < 4) time = 4;
            else if (h < 8) time = 8;
            else if (h < 15) time = 15;
            var datetoday = DateTime.Today.TotalSeconds();
            var query = from c in FDIDB.DN_RequestWare
                        where c.Today == datetoday && c.Hour == time && c.QuantityUsed < c.QuantityActive && c.AreaID == areaID
                        orderby c.MarketID, c.Date
                        select new DNRequestWareItem
                        {
                            GID = c.GID,
                            AgencyID = c.AgencyID
                        };
            return query.FirstOrDefault();
        }

        public CateValueItem GetByCode(string code)
        {
            var query = from c in FDIDB.Cate_Value
                where c.Code == code
                select new CateValueItem()
                {
                    ID = c.ID,
                    Code = c.Code,
                    
                };
            return query.FirstOrDefault();
        }

        public Cate_Value GetById(int id)
        {
            var query = from c in FDIDB.Cate_Value where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public int GetProductIDById(int id)
        {
            var query = from c in FDIDB.Shop_Product where c.ProductDetailID == id select c.ID;
            return query.FirstOrDefault();
        }
        public void Add(Cate_Value item)
        {
            FDIDB.Cate_Value.Add(item);
        }

        public void Delete(Cate_Value item)
        {
            FDIDB.Cate_Value.Remove(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
