using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Base;

namespace FDI.DA.DA.StorageWarehouse
{
    public class StorageWareHouseLogDA : BaseDA
    {
        public void AddLog(StorageWarehousingLog item)
        {
            FDIDB.StorageWarehousingLogs.Add(item);
            FDIDB.StorageWarehousingLogs.Add(item);
        }
    }
}
