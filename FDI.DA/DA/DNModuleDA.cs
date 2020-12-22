using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;
namespace FDI.DA
{
    public partial class DNModuleDA : BaseDA
    {
        #region Constructer

        public DNModuleDA(string pathPaging)
        {
            PathPaging = pathPaging;

        }

        public DNModuleDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;

        }
        #endregion

        public DN_Module GetById(int id)
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID == id
                        select c;
            return query.FirstOrDefault();
        }

        public List<ST_Group> ListST_GroupByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.ST_Group where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        #region các function lấy đệ quy

        /// <summary>
        /// Lấy về cây có tổ chức
        /// </summary>
        /// <param name="ltsSource">Toàn bộ danh mục</param>
        /// <param name="moduleIDRemove"> </param>
        /// <param name="checkShow"> </param>
        /// <returns></returns>
        public List<ModuleItem> GetAllSelectList(List<ModuleItem> ltsSource, int moduleIDRemove, bool checkShow)
        {
            if (checkShow)
                ltsSource = ltsSource.Where(o => o.IsShow == true).ToList();
            var ltsConvert = new List<ModuleItem>
                                 {
                                     new ModuleItem
                                         {
                                             ID = 1,
                                             NameModule = "Thư mục gốc"
                                         }
                                 };

            BuildTreeListItem(ltsSource, 1, string.Empty, moduleIDRemove, ref ltsConvert);
            return ltsConvert;
        }

        /// <summary>
        /// Build cây đệ quy
        /// </summary>
        /// <param name="ltsItems"></param>
        /// <param name="rootID"> </param>
        /// <param name="space"></param>
        /// <param name="moduleIDRemove"> </param>
        /// <param name="ltsConvert"></param>
        private void BuildTreeListItem(IEnumerable<ModuleItem> ltsItems, int rootID, string space, int moduleIDRemove, ref List<ModuleItem> ltsConvert)
        {
            space += "---";
            var ltsChils = ltsItems.Where(o => o.PrarentID == rootID && o.ID != moduleIDRemove).OrderBy(o => o.Ord).ToList();
            foreach (var currentItem in ltsChils)
            {
                currentItem.NameModule = string.Format("|{0} {1}", space, currentItem.NameModule);
                ltsConvert.Add(currentItem);
                BuildTreeListItem(ltsItems, currentItem.ID, space, moduleIDRemove, ref ltsConvert);
            }
        }

        /// <summary>
        /// Hàm build ra treeview có checkbox chứa danh sách category
        /// </summary>
        public void BuildTreeViewCheckBox(List<ModuleItem> ltsSource, int moduleID, bool checkShow, List<int> ltsValues, ref StringBuilder treeViewHtml)
        {
            var tempModule = ltsSource.OrderBy(o => o.Ord).Where(m => m.PrarentID == moduleID && m.ID > 1);
            if (checkShow)
                tempModule = tempModule.Where(m => m.IsShow == true);

            foreach (var module in tempModule)
            {
                var countQuery = ltsSource.Where(m => m.PrarentID == module.ID && m.ID > 1);
                if (checkShow)
                    countQuery = countQuery.Where(m => m.IsShow == true);
                var totalChild = countQuery.Count();
                if (totalChild > 0)
                {
                    treeViewHtml.Append("<li title=\"" + module.Content + "\" class=\"unselect\" id=\"" + module.ID.ToString() + "\"><span class=\"folder\"> <input id=\"Category_" + module.ID + "\" name=\"Category_" + module.ID + "\" value=\"" + module.ID + "\" type=\"checkbox\" title=\"" + module.NameModule + "\" " + (ltsValues.Contains(module.ID) ? " checked" : string.Empty) + "/> ");
                    if (!module.IsShow == true)
                        treeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(module.NameModule) + "</strike>");
                    else
                        treeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(module.NameModule));
                    treeViewHtml.Append("</span>\r\n");
                    treeViewHtml.Append("<ul>\r\n");
                    BuildTreeViewCheckBox(ltsSource, module.ID, checkShow, ltsValues, ref treeViewHtml);
                    treeViewHtml.Append("</ul>\r\n");
                    treeViewHtml.Append("</li>\r\n");
                }
                else
                {
                    treeViewHtml.Append("<li title=\"" + module.Content + "\" class=\"unselect\" id=\"" + module.ID.ToString() + "\"><span class=\"file\"> <input id=\"Category_" + module.ID + "\" name=\"Category_" + module.ID + "\" value=\"" + module.ID + "\" type=\"checkbox\" title=\"" + module.NameModule + "\" " + (ltsValues.Contains(module.ID) ? " checked" : string.Empty) + "/> ");
                    if (!module.IsShow == true)
                        treeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(module.NameModule) + "</strike>");
                    else
                        treeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(module.NameModule));
                    treeViewHtml.Append("</span></li>\r\n");
                }
            }
        }


        /// <summary>
        /// Hàm build ra treeview chứa danh sách category
        /// </summary>
        public void BuildTreeView(List<ModuleItem> ltsSource, int moduleID, bool checkShow, ref StringBuilder treeViewHtml)
        {
            var tempModule = ltsSource.OrderBy(o => o.Ord).Where(m => m.PrarentID == moduleID && m.ID > 1);
            if (checkShow)
                tempModule = tempModule.Where(m => m.IsShow == true);

            foreach (var module in tempModule)
            {
                var countQuery = ltsSource.Where(m => m.PrarentID == module.ID && m.ID > 1);
                if (checkShow)
                    countQuery = countQuery.Where(m => m.IsShow == true);
                var totalChild = countQuery.Count();
                if (totalChild > 0)
                {
                    treeViewHtml.Append("<li title=\"" + module.Content + "\" class=\"unselect\" id=\"" + module.ID.ToString() + "\"><span class=\"folder\"><a class=\"tool\" href=\"javascript:;\">");
                    if (!module.IsShow == true)
                        treeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(module.NameModule) + "</strike>");
                    else
                        treeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(module.NameModule));
                    treeViewHtml.Append("</a>\r\n");
                    treeViewHtml.AppendFormat(" <i>({0})</i>\r\n", totalChild);
                    treeViewHtml.Append(buildEditToolByID(module) + "\r\n");
                    treeViewHtml.Append("</span>\r\n");
                    treeViewHtml.Append("<ul>\r\n");
                    BuildTreeView(ltsSource, module.ID, checkShow, ref treeViewHtml);
                    treeViewHtml.Append("</ul>\r\n");
                    treeViewHtml.Append("</li>\r\n");
                }
                else
                {
                    treeViewHtml.Append("<li title=\"" + module.Content + "\" class=\"unselect\" id=\"" + module.ID.ToString() + "\"><span class=\"file\"><a class=\"tool\" href=\"javascript:;\">");
                    if (!module.IsShow == true)
                        treeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(module.NameModule) + "</strike>");
                    else
                        treeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(module.NameModule));
                    treeViewHtml.Append("</a> <i>(0)</i>" + buildEditToolByID(module) + "</span></li>\r\n");
                }
            }
        }

        /// Replace for upper function
        /// <summary>
        /// Build ra editor cho từng FAQCategoryItem
        /// </summary>
        /// <returns></returns>
        private string buildEditToolByID(ModuleItem moduleItem)
        {
            var strTool = new StringBuilder();
            strTool.Append("<div class=\"quickTool\">\r\n");
            //strTool.AppendFormat("    <a title=\"Xem tính năng: {1}\" class=\"showModule\" href=\"#{0}\">\r\n", moduleItem.Id, moduleItem.NameModule);
            //strTool.Append("        <img border=\"0\" title=\"Xem tính năng\" src=\"/Content/Admin/images/gridview/show.gif\">\r\n");
            //strTool.Append("    </a>");

            strTool.AppendFormat("    <a title=\"Gán Role: {1}\" data-event=\"rolemodule\" href=\"#{0}\">\r\n", moduleItem.ID, moduleItem.NameModule);
            strTool.Append("       <i class=\"fa fa-users icon\"></i>");
            strTool.Append("    </a>");

            strTool.AppendFormat("    <a title=\"Gán User: {1}\" data-event=\"usermodule\" href=\"#{0}\">\r\n", moduleItem.ID, moduleItem.NameModule);
            strTool.Append("       <i class=\"fa fa-user\"></i>");
            strTool.Append("    </a>");


            strTool.AppendFormat("<a title=\"Thêm mới Module: {1}\" data-event=\"add\" href=\"#{0}\">\r\n",
                                 moduleItem.ID, moduleItem.NameModule);
            strTool.Append("       <i class=\"fa fa-plus\"></i>");
            strTool.Append("    </a>");

            strTool.AppendFormat("    <a title=\"Chỉnh sửa: {1}\" data-event=\"edit\" href=\"#{0}\">\r\n", moduleItem.ID, moduleItem.NameModule);
            strTool.Append("       <i class=\"fa fa-pencil-square-o\"></i>");
            strTool.Append("    </a>");

            if (moduleItem.IsShow != null && moduleItem.IsShow.Value)
            {
                strTool.AppendFormat("    <a title=\"Ẩn: {1}\" href=\"#{0}\" data-event=\"hide\">\r\n", moduleItem.ID, moduleItem.NameModule);
                strTool.Append("<i class=\"fa fa-minus-circle\"></i>");
                strTool.Append("    </a>\r\n");

            }
            else
            {
                strTool.AppendFormat("    <a title=\"Hiển thị: {1}\" href=\"#{0}\" data-event=\"show\">\r\n",
                                     moduleItem.ID, moduleItem.NameModule);
                strTool.Append("<i class=\"fa fa-eye\"></i>");
                strTool.Append("    </a>\r\n");

            }

            strTool.AppendFormat("    <a title=\"Xóa: {1}\" href=\"#{0}\" data-event=\"delete\">\r\n", moduleItem.ID,
                                 moduleItem.NameModule);
            strTool.Append("       <i class=\"fa fa-trash-o\"></i>");
            strTool.Append("    </a>\r\n");

            strTool.AppendFormat("    <a title=\"Sắp xếp các Module con: {1}\" href=\"#{0}\" data-event=\"sort\">\r\n",
                                 moduleItem.PrarentID, moduleItem.NameModule);
            strTool.Append("       <i class=\"fa fa-sort\"></i>");
            strTool.Append("    </a>\r\n");

            strTool.Append("</div>\r\n");
            return strTool.ToString();
        }
        #endregion
        public List<TreeViewItem> GetListTreeNew(string lstId, int agencyId = 0)
        {
            var ltsArrId = FDIUtils.StringToListInt(lstId);
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsDelete == false && c.IsShow == true && c.Agency_Module.Any(v => v.AgencyId == agencyId && (v.IsShow.HasValue && v.IsShow.Value))
                        orderby c.Level, c.Ord
                        select new TreeViewItem
                        {
                            ID = c.ID,
                            Name = c.NameModule,
                            ParentId = c.ParentID,
                            Level = c.Level,
                            IsShow = ltsArrId.Any(m => m == c.ID),
                            Count = c.DN_Module1.Count(m => m.IsDelete == false && m.ParentID == c.ID && m.Agency_Module.Any(v => v.AgencyId == agencyId && (v.IsShow.HasValue && v.IsShow.Value)))
                            //Count = FDIDB.DN_Module.Count(m => m.IsDelete == false && m.ParentID == c.ID && m.Agency_Module.Any(v => v.AgencyId == agencyId && (v.IsShow.HasValue && v.IsShow.Value)))
                        };
            return query.ToList();
        }

        public List<RouterItem> GetRouter()
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsDelete == false
                        select new RouterItem
                        {
                            Controller = c.Link,
                            ParentID = c.ParentID == 1 ? c.ID : c.ParentID,
                            ID = c.ID
                        };
            return query.ToList();
        }
        public List<TreeViewItem> GetListAdminTree()
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsDelete == false
                        orderby c.Level, c.Ord
                        select new TreeViewItem
                        {
                            ID = c.ID,
                            Name = c.NameModule,
                            ParentId = c.ParentID,
                            IsShow = c.IsShow,
                            Count = c.DN_Module1.Count(m => m.IsDelete == false)
                        };
            return query.ToList();
        }

        public List<TreeViewItem> GetListTree(int agencyid)
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsDelete == false && c.IsShow == true && c.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value)) && c.DN_Module2.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value))
                        orderby c.Level, c.Ord
                        select new TreeViewItem
                        {
                            ID = c.ID,
                            Name = c.NameModule,
                            ParentId = c.ParentID,
                            IsShow = c.Agency_Module.Where(m => m.AgencyId == agencyid).Select(m => m.IsShow).FirstOrDefault(),
                            Count = c.DN_Module1.Count(m => m.IsDelete == false && m.ParentID == c.ID && m.Agency_Module.Any(v => v.AgencyId == agencyid && (v.IsShow.HasValue && v.IsShow.Value)))
                        };
            return query.ToList();
        }

        public List<ModuleItem> GetAllListSimple(int agencyid)
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsShow == true && c.IsDelete == false && c.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value))
                        orderby c.Ord
                        select new ModuleItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Tag = c.Link.ToLower(),
                            Ord = c.Ord,
                            PrarentID = c.ParentID,
                            Content = c.Content,
                            IsShow = c.IsShow,
                            IsShowAgency = c.Agency_Module.Where(m => m.AgencyId == agencyid).Select(m => m.IsShow).FirstOrDefault()
                        };
            return query.ToList();
        }

        public List<ModuleItem> GetListItemByParentID()
        {
            var query = from c in FDIDB.DN_Module
                        where c.IsShow == true && c.IsDelete == false
                        orderby c.Level, c.Ord
                        select new ModuleItem
                        {
                            ID = c.ID,
                            NameModule = c.NameLevel + c.NameModule,
                        };
            return query.ToList();
        }

        public List<ModuleItem> ListItemByParentID(int prarentID)
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsShow == true && c.IsDelete == false && c.ParentID == prarentID
                        orderby c.Level, c.Ord
                        select new ModuleItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Ord = c.Ord,
                            PrarentID = c.ParentID.Value,
                            Level = c.Level
                        };
            return query.ToList();
        }

        public List<ModuleItem> GetAllListSimpleByParentID(int parentID, int agencyid)
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsShow == true && c.ParentID == parentID && c.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value))
                        orderby c.Ord
                        select new ModuleItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Tag = c.Link,
                            Ord = c.Ord,
                            PrarentID = c.ParentID.Value,
                            Content = c.Content,
                            IsShow = c.IsShow != null && c.IsShow.Value
                        };
            return query.ToList();
        }

        public List<ModuleItem> GetListByParentID(int parentID, int agencyid, Guid UserId, List<int> listrole)
        {
            var query1 = from c in FDIDB.DN_Module
                         where c.ID > 1 && c.IsShow == true
                         && c.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value))
                         && (c.ParentID == parentID || c.ID == parentID)
                         && c.DN_Users.Any(v => v.UserId == UserId)
                         orderby c.Ord descending
                         select new ModuleItem
                         {
                             ID = c.ID,
                             NameModule = c.NameModule,
                             Tag = c.Link,
                             PrarentID = c.ParentID
                         };
            if (query1.Any())
            {
                return query1.ToList();
            }
            var query2 = from c in FDIDB.DN_Module
                         where c.ID > 1 && c.IsShow == true
                         && c.Agency_Module.Any(m => m.AgencyId == agencyid)
                         && (c.ParentID == parentID || c.ID == parentID)
                         && (c.DN_Roles.Any(v => listrole.Contains(v.ID)) || c.DN_Module2.DN_Roles.Any(v => listrole.Contains(v.ID)))
                         orderby c.Ord descending
                         select new ModuleItem
                         {
                             ID = c.ID,
                             NameModule = c.NameModule,
                             Tag = c.Link,
                             PrarentID = c.ParentID
                         };
            return query2.ToList();
        }

        public List<ModuleItem> GetListByParentIdAdmin(int parentID, int agencyid)
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsShow == true && (!c.IsDelete.HasValue || !c.IsDelete.Value)
                        && c.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value))
                        && (c.ParentID == parentID || c.ID == parentID)
                        orderby c.Ord descending
                        select new ModuleItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Tag = c.Link,
                            PrarentID = c.ParentID,
                            Ord = c.Ord
                        };
            return query.ToList();
        }
        public List<ModuleItem> DNModule_GetChildByParentId(int parentID, int agencyid)
        {
            var query = from c in FDIDB.DNModule_GetChildByParentId(parentID, agencyid, true)
                        select new ModuleItem
                        {
                            ID = c.Id.Value,
                            NameModule = c.Name,
                            Tag = c.Link,
                            PrarentID = c.Parent,
                        };
            return query.ToList();
        }


        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <param name="isShow"> </param>
        /// <returns></returns>
        public List<ModuleItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow, int agencyid)
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID > 1 && c.IsShow == isShow && c.NameModule.StartsWith(keyword) && c.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value))
                        orderby c.NameModule
                        select new ModuleItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Tag = c.Link,
                            Ord = c.Ord,
                            PrarentID = c.ParentID.Value,
                            Content = c.Content,
                            IsShow = c.IsShow != null && c.IsShow.Value
                        };
            return query.Take(showLimit).ToList();
        }

        public List<DN_Module> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.DN_Module where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<Agency_Module> GetListAgencyModuleByArrID(List<int?> ltsArrID, int agencyid)
        {
            var query = from c in FDIDB.Agency_Module where c.AgencyId == agencyid && ltsArrID.Contains(c.ModuleId) select c;
            return query.ToList();
        }

        public List<DN_Active> GetListActive()
        {
            var query = from c in FDIDB.DN_Active
                        select c;
            return query.ToList();
        }

        public List<DN_Users> GetListUserbyListGuid(List<Guid> lstGuid)
        {
            var query = from c in FDIDB.DN_Users
                        where lstGuid.Contains(c.UserId)
                        select c;
            return query.ToList();
        }

        public List<DN_Roles> GetListRolesbyListGuid(List<Guid> lstGuid)
        {
            var query = from c in FDIDB.DN_Roles
                        where lstGuid.Contains(c.RoleId)
                        select c;
            return query.ToList();
        }

        public DN_Module GetByID(int id)
        {
            var query = from c in FDIDB.DN_Module where c.ID == id select c;
            return query.FirstOrDefault();
        }

        public ModuleItem GetItemById(int id)
        {
            var query = from c in FDIDB.DN_Module
                        where c.ID == id
                        select new ModuleItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            PrarentID = c.ParentID,
                            Ord = c.Ord,
                            Tag = c.Link,
                            ClassCss = c.ClassCss,
                            Content = c.Content,
                            IsShow = c.IsShow,
                            Level = c.Level,
                            DN_Module = new ModuleadminItem
                            {
                                PrarentID = c.DN_Module2.ParentID
                            },
                            Role_ModuleActive = c.DN_Role_ModuleActive.Select(m => new Role_ModuleActiveItem
                            {
                                RoleId = m.RoleId
                            }),
                            User_ModuleActive = c.DN_User_ModuleActive.Select(m => new UserModuleActiveItem
                            {
                                UserId = m.UserId
                            })
                        };
            return query.FirstOrDefault();
        }

        public DN_Roles GetByRoleId(Guid roleId)
        {
            var query = from c in FDIDB.DN_Roles
                        where c.RoleId == roleId
                        select c;
            return query.FirstOrDefault();
        }

        public DN_Users GetByUserId(Guid id)
        {
            var query = from c in FDIDB.DN_Users
                        where c.UserId == id
                        select c;
            return query.FirstOrDefault();
        }

        public void DeleteModuleUserRole(int id, string listuser, string listrole)
        {
            FDIDB.DeleteModuleUserRole(id, listuser, listrole);
        }

        public List<ActionActiveItem> GetlistByTagUserId(string tag, Guid userId, List<int> listrole, int agencyid, int parentId, int moduleId)
        {
            var id = FDIDB.DN_Module.Where(c => c.ID > 1 && c.ParentID == parentId && (!c.IsDelete.HasValue || !c.IsDelete.Value) && c.Link == tag && c.Agency_Module.Any(v => v.AgencyId == agencyid && (v.IsShow.HasValue && v.IsShow.Value))).Select(c => c.ID).FirstOrDefault();
            var query1 = from c in FDIDB.DN_User_ModuleActive
                         where c.Active == true && c.UserId == userId && c.AgencyId == agencyid
                         && c.ModuleId == id                         
                         select new ActionActiveItem
                         {
                             ID = c.ID,
                             NameActive = c.DN_Active.NameActive,
                             ModuleId = c.ModuleId
                         };
            if (query1.Any())
            {
                return query1.GroupBy(m => m.NameActive).Select(m => m.FirstOrDefault()).ToList();
            }

            var obj = FDIDB.DN_Module.Where(c => c.ID > 1 && (!c.IsDelete.HasValue || !c.IsDelete.Value) && c.Link == tag && c.Agency_Module.Any(v => v.AgencyId == agencyid && (v.IsShow.HasValue && v.IsShow.Value)));
            var lst = obj.Select(c => c.ID);
            var lstParent = obj.Select(c => c.ParentID);            
            var query2 = from c in FDIDB.DN_Role_ModuleActive
                         where c.Active == true && listrole.Contains(c.DN_Roles.ID) && c.AgencyId == agencyid
                         && (lst.Contains(c.ModuleId ?? 0) || lstParent.Contains(c.DN_Module.ID))
                         select new ActionActiveItem
                         {
                             ID = c.ID,
                             NameActive = c.DN_Active.NameActive,
                             ModuleId = c.ModuleId
                         };
            return query2.GroupBy(m => m.NameActive).Select(m => m.FirstOrDefault()).ToList();
        }

        public void Add(DN_Module module)
        {
            FDIDB.DN_Module.Add(module);
        }

        public void AddItemByIAll(int moduleid, string listid)
        {
            FDIDB.AddItemByIsAll(moduleid, listid);
        }

        public void DeleteItemByIAll(int moduleid, string lid)
        {
            FDIDB.DeleteItemByIAll(moduleid, lid);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="module"> </param>

        public void Delete(DN_Module module)
        {
            FDIDB.DN_Module.Remove(module);
        }
        public void Delete(DN_User_ModuleActive module)
        {
            FDIDB.DN_User_ModuleActive.Remove(module);
        }
        public void Delete(DN_Role_ModuleActive module)
        {
            FDIDB.DN_Role_ModuleActive.Remove(module);
        }
        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            FDIDB.SaveChanges();
        }

        public List<ModuleadminItem> GetAllListSimpleItems(int agencyid)
        {
            var query = from c in FDIDB.DN_Module
                        where c.IsShow == true && (!c.IsDelete.HasValue || !c.IsDelete.Value) && c.Agency_Module.Any(m => (m.IsShow.HasValue && m.IsShow.Value) && m.AgencyId == agencyid)
                        orderby c.Ord
                        select new ModuleadminItem
                        {
                            ID = c.ID,
                            NameModule = c.NameModule,
                            Tag = c.Link,
                            ClassCss = c.ClassCss,
                            PrarentID = c.ParentID,
                            Content = c.Content,
                        };
            return query.ToList();
        }

        public List<ModuleadminItem> GetAllModuleByUserName(Guid userId, int agencyid)
        {
            var query1 = from c in FDIDB.DN_Module
                         where c.ParentID == 1 && (!c.IsDelete.HasValue || !c.IsDelete.Value) && (c.IsShow.HasValue && c.IsShow.Value) && c.DN_Users.Any(m => m.UserId == userId) && c.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value))
                         orderby c.Level, c.Ord
                         select new ModuleadminItem
                         {
                             ID = c.ID,
                             NameModule = c.NameModule,
                             Tag = c.Link,
                             ClassCss = c.ClassCss,
                             PrarentID = c.ParentID,
                             Content = c.Content
                         };
            if (query1.Any())
            {
                return query1.ToList();
            }

            var query2 = from c in FDIDB.DN_Module
                         where c.ParentID == 1 && c.IsShow == true && (!c.IsDelete.HasValue || !c.IsDelete.Value)
                         && c.Agency_Module.Any(m => m.AgencyId == agencyid && (m.IsShow.HasValue && m.IsShow.Value))
                         && c.DN_Roles.Any(r => r.DN_UsersInRoles.Any(m => m.DN_Users.UserId == userId && (!m.IsDelete.HasValue || !m.IsDelete.Value)))
                         orderby c.Level, c.Ord
                         select new ModuleadminItem
                         {
                             ID = c.ID,
                             NameModule = c.NameModule,
                             Tag = c.Link,
                             ClassCss = c.ClassCss,
                             PrarentID = c.ParentID,
                             Content = c.Content,
                         };

            return query2.ToList();
        }
    }
}