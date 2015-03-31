using ChildcareApplication;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace ParentTools {
   
    public partial class ParentLogin : Window {

        private bool IDBoxSelected = false;
        private bool PINBoxSelected = false;
        private ChildCheckInDatabase db;

        public ParentLogin() {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.db = new ChildCheckInDatabase();
            this.txt_IDEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_IDEntry.GotFocus += OnIDBoxFocus;
            this.txt_PINEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_PINEntry.GotFocus += OnPINBoxFocus;
            this.txt_IDEntry.Focus();
            this.btn_Login.GotFocus += OnLoginFocus;
            
        }//end win_LoginWindow

        private void OnIDBoxFocus(object sender, EventArgs e) {
            this.IDBoxSelected = true;
            this.PINBoxSelected = false;
        }//end OnTDBoxFocus

        private void OnPINBoxFocus(object sender, EventArgs e) {
            this.PINBoxSelected = true;
            this.IDBoxSelected = false;
        }//end OnPINBoxFocus

        private void KeyPressedValidateNumber(Object o, KeyEventArgs e) {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter) {
                if (e.Key == Key.Return){
                    if (IDBoxSelected){
                        txt_PINEntry.Focus();
                    }
                    else if (PINBoxSelected){
                        btn_Login.Focus();
                    }
                }
            }
            else {
                MessageBox.Show("Please use only numbers.");
                e.Handled = true;
            }
        }//end KeyPressedValidateNumber

        private void btn_Number1_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "1";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "1";
            }
        }//btn_Number1_Click

        private void btn_Number2_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "2";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "2";
            }
        }//btn_Number2_Click

        private void btn_Number3_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "3";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "3";
            }
        }//btn_Number3_Click

        private void btn_Number4_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "4";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "4";
            }
        }//btn_Number4_Click

        private void btn_Number5_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "5";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "5";
            }
        }//btn_Number5_Click

        private void btn_Number6_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "6";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "6";
            }
        }//btn_Number6_Click

        private void btn_Number7_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "7";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "7";
            }
        }//btn_Number7_Click

        private void btn_Number8_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "8";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "8";
            }
        }//btn_Number8_Click

        private void btn_Number9_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "9";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "9";
            }
        }//btn_Number9_Click

        private void btn_Number0_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "0";
            }
            if (PINBoxSelected && this.txt_PINEntry.Password.Length < 4) {
                this.txt_PINEntry.Password += "0";
            }
        }//btn_Number0_Click

        private void btn_Clear_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected) {
                this.txt_IDEntry.Clear();
                this.txt_IDEntry.Focus();
            }
            if (PINBoxSelected) {
                this.txt_PINEntry.Clear();
                this.txt_PINEntry.Focus();
            }
        }//btn_Clear_Click

        private void btn_Login_Click(object sender, RoutedEventArgs e) {

            if (string.IsNullOrWhiteSpace(this.txt_IDEntry.Text) || string.IsNullOrWhiteSpace(this.txt_PINEntry.Password)) {
                MessageBox.Show("Please enter a User ID and a PIN.");

            }
            else {
                string ID = txt_IDEntry.Text;
                string PIN = txt_PINEntry.Password;
                bool userFound = this.db.validateLogin(ID, PIN);
                if (userFound) {
                    ChildLogin ChildLoginWindow = new ChildLogin(ID);
                    ChildLoginWindow.Show();
                    ChildLoginWindow.WindowState = WindowState.Maximized;
                    this.Close();
                }
                else {
                    MessageBox.Show("User ID or PIN does not exist");
                }
            }
        }

        private void btn_UserSelect_Click(object sender, RoutedEventArgs e)
        {
            UserSelection userSelect = new UserSelection();
            userSelect.Show();
            this.Close();
        }//btn_Login_Click

        private void OnLoginFocus(object sender, EventArgs e) {
            if (String.IsNullOrWhiteSpace(txt_IDEntry.Text.ToString()) || String.IsNullOrWhiteSpace(txt_PINEntry.Password.ToString())) {
                btn_Login.Background = Brushes.Red;
            }
            else {
                btn_Login.Background = Brushes.Green;
            }
        }

    }//end win_LoginWindow(class)
}
