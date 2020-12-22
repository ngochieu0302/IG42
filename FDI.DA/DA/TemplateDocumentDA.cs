using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA
{
    public class TemplateDocumentDA:BaseDA
    {
        readonly DNDocumentDA _dnDocumentDa = new DNDocumentDA("#");
        #region Contruction
        public TemplateDocumentDA()
        {
        }
        public TemplateDocumentDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public TemplateDocumentDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion
        public List<TemplateDocumentItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var id = int.Parse(httpRequest["type_T"] ?? "0");
            var query = from o in FDIDB.TemplateDocuments
                        where o.IsDelete != true && o.IsShow == true
                        orderby o.ID descending
                        select new TemplateDocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            DateCreate = o.DateCreate,
                            Type = o.Type,
                            Description = o.Description,
                        };
            if (id>0)
            {
                query = query.Where(c => c.Type == id);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<TemplateDocumentItem> GetList(int id)
        {
            var query = from o in FDIDB.TemplateDocuments
                        where o.IsDelete != true && o.IsShow == true
                        orderby o.ID descending
                        select new TemplateDocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                        };
            return query.ToList();
        }
        public TemplateDocumentItem GetTemplateDocItem(int id)
        {
            var query = from o in FDIDB.TemplateDocuments
                        where o.ID == id
                        orderby o.ID descending
                        select new TemplateDocumentItem
                        {
                            ID = o.ID,
                            Name = o.Name,
                            DateCreate = o.DateCreate,
                            Type = o.Type,
                            Description = o.Description,
                            Content = o.Content,
                        };
            return query.FirstOrDefault();
        }
        public string GetItemByIDAndIDDoc(int id, int idncc)
        {
            var query = (from o in FDIDB.TemplateDocuments
                         where o.ID == id
                         orderby o.ID descending
                         select o.Content).FirstOrDefault();
            if (query != null)
            {
                var obj = _dnDocumentDa.GetById(idncc);
                if (obj != null)
                {
                    query = query.Replace("{NameB}", obj.NameB);
                    query = query.Replace("{AddressB}", obj.Address);
                    query = query.Replace("{MSTB}", obj.MST);
                    query = query.Replace("{MobieB}", obj.MobieB);
                    query = query.Replace("{STKB}", obj.STK);
                    query = query.Replace("{BankName}", obj.BankName);
                    query = query.Replace("{Department}", obj.Department);
                    query = query.Replace("{Value}", obj.Value.Money());
                    query = query.Replace("{DateStart}", obj.DateStart.DecimalToString("dd/MM/yyyy"));
                    query = query.Replace("{DateEnd}", obj.DateEnd.DecimalToString("dd/MM/yyyy"));
                    query = query.Replace("{NameCompanyB}", obj.NameCompany);
                    query = query.Replace("{Deposit}", obj.Deposit.Money());
                    query = query.Replace("{Value}", obj.Value.Money());
                    query = query.Replace("{marketname}", obj.DN_Agency.Market.Name);
                    var readdesposit = obj.Deposit.NumberToWord(" đ");
                    var val = obj.Value.NumberToWord(" đ");
                    query = query.Replace("{ReadDeposit}", readdesposit);
                    query = query.Replace("{ReadValue}", val);
                    return query;
                }
            }
            return "";
        }
        public TemplateDocument GetById(int id)
        {
            var query = from o in FDIDB.TemplateDocuments where o.ID == id select o;
            return query.FirstOrDefault();
        }
        public List<TemplateDocument> GetListArrId(List<int> lst)
        {
            var query = from o in FDIDB.TemplateDocuments where lst.Contains(o.ID) select o;
            return query.ToList();
        }
        public void Add(TemplateDocument item)
        {
            FDIDB.TemplateDocuments.Add(item);
        }
        public void Delete(TemplateDocument item)
        {
            FDIDB.TemplateDocuments.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
