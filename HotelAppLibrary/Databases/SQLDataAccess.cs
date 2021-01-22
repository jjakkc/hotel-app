using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Databases
{
    public class SQLDataAccess : ISQLDataAccess
    {
        private readonly IConfiguration _config;

        // Using dependency injection here to set config on an already instantiated object elsewhere
        public SQLDataAccess(IConfiguration config)
        {
            _config = config;
        }
        public List<T> LoadData<T, U>(string sqlStatement,
                                      U parameters,
                                      string connectionStringName,
                                      bool isStoredProcedure = false)
        {
            // if we have multiple dbs want to be able to change connection at runtime
            string connectionString = _config.GetConnectionString(connectionStringName);

            CommandType commandType = CommandType.Text;

            // determing if stored procedure or plain query
            if (isStoredProcedure)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters, commandType: commandType).ToList();
                return rows;
            }
        }

        public void SaveData<T>(string sqlStatement,
                                T parameters,
                                string connectionStringName,
                                bool isStoredProcedure = false)
        {
            // if we have multiple dbs want to be able to change connection at runtime
            string connectionString = _config.GetConnectionString(connectionStringName);

            CommandType commandType = CommandType.Text;

            // determing if stored procedure or plain query
            if (isStoredProcedure)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters, commandType: commandType);
            }
        }
    }
}
