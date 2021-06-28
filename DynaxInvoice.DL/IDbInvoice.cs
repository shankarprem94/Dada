using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.DL
{
   public  interface IDbInvoice
    {
        int AddInvoice(DynaxInvoices invoice);
        InvViewModel GetInvoiceDetails(int id);
        IEnumerable<DynaxInvoices> GetInvoiceList(int userId);
        void AddCustomerPackage(CustomerPackage pkgCust);
    }
}
