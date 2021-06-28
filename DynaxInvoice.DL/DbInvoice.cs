using DynaxInvoice.BO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaxInvoice.DL
{
    public class DbInvoice : IDbInvoice
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();

        public int AddInvoice(DynaxInvoices invoice)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_INVOICE", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@INVOICEDATE", SqlDbType.DateTime).Value = invoice.InvoiceDate;
                       // myCommand.Parameters.Add("@ZONEID", SqlDbType.Int).Value = invoice.ZoneId;
                        myCommand.Parameters.Add("@CUSTOMERID", SqlDbType.Int).Value = invoice.CustomerId;
                        //myCommand.Parameters.Add("@HOTELID", SqlDbType.Int).Value = 0;
                        myCommand.Parameters.Add("@TAXAMOUNT", SqlDbType.Int).Value = invoice.TaxAmount;
                        myCommand.Parameters.Add("@TOTALDISCOUNT", SqlDbType.Int).Value = invoice.TotalDiscount;
                        myCommand.Parameters.Add("@TOTALAMOUNT", SqlDbType.Int).Value = invoice.TotalAmount;
                        myCommand.Parameters.Add("@TotalAfterDiscount", SqlDbType.Int).Value = invoice.TotalAfterDiscount;
                        myCommand.Parameters.Add("@USERID", SqlDbType.Int).Value = invoice.UserId;                       
                        myCommand.Parameters.Add("@REMARK", SqlDbType.VarChar).Value = invoice.Remark;
                        myCommand.Parameters.Add("@ACTIVATIONDATE", SqlDbType.DateTime).Value = invoice.ActivationDate;
                        myCommand.Parameters.Add("@EXPIRYDATE", SqlDbType.DateTime).Value = invoice.ExpiryDate;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        id = (int)myCommand.Parameters["@ID"].Value;
                        conn.Close();
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:AddInvoice() -" + ex.ToString());
            }
        }

        public InvViewModel GetInvoiceDetails(int id)
        {
            try
            {
                var invoice = new InvViewModel();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_INVOICE_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            invoice.Id = (int)dataReader["ID"];
                            invoice.InvoiceDate = (DateTime)dataReader["INVOICEDATE"];
                            invoice.TaxAmount = (int)dataReader["TAXAMOUNT"];
                            invoice.TotalDiscount = (int)dataReader["TOTALDISCOUNT"];
                            invoice.TotalAmount = (int)dataReader["TOTALAMOUNT"];
                            invoice.TotalAfterDiscount = (int)dataReader["TotalAfterDiscount"];
                            invoice.DealerName = (string)dataReader["DealerName"];
                            invoice.DealerAddr1 = (string)dataReader["DealerAddress1"];
                            invoice.DealerAddr2 = (string)dataReader["DealerAddress2"];
                            invoice.DealerCity = (string)dataReader["DealerCity"];
                            invoice.DealerState = (string)dataReader["DealerState"];
                            invoice.DealerPincode = (string)dataReader["DealerPincode"];
                            invoice.DealerMobile = (string)dataReader["DealerMobile"];   
                            invoice.DealerEmail = (string)dataReader["DealerEmail"];
                            invoice.GstIn = (string)dataReader["GSTNO"];
                            invoice.CompanyName = (string)dataReader["CompanyName"];
                            invoice.CompanyAddress1 = (string)dataReader["CompanyAddress1"];
                            invoice.CompanyAddress2 = (string)dataReader["CompanyAddress2"];                           
                            invoice.CompanyCity = (string)dataReader["CompanyCity"];
                            invoice.CompanyState = (string)dataReader["DealerState"];
                            invoice.CompanyPincode = (string)dataReader["CompanyPincode"];
                            invoice.CustMobile = (string)dataReader["CustMobile"];
                            invoice.CustEmail = (string)dataReader["CustEmail"];
                            invoice.Salesman = (string)dataReader["Salesman"];
                            invoice.SalesmanMobile = (string)dataReader["SalesmanMobile"];
                            invoice.SalesmanEmail = (string)dataReader["SALESEMAIL"];
                            invoice.AccountNo = (string)dataReader["AccountNo"];
                            invoice.AccountName = (string)dataReader["AccountName"];
                            invoice.BankName = (string)dataReader["BankName"];
                            invoice.IFSCCode = (string)dataReader["IFSCCode"];
                            invoice.Hotel = (string)dataReader["HOTEL"];
                            invoice.DealerId = (int)dataReader["DealerId"];
                        }
                    }
                }
                return invoice;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetInvoiceDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxInvoices> GetInvoiceList(int userId)
        {
            try
            {
                var objInvoiceList = new List<DynaxInvoices>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_INVOICE_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = userId;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objInvoice = new DynaxInvoices
                                {
                                    Id = (int)dataReader["ID"],
                                    InvoiceDate = (DateTime)dataReader["INVOICEDATE"],
                                    //ZoneId = (int)dataReader["ZONEID"],
                                    CustomerId = (int)dataReader["CUSTOMERID"],                                   
                                    TaxAmount = (int)dataReader["TAXAMOUNT"],
                                    TotalDiscount = (int)dataReader["TOTALDISCOUNT"],
                                    TotalAmount = (int)dataReader["TOTALAMOUNT"],
                                    UserId = (int)dataReader["USERID"],                                     
                                    Remark = (string)dataReader["REMARK"],
                                    ActivationDate = (DateTime)dataReader["ACTIVATIONDATE"],
                                    ExpiryDate = (DateTime)dataReader["EXPIRYDATE"],
                                    //ZoneName=(string)dataReader["ZoneName"],
                                    CompanyName= (string)dataReader["CompanyName"],                                   
                                    DealerName= (string)dataReader["DealerName"],
                                    Status=(bool)dataReader["Status"]

                                };
                                objInvoiceList.Add(objInvoice);
                            }
                        }
                    }
                }
                return objInvoiceList;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetInvoiceList() -" + ex.ToString());
            }
        }

        public void AddInvoiceHotel(InvoiceHotel objHotel)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_INOICEHOTEL", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@INVOICEID", SqlDbType.Int).Value = objHotel.InvoiceId;
                        myCommand.Parameters.Add("@HOTELID", SqlDbType.Int).Value = objHotel.HotelId;                      
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:AddInvoiceHodel() -" + ex.ToString());
            }
        }

        public void AddCustomerPackage(CustomerPackage pkgCust)
        {
            try
            {                
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_CUSTOMERPACKAGE", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@INVOICEID", SqlDbType.Int).Value = pkgCust.InvoiceId;
                        myCommand.Parameters.Add("@PACKAGEID", SqlDbType.Int).Value = pkgCust.PackageId;
                        myCommand.Parameters.Add("@QUANTITY", SqlDbType.Int).Value = pkgCust.Quantity;
                        myCommand.Parameters.Add("@PACKAGEAMOUNT", SqlDbType.Int).Value = pkgCust.PackageAmount;
                        myCommand.Parameters.Add("@PACKAGEDISCOUNT", SqlDbType.Int).Value = pkgCust.PackageDiscount;
                        myCommand.Parameters.Add("@AMOUNTAFTERDISCOUNT", SqlDbType.Int).Value = pkgCust.AmountAfterDiscount;  
                        conn.Open();
                        myCommand.ExecuteNonQuery();                       
                        conn.Close();
                    }
                }               
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:AddCustomerPackage() -" + ex.ToString());
            }
        }
    }
}
