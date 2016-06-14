using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HansWebCrawler
{
    public static class DbControler
    {
        static readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jwludzik\Documents\Studia\HansWebCrawler\HansWebCrawler\WebBase.mdf;Integrated Security=True";

        static public List<object[]> GetRowsFromSqlQuery(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<object[]> listResult = new List<object[]>(); 
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var result = command.ExecuteReader();
                while (result.Read())
                {
                    object[] rowValues = new object[result.FieldCount];
                    result.GetValues(rowValues);
                    listResult.Add(rowValues);
                }
                return listResult;
            }
        }

        static public void PutNewRow(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                List<object[]> listResult = new List<object[]>();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
