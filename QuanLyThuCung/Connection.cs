using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace QuanLyThuCung
{
    public class Connection
    {
        private static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\trung hieu\Documents\DBshopthucung.mdf;Integrated Security=True;Connect Timeout=30";
        private static SqlConnection Con;

        public static SqlConnection GetConnection()
        {
            if (Con == null)
            {
                Con = new SqlConnection(connectionString);
            }
            return Con;
        }
    }
}
