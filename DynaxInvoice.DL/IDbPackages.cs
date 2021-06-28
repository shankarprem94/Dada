using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.DL
{
   public interface IDbPackages
    {
        int AddPackage(DynaxPackage hotel);
        DynaxPackage GetPackageDetails(int id);
        bool UpdatePackage(DynaxPackage hotel);
        IEnumerable<DynaxPackage> GetPackageList();
    }
}
