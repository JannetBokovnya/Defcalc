using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBConn;
using System.Data.SQLite;
using System.Configuration;

namespace DEFCALC
{
    class DataSQLightHelper
    {
        protected string con_str;//строка подключения к базе

        public static SQLiteConnection tt()
        {
            string conStr = ConfigurationManager.ConnectionStrings["SqlLiteDataBaseName"].ConnectionString;
            SQLiteConnection conn = new SQLiteConnection(conStr);
            conn.Open();
            return conn;
        }
    }
}
