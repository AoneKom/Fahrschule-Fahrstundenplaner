using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fahrschule___Fahrstundenplaner
{
    internal class Database
    {
        public static string ConnectionString =
            "Server=localhost;Database=DrivingSchoolDB;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}