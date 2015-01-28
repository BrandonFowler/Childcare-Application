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

namespace ChildCareAppParentSide {
   
    public partial class win_LoginWindow : Window {

        private bool IDBoxSelected = false;
        private bool PINBoxSelected = false;
        private Database db;

        public win_LoginWindow() {
            InitializeComponent();
            this.db = new Database();
            this.txt_IDEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_IDEntry.GotFocus += OnIDBoxFocus;
            this.txt_PINEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_PINEntry.GotFocus += OnPINBoxFocus;
            this.txt_IDEntry.Focus();
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
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back) {

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
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "1";
            }
        }//btn_Number1_Click

        private void btn_Number2_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "2";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "2";
            }
        }//btn_Number2_Click

        private void btn_Number3_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "3";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "3";
            }
        }//btn_Number3_Click

        private void btn_Number4_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "4";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "4";
            }
        }//btn_Number4_Click

        private void btn_Number5_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "5";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "5";
            }
        }//btn_Number5_Click

        private void btn_Number6_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "6";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "6";
            }
        }//btn_Number6_Click

        private void btn_Number7_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "7";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "7";
            }
        }//btn_Number7_Click

        private void btn_Number8_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "8";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "8";
            }
        }//btn_Number8_Click

        private void btn_Number9_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "9";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "9";
            }
        }//btn_Number9_Click

        private void btn_Number0_Click(object sender, RoutedEventArgs e) {
            if (IDBoxSelected && this.txt_IDEntry.Text.Length < 6) {
                this.txt_IDEntry.Text += "0";
            }
            if (PINBoxSelected && this.txt_PINEntry.Text.Length < 4) {
                this.txt_PINEntry.Text += "0";
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
            string ID = txt_IDEntry.Text;
            string PIN = txt_PINEntry.Text;
            bool userFound = this.db.validateLogin(ID, PIN);
            bool adminFound = this.db.validateAdmin(ID, PIN);

            if (userFound) {
                win_ChildLogin ChildLoginWindow = new win_ChildLogin(ID);
                ChildLoginWindow.Show();
                this.Close();
            }
            else if (adminFound) {
                win_AdminWindow AdminWindow = new win_AdminWindow(ID);
                AdminWindow.Show();
                this.Close();
            }
            else {
                MessageBox.Show("User ID or PIN does not exist");
            }

        }//btn_Login_Click

    }//end win_LoginWindow(class)
}
