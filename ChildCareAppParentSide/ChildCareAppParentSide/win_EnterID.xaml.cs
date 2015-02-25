using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ChildCareAppParentSide {
    
    public partial class win_EnterID : Window {

        private string ID;
        private AdminDatabase db;
        private bool isTablet;

        public win_EnterID(bool isTablet) {
            InitializeComponent();
            this.db = new AdminDatabase();
            this.isTablet = isTablet;
            this.txt_ID.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_ID.GotFocus += FocusedTextBox;
            if (isTablet) {
                btn_Keyboard.Visibility = Visibility.Visible;
                btn_Keyboard.IsEnabled = true;
            }
        }

        private void btn_Enter_Click(object sender, RoutedEventArgs e) {
            this.ID = txt_ID.Text;
            if (txt_ID.Text.Length != 6) {
                MessageBox.Show("Invalid ID length");
            }
            else if (db.IDExists(ID)) {
                this.Close();
            }
            else {
                bool? delete;
                win_Confirmation DeleteConformation = new win_Confirmation("This ID does not exist. Would you like to register a new guardian?");
                delete = DeleteConformation.ShowDialog();
                if ((bool)delete == true) {
                    db.addNewParent(ID);
                    win_EnterNewPin pinEntry = new win_EnterNewPin(ID,isTablet);
                    pinEntry.WindowState = WindowState.Maximized;
                    bool? done = pinEntry.ShowDialog();
                    this.Close();
                }
                else {
                    txt_ID.Clear();
                }
            }
        }//end btn_Enter_Click

        private void KeyPressedValidateNumber(Object o, KeyEventArgs e) {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Tab) {

            }
            else {
                MessageBox.Show("Please use only numbers.");
                e.Handled = true;
            }
        }//end KeyPressedValidateNumber

        public string getID(){
            return this.ID;
        }

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

    }//end of win_EnterID(class)
}
