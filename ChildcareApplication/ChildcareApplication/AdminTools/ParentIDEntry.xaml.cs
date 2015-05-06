using DatabaseController;
using System;
using System.Windows;
using System.Windows.Input;

namespace AdminTools {

    public partial class ParentIDEntry : Window {

        private LoginDB db;
        private bool editParent;
        public ParentIDEntry(bool editP) {
            InitializeComponent();
            editParent = editP;
            this.db = new LoginDB();
            this.txt_IDEntry.KeyDown += new KeyEventHandler(KeyPressedValidateNumber);
            this.txt_IDEntry.Focus();
            this.MouseDown += WindowMouseDown;
        }

        private void KeyPressedValidateNumber(Object o, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                SubmitID();
            } else if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back)) {
                MessageBox.Show("Please use only numbers.");
                e.Handled = true;
            }
        }


        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            SubmitID();
        }

        private void SubmitID() {
            GuardianInfoDB parentDB = new GuardianInfoDB();
            if (string.IsNullOrWhiteSpace(this.txt_IDEntry.Text)) {
                MessageBox.Show("Please enter the parents ID number.");

            } else {
                string ID = txt_IDEntry.Text;

                if (parentDB.GuardianIDExists(ID)) {
                    if (editParent)
                        DisplayAdminEditParentWindow(ID);
                    else
                        DisplayAdminEditChildInfo(ID);
                } else {
                    MessageBox.Show("User ID or PIN does not exist");
                }
            }
        }

        private void DisplayAdminEditParentWindow(string ID) {

            AdminEditParentInfo AdminEditParentWindow = new AdminEditParentInfo(ID);
            AdminEditParentWindow.Show();
            this.Close();
        }//end DisplayAdminEditParentWindow

        private void DisplayAdminEditChildInfo(string ID) {

            AdminEditChildInfo AdminEditChildInfo = new AdminEditChildInfo(ID);
            AdminEditChildInfo.Show();
            this.Close();
        }//end DisplayAdminEditChildInfo

        private void WindowMouseDown(object sender, MouseButtonEventArgs e){
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e) {
            AdminMenu adminMenu = new AdminMenu();
            adminMenu.Show();
            this.Close(); 
        }
    }
}
