using System.Collections.Generic;

namespace FDI.Simple
{
    public class RateCityItem
    {
        public string CityName { set; get; }

        public IEnumerable<RateItem> ListRateItem { get; set; }
    }

    public class ModelRateCityItem
    {
        public IEnumerable<RateCityItem> ListRateCityItem { get; set; }
        public string DateUpdated { set; get; }
    }
}