using FDI.Base;
using FDI.Simple.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FDI.Simple.Logistics;

namespace FDI.DA.DA.Logistics
{
    public class CarDA : BaseDA
    {
        public List<CarItem> GetListSimpleByRequest(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from o in FDIDB.Cars
                        where o.IsDelete == false
                        orderby o.ID descending
                        select new CarItem()
                        {
                            ID = o.ID,
                            Quantity = o.Quantity,
                            Name = o.Name,
                            Address = o.Address,
                            CarType = o.CarType,
                            Latitude = o.Latitude,
                            Longitude = o.Longitude,
                            Phone = o.Phone,
                            UnitID = o.UnitID,
                            UnitName = o.DN_Unit.Name
                        };
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public void Add(Car item)
        {
            FDIDB.Cars.Add(item);
        }
        public Car GetById(int id)
        {
            return FDIDB.Cars.FirstOrDefault(m => m.ID == id);
        }

        public CarItem GetItemById(int id)
        {
            return FDIDB.Cars.Where(m => m.ID == id).Select(o => new CarItem()
            {
                ID = o.ID,
                Quantity = o.Quantity,
                Name = o.Name,
                Address = o.Address,
                CarType = o.CarType,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
                Phone = o.Phone,
                UnitID = o.UnitID,
                UnitName = o.DN_Unit.Name
            }).FirstOrDefault();
        }
        public List<CarItem> GetAll()
        {
            return FDIDB.Cars.Select(o => new CarItem()
            {
                ID = o.ID,
                Quantity = o.Quantity,
                Name = o.Name,
                Address = o.Address,
                CarType = o.CarType,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
                Phone = o.Phone,
                UnitID = o.UnitID,
                UnitName = o.DN_Unit.Name
            }).ToList();
        }

        public List<CarItem> GetListAssign(int unitId)
        {
            return FDIDB.Cars.Where(m => m.UnitID == unitId && m.IsDelete == false).Select(o => new CarItem()
            {
                ID = o.ID,
                Quantity = o.Quantity,
                Name = o.Name,
                Address = o.Address,
                CarType = o.CarType,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
                Phone = o.Phone,
                UnitID = o.UnitID,
                UnitName = o.DN_Unit.Name
            }).ToList();
        }


    }
}
