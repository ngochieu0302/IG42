using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class CustomerAppItem:BaseSimple
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string CodeLogin { get; set; }
        public int Status { get; set; }
        public string Company { get; set; }
        public string MST { get; set; }
        public string STK { get; set; }
        public string BankName { get; set; }
        public string FullName { get; set; }
        public string Depart { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public decimal? Birthday { get; set; }
        public decimal? Reward { get; set; }
        public string Phone { get; set; }
        public int? GroupID { get; set; }
        public string NameGroup { get; set; }
        public string PhoneAgency { get; set; }
        public int? AgencyId { get; set; }
    }
}
