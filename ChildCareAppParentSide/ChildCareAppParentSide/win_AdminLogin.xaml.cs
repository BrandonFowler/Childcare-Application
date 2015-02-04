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
using System.Runtime.InteropServices;



namespace ChildCareAppParentSide {
   
    public partial class win_AdminLogin : Window {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
        private const int SM_TABLETPC = 86;
        private bool isTablet = false;
        private Database db;

        public win_AdminLogin(){
            InitializeComponent();
            this.isTablet = IsTablet();
            this.txt_UserName.GotFocus += OnUserNameFocus;
            this.txt_Password.GotFocus += OnPasswordFocus;
            this.db = new Database();
            if (isTablet){
                btn_Keyboard.Visibility = Visibility.Visible;
                btn_Keyboard.IsEnabled = true;
            }
            else{
                txt_UserName.Focus();
            }
            
        }//end win_LoginWindow

        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            string UN = txt_UserName.Text;
            string PW = txt_Password.Text;
            bool userFound = this.db.validateAdmin(UN, PW);
            if (string.IsNullOrWhiteSpace(this.txt_UserName.Text) || string.IsNullOrWhiteSpace(this.txt_Password.Text))
            {
                MessageBox.Show("Please enter a User Name and a Password.");

            }
            else
            {
                if (userFound)
                {
                    DisplayAdminWindow();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User Name or Password does not exist");
                }
            }
          
        }//btn_Login_Click(Class)

        private void DisplayAdminWindow() {

            win_AdminWindow AdminWindow = new win_AdminWindow();
            AdminWindow.Show();
            AdminWindow.WindowState = WindowState.Maximized;
            this.Close(); 

        }//end DisplayAdminWindow

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            loginWindow.WindowState = WindowState.Maximized;
            this.Close();
        }//end shortcut click

        private void OnUserNameFocus(object sender, EventArgs e){
            if (isTablet){
                if (string.IsNullOrWhiteSpace(this.txt_Password.Text))
                {
                    System.Diagnostics.Process.Start("osk.exe");
                }
            }
        }//end OnTDBoxFocus

        private void OnPasswordFocus(object sender, EventArgs e){
            if (isTablet){
                if (string.IsNullOrWhiteSpace(this.txt_UserName.Text)){
                    System.Diagnostics.Process.Start("osk.exe");
                }
            }
        }//end OnPINBoxFocus

        private bool IsTablet(){
            return (GetSystemMetrics(SM_TABLETPC) != 0);
        }//end IsTablet

        private void btn_Keyboard_Click(object sender, RoutedEventArgs e){
            System.Diagnostics.Process.Start("osk.exe");
        }//end btn_KeyBoard_Click
    }
}
