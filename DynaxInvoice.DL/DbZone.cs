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
    public class DbZone : IDbZone
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();
        public int AddZone(DynaxZone zone)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_ZONE", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = zone.StateId;
                        myCommand.Parameters.Add("@ZONENAME", SqlDbType.VarChar).Value = zone.ZoneName;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.Bit).Value = zone.Status;
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
                throw new Exception("DynaxInvoice.DL:AddZone() -" + ex.ToString());
            }
        }

        public DynaxZone GetZoneDetails(int id)
        {
            try
            {
                var objZone = new DynaxZone();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ZONE_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            objZone.Id = (int)dataReader["ID"];
                            objZone.StateId = (int)dataReader["STATEID"];
                            objZone.ZoneName = (string)dataReader["ZONENAME"];
                            objZone.Status = (bool)dataReader["STATUS"];
                        }
                    }
                }
                return objZone;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetZoneDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxZone> GetZoneList(int id)
        {
            try
            {
                var objZoneList = new List<DynaxZone>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ZONE_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objZone = new DynaxZone
                                {
                                    Id = (int)dataReader["ID"],
                                    StateId = (int)dataReader["STATEID"],
                                    StateName= (string)dataReader["STATENAME"],
                                    ZoneName = (string)dataReader["ZONENAME"],
                                    Status = (bool)dataReader["STATUS"]
                                };
                                objZoneList.Add(objZone);
                            }
                        }
                    }
                }
                return objZoneList;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetZoneList() -" + ex.ToString());
            }
        }

        public bool UpdateZone(DynaxZone zone)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_Update_Zone", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = zone.Id;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = zone.StateId;
                        myCommand.Parameters.Add("@ZONENAME", SqlDbType.VarChar).Value = zone.ZoneName;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.Bit).Value = zone.Status;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {              
                throw new Exception("DynaxInvoice.DL:UpdateZone() -" + ex.ToString());
            }
            return flag;
        }

        public IEnumerable<DynaxZone> GetDealersZoneList(int id)
        {
            try
            {
                var objZoneList = new List<DynaxZone>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_DEALER_ZONE_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objZone = new DynaxZone
                                {
                                    Id = (int)dataReader["ID"],                                   
                                    ZoneName = (string)dataReader["ZONENAME"]                                   
                                };
                                objZoneList.Add(objZone);
                            }
                        }
                    }
                }
                return objZoneList;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetDealersZoneList() -" + ex.ToString());
            }
        }
    }
}
