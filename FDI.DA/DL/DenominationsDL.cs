using System.Collections.Generic;
using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class DenominationsDL : BaseDA
    {
        public DenominationsItem GetByPricesToPrice(decimal PriceS, decimal PriceE)
        {
            var query = from n in FDIDB.Denominations
                        where n.PriceS == PriceS && n.PriceE == PriceE
                        orderby n.ID descending
                        select new DenominationsItem
                        {
                            ID = n.ID,
                            Title = n.Title,
                            Keywords = n.Keywords,
                            Description = n.Description,
                        };
            return query.FirstOrDefault();
        }
    }
}