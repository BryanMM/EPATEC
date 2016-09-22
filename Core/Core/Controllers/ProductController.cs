using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApplication1.Models;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Results;

namespace WebApplication1.Controllers
{
    public class ProductController : ApiController
    {
        public static IList<Product> prolist = new List<Product>();
        [AcceptVerbs("GET")]
        public Product RPCStyleMethodFetchFirstEmployees()
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
            action = updateString.UpdateConnectionString("UPDATE PRODUCT ", uattr, uvalue, cattr, cvalue);
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
        public JsonResult<List<Product>> Get(string attribute, string id)
        {
            string[] attr = attribute.Split(',');
            string[] ids = id.Split(',');
            List<Product> values = new List<Product>();
            Product emp = null;

            System.Diagnostics.Debug.WriteLine("entrando al get");
            SqlDataReader reader = null;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            System.Diagnostics.Debug.WriteLine("cargo base");
            SqlCommand sqlCmd = new SqlCommand();
            System.Diagnostics.Debug.WriteLine("cargo sqlcommand");
            string action = "";

            if(id != "undefined")
            {
                SucursalController constructor = new SucursalController();
                action = constructor.FormConnectionString("PRODUCT", attr, ids);
            }
            else
            {
                action = "SELECT * FROM PRODUCT;";
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
                emp = new Product();
                emp.PR_ID = Convert.ToInt32(reader.GetValue(0).ToString());
                emp.PName = reader.GetValue(1).ToString();
                emp.Price = Convert.ToInt32(reader.GetValue(5).ToString());
                emp.Extent = Convert.ToBoolean(reader.GetValue(2).ToString());
                emp.PDescription = reader.GetValue(3).ToString();
                emp.Quantity = Convert.ToInt32(reader.GetValue(4).ToString());
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
            string deletepc = "DELETE FROM PC WHERE ";
            SqlConnection DeleteProduct = new SqlConnection();
            DeleteProduct.ConnectionString = GetConnectionString();
            for(int i = 0; i < actions.Length; i++)
            {
                if (actions[i].Equals("PR_ID"))
                {
                    deletepc = deletepc + actions[i] + "='" + ids[i] + "';";
                }
            }
            SqlCommand PCmd = new SqlCommand();
            PCmd.CommandType = CommandType.Text;
            PCmd.CommandText = deletepc;
            PCmd.Connection = DeleteProduct;
            DeleteProduct.Open();
            PCmd.ExecuteNonQuery();
            DeleteProduct.Close();

            string deleteproduct = "DELETE FROM PRODUCT WHERE ";
            for (int i = 0; i < actions.Length; i++)
            {
                if(i == (actions.Length - 1))
                {
                    deleteproduct += actions[i] + "='" + ids[i] + "';";
                }
                else
                {
                    deleteproduct += actions[i] + "='" + ids[i] + "' AND ";
                }
            }
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = deleteproduct;
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            int rowDeleted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();
        }
        [HttpPost]
        [ActionName("Post")]
        public void AddProduct(Product product)
        {
            
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;

            sqlCmd.CommandText = "INSERT INTO PRODUCT(PR_ID,Price,Extent,PDescription,Quantity,PName,P_ID,S_ID) Values(@S_ID,@PR_ID,@Price,@Extent,@PDescription,@Quantity,@PName,@P_ID)";

            sqlCmd.Connection = myConnection;
            sqlCmd.Parameters.AddWithValue("@PR_ID", product.PR_ID);
            sqlCmd.Parameters.AddWithValue("@Price", product.Price);
            sqlCmd.Parameters.AddWithValue("@Extent", product.Extent);
            sqlCmd.Parameters.AddWithValue("@PDescription", product.PDescription);
            sqlCmd.Parameters.AddWithValue("@Quantity", product.Quantity);
            sqlCmd.Parameters.AddWithValue("@PName", product.PName);
            sqlCmd.Parameters.AddWithValue("@P_ID", product.PDR_ID);
            sqlCmd.Parameters.AddWithValue("@S_ID", product.S_ID);

            myConnection.Open();
            int rowInserted = sqlCmd.ExecuteNonQuery();
            myConnection.Close();

            SqlConnection CategoryConnection = new SqlConnection();
            myConnection.ConnectionString = GetConnectionString();
            SqlCommand CateCmd = new SqlCommand();
            CateCmd.CommandType = CommandType.Text;
            CateCmd.CommandText = "INSERT INTO PC(CA_ID,PR_ID) Values(@CA_ID,@PR_ID)";
            CateCmd.Connection = CategoryConnection;
            CateCmd.Parameters.AddWithValue("@CA_ID",product.CA_ID);
            CateCmd.Parameters.AddWithValue("@PR_ID",product.PR_ID);
            CategoryConnection.Open();
            CateCmd.ExecuteNonQuery();
            CategoryConnection.Close();

        }
    }
}
