using DynaxInvoice.BO;
using System.Collections.Generic;

namespace DynaxInvoice.DL
{
    public interface IDbState
    {
        int AddState(DynaxState st);
        bool UpdateState(DynaxState st);
        DynaxState GetStateDetails(int id);
        IEnumerable<DynaxState> GetStateList();
    }
}
