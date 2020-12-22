using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA.DA.DN_Sales
{
    public class DNDiscountDA:BaseDA
    {
        #region Constructer
        public DNDiscountDA()
        {
        }

        public DNDiscountDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNDiscountDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<DNDiscountItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Discount
                        where (agencyId == 0 || c.AgencyId == agencyId) && (!c.IsDeleted.HasValue || c.IsDeleted == false) && c.IsShow == true
                        orderby c.ID descending
                        select new DNDiscountItem()
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Price = c.Price,
                            Percent = c.Percent,
                            AgencyId = c.AgencyId,
                            IsAgency = c.IsAgency,
                            IsAll = c.IsAll,
                            TotalOrder = c.TotalOrder,

                        };
            var agent = httpRequest["agency"];
            if (!string.IsNullOrEmpty(agent))
            {
                var id = int.Parse(agent);
                query = query.Where(c => c.AgencyId == id);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public DNDiscountItem GetDNDiscountItem(int id)
        {
            var query = from c in FDIDB.DN_Discount
                        where c.ID == id && c.IsDeleted == false
                        orderby c.ID descending
                        select new DNDiscountItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            Price = c.Price,
                            Percent = c.Percent,
                            AgencyId = c.AgencyId,
                            IsAgency = c.IsAgency,
                            IsAll = c.IsAll,
                            IsMonth = c.IsMonth,
                            IsDay = c.IsDay,
                            TotalOrder = c.TotalOrder,
                        };
            return query.FirstOrDefault();
        }
        public List<DN_Discount> GetListByArrId(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Discount where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public DN_Discount GetById(int promotion)
        {
            var query = from c in FDIDB.DN_Discount where c.ID == promotion select c;
            return query.FirstOrDefault();
        }
        public void Add(DN_Discount item)
        {
            FDIDB.DN_Discount.Add(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
