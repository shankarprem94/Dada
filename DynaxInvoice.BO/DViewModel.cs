using System.ComponentModel.DataAnnotations;

namespace DynaxInvoice.BO
{
    public class DViewModel
    {
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
