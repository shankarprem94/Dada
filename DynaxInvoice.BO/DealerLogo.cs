using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace DynaxInvoice.BO
{
    public class DealerLogo
    {
        public int DealerId { get; set; }

        //[FileTypes("jpg,jpeg,png")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public HttpPostedFileBase FileName { get; set; }
    }
}
