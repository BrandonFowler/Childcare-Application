using ChildcareApplication.AdminTools;
using System.Windows;
using System.Windows.Input;

namespace AdminTools {
    public partial class AdminMenu : Window {
        private int accessLevel;
        private string username;

        public AdminMenu(int accessLevel, string username) {
            InitializeComponent();
            this.accessLevel = accessLevel;
            this.username = username;
            HideAdminOptions();
            this.MouseDown += WindowMouseDown;
        }

        private void HideAdminOptions() {
            if (accessLevel == 2) {
                btn_EditAddEvents.IsEnabled = false;
                btn_EditAddEvents.Visibility = Visibility.Hidden;
                btn_AddEditAdmin.IsEnabled = false;
                btn_AddEditAdmin.Visibility = Visibility.Hidden;
                btn_AppSettings.IsEnabled = false;
                btn_AppSettings.Visibility = Visibility.Hidden;
                btn_BusinessReport.IsEnabled = false;
                btn_BusinessReport.Visibility = Visibility.Hidden;
                btn_EditAddEvents.IsEnabled = false;
                btn_EditAddEvents.Visibility = Visibility.Hidden;
                btn_EditTransactions.IsEnabled = false;
                btn_EditTransactions.Visibility = Visibility.Hidden;
                btn_ParentReport.IsEnabled = false;
                btn_ParentReport.Visibility = Visibility.Hidden;
                btn_RestoreRecords.IsEnabled = false;
                btn_RestoreRecords.Visibility = Visibility.Hidden;
            } else if (accessLevel == 0) {
                btn_EditAddEvents.IsEnabled = false;
                btn_EditAddEvents.Visibility = Visibility.Hidden;
                btn_AppSettings.IsEnabled = false;
                btn_AppSettings.Visibility = Visibility.Hidden;
                btn_BusinessReport.IsEnabled = false;
                btn_BusinessReport.Visibility = Visibility.Hidden;
                btn_EditAddEvents.IsEnabled = false;
                btn_EditAddEvents.Visibility = Visibility.Hidden;
                btn_EditTransactions.IsEnabled = false;
                btn_EditTransactions.Visibility = Visibility.Hidden;
                btn_ParentReport.IsEnabled = false;
                btn_ParentReport.Visibility = Visibility.Hidden;
                btn_AddNewParent.IsEnabled = false;
                btn_AddNewParent.Visibility = Visibility.Hidden;
                btn_EditOrDeleteChild.IsEnabled = false;
                btn_EditOrDeleteChild.Visibility = Visibility.Hidden;
                btn_EditOrDeleteParent.IsEnabled = false;
                btn_EditOrDeleteParent.Visibility = Visibility.Hidden;
                btn_RestoreRecords.IsEnabled = false;
                btn_RestoreRecords.Visibility = Visibility.Hidden;
            }
        }

        private void btn_EditOrDeleteParent_Click(object sender, RoutedEventArgs e) {
            bool editParent = true;
            ParentIDEntry ParentLogin = new ParentIDEntry(editParent);
            ParentLogin.ShowDialog();
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
            businessReportWin.ShowDialog();
        }

        private void btn_EditOrDeleteChild_Click(object sender, RoutedEventArgs e) {
            bool editParent = false;
            ParentIDEntry ParentLogin = new ParentIDEntry(editParent);
            ParentLogin.ShowDialog();
        }

        private void btn_EditAddEvents_Click(object sender, RoutedEventArgs e) {
            EditEvents editEvents = new EditEvents();
            editEvents.ShowDialog();
        }

        private void btn_AddNewParent_Click(object sender, RoutedEventArgs e) {
            NewParentLogin newParentLogin = new NewParentLogin();
            newParentLogin.ShowDialog(); 
        }

        private void btn_EditTransaction_Click(object sender, RoutedEventArgs e) {
            EditTransactionWindow editEvents = new EditTransactionWindow();
            editEvents.ShowDialog();
        }

        private void btn_AddEditAdmin_Click(object sender, RoutedEventArgs e) {
            AdminAddEdit adminedit = new AdminAddEdit(username);
            adminedit.ShowDialog();
        }

        private void btn_AppSettings_Click(object sender, RoutedEventArgs e) {
            ApplicationSettings appsettings = new ApplicationSettings();
            appsettings.ShowDialog();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btn_RestoreRecords_Click(object sender, RoutedEventArgs e) {
            RestoreRecords restore = new RestoreRecords(accessLevel, username);
            restore.Show();
            this.Close();
        }

    }
}
