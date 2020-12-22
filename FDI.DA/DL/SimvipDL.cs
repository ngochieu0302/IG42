using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DL
{
    public class SimvipDL:BaseDA
    {
        public List<SimvipItem> GetListHome()
        {
            var query = from c in FDIDB.Simvips
                        where  c.IsDelete == false && c.IsShow == true && c.IsShowHome == true
                        orderby c.ID descending
                        select new SimvipItem
                        {
                            ID = c.ID,
                            Icon = c.Icon,
                            IsShowHome = c.IsShowHome,
                            Name = c.Name,
                            Categories = c.Categories.Where(a=>a.IsDeleted == false && a.IsShow == true).Select(z => new CategoryItem
                            {
                                Slug = z.Slug,
                                ID = z.Id
                            })
                        };
            return query.ToList();
        }
        public SimvipItem GetByid(int id)
        {
            var query = from c in FDIDB.Simvips
                        where c.ID == id
                        select new SimvipItem
                        {
                            ID = c.ID,
                            Icon = c.Icon,
                            IsShowHome = c.IsShowHome,
                            Name = c.Name,
                            
                        };
            return query.FirstOrDefault();
        }
    }
}
