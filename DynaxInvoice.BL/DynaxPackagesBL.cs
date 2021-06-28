using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxPackagesBL
    {
        public int AddPackage(DynaxPackage obj)
        {
            try
            {
                var _objDb = new DbPackages();
                var id = _objDb.AddPackage(obj);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddPackage() - " + ex.Message);
            }
        }

        public DynaxPackage GetPackageDetails(int id)
        {
            try
            {
                var _objDb = new DbPackages();
                var pkg = _objDb.GetPackageDetails(id);
                return pkg;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetPackageDetails() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxPackage> GetPackageList()
        {
            try
            {
                var _objDb = new DbPackages();
                var pcDetailsList = _objDb.GetPackageList();
                return pcDetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetPackageList() - " + ex.Message);
            }
        }

        public bool UpdatePackage(DynaxPackage obj)
        {
            try
            {
                var _objDb = new DbPackages();
                var flag = _objDb.UpdatePackage(obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:UpdatePackage() - " + ex.Message);
            }
        }
    }
}
