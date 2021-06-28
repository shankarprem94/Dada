using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BO
{
   public class InvoiceViewModel
    {
        public int Id { get; set; }       
       // public int ZoneId { get; set; }
        public int CustomerId { get; set; }
        public string HotelId { get; set; }
        public int PackageId { get; set; }
        public int Quantity { get; set; }
        public int PackageAmount { get; set; }
        public int PackageDiscount { get; set; }
        public int AmountAfterDiscount { get; set; }
        public int UserId { get; set; }       
        public string Remark { get; set; }       
        public string CustomerPackage { get; set; }
    }
}
