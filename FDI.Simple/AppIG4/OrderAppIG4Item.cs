using System;
using System.Collections.Generic;
using System.ServiceModel.Configuration;

namespace FDI.Simple
{
    public class OrderAppIG4Item : BaseSimple
    {
        public int ID { get; set; }
        public decimal? OrderTotal { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public bool? IsDelete { get; set; }
        public int? Status { get; set; }
        public int? StatusPayment { get; set; }
        public string Message { get; set; }
        public string NoOrder { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? PaymentmethodId { get; set; }
        public bool? IsTime { get; set; }
        public int? CustomerID { get; set; }
        public decimal? FeeShip { get; set; }
        public decimal? Payment { get; set; }
        public string CityName { get; set; }
        public string District { get; set; }
        public int ShopID { get; set; }
        public string Shopname { get; set; }
        public decimal? CouponPrice { get; set; }
        public string Wards { get; set; }
        public string vpc_TransactionNo { get; set; }
        public IEnumerable<OrderDetailAppIG4Item> LisOrderDetailItems { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Coupon { get; set; }
        public decimal Discount { get; set; }
        public int? ContactOrderID { get; set; }
        public string ContactOrderName { get; set; }
        public int? CustomerAddressID { get; set; }
    }

    public class ModelOrderAppIG4Item : BaseModelSimple
    {
        public virtual List<OrderAppIG4Item> ListItem { get; set; }
        public decimal? TotalSuccess { get; set; }
        public decimal? totalVNpay { get; set; }
        public decimal? totalWallets { get; set; }
        public decimal? totalCod { get; set; }
        public decimal? TotalPending { get; set; }
        public decimal? TotalCancel { get; set; }
        public decimal? Total { get; set; }
    }

    public class PaymentItem
    {
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string Code { get; set; }
        public int? CustomerID { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? PaymentmethodId { get; set; }
        public string Note { get; set; }
        public decimal? OrderTotal { get; set; }

    }

    public class OrderReturnItem
    {
        public string vnp_PayDate { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_TransactionNo { get; set; }
        public decimal? Amount { get; set; }
        public string Message { get; set; }
        public int? pageId { get; set; }
        public int? CustomerID { get; set; }
    }

    public class DashboardItem
    {
        public int? CategoryId { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? totalquantity { get; set; }
        public int? totalSp { get; set; }
        public string Name { get; set; }
        public DateTime? DateDh { get; set; }
        public DateTime? DateSp { get; set; }
    }

    public class dataItem
    {
        public string customerNumber { get; set; }
        public int CustomerId { get; set; }
        public string token { get; set; }
        public double? amount { get; set; }
        public string description { get; set; }
    }
    public class HashMoMoItem
    {
        public string partnerCode { get; set; }
        public string partnerRefId { get; set; }
        public string partnerTransId { get; set; }
        public double? amount { get; set; }


        //public string partnerName { get; set; }

        public string description { get; set; }
        //public string customerNumber { get; set; }
        //public string customerName { get; set; }
        //public string storeId { get; set; }
        //public string storeName { get; set; }
    }

    public class ProcessMoMoItem
    {
        public string customerNumber { get; set; }
        public string partnerCode { get; set; }
        public string partnerRefId { get; set; }
        public string appData { get; set; }
        public string hash { get; set; }
        public string description { get; set; }
        public int? payType { get; set; }

        public decimal? version { get; set; }
    }

    public class ResultMoMoItem
    {
        public int status { get; set; }
        public string message { get; set; }
        public double? amount { get; set; }
        public string transid { get; set; }
        public string signature { get; set; }
        public ErrorMoMoItem error { get; set; }
    }
    public class ResultDataMoMoAppIG4Item
    {
        public int status { get; set; }
        public string message { get; set; }
        public double? amount { get; set; }
        public string transid { get; set; }
        public string signature { get; set; }
        public ErrorMoMoItem error { get; set; }
        public DataResult data { get; set; }
    }

    public class DataResult
    {
        public string partnerCode { get; set; }
        public string momoTransId { get; set; }
        public double amount { get; set; }
        public string partnerRefId { get; set; }
    }
    public class ConfirmDataMoMoItem
    {
        public string partnerCode { get; set; }
        public string partnerRefId { get; set; }
        public string momoTransId { get; set; }
        public string signature { get; set; }
        public string customerNumber { get; set; }
        public decimal? amount { get; set; }
    }
    public class ConfirmMoMoItem
    {
        public string partnerCode { get; set; }
        public string partnerRefId { get; set; }
        public string requestType { get; set; }
        public string requestId { get; set; }
        public string momoTransId { get; set; }
        public string signature { get; set; }
        public string customerNumber { get; set; }
        public string description { get; set; }
    }
    
    public class ErrorMoMoItem
    {
        public string partnerCode { get; set; }
        public string amount { get; set; }
        public string hash { get; set; }
        public string appData { get; set; }
    }
    public class OrderCustomerAppItem
    {
        public int Id { get; set; }
        public string Customername { get; set; }
        public string Phone { get; set; }
        public string Productname { get; set; }
        public string UrlPicture { get; set; }
        public int? Status { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PriceShip { get; set; }
        public int? count { get; set; }
        public decimal? CouponPrice { get; set; }
        public int? Check { get; set; }
        public decimal? TotalPrice { get; set; }
        public IEnumerable<OrderDetailCustomerAppItem> LisOrderDetailItems { get; set; }
    }

    public class OrderDetailCustomerAppItem
    {
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Shopname { get; set; }
        public string Productname { get; set; }
        public string UrlPicture { get; set; }
        public decimal? DateCreate { get; set; }
        public int? Status { get; set; }
        public int? Check { get; set; }
    }
    public class OrderProcessAppIG4Item : BaseSimple
    {
        public int OrderId { get; set; }
        public decimal? EndDate { get; set; }
    }

    public class StaticOrderAppIG4Item : BaseSimple
    {
        public DateTime? DateCreate { get; set; }
        public string Code { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public List<string> Categoryname { get; set; }
        public List<int> LstCateInts { get; set; }
        public List<string> Productname { get; set; }
        public int? Status { get; set; }
        public string Whsname { get; set; }
        public string Address { get; set; }
        public int? Quantity { get; set; }
        public decimal? Payment { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalKygui { get; set; }
        public decimal? TotalNoKygui { get; set; }
        public decimal? FeeShip { get; set; }
        public decimal? CouponPrice { get; set; }
        public decimal? TotalRecive { get; set; }
        public decimal? TotalWhs { get; set; }
        public decimal? F1 { get; set; }
        public decimal? Gstore { get; set; }
        public bool? Iskygui { get; set; }
    }

    public class ModelStaticAppIG4Order : BaseModelSimple
    {
        public IEnumerable<StaticOrderAppIG4Item> ListItems { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalGstore { get; set; }
        public decimal? Percent { get; set; }
        public decimal? PercentWhs { get; set; }
        public decimal? PercentF1 { get; set; }
        public decimal? PercentF0 { get; set; }
    }
}
