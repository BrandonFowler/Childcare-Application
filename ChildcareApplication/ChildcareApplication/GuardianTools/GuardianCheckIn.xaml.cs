using ChildcareApplication;
using DatabaseController;
using MessageBoxUtils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GuardianTools {
   
    public partial class GuardianCheckIn : Window {

        private bool IDBoxSelected = false;
        private bool PINBoxSelected = false;
        private bool altKeyPressed = false;
        private LoginDB db;

        public GuardianCheckIn() {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.db = new LoginDB();
            this.txt_IDEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_PINEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_IDEntry.GotFocus += OnBoxFocus;
            this.txt_PINEntry.GotFocus += OnBoxFocus;
            this.txt_IDEntry.GotMouseCapture += new System.Windows.Input.MouseEventHandler(OnBoxFocus);
            this.txt_PINEntry.GotMouseCapture += new System.Windows.Input.MouseEventHandler(OnBoxFocus);
            this.txt_IDEntry.Focus();
            this.MouseDown += WindowMouseDown;
        }

        private void OnBoxFocus(object sender, RoutedEventArgs e) {
            if (e.OriginalSource as TextBox != null) {
                this.IDBoxSelected = true;
                this.PINBoxSelected = false;
                this.txt_IDEntry.SelectAll();
            }
            else {
                this.PINBoxSelected = true;
                this.IDBoxSelected = false;
                this.txt_PINEntry.SelectAll();
            }
        }

        private void KeyPressedValidateNumber(Object o, KeyEventArgs e) {
            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt) {
                this.altKeyPressed = true;
            }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) 
                || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.NumLock 
                || this.altKeyPressed) {
                if (e.Key == Key.Return){
                    if (IDBoxSelected){
                        this.txt_PINEntry.Focus();
                    }
                    else if (PINBoxSelected){
                        GuardianLogin();
                    }
                }
                this.altKeyPressed = false;
            }
            else {
                WPFMessageBox.Show("Please use only numbers.");
                e.Handled = true;
                this.altKeyPressed = false;
            }
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected) {
                this.txt_IDEntry.Clear();
                this.txt_IDEntry.Focus();
            }
            if (PINBoxSelected) {
                this.txt_PINEntry.Clear();
                this.txt_PINEntry.Focus();
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(this.txt_IDEntry.Text) || string.IsNullOrWhiteSpace(this.txt_PINEntry.Password)) {
                WPFMessageBox.Show("Please enter a User ID and a PIN.");

            }
            else {
                GuardianLogin();
            }
        }

        private void btn_UserSelect_Click(object sender, RoutedEventArgs e){
            UserSelection userSelect = new UserSelection();
            userSelect.Show();
            this.Close();
        }

        private void GuardianLogin() {
            string ID = txt_IDEntry.Text;
            string PIN = txt_PINEntry.Password;
            string hashedPIN = ChildcareApplication.AdminTools.Hashing.HashPass(PIN);
            bool userFound = this.db.ValidateGuardianLogin(ID, hashedPIN);
            if (userFound) {
                ChildLogin ChildLoginWindow = new ChildLogin(ID);
                ChildLoginWindow.Show();
                ChildLoginWindow.WindowState = WindowState.Maximized;
                this.Close();
            }
            else {
                WPFMessageBox.Show("User ID or PIN does not exist");
            }
        }

        private void btn_Number1_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "1";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "1";
            }
        }

        private void btn_Number2_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "2";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "2";
            }
        }

        private void btn_Number3_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "3";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "3";
            }
        }

        private void btn_Number4_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "4";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "4";
            }
        }

        private void btn_Number5_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "5";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "5";
            }
        }

        private void btn_Number6_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "6";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "6";
            }
        }

        private void btn_Number7_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "7";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "7";
            }
        }

        private void btn_Number8_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "8";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "8";
            }
        }

        private void btn_Number9_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "9";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "9";
            }
        }

        private void btn_Number0_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "0";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "0";
            }
        }

        private void btn_AdminLogin_Click(object sender, RoutedEventArgs e) {
            AdminTools.AdminLogin adminLogin = new AdminTools.AdminLogin("parentTools");
            adminLogin.Show();
            this.Close();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void PasteCheck(object sender, ExecutedRoutedEventArgs e) {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste) {
                e.Handled = true;
            }
        }

    }
}
