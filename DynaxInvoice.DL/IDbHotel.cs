using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.DL
{
    public interface IDbHotel
    {
        int AddHotel(DynaxHotel hotel);
        DynaxHotel GetHotelDetails(int id);
        bool UpdateHotel(DynaxHotel hotel);
        IEnumerable<DynaxHotel> GetHotelList(int id);
        IEnumerable<DynaxHotel> CustomersHotelList(int id);
    }
}
