using DynaxInvoice.BO;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DynaxInvoice.DL
{
    public class DbState : IDbState
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();

        /// <summary>
        /// This function will add new st
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public int AddState(DynaxState st)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_STATE", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@STATENAME", SqlDbType.VarChar).Value = st.StateName;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Direction=ParameterDirection.Output;
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
                throw new Exception("DynaxInvoice.DL:AddState() -" + ex.ToString());
            }
        }

        public DynaxState GetStateDetails(int id)
        {
            try
            {
                var objState = new DynaxState();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_STATE_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.VarChar).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            objState.Id = (int)dataReader["ID"];
                            objState.StateName = (string)dataReader["STATENAME"];
                        }
                    }
                }
                return objState;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetStateDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxState> GetStateList()
        {
            try
            {
                var objStateList = new List<DynaxState>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_STATE_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objState = new DynaxState
                                {
                                    Id = (int)dataReader["ID"],
                                    StateName = (string)dataReader["STATENAME"]
                                };
                                objStateList.Add(objState);
                            }
                        }
                    }
                }
                return objStateList;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetStateList() -" + ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public bool UpdateState(DynaxState st)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_EDIT_STATE", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.VarChar).Value = st.Id;
                        myCommand.Parameters.Add("@STATENAME", SqlDbType.VarChar).Value = st.StateName;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {              
                throw new Exception("DynaxInvoice.DL:UpdateState() -" + ex.ToString());
            }
            return flag;
        }
    }
}
