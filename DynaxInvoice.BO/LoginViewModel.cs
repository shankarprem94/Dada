using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DynaxInvoice.BO
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter the userid")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter the password")]
        public string Password { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }

    }
}
