using System.Linq;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class ProductValueDA : BaseDA
    {
        public void Add(Product_Value item)
        {
            FDIDB.Product_Value.Add(item);
        }

        public ProductValueItem GetByCode(string code)
        {
            return FDIDB.Product_Value.Where(m => m.IdLog == code).Select(m => new ProductValueItem()
            {
                ID = m.ID,
                ProductId = m.ProduceId ?? 0

            }).FirstOrDefault();
        }
    }
}
