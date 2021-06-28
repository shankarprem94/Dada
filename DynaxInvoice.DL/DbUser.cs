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
    public class DbUser : IDbUser
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();

        public int AddUser(DynaxUser user)
        {           
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_USER", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                        myCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = user.Password;
                        myCommand.Parameters.Add("@FULLNAME", SqlDbType.VarChar).Value = user.FullName;
                        myCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.Email;
                        myCommand.Parameters.Add("@MOBILE", SqlDbType.VarChar).Value = user.Mobile;
                        myCommand.Parameters.Add("@DEALERID", SqlDbType.VarChar).Value = user.DealerId;
                        myCommand.Parameters.Add("@USERTYPE", SqlDbType.VarChar).Value = user.UserType;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = user.Status;
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
                throw new Exception("DbUser:AddUser() -" + ex.ToString());
            }           
        }

        public DynaxUser GetUserDetails(int id)
        {
            try
            {
                var objUser = new DynaxUser();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_GET_USERDETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.VarChar).Value = id;                     
                        conn.OpenAsync();
                        SqlDataReader dr= myCommand.ExecuteReader();
                        if(dr.HasRows)
                        {
                            dr.ReadAsync();
                            objUser.Id = (int)dr["ID"];
                            objUser.UserName = (string)dr["USERNAME"];
                            objUser.FullName = (string)dr["FULLNAME"];
                            objUser.Email = (string)dr["EMAIL"];
                            objUser.Mobile = (string)dr["MOBILE"];
                            objUser.DealerId = (int)dr["DEALERID"];
                            objUser.UserType = (int)dr["USERTYPE"];
                            objUser.Status = (bool)dr["STATUS"];
                        }
                        if (!dr.IsClosed)
                            dr.Close();
                        conn.Close();
                    }
                }
                return objUser;
            }
            catch (Exception ex)
            {
                throw new Exception("DbUser:GetUserDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxUser> GetUserList()
        {           
            try
            {
                var objDynaxUser = new List<DynaxUser>();              
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_GET_ALLUSER", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;                      
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objuSer = new DynaxUser
                                {
                                    Id = (int)dataReader["ID"],
                                    UserName = (string)dataReader["USERNAME"],
                                    FullName = (string)dataReader["FULLNAME"],
                                    Email = (string)dataReader["EMAIL"],
                                    Mobile = (string)dataReader["MOBILE"],
                                    DealerId = (int)dataReader["DEALERID"],
                                    UserType= (int)dataReader["USERTYPE"],
                                    Status = (bool)dataReader["STATUS"]
                                };
                                objDynaxUser.Add(objuSer);
                            }
                        }
                    }
                }
                return objDynaxUser;
            }
            catch(Exception ex)
            {
                throw new Exception("DbUser:GetUserList() -" + ex.ToString());
            }            
        }

        public bool UpdateUser(DynaxUser user)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_UPDATE_USER", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = user.Id;
                        myCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = user.UserName;
                        myCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = user.Password;
                        myCommand.Parameters.Add("@FULLNAME", SqlDbType.VarChar).Value = user.FullName;
                        myCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = user.Email;
                        myCommand.Parameters.Add("@MOBILE", SqlDbType.VarChar).Value = user.Mobile;
                        myCommand.Parameters.Add("@DEALERID", SqlDbType.VarChar).Value = user.DealerId;
                        myCommand.Parameters.Add("@USERTYPE", SqlDbType.VarChar).Value = user.UserType;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = user.Status;

                        conn.Open();
                        myCommand.ExecuteNonQuery();                     
                        conn.Close();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {                
                throw new Exception("DbUser:UpdateUser() -" + ex.ToString());
            }
            return flag;
        }
        public DynaxUser Login(LoginViewModel objLogin)
        {
            try
            {
                var objUser = new DynaxUser();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_USER_LOGIN", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = objLogin.UserName;
                        myCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = objLogin.Password;
                        conn.Open();
                        SqlDataReader dr = myCommand.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objUser.Id = (int)dr["ID"];
                            objUser.UserName = (string)dr["USERNAME"];
                            objUser.FullName = (string)dr["FULLNAME"];
                            objUser.Email = (string)dr["EMAIL"];
                            objUser.Mobile = (string)dr["MOBILE"];
                            objUser.DealerId = (int)dr["DEALERID"];
                            objUser.UserType = (int)dr["USERTYPE"];
                            objUser.Status = (bool)dr["STATUS"];
                        }
                        if (!dr.IsClosed)
                            dr.Close();
                        conn.Close();
                    }
                }
                return objUser;
            }
            catch(Exception ex)
            {
                throw new Exception("DbUser:Login() -" + ex.ToString());
            }
        }

        public bool ChangePassword(int id,string pass)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_CHANGEPASSWORD", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        myCommand.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value =pass;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DbUser:ChangePassword() -" + ex.ToString());
            }
            return flag;
        }
    }
}
