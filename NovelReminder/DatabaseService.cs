﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;

namespace NovelReminder
{
    class DatabaseService
    {
        const string connectstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NovalReminderDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection _connection;
        public DatabaseService()
        {
            _connection = new SqlConnection(connectstring);
            try
            {
                _connection.Open();
                SqlCommand createtable = new SqlCommand(
                    "IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'UpdateRecords') AND type in (N'U'))"+
                    "Create Table UpdateRecords" +
                    "(Url varchar(30) Primary Key not null,LastChapter int Not null)"
                    , _connection);
                createtable.ExecuteNonQuery();
                //Console.WriteLine(version);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        ~DatabaseService()
        {
            _connection.Close();
        }
        public async ValueTask InsertOrUpdateOneAsync(string url,int lastUpdate)
        {
            try
            {
                var query = $"IF EXISTS(select * from UpdateRecords where Url=@url) " +
                    $"update UpdateRecords set LastChapter = @lastUpdate where Url=@url " +
                    "ELSE" +
                    $" insert into UpdateRecords(Url,LastChapter) values(@url,@lastUpdate)";
                SqlCommand insert = new SqlCommand(query, _connection);
                insert.Parameters.Add("@url", SqlDbType.VarChar,255).Value = url;
                insert.Parameters.Add("@lastUpdate", SqlDbType.Int).Value = lastUpdate;
                await insert.PrepareAsync();
                await insert.ExecuteScalarAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public int GetLastChapter(string url)
        {
            var query = "Select LastChapter from UpdateRecords";// where Url=@url";
            var comm = new SqlCommand(query, _connection);
            //comm.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
            comm.Prepare();
            SqlDataReader reader = comm.ExecuteReader();
            reader.Read();
            return reader.GetInt32(0);
        }
    }
}
