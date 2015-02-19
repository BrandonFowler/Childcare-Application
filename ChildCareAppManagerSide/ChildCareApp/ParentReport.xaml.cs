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
using MySql.Data.MySqlClient;
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
            MySqlConnection connection = new MySqlConnection("Server=146.187.135.22;Uid=ccdev;Pwd=devpw821;Database=childcare_v4;");

            try {
                connection.Open();
                string query = "SELECT * FROM AllowedConnections NATURAL JOIN Child";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable("Parent Report");
                adapter.Fill(table);
                ParentDataGrid.ItemsSource = table.DefaultView;
                adapter.Update(table);

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
