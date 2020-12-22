using System.Collections.Generic;
using System.Linq;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DocumentDl : BaseDA
    {
        public List<DocumentItem> GetList(int page, int cateId, ref int total)
        {
            var query = from n in FDIDB.Documents
                        where n.IsDelete == false && n.IsShow == true
                        && (cateId == 0 || n.Categories.Any(m => m.Id == cateId))
                        orderby n.Id descending
                        select new DocumentItem
                        {
                            ID = n.Id,
                            Slug = n.Slug,
                            CateName = n.Categories.OrderByDescending(c => c.IsLevel).Select(v => v.Name).FirstOrDefault(),
                            Name = n.Name,
                            PictureUrl = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Description = n.Description,
                            DateCreated = n.CreatedDate,
                            FileUrl = n.DocumentFileID != null ? (n.DocumentFile.Folder + n.DocumentFile.FileUrl) : "",
                        };
           
            query = query.Paging(page, 10, ref total);
            return query.ToList();
        }


        public List<DocumentItem> GetListOther(int cateId, int ortherId)
        {
            var query = from n in FDIDB.Documents
                        where n.IsDeleted == false && n.IsShow == true && n.Id != ortherId// && n.Categories.Any(m => m.Id == cateId)
                        orderby n.Id descending
                        select new DocumentItem
                        {
                            ID = n.Id,
                            Slug = n.Slug,
                            CateName = n.Categories.OrderByDescending(c => c.IsLevel).Select(v => v.Name).FirstOrDefault(),
                            Name = n.Name,
                            PictureUrl = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Description = n.Description,
                            DateCreated = n.CreatedDate,
                            FileUrl = n.DocumentFileID != null ? (n.DocumentFile.Folder + n.DocumentFile.FileUrl) : "",
                        };
            return query.Take(6).ToList();
        }
        public List<DocumentItem> GetListByCateId(int id)
        {
            var query = from n in FDIDB.Documents
                        where n.Categories.Any(m => m.Id == id) && n.IsDeleted == false
                        select new DocumentItem
                        {
                            ID = n.Id,
                            Slug = n.Slug,
                            CateName = n.Categories.OrderByDescending(c => c.IsLevel).Select(v => v.Name).FirstOrDefault(),
                            Name = n.Name,
                            PictureUrl = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Description = n.Description,
                            DateCreated = n.CreatedDate,
                            FileUrl = n.DocumentFileID != null ? (n.DocumentFile.Folder + n.DocumentFile.FileUrl) : "",
                        };
            return query.ToList();
        }
        public DocumentItem GetById(int id)
        {
            var query = from n in FDIDB.Documents
                        where n.Id == id && n.IsDeleted == false
                        select new DocumentItem
                        {
                            ID = n.Id,
                            Slug = n.Slug,
                            CateName = n.Categories.OrderByDescending(c => c.IsLevel).Select(v => v.Name).FirstOrDefault(),
                            Name = n.Name,
                            PictureUrl = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Description = n.Description,
                            DateCreated = n.CreatedDate,
                            FileUrl = n.DocumentFileID != null ? (n.DocumentFile.Folder + n.DocumentFile.FileUrl) : "",
                        };
            return query.FirstOrDefault();
        }
        public List<DocumentItem> GetListNew()
        {
            var query = from n in FDIDB.Documents
                        where n.IsDeleted == false
                        orderby n.Id descending
                        select new DocumentItem
                        {
                            ID = n.Id,
                            Slug = n.Slug,
                            CateName = n.Categories.OrderByDescending(c => c.IsLevel).Select(v => v.Name).FirstOrDefault(),
                            Name = n.Name,
                            PictureUrl = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Description = n.Description,
                            DateCreated = n.CreatedDate,
                            FileUrl = n.DocumentFileID != null ? (n.DocumentFile.Folder + n.DocumentFile.FileUrl) : "",
                        };
            return query.Take(5).ToList();
        }

        public DocumentItem GetDocumentItem(int id)
        {
            var query = from n in FDIDB.Documents
                        where n.Id == id
                        select new DocumentItem
                        {
                            ID = n.Id,
                            Slug = n.Slug,
                            CateName = n.Categories.OrderByDescending(c => c.IsLevel).Select(v => v.Name).FirstOrDefault(),
                            Name = n.Name,
                            PictureUrl = n.Gallery_Picture.Folder + n.Gallery_Picture.Url,
                            Description = n.Description,
                            DateCreated = n.CreatedDate,
                            FileUrl = n.DocumentFileID != null ? (n.DocumentFile.Folder + n.DocumentFile.FileUrl) : "",
                        };
            return query.FirstOrDefault();
        }
    }
}