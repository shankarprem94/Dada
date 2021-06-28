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
    public class DbCustomerPackage : IDbCustomerPackage
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DynaxConnection"].ToString();

        public int AddCustomerPackage(CustomerPackage custPackage)
        {
            try
            {
                int id = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_ADD_CUSTOMERPACKAGE", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@INVOICEID", SqlDbType.Int).Value = custPackage.InvoiceId;
                        myCommand.Parameters.Add("@PACKAGEAMOUNT", SqlDbType.Int).Value = custPackage.PackageAmount;
                        myCommand.Parameters.Add("@PACKAGEID", SqlDbType.Int).Value = custPackage.PackageId;
                        myCommand.Parameters.Add("@PACKAGEDISCOUNT", SqlDbType.Int).Value = custPackage.PackageDiscount;
                        myCommand.Parameters.Add("@AMOUNTAFTERDISCOUNT", SqlDbType.Int).Value = custPackage.AmountAfterDiscount;
                        conn.OpenAsync();
                        myCommand.ExecuteNonQueryAsync();
                        id = (int)myCommand.Parameters["@ID"].Value;
                        conn.Close();
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:AddCustomerPackage() -" + ex.ToString());
            }
        }

        public IEnumerable<PkgViewModel> GetCustomerPackageDetails(int id)
        {
            try
            {
                var ObjPkgList = new List<PkgViewModel>();
                
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_CUSTOMERPACKAGE_DETAILS", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                        conn.Open();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objCustPackage = new PkgViewModel();
                                objCustPackage.PkgId = (int)dataReader["PACKAGEID"];
                                objCustPackage.PkgName = (string)dataReader["PACKAGENAME"];
                                objCustPackage.PkgDescription = (string)dataReader["PACKAGEDESCRIPTION"];
                                objCustPackage.PkgAmount = (int)dataReader["PACKAGEAMOUNT"];
                                objCustPackage.Quantity= (int)dataReader["QUANTITY"];
                                objCustPackage.PkgDiscount = (int)dataReader["PACKAGEDISCOUNT"];
                                objCustPackage.PkgAmountAfterDiscount = (int)dataReader["AMOUNTAFTERDISCOUNT"];
                                ObjPkgList.Add(objCustPackage);
                            }
                        }
                    }
                }
                return ObjPkgList;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetCustomerPackageDetails() -" + ex.ToString());
            }
        }

        public IEnumerable<CustomerPackage> GetCustomerPackageList()
        {
            try
            {
                var objCustPackageList = new List<CustomerPackage>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_CUSTOMERPACKAGE_DETAILS_LIST", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        conn.OpenAsync();
                        using (SqlDataReader dataReader = myCommand.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var objCP = new CustomerPackage
                                {
                                    Id = (int)dataReader["ID"],
                                    InvoiceId = (int)dataReader["INVOICEID"],
                                    PackageAmount = (int)dataReader["PACKAGEAMOUNT"],
                                    PackageId = (int)dataReader["PACKAGEID"],
                                    PackageDiscount = (int)dataReader["PACKAGEDISCOUNT"],
                                    AmountAfterDiscount = (int)dataReader["AMOUNTAFTERDISCOUNT"]
                                };
                                objCustPackageList.Add(objCP);
                            }
                        }
                    }
                }
                return objCustPackageList;
            }
            catch (Exception ex)
            {
                throw new Exception("DynaxInvoice.DL:GetCustomerPackageList() -" + ex.ToString());
            }
        }

        public bool UpdateCustomerPackage(CustomerPackage custPackage)
        {
            bool flag;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand myCommand = new SqlCommand("DI_UPDATE_CUSTOMERPACKAGE", conn))
                    {
                        myCommand.CommandType = CommandType.StoredProcedure;
                        myCommand.Parameters.Add("@ID", SqlDbType.Int).Value = custPackage.Id;
                        myCommand.Parameters.Add("@INVOICEID", SqlDbType.Int).Value = custPackage.InvoiceId;
                        myCommand.Parameters.Add("@PACKAGEAMOUNT", SqlDbType.Int).Value = custPackage.PackageAmount;
                        myCommand.Parameters.Add("@PACKAGEID", SqlDbType.Int).Value = custPackage.PackageId;
                        myCommand.Parameters.Add("@PACKAGEDISCOUNT", SqlDbType.Int).Value = custPackage.PackageDiscount;
                        myCommand.Parameters.Add("@AMOUNTAFTERDISCOUNT", SqlDbType.Int).Value = custPackage.AmountAfterDiscount;
                        conn.OpenAsync();
                        myCommand.ExecuteNonQueryAsync();
                        conn.Close();
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {               
                throw new Exception("DynaxInvoice.DL:UpdateCustomerPackage() -" + ex.ToString());
            }
            return flag;
        }
    }
}
