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
    public class DbFacility : IDbFacility
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();
        public int AddFacility(DynaxFacility facility)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_FACILITIES", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@FACILITY", SqlDbType.VarChar).Value = facility.Facility;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.Bit).Value = facility.Status;
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
                throw new Exception("DynaxInvoice.DL:AddFacility() -" + ex.ToString());
            }
        }

        public DynaxFacility GetFacilityDetails(int id)
        {
            try
            {
                var objFacility = new DynaxFacility();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_FACILITIES_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.OpenAsync();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            objFacility.Id = (int)dataReader["ID"];
                            objFacility.Facility = (string)dataReader["FACILITY"];
                            objFacility.Status = (bool)dataReader["STATUS"];
                        }
                    }
                }
                return objFacility;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetFacilityDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxFacility> GetFacilityList()
        {
            var FacilityList = new List<DynaxFacility>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_FACILITIES_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        conn.OpenAsync();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objFacility = new DynaxFacility
                                {
                                    Id = (int)dataReader["ID"],
                                    Facility = (string)dataReader["FACILITY"],
                                    Status = (bool)dataReader["STATUS"]
                                };
                                FacilityList.Add(objFacility);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetFacilityList() -" + ex.ToString());
            }
            return FacilityList;
        }

        public bool UpdateFacility(DynaxFacility facility)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_UPDATE_FACILITIES", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = facility.Id;
                        myCommand.Parameters.Add("@FACILITY", SqlDbType.VarChar).Value = facility.Facility;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.Bit).Value = facility.Status;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {               
                throw new Exception("DynaxInvoice.DL:UpdateFacility() -" + ex.ToString());
            }
            return flag;
        }
    }
}
