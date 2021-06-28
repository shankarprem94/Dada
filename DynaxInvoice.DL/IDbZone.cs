using DynaxInvoice.BO;
using System.Collections.Generic;

namespace DynaxInvoice.DL
{
    public interface IDbZone
    {
        int AddZone(DynaxZone zone);
        bool UpdateZone(DynaxZone zone);
        DynaxZone GetZoneDetails(int id);
        IEnumerable<DynaxZone> GetZoneList(int id);
    }
}
