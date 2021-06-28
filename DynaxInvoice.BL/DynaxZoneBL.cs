using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxZoneBL
    {
        public int AddZone(DynaxZone zone)
        {
            try
            {
                var _objDb = new DbZone();
                var id = _objDb.AddZone(zone);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddZone() - " + ex.Message);
            }
        }

        public DynaxZone GetZoneDetails(int id)
        {
            try
            {
                var _objDb = new DbZone();
                var stDetails = _objDb.GetZoneDetails(id);
                return stDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetZoneDetails() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxZone> GetZoneList(int id)
        {
            try
            {
                var _objDb = new DbZone();
                var stDetailsList = _objDb.GetZoneList(id);
                return stDetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetZoneList() - " + ex.Message);
            }
        }

        public bool UpdateZone(DynaxZone zone)
        {
            try
            {
                var _objDb = new DbZone();
                var flag = _objDb.UpdateZone(zone);
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:UpdateZone() - " + ex.Message);
            }
        }
   
        public IEnumerable<DynaxZone> GetDealersZone(int id)
        {
            try
            {
                var _objDb = new DbZone();
                var zoneList = _objDb.GetDealersZoneList(id);
                return zoneList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetDealersZone() - " + ex.Message);
            }
        }
    }

}
