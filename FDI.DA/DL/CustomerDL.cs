using System.Linq;
using FDI.Simple;
using System.Collections.Generic;

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
        
    }
}