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

namespace WebApplication1.Controllers
{
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

            return "Data Source=DESKTOP-E6QPTVT;Initial Catalog=EPATEC;"
                + "Integrated Security=true;";
        }

        [HttpGet]
        [ActionName("GetEmployeeByID")]
        public Employee Get(int id)
        {
            System.Diagnostics.Debug.WriteLine("entrando al get");
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            System.Diagnostics.Debug.WriteLine("cargo base");
            SqlCommand sqlCmd = new SqlCommand();
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");

            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Select * from EMPLOYEE where E_ID=" + id + "";
            System.Diagnostics.Debug.WriteLine("cargo comando");

            sqlCmd.Connection = myConnection;
            myConnection.Open();
            System.Diagnostics.Debug.WriteLine("estado " + myConnection.State);

            reader = sqlCmd.ExecuteReader();
            Employee emp = null;
            while (reader.Read())
            {
                emp = new Employee();
                emp.E_ID = Convert.ToInt32(reader.GetValue(0));
                emp.CName = reader.GetValue(1).ToString();

            }
            return emp;
            myConnection.Close();
        }

        [HttpPost]
        public void AddEmployee([FromBody]int e_id,[FromBody]string cname)
        {
            System.Diagnostics.Debug.WriteLine(e_id);
            System.Diagnostics.Debug.WriteLine(cname);
            System.Diagnostics.Debug.WriteLine("entrando al post");

            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString(); 
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            System.Diagnostics.Debug.WriteLine(myConnection.State);

            sqlCmd.CommandText = "INSERT INTO EMPLOYEE(E_ID,CName) Values(@e_id,@cname)";
            System.Diagnostics.Debug.WriteLine("generando comando");

            sqlCmd.Connection = myConnection;
            sqlCmd.Parameters.AddWithValue("@e_id", e_id);
            sqlCmd.Parameters.AddWithValue("@cname", cname);
            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
    }
}
