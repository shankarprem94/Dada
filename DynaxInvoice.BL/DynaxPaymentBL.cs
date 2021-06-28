using DynaxInvoice.BO;
using DynaxInvoice.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BL
{
    public class DynaxPaymentBL
    {
        public int AddPayment(DynaxPayment payment)
        {
            try
            {
                var _objDb = new DbPayment();
                var id = _objDb.AddPayment(payment);
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:AddPayment() - " + ex.Message);
            }
        }

        public DynaxPayment GetPaymentDetails(int id)
        {
            try
            {
                var _objDb = new DbPayment();
                var Details = _objDb.GetPaymentDetails(id);
                return Details;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetPaymenyDetails() - " + ex.Message);
            }
        }

        public IEnumerable<DynaxPayment> GetPaymentList(int Id)
        {
            try
            {
                var _objDb = new DbPayment();
                var DetailsList = _objDb.GetPaymentList(Id);
                return DetailsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Dynax:GetPaymenyList() - " + ex.Message);
            }
        }
              

     }
}
