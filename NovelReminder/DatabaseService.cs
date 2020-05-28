using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;

namespace NovelReminder
{
    class DatabaseService
    {
        const string connectstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NovalReminderDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection _connection;
        public DatabaseService()
        {
            _connection = new SqlConnection(connectstring);
        }
        public void Find()
        {

        }
    }
}
