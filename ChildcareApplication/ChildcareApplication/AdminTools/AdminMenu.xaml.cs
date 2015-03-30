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

namespace AdminTools {
    /// <summary>
    /// Interaction logic for win_AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window {
        private int accessLevel;
        public AdminMenu(int accessLevel) {
            InitializeComponent();
            this.accessLevel = accessLevel;
            HideAdminOptions();
        }

        public AdminMenu() {
            InitializeComponent();
        }

        private void HideAdminOptions() {
            if (accessLevel == 2) {
                btn_EditAddEvents.IsEnabled = false;
                btn_EditAddEvents.Visibility = Visibility.Hidden;
            }
        }

        private void btn_EditOrDeleteParent_Click(object sender, RoutedEventArgs e) {
            bool editParent = true;
            ParentIDEntry ParentLogin = new ParentIDEntry(editParent);
            ParentLogin.Show();
            this.Close();
        }

        private void btn_Logout_Click(object sender, RoutedEventArgs e) {
            AdminLogin loginWindow = new AdminLogin();
            loginWindow.Show();
            this.Close();
        }

        private void btn_ParentReport_Click(object sender, RoutedEventArgs e) {
            ParentReport parentReportWin = new ParentReport();
            parentReportWin.ShowDialog();
        }

        private void btn_BusinessReport_Click(object sender, RoutedEventArgs e) {
            BusinessReport businessReportWin = new BusinessReport();
            businessReportWin.Show();
        }

        private void btn_EditOrDeleteChild_Click(object sender, RoutedEventArgs e) {
            bool editParent = false;
            ParentIDEntry ParentLogin = new ParentIDEntry(editParent);
            ParentLogin.Show();
            this.Close();
        }

        private void btn_EditAddEvents_Click(object sender, RoutedEventArgs e) {
            EditEvents editEvents = new EditEvents();
            editEvents.Show();
        }

        private void btn_AddNewParent_Click(object sender, RoutedEventArgs e) {
            NewParentLogin newParentLogin = new NewParentLogin();
            newParentLogin.ShowDialog();
            this.Close(); 
        }
    }//end class
}//end namespace
