using ChildcareApplication.AdminTools;
using DatabaseController;
using System;
using System.Collections.Generic;
using System.Data;
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
using MessageBoxUtils;

namespace AdminTools {
    public partial class EventModificationWindow : Window {
        private bool valueChanged, maxHoursChanged;
        String oldEventName;

        public EventModificationWindow() {
            InitializeComponent();
            valueChanged = false;
            maxHoursChanged = false;
            this.MouseDown += WindowMouseDown;
        }

        public EventModificationWindow(String oldEventName) {
            InitializeComponent();
            LoadData(oldEventName);
            valueChanged = false;
            maxHoursChanged = false;
            if (IsProtected(oldEventName)) {
                ProtectEvents(oldEventName);
            }
            this.oldEventName = oldEventName;
            this.MouseDown += WindowMouseDown;
        }

        private void ProtectEvents(string oldEventName) {
            this.txt_EventName.IsEnabled = false;
            this.cmb_Occurence.IsEnabled = false;
            this.cmb_PriceType.IsEnabled = false;
            this.lbl_EventName.Content = "Event Name (Protected Event)";
            if (oldEventName == "Late Fee") {
                this.txt_MaxHours.IsEnabled = false;
            }
        }

        private bool IsProtected(string eventName) {
            return (eventName == "Regular Childcare" || eventName == "Infant Childcare" || eventName == "Adolescent Childcare" || eventName == "Late Fee");
        }

        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            if (this.valueChanged) {
                ProcessModification();
            } else {
                WPFMessageBox.Show("You have not changed any values!  If you would like to return to the previous window without making changes, please hit cancel.");
            }
        }

        private void LoadData(String eventName) {
            EventDB eventDB = new EventDB();
            string[] eventData = eventDB.GetEvent(eventName);

            txt_EventName.Text = eventData[0];

            SetPriceCombo(eventData);
            SetAvailability(eventData);
            SetMaxHours(eventData);
        }

        private void SetPriceCombo(string[] eventData) {
            if (eventData[1] != "") { //hourly price
                cmb_PriceType.SelectedIndex = 0;
                txt_Rate.Text = eventData[1];
                if (eventData[2] != "") {
                    txt_DiscountPrice.Text = eventData[2];
                }
            } else if (eventData[3] != "") { //daily price
                cmb_PriceType.SelectedIndex = 1;
                txt_Rate.Text = eventData[3];
                if (eventData[4] != "") {
                    txt_DiscountPrice.Text = eventData[4];
                }
            }
        }

        private void SetAvailability(string[] eventData) {
            if (eventData[5] != "" && eventData[6] != "") { //specific day
                cmb_Occurence.SelectedIndex = 1;
                txt_MonthNum.Text = eventData[5];
                txt_DayOfMonth.Text = eventData[6];
                lbl_DayNum.Visibility = Visibility.Visible;
                lbl_MonthNum.Visibility = Visibility.Visible;
                txt_DayOfMonth.Visibility = Visibility.Visible;
                txt_MonthNum.Visibility = Visibility.Visible;

                lbl_DayName.Visibility = Visibility.Hidden;
                cmb_DayName.Visibility = Visibility.Hidden;
            } else if (eventData[7] != "") { //weekday
                String dayName = eventData[7];
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

        private void SetMaxHours(string[] eventInfo) {
            if (eventInfo[8] != null) {
                txt_MaxHours.Text = eventInfo[8];
            }
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            EditEvents win = new EditEvents();
            win.Show();
            this.Close();
        }

        private void DataChanged_Event(object sender, TextChangedEventArgs e) {
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

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e) {
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
                if (this.maxHoursChanged) {
                    EventDB eventDB = new EventDB();
                    if (txt_MaxHours.Text != "") {
                        eventDB.UpdateMaxHours(txt_EventName.Text, "'" + txt_MaxHours.Text + "'");
                    } else {
                        eventDB.UpdateMaxHours(txt_EventName.Text, "null");
                    }
                }
                CloseWindow();
            }
        }

        private bool FormDataValid() {
            if (!EventNameValid(txt_EventName.Text)) {
                txt_EventName.Focus();
                return false;
            }
            if (cmb_PriceType.SelectedIndex == -1) {
                WPFMessageBox.Show("You must select a price type from the drop down menu.");
                cmb_PriceType.Focus();
                return false;
            }
            if (!ValidDoubleGreaterThanZero(txt_Rate.Text)) {
                WPFMessageBox.Show("You must enter a valid dollar figure greater than zero in the Rate box.");
                txt_Rate.Focus();
                return false;
            }
            if (txt_DiscountPrice.Text != "" && !ValidDoubleGreaterThanZero(txt_DiscountPrice.Text)) {
                WPFMessageBox.Show("You must enter a valid dollar figure greater than zero in the Discount Price box.");
                txt_DiscountPrice.Focus();
                return false;
            }
            if (cmb_Occurence.SelectedIndex == -1) {
                WPFMessageBox.Show("You must select an event occurence from the drop down menu.");
                cmb_Occurence.Focus();
                return false;
            }
            if (cmb_Occurence.SelectedIndex == 1) {
                if (!DateValid()) {
                    return false;
                }
            }
            if (cmb_Occurence.SelectedIndex == 2 && cmb_DayName.SelectedIndex == -1) {
                WPFMessageBox.Show("You must select a valid day of the week from the drop down menu.");
                cmb_Occurence.Focus();
                return false;
            }
            if (txt_MaxHours.Text != "" && !ValidDoubleGreaterThanZero(txt_MaxHours.Text)) {
                WPFMessageBox.Show("You must enter a valid number greater than zero in the maximum hours text box.");
                txt_MaxHours.Focus();
                return false;
            }
            return true;
        }

        private bool EventNameValid(string eventName) {
            if (eventName.Length < 1) {
                WPFMessageBox.Show("You must enter an event name.");
                return false;
            }
            if (!RegExpressions.RegexValidateEventName(eventName)) {
                WPFMessageBox.Show("Event names may only contain letters and spaces.");
                return false;
            }
            return true;
        }

        private bool ValidDoubleGreaterThanZero(string rate) {
            double temp;
            if (!Double.TryParse(rate, out temp)) {
                return false;
            }
            if (temp < 0) {
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
            EventDB db = new EventDB();
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
            EventDB db = new EventDB();
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
            EventDB db = new EventDB();
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
            EventDB db = new EventDB();
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

        private void txt_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)((TextBox)sender).SelectAll); //idea found at: http://stackoverflow.com/questions/97459/automatically-select-all-text-on-focus-in-winforms-textbox
        }

        private bool DateValid() {
            int dayNum = 0;
            int monthNum = 0;
            if (!(Int32.TryParse(txt_DayOfMonth.Text, out dayNum))) { 
                WPFMessageBox.Show("You must enter a valid number in the Day of Month box.");
                txt_DayOfMonth.Focus();
                return false;
            }
            if (!(Int32.TryParse(txt_MonthNum.Text, out monthNum) && monthNum > 0 && monthNum < 13)) {
                WPFMessageBox.Show("You must enter a valid number in the Month number box.");
                txt_MonthNum.Focus();
                return false;
            }
            GregorianCalendar cal = new GregorianCalendar();
            if(dayNum <= cal.GetDaysInMonth(DateTime.Now.Year, monthNum) && dayNum > 0) {
                return true;
            } else {
                WPFMessageBox.Show("You must enter a valid month number and day number in the month number and day number boxes!");
                txt_DayOfMonth.Focus();
            }
            return false;
        }

        private void txt_MaxHours_TextChanged(object sender, TextChangedEventArgs e) {
            this.valueChanged = true;
            this.maxHoursChanged = true;
        }

        private void KeyUp_Event(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next); //Found at: http://stackoverflow.com/questions/23008670/wpf-and-mvvm-how-to-move-focus-to-the-next-control-automatically
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }

        private void cmb_PriceType_KeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                txt_Rate.Focus();
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
