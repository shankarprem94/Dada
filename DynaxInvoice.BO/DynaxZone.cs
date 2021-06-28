using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.BO
{
    public class DynaxZone
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string ZoneName { get; set; }       
        public bool Status { get; set; }
    }
}
