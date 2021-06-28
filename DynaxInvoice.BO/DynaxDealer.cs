using System;

namespace DynaxInvoice.BO
{
    public class DynaxDealer
    {
        public int? Id { get; set; }
        public string DealerName { get; set; }
        public string DealerAddress1 { get; set; }
        public string DealerAddress2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string Pincode { get; set; }
        public string ZoneId { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string ContactPerson { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string GSTNo { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
    }
}
