using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxCustomerBL
    {
        public int AddCustomer(DynaxCustomer cust)
        {
            try
            {
                var _objDb = new DbCustomer();
                var id = _objDb.AddCustomer(cust);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddCustomer() - " + ex.Message);
            }
        }

        public DynaxCustomer GetCustomerDetails(int id)
        {
            try
            {
                var _objDb = new DbCustomer();
                var ctDetails = _objDb.GetCustomerDetails(id);
                return ctDetails;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetCustomerDetails() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxCustomer> GetCustomerList(int id)
        {
            try
            {
                var _objDb = new DbCustomer();
                var ctDetailsList = _objDb.CustomerList(id);
                return ctDetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetCustomerList() - " + ex.Message);
            }
        }

        public bool UpdateCustomer(DynaxCustomer cust)
        {
            try
            {
                var _objDb = new DbCustomer();
                var flag = _objDb.UpdateCustomer(cust);
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:UpdateCustomer() - " + ex.Message);
            }
        }

        public IEnumerable<UserVm> GetUserList(int id)
        {
            try
            {
                var _objDb = new DbCustomer();
                var userList = _objDb.GetdealerWiseUserLis(id);
                return userList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetUserList() - " + ex.Message);
            }
        }
    }
}
