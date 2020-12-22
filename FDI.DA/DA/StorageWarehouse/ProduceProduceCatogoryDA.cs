using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Base;
using FDI.CORE;
using FDI.Simple;
using FDI.Simple.StorageWarehouse;

namespace FDI.DA.DA.StorageWarehouse
{
    public class ProduceProduceCatogoryDA : BaseDA
    {
        public void Add(ProduceCatogory item)
        {
            FDIDB.ProduceCatogories.Add(item);
        }

    }
}
