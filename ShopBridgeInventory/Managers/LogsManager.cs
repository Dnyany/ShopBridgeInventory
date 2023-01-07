using System;
using System.Data;
using System.Data.SqlClient;

namespace ShopBridgeInventory.Managers
{
    public class LogsManager
    {
        public static void WriteLog(string ActionName, string JsonData,string _connectionString)
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_Insert_Log_InventoryAPI", con))
                {
                    try
                    {
                        if (con.State != 0)
                            con.Close();
                        cmd.CommandType = CommandType.StoredProcedure; 
                        cmd.Parameters.Add(new SqlParameter("@ActionName", ActionName));
                        cmd.Parameters.Add(new SqlParameter("@RequestData", JsonData)); 
                        con.Open();
                        //Execute the command  
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine("Writing log Error :" + ex.Message);
                    }
                    finally
                    {
                        if (con.State != 0)
                            con.Close();
                    }
                }
            }

        }
        public static void ErrorWriteLog(string ActionName, string Errormessage, string JsonData,string _connectionString)
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Prc_Insert_Error_InventoryAPI", con))
                {
                    try
                    {
                        if (con.State != 0)
                            con.Close();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Message", Errormessage));
                        cmd.Parameters.Add(new SqlParameter("@ActionName", ActionName));
                        cmd.Parameters.Add(new SqlParameter("@RequestData", JsonData)); 
                        con.Open();
                        //Execute the command  
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine("Writing log Error :" + ex.Message);
                    }
                    finally
                    {
                        if (con.State != 0)
                            con.Close();
                    }
                }
            }

        }

    }
}
