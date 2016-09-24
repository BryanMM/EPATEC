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

namespace Core.Controllers
{
    public class StatisticController : ApiController
    {
        
        static private string GetConnectionString()
        {
            return @"Data Source=ISAAC\ISAACSERVER;Initial Catalog=EPATEC;"
                + "Integrated Security=true;";
        }
        public string FormGetString(string baseString, string[] attr, string[] ids)
        {
            string ConnectionString = "SELECT HAS.PRNAME, SUM(HAS.PRAmount) AS E FROM HAS, EORDER WHERE HAS.O_ID=EORDER.O_ID AND ";
            if (attr[0].Equals("case0"))
            {
                return "SELECT HAS.PRNAME, SUM(HAS.PRAmount) AS E FROM HAS, EORDER WHERE HAS.O_ID=EORDER.O_ID GROUP BY HAS.PRName ORDER BY E DESC;";
            }
            if (attr[0].Equals("case1"))
            {
                return "select EORDER.S_ID, HAS.PRName, count(HAS.PRAmount) as E from HAS,EORDER where HAS.O_ID = EORDER.O_ID group by EORDER.S_ID, HAS.PRName order by E DESC;";
            }
            else if (attr[0].Equals("case2"))
            {
                return "SELECT EORDER.S_ID, count(*) as E FROM EORDER GROUP BY EORDER.S_ID";
            }
            else { 
            System.Diagnostics.Debug.WriteLine("coso");
                for (int i = 0; i < attr.Length; i++)
                {
                    System.Diagnostics.Debug.WriteLine(i);
                    if (i == (attr.Length - 1))
                    {
                        ConnectionString = ConnectionString + attr[i] + " LIKE \'%" + ids[i] + "%\' ";

                    }
                    else
                    {
                        ConnectionString = ConnectionString + attr[i] + " LIKE \'%" + ids[i] + "%\' " + "AND" + " ";
                    }
                }

            }
            System.Diagnostics.Debug.WriteLine("ASDASDAS");

            ConnectionString = ConnectionString + "GROUP BY HAS.PRNAME ORDER BY E DESC;";

            System.Diagnostics.Debug.WriteLine(ConnectionString);

            return ConnectionString;
        }

        [HttpGet]
        [ActionName("GetMostSold")]
        public JsonResult<List<Statistic>> GetMostSold(string attribute, string id)
        {
            string[] attr = attribute.Split(',');
            string[] ids = id.Split(',');
            List<Statistic> values = new List<Statistic>();
            Statistic emp = null;

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
                action = FormGetString("HAS", attr, ids);
            }
            else
            {
                action = "SELECT COUNT() FROM EORDER;";
            }

            System.Diagnostics.Debug.WriteLine(action);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = action;
            System.Diagnostics.Debug.WriteLine("cargo comando");

            sqlCmd.Connection = myConnection;
            myConnection.Open();
            System.Diagnostics.Debug.WriteLine("estado " + myConnection.State);

            reader = sqlCmd.ExecuteReader();
            if (attr[0].Equals("case1")) {
                while (reader.Read())
                {
                    
                    emp = new Statistic();
                    emp.S_ID = Convert.ToInt32(reader.GetValue(0));
                    emp.PRName = reader.GetValue(1).ToString();
                    emp.PRAmount = Convert.ToInt32(reader.GetValue(2));

                    values.Add(emp);
                } }
            if (attr[0].Equals("case2"))
            {
                while (reader.Read())
                {
                   
                    emp = new Statistic();
                    emp.S_ID = Convert.ToInt32(reader.GetValue(0));
                   
                    emp.SAmount = Convert.ToInt32(reader.GetValue(1));

                    values.Add(emp);
                }
            }
            else 
            {
                while (reader.Read())
                {
                    
                    emp = new Statistic();
                    emp.PRName = reader.GetValue(0).ToString();
                    emp.PRAmount = Convert.ToInt32(reader.GetValue(1));

                    values.Add(emp);
                }
            }



            myConnection.Close();
            return Json(values);
        }
    }
}
