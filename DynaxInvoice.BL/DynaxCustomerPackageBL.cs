using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxCustomerPackageBL
    {
        public int AddCustomerPackage(CustomerPackage custPackage)
        {
            try
            {
                var _objDb = new DbCustomerPackage();
                var id = _objDb.AddCustomerPackage(custPackage);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddCustomerPackage() - " + ex.Message);
            }
        }

        public IEnumerable<PkgViewModel> GetCustomerPackageDetails(int id)
        {
            try
            {
                var _objDb = new DbCustomerPackage();
                var cPDetails = _objDb.GetCustomerPackageDetails(id);
                return cPDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetCustomerPackageDetails() - " + ex.Message);
            }
        }

        public IEnumerable<CustomerPackage> GetCustomerPackageList()
        {
            try
            {
                var _objDb = new DbCustomerPackage();
                var cPDetailsList = _objDb.GetCustomerPackageList();
                return cPDetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetCustomerPackageList() - " + ex.Message);
            }
        }

        public bool UpdateCustomerPackage(CustomerPackage custPackage)
        {
            try
            {
                var _objDb = new DbCustomerPackage();
                var flag = _objDb.UpdateCustomerPackage(custPackage);
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:UpdateCustomerPackage() - " + ex.Message);
            }
        }
    }
}