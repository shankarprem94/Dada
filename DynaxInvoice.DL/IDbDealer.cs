using DynaxInvoice.BO;
using System.Collections.Generic;

namespace DynaxInvoice.DL
{
    public interface IDbDealer
    {
        int AddDealer(DynaxDealer zone);
        bool UpdateDealer(DynaxDealer zone);
        DynaxDealer GetDealerDetails(int id);
        IEnumerable<DynaxDealer> GetDealerList();
    }
}
