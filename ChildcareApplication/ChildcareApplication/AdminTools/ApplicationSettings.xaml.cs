using System;
using System.Windows;
using System.Windows.Media;

namespace ChildcareApplication.AdminTools {
    /// <summary>
    /// Interaction logic for ApplicationSettings.xaml
    /// </summary>
    public partial class ApplicationSettings : Window {
        
        private string closed = "00:00:00";
        private bool errorPresent = false;

        public ApplicationSettings() {
            InitializeComponent();

            loadSettings();
        }

        private void loadSettings() {
            txt_BillingDate.Text = Properties.Settings.Default.BillingStartDate.ToString();
            txt_MaxMonthlyFee.Text = Properties.Settings.Default.MaxMonthlyFee.ToString();
            txt_ExpirationDays.Text = Properties.Settings.Default.HoldExpiredRecords.ToString();
            txt_InfantAge.Text = Properties.Settings.Default.InfantMaxAge.ToString();
            txt_RegularAge.Text = Properties.Settings.Default.RegularMaxAge.ToString();

            txt_MonOpening.Text = Properties.Settings.Default.MonOpen.TimeOfDay.ToString();
            txt_MonClosing.Text = Properties.Settings.Default.MonClose.TimeOfDay.ToString();
            txt_TueOpening.Text = Properties.Settings.Default.TueOpen.TimeOfDay.ToString();
            txt_TueClosing.Text = Properties.Settings.Default.TueClose.TimeOfDay.ToString();
            txt_WedOpening.Text = Properties.Settings.Default.WedOpen.TimeOfDay.ToString();
            txt_WedClosing.Text = Properties.Settings.Default.WedClose.TimeOfDay.ToString();
            txt_ThuOpening.Text = Properties.Settings.Default.ThuOpen.TimeOfDay.ToString();
            txt_ThuClosing.Text = Properties.Settings.Default.ThuClose.TimeOfDay.ToString();
            txt_FriOpening.Text = Properties.Settings.Default.FriOpen.TimeOfDay.ToString();
            txt_FriClosing.Text = Properties.Settings.Default.FriClose.TimeOfDay.ToString();
            txt_SatOpening.Text = Properties.Settings.Default.SatOpen.TimeOfDay.ToString();
            txt_SatClosing.Text = Properties.Settings.Default.SatClose.TimeOfDay.ToString();
            txt_SunOpening.Text = Properties.Settings.Default.SunOpen.TimeOfDay.ToString();
            txt_SunClosing.Text = Properties.Settings.Default.SunClose.TimeOfDay.ToString();

            checkDays();
        }

        private void checkDays() {
            if ((txt_MonOpening.Text == closed && txt_MonClosing.Text == closed) || (txt_MonOpening.Text == "closed" && txt_MonClosing.Text == "closed")) {
                chk_MonClosed_Checked(null, null);
                chk_MonClosed.IsChecked = true;
            }
            if ((txt_TueOpening.Text == closed && txt_TueClosing.Text == closed) || (txt_TueOpening.Text == "closed" && txt_TueClosing.Text == "closed")) {
                chk_TueClosed_Checked(null, null);
                chk_TueClosed.IsChecked = true;
            }
            if ((txt_WedOpening.Text == closed && txt_WedClosing.Text == closed) || (txt_WedOpening.Text == "closed" && txt_WedClosing.Text == "closed")) {
                chk_WedClosed_Checked(null, null);
                chk_WedClosed.IsChecked = true;
            }
            if ((txt_ThuOpening.Text == closed && txt_ThuClosing.Text == closed) || (txt_ThuOpening.Text == "closed" && txt_ThuClosing.Text == "closed")) {
                chk_ThuClosed_Checked(null, null);
                chk_ThuClosed.IsChecked = true;
            }
            if ((txt_FriOpening.Text == closed && txt_FriClosing.Text == closed) || (txt_FriOpening.Text == "closed" && txt_FriClosing.Text == "closed")) {
                chk_FriClosed_Checked(null, null);
                chk_FriClosed.IsChecked = true;
            }
            if ((txt_SatOpening.Text == closed && txt_SatClosing.Text == closed) || (txt_SatOpening.Text == "closed" && txt_SatClosing.Text == "closed")) {
                chk_SatClosed_Checked(null, null);
                chk_SatClosed.IsChecked = true;
            }
            if ((txt_SunOpening.Text == closed && txt_SunClosing.Text == closed) || (txt_SunOpening.Text == "closed" && txt_SunClosing.Text == "closed")) {
                chk_SunClosed_Checked(null, null);
                chk_SunClosed.IsChecked = true;
            }
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.BillingStartDate = txt_BillingDate.Text;
            Properties.Settings.Default.MaxMonthlyFee = txt_MaxMonthlyFee.Text;
            Properties.Settings.Default.HoldExpiredRecords = txt_ExpirationDays.Text;
            Properties.Settings.Default.InfantMaxAge = txt_InfantAge.Text;
            Properties.Settings.Default.RegularMaxAge = txt_RegularAge.Text;

            if(txt_MonOpening.Text.Equals("closed"))
                Properties.Settings.Default.MonOpen = DateTime.Parse(closed);
            else
                Properties.Settings.Default.MonOpen = DateTime.Parse(txt_MonOpening.Text);

            if (txt_MonClosing.Text.Equals("closed"))
                Properties.Settings.Default.MonClose = DateTime.Parse(closed); 
            else
                Properties.Settings.Default.MonClose = DateTime.Parse(txt_MonClosing.Text);

            if (txt_TueOpening.Text.Equals("closed"))
                Properties.Settings.Default.TueOpen = DateTime.Parse(closed);
            else
                Properties.Settings.Default.TueOpen = DateTime.Parse(txt_TueOpening.Text);

            if (txt_TueClosing.Text.Equals("closed"))
                Properties.Settings.Default.TueClose = DateTime.Parse(closed);
            else
                Properties.Settings.Default.TueClose = DateTime.Parse(txt_TueClosing.Text);

            if (txt_WedOpening.Text.Equals("closed"))
                Properties.Settings.Default.WedOpen = DateTime.Parse(closed);
            else
                Properties.Settings.Default.WedOpen = DateTime.Parse(txt_WedOpening.Text);

            if (txt_WedClosing.Text.Equals("closed"))
                Properties.Settings.Default.WedClose = DateTime.Parse(closed);
            else
                Properties.Settings.Default.WedClose = DateTime.Parse(txt_WedClosing.Text);

            if (txt_ThuOpening.Text.Equals("closed"))
                Properties.Settings.Default.ThuOpen = DateTime.Parse(closed);
            else
                Properties.Settings.Default.ThuOpen = DateTime.Parse(txt_ThuOpening.Text);

            if (txt_ThuClosing.Text.Equals("closed"))
                Properties.Settings.Default.ThuClose = DateTime.Parse(closed);
            else
                Properties.Settings.Default.ThuClose = DateTime.Parse(txt_ThuClosing.Text);

            if (txt_FriOpening.Text.Equals("closed"))
                Properties.Settings.Default.FriOpen = DateTime.Parse(closed);
            else
                Properties.Settings.Default.FriOpen = DateTime.Parse(txt_FriOpening.Text);

            if (txt_FriClosing.Text.Equals("closed"))
                Properties.Settings.Default.FriClose = DateTime.Parse(closed);
            else
                Properties.Settings.Default.FriClose = DateTime.Parse(txt_FriClosing.Text);

            if (txt_SatOpening.Text.Equals("closed"))
                Properties.Settings.Default.SatOpen = DateTime.Parse(closed);
            else
                Properties.Settings.Default.SatOpen = DateTime.Parse(txt_SatOpening.Text);

            if (txt_SatClosing.Text.Equals("closed"))
                Properties.Settings.Default.SatClose = DateTime.Parse(closed);
            else
                Properties.Settings.Default.SatClose = DateTime.Parse(txt_SatClosing.Text);

            if (txt_SunOpening.Text.Equals("closed"))
                Properties.Settings.Default.SunOpen = DateTime.Parse(closed);
            else
                Properties.Settings.Default.SunOpen = DateTime.Parse(txt_SunOpening.Text);

            if (txt_SunClosing.Text.Equals("closed"))
                Properties.Settings.Default.SunClose = DateTime.Parse(closed);
            else
                Properties.Settings.Default.SunClose = DateTime.Parse(txt_SunClosing.Text);

            Properties.Settings.Default.Save();
        }

        private void disableAll() {
            txt_BillingDate.IsEnabled = false;
            txt_MaxMonthlyFee.IsEnabled = false;
            txt_ExpirationDays.IsEnabled = false;
            txt_InfantAge.IsEnabled = false;
            txt_RegularAge.IsEnabled = false;
            chk_MonClosed.IsEnabled = false;
            txt_MonOpening.IsEnabled = false;
            txt_MonClosing.IsEnabled = false;
            chk_TueClosed.IsEnabled = false;
            txt_TueOpening.IsEnabled = false;
            txt_TueClosing.IsEnabled = false;
            chk_WedClosed.IsEnabled = false;
            txt_WedOpening.IsEnabled = false;
            txt_WedClosing.IsEnabled = false;
            chk_ThuClosed.IsEnabled = false;
            txt_ThuOpening.IsEnabled = false;
            txt_ThuClosing.IsEnabled = false;
            chk_FriClosed.IsEnabled = false;
            txt_FriOpening.IsEnabled = false;
            txt_FriClosing.IsEnabled = false;
            chk_SatClosed.IsEnabled = false;
            txt_SatOpening.IsEnabled = false;
            txt_SatClosing.IsEnabled = false;
            chk_SunClosed.IsEnabled = false;
            txt_SunOpening.IsEnabled = false;
            txt_SunClosing.IsEnabled = false;
            btn_Confirm.IsEnabled = false;
            btn_Cancel.IsEnabled = false;
        }

        private void enableAll() {
            txt_BillingDate.IsEnabled = true;
            txt_MaxMonthlyFee.IsEnabled = true;
            txt_ExpirationDays.IsEnabled = true;
            txt_InfantAge.IsEnabled = true;
            txt_RegularAge.IsEnabled = true;
            chk_MonClosed.IsEnabled = true;
            txt_MonOpening.IsEnabled = true;
            txt_MonClosing.IsEnabled = true;
            chk_TueClosed.IsEnabled = true;
            txt_TueOpening.IsEnabled = true;
            txt_TueClosing.IsEnabled = true;
            chk_WedClosed.IsEnabled = true;
            txt_WedOpening.IsEnabled = true;
            txt_WedClosing.IsEnabled = true;
            chk_ThuClosed.IsEnabled = true;
            txt_ThuOpening.IsEnabled = true;
            txt_ThuClosing.IsEnabled = true;
            chk_FriClosed.IsEnabled = true;
            txt_FriOpening.IsEnabled = true;
            txt_FriClosing.IsEnabled = true;
            chk_SatClosed.IsEnabled = true;
            txt_SatOpening.IsEnabled = true;
            txt_SatClosing.IsEnabled = true;
            chk_SunClosed.IsEnabled = true;
            txt_SunOpening.IsEnabled = true;
            txt_SunClosing.IsEnabled = true;
            btn_Confirm.IsEnabled = true;
            btn_Cancel.IsEnabled = true;
            checkDays();
        }

        private void statusGood() {
            txt_Status.Text = "OK";
            txt_Status.Foreground = Brushes.LightGreen;
            errorPresent = false;
        }

        private void statusBad() {
            txt_Status.Text = "ERROR";
            txt_Status.Foreground = Brushes.Red;
            errorPresent = true;
        }

        private void chk_MonClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Monday.IsEnabled = false;
            txt_MonOpening.Text = "closed";
            txt_MonOpening.IsEnabled = false;
            txt_MonClosing.Text = "closed";
            txt_MonClosing.IsEnabled = false;
        }

        private void chk_MonClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Monday.IsEnabled = true;
            txt_MonOpening.IsEnabled = true;
            txt_MonOpening.Text = "09:00:00";
            txt_MonClosing.IsEnabled = true;
            txt_MonClosing.Text = "17:00:00";
        }

        private void chk_TueClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Tuesday.IsEnabled = false;
            txt_TueOpening.Text = "closed";
            txt_TueOpening.IsEnabled = false;
            txt_TueClosing.Text = "closed";
            txt_TueClosing.IsEnabled = false;
        }

        private void chk_TueClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Tuesday.IsEnabled = true;
            txt_TueOpening.IsEnabled = true;
            txt_TueOpening.Text = "09:00:00";
            txt_TueClosing.IsEnabled = true;
            txt_TueClosing.Text = "17:00:00";
        }

        private void chk_WedClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Wednesday.IsEnabled = false;
            txt_WedOpening.Text = "closed";
            txt_WedOpening.IsEnabled = false;
            txt_WedClosing.Text = "closed";
            txt_WedClosing.IsEnabled = false;
        }

        private void chk_WedClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Wednesday.IsEnabled = true;
            txt_WedOpening.IsEnabled = true;
            txt_WedOpening.Text = "09:00:00";
            txt_WedClosing.IsEnabled = true;
            txt_WedClosing.Text = "17:00:00";
        }

        private void chk_ThuClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Thursday.IsEnabled = false;
            txt_ThuOpening.Text = "closed";
            txt_ThuOpening.IsEnabled = false;
            txt_ThuClosing.Text = "closed";
            txt_ThuClosing.IsEnabled = false;
        }

        private void chk_ThuClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Thursday.IsEnabled = true;
            txt_ThuOpening.IsEnabled = true;
            txt_ThuOpening.Text = "09:00:00";
            txt_ThuClosing.IsEnabled = true;
            txt_ThuClosing.Text = "17:00:00";
        }

        private void chk_FriClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Friday.IsEnabled = false;
            txt_FriOpening.Text = "closed";
            txt_FriOpening.IsEnabled = false;
            txt_FriClosing.Text = "closed";
            txt_FriClosing.IsEnabled = false;
        }

        private void chk_FriClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Friday.IsEnabled = true;
            txt_FriOpening.IsEnabled = true;
            txt_FriOpening.Text = "09:00:00";
            txt_FriClosing.IsEnabled = true;
            txt_FriClosing.Text = "17:00:00";
        }

        private void chk_SatClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Saturday.IsEnabled = false;
            txt_SatOpening.Text = "closed";
            txt_SatOpening.IsEnabled = false;
            txt_SatClosing.Text = "closed";
            txt_SatClosing.IsEnabled = false;
        }

        private void chk_SatClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Saturday.IsEnabled = true;
            txt_SatOpening.IsEnabled = true;
            txt_SatOpening.Text = "09:00:00";
            txt_SatClosing.IsEnabled = true;
            txt_SatClosing.Text = "17:00:00";
        }

        private void chk_SunClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Sunday.IsEnabled = false;
            txt_SunOpening.Text = "closed";
            txt_SunOpening.IsEnabled = false;
            txt_SunClosing.Text = "closed";
            txt_SunClosing.IsEnabled = false;
        }

        private void chk_SunClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Sunday.IsEnabled = true;
            txt_SunOpening.IsEnabled = true;
            txt_SunOpening.Text = "09:00:00";
            txt_SunClosing.IsEnabled = true;
            txt_SunClosing.Text = "17:00:00";
        }

        ///////////////////
        //GotFocus Events//
        ///////////////////

        private void txt_BillingDate_GotFocus(object sender, RoutedEventArgs e) {
            txt_BillingDate.SelectAll();
        }

        private void txt_MaxMonthlyFee_GotFocus(object sender, RoutedEventArgs e) {
            txt_MaxMonthlyFee.SelectAll();
        }

        private void txt_ExpirationDays_GotFocus(object sender, RoutedEventArgs e) {
            txt_ExpirationDays.SelectAll();
        }

        private void txt_InfantAge_GotFocus(object sender, RoutedEventArgs e) {
            txt_InfantAge.SelectAll();
        }

        private void txt_RegularAge_GotFocus(object sender, RoutedEventArgs e) {
            txt_RegularAge.SelectAll();
        }

        private void txt_MonOpening_GotFocus(object sender, RoutedEventArgs e) {
            txt_MonOpening.SelectAll();
        }

        private void txt_MonClosing_GotFocus(object sender, RoutedEventArgs e) {
            txt_MonClosing.SelectAll();
        }

        private void txt_TueOpening_GotFocus(object sender, RoutedEventArgs e) {
            txt_TueOpening.SelectAll();
        }

        private void txt_TueClosing_GotFocus(object sender, RoutedEventArgs e) {
            txt_TueClosing.SelectAll();
        }

        private void txt_WedOpening_GotFocus(object sender, RoutedEventArgs e) {
            txt_WedOpening.SelectAll();
        }

        private void txt_WedClosing_GotFocus(object sender, RoutedEventArgs e) {
            txt_WedClosing.SelectAll();
        }

        private void txt_ThuOpening_GotFocus(object sender, RoutedEventArgs e) {
            txt_ThuOpening.SelectAll();
        }

        private void txt_ThuClosing_GotFocus(object sender, RoutedEventArgs e) {
            txt_ThuClosing.SelectAll();
        }

        private void txt_FriOpening_GotFocus(object sender, RoutedEventArgs e) {
            txt_FriOpening.SelectAll();
        }

        private void txt_FriClosing_GotFocus(object sender, RoutedEventArgs e) {
            txt_FriClosing.SelectAll();
        }

        private void txt_SatOpening_GotFocus(object sender, RoutedEventArgs e) {
            txt_SatOpening.SelectAll();
        }

        private void txt_SatClosing_GotFocus(object sender, RoutedEventArgs e) {
            txt_SatClosing.SelectAll();
        }

        private void txt_SunOpening_GotFocus(object sender, RoutedEventArgs e) {
            txt_SunOpening.SelectAll();
        }

        private void txt_SunClosing_GotFocus(object sender, RoutedEventArgs e) {
            txt_SunClosing.SelectAll();
        }

        ////////////////////
        //LostFocus Events//
        ////////////////////

        private void txt_BillingDate_LostFocus(object sender, RoutedEventArgs e) {
            int res = -1;

            if (int.TryParse(txt_BillingDate.Text, out res)) {
                if (res < 1 || res > 29) {
                    MessageBox.Show("Billing date must be from 1 to 29");
                    disableAll();
                    statusBad();
                    txt_BillingDate.IsEnabled = true;
                }
                else if (!errorPresent) {
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show("Invalid Input. Billing Date must be a number from 1 to 29");
                disableAll();
                statusBad();
                txt_BillingDate.IsEnabled = true;
            }
        }

        private void txt_MaxMonthlyFee_LostFocus(object sender, RoutedEventArgs e) {
            int res = -1;

            if (int.TryParse(txt_MaxMonthlyFee.Text, out res)) {
                if (res < 0) {
                    MessageBox.Show("Maximum Monthly Fee must be a positive number");
                    disableAll();
                    statusBad();
                    txt_MaxMonthlyFee.IsEnabled = true;
                }
                else if (!errorPresent) {
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show("Invalid Input. Maximum Monthly Fee must be a positive number");
                disableAll();
                statusBad();
                txt_MaxMonthlyFee.IsEnabled = true;
            }
        }

        private void txt_ExpirationDays_LostFocus(object sender, RoutedEventArgs e) {
            int res = -1;

            if (int.TryParse(txt_ExpirationDays.Text, out res)) {
                if (res < 0) {
                    MessageBox.Show("Days to hold expired records must be a positive number");
                    disableAll();
                    statusBad();
                    txt_ExpirationDays.IsEnabled = true;
                }
                else if (!errorPresent) {
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show("Invalid Input. Days to hold expired records must be a positive number");
                disableAll();
                statusBad();
                txt_ExpirationDays.IsEnabled = true;
            }
        }

        private void txt_InfantAge_LostFocus(object sender, RoutedEventArgs e) {
            int res = -1;

            if (int.TryParse(txt_InfantAge.Text, out res)) {
                if (res < 0 || res > int.Parse(Properties.Settings.Default.RegularMaxAge)) {
                    MessageBox.Show("Infant must be a positive number less than Regular Age");
                    disableAll();
                    statusBad();
                    txt_InfantAge.IsEnabled = true;
                }
                else if (!errorPresent) {
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show("Invalid Input. Infant Age must be a positive number less than Regular Age");
                disableAll();
                statusBad();
                txt_InfantAge.IsEnabled = true;
            }
        }

        private void txt_RegularAge_LostFocus(object sender, RoutedEventArgs e) {
            int res = -1;

            if (int.TryParse(txt_RegularAge.Text, out res)) {
                if (res < 0 || res < int.Parse(Properties.Settings.Default.InfantMaxAge)) {
                    MessageBox.Show("Regular must be a positive number greater than Infant Age");
                    disableAll();
                    statusBad();
                    txt_RegularAge.IsEnabled = true;
                }
                else if (!errorPresent) {
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show("Invalid Input. Regular Age must be a positive number greater than Infant Age");
                disableAll();
                statusBad();
                txt_RegularAge.IsEnabled = true;
            }
        }

        private void txt_MonOpening_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_MonOpening.Text, out res)) {
                if (res.TimeOfDay > DateTime.Parse(txt_MonClosing.Text).TimeOfDay) {
                    MessageBox.Show("Opening time must be less than closing time");
                    disableAll();
                    statusBad();
                    txt_MonOpening.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_MonOpening.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_MonOpening.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_MonOpening.IsEnabled = true;
            }
        }

        private void txt_MonClosing_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_MonClosing.Text, out res)) {
                if (res.TimeOfDay < DateTime.Parse(txt_MonOpening.Text).TimeOfDay) {
                    MessageBox.Show("Closing time must be greater than opening time");
                    disableAll();
                    statusBad();
                    txt_MonClosing.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_MonClosing.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_MonClosing.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_MonClosing.IsEnabled = true;
            }
        }

        private void txt_TueOpening_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_TueOpening.Text, out res)) {
                if (res.TimeOfDay > DateTime.Parse(txt_TueClosing.Text).TimeOfDay) {
                    MessageBox.Show("Opening time must be less than closing time");
                    disableAll();
                    statusBad();
                    txt_TueOpening.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_TueOpening.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_TueOpening.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_TueOpening.IsEnabled = true;
            }
        }

        private void txt_TueClosing_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_TueClosing.Text, out res)) {
                if (res.TimeOfDay < DateTime.Parse(txt_TueOpening.Text).TimeOfDay) {
                    MessageBox.Show("Closing time must be greather than opening time");
                    disableAll();
                    statusBad();
                    txt_TueClosing.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_TueClosing.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_TueClosing.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_TueClosing.IsEnabled = true;
            }
        }

        private void txt_WedOpening_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_WedOpening.Text, out res)) {
                if (res.TimeOfDay > DateTime.Parse(txt_WedClosing.Text).TimeOfDay) {
                    MessageBox.Show("Opening time must be less than closing time");
                    disableAll();
                    statusBad();
                    txt_WedOpening.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_WedOpening.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_WedOpening.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_WedOpening.IsEnabled = true;
            }
        }

        private void txt_WedClosing_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_WedClosing.Text, out res)) {
                if (res.TimeOfDay < DateTime.Parse(txt_WedOpening.Text).TimeOfDay) {
                    MessageBox.Show("Closing time must be greather than opening time");
                    disableAll();
                    statusBad();
                    txt_WedClosing.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_WedClosing.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_WedClosing.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_WedClosing.IsEnabled = true;
            }
        }

        private void txt_ThuOpening_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_ThuOpening.Text, out res)) {
                if (res.TimeOfDay > DateTime.Parse(txt_ThuClosing.Text).TimeOfDay) {
                    MessageBox.Show("Opening time must be less than closing time");
                    disableAll();
                    statusBad();
                    txt_ThuOpening.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_ThuOpening.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_ThuOpening.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_ThuOpening.IsEnabled = true;
            }
        }

        private void txt_ThuClosing_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_ThuClosing.Text, out res)) {
                if (res.TimeOfDay < DateTime.Parse(txt_ThuOpening.Text).TimeOfDay) {
                    MessageBox.Show("Closing time must be greather than opening time");
                    disableAll();
                    statusBad();
                    txt_ThuClosing.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_ThuClosing.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_ThuClosing.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_ThuClosing.IsEnabled = true;
            }
        }

        private void txt_FriOpening_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_FriOpening.Text, out res)) {
                if (res.TimeOfDay > DateTime.Parse(txt_FriClosing.Text).TimeOfDay) {
                    MessageBox.Show("Opening time must be less than closing time");
                    disableAll();
                    statusBad();
                    txt_FriOpening.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_FriOpening.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_FriOpening.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_FriOpening.IsEnabled = true;
            }
        }

        private void txt_FriClosing_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_FriClosing.Text, out res)) {
                if (res.TimeOfDay < DateTime.Parse(txt_FriOpening.Text).TimeOfDay) {
                    MessageBox.Show("Closing time must be greather than opening time");
                    disableAll();
                    statusBad();
                    txt_FriClosing.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_FriClosing.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_FriClosing.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_FriClosing.IsEnabled = true;
            }
        }

        private void txt_SatOpening_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_SatOpening.Text, out res)) {
                if (res.TimeOfDay > DateTime.Parse(txt_SatClosing.Text).TimeOfDay) {
                    MessageBox.Show("Opening time must be less than closing time");
                    disableAll();
                    statusBad();
                    txt_SatOpening.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_SatOpening.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_SatOpening.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_SatOpening.IsEnabled = true;
            }
        }

        private void txt_SatClosing_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_SatClosing.Text, out res)) {
                if (res.TimeOfDay < DateTime.Parse(txt_SatOpening.Text).TimeOfDay) {
                    MessageBox.Show("Closing time must be greather than opening time");
                    disableAll();
                    statusBad();
                    txt_SatClosing.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_SatClosing.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_SatClosing.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_SatClosing.IsEnabled = true;
            }
        }

        private void txt_SunOpening_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_SunOpening.Text, out res)) {
                if (res.TimeOfDay > DateTime.Parse(txt_SunClosing.Text).TimeOfDay) {
                    MessageBox.Show("Opening time must be less than closing time");
                    disableAll();
                    statusBad();
                    txt_SunOpening.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_SunOpening.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_SunOpening.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_SunOpening.IsEnabled = true;
            }
        }

        private void txt_SunClosing_LostFocus(object sender, RoutedEventArgs e) {
            DateTime res;

            if (DateTime.TryParse(txt_SunClosing.Text, out res)) {
                if (res.TimeOfDay < DateTime.Parse(txt_SunOpening.Text).TimeOfDay) {
                    MessageBox.Show("Closing time must be greather than opening time");
                    disableAll();
                    statusBad();
                    txt_SunClosing.IsEnabled = true;
                }
                else if (!errorPresent) {
                    txt_SunClosing.Text = res.TimeOfDay.ToString();
                    enableAll();
                    statusGood();
                }
            }
            else {
                MessageBox.Show(txt_SunClosing.Text + " is Not a valid time");
                disableAll();
                statusBad();
                txt_SunClosing.IsEnabled = true;
            }
        }
    }
}