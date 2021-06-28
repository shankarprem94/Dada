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
    public class DbPayment : IDbPayment
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();
       
        public int AddPayment(DynaxPayment payment)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_PAYMENT", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@INVOICEID", SqlDbType.Int).Value = payment.InvoiceId;
                        myCommand.Parameters.Add("@PAYMENTMODE", SqlDbType.VarChar).Value = payment.PaymentMode;
                        myCommand.Parameters.Add("@CHEQUENUMBER", SqlDbType.VarChar).Value = ((payment.ChequeNumber==null)?"": payment.ChequeNumber);
                        myCommand.Parameters.Add("@CHEQUEDATE", SqlDbType.DateTime).Value = payment.ChequeDate;
                        myCommand.Parameters.Add("@BANKNAME", SqlDbType.VarChar).Value = payment.BankName;
                        myCommand.Parameters.Add("@PAIDAMOUNT", SqlDbType.Int).Value = payment.PaidAmount;
                        myCommand.Parameters.Add("@PAYMENTDATE", SqlDbType.DateTime).Value = payment.PaymentDate;
                        myCommand.Parameters.Add("@NEFTDetails", SqlDbType.VarChar).Value = ((payment.TransactionId==null)?"": payment.TransactionId);
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
                throw new Exception("DynaxInvoice.DL:AddPayment() -" + ex.ToString());
            }
        }

        public DynaxPayment GetPaymentDetails(int id)
        {
            try
            {
                var objPayment = new DynaxPayment();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_PAYMENT_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            objPayment.Id = (int)dataReader["ID"];
                            objPayment.InvoiceId = (int)dataReader["INVOICEID"];
                            objPayment.PaymentMode = (string)dataReader["PAYMENTMODE"];
                            objPayment.ChequeNumber = (string)dataReader["CHEQUENUMBER"];
                            objPayment.ChequeDate = (DateTime)dataReader["CHEQUEDATE"];
                            objPayment.BankName = (string)dataReader["BANKNAME"];
                            objPayment.PaidAmount = (int)dataReader["PAIDAMOUNT"];
                            objPayment.PaymentDate = (DateTime)dataReader["PAYMENTDATE"];
                            objPayment.TransactionId = (string)dataReader["NEFTDETAILS"];
                        }
                    }
                }
                return objPayment;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetPaymenyDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxPayment> GetPaymentList(int id)
        {
            try
            {
                var objPayment = new List<DynaxPayment>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_PAYMENT_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objPmt = new DynaxPayment
                                {
                                    Id = (int)dataReader["ID"],
                                    InvoiceId = (int)dataReader["INVOICEID"],
                                    PaymentMode = (string)dataReader["PAYMENTMODE"],
                                    ChequeNumber = (string)dataReader["CHEQUENUMBER"],
                                    ChequeDate = (DateTime)dataReader["CHEQUEDATE"],
                                    BankName = (string)dataReader["BANKNAME"],
                                    PaidAmount = (int)dataReader["PAIDAMOUNT"],
                                    PaymentDate = (DateTime)dataReader["PAYMENTDATE"],
                                    TransactionId = (string)dataReader["NEFTDETAILS"]
                                };
                                objPayment.Add(objPmt);
                            }
                        }
                    }
                }
                return objPayment;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetPaymenyList() -" + ex.ToString());
            }
        }       
    }
}
