using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class PolicyAgencyDA : BaseDA
    {

        public List<PolicyAgenciesItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.PolicyAgencies
                        orderby c.ID
                        where (!c.Isdelete)
                        select new PolicyAgenciesItem
                        {
                            ID = c.ID,
                            CategoryId = c.CategoryId,
                            Formula = c.Formula,
                            LevelAgency = c.LevelAgency,
                            Profit = c.Profit,
                            Quantity = c.Quantity,
                            PercentProfit = c.PercentProfit??0
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<PolicyAgenciesItem> GetAll(int categoryId)
        {
            var query = from c in FDIDB.PolicyAgencies
                        orderby c.ID
                        where !c.Isdelete && c.CategoryId == categoryId
                        select new PolicyAgenciesItem
                        {
                            ID = c.ID,
                            CategoryId = c.CategoryId,
                            Formula = c.Formula,
                            LevelAgency = c.LevelAgency,
                            Profit = c.Profit,
                            Quantity = c.Quantity,
                            PercentProfit = c.PercentProfit??0
                        };
            return query.ToList();
        }

        public void Add(PolicyAgency item)
        {
            FDIDB.PolicyAgencies.Add(item);
        }

        public PolicyAgenciesItem GetItemById(int id)
        {
            var query = from c in FDIDB.PolicyAgencies
                orderby c.ID
                where !c.Isdelete && c.ID == id
                select new PolicyAgenciesItem
                {
                    ID = c.ID,
                    CategoryId = c.CategoryId,
                    Formula = c.Formula,
                    LevelAgency = c.LevelAgency,
                    Profit = c.Profit,
                    Quantity = c.Quantity,
                    PercentProfit = c.PercentProfit??0
                };
            return query.FirstOrDefault();
        }

        public PolicyAgency GetById(int id)
        {
            return FDIDB.PolicyAgencies.FirstOrDefault(m => m.ID == id);
        }
    }
}
