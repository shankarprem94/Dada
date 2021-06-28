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
    public class DbHotel : IDbHotel
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();
        public int AddHotel(DynaxHotel hotel)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_HOTELS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@CUSTOMERID", SqlDbType.Int).Value = hotel.CustomerId;
                        myCommand.Parameters.Add("@HOTELNAME", SqlDbType.VarChar).Value = hotel.HotelName;
                        myCommand.Parameters.Add("@HOTELTYPE", SqlDbType.VarChar).Value = hotel.HotelType;
                        myCommand.Parameters.Add("@ADDRESS1", SqlDbType.VarChar).Value = hotel.Address1;
                        myCommand.Parameters.Add("@ADDRESS2", SqlDbType.VarChar).Value = ((hotel.Address2 == null) ? "" : hotel.Address2);
                        myCommand.Parameters.Add("@CITY", SqlDbType.VarChar).Value = hotel.City;
                        myCommand.Parameters.Add("@PINCODE", SqlDbType.VarChar).Value = hotel.Pincode;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = hotel.StateId;
                        myCommand.Parameters.Add("@MOBILENO", SqlDbType.VarChar).Value = hotel.MobileNo;
                        myCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = hotel.Email;
                        myCommand.Parameters.Add("@LANDMARK", SqlDbType.VarChar).Value = ((hotel.Landmark == null) ? "" : hotel.Landmark);
                        myCommand.Parameters.Add("@DISTFROMLANDMARK", SqlDbType.VarChar).Value = ((hotel.DistFromLandmark == null) ? "" : hotel.DistFromLandmark);
                        myCommand.Parameters.Add("@RATEPERNIGHT", SqlDbType.Int).Value = hotel.RatePerNight;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        id = (int)myCommand.Parameters["@ID"].Value;
                        conn.Close();
                    }
                    if (id > 0)
                    {
                        string[] strFacility = hotel.FacilityId.Split(',');
                        for (int i = 0; i < strFacility.Length; i++)
                        {
                            using (SqlCommand myCommand1 = new SqlCommand("DI_ADD_HOTELS_FACILITY", conn))
                            {
                                myCommand1.CommandType = CommandType.StoredProcedure;
                                myCommand1.Parameters.Add("@HOTELID", SqlDbType.Int).Value = id;
                                myCommand1.Parameters.Add("@FACILITYID", SqlDbType.Int).Value = int.Parse(strFacility[i]);
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
                throw new Exception("DynaxInvoice.DL:AddHotel() -" + ex.ToString());
            }
        }

        public DynaxHotel GetHotelDetails(int id)
        {
            try
            {
                var objHotel = new DynaxHotel();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_HOTELS_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            objHotel.Id = (int)dataReader["ID"];
                            objHotel.CustomerId = (int)dataReader["CUSTOMERID"];
                            objHotel.HotelName = (string)dataReader["HOTELNAME"];
                            objHotel.Address1 = (string)dataReader["ADDRESS1"];
                            objHotel.Address2 = ((dataReader["ADDRESS2"] == DBNull.Value) ? "" : (string)dataReader["ADDRESS2"]);
                            objHotel.City = (string)dataReader["CITY"];
                            objHotel.Pincode = (string)dataReader["PINCODE"];
                            objHotel.StateId = (int)dataReader["STATEID"];
                            objHotel.MobileNo = (string)dataReader["MOBILENO"];
                            objHotel.Email = (string)dataReader["EMAIL"];
                            objHotel.FacilityId = (string)dataReader["FacilityId"];
                            objHotel.HotelType = (string)dataReader["HotelType"];
                            objHotel.RatePerNight = (int)dataReader["RatePerNight"];
                            objHotel.Landmark = (string)dataReader["Landmark"];
                            objHotel.DistFromLandmark = (string)dataReader["DistFromLandmark"];
                        }
                    }
                }
                return objHotel;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetHotelDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxHotel> GetHotelList(int id)
        {
            var HotelList = new List<DynaxHotel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_HOTELS_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objHotel = new DynaxHotel
                                {
                                    Id = (int)dataReader["ID"],
                                    CustomerId = (int)dataReader["CUSTOMERID"],
                                    HotelName = (string)dataReader["HOTELNAME"],
                                    HotelType = (string)dataReader["HOTELTYPE"],
                                    Address1 = (string)dataReader["ADDRESS1"],
                                    Address2 = ((dataReader["ADDRESS2"] == DBNull.Value) ? "" : (string)dataReader["ADDRESS2"]),
                                    City = (string)dataReader["CITY"],
                                    Pincode = (string)dataReader["PINCODE"],
                                    StateId = (int)dataReader["STATEID"],
                                    MobileNo = (string)dataReader["MOBILENO"],
                                    Email = (string)dataReader["EMAIL"],
                                    Landmark = (string)dataReader["LANDMARK"],
                                    DistFromLandmark = (string)dataReader["DISTFROMLANDMARK"],
                                    RatePerNight = (int)dataReader["RATEPERNIGHT"],
                                    CompanyName = (string)dataReader["COMPANYNAME"],
                                    StateName = (string)dataReader["STATENAME"],
                                    FacilityId = (string)dataReader["FACILITYID"]
                                };
                                HotelList.Add(objHotel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetHotelList() -" + ex.ToString());
            }
            return HotelList;
        }

        public bool UpdateHotel(DynaxHotel hotel)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_UPDATE_HOTELS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = hotel.Id;
                        myCommand.Parameters.Add("@CUSTOMERID", SqlDbType.Int).Value = hotel.CustomerId;
                        myCommand.Parameters.Add("@HOTELNAME", SqlDbType.VarChar).Value = hotel.HotelName;
                        myCommand.Parameters.Add("@HOTELTYPE", SqlDbType.VarChar).Value = hotel.HotelType;
                        myCommand.Parameters.Add("@ADDRESS1", SqlDbType.VarChar).Value = hotel.Address1;
                        myCommand.Parameters.Add("@ADDRESS2", SqlDbType.VarChar).Value = ((hotel.Address2 == null) ? "" : hotel.Address2); 
                        myCommand.Parameters.Add("@CITY", SqlDbType.VarChar).Value = hotel.City;
                        myCommand.Parameters.Add("@PINCODE", SqlDbType.VarChar).Value = hotel.Pincode;
                        myCommand.Parameters.Add("@STATEID", SqlDbType.Int).Value = hotel.StateId;
                        myCommand.Parameters.Add("@MOBILENO", SqlDbType.VarChar).Value = hotel.MobileNo;
                        myCommand.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = hotel.Email;
                        myCommand.Parameters.Add("@LANDMARK", SqlDbType.VarChar).Value = ((hotel.Landmark == null) ? "" : hotel.Landmark);
                        myCommand.Parameters.Add("@DISTFROMLANDMARK", SqlDbType.VarChar).Value = ((hotel.DistFromLandmark == null) ? "" : hotel.DistFromLandmark);
                        myCommand.Parameters.Add("@RATEPERNIGHT", SqlDbType.Int).Value = hotel.RatePerNight;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                    }

                    using (SqlCommand myCommand1 = new SqlCommand("DI_UPDATE_HOTELS_FACILITY", conn))
                    {
                        myCommand1.CommandType = CommandType.StoredProcedure;
                        myCommand1.Parameters.Add("@HOTELID", SqlDbType.Int).Value = hotel.Id;
                        conn.Open();
                        myCommand1.ExecuteNonQuery();
                        conn.Close();
                    }

                    string[] strFacility = hotel.FacilityId.Split(',');
                    for (int i = 0; i < strFacility.Length; i++)
                    {
                        using (SqlCommand myCommand2 = new SqlCommand("DI_ADD_HOTELS_FACILITY", conn))
                        {
                            myCommand2.CommandType = CommandType.StoredProcedure;
                            myCommand2.Parameters.Add("@HOTELID", SqlDbType.Int).Value = hotel.Id;
                            myCommand2.Parameters.Add("@FACILITYID", SqlDbType.Int).Value = int.Parse(strFacility[i]);
                            conn.Open();
                            myCommand2.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:UpdateHotel() -" + ex.ToString());
            }
            return flag;
        }

        public IEnumerable<DynaxHotel> CustomersHotelList(int id)
        {
            var HotelList = new List<DynaxHotel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_CUSTOMER_HOTELS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objHotel = new DynaxHotel
                                {
                                    Id = (int)dataReader["ID"],
                                    HotelName = (string)dataReader["HOTELNAME"]
                                };
                                HotelList.Add(objHotel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:CustomersHotelList() -" + ex.ToString());
            }
            return HotelList;
        }
    }
}
