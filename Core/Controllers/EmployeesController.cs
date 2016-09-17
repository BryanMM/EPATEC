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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeesController : ApiController
    {
        public static IList<Employee> listEmp = new List<Employee>();
        [AcceptVerbs("GET")]
        public Employee RPCStyleMethodFetchFirstEmployees()
        {
            return listEmp.FirstOrDefault();
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
            action = updateString.UpdateConnectionString("UPDATE EMPLOYEE ", uattr, uvalue, cattr, cvalue);
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
        public JsonResult<List<Employee>> Get(string attribute, string id)
        {
            Employee emp = null;
            List<Employee> values = new List<Employee>();
            string[] attr = attribute.Split(',');
            string[] ids = id.Split(',');
            System.Diagnostics.Debug.WriteLine(string.IsNullOrEmpty(id)+" "+attribute+" "+id.Length);
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
                action =  constructor.FormConnectionString("EMPLOYEE", attr, ids);
            }
            else
            {
                action = "SELECT * FROM EMPLOYEE;";
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
                emp = new Employee();
                emp.E_ID = Convert.ToInt32(reader.GetValue(0));
                emp.CName = reader.GetValue(1).ToString();
                emp.LName = reader.GetValue(2).ToString();
                emp.CAddress = reader.GetValue(3).ToString();
                emp.Charge = reader.GetValue(4).ToString();
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
            sqlCmd.CommandText = deleteString.FormConnectionString("DELETE FROM EMPLOYEE WHERE ", actions, ids);
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            int rowDeleted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
        [HttpPost]
        [ActionName("Post")]
        public void AddEmployee(Employee employee)
        {
            System.Diagnostics.Debug.WriteLine(employee.E_ID);
            System.Diagnostics.Debug.WriteLine(employee.CName);
            System.Diagnostics.Debug.WriteLine("entrando al post ");

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString(); 
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            System.Diagnostics.Debug.WriteLine(myConnection.State);

            sqlCmd.CommandText = "INSERT INTO EMPLOYEE(E_ID,CName,LName,CAddress,Charge) Values(@E_ID,@CName,@LName,@CAddress,@Charge)";
            System.Diagnostics.Debug.WriteLine("generando comando");

            sqlCmd.Connection = myConnection;
            sqlCmd.Parameters.AddWithValue("@E_ID", employee.E_ID);
            sqlCmd.Parameters.AddWithValue("@CName", employee.CName);
            sqlCmd.Parameters.AddWithValue("@LName", employee.LName);
            sqlCmd.Parameters.AddWithValue("@CAddress", employee.CAddress);
            sqlCmd.Parameters.AddWithValue("@Charge", employee.Charge);
            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
