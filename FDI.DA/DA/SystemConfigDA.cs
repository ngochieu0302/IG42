using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public class SystemConfigDA : BaseDA
    {
        #region Constructer
        public SystemConfigDA()
        {
        }

        public SystemConfigDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public SystemConfigDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<SystemConfigItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.System_Config 
                        orderby o.ID descending 
                        where o.LanguageId==LanguageId
                        select new SystemConfigItem
                                   {
                                       ID = o.ID,
                                       Name = o.Name.Trim(),
                                       IsShow = o.IsShow,
                                       Email = o.Email,
                                       Address = o.Address

                                   };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public System_Config GetById(int id)
        {
            var query = from c in FDIDB.System_Config where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public List<System_Config> GetListByArrId(List<int> ltsArrID)
        {
            var query = from c in FDIDB.System_Config where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public void Add(System_Config systemConfig)
        {
            FDIDB.System_Config.Add(systemConfig);
        }

        public void Delete(System_Config systemConfig)
        {
            FDIDB.System_Config.Remove(systemConfig);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
