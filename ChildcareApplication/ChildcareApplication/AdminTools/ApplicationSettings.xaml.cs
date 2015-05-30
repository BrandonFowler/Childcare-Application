using MessageBoxUtils;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChildcareApplication.AdminTools {
    public partial class ApplicationSettings : Window {

        private string closedString;
        private string defaultOpen;
        private string defaultClose;
        private bool errorPresent = false;

        public ApplicationSettings() {
            InitializeComponent();

            closedString = "00:00:00";
            defaultOpen = "09:00:00";
            defaultClose = "17:00:00";
            errorPresent = false;

            loadSettings();
            this.MouseDown += WindowMouseDown;
        }

        private void loadSettings() {
            txt_BillingDate.Text = Properties.Settings.Default.BillingStartDate.ToString();
            txt_MaxMonthlyFee.Text = Properties.Settings.Default.MaxMonthlyFee.ToString();
            txt_ExpirationDays.Text = Properties.Settings.Default.HoldExpiredRecords.ToString();
            txt_InfantAge.Text = Properties.Settings.Default.InfantMaxAge.ToString();
            txt_RegularAge.Text = Properties.Settings.Default.RegularMaxAge.ToString();
            txt_defaultReportFolder.Text = Properties.Settings.Default.DefaultSaveFolder.ToString();

            txt_MonOpening.Text = Properties.Settings.Default.MonOpen.ToString("HH:mm");
            txt_MonClosing.Text = Properties.Settings.Default.MonClose.ToString("HH:mm");
            txt_TueOpening.Text = Properties.Settings.Default.TueOpen.ToString("HH:mm");
            txt_TueClosing.Text = Properties.Settings.Default.TueClose.ToString("HH:mm");
            txt_WedOpening.Text = Properties.Settings.Default.WedOpen.ToString("HH:mm");
            txt_WedClosing.Text = Properties.Settings.Default.WedClose.ToString("HH:mm");
            txt_ThuOpening.Text = Properties.Settings.Default.ThuOpen.ToString("HH:mm");
            txt_ThuClosing.Text = Properties.Settings.Default.ThuClose.ToString("HH:mm");
            txt_FriOpening.Text = Properties.Settings.Default.FriOpen.ToString("HH:mm");
            txt_FriClosing.Text = Properties.Settings.Default.FriClose.ToString("HH:mm");
            txt_SatOpening.Text = Properties.Settings.Default.SatOpen.ToString("HH:mm");
            txt_SatClosing.Text = Properties.Settings.Default.SatClose.ToString("HH:mm");
            txt_SunOpening.Text = Properties.Settings.Default.SunOpen.ToString("HH:mm");
            txt_SunClosing.Text = Properties.Settings.Default.SunClose.ToString("HH:mm");

            checkDays();
        }

        private void checkDays() {
            if ((txt_MonOpening.Text == closedString && txt_MonClosing.Text == closedString) ||
                (txt_MonOpening.Text == "closed" && txt_MonClosing.Text == "closed")) {
                chk_MonClosed_Checked(null, null);
                chk_MonClosed.IsChecked = true;
            }
            if ((txt_TueOpening.Text == closedString && txt_TueClosing.Text == closedString) ||
                (txt_TueOpening.Text == "closed" && txt_TueClosing.Text == "closed")) {
                chk_TueClosed_Checked(null, null);
                chk_TueClosed.IsChecked = true;
            }
            if ((txt_WedOpening.Text == closedString && txt_WedClosing.Text == closedString) ||
                (txt_WedOpening.Text == "closed" && txt_WedClosing.Text == "closed")) {
                chk_WedClosed_Checked(null, null);
                chk_WedClosed.IsChecked = true;
            }
            if ((txt_ThuOpening.Text == closedString && txt_ThuClosing.Text == closedString) ||
                (txt_ThuOpening.Text == "closed" && txt_ThuClosing.Text == "closed")) {
                chk_ThuClosed_Checked(null, null);
                chk_ThuClosed.IsChecked = true;
            }
            if ((txt_FriOpening.Text == closedString && txt_FriClosing.Text == closedString) ||
                (txt_FriOpening.Text == "closed" && txt_FriClosing.Text == "closed")) {
                chk_FriClosed_Checked(null, null);
                chk_FriClosed.IsChecked = true;
            }
            if ((txt_SatOpening.Text == closedString && txt_SatClosing.Text == closedString) ||
                (txt_SatOpening.Text == "closed" && txt_SatClosing.Text == "closed")) {
                chk_SatClosed_Checked(null, null);
                chk_SatClosed.IsChecked = true;
            }
            if ((txt_SunOpening.Text == closedString && txt_SunClosing.Text == closedString) ||
                (txt_SunOpening.Text == "closed" && txt_SunClosing.Text == "closed")) {
                chk_SunClosed_Checked(null, null);
                chk_SunClosed.IsChecked = true;
            }
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e) {
            if (!errorPresent) {

                Properties.Settings.Default.BillingStartDate = txt_BillingDate.Text;
                Properties.Settings.Default.MaxMonthlyFee = txt_MaxMonthlyFee.Text;
                Properties.Settings.Default.HoldExpiredRecords = txt_ExpirationDays.Text;
                Properties.Settings.Default.InfantMaxAge = txt_InfantAge.Text;
                Properties.Settings.Default.RegularMaxAge = txt_RegularAge.Text;
                Properties.Settings.Default.DefaultSaveFolder = txt_defaultReportFolder.Text;

                if (txt_MonOpening.Text.Equals("closed"))
                    Properties.Settings.Default.MonOpen = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.MonOpen = DateTime.Parse(txt_MonOpening.Text);

                if (txt_MonClosing.Text.Equals("closed"))
                    Properties.Settings.Default.MonClose = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.MonClose = DateTime.Parse(txt_MonClosing.Text);

                if (txt_TueOpening.Text.Equals("closed"))
                    Properties.Settings.Default.TueOpen = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.TueOpen = DateTime.Parse(txt_TueOpening.Text);

                if (txt_TueClosing.Text.Equals("closed"))
                    Properties.Settings.Default.TueClose = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.TueClose = DateTime.Parse(txt_TueClosing.Text);

                if (txt_WedOpening.Text.Equals("closed"))
                    Properties.Settings.Default.WedOpen = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.WedOpen = DateTime.Parse(txt_WedOpening.Text);

                if (txt_WedClosing.Text.Equals("closed"))
                    Properties.Settings.Default.WedClose = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.WedClose = DateTime.Parse(txt_WedClosing.Text);

                if (txt_ThuOpening.Text.Equals("closed"))
                    Properties.Settings.Default.ThuOpen = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.ThuOpen = DateTime.Parse(txt_ThuOpening.Text);

                if (txt_ThuClosing.Text.Equals("closed"))
                    Properties.Settings.Default.ThuClose = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.ThuClose = DateTime.Parse(txt_ThuClosing.Text);

                if (txt_FriOpening.Text.Equals("closed"))
                    Properties.Settings.Default.FriOpen = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.FriOpen = DateTime.Parse(txt_FriOpening.Text);

                if (txt_FriClosing.Text.Equals("closed"))
                    Properties.Settings.Default.FriClose = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.FriClose = DateTime.Parse(txt_FriClosing.Text);

                if (txt_SatOpening.Text.Equals("closed"))
                    Properties.Settings.Default.SatOpen = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.SatOpen = DateTime.Parse(txt_SatOpening.Text);

                if (txt_SatClosing.Text.Equals("closed"))
                    Properties.Settings.Default.SatClose = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.SatClose = DateTime.Parse(txt_SatClosing.Text);

                if (txt_SunOpening.Text.Equals("closed"))
                    Properties.Settings.Default.SunOpen = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.SunOpen = DateTime.Parse(txt_SunOpening.Text);

                if (txt_SunClosing.Text.Equals("closed"))
                    Properties.Settings.Default.SunClose = DateTime.Parse(closedString);
                else
                    Properties.Settings.Default.SunClose = DateTime.Parse(txt_SunClosing.Text);

                Properties.Settings.Default.Save();
                WPFMessageBox.Show("Your new settings have been saved.", "Settings Saved!");
            } else {
                WPFMessageBox.Show("There is an error in the settings window, please fix all errors before submitting settings.", "ERROR!");
            }
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
            txt_BillingDate.Foreground = Brushes.Black;
            txt_MaxMonthlyFee.Foreground = Brushes.Black;
            txt_ExpirationDays.Foreground = Brushes.Black;
            txt_InfantAge.Foreground = Brushes.Black;
            txt_RegularAge.Foreground = Brushes.Black;
            txt_MonOpening.Foreground = Brushes.Black;
            txt_MonClosing.Foreground = Brushes.Black;
            txt_TueOpening.Foreground = Brushes.Black;
            txt_TueClosing.Foreground = Brushes.Black;
            txt_WedOpening.Foreground = Brushes.Black;
            txt_WedClosing.Foreground = Brushes.Black;
            txt_ThuOpening.Foreground = Brushes.Black;
            txt_ThuClosing.Foreground = Brushes.Black;
            txt_FriOpening.Foreground = Brushes.Black;
            txt_FriClosing.Foreground = Brushes.Black;
            txt_SatOpening.Foreground = Brushes.Black;
            txt_SatClosing.Foreground = Brushes.Black;
            txt_SunOpening.Foreground = Brushes.Black;
            txt_SunClosing.Foreground = Brushes.Black;
            checkDays();
        }

        private void statusGood() {
            btn_Confirm.Content = "Confirm";
            enableAll();
            txt_Status.Text = "OK";
            txt_Status.Foreground = Brushes.LightGreen;
            errorPresent = false;
            btn_Confirm.ToolTip = "Confirm and save changes.";
        }

        private void statusBad() {
            btn_Confirm.Content = "Fix Errors";
            disableAll();
            txt_Status.Text = "ERROR";
            txt_Status.Foreground = Brushes.Red;
            errorPresent = true;
            btn_Confirm.ToolTip = "Confirm that you have fixed errors present before saving your settings.";
        }

        private void statusBad(TextBox errorBox) {
            Dispatcher.BeginInvoke((ThreadStart)delegate { errorBox.Focus(); });
            statusBad();
            errorBox.IsEnabled = true;
            errorBox.Foreground = Brushes.Red;
        }

        private void statusBad(TextBox errorBox1, TextBox errorBox2) {
            statusBad(errorBox1);
            errorBox2.IsEnabled = true;
            errorBox2.Foreground = Brushes.Red;
        }

        private void chk_MonClosed_Checked(object sender, RoutedEventArgs e) {
            DayIsClosed(txt_MonOpening, txt_MonClosing);
        }

        private void chk_MonClosed_Unchecked(object sender, RoutedEventArgs e) {
            DayIsOpen(txt_MonOpening, txt_MonClosing);
        }

        private void chk_TueClosed_Checked(object sender, RoutedEventArgs e) {
            DayIsClosed(txt_TueOpening, txt_TueClosing);
        }

        private void chk_TueClosed_Unchecked(object sender, RoutedEventArgs e) {
            DayIsOpen(txt_TueOpening, txt_TueClosing);
        }

        private void chk_WedClosed_Checked(object sender, RoutedEventArgs e) {
            DayIsClosed(txt_WedOpening, txt_WedClosing);
        }

        private void chk_WedClosed_Unchecked(object sender, RoutedEventArgs e) {
            DayIsOpen(txt_WedOpening, txt_WedClosing);
        }

        private void chk_ThuClosed_Checked(object sender, RoutedEventArgs e) {
            DayIsClosed(txt_ThuOpening, txt_ThuClosing);
        }

        private void chk_ThuClosed_Unchecked(object sender, RoutedEventArgs e) {
            DayIsOpen(txt_ThuOpening, txt_ThuClosing);
        }

        private void chk_FriClosed_Checked(object sender, RoutedEventArgs e) {
            DayIsClosed(txt_FriOpening, txt_FriClosing);
        }

        private void chk_FriClosed_Unchecked(object sender, RoutedEventArgs e) {
            DayIsOpen(txt_FriOpening, txt_FriClosing);
        }

        private void chk_SatClosed_Checked(object sender, RoutedEventArgs e) {
            DayIsClosed(txt_SatOpening, txt_SatClosing);
        }

        private void chk_SatClosed_Unchecked(object sender, RoutedEventArgs e) {
            DayIsOpen(txt_SatOpening, txt_SatClosing);
        }

        private void chk_SunClosed_Checked(object sender, RoutedEventArgs e) {
            DayIsClosed(txt_SunOpening, txt_SunClosing);
        }

        private void chk_SunClosed_Unchecked(object sender, RoutedEventArgs e) {
            DayIsOpen(txt_SunOpening, txt_SunClosing);
        }

        private void DayIsClosed(TextBox dayOpens, TextBox dayCloses) {
            dayOpens.Text = "closed";
            dayOpens.IsEnabled = false;
            dayCloses.Text = "closed";
            dayCloses.IsEnabled = false;
        }

        private void DayIsOpen(TextBox dayOpens, TextBox dayCloses) {
            dayOpens.IsEnabled = true;
            dayOpens.Text = defaultOpen;
            dayCloses.IsEnabled = true;
            dayCloses.Text = defaultClose;
        }

        private void SelectAllonFocus(object sender, RoutedEventArgs e) {
            TextBox tb = (TextBox)sender;
            Dispatcher.BeginInvoke((Action)(tb.SelectAll));
        }

        ////////////////////
        //LostFocus Events//
        ////////////////////

        private void txt_BillingDate_LostFocus(object sender, RoutedEventArgs e) {
            if (SettingsValidation.ValidBillingDate(txt_BillingDate.Text)) {
                statusGood();
            } else if (!errorPresent) {
                WPFMessageBox.Show("Invalid Input. Billing Date must be a number from 2 to 27");
                statusBad(txt_BillingDate);
            }
        }

        private void txt_MaxMonthlyFee_LostFocus(object sender, RoutedEventArgs e) {
            if (SettingsValidation.PositiveInteger(txt_MaxMonthlyFee.Text)) {
                statusGood();
            } else if (!errorPresent) {
                WPFMessageBox.Show("Invalid Input. Maximum Monthly Fee must be a positive number");
                statusBad(txt_MaxMonthlyFee);
            }
        }

        private void txt_ExpirationDays_LostFocus(object sender, RoutedEventArgs e) {
            if (SettingsValidation.PositiveInteger(txt_ExpirationDays.Text)) {
                statusGood();
            } else if (!errorPresent) {
                WPFMessageBox.Show("Invalid Input. Days to hold expired records must be a positive number");
                statusBad(txt_ExpirationDays);
            }
        }

        private void txt_InfantAge_LostFocus(object sender, RoutedEventArgs e) {
            if (SettingsValidation.ValidAge(txt_InfantAge.Text, txt_RegularAge.Text)) {
                statusGood();
            } else if (!errorPresent) {
                WPFMessageBox.Show("Invalid Input. Infant Age must be a positive number less than Regular Age");
                statusBad(txt_InfantAge, txt_RegularAge);
            }
        }

        private void txt_RegularAge_LostFocus(object sender, RoutedEventArgs e) {
            if (SettingsValidation.ValidAge(txt_InfantAge.Text, txt_RegularAge.Text)) {
                statusGood();
            } else if (!errorPresent) {
                WPFMessageBox.Show("Invalid Input. Regular Age must be a positive number greater than Infant Age");
                statusBad(txt_RegularAge, txt_InfantAge);
            }
        }

        private void btn_selectFolder_Click(object sender, RoutedEventArgs e) {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.SelectedPath = txt_defaultReportFolder.Text;

            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK") {
                txt_defaultReportFolder.Text = folderDialog.SelectedPath;
            }
        }

        private void txt_MonOpening_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingO(txt_MonOpening, txt_MonClosing);
        }

        private void txt_MonClosing_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingC(txt_MonOpening, txt_MonClosing);
        }

        private void txt_TueOpening_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingO(txt_TueOpening, txt_TueClosing);
        }

        private void txt_TueClosing_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingC(txt_TueOpening, txt_TueClosing);
        }

        private void txt_WedOpening_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingO(txt_WedOpening, txt_WedClosing);
        }

        private void txt_WedClosing_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingC(txt_WedOpening, txt_WedClosing);
        }

        private void txt_ThuOpening_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingO(txt_ThuOpening, txt_ThuClosing);
        }

        private void txt_ThuClosing_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingC(txt_ThuOpening, txt_ThuClosing);
        }

        private void txt_FriOpening_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingO(txt_FriOpening, txt_FriClosing);
        }

        private void txt_FriClosing_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingC(txt_FriOpening, txt_FriClosing);
        }

        private void txt_SatOpening_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingO(txt_SatOpening, txt_SatClosing);
        }

        private void txt_SatClosing_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingC(txt_SatOpening, txt_SatClosing);
        }

        private void txt_SunOpening_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingO(txt_SunOpening, txt_SunClosing);
        }

        private void txt_SunClosing_LostFocus(object sender, RoutedEventArgs e) {
            TimeCheckingC(txt_SunOpening, txt_SunClosing);
        }

        private void TimeCheckingO(TextBox openTime, TextBox closeTime) {
            if (SettingsValidation.ValidHours(openTime.Text, closeTime.Text)) {
                statusGood();
            } else if (!errorPresent) {
                WPFMessageBox.Show("Invalid Input. Must be a valid time less than closing time");
                statusBad(openTime, closeTime);
            }
        }

        private void TimeCheckingC(TextBox openTime, TextBox closeTime) {
            if (SettingsValidation.ValidHours(openTime.Text, closeTime.Text)) {
                statusGood();
            } else if (!errorPresent) {
                WPFMessageBox.Show("Invalid Input. Closing time must be a valid time greater than opening time");
                statusBad(closeTime, openTime);
            }
        }

        private void EnterKeyUpEvent(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}