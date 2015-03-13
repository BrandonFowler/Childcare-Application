﻿using System;
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
    /// Interaction logic for win_EventModificationWindow.xaml
    /// </summary>
    public partial class EventModificationWindow : Window {

        public EventModificationWindow() {
            InitializeComponent();
        }

        public EventModificationWindow(String eventID) {
            InitializeComponent();
            LoadData(eventID);
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            EditEvents win = new EditEvents();
            win.Show();
            this.Close();
        }

        private void cmb_Occurence_DropDownClosed(object sender, EventArgs e) {
            if (((ComboBoxItem)cmb_Occurence.SelectedItem).Content.ToString() == "Specific Day") {
                lbl_DayNum.Visibility = Visibility.Visible;
                lbl_MonthNum.Visibility = Visibility.Visible;
                txt_DayOfMonth.Visibility = Visibility.Visible;
                txt_MonthNum.Visibility = Visibility.Visible;

                lbl_DayName.Visibility = Visibility.Hidden;
                cmb_DayName.Visibility = Visibility.Hidden;
            } else if (((ComboBoxItem)cmb_Occurence.SelectedItem).Content.ToString() == "Weekly") {
                lbl_DayName.Visibility = Visibility.Visible;
                cmb_DayName.Visibility = Visibility.Visible;

                lbl_DayNum.Visibility = Visibility.Hidden;
                lbl_MonthNum.Visibility = Visibility.Hidden;
                txt_DayOfMonth.Visibility = Visibility.Hidden;
                txt_MonthNum.Visibility = Visibility.Hidden;
            }
        }

        private void LoadData(String eventID) { //TODO: refactor
            SQLiteConnection connection = new SQLiteConnection("Server=146.187.135.22;Uid=ccdev;Pwd=devpw821;Database=childcare_v4;");

            try {
                connection.Open();
                String query = "SELECT * FROM EventData WHERE Event_ID = '" + eventID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                txt_EventName.Text = reader.GetString(1);

                float hourlyPrice = reader.GetFloat(2);
                float dailyPrice = reader.GetFloat(4);
                int month = reader.GetInt32(6);
                int day = reader.GetInt32(7);
                String dayName = reader.GetString(8);

                if (hourlyPrice != null) {
                    cmb_PriceType.SelectedIndex = 0;
                    txt_Rate.Text = "" + hourlyPrice;
                } else {
                    cmb_PriceType.SelectedIndex = 1;
                    txt_Rate.Text = "" + dailyPrice;
                }

                if (month != null) {
                    cmb_Occurence.SelectedIndex = 1;
                    txt_MonthNum.Text = "" + month;
                    txt_DayOfMonth.Text = "" + day;
                    lbl_DayNum.Visibility = Visibility.Visible;
                    lbl_MonthNum.Visibility = Visibility.Visible;
                    txt_DayOfMonth.Visibility = Visibility.Visible;
                    txt_MonthNum.Visibility = Visibility.Visible;

                    lbl_DayName.Visibility = Visibility.Hidden;
                    cmb_DayName.Visibility = Visibility.Hidden;
                } else if (dayName != null) {
                    cmb_Occurence.SelectedIndex = 2;
                    //show day name
                    lbl_DayName.Visibility = Visibility.Visible;
                    cmb_DayName.Visibility = Visibility.Visible;

                    lbl_DayNum.Visibility = Visibility.Hidden;
                    lbl_MonthNum.Visibility = Visibility.Hidden;
                    txt_DayOfMonth.Visibility = Visibility.Hidden;
                    txt_MonthNum.Visibility = Visibility.Hidden;
                } else {
                    cmb_Occurence.SelectedIndex = 0;
                }
                //String result = reader.GetString("FirstName") + " " + reader.GetString("LastName");

                reader.Close();
                //return result;

                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }
    }
}