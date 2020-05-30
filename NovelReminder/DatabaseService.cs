using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace NovelReminder
{

    class DatabaseService
    {
        private SqlConnection _connection;
        public DatabaseService()
        {
            var connstring= GetConnectionStrings("Database");
            try
            {
                _connection = new SqlConnection(connstring);
                _connection.Open();
                SqlCommand createtable = new SqlCommand(
                    "IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'UpdateRecords') AND type in (N'U'))" +
                    "Create Table UpdateRecords" +
                    "(Url varchar(30) Primary Key not null," +
                    "LastChapter int Not null," +
                    "IsInit bit Not null)"
                    , _connection);
                createtable.ExecuteNonQuery();
                //Console.WriteLine(version);
            }
            catch (Exception e)
            {
                Console.WriteLine("Dataservice36"+e.Message);
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
                    $" insert into UpdateRecords(Url,LastChapter,IsInit) values(@url,@lastUpdate,@flag)";
                SqlCommand insert = new SqlCommand(query, _connection);
                insert.Parameters.Add("@url", SqlDbType.VarChar,255).Value = url;
                insert.Parameters.Add("@lastUpdate", SqlDbType.Int).Value = lastUpdate;
                insert.Parameters.Add("@flag", SqlDbType.Bit).Value = false;
                await insert.PrepareAsync();
                await insert.ExecuteScalarAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine("Dataservice60"+e.Message);
            }
        }
        public async ValueTask UpdateAsync(string url,int lastUpdate)
        {
            try
            {
                var update = "update UpdateRecords set LastChapter = @lastUpdate where Url = @url";
                SqlCommand insert = new SqlCommand(update, _connection);
                insert.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
                insert.Parameters.Add("@lastUpdate", SqlDbType.Int).Value = lastUpdate;
                await insert.PrepareAsync();
                await insert.ExecuteScalarAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public async ValueTask UpdateAsync(string url,bool isinit)
        {
            try
            {
                var update = "update UpdateRecords set IsInit=@isinit where Url = @url";
                SqlCommand insert = new SqlCommand(update, _connection);
                insert.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
                insert.Parameters.Add("@isinit", SqlDbType.Bit).Value = isinit;
                await insert.PrepareAsync();
                await insert.ExecuteScalarAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public async ValueTask<bool> GetIsInit(string url)
        {
            var query = "Select IsInit from UpdateRecords where Url=@url";

            var comm = new SqlCommand(query, _connection);
            comm.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
            comm.Prepare();
            SqlDataReader reader =await comm.ExecuteReaderAsync();
            await reader.ReadAsync();
            return reader.GetBoolean(2);
        }
        public async ValueTask<int> GetLastChapterAsync(string url)
        {
            var query = "Select LastChapter from UpdateRecords";// where Url=@url";
            var comm = new SqlCommand(query, _connection);
            //comm.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
            comm.Prepare();
            SqlDataReader reader =await comm.ExecuteReaderAsync();
            await reader.ReadAsync();
            return reader.GetInt32(1);
        }
        private static string GetConnectionStrings(string databaseName)
        {
            ConnectionStringSettingsCollection settings =
                ConfigurationManager.ConnectionStrings;

            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.Name == databaseName)
                    {
                        return cs.ConnectionString;
                    }
                }
            }
            throw new Exception("Haven't find the database!");
        }
    }
}
