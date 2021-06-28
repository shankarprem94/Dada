using DynaxInvoice.BL;
using DynaxInvoice.BO;
using DynaxInvoice.Utility;
using ExpertPdf.HtmlToPdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace DynaxInvoice.Web.Controllers
{
    [Authorize]
    public class DinvoiceController : Controller
    {
        // GET: Dinvoice
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllInvoice()
        {
            IEnumerable<DynaxInvoices> lst = new List<DynaxInvoices>();
            int Count = 0;

            try
            {
                var userId = DynaxInvoice.Web.Models.ClaimsExtensions.GetUserId(this.User);
                DynaxInvoiceBL obj = new DynaxInvoiceBL();
                lst = obj.GetInvoiceList(userId.Value);
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            var objFac = lst.Select(s => new
            {
                id = s.Id,
                invoiceDate = String.Format("{0:dd MMM yyyy}", s.InvoiceDate),
                taxAmount = String.Format("{0:#,000.00}", s.TaxAmount),
                totalDiscount = String.Format("{0:#,000.00}", s.TotalDiscount),
                totalAmount = String.Format("{0:#,000.00}", s.TotalAmount),
                activationDate = String.Format("{0:dd MMM yyyy}", s.ActivationDate),
                //zoneName = s.ZoneName,
                companyName = s.CompanyName,
                hotelName = s.HotelName,
                dealerName = s.DealerName,
                paymentLink = PaymentLink(s.Status, s.Id)
            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 10, data = objFac }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {

            var uid = Models.ClaimsExtensions.GetUserId(this.User);
            DynaxCustomerBL objCustomer = new DynaxCustomerBL();
            var CustomerList = objCustomer.GetCustomerList(uid.Value);
            ViewBag.cstList = CustomerList;
            var UserList = objCustomer.GetUserList(uid.Value);
            ViewBag.UserList = UserList;

            //DynaxZoneBL ObjZone = new DynaxZoneBL();
            //var ZoneList = ObjZone.GetDealersZone(uid.Value);
            //ViewBag.ZoneList = ZoneList;

            DynaxPackagesBL objPack = new DynaxPackagesBL();
            var PackageLists = objPack.GetPackageList();
            ViewBag.PackageList = PackageLists;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceViewModel obInv)
        {
            try
            {
                var uid = Models.ClaimsExtensions.GetUserId(this.User);
                DynaxCustomerBL objCustomer = new DynaxCustomerBL();
                var CustomerList = objCustomer.GetCustomerList(uid.Value);
                ViewBag.cstList = CustomerList;
                var UserList = objCustomer.GetUserList(uid.Value);
                ViewBag.UserList = UserList;

                //DynaxZoneBL ObjZone = new DynaxZoneBL();
                //var ZoneList = ObjZone.GetDealersZone(uid.Value);
                //ViewBag.ZoneList = ZoneList;

                DynaxPackagesBL objPack = new DynaxPackagesBL();
                var PackageLists = objPack.GetPackageList();
                ViewBag.PackageList = PackageLists;

                int totalAmount = 0;
                int totalDiscount = 0;
                int totalAfterDiscount = 0;
                int taxAmount = 0;
                var custPkg = GetCustomerPackages(obInv.CustomerPackage);
                foreach (var pk in custPkg)
                {
                    totalAmount += (pk.PackageAmount * pk.Quantity);
                    totalDiscount += pk.PackageDiscount;
                    totalAfterDiscount += pk.AmountAfterDiscount;
                }

                taxAmount = TaxAmount(totalAfterDiscount);

                ViewBag.Status = 0;
                DynaxInvoiceBL objBl = new DynaxInvoiceBL();
                DynaxInvoices objInvoice = new DynaxInvoices();

                objInvoice.InvoiceDate = DateTime.Now;
                objInvoice.CustomerId = obInv.CustomerId;
                //objInvoice.HotelId = 0;
                //objInvoice.ZoneId = obInv.ZoneId;
                objInvoice.TotalAmount = totalAmount;
                objInvoice.TotalDiscount = totalDiscount;
                objInvoice.TotalAfterDiscount = totalAfterDiscount;
                objInvoice.TaxAmount = taxAmount;
                objInvoice.ActivationDate = DateTime.Now;
                objInvoice.ExpiryDate = DateTime.Now.AddDays(365);
                objInvoice.Remark = ((obInv.Remark == null) ? "" : obInv.Remark);
                objInvoice.Status = false;
                objInvoice.UserId = obInv.UserId;

                int id = objBl.AddInvoice(objInvoice);
                if (id > 0)
                {
                    foreach (var pk in custPkg)
                    {
                        pk.InvoiceId = id;
                        objBl.AddCustomerPkg(pk);
                    }

                    var lstHotel = GetInvoiceHotels(obInv.HotelId);
                    foreach (var hotel in lstHotel)
                    {
                        hotel.InvoiceId = id;
                        objBl.AddInvoiceHotel(hotel);
                    }

                    DynaxCustomerPackageBL objPkg = new DynaxCustomerPackageBL();
                    var objCustPkg = objPkg.GetCustomerPackageDetails(id);
                    var objInv = objBl.GetInvoiceDetails(id);

                    string mailContent = GetInvoiceDetails(objInv, objCustPkg);

                    string strSMTP = ConfigurationManager.AppSettings["SMTP"].ToString();
                    string strFrom = ConfigurationManager.AppSettings["FROMEMAIL"].ToString();
                    string strPWD = ConfigurationManager.AppSettings["PASSWORD"].ToString();
                    string port = ConfigurationManager.AppSettings["PORT"].ToString();
                    string strTO = ConfigurationManager.AppSettings["TOEMAIL"].ToString();
                    // string strMobile = ConfigurationManager.AppSettings["MOBILE"].ToString();
                    // strMobile = strMobile + "," + objInv.DealerMobile + "," + objInv.CustMobile + "," + objInv.SalesmanMobile;

                    var objUtility = new Utilities();

                    string strCC = objInv.DealerEmail + ";" + objInv.SalesmanEmail;
                    objUtility.SendEmail(strFrom, objInv.CustEmail, strCC, strTO, strSMTP, strPWD, int.Parse(port), "Booking Master: Invoice", mailContent);

                    ViewBag.Status = id;
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            return View();
        }

        [HttpGet]
        public ActionResult Payment(int Id)
        {
            DynaxPayment objPayment = new DynaxPayment();
            try
            {
                DynaxInvoiceBL ObjBl = new DynaxInvoiceBL();
                var objInvoice = ObjBl.GetInvoiceDetails(Id);
                objPayment.InvoiceId = objInvoice.Id;
                objPayment.PaidAmount = objInvoice.TotalAfterDiscount;
                objPayment.ChequeNumber = "";
                objPayment.ChequeDate = DateTime.Today;
                objPayment.TransactionId = "";
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(objPayment);
        }

        [HttpPost]
        public ActionResult Payment(DynaxPayment objPayment)
        {
            try
            {
                DynaxPaymentBL ObjBl = new DynaxPaymentBL();
                objPayment.PaymentDate = DateTime.Now;
                var id = ObjBl.AddPayment(objPayment);
                if (id > 0)
                {
                    DynaxInvoiceBL objBl = new DynaxInvoiceBL();
                    DynaxCustomerPackageBL objPkg = new DynaxCustomerPackageBL();
                    var objCustPkg = objPkg.GetCustomerPackageDetails(objPayment.InvoiceId);
                    var objInvoice = objBl.GetInvoiceDetails(objPayment.InvoiceId);

                    string strSMTP = ConfigurationManager.AppSettings["SMTP"].ToString();
                    string strFrom = ConfigurationManager.AppSettings["FROMEMAIL"].ToString();
                    string strPWD = ConfigurationManager.AppSettings["PASSWORD"].ToString();
                    string port = ConfigurationManager.AppSettings["PORT"].ToString();
                    string strTO = ConfigurationManager.AppSettings["TOEMAIL"].ToString();
                    string strMobile = ConfigurationManager.AppSettings["MOBILE"].ToString();
                    strMobile = strMobile + "," + objInvoice.DealerMobile + "," + objInvoice.CustMobile + "," + objInvoice.SalesmanMobile;
                    var ccEmail = objInvoice.DealerEmail + ";" + objInvoice.SalesmanEmail;
                   
                    var objUtility = new Utilities();

                    var receiptContent = GetReceiptDetails(objInvoice, objPayment);
                    objUtility.SendEmail(strFrom, objInvoice.CustEmail, ccEmail, strTO, strSMTP, strPWD, int.Parse(port), "Booking Master: Receipt", receiptContent);

                    string strMessage = "We hereby acknowledge receipt of Your payment of Rs. " + String.Format("{0:#,000.00}", objPayment.PaidAmount);
                    if(objPayment.PaymentMode== "CHEQUE")
                    {
                        strMessage += " (by Chq. No. : " + objPayment.ChequeNumber + ")";
                    }
                    else if (objPayment.PaymentMode == "CASH")
                    {
                        strMessage += " (by CASH )";
                    }
                    else if (objPayment.PaymentMode == "CHEQUE")
                    {
                        strMessage += " (by NEFT : " + objPayment.TransactionId + ")";
                    }
                    strMessage += "for Booking Master Licence (Validity 1 Year). Invoice No.: DSBM/" + objInvoice.Id + ". Thanking for joining us.\n\nOur IT Team will contact you soon for further correspondence.\n\nTeam - bookingMaster.in";

                    SendSMS(strMobile, strMessage);

                    return Redirect(Url.Content("~/dinvoice"));
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View();
        }

        [HttpGet]
        public ActionResult PaymentDetails()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllPayment()
        {
            IEnumerable<DynaxPayment> lst = new List<DynaxPayment>();
            int Count = 0;

            try
            {
                var userId = DynaxInvoice.Web.Models.ClaimsExtensions.GetUserId(this.User);
                DynaxPaymentBL obj = new DynaxPaymentBL();
                lst = obj.GetPaymentList(userId.Value);
                Count = lst.Count();
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            var objFac = lst.Select(P => new
            {
                id = P.Id,
                invoiceId = P.InvoiceId,
                paymentMode = P.PaymentMode,
                paidAmount = String.Format("{0:#,000.00}", P.PaidAmount),
                chqNumber = P.ChequeNumber,
                chqDate = ((P.ChequeNumber == "") ? "" : String.Format("{0:dd MMM yyyy}", P.ChequeDate)),
                bankName = P.BankName,
                paymentDate = String.Format("{0:dd MMM yyyy}", P.PaymentDate),
                TransactionId = P.TransactionId

            });
            return Json(new { draw = 1, recordsTotal = Count, recordsFiltered = 10, data = objFac }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ViewInvoice(int id)
        {
            DynaxInvoiceBL objBl = new DynaxInvoiceBL();
            DynaxCustomerPackageBL objPkg = new DynaxCustomerPackageBL();
            var objCustPkg = objPkg.GetCustomerPackageDetails(id);
            var objInvoice = objBl.GetInvoiceDetails(id);

            string mailContent = GetInvoiceDetails(objInvoice, objCustPkg);
            ViewBag.Invoice = mailContent;
            ViewBag.Id = id;
            return View();

        }

        [HttpGet]
        public ActionResult ViewReceipt(int id)
        {
            DynaxInvoiceBL objBl = new DynaxInvoiceBL();
            DynaxPaymentBL objPkg = new DynaxPaymentBL();
            var objCustPkg = objPkg.GetPaymentDetails(id);
            var objInvoice = objBl.GetInvoiceDetails(id);

            string mailContent = GetReceiptDetails(objInvoice, objCustPkg);
            ViewBag.Receipt = mailContent;
            return View();

        }

        [HttpGet]
        public ActionResult Download(int id)
        {
            DynaxInvoiceBL objBl = new DynaxInvoiceBL();
            DynaxCustomerPackageBL objPkg = new DynaxCustomerPackageBL();
            var objCustPkg = objPkg.GetCustomerPackageDetails(id);
            var objInvoice = objBl.GetInvoiceDetails(id);

            string mailContent = GetInvoiceDetails(objInvoice, objCustPkg);
            string fileName = "Inv-" + id + ".pdf";
            ConvertHtmltoPDF(mailContent, fileName);
            return RedirectToAction("ViewInvoice");
        }

        [HttpGet]
        public JsonResult GetHotelList(int Id)
        {

            DynaxHotelBL obj = new DynaxHotelBL();
            var lstHotel = obj.CustomerWiseHotels(Id);
            var hotelJason = lstHotel.Select(x => new
            {
                id = x.Id,
                hotelName = x.HotelName
            });

            return Json(hotelJason, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPackageDetails(int id)
        {
            DynaxPackagesBL objPkg = new DynaxPackagesBL();
            var lstPkg = objPkg.GetPackageDetails(id);

            return Json(lstPkg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCustomerDetails(int id)
        {
            DynaxCustomerBL objPkg = new DynaxCustomerBL();
            var lstPkg = objPkg.GetCustomerDetails(id);
            return Json(lstPkg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDealerZoneList(int id)
        {
            DynaxZoneBL objPkg = new DynaxZoneBL();
            var lstPkg = objPkg.GetDealersZone(id);
            var zoneJason = lstPkg.Select(x => new
            {
                id = x.Id,
               zoneName = x.ZoneName
            });
            return Json(zoneJason, JsonRequestBehavior.AllowGet);
        }

       


        private int TaxAmount(int amount)
        {
            int Amount = 0;
            int Tax = 0;
            Amount = ((amount * 100) / 118);
            Tax = amount - Amount;
            return Tax;
        }

        protected IEnumerable<CustomerPackage> GetCustomerPackages(string customerPackage)
        {
            var lstPackage = new List<CustomerPackage>();
            if (customerPackage.Length > 0)
            {
                string[] strPackageList = customerPackage.Split('|');
                for (int i = 0; i < strPackageList.Length; i++)
                {
                    string[] pkg = strPackageList[i].Split('~');
                    var objPkg = new CustomerPackage
                    {
                        PackageId = int.Parse(pkg[0]),
                        PackageAmount = int.Parse(pkg[2]),
                        Quantity = int.Parse(pkg[3]),
                        PackageDiscount = int.Parse(pkg[4]),
                        AmountAfterDiscount = int.Parse(pkg[5])
                    };

                    lstPackage.Add(objPkg);
                }
            }
            return lstPackage;
        }

        protected IEnumerable<InvoiceHotel> GetInvoiceHotels(string invoiceHotel)
        {
            var lstHotel = new List<InvoiceHotel>();
            if (invoiceHotel.Length > 0)
            {
                string[] strHotelList = invoiceHotel.Split(',');
                for (int i = 0; i < strHotelList.Length; i++)
                {
                    var objhotel = new InvoiceHotel
                    {
                        HotelId = int.Parse(strHotelList[i])
                    };

                    lstHotel.Add(objhotel);
                }
            }
            return lstHotel;
        }

        private string GetInvoiceDetails(InvViewModel objInvoice, IEnumerable<PkgViewModel> objPkg)
        {
            var mailContents = System.IO.File.ReadAllText(Server.MapPath(@"~/mailer/PrintInvoice.html"));

            mailContents = mailContents.Replace("#DealerName#", objInvoice.DealerName);
            mailContents = mailContents.Replace("#DealerAddr1#", objInvoice.DealerAddr1);
            mailContents = mailContents.Replace("#DealerAddr2#", objInvoice.DealerAddr2);
            mailContents = mailContents.Replace("#DealerCity#", objInvoice.DealerCity);
            mailContents = mailContents.Replace("#DealerPincode#", objInvoice.DealerPincode);
            mailContents = mailContents.Replace("#DealerMobile#", objInvoice.DealerMobile);
            mailContents = mailContents.Replace("#DealerEmail#", objInvoice.DealerEmail);
            mailContents = mailContents.Replace("#GST#", objInvoice.GstIn);

            mailContents = mailContents.Replace("#Company#", objInvoice.CompanyName);
            mailContents = mailContents.Replace("#CompanyAddress1#", objInvoice.CompanyAddress1);
            string strAddress2 = "";
            if (!string.IsNullOrEmpty(objInvoice.CompanyAddress2))
                strAddress2 = objInvoice.CompanyAddress2 + "<br>";
            mailContents = mailContents.Replace("#CompanyAddress2#", strAddress2);
            mailContents = mailContents.Replace("#CompanyCity#", objInvoice.CompanyCity);
            mailContents = mailContents.Replace("#CompanyPincode#", objInvoice.CompanyPincode);
            mailContents = mailContents.Replace("#CompanyMobile#", objInvoice.CustMobile);
            mailContents = mailContents.Replace("#CompanyEmail#", objInvoice.CustEmail);

            mailContents = mailContents.Replace("#InvNo#", objInvoice.Id.ToString());
            mailContents = mailContents.Replace("#InvDate#", String.Format("{0:dd MMM yyyy}", objInvoice.InvoiceDate));
            var packageDetails = "";
            int count = 0;
            foreach (var pk in objPkg)
            {
                count++;
                if (count > 1)
                {
                    packageDetails += "<tr style=\"border-top:1px solid #ccc; border-bottom:1px solid #eee;\">";
                    packageDetails += "<td style =\"text-align:left\"><b>" + pk.PkgName + "</b><br>" + pk.PkgDescription + "</td>";
                    packageDetails += "<td style =\"text-align:right\">Rs. " + String.Format("{0:#,000.00}", pk.PkgAmount) + " </td>";
                    packageDetails += "<td style =\"text-align:right\">" + pk.Quantity + "</td>";
                    packageDetails += "<td style =\"text-align:right\">Rs. " + String.Format("{0:#,000.00}", pk.PkgDiscount) + " </td>";
                    packageDetails += "<td style =\"text-align:right\">Rs. " + String.Format("{0:#,000.00}", pk.PkgAmountAfterDiscount) + "</td></tr>";
                }
                else
                {
                    packageDetails += "<tr style=\"border-top:2px solid #ccc; border-bottom:1px solid #eee;\">";
                    packageDetails += "<td style =\"text-align:left\"><b>" + pk.PkgName + "</b><br>" + pk.PkgDescription + "</td>";
                    packageDetails += "<td style =\"text-align:right\">Rs. " + String.Format("{0:#,000.00}", pk.PkgAmount) + " </td>";
                    packageDetails += "<td style =\"text-align:right\">" + pk.Quantity + "</td>";
                    packageDetails += "<td style =\"text-align:right\">Rs. " + String.Format("{0:#,000.00}", pk.PkgDiscount) + " </td>";
                    packageDetails += "<td style =\"text-align:right\">Rs. " + String.Format("{0:#,000.00}", pk.PkgAmountAfterDiscount) + "</td></tr>";
                }
            }

            mailContents = mailContents.Replace("#PkgDetails#", packageDetails);

            mailContents = mailContents.Replace("#AccountName#", objInvoice.AccountName);
            mailContents = mailContents.Replace("#AccountNumber#", objInvoice.AccountNo);
            mailContents = mailContents.Replace("#BankName#", objInvoice.BankName);
            mailContents = mailContents.Replace("#IFSCCode#", objInvoice.IFSCCode);
            mailContents = mailContents.Replace("#Hotel#", objInvoice.Hotel);
            mailContents = mailContents.Replace("#TotalAmount#", String.Format("{0:#,000.00}", objInvoice.TotalAfterDiscount));
            mailContents = mailContents.Replace("#TaxAmt#", String.Format("{0:#,000.00}", objInvoice.TaxAmount));
            mailContents = mailContents.Replace("#NetTot#", String.Format("{0:#,000.00}", (objInvoice.TotalAfterDiscount - objInvoice.TaxAmount)));
            mailContents = mailContents.Replace("#GrantTot#", String.Format("{0:#,000.00}", objInvoice.TotalAfterDiscount));
            mailContents = mailContents.Replace("#AmtTot#", String.Format("{0:#,000.00}", objInvoice.TotalAfterDiscount));
            var strImg = "<img src=\"http://booking.dynaxsolutions.co.in/images/" + objInvoice.DealerId + ".jpg\">";
            mailContents = mailContents.Replace("#DealerSeal#", strImg);

            return mailContents;
        }

        private string GetReceiptDetails(InvViewModel objInvoic, DynaxPayment objPay)
        {
            var mailContents = System.IO.File.ReadAllText(Server.MapPath(@"~/mailer/Receipt.html"));
            var strImg = "<img src=\"http://booking.dynaxsolutions.co.in/images/"+ objInvoic.DealerId+ ".jpg\">";
            mailContents = mailContents.Replace("#DealerSeal#", strImg);
            mailContents = mailContents.Replace("#Receiptdate#", String.Format("{0:dd MMM yyyy}", objPay.PaymentDate));
            mailContents = mailContents.Replace("#Client#", objInvoic.CompanyName);
            var strAmount = ConvertNumbertoWords(objInvoic.TotalAfterDiscount);
            mailContents = mailContents.Replace("#Amount#", strAmount);
            //string PkgName = await PackageName(inv.PackageId);
            // mailContents = mailContents.Replace("#Membership#", objInvoic.P PkgName);
            mailContents = mailContents.Replace("#InvNo#", objInvoic.Id.ToString());
            var strPaymentMode = objPay.PaymentMode;

            var txtMode = "";
            if (strPaymentMode == "CASH")
            {
                txtMode = "<b>CASH</b>";
            }
            else if (strPaymentMode == "CHEQUE")
            {
                txtMode = "Cheque No: <b>" + objPay.ChequeNumber + "</b> Bank Name: <b>" + objPay.BankName + "</b> CHQ Date: <b>" + objPay.ChequeDate + "</b>";
            }
            else
            {
                txtMode = "<b>NEFT Transfer</b>, Transaction Id:  <b>" + objPay.TransactionId + "</b>";
            }
            mailContents = mailContents.Replace("#Mode#", txtMode);
            mailContents = mailContents.Replace("#Amt#", String.Format("{0:#,000.00}", objPay.PaidAmount));
            mailContents = mailContents.Replace("#Salesman#", objInvoic.Salesman);
            mailContents = mailContents.Replace("#DealerName#", objInvoic.DealerName);
            mailContents = mailContents.Replace("#GSTNO#", objInvoic.GstIn);
            mailContents = mailContents.Replace("#DealerAddress1#", objInvoic.DealerAddr1);
            mailContents = mailContents.Replace("#DealerAddress2#", objInvoic.DealerAddr2);
            mailContents = mailContents.Replace("#DealerMobile#", objInvoic.DealerMobile);
            mailContents = mailContents.Replace("#DealerEmail#", objInvoic.DealerEmail);

            return mailContents;
        }

        private string PaymentLink(bool st, int id)
        {
            string strTxt = "";
            if (st == false)
            {
                strTxt = "<a href =\"dinvoice/payment/" + id + "\"><i class=\"fas fa-dollar-sign\"></i></a>";
            }
            return strTxt;
        }

        private void SendSMS(string MobileNo, string txtMsg)
        {
            try
            {
                string Url = "http://sms.technoland.biz/http-api.php?username=bminvoice&password=bminvoice@123&senderid=BKMSTR&route=1&number=" + MobileNo + "&message=" + txtMsg;

                var Client = new HttpClient();
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage apiResult = Client.GetAsync(Url).Result;

            }
            catch (Exception ex)
            {
                throw new Exception("Error:" + ex.Message);
            }
        }

        public string ConvertNumbertoWords(long number)
        {
            if (number == 0) return "ZERO";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            //if ((number / 1000000) > 0)
            //{
            //    words += ConvertNumbertoWords(number / 100000) + " LAKES ";
            //    number %= 1000000;
            //}
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "") words += "AND ";
                var unitsMap = new[]
                {
                "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"
            };
                var tensMap = new[]
                {
                "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
            };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        private void ConvertHtmltoPDF(string content, string fileName)
        {
            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.Letter;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.ShowHeader = false;
            pdfConverter.PdfDocumentOptions.ShowFooter = true;
            pdfConverter.PdfDocumentOptions.LeftMargin = 20;
            pdfConverter.PdfDocumentOptions.RightMargin = 20;
            pdfConverter.PdfDocumentOptions.TopMargin = 30;
            pdfConverter.PdfDocumentOptions.BottomMargin = 10;
            pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
            pdfConverter.PdfFooterOptions.ShowPageNumber = true;
            pdfConverter.PdfFooterOptions.PageNumberTextFontType = PdfFontType.Courier;
            pdfConverter.PdfFooterOptions.PageNumberingFormatString = "Page &p; of &P;";

            pdfConverter.LicenseKey = "UnlgcmpyYGJnYnJrfGJyYWN8Y2B8a2traw==";
            byte[] downloadBytes = pdfConverter.GetPdfBytesFromHtmlString(content);

            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Type", "binary/octet-stream");
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"; size=\"" + downloadBytes.Length.ToString() + "\"");
            response.Flush();
            response.BinaryWrite(downloadBytes);
            response.Flush();
            response.End();
        }

    }
}