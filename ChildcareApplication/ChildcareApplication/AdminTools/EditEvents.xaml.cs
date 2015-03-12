//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
            SQLiteConnection connection = new SQLiteConnection("Server=146.187.135.22;Uid=ccdev;Pwd=devpw821;Database=childcare_v4;");
            String query = "SELECT * FROM EventData;";

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
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
            SQLiteConnection connection = new SQLiteConnection("Server=146.187.135.22;Uid=ccdev;Pwd=devpw821;Database=childcare_v4;");
            String query = "SELECT Event_ID FROM EventData;";

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    int eventID = reader.GetInt32(0);
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
            EventModificationWindow win = new EventModificationWindow(((ComboBoxItem)cmd_EventIDCombo.SelectedItem).Content.ToString());
            win.Show();
            this.Close();
        }

        private void btn_AddEvent_Click(object sender, RoutedEventArgs e) {
            EventModificationWindow win = new EventModificationWindow();
            win.Show();
            this.Close();
        }
    }
}
