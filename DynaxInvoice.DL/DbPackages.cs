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
    public class DbPackages : IDbPackages
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();
        public int AddPackage(DynaxPackage obj)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_PACKAGE", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@PACKAGENAME", SqlDbType.VarChar).Value = obj.PackageName;
                        myCommand.Parameters.Add("@PACKAGEDESCRIPTION", SqlDbType.VarChar).Value = obj.PackageDescription;
                        myCommand.Parameters.Add("@PACKAGEAMOUNT", SqlDbType.Int).Value = obj.PackageAmount;
                        myCommand.Parameters.Add("@MAXDISCOUNT", SqlDbType.Int).Value = obj.MaxDiscount;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.Bit).Value = obj.Status;
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
                throw new Exception("DynaxInvoice.DL:AddCustomer() -" + ex.ToString());
            }
        }

        public DynaxPackage GetPackageDetails(int id)
        {
            try
            {
                var objPackage = new DynaxPackage();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_PACKAGE_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            dataReader.Read();
                            objPackage.Id = (int)dataReader["ID"];
                            objPackage.PackageName = (string)dataReader["PACKAGENAME"];
                            objPackage.PackageDescription = ((dataReader["PACKAGEDESCRIPTION"] == DBNull.Value) ? "" : (string)dataReader["PACKAGEDESCRIPTION"]);
                            objPackage.MaxDiscount = (int)dataReader["MAXDISCOUNT"];
                            objPackage.PackageAmount = (int)dataReader["PACKAGEAMOUNT"];
                            objPackage.Status = (bool)dataReader["STATUS"];
                        }
                    }
                }
                return objPackage;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetPackageDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<DynaxPackage> GetPackageList()
        {
            try
            {
                var objPackageList = new List<DynaxPackage>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_PACKAGE_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objPackage = new DynaxPackage
                                {
                                    Id = (int)dataReader["ID"],
                                    PackageName = (string)dataReader["PACKAGENAME"],
                                    PackageDescription =((dataReader["PACKAGEDESCRIPTION"]== DBNull.Value)?"":(string)dataReader["PACKAGEDESCRIPTION"]),
                                    PackageAmount = (int)dataReader["PACKAGEAMOUNT"],
                                    MaxDiscount = (int)dataReader["MAXDISCOUNT"],
                                    Status = (bool)dataReader["STATUS"]
                                };
                                objPackageList.Add(objPackage);
                            }
                        }
                    }
                }
                return objPackageList;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetPackageList() -" + ex.ToString());
            }
        }

        public bool UpdatePackage(DynaxPackage obj)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_Update_Package", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = obj.Id;
                        myCommand.Parameters.Add("@PACKAGENAME", SqlDbType.VarChar).Value = obj.PackageName;
                        myCommand.Parameters.Add("@PACKAGEDESCRIPTION", SqlDbType.VarChar).Value = obj.PackageDescription;
                        myCommand.Parameters.Add("@PACKAGEAMOUNT", SqlDbType.Int).Value = obj.PackageAmount;
                        myCommand.Parameters.Add("@MAXDISCOUNT", SqlDbType.Int).Value = obj.MaxDiscount;
                        myCommand.Parameters.Add("@STATUS", SqlDbType.Bit).Value = obj.Status;
                        conn.Open();
                        myCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {               
                throw new Exception("DynaxInvoice.DL:UpdatePackage() -" + ex.ToString());
            }
            return flag;
        }
    }
}
