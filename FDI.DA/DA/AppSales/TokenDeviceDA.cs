using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA.DA.AppSales
{
    public class TokenDeviceDA:BaseDA
    {
        #region Constructer
        public TokenDeviceDA()
        {
        }

        public TokenDeviceDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public TokenDeviceDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<TokenDeiveItem> GetListToken()
        {
            var query = from c in FDIDB.TokenDevices
                select new TokenDeiveItem
                {
                    ID = c.ID,
                    AreaId = c.AreaId,
                    Token = c.Token,
                };
            return query.ToList();
        }
        public TokenDeiveItem GetTokenByToken(string token)
        {
            var query = from c in FDIDB.TokenDevices
                        where c.Token == token
                        select new TokenDeiveItem
                        {
                            ID = c.ID,
                            AreaId = c.AreaId,
                            Token = c.Token,
                        };
            return query.FirstOrDefault();
        }

        public TokenDeiveItem GetTokenById(int id)
        {
            var query = from c in FDIDB.TokenDevices
                        where c.ID == id
                        select new TokenDeiveItem
                        {
                            ID = c.ID,
                            AreaId = c.AreaId,
                            Token = c.Token,
                        };
            return query.FirstOrDefault();
        }
        public List<TokenDeiveItem> GetListTokenByArea(int areaId)
        {
            var query = from c in FDIDB.TokenDevices
                        where c.AreaId == areaId
                        select new TokenDeiveItem
                        {
                            ID = c.ID,
                            AreaId = c.AreaId,
                            Token = c.Token,
                        };
            return query.ToList();
        }

        public TokenDevice GetToken(string app, string token)
        {
            return FDIDB.TokenDevices.FirstOrDefault(m => m.App.Equals(app) && m.Token == token);
        }
        public TokenDevice GetToken( string token)
        {
            return FDIDB.TokenDevices.FirstOrDefault(m =>m.Token == token);
        }

        public void Add(TokenDevice item)
        {
            FDIDB.TokenDevices.Add(item);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
    }
}
