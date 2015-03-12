//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace AdminTools {
    /// <summary>
    /// Interaction logic for win_EditEvents.xaml
    /// </summary>
    public partial class EditEvents : Window {
        public EditEvents() {
            InitializeComponent();
            LoadEvents();
            FillComboBox();
        }

        private void LoadEvents() {
            MySqlConnection connection = new MySqlConnection("Server=146.187.135.22;Uid=ccdev;Pwd=devpw821;Database=childcare_v4;");
            String query = "SELECT * FROM EventData;";

            try {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable("Parent Report");
                adapter.Fill(table);
                EventViewDataGrid.ItemsSource = table.DefaultView;
                adapter.Update(table);

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void FillComboBox() {
            MySqlConnection connection = new MySqlConnection("Server=146.187.135.22;Uid=ccdev;Pwd=devpw821;Database=childcare_v4;");
            String query = "SELECT Event_ID FROM EventData;";

            try {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    int eventID = reader.GetInt32("Event_ID");
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = eventID;
                    cmd_EventIDCombo.Items.Add(item);
                }
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void btn_EditEvent_Click(object sender, RoutedEventArgs e) {
            win_EventModificationWindow win = new win_EventModificationWindow(((ComboBoxItem)cmd_EventIDCombo.SelectedItem).Content.ToString());
            win.Show();
            this.Close();
        }

        private void btn_AddEvent_Click(object sender, RoutedEventArgs e) {
            win_EventModificationWindow win = new win_EventModificationWindow();
            win.Show();
            this.Close();
        }
    }
}
