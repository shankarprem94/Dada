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
    public class DbCustomer : IDbCustomer
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();
        public int AddCustomer(DynaxCustomer cust)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_CUSTOMER", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@COMPANYNAME", SqlDbType.VarChar).Value = cust.CompanyName;
                        myCommand.Parameters.Add("@ADDRESS1", SqlDbType.VarChar).Value = cust.Address1;
                        myCommand.Parameters.Add("@ADDRESS2", SqlDbType.VarChar).Value = cust.Address2;
                        myCommand.Parameters.Add("@CITY", SqlDbType.VarChar).Value = cust.City;
                        myCommand.Parameters.Add("@PINCODE", SqlDbType.VarChar).Value = cust.Pincode;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = cust.StateId;
                        myCommand.Parameters.Add("@CONTACTPERSON", SqlDbType.VarChar).Value = cust.ContactPerson;
                        myCommand.Parameters.Add("@DESIGNATION", SqlDbType.VarChar).Value = cust.Designation;
                        myCommand.Parameters.Add("@MOBILENO", SqlDbType.VarChar).Value = cust.MobileNo;
                        myCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = cust.Email;
                        myCommand.Parameters.Add("@GSTN", SqlDbType.VarChar).Value = cust.GSTN;
                        myCommand.Parameters.Add("@USERID", SqlDbType.VarChar).Value = cust.UserId;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        id = (int)myCommand.Parameters["@ID"].Value;
                        conn.Close();

                    }
                }
                return id;
            }
            catch(Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:AddCustomer() -" + ex.ToString());
            }
        }

        public DynaxCustomer GetCustomerDetails(int id)
        {
            try
            {
                var objCustomer = new DynaxCustomer();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_CUSTOMER_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            objCustomer.Id = (int)dataReader["ID"];
                            objCustomer.CompanyName = (string)dataReader["COMPANYNAME"];
                            objCustomer.Address1 = (string)dataReader["ADDRESS1"];
                            objCustomer.Address2 = ((dataReader["ADDRESS2"] == DBNull.Value) ? "" : (string)dataReader["ADDRESS2"]);
                            objCustomer.City = (string)dataReader["CITY"];
                            objCustomer.Pincode = (string)dataReader["PINCODE"];
                            objCustomer.StateId = (int)dataReader["STATEID"];
                            objCustomer.ContactPerson = (string)dataReader["CONTACTPERSON"];
                            objCustomer.Designation = (string)dataReader["Designation"];
                            objCustomer.MobileNo = (string)dataReader["MOBILENO"];
                            objCustomer.Email = (string)dataReader["EMAIL"];
                            objCustomer.GSTN = (string)dataReader["GSTN"];
                            objCustomer.DealerId=(int)dataReader["DealerId"];
                            objCustomer.UserId = (int)dataReader["UserId"];
                        }
                    }
                }
                return objCustomer;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetCustomerDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxCustomer> CustomerList(int id)
        {
            var customerList = new List<DynaxCustomer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_CUSTOMER_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objCust = new DynaxCustomer
                                {
                                    Id = (int)dataReader["ID"],
                                    CompanyName = (string)dataReader["COMPANYNAME"],
                                    Address1 = (string)dataReader["ADDRESS1"],
                                    Address2 = ((dataReader["ADDRESS2"] == DBNull.Value) ? "" : (string)dataReader["ADDRESS2"]),
                                    City = (string)dataReader["CITY"],
                                    Pincode = (string)dataReader["PINCODE"],
                                    StateId = (int)dataReader["STATEID"],
                                    ContactPerson = (string)dataReader["CONTACTPERSON"],
                                    Designation = (string)dataReader["Designation"],
                                    MobileNo = (string)dataReader["MOBILENO"],
                                    Email = (string)dataReader["EMAIL"],
                                    GSTN = (string)dataReader["GSTN"],
                                    UserId=(int)dataReader["UserId"],
                                    UserName =(string)dataReader["FullName"],
                                    DealerId=(int)dataReader["DealerId"],
                                    DealerName =(string)dataReader["DealerName"],
                                    StateName = (string)dataReader["STATENAME"]
                                };
                                customerList.Add(objCust);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetCustomerList() -" + ex.ToString());
            }
            return customerList;
        }

        public bool UpdateCustomer(DynaxCustomer cust)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_UPDATE_CUSTOMER", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = cust.Id;
                        myCommand.Parameters.Add("@COMPANYNAME", SqlDbType.VarChar).Value = cust.CompanyName;
                        myCommand.Parameters.Add("@ADDRESS1", SqlDbType.VarChar).Value = cust.Address1;
                        myCommand.Parameters.Add("@ADDRESS2", SqlDbType.VarChar).Value = cust.Address2;
                        myCommand.Parameters.Add("@CITY", SqlDbType.VarChar).Value = cust.City;
                        myCommand.Parameters.Add("@PINCODE", SqlDbType.VarChar).Value = cust.Pincode;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = cust.StateId;
                        myCommand.Parameters.Add("@CONTACTPERSON", SqlDbType.VarChar).Value = cust.ContactPerson;
                        myCommand.Parameters.Add("@DESIGNATION", SqlDbType.VarChar).Value = cust.Designation;
                        myCommand.Parameters.Add("@MOBILENO", SqlDbType.VarChar).Value = cust.MobileNo;
                        myCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = cust.Email;
                        myCommand.Parameters.Add("@GSTN", SqlDbType.VarChar).Value = cust.GSTN;
                        myCommand.Parameters.Add("@USERID", SqlDbType.Int).Value = cust.UserId;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {               
                throw new Exception("DynaxInvoice.DL:UpdateCustomer() -" + ex.ToString());
            }
            return flag;
        }

        public IEnumerable<UserVm> GetdealerWiseUserLis(int id)
        {
            try
            {
                var userList = new List<UserVm>();              
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_DEALERWISE_USERLIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                          while(dataReader.Read())
                            {
                                var objUser = new UserVm();
                                objUser.UserId = (int)dataReader["ID"];
                                objUser.UserName = (string)dataReader["FULLNAME"];
                                userList.Add(objUser);
                            }   
                        }
                    }
                }
                return userList;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetdealerWiseUserLis() -" + ex.ToString());
            }
        }
   
    }
}
