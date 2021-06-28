using System;

namespace DynaxInvoice.BO
{
    public class DynaxInvoices
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int ZoneId { get; set; }
        public int CustomerId { get; set; }
       // public int HotelId { get; set; }       
        public int TotalDiscount { get; set; }
        public int TotalAmount { get; set; }
        public int TotalAfterDiscount { get; set; }
        public int TaxAmount { get; set; }
        public int UserId { get; set; }        
        public string Remark { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool Status { get; set; }
        //public string ZoneName { get; set; }
        public string CompanyName { get; set; }
        public string HotelName { get; set; }
        public string UserName { get; set; }
        public string DealerName { get; set; }
    }
}
