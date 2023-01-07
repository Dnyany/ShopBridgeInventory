using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopBridgeInventory.Models;
using ShopBridgeInventory.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace ShopBridgeInventory.Managers
{
    public class InventoryManager : IRepository
    {
        private readonly string _connectionString;
        public InventoryManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("APIConnection");
        }

        public async Task<List<InventoryData>> GetAllDetails()
        {
            List<InventoryData> _InventoryData = new List<InventoryData>();
            try
            {
                await Task.Delay(300);
                _InventoryData = ConvertDataTable<InventoryData>(getInventoryDetails());
            }
            catch (Exception ex)
            {

                LogsManager.ErrorWriteLog("GetAllDetails", "Exception : " + ex.Message.ToString(), "", _connectionString);

            }

            return _InventoryData;

        }
        public async Task<List<InventoryData>> GetDetailsByID(int ID)
        {

            List<InventoryData> _InventoryData = new List<InventoryData>();
            try
            {
                await Task.Delay(300);
                _InventoryData = ConvertDataTable<InventoryData>(getDetailsByID(ID));
            }
            catch (Exception ex)
            {

                LogsManager.ErrorWriteLog("GetDetailsByID", "Exception : " + ex.Message.ToString(), "", _connectionString);

            }
            return _InventoryData;

        }
        public async Task<ResultData> AddInventory(InventoryData param)
        {
            ResultData objResult = new ResultData();
            objResult.Status = false;
            try
            {
                await Task.Delay(300);
                int result = addInventory(param);
                if (result > 0)
                {
                    objResult.Status = true;
                    objResult.Result = "Record has been added successfully.";
                }
            }
            catch (Exception ex)
            {
                objResult.ErrorMessage = ex.Message;
                objResult.Result = "Error Occured.";
                LogsManager.ErrorWriteLog("AddInventory", "Exception : " + ex.Message.ToString(), "", _connectionString);

            }

            return objResult;
        }
        public async Task<ResultData> UpdateInventory(uInventoryData param)
        {
            ResultData objResult = new ResultData();
            objResult.Status = false;
            try
            {
                await Task.Delay(300);
                int result = updateInventory(param);
                if (result > 0)
                {
                    objResult.Status = true;
                    objResult.Result = "Record has been updated successfully.";
                }
            }
            catch (Exception ex)
            {
                objResult.ErrorMessage = ex.Message;
                objResult.Result = "Error Occured.";
                LogsManager.ErrorWriteLog("UpdateInventory", "Exception : " + ex.Message.ToString(), "", _connectionString);
            }

            return objResult;
        }
        public async Task<ResultData> DeleteInventory(dInventoryData param)
        {
            ResultData objResult = new ResultData();
            objResult.Status = false;
            try
            {
                await Task.Delay(300);
                int result = deleteInventory(param);
                if (result > 0)
                {
                    objResult.Status = true;
                    objResult.Result = "Record has been deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                objResult.ErrorMessage = ex.Message;
                objResult.Result = "Error Occured.";
                LogsManager.ErrorWriteLog("DeleteInventory", "Exception : " + ex.Message.ToString(), "", _connectionString);

            }

            return objResult;
        }

        private DataTable getInventoryDetails()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_Inventory_Crud_Operation", con))
                {
                    if (con.State != 0) con.Close();
                    con.Open();
                    try
                    {
                        // Start transaction. 
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.NVarChar).Value = "List";
                        //Execute the command  
                        sda.SelectCommand = cmd; // Select Command From Command to SqlDataAdaptor
                        sda.Fill(dt); // Execute Query and Get Result into DataSet 
                    }
                    catch (Exception ex)
                    {
                        LogsManager.ErrorWriteLog("List", "Exception : " + ex.Message.ToString(), "", _connectionString);
                    }
                    finally
                    {
                        if (con.State != 0) con.Close();
                    }

                    return dt;

                }
            }
        }
        private DataTable getDetailsByID(int ID)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_Inventory_Crud_Operation", con))
                {
                    if (con.State != 0) con.Close();
                    con.Open();
                    try
                    {
                        // Start transaction. 
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.NVarChar).Value = "List";
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;
                        //Execute the command  
                        sda.SelectCommand = cmd; // Select Command From Command to SqlDataAdaptor
                        sda.Fill(dt); // Execute Query and Get Result into DataSet 
                    }
                    catch (Exception ex)
                    {
                        LogsManager.ErrorWriteLog("List", "Exception : " + ex.Message.ToString(), "", _connectionString);
                    }
                    finally
                    {
                        if (con.State != 0) con.Close();
                    }

                    return dt;

                }
            }
        }
        private int addInventory(InventoryData param)
        {
            int iR = 0;
            SqlTransaction transaction;
            string serializedJsondata = JsonConvert.SerializeObject(param);

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_Inventory_Crud_Operation", con))
                {
                    if (con.State != 0) con.Close();
                    con.Open();
                    transaction = con.BeginTransaction("addNewTransaction");
                    try
                    {
                        // Start transaction. 
                        cmd.Connection = con;
                        cmd.Transaction = transaction;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.NVarChar).Value = "Add";
                        cmd.Parameters.Add("@InvName", SqlDbType.NVarChar).Value = param.InventoryName;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = param.Description;
                        cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = param.Price;

                        //Execute the command  
                        iR = cmd.ExecuteNonQuery();
                        transaction.Commit();
                        LogsManager.WriteLog("Add", serializedJsondata, _connectionString);
                    }
                    catch (Exception ex)
                    {
                        iR = 0;
                        transaction.Rollback();
                        if (con.State != 0) con.Close();
                        LogsManager.ErrorWriteLog("Add", "Exception : " + ex.Message.ToString(), serializedJsondata, _connectionString);
                    }
                    finally
                    {
                        if (con.State != 0) con.Close();
                    }
                    return iR;
                }
            }
        }
        private int updateInventory(uInventoryData param)
        {
            int iR = 0;
            SqlTransaction transaction;
            string serializedJsondata = JsonConvert.SerializeObject(param);

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_Inventory_Crud_Operation", con))
                {
                    if (con.State != 0) con.Close();
                    con.Open();
                    transaction = con.BeginTransaction("addNewTransaction");
                    try
                    {
                        // Start transaction. 
                        cmd.Connection = con;
                        cmd.Transaction = transaction;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.NVarChar).Value = "Update";
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = param.ID;
                        cmd.Parameters.Add("@InvName", SqlDbType.NVarChar).Value = param.InventoryName;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = param.Description;
                        cmd.Parameters.Add("@Price", SqlDbType.NVarChar).Value = param.Price;

                        //Execute the command  
                        iR = cmd.ExecuteNonQuery();
                        transaction.Commit();
                        LogsManager.WriteLog("Update", serializedJsondata, _connectionString);
                    }
                    catch (Exception ex)
                    {
                        iR = 0;
                        transaction.Rollback();
                        if (con.State != 0) con.Close();
                        LogsManager.ErrorWriteLog("Update", "Exception : " + ex.Message.ToString(), serializedJsondata, _connectionString);
                    }
                    finally
                    {
                        if (con.State != 0) con.Close();
                    }
                    return iR;
                }
            }
        }
        private int deleteInventory(dInventoryData param)
        {
            int iR = 0;
            SqlTransaction transaction;
            string serializedJsondata = JsonConvert.SerializeObject(param);

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_Inventory_Crud_Operation", con))
                {
                    if (con.State != 0) con.Close();
                    con.Open();
                    transaction = con.BeginTransaction("deleteTransaction");
                    try
                    {
                        // Start transaction. 
                        cmd.Connection = con;
                        cmd.Transaction = transaction;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@flag", SqlDbType.NVarChar).Value = "Delete";
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = param.ID;

                        //Execute the command  
                        iR = cmd.ExecuteNonQuery();
                        transaction.Commit();
                        LogsManager.WriteLog("Delete", serializedJsondata, _connectionString);
                    }
                    catch (Exception ex)
                    {
                        iR = 0;
                        transaction.Rollback();
                        if (con.State != 0) con.Close();
                        LogsManager.ErrorWriteLog("Delete", "Exception : " + ex.Message.ToString(), serializedJsondata, _connectionString);
                    }
                    finally
                    {
                        if (con.State != 0) con.Close();
                    }
                    return iR;
                }
            }

        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public void LogDataInsert(string JsonData, string ErrorMsg, string EventName)
        {
            LogsManager.ErrorWriteLog(EventName, ErrorMsg, JsonData, _connectionString);
        }
    }
}
