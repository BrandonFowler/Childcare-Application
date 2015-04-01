using ChildcareApplication.AdminTools;
using DatabaseController;
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
        String oldEventName;

        public EventModificationWindow() {
            InitializeComponent();
            valueChanged = false;
        }

        public EventModificationWindow(String oldEventName) {
            InitializeComponent();
            LoadData(oldEventName);
            valueChanged = false;
            this.oldEventName = oldEventName;
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            if (this.valueChanged) {
                ProcessModification();
            } else {
                MessageBox.Show("You have not changed any values!  If you would like to return to the previous window without making changes, please hit cancel.");
            }
        }

        private void LoadData(String eventName) { 
            SQLiteConnection connection = new SQLiteConnection("Data Source=../../Database/Childcare_v5.s3db;Version=3;");

            try {
                connection.Open();
                String query = "SELECT * FROM EventData WHERE EventName = '" + eventName + "';";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                txt_EventName.Text = reader.GetString(0);

                SetPriceCombo(reader);
                SetAvailability(reader);

                reader.Close();
                connection.Close();
            } catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void SetPriceCombo(SQLiteDataReader reader) {
            if (!reader.IsDBNull(1)) { //hourly price
                cmb_PriceType.SelectedIndex = 0;
                txt_Rate.Text = "" + reader.GetDouble(1);
                if (!reader.IsDBNull(2)) {
                    txt_DiscountPrice.Text = "" + reader.GetDouble(2);
                }
            } else if (!reader.IsDBNull(3)) { //daily price
                cmb_PriceType.SelectedIndex = 1;
                txt_Rate.Text = "" + reader.GetDouble(3);
                if (!reader.IsDBNull(4)) {
                    txt_DiscountPrice.Text = "" + reader.GetDouble(4);
                }
            }
        }

        private void SetAvailability(SQLiteDataReader reader) {
            if (!reader.IsDBNull(5) && !reader.IsDBNull(6)) { //specific day
                cmb_Occurence.SelectedIndex = 1;
                txt_MonthNum.Text = "" + reader.GetInt32(5);
                txt_DayOfMonth.Text = "" + reader.GetInt32(6);
                lbl_DayNum.Visibility = Visibility.Visible;
                lbl_MonthNum.Visibility = Visibility.Visible;
                txt_DayOfMonth.Visibility = Visibility.Visible;
                txt_MonthNum.Visibility = Visibility.Visible;

                lbl_DayName.Visibility = Visibility.Hidden;
                cmb_DayName.Visibility = Visibility.Hidden;
            } else if (!reader.IsDBNull(7)) { //weekday
                String dayName = reader.GetString(7);
                cmb_Occurence.SelectedIndex = 2;
                //show day name
                lbl_DayName.Visibility = Visibility.Visible;
                cmb_DayName.Visibility = Visibility.Visible;
                cmb_DayName.SelectedIndex = GetDayIndex(dayName);

                lbl_DayNum.Visibility = Visibility.Hidden;
                lbl_MonthNum.Visibility = Visibility.Hidden;
                txt_DayOfMonth.Visibility = Visibility.Hidden;
                txt_MonthNum.Visibility = Visibility.Hidden;
            } else { //always available
                cmb_Occurence.SelectedIndex = 0;
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
            if (FormDataValid()) {
                if (this.oldEventName == null) {
                    if (txt_DiscountPrice.Text != "") {
                        AddEventDiscount();
                    } else {
                        AddEventNoDiscount();
                    }
                } else if(txt_DiscountPrice.Text != "") {
                    EditEventDiscount();
                } else {
                    EditEventNoDiscount();
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
            if (txt_DiscountPrice.Text != "" && !Double.TryParse(txt_DiscountPrice.Text, out temp)) {
                MessageBox.Show("You must enter a valid dollar figure in the Discount Rate box.");
                return false;
            }
            if (cmb_Occurence.SelectedIndex == -1) {
                MessageBox.Show("You must select an event occurence from the drop down menu.");
                return false;
            }
            if (cmb_Occurence.SelectedIndex == 1) {
                if (!DateValid()) {
                    return false;
                }
            }
            if (cmb_Occurence.SelectedIndex == 2 && cmb_DayName.SelectedIndex == -1) {
                MessageBox.Show("You must select a valid day of the week from the drop down menu.");
                return false;
            }
            return true;
        }

        private int GetDayIndex(String eventDay) {
            if (eventDay == "Sunday") {
                return 0;
            } else if (eventDay == "Monday") {
                return 1;
            } else if (eventDay == "Tuesday") {
                return 2;
            } else if (eventDay == "Wednesday") {
                return 3;
            } else if (eventDay == "Thursday") {
                return 4;
            } else if(eventDay == "Friday") {
                return 5;
            } else {
                return 6;
            }
        }

        private void AddEventDiscount() {
            EventModificationDB db = new EventModificationDB();
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
        }

        private void AddEventNoDiscount() {
            EventModificationDB db = new EventModificationDB();
            if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0) {
                db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text));
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0) {
                db.DailyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text));
            } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1) {
                db.HourlyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text));
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1) {
                db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text));
            } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2) {
                db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString());
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2) {
                db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString());
            }
        }

        private void EditEventDiscount() {
            EventModificationDB db = new EventModificationDB();
            if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0) {
                db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0) {
                db.DailyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1) {
                db.HourlyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1) {
                db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2) {
                db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2) {
                db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToDouble(txt_DiscountPrice.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.oldEventName);
            }
        }

        private void EditEventNoDiscount() {
            EventModificationDB db = new EventModificationDB();
            if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 0) {
                db.HourlyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 0) {
                db.DailyPriceAlwaysAvailable(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 1) {
                db.HourlyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 1) {
                db.DailyPriceSpecificDay(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), Convert.ToInt32(txt_MonthNum.Text), Convert.ToInt32(txt_DayOfMonth.Text), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 0 && cmb_Occurence.SelectedIndex == 2) {
                db.HourlyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.oldEventName);
            } else if (cmb_PriceType.SelectedIndex == 1 && cmb_Occurence.SelectedIndex == 2) {
                db.DailyPriceWeeklyOcur(txt_EventName.Text, Convert.ToDouble(txt_Rate.Text), ((ComboBoxItem)cmb_DayName.SelectedItem).Content.ToString(), this.oldEventName);
            }
        }

        private void CloseWindow() {
            EditEvents win = new EditEvents();
            win.Show();
            this.Close();
        }

        private void cmb_DayName_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void txt_DiscountPrice_TextChanged(object sender, TextChangedEventArgs e) {
            this.valueChanged = true;
        }

        private void txt_EventName_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_EventName.SelectAll); //idea found at: http://stackoverflow.com/questions/97459/automatically-select-all-text-on-focus-in-winforms-textbox
        }

        private void txt_Rate_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_Rate.SelectAll);
        }

        private void txt_DiscountPrice_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_DiscountPrice.SelectAll);
        }

        private void txt_MonthNum_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_MonthNum.SelectAll);
        }

        private void txt_DayOfMonth_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)txt_DayOfMonth.SelectAll);
        }

        private bool DateValid() {
            int dayNum = 0;
            int monthNum = 0;
            if (!(Int32.TryParse(txt_DayOfMonth.Text, out dayNum))) { 
                MessageBox.Show("You must enter a number in the Day of Month box.");
                return false;
            }
            if (!(Int32.TryParse(txt_MonthNum.Text, out monthNum) && monthNum > 0 && monthNum < 13)) {
                MessageBox.Show("You must enter a number in the Month number box.");
                return false;
            }
            GregorianCalendar cal = new GregorianCalendar();
            if(dayNum <= cal.GetDaysInMonth(DateTime.Now.Year, monthNum) && dayNum > 0) {
                return true;
            } else {
                MessageBox.Show("You must enter a valid month number and day number in the month number and day number boxes!");
            }
            return false;
        }
    }
}
