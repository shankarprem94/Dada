using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxInvoiceBL
    {
        public int AddInvoice(DynaxInvoices invoice)
        {
            try
            {
                var _objDb = new DbInvoice();
                var id = _objDb.AddInvoice(invoice);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddInvoice() - " + ex.Message);
            }
        }

        public void AddCustomerPkg(CustomerPackage custPkg)
        {
            try
            {
                var _objDb = new DbInvoice();
                _objDb.AddCustomerPackage(custPkg);               
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddCustomerPkg() - " + ex.Message);
            }
        }
     
        public void AddInvoiceHotel(InvoiceHotel objHotel)
        {
            try
            {
                var _objDb = new DbInvoice();
                _objDb.AddInvoiceHotel(objHotel);
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddInvoiceHotel() - " + ex.Message);
            }
        }
       
        public InvViewModel GetInvoiceDetails(int id)
        {
            try
            {
                var _objDb = new DbInvoice();
                var Details = _objDb.GetInvoiceDetails(id);
                return Details;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetInvoiceDetails() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxInvoices> GetInvoiceList(int userId)
        {
            try
            {
                var _objDb = new DbInvoice();
                var invList = _objDb.GetInvoiceList(userId);
                return invList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetInvoiceList() - " + ex.Message);
            }
        }
        
    }
}
