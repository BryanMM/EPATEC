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
                emp.LName = reader.GetValue(2).ToString();
                emp.CAddress = reader.GetValue(3).ToString();
                emp.Charge = reader.GetValue(4).ToString();

            }
            return emp;
            myConnection.Close();
        }
        [ActionName("DeleteEmployee")]
        public void DeleteEmployeeByID(int id)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString =GetConnectionString();

            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "DELETE FROM EMPLOYEE WHERE E_ID=" + id + "";
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            int rowDeleted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
        [HttpPost]
        public void AddEmployee(Employee employee)
        {
            System.Diagnostics.Debug.WriteLine(employee.E_ID);
            System.Diagnostics.Debug.WriteLine(employee.CName);
            System.Diagnostics.Debug.WriteLine("entrando al post");

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
