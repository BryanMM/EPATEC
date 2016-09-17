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
    public class ProviderController : ApiController
    {
        public static IList<Provider> prolist = new List<Provider>();
        [AcceptVerbs("GET")]
        public Provider RPCStyleMethodFetchFirstEmployees()
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
            action = updateString.UpdateConnectionString("UPDATE EPROVIDER ", uattr, uvalue, cattr, cvalue);
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
        public JsonResult<List<Provider>> Get(string attribute, string id)
        {
            List<Provider> values = new List<Provider>();
            Provider emp = null;
            string[] attr = attribute.Split(',');
            string[] ids = id.Split(',');

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
                action = constructor.FormConnectionString("EPROVIDER", attr, ids);
            }
            else
            {
                action = "SELECT * FROM EPROVIDER;";
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
                emp = new Provider();
                emp.P_ID = Convert.ToInt32(reader.GetValue(0));
                emp.PName = reader.GetValue(1).ToString();
                emp.LName = reader.GetValue(2).ToString();
                emp.PAddress = reader.GetValue(3).ToString();
                emp.Phone = Convert.ToInt32(reader.GetValue(4).ToString());
                emp.Day = Convert.ToInt32(reader.GetValue(5).ToString());
                emp.Month = Convert.ToInt32(reader.GetValue(6).ToString());
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
            sqlCmd.CommandText = deleteString.FormConnectionString("DELETE FROM EPROVIDER WHERE ", actions, ids);
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            int rowDeleted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
        [HttpPost]
        [ActionName("Post")]
        public void AddProvider(Provider provider)
        {
            System.Diagnostics.Debug.WriteLine(provider.P_ID);
            System.Diagnostics.Debug.WriteLine(provider.PName);
            System.Diagnostics.Debug.WriteLine("entrando al post");

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            System.Diagnostics.Debug.WriteLine(myConnection.State);

            sqlCmd.CommandText = "INSERT INTO EPROVIDER(P_ID,PName,LName,PAddress,Phone,Day,Month,Year) Values(@P_ID,@PName,@LName,@PAddress,@Phone,@Day,@Month,@Year)";
            System.Diagnostics.Debug.WriteLine("generando comando");

            sqlCmd.Connection = myConnection;
            sqlCmd.Parameters.AddWithValue("@P_ID", provider.P_ID);
            sqlCmd.Parameters.AddWithValue("@PName", provider.PName);
            sqlCmd.Parameters.AddWithValue("@LName", provider.LName);
            sqlCmd.Parameters.AddWithValue("@PAddress", provider.PAddress);
            sqlCmd.Parameters.AddWithValue("@Phone", provider.Phone);
            sqlCmd.Parameters.AddWithValue("@Day", provider.Day);
            sqlCmd.Parameters.AddWithValue("@Month", provider.Month);
            sqlCmd.Parameters.AddWithValue("@Year", provider.Year);
            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
