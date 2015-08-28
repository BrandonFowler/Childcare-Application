using ChildcareApplication;
using DatabaseController;
using MessageBoxUtils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdminTools {
    public partial class AdminLogin : Window {
        private LoginDB db;
        private bool parentTools = false;

        public AdminLogin() {
            InitializeComponent();
            this.db = new LoginDB();
            this.txt_UserName.Focus();
            this.MouseDown += WindowMouseDown;
        }

        public AdminLogin(string login) {
            InitializeComponent();
            this.db = new LoginDB();
            this.txt_UserName.Focus();
            this.parentTools = true;
            this.MouseDown += WindowMouseDown;
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            LoginCheck();
        }

        private void LoginCheck() {
            if (string.IsNullOrWhiteSpace(this.txt_UserName.Text) || string.IsNullOrWhiteSpace(this.pwd_Password.Password)) {
                WPFMessageBox.Show("Please enter a User Name and a Password.");
            } else {
                string ID = txt_UserName.Text;
                string PIN = pwd_Password.Password;
                string hashedPIN = ChildcareApplication.AdminTools.Hashing.HashPass(PIN);

                bool userFound = this.db.validateAdminLogin(ID, hashedPIN);

                if (userFound) {
                    if (parentTools) {
                        DisplayAdminChildCheckIn();
                    } else {
                        int accessLevel = db.GetAccessLevel(ID);
                        DisplayAdminWindow(accessLevel, txt_UserName.Text);
                    }
                } else {
                    WPFMessageBox.Show("User ID or PIN does not exist");
                }
            }
        }

        private void DisplayAdminChildCheckIn() {
            GuardianTools.AdminChildCheckIn AdminCheckIn = new GuardianTools.AdminChildCheckIn();
            AdminCheckIn.Show();
            this.Close();
        }

        private void DisplayAdminWindow(int accessLevel, string username) {
            AdminMenu AdminMenu = new AdminMenu(accessLevel, username);
            AdminMenu.Show();
            this.Close();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            UserSelection userSelect = new UserSelection();
            userSelect.Show();
            this.Close();
        }

        private void KeyUp_Event_USRName(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next); //Found at: http://stackoverflow.com/questions/23008670/wpf-and-mvvm-how-to-move-focus-to-the-next-control-automatically
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }

        private void KeyUp_Event_PWD(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                LoginCheck();
            }
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)((TextBox)sender).SelectAll);
        }

        private void pwd_GotFocus(object sender, RoutedEventArgs e) {
            Dispatcher.BeginInvoke((Action)pwd_Password.SelectAll);
        }
    }
}
