using AdminTools;
using GuardianTools;
using MessageBoxUtils;
using System.Windows;
using System.Windows.Input;

namespace ChildcareApplication {
    /// <summary>
    /// Interaction logic for UserSelection.xaml
    /// </summary>
    public partial class UserSelection : Window {
        public UserSelection() {
            InitializeComponent();
            this.MouseDown += WindowMouseDown;
        }

        private void btn_ParentUse_Click(object sender, RoutedEventArgs e) {
            GuardianCheckIn parentLogin = new GuardianCheckIn();
            parentLogin.Show();
            this.Close();
        }

        private void btn_AdminLogin_Click(object sender, RoutedEventArgs e) {
            AdminLogin adminLogin = new AdminLogin();
            adminLogin.Show();
            this.Close();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btn_Help_Click(object sender, RoutedEventArgs e) {
            WPFMessageBox.Show("Not yet implemented: will open the help pdf.");
        }

        private void btn_About_Click(object sender, RoutedEventArgs e) {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
