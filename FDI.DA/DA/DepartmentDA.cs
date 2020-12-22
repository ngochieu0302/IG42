using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DepartmentDA : BaseDA
    {
        #region Constructer
        public DepartmentDA()
        {
        }

        public DepartmentDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DepartmentDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DepartmentItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.DN_Department 
                        where o.IsDelete == false && o.AgencyID == agencyId
                        orderby o.ID descending 
                        select new DepartmentItem
                        {
                                       ID = o.ID,
                                       Name = o.Name.Trim(),
                                       Description = o.Description,
                                       DateCreate = o.DateCreate,
                                       Sort = o.Sort,
                                       IsShow = o.IsShow
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public DN_Department GetById(int id)
        {
            var query = from o in FDIDB.DN_Department where o.ID == id select o;
            return query.FirstOrDefault();
        }

        public DepartmentItem GetDepartmentItemById(int id)
        {
            var query = from o in FDIDB.DN_Department
                        where o.ID == id
                        select new DepartmentItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Description = o.Description,
                            DateCreate = o.DateCreate,
                            Sort = o.Sort,
                            IsShow = o.IsShow
                        };
            return query.FirstOrDefault();
        }

        public List<DepartmentItem> GetAll()
        {
            var query = from o in FDIDB.DN_Department
                        where o.IsDelete == false
                        select new DepartmentItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Description = o.Description,
                            Sort = o.Sort,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public List<DepartmentItem> GetAll(int agencyId)
        {
            var query = from o in FDIDB.DN_Department where o.IsDelete == false
                        where  o.AgencyID == agencyId
                        select new DepartmentItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Description = o.Description,
                            Sort = o.Sort,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public List<DepartmentItem> GetListByArrId(string lstId)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from o in FDIDB.DN_Department
                        where ltsArrId.Contains(o.ID)
                        select new DepartmentItem
                        {
                            ID = o.ID,
                            Name = o.Name.Trim(),
                            Description = o.Description,
                            Sort = o.Sort,
                            IsShow = o.IsShow
                        };
            return query.ToList();
        }

        public List<DN_Department> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.DN_Department where lst.Contains(o.ID) select o;
            return query.ToList();
        }

        public void Add(DN_Department department)
        {
            FDIDB.DN_Department.Add(department);
        }

        public void Delete(DN_Department department)
        {
            FDIDB.DN_Department.Remove(department);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
