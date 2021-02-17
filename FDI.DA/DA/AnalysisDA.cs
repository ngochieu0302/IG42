using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class AnalysisDA : BaseDA
    {
        #region Constructer
        public AnalysisDA()
        {
        }

        public AnalysisDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public AnalysisDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion
        public AnalysisItem GetAllAnalysis(int agencyid, decimal start, decimal end)
        {
            var query = from c in FDIDB.GetAllAnalysis(agencyid, start, end)
                        select new AnalysisItem
                        {
                            CountC = c.CountC,
                            Later = c.Later,
                            CountO = c.CountO,
                            Cancelled = c.Cancelled,
                            CashAdvance = c.CashAdvance,
                            Complete = c.Complete,
                            CountU = c.CountU,
                            Onl = c.Onl,
                            Payment = c.Payment,
                            Pending = c.Pending,
                            PriceReceive = c.PriceReceive,
                            PriceReward = c.PriceReward,
                            Processing = c.Processing,
                            Receipt = c.Receipt,
                            Refunded = c.Refunded,
                            Repay = c.Repay,
                        };
            return query.FirstOrDefault();
        }

        public List<AnalysisItem> AnalysisByAdmin(decimal start, decimal end)
        {
            var query = from c in FDIDB.AnalysisByAdmin(start, end)
                        select new AnalysisItem
                        {
                            CountC = c.CountC,
                            Later = c.Later,
                            CountO = c.CountO,
                            Cancelled = c.Cancelled,
                            CashAdvance = c.CashAdvance,
                            Complete = c.Complete,
                            CountU = c.CountU,
                            Onl = c.Onl,
                            Payment = c.Payment,
                            Pending = c.Pending,
                            PriceReceive = c.PriceReceive,
                            PriceReward = c.PriceReward,
                            Processing = c.Processing,
                            Receipt = c.Receipt,
                            Refunded = c.Refunded,
                            Repay = c.Repay,
                        };
            return query.ToList();
        }
        public List<AnalysisItem> AnalysisByEnterprise(int enterpriseId, decimal start, decimal end)
        {
            var query = from c in FDIDB.AnalysisByEnterprise(enterpriseId, start, end)
                        select new AnalysisItem
                        {
                            CountC = c.CountC,
                            Later = c.Later,
                            CountO = c.CountO,
                            Cancelled = c.Cancelled,
                            CashAdvance = c.CashAdvance,
                            Complete = c.Complete,
                            CountU = c.CountU,
                            Onl = c.Onl,
                            Payment = c.Payment,
                            Pending = c.Pending,
                            PriceReceive = c.PriceReceive,
                            PriceReward = c.PriceReward,
                            Processing = c.Processing,
                            Receipt = c.Receipt,
                            Refunded = c.Refunded,
                            Repay = c.Repay,
                        };
            return query.ToList();
        }

        public List<ProductExportItem> ProductTop(int agencyid, decimal start, decimal end)
        {
            var query = from c in FDIDB.Shop_Product
                        where c.Shop_Order_Details.Any(m => m.Shop_Orders.Status < 4 && m.Shop_Orders.DateCreated >= start && m.Shop_Orders.DateCreated <= end && m.Shop_Orders.AgencyId == agencyid)
                        orderby c.Shop_Order_Details.Where(m => m.Shop_Orders.Status < 4 && m.Shop_Orders.DateCreated >= start && m.Shop_Orders.DateCreated <= end).Sum(m => m.Quantity - m.QuantityOld) descending
                        select new ProductExportItem
                        {
                            ID = c.ID,
                            Name = c.Shop_Product_Detail.Name,
                            CodeSku = c.CodeSku,
                            UrlPicture = c.Shop_Product_Detail.Gallery_Picture.Folder + c.Shop_Product_Detail.Gallery_Picture.Url,
                            SizeName = c.Product_Size.Name,
                            ColorName = c.System_Color.Name,
                            Quantity = c.Shop_Order_Details.Where(m => m.Shop_Orders.Status < 4 && m.Shop_Orders.DateCreated >= start && m.Shop_Orders.DateCreated <= end).Sum(m => m.Quantity - m.QuantityOld),
                        };
            return query.Take(10).ToList();
        }
        public ModelLineItem CustomerAwards(int agencyid, decimal start, decimal end)
        {
            var query = from c in FDIDB.ReceiveHistories
                        where c.AgencyId == agencyid && c.Date >= start && c.Date < end
                        select new ValueAnalysisItem
                        {
                            Value = c.Price,
                            Date = c.Date,
                        };
            var query1 = from c in FDIDB.RewardHistories
                         where c.Date >= start && c.Date < end
                         select new ValueAnalysisItem
                         {
                             Value = c.Price,
                             Date = c.Date,
                         };
            var obj = new LineItem
            {
                Line = "Rút lũy",
                ListLine = query
            };
            var obj1 = new LineItem
            {
                Line = "Tích lũy",
                ListLine = query1
            };

            IEnumerable<LineItem> arr = new[] { obj1, obj };
            var model = new ModelLineItem { ListItem = arr };
            return model;
        }
        public ModelLineItem Revenue(int agencyid, decimal start, decimal end)
        {
            var query = from c in FDIDB.PaymentVouchers
                        where c.AgencyId == agencyid && c.DateCreated >= start && c.DateCreated < end
                        select new ValueAnalysisItem
                        {
                            Value = c.Price,
                            Date = c.DateCreated,
                        };
            var query1 = from c in FDIDB.ReceiptVouchers
                         where c.AgencyId == agencyid && c.DateCreated >= start && c.DateCreated < end
                         select new ValueAnalysisItem
                         {
                             Value = c.Price,
                             Date = c.DateCreated,
                         };
            var query2 = from c in FDIDB.Shop_Orders
                         where c.AgencyId == agencyid && c.Status < 4 && c.DateCreated >= start && c.DateCreated < end
                         select new ValueAnalysisItem
                         {
                             Value = c.TotalPrice,
                             Date = c.DateCreated,
                         };
            var obj = new LineItem
            {
                Line = "Thu",
                ListLine = query
            };
            var obj1 = new LineItem
            {
                Line = "Chi",
                ListLine = query1
            };
            var obj2 = new LineItem
            {
                Line = "Đơn hàng",
                ListLine = query2
            };
            IEnumerable<LineItem> arr = new[] { obj1, obj, obj2 };
            var model = new ModelLineItem { ListItem = arr };
            return model;
        }
    }
}