using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxHotelBL
    {
        public int AddHotel(DynaxHotel hotel)
        {
            try
            {
                var _objDb = new DbHotel();
                var id = _objDb.AddHotel(hotel);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddHotel() - " + ex.Message);
            }
        }

        public DynaxHotel GetHotelDetails(int id)
        {
            try
            {
                var _objDb = new DbHotel();
                var Details = _objDb.GetHotelDetails(id);
                return Details;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetHotelDetails() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxHotel> GetHotelList(int id)
        {
            try
            {
                var _objDb = new DbHotel();
                var DetailsList = _objDb.GetHotelList(id);
                return DetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:DynaxHotel() - " + ex.Message);
            }
        }

        public bool UpdateHotel(DynaxHotel hotel)
        {
            try
            {
                var _objDb = new DbHotel();
                var flag = _objDb.UpdateHotel(hotel);
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:UpdateHotel() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxHotel> CustomerWiseHotels(int id)
        {
            try
            {
                var _objDb = new DbHotel();
                var hotelList = _objDb.CustomersHotelList(id);
                return hotelList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:DynaxHotel() - " + ex.Message);
            }
        }
    }
}
