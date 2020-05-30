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
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                string str = "IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'UpdateRecords') AND type in (N'U'))" +
                    "Create Table UpdateRecords" +
                    "(Url varchar(30) Primary Key not null," +
                    "LastChapter int Not null," +
                    "IsInit bit Not null)";
                using (SqlCommand createtable = new SqlCommand(str,_connection))
                {
                    createtable.ExecuteNonQuery();
                    _connection.Close();
                }
                //Console.WriteLine(version);
            }
            catch (Exception e)
            {
                Console.WriteLine("Constructor:  "+e.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }

        public async ValueTask InsertOrUpdateOneAsync(string url,int lastUpdate)
        {
            try
            {
                var query = $"IF EXISTS(select * from UpdateRecords where Url=@url) " +
                    $"update UpdateRecords set LastChapter = @lastUpdate where Url=@url " +
                    "ELSE" +
                    $" insert into UpdateRecords(Url,LastChapter,IsInit) values(@url,@lastUpdate,@flag)";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                using (SqlCommand insert = new SqlCommand(query, _connection))
                {
                    insert.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
                    insert.Parameters.Add("@lastUpdate", SqlDbType.Int).Value = lastUpdate;
                    insert.Parameters.Add("@flag", SqlDbType.Bit).Value = false;
                    await insert.PrepareAsync();
                    await insert.ExecuteScalarAsync();
                }
                _connection.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("InsertOrUpdateOneAsync:  "+e.Message);
            }
            finally
            {
                if(_connection.State==ConnectionState.Open)
                _connection.Close();
            }
        }
        public async ValueTask UpdateAsync(string url,int lastUpdate)
        {
            try
            {
                var update = "update UpdateRecords set LastChapter = @lastUpdate where Url = @url";
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                using (SqlCommand insert = new SqlCommand(update, _connection))
                {
                    insert.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
                    insert.Parameters.Add("@lastUpdate", SqlDbType.Int).Value = lastUpdate;
                    await insert.PrepareAsync();
                    await insert.ExecuteScalarAsync();

                }
                _connection.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateAsync:  "+e.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
        public async ValueTask UpdateAsync(string url,bool isinit)
        {
            try
            {
                var update = "update UpdateRecords set IsInit=@isinit where Url = @url";
                if(_connection.State == ConnectionState.Closed)
                _connection.Open();
                using (SqlCommand insert = new SqlCommand(update, _connection))
                {
                    insert.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
                    insert.Parameters.Add("@isinit", SqlDbType.Bit).Value = isinit;
                    await insert.PrepareAsync();
                    await insert.ExecuteScalarAsync();
                }
                _connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("UpdateAsync:  " + e.Message);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }
        }
        public async ValueTask<bool> GetIsInit(string url)
        {
            var query = "Select IsInit from UpdateRecords where Url=@url";
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            using (var comm = new SqlCommand(query, _connection))
            {
                comm.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
                comm.Prepare();
                SqlDataReader reader = await comm.ExecuteReaderAsync();
                await reader.ReadAsync();
                var result = reader.GetBoolean(0);
                _connection.Close();
                return result;
            }

        }
        public async ValueTask<int> GetLastChapterAsync(string url)
        {
            var query = "Select LastChapter from UpdateRecords where Url=@url";
            using (var comm = new SqlCommand(query, _connection))
            {
                _connection.Open();
                comm.Parameters.Add("@url", SqlDbType.VarChar, 255).Value = url;
                comm.Prepare();
                SqlDataReader reader = await comm.ExecuteReaderAsync();
                await reader.ReadAsync();
                var result = reader.GetInt32(0);
                _connection.Close();
    
                return result;
            }
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
