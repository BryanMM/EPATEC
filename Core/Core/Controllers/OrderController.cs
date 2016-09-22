using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using Core.Models;
using Newtonsoft.Json.Linq;

namespace WebApplication1.Controllers
{
    public class OrderController : ApiController
    {
        public static IList<Order> prolist = new List<Order>();
        [AcceptVerbs("GET")]
        public Order RPCStyleMethodFetchFirstEmployees()
        {
            return prolist.FirstOrDefault();
        }
        static private string GetConnectionString()
        {
            return @"Data Source=ISAAC\ISAACSERVER;Initial Catalog=EPATEC;"
                + "Integrated Security=true;";
        }

        [HttpGet]
        [ActionName("Update")]
        public void UpdateRecords(string attr, string avalue, string clause, string id)
        {
            string[] uattr = attr.Split(',');
            string[] uvalue = avalue.Split(',');
            string[] cattr = clause.Split(',');
            string[] cvalue = id.Split(',');
            string action = "";
            CategoryController updateString = new CategoryController();
            action = updateString.UpdateConnectionString("UPDATE EORDER ", uattr, uvalue, cattr, cvalue);
            System.Diagnostics.Debug.WriteLine(action);

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            System.Diagnostics.Debug.WriteLine("cargo base");
            SqlCommand sqlCmd = new SqlCommand();
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");

            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = action;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }

        [HttpGet]
        [ActionName("Get")]
        public JsonResult<List<Order>> Get(string attribute, string id)
        {
            string[] attr = attribute.Split(',');
            string[] ids = id.Split(',');
            List<Order> values = new List<Order>();
            Order emp = null;

            System.Diagnostics.Debug.WriteLine("entrando al get");
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            System.Diagnostics.Debug.WriteLine("cargo base");
            SqlCommand sqlCmd = new SqlCommand();
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");
            string action = "";
            if (id != "undefined")
            {
                SucursalController constructor = new SucursalController();
                action = constructor.FormConnectionString("EORDER", attr, ids);
            }
            else
            {
                action = "SELECT * FROM EORDER;";
            }

            System.Diagnostics.Debug.WriteLine(action);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = action;
            System.Diagnostics.Debug.WriteLine("cargo comando");

            sqlCmd.Connection = myConnection;
            myConnection.Open();
            System.Diagnostics.Debug.WriteLine("estado " + myConnection.State);

            reader = sqlCmd.ExecuteReader();

            while (reader.Read())
            {
                emp = new Order();
                emp.O_ID = Convert.ToInt32(reader.GetValue(0));
                emp.OPriority = Convert.ToInt32(reader.GetValue(1).ToString());
                emp.OStatus = reader.GetValue(2).ToString();
                emp.OrderDate = Convert.ToDateTime(reader.GetValue(3).ToString());
                values.Add(emp);
            }
            
            myConnection.Close();
            return Json(values);
        }
        [HttpGet]
        [ActionName("Delete")]
        public void Delete(string attribute, string id) 
        {
            string[] actions = attribute.Split(',');
            string[] ids = id.Split(',');
            SqlConnection EraseHas = new SqlConnection();
            EraseHas.ConnectionString = GetConnectionString();
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].Equals("O_ID"))
                {
                    SqlCommand EraseCmd = new SqlCommand();
                    EraseCmd.CommandType = CommandType.Text;

                    EraseCmd.CommandText = "DELETE FROM HAS WHERE O_ID=" + ids[i];
                    EraseCmd.Connection = EraseHas;
                    EraseHas.Open();
                    EraseCmd.ExecuteNonQuery();
                    EraseHas.Close();
                }
            }
            


            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            SucursalController deleteString = new SucursalController();
            string action = "DELETE FROM EORDER WHERE ";
            for (int i = 0; i < actions.Length; i++)
            {
                if(i == (actions.Length - 1))
                {
                    action = action +  actions[i] + "='" + ids[i] + "';";
                }
                else
                {
                    action = action + actions[i] + "='" + ids[i] + "' AND ";
                }
            }
            sqlCmd.CommandText = action;
            System.Diagnostics.Debug.WriteLine(action);
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            int rowDeleted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }

        [ActionName("post")]
        public void AddOrder(Order order)
        {
            int rowInserted = 0;
            System.Diagnostics.Debug.WriteLine(order.C_ID);
            string[] products = order.Products.Split(',');
            string[] values = order.Amount.Split(',');
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand sqlCmd = new SqlCommand();
            myConnection.Open();
            sqlCmd.CommandType = CommandType.Text;
            System.Diagnostics.Debug.WriteLine(myConnection.State);
            sqlCmd.CommandText = "INSERT INTO EORDER(O_ID,OPriority,OStatus, OrderDate,S_ID,OPlatform,C_ID) Values(@O_ID,@OPriority,@OStatus, @OrderDate,@S_ID,@OPlatform,@C_ID)";
            System.Diagnostics.Debug.WriteLine("generando comando");
            sqlCmd.Connection = myConnection;
            sqlCmd.Parameters.AddWithValue("@O_ID", order.O_ID);
            sqlCmd.Parameters.AddWithValue("@OPriority", order.OPriority);
            sqlCmd.Parameters.AddWithValue("@OStatus", order.OStatus);
            sqlCmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
            sqlCmd.Parameters.AddWithValue("@S_ID", order.S_ID);
            sqlCmd.Parameters.AddWithValue("@OPlatform", order.OPlatform);
            sqlCmd.Parameters.AddWithValue("@C_ID", order.C_ID);
            rowInserted = sqlCmd.ExecuteNonQuery();
            
            string action = "INSERT INTO HAS(O_ID,PRName,PRAmount) Values(@O_ID,@PRName,@PRAmount)";
            for (int i = 0; i< products.Length; i++)
            {
                SqlCommand sqlCmd2 = new SqlCommand();
                sqlCmd2.Connection = myConnection;
                sqlCmd2.CommandText = action;
                sqlCmd2.Parameters.AddWithValue("@O_ID", order.O_ID);
                sqlCmd2.Parameters.AddWithValue("@PRName", products[i]);
                sqlCmd2.Parameters.AddWithValue("@PRAmount", Convert.ToInt32(values[i]));
                rowInserted = sqlCmd2.ExecuteNonQuery();
            }
            
            myConnection.Close();
        }
    }
}
