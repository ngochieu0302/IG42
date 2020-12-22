using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class CateAppDA : BaseDA
    {
        #region Contruction

        public CateAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public CateAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion

        public List<CateAppItem> GetListApp(int type)
        {
            var query1 = from c in FDIDB.Categories
                         where (!c.IsDeleted.HasValue || c.IsDeleted == false) && c.IsShowApp == true
                               && c.Type == type
                         select new CateAppItem
                         {
                             ID = c.Id,
                             img = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                             PId = c.ParentId,
                             n = c.Name,
                         };

            return query1.ToList();

        }
        public List<CateAppItem> GetCategorysParentId(int categoryId)
        {
            var query = from c in FDIDB.Categories
                where c.ParentId == categoryId
                select new CateAppItem
                {
                    ID = c.Id,
                    n = c.Name,
                };
            return query.ToList();
        }
    }
}