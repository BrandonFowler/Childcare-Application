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
   
    public partial class win_AdminLogin : Window {
        
        private Database db;

        public win_AdminLogin(){
            InitializeComponent();
            this.txt_UserName.Focus();
            this.db = new Database();
        }//end win_LoginWindow

        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            string UN = txt_UserName.Text;
            string PW = txt_Password.Text;
            bool userFound = this.db.validateAdmin(UN, PW);

            if (userFound) {
                DisplayAdminWindow();
                this.Close();
            }
            else {
                MessageBox.Show("User Name or Password does not exist");
            }

          
        }//btn_Login_Click(Class)

        private void DisplayAdminWindow() {

            win_AdminWindow AdminWindow = new win_AdminWindow();
            AdminWindow.Show();
            this.Close(); 

        }//end DisplayAdminWindow

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            this.Close();
        }//end shortcut click
    }
}
