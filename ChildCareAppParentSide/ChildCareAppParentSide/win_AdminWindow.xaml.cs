using System.Windows;

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
