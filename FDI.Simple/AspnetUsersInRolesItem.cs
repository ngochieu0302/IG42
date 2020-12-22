using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class AspnetUsersInRolesItem
    {
        public int ID { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

    }
    public class ModelAspnetUsersInRolesItem : BaseModelSimple
    {
        public IEnumerable<AspnetUsersInRolesItem> ListItem { get; set; }
    }
}
