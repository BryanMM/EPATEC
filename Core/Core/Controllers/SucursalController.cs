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
    public class SucursalController : ApiController
    {
        public static IList<Sucursal> prolist = new List<Sucursal>();
        [AcceptVerbs("GET")]
        public Sucursal RPCStyleMethodFetchFirstEmployees()
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
            action = updateString.UpdateConnectionString("UPDATE SUCURSAL ", uattr, uvalue, cattr, cvalue);
            System.Diagnostics.Debug.WriteLine(action);

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
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

        public string FormConnectionString(string baseString, string[] attr, string[] ids)
        {
            string ConnectionString = "SELECT * FROM " + baseString + " WHERE ";
            string LogicalConnector = "OR";
            if(ids == null)
            {
                return "SELECT * FROM " + baseString + ";";
            }
            if (attr.Length > 1)
            {
                LogicalConnector = "AND";
            }
            System.Diagnostics.Debug.WriteLine(attr.Length);
            for (int i = 0; i < attr.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine(i);
                if (i == (attr.Length - 1))
                {
                    ConnectionString = ConnectionString + attr[i] + " LIKE \'%" + ids[i]+"%\';";
                    return ConnectionString;
                }
                else
                {
                    ConnectionString = ConnectionString + attr[i] + " LIKE \'%" + ids[i]+"%\' "+LogicalConnector+" ";
                }

            }
            return ConnectionString;
        }
        [HttpGet]
        [ActionName("Get")]
        public JsonResult<List<Sucursal>> Get(string attribute, string id)
        {
            List<Sucursal> values = new List<Sucursal>();
            Sucursal emp = null;
            string[] attr = attribute.Split(',');
            string[] ids = id.Split(',');
            System.Diagnostics.Debug.WriteLine("entrando al get");
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            System.Diagnostics.Debug.WriteLine("cargo base");
            SqlCommand sqlCmd = new SqlCommand();
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");
            string action = "";
            if (id != "undefined")
            {
                action = FormConnectionString("SUCURSAL",attr, ids);
            }
            else
            {
                action = "SELECT * FROM SUCURSAL;";
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
                emp = new Sucursal();
                emp.S_ID = Convert.ToInt32(reader.GetValue(0).ToString());
                emp.SName = reader.GetValue(1).ToString();
                emp.SAddress = reader.GetValue(2).ToString();
                values.Add(emp);

            }
            
            myConnection.Close();
            JsonResult<List<Sucursal>> results = Json(values);
            System.Diagnostics.Debug.WriteLine("Estp se va a poner loco "+results);
            return Json(values);
        }
        [HttpGet]
        [ActionName("Delete")]
        public void Delete(string attribute, string id)
        {

            SqlConnection DeleteSP = new SqlConnection();
            DeleteSP.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlCommand SPCmd = new SqlCommand();
            SPCmd.CommandType = CommandType.Text;
            SPCmd.CommandText = "DELETE FROM NEED WHERE S_ID=" + id + ";";
            SPCmd.Connection = DeleteSP;
            DeleteSP.Open();
            SPCmd.ExecuteNonQuery();
            DeleteSP.Close();

            SqlConnection DeleteSE = new SqlConnection();
            DeleteSE.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlCommand SECmd = new SqlCommand();
            SECmd.CommandType = CommandType.Text;
            SECmd.CommandText = "DELETE FROM EMPLOYEE WHERE S_ID=" + id + ";";
            SECmd.Connection = DeleteSE;
            DeleteSE.Open();
            SECmd.ExecuteNonQuery();
            DeleteSE.Close();

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "DELETE FROM SUCURSAL WHERE S_ID=" + id + ";";
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
        [HttpPost]
        [ActionName("Post")]
        public void AddSucursal(Sucursal sucursal)
        {
            System.Diagnostics.Debug.WriteLine(sucursal.S_ID);
            System.Diagnostics.Debug.WriteLine("entrando al post");

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            System.Diagnostics.Debug.WriteLine(myConnection.State);

            sqlCmd.CommandText = "INSERT INTO SUCURSAL(S_ID,SName,SAddress) Values(@S_ID,@SName,@SAddress)";
            System.Diagnostics.Debug.WriteLine("generando comando");

            sqlCmd.Connection = myConnection;
            sqlCmd.Parameters.AddWithValue("@S_ID", sucursal.S_ID);
            sqlCmd.Parameters.AddWithValue("@SName", sucursal.SName);
            sqlCmd.Parameters.AddWithValue("@SAddress", sucursal.SAddress);

            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
