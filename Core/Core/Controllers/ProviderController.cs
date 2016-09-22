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
            List<int> PR_ID = new List<int>();
            SqlDataReader reader;
            SqlConnection SelectProducts = new SqlConnection();
            SelectProducts.ConnectionString = GetConnectionString();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            SucursalController deleteString = new SucursalController();
            sqlCmd.CommandText = "SELECT PR_ID FROM PRODUCT WHERE " + attribute + "='" + id + "';";
            sqlCmd.Connection = SelectProducts;
            SelectProducts.Open();
            reader = sqlCmd.ExecuteReader();
            while (reader.Read())
            {
                PR_ID.Add(Convert.ToInt32(reader.GetValue(0)));
            }
            SelectProducts.Close();

            DeletePC(PR_ID,"PC","PR_ID");
            DeletePC(PR_ID, "PRODUCT", "PR_ID");
            List<int> PDR_ID = new List<int>();
            PDR_ID.Add(Convert.ToInt32(id));
            DeletePC(PDR_ID, "NEED","PDR_ID");

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand PDRCmd = new SqlCommand();
            PDRCmd.CommandType = CommandType.Text;
            PDRCmd.CommandText = "DELETE FROM EPROVIDER WHERE " + attribute + "='" + id + "';";
            PDRCmd.Connection = myConnection;
            myConnection.Open();
            int rowDeleted = PDRCmd.ExecuteNonQuery();
            myConnection.Close();
        }


        private void DeletePC(List<int> products, string table,string attribute)
        {
            string action = "DELETE FROM "+table+" WHERE ";

            for(int i = 0; i < products.Count; i++)
            {
                if(i == (products.Count - 1))
                {
                    action += attribute+"=" + products[i] + ";";
                }
                else
                {
                    action += attribute+"=" + products[i] + " AND ";
                }
            }
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = action;
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

            SqlConnection CategoryConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand CateCmd = new SqlCommand();
            CateCmd.CommandType = CommandType.Text;
            CateCmd.CommandText = "INSERT INTO NEED(S_ID,PDR_ID) Values(@S_ID,@PDR_ID)";
            CateCmd.Connection = CategoryConnection;
            CateCmd.Parameters.AddWithValue("@S_ID", provider.S_ID);
            CateCmd.Parameters.AddWithValue("@PDR_ID", provider.P_ID);
            CategoryConnection.Open();
            CateCmd.ExecuteNonQuery();
            CategoryConnection.Close();
        }
    }
}
