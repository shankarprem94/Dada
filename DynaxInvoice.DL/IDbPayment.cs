using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.DL
{
    public interface IDbPayment
    {
        int AddPayment(DynaxPayment payment);        
        DynaxPayment GetPaymentDetails(int id);
        IEnumerable<DynaxPayment> GetPaymentList(int id);
    }
}
