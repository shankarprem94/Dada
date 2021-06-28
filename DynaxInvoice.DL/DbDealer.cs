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
    public class DbDealer : IDbDealer
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();
        
        public int AddDealer(DynaxDealer dealer)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_DEALER", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@DEALERNAME", SqlDbType.VarChar).Value = dealer.DealerName;
                        myCommand.Parameters.Add("@DEALERADDRESS1", SqlDbType.VarChar).Value = dealer.DealerAddress1;
                        myCommand.Parameters.Add("@DEALERADDRESS2", SqlDbType.VarChar).Value = dealer.DealerAddress2;
                        myCommand.Parameters.Add("@CITY", SqlDbType.VarChar).Value = dealer.City;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = dealer.StateId;
                        myCommand.Parameters.Add("@PINCODE", SqlDbType.VarChar).Value = dealer.Pincode;   
                        myCommand.Parameters.Add("@EMAILID", SqlDbType.VarChar).Value = dealer.EmailId;
                        myCommand.Parameters.Add("@MOBILENO", SqlDbType.VarChar).Value = dealer.MobileNo;
                        myCommand.Parameters.Add("@CONTACTPERSON", SqlDbType.VarChar).Value = dealer.ContactPerson;
                        myCommand.Parameters.Add("@ACCOUNTNO", SqlDbType.VarChar).Value = dealer.AccountNo;
                        myCommand.Parameters.Add("@ACCOUNTNAME", SqlDbType.VarChar).Value = dealer.AccountName;
                        myCommand.Parameters.Add("@BANKNAME", SqlDbType.VarChar).Value = dealer.BankName;
                        myCommand.Parameters.Add("@IFSCCODE", SqlDbType.VarChar).Value = dealer.IFSCCode;
                        myCommand.Parameters.Add("@GSTNO", SqlDbType.VarChar).Value = dealer.GSTNo;
                        myCommand.Parameters.Add("@JOINDATE", SqlDbType.DateTime).Value = dealer.JoinDate;
                        myCommand.Parameters.Add("@ENDDATE", SqlDbType.DateTime).Value = dealer.EndDate;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.Bit).Value = dealer.Status;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        id = (int)myCommand.Parameters["@ID"].Value;
                        conn.Close();
                    }
                    if (id > 0)
                    {
                        string[] strZone = dealer.ZoneId.Split(',');
                        for (int i = 0; i < strZone.Length; i++)
                        {
                            using (SqlCommand myCommand1 = new SqlCommand("DI_ADD_DEALERZONE", conn))
                            {
                                myCommand1.CommandType = CommandType.StoredProcedure;
                                myCommand1.Parameters.Add("@DEALERID", SqlDbType.Int).Value = id;
                                myCommand1.Parameters.Add("@ZONEID", SqlDbType.Int).Value = int.Parse(strZone[i]);
                                conn.Open();
                                myCommand1.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                    }

                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:AddDealer() -" + ex.ToString());
            }
        }

        public DynaxDealer GetDealerDetails(int id)
        {
            try
            {
                var objDealer = new DynaxDealer();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_DEALER_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            objDealer.Id = (int)dataReader["ID"];
                            objDealer.DealerName = (string)dataReader["DealerName"];
                            objDealer.DealerAddress1 = (string)dataReader["DealerAddress1"];
                            objDealer.DealerAddress2 = ((dataReader["DealerAddress2"] == DBNull.Value) ? "" : (string)dataReader["DealerAddress2"]);
                            objDealer.City = (string)dataReader["CITY"];
                            objDealer.ZoneId= (string)dataReader["ZoneId"];
                            objDealer.StateId = (int)dataReader["STATEID"];
                            objDealer.StateName = (string)dataReader["STATENAME"];
                            objDealer.Pincode = (string)dataReader["PINCODE"];
                            objDealer.EmailId = (string)dataReader["EMAILID"];
                            objDealer.MobileNo = (string)dataReader["MOBILENO"];
                            objDealer.ContactPerson = (string)dataReader["CONTACTPERSON"];
                            objDealer.AccountNo = (string)dataReader["ACCOUNTNO"];
                            objDealer.AccountName = (string)dataReader["ACCOUNTNAME"];
                            objDealer.BankName = (string)dataReader["BANKNAME"];
                            objDealer.IFSCCode = (string)dataReader["IFSCCODE"];
                            objDealer.GSTNo= (string)dataReader["GSTNO"];
                            objDealer.JoinDate = (DateTime)dataReader["JOINDATE"];
                            objDealer.EndDate = (DateTime)((dataReader["ENDDATE"] == DBNull.Value) ? "" : dataReader["ENDDATE"]);
                            objDealer.Status = (bool)dataReader["STATUS"];
                        }
                    }
                }
                return objDealer;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetDealerDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxDealer> GetDealerList()
        {
            var DealerList = new List<DynaxDealer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_DEALER_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objDealer = new DynaxDealer
                                {
                                    Id = (int)dataReader["ID"],
                                    DealerName = (string)dataReader["DealerName"],
                                    DealerAddress1 = (string)dataReader["DealerAddress1"],
                                    DealerAddress2 = ((dataReader["DealerAddress2"] == DBNull.Value) ? "" : (string)dataReader["DealerAddress2"]),
                                    City = (string)dataReader["CITY"],
                                    StateId = (int)dataReader["STATEID"],
                                    Pincode = (string)dataReader["PINCODE"],
                                    EmailId = (string)dataReader["EMAILID"],
                                    MobileNo = (string)dataReader["MOBILENO"],
                                    ContactPerson = (string)dataReader["CONTACTPERSON"],
                                    AccountNo = (string)dataReader["ACCOUNTNO"],
                                    AccountName = (string)dataReader["ACCOUNTNAME"],
                                    BankName = (string)dataReader["BANKNAME"],
                                    IFSCCode = (string)dataReader["IFSCCODE"],
                                    GSTNo = (string)dataReader["GSTNO"],
                                    JoinDate = (DateTime)dataReader["JOINDATE"],
                                    EndDate = (DateTime)((dataReader["ENDDATE"] == DBNull.Value) ? "" : dataReader["ENDDATE"]),
                                    Status = (bool)dataReader["STATUS"]
                                };
                                DealerList.Add(objDealer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetDealerList() -" + ex.ToString());
            }
            return DealerList;
        }

        public bool UpdateDealer(DynaxDealer dealer)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_UPDATE_DEALER", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = dealer.Id;
                        myCommand.Parameters.Add("@DealerName", SqlDbType.VarChar).Value = dealer.DealerName;
                        myCommand.Parameters.Add("@DealerAddress1", SqlDbType.VarChar).Value = dealer.DealerAddress1;
                        myCommand.Parameters.Add("@DealerAddress2", SqlDbType.VarChar).Value = dealer.DealerAddress2;
                        myCommand.Parameters.Add("@CITY", SqlDbType.VarChar).Value = dealer.City;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = dealer.StateId;
                        myCommand.Parameters.Add("@PINCODE", SqlDbType.VarChar).Value = dealer.Pincode;  
                        myCommand.Parameters.Add("@EMAILID", SqlDbType.VarChar).Value = dealer.EmailId;
                        myCommand.Parameters.Add("@MOBILENO", SqlDbType.VarChar).Value = dealer.MobileNo;
                        myCommand.Parameters.Add("@CONTACTPERSON", SqlDbType.VarChar).Value = dealer.ContactPerson;
                        myCommand.Parameters.Add("@ACCOUNTNO", SqlDbType.VarChar).Value = dealer.AccountNo;
                        myCommand.Parameters.Add("@ACCOUNTNAME", SqlDbType.VarChar).Value = dealer.AccountName;
                        myCommand.Parameters.Add("@BANKNAME", SqlDbType.VarChar).Value = dealer.BankName;
                        myCommand.Parameters.Add("@IFSCCODE", SqlDbType.VarChar).Value = dealer.IFSCCode;
                        myCommand.Parameters.Add("@GSTNO", SqlDbType.VarChar).Value = dealer.GSTNo;
                        myCommand.Parameters.Add("@JOINDATE", SqlDbType.DateTime).Value = dealer.JoinDate;
                        myCommand.Parameters.Add("@ENDDATE", SqlDbType.DateTime).Value = dealer.EndDate;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.Bit).Value = dealer.Status;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:UpdateDealer() -" + ex.ToString());
            }
            return flag;
        }
    }
}
