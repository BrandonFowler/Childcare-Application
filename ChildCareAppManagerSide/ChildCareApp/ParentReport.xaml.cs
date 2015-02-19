using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;

namespace ChildCareApp {
    /// <summary>
    /// Interaction logic for ParentReport.xaml
    /// </summary>
    public partial class ParentReport : Window {
        public ParentReport() {
            InitializeComponent();
        }

        private void LoadReport(int parentID, int startDate, int endDate) {
           
        }

        private void btn_LoadAll_Click(object sender, RoutedEventArgs e) {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../ChildCare_v3.s3db;Version=3;");

            try {
                connection.Open();//Date, FirstName, LastName, TransactionTotal
                string query = "SELECT * FROM AllowedConnections NATURAL JOIN Child";//(Guardian NATURAL JOIN AllowedConnections)";// NATURAL JOIN Child";// NATURAL JOIN ChildCareTransaction";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable table = new DataTable("Parent Report");
                adapter.Fill(table);
                ParentDataGrid.ItemsSource = table.DefaultView;
                adapter.Update(table);

                connection.Close();
            } catch (Exception except) {
                MessageBox.Show(except.Message);
            }
        }
    }
}
