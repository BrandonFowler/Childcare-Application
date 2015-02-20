using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ChildCareAppParentSide {
   
    public partial class win_AdminLogin : Window {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);
       
        private const int SM_TABLETPC = 86;
        private bool isTablet = false;
        private AdminDatabase db;

        public win_AdminLogin(){
            InitializeComponent();
            this.isTablet = IsTablet();
            this.txt_UserName.GotFocus += FocusedTextBox;
            this.txt_Password.GotFocus += FocusedTextBox;
            this.db = new AdminDatabase();
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
            string PW = txt_Password.Password;
            bool userFound = this.db.validateAdmin(UN, PW);
            if (string.IsNullOrWhiteSpace(this.txt_UserName.Text) || string.IsNullOrWhiteSpace(this.txt_Password.Password))
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

        private void btn_Back_Click(object sender, RoutedEventArgs e) {
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            loginWindow.WindowState = WindowState.Maximized;
            this.Close();
        }//end shortcut click

        private void FocusedTextBox(object sender, EventArgs e){
            if (isTablet) {
                startKeyBoard();
            }
        }//end OnTDBoxFocus

        private bool IsTablet(){
            return (GetSystemMetrics(SM_TABLETPC) != 0);
        }//end IsTablet

        private void btn_Keyboard_Click(object sender, RoutedEventArgs e){
            startKeyBoard();
        }//end btn_KeyBoard_Click

        private void startKeyBoard(){
            Version win8version = new Version(6, 2, 9200, 0);

            if (Environment.OSVersion.Version >= win8version)
            {
                string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                string keyboardPath = Path.Combine(progFiles, "TabTip.exe");
                Process.Start(keyboardPath);
            }
        }//end startKeyBoard
  
    }//end win_AdminLogin(class)
}
