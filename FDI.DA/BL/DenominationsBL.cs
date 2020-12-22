using System.Collections.Generic;
using System.Linq;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DenominationsBL : BaseBL
    {
        private readonly DenominationsDL _dl = new DenominationsDL();

        public DenominationsItem GetByPricesToPrice(decimal PriceS, decimal PriceE)
        {
            var key = "DenominationsBL_GetByPricesToPrice" + PriceS + "_" + PriceE;
            if (ConfigCache.EnableCache != 1)
                return _dl.GetByPricesToPrice(PriceS, PriceE);
            if (Cache.KeyExistsCache(key))
            {
                var lst = (DenominationsItem)Cache.GetCache(key);
                if (lst != null) return lst;
                Cache.DeleteCache(key);
                var data = _dl.GetByPricesToPrice(PriceS, PriceE);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
            else
            {
                var data = _dl.GetByPricesToPrice(PriceS, PriceE);
                Cache.Set(key, data, ConfigCache.TimeExpire);
                return data;
            }
        }
    }
}