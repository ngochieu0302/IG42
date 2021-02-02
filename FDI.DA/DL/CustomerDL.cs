using System.Linq;
using FDI.Simple;
using System.Collections.Generic;
using FDI.Utils;

namespace FDI.DA
{
    public class CustomerDL : BaseDA
    {
        public CustomerItem GetByid(int id)
        {
            var query = from c in FDIDB.Customers
                        where c.ID == id
                        select new CustomerItem
                        {
                            ID = c.ID,
                            FullName = c.FullName,
                            Phone = c.Phone,
                            Birthday = c.Birthday,
                            Gender = c.Gender
                        };
            return query.FirstOrDefault();
        }
        public List<CustomerAppIG4Item> ListByMap(int km, float la, float lo)
        {
            var query = from c in FDIDB.Customers
                        where c.Type == 2 && (km == 0 || ConvertUtil.DistanceBetween(la, lo, (float)c.Latitude, (float)c.Longitude) <= km)
                        orderby c.Ratings descending
                        select new CustomerAppIG4Item
                        {
                            ID = c.ID,
                            Fullname = c.FullName,
                            Address = c.Address,
                            Ratings = c.Ratings,
                            AvgRating = c.AvgRating,
                            LikeTotal = c.LikeTotal,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                            ImageTimeline = c.ImageTimeline,
                            Description = c.Description,
                            ListPId = c.Shop_Product_Detail.Where(m => !m.IsDelete.HasValue || !m.IsDelete.Value).Select(m => m.ID)
                        };
            return query.ToList();
        }
    }
}