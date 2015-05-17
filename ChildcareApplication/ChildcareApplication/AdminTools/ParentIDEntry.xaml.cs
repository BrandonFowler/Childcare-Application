using DatabaseController;
using System;
using System.Windows;
using System.Windows.Input;
using MessageBoxUtils;
using System.Windows.Controls;

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
            txt_IDEntry.Focus();
        }

        private void KeyPressedValidateNumber(Object o, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                SubmitID();
            } else if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back)) {
                WPFMessageBox.Show("Please use only numbers.");
                e.Handled = true;
            }
        }


        private void btn_Submit_Click(object sender, RoutedEventArgs e) {
            SubmitID();
        }

        private void SubmitID() {
            GuardianInfoDB parentDB = new GuardianInfoDB();
            if (string.IsNullOrWhiteSpace(this.txt_IDEntry.Text)) {
                WPFMessageBox.Show("Please enter the parents ID number.");

            } else {
                string ID = txt_IDEntry.Text;

                if (parentDB.GuardianIDExists(ID)) {
                    if (editParent)
                        DisplayAdminEditParentWindow(ID);
                    else
                        DisplayAdminEditChildInfo(ID);
                } else {
                    WPFMessageBox.Show("User ID or PIN does not exist");
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
            this.Close(); 
        }

        private void SelectAllOnFocus(object sender, RoutedEventArgs e) {

            TextBox tb = (TextBox)sender;
            Dispatcher.BeginInvoke((Action)(tb.SelectAll));
        }

        private void Key_Up_Event(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next); //Found at: http://stackoverflow.com/questions/23008670/wpf-and-mvvm-how-to-move-focus-to-the-next-control-automatically
                request.Wrapped = true;
                ((Control)e.Source).MoveFocus(request);
            }
        }
        
    }
}
