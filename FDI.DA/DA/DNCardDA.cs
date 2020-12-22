using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA
{
    public class DNCardDA : BaseDA
    {
        #region Constructer
        public DNCardDA()
        {
        }

        public DNCardDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public DNCardDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<DNCardItem> GetListByCustomer(int customerId)
        {
            var query = from c in FDIDB.Send_Card
                        where c.CustomerID == customerId
                        select new DNCardItem
                        {
                            ID = c.ID,
                            Code = c.DN_Card.Code,
                            Serial = c.DN_Card.Serial,
                            PinCard = c.DN_Card.PinCard,
                            CreateDate = c.DateCreate,
                            CustomerName = c.DN_Card.Customers.Select(o => o.FullName).FirstOrDefault()
                        };
            return query.ToList();
        }
        public List<DNCardItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Card
                        orderby c.ID descending
                        select new DNCardItem
                        {
                            ID = c.ID,
                            Serial = c.Serial,
                            Code = c.Code,
                            PinCard = c.PinCard,
                            CreateDate = c.DN_OrderCard.DateCreate,
                            Status = c.Status
                        };
            var status = httpRequest.QueryString["Status"];
            if (!string.IsNullOrEmpty(status))
            {
                var s = int.Parse(status);
                query = query.Where(c => c.Status == s);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<DNCardItem> GetListExport(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.DN_Card
                        orderby c.ID descending
                        select new DNCardItem
                        {
                            ID = c.ID,
                            Serial = c.Serial,
                            Code = c.Code,
                            PinCard = c.PinCard
                        };
            query = query.SelectByRequest(Request);
            return query.ToList();
        }
        public DNCardItem GetCardItem(string card)
        {
            var query = from c in FDIDB.DN_Card
                        where c.Serial == card
                        select new DNCardItem
                        {
                            Code = c.Code,
                            Serial = c.Serial
                        };
            return query.FirstOrDefault();
        }
        public DNCardItem GetCardItem(string card, string pinCard)
        {
            var query = from c in FDIDB.DN_Card
                        where c.Serial == card && c.PinCard == pinCard
                        select new DNCardItem
                        {
                            ID = c.ID,
                            Serial = c.Serial,
                            Code = c.Code,
                            PinCard = c.PinCard
                        };
            return query.FirstOrDefault();
        }
        public bool CheckSendCard(int? id)
        {
            return FDIDB.Send_Card.Any(c => c.CardID == id);
        }
        public bool CheckCardSerial(string sirial)
        {
            const int stt = (int)Card.Released;
            var query1 = FDIDB.DN_Card.Where(c => c.Serial == sirial && c.Customers.Any() == false && c.Status == stt).Select(c => c.ID);
            return query1.Any();
        }

        public DN_Card GetById(int id)
        {
            var query = from c in FDIDB.DN_Card where c.ID == id select c;
            return query.FirstOrDefault();
        }
        public int Count(int? code)
        {
            var query = FDIDB.DN_Card.Where(c => c.DN_OrderCard.Code == code).Select(c => c.Code);
            return query.Count();
        }
        public List<DN_Card> GetById(List<int> ltsArrId)
        {
            var query = from c in FDIDB.DN_Card where ltsArrId.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<SuggestionsProduct> GetListAuto(string keword, int showLimit)
        {
            var query = from c in FDIDB.DN_Card
                        where c.IsActive == false && c.Code == keword
                        select new SuggestionsProduct
                        {
                            value = c.Code,
                            title = c.Serial,
                            data = c.PinCard
                        };
            return query.Take(showLimit).ToList();
        }
        public List<DN_Card> UpdateCard(string firt, string old)
        {
            var firtId = FDIDB.DN_Card.Where(c => c.Serial == firt).Select(c => c.ID).FirstOrDefault();
            var oldId = FDIDB.DN_Card.Where(c => c.Serial == old).Select(c => c.ID).FirstOrDefault();
            var query = FDIDB.DN_Card.Where(c => c.ID >= firtId && c.ID <= oldId).Select(c => c);
            return query.ToList();
        }
        public void Add(DN_OrderCard item)
        {
            FDIDB.DN_OrderCard.Add(item);
        }
        public void Add(Send_Card item)
        {
            FDIDB.Send_Card.Add(item);
        }
        public void Add(DN_Card item)
        {
            FDIDB.DN_Card.Add(item);
        }
        public void Delete(DN_Card item)
        {
            FDIDB.DN_Card.Remove(item);
        }
        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
