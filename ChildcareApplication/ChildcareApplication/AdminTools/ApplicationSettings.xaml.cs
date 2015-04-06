using System;
using System.Collections.Generic;
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
        }

        private void chk_MonClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Monday.IsEnabled = false;
            txt_MonOpening.Clear();
            txt_MonOpening.IsEnabled = false;
            txt_MonClosing.Clear();
            txt_MonClosing.IsEnabled = false;
        }

        private void chk_MonClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Monday.IsEnabled = true;
            txt_MonOpening.IsEnabled = true;
            txt_MonClosing.IsEnabled = true;
        }

        private void chk_TueClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Tuesday.IsEnabled = false;
            txt_TueOpening.Clear();
            txt_TueOpening.IsEnabled = false;
            txt_TueClosing.Clear();
            txt_TueClosing.IsEnabled = false;
        }

        private void chk_TueClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Tuesday.IsEnabled = true;
            txt_TueOpening.IsEnabled = true;
            txt_TueClosing.IsEnabled = true;
        }

        private void chk_WedClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Wednesday.IsEnabled = false;
            txt_WedOpening.Clear();
            txt_WedOpening.IsEnabled = false;
            txt_WedClosing.Clear();
            txt_WedClosing.IsEnabled = false;
        }

        private void chk_WedClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Wednesday.IsEnabled = true;
            txt_WedOpening.IsEnabled = true;
            txt_WedClosing.IsEnabled = true;
        }

        private void chk_ThuClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Thursday.IsEnabled = false;
            txt_ThuOpening.Clear();
            txt_ThuOpening.IsEnabled = false;
            txt_ThuClosing.Clear();
            txt_ThuClosing.IsEnabled = false;
        }

        private void chk_ThuClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Thursday.IsEnabled = true;
            txt_ThuOpening.IsEnabled = true;
            txt_ThuClosing.IsEnabled = true;
        }

        private void chk_FriClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Friday.IsEnabled = false;
            txt_FriOpening.Clear();
            txt_FriOpening.IsEnabled = false;
            txt_FriClosing.Clear();
            txt_FriClosing.IsEnabled = false;
        }

        private void chk_FriClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Friday.IsEnabled = true;
            txt_FriOpening.IsEnabled = true;
            txt_FriClosing.IsEnabled = true;
        }

        private void chk_SatClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Saturday.IsEnabled = false;
            txt_SatOpening.Clear();
            txt_SatOpening.IsEnabled = false;
            txt_SatClosing.Clear();
            txt_SatClosing.IsEnabled = false;
        }

        private void chk_SatClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Saturday.IsEnabled = true;
            txt_SatOpening.IsEnabled = true;
            txt_SatClosing.IsEnabled = true;
        }

        private void chk_SunClosed_Checked(object sender, RoutedEventArgs e) {
            lbl_Sunday.IsEnabled = false;
            txt_SunOpening.Clear();
            txt_SunOpening.IsEnabled = false;
            txt_SunClosing.Clear();
            txt_SunClosing.IsEnabled = false;
        }

        private void chk_SunClosed_Unchecked(object sender, RoutedEventArgs e) {
            lbl_Sunday.IsEnabled = true;
            txt_SunOpening.IsEnabled = true;
            txt_SunClosing.IsEnabled = true;
        }


    }
}
