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
using System.Windows.Shapes;

namespace ChildCareAppParentSide {
  
    public partial class win_AdminWindow : Window {

        private string ID;
        private ChildCheckInDatabase db;

        public win_AdminWindow() {
            InitializeComponent();
        }//end default constructor

        public win_AdminWindow(string ID) {
            InitializeComponent();
            this.ID = ID;
            this.db = new ChildCheckInDatabase();
        }//end constructor

        private void btn_LogOutAdmin_Click(object sender, RoutedEventArgs e) {
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            this.Close();
        }//end btn_LogOutAdmin

    }//end win_AdminWindow
}
