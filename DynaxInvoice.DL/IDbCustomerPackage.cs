using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.DL
{
    public interface IDbCustomerPackage
    {
        int AddCustomerPackage(CustomerPackage custPackage);
        bool UpdateCustomerPackage(CustomerPackage custPackage);
        IEnumerable<PkgViewModel> GetCustomerPackageDetails(int id);
        IEnumerable<CustomerPackage> GetCustomerPackageList();
    }
}
