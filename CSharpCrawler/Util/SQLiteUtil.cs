using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace SmartImage.Utils
{
    /// <summary>
    /// SQLite帮助类
    /// </summary>
    public class SQLiteUtil : IDisposable
    {      
        private SQLiteConnection con;
        private bool openFlag = false;

        public SQLiteUtil()
        {
            
        }

        private void OpenConnection(string dbPath)
        {
            string conStr = string.Format($"Data Source={dbPath};Version=3;");            

            try
            {
                con = new SQLiteConnection(conStr);
                con.Open();
              
                if (con.State == ConnectionState.Open)
                    openFlag = true;
                else
                    openFlag = false;
            }
            catch (Exception ex)
            {
                openFlag = false;
                throw new Exception("Open connection failure," + ex.Message);
            }
        }

        private DataTable Query(string sql)
        {
            if (openFlag == false)
                throw new Exception("Database has not open yet.");

            SQLiteDataAdapter sda = new SQLiteDataAdapter(sql, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        private object ExecuteScalar(string sql)
        {
            if (openFlag == false)
                throw new Exception("Database has not open yet.");

            SQLiteCommand cmd = new SQLiteCommand(con);
            cmd.CommandText = sql;
            return cmd.ExecuteScalar();
        }

        private int ExecuteNonQuery(string sql)
        {
            if (openFlag == false)
                throw new Exception("Database has not open yet.");

            SQLiteCommand cmd = new SQLiteCommand(sql);
            cmd.CommandText = sql;
            return cmd.ExecuteNonQuery();
        }

        private int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            if (openFlag == false)
                throw new Exception("Database has not open yet.");

            int result = 0;
            using (SQLiteCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.Connection = con;
                cmd.Parameters.AddRange(parameters);
                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        private void UpdateDataTableToDB(DataTable dt,string sql)
        {
            if (openFlag == false)
                throw new Exception("Database has not open yet.");

            SQLiteDataAdapter da = new SQLiteDataAdapter();
            da.SelectCommand = new SQLiteCommand(sql, con);
            SQLiteCommandBuilder builder = new SQLiteCommandBuilder(da);
            da.Update(dt);
        }

        public void CloseConnection()
        {
            try
            {
                con.Close();
                
            }
            catch
            {
                
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
