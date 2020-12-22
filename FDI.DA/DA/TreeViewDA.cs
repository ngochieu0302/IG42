using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FDI.Simple;

namespace FDI.DA
{
    public class TreeViewDA
    {
        public void BuildTreeView(List<TreeViewItem> ltsSource, int menuId, bool checkShow, ref StringBuilder treeViewHtml, bool add, bool delete, bool edit, bool show, bool order)
        {
            var tempMenu = ltsSource.Where(m => m.ParentId == menuId && m.ID > 1);
            if (checkShow)
                tempMenu = tempMenu.Where(m => m.IsShow == checkShow);

            foreach (var menu in tempMenu)
            {
                var countQuery = ltsSource.Where(m => m.ParentId == menu.ID && m.ID > 1);
                if (checkShow)
                    countQuery = countQuery.Where(m => m.IsShow == checkShow);
                var totalChild = countQuery.Count();
                if (totalChild > 0)
                {
                    treeViewHtml.Append("<li class=\"unselect\" id=\"" + menu.ID + "\"><span class=\"folder\"><a class=\"tool\" data-id=" + menu.ID + " href=\"javascript:;\">");
                    if (menu.IsShow == false)
                        treeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(menu.Name) + "</strike>");
                    else
                        treeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(menu.Name));
                    treeViewHtml.Append("</a>\r\n");
                    treeViewHtml.AppendFormat(" <i>({0})</i>\r\n", totalChild);
                    treeViewHtml.Append(BuildEditToolById(menu, add, delete, edit, show, order) + "\r\n");
                    treeViewHtml.Append("</span>\r\n");
                    treeViewHtml.Append("<ul>\r\n");
                    BuildTreeView(ltsSource, menu.ID, checkShow, ref treeViewHtml, add, delete, edit, show, order);
                    treeViewHtml.Append("</ul>\r\n");
                    treeViewHtml.Append("</li>\r\n");
                }
                else
                {
                    treeViewHtml.Append("<li  class=\"unselect\" id=\"" + menu.ID + "\"><span class=\"file\"><a class=\"tool\" data-id=" + menu.ID + " href=\"javascript:;\">");
                    if (menu.IsShow == false)
                        treeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(menu.Name) + "</strike>");
                    else
                        treeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(menu.Name));
                    treeViewHtml.Append("</a> <i>(0)</i>" + BuildEditToolById(menu, add, delete, edit, show, order) + "</span></li>\r\n");
                }
            }
        }

        private static string BuildEditToolById(TreeViewItem menuItem, bool add, bool delete, bool edit, bool show, bool order)
        {
            var strTool = new StringBuilder();
            strTool.Append("<div class=\"quickTool\">\r\n");

            if (add)
            {
                strTool.AppendFormat("    <a title=\"Thêm mới menu: {1}\" data-event=\"add\"   href=\"#{0}\">\r\n ", menuItem.ID, menuItem.Name);
                strTool.Append("       <i class=\"fa fa-plus\"></i>");
                strTool.Append("    </a>");
            }
            if (edit)
            {
                strTool.AppendFormat("    <a title=\"Chỉnh sửa: {1}\" data-event=\"edit\"  href=\"#{0}\">\r\n", menuItem.ID, menuItem.Name);
                strTool.Append("       <i class=\"fa fa-pencil-square-o\"></i>");
                strTool.Append("    </a>");
            }

            if (show)
            {
                if (menuItem.IsShow == true)
                {
                    strTool.AppendFormat("    <a title=\"Ẩn: {1}\" href=\"#{0}\" data-event=\"hide\">\r\n", menuItem.ID, menuItem.Name);
                    strTool.Append("       <i class=\"fa fa-minus-circle\"></i>");
                    strTool.Append("    </a>\r\n");
                }
                else
                {
                    strTool.AppendFormat("    <a title=\"Hiển thị: {1}\" href=\"#{0}\" data-event=\"show\">\r\n", menuItem.ID, menuItem.Name);
                    strTool.Append("       <i class=\"fa fa-eye\"></i>");
                    strTool.Append("    </a>\r\n");
                }
            }
            if (delete)
            {
                strTool.AppendFormat("    <a title=\"Xóa: {1}\" href=\"#{0}\" data-event=\"delete\">\r\n", menuItem.ID, menuItem.Name);
                strTool.Append("       <i class=\"fa fa-trash-o\"></i>");
                strTool.Append("    </a>\r\n");
            }

            if (order)
            {
                strTool.AppendFormat("    <a title=\"Sắp xếp các menu con: {1}\" href=\"#{0}\" data-event=\"sort\">\r\n", menuItem.ParentId, menuItem.Name);
                strTool.Append("       <i class=\"fa fa-sort\"></i>");
                strTool.Append("    </a>\r\n");
            }


            strTool.Append("</div>\r\n");
            return strTool.ToString();
        }

        public void BuildTreeViewCheckBox(List<TreeViewItem> ltsSource, int categoryID, bool checkShow, List<int> ltsValues, ref StringBuilder treeViewHtml)
        {
            var listChild = ltsSource.OrderBy(o => o.Sort).Where(m => m.ParentId == categoryID && m.ID > 1);
            if (checkShow)
                listChild = listChild.Where(m => m.IsShow == checkShow);
            foreach (var category in listChild)
            {
                var countQuery = ltsSource.Where(m => m.ParentId == category.ID && m.ID > 1);
                if (checkShow)
                    countQuery = countQuery.Where(m => m.IsShow == checkShow);
                int totalChild = countQuery.Count();
                if (totalChild > 0)
                {
                    treeViewHtml.Append("<li class=\"unselect\" id=\"" + category.ID + "\"><span class=\"folder\"> <input id=\"Category_" + category.ID + "\" name=\"Category_" + category.ID + "\" value=\"" + category.ID + "\" type=\"checkbox\" title=\"" + category.Name + "\" " + (ltsValues.Contains(category.ID) ? " checked" : string.Empty) + "/> ");
                    if (category.IsShow == false)
                        treeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(category.Name) + "</strike>");
                    else
                        treeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(category.Name));
                    treeViewHtml.Append("</span>\r\n");
                    treeViewHtml.Append("<ul>\r\n");
                    BuildTreeViewCheckBox(ltsSource, category.ID, checkShow, ltsValues, ref treeViewHtml);
                    treeViewHtml.Append("</ul>\r\n");
                    treeViewHtml.Append("</li>\r\n");
                }
                else
                {
                    treeViewHtml.Append("<li  class=\"unselect\" id=\"" + category.ID + "\"><span class=\"file\"> <input id=\"Category_" + category.ID + "\" name=\"Category_" + category.ID + "\" value=\"" + category.ID + "\" type=\"checkbox\" title=\"" + category.Name + "\" " + (ltsValues.Contains(category.ID) ? " checked" : string.Empty) + "/> ");
                    if (category.IsShow == false)
                        treeViewHtml.Append("<strike>" + HttpContext.Current.Server.HtmlEncode(category.Name) + "</strike>");
                    else
                        treeViewHtml.Append(HttpContext.Current.Server.HtmlEncode(category.Name));
                    treeViewHtml.Append("</span></li>\r\n");
                }
            }
        }
    }
}