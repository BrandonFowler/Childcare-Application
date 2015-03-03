using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace ChildCareAppParentSide {
  
    public partial class win_Settings : Window {

        bool isTablet;
        Settings settings;

        public win_Settings(bool isTablet) {
            InitializeComponent();
            this.isTablet = isTablet;
            this.txt_Server.GotFocus += FocusedTextBox;
            this.txt_Port.GotFocus += FocusedTextBox;
            this.txt_DatabaseName.GotFocus += FocusedTextBox;
            this.txt_DatabaseUser.GotFocus += FocusedTextBox;
            this.txt_DatabasePassword.GotFocus += FocusedTextBox;
            this.txt_RegularCareCap.GotFocus += FocusedTextBox;
            this.settings = Settings.Instance;
            this.txt_Server.Text = settings.server;
            this.txt_Port.Text = settings.port;
            this.txt_DatabaseName.Text = settings.databaseName;
            this.txt_DatabaseUser.Text = settings.databaseUser;
            this.txt_DatabasePassword.Text = settings.databasePassword;
            this.txt_RegularCareCap.Text = settings.regularCareCap;
            this.txt_PhotoDirectory.Text = settings.photoPath;
            this.txt_BillDateStart.Text = settings.billStart;
            this.txt_BillDateEnd.Text = settings.billEnd;
        }//end constructor

        private void btn_LogOutAdmin_Click(object sender, RoutedEventArgs e) {
            win_LoginWindow loginWindow = new win_LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void btn_chooseFile_Click(object sender, RoutedEventArgs e) {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            this.txt_PhotoDirectory.Text = dialog.SelectedPath;
        }

        private void btn_ApplySettings_Click(object sender, RoutedEventArgs e) {
            string server = txt_Server.Text;
            string port = txt_Port.Text;
            string databaseName = txt_DatabaseName.Text;
            string databaseUser = txt_DatabaseUser.Text;
            string databasePassword = txt_DatabasePassword.Text;
            string photoPath = txt_PhotoDirectory.Text;
            photoPath = photoPath.Replace(@"\", @"/");
            string chargeMax = txt_RegularCareCap.Text;
            string billStart = txt_BillDateStart.Text;
            string billEnd = txt_BillDateEnd.Text;
            string writeToFile = "Server=" + server + "\r\n" + "Port=" + port + "\r\n" + "Database=" + databaseName +
                                 "\r\n" + "UID=" + databaseUser + "\r\n" + "UPW=" + databasePassword + "\r\n" +
                                 "PhotoPath=" + photoPath + "\r\n" + "CareCap=" + chargeMax + "\r\n" + "BillStart=" + billStart +
                                 "\r\n" + "BillEnd=" + billEnd;
            System.IO.StreamWriter file = new System.IO.StreamWriter("settings.txt");
            file.WriteLine(writeToFile);
            file.Close();
            Settings.reset();
            System.Windows.MessageBox.Show("Settings Applied");
        }//end btn_LogOutAdmin

        private void FocusedTextBox(object sender, EventArgs e) {
            if (isTablet) {
                startKeyBoard();
            }
        }//end OnTDBoxFocus

        private void startKeyBoard() {
            Version win8version = new Version(6, 2, 9200, 0);
            if (Environment.OSVersion.Version >= win8version) {
                string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                string keyboardPath = Path.Combine(progFiles, "TabTip.exe");
                Process.Start(keyboardPath);
            }
        }//end startKeyBoard

    }//end win_AdminWindow
}
