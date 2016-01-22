using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace WebaApiTest
{

    /// <summary>
    /// DatabaseQueryExecutor
    /// </summary>
    public class DatabaseQueryExecutor
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DatabaseToExecution"]]
                .ConnectionString;

        private readonly string _sqlScriptsPath = ConfigurationManager.AppSettings["SqlFilesPath"];
        private readonly string _sqlDbTimeout = ConfigurationManager.AppSettings["DataBaseTimeout"];

        /// <summary>
        /// Gets the data from data base.
        /// </summary>
        /// <param name="sqlScript">The SQL script.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="throwExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <returns>DataTable.</returns>
        public DataTable GetDataFromDataBase(string sqlScript, Dictionary<string, object> parameters,
            bool throwExceptions = false)
        {
            using (var connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string sqlStatement = GetStringQueryFromFile(sqlScript);
                        foreach (var parameter in parameters)
                        {
                            sqlStatement = sqlStatement.Replace(parameter.Key, parameter.Value.ToString());
                        }
                        var sqlAdapter = new SqlDataAdapter(sqlStatement, connection)
                        {
                            SelectCommand = {CommandTimeout = 200}
                        };


                        var dtResult = new DataTable();
                        // Fill the DataTable with the result of the SQL statement
                        sqlAdapter.Fill(dtResult);

                        sqlAdapter.Dispose();
                        connection.Close();
                        connection.Dispose();

                        return dtResult;
                    }

                }
                catch (Exception exception)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (throwExceptions)
                    {
                        throw;
                    }

                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

            return null;
        }

        public DataTable GetDataFromDataBase(string sqlScript, bool throwExceptions = false)
        {
            return GetDataFromDataBase(sqlScript, new Dictionary<string, object>(), throwExceptions);
        }

        public string GetStringQueryFromFile(string fileName)
        {
            DirectoryInfo directory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "SQLScripts"));
            var fileNameWith = string.Format("{0}{1}", fileName, fileName.EndsWith(".sql") ? string.Empty : ".sql");

            FileInfo file = directory.GetFiles(fileNameWith, SearchOption.AllDirectories).FirstOrDefault();
            if (file == null)
            {
                throw new FileNotFoundException();
            }

            string query;

            using (var scriptReader = file.OpenText())
            {
                query = scriptReader.ReadToEnd();
            }

            return query;
        }
    }
}
