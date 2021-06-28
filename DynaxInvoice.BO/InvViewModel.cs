using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynaxInvoice.BO
{
    public class InvViewModel
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int TotalAmount { get; set; }
        public int  TotalDiscount { get; set; }
        public int TaxAmount { get; set; }
        public int TotalAfterDiscount { get; set; }
        public string DealerName { get; set; }
        public string DealerAddr1 { get; set; }
        public string DealerAddr2 { get; set; }
        public string DealerCity { get; set; }
        public string DealerState { get; set; }
        public string DealerPincode { get; set; }
        public string DealerMobile { get; set; }
        public string DealerEmail { get; set; }
        public string GstIn { get; set; }
        public string CompanyName { get; set; }        
        public string CompanyAddress1 { get; set; }
        public string CompanyAddress2 { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyPincode { get; set; }      
        public string CustMobile { get; set; }
        public string CustEmail { get; set; }
        public string Salesman { get; set; }
        public string SalesmanMobile { get; set; }
        public string SalesmanEmail { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public string Hotel { get; set; }
        public int DealerId { get; set; }
    }
}