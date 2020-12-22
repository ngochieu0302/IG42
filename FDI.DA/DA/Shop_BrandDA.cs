using System.Collections.Generic;
using System.Web;
using FDI.Base;
using FDI.Simple;
using System.Linq;

namespace FDI.DA
{
    public partial class Shop_BrandDA : BaseDA
    {
        #region Constructer
        public Shop_BrandDA()
        {
        }

        public Shop_BrandDA(string pathPaging)
        {
            this.PathPaging = pathPaging;
        }

        public Shop_BrandDA(string pathPaging, string pathPagingExt)
        {
            this.PathPaging = pathPaging;
            this.PathPagingext = pathPagingExt;
        }
        #endregion

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<ProductBrandItem> GetListSimpleAll()
        {
            var query = from c in FDIDB.Shop_Brands
                        where c.IsDeleted == false && c.IsShow == true
                        orderby c.Name
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <param name="isShow">Kiểm tra hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<ProductBrandItem> GetListSimpleAll(bool isShow)
        {
            var query = from c in FDIDB.Shop_Brands
                        where (c.IsShow == isShow)
                        orderby c.Name
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted
                        };
            return query.ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<ProductBrandItem> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in FDIDB.Shop_Brands
                        where c.Name.StartsWith(keyword)
                        orderby c.Name
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <param name="isShow"> </param>
        /// <returns></returns>
        public List<ProductBrandItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.Shop_Brands
                        where c.IsShow == isShow && c.Name.StartsWith(keyword)
                        orderby c.Name
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="httpRequest"> </param>
        /// <returns>Danh sách bản ghi</returns>
        public List<ProductBrandItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Shop_Brands
                        where c.ID > 1 && c.IsDeleted == false
                        orderby c.ID descending 
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.OrderByDescending(c => c.ID).ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="ltsArrID"></param>
        /// <returns></returns>
        public List<ProductBrandItem> GetListSimpleByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Shop_Brands
                        where ltsArrID.Contains(c.ID)
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        /// <summary>
        /// Lấy về mảng đơn giản qua mảng ID
        /// </summary>
        /// <param name="ltsArrID"></param>
        /// <returns></returns>
        public ProductBrandItem GetListCategoryById(string slug)
        {
            var query = from c in FDIDB.Shop_Brands
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted

                        };
            return query.FirstOrDefault();
        }

        public ProductBrandItem GetListCategoryByNameAscii(string nameAscii)
        {
            var query = from c in FDIDB.Shop_Brands
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted
                        };
            return query.FirstOrDefault();
        }

        public ProductBrandItem GetItemById(int id)
        {
            var query = from c in FDIDB.Shop_Brands
                        where c.ID == id
                        select new ProductBrandItem
                        {
                            ID = c.ID,
                            Code = c.Code,
                            Name = c.Name,
                            Address = c.Address,
                            Email = c.Email,
                            Phone = c.Phone,
                            TaxCode = c.TaxCode,
                            Note = c.Note,
                            PictureID = c.PictureID,
                            PictureUrl = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            IsShow = c.IsShow,
                            LanguageId = c.LanguageId,
                            IsDeleted = c.IsDeleted
                        };
            return query.FirstOrDefault();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="brandID">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Shop_Brands GetById(int brandID)
        {
            var query = from c in FDIDB.Shop_Brands where c.ID == brandID select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Shop_Brands> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Shop_Brands where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public List<Category> GetListCategoryByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Categories where ltsArrID.Contains(c.Id) select c;
            return query.ToList();
        }


        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="shopBrand">Đối tượng kiểm tra</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool CheckExits(Shop_Brands shopBrand)
        {
            var query = from c in FDIDB.Shop_Brands where ((c.Name == shopBrand.Name) && (c.ID != shopBrand.ID)) select c;
            return query.Any();
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="brandName">Tên bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Shop_Brands GetByName(string brandName)
        {
            var query = from c in FDIDB.Shop_Brands where ((c.Name == brandName)) select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="shopBrand">bản ghi cần thêm</param>
        public void Add(Shop_Brands shopBrand)
        {
            FDIDB.Shop_Brands.Add(shopBrand);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="shopBrand">Xóa bản ghi</param>
        public void Delete(Shop_Brands shopBrand)
        {
            FDIDB.Shop_Brands.Remove(shopBrand);
        }

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion
    }
}
