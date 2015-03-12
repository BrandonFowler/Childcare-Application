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


namespace AdminTools {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminLogin : Window {
        
        private bool IDBoxSelected = false;
        private bool PINBoxSelected = false;
        private Database db;
        private string pin; 

        public AdminLogin(){
            InitializeComponent();
            this.db = new Database();
            this.txt_UserName.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_UserName.GotFocus += OnIDBoxFocus;
            this.txt_Password.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_Password.GotFocus += OnPINBoxFocus;
            this.txt_UserName.Focus();
        }//end win_LoginWindow

        private void KeyPressedValidateNumber(Object o, KeyEventArgs e)
        {
           /* if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back)
            {

            }
            else
            {
                MessageBox.Show("Please use only numbers.");
                e.Handled = true;
            }*/
  
        }//end KeyPressedValidateNumber

        private void OnIDBoxFocus(object sender, EventArgs e) {
            this.IDBoxSelected = true;
            this.PINBoxSelected = false;
        }//end OnTDBoxFocus

        private void OnPINBoxFocus(object sender, EventArgs e) {
            this.PINBoxSelected = true;
            this.IDBoxSelected = false;
        }//end OnPINBoxFocus

        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(this.txt_UserName.Text) || string.IsNullOrWhiteSpace(this.txt_Password.Password))
            {
                MessageBox.Show("Please enter a User Name and a Password.");

            }
            else
            {
                
                string ID = txt_UserName.Text;
                string PIN = txt_Password.Password;
                bool userFound = this.db.validateAdminLogin(ID, PIN);

                if (userFound)
                {
                    DisplayAdminWindow();
                }
                else
                {
                    MessageBox.Show("User ID or PIN does not exist");
                }

            }
            /*
              Still need to account for admin login
            */
        }//btn_Login_Click(Class)

        private void DisplayAdminWindow() {

            AdminMenu AdminMenu = new AdminMenu();
            AdminMenu.Show();
            this.Close(); 

        }//end DisplayAdminWindow

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }//end shortcut click
    }
}
