using FDI.Base;
using FDI.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FDI.Utils;

namespace FDI.DA
{
    public class TokenOtpDA : BaseDA
    {
        public void Add(TokenOtp data)
        {
            FDIDB.TokenOtps.Add(data);
        }

        public bool ValidateToken(string token, string phone, int typeToken)
        {
            return FDIDB.TokenOtps.Any(m => m.IsDeleted == false && !m.IsUsed && m.ObjectId == phone && m.Token == token && m.TypeToken == typeToken);
        }

        public void UpdateIsUsed(string token, string phone)
        {
            var data = FDIDB.TokenOtps.FirstOrDefault(m => m.Token == token && m.ObjectId == phone && !m.IsUsed);
            if (data != null)
            {
                data.IsUsed = true;
            }
        }
    }
}
