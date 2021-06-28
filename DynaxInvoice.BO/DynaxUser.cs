
namespace DynaxInvoice.BO
{
   public class DynaxUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int UserType { get; set; }
        public int DealerId { get; set; }       
        public bool Status { get; set; }
    }
}
