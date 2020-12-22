using System.Collections.Generic;
using System.Globalization;

namespace FDI.Utils
{
    public class PageItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Current { get; set; }
        public bool Span { get; set; }
        public bool Link { get; set; }
    }

    public static class Paging
    {
        public static int Step { get; set; }

        public static int Total { get; set; }

        public static int Current { get; set; }

        public static string Link { get; set; }

        public static string LinkExt { get; set; }

        public static string Focus { get; set; }

        static Paging()
        {
            Current = 1;
            Link = string.Empty;
            Total = 1;
            Step = 3;
            LinkExt = "";
        }

        public static string GetHtmlPage(string linkPage, int pageStep, int currentPage, int rowPerPage, int totalRow)
        {
            Step = pageStep;
            Current = currentPage;
            Link = linkPage;
            if (rowPerPage == 0)
                rowPerPage = 5;
            Total = (totalRow % rowPerPage == 0) ? totalRow / rowPerPage : ((totalRow - (totalRow % rowPerPage)) / rowPerPage) + 1;
            return WriteHtmlPage();
        }

        public static string GetHtmlPage(string linkPage, string linkPageExt, int pageStep, int currentPage, int rowPerPage, int totalRow)
        {
            Step = pageStep;
            Current = currentPage;
            Link = linkPage;
            LinkExt = linkPageExt;
            Total = (totalRow % rowPerPage == 0) ? totalRow / rowPerPage : ((totalRow - (totalRow % rowPerPage)) / rowPerPage) + 1;
            return WriteHtmlPage();
        }

        public static List<PageItem> GetPageItems(int pageStep, int currentPage, int rowPerPage, int totalRow)
        {
            Step = pageStep;
            Current = currentPage;
            Link = Link;
            LinkExt = LinkExt;
            Total = (totalRow % rowPerPage == 0) ? totalRow / rowPerPage : ((totalRow - (totalRow % rowPerPage)) / rowPerPage) + 1;
            var pageItems = new List<PageItem>();
            PageItem page;

            if (Current > Step + 1)
            {
                page = new PageItem { Text = "« Đầu", Value = "1", Link = true };
                pageItems.Add(page);

                page = new PageItem { Text = "Trước", Value = (Current - 1).ToString(CultureInfo.InvariantCulture), Link = true };
                pageItems.Add(page);

                page = new PageItem { Span = true, Text = "..." };
                pageItems.Add(page);

            }
            var beginFor = ((Current - Step) > 1) ? (Current - Step) : 1;
            var endFor = ((Current + Step) > Total) ? Total : (Current + Step);

            for (var pNumber = beginFor; pNumber <= endFor; pNumber++)
            {
                if (pNumber == Current)
                {
                    page = new PageItem { Text = pNumber.ToString(), Current = true };
                    pageItems.Add(page);
                }
                else
                {
                    page = new PageItem { Text = pNumber.ToString(), Value = pNumber.ToString(), Link = true };
                    pageItems.Add(page);
                }
            }

            if (Current < (Total - Step))
            {
                page = new PageItem { Span = true, Text = "..." };
                pageItems.Add(page);

                page = new PageItem { Text = "Sau", Value = (Current + 1).ToString(), Link = true };
                pageItems.Add(page);

                page = new PageItem { Text = "Cuối »", Value = Total.ToString(), Link = true };
                pageItems.Add(page);
            }

            return pageItems;
        }

        private static string WriteHtmlPage()
        {
            var strPageHtml = "<div class=\"paging\">";
            if (Current > Step + 1)
            {
                strPageHtml += "<a href=\"" + Link + 1 + LinkExt + "\">« Đầu</a>";
                strPageHtml += "<a href=\"" + Link + (Current - 1) + LinkExt + "\">Trước</a>";
                strPageHtml += "<span>...</span>";
            }

            var beginFor = ((Current - Step) > 1) ? (Current - Step) : 1;
            var endFor = ((Current + Step) > Total) ? Total : (Current + Step);

            for (var pNumber = beginFor; pNumber <= endFor; pNumber++)
            {
                if (pNumber == Current)
                    strPageHtml += "<a href=\"javascript:;\" class=\"current\">" + pNumber + "</a>";
                else
                    strPageHtml += "<a href=\"" + Link + pNumber + LinkExt + "\">" + pNumber + "</a>";
            }

            if (Current < (Total - Step))
            {
                strPageHtml += "<span>...</span>";
                strPageHtml += "<a href=\"" + Link + (Current + 1) + LinkExt + "\">Sau</a>";
                strPageHtml += "<a href=\"" + Link + Total + LinkExt + "\">Cuối »</a>";

            }
            strPageHtml += "</div>";
            return Total > 1 ? strPageHtml : string.Empty;
        }

        public static string GetPage(string link, int step, int current, int row, int total, string name = null, string key = null)
        {
            Step = step;
            Current = current;
            Link = link;
            if (row == 0)
                row = 5;
            Total = (total % row == 0) ? total / row : ((total - (total % row)) / row) + 1;
            return WritePage(name, key);
        }

        private static string WritePage(string name, string key)
        {
            var html = "";
            const string firt = "<span><i class='fa fa-angle-double-left'></i></span>";
            const string back = "<span><i class='fa fa-angle-left'></i></span>";
            const string next = "<span><i class='fa fa-angle-right'></i></span>";
            const string last = "<span><i class='fa fa-angle-double-right'></i></span>";
            var begin = ((Current - Step) > 1) ? (Current - Step) : 1;
            var end = ((Current + Step) > Total) ? Total : (Current + Step);
            if (Current > Step + 1 && name == null)
            {
                html += string.Format("<li><a title='Trang đầu' href='{0}1{1}'>{2}</a></li>", Link, LinkExt, firt);
                html += string.Format("<li><a title='Trang trước' href='{0}{1}{2}'>{3}</a></li>", Link, Current - 1, LinkExt, back);
            }
            else if (Current > Step + 1 && name != null)
            {
                html += string.Format("<li><a title='Trang đầu' href='{0}1{1}?{2}={3}'>{4}</a></li>", Link, LinkExt, name, key, firt);
                html += string.Format("<li><a title='Trang trước' href='{0}{1}{2}?{3}={4}'>{5}</a></li>", Link, Current - 1, LinkExt, name, key, back);
            }
            for (var p = begin; p <= end; p++)
            {
                if (p == Current)
                    html += string.Format("<li class='current'><a title='Trang hiện tại' href='javascript:;'>{0}</a></li>",
                            p);
                else if (name == null)
                {
                    html += string.Format("<li><a title='Trang {0}' href='{1}{0}{2}'>" + p + "</a></li>", p, Link, LinkExt);
                }
                else
                {
                    html += string.Format("<li><a title='Trang {0}' href='{1}{0}{2}?{3}={4}'>{0}</a></li>", p, Link,
                       LinkExt, name, key);
                }
            }
            if (Current < (Total - Step) && name == null)
            {
                html += string.Format("<li><a title='Trang tiếp' href='{0}{1}{2}'>{3}</a></li>", Link, Current + 1, LinkExt, next);
                html += string.Format("<li><a title='Trang cuối' href='{0}{1}{2}'>{3}</a></li>", Link, Total, LinkExt, last);

            }
            else if (Current < (Total - Step) && name != null)
            {
                html += string.Format("<li><a title='Trang tiếp' href='{0}1{1}?{2}={3}'>{4}</a></li>", Link, LinkExt, name, key, next);
                html += string.Format("<li><a title='Trang cuối' href='{0}{1}{2}?{3}={4}'>{5}</a></li>", Link, Current - 1, LinkExt, name, key, last);
            }
            return Total > 1 ? html : string.Empty;
        }
    }
}
