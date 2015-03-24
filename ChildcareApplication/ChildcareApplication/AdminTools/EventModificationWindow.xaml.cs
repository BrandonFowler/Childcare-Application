using ChildcareApplication.AdminTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
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
    public partial class EventModificationWindow : Window {
        private bool valueChanged;
        String eventID;

        public EventModificationWindow() {
            InitializeComponent();
            valueChanged = false;
        }

        public EventModificationWindow(String eventID) {
            InitializeComponent();
            LoadData(eventID);
            valueChanged = false;
            this.eventID = eventID;
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            if (this.valueChanged) {
                ProcessModification();
            }
        }

        private void LoadData(String eventID) { //TODO: refactor
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");

            try {
                connection.Open();
                String query = "SELECT * FROM EventData WHERE Event_ID = '" + eventID + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                txt_EventName.Text = reader.GetString(1);
                
                if (!reader.IsDBNull(2)) {
                    cmb_PriceType.SelectedIndex = 0;
                    txt_Rate.Text = "" + reader.GetDouble(2);
                    if (!reader.IsDBNull(3)) {
                        txt_DiscountPrice.Text = "" + reader.GetDouble(3);
                    }
                } else if(!reader.IsDBNull(4)) {
                    cmb_PriceType.SelectedIndex = 1;
                    txt_Rate.Text = "" + reader.GetDouble(4);
                    if (!reader.IsDBNull(5)) {
                        txt_DiscountPrice.Text = "" + reader.GetDouble(5);
                    }
                }

                if (!reader.IsDBNull(6) && !reader.IsDBNull(7)) {
                    cmb_Occurence.SelectedIndex = 1;
                    txt_MonthNum.Text = "" + reader.GetInt32(6);
                    txt_DayOfMonth.Text = "" + reader.GetInt32(7);
                    lbl_DayNum.Visibility = Visibility.Visible;
                    lbl_MonthNum.Visibility = Visibility.Visible;
                    txt_DayOfMonth.Visibility = Visibility.Visible;
                    txt_MonthNum.Visibility = Visibility.Visible;

                    lbl_DayName.Visibility = Visibility.Hidden;
                    cmb_DayName.Visibility = Visibility.Hidden;
                } else if (!reader.IsDBNull(8)) {
                    String dayName = reader.GetString(8);
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

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            EditEvents win = new EditEvents();
            win.Show();
            this.Close();
        }

        private void txt_EventName_TextChanged(object sender, TextChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void cmb_PriceType_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void txt_Rate_TextChanged(object sender, TextChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void cmb_Occurence_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.valueChanged = true;
            if (cmb_Occurence.SelectedIndex != -1 && ((ComboBoxItem)cmb_Occurence.SelectedItem).Content.ToString() == "Specific Day") {
                lbl_DayNum.Visibility = Visibility.Visible;
                lbl_MonthNum.Visibility = Visibility.Visible;
                txt_DayOfMonth.Visibility = Visibility.Visible;
                txt_MonthNum.Visibility = Visibility.Visible;

                lbl_DayName.Visibility = Visibility.Hidden;
                cmb_DayName.Visibility = Visibility.Hidden;
            } else if (cmb_Occurence.SelectedIndex != -1 && ((ComboBoxItem)cmb_Occurence.SelectedItem).Content.ToString() == "Weekly") {
                lbl_DayName.Visibility = Visibility.Visible;
                cmb_DayName.Visibility = Visibility.Visible;

                lbl_DayNum.Visibility = Visibility.Hidden;
                lbl_MonthNum.Visibility = Visibility.Hidden;
                txt_DayOfMonth.Visibility = Visibility.Hidden;
                txt_MonthNum.Visibility = Visibility.Hidden;
            } else if (cmb_Occurence.SelectedIndex != -1 && ((ComboBoxItem)cmb_Occurence.SelectedItem).Content.ToString() == "Always Available") {
                lbl_DayName.Visibility = Visibility.Hidden;
                cmb_DayName.Visibility = Visibility.Hidden;

                lbl_DayNum.Visibility = Visibility.Hidden;
                lbl_MonthNum.Visibility = Visibility.Hidden;
                txt_DayOfMonth.Visibility = Visibility.Hidden;
                txt_MonthNum.Visibility = Visibility.Hidden;
            }
        }

        private void txt_DayOfMonth_TextChanged(object sender, TextChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void txt_MonthNum_TextChanged(object sender, TextChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void ProcessModification() {
            EventModificationDB db = new EventModificationDB();
            if (FormDataValid()) {
                if (this.eventID == null) {
                    if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0) {
                        db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text));
                    } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0) {
                        db.DailyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text));
                    } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1) {
                        db.HourlyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text));
                    } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1) {
                        db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text));
                    } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2) {
                        db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString());
                    } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2) {
                        db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString());
                    }
                } else {
                    if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0) {
                        db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), this.eventID);
                    } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0) {
                        db.DailyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), this.eventID);
                    } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1) {
                        db.HourlyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.eventID);
                    } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1) {
                        db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.eventID);
                    } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2) {
                        db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.eventID);
                    } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2) {
                        db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.eventID);
                    }
                }

                CloseWindow();
            }
        }

        private bool FormDataValid() {
            if (txt_EventName.Text.Length < 1) {
                MessageBox.Show("You must enter an event name.");
                return false;
            }
            if (cmb_PriceType.SelectedIndex == -1) {
                MessageBox.Show("You must select a price type from the drop down menu.");
                return false;
            }
            double temp;
            if (!Double.TryParse(txt_Rate.Text, out temp)) {
                MessageBox.Show("You must enter a valid dollar figure in the Rate box.");
                return false;
            }
            if (!Double.TryParse(txt_DiscountPrice.Text, out temp)) {
                MessageBox.Show("You must enter a valid dollar figure in the Discount Rate box.");
                return false;
            }
            if (cmb_Occurence.SelectedIndex == -1) {
                MessageBox.Show("You must select an event occurence from the drop down menu.");
                return false;
            }
            if (cmb_Occurence.SelectedIndex == 1) {
                double dayNum = 0;
                double monthNum = 0;
                if (!(Double.TryParse(txt_DayOfMonth.Text, out dayNum) && dayNum > 0 && dayNum < 29)) { //TODO: fix somehow
                    MessageBox.Show("You must enter a valid day number in the Day of Month box.");
                    return false;
                }
                if (!(Double.TryParse(txt_MonthNum.Text, out monthNum) && monthNum > 0 && monthNum < 13)) {
                    MessageBox.Show("You must enter a valid month number in the Month number box.");
                    return false;
                }
            }
            if (cmb_Occurence.SelectedIndex == 2 && cmb_DayName.SelectedIndex == -1) {
                MessageBox.Show("You must select a valid day of the week from the drop down menu.");
                return false;
            }
            return true;
        }

        private void CloseWindow() {
            EditEvents win = new EditEvents();
            win.Show();
            this.Close();
        }
    }
}
