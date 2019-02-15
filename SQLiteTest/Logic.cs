using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace SQLiteTest
{
    class Logic
    {
        private string dbPath;
        private SQLiteConnection dbConnection;
        private SQLiteCommand dbCommand;

        public Logic(string dbPath)
        {
            this.dbPath = dbPath;
            dbCommand = new SQLiteCommand();
        }

        #region Создание БД
        public string CreateDataBase()
        {
            string result = "не понятно";
            try
            {
                //Создаём файл БД
                if (!File.Exists(dbPath))
                {
                    SQLiteConnection.CreateFile(dbPath);
                }

                //Устанавливаем соединение с файлом БД
                dbConnection = new SQLiteConnection($"DataSource={dbPath};Version=3;");
                

                dbConnection.Open();

                //Содготавливаемся к выполнению SQL команды
                dbCommand.Connection = dbConnection;
                dbCommand.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (id INTEGER PRIMARY KEY AUTOINCREMENT, author TEXT, book TEXT)";
                dbCommand.ExecuteNonQuery(); //Выполняем SQL команду

                result = "База данных успешно создана";
            }
            catch(Exception e)
            {
                result = $"Ошибка: {e.Message}";
            }

            return result;
        }
        #endregion

        #region Соединяемся с БД

        public bool ConnectToDb()
        {
            bool connected = false;
            try
            {
                dbConnection = new SQLiteConnection($"DataSource={dbPath};Version=3;");
                dbConnection.Open();
                dbCommand.Connection = dbConnection;

                connected = true;
            }
            catch
            {
                connected = false;
            }

            return connected;
        }

        #endregion

        public List<GridBind> GetAll()
        {
            List<GridBind> result = new List<GridBind>();

            if(dbConnection.State == ConnectionState.Open)
            {
                dbCommand.CommandText = "SELECT author, book FROM Catalog";
                DataTable dt = new DataTable();
                dt.Load(dbCommand.ExecuteReader());
                foreach ( DataRow dr in dt.Rows)
                {
                    result.Add(new GridBind(dr.Field<string>("author"), dr.Field<string>("book")));
                }
            }

            return result;
        }
    }
}
