﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace POS_App {
    static class dbConfig {
        static private string server = "DESKTOP-15449HT\\KMUTNBMC";
        static private string database = "airbooking";
        static private string userID = "sa";
        static private string password = "123";

        static private String cnn =
            $"server={server};" +
            $"database={database};" +
            $"user id={userID};" +
            $"password={password}";

        static public SqlConnection connection =
            new SqlConnection(cnn);
    }
}
