using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynaxInvoice.BO
{
    public class PkgViewModel
    {
        public int PkgId { get; set; }
        public string PkgName { get; set; }
        public string PkgDescription { get; set; }
        public int PkgAmount { get; set; }
        public int Quantity { get; set; }
        public int PkgDiscount { get; set; }
        public int PkgAmountAfterDiscount { get; set; }
    }
}