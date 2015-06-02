using MessageBoxUtils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AdminTools {

    public partial class RestoreRecords : Window {

        private string userName;
        private int access;

        public RestoreRecords(int access, string userName) {
            InitializeComponent();
            this.access = access;
            this.userName = userName;
            this.MouseDown += WindowMouseDown;
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            AdminMenu menu = new AdminMenu(access, userName);
            menu.Show();
            this.Close();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btn_SelectRestore_Click(object sender, RoutedEventArgs e) {
            string imagePath = System.IO.Path.GetFullPath("../../Backup Records");
            imagePath = imagePath.Replace(@"/", @"\");
            System.Windows.Forms.OpenFileDialog openFileDialog;
            openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = imagePath;
            openFileDialog.ShowDialog();
            string path = openFileDialog.FileName;
            if (path != "") {
                txt_path.Text = path;
            }
        }

        private void btn_Restore_Click(object sender, RoutedEventArgs e) {
            if (txt_path.Text == null || txt_path.Text == "") {
                WPFMessageBox.Show("You must choose a backup file first.");
                return;
            }
            string pathExtention = txt_path.Text.Substring(txt_path.Text.LastIndexOf('.') + 1);
            if (pathExtention.CompareTo("s3db") != 0){
                WPFMessageBox.Show("The file you have chosen is not valid. Please choose a valid database file.");
            }else{
                try {
                    MessageBoxResult messageBoxResult = WPFMessageBox.Show("Are you sure you wish to perform a record restore?", "Restore Confirmation", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes) {
                        File.Copy(txt_path.Text, @"..\..\Database\ChildcareDB.s3db", true);
                        WPFMessageBox.Show("Restore Completed.");
                    }
                } catch (System.IO.IOException) {
                    WPFMessageBox.Show("Unable to perform restore. Please insure write permissions are set for the database backup location.");
                } catch (Exception) {
                    WPFMessageBox.Show("Unable to perform restore");
                }
            }
        }
    }
}
