using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;


namespace HRMS.Helper
{
    public class DBHelper
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["HRMSConnection"].ConnectionString;

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
