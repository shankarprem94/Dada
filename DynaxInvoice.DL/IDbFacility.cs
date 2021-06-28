using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.DL
{
    public interface IDbFacility
    {
        int AddFacility(DynaxFacility facility);
        DynaxFacility GetFacilityDetails(int id);
        bool UpdateFacility(DynaxFacility facility);
        IEnumerable<DynaxFacility> GetFacilityList();
    }
}
