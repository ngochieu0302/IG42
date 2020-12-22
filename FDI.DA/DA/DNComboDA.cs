using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
	public class DNComboDA : BaseDA
	{
		public DNComboDA()
		{
		}
		public DNComboDA(string pathPaging)
		{
			PathPaging = pathPaging;
		}
		public DNComboDA(string pathPaging, string pathPagingExt)
		{
			PathPaging = pathPaging;
			PathPagingext = pathPagingExt;
		}
		public List<DNComboItem> GetListSimple(int agencyId)
		{
			var query = from o in FDIDB.DN_Combo
				where o.AgencyId == agencyId
				orderby o.ID descending
				select new DNComboItem
				{
					ID = o.ID,
					Name = o.Name.Trim(),
					Price = o.Price,
					DateEnd = o.DateEnd,
					DateStart = o.DateStart,
					Percent = o.Price
				};
			return query.ToList();
		}

		public List<DNComboItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyid)
		{
			Request = new ParramRequest(httpRequest);
			var query = from o in FDIDB.DN_Combo
				where o.AgencyId == agencyid && o.IsDeleted == false
				orderby o.ID descending
				select new DNComboItem
				{
					ID = o.ID,
					Name = o.Name.Trim(),
					Price = o.Price,
					DateEnd = o.DateEnd,
					DateStart = o.DateStart,
					Percent = o.Price
				};
			query = query.SelectByRequest(Request, ref TotalRecord);
			return query.ToList();
		}

		public DNComboItem GetComboItem(int id)
		{
			var query = from c in FDIDB.DN_Combo
				where c.ID == id
				select new DNComboItem
				{
					ID = c.ID,
					Name = c.Name,
					Price = c.Price,
					PictureId = c.PictureID,
					UrlPicture = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
					DateStart = c.DateStart,
					DateEnd = c.DateEnd,
					LstProducts = from v in c.Shop_Product
						where c.IsDeleted == false
						select new ProductItem
						{
							ID = v.ID,
							Name = v.Shop_Product_Detail.Name,
							//PriceNew = v.PriceNew
						}
				};
			return query.FirstOrDefault();
		}
		public DN_Combo GetById(int id)
		{
			var query = from c in FDIDB.DN_Combo where c.ID == id select c;
			return query.FirstOrDefault();
		}
		public List<DN_Combo> GetListByArrId(List<int> ltsArrId)
		{
			var query = from c in FDIDB.DN_Combo where ltsArrId.Contains(c.ID) select c;
			return query.ToList();
		}
		public List<Shop_Product> GetListProduct(List<int> ltsArrId)
		{
			var query = from c in FDIDB.Shop_Product
				where ltsArrId.Contains(c.ID) && c.IsDelete == false
				select c;
			return query.ToList();
		}
		public void Add(DN_Combo item)
		{
			FDIDB.DN_Combo.Add(item);
		}
		public void Delete(DN_Combo item)
		{
			FDIDB.DN_Combo.Remove(item);
		}
		public void Save()
		{
			FDIDB.SaveChanges();
		}
	}
}