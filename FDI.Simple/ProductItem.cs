using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ProductItem : BaseSimple
    {
        public int? AgencyID { get; set; }
        public string UrlPicture { get; set; }
        public string Name { get; set; }
        public string CodeSku { get; set; }
        public int? Value { get; set; }
        public int? CateID { get; set; }
        public string NameAscii { get; set; }
        public string NameCate { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public decimal? PriceCost { get; set; }
        public decimal? PriceOld { get; set; }
        public decimal? PriceNew { get; set; }
        public int? Quantity { get; set; }
        public decimal? Percent { get; set; }
        public int? QuantityDay { get; set; }
        public string CreateBy { get; set; }
        public int? TypeID { get; set; }
        public string BarCode { get; set; }
        public decimal? CreateDate { get; set; }
        public decimal? DateEnd { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsHot { get; set; }
        public string LanguageId { get; set; }
        public bool? IsDelete { get; set; }
        public int? UnitID { get; set; }
        public int? SupplierID { get; set; }
        public int? HomeID { get; set; }
        public int? SizeID { get; set; }
        public int? ColorID { get; set; }
        public int? ProductionCostID { get; set; }
        public int? ProductDetailID { get; set; }
        public string LstSort { get; set; }
        public int? QuantityOut { get; set; }
        public IEnumerable<CostProductItem> LstCostProducts { get; set; }
        public IEnumerable<CostProductUserItem> LstCostProductUserItems { get; set; }
        public IEnumerable<RecipeItem> LstRecipeItems { get; set; }
        public IEnumerable<ShopProductPictureItem> LstShopProductPictureItem { get; set; }
        public IEnumerable<PictureItem> LstPictures { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string UnitName { get; set; }
        public decimal? PriceCostParent { get; set; }
        public decimal? PriceInCurr { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public ImportProductItem ImportProductItem { get; set; }
        public decimal? PriceUnit { get; set; }  // //don gia
    }

    public class RecipeItem : BaseSimple
    {
        public int? ProductID { get; set; }
        public int? ValueId { get; set; }
        public string ValueName { get; set; }
        public decimal? ValueQuantity { get; set; }
        public decimal? Quantity { get; set; }
        public string UnitName { get; set; }
        public decimal? Price { get; set; }
        public bool? IsDelete { get; set; }
        public string Key { get; set; }
        public string ProductName { get; set; }
        public int? ProductDetailID { get; set; }

    }

    public class ProductPacketItem : BaseSimple
    {
        public int? PacketID { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }
    }
    public class ProductAppItem : BaseSimple
    {
        public string Name { get; set; } // ten san pham
        public string Pu { get; set; } // duong dan anh san pham
        public decimal? Q { get; set; }// so luong
        public double? Km { get; set; }
        public decimal? D { get; set; }
        public int? Aid { get; set; }
        public string An { get; set; }
        public decimal? Pr { get; set; }
        public decimal? Weight { get; set; }
        public decimal? PrD { get; set; }//gia
        public string Size { get; set; } // size mac dinh
        public int? SizeValue { get; set; }
        public string des { get; set; } // mo ta sp
        public string detail { get; set; } // cjhi tiêt
        public string ledge { get; set; }// kien thuc
        public string procces { get; set; } // process
        public IEnumerable<TypeAppItem> lstS { get; set; }
        public IEnumerable<ImgAppItem> lstImg { get; set; }
        public IEnumerable<PtemAppItem> lstId { get; set; }
        public int OrderType { get; set; }

    }
    public class PtemAppItem
    {
        public int ID { get; set; }
    }

    public class TypeAppItem
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
    }

    public class ImgAppItem
    {
        public int ID { get; set; }
        public string Url { get; set; }
    }
    public class ModelProductItem : BaseModelSimple
    {
        public IEnumerable<ProductItem> ListItem { get; set; }
        public IEnumerable<ShopProductDetailItem> ListItemDetail { get; set; }
        public ProductItem ProductItem { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalOld { get; set; }
        public int Quantity { get; set; }
    }

    public class ProductExportItem : BaseSimple
    {
        public string Name { get; set; }
        public string UrlPicture { get; set; }
        public string UnitName { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string CodeSku { get; set; }
    }

    public class ModelProductExportItem : BaseSimple
    {
        public IEnumerable<ProductExportItem> ListItem { get; set; }
        public IEnumerable<ExportProductItem> LstExportItem { get; set; }
        public IEnumerable<ExportProductItem> LstExportProductItem { get; set; }
        public decimal? Quantity { get; set; }
        public int? AgencyID { get; set; }
        public Guid? UserID { get; set; }
        public string UserName { get; set; }
        public string DateTime { get; set; }
    }
}
