using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Data
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public DataTable ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(query, conn);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            DataTable dt = new DataTable();
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            return dt;
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object>? parameters = null)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(query, conn);
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // You can also add ExecuteScalar, ExecuteReader, etc.
    }
}
