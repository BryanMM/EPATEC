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

namespace Core.Controllers
{
    public class CheckController : ApiController
    {
        static private string GetConnectionString()
        {
            return @"Data Source=ISAAC\ISAACSERVER;Initial Catalog=EPATEC;"
                + "Integrated Security=true;";
        }

        [HttpGet]
        [ActionName("get")]
        public string Checkclient(string attribute, string id,string charge)
        {
            string confirmation = "false";
            string action = "";
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            System.Diagnostics.Debug.WriteLine("cargo base");
            SqlCommand sqlCmd = new SqlCommand();
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");
            if (charge.Equals("Client"))
            {
                action = "SELECT C_ID,CPassword FROM CLIENT WHERE C_ID=" + attribute + " AND CPassword='" + id + "';";
                System.Diagnostics.Debug.WriteLine(action);
            }
            else
            {
                action = "SELECT E_ID,EPassword,Charge FROM EMPLOYEE WHERE E_ID" + attribute + " AND EPassword='" + id + "' AND Charge='" + charge + "';";
            }
            
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = action;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            int result = 0;
            if (sqlCmd.ExecuteScalar() !=null)
                 result = (int)sqlCmd.ExecuteScalar();
            if (result > 1)
            {
                confirmation = "true";
            }
            return confirmation;
        }
    }
}
