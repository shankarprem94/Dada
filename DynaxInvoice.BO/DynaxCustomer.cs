namespace DynaxInvoice.BO
{
    public class DynaxCustomer
    {
        public int? Id { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public int StateId { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string GSTN { get; set; }
        public int DealerId { get; set; }
        public string DealerName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string StateName { get; set; }
        public int ZoneId { get; set; }
    }
}
