using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDI.Utils
{
    public class Pager
    {
        /// <summary>
        /// Phân trang cho gridView
        /// </summary>
        /// <param name="strPathPage">Tiền tố trước page. Có dạng #parram1=value&...</param>
        /// <param name="intCurrentPage">Trang hiện tại</param>
        /// <param name="intRowPerPage">SỐ bản ghi trên 1 trang</param>
        /// <param name="intTotalRecord">Tổng số bản ghi</param>
        /// <returns>Mã html phân trang</returns>

        public string GetPage(string strPathPage, int intCurrentPage, int intRowPerPage, int intTotalRecord)
        {
            var ltsRowPerpage = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 200 };
            if (!ltsRowPerpage.Contains(intRowPerPage))
                ltsRowPerpage.Add(intRowPerPage);
            ltsRowPerpage.Sort();
            var intTotalPage = (intTotalRecord % intRowPerPage == 0) ? intTotalRecord / intRowPerPage : ((intTotalRecord - (intTotalRecord % intRowPerPage)) / intRowPerPage) + 1;
            var strBuilder = new StringBuilder();
            if (intTotalRecord > 0)
            {
                strBuilder.Append("<div class=\"bottom-pager\">\r\n");
                strBuilder.Append("    <div class=\"left\">\r\n");
                if (intCurrentPage > 1)
                {
                    strBuilder.AppendFormat("        <a href=\"#{0}1\" class=\"first\" title=\"Trang đầu\"><span class=\"fa fa-step-backward\"></span></a>\r\n", strPathPage);
                    strBuilder.AppendFormat("        <a href=\"#{0}{1}\" class=\"pre\" title=\"Trang trước\"><span class=\"fa fa-caret-left\"></span></a>\r\n", strPathPage, intCurrentPage - 1);
                }
                else
                {
                    strBuilder.Append("        <a href=\"javascript:;\" class=\"first-disable\" title=\"Trang đầu\"><span class=\"fa fa-step-backward\"></span></a>\r\n");
                    strBuilder.Append("        <a href=\"javascript:;\" class=\"pre-disable\" title=\"Trang trước\"><span class=\"fa fa-caret-left\"></span></a>\r\n");
                }
                strBuilder.Append("        <span>Trang</span>\r\n");
                strBuilder.AppendFormat("        <input type=\"text\" name=\"page\" value=\"{0}\" />\r\n", intCurrentPage);
                strBuilder.AppendFormat("        <input type=\"hidden\" value=\"{0}\" />\r\n", intTotalPage);
                strBuilder.AppendFormat("        <span>/{0}</span>\r\n", intTotalPage);

                if (intCurrentPage < intTotalPage)
                {
                    strBuilder.AppendFormat("        <a href=\"#{0}{1}\" class=\"next\" title=\"Trang tiếp\"><span class=\"fa fa-caret-right\"></span></a>\r\n", strPathPage, intCurrentPage + 1);
                    strBuilder.AppendFormat("        <a href=\"#{0}{1}\" class=\"last\" title=\"Trang cuối\"><span class=\"fa fa-step-forward\"></span></a>\r\n", strPathPage, intTotalPage);
                }
                else
                {
                    strBuilder.Append("        <a href=\"javascript:;\" class=\"next-disable\" title=\"Trang tiếp\"><span class=\"fa fa-caret-right\"></span></a>\r\n");
                    strBuilder.Append("        <a href=\"javascript:;\" class=\"last-disable\" title=\"Trang cuối\"><span class=\"fa fa-step-forward\"></span></a>\r\n");
                }
                strBuilder.Append("    </div>\r\n");
                strBuilder.Append("    <div class=\"right\">\r\n");
                strBuilder.Append("        <span>Kết quả trên 1 trang:</span>\r\n");
                strBuilder.Append("        <select name=\"RowPerPage\">\r\n");
                foreach (var item in ltsRowPerpage)
                {
                    strBuilder.AppendFormat("            <option value=\"{0}\"{1}>{2}</option>\r\n", item, (item == intRowPerPage) ? " selected" : "", item);
                }
                strBuilder.Append("        </select>\r\n");
                strBuilder.AppendFormat("        <span>/ Tổng số: {0}</span>\r\n", intTotalRecord);
                strBuilder.Append("    </div>\r\n");
                strBuilder.Append("</div>\r\n");
            }
            else
            {
                strBuilder.Append("<div class=\"bottom-pager\"><span>Hiện tại danh sách này chưa có dữ liệu.</span></div>\r\n");
            }
            return strBuilder.ToString();
        }
        
    }
}