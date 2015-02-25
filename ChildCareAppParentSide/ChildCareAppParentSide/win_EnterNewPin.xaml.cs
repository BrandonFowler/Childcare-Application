using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ChildCareAppParentSide {

    public partial class win_EnterNewPin : Window {

        private string ID;
        private AdminDatabase db;
        private bool isTablet;

        public win_EnterNewPin(string ID, bool isTablet) {
            InitializeComponent();
            this.ID = ID;
            this.db = new AdminDatabase();
            this.isTablet = isTablet;
            this.pw_pinBox1.GotFocus += FocusedTextBox;
            this.pw_pinBox2.GotFocus += FocusedTextBox;
            if (isTablet) {
                btn_Keyboard.Visibility = Visibility.Visible;
                btn_Keyboard.IsEnabled = true;
            }
        }

        private void btn_Enter_Click(object sender, RoutedEventArgs e) {
            string pin1 = pw_pinBox1.Password;
            string pin2 = pw_pinBox2.Password;
            if (String.IsNullOrWhiteSpace(pin1) || String.IsNullOrWhiteSpace(pin2)) {
                MessageBox.Show("Please enter a pin into both boxes");
            }
            else if (pin1.Length != 4 || pin2.Length != 4) {
                MessageBox.Show("Please choose a four digit pin");
            }
            else {
                if (pin1.CompareTo(pin2) != 0) {
                    MessageBox.Show("Please enter the same pin into both boxes");
                }
                else {
                    db.editPin(ID, pin1);
                    this.Close();
                }
            }
        }//end btn_enter_click

        private void FocusedTextBox(object sender, EventArgs e) {
            if (isTablet) {
                startKeyBoard();
            }
        }//end OnTDBoxFocus

        private void btn_Keyboard_Click(object sender, RoutedEventArgs e) {
            startKeyBoard();
        }//end btn_KeyBoard_Click

        private void startKeyBoard() {
            Version win8version = new Version(6, 2, 9200, 0);

            if (Environment.OSVersion.Version >= win8version) {
                string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                string keyboardPath = Path.Combine(progFiles, "TabTip.exe");
                Process.Start(keyboardPath);
            }
        }//end startKeyBoard

    }// end win_EnterNewPin
}
