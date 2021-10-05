using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace api_mssql_dapper
{
    public class SQLHelper
    {
        private static string _CONNECTION_STRINGS;
        public static string CONNECTION_STRINGS
        {
            get
            {
                if (String.IsNullOrEmpty(_CONNECTION_STRINGS))
                    //return "Data Source=.;Initial Catalog=DEMO_DB; Trusted_Connection=True;";
                    return "";
                return _CONNECTION_STRINGS;
            }
            set { _CONNECTION_STRINGS = value; }
        }
        // public static string CONNECTION_STRINGS { get { return $"Data Source={sqlSetting.Host};Initial Catalog=DEMO_DB;User ID=sa;Password=minh123"; } }
        public static long Insert<T>(T entityToInsert) where T : class
        {

            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                long re = 0;
                try
                {
                    re = connection.Insert(entityToInsert);
                }
                catch (Exception ex)
                {
                    re = 0;
                    System.Diagnostics.Debug.WriteLine("SQLHelper/Insert: x_Error: ", ex.Message);
                    //XtraMessageBox.Show(ex.Message);
                }
                return re;
            }
        }

        public static bool Update<T>(T entityToUpdate) where T : class
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                var result = false;
                try
                {
                    result = connection.Update(entityToUpdate);
                }
                catch (Exception ex)
                {
                    result = false;
                    System.Diagnostics.Debug.WriteLine("SQLHelper/Update: x_Error: ", ex.Message);
                }
                return result;
            }
        }

        public static bool Delete<T>(T entityToDelete) where T : class
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                bool success = false;
                try
                {
                    success = connection.Delete(entityToDelete);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("SQLHelper/Delete: x_Error: ", ex.Message);
                }
                return success;
            }
        }
        public static DataTable ExecProcedureDataAsDataTable(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                DataTable table = new DataTable();
                try
                {
                    var reader = connection.ExecuteReader(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
                    table.Load(reader);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("SQLHelper/ExecProcedureDataAsDataTable: x_Error:  ", ex.Message);
                }

                return table;
            }
        }



        public static async Task<DataTable> ExecProcedureDataAsyncAsDataTable(string ProcedureName, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                var reader = await c.ExecuteReaderAsync(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            });

        }


        public static DataTable ExecQueryDataAsDataTable(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                var reader = connection.ExecuteReader(T_SQL, param: parametter, commandType: CommandType.Text);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        public static async Task<DataTable> ExecQueryDataAsyncAsDataTable(string T_SQL, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                var reader = await c.ExecuteReaderAsync(T_SQL, param: parametter, commandType: CommandType.Text);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            });

        }

        public static IEnumerable<T> ExecProcedureData<T>(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.Query<T>(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public static T ExecProcedureDataFistOrDefault<T>(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public static async Task<IEnumerable<T>> ExecProcedureDataAsync<T>(string ProcedureName, object parametter = null)
        {
            return await WithConnection(async c =>
            {
                return await c.QueryAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            });
        }

        public static async Task<T> ExecProcedureDataFirstOrDefaultAsync<T>(string ProcedureName, object parametter = null)
        {
            // using (var connection = new SqlConnection(CONNECTION_STRINGS))
            // {
            //     connection.Open();
            //     return connection.QueryFirstOrDefaultAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            // }
            return await WithConnection(async c =>
            {
                return await c.QueryFirstOrDefaultAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            });
        }

        public static int ExecProcedureNonData(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                //return affectedRows 
                return connection.Execute(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public static Task<int> ExecProcedureNonDataAsync(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                //return affectedRows 
                return connection.ExecuteAsync(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<T> ExecQueryData<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.Query<T>(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public static T ExecQueryDataFistOrDefault<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public static async Task<IEnumerable<T>> ExecQueryDataAsync<T>(string T_SQL, object parametter = null)
        {

            return await WithConnection(async c =>
            {
                return await c.QueryAsync<T>(T_SQL, parametter, commandType: CommandType.Text);
            });


        }

        public static T ExecQueryDataFirstOrDefaultAsync<T>(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.QueryFirstOrDefaultAsync<T>(T_SQL, parametter, commandType: CommandType.Text).Result;
            }
        }

        public static int ExecQueryNonData(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.Execute(T_SQL, parametter, commandType: CommandType.Text);
            }
        }

        public static async Task<int> ExecQueryNonDataAsync(string T_SQL, object parametter = null)
        {

            return await WithConnection(async c =>
            {
                return await c.ExecuteAsync(T_SQL, parametter, commandType: CommandType.Text);
            });



        }

        public static async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {

            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                await connection.OpenAsync(); // Asynchronously open a connection to the database
                return await getData(connection); // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
            }

        }

        public static object ExecProcedureSacalar(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.ExecuteScalar<object>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
            }

        }

        public static object ExecProcedureSacalarAsync(string ProcedureName, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.ExecuteScalarAsync<object>(ProcedureName, parametter, commandType: CommandType.StoredProcedure).Result;
            }
        }

        public static object ExecQuerySacalar(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.ExecuteScalar<object>(T_SQL, parametter, commandType: CommandType.Text);
            }

        }

        public static object ExecQuerySacalarAsync(string T_SQL, object parametter = null)
        {
            using (var connection = new SqlConnection(CONNECTION_STRINGS))
            {
                connection.Open();
                return connection.ExecuteScalarAsync<object>(T_SQL, parametter, commandType: CommandType.Text).Result;
            }

        }
    }
}