using System;
using System.Data.SqlClient;

namespace api_mssql_dapper.Settings
{
    public class SQLSettings
    {
        //object  SQLSettings in appsettings.json
        public string Host { get; set; }

        public string DBName { get; set; }

        public string User { get; set; }

        public string Password { get; set; } //will be saved like a secret value in appsettings.json
        public string ConnectionString
        {
            get { return $"Data Source={Host};Initial Catalog={DBName};User ID={User};Password={Password}"; }
        }
        public bool CheckConnectionString(string connnection_string)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connnection_string))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("===ConnectionTest: ", ex.Message);
                return false;
            }
        }

    }
}