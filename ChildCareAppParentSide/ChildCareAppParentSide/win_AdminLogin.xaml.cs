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
using System.Windows.Interop;
using System.Diagnostics;

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
            this.txt_UserName.LostFocus += TextBoxFocusLost;
            this.txt_Password.LostFocus += TextBoxFocusLost;
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
                    endKeyboard();
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
            endKeyboard();
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            loginWindow.WindowState = WindowState.Maximized;
            this.Close();
        }//end shortcut click

        private void FocusedTextBox(object sender, EventArgs e){
            if (isTablet) {
                Process.Start("osk.exe");
            }
        }//end OnTDBoxFocus

        private bool IsTablet(){
            return (GetSystemMetrics(SM_TABLETPC) != 0);
        }//end IsTablet

        private void btn_Keyboard_Click(object sender, RoutedEventArgs e){
            Process.Start("osk.exe");
        }//end btn_KeyBoard_Click

        private void TextBoxFocusLost(object sender, EventArgs e) {
            endKeyboard();
        }//end LostUserNameFocus

        private void endKeyboard() {
            string processName = "osk";
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes) {
                process.Kill();
            }
        }//end endKeyboard
  
    }//end win_AdminLogin(class)
}
