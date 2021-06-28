using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BO
{
    public class CustomerPackage
    {
        public int? Id { get; set; }
        public int InvoiceId { get; set; }
        public int PackageId { get; set; }
        public int Quantity { get; set; }
        public int PackageAmount { get; set; }        
        public int PackageDiscount { get; set; }
        public int AmountAfterDiscount { get; set; }
    }
}
