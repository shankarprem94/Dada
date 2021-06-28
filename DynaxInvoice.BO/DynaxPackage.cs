
namespace DynaxInvoice.BO
{
    public class DynaxPackage
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string PackageDescription { get; set; }
        public int PackageAmount { get; set; }
        public bool Status { get; set; }
        public int MaxDiscount { get; set; }
    }
}
