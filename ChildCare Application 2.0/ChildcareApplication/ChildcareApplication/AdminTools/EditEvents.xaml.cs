using DatabaseController;
using MessageBoxUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

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

            for (int i = 0; i < namesList.Count; i++) {
                String eventID = namesList[i];
                ComboBoxItem item = new ComboBoxItem();
                item.Content = eventID;
                cmd_EventIDCombo.Items.Add(item);
            }
        }

        private void ReFillComboBox() {
            this.cmd_EventIDCombo.Items.Clear();
            FillComboBox();
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

                if (!IsProtected(test)) {
                    eventDB.DeleteEvent(test);
                    LoadEvents();
                    ReFillComboBox();
                } else {
                    WPFMessageBox.Show("" + test + " is a protected event.  You may not delete it.");
                }
            } else {
                WPFMessageBox.Show("You must select an event name from the drop down box.");
            }
        }

        private bool IsProtected(string eventName) {
            return (eventName == "Regular Childcare" || eventName == "Late Fee" || eventName == "Adolescent Childcare" || eventName == "Infant Childcare");
        }

        private void btn_AddEvent_Click(object sender, RoutedEventArgs e) {
            EventModificationWindow win = new EventModificationWindow();
            win.Show();
            this.Close();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void EventViewDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if (this.EventViewDataGrid.HasItems) {
                DataRowView row = (DataRowView)EventViewDataGrid.SelectedItem;
                string eventName = row.Row[0].ToString();
                this.cmd_EventIDCombo.SelectedIndex = FindCMBIndex(eventName);
            }
        }

        private int FindCMBIndex(string eventName) {
            for (int i = 0; i < this.cmd_EventIDCombo.Items.Count; i++) {
                if (((DataRowView)EventViewDataGrid.Items[i]).Row[0].ToString() == eventName) {
                    return i;
                }
            }
            return -1;
        }
    }
}
