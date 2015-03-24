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
    public partial class EditEvents : Window {
        public EditEvents() {
            InitializeComponent();
            LoadEvents();
            FillComboBox();
        }

        private void LoadEvents() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "SELECT * FROM EventData;";

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataTable table = new DataTable("Event Info");
                adapter.Fill(table);
                EventViewDataGrid.ItemsSource = table.DefaultView;
                //adapter.Update(table);

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void FillComboBox() {
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");
            String query = "SELECT Event_ID FROM EventData;";

            try {
                connection.Open();
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    String eventID = reader.GetString(0);
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
            if (cmd_EventIDCombo.SelectedIndex != -1) {
                String test = ((ComboBoxItem)cmd_EventIDCombo.SelectedItem).Content.ToString();
                EventModificationWindow win = new EventModificationWindow(test);
                win.Show();
                this.Close();
            } else {
                MessageBox.Show("You must select an event ID from the drop down box.");
            }
        }

        private void btn_AddEvent_Click(object sender, RoutedEventArgs e) {
            EventModificationWindow win = new EventModificationWindow();
            win.Show();
            this.Close();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
