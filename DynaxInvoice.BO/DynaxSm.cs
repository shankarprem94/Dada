
namespace DynaxInvoice.BO
{
    public class DynaxSm
    {
        public int Id { get; set; }

        public string PackageName { get; set; }

        public int PackagePrice { get; set; }

        public string SmsQuantity { get; set; }

        public bool Status { get; set; }

        public int MaxDiscount { get; set; }
    }
}
