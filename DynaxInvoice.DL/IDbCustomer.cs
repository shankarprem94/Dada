using DynaxInvoice.BO;
using System.Collections.Generic;

namespace DynaxInvoice.DL
{
    public interface IDbCustomer
    {
        int AddCustomer(DynaxCustomer cust);
        bool UpdateCustomer(DynaxCustomer cust);
        DynaxCustomer GetCustomerDetails(int id);
        IEnumerable<DynaxCustomer> CustomerList(int id);
    }
}
