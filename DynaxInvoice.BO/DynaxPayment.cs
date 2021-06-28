using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BO
{
    public class DynaxPayment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string PaymentMode { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string BankName { get; set; }
        public int PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }
    }
}
