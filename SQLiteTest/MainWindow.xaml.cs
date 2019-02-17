using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;

namespace SQLiteTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Logic connectedDb;
        DataGrid dg1 = new DataGrid();

        public MainWindow()
        {
            InitializeComponent();
        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateDbButton_Click(object sender, RoutedEventArgs e)
        {
            Logic createDB = new Logic("test.sqlite");
            MessageBox.Show(createDB.CreateDataBase());
        }

        private void ConnectToDb_Click(object sender, RoutedEventArgs e)
        {
            string dbPath = "test.sqlite";
            if (!File.Exists(dbPath))
            {
                MessageBox.Show($"Базы данных {dbPath} не существует");
            }
            else
            {
                connectedDb = new Logic(dbPath);
                bool connected = connectedDb.ConnectToDb();
                if(connected)
                {
                    StatusPanel.Text = "Соединение с БД установлено, можно работать";
                }
            }

        }

        private void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            if(connectedDb!=null)
            {
                //List<GridBind> catalog = connectedDb.GetAll();
                //MainDataGrid.ItemsSource = catalog;
              DataTable dt = connectedDb.GetDT();


              var dataList = dt.Select();
                MainDataGrid.ItemsSource = dataList;

                // dg1.ItemsSource = catalog;
               // MainField.Children.Add(dg1);
            }
        }

        private void AddRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (connectedDb.AddRecord())
                MessageBox.Show("Тестовые записи добавлены");
        }
    }
}
