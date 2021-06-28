using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxFacilityBL
    {
        public int AddFacility(DynaxFacility facility)
        {
            try
            {
                var _objDb = new DbFacility();
                var id = _objDb.AddFacility(facility);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddFacility() - " + ex.Message);
            }
        }

        public DynaxFacility GetFacilityDetails(int id)
        {
            try
            {
                var _objDb = new DbFacility();
                var Details = _objDb.GetFacilityDetails(id);
                return Details;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetFacilityDetails() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxFacility> GetFacilityList()
        {
            try
            {
                var _objDb = new DbFacility();
                var DetailsList = _objDb.GetFacilityList();
                return DetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetFacilityList() - " + ex.Message);
            }
        }

        public bool UpdateFacility(DynaxFacility facility)
        {
            try
            {
                var _objDb = new DbFacility();
                var flag = _objDb.UpdateFacility(facility);
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:UpdateFacility() - " + ex.Message);
            }
        }
    }
}
