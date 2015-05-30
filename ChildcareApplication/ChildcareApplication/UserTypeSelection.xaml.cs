using AdminTools;
using GuardianTools;
using MessageBoxUtils;
using System;
using System.IO;
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

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btn_Help_Click(object sender, RoutedEventArgs e) {
            if (File.Exists("C:/Users/Public/Documents" + "/Childcare Application/Childcare Application User Manual.pdf")) {
                try {
                    System.Diagnostics.Process.Start("C:/Users/Public/Documents" + "/Childcare Application/Childcare Application User Manual.pdf");
                } catch (System.IO.FileNotFoundException) {
                    WPFMessageBox.Show("Unable to open user manual. It may not exist.");
                } catch (Exception) {
                    WPFMessageBox.Show("Unable to open user manual.");
                }
            } else {
                WPFMessageBox.Show("Unable to open user manual.");
            }
        }

        private void btn_About_Click(object sender, RoutedEventArgs e) {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
