using DynaxInvoice.BO;
using System.Collections.Generic;

namespace DynaxInvoice.DL
{
    public interface IDbUser
    {
        int AddUser(DynaxUser zone);
        bool UpdateUser(DynaxUser zone);
        DynaxUser GetUserDetails(int id);
        IEnumerable<DynaxUser> GetUserList();
    }
}
