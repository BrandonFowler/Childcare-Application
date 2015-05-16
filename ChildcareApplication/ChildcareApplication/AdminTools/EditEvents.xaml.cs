using DatabaseController;
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
using MessageBoxUtils;

namespace AdminTools {
    public partial class EditEvents : Window {
        public EditEvents() {
            InitializeComponent();
            LoadEvents();
            FillComboBox();
            this.MouseDown += WindowMouseDown;
        }

        private void LoadEvents() {
            EventDB eventDB = new EventDB();
            DataTable table = eventDB.GetEventDisplay();
            EventViewDataGrid.ItemsSource = table.DefaultView;
        }

        private void FillComboBox() {
            EventDB eventDB = new EventDB();
            List<string> namesList = eventDB.GetAllEventNames();

            for(int i = 0; i < namesList.Count; i++) {
                String eventID = namesList[i];
                ComboBoxItem item = new ComboBoxItem();
                item.Content = eventID;
                cmd_EventIDCombo.Items.Add(item);
            }
        }

        private void btn_EditEvent_Click(object sender, RoutedEventArgs e) {
            if (cmd_EventIDCombo.SelectedIndex != -1) {
                String test = ((ComboBoxItem)cmd_EventIDCombo.SelectedItem).Content.ToString();
                EventModificationWindow win = new EventModificationWindow(test);
                win.Show();
                this.Close();
            } else {
                WPFMessageBox.Show("You must select an event name from the drop down box.");
            }
        }

        private void btn_DeleteEvent_Click(object sender, RoutedEventArgs e) {
            if (cmd_EventIDCombo.SelectedIndex != -1) {
                EventDB eventDB = new EventDB();
                String test = ((ComboBoxItem)cmd_EventIDCombo.SelectedItem).Content.ToString();

                eventDB.DeleteEvent(test);
                LoadEvents();
            } else {
                WPFMessageBox.Show("You must select an event name from the drop down box.");
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

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
