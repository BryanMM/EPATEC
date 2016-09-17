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

namespace WebApplication1.Controllers
{
    public class ClientController : ApiController
    {
        public static IList<Client> prolist = new List<Client>();
        [AcceptVerbs("GET")]
        public Client RPCStyleMethodFetchFirstEmployees()
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
            action = updateString.UpdateConnectionString("UPDATE CLIENT ", uattr, uvalue, cattr, cvalue);
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
        public JsonResult<List<Client>> Get(string attribute, string id)
        {
            string[] attr = attribute.Split(',');
            string[] ids = id.Split(',');
            List<Client> values = new List<Client>();
            Client emp = null;

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
                action = constructor.FormConnectionString("CLIENT", attr, ids);
            }
            else
            {
                action = "SELECT * FROM CLIENT;";
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
                emp = new Client();
                emp.C_ID = Convert.ToInt32(reader.GetValue(0));
                emp.FName = reader.GetValue(1).ToString();
                emp.LName = reader.GetValue(2).ToString();
                emp.CAddress = reader.GetValue(3).ToString();
                emp.Phone = Convert.ToInt32(reader.GetValue(4).ToString());
                emp.Day = Convert.ToInt32(reader.GetValue(5).ToString());
                emp.Month = Convert.ToInt32(reader.GetValue(6).ToString());
                emp.Penalization = Convert.ToInt32(reader.GetValue(7).ToString());
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
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            SucursalController deleteString = new SucursalController();
            sqlCmd.CommandText = deleteString.FormConnectionString("DELETE FROM CLIENT WHERE ", actions, ids);
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            int rowDeleted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
        [HttpPost]
        [ActionName("Post")]
        public void AddClient(Client client)
        {
            System.Diagnostics.Debug.WriteLine(client.C_ID);
            System.Diagnostics.Debug.WriteLine(client.FName);
            System.Diagnostics.Debug.WriteLine("entrando al post");

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            System.Diagnostics.Debug.WriteLine(myConnection.State);

            sqlCmd.CommandText = "INSERT INTO CLIENT(C_ID,FName,LName,CAddress,Phone,Day,Month,Year, Penalization, CPassword) Values(@C_ID,@FName,@LName,@CAddress,@Phone,@Day,@Month,@Year, @Penalization,@CPassword)";
            System.Diagnostics.Debug.WriteLine("generando comando");

            sqlCmd.Connection = myConnection;
            sqlCmd.Parameters.AddWithValue("@C_ID", client.C_ID);
            sqlCmd.Parameters.AddWithValue("@FName", client.FName);
            sqlCmd.Parameters.AddWithValue("@LName", client.LName);
            sqlCmd.Parameters.AddWithValue("@CAddress", client.CAddress);
            sqlCmd.Parameters.AddWithValue("@Phone", client.Phone);
            sqlCmd.Parameters.AddWithValue("@Day", client.Day);
            sqlCmd.Parameters.AddWithValue("@Month", client.Month);
            sqlCmd.Parameters.AddWithValue("@Year", client.Year);
            sqlCmd.Parameters.AddWithValue("@Penalization", client.Penalization);
            sqlCmd.Parameters.AddWithValue("@CPassword", client.CPassword);
            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
