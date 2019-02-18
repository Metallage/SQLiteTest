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
        private SQLiteDataAdapter adapter;
        private DataSet ds;

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


        public DataTable GetDT()
        {
            DataTable result = new DataTable();

            if (dbConnection.State == ConnectionState.Open)
            {
                dbCommand.CommandText = "SELECT author, book FROM Catalog";
               
                result.Load(dbCommand.ExecuteReader());

            }

            return result;
        }

        public DataTable GetWithDS()
        {
            DataTable result = new DataTable();
            adapter = new SQLiteDataAdapter("SELECT * FROM Catalog ",dbConnection);
            ds = new DataSet();
            adapter.FillSchema(ds, SchemaType.Source, "Catalog");
            adapter.Fill(ds, "Catalog");
            result = ds.Tables["Catalog"];
            return result;
        }

        public bool AddRecord()
        {
            bool success = false;
            try
            {
                dbCommand.CommandText = "BEGIN TRANSACTION";
                dbCommand.ExecuteNonQuery();

                for(int i=1; i<=10; i++)
                {
                    dbCommand.CommandText = $"INSERT INTO Catalog (author, book) VALUES ('test author {i}','test book {i}')";
                    dbCommand.ExecuteNonQuery();
                }

                dbCommand.CommandText = "COMMIT TRANSACTION;";
                dbCommand.ExecuteNonQuery();

                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}
