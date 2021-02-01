using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class CategoryAppIG4Item : BaseSimple
    {
        public string Name { get; set; }
        public string NameRoot { get; set; }
        public string Slug { get; set; }
        public int? ParentId { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int? IsLevel { get; set; }
        public int? Type { get; set; }
        public int? Step { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsHome { get; set; }
        public bool? IsMenu { get; set; }
        public bool? IsMenuFooter { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Sort { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyword { get; set; }
        public string LanguageId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UserCreate { get; set; }
        public string UrlPicture { get; set; }
        public string UrlIcon { get; set; }
        public bool IsPrestige { get; set; }
        public IEnumerable<NewsItem> ListNewsItem { get; set; }
        public IEnumerable<ProductAppIG4Item> ListProductItem { get; set; }
        public int? TotalProduct { get; set; }
        public List<int> lstInt { get; set; }

    }

    public class CategoryItemComparer : IEquatable<CategoryItemComparer>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool Equals(CategoryItemComparer person)
        {
            return person.Id == Id;
        }
          

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
    public class ModelCategoryAppIG4Item : BaseModelSimple
    {
        public IEnumerable<CategoryItem> ListItem { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public string Slug { get; set; }
        public string Listid { get; set; }
        public string Title { get; set; }
        public bool SelectMutil { get; set; }
        public bool IsAdmin { get; set; }
    }
}

