using ChildcareApplication.DatabaseController;
using System;
using System.Collections;
using System.Windows;

namespace ChildcareApplication.AdminTools
{
    /// <summary>
    /// Interaction logic for ApplicationSettings.xaml
    /// </summary>
    public partial class ApplicationSettings : Window
    {
        public ApplicationSettings()
        {
            InitializeComponent();

            loadSettings();
        }

        private void loadSettings()
        {
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

            if (txt_MonOpening.Text == "00:00:00" && txt_MonClosing.Text == "00:00:00")
            {
                chk_MonClosed_Checked(null, null);
                chk_MonClosed.IsChecked = true;
            }
            if (txt_TueOpening.Text == "00:00:00" && txt_TueClosing.Text == "00:00:00")
            {
                chk_TueClosed_Checked(null, null);
                chk_TueClosed.IsChecked = true;
            }
            if (txt_WedOpening.Text == "00:00:00" && txt_WedClosing.Text == "00:00:00")
            {
                chk_WedClosed_Checked(null, null);
                chk_WedClosed.IsChecked = true;
            }
            if (txt_ThuOpening.Text == "00:00:00" && txt_ThuClosing.Text == "00:00:00")
            { 
                chk_ThuClosed_Checked(null, null);
                chk_ThuClosed.IsChecked = true;
            }
            if (txt_FriOpening.Text == "00:00:00" && txt_FriClosing.Text == "00:00:00")
            { 
                chk_FriClosed_Checked(null, null);
                chk_FriClosed.IsChecked = true;
            }
            if (txt_SatOpening.Text == "00:00:00" && txt_SatClosing.Text == "00:00:00")
            {
                chk_SatClosed_Checked(null, null);
                chk_SatClosed.IsChecked = true;
            }
            if (txt_SunOpening.Text == "00:00:00" && txt_SunClosing.Text == "00:00:00")
            { 
                chk_SunClosed_Checked(null, null);
                chk_SunClosed.IsChecked = true;
            }
        }

        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.BillingStartDate = txt_BillingDate.Text;
            Properties.Settings.Default.MaxMonthlyFee = txt_MaxMonthlyFee.Text;
            Properties.Settings.Default.HoldExpiredRecords = txt_ExpirationDays.Text;
            Properties.Settings.Default.InfantMaxAge = txt_InfantAge.Text;
            Properties.Settings.Default.RegularMaxAge = txt_RegularAge.Text;


            try
            {
                Properties.Settings.Default.MonOpen = DateTime.Parse(txt_MonOpening.Text);
                Properties.Settings.Default.MonClose = DateTime.Parse(txt_MonClosing.Text);
                Properties.Settings.Default.TueOpen = DateTime.Parse(txt_TueOpening.Text);
                Properties.Settings.Default.TueClose = DateTime.Parse(txt_TueClosing.Text);
                Properties.Settings.Default.WedOpen = DateTime.Parse(txt_WedOpening.Text);
                Properties.Settings.Default.WedClose = DateTime.Parse(txt_WedClosing.Text);
                Properties.Settings.Default.ThuOpen = DateTime.Parse(txt_ThuOpening.Text);
                Properties.Settings.Default.ThuClose = DateTime.Parse(txt_ThuClosing.Text);
                Properties.Settings.Default.FriOpen = DateTime.Parse(txt_FriOpening.Text);
                Properties.Settings.Default.FriClose = DateTime.Parse(txt_FriClosing.Text);
                Properties.Settings.Default.SatOpen = DateTime.Parse(txt_SatOpening.Text);
                Properties.Settings.Default.SatClose = DateTime.Parse(txt_SatClosing.Text);
                Properties.Settings.Default.SunOpen = DateTime.Parse(txt_SunOpening.Text);
                Properties.Settings.Default.SunClose = DateTime.Parse(txt_SunClosing.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter time in the format hour:minute:second AM/PM or represented as 24 hour time", "Invalid Time Entered");
            }

            Properties.Settings.Default.Save();
            loadSettings();
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
            txt_MonClosing.IsEnabled = true;
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
            txt_TueClosing.IsEnabled = true;
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
            txt_WedClosing.IsEnabled = true;
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
            txt_ThuClosing.IsEnabled = true;
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
            txt_FriClosing.IsEnabled = true;
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
            txt_SatClosing.IsEnabled = true;
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
            txt_SunClosing.IsEnabled = true;
        }
    }
}
